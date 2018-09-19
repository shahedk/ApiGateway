using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Data;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Core
{
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceData _serviceData;
        private readonly IStringLocalizer<IServiceManager> _localizer;
        private readonly ILogger<IServiceManager> _logger;
        private readonly IKeyManager _keyManager;

        public ServiceManager(IServiceData serviceData, IStringLocalizer<IServiceManager> localizer, ILogger<IServiceManager> logger, IKeyManager keyManager)
        {
            _serviceData = serviceData;
            _localizer = localizer;
            _logger = logger;
            _keyManager = keyManager;
        }

        public async Task<ServiceModel> Create(string ownerPublicKey, ServiceModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            return await _serviceData.Create(model);
        }

        public async Task<ServiceModel> Update(string ownerPublicKey, ServiceModel model)
        {
            // Check if exists, otherwise it will throw exception
            await Get(ownerPublicKey, model.Id);

            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            return await _serviceData.Update(model);
        }

        public async Task Delete(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);

            await _serviceData.Delete(ownerKey.Id, id);
        }

         public async Task<ServiceModel> Get(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);

            var model = await _serviceData.Get(ownerKey.Id, id);

                if(model == null)
                {
                    var msg = _localizer["Service not found for the specified owner and id"];
                    throw new ItemNotFoundException(msg, HttpStatusCode.NotFound);
                }

            return model;
        }

        public async Task<IList<ServiceModel>> GetAll(string ownerPublicKey)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            return await _serviceData.GetAll(ownerKey.Id);
        }

        public async Task<ServiceModel> GetByName(string ownerPublicKey, string serviceName)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            return await _serviceData.GetByName(ownerKey.Id, serviceName);
        }

        public async Task<int> Count()
        {
            return await _serviceData.Count();
        }
    }
}