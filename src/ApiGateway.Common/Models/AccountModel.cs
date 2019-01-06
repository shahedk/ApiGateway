using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApiGateway.Common.Models
{
    public class AccountModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(32)] 
        public string LoginName { get; set; } = "";


        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
