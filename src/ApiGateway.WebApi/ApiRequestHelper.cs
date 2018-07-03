using ApiGateway.Client;
using ApiGateway.Common.Constants;
using Microsoft.AspNetCore.Http;

namespace ApiGateway.WebApi
{
    public class ApiRequestHelper : IApiRequestHelper
    {
        private readonly HttpContext _context;

        public ApiRequestHelper(HttpContext context)
        {
            _context = context;
        }
        public string GetApiPublicKey()
        {
            var apiKey = _context.Items[ApiHttpHeaders.ApiKey] as string;

            return apiKey;
        }
        
    }
}