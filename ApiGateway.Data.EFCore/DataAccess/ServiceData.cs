using System.IO;
using System.Threading.Tasks;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;


namespace ApiGateway.Data.EFCore.DataAccess
{
    public class ServiceData : IServiceData
    {
        private readonly ApiGatewayContext _context;
        private readonly IStringLocalizer<ServiceData> _localizer;

        public ServiceData(ApiGatewayContext context, IStringLocalizer<ServiceData> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
         
        public async Task<ServiceModel> Create(string ownerKeyId, ServiceModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = ModelHelper.GenerateNewId();
            }
            else
            {
                var existing =
                    await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == model.Id);

                if (existing != null)
                {
                    var errorMessage = _localizer["Another service with same ID already exists."];
                    throw new InvalidDataException(errorMessage);
                }
            }
             
            var service = model.ToEntity(ownerKeyId);
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return service.ToModel();
        }

        public async Task<ServiceModel> Update(string ownerKeyId, ServiceModel model)
        {
            var errorMessage = _localizer["No service found for the speified Id."];
            if (string.IsNullOrEmpty(model.Id))
            {
                throw new InvalidDataException(errorMessage);
            }
            else
            {
                var existing =
                    await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == model.Id);

                if (existing == null)
                {
                    throw new InvalidDataException(errorMessage);
                }
                else
                {
                    // Update existing

                    existing.Name = model.Name;
                    existing.Tags = model.Tags.ToJson();

                    await _context.SaveChangesAsync();

                    return existing.ToModel();
                }
            }

        }
        

        public Task Delete(string ownerKeyId, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}