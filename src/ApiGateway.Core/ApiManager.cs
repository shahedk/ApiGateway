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
        private readonly IRoleData _roleData;
        

        public ApiManager(IApiData apiData,IStringLocalizer<IApiManager> localizer,ILogger<IApiManager> logger, 
            IKeyManager keyManager, IRoleData roleData)
        {
            _apiData = apiData;
            _localizer = localizer;
            _logger = logger;
            _keyManager = keyManager;
            _roleData = roleData;
        }

        public async Task<ApiModel> Create(string ownerPublicKey, ApiModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            // Check if same name or url is already used by other api in the same service
            if (await _apiData.ExistsByName(ownerKey.Id, model.ServiceId, model.HttpMethod, model.Name))
            {
                var msg = _localizer["Another api with the same name and http method already exists"];
                throw new DataValidationException(msg, HttpStatusCode.Conflict);       
            }
                
                
            if (await _apiData.ExistsByUrl(ownerKey.Id, model.ServiceId, model.HttpMethod, model.Url))
            {
                var msg = _localizer["Another api with the same url and http method already exists"];
                throw new DataValidationException(msg, HttpStatusCode.Conflict);
            }
            
            return await _apiData.Create(model);
        }

        public async Task<ApiModel> Update(string ownerPublicKey, ApiModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerKeyId = ownerKey.Id;

            // Check if name is being updated then is the new name & http-method is unique
            var existing = await _apiData.Get(ownerKey.Id, model.Id);
            if (existing.Name != model.Name)
            {
                // Check if same name or url is already used by other api in the same service
                if (await _apiData.ExistsByName(ownerKey.Id, model.ServiceId, model.HttpMethod, model.Name))
                {
                    var msg = _localizer["Another api with the same name and http method already exists"];
                    throw new DataValidationException(msg, HttpStatusCode.Conflict);
                }
            }

            // Check if url is being updated then is the new url & http-method is unique
            if (existing.Url != model.Url)
            {
                if (await _apiData.ExistsByUrl(ownerKey.Id, model.ServiceId, model.HttpMethod, model.Url))
                {
                    var msg = _localizer["Another api with the same url and http method already exists"];
                    throw new DataValidationException(msg, HttpStatusCode.Conflict);
                }
            }

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
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            return await _apiData.GetAll(ownerKey.Id);
        }

        public async Task<ApiModel> GetByApiName(string ownerPublicKey, string serviceId, string httpMethod, string apiName)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            var api = await _apiData.GetByName(ownerKey.Id, serviceId, httpMethod, apiName);

            return api;
        }
        public async Task<ApiModel> GetByApiUrl(string ownerPublicKey, string serviceId, string httpMethod, string apiUrl)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            var api = await _apiData.GetByUrl(ownerKey.Id, serviceId, httpMethod, apiUrl);

            return api;
        }

        public async Task<int> Count(string ownerKeyId, string serviceId)
        {
            return await _apiData.Count(ownerKeyId, serviceId);
        }

        public async Task<IList<ApiSummaryModel>> GetAllSummary(string ownerPublicKey)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            var list = await _apiData.GetAll(ownerKey.Id);

            var result = new List<ApiSummaryModel>(list.Count);
            
            foreach (var a in list)
            {
                var api = new ApiSummaryModel(a)
                {
                    ActiveRoleCount = await _roleData.CountByApi(a.OwnerKeyId, a.Id, false),
                    DisabledRoleCount = await _roleData.CountByApi(a.OwnerKeyId, a.Id, true)
                };

                result.Add(api);
            }

            return result;
        }
    }
}