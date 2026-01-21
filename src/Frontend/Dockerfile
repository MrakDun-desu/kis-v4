# Set context to the root of the repo

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ENV DOTNET_PRINT_TELEMETRY_MESSAGE="false"
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE="1"
ENV DOTNET_NOLOGO="true"

WORKDIR /src

# Run restore separately
COPY ./Frontend/Frontend.csproj Frontend/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore ./Frontend/Frontend.csproj

# Copy entire source code
COPY ./Frontend/. Frontend/

# Build and publish
WORKDIR /src/Frontend
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish --no-restore -o /publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine-extra AS runtime
EXPOSE 8080
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["./Frontend"]
