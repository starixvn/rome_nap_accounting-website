using System;
using System.Data;
using System.Collections;
using System.Text;

using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;


public partial class Customer_AgentList : RequiredAuthorizationPage
{

    private string strQuery;
    int PageIndex = 1, TotalPages = 0, type = 1;
    protected void Page_Load(object sender, EventArgs e) 
    {
    }

    protected override int CustId
    {
        get
        {
            return CastAide.AsInt32(Request[Sunplus.Accounting.Core.Common.Constants.QueryStringKey.MasterId]);
        }
    }

}
