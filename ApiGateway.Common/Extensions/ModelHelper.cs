using System;
using System.Collections.Generic;
using ApiGateway.Common.Models;

namespace ApiGateway.Common.Extensions
{
    public static class ModelHelper
    {
        public static string GenerateNewId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static DateTime ToClientLocalTime(this DateTime dateTime)
        {
            return dateTime;
        }

        public static string ToJson(this List<KeyProperty> properties)
        {
            return "";
        }

        public static List<KeyProperty> ToProperties(this string json)
        {
            return new List<KeyProperty>();
        }

        public static string ToJson(this List<Tag> tags)
        {
            return "";
        }

        public static List<Tag> ToTags(this string json)
        {
            return new List<Tag>();
        }
    }
}