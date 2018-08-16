namespace ApiGateway.Client
{
    public interface IApiRequestHelper
    {
        string GetApiKey();

        string GetApiSecret();

        string GetServiceApiKey();

        string GetServiceApiSecret();
    }
}