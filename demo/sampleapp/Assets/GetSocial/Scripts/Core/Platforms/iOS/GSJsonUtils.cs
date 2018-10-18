using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;

#if UNITY_IOS

namespace GetSocialSdk.Core
{
    public static class GSJsonUtils
    {
        public static T Parse<T>(string json) where T : IConvertableFromNative<T>, new()
        {
            T result = new T();
            if (string.IsNullOrEmpty(json))
            {
                // return immediately in case of unexpected empty/null json
                GetSocialDebugLogger.E("ParseList is parsing null or empty json string");
                return result;
            }

            return result.ParseFromJson(json.ToDict());
        }

        public static List<T> ParseList<T>(string json) where T : IConvertableFromNative<T>, new()
        {
            var result = new List<T>();
            
            if (string.IsNullOrEmpty(json))
            {
                // return immediately in case of unexpected empty/null json
                GetSocialDebugLogger.E("ParseList is parsing null or empty json string");
                return result;
            }
            
            
            var entities = GSJson.Deserialize(json) as List<object>;
            
            if (entities == null)
            {
                return result;
            }
            return entities
                .ConvertAll(item => item as Dictionary<string, object>)
                .ConvertAll(entity => new T().ParseFromJson(entity));
        }

        public static Dictionary<string, string> ParseDictionary(string json)
        {
            var dictionary = GSJson.Deserialize(json) as Dictionary<string, object>;

            var result = new Dictionary<string, string>();

            foreach (var key in dictionary.Keys)
            {
                result[key] = (string) dictionary[key];
            }

            return result;
        }

        public static Dictionary<string, object> ToDict(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                GetSocialDebugLogger.W("Trying to deserialize empty or null string as dictionary");
                return new Dictionary<string, object>();
            }

            return GSJson.Deserialize(json) as Dictionary<string, object>;
        }

        public static Dictionary<string, string> ToStrStrDict(this Dictionary<string, object> json)
        {
            if (json == null || json.Count == 0)
            {
                return new Dictionary<string, string>();
            }

            return json.ToDictionary(entry => entry.Key, entry => entry.Value as string);
        }

        public static Dictionary<string, TValue> ParseDictionary<TValue>(string json)
            where TValue : IConvertableFromNative<TValue>, new()
        {
            var result = new Dictionary<string, TValue>();

            if (string.IsNullOrEmpty(json))
            {
                // return immediately in case of unexpected empty/null json
                GetSocialDebugLogger.E("ParseDictionary is parsing null or empty json string");
                return result;
            }

            var stringDict = ParseDictionary(json);
            foreach (var keyValuePairs in stringDict)
            {
                result.Add(keyValuePairs.Key, Parse<TValue>(keyValuePairs.Value));
            }
            return result;
        }
    }
}

#endif