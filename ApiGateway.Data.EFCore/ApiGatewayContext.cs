using System;
using ApiGateway.Data.EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace ApiGateway.Data.EFCore
{
    public class ApiGatewayContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<Api> Apis { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ApiInRole> ApiInRoles { get; set; }
        public DbSet<KeyInRole> KeyInRoles { get; set; }
        
        public DbSet<AccessRule> AccessRules { get; set; }
        public DbSet<AccessRuleForRole> AccessRuleForRoles { get; set; }
        
        public ApiGatewayContext(DbContextOptions<ApiGatewayContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Key
            modelBuilder.Entity<Key>().HasIndex(x => x.PublicKey).IsUnique();
            modelBuilder.Entity<Key>().HasIndex(x => x.OwnerKeyId).IsUnique(false);
            
            // Key in Role
            modelBuilder.Entity<KeyInRole>().HasIndex(x => x.Id).IsUnique();
            modelBuilder.Entity<KeyInRole>().HasIndex(x => x.KeyId).IsUnique(false);
            modelBuilder.Entity<KeyInRole>().HasIndex(x => new {x.KeyId, x.RoleId}).IsUnique();

            // Service
            modelBuilder.Entity<Service>().HasIndex(x => new {x.OwnerKeyId, x.Name});
            
            // Api
            modelBuilder.Entity<Api>().HasIndex(x => new {x.ServiceId, x.Url, x.HttpMethod}).IsUnique();
            modelBuilder.Entity<Api>().HasIndex(x => x.ServiceId).IsUnique(false);
            
            // Role
            modelBuilder.Entity<Role>().HasIndex(x => new {x.ServiceId, x.Name}).IsUnique();
            
            // ApiInRole
            modelBuilder.Entity<ApiInRole>().HasIndex(x => new{x.ApiId, x.RoleId}).IsUnique();
            
            // KeyInRole
            modelBuilder.Entity<KeyInRole>().HasIndex(x => new{x.KeyId, x.RoleId}).IsUnique();
            
            // AccessRule
            modelBuilder.Entity<AccessRule>().HasIndex(x => x.OwnerKeyId).IsUnique(false);
            modelBuilder.Entity<AccessRule>().HasIndex(x => new{x.ServiceId, x.Name}).IsUnique();
 
            //AccessRuleForRoles
            modelBuilder.Entity<AccessRuleForRole>().HasIndex(x => x.OwnerKeyId).IsUnique(false);
            modelBuilder.Entity<AccessRuleForRole>().HasIndex(x => new { x.RoleId, x.AcccessRuleId }).IsUnique();
        }
    }
}