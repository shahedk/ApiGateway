using System.Net;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ApiGateway.InternalClient
{
    public class InternalClientApiKeyValidationMiddleware
    {
    private readonly RequestDelegate _next;
        private readonly IClientLoginService _clientLoginService;
        private readonly ApiGatewaySettings _settings;

        public InternalClientApiKeyValidationMiddleware(RequestDelegate next, IClientLoginService clientLoginService, IOptions<ApiGatewaySettings> settings)
        {
            _next = next;
            _clientLoginService = clientLoginService;
            _settings = settings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue)
            {
                var path = context.Request.Path.Value.ToLower();
                if (path.ToLower().StartsWith("/sys/appenv/") || path.ToLower().StartsWith("/sys/isvalid/"))
                {
                    // These two special paths don't need api-key validation
                    await _next.Invoke(context);
                    return;
                }
            }
            
            
            if (!context.Request.Headers.Keys.Contains(ApiHttpHeaders.ApiKey) || !context.Request.Headers.Keys.Contains(ApiHttpHeaders.ApiSecret))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync($"{ApiHttpHeaders.ApiKey} and/or {ApiHttpHeaders.ApiSecret} missing");
            }
            else
            {
                string apiKey = context.Request.Headers[ApiHttpHeaders.ApiKey];
                string apiSecret = context.Request.Headers[ApiHttpHeaders.ApiSecret];
                string serviceKey = context.Request.Headers[ApiHttpHeaders.ServiceApiKey];
                string serviceSecret = context.Request.Headers[ApiHttpHeaders.ServiceApiSecret];
                string action = context.Request.Method;
                string apiUrl = context.Request.Path;

                var serviceId = "1";
                
                var result = await _clientLoginService.IsClientApiKeyValidAsync(apiKey, apiSecret, serviceKey, serviceSecret, serviceId, apiUrl, action);

                if (result.IsValid)
                {
                    context.Items.Add(ApiHttpHeaders.ApiKey, apiKey);
                    await _next.Invoke(context);
                }
                else
                {
                    // Validation failed
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(result.ToJson());
                }
            }
        }
    }

}