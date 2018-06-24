using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class AccessRule : EntityBase
    {
        [Required]
        [StringLength(20)]
        public string ServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name{ get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(30)]
        public string Type { get; set; }

        [StringLength(1000)]
        public string Properties { get; set; }        
    }
}