using System;
using System.Net.Http;
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
        private readonly IApiManager _apiManager;

        public AppServiceController(IApiRequestHelper apiRequestHelper, IApiManager apiManager) : base(apiRequestHelper)
        {
            _apiManager = apiManager;
        }

        private async Task<HttpClient> GetHttpClient()
        {
            var apiKey = HttpContext.Items[ApiHttpHeaders.ApiKey].ToString();
            var apiId = HttpContext.Items[ApiHttpHeaders.ApiId].ToString();
            var clientId = HttpContext.Items[ApiHttpHeaders.KeyId].ToString();

            var api = await _apiManager.Get(apiKey, apiId);

            var client = new HttpClient {BaseAddress = new Uri(api.Url)};
            client.DefaultRequestHeaders.Add("clientId", clientId);

            return client;
        }
        
        [HttpGet]
        public async Task Get()
        {
            var client = await GetHttpClient();
            
            var queryString = Request.QueryString;
            var response = await client.GetAsync(queryString.Value);
            var content = await response.Content.ReadAsStringAsync();

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            await Response.WriteAsync(content);
        }
        
        
        [HttpPut]
        public async Task Put()
        {
            var client = await GetHttpClient();
            
            using (var streamContent = new StreamContent(Request.Body))
            {
                var response = await client.PutAsync(string.Empty, streamContent);
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
            var client = await GetHttpClient();
            
            using (var streamContent = new StreamContent(Request.Body))
            {
                var response = await client.PostAsync(string.Empty, streamContent);
                var content = await response.Content.ReadAsStringAsync();

                Response.StatusCode = (int)response.StatusCode;

                Response.ContentType = response.Content.Headers.ContentType?.ToString();
                Response.ContentLength = response.Content.Headers.ContentLength;

                await Response.WriteAsync(content);
            }
        }
        
        [HttpDelete]
        public  async Task  Delete()
        {
            var client = await GetHttpClient();
            
            var queryString = Request.QueryString;
            var response = await client.DeleteAsync(queryString.Value);
            var content = await response.Content.ReadAsStringAsync();

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            await Response.WriteAsync(content);
        }
    }
}