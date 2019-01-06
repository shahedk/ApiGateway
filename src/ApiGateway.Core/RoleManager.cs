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
    public class RoleManager : IRoleManager
    {
        private readonly IRoleData _roleData;
        private readonly IKeyManager _keyManager;
        private readonly IStringLocalizer<RoleManager> _localizer;
        private readonly ILogger<RoleManager> _logger;


        public RoleManager(IRoleData roleData, IStringLocalizer<RoleManager> localizer, ILogger<RoleManager> logger, IKeyManager keyManager)
        {
            _roleData = roleData;
            _localizer = localizer;
            _logger = logger;
            _keyManager = keyManager;
        }

        public async Task<RoleModel> Create(string ownerPublicKey, RoleModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerId = ownerKey.Id;

            return await _roleData.Create(model);
        }

        public async Task<RoleModel> Update(string ownerPublicKey, RoleModel model)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            model.OwnerId = ownerKey.Id;

            return await _roleData.Update(model);
        }

        public async Task Delete(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            await _roleData.Delete(ownerKey.Id, id);
        }

        public async Task<RoleModel> Get(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            var model = await _roleData.Get(ownerKey.Id, id);
            if (model == null)
            {
                var msg = _localizer["Role not found for the specified owner and id"];
                throw new ItemNotFoundException(msg, HttpStatusCode.NotFound);
            }

            return model;
        }

        public async Task<IList<RoleModel>> GetAll(string ownerPublicKey)
        {
            var ownerKey = await _keyManager.GetByPublicKey(ownerPublicKey);
            return await _roleData.GetAll(ownerKey.Id);
        }

        public async Task AddKeyInRole(string roleOwnerPublicKey, string roleId, string keyPublicKey)
        {
            var ownerKey = await _keyManager.GetByPublicKey(roleOwnerPublicKey);
            var key = await _keyManager.GetByPublicKey(keyPublicKey);

            if (await _roleData.IsKeyInRole(ownerKey.Id, roleId, key.Id))
            {
                var msg = _localizer["Key already in role"];
                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }
            else
            {
                await _roleData.AddKeyInRole(ownerKey.Id, roleId, key.Id);
            }
        }

        public async  Task RemoveKeyFromRole(string roleOwnerPublicKey, string roleId, string keyPublicKey)
        {
            var ownerKey = await _keyManager.GetByPublicKey(roleOwnerPublicKey);
            var key = await _keyManager.GetByPublicKey(keyPublicKey);

            if (await _roleData.IsKeyInRole(ownerKey.Id, roleId, key.Id))
            {
                await _roleData.RemoveKeyFromRole(ownerKey.Id, roleId, key.Id);
            }
            else
            {
                var msg = _localizer["Key does not exits in role"];
                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }
        }

        public  async Task AddApiInRole(string roleOwnerPublicKey, string roleId, string apiId)
        {
            var ownerKey = await _keyManager.GetByPublicKey(roleOwnerPublicKey);

            if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(apiId))
            {
                var msg = _localizer["RoleId or ApiId can not be null"];
                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }
            
            if (await _roleData.IsApiInRole(ownerKey.Id, roleId, apiId))
            {
                var msg = _localizer["Api already in role"];
                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }
            else
            {
                await _roleData.AddApiInRole(ownerKey.Id, roleId, apiId);
            }
        }

        public  async Task RemoveApiFromRole(string roleOwnerPublicKey, string roleId, string apiId)
        {
            var ownerKey = await _keyManager.GetByPublicKey(roleOwnerPublicKey);

            if (await _roleData.IsApiInRole(ownerKey.Id, roleId, apiId))
            {
                await _roleData.RemoveApiFromRole(ownerKey.Id, roleId, apiId);
            }
            else
            {
                var msg = _localizer["Api does not exits in role"];
                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }
        }

        public async Task<int> CountByService(string roleOwnerPublicKey, string serviceId, bool isDisabled)
        {
            return await _roleData.CountByService(roleOwnerPublicKey, serviceId, isDisabled);
        }

        public async Task<IList<RoleSummaryModel>> GetAllSummary(string ownerPublicKey)
        {
            var list = await GetAll(ownerPublicKey);

            var result = new List<RoleSummaryModel>(list.Count);

            foreach (var r in list)
            {
                var role = new RoleSummaryModel(r)
                {
                    ApiCount = await _roleData.ApiCountInRole(r.OwnerId, r.Id),
                    KeyCount = await _roleData.KeyCountInRole(r.OwnerId, r.Id)
                };

                result.Add(role);
            }

            return result;
        }
    }
}