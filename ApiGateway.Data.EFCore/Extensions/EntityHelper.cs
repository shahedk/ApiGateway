using System;
using System.Collections.Generic;
using System.Linq;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using ApiGateway.Data.EFCore.Entity;

namespace ApiGateway.Data.EFCore.Extensions
{
    public static class EntityHelper
    {
        private static DateTime ToDbTime(this DateTime dateTime)
        {
            return dateTime;
        }
        
        /*
         * Key - KeyModel
         */
        public static Key ToEntity(this KeyModel model)
        {
            return new Key
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                OwnerKeyId = model.OwnerKeyId,
                Properties = model.Properties.ToJson(),
                Tags = model.Tags.ToJson(),
                PublicKey = model.PublicKey,
                Type = model.Type,
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static KeyModel ToModel(this Key entity)
        {
            return new  KeyModel
            {
                Id = entity.Id.ToString(),
                Properties = entity.Properties.ToProperties(),
                PublicKey =  entity.PublicKey,
                Tags = entity.Tags.ToTags(),
                Type = entity.Type,
                OwnerKeyId = entity.OwnerKeyId,
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        public static KeyModel ToModel(this Key entity, List<Role> roles)
        {
            return new  KeyModel
            {
                Id = entity.Id.ToString(),
                Properties = entity.Properties.ToProperties(),
                PublicKey =  entity.PublicKey,
                Roles = roles.Select(x=>x.ToModel(null,null)).ToList(),
                Tags = entity.Tags.ToTags(),
                Type = entity.Type,
                OwnerKeyId = entity.OwnerKeyId,
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        /*
         * Api - ApiModel
         */
        public static Api ToEntity(this ApiModel model)
        {
            return new Api
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                HttpMethod = model.HttpMethod,
                Name = model.Name,
                ServiceId = model.ServiceId,
                Tags = model.Tags.ToJson(),
                Url = model.Url,
                OwnerKeyId = model.OwnerKeyId,
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static ApiModel ToModel(this Api entity, List<RoleModel> roles)
        {
            return new ApiModel
            {
                Id = entity.Id.ToString(),
                ApiInRole = roles,
                HttpMethod = entity.HttpMethod,
                Name = entity.Name,
                ServiceId = entity.ServiceId,
                Tags = entity.Tags.ToTags(),
                OwnerKeyId = entity.OwnerKeyId,
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        /*
         * Role - RoleModel
         */
        public static Role ToEntity(this RoleModel model)
        {
            return new Role
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                Name = model.Name,
                ServiceId = model.ServiceId,
                Tags = model.Tags.ToJson(),
                
                OwnerKeyId = model.OwnerKeyId,
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static RoleModel ToModel(this Role entity, List<AccessRuleModel> accessRules, List<ApiModel> apiInRole)
        {
            return new  RoleModel
            {
                Id = entity.Id.ToString(),
                AccessRulesForRole = accessRules,
                Tags = entity.Tags.ToTags(),
                ApiInRole = apiInRole,
                Name = entity.Name,
                ServiceId = entity.ServiceId,
                OwnerKeyId = entity.OwnerKeyId,
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }
        
        /*
         * Service - ServiceModel
         */
        public static Service ToEntity(this ServiceModel model, string ownerKeyId)
        {
            return new Service
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                Name = model.Name,
                OwnerKeyId = ownerKeyId,
                Tags = model.Tags.ToJson(),
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static ServiceModel ToModel(this Service entity)
        {
            return new  ServiceModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                Tags = entity.Tags.ToTags(),
                OwnerKeyId = entity.OwnerKeyId,
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        
        /*
         * AccessRule - AccessRuleModel
         */
        public static AccessRule ToEntity(this AccessRuleModel model)
        {
            return new AccessRule
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                Description = model.Description,
                Properties = model.Properties.ToJson(),
                Name = model.Name,
                ServiceId = model.ServiceId,
                Tags = model.Tags.ToJson(),
                Type = model.Type,
                OwnerKeyId = model.OwnerKeyId,
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static AccessRuleModel ToModel(this AccessRule entity)
        {
            return new  AccessRuleModel
            {
                Id = entity.Id.ToString(),
                Description = entity.Description,
                Name = entity.Name,
                Properties = entity.Properties.ToProperties(),
                ServiceId = entity.ServiceId,
                Tags = entity.Tags.ToTags(),
                Type = entity.Type,
                OwnerKeyId = entity.OwnerKeyId,
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }
    }
}