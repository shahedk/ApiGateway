using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Service : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [StringLength(500)]
        public string Tags { get; set; }
        
    }
}