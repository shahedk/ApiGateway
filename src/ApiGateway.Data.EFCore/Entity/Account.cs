using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Account : EntityBase
    {
        [Required]
        [StringLength(20)]
        public string LoginName { get; set; }
    }
}
