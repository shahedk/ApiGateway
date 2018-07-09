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
