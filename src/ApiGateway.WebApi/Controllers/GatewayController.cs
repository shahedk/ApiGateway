using System.Collections.Generic;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Gateway")]
    public class GatewayController : ApiControllerBase
    {
        public GatewayController(IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
        }

        
        // GET: api/Gateway/{gateway-name-id}/?params=
        [HttpGet("{id}")]
        public GatewayResponseModel Get(string id ,FormCollection otherParams)
        {
            return new GatewayResponseModel();
        }
         
        
        // POST: api/Gateway/{gateway-name-id}
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Gateway/{gateway-name-id}
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/Gateway/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}