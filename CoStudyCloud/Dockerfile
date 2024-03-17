#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
# Copy the service account key file to the app directory
COPY "CoStudyCloud/application_default_credentials.json" /app/application_default_credentials.json

# Set environment variable for the application credentials
ENV GOOGLE_APPLICATION_CREDENTIALS="/app/application_default_credentials.json"
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoStudyCloud/CoStudyCloud.csproj", "CoStudyCloud/"]
RUN dotnet restore "./CoStudyCloud/./CoStudyCloud.csproj"
COPY . .
WORKDIR "/src/CoStudyCloud"
RUN dotnet build "./CoStudyCloud.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoStudyCloud.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoStudyCloud.dll"]