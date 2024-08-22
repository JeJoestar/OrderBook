FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["OrderBook.WebAPI/OrderBook.WebAPI.csproj", "OrderBook.WebAPI/"]
COPY ["OrderBook.Application/OrderBook.Application.csproj", "OrderBook.Application/"]
COPY ["OrderBook.Infrastructure/OrderBook.Infrastructure.csproj", "OrderBook.Infrastructure/"]
COPY ["OrderBook.Domain/OrderBook.Domain.csproj", "OrderBook.Domain/"]
RUN dotnet restore "./OrderBook.WebAPI/OrderBook.WebAPI.csproj"
COPY . .
WORKDIR "/src/OrderBook.WebAPI"
RUN dotnet build "./OrderBook.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderBook.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderBook.WebAPI.dll"]