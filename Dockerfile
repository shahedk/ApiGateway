FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app


# Copy everything else and build
COPY ./src ./src
COPY ./test ./test
COPY ./demo ./demo
COPY ApiGateway.sln ApiGateway.sln
RUN dotnet restore ApiGateway.sln

RUN dotnet publish src/ApiGateway.WebApi/ApiGateway.WebApi.csproj --output published-app

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app

COPY --from=build-env app/src/ApiGateway.WebApi/published-app .

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "ApiGateway.WebApi.dll"]
