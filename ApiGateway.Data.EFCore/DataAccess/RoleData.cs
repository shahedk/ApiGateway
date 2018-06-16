using System.Threading.Tasks;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class RoleData: IRoleData
    {
        private readonly ApiGatewayContext _context;
        private readonly IStringLocalizer<RoleData> _localizer;
        private readonly ILogger<RoleData> _logger;

        public RoleData(ApiGatewayContext context, IStringLocalizer<RoleData> localizer, ILogger<RoleData> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
        }


        public async Task<RoleModel> Create(string ownerPublicKey, RoleModel model)
        {
            var entity = model.ToEntity();

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToModel();
        }

        public Task<RoleModel> Update(string ownerPublicKey, RoleModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RoleModel> Get(string ownerPublicKey, string id)
        {
            throw new System.NotImplementedException();
        }
    }
}