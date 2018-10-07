using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class AccessLogData :IAccessLogData
    {
        private readonly ApiGatewayContext _context;
        public AccessLogData(ApiGatewayContext context)
        {
            _context = context;
        }
        
        public async Task<AccessLogModel> Create(AccessLogModel model)
        {
            var entity = model.ToEntity();

            _context.AccessLogs.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public Task<AccessLogModel> Update(AccessLogModel model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string ownerKeyId, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccessLogModel> Get(string ownerKeyId, string id)
        {
            var keyId = int.Parse(ownerKeyId);
            var logId = int.Parse(id);
            
            var entity = await _context.AccessLogs.FirstOrDefaultAsync(x => x.OwnerKeyId == keyId && x.Id == logId);
            
            return entity.ToModel();
        }

        public Task<IList<AccessLogModel>> GetAll(string ownerKeyId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<AccessLogModel>> Get(string ownerKeyId, string serviceId, DateTime start, DateTime end)
        {
            var keyId = int.Parse(ownerKeyId);
            var sId = int.Parse(serviceId);
            
            var list = await _context.AccessLogs.Where(x => x.OwnerKeyId == keyId 
                                                            && x.ServiceId == sId 
                                                            && x.LogTime >= start 
                                                            && x.LogTime <= end)
                                                .Select(x=>x.ToModel()).ToListAsync();

            return list;
        }

        public async Task<IList<AccessLogModel>> Get(string ownerKeyId, string serviceId, string apiId, DateTime start, DateTime end)
        {
            var keyId = int.Parse(ownerKeyId);
            var sId = int.Parse(serviceId);
            var aId = int.Parse(apiId);
            
            var list = await _context.AccessLogs.Where(x => x.OwnerKeyId == keyId 
                                                            && x.ServiceId == sId
                                                            && x.ApiId == aId
                                                            && x.LogTime >= start 
                                                            && x.LogTime <= end)
                                                .Select(x=>x.ToModel()).ToListAsync();

            return list;
        }
    }
}