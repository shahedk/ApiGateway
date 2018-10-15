using System.Net;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApiGateway.InternalClient
{
    public class InternalClientApiKeyValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<InternalClientApiKeyValidationMiddleware> _logger;

        public InternalClientApiKeyValidationMiddleware(RequestDelegate next, ILogger<InternalClientApiKeyValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
                string action = context.Request.Method;
                
                var serviceName = context.Request.GetServiceName();
                var apiName = context.Request.GetApiName();
                
                var result = await clientLoginService.IsClientApiKeyValidAsync(apiKey, apiSecret, serviceName, apiName, action);

                if (result.IsValid)
                {
                    _logger.LogInformation(LogEvents.ApiKeyValidationPassed, path, apiKey, serviceName, apiName);
                    
                    // Add IDs into context to forward to actual API for client identification
                    context.Items.Add(ApiHttpHeaders.ApiKey, apiKey);
                    context.Items.Add(ApiHttpHeaders.KeyId, result.KeyId);
                    context.Items.Add(ApiHttpHeaders.ApiId, result.ApiId);
                    
                    await _next.Invoke(context);
                }
                else
                {
                    // Validation failed
                    _logger.LogInformation(LogEvents.ApiKeyValidationFailed, path, apiKey, serviceName, apiName);
                    
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(result.ToJson());
                }
            }
        }

    }

}