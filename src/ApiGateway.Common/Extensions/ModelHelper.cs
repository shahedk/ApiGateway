using System;
using System.Collections.Generic;
using ApiGateway.Common.Models;
using Newtonsoft.Json;

namespace ApiGateway.Common.Extensions
{
    public static class ModelHelper
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        
        public static string GenerateNewId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string GeneratePublicKey()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string GenerateSecret()
        {
            return Guid.NewGuid().ToString("N");// + Guid.NewGuid().ToString("N") ;
        }

        public static DateTime ToClientLocalTime(this DateTime dateTime)
        {
            return dateTime;
        }

        public static string ToJson(this Dictionary<string,string> properties)
        {
            return JsonConvert.SerializeObject(properties);
        }

        public static Dictionary<string,string> ToProperties(this string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public static KeyValidationResultLite ToLite(this KeyValidationResult val)
        {
            var result = new KeyValidationResultLite()
            {
                KeyId = val.KeyId,
                IsValid =     val.IsValid,
                InnerValidationResult =  val.InnerValidationResult
            };

            return result;
        }
        
    }
}