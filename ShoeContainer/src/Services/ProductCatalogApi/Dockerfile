# run in terminal docker build -t shoes/catalog .

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0.404 AS build
WORKDIR /code

COPY . .

# copy everything else and build app

RUN dotnet publish -c release -o /app 

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0.13
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ProductCatalogApi.dll", "--server.urls", "http://*:5000"]





