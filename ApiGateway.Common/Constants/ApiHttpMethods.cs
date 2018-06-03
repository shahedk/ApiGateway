using System.Collections.Generic;

namespace ApiGateway.Common.Constants
{
    public class ApiHttpMethods 
    {
        public const int Count = 4;
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Delete = "DELETE";
        public const string Put = "Put";
    
        private static List<string> _listCache = null;

        public static List<string> ToList()
        {
            return new List<string> {Get, Put, Post, Delete};
        } 

        public static bool IsValid(string type)
        {
            if (_listCache == null)
                _listCache = ToList();

            return _listCache.Contains(type);
        }
    }
}