using System;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class EntityBase
    {
        [Key]
        [Required]
        [StringLength(20)]
        public string Id { get; set; }

        [Required]
        [StringLength(32)]
        public string OwnerKeyId { get; set; }


        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ModifiedDate { get; set; }  = DateTime.UtcNow;
    }
}
