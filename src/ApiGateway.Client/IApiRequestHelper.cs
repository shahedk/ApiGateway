using ApiGateway.Common.Constants;

namespace ApiGateway.Client
{
    public interface IApiRequestHelper
    {
        string GetApiKey();

        string GetApiSecret();

        string GetServiceApiKey();

        string GetServiceApiSecret();

        string GetApiKeyType();

        string GetServiceApiKeyType();
    }
}