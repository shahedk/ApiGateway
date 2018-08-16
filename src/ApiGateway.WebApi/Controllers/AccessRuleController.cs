﻿using System.Collections.Generic;
using ApiGateway.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/AccessRule")]
    public class AccessRuleController : ApiControllerBase
    {
        public AccessRuleController(IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {

        }

        // GET: api/AccessRule
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AccessRule/5
        [HttpGet("{id}", Name = "Get")]
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
