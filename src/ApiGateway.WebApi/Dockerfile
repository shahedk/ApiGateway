FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ApiGateway.WebApi/ApiGateway.WebApi.csproj ApiGateway.WebApi/
COPY ApiGateway.Data.EFCore/ApiGateway.Data.EFCore.csproj ApiGateway.Data.EFCore/
COPY ApiGateway.Data/ApiGateway.Data.csproj ApiGateway.Data/
COPY ApiGateway.Common/ApiGateway.Common.csproj ApiGateway.Common/
COPY ApiGateway.Core/ApiGateway.Core.csproj ApiGateway.Core/
COPY ApiGateway.Client/ApiGateway.Client.csproj ApiGateway.Client/
RUN dotnet restore ApiGateway.WebApi/ApiGateway.WebApi.csproj
COPY . .
WORKDIR /src/ApiGateway.WebApi
RUN dotnet build ApiGateway.WebApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ApiGateway.WebApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiGateway.WebApi.dll"]
