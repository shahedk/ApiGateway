using System.IO;
using ApiGateway.Common.Constants;

namespace ApiGateway.Common.Models
{
    public class KeyProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }

        private string _type = string.Empty;
        public string Type
        {
            get => _type;
            set
            {
                if (PropertyDataTypes.IsValid(value))
                {
                    _type = value;
                }
                else
                {
                    var errorMessage = "Invalid data. Valid types are: " + string.Join(", ", PropertyDataTypes.ToList());
                    throw new InvalidDataException(errorMessage);
                }
            }
        }
    }
}