using System;
using System.Web.UI;

public partial class CheckRedis : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        var type = actionType.Value; //Request["type"];
        var key = txtKey.Value; //Request["key"];
        var value = txtValue.Value; //Request["value"];
        if (type.Equals("get", StringComparison.CurrentCultureIgnoreCase))
        {
            txtValue.Value = Sunplus.Accounting.Core.Cache.Extension.CacheExtension.Get<string>(key);
            //Response.Write(Sunplus.Accounting.Core.Cache.Extension.CacheExtension.Get<string>(key));
        }

        if (type.Equals("set", StringComparison.CurrentCultureIgnoreCase))
        {
            Sunplus.Accounting.Core.Cache.Extension.CacheExtension.Set(key, value);
            Response.Write("Set success!");
            txtValue.Value = string.Empty;
        }
    }


}