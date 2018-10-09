namespace ApiGateway.Common.Models
{
    public class KeyValidationResultLite
    {
        public string KeyId = string.Empty;
        public bool IsValid = false;
        public string Message = string.Empty;

        public KeyValidationResultLite InnerValidationResult = null;
    }
}