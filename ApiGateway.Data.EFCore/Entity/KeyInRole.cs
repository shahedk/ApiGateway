using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class KeyInRole : EntityBase
    {
        [Required]
        [StringLength(20)]
        public string RoleId { get; set; }

        [Required]
        [StringLength(20)]
        public string KeyId { get; set; }
    }
}