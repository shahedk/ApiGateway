using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("sys/AccessRule")]
    public class AccessRuleController : ApiControllerBase
    {
        private readonly IAccessRuleManager _manager;

        public AccessRuleController(IApiRequestHelper apiRequestHelper, IAccessRuleManager manager) : base(apiRequestHelper)
        {
            _manager = manager;
        }
        
        [HttpGet]
        public async Task<IList<AccessRuleModel>> Get()
        {
            try
            {
                return await _manager.GetAll(ApiKey);
            }
            catch (ApiGatewayException e)
            {
                if (e.ErrorCode == HttpStatusCode.NotFound)
                {
                    Response.StatusCode = (int)e.ErrorCode;
                    
                }
            }
            return null;
        }

        // GET: api/AccessRule/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/AccessRule
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/AccessRule/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
