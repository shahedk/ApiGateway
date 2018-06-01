using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class ApiInRole : EntityBase
    {
        [Required]
        [StringLength(20)]
        public string ApiId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string RoleId { get; set; }
    }
}