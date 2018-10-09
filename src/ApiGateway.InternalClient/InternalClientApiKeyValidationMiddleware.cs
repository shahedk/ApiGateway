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
                string serviceKey = context.Request.Headers[ApiHttpHeaders.ServiceApiKey];
                string serviceSecret = context.Request.Headers[ApiHttpHeaders.ServiceApiSecret];
                string action = context.Request.Method;
                
                string serviceName = GetServiceNameFromPath(path);
                
                var apiName = GetApiNameFromPath(path);
                
                // TODO: Review: Do we need Service Key/Secret??
                if (path.StartsWith(AppConstants.SysApiUrlPrefix))
                {
                    // Local System API, client api is the owner. 
                    serviceKey = apiKey;
                    serviceSecret = apiSecret;
                    serviceName = AppConstants.SysApiName;
                }

                
                
                var result = await clientLoginService.IsClientApiKeyValidAsync(apiKey, apiSecret, serviceKey, serviceSecret, serviceName, apiName, action);

                if (result.IsValid)
                {
                    _logger.LogInformation(LogEvents.ApiKeyValidationPassed, path, serviceKey, apiKey, serviceName, apiName);
                    
                    // Add IDs into context to forward to actual API for client identification
                    context.Items.Add(ApiHttpHeaders.ApiKey, apiKey);
                    context.Items.Add(ApiHttpHeaders.KeyId, result.KeyId);
                    context.Items.Add(ApiHttpHeaders.ApiId, result.ApiId);
                    
                    await _next.Invoke(context);
                }
                else
                {
                    // Validation failed
                    _logger.LogInformation(LogEvents.ApiKeyValidationFailed, path, serviceKey, apiKey, serviceName, apiName);
                    
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(result.ToJson());
                }
            }
        }

        private string GetApiNameFromPath(string path)
        {
            var apiNameFromPath = "";
            var tokens = path.Split("/");
            if (tokens.Length > 3)
            {
                apiNameFromPath = tokens[3];
            }

            return apiNameFromPath;
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