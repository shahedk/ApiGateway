using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using ApiGateway.Data;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Key")]
    public class KeyController : ApiControllerBase
    {
        private readonly IKeyManager _manager;

        public KeyController( IKeyManager manager, IApiRequestHelper apiRequestHelper):base(apiRequestHelper)
        {
            _manager = manager;
        }

        // GET: api/Key
        [HttpGet]
        public string Get()
        {
            return ":)";
        }

        // GET: api/Key/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<KeyModel> Get(string id)
        {
            return await _manager.Get(ApiKey, id);
        }
        
        // POST: api/Key
        [HttpPost]
        public async Task<KeyModel> Post([FromBody]KeyModel model)
        {
            var keyModel = await _manager.Create(ApiKey, model);

            return keyModel;
        }
        
        // PUT: api/Key/5
        [HttpPut("{id}")]
        public async Task<KeyModel> Put(string id, [FromBody]KeyModel model)
        {
            model.Id = id;
            var keyModel = await _manager.Update(ApiKey, model);

            return keyModel;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _manager.Delete(ApiKey, id);
        }
    }
}
