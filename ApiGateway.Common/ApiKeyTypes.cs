using System.Collections.Generic;

namespace ApiGateway.Common
{
    public class ApiKeyTypes
    {
        public const string ClientSecret = "SECRET";
        public const string JwtToken = "JWT";

        public static readonly List<string> AsList = new List<string> { ClientSecret, JwtToken };

        public static bool IsValid(string keyType)
        {
            return AsList.Contains(keyType);
        }
    }
}