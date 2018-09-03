using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Role")]
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleManager _manager;

        public RoleController( IRoleManager manager, IApiRequestHelper apiRequestHelper):base(apiRequestHelper)
        {
            _manager = manager;
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<RoleModel> Get(string id)
        {
            return await _manager.Get(ApiKey, id);
        }
        
        // POST: api/Role
        [HttpPost]
        public async Task<RoleModel> Post([FromBody]RoleModel model)
        {
            return await _manager.Create(ApiKey, model);
        }

        [Route("/api/Role/AddKeyInRole")]
        [HttpPost]
        public async Task AddKeyInRole(string key, string roleId)
        {
            await _manager.AddKeyInRole(ApiKey, roleId, key);
        }

        [Route("/api/Role/RemoveKeyFromRole")]
        [HttpPost]
        public async Task RemoveKeyFromRole(string key, string roleId)
        {
            await _manager.RemoveKeyFromRole(ApiKey, roleId, key);
        }
        
        [Route("/api/Role/AddApiInRole")]
        [HttpPost]
        public async Task AddApiInRole(string apiId, string roleId)
        {
            await _manager.AddApiInRole(ApiKey, roleId, apiId);
        }

        [Route("/api/Role/RemoveApiFromRole")]
        [HttpPost]
        public async Task RemoveApiFromRole(string apiId, string roleId)
        {
            await _manager.RemoveApiFromRole(ApiKey, roleId, apiId);
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<RoleModel> Put(string id, [FromBody]RoleModel model)
        {
            model.Id = id;
            return await _manager.Update(ApiKey, model);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _manager.Delete(ApiKey, id);
        }
    }
}
