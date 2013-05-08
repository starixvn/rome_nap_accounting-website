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
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils.DAL;
using DNAide.Utils;
using DNAide.Data;

/// <summary>
/// @Khoa.Tran 
/// HedgeRecordEdit.tpl
/// HedgeRecordEdit.aspx
/// HedgeRecordEdit.cs
/// UpdateHedgeRecord.ashx
/// AccountReport.cs
/// sp select : Acc_HedgeSel
/// sp update: Acc_HedgeUpd
/// </summary>
public partial class Report_HedgeRecordEdit : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/HedgeRecordEdit.htm");
        long TransID = CastAide.AsInt64(Request["TransID"], 0);
        DbResult rs = AccountReport.Acc_HedgeSel(TransID);
        DataTable dt = rs.ResultSet.Tables[0];
        string UserName = Request["CustId"];
        string KOT = "", Event = "", strChoice = "", HDP_OU = "", LiveHomeCore = "", LiveAwayScore = "", Price = "", Amount = "", Remark = "", EditedBy = "", Display = "";
        DataRow dr = null;
        bool IsLive = false;
        decimal hdp1 = 0, hdp2 = 0;
        if (dt.Rows.Count > 0)
        {
            dr = dt.Rows[0];
            KOT = dr["ShowTime"].ToString();
            //Event = dr["Home"].ToString() + " -vs- " + dr["Away"];
            Event = CacheRef.GetTeamName(dr["HomeId"]) + (CastAide.AsBoolean(dr["IsNeutral"]) ? Resources.Language.IsNeutral : string.Empty) + " -vs- " + CacheRef.GetTeamName(dr["AwayId"]);
            strChoice = CacheRef.GetTeamName(dr["HomeId"]);
            int BetType = CastAide.AsInt32(dr["bettype"]);
            switch (BetType)
            {
                case 1:
                case 7:
                    if (string.Compare(dr["betteam"].ToString(), "a", true) == 0)
                        strChoice = CacheRef.GetTeamName(dr["AwayId"]);
                    break;
                case 3:
                case 8:
                    if (string.Compare(dr["betteam"].ToString(), "a", true) == 0)
                        strChoice = "Under";
                    else
                        strChoice = "Over";
                    break;
            }

            //Team = dr["Choice"].ToString();
            LiveHomeCore = dr["LiveHomeScore"].ToString();
            LiveAwayScore = dr["LiveAwayScore"].ToString();
            Price = NumberFormat.DecFormat(dr["Odds"], 3);
            Amount = NumberFormat.DecFormat(dr["Stake"], 2);
            Remark = dr["Remark"].ToString();
            EditedBy = dr["creator"].ToString();
            HDP_OU = NumberFormat.DecFormat(dr["hdp"], 2);
            hdp1 = CastAide.AsDecimal(dr["hdp1"], 2);
            hdp2 = CastAide.AsDecimal(dr["hdp2"], 2);
            TemplateHandler.Assign(ref tpl, "errmsg", "");
            IsLive = CastAide.AsBoolean(dr["LiveIndicator"]);
        }
        else
            TemplateHandler.Assign(ref tpl, "errmsg", "Invalid TransId!");
        //test
        //IsLive = true;
        //HDP_OU = "";
        if (IsLive)
        {
            TemplateHandler.Assign(ref tpl, "Live", "1");
        }
        else
        {
            TemplateHandler.Assign(ref tpl, "Live", "0");
            TemplateHandler.RemoveBlock(ref tpl, "Live");
        }
        TemplateHandler.Assign(ref tpl, "username", UserName);
        TemplateHandler.Assign(ref tpl, "ShowTime", KOT);
        TemplateHandler.Assign(ref tpl, "Event", Event);
        TemplateHandler.Assign(ref tpl, "Team", strChoice);
        TemplateHandler.Assign(ref tpl, "CreatorBy", EditedBy);

        TemplateHandler.Assign(ref tpl, "Hdp-val", HDP_OU);
        TemplateHandler.Assign(ref tpl, "HomeScore-val", LiveHomeCore);
        TemplateHandler.Assign(ref tpl, "AwayScore-val", LiveAwayScore);
        TemplateHandler.Assign(ref tpl, "Price-val", Price);
        TemplateHandler.Assign(ref tpl, "Amount-val", Amount);
        TemplateHandler.Assign(ref tpl, "Remark-val", Remark);

        TemplateHandler.Assign(ref tpl, "hdp1", LoadHDP(hdp1, 0, 5, CastAide.AsDecimal(0.25), "ddlHDP1"));
        TemplateHandler.Assign(ref tpl, "hdp2", LoadHDP(hdp2, 0, 5, CastAide.AsDecimal(0.25), "ddlHDP2"));
        TemplateHandler.Assign(ref tpl, "TransID", TransID);
        content.Text = tpl;
    }

    string LoadHDP(decimal curHDP, decimal minValue, decimal maxValue, decimal speed, string Id)
    {
        string str = "";
        str += "<select id='"+ Id +"' name='"+ Id +"'>";
        for (decimal i = minValue; i <= maxValue; i = i + speed)
        {
            if(curHDP == i)
                str += "<option selected='selected' value='" + i + "'>" + NumberFormat.DecFormat(i, 2) + "</option>";
            else
                str += "<option value='"+ i +"'>"+ NumberFormat.DecFormat(i,2) +"</option>";
        }
        str += "</select>";
        return str;
    }
}
