using System.Collections.Generic;

namespace ApiGateway.Common.Constants
{
    public class ApiKeyTypes 
    {
        public const int Count = 2;
        public const string ClientSecret = "CLIENT_SECRET";
        public const string JwtToken = "JWT_TOKEN";

        //TODO: Consider using ImmutableList<> and reuse in ToList() 
        private static List<string> _list = null;

        public static List<string> ToList()
        {
            return new List<string> { ClientSecret, JwtToken };
        } 

        public static bool IsValid(string type)
        {
            if (_list == null)
                _list = ToList();

            return _list.Contains(type);
        }
    }
}