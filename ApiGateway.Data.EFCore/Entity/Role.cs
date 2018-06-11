using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Role : EntityBase
    {
        [Required]
        [StringLength(20)]
        public string ServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}