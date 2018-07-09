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
        private readonly IApiKeyValidator _keyValidator;
        private readonly IKeyData _keyData;
        private readonly IStringLocalizer<ServiceController> _stringLocalizer;
        private readonly IApiRequestHelper _apiRequestHelper;

        public ServiceController(IServiceData serviceData, IApiKeyValidator keyValidator, IKeyData keyData,
            IStringLocalizer<ServiceController> stringLocalizer, IApiRequestHelper apiRequestHelper)
        {
            _serviceData = serviceData;
            _keyValidator = keyValidator;
            _keyData = keyData;
            _stringLocalizer = stringLocalizer;
            _apiRequestHelper = apiRequestHelper;
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
            var publicKey = _apiRequestHelper.GetApiPublicKey();
            var model = await _serviceData.Get(publicKey, id);

            return model;
        }

        // POST: api/Service
        [HttpPost]
        public async Task<ServiceModel> Post([FromBody]ServiceModel model)
        {
            var publicKey = _apiRequestHelper.GetApiPublicKey();
            var result = await _serviceData.Create(publicKey, model);

            return result;
        }
        
        // PUT: api/Service/5
        [HttpPut("{id}")]
        public async Task<ServiceModel> Put(string id, [FromBody]ServiceModel model)
        {
            model.Id = id;
            var publicKey = _apiRequestHelper.GetApiPublicKey();
            var result = await _serviceData.Update(publicKey, model);

            return result;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var publicKey = _apiRequestHelper.GetApiPublicKey();
            await _serviceData.Delete(publicKey, id);
        }
    }
}
