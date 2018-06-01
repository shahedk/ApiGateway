using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ApiGateway.Common.Model
{
    public class KeyModel : ModelBase
    {
        
        [Required]
        public KeyAccessLevel AccessLevel { get; set; }
        
        public string PublicKey { get; set; }
        public List<Tag> Tags { get; set; }

        private string _type = string.Empty;

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
                    var errorMessage = "Invalid data. Valid types are: " + string.Join(", ", ApiKeyTypes.AsList);
                    throw new InvalidDataException(errorMessage);
                }
            }
        }

        public List<KeyProperty> Properties { get; set; }

        public List<RoleModel> Roles { get; set; }
    }
}