using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Entity;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ApiData: IApiData
    {
        private readonly ApiGatewayContext _context;
        public ApiData(ApiGatewayContext context)
        {
            _context = context;
        }

        public async Task<ApiModel> Create(ApiModel model)
        {
            var entity = model.ToEntity();

            _context.Apis.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public async Task<ApiModel> Update(ApiModel model)
        {
            var ownerKey = int.Parse(model.OwnerKeyId);
            var apiId = int.Parse(model.Id);
            var existing = await _context.Apis.SingleOrDefaultAsync(x => x.OwnerKeyId ==  ownerKey && x.Id == apiId);

            existing.Name = model.Name;
            existing.HttpMethod = model.HttpMethod;
            existing.ServiceId = int.Parse(model.ServiceId);
            existing.Url = model.Url;
            existing.OwnerKeyId = int.Parse(model.OwnerKeyId);
            existing.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existing.ToModel();
        }

        public async Task Delete(string ownerKeyId, string id)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var keyId = int.Parse(id);
            var entity = await _context.Apis.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKey && x.Id == keyId);

            _context.Apis.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ApiModel> Get(string ownerKeyId, string id)
        {
            var entity = await GetEntity(ownerKeyId, id);

            if (entity == null)
            {
                return null;
            }
            else
            {
                var roles = await _context.ApiInRoles.Where(x => x.ApiId == entity.Id).Select(x => x.Role.ToModel())
                    .ToListAsync();

                return entity.ToModel(roles);
            }
        }

        public async Task<IList<ApiModel>> GetAll(string ownerKeyId)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var list = await _context.Apis.Where(x => x.OwnerKeyId == ownerKey).Select(x=>x.ToModel()).ToListAsync();

            return list;
        }

        private async Task<Api> GetEntity(string ownerKeyId, string id)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var keyId = int.Parse(id);
            var entity = await _context.Apis.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKey && x.Id == keyId);

            return entity;
        }

        public async Task<ApiModel> GetByUrl(string ownerKeyId, string serviceId, string httpMethod, string apiUrl)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var serviceId2 = int.Parse(serviceId);
            var url = string.IsNullOrEmpty(apiUrl) ? string.Empty : apiUrl.ToLower();

            var api = await _context.Apis.SingleOrDefaultAsync(x =>
                x.OwnerKeyId == ownerKey && x.ServiceId == serviceId2 && x.HttpMethod == httpMethod &&
                x.Url == url);

            if (api == null)
            {
                return null;
            }

            var roles = await _context.ApiInRoles.Where(x => x.ApiId== api.Id).Select(x => x.Role.ToModel()).ToListAsync();
            return api.ToModel(roles);
        }

        public async Task<ApiModel> GetByName(string ownerKeyId, string serviceId, string httpMethod, string name)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var serviceId2 = int.Parse(serviceId);
            var apiName = string.IsNullOrEmpty(name) ? string.Empty : name.ToLower();

            var api = await _context.Apis.SingleOrDefaultAsync(x =>
                x.OwnerKeyId == ownerKey && x.ServiceId == serviceId2 && x.HttpMethod == httpMethod &&
                x.Name == apiName);

            if (api == null)
            {
                return null;
            }

            var roles = await _context.ApiInRoles.Where(x => x.ApiId== api.Id).Select(x => x.Role.ToModel()).ToListAsync();
            return api.ToModel(roles);
        }

        public async Task<bool> ExistsByName(string ownerKeyId, string serviceId, string httpMethod, string name)
        {
            var ownerKey = int.Parse(ownerKeyId);
            var serviceId2 = int.Parse(serviceId);
            var apiName = string.IsNullOrEmpty(name) ? string.Empty : name.ToLower();

            var api = await _context.Apis.SingleOrDefaultAsync(x =>
                x.OwnerKeyId == ownerKey && x.ServiceId == serviceId2 && x.HttpMethod == httpMethod &&
                x.Name == apiName);

            return api != null;
        }

        public async Task<bool> ExistsByUrl(string ownerKeyId, string serviceId, string httpMethod, string url)
        {
            
            var ownerKey = int.Parse(ownerKeyId);
            var serviceId2 = int.Parse(serviceId);
            var apiUrl = string.IsNullOrEmpty(url) ? string.Empty : url.ToLower();

            var api = await _context.Apis.SingleOrDefaultAsync(x =>
                x.OwnerKeyId == ownerKey && x.ServiceId == serviceId2 && x.HttpMethod == httpMethod &&
                x.Url == apiUrl);

            return api != null;
        }

        public Task<int> Count(string ownerKeyId, string serviceId)
        {
            int ownerKeyId2 = int.Parse(ownerKeyId);
            int serviceId2 = int.Parse(serviceId);

            return _context.Apis.CountAsync(x => x.OwnerKeyId == ownerKeyId2 && x.ServiceId == serviceId2);
        }
    }
}