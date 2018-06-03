using System.Collections.Generic;

namespace ApiGateway.Common.Constants
{
    public class ApiKeyTypes 
    {
        public const int Count = 2;
        public const string ClientSecret = "CLIENT_SECRET";
        public const string JwtToken = "JWT_TOKEN";

        private static List<string> _listCache = null;

        public static List<string> ToList()
        {
            return new List<string> { ClientSecret, JwtToken };
        } 

        public static bool IsValid(string type)
        {
            if (_listCache == null)
                _listCache = ToList();

            return _listCache.Contains(type);
        }
    }
}