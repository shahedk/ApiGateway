using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Common.Constants
{
    public sealed class ApiKeyPropertyNames
    {
        // For KeyModel
        public const string ClientSecret1 = "ClientSecret1";
        public const string ClientSecret2 = "ClientSecret2";
        public const string ClientSecret3 = "ClientSecret3";

        // For KeyChallenge
        public const string ClientSecret = "ClientSecret";
        public const string PublicKey = "PublicKey";
    }
}
