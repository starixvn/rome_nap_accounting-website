using System;
using Sunplus.Accounting.WcfServiceLibrary.CreditTerm;

/// <summary>
/// CITS
/// </summary>
public partial class CreditTermGroupView : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["IsExport"] == "1")
        {
            string query = Request["Query"];
            string sortOrder = Request["SortOrder"];
            string sortName = Request["SortName"];

            ICreditTermService creditTermService = new CreditTermService();
            creditTermService.ExportAllGroup(query, sortOrder, sortName);
        }
    }
}