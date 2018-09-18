using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class AccessRuleForRole : EntityBase
    {
        [Required]
        [StringLength(20)]
        public string ServiceId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string AcccessRuleId { get; set; }
        
        [Required]
        public int RoleId { get; set; }
        
        public Role Role { get; set; }
    }
}