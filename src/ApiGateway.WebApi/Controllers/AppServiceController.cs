using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
using ApiGateway.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/{*url}")]
    public class AppServiceController : ApiControllerBase
    {
        private readonly IApiRequestHelper _apiRequestHelper;
        private readonly IApiManager _apiManager;
        private readonly IServiceManager _serviceManager;
        private readonly IHttpClientFactory _clientFactory;

        public AppServiceController(IApiRequestHelper apiRequestHelper, IApiManager apiManager, IServiceManager serviceManager, IHttpClientFactory clientFactory) : base(apiRequestHelper)
        {
            _apiRequestHelper = apiRequestHelper;
            _apiManager = apiManager;
            _serviceManager = serviceManager;
            _clientFactory = clientFactory;
        }


        private async Task<HttpRequestMessage> GetRequestMessage(HttpMethod method)
        {
            var apiKey = HttpContext.Items[ApiHttpHeaders.ApiKey].ToString();
            var apiId = HttpContext.Items[ApiHttpHeaders.ApiId].ToString();
            var serviceId = HttpContext.Items[ApiHttpHeaders.ServiceId].ToString();
            var clientId = HttpContext.Items[ApiHttpHeaders.KeyId].ToString();

            var api = await _apiManager.Get(apiKey, apiId);
            var service = await _serviceManager.Get(apiKey, serviceId);

            var queryString = GetQueryString(api.Name, service.Name);
            var fullUrl = api.Url;
            if (!string.IsNullOrWhiteSpace(queryString))
            {
                if (fullUrl.EndsWith("/") || queryString.StartsWith("/"))
                {
                    fullUrl += queryString;
                }
                else
                {
                    fullUrl += "/" + queryString;
                }
            }
            var request = new HttpRequestMessage(method, fullUrl);
            
            request.Headers.Add("clientid", clientId);
            request.Headers.Add("apiid", apiId);
            request.Headers.Add("apigateway-url", Request.GetDisplayUrl());
            
            if (api.CustomHeaders.Keys.Count > 0)
            {
                foreach (var key in api.CustomHeaders.Keys)
                {
                    request.Headers.Add(key, api.CustomHeaders[key]);
                }
            }

            return request;
        }
        
        [HttpGet]
        public async Task Get()
        {
            var client = _clientFactory.CreateClient();
            var request = await GetRequestMessage(HttpMethod.Get);

            var response = await client.SendAsync(request);            
            var content = await response.Content.ReadAsStringAsync();

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            await Response.WriteAsync(content);
        }

        

        [HttpPost]
        public async Task Post()
        {
            var client = _clientFactory.CreateClient();
            var request = await GetRequestMessage(HttpMethod.Post);

            var reqStream = new StreamContent(Request.Body);
            var reqBody = await reqStream.ReadAsStringAsync();
            request.Content = new StringContent(reqBody,Encoding.UTF8,"application/json");
            
            
            var response = await client.SendAsync(request);

            Response.StatusCode = (int) response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType?.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            var content = await response.Content.ReadAsStringAsync();
            await Response.WriteAsync(content);

        }

        
        [HttpPut]
        public async Task Put()
        {
            var client = _clientFactory.CreateClient();
            var request = await GetRequestMessage(HttpMethod.Put);

            var reqStream = new StreamContent(Request.Body);
            var reqBody = await reqStream.ReadAsStringAsync();
            request.Content = new StringContent(reqBody,Encoding.UTF8,"application/json");
            
            
            var response = await client.SendAsync(request);

            Response.StatusCode = (int) response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType?.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            var content = await response.Content.ReadAsStringAsync();
            await Response.WriteAsync(content);

        }


        [HttpDelete]
        public  async Task  Delete()
        {
            var client = _clientFactory.CreateClient();
            var request = await GetRequestMessage(HttpMethod.Delete);

            var response = await client.SendAsync(request);

            Response.StatusCode = (int) response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType?.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            var content = await response.Content.ReadAsStringAsync();
            await Response.WriteAsync(content);

        }
        
        
        private string GetQueryString(string apiName, string serviceName)
        {
            var apiUrl = serviceName;
            if (!string.IsNullOrEmpty(apiName))
            {
                apiUrl += "/" + apiName;
            }
            
            var url = Request.GetDisplayUrl();
            var lastIndexOf = url.LastIndexOf(apiUrl) + apiUrl.Length;
            
            if (url.Length == lastIndexOf)
            {
                return string.Empty;
            }
            else
            {
                var queryString = url.Substring(lastIndexOf);
                if (queryString.StartsWith("/"))
                {
                    queryString = queryString.Substring(1);
                }

                return queryString;
            }
        }

    }
}