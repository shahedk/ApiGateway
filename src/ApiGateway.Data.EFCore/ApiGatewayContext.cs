using System;
using ApiGateway.Data.EFCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiGateway.Data.EFCore
{
    public sealed class ApiGatewayContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<Api> Apis { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ApiInRole> ApiInRoles { get; set; }
        public DbSet<KeyInRole> KeyInRoles { get; set; }
        public DbSet<ServiceInRole> ServiceInRoles { get; set; }
        public DbSet<AccessRule> AccessRules { get; set; }
        public DbSet<AccessRuleForRole> AccessRuleForRoles { get; set; }
        
        public ApiGatewayContext(DbContextOptions<ApiGatewayContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Adding "AG_" prefix to all table names
            foreach (IMutableEntityType type in modelBuilder.Model.GetEntityTypes())
            {
                type.Relational().TableName = "AG_" + type.Relational().TableName;
            }
            
            // Key
            modelBuilder.Entity<Key>().HasIndex(x => x.PublicKey).IsUnique();
            modelBuilder.Entity<Key>().HasIndex(x => x.OwnerKeyId).IsUnique(false);
             
            // Service
            modelBuilder.Entity<Service>().HasIndex(x => new {x.OwnerKeyId, x.Name});
            modelBuilder.Entity<Service>().HasOne(x => x.OwnerKey)
                                          .WithMany(x => x.Services)
                                          .HasForeignKey(x => x.OwnerKeyId)
                                          .HasConstraintName("FK_OwnerKey_Service")
                                          .OnDelete(DeleteBehavior.Restrict);
            
            // Api
            modelBuilder.Entity<Api>().HasIndex(x => new {x.ServiceId, x.Url, x.HttpMethod}).IsUnique();
            modelBuilder.Entity<Api>().HasIndex(x => x.ServiceId).IsUnique(false);
            modelBuilder.Entity<Api>().HasOne(x => x.Service).WithMany(x => x.Apis)
                .HasForeignKey(x => x.ServiceId).HasConstraintName("FK_Service_Api").OnDelete(DeleteBehavior.Restrict);
            
            // Role
            modelBuilder.Entity<Role>().HasOne(x => x.OwnerKey)
                                       .WithMany(x => x.Roles)
                                       .HasConstraintName("FK_Role_OwnerKey")
                                       .OnDelete(DeleteBehavior.Restrict);

            // ApiInRole (Many to Many)
            modelBuilder.Entity<ApiInRole>().HasIndex(x => new{x.ApiId, x.RoleId});
            modelBuilder.Entity<ApiInRole>().HasOne(x => x.Api)
                                            .WithMany(x => x.ApiInRoles)
                                            .HasForeignKey(x => x.ApiId)
                                            .HasConstraintName("FK_Api_ApiInRole")
                                            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApiInRole>()
                        .HasOne(x => x.Role)
                        .WithMany(x => x.ApiInRoles)
                        .HasForeignKey(x => x.RoleId)
                        .HasConstraintName("FK_Role_ApiInRole")
                        .OnDelete(DeleteBehavior.Restrict);

            // ServiceInRole (Many to Many)
            modelBuilder.Entity<ServiceInRole>()
                        .HasIndex(x => new { x.ServiceId, x.RoleId });

            modelBuilder.Entity<ServiceInRole>()
                        .HasOne(x => x.Service)
                        .WithMany(x => x.ServiceInRoles)
                        .HasForeignKey(x => x.ServiceId)
                        .HasConstraintName("FK_Service_ServiceInRole")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceInRole>()
                        .HasOne(x => x.Role)
                        .WithMany(x => x.ServiceInRoles)
                        .HasForeignKey(x => x.RoleId)
                        .HasConstraintName("FK_Role_ServiceInRole")
                        .OnDelete(DeleteBehavior.Restrict);

            // KeyInRole (Many to Many)
            modelBuilder.Entity<KeyInRole>()
                        .HasIndex(x => x.KeyId)
                        .IsUnique(false);

            modelBuilder.Entity<KeyInRole>()
                        .HasIndex(x => new {x.KeyId, x.RoleId})
                        .IsUnique();

            modelBuilder.Entity<KeyInRole>()
                        .HasOne(x => x.Key)
                        .WithMany(x => x.KeyInRoles)
                        .HasForeignKey(x => x.KeyId)
                        .HasConstraintName("FK_Key_KeyInRole")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<KeyInRole>()
                        .HasOne(x => x.Role)
                        .WithMany(x => x.KeyInRoles)
                        .HasForeignKey(x => x.RoleId)
                        .HasConstraintName("FK_Role_KeyInRole")
                        .OnDelete(DeleteBehavior.Restrict);;
            
            // AccessRule
            modelBuilder.Entity<AccessRule>()
                        .HasIndex(x => x.OwnerKeyId)
                        .IsUnique(false);

            modelBuilder.Entity<AccessRule>()
                        .HasOne(x => x.OwnerKey)
                        .WithMany(x => x.AccessRules)
                        .HasForeignKey(x => x.OwnerKeyId)
                        .HasConstraintName("FK_OwnerKey_AccessRule")
                        .OnDelete(DeleteBehavior.Restrict);

            //AccessRuleForRoles
            modelBuilder.Entity<AccessRuleForRole>()
                        .HasOne(x => x.Role)
                        .WithMany(x => x.AccessRuleForRoles)
                        .HasForeignKey(x => x.RoleId)
                        .HasConstraintName("FK_Role_AccessRuleForRole")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccessRuleForRole>()
                .HasOne(x => x.AccessRule)
                .WithMany(x => x.AccessRulesForRoles)
                .HasForeignKey(x => x.AccessRuleId)
                .HasConstraintName("FK_AccessRule_AccessRuleForRole")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}