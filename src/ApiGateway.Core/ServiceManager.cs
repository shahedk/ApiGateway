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
        private readonly IRoleManager _roleManager;
        private readonly IApiManager _apiManager;

        public ServiceManager(IServiceData serviceData, IStringLocalizer<IServiceManager> localizer, ILogger<IServiceManager> logger, 
            IKeyManager keyManager, IRoleManager roleManager, IApiManager apiManager
            )
        {
            _serviceData = serviceData;
            _localizer = localizer;
            _logger = logger;
            _keyManager = keyManager;
            _roleManager = roleManager;
            _apiManager = apiManager;
        }

        public async Task<ServiceModel> Create(string ownerPublicKey, ServiceModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            // Service name must be unique
            if (await _serviceData.Exists(model.Name))
            {
                var msg = _localizer["Another service with the same name already exists"];
                throw new DataValidationException(msg, HttpStatusCode.Conflict);
            }
            
            return await _serviceData.Create(model);
        }

        public async Task<ServiceModel> Update(string ownerPublicKey, ServiceModel model)
        {
            // Check if exists, otherwise it will throw exception
            await Get(ownerPublicKey, model.Id);

            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            // Check if name changed and whether another service exists with the same name
            var existing = await _serviceData.GetByName(model.Name);
            if ( existing != null && existing.Id != model.Id)
            {
                var msg = _localizer["Another service with the same name already exists"];
                throw new DataValidationException(msg, HttpStatusCode.Conflict);
            }
            
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

        public async Task<List<ServiceSummaryModel>> GetAllSummary(string ownerPublicKey)
        {
            var services = await GetAll(ownerPublicKey);

            var result = new List<ServiceSummaryModel>();
            foreach (var s in services)
            {
                var summary = new ServiceSummaryModel(s);

                summary.ActiveRoleCount = await _roleManager.Count(s.OwnerKeyId, s.Id, false);
                summary.DisabledRoleCount = await _roleManager.Count(s.OwnerKeyId, s.Id, true);

                summary.ApiCount = await _apiManager.Count(s.OwnerKeyId, s.Id);
                
                result.Add(summary);
            }

            return result;
        }

        public async Task<ServiceModel> GetByName(string ownerPublicKey, string serviceName)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);

            if (ownerKey == null) return null;
            
            return await _serviceData.GetByName(ownerKey.Id, serviceName);
        }

        public async Task<int> Count()
        {
            return await _serviceData.Count();
        }
    }
}