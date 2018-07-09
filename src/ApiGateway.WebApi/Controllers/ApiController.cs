using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Api")]
    public class ApiController : ApiControllerBase
    {
        private readonly IApiData _apiData;

        public ApiController(IApiData apiData, IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
            _apiData = apiData;
        }

        // GET: api/Api
        [HttpGet]
        public string Get()
        {
            return ":)";
        }

        // GET: api/Api/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ApiModel> Get(string id)
        {
            return await _apiData.Get(ApiKey, id);
        }
        
        // POST: api/Api
        [HttpPost]
        public async Task<ApiModel> Post([FromBody]ApiModel model)
        {
            var apiModel = await _apiData.Create(ApiKey, model);

            return apiModel;
        }
        
        // PUT: api/Api/5
        [HttpPut("{id}")]
        public async Task<ApiModel> Put(string id, [FromBody]ApiModel model)
        {
            model.Id = id;
            var apiModel = await _apiData.Update(ApiKey, model);

            return apiModel;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public  async Task  Delete(string id)
        {
            await _apiData.Delete(ApiKey, id);
        }
    }
}
