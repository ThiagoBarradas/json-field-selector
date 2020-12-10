using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsonFieldSelector
{
    public static class JsonFieldSelectorExtension
    {
        public static string SelectFieldsFromString(this string json, string fields, char separator = ',')
        {
            return SelectFieldsFromString(json, fields.SplitFields(separator));
        }

        public static JToken SelectFieldsFromJToken(this JToken jtoken, string fields, char separator = ',')
        {
            return SelectFieldsFromJToken(jtoken, fields.SplitFields(separator));
        }

        public static T SelectFieldsFromObject<T>(this T obj, string fields, char separator = ',')
        {
            return SelectFieldsFromObject(obj, fields.SplitFields(separator));
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

            return ((JObject)JsonConvert.DeserializeObject(json))
                        .SelectFieldsFromJTokenCore(fields)
                        .ToString(Formatting.None);
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

            return jtoken.SelectFieldsFromJTokenCore(fields);
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

            return JToken.FromObject(obj)
                        .SelectFieldsFromJTokenCore(fields)
                        .ToObject<T>();
        }

        private static JToken SelectFieldsFromJTokenCore(this JToken jtoken, string[] fields)
        {
            var removeList = new List<JToken>();
            var stack = new Stack<JToken>();
            stack.Push(jtoken);
            do
            {
                var container = stack.Pop() as JContainer;
                if (container == null) continue;

                for (var i = 0; i < container.Count; i++)
                {
                    var jtokenChild = container.ElementAt(i);
                    if (jtokenChild is JProperty prop)
                    {
                        var path = prop.GetPathWithoutArrayConnotation();

                        var matching = fields.Any(item =>
                            path.Equals(item, StringComparison.InvariantCultureIgnoreCase) ||
                            path.StartsWith($"{item}.", StringComparison.InvariantCultureIgnoreCase));

                        if (!matching && !jtokenChild.IsObject())
                        {
                            removeList.Add(jtokenChild);
                        }
                    }
                    stack.Push(jtokenChild);
                }
            } while (stack.Any());
            
            for (var i = 0; i < removeList.Count; i++)
            {
                removeList.ElementAt(i).Remove();
            }
            
            return jtoken.RemoveEmptyChildren();
        }

        private static string[] SplitFields(this string fields, char separator)
        {
            return fields?.Split(separator) ?? new string[] { };
        }

        private static string GetPathWithoutArrayConnotation(this JProperty property)
        {
            return property.Path.Contains("[")
                ? Regex.Replace(property.Path, @"\[{1}\d+\]{1}", string.Empty)
                : property.Path;
        }

        private static JToken RemoveEmptyChildren(this JToken jtoken)
        {
            switch(jtoken.Type)
            {
                case JTokenType.Object:
                    return jtoken.RemoveEmptyChildrenFromObject();
                case JTokenType.Array:
                    return jtoken.RemoveEmptyChildrenFromArray();
                default:
                    return jtoken;
            }
        }

        private static JToken RemoveEmptyChildrenFromArray(this JToken jtoken)
        {
            JArray copy = new JArray();
            foreach (JToken item in jtoken.Children())
            {
                var child = item;
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

        private static JToken RemoveEmptyChildrenFromObject(this JToken jtoken)
        {
            var copy = new JObject();
            foreach (JProperty prop in jtoken.Children<JProperty>())
            {
                var child = prop.Value;
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

        private static bool IsEmpty(this JToken jtoken)
        {
            return (jtoken.Type == JTokenType.Array && !jtoken.HasValues) ||
                   (jtoken.Type == JTokenType.Object && !jtoken.HasValues) ||
                   (jtoken.Type == JTokenType.String && jtoken.ToString() == String.Empty) ||
                   (jtoken.Type == JTokenType.Null);
        }

        private static readonly List<JTokenType?> _objectTypes = new List<JTokenType?>(2) { JTokenType.Property, JTokenType.Object };

        private static bool IsObject(this JToken jtoken)
        {
            return _objectTypes.Contains(jtoken.Values().FirstOrDefault()?.Type);
        }
    }
}
