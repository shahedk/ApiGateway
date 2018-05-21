using ApiGateway.Data.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore
{
    public class ApiGatewayContext : DbContext
    {
        public DbSet<AccessRule> AccessRules { get; set; }
        public DbSet<AccessRuleForRole> AccessRuleForRoles { get; set; }
        public DbSet<Api> Apis { get; set; }
        public DbSet<ApiInRole> ApiInRoles { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<KeyInRole> KeyInRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }


        public ApiGatewayContext(DbContextOptions<ApiGatewayContext> options) : base(options)
        {
        }

        public ApiGatewayContext()
        {
        }
    }
}