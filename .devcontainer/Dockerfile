FROM mcr.microsoft.com/devcontainers/dotnet:1-7.0-bullseye

COPY ../MyFire.sln ./
COPY ../DemoConsoleApp/*.csproj ./DemoConsoleApp/
COPY ../EtlJobApp/*.csproj ./EtlJobApp/
COPY ../Services/*.csproj ./Services/
COPY ../Services.Tests/*.csproj ./Services.Tests/

RUN dotnet restore
# RUN dotnet build .