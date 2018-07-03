using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Data;
using ApiGateway.Data.EFCore.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Key")]
    public class KeyController : Controller
    {
        private readonly IKeyData _keyData;
        private readonly IApiRequestHelper _apiRequestHelper;

        public KeyController( IKeyData keyData, IApiRequestHelper apiRequestHelper)
        {
            _keyData = keyData;
            _apiRequestHelper = apiRequestHelper;
        }

        // GET: api/Key
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Key/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<KeyModel> Get(string id)
        {
            var publicKey = _apiRequestHelper.GetApiPublicKey();

            return await _keyData.Get(publicKey, id);
        }
        
        // POST: api/Key
        [HttpPost]
        public async Task<KeyModel> Post([FromBody]KeyModel model)
        {
            var publicKey = _apiRequestHelper.GetApiPublicKey();

            var keyModel = await _keyData.Create(publicKey, model);

            return keyModel;
        }
        
        // PUT: api/Key/5
        [HttpPut("{id}")]
        public async Task<KeyModel> Put(string id, [FromBody]KeyModel model)
        {
            var publicKey = _apiRequestHelper.GetApiPublicKey();

            model.Id = id;

            var keyModel = await _keyData.Update(publicKey, model);

            return keyModel;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var publicKey = _apiRequestHelper.GetApiPublicKey();

            await _keyData.Delete(publicKey, id);
        }
    }
}
