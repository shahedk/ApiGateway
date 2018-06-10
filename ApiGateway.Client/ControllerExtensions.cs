using ApiGateway.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.Client
{
    public static class ControllerExtensions
    {
        public static string GetApiPublicKey(this HttpContext self)
        {
            var apiKey = self.Items[ApiHttpHeaders.ApiKey] as string;

            return apiKey;
        }
    }
}
