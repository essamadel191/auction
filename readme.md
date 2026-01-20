# Run only SearchService + MongoDB
docker compose up -d mongodb search-svc

# Run only AuctionService + PostgreSQL
docker compose up -d postgres auction-svc

# Or run databases only and services locally
docker compose up -d postgres mongodb



# ##################### Harbor ###################### #
# Tag images
docker tag auction-svc:latest harbor.example.com/auction-project/auction-svc:1.0.0
docker tag search-svc:latest harbor.example.com/auction-project/search-svc:1.0.0

# Login and push
docker login harbor.example.com
docker push harbor.example.com/auction-project/auction-svc:1.0.0
docker push harbor.example.com/auction-project/search-svc:1.0.0


### Docker package as 
Pulling and retagging it: 

"docker pull mcr.microsoft.com/dotnet/aspnet:10.0" 

then 

"docker tag mcr.microsoft.com/dotnet/aspnet:10.0 dotnet/aspnet:10.0"