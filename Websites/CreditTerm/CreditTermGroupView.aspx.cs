using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.WcfServiceLibrary.CreditTerm;
using Sunplus.Accounting.Business.BootStrappers;

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
