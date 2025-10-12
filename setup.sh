#!/usr/bin/env bash

set -eo pipefail
BASE_DIR=$(pwd)

API_DOC="$BASE_DIR/kisv4-openapi-doc.json"
API_DOC_HASH="$BASE_DIR/.kisv4-openapi-doc.hash"
GEN_OUT_DIR="$BASE_DIR/frontend/src/api-generated"

database() {
    echo "Setting up the database..."
    docker compose create database
    docker compose start database
}

build_backend() {
    echo "Building backend..."
    cd "$BASE_DIR/backend"
    dotnet build
}

install_frontend() {
    echo "Installing frontend dependencies..."
    cd "$BASE_DIR/frontend"
    npm ci
}

install_backend() {
    echo "Installing backend dependencies..."
    cd "$BASE_DIR/backend"
    dotnet tool restore
}

generate_api_code() {
    if [ ! -f "$API_DOC" ]; then
        build_backend
    fi

    local hash=$(sha256sum "$API_DOC")
    local generate=false

    if [ ! -f "$API_DOC_HASH" ]; then
        generate=true
    else
        local old_hash=$(cat "$API_DOC_HASH" 2> /dev/null || echo "")
        if [ "$hash" != "$old_hash" ]; then
            generate=true
        fi
    fi

    if [ $generate = true ]; then
        echo "Generating frontend API code from backend..."
        echo "$hash" > "$API_DOC_HASH"
        cd "$BASE_DIR/frontend"
        npx openapi-generator-cli generate -i $API_DOC -g typescript-fetch -o $GEN_OUT_DIR > /dev/null
    else
        echo "Skipping generation, API doc not changed"
    fi
}

update_backend() {
    install_backend

    echo "Checking backend updates..."
    cd "$BASE_DIR/backend"
    dotnet outdated -u:prompt
}

update_frontend() {
    install_frontend

    echo "Checking frontend updates..."
    cd "$BASE_DIR/frontend"
    npx ncu --interactive --format group
    npm install
}

update() {
    update_backend
    update_frontend
}

migrate_db() {
    database
    install_backend

    echo "Migrating database..."
    cd "$BASE_DIR/backend"
    dotnet ef database update --project DAL.EF --startup-project App
}

run_backend() {
    migrate_db
    build_backend

    echo "Running backend in dev mode..."
    cd "$BASE_DIR/backend"
    dotnet run --project App --no-build > /dev/null &
    echo "Backend started. Don't forget to kill the job after you're done."
}

run_frontend() {
    install_frontend
    run_backend
    generate_api_code

    echo "Running frontend in dev mode..."
    cd "$BASE_DIR/frontend"
    npx vite --open > /dev/null &
    echo "Frontend started. Don't forget to kill the job after you're done."
}

deploy() {
    generate_api_code

    echo "Deploying to Docker Compose..."
    cd $BASE_DIR
    docker compose up --build
}

down_dev() {
    pkill -f App
    pkill -f vite
}

down_deploy() {
    docker compose down
}

case "$1" in
    database) migrate_db
    ;;
    backend)
        case "$2" in
            build) build_backend
                ;;
            install) install_backend
                ;;
            run) run_backend
                ;;
            update) update_backend
                ;;
            *)
                echo "Available subcommands for backend:"
                echo "    build     Compiles backend and generates OpenAPI schema file to the root folder"
                echo "    install   Installs dotnet tools"
                echo "    run       Runs a local dev server for the API"
                echo "    update    Updates backend dependencies interactively"
                ;;
        esac
        ;;
    frontend)
        case "$2" in
            install) install_frontend
                ;;
            run) run_frontend
                ;;
            update) update_frontend
                ;;
            *)
                echo "Available subcommands for frontend:"
                echo "    install   Installs node modules"
                echo "    run       Runs a local dev server for frontend"
                echo "    update    Updates frontend dependencies interactively"
                ;;
        esac
        ;;
    update) update
    ;;
    deploy) deploy
    ;;
    down)
        case "$2" in
            dev) down_dev
                ;;
            deploy) down_deploy
                ;;
            *)
                echo "Available subcommands for down:"
                echo "    dev      Shuts down the local servers"
                echo "    deploy   Shuts down the Docker deployment"
                ;;
        esac
    ;;
    *)
        echo "Available commands:"
        echo "    database    Start the database and migrate it"
        echo "    backend     Subcommands related to the backend"
        echo "    frontend    Subcommands related to the frontend"
        echo "    update      Update all dependencies"
        echo "    deploy      Deploy the whole system locally on containers"
        echo "    down        Subcommands related to stopping running processes"
        echo ""
        echo "All the commands execute all the necessary prerequisites first."
    ;;
esac


