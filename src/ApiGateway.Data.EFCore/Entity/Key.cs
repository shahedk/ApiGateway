using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using ApiGateway.Common.Constants;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Key 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string PublicKey { get; set; }
        
        [Required]
        public bool IsDisabled { get; set; }
        
        private string _type = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string Type
        {
            get => _type;
            set
            {
                if (ApiKeyTypes.IsValid(value))
                {
                    _type = value;
                }
                else
                {
                    var errorMessage = "Invalid data. Valid types are: " + string.Join(", ", ApiKeyTypes.ToList());
                    throw new InvalidDataException(errorMessage);
                }
            }
        }

        [Required]
        [StringLength(1000)]
        public string Properties { get; set; }

        public int? OwnerKeyId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ModifiedDate { get; set; }  = DateTime.UtcNow;

        public List<KeyInRole> KeyInRoles { get; set; }
        
        
        // Owner references
        public List<Key> Keys { get; set; }
        public List<Service> Services { get; set; }
        public List<Role> Roles { get; set; }
        public List<AccessRule> AccessRules { get; set; }
    }
}