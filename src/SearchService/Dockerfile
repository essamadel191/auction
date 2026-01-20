FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
EXPOSE 80

# Copy csproj and restore dependencies
COPY src/SearchService/SearchService.csproj src/SearchService/
RUN dotnet restore src/SearchService/SearchService.csproj

# Copy everything else and build
COPY src/SearchService src/SearchService
WORKDIR /app/src/SearchService
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SearchService.dll"]
