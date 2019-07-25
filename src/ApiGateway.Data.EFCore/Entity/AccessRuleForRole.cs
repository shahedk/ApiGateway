using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class AccessRuleForRole : ManyToManyBase
    {
        [Required]
        public int AccessRuleId { get; set; }
        
        [Required]
        public int RoleId { get; set; }
        
        public Role Role { get; set; }
        public AccessRule AccessRule { get; set; }
    }
}