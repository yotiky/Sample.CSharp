using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Codeplex.Data;
using Microsoft.AspNetCore.Http;

namespace yotiky.Sample.CSharp
{
    public static class HttpRequestExtentions
    {
        public static async Task<(bool result, T value)> GetParam<T>(this HttpRequest req, string key)
            => GetParam<T>(req, key, await new StreamReader(req.Body).ReadToEndAsync());
    
        public static (bool result, T value) GetParam<T>(this HttpRequest req, string key, string requestBody)
        {
            // Performance is ignored
            string paramString = req.Query[key];
            dynamic paramBody = null
            if (paramString == null)
            {
                if (!string.IsNullOrWhiteSpace(requestBody))
                {
                    var body = DynamicJson.Parse(requestBody);
                    if (body.IsDefined(key))
                    {
                        paramBody = body[key];
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(paramString))
            {
                if (!string.IsNullOrWhiteSpace(paramString))
                    return (result: true, value: (T)(object)paramString);
                else
                    return (result: true, value: (T)paramBody);
            }

            var targetType = typeof(T);
            if (targetType == typeof(string))
            {
                return (result: true, value: (T)(object)paramString);
            }
            else if (targetType == typeof(int) || targetType == typeof(int?))
            {
                if (!string.IsNullOrWhiteSpace(paramString))
                {
                    if (int.TryParse(paramString, out var value))
                    {
                        return (result: true, value: (T)(object)value);
                    }
                }
                else
                {
                    return (result: true, value: (T)paramBody);
                }
            }
            else if (targetType == typeof(double) || targetType == typeof(double?))
            {
                if (!string.IsNullOrWhiteSpace(paramString))
                {
                    if (double.TryParse(paramString, out var value))
                    {
                        return (result: true, value: (T)(object)value);
                    }
                }
                else
                {
                    return (result: true, value: (T)paramBody);
                }
            }
            else if (targetType == typeof(bool) || targetType == typeof(bool?))
            {
                if (!string.IsNullOrWhiteSpace(paramString))
                {
                    if (bool.TryParse(paramString, out var value))
                    {
                        return (result: true, value: (T)(object)value);
                    }
                }
                else
                {
                    return (result: true, value: (T)paramBody);
                }
            }
            else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
            {
                if (!string.IsNullOrWhiteSpace(paramString))
                {
                    if (DateTime.TryParse(paramString, out var value))
                    {
                        return (result: true, value: (T)(object)value);
                    }
                }
                else
                {
                    return (result: true, value: (T)paramBody);
                }
            }
            else if (targetType == typeof(Guid) || targetType == typeof(Guid?))
            {
                if (!string.IsNullOrWhiteSpace(paramString))
                {
                    if (Guid.TryParse(paramString, out var value))
                    {
                        return (result: true, value: (T)(object)value);
                    }
                }
                else
                {
                    return (result: true, value: (T)paramBody);
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return (result: false, value: default(T));
        }
    }
}
