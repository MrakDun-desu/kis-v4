# Kachna Information System version 4

This is a complex information system for product management created by bc. Marek
Dančo for the school club Kachna at FIT BUT. It offers an API (in the folder
./src/Api/) and a new front-end (in the folder ./src/Frontend/).

## Prerequisites for running locally

Dependencies: .NET 10, Docker, npm

To setup the local database, installing necesary .NET tools is required. The
tools are set up for the workspace, so just run following to install them:

```bash
dotnet tool restore
```

This API doesn't provide authentication, it expects to be authenticated against
a different service. To emulate this, .NET provides a local tool to generate JWT
tokens for testing. You can generate a token by running:

```bash
dotnet user-jwts create -p App --role admin --name 1
```

The user name is 1 because the external authentication service uses numerical
user IDs. Copy the output after the `Token: ` to pass it to the API for
authentication.

## Running locally

To start the Docker container with the database, run:

```bash
docker compose up database
```

After starting the database container, run the following command to migrate the
database:

```bash
dotnet ef database update --project DAL.EF --startup-project Api
```

After the database has been successfully migrated, the local program should
function correctly. Run the project with `dotnet run -- --testing-auth` and the
API should be live at `https://localhost:7001`. The API itself will not be
accessible without an authentication token. To test the API, go to
`localhost:7001/scalar`.

To authenticate in Scalar, in the Authentication section of the page, select
"Bearer" as the Auth Type. Then paste the generated user JWT into the "Token"
field.
