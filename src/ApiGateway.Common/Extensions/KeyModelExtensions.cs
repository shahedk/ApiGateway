using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Models;

namespace ApiGateway.Common.Extensions
{
    public static class KeyModelExtensions
    {
        public static string GetSecret(this KeyModel key)
        {
            if(key.Properties.Keys.Contains(ApiKeyPropertyNames.ClientSecret))
            {
                return key.Properties[ApiKeyPropertyNames.ClientSecret];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
