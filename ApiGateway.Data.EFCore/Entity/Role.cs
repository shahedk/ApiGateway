using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Role : EntityBase
    {
        [Required]
        public int ServiceId { get; set; }

        public Service Service { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<KeyInRole> KeyInRoles { get; set; }
        public List<ApiInRole> ApiInRoles { get; set; }
    }
}