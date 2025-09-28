
# Kachna Information System version 4

This is a complex information system for product management created by bc. Marek Dančo for the
school club Kachna at FIT BUT.

## Prerequisites for running locally

Dependencies: .NET 9, Docker

To setup the local database, .NET tool and dotnet-ef is required. Before testing, install this tool
by running:

```bash
dotnet tool install --global dotnet-ef
```

If you're using Linux, you might also need to add the path to your .NET tool to the `$PATH` variable.

This API doesn't provide authentication, it expects to be authenticated against a different service.
To emulate this, .NET provides a local tool to generate JWT tokens for testing. You can generate a
token by running:

```bash
dotnet user-jwts create --project App
```

Copy the output after the `Token: ` to pass it to Swagger for authentication.

## Running locally

To start the Docker container with the database, run:

```bash
docker-compose create database && docker-compose start database
```

After starting the database container, run the following command to migrate the database:

```bash
dotnet ef database update --project DAL.EF --startup-project App
```

After the database has successfully been migrated, the local program should function correctly. Run
the project with `dotnet run` and the API should be live at `localhost:5242`. The API is set to only
allow authenticated users, so this page will not work - to test the API, go to
`localhost:5242/swagger`.

To authenticate, click the "Authorize" button and paste in the generated JWT token. After that, all
endpoints should function.


