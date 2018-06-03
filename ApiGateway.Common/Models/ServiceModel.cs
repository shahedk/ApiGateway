using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Common.Models
{
    public class ServiceModel : ModelBase
    {
        [Required]
        public string Name { get; set; }

        public List<Tag> Tags { get; set; }
    }
}