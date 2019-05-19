using System;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
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

                throw new ApiGatewayException(msg, HttpStatusCode.BadRequest);
            }

            // Generate initial root key
            var rootKey = await _keyManager.CreateRootKey();

            // Create ApiGateway service definitions
            var sysService = await _serviceManager.Create(rootKey.PublicKey, new ServiceModel
            {
                Name = AppConstants.SysApiServiceName
            });
            
            var echoService = await _serviceManager.Create(rootKey.PublicKey, new ServiceModel
            {
                Name = "Echo"
            });

            // Create Role definitions
            var role = await _roleManager.Create(rootKey.PublicKey, new RoleModel
            {
                Name = "ApiGateway.Admin"
            });
            await _roleManager.AddKeyInRole(rootKey.PublicKey, role.Id, rootKey.PublicKey);

            #region Create API access for: Key

            var keyApiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "key/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiGet.Id);
            
            var keyApiGetDetail = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key-Detail",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "key-detail/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiGetDetail.Id);

            var keyApiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.SysApiUrlPrefix + "key/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiPut.Id);

            var keyApiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "key/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiPost.Id);

            
            var keyApiReGenerateSecret1 = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "ReGenerate-Secret1",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "key/regenerate-secret1",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiReGenerateSecret1.Id);

            
            var keyApiReGenerateSecret2 = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "ReGenerate-Secret2",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "key/regenerate-secret2",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiReGenerateSecret2.Id);

            
            var keyApiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Key",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.SysApiUrlPrefix + "key/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, keyApiDel.Id);

            var echoGetApi = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "",
                HttpMethod = ApiHttpMethods.Get,
                Url = "http://echo.shahed.ca",
                ServiceId = echoService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, echoGetApi.Id);
            
            
            var echoPostApi = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "",
                HttpMethod = ApiHttpMethods.Post,
                Url = "http://echo.shahed.ca",
                ServiceId = echoService.Id,
                
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, echoPostApi.Id);
            
            var echoPutApi = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "",
                HttpMethod = ApiHttpMethods.Put,
                Url = "http://echo.shahed.ca",
                ServiceId = echoService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, echoPutApi.Id);
            
            
            var echoDelApi = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "",
                HttpMethod = ApiHttpMethods.Delete,
                Url = "http://echo.shahed.ca",
                ServiceId = echoService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, echoDelApi.Id);
            
            #endregion

            #region Create API access for: Service

            var serviceApiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "service/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiGet.Id);

            var serviceApiGetDetail = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service-Detail",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "service-detail/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiGetDetail.Id);

            
            var serviceApiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.SysApiUrlPrefix + "service/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiPut.Id);

            var serviceApiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "service/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiPost.Id);

            var serviceApiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Service",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.SysApiUrlPrefix + "service/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, serviceApiDel.Id);

            #endregion

            #region Create API assess for: Role

            var roleApiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "role/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiGet.Id);

            // URL: /sys/role-detail/{id}
            var roleApiGetDetail = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role-Detail",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "role-detail/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiGetDetail.Id);

            // URL: /sys/role-summary
            var roleApiGetSummary = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role-Summary",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "role-summary/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiGetSummary.Id);


            var roleApiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "role/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost.Id);

            var roleApiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.SysApiUrlPrefix + "role/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPut.Id);

            var roleApiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Role",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.SysApiUrlPrefix + "role/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiDel.Id);

            var roleApiPost_AddKeyInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "AddKeyInRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "AddKeyInRole/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_AddKeyInRole.Id);

            var roleApiPost_RemoveKeyInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "RemoveApiFromRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "RemoveApiFromRole",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_RemoveKeyInRole.Id);

            var roleApiPost_AddApiInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "AddApiInRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "AddApiInRole/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_AddApiInRole.Id);

            var roleApiPost_RemoveApiInRole = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "RemoveApiInRole",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "RemoveApiInRole/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, roleApiPost_RemoveApiInRole.Id);

            #endregion

            #region Create API access for: Api

            var apiGet = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "api/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiGet.Id);

            var apiGetDetail = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api-Detail",
                HttpMethod = ApiHttpMethods.Get,
                Url = AppConstants.SysApiUrlPrefix + "api-detail/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiGetDetail.Id);

           
            var apiPut = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api",
                HttpMethod = ApiHttpMethods.Put,
                Url = AppConstants.SysApiUrlPrefix + "api/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiPut.Id);

            var apiPost = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api",
                HttpMethod = ApiHttpMethods.Post,
                Url = AppConstants.SysApiUrlPrefix + "api/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiPost.Id);

            var apiDel = await _apiManager.Create(rootKey.PublicKey, new ApiModel
            {
                Name = "Api",
                HttpMethod = ApiHttpMethods.Delete,
                Url = AppConstants.SysApiUrlPrefix + "api/",
                ServiceId = sysService.Id
            });
            await _roleManager.AddApiInRole(rootKey.PublicKey, role.Id, apiDel.Id);
            #endregion
            
            state.IsConfigured = true;
            var successMessage = _stringLocalizer["System initialized. Please save the key in secured place. ApiKey: {0} | ApiSecret1: {1} |  ApiSecret2: {2}"];

            state.Message = string.Format(successMessage, rootKey.PublicKey, rootKey.GetSecret1(), rootKey.GetSecret2());
            return state;

        }
    }
}