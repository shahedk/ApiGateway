using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Common.Model
{
    public class AccessRuleModel : ModelBase
    {
        [Required]
        public string ServiceId { get; set; }

        [Required]
        public string Name{ get; set; }

        public string Description { get; set; }
        
        public List<Tag> Tags { get; set; }
        
        [Required]
        public string Type { get; set; }
        
        public List<KeyProperty> Properties { get; set; }
    }
}