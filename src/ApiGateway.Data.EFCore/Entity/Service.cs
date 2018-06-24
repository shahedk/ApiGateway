using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Service : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Role> Roles { get; set; }
    }
}