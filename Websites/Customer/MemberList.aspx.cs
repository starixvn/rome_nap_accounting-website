using System;
using DNAide.Core;

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