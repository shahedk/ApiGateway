namespace ApiGateway.Common.Models
{
    public class KeyValidationResult : KeyValidationResultLite
    {
        public string ApiId { get; set; }
        public string ServiceId { get; set; }
    }
}