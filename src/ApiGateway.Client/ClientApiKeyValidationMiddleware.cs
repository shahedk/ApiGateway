﻿using System.Net;
using System.Text.RegularExpressions;
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
        private readonly IClientApiKeyService _clientApiKeyService;
        private readonly ApiGatewaySettings _settings;
        private static Regex _allowAnnon;

        public ClientApiKeyValidationMiddleware(RequestDelegate next, IClientApiKeyService clientApiKeyService, IOptions<ApiGatewaySettings> settings)
        {
            _next = next;
            _clientApiKeyService = clientApiKeyService;
            _settings = settings.Value;

            if (!string.IsNullOrWhiteSpace(_settings.AllowAnonymousApiPath) && _settings.AllowAnonymousApiPath.Length > 0)
            {
                _allowAnnon = new Regex(_settings.AllowAnonymousApiPath, RegexOptions.IgnoreCase);
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();

            if (_allowAnnon != null && _allowAnnon.IsMatch(path))
            {
                // Skip ApiKey validation. This path is allowed for anonymous access
                await _next.Invoke(context);
            }
            else
            {
                if (!context.Request.Headers.Keys.Contains(ApiHttpHeaders.ApiKey) ||
                    !context.Request.Headers.Keys.Contains(ApiHttpHeaders.ApiSecret))
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync(
                        $"{ApiHttpHeaders.ApiKey} and/or {ApiHttpHeaders.ApiSecret} missing");
                }
                else
                {
                    string apiKey = context.Request.Headers[ApiHttpHeaders.ApiKey];
                    string apiSecret = context.Request.Headers[ApiHttpHeaders.ApiSecret];
                    string action = context.Request.Method;
                    string apiName = context.Request.GetApiName();

                    var result = await _clientApiKeyService.IsClientApiKeyValidAsync(apiKey, apiSecret,
                        _settings.ServiceName, apiName, action);

                    if (result.IsValid)
                    {
                        context.Items.Add(ApiHttpHeaders.ApiKey, apiKey);
                        await _next.Invoke(context);
                    }
                    else
                    {
                        // Validation failed
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync(result.ToJson());
                    }
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
