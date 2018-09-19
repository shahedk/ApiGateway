using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class KeyInRole : ManyToManyBase
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public int KeyId { get; set; }
    
        public Key Key { get; set; }
        public Role Role { get; set; }
    }
}