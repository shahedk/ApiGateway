using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ApiGateway.Data.EFCore.Entity
{
    public class EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string OwnerKeyId { get; set; }


        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ModifiedDate { get; set; }  = DateTime.UtcNow;
    }
}
