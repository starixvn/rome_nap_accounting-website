using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;

public partial class Test_testpager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var name = "STeven";
        var id = 1234;
        try
        {
            var test = Convert.ToInt32("fasdfa");
            //var test = GetName(new {name});
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex, name, id);
        }

    }

    static string GetName<T>(T item) where T : class
    {
        var properties = typeof(T).GetProperties();
        //Enforce.That(properties.Length == 1);
        return properties[0].Name;
    }
}