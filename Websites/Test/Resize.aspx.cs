using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL.Report;
using DNAide.Data;
using DNAide.Utils;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.PT.Racing;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.AdoNet;
using Sunplus.Core.LoggingLib;

using System.Text.RegularExpressions;
using Sunplus.Bingo.Core;
using Sunplus.Bingo.Entities;

using Sunplus.Accounting.Services;

public partial class ExcelData1234 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //StoreProcedureSection.Instance.
      try
      {
          Convert.ToInt16("hell");
      }
        catch(Exception ex)
        {
            string message = ex.Message +
                             "<br>\r\nSOURCE: " + ex.Source +
                             "<br>\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "<br>\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                             "<br>\r\nFORM: " + HttpContext.Current.Request.Form.ToString() +
                             "<br>\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString.ToString() +
                             "<br>\r\nTARGETSITE: " + ex.TargetSite +
                             "<br>\r\nSTACKTRACE: " + ex.StackTrace;

            Response.Write(message);
        }

    }

    protected void Export2ExcelDailyWLDirectRacing(string winlossDate)
    {
        new Sunplus.Accounting.WcfServiceLibrary.Report.DailyWL.ReportDailyWL().Export2ExcelDailyWLDirectRacing(winlossDate);
    }

    protected void Export2ExcelDailyWLDirectRacingBySport(string sporttype, string winlossDate)
    {
        new Sunplus.Accounting.WcfServiceLibrary.Report.DailyWL.ReportDailyWL().Export2ExcelDailyWLDirectRacingBySport(sporttype, winlossDate);
    }

    protected void Export2ExcelDailyWLDirectRacingBySportDetails(string bettype, string winlossDate)
    {
      //  new Sunplus.Accounting.WcfServiceLibrary.Report.DailyWL.ReportDailyWL().Export2ExcelDailyWLDirectRacingBySportDetails(bettype, winlossDate);
    }
}
