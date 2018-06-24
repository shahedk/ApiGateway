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
        private readonly IStringLocalizer<ServiceData> _localizer;
        private readonly IKeyData _keyData;
        private readonly ILogger<ServiceData> _logger;

        public ServiceData(ApiGatewayContext context, IStringLocalizer<ServiceData> localizer, ILogger<ServiceData> logger,IKeyData keyData)
        {
            _context = context;
            _localizer = localizer;
            _keyData = keyData;
            _logger = logger;
        }
         
        public async Task<ServiceModel> Create(string ownerPublicKey, ServiceModel model)
        {
            var ownerKey = await _keyData.GetByPublicKey(ownerPublicKey);
            var service = model.ToEntity();
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return service.ToModel();
        }

        public async Task<ServiceModel> Update(string ownerPublicKey, ServiceModel model)
        {
            var errorMessage = _localizer["No service found for the speified Id."];
            if (string.IsNullOrEmpty(model.Id))
            {
                throw new InvalidDataException(errorMessage);
            }
            else
            {
                var key = await _keyData.GetByPublicKey(ownerPublicKey);
                var ownerKeyId = int.Parse(key.Id);

                var id = int.Parse(model.Id);
                var existing =
                    await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == id);

                if (existing == null)
                {
                    throw new InvalidDataException(errorMessage);
                }
                else
                {
                    // Update existing

                    existing.Name = model.Name;
                    
                    await _context.SaveChangesAsync();

                    return existing.ToModel();
                }
            }

        }
        
        public async Task Delete(string ownerPublicKey, string id)
        {
            var entity = await GetEntity(ownerPublicKey, id);
             _context.Services.Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<Service> GetEntity(string ownerPublicKey, string id)
        {
            var key = await _keyData.GetByPublicKey(ownerPublicKey);
            var ownerKeyId = int.Parse(key.Id);

            var result = await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerKeyId && x.Id == int.Parse(id));

            if (result == null)
            {
                var msg = _localizer["Service not found for the specified owner and id"];
                throw new ItemNotFoundException(msg, HttpStatusCode.NotFound);
            }

            return result;
        }

        public async Task<ServiceModel> Get(string ownerPublicKey, string id)
        {
            var result = await GetEntity(ownerPublicKey, id);

            return result.ToModel();
        }
    }
}