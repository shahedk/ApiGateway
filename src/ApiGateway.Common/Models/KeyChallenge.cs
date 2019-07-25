using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Common.Models
{
    public class KeyChallenge
    {
        public string Type { get; set; }
        public Dictionary<string,string> Properties { get; set; }
    }
}
