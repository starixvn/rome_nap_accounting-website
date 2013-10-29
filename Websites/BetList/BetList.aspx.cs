using System;
using System.Web;
using DNAide.Core;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Core.Extensions;

public partial class BetList : RequiredAuthorizationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int listType = 1; // 1:default (Link from BetList);  ||  2:by day (Link from Statement);  ||  3:by IP (Link from By IP)
            listType = CastAide.AsInt16(Request["bltype"], 1);
            if (listType < 1 || listType > 3) listType = 1; // Max = 3
            if (listType == 1) //Main
            {
                BetListMain betlist = new BetListMain();
                reportHeader.Text = betlist.CreateHeader(null);
            }
            else if (listType == 2) //Bet list statement
            {
                BetlistByStatement betlist = new BetlistByStatement();
                reportHeader.Text = betlist.CreateHeader(null);
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            Response.Write(ex.Message);
        }
    }

    protected override int CustId
    {
        get
        {
            return CastAide.AsInt32(HttpContext.Current.Session["CustId"]);
        }
    }
}