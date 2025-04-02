using System.Web.Mvc;

namespace Fanex.Tarus.Customer.Extensions
{
    public static class ViewDataExtensions
    {
        public static bool GetBoolean(this ViewDataDictionary viewData, string key)
        {
            if (viewData.ContainsKey(key) && viewData[key] != null)
            {
                if (bool.TryParse(viewData[key].ToString(), out var result))
                {
                    return result;
                }
            }

            return false; // Default to false if the key is missing or parsing fails
        }
    }
}