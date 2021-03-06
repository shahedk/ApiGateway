﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Role : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsDisabled { get; set; }
        
        public List<KeyInRole> KeyInRoles { get; set; }
        public List<ApiInRole> ApiInRoles { get; set; }

        public List<ServiceInRole> ServiceInRoles { get; set; }
        public List<AccessRuleForRole> AccessRuleForRoles { get; set; }
    }
}