using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Key : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string PublicKey { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Tags { get; set; }
        
        [Required]
        [StringLength(20)]
        public string Type
        {
            get;
            set;
        }

        [Required]
        [StringLength(1000)]
        public string Properties { get; set; }
    }
}