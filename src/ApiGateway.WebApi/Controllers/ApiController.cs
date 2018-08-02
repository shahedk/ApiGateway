using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Api")]
    public class ApiController : ApiControllerBase
    {
        private readonly IApiManager _manager;

        public ApiController(IApiManager manager, IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
            _manager = manager;
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
            return await _manager.Get(ApiKey, id);
        }
        
        // POST: api/Api
        [HttpPost]
        public async Task<ApiModel> Post([FromBody]ApiModel model)
        {
            var apiModel = await _manager.Create(ApiKey, model);

            return apiModel;
        }
        
        // PUT: api/Api/5
        [HttpPut("{id}")]
        public async Task<ApiModel> Put(string id, [FromBody]ApiModel model)
        {
            model.Id = id;
            var apiModel = await _manager.Update(ApiKey, model);

            return apiModel;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public  async Task  Delete(string id)
        {
            await _manager.Delete(ApiKey, id);
        }
    }
}
