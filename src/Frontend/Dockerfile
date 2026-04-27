FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ENV DOTNET_PRINT_TELEMETRY_MESSAGE="false"
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE="1"
ENV DOTNET_NOLOGO="true"

WORKDIR /src

# # Run restore separately - doesn't work, some nugets just aren't found
# COPY ./Frontend/Frontend.csproj Frontend/
# RUN dotnet restore "./Frontend/Frontend.csproj"
#
# Copy entire source code
COPY ./Frontend/. Frontend/

# Build and publish
WORKDIR /src/Frontend
# RUN dotnet publish --no-restore -o /publish
RUN dotnet publish -o /publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine-extra AS runtime
EXPOSE 8080
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "./Frontend.dll"]
