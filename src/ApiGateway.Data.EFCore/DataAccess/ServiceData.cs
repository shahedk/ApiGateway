using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Entity;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;


namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ServiceData : IServiceData
    {
        private readonly ApiGatewayContext _context;

        public ServiceData(ApiGatewayContext context)
        {
            _context = context;
        }
         
        public async Task<ServiceModel> Create(ServiceModel model)
        {
            var service = model.ToEntity();
            
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return service.ToModel();
        }

        public async Task<ServiceModel> Update(ServiceModel model)
        {
            var ownerKeyId = int.Parse(model.OwnerKeyId);
            var id = int.Parse(model.Id);
            var existing =
                await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == id);

            // Update existing
            existing.Name = model.Name;
            await _context.SaveChangesAsync();

            return existing.ToModel();
        }

        public async Task Delete(string ownerKeyId, string id)
        {
            var entity = await GetEntity(ownerKeyId, id);
             _context.Services.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<Service> GetEntity(string ownerKeyId, string id)
        {
            var ownerKeyId2 = int.Parse(ownerKeyId);

            var result = await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId2 && x.Id == int.Parse(id));
            return result;
        }

        public async Task<ServiceModel> Get(string ownerKeyId, string id)
        {
            var result = await GetEntity(ownerKeyId, id);

            return result.ToModel();
        }
    }
}