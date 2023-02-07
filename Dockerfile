FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

COPY IntrepionApp.sln .
COPY IntrepionApp.Tests/*.csproj ./IntrepionApp.Tests/
COPY IntrepionApp.WebApi/*.csproj ./IntrepionApp.WebApi/
RUN dotnet restore

COPY IntrepionApp.Tests/. ./IntrepionApp.Tests/
COPY IntrepionApp.WebApi/. ./IntrepionApp.WebApi/
WORKDIR /source/IntrepionApp.WebApi
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "IntrepionApp.WebApi.dll"]
