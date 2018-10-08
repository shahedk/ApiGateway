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
        public static string GetSecret1(this KeyModel key)
        {
            if(key.Properties.Keys.Contains(ApiKeyPropertyNames.ClientSecret1))
            {
                return key.Properties[ApiKeyPropertyNames.ClientSecret1];
            }
            else
            {
                return string.Empty;
            }
        }
        
        public static string GetSecret2(this KeyModel key)
        {
            if(key.Properties.Keys.Contains(ApiKeyPropertyNames.ClientSecret2))
            {
                return key.Properties[ApiKeyPropertyNames.ClientSecret2];
            }
            else
            {
                return string.Empty;
            }
        }
        
        public static string GetSecret3(this KeyModel key)
        {
            if(key.Properties.Keys.Contains(ApiKeyPropertyNames.ClientSecret3))
            {
                return key.Properties[ApiKeyPropertyNames.ClientSecret3];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
