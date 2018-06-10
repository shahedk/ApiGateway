using System;
using System.Collections.Generic;
using ApiGateway.Common.Models;
using Newtonsoft.Json;

namespace ApiGateway.Common.Extensions
{
    public static class ModelHelper
    {
        public static string GenerateNewId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string GeneratePublicKey()
        {
            return Guid.NewGuid().ToString("N");
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

        public static string ToJson(this List<Tag> tags)
        {
            return JsonConvert.SerializeObject(tags);
        }

        public static List<Tag> ToTags(this string json)
        {
            return JsonConvert.DeserializeObject<List<Tag>>(json);
        }
    }
}