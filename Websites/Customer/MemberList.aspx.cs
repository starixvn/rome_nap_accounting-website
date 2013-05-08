using System;
using System.Data;
using System.Collections;
using System.Text;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;


using Sunplus.Accounting.Core.Common;

public partial class Customer_MemberList : RequiredAuthorizationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override int CustId
    {
        get
        {
            return CastAide.AsInt32(Request[Sunplus.Accounting.Core.Common.Constants.QueryStringKey.AgentId]);
        }
    }
}
