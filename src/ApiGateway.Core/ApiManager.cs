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
    public class ApiManager : IApiManager
    {
        private readonly IApiData _apiData;
        private readonly IStringLocalizer<IApiManager> _localizer;
        private readonly ILogger<IApiManager> _logger;
        private readonly IKeyManager _keyManager;

        public ApiManager(IApiData apiData,IStringLocalizer<IApiManager> localizer,ILogger<IApiManager> logger, IKeyManager keyManager)
        {
            _apiData = apiData;
            _localizer = localizer;
            _logger = logger;
            _keyManager = keyManager;
        }

        public async Task<ApiModel> Create(string ownerPublicKey, ApiModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            return await _apiData.Create(model);
        }

        public async Task<ApiModel> Update(string ownerPublicKey, ApiModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            return await _apiData.Update(model);
        }

        public async Task Delete(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            await _apiData.Delete(ownerKey.Id, id);
        }

        public async Task<ApiModel> Get(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            
            var model = await _apiData.Get(ownerKey.Id, id);

            if (model == null)
            {
                var errorMessage = _localizer["No Api found for the specified owner and Id"];
                throw new ItemNotFoundException(errorMessage);
            }
            else
            {
                return model;
            }

        }

        public async Task<IList<ApiModel>> GetAll(string ownerPublicKey)
        {
            return await _apiData.GetAll(ownerPublicKey);
        }

        public async Task<ApiModel> Get(string ownerPublicKey, string serviceId, string httpMethod, string apiUrl)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            var api = await _apiData.Get(ownerKey.Id, serviceId, httpMethod, apiUrl);

            return api;
        }
    }
}