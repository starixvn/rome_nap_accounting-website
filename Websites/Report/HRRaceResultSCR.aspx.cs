using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Web;
using DNAide.Core;
using DNAide.Data;
using System.Data;
using Sunplus.Accounting.Core.Cache;

public partial class Report_HRRaceResultSCR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int leagueid = CastAide.AsInt32(Request.QueryString["leagueid"], 0); ;
        string raceno = CastAide.AsString(Request.QueryString["raceno"], string.Empty);
        string eventdate = CastAide.AsString(Request.QueryString["eventdate"], string.Empty);
        lHRRaceResult.Text = GenHTMLHRRaceResult(leagueid, raceno, eventdate);
    }

    private string GenHTMLHRRaceResult(int leagueid, string raceno, string eventdate)
    {
        DbResult dbRaceResult = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.HRRaceResultSCR(leagueid, raceno, eventdate);            
        // for SCR Info
        int totalPlace = 1;
        string htmlDataSCR = "";
        string tplRaceResultSCR = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/HRRaceResultSCR.htm");
        TemplateHandler.Assign(ref tplRaceResultSCR, "SCRInfo", "Racing: " + CacheRef.GetLeagueName(leagueid));
        TemplateHandler.Assign(ref tplRaceResultSCR, "lblRace", Resources.Language.Race);
        TemplateHandler.Assign(ref tplRaceResultSCR, "lblHorseNo", Resources.Language.HorseNo);
        DataRowCollection dtRowSCR = dbRaceResult.ResultSet.Tables[0].Rows;
        if (dtRowSCR.Count > 0)
        {                
            string tplDataSCR = TemplateHandler.CopyBlock(tplRaceResultSCR, "data");                
            foreach (DataRow row in dtRowSCR)
            {
                totalPlace++;
                string tmpDataSCR = tplDataSCR;
                TemplateHandler.Assign(ref tmpDataSCR, "vHorseno", row["matchcode"] + ". " + CacheRef.GetHorseTeamName(row["teamid"]));
                TemplateHandler.Assign(ref tmpDataSCR, "vTime", CastAide.AsString(row["SCRTime"])==""?"": CastAide.AsString(CastAide.AsDateTime(row["SCRTime"]).TimeOfDay));
                TemplateHandler.Assign(ref tmpDataSCR, "vDeduct", CastAide.AsString(CastAide.AsInt16(row["Deduction"])) + "%");
                htmlDataSCR += tmpDataSCR;
            }
            TemplateHandler.Assign(ref tplRaceResultSCR, "racename", raceno);
            TemplateHandler.Assign(ref tplRaceResultSCR, "totalplace", totalPlace);
            TemplateHandler.ReplaceBlock(ref tplRaceResultSCR, "data", htmlDataSCR);
        }
        else
        {
            htmlDataSCR = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.tpl");            
            TemplateHandler.Assign(ref htmlDataSCR, "tdno", "4");
            TemplateHandler.Assign(ref htmlDataSCR, "nodata", Resources.Language.thereisnodata);
            TemplateHandler.RemoveBlock(ref tplRaceResultSCR, "race");
            TemplateHandler.ReplaceBlock(ref tplRaceResultSCR, "data", htmlDataSCR);
        }
        return tplRaceResultSCR;
    }
}
