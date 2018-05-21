using System.Collections.Generic;
using System.IO;

namespace ApiGateway.Common.Model
{
    public class ApiModel : ModelBase
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }

        public string Name { get; set; }
        public List<Tag> Tags { get; set; }

        private string _httpMethod = string.Empty;
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
                    var errorMessage = "Invalid data. Valid types are: " + string.Join(", ", ApiHttpMethods.AsList);
                    throw new InvalidDataException(errorMessage);
                }
            }
        }
        public string Url { get; set; }

        public List<RoleModel> ApiInRole { get; set; }
    }
}