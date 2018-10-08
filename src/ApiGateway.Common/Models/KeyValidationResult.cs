namespace ApiGateway.Common.Models
{
    public class KeyValidationResult
    {
        public string KeyId = string.Empty;
        public bool IsValid = false;
        public string Message = string.Empty;

        public KeyValidationResult InnerValidationResult = null;
    }
}