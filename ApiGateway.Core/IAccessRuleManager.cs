using ApiGateway.Common.Models;

namespace ApiGateway.Core
{
    public interface IAccessRuleManager
    {
        AccessRuleModel CreateAccessRule(KeyModel clieKey, AccessRuleModel model);
        AccessRuleModel UpdateAccessRule(KeyModel clieKey, AccessRuleModel model);
        void DeleteAccessRule(KeyModel clieKey, string id);
    }
}