using System;
using System.Collections;
using DNAide.Core;
using DNAide.Data;
using BLL.Report;
using BLL.Report.LeagueWL;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;


/// <summary>
/// CITS
/// </summary>
public partial class DailyWL_Sporttype : PageBase
{

    public string sContent = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        ReportBase report = new LeagueWLSporttype (); 
        
        string fdate = Request["fdate"];
        fdate = ReportHTML.ValidDate(fdate);

        string path = "<span id='spPath'> <b>Main</b> </span>";        
        string db = Request["db"];

        Hashtable hash = new Hashtable();
        hash.Add("db", db);
        hash.Add("path", path);
        hash.Add("wldate", fdate);



        if (Request["IsExport"] == "1")
        {
            string type = Request["type"];
            switch(type)
            {
                case "1":
                    report.Export2Excel(hash);
                    break;
                case "2":
                    hash.Add("sporttype", Request["sporttype"]);
                    hash.Add("leagueid", Request["leagueid"]);
                    report = new LeagueWLBettype();
                    report.Export2Excel(hash);
                    break;
                case "3":
                    hash.Add("sporttype", Request["sporttype"]);
                    hash.Add("leagueid", Request["leagueid"]);
                    hash.Add("bettype", Request["bettype"]);
                    hash.Add("liveindicator", Request["liveindicator"]);
                    hash.Add("IsHedge", Request["IsHedge"]);

                    //report = new LeagueWLDetails_Linq();
                    report = new LeagueWLDetails();
                    report.Export2Excel(hash);
                    break;
            }
            
            
        }
        else
        {
            headerDatetime.Text = report.CreateHeader(hash);
            sContent= report.RenderHTML(hash).Replace("{border}", "0");
        }
    }

    

    

    
}
