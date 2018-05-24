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
            modelBuilder.Entity<Key>().HasIndex(x => x.Id).IsUnique(true);
            modelBuilder.Entity<Key>().HasIndex(x => x.PublicKey).IsUnique(true).ForSqlServerIsClustered();
            modelBuilder.Entity<Key>().HasIndex(x => x.OwnerKeyId).IsUnique(false);
            
            // Service
            modelBuilder.Entity<Service>().HasIndex(x => x.Id).IsUnique(true).ForSqlServerIsClustered();
            modelBuilder.Entity<Service>().HasIndex(x => new {x.OwnerKeyId, x.Name}).IsUnique();
            
            // Api
            modelBuilder.Entity<Api>().HasIndex(x => x.Id).IsUnique(true);
            modelBuilder.Entity<Api>().HasIndex(x => new {x.ServiceId, x.Url, x.HttpMethod}).IsUnique(true);
            modelBuilder.Entity<Api>().HasIndex(x => x.ServiceId).IsUnique(false);
            
            // Role
            modelBuilder.Entity<Role>().HasIndex(x => x.Id).IsUnique(true);
            modelBuilder.Entity<Role>().HasIndex(x => new {x.ServiceId, x.Name}).IsUnique(true);
            
            // ApiInRole
            modelBuilder.Entity<ApiInRole>().HasIndex(x => x.Id).IsUnique(true);
            modelBuilder.Entity<ApiInRole>().HasIndex(x => new{x.ApiId, x.RoleId}).IsUnique(true);
            
            // KeyInRole
            modelBuilder.Entity<KeyInRole>().HasIndex(x => x.Id).IsUnique(true);
            modelBuilder.Entity<KeyInRole>().HasIndex(x => new{x.KeyId, x.RoleId}).IsUnique(true);
            
            // AccessRule
            modelBuilder.Entity<AccessRule>().HasIndex(x => x.Id).IsUnique(true);
            modelBuilder.Entity<AccessRule>().HasIndex(x => x.OwnerKeyId).IsUnique(false);
            modelBuilder.Entity<AccessRule>().HasIndex(x => new{x.ServiceId, x.Name}).IsUnique(true);
 
            //AccessRuleForRoles
            modelBuilder.Entity<AccessRuleForRole>().HasIndex(x => x.Id).IsUnique(true);
            modelBuilder.Entity<AccessRuleForRole>().HasIndex(x => x.OwnerKeyId).IsUnique(false);
            modelBuilder.Entity<AccessRuleForRole>().HasIndex(x => new { x.RoleId, x.AcccessRuleId }).IsUnique(true);
        }
    }
}