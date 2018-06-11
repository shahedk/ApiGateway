using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ApiGateway.Common.Constants;

namespace ApiGateway.Common.Models
{
    public class ApiModel : ModelBase
    {
        [Required]
        public string ServiceId { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        private string _httpMethod = string.Empty;
        
        [Required]
        public string HttpMethod
        {
            get => _httpMethod;
            set
            {
                if (ApiHttpMethods.IsValid(value))
                {
                    _httpMethod= value;
                }
                else
                {
                    var errorMessage = "Invalid data. Valid types are: " + string.Join(", ", ApiHttpMethods.ToList());
                    throw new InvalidDataException(errorMessage);
                }
            }
        }

        [Required]
        public string Url { get; set; }

        public List<RoleModel> ApiInRole { get; set; }
    }
}