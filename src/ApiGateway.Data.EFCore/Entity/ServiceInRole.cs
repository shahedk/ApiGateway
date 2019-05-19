using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApiGateway.Data.EFCore.Entity
{
    public class ServiceInRole : ManyToManyBase
    {
        [Required]
        [StringLength(20)]
        public int ServiceId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public Service Service { get; set; }

        public Role Role { get; set; }
    }
}
