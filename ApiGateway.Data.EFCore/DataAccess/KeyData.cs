using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class KeyData: IKeyData
    {
        private readonly ApiGatewayContext _context;
        private readonly IStringLocalizer<KeyData> _localizer;
        private readonly ILogger<KeyData> _logger;

        public KeyData(ApiGatewayContext context, IStringLocalizer<KeyData> localizer, ILogger<KeyData> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<KeyModel> Create(string ownerPublicKey, KeyModel model)
        {
            var entity = model.ToEntity();

            _context.Keys.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public Task<KeyModel> Update(string ownerPublicKey, KeyModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }


        public Task<KeyModel> Get(string ownerPublicKey, string keyId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<KeyModel> GetByPublicKey(string publicKey)
        {
            var key = await _context.Keys.SingleOrDefaultAsync(x => x.PublicKey == publicKey);

            if (key == null)
            {
                var message = _localizer["Invalid Key"];

                // Log
                _logger.LogWarning(LogEvents.Warning.InvalidPublicKey, message + ": " + publicKey);

                // Throw exception
                throw new InvalidKeyException(message);
            }
            else
            {
                return key.ToModel();
            }
        } 
    }
}