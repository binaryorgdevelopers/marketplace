FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["../src/Api/Marketplace.Api/Marketplace.Api.csproj", "../src/Api/Marketplace.Api/"]
RUN dotnet restore "../src/Api/Marketplace.Api/Marketplace.Api.csproj"
COPY . .
WORKDIR "/src/../src/Api/Marketplace.Api"
RUN dotnet build "Marketplace.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Marketplace.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Marketplace.Api.dll"]
