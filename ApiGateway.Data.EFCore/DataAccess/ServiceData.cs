using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
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
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = ModelHelper.GenerateNewId();
            }
            else
            {
                var key = await _keyData.GetByPublicKey(ownerPublicKey);

                var id = int.Parse(model.Id);
                var existing =
                    await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == key.Id && x.Id == id);

                if (existing != null)
                {
                    var errorMessage = _localizer["Another service with same ID already exists."];
                    throw new InvalidDataException(errorMessage);
                }
            }
             
            var service = model.ToEntity(ownerPublicKey);
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
                var id = int.Parse(model.Id);
                var existing =
                    await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == ownerPublicKey && x.Id == id);

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
        
        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ServiceModel> Get(string ownerPublicKey, string id)
        {
            var key = await _keyData.GetByPublicKey(ownerPublicKey);
            var result = await _context.Services.SingleOrDefaultAsync(x => x.OwnerKeyId == key.PublicKey && x.Id == int.Parse(id));

            return result.ToModel();
        }
    }
}