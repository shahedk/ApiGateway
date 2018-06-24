namespace ApiGateway.Common.Models
{
    public class KeyValidationResult
    {
        public bool IsValid = false;
        public string Message = string.Empty;

        public KeyValidationResult InnerValidationResult = null;
    }
}