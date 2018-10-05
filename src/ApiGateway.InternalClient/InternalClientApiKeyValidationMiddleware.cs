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

        public InternalClientApiKeyValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IClientLoginService clientLoginService)
        {

            var path = context.Request.Path.Value.ToLower();
            if (context.Request.Path.HasValue)
            {
                if (path.StartsWith("/sys/appenv") || path.StartsWith("/api/isvalid/")|| path.StartsWith("/swagger/"))
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

                string serviceName = GetServiceNameFromPath(path);
                
                if (path.StartsWith(AppConstants.LocalApiUrlPrefix))
                {
                    // Local API, client api is the owner. 
                    serviceKey = apiKey;
                    serviceSecret = apiSecret;
                    serviceName = AppConstants.LocalApiServiceName;
                }
                
                
                
                var result = await clientLoginService.IsClientApiKeyValidAsync(apiKey, apiSecret, serviceKey, serviceSecret, serviceName, apiUrl, action);

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

        private string GetServiceNameFromPath(string path)
        {
            var serviceName = "";
            var tokens = path.Split("/");
            if (tokens.Length > 2)
            {
                serviceName = tokens[2];
            }

            return serviceName;
        }
    }

}