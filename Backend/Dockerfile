FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY "Marketplace.Common\Marketplace.Common.csproj" 
COPY *.csproj ./
RUN  dotnet restore

COPY . ./
RUN  dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet","Basket.dll"]