using ApiGateway.Common.Constants;

namespace ApiGateway.Client
{
    public interface IApiRequestHelper
    {
        string GetApiKey();

        string GetApiSecret();

        string GetApiKeyType();

//        string GetServiceApiKeyType();

//        string GetApiName();

//        string GetServiceName();
    }
}