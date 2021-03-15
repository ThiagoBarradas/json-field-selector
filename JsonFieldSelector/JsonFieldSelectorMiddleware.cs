using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFieldSelector
{
    public class JsonFieldSelectorMiddleware
    {
        private readonly RequestDelegate Next;

        private readonly string PropertyName;

        public JsonFieldSelectorMiddleware(RequestDelegate next)
        {
            this.Next = next;
            this.PropertyName = "fields";
        }

        public JsonFieldSelectorMiddleware(RequestDelegate next, string property)
        {
            if (string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentNullException(nameof(property));
            }

            this.Next = next;
            this.PropertyName = property.ToLowerInvariant();
        }

        public async Task Invoke(HttpContext context)
        {
            await this.Next(context);

            if (context.Request.Query.Any(r => r.Key.ToLowerInvariant() == this.PropertyName))
            {
                var json = BodyAsString(context.Response.Body);

                try
                {
                    var fields = context.Request.Query
                        .FirstOrDefault(r => r.Key.ToLowerInvariant() == this.PropertyName)
                        .Value.ToString();
                    json = JsonFieldSelectorExtension.SelectFieldsFromString(json, fields);
                }
                catch
                {
                    json = "{ }";
                }

                context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(json ?? ""));
            }
        }

        public static string BodyAsString(Stream stream)
        {
            try
            {
                var result = "";
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                {
                    result = reader.ReadToEnd();
                }
                stream.Position = 0;

                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        
    }
}

namespace JsonFieldSelector
{
    public static class JsonFieldSelectorMiddlewareExtension
    {
        public static void UseJsonFieldSelector(this IApplicationBuilder app, string property = "fields", bool enabled = true)
        {
            if (enabled)
            {
                app.UseMiddleware<JsonFieldSelectorMiddleware>(property);
            }
        }
    }
}
