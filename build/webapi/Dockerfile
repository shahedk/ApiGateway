FROM microsoft/dotnet:2.1-aspnetcore-runtime

COPY app /app
WORKDIR /app

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "ApiGateway.WebApi.dll"]
