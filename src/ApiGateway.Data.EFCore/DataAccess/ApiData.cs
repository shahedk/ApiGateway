using System;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ApiData: IApiData
    {
        private readonly ApiGatewayContext _context;
        private readonly IStringLocalizer<ApiData> _localizer;
        private readonly ILogger<ApiData> _logger;
        private readonly IKeyData _keyData;

        public ApiData(ApiGatewayContext context, IStringLocalizer<ApiData> localizer, ILogger<ApiData> logger, IKeyData keyData)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
            _keyData = keyData;
        }

        public async Task<ApiModel> Create(string ownerPublicKey, ApiModel model)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);

            model.OwnerKeyId = ownerKey.Id;
            var entity = model.ToEntity();

            _context.Apis.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public async Task<ApiModel> Update(string ownerPublicKey, ApiModel model)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            var ownerKeyId = int.Parse(ownerKey.Id);
            var apiId = int.Parse(model.Id);

            var existing = await _context.Apis.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == apiId);

            existing.Name = model.Name;
            existing.HttpMethod = model.HttpMethod;
            existing.ServiceId = int.Parse(model.ServiceId);
            existing.Url = model.Url;
            existing.OwnerKeyId = int.Parse(model.OwnerKeyId);
            existing.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existing.ToModel();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiModel> Get(string ownerPublicKey, string id)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            var ownerKeyId = int.Parse(ownerKey.Id);
            var keyId = int.Parse(id);
            var entity = await _context.Apis.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == keyId);

            var roles = await _context.ApiInRoles.Where(x => x.ApiId== entity.Id).Select(x => x.Role.ToModel()).ToListAsync();

            return entity.ToModel(roles);
        }

        public async Task<ApiModel> Get(string ownerPublicKey, string serviceId, string httpMethod, string apiUrl)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            var ownerKeyId = int.Parse(ownerKey.Id);
            var serviceId2 = int.Parse(serviceId);
            var url = string.IsNullOrEmpty(apiUrl) ? string.Empty : apiUrl.ToLower();

            var api = await _context.Apis.SingleOrDefaultAsync(x =>
                x.OwnerKeyId == ownerKeyId && x.ServiceId == serviceId2 && x.HttpMethod == httpMethod &&
                x.Url == url);

            var roles = await _context.ApiInRoles.Where(x => x.ApiId== api.Id).Select(x => x.Role.ToModel()).ToListAsync();

            return api.ToModel(roles);
        }
    }
}