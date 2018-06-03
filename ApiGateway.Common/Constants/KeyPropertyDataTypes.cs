using System.Collections.Generic;

namespace ApiGateway.Common.Constants
{
    //TODO: Consider renaming...
    public class KeyPropertyDataTypes
    {
        public const int Count = 5;
        public const string String = "string";
        public const string Int = "int";
        public const string Float = "float";
        public const string StringArray = "stringArray";
        public const string Boolean = "bool";
        
        //TODO: Consider using ImmutableList<> and reuse in ToList() 
        private static List<string> _list = null;

        public static List<string> ToList()
        {
            return new List<string>(){ String, Int, Float, StringArray, Boolean};
        } 

        public static bool IsValid(string type)
        {
            if (_list == null)
                _list = ToList();

            return _list.Contains(type);
        }
 
    }
}
