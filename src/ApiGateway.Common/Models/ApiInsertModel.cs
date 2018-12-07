using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using ApiGateway.Common.Constants;

namespace ApiGateway.Common.Models
{
    public class ApiInsertModel
    {
        private string _name;
        private string _url;
        
        [Required]
        public string ServiceId { get; set; }

        [Required]
        public string Name
        {
            get => _name;
            set => _name = value.ToLower();
        }

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
                    _httpMethod= value.ToUpper();
                }
                else
                {
                    var errorMessage = "Invalid data. Valid types are: " + string.Join(", ", ApiHttpMethods.ToList());
                    throw new InvalidDataException(errorMessage);
                }
            }
        }

        [Required]
        public string Url
        {
            get => _url;
            set => _url = value.ToLower();
        }
        
        public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
    }
}