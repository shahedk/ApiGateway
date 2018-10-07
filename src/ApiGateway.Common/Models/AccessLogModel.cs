using System;

namespace ApiGateway.Common.Models
{
    public class AccessLogModel : ModelBase
    {
        public string ApiId { get; set; }
        public string ServiceId { get; set; }
        public string KeyId { get; set; }
        public string PublicKey { get; set; }
        public string Url { get; set; }
        public DateTime LogTime { get; set; }
        public string HttpMethod { get; set; }
        public bool IsValid { get; set; }
        public string ValidationResult { get; set; }
        public string RequestInfo { get; set; }
    }
}