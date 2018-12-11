using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ApiGateway.Client
{
    public class ClientApiKeyValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IClientLoginService _clientLoginService;
        private readonly ApiGatewaySettings _settings;

        public ClientApiKeyValidationMiddleware(RequestDelegate next, IClientLoginService clientLoginService, IOptions<ApiGatewaySettings> settings)
        {
            _next = next;
            _clientLoginService = clientLoginService;
            _settings = settings.Value;
        }

        public async Task Invoke(HttpContext context)
        {   
            if (!context.Request.Headers.Keys.Contains(ApiHttpHeaders.ApiKey) || !context.Request.Headers.Keys.Contains(ApiHttpHeaders.ApiSecret))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync($"{ApiHttpHeaders.ApiKey} and/or {ApiHttpHeaders.ApiSecret} missing");
            }
            else
            {
                string apiKey = context.Request.Headers[ApiHttpHeaders.ApiKey];
                string apiSecret = context.Request.Headers[ApiHttpHeaders.ApiSecret];
                string action = context.Request.Method;
                string apiName = context.Request.GetApiName();

                var result = await _clientLoginService.IsClientApiKeyValidAsync(apiKey, apiSecret, _settings.ServiceName,  apiName, action);

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

    public static class UserKeyValidationExtension
    {
        public static IApplicationBuilder UseClientApiKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ClientApiKeyValidationMiddleware>();
            return app;
        }
    }
}
