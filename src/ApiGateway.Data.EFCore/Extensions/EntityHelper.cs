using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if (model == null) return null;

            return new Key
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                OwnerKeyId = string.IsNullOrEmpty(model.OwnerKeyId) ? 0 : Int32.Parse(model.OwnerKeyId),
                Properties = model.Properties.ToJson(),
                PublicKey = model.PublicKey,
                Type = model.Type,
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static KeyModel ToModel(this Key entity)
        {
            if (entity == null) return null;

            return new  KeyModel
            {
                Id = entity.Id.ToString(),
                Properties = entity.Properties.ToProperties(),
                PublicKey =  entity.PublicKey,
                Type = entity.Type,
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        public static KeyModel ToModel(this Key entity, List<RoleModel> roles)
        {
            if (entity == null) return null;

            return new  KeyModel(roles)
            {
                Id = entity.Id.ToString(),
                Properties = entity.Properties.ToProperties(),
                PublicKey =  entity.PublicKey,
                Type = entity.Type,
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        /*
         * Api - ApiModel
         */
        public static Api ToEntity(this ApiModel model)
        {
            if (model == null) return null;

            return new Api
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                HttpMethod = model.HttpMethod,
                Name = model.Name,
                ServiceId = int.Parse(model.ServiceId),
                
                Url = model.Url,
                OwnerKeyId = int.Parse( model.OwnerKeyId),
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static ApiModel ToModel(this Api entity)
        {
            if (entity == null) return null;

            return new ApiModel
            {
                Id = entity.Id.ToString(),
                HttpMethod = entity.HttpMethod,
                Name = entity.Name,
                ServiceId = entity.ServiceId.ToString(),
                Url = entity.Url,
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }


        public static ApiModel ToModel(this Api entity, List<RoleModel> roles)
        {
            if (entity == null) return null;

            return new ApiModel(roles)
            {
                Id = entity.Id.ToString(),
                HttpMethod = entity.HttpMethod,
                Name = entity.Name,
                ServiceId = entity.ServiceId.ToString(),
                Url = entity.Url,
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        /*
         * Role - RoleModel
         */
        public static Role ToEntity(this RoleModel model)
        {
            if (model == null) return null;

            return new Role
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                Name = model.Name,
                ServiceId = Int32.Parse( model.ServiceId),
                
                OwnerKeyId = int.Parse(model.OwnerKeyId),
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static RoleModel ToModel(this Role entity)
        {
            if (entity == null) return null;

            return new  RoleModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                ServiceId = entity.ServiceId.ToString(),
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        public static RoleModel ToModel(this Role entity, List<AccessRuleModel> accessRules, List<ApiModel> apiInRole)
        {
            if (entity == null) return null;

            return new  RoleModel
            {
                Id = entity.Id.ToString(),
                AccessRulesForRole = accessRules,
                
                ApiInRole = apiInRole,
                Name = entity.Name,
                ServiceId = entity.ServiceId.ToString(),
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }
        
        /*
         * Service - ServiceModel
         */
        public static Service ToEntity(this ServiceModel model)
        {
            if (model == null) return null;

            return new Service
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                Name = model.Name,
                OwnerKeyId = int.Parse(model.OwnerKeyId),
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static ServiceModel ToModel(this Service entity)
        {
            if (entity == null) return null;

            return new  ServiceModel
            {
                Id = entity.Id.ToString(),
                Name = entity.Name,
                
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        
        /*
         * AccessRule - AccessRuleModel
         */
        public static AccessRule ToEntity(this AccessRuleModel model)
        {
            if (model == null) return null;

            return new AccessRule
            {
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                Description = model.Description,
                Properties = model.Properties.ToJson(),
                Name = model.Name,
                ServiceId = model.ServiceId,
                
                Type = model.Type,
                OwnerKeyId = int.Parse(model.OwnerKeyId),
                CreateDate = model.CreateDate.ToDbTime(),
                ModifiedDate = model.ModifiedDate.ToDbTime()
            };
        }

        public static AccessRuleModel ToModel(this AccessRule entity)
        {
            if (entity == null) return null;

            return new  AccessRuleModel
            {
                Id = entity.Id.ToString(),
                Description = entity.Description,
                Name = entity.Name,
                Properties = entity.Properties.ToProperties(),
                ServiceId = entity.ServiceId,
                
                Type = entity.Type,
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                CreateDate = entity.CreateDate.ToClientLocalTime(),
                ModifiedDate = entity.ModifiedDate.ToClientLocalTime()
            };
        }

        public static AccessLog ToEntity(this AccessLogModel model)
        {
            if (model == null) return null;

            return new AccessLog
            {
                ServiceId = string.IsNullOrEmpty(model.ServiceId) ? 0 : int.Parse(model.ServiceId),
                Id = string.IsNullOrEmpty(model.Id) ? 0 : int.Parse(model.Id),
                IsValid = model.IsValid,
                KeyId =  model.KeyId,
                ApiId = string.IsNullOrEmpty(model.ApiId) ? 0 : int.Parse(model.ApiId),
                LogTime = model.LogTime,
                Url = model.Url,
                CreateDate = model.CreateDate,
                ModifiedDate = model.ModifiedDate,
                HttpMethod = model.HttpMethod,
                OwnerKeyId = string.IsNullOrEmpty(model.OwnerKeyId) ? 0 : int.Parse(model.OwnerKeyId),
                PublicKey = model.PublicKey,
                RequestInfo =  model.RequestInfo,
                ValidationResult = model.ValidationResult
            };
        }
        
        public static AccessLogModel ToModel(this AccessLog entity)
        {
            if (entity == null) return null;

            return new AccessLogModel
            {
                ServiceId = entity.ServiceId.ToString(),
                Id = entity.Id.ToString(),
                IsValid = entity.IsValid,
                KeyId =  entity.KeyId,
                ApiId = entity.ApiId.ToString(),
                LogTime = entity.LogTime,
                Url = entity.Url,
                CreateDate = entity.CreateDate,
                ModifiedDate = entity.ModifiedDate,
                HttpMethod = entity.HttpMethod,
                OwnerKeyId = entity.OwnerKeyId.ToString(),
                PublicKey = entity.PublicKey,
                RequestInfo =  entity.RequestInfo,
                ValidationResult = entity.ValidationResult
            };
        }
    }
}