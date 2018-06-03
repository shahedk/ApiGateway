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
    
        //TODO: Consider using ImmutableList<> and reuse in ToList() 
        private static List<string> _list = null;

        public static List<string> ToList()
        {
            return new List<string> {Get, Put, Post, Delete};
        } 

        public static bool IsValid(string type)
        {
            if (_list == null)
                _list = ToList();

            return _list.Contains(type);
        }
    }
}