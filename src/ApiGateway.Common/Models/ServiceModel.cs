using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Common.Models
{
    public class ServiceModel : ModelBase
    {
        private string _name;

        [Required]
        public string Name
        {
            get => _name;
            set => _name = value.ToLower();
        }

        [MaxLength(200)] public string Description { get; set; } = string.Empty;
    }
}