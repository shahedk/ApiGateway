using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IServiceManager
    {
        ServiceModel CreateService(KeyModel clientKey, ServiceModel model);
        ServiceModel UpdateService(KeyModel clientKey, ServiceModel model);
        void DeleteService(KeyModel clientKey, string id);

        ApiModel CreateApi(KeyModel clieKey, ApiModel model);
        ApiModel UpdateApi(KeyModel clieKey, ApiModel model);
        void DeleteApi(KeyModel clieKey, string id);

        RoleModel CreateRole(KeyModel clieKey, RoleModel model);
        RoleModel UpdateRole(KeyModel clieKey, RoleModel model);
        void DeleteRole(KeyModel clieKey, string id);

        
    }
}