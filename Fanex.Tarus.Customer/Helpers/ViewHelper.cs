using System.Web.Mvc;

namespace Fanex.Tarus.Customer.Helpers
{
    public static class ViewHelper
    {
        public static bool GetBooleanFromViewData(ViewDataDictionary viewData, string key)
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