echo ">>>>>>> Restoring Nuget packages..."
dotnet restore ../../src/ApiGateway.sln

echo
echo ">>>>>>> Build project..."
dotnet build ../../src/ApiGateway.sln

echo
echo ">>>>>>> Cleanup Docker/app directory..."
rm -r app/*

echo
echo ">>>>>>> Publish project into Docker/app directory..."
dotnet publish ../../src/ApiGateway.WebApi/ApiGateway.WebApi.csproj --output published-app

mv ../../src/ApiGateway.WebApi/published-app/* app/

echo
echo ">>>>>>> Build docker image"
docker build -t shahedk/apigateway:latest .

echo
echo ">>>>>>> Push image to Azure repository"
docker push shahedk/apigateway:latest
