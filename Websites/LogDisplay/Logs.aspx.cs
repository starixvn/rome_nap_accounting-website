using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DNAide.Web;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Resources;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.IBCDLL;

public partial class LogDisplay_Logs : PageBase
{
    protected override bool HasPermission()
    {
        return Functions.Customer.AllowViewLog && base.HasPermission();
    }

    StringBuilder str = new StringBuilder();
    protected string hideProgressBar = string.Empty;
    #region "local variables"
    int cntRow = 0;
    string[] bettype ={ "_42","_41","_32","_31","_22", "_21", "_20", "_10", "_9", "_8", "_7", "_6", "_5", "_4", "_3", "_2", "_1" };    
    string[] sporttype ={ "MaxBv10", "MaxBv11", "MaxBv201", "MaxBv2", "MaxBv3", "MaxBv5", "MaxBv8", "MaxBv99", 
                            "MinBv10", "MinBv11", "MinBv201", "MinBv2", "MinBv3", "MinBv5", "MinBv8", "MinBv99", 
                            "MPMv10", "MPMv11", "MPMv201", "MPMv2", "MPMv3", "MPMv5", "MPMv8", "MPMv99","MPM","MinB","MaxB" };
    string[] strInfo ={"fname", "lname", "mphone"};
    //, "UMMB"
    string Color = "f5eddd";//Background content
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        string strSearch = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("LogSearch.htm");
        DateTime fromdate = DateTime.Now;
        DateTime todate = DateTime.Now;
        string strUsername = string.Empty;
        string strEditBy = string.Empty;

        strUsername = CastAide.AsString(Request["txtUsername"]).Trim();
        if (!string.IsNullOrEmpty(Request["fdate"])) fromdate = CastAide.AsDateTime(Request["fdate"]);
        if (CastAide.AsDateTime("1/1/2009").CompareTo(fromdate) < 0)
            fromdate = CastAide.AsDateTime("12/31/2008");
        if (!string.IsNullOrEmpty(Request["tdate"])) todate = CastAide.AsDateTime(Request["tdate"]);
        if (CastAide.AsDateTime("1/1/2009").CompareTo(todate) < 0)
            todate = CastAide.AsDateTime("12/31/2008");

        divContent.Visible = true;
        TemplateHandler.Assign(ref strSearch, "username-val", strUsername);
        TemplateHandler.Assign(ref strSearch, "{fromdate}", Calendar.GenCalendarControl("", "20081231", "fdate", "../", "fdateTrigger", fromdate.ToShortDateString(), false, false, "","",""));
        TemplateHandler.Assign(ref strSearch, "{todate}", Calendar.GenCalendarControl("", "20081231", "tdate", "../", "tdateTrigger", todate.ToShortDateString(), true, false, "","",""));

        ltrSearch.Text = strSearch;


        if (!string.IsNullOrEmpty(Request["hdSearch"]) && !string.IsNullOrEmpty(strUsername))
        {
            DataSet ds = LogDisplay.GetLogData(strUsername, strEditBy, IBCUtils.FormatHandler.ResetStartDate(fromdate), IBCUtils.FormatHandler.ResetEndDate(todate));
            if (ds.Tables[0].Rows.Count > 0)
            {
                var _IPVNList = IpManagement.IPVNList;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string strDoing = Convert.ToString(dr["doing"]);
                    str.Append("<tr class='c' style=\"vertical-align:top;\">");
                    str.Append("<td>" + dr["logid"] + "</td>");
                    str.Append("<td>" + dr["logdate"] + "</td>");
                    str.Append("<td>" + dr["owner"] + "</td>");
                    str.Append("<td>" + dr["username"] + "</td>");
                    str.Append("<td>" + GetText(Convert.ToString(dr["spname"])) + "</td>");                    
                    if (strDoing.IndexOf("minpt") > -1)
                    {
                        strDoing = ReplaceRedundant(strDoing);
                        strDoing = strDoing.Replace("minpt", "Min ");
                        strDoing = strDoing.Replace("minlivept", "Min live ");
                        strDoing = strDoing.Replace("maxpt", "Max ");
                        strDoing = strDoing.Replace("maxlivept", "Max live ");
                        strDoing = GetTbl(GetIP(strDoing, _IPVNList), 8);
                        str.Append("<td class='l'>" + strDoing+ "</td>");
                    }
                    else
                    {
                        if (strDoing.IndexOf("<ag>") > -1 || strDoing.IndexOf("<m>") > -1 || strDoing.IndexOf("<s>") > -1 || strDoing.IndexOf("<au>") > -1 || strDoing.IndexOf("<ia>") > -1)
                            str.Append("<td class='l'>" + ParseDataPT(GetIP(strDoing, _IPVNList)) + "</td>");
                        else
                            str.Append("<td class='l'>" + ParseInfoSetting(strDoing, _IPVNList) + "</td>");
                    }
                    str.Append("</tr>");
                }
            }
            else
            {
                str.Append(AccCommon.GenNoInformation("6"));
            }
        }
        ltrContent.Text = str.ToString() ;
    }

    #region "common function"
    private string ReplaceRedundantTabInfo(string strDoing)
    {
        string[] arrSportID = { "Updpt=", "Updptv2", "Updptv3", "Updptv5", "Updptv8", "Updptv10", "Updptv11", "Updptv99" };
        string[] arrSportName = { CacheRef.GetSportName(1)+"=", CacheRef.GetSportName(2), CacheRef.GetSportName(3), CacheRef.GetSportName(5), CacheRef.GetSportName(8), CacheRef.GetSportName(10), CacheRef.GetSportName(11), CacheRef.GetSportName(99) };        
        for (int i = 0; i < arrSportID.Length; i++)
            strDoing = strDoing.Replace(arrSportID[i], "Updpt" + GetText(arrSportName[i]));
        return strDoing;
    }
    private string ReplaceRedundant(string strDoing)
    {
        //strDoing = strDoing.Replace("v151", "");
        strDoing = strDoing.Replace("v99", "");
        //strDoing = strDoing.Replace("v201", "");
        strDoing = strDoing.Replace("v11", "");
        strDoing = strDoing.Replace("v10", "");
        strDoing = strDoing.Replace("v8", "");
        strDoing = strDoing.Replace("v5", "");
        strDoing = strDoing.Replace("v3", "");
        if(strDoing.IndexOf("v201") == -1)
            strDoing = strDoing.Replace("v2", "");        
        for (int i = 0; i < bettype.Length; i++)
        {
            strDoing = strDoing.Replace(bettype[i], GetText(bettype[i]));
        }        
        return strDoing;
    }

    private string ReplaceRedundant3rd(string strDoing)
    {
        strDoing = strDoing.Replace("v151", "");
        strDoing = strDoing.Replace("v201", "");
        for (int i = 0; i < bettype.Length; i++)
        {
            strDoing = strDoing.Replace(bettype[i], GetText(bettype[i]));
        }
        return strDoing;
    }    
    private string GetBgColor()
    {
        switch (Color)
        {
            case "e1e9f1":
                Color = "f5eddd";
                return Color;
            case "f5eddd":
                Color = "e1e9f1";
                return Color;
            default:
                return "e1e9f1";
        }
    }
    public string GetText(string key)
    {
        object text = GetGlobalResourceObject("Language", key);
        if (text == null) return key;
        return text.ToString();
    }
    private string ReplacePermissionName(string strResult)
    {
        strResult = strResult.Replace("[A]", "[CreateAccount]");
        strResult = strResult.Replace("[B]", "[UpdateAccount]");
        strResult = strResult.Replace("[C]", "[Transfer]");
        strResult = strResult.Replace("[D]", "[TotalBets]");
        strResult = strResult.Replace("[E]", "[Reports]");
        return strResult;
    }

    public static string CopyBlock(string str, string tag)
    {
        string startTag = "<" + tag + ">";
        int st = str.IndexOf(startTag);
        if (st == -1)
            return string.Empty;
        int count = str.IndexOf("</" + tag + ">") - str.IndexOf(startTag) - startTag.Length;
        if (count <= 0)
            return string.Empty;
        return str.Substring(st + startTag.Length, count);
    }
    #endregion

    #region "parse data for position taking"
    private string ParseDataPT(string strDoing)
    {
        string master = string.Empty;
        string super = string.Empty;
        string agent = string.Empty;
        string isauto = string.Empty;
        string auto = string.Empty;
        string info = string.Empty;
        string autom = string.Empty;
        string isautom = string.Empty;
        string isautosm = string.Empty;
        string ibcpt = string.Empty;
        
        if (strDoing.IndexOf("<info>") > -1)
        {
            strDoing = ReplaceRedundantTabInfo(strDoing);
            info = GetTbl(CopyBlock(strDoing, "info"), 8);
            info = info.Replace(",", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            info = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773 !important;font-weight:bold;color:#ffffff\">Customer Info</td></tr><tr><td >" + info + "</td></tr></table>";
        }

        strDoing = ReplaceRedundant(strDoing);
        if (strDoing.IndexOf("<ibcpt>") > -1)
        {
            ibcpt = GetTbl(CopyBlock(strDoing, "ibcpt"), 8);
            ibcpt = ibcpt.Replace("IBCpt", "");

            ibcpt = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;width:100%;font-weight:bold;color:#ffffff\">IBC PT</td></tr><tr ><td >" + ibcpt + "</td></tr></table>";
        }
        if (strDoing.IndexOf("<ag>") > -1)
        {
            agent = GetTbl(CopyBlock(strDoing, "ag"), 8);
            agent = agent.Replace("apt", "");
            agent = agent.Replace("alivept", "live ");
            agent = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;font-weight:bold;color:#ffffff\">Agent PT</td></tr><tr ><td >" + agent + "</td></tr></table>";
        }
        if (strDoing.IndexOf("<m>") > -1)
        {
            master = GetTbl(CopyBlock(strDoing, "m"), 8);
            master = master.Replace("mpt", "");
            master = master.Replace("mlivept", "live ");
            master = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;font-weight:bold;color:#ffffff\">Master PT</td></tr><tr ><td >" + master + "</td></tr></table>";
        }
        if (strDoing.IndexOf("<s>") > -1)
        {
            super = GetTbl(CopyBlock(strDoing, "s"), 8);
            super = super.Replace("spt", "");
            super = super.Replace("slivept", "live ");
            super = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;width:100%;font-weight:bold;color:#ffffff\">Super PT</td></tr><tr ><td >" + super + "</td></tr></table>";
        }

        if (strDoing.IndexOf("<au>") > -1)
        {
            auto = GetTbl(CopyBlock(strDoing, "au"), 8);
            auto = auto.Replace("a_spt", "");
            auto = auto.Replace("a_slivept", "live ");
            auto = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;font-weight:bold;color:#ffffff\">Super Auto PT</td></tr><tr ><td >" + auto + "</td></tr></table>";
        }
        if (strDoing.IndexOf("<aum>") > -1)
        {
            autom = GetTbl(CopyBlock(strDoing, "aum"), 8);
            autom = autom.Replace("a_mpt", "");
            autom = autom.Replace("a_mlivept", "live ");
            autom = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;width:100%;font-weight:bold;color:#ffffff\">Master Auto PT</td></tr><tr ><td >" + autom + "</td></tr></table>";
        }

        if (strDoing.IndexOf("<ia>") > -1)
        {
            isauto = GetTbl(CopyBlock(strDoing, "ia"), 8);
            isauto = isauto.Replace("isa_spt", "");
            isauto = isauto.Replace("isa_slivept", "live ");
            isauto = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;width:100%;font-weight:bold;color:#ffffff\">Is Super Auto PT On Member</td></tr><tr ><td >" + isauto + "</td></tr></table>";
        }
        if (strDoing.IndexOf("<iam>") > -1)
        {
            isautom = GetTbl(CopyBlock(strDoing, "iam"), 8);
            isautom = isautom.Replace("isa_mpt", "");
            isautom = isautom.Replace("isa_mlivept", "live ");
            isautom = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;width:100%;font-weight:bold;color:#ffffff\">Is Auto Master PT</td></tr><tr ><td >" + isautom + "</td></tr></table>";
        }
        //auto super on master
        if (strDoing.IndexOf("<isau>") > -1)
        {
            isautosm = GetTbl(CopyBlock(strDoing, "isau"), 8);
            isautosm = isautosm.Replace("isa_smpt", "");
            isautosm = isautosm.Replace("isa_smlivept", "live ");
            isautosm = "<table style=\"width:100%\" class=\"" + GetBgColor() + "\"><tr ><td colspan=\"7\" style=\"background-color:#2f5773  !important;width:100%;font-weight:bold;color:#ffffff\">Is Super Auto PT On Master</td></tr><tr ><td >" + isautosm + "</td></tr></table>";
        }
        
        string strDisplay = info + ibcpt + agent + master + super + auto + isauto + isautosm + autom + isautom;
        return strDisplay;
    }

    private string GetTbl(string str, int cols)
    {
        if (str.Equals("")) return "";
        string strR = string.Empty;
        char tmp2 = Microsoft.VisualBasic.Strings.Chr(7);

        string[] arrSport = str.Split(tmp2);
        if (IsHorseRacing(str) || IsFinance(str))
        {
            //this for display Soccer
            //str = ReplaceRedundant3rd(str);
            
            strR += "<table class=\"PT\" style=\"width:100%\">";
            if (arrSport[0] != "")
            {
                cntRow++;
                strR += "<tr><td colspan=\"5\" style=\"font-weight:bold;color:#2e3756\">" + GetSportName(GetSportType(str)) + "</td></tr>";
                string[] bettype = arrSport[0].Split(',');
                string cell = string.Empty;
                strR += "<tr>";
                for (int j = 0; j < bettype.Length; j++)
                {
                    if ((j) % cols == 0)
                    {
                        strR += "</tr><tr>";
                    }

                    strR += "<td nowrap>" + ReplaceRedundant3rd(bettype[j]) + "</td>";
                }
               
                strR += "<td colspan=\"" + (cols - (bettype.Length % cols)).ToString() + "\" class=\"detail\"></td>";
                strR += "</tr>";
            }
            strR += "</table>";
            strR = ParseHRBettype(strR, (strR.IndexOf("IBCpt") != -1));
            
        }
        else
        {
            if (arrSport.Length > 1)
            {
                string[] SportTitle = { "Soccer", "Basketball", "Football", "Tennis", "Baseball", "Gofl", "Motorsports", "OtherSports", "", "" };
                //this for display Soccer
                strR += "<table class=\"PT\" style=\"width:100%\">";
                if (arrSport[0] != "")
                {
                    cntRow++;
                    strR += "<tr><td colspan=\"5\" style=\"font-weight:bold;color:#2e3756\">" + SportTitle[0] + "</td></tr>";
                    string[] bettype = arrSport[0].Split(',');
                    string cell = string.Empty;
                    strR += "<tr>";
                    for (int j = 0; j < bettype.Length; j++)
                    {
                        if ((j) % cols == 0)
                        {
                            strR += "</tr><tr>";
                        }

                        strR += "<td nowrap>" + bettype[j] + "</td>";
                    }
                    strR += "<td colspan=\"" + (cols - (bettype.Length % cols)).ToString() + "\" class=\"detail\"><span onclick=\"OpenDetail(this,'detail1" + cntRow + "');\">Details +</span></td>";
                    strR += "</tr>";
                }
                strR += "</table>";
                ////////////////////////            
                strR += "<div style=\"display:none\" id=\"detail1" + cntRow + "\">";
                strR += "<table class=\"PT\" style=\"width:100%\">";
                for (int i = 1; i < arrSport.Length; i++)
                {
                    if (arrSport[i] != "")
                    {
                        string title = "";
                        if (i >= SportTitle.Length)
                            title = "";
                        else
                            title = SportTitle[i];

                        strR += "<tr><td colspan=\"" + (cols - (bettype.Length % cols)).ToString() + "\" style=\"font-weight:bold;color:#2e3756\">" + title + "</td></tr>";
                        string[] strBettype = arrSport[i].Split(',');
                        string cell = string.Empty;
                        strR += "<tr>";
                        for (int j = 0; j < strBettype.Length; j++)
                        {
                            if ((j) % cols == 0)
                            {
                                strR += "</tr><tr>";
                            }

                            strR += "<td nowrap>" + strBettype[j] + "</td>";
                        }
                        strR += "</tr>";
                    }
                }
                strR += "</table>";
                strR += "</div>";
            }
            else
            {
                strR = str;
            }
        }
        return strR;
    }
    #endregion

    #region "parse data for Info setting"
    private string ParseInfoSetting(string strDoing, List<IPVNBean> _IPVN)
    {
        if (strDoing == "") return "";
        strDoing = ReplacePermissionName(strDoing);
        for (int i = 0; i < strInfo.Length; i++)
        {
            strDoing = strDoing.Replace(strInfo[i], GetText(strInfo[i]));
        }
        if (strDoing.IndexOf("MPMv99", StringComparison.CurrentCultureIgnoreCase) > -1)
        {
            for (int i = 0; i < sporttype.Length; i++)
            {
                strDoing = strDoing.Replace(sporttype[i], GetText(sporttype[i]));
            }
        }
        string strR = "";
        strDoing = GetIP(strDoing, _IPVN);
        //END
        string[] arrStr = strDoing.Split(',');
        string cell = string.Empty;
        cntRow++;
        strR += "<table style=\"width:100%;\">";
        strR += "<tr>";
        if (arrStr.Length > 7)
        {
            for (int j = 0; j < 7; j++)
            {
                if ((j) % 4 == 0)
                {
                    strR += "</tr><tr>";
                }
                strR += "<td style=\"width:20%\" nowrap>" + arrStr[j] + "</td>";
            }
            strR += "<td class=\"detail\" nowrap><span onclick=\"OpenDetail(this,'detail1" + cntRow + "');\">Details +</span></td>";
        }
        else
        {
            for (int i = 0; i < arrStr.Length; i++)
            {
                if ((i) % 4 == 0)
                {
                    strR += "</tr><tr>";
                }
                strR += "<td style=\"width:20%\" nowrap>" + arrStr[i] + "</td>";
            }
        }
        strR += "</tr>";

        strR += "<tr style=\"display:none\" id=\"detail1" + cntRow + "\">";
        strR += "<td colspan=\"5\">";

        strR += "<table class=\"infosetting\">";
        strR += "<tr>";
        int length = arrStr.Length;
        for (int j = 7; j < length; j++)
        {
            if ((j + 1) % 4 == 0)
            {
                strR += "</tr><tr>";
            }
            strR += "<td style=\"width:20%\" nowrap>" + arrStr[j] + "</td>";
        }
        strR += "</tr></table>";
        strR += "</td></tr></table>";

        return strR;
    }

    private string GetIP(string strSetting, List<IPVNBean> _IPVNList)
    {
        string html = string.Empty;
        string ip = "";
        int start = 0;
        int end = 0;
        start=strSetting.IndexOf("ip=", StringComparison.CurrentCultureIgnoreCase);
        if (start < 0) start = strSetting.IndexOf("ip:", StringComparison.CurrentCultureIgnoreCase);
        if (start < 0) start = strSetting.IndexOf("ip = ", StringComparison.CurrentCultureIgnoreCase);
        if (start > -1)
        {
            ip = strSetting.Substring(start + 3);
            end = ip.IndexOf(",");
            if (end > -1)
                ip = ip.Substring(0, end);
        }        

        ip = ip.Replace("\r\n", "").Trim();
        if (!ip.Equals(""))
        {
            string link = "<a href=\"" + ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", IpManagement.ReplaceVNIP(ip, _IPVNList)) + "\" target=\"_blank\">" + IpManagement.ReplaceVNIP(ip, _IPVNList) + "</a>";
            html = strSetting.Replace(ip, link);
        }
        else
            html = strSetting;
        return html;
    }

    private string GetSportName(SportType sportType)
    {
        switch (sportType)
        {
            case SportType.HorseRacing:
                return Resources.Language.lblHorseRacing; 
            case SportType.Finance:
                return Resources.Language.lblFinancial;
            default:
                return "";
        }
    }

    public enum SportType
    {
        HorseRacing = 1, Finance = 2, NOT_KNOW = 0
    }

    private SportType GetSportType(string logContent)
    {
        if (logContent.IndexOf("v201") != -1)
            return SportType.Finance;
        else if (logContent.IndexOf("v151") != -1)
            return SportType.HorseRacing;
        return SportType.NOT_KNOW;
    }

    private bool IsHorseRacing(string logContent)
    {
        if(logContent.IndexOf("v151") == -1)
            return false;

        return true;
    }

    private bool IsFinance(string logContent)
    {
        if (logContent.IndexOf("v201") == -1)
            return false;

        return true;
    }

    private string ParseHRBettype(string content, bool isIBCPT)
    {
        try
        {
            content = content.Replace("OU1", string.Format(Language.lblToteFormat, (isIBCPT) ? string.Empty : Language.lblWin));
            content = content.Replace("OU2", string.Format(Language.lblToteFormat, (isIBCPT) ? string.Empty : Language.lblPlace));

            content = content.Replace("CS1", string.Format(Language.lblOpenToteFormat, (isIBCPT) ? string.Empty : Language.lblWin));
            content = content.Replace("CS2", string.Format(Language.lblOpenToteFormat, (isIBCPT) ? string.Empty : Language.lblPlace));
            return content;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }
   #endregion
   
}
