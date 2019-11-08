using System;
using Fanex.Result.Core.CardClub;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewCardClubResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result;
        var service = new CardClubResult();

        try
        {
            result = service.GetResult(
                Request.QueryString["custId"],
                Request.QueryString["gameTypeId"],
                Request.QueryString["transId3rd"]);
            if (string.IsNullOrEmpty(result))
            {
                result = "<div class=\"alertboxerr\"><div>There is no information available</div></div>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            result = "<div class=\"alertboxerr\"><div>" + ex.Message + "</div></div>";
        }

        this.lResult.Text = result;
    }
}
