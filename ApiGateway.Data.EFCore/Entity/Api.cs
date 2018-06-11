using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class Api : EntityBase
    {
        [Required]
        [StringLength(20)]
        public string ServiceId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(20)]
        public string HttpMethod
        {
            get;
            set;
        }

        [Required]
        [StringLength(500)]
        public string Url { get; set; }

    }
}