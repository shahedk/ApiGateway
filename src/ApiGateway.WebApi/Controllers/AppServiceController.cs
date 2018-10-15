using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
using ApiGateway.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/{*url}")]
    public class AppServiceController : ApiControllerBase
    {
        private readonly IApiRequestHelper _apiRequestHelper;
        private readonly IApiManager _apiManager;

        public AppServiceController(IApiRequestHelper apiRequestHelper, IApiManager apiManager) : base(apiRequestHelper)
        {
            _apiRequestHelper = apiRequestHelper;
            _apiManager = apiManager;
        }

        private async Task<HttpWebClient> GetHttpWebClient()
        {
            var result = new HttpWebClient();
            
            var apiKey = HttpContext.Items[ApiHttpHeaders.ApiKey].ToString();
            var apiId = HttpContext.Items[ApiHttpHeaders.ApiId].ToString();
            var clientId = HttpContext.Items[ApiHttpHeaders.KeyId].ToString();

            var api = await _apiManager.Get(apiKey, apiId);

            var client = new HttpClient {BaseAddress = new Uri(api.Url)};
            client.DefaultRequestHeaders.Add("clientId", clientId);
            if (api.CustomHeaders.Keys.Count > 0)
            {
                foreach (var key in api.CustomHeaders.Keys)
                {
                    if (string.Equals(key, "Content-Type",StringComparison.OrdinalIgnoreCase) )
                    {
                        // Skip, it will be added in request message
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add(key, api.CustomHeaders[key]);    
                    }
                }
            }

            result.HttpClient = client;
            result.QueryString = GetQueryString(api.Name);
            return result;
        }
        
        [HttpGet]
        public async Task Get()
        {
            var webClient = await GetHttpWebClient();

            var response = await webClient.HttpClient.GetAsync(webClient.QueryString);
            var content = await response.Content.ReadAsStringAsync();

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            await Response.WriteAsync(content);
        }



        [HttpPut]
        public async Task Put()
        {
            var webClient = await GetHttpWebClient();
            
            using (var streamContent = new StreamContent(Request.Body))
            {
                var response = await webClient.HttpClient.PutAsync(webClient.QueryString, streamContent);
                var content = await response.Content.ReadAsStringAsync();

                Response.StatusCode = (int)response.StatusCode;

                Response.ContentType = response.Content.Headers.ContentType?.ToString();
                Response.ContentLength = response.Content.Headers.ContentLength;

                await Response.WriteAsync(content);
            }
        }


        [HttpPost]
        public async Task Post()
        {
            var webClient = await GetHttpWebClient();

            var reqStream = new StreamContent(Request.Body);
            var reqBody = await reqStream.ReadAsStringAsync();
            var reqContent = new StringContent(reqBody,Encoding.UTF8,"application/json");
            
            var request = new HttpRequestMessage(HttpMethod.Post, webClient.QueryString ) {Content = reqContent};

            var response = await webClient.HttpClient.SendAsync(request);

            Response.StatusCode = (int) response.StatusCode;

            Response.ContentType = response.Content.Headers.ContentType?.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            var responseStream = await response.Content.ReadAsStreamAsync();
            var x = new StreamContent(responseStream);
            var strRes = await x.ReadAsStringAsync();
            await Response.WriteAsync(strRes);

        }


        [HttpDelete]
        public  async Task  Delete()
        {
            var webClient = await GetHttpWebClient();
            
            var response = await webClient.HttpClient.DeleteAsync(webClient.QueryString);
            var content = await response.Content.ReadAsStringAsync();

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            await Response.WriteAsync(content);
        }
        
        
        private string GetQueryString(string apiName)
        {
            if (string.IsNullOrWhiteSpace(apiName))
            {
                var idVal = _apiRequestHelper.GetApiName();

                return idVal + (Request.QueryString.HasValue ? Request.QueryString.Value: "");
            }

            return Request.QueryString.HasValue ? Request.QueryString.Value: "";
        }

        private struct HttpWebClient
        {
            public HttpClient HttpClient;
            public string QueryString;
        }
    }
}