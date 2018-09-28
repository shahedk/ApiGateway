using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Extensions;
using ApiGateway.Common.Models;
using Microsoft.Extensions.Localization;

namespace ApiGateway.Core
{
    public class AppEnvironment : IAppEnvironment
    {
        private readonly IKeyManager _keyManager;
        private readonly IServiceManager _serviceManager;
        private readonly IRoleManager _roleManager;
        private readonly IApiManager _apiManager;
        private readonly IStringLocalizer<IAppEnvironment> _stringLocalizer;

        public AppEnvironment(IKeyManager keyManager, IServiceManager serviceManager, IRoleManager roleManager, IApiManager apiManager, IStringLocalizer<IAppEnvironment> stringLocalizer)
        {
            _keyManager = keyManager;
            _serviceManager = serviceManager;
            _roleManager = roleManager;
            _apiManager = apiManager;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<AppState> GetApplicationState()
        {
            var state = new AppState();
            
            var serviceCount = await GetServiceCount();

            if (serviceCount > 0)
            {
                var msgTemplate = _stringLocalizer["Total {0} services registered."];
                state.Message = string.Format(msgTemplate, serviceCount);
                state.IsConfigured = true;
            }
            else
            {
                state.Message = _stringLocalizer["No service found. Please configure application environment."];
                state.IsConfigured = false;
            }

            return state;
        }

        public async Task<int> GetServiceCount()
        {
            return await _serviceManager.Count();
        }

        public async Task<AppState> Initialize()
        {
            var state = new AppState();
            
            var serviceCount = await _serviceManager.Count();
            if ( serviceCount > 0)
            {
                var msgTemplate = _stringLocalizer["There are {0} active service(s). System can not be re-initialized."];
                var msg = string.Format(msgTemplate, serviceCount);

                throw new InvalidOperationException(msg);
            }

            // Generate initial root key
            var rootKey = await _keyManager.CreateRootKey();

            // Create ApiGateway service definitions
            var service = await _serviceManager.Create(rootKey.PublicKey, new ServiceModel
            {
                Name = AppConstants.LocalApiServiceName
            });

            // Create Role definitions
            var role = await _roleManager.Create(rootKey.PublicKey, new RoleModel
            {
                Name = "ApiGateway.Admin",
                ServiceId = service.Id
            });
            await _roleManager.AddKeyInRole(rootKey.PublicKey, role.Id, rootKey.PublicKey);

            #region Create API access for: Key

            var keyApiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key.Get",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.LocalApiUrlPrefix + "key/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiGet.Id);

            var keyApiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key.Put",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.LocalApiUrlPrefix + "key/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiPut.Id);

            var keyApiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key.Post",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "key/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiPost.Id);

            
            var keyApiReGenerateSecret = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key.ReGenerateSecret",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "key/regenerate-secret",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiReGenerateSecret.Id);

            var keyApiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key.Delete",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.LocalApiUrlPrefix + "key/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiDel.Id);

            #endregion

            #region Create API access for: Service

            var serviceApiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service.Get",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.LocalApiUrlPrefix + "service/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiGet.Id);

            var serviceApiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service.Put",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.LocalApiUrlPrefix + "service/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiPut.Id);

            var serviceApiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service.Post",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "service/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiPost.Id);

            var serviceApiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service.Delete",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.LocalApiUrlPrefix + "service/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiDel.Id);

            #endregion

            #region Create API assess for: Role

            var roleApiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Get",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.LocalApiUrlPrefix + "role/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiGet.Id);

            var roleApiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Post",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "role/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost.Id);

            var roleApiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Put",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.LocalApiUrlPrefix + "role/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPut.Id);

            var roleApiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Delete",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.LocalApiUrlPrefix + "role/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiDel.Id);

            var roleApiPost_AddKeyInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Post.AddKeyInRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "role/AddKeyInRole/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_AddKeyInRole.Id);

            var roleApiPost_RemoveKeyInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Post.RemoveKeyInRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "role/RemoveKeyInRole/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_RemoveKeyInRole.Id);

            var roleApiPost_AddApiInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Post.AddApiInRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "role/AddApiInRole/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_AddApiInRole.Id);

            var roleApiPost_RemoveApiInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role.Post.RemoveApiInRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "role/RemoveApiInRole/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_RemoveApiInRole.Id);

            #endregion

            #region Create API access for: Api

            var apiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api.Get",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.LocalApiUrlPrefix + "api/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiGet.Id);

            var apiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api.Put",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.LocalApiUrlPrefix + "api/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiPut.Id);

            var apiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api.Post",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.LocalApiUrlPrefix + "api/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiPost.Id);

            var apiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api.Delete",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.LocalApiUrlPrefix + "api/",
                ServiceId = service.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiDel.Id);

            #endregion
            
            state.IsConfigured = true;
            var successMessage = _stringLocalizer["System initialized. Please save the root key in secured place. Root key >> ApiKey: [{0}] | ApiSecret1: [{1}] |  ApiSecret2: [{2}] "];

            state.Message = string.Format(successMessage, rootKey.PublicKey, rootKey.GetSecret1(), rootKey.GetSecret2());
            return state;

        }
    }
}