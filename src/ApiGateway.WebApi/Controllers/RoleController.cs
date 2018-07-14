using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Data;
using ApiGateway.Data.EFCore.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Role")]
    public class RoleController : ApiControllerBase
    {
        private readonly IRoleData _roleData;

        public RoleController( IRoleData roleData, IApiRequestHelper apiRequestHelper):base(apiRequestHelper)
        {
            _roleData = roleData;
        }

        // GET: api/Role
        [HttpGet]
        public string Get()
        {
            return ":)";
        }

        // GET: api/Role/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<RoleModel> Get(string id)
        {
            return await _roleData.Get(ApiKey, id);
        }
        
        // POST: api/Role
        [HttpPost]
        public async Task<RoleModel> Post([FromBody]RoleModel model)
        {
            return await _roleData.Create(ApiKey, model);
        }

        [Route("/api/Role/AddKeyInRole")]
        [HttpPost]
        public async Task AddKeyInRole(string key, string roleId)
        {
            await _roleData.AddKeyInRole(ApiKey, roleId, key);
        }

        [Route("/api/Role/RemoveKeyFromRole")]
        [HttpPost]
        public async Task RemoveKeyFromRole(string key, string roleId)
        {
            await _roleData.RemoveKeyFromRole(ApiKey, roleId, key);
        }
        
        [Route("/api/Role/AddApiInRole")]
        [HttpPost]
        public async Task AddApiInRole(string apiId, string roleId)
        {
            await _roleData.AddApiInRole(ApiKey, roleId, apiId);
        }

        [Route("/api/Role/RemoveApiFromRole")]
        [HttpPost]
        public async Task RemoveApiFromRole(string apiId, string roleId)
        {
            await _roleData.RemoveApiFromRole(ApiKey, roleId, apiId);
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<RoleModel> Put(string id, [FromBody]RoleModel model)
        {
            model.Id = id;
            return await _roleData.Update(ApiKey, model);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _roleData.Delete(ApiKey, id);
        }
    }
}
