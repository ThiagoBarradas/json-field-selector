using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JsonFieldSelector
{
    public static class JsonFieldSelectorExtension
    {
        public static string SelectFieldsFromString(this string json, string fields, char separator = ',')
        {
            var fieldsArray = fields.SplitFields(separator);
            return SelectFieldsFromString(json, fieldsArray);
        }

        public static JToken SelectFieldsFromJToken(this JToken jtoken, string fields, char separator = ',')
        {
            var fieldsArray = fields.SplitFields(separator);
            return SelectFieldsFromJToken(jtoken, fieldsArray);
        }

        public static T SelectFieldsFromObject<T>(this T obj, string fields, char separator = ',')
        {
            var fieldsArray = fields.SplitFields(separator);
            return SelectFieldsFromObject(obj, fieldsArray);
        }

        public static string SelectFieldsFromString(this string json, string[] fields)
        {
            if (string.IsNullOrWhiteSpace(json) == true)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (fields?.Any() != true)
            {
                return json;
            }

            var jtoken = (JObject)JsonConvert.DeserializeObject(json);

            SelectFieldsFromJTokenCore(jtoken, fields);

            return jtoken.RemoveEmptyChildren().ToString(Formatting.None);
        }

        public static JToken SelectFieldsFromJToken(this JToken jtoken, string[] fields)
        {
            if (jtoken == null)
            {
                throw new ArgumentNullException(nameof(jtoken));
            }

            if (fields?.Any() != true)
            {
                return jtoken;
            }

            SelectFieldsFromJTokenCore(jtoken, fields);

            return jtoken.RemoveEmptyChildren();
        }

        public static T SelectFieldsFromObject<T>(this T obj, string[] fields) 
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (fields?.Any() != true)
            {
                return obj;
            }

            var jtoken = JToken.FromObject(obj);

            SelectFieldsFromJTokenCore(jtoken, fields);

            return jtoken.RemoveEmptyChildren().ToObject<T>();
        }

        private static void SelectFieldsFromJTokenCore(this JToken jtoken, string[] fields)
        {
            JContainer container = jtoken as JContainer;

            if (container == null)
            {
                return; // abort recursive
            }

            List<JToken> removeList = new List<JToken>();

            foreach (JToken jtokenChildren in container.Children())
            {
                if (jtokenChildren is JProperty prop)
                {
                    var path = prop.Path.RemoveArrayConnotation();

                    var matching = fields.Any(item =>
                    {
                        return path.Equals(item, StringComparison.InvariantCultureIgnoreCase) ||
                               path.StartsWith(item + ".", StringComparison.InvariantCultureIgnoreCase);
                    });

                    if (!matching && !IsObject(jtokenChildren))
                    {
                        removeList.Add(jtokenChildren);
                    }
                }

                // call recursive 
                SelectFieldsFromJTokenCore(jtokenChildren, fields);
            }

            for (int i = removeList.Count() - 1; i >= 0; i--)
            {
                removeList[i].Remove();
            }
        }

        private static string[] SplitFields(this string fields, char separator)
        {
            return fields?.Split(separator) ?? new string[] { };
        }

        private static string RemoveArrayConnotation(this string path)
        {
            return Regex.Replace(path, @"\[{1}[0-9]+\]{1}", "");
        }

        private static List<JTokenType?> ObjectTypes = new List<JTokenType?> { JTokenType.Property, JTokenType.Object };

        private static bool IsObject(JToken jtoken)
        {
            var first = jtoken.Values().FirstOrDefault();

            return ObjectTypes.Contains(first?.Type);
        }

        private static JToken RemoveEmptyChildren(this JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                JObject copy = new JObject();
                foreach (JProperty prop in token.Children<JProperty>())
                {
                    JToken child = prop.Value;
                    if (child.HasValues)
                    {
                        child = child.RemoveEmptyChildren();
                    }
                    if (!child.IsEmpty())
                    {
                        copy.Add(prop.Name, child);
                    }
                }
                return copy;
            }
            else if (token.Type == JTokenType.Array)
            {
                JArray copy = new JArray();
                foreach (JToken item in token.Children())
                {
                    JToken child = item;
                    if (child.HasValues)
                    {
                        child = child.RemoveEmptyChildren();
                    }
                    if (!child.IsEmpty())
                    {
                        copy.Add(child);
                    }
                }
                return copy;
            }
            return token;
        }

        private static bool IsEmpty(this JToken token)
        {
            return (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }
}
