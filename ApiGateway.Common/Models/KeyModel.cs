using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ApiGateway.Common.Constants;

namespace ApiGateway.Common.Models
{
    public class KeyModel : ModelBase
    {
        public bool IsDisabled { get; set; } = false;

        [Key]
        public string PublicKey { get; set; }
        
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
                    var errorMessage = "Invalid data. Valid types are: " + string.Join(", ", ApiKeyTypes.ToList());
                    throw new InvalidDataException(errorMessage);
                }
            }
        }

        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
    }
}