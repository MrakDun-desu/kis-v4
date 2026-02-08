# Set context to the root of the repo

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ENV DOTNET_PRINT_TELEMETRY_MESSAGE="false"
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE="1"
ENV DOTNET_NOLOGO="true"

WORKDIR /src

# Run restore separately
COPY ./Api/Api.csproj Api/
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore ./Api/Api.csproj

# Copy entire source code
COPY ./Api/. Api/
COPY ./BL.EF/. BL.EF/
COPY ./DAL.EF/ DAL.EF/
COPY ./Common/ Common/

# Build and publish
WORKDIR /src/Api
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish --no-restore -o /publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine-extra AS runtime
EXPOSE 8080
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["./Api"]
