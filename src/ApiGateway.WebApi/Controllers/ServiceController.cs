﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using ApiGateway.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("sys/Service")]
    public class ServiceController : ApiControllerBase
    {
        private readonly IServiceManager _manager;

        public ServiceController(IServiceManager manager, IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
            _manager = manager;
        }

        [HttpGet("/sys/service-summary/")]
        public async Task<IList<ServiceSummaryModel>> GetSummary()
        {
            try
            {
                return await _manager.GetAllSummary(ApiKey);
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
        
        [HttpGet]
        public async Task<IList<ServiceModel>> Get()
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
        
        // GET: api/Service/5
        [HttpGet("/sys/service-detail/{id}")]
        public async Task<ServiceModel> Get(string id)
        {
            var model = await _manager.Get(ApiKey, id);

            return model;
        }

        // POST: api/Service
        [HttpPost]
        public async Task<ServiceModel> Post([FromBody]ServiceModel model)
        {
            var result = await _manager.Create(ApiKey, model);

            return result;
        }
        
        // PUT: api/Service/5
        [HttpPut("{id}")]
        public async Task<ServiceModel> Put(string id, [FromBody]ServiceModel model)
        {
            model.Id = id;
            var result = await _manager.Update(ApiKey, model);

            return result;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _manager.Delete(ApiKey, id);
        }
    }
}
