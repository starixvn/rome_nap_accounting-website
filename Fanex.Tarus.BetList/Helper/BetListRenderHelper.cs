namespace Fanex.Tarus.BetList.Helper
{
    using System.Text;
    using Fanex.Tarus.BetList.Resources;

    public static class BetListRenderHelper
    {
        public static string GetCommonHeaderRows()
        {
            var result = new StringBuilder();
            result.Append("<tr>");
            result.Append("<th><div class=\"width30 height34 table-cell\">" + BetListResources.No + "</div></th>");
            result.Append("<th><div class=\"width120 height34 table-cell\">" + BetListResources.Date + "</div></th>");
            result.Append("<th><div class=\"column-choice height34 table-cell\">" + BetListResources.Choice + "</div></th>");
            result.Append("<th><div class=\"width60 height34 table-cell\">" + BetListResources.Odds + "</div></th>");
            result.Append("<th><div class=\"width50 height34 table-cell\">" + BetListResources.Stake + "</div></th>");
            result.Append("<th><div class=\"column-winloss height34 table-cell\">" + BetListResources.Win_Loss + "</div></th>");
            result.Append("<th><div class=\"width100 height34 table-cell\">" + BetListResources.Status + "</div></th>");
            result.Append("<th><div class=\"width60 height34 table-cell\">" + BetListResources.Super + "<br />" + BetListResources.PT + "/" + BetListResources.Comm + "</div></th>");
            result.Append("<th><div class=\"width60 height34 table-cell\">" + BetListResources.Master + "<br />" + BetListResources.PT + "/" + BetListResources.Comm + "</div></th>");
            result.Append("<th><div class=\"width60 height34 table-cell\">" + BetListResources.Agent + "<br />" + BetListResources.PT + "/" + BetListResources.Comm + "</div></th>");
            result.Append("</tr>");
            return result.ToString();
        }
    }
}
