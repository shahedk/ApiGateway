using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using ApiGateway.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Service")]
    public class ServiceController : ApiControllerBase
    {
        private readonly IServiceData _serviceData;

        public ServiceController(IServiceData serviceData, IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
            _serviceData = serviceData;
        }


        // GET: api/Service
        [HttpGet]
        public string Get()
        {
            return ":)";
        }

        // GET: api/Service/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ServiceModel> Get(string id)
        {
            var model = await _serviceData.Get(ApiKey, id);

            return model;
        }

        // POST: api/Service
        [HttpPost]
        public async Task<ServiceModel> Post([FromBody]ServiceModel model)
        {
            var result = await _serviceData.Create(ApiKey, model);

            return result;
        }
        
        // PUT: api/Service/5
        [HttpPut("{id}")]
        public async Task<ServiceModel> Put(string id, [FromBody]ServiceModel model)
        {
            model.Id = id;
            var result = await _serviceData.Update(ApiKey, model);

            return result;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _serviceData.Delete(ApiKey, id);
        }
    }
}
