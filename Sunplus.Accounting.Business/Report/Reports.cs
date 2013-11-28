using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.Report;

namespace Sunplus.Accounting.Business.Report
{
    /// <summary>
    /// Summary description for ReRports
    /// </summary>
    public class Reports
    {
        public Reports()
        {
        }

        public static string Title
        {
            get { return "Accounting - Company Report"; }
        }

        /// <summary>
        /// [2HS]-1
        /// </summary>
        public const string Report2H_Soccer = "[Report2HS]-1";

        /// <summary>
        /// [2HO]-1
        /// </summary>
        public const string Report2H_Other = "[Report2HO]-1";

        #region string colorvalue(object value)

        public static string colorvalue(object value)
        {
            string str = "";
            if (FormatHandler.GetDouble(value) > 0) str = "<font color ='red'>" + FormatHandler.FormatNumber20(value) + "</font>";
            else
            {
                str = FormatHandler.FormatNumber20(value);
            }
            return str;
        }

        public static string colorvalue(object value, string SpecialText)
        {
            string str = "";
            if (FormatHandler.GetDouble(value) > 0) str = "<font color ='red'>" + FormatHandler.FormatNumber20(value) + SpecialText + "</font>";
            else
            {
                str = FormatHandler.FormatNumber20(value) + SpecialText;
            }
            return str;
        }

        #region colorvalueCreditTerm(double value)

        public static string colorvalueCreditTerm(double value)
        {
            if (value >= 0) return "<font color='blue'>" + FormatHandler.FormatNumber20(value) + "</font>";
            else return "<font color='red'>" + FormatHandler.FormatNumber20(value) + "</font>";
        }

        #endregion colorvalueCreditTerm(double value)

        #endregion string colorvalue(object value)

        #region void genrow(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double Turnover)

        public static void genrow(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double Turnover, bool IsBlankCell)
        {
            TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
            TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
            TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", colorvalue(WinLost));
            TemplateHandler.Assign(ref totalRowSportType, "{Position}", FormatHandler.FormatNumber20(Position));
            TemplateHandler.Assign(ref totalRowSportType, "{Comm}", FormatHandler.FormatNumber20(Comm));
            double l = WinLost + Position + Comm;
            double margin = (Turnover == 0) ? 0 : ((l / Turnover) * 100);
            TemplateHandler.Assign(ref totalRowSportType, "{Margin}", Reports.colorvalue(FormatHandler.FormatNumber20(margin), "%"));
            TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", colorvalue(l));
            if (IsBlankCell)
            {
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{blankcell}", "blankcell");
            }
            else
            {
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", FormatHandler.FormatNumber20(Turnover));
                TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", colorvalue(Turnover));
                TemplateHandler.Assign(ref totalRowSportType, "{blankcell}", "");
            }
            if (BetType.Equals("8RM")) TemplateHandler.Assign(ref totalRowSportType, "{align}", "c");
            else TemplateHandler.Assign(ref totalRowSportType, "{align}", "l");
        }

        public static void genrow(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double linetotal, double Turnover, bool IsBlankCell)
        {
            TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
            TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
            TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", colorvalue(WinLost));
            TemplateHandler.Assign(ref totalRowSportType, "{Position}", FormatHandler.FormatNumber20(Position));
            TemplateHandler.Assign(ref totalRowSportType, "{Comm}", FormatHandler.FormatNumber20(Comm));
            double l = linetotal;
            double margin = (Turnover == 0) ? 0 : ((l / Turnover) * 100);
            TemplateHandler.Assign(ref totalRowSportType, "{Margin}", Reports.colorvalue(FormatHandler.FormatNumber20(margin), "%"));
            TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", colorvalue(l));
            if (IsBlankCell)
            {
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{blankcell}", "blankcell");
            }
            else
            {
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", FormatHandler.FormatNumber20(Turnover));
                TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", colorvalue(Turnover));
                TemplateHandler.Assign(ref totalRowSportType, "{blankcell}", "");
            }
            if (BetType.Equals("8RM")) TemplateHandler.Assign(ref totalRowSportType, "{align}", "c");
            else TemplateHandler.Assign(ref totalRowSportType, "{align}", "l");
        }

        #endregion void genrow(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double Turnover)

        #region public static void genrow(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double Turnover)

        public static void genrow(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double Turnover)
        {
            TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
            TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
            TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", colorvalue(WinLost));
            TemplateHandler.Assign(ref totalRowSportType, "{Position}", FormatHandler.FormatNumber20(Position));
            TemplateHandler.Assign(ref totalRowSportType, "{Comm}", FormatHandler.FormatNumber20(Comm));
            double l = WinLost + Position + Comm;
            double margin = (Turnover == 0) ? 0 : ((l / Turnover) * 100);
            TemplateHandler.Assign(ref totalRowSportType, "{Margin}", Reports.colorvalue(FormatHandler.FormatNumber20(margin), "%"));
            TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", colorvalue(l));
            TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", FormatHandler.FormatNumber20(Turnover));
            TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", colorvalue(Turnover));
            TemplateHandler.Assign(ref totalRowSportType, "{bgcolor}", "");
            if (BetType.Equals("8RM") || BetType.Equals("BARM")) TemplateHandler.Assign(ref totalRowSportType, "{align}", "c");
            else TemplateHandler.Assign(ref totalRowSportType, "{align}", "l");
        }

        public static void genrow2(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double Turnover)
        {
            TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
            TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
            TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", colorvalue(WinLost));
            TemplateHandler.Assign(ref totalRowSportType, "{Position}", colorvalue(Position));
            TemplateHandler.Assign(ref totalRowSportType, "{Comm}", colorvalue(Comm));
            double l = WinLost + Position + Comm;
            double margin = (Turnover == 0) ? 0 : ((l / Turnover) * 100);
            TemplateHandler.Assign(ref totalRowSportType, "{Margin}", Reports.colorvalue(FormatHandler.FormatNumber20(margin), "%"));
            TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", colorvalue(l));
            TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", colorvalue(Turnover));
            TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", colorvalue(Turnover));
            TemplateHandler.Assign(ref totalRowSportType, "{bgcolor}", "");
            if (BetType.Equals("8RM") || BetType.Equals("BARM")) TemplateHandler.Assign(ref totalRowSportType, "{align}", "c");
            else TemplateHandler.Assign(ref totalRowSportType, "{align}", "l");
        }

        public static void genrow2h(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double LineTotal, double Turnover, bool Is8RM)
        {
            if (Is8RM)
            {
                TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
                TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
                TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{Position}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{Comm}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{Margin}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", colorvalue(LineTotal));
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{bgcolor}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{align}", "c");
            }
            else
            {
                TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
                TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
                TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", colorvalue(WinLost));
                TemplateHandler.Assign(ref totalRowSportType, "{Position}", FormatHandler.FormatNumber20(Position));
                TemplateHandler.Assign(ref totalRowSportType, "{Comm}", FormatHandler.FormatNumber20(Comm));
                double margin = (Turnover == 0) ? 0 : ((LineTotal / Turnover) * 100);
                TemplateHandler.Assign(ref totalRowSportType, "{Margin}", Reports.colorvalue(FormatHandler.FormatNumber20(margin), "%"));
                TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", colorvalue(LineTotal));
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", FormatHandler.FormatNumber20(Turnover));
                TemplateHandler.Assign(ref totalRowSportType, "{bgcolor}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{align}", "l");
            }
        }

        #endregion public static void genrow(ref string totalRowSportType, string color, string BetType, double WinLost, double Position, double Comm, double Turnover)

        #region void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line)

        public static void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line)
        {
            TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
            TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
            TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", (WinLost));
            TemplateHandler.Assign(ref totalRowSportType, "{Position}", (Position));
            TemplateHandler.Assign(ref totalRowSportType, "{Comm}", (Comm));
            TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", (Line));
            TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", (Turnover));
            TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", (Turnover));
        }

        #endregion void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line)

        #region void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line)

        public static void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line, string Margin)
        {
            TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
            TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
            TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", (WinLost));
            TemplateHandler.Assign(ref totalRowSportType, "{Position}", (Position));
            TemplateHandler.Assign(ref totalRowSportType, "{Comm}", (Comm));
            TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", (Line));
            TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", (Turnover));
            TemplateHandler.Assign(ref totalRowSportType, "{Margin}", (Margin));
            TemplateHandler.Assign(ref totalRowSportType, "{Admin2}", (Turnover));
        }

        #endregion void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line)

        #region public static void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line, bool IsBlankCell)

        public static void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line, bool IsBlankCell)
        {
            TemplateHandler.Assign(ref totalRowSportType, "{color}", color);
            TemplateHandler.Assign(ref totalRowSportType, "{Bettype}", BetType);
            TemplateHandler.Assign(ref totalRowSportType, "{WinLost}", (WinLost));
            TemplateHandler.Assign(ref totalRowSportType, "{Position}", (Position));
            TemplateHandler.Assign(ref totalRowSportType, "{Comm}", (Comm));
            TemplateHandler.Assign(ref totalRowSportType, "{LineTotal}", (Line));

            if (IsBlankCell)
            {
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", "");
                TemplateHandler.Assign(ref totalRowSportType, "{blankcell}", "blankcell");
            }
            else
            {
                TemplateHandler.Assign(ref totalRowSportType, "{Turnover}", colorvalue(Turnover));
                TemplateHandler.Assign(ref totalRowSportType, "{blankcell}", "");
            }
        }

        #endregion public static void genrow(ref string totalRowSportType, string color, string BetType, string WinLost, string Position, string Comm, string Turnover, string Line, bool IsBlankCell)

        #region GetHeader

        public static string GetHeader(string strfdate, string strtdate, string path, bool IsGetData)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
                string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/DailyWL_Header.htm");
                DateTime fdate = DateTime.Today, tdate = DateTime.Today;
                if (!Validator.ValidDateMMDDYYYY(strfdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(strtdate, out tdate).IsSuccess())
                {
                    html.Append("</table>");
                    TemplateHandler.Assign(ref norow, "{nodata}", (IbcError.DATE_INVALID_FORMAT_MSG));
                    html.Append(norow);
                    html.Append("</table>");
                    return html.ToString();
                }

                TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", fdate.ToShortDateString(), false, false, 3, 0, ""));//Phuong.Nguyen <2009-01-03>, show other days of next month
                TemplateHandler.Assign(ref tpl, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../", "tdateTrigger", tdate.ToShortDateString(), true, false, 3, 0, "")); //END
                TemplateHandler.Assign(ref tpl, "{path}", path);

                if (!IsGetData)
                    TemplateHandler.RemoveBlock(ref tpl, "GetData");
                else
                {
                    if (ReportUtils.IsRepDB)
                    {
                        TemplateHandler.Assign(ref tpl, "{status-main}", string.Empty);
                        TemplateHandler.Assign(ref tpl, "{status-rep}", "checked='checked'");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref tpl, "{status-main}", "checked='checked'");
                        TemplateHandler.Assign(ref tpl, "{status-rep}", string.Empty);
                    }
                }

                html.Append(tpl);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return string.Empty;
            }
            return html.ToString();
        }

        public static string GetDateTimeHeader(string strFdate, string strTdate)
        {
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeHeader.htm");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", strFdate, false, false, 3, 0, ""));//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../", "tdateTrigger", strTdate, true, false, 3, 0, ""));   //END
            return tpl;
        }

        public static string GetDateTimeSingle(string strFdate, string TextValue, string TitleReport)
        {
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSingle.html");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", strFdate, false, false, 3, 0, ""));//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);

            return tpl;
        }

        public static string GetDateTimeSingle(string strFdate, string TextValue, string TitleReport, int displaydays)
        {
            int displayMonths = 0;
            if (DateTime.Today.AddDays(-displaydays).Month != DateTime.Today.Month)
            {
                displayMonths = 1;
            }
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSingle.html");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", strFdate, false, false, displayMonths, displaydays, string.Empty));//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);

            return tpl;
        }

        public static string GetDateTimeSingle(string strFdate, string TextValue, string TitleReport, int displaydays, string path, string sitemap, string eventfunction)
        {
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSinglePath.html");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", "<input type=\"text\" id=\"fdate\" readonly=\"readonly\" class=\"txtdate\" style=\"width: 100px; background-color: white;\" />");//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);
            TemplateHandler.Assign(ref tpl, "{path}", sitemap);
            TemplateHandler.Assign(ref tpl, "{MoveBetsDay}", ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString());

            return tpl;
        }

        public static string GetDateTimeSingleV2(string strFdate, string TextValue, string TitleReport, int displaydays, string path, string sitemap)
        {
            int displayMonths = 0;

            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSinglePath.html");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", path, "fdateTrigger", strFdate, false, false, displayMonths, displaydays, ""));//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);
            TemplateHandler.Assign(ref tpl, "{path}", sitemap);
            TemplateHandler.Assign(ref tpl, "{MoveBetsDay}", ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString());

            return tpl;
        }

        public static string GetDateTimeSingleMaxDays(string strFdate, string TextValue, string TitleReport, int displaydays, int maxdays, string path, string sitemap, string eventfunction)
        {
          
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSinglePath.html");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", "<input type=\"text\" id=\"fdate\" readonly=\"readonly\" class=\"txtdate\" style=\"width: 100px; background-color: white;\" />");//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);
            TemplateHandler.Assign(ref tpl, "{path}", sitemap);
            TemplateHandler.Assign(ref tpl, "{MoveBetsDay}", ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString());

            return tpl;
        }

        public static string GetDateTimeSingleMaxDays_2h(string strFdate, string TextValue, string TitleReport, int displaydays, int maxdays, string path, string sitemap, string eventfunction)
        {
            int displayMonths = 0;

            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSinglePath_2h.htm");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControlMaxDays("fdate", path, "fdateTrigger", strFdate, false, false, displayMonths, displaydays, maxdays, eventfunction, "blue", ""));//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);
            TemplateHandler.Assign(ref tpl, "{path}", sitemap);
            TemplateHandler.Assign(ref tpl, "{MoveBetsDay}", ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString());

            return tpl;
        }

        public static string GetDateTimeSingleWithoutGetDataFromRegion(string strFdate, string TextValue, string TitleReport, int displaydays, string path, string sitemap, string eventfunction)
        {
            int displayMonths = 0;

            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSinglePath.html");
            TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", path, "fdateTrigger", strFdate, false, false, displayMonths, displaydays, eventfunction, "blue"));//Phuong.Nguyen@Updated: <2009-01-03>, show others day of next month
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);
            TemplateHandler.Assign(ref tpl, "{path}", sitemap);
            TemplateHandler.Assign(ref tpl, "{MoveBetsDay}", ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString());

            TemplateHandler.RemoveBlock(ref tpl, "getdata");

            return tpl;
        }

        public static string GetDateTimeSingleDropDownBox(string strFdate, string TextValue, string TitleReport, int displaydays, string path, string sitemap, string eventfunction)
        {
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/DateTimeSinglePath.html");

            StringBuilder html = new StringBuilder();
            html.Append("<select " + eventfunction + " id='fdate' name='fdate'>");

            string sday = "", sbgcolor = "";
            for (int i = 0; i <= displaydays; i++)
            {
                sday = DateTime.Today.AddDays(-i).DayOfWeek.ToString() + " - ";

                if (i == 0)
                {
                    sday += "Today";
                    sbgcolor = "style='background:#def7fe'";//bdeffd
                }
                else if (i == 1)
                {
                    sday += "Yesterday";
                    sbgcolor = "style='background:#def7fe'";
                }
                else
                {
                    sday += DateTime.Today.AddDays(-i).ToShortDateString();
                    sbgcolor = "";
                }

                if (strFdate == DateTime.Today.AddDays(-i).ToShortDateString())
                    html.Append("<option " + sbgcolor + " selected='selected' value='" + DateTime.Today.AddDays(-i).ToShortDateString() + "'>" + sday + "</option>");
                else
                    html.Append("<option " + sbgcolor + " value='" + DateTime.Today.AddDays(-i).ToShortDateString() + "'>" + sday + "</option>");
            }

            html.Append("</select>");

            TemplateHandler.Assign(ref tpl, "{FromDateCal}", html.ToString());
            TemplateHandler.Assign(ref tpl, "{Text-value}", TextValue);
            TemplateHandler.Assign(ref tpl, "{Title-Report}", TitleReport);
            TemplateHandler.Assign(ref tpl, "{path}", sitemap);
            TemplateHandler.Assign(ref tpl, "{MoveBetsDay}", ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString());

            return tpl;
        }

        public static string GetHeader1(string strfdate, string strtdate, string path)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
                string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/DateTimeHeader.htm");
                DateTime fdate = DateTime.Today, tdate = DateTime.Today;
                if (!Validator.ValidDateMMDDYYYY(strfdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(strtdate, out tdate).IsSuccess())
                {
                    html.Append("</table>");
                    TemplateHandler.Assign(ref norow, "{nodata}", (IbcError.DATE_INVALID_FORMAT_MSG));
                    html.Append(norow);
                    html.Append("</table>");
                    return html.ToString();
                }

                TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../" + path, "fdateTrigger", fdate.ToShortDateString(), false, false, 0, 0, "")); //PHuong.Nguyen <2009-01-03>
                TemplateHandler.Assign(ref tpl, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../" + path, "tdateTrigger", tdate.ToShortDateString(), true, false, 0, 0, ""));   //END
                TemplateHandler.Assign(ref tpl, "{path}", path);

                html.Append(tpl);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return string.Empty;
            }
            return html.ToString();
        }

        public static string GetHeader1(string strfdate, string strtdate, string path, int displaydays, string eventfunction)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
                string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/DateTimeHeader.htm");
                DateTime fdate = DateTime.Today, tdate = DateTime.Today;
                if (!Validator.ValidDateMMDDYYYY(strfdate, out fdate).IsSuccess() || !Validator.ValidDateMMDDYYYY(strtdate, out tdate).IsSuccess())
                {
                    html.Append("</table>");
                    TemplateHandler.Assign(ref norow, "{nodata}", (IbcError.DATE_INVALID_FORMAT_MSG));
                    html.Append(norow);
                    html.Append("</table>");
                    return html.ToString();
                }
                int displayMonths = 0;
                if (DateTime.Today.AddDays(-displaydays).Month != DateTime.Today.Month)
                {
                    displayMonths = 1;
                }
                TemplateHandler.Assign(ref tpl, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../" + path, "fdateTrigger", fdate.ToShortDateString(), false, false, displayMonths, displaydays, eventfunction)); //PHuong.Nguyen <2009-01-03>
                TemplateHandler.Assign(ref tpl, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../" + path, "tdateTrigger", tdate.ToShortDateString(), true, false, displayMonths, displaydays, eventfunction));   //END
                TemplateHandler.Assign(ref tpl, "{path}", path);

                html.Append(tpl);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return string.Empty;
            }
            return html.ToString();
        }

        #endregion GetHeader

        #region ShowFullPath

        public static string GetPath(string path)
        {
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Hidden.htm");
            TemplateHandler.Assign(ref tpl, "{name}", "path");
            TemplateHandler.Assign(ref tpl, "{strValue}", path);
            return tpl;
        }

        #endregion ShowFullPath

        #region public static string RemoveBlock(ref string tpl, string[] arrBlocks)

        public static string RemoveBlock(ref string tpl, string[] arrBlocks)
        {
            if (string.IsNullOrEmpty(tpl)) return string.Empty;
            foreach (string block in arrBlocks)
            {
                TemplateHandler.RemoveBlock(ref tpl, block);
            }
            return tpl;
        }

        #endregion public static string RemoveBlock(ref string tpl, string[] arrBlocks)

        #region public static string RemoveTag(ref string tpl, string[] arrBlocks)

        public static string RemoveTag(ref string tpl, string[] arrTags)
        {
            if (string.IsNullOrEmpty(tpl)) return string.Empty;
            foreach (string tag in arrTags)
            {
                TemplateHandler.Assign(ref tpl, tag, "");
            }
            return tpl;
        }

        #endregion public static string RemoveTag(ref string tpl, string[] arrBlocks)

        #region public static bool IsUserNameDeny(object CurrentUser)

        public static bool IsUserNameDeny(object CurrentUser)
        {
            string[] ArrUserDenies = { "SS33", "SSJOE", "SSMICHAEL", "BChai", "SSCK", "SSMK", "SSRAYMOND", "SSYEN", "SSFATTCHAI", "SSXING", "LAI" };
            bool IsDenyUser = false;
            if (CurrentUser == null) return IsDenyUser;
            foreach (string UserDeny in ArrUserDenies)
            {
                if (CurrentUser.ToString().ToLower().Equals(UserDeny.ToLower()))
                { IsDenyUser = true; break; }
            }
            return IsDenyUser;
        }

        #endregion public static bool IsUserNameDeny(object CurrentUser)

        #region public static bool IsFileNameDeny(CurrentFile)

        public static bool IsFileNameDeny(string CurrentFile)
        {
            string[] ArrFiles = { "../Report/MatchWinLoss2.aspx", "../Report/MatchWinLoss2_14.aspx", "../Report/MatchWinLoss2V2.aspx", "../Report/MatchWinLoss2_14V2.aspx" };
            bool IsDeny = false;
            foreach (string FileDeny in ArrFiles)
            {
                if (FileDeny.ToLower().Equals(CurrentFile.ToLower()))
                { IsDeny = true; break; }
            }
            return IsDeny;
        }

        #endregion public static bool IsFileNameDeny(CurrentFile)

        #region public static void RemoveCharacterNewLine(ref string str)

        public static void RemoveCharacterNewLine(ref string str)
        {
            str = str.Replace(Environment.NewLine, "");
        }

        #endregion public static void RemoveCharacterNewLine(ref string str)

        #region GetStatusDBType

        public static bool GetStatusDBType(ref string currdbtype)
        {
            bool IsMainDB = true;
            string type = HttpContext.Current.Request["dbtype"]; //that is a button submit for get data with value as "Use Main DB od Use Report DB"
            currdbtype = HttpContext.Current.Request["currdbtype"];// that is a hidden field for hold status current value as "dbreport od dbmain"

            currdbtype = (string.IsNullOrEmpty(currdbtype)) ? "dbreport" : currdbtype;
            if (!string.IsNullOrEmpty(type))
            {
                if (type.IndexOf("Main") > 0) currdbtype = "dbmain";
                else { currdbtype = "dbreport"; IsMainDB = false; }
            }
            else
            {
                IsMainDB = (currdbtype.Equals("dbmain")) ? true : false;
            }
            return IsMainDB;
        }

        #endregion GetStatusDBType

        #region string datevalue(object value)

        public static string datevalue(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            int mm = date.Month;
            int dd = date.Day;
            int hh = date.Hour;
            int mn = date.Minute;
            return mm.ToString() + "/" + dd.ToString() + " " + hh.ToString() + ":" + mn.ToString();
        }

        #endregion string datevalue(object value)

        #region GetCreditTerm()

        public static string GetCreditTerm_1()
        {
            StringBuilder html = new StringBuilder();
            string valueSearch = HttpContext.Current.Request.Form["txtValue"];

            string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
            TemplateHandler.Assign(ref norow, "{tdno}", "10");
            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");

            try
            {
                string tplHeader = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CreditTeam/CreditTeamAllHeader.htm");
                string tplHeaderParent = tplHeader.Clone().ToString();
                TemplateHandler.Assign(ref tplHeaderParent, "{role}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Role"));
                TemplateHandler.Assign(ref tplHeaderParent, "{username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UserName"));
                TemplateHandler.Assign(ref tplHeaderParent, "{curr}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Curr"));
                TemplateHandler.Assign(ref tplHeaderParent, "{pre}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Pre"));
                TemplateHandler.Assign(ref tplHeaderParent, "{balance}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Balance"));
                TemplateHandler.Assign(ref tplHeaderParent, "{basecurrency}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Base"));
                TemplateHandler.Assign(ref tplHeaderParent, "{remark}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Remark"));
                TemplateHandler.Assign(ref tplHeaderParent, "{Phone}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Phone"));
                TemplateHandler.Assign(ref tplHeaderParent, "{Checker}", "Checker");
                TemplateHandler.Assign(ref tplHeaderParent, "{time}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "RemarkTime"));

                TemplateHandler.Assign(ref tplHeaderParent, "{widthrole}", "2%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthusername}", "11%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthcurr}", "3%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthpre}", "11%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthbalance}", "10%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthbase}", "10%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthphone}", "15%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthchecker}", "5%");
                TemplateHandler.Assign(ref tplHeaderParent, "{widthtime}", "6%");

                TemplateHandler.RemoveBlock(ref tplHeaderParent, "edit");

                html.Append(tplHeaderParent);

                Hashtable hash = new Hashtable();
                hash.Add("groupid", 0);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request["checkname"]))
                    hash.Add("checkername", HttpContext.Current.Request["checkname"]);

                if (!string.IsNullOrEmpty(HttpContext.Current.Request["pre1"]))
                    hash.Add("pre1", CastAide.AsInt32(HttpContext.Current.Request["pre1"], 0));

                if (!string.IsNullOrEmpty(HttpContext.Current.Request["pre2"]))
                    hash.Add("pre2", CastAide.AsInt32(HttpContext.Current.Request["pre2"], 0));

                DbResult rs = AccountReport.Acc_Rpt_CreditTerm(hash);
                DataTable dt = new DataTable();
                DataTable dtDetail = new DataTable();
                string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CreditTeam/CreditTeamAllRow.htm");
                string rowsChild = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CreditTeam/CreditTeamChildlRow.htm");

                // Export To Excel
                if (CastAide.AsString(HttpContext.Current.Request["btnExport"]) != "")
                    TemplateHandler.RemoveBlock(ref rows, "no_excel");
                string rowsParent = rows.Clone().ToString();

                TemplateHandler.RemoveBlock(ref rowsParent, "edit");

                if (rs.ResultSet.Tables.Count > 0 && rs.ResultSet.Tables[0].Rows.Count > 0)
                {
                    dt = rs.ResultSet.Tables[0];
                    dtDetail = rs.ResultSet.Tables[1];

                    string odd_even = "";
                    bool IsOverLimit = false;

                    foreach (DataRow dr in dt.Rows)
                    {
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        string row = rowsParent.Clone().ToString();
                        string rowChild = rowsChild.Clone().ToString();
                        IsOverLimit = Convert.ToBoolean(dr["OverLimit"]);
                        {
                            int groupid = Convert.ToInt32(dr["groupID"]);
                            if (IsOverLimit)
                            {
                                if (groupid == 0) odd_even = "oddrow";
                                else odd_even = "evenrow";
                            }
                            else
                            {
                                odd_even = "limit-header";
                            }

                            string curr = CacheUtils.GetExchangeName(DNAide.Core.CastAide.AsInt32((dr["currency"])));
                            TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                            TemplateHandler.Assign(ref row, "{role}", Convert.ToString(dr["role"]));
                            TemplateHandler.Assign(ref row, "{username}", "<b>" + Convert.ToString(dr["username"]) + "</b>");
                            TemplateHandler.Assign(ref row, "{curr}", curr);
                            TemplateHandler.Assign(ref row, "{pre}", colorvalue(FormatHandler.GetDouble(dr["PreAgreeAmount"])));
                            TemplateHandler.Assign(ref row, "{balance}", colorvalue(FormatHandler.GetDouble(dr["Balance"])));
                            TemplateHandler.Assign(ref row, "{basecurrency}", colorvalue(FormatHandler.GetDouble(dr["Balance_Base"])));
                            TemplateHandler.Assign(ref row, "{remark}", Convert.ToString(dr["AccountRemark"]));
                            TemplateHandler.Assign(ref row, "{time}", datevalue(dr["AccountRemarkDate"]));
                            TemplateHandler.Assign(ref row, "{width}", "400");
                            TemplateHandler.Assign(ref row, "{Phone}", dr["phone"].ToString());
                            TemplateHandler.Assign(ref row, "{Checker}", dr["CheckerName"].ToString());

                            html.Append(row);

                            if (groupid > 0)
                            {
                                DataRow[] drChilds = dtDetail.Select("groupid=" + groupid);
                                StringBuilder htmlChild = new StringBuilder();

                                string tplHeaderChild = tplHeader.Clone().ToString();
                                TemplateHandler.Assign(ref tplHeaderChild, "{role}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Role"));
                                TemplateHandler.Assign(ref tplHeaderChild, "{username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UserName"));
                                TemplateHandler.Assign(ref tplHeaderChild, "{curr}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Curr"));
                                TemplateHandler.Assign(ref tplHeaderChild, "{pre}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Balance"));
                                TemplateHandler.Assign(ref tplHeaderChild, "{balance}", curr);
                                TemplateHandler.Assign(ref tplHeaderChild, "{basecurrency}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "RM"));
                                TemplateHandler.Assign(ref tplHeaderChild, "{remark}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Remark"));
                                TemplateHandler.Assign(ref tplHeaderChild, "{Phone}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Phone"));
                                TemplateHandler.Assign(ref tplHeaderChild, "{Checker}", "Checker");
                                TemplateHandler.Assign(ref tplHeaderChild, "{time}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "RemarkTime"));

                                TemplateHandler.Assign(ref tplHeaderChild, "{widthrole}", "5%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthusername}", "6%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthcurr}", "3%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthpre}", "12%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthbalance}", "10%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthbase}", "10%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthphone}", "15%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthchecker}", "5%");
                                TemplateHandler.Assign(ref tplHeaderChild, "{widthtime}", "6%");

                                string rowsChildNested = rows.Clone().ToString();
                                TemplateHandler.RemoveBlock(ref tplHeaderChild, "phone-checker");
                                TemplateHandler.RemoveBlock(ref rowsChildNested, "phone-checker");

                                htmlChild.Append(tplHeaderChild);

                                foreach (DataRow drChild in drChilds)
                                {
                                    string rownew = rowsChildNested.Clone().ToString();
                                    if (!IsOverLimit)
                                        TemplateHandler.Assign(ref rownew, "{odd_even}", "limit");

                                    TemplateHandler.Assign(ref rownew, "{custid}", Convert.ToString(drChild["custid"]));
                                    TemplateHandler.Assign(ref rownew, "{role}", Convert.ToString(drChild["role"]));
                                    TemplateHandler.Assign(ref rownew, "{username}", Convert.ToString(drChild["username"]));
                                    string currChild = CacheUtils.GetExchangeName(DNAide.Core.CastAide.AsInt32((drChild["currency"])));
                                    TemplateHandler.Assign(ref rownew, "{curr}", currChild);
                                    string balance = colorvalue(FormatHandler.GetDouble(drChild["balance"]));
                                    if (curr.Equals(currChild)) balance = "";
                                    TemplateHandler.Assign(ref rownew, "{pre}", balance);
                                    TemplateHandler.Assign(ref rownew, "{balance}", colorvalue(FormatHandler.GetDouble(drChild["balance_Group"])));
                                    TemplateHandler.Assign(ref rownew, "{basecurrency}", colorvalue(FormatHandler.GetDouble(drChild["Balance_Base"])));
                                    TemplateHandler.Assign(ref rownew, "{remark}", Convert.ToString(drChild["AccountRemark"]));
                                    TemplateHandler.Assign(ref rownew, "{time}", datevalue(drChild["AccountRemarkDate"]));
                                    TemplateHandler.Assign(ref rownew, "{width}", "350");
                                    htmlChild.Append(rownew);
                                }
                                htmlChild.Append("</table>");
                                TemplateHandler.Assign(ref rowChild, "{childtable}", htmlChild.ToString());
                                html.Append(rowChild);
                            }
                        }
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    html.Append(norow);
                    html.Append("</table>");
                }
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                TemplateHandler.Assign(ref norow, "{nodata}", ex.Message);
                html.Append(norow);
                html.Append("</table>");
            }
            html.Append("</table>");
            return html.ToString();
        }

        #endregion GetCreditTerm()

        #region IsContains

        public static bool IsContains(string strParent, string strChild)
        {
            bool Result = false;

            foreach (string Element in strParent.Split(','))
            {
                if (Element.Trim().ToUpper().Equals(strChild.Trim().ToUpper()))
                {
                    Result = true;
                    break;
                }
            }
            return Result;
        }

        #endregion IsContains

        #region ExportToExcel

        public static void ExportToExcel(string strData, string fileName)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("<html>");
            content.AppendLine("<head>");
            content.AppendLine("<META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>");
            content.AppendLine("</head>");
            content.AppendLine(ReportHTML.CSSExcelReport());
            content.Append("</hmtl> ");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
            HttpContext.Current.Response.Charset = "";
            strData = strData.Replace("href", "");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

            HttpContext.Current.Response.Write(content.ToString());
            HttpContext.Current.Response.Write(strData);
            HttpContext.Current.Response.Write("<table><tr><td class='tdhidden'></td></tr></table>");

            HttpContext.Current.Response.End();
        }

        #endregion ExportToExcel

        #region 2A Report

        #endregion 2A Report
    }
}