using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class ApiInRole : ManyToManyBase
    {
        [Required]
        [StringLength(20)]
        public int ApiId { get; set; }
        
        [Required]
        public int RoleId { get; set; }

        public Api Api { get; set; }

        public Role Role { get; set; }
    }
}