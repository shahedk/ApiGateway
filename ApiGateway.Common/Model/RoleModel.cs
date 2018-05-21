using System.Collections.Generic;

namespace ApiGateway.Common.Model
{
    public class RoleModel : ModelBase
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public string Name { get; set; }
        public List<Tag> Tags { get; set; }

        public List<ApiModel> ApiInRole { get; set; }
        public List<AccessRuleModel> AccessRulesForRole { get; set; }
    }
}