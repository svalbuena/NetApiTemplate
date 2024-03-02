#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0.2-alpine3.19 AS base
# Run as the non-admin `app` user
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Build the Service
FROM mcr.microsoft.com/dotnet/sdk:8.0.201-alpine3.19 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY global.json .
COPY .editorconfig .
COPY .config .

COPY src/Service/Service.csproj Service/
COPY src/SwaggerDefinition/SwaggerDefinition.csproj SwaggerDefinition/

RUN dotnet restore Service/Service.csproj

COPY src .

RUN dotnet build Service/Service.csproj -c $BUILD_CONFIGURATION -o /app/build


# Create the Service binary, a separate step from the build stage as Visual Studio does some optimizations and needs to target the publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Service/Service.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Run the Service on the .NET runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "Service.dll"]
