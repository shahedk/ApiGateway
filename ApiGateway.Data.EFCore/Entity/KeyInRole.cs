using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class KeyInRole : EntityBase
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public int KeyId { get; set; }
    
        public Key Key { get; set; }
        public Role Role { get; set; }
    }
}