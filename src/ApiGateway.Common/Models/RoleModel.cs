using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Common.Models
{
    public class RoleModel : ModelBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsDisabled { get; set; } = false;

        public List<ServiceModel> ServiceInRole { get; set; }
        public List<ApiModel> ApiInRole { get; set; }
        public List<AccessRuleModel> AccessRulesForRole { get; set; }


    }
}