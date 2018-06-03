﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Common.Models
{
    public class RoleModel : ModelBase
    {
        [Required]
        public string ServiceId { get; set; }
        
        [Required]
        public string Name { get; set; }

        public List<Tag> Tags { get; set; }

        public List<ApiModel> ApiInRole { get; set; }
        public List<AccessRuleModel> AccessRulesForRole { get; set; }
    }
}