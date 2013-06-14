using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Text;
using DNAide.Data;


public partial class SpecialDelete_Outright : PageBase
{
    public string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            username = AppSession.AccUserName;

            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("SpecialDelete/FillterFormOutright.htm");
            StringBuilder Options = new StringBuilder();
            string op = string.Empty;
            DataTable dt = new DataTable();

            #region Leagues
            

            #region DNAide Lib
            DbResult rs = AccountTransaction.BettransDelSpecial_LeagueSelOutright();
            dt = rs.ResultSet.Tables[0];
            #endregion

            int leagueid = 0, index=0;

            foreach (DataRow dr in dt.Rows)
            {
                if(index==0)
                    leagueid = CastAide.AsInt32(dr["leagueid"]);
                op = "<option value='" + dr["leagueid"] + "'>" + CacheRef.GetLeagueName(dr["leagueid"])  + "</option>";
                Options.Append(op);
                index++;
            }
            TemplateHandler.Assign(ref tpl, "option-league", Options.ToString());
            #endregion

            #region Outright
            

            #region DNAide Lib
            rs = AccountTransaction.BettransDelSpecial_OutrightSel(leagueid);
            dt = rs.ResultSet.Tables[0];
            #endregion

            Options = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                op = "<option value='" + dr["orid"] + "'>" + CacheRef.GetTeamName(dr["TeamId"]) + "</option>";
                Options.Append(op);
            }
            TemplateHandler.Assign(ref tpl, "option-outright", Options.ToString());
            #endregion

            sContent.InnerHtml = tpl;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
            sContent.InnerHtml = "<span style='color:red; font-weight:bold'>" + ex.Message + "</span>";
        }
    }
}
