using System;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Data.EFCore.Entity
{
    public class AccessLog : EntityBase
    {
        [Required]
        public int ApiId { get; set; }
        
        [Required]
        public int ServiceId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string KeyId { get; set; }
        
        [Required]
        [StringLength(20)]
        public string PublicKey { get; set; }
        
        [Required]
        [StringLength(1000)]
        public string Url { get; set; }
        
        [Required]
        public DateTime LogTime { get; set; }
        
        [Required]
        [StringLength(10)]
        public string HttpMethod { get; set; }
        
        [Required]
        public bool IsValid { get; set; }
        
        [StringLength(500)]
        public string ValidationResult { get; set; }
        
        [StringLength(500)]
        public string RequestInfo { get; set; }
    }
}