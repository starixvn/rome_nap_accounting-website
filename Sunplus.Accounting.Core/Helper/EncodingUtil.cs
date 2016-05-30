using System;
using System.Reflection;
using System.Web;

namespace Sunplus.Accounting.Core.Helper
{
    public static class EncodingUtil
    {
        public static T HtmlEncode<T>(T entity) where T : class
        {
            return Encoding(entity, true);
        }

        public static T HtmlDecode<T>(T entity) where T : class
        {
            return Encoding(entity, false);
        }

        private static T Encoding<T>(T entity, bool encode) where T : class
        {
            Func<string, string> encodingFunc = (str) => encode ? HttpUtility.HtmlEncode(str) : HttpUtility.HtmlDecode(str);

            if (typeof(T) == typeof(string))
            {
                return encodingFunc(entity as string) as T;
            }

            foreach (PropertyInfo prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public))
            {
                if (prop.PropertyType == typeof(string) && prop.CanRead && prop.CanWrite)
                {
                    object value = prop.GetValue(entity, null);
                    string encodedValue = encodingFunc(value as string);

                    prop.SetValue(entity, encodedValue, null);
                }
            }

            return entity;
        }
    }
}
