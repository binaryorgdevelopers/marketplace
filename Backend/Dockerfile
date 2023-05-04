FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

#COPY "Marketplace.sln" "Marketplace.sln"
COPY "Shared/Shared.csproj" "Shared/Shared.csproj"
COPY "Basket/Basket.csproj" "Basket/Basket.csproj" 

RUN dotnet restore "Basket/Basket.csproj"

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build /app/out ./

EXPOSE 80

ENTRYPOINT ["dotnet", "Basket.dll"]