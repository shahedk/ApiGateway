using System.Collections.Generic;

namespace ApiGateway.Common
{
    public class ApiHttpMethods
    {
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Delete = "DELETE";
        public const string Put = "Put";

        public static readonly List<string> AsList = new List<string> {Get, Put, Post, Delete};

        public static bool IsValid(string keyType)
        {
            return AsList.Contains(keyType);
        }
    }
}