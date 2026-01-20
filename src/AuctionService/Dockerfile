FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 80

# Copy csproj and restore dependencies
COPY src/AuctionService/AuctionService.csproj src/AuctionService/
RUN dotnet restore src/AuctionService/AuctionService.csproj

# Copy everything else and build
COPY src/AuctionService src/AuctionService
WORKDIR /app/src/AuctionService
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "AuctionService.dll"]
