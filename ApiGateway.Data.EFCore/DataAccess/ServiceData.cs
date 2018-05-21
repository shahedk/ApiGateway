using System.Threading.Tasks;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Model;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ServiceData : IServiceData
    {
        private readonly ApiGatewayContext _context;

        public ServiceData(ApiGatewayContext context)
        {
            _context = context;
        }

        public async Task<ServiceModel> SaveService(string ownerKeyId, ServiceModel model)
        {
            
            if (!string.IsNullOrEmpty(model.Id))
            {
                var existing =
                    await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == model.Id);

                if (existing != null)
                {
                    // Update existing

                    existing.Name = model.Name;
                    existing.Tags = model.Tags.ToJson();

                    await _context.SaveChangesAsync();

                    return existing.ToModel();
                }
            }
            
            // Else, create new
            var service = model.ToEntity(ownerKeyId);
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return service.ToModel();
        }

        public Task DeleteService(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}