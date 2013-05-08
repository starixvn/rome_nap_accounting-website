using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Sunplus.Accounting.Business.Customer.ConsolidatedGroup;
using Sunplus.Accounting.Business.BootStrappers;

public partial class Customer_AgencyGroup_GroupHeadLine : PageBase
{
    public string strRows = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadGroups();
        }
    }

    private void LoadGroups()
    {
        var consolidatedGroupManager = CommonBootStrapper.Locator.GetInstance<IConsolidatedGroupManager>();
        var listGroups = consolidatedGroupManager.GetAllGroup();
        int index = 0;
        foreach (var item in listGroups)
        {
            strRows += (index == 0 ? string.Empty : ",") + @"
                {
                    GroupID: " + item.GroupID.ToString() + @"
                    , CustID: '" + item.CustID + @"'
                    , UserName: '" + item.UserName + @"'
                    , Currency: '" + item.Currency + @"'
                    , CurrencyName: '" + item.CurrencyName + @"'
                    , TotalMember: '" + item.TotalMember.ToString() + @"'
                }
            ";
            index++;
        }
    }
}
