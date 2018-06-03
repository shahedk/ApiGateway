using System.Collections.Generic;

namespace ApiGateway.Common.Constants
{
    public class PropertyDataTypes
    {
        public const int Count = 5;
        public const string String = "string";
        public const string Int = "int";
        public const string Float = "float";
        public const string StringArray = "stringArray";
        public const string Boolean = "bool";
        
        private static List<string> _listCache = null;

        public static List<string> ToList()
        {
            return new List<string>(){ String, Int, Float, StringArray, Boolean};
        } 

        public static bool IsValid(string type)
        {
            if (_listCache == null)
                _listCache = ToList();

            return _listCache.Contains(type);
        }
 
    }
}
