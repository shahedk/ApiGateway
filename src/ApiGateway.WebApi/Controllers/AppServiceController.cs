using System.Net.Http;
using System.Threading.Tasks;
using ApiGateway.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/{*url}")]
    public class AppServiceController : ApiControllerBase
    {
        private readonly HttpClient _httpClient;

        public AppServiceController(IApiRequestHelper apiRequestHelper, HttpClient httpClient) : base(apiRequestHelper)
        {
            _httpClient = httpClient;
        }
       
        [HttpGet]
        public async Task Get()
        {
            var path = Request.Path;
            
            var queryString = Request.QueryString;
            var response = await _httpClient.GetAsync(queryString.Value);
            var content = await response.Content.ReadAsStringAsync();

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            await Response.WriteAsync(content);
        }
        
        
        [HttpPut]
        public async Task Put()
        {
            using (var streamContent = new StreamContent(Request.Body))
            {
                var response = await _httpClient.PutAsync(string.Empty, streamContent);
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
            using (var streamContent = new StreamContent(Request.Body))
            {
                var response = await _httpClient.PostAsync(string.Empty, streamContent);
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
            var queryString = Request.QueryString;
            var response = await _httpClient.DeleteAsync(queryString.Value);
            var content = await response.Content.ReadAsStringAsync();

            Response.StatusCode = (int)response.StatusCode;
            Response.ContentType = response.Content.Headers.ContentType.ToString();
            Response.ContentLength = response.Content.Headers.ContentLength;

            await Response.WriteAsync(content);
        }
    }
}