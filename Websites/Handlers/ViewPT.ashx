<%@ WebHandler Language="C#" Class="ViewPT" %>
using System;
using System.Web;
using System.Data;
using System.Text;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Business.PT.ViewPT;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;

using Sunplus.Core.LoggingLib;
using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;
using DNAide.Data;
using CasinoLibrary.SBAgent;


public class ViewPT : BaseHandler
{    
    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string _s = string.Empty;
        try
        {
            string _type = context.Request["type"];
            int _sportID = CastAide.AsInt32(context.Request["sid"], 0);            
            switch (_type.ToLowerInvariant())
            {
                case "changesport": _s = GetBettypeList(context, _sportID); break;
                case "pt": 
                case "comm": 
                    string username = context.Request["username"].Trim();
                    int _bettype = CastAide.AsInt32(context.Request["bid"], 0);
                    bool _isLive = (context.Request["isLive"] == "1");
                    bool _hasComm = (string.Compare(_type, "comm", true) ==0);

                    if (_sportID == 252)
                    {
                        IBingoViewPT bingoViewPT = new BingoViewPT();
                        _s = bingoViewPT.GetPTBingo(username,_bettype);
                    }
                    else if (_sportID != 251)
                    {
                        DbResult dr = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetPT(username, _sportID == 999 ? Sunplus.Accounting.Core.Common.Constants.Sport.OtherSports : _sportID, _bettype, _isLive, _hasComm);
                        if (CastAide.AsInt32(dr.Outputs["errcode"], -1) != 0)
                        {
                            _s = "<script>showAlertMessage('err','" + dr.Outputs["errmsg"].ToString() + "',1);</script>";
                        }
                        else
                        {
                            if (_sportID == 999 || _sportID == Sunplus.Accounting.Core.Common.Constants.Sport.OtherSports)
                                _s = GenerateHTMLParlayMixedSportsPT(dr, _sportID);
                            
                            else
                                _s = (_hasComm) ? GenerateHTMLCommission(dr) : GenerateHTMLPosiontaking(dr);
                        }
                    }
                    else
                    {
                        _s = GenerateHTMLCasinoPTCom(username, _bettype);
                    }
                    break;
                default: _s = "Bad Request!";
                    break;
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            GotoErroPage(context);
            
        }
        context.Response.Write(_s);
    }

    public static string GetGameTypeCasino(string rngSiteName)
    {
        string ListGameKind = string.Empty;
        string ListGameType = string.Empty;
        BasicInfo bi = new BasicInfo();
        bi.SiteName = rngSiteName;
        DataTable tblGameKind = bi.GetGameKind("01");
        if (tblGameKind != null && tblGameKind.Rows.Count != 0)
        {
            //Load Game Type
            foreach (DataRow dr in tblGameKind.Rows)
            {
                DataTable tblGameType = bi.GetGameType(CastAide.AsInt32(dr["CodeID"]));
                if (tblGameType != null && tblGameType.Rows.Count != 0)
                {
                    ListGameKind += "<option value='-" + dr["CodeID"].ToString() + "'>Group - " + dr["CodeName"].ToString() + "</option>";              
                    foreach (DataRow dr2 in tblGameType.Rows)
                    {
                        ListGameType+="<option value='" + dr2["GameTypeID"].ToString() + "'>" + dr2["GameTypeName"].ToString() + "</option>";
                    }
                }
            }
        }
        string ALL  = "<option value='-1'>.: ALL - Game Kind :.</option>"
                    + "<option value='0'>.: ALL - Game Type :.</option></select>";
        return ListGameType + ListGameKind + ALL;
    }

    public static string GetGameTypeBingo()
    {
        IBingoViewPT bingoViewPT = new BingoViewPT();
        return bingoViewPT.GetListBingoGameKindGameType();
    }
    
    public static string GetBettypeList(HttpContext hc, int sportID)
    {
        StringBuilder _s = new StringBuilder();
        try
        {
            _s.Append("<select id='ddlBettype'>");
            if (sportID == 251) // For Casino
            {
                var custInfo = new Sunplus.Accounting.DataAccess.AdoNet.CustomerDAO().GetCustomerInfo(hc.Request["username"].Trim());
                if (custInfo != null && custInfo.Rows.Count > 0)
                {
                    var rngSiteName = new Sunplus.Accounting.DataAccess.AdoNet.CustomerDAO().GetRNGSiteNameByCustId(CastAide.AsInt32(custInfo.Rows[0]["custid"]));
                    _s.Append(GetGameTypeCasino(rngSiteName));
                }
                
            }
            else if (sportID == 252)
            {
                _s.Append(GetGameTypeBingo());
            }
            else
            {
                using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetPTBetType(sportID))
                {
                    while (dr.Read())
                    {
                        if (sportID == 151) // For Racing
                        {
                            if (CastAide.AsInt32(dr["bettype"]) < 33)
                            {
                                if (!WebSettings.DisableHRPT_Tote)
                                    _s.Append("<option value='" + dr["bettype"].ToString() + "'>" +
                                              dr["typenamee"].ToString() + "</option>");
                            }
                            else
                            {
                                if (!WebSettings.DisableHRPT_OpenTote)
                                    _s.Append("<option value='" + dr["bettype"].ToString() + "'>" +
                                              dr["typenamee"].ToString() + "</option>");
                            }
                        }
                        else if (sportID == Constants.Sport.OtherSports)
                        {
                            if (CastAide.AsInt32(dr["bettype"]) != Constants.BetType.MixParlay)
                                _s.Append("<option value='" + dr["bettype"].ToString() + "'>" + dr["typenamee"].ToString() +
                                      "</option>");
                        }
                        else
                            _s.Append("<option value='" + dr["bettype"].ToString() + "'>" + dr["typenamee"].ToString() + "</option>");
                    }
                }
                _s.Append("<option value='0'>..:: ALL ::..</option></select>");
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            
        }
        return _s.ToString();
    }

    public static string GenerateHTMLCasinoPTCom(string username, int BetType)
    {
        DataTable dtb = null;
        if (BetType < 0)
            dtb = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetCSPT(username, BetType*(-1), 0);        
        else
            dtb = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetCSPT(username, 0, BetType);

        if (dtb == null) return "<b><br/>There is no information available.</b>";
        if (dtb.Rows.Count == 0) return "<b><br/>There is no information available.</b>";
        
        string _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/ViewPT_CSPT.htm");
        DataRow rInfo = dtb.Rows[dtb.Rows.Count-1];

        int roleID = CastAide.AsInt32(rInfo["roleid"], 1);

        // information
        string _sUpline = TemplateHandler.CopyBlock(_s, "upline");
        string curUpline = string.Empty;
        StringBuilder _sbUpline = new StringBuilder();
        if (roleID < 2)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "AGENT");
            TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[2]["username"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[2]["custid"].ToString());
            _sbUpline.Append(curUpline);
        }
        if (roleID < 3)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "MASTER");
            TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[1]["username"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[1]["custid"].ToString());
            _sbUpline.Append(curUpline);
        }
        if (roleID < 4)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "SUPER");
            TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[0]["username"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[0]["custid"].ToString());
            _sbUpline.Append(curUpline);
        }
        TemplateHandler.ReplaceBlock(ref _s, "upline", _sbUpline.ToString());
        TemplateHandler.Assign(ref _s, "CustName", rInfo["username"].ToString());
        TemplateHandler.Assign(ref _s, "CustID", rInfo["custid"].ToString());

        // position taking
        string sTable = TemplateHandler.CopyBlock(_s, "tbl");
        for (int i = 1; i < 5; i++)
        {
            if (roleID > i) TemplateHandler.RemoveBlock(ref sTable, "r" + i.ToString());
        }
        string curRow = string.Empty, _sTable = string.Empty;
        StringBuilder _mainTable = new StringBuilder();
        int curRoleID;
        foreach (DataRow r in dtb.Rows)
        {
            curRoleID = CastAide.AsInt16(r["roleid"].ToString());
            if (curRoleID == 4)
            {
                _sTable = sTable;
                TemplateHandler.Assign(ref _sTable, "sportName", "Casino");
                TemplateHandler.Assign(ref _sTable, "GameKind", r["gamekind"].ToString());
                TemplateHandler.Assign(ref _sTable, "GameType", r["gametype"].ToString());
            }
            
            // super
            if (curRoleID == 4)
            {
                TemplateHandler.Assign(ref _sTable, "s4", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m4", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a4", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT4", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT4", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT4", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com4", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            }            
                
            // master
            if (curRoleID == 3)
            {
                TemplateHandler.Assign(ref _sTable, "s3", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m3", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a3", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT3", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT3", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT3", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com3", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            }  

            // agent                                                            
            if (curRoleID == 2)
            {
                TemplateHandler.Assign(ref _sTable, "s2", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m2", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a2", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT2", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT2", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT2", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com2", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            } 

            // member                                                           
            if (curRoleID == 1)
            {
                TemplateHandler.Assign(ref _sTable, "s1", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m1", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a1", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT1", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT1", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT1", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com1", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            }

            if (roleID == curRoleID)
                _mainTable.Append(_sTable);
        }
        TemplateHandler.ReplaceBlock(ref _s, "tbl", _mainTable.ToString());
        return _s;
    }

    public static string GenerateHTMLMiniGamePTCom(string username)
    {
        DataTable dtb = null;
      
        dtb = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetMiniGamePt(username);

        if (dtb == null) return "<b><br/>There is no information available.</b>";
        if (dtb.Rows.Count == 0) return "<b><br/>There is no information available.</b>";

        string _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/ViewPT_MinigamePT.htm");
        DataRow rInfo = dtb.Rows[dtb.Rows.Count - 1];

        int roleID = CastAide.AsInt32(rInfo["roleid"], 1);

        // information
        string _sUpline = TemplateHandler.CopyBlock(_s, "upline");
        string curUpline = string.Empty;
        StringBuilder _sbUpline = new StringBuilder();
        if (roleID < 2)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "AGENT");
            TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[2]["username"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[2]["custid"].ToString());
            _sbUpline.Append(curUpline);
        }
        if (roleID < 3)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "MASTER");
            TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[1]["username"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[1]["custid"].ToString());
            _sbUpline.Append(curUpline);
        }
        if (roleID < 4)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "SUPER");
            TemplateHandler.Assign(ref curUpline, "Name", dtb.Rows[0]["username"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", dtb.Rows[0]["custid"].ToString());
            _sbUpline.Append(curUpline);
        }
        TemplateHandler.ReplaceBlock(ref _s, "upline", _sbUpline.ToString());
        TemplateHandler.Assign(ref _s, "CustName", rInfo["username"].ToString());
        TemplateHandler.Assign(ref _s, "CustID", rInfo["custid"].ToString());

        // position taking
        string sTable = TemplateHandler.CopyBlock(_s, "tbl");
        for (int i = 1; i < 5; i++)
        {
            if (roleID > i) TemplateHandler.RemoveBlock(ref sTable, "r" + i.ToString());
        }
        string curRow = string.Empty, _sTable = string.Empty;
        StringBuilder _mainTable = new StringBuilder();
        int curRoleID;
        foreach (DataRow r in dtb.Rows)
        {
            curRoleID = CastAide.AsInt16(r["roleid"].ToString());
            if (curRoleID == 4)
            {
                _sTable = sTable;
                TemplateHandler.Assign(ref _sTable, "sportName", "Mini Game");
            }

            // super
            if (curRoleID == 4)
            {
                TemplateHandler.Assign(ref _sTable, "s4", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m4", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a4", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT4", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT4", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT4", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com4", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            }

            // master
            if (curRoleID == 3)
            {
                TemplateHandler.Assign(ref _sTable, "s3", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m3", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a3", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT3", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT3", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT3", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com3", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            }

            // agent                                                            
            if (curRoleID == 2)
            {
                TemplateHandler.Assign(ref _sTable, "s2", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m2", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a2", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT2", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT2", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT2", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com2", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            }

            // member                                                           
            if (curRoleID == 1)
            {
                TemplateHandler.Assign(ref _sTable, "s1", NumberFormat.DecFormat(r["sPT"], 2));
                TemplateHandler.Assign(ref _sTable, "m1", NumberFormat.DecFormat(r["mPT"], 2));
                TemplateHandler.Assign(ref _sTable, "a1", NumberFormat.DecFormat(r["aPT"], 2));
                TemplateHandler.Assign(ref _sTable, "MinPT1", NumberFormat.DecFormat(r["MinPT"], 2));
                TemplateHandler.Assign(ref _sTable, "isAutoPT1", (CastAide.AsBoolean(r["isAutoPT"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "AutoPT1", NumberFormat.DecFormat(r["AutoPT"], 2));
                TemplateHandler.Assign(ref _sTable, "Com1", IBCUtils.FormatHandler.FormatNumber4(r["RollingRate"]));
            }

            if (roleID == curRoleID)
                _mainTable.Append(_sTable);
        }
        TemplateHandler.ReplaceBlock(ref _s, "tbl", _mainTable.ToString());
        return _s;
    }

    #region Generate HTML Bingo PT-Com
    public static string GenerateHTMLBingoPTCom(string username, int BetType, DbResult dr)
    {
        DataRow rInfo = dr.ResultSet.Tables[0].Rows[0];
        DataTable dtb = dr.ResultSet.Tables[1];
        
        if (dtb == null && dtb.Rows.Count == 0)         
            return "<b><br/>There is no information available.</b>";
        
        string _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/ViewPT_BingoPT.htm");

        int roleID = CastAide.AsInt32(rInfo["roleid"], 1);

        // information
        string _sUpline = TemplateHandler.CopyBlock(_s, "upline");
        string curUpline = string.Empty;
        StringBuilder _sbUpline = new StringBuilder();
        if (roleID < 2)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "AGENT");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["Ag"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["AgID"].ToString());
            _sbUpline.Append(curUpline);
        }
        if (roleID < 3)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "MASTER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["MAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["MAgID"].ToString());
            _sbUpline.Append(curUpline);
        }
        if (roleID < 4)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "SUPER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["SAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["SAgID"].ToString());
            _sbUpline.Append(curUpline);
        }
        TemplateHandler.ReplaceBlock(ref _s, "upline", _sbUpline.ToString());
        TemplateHandler.Assign(ref _s, "CustName", rInfo["username"].ToString());
        TemplateHandler.Assign(ref _s, "CustID", rInfo["custid"].ToString());

        // position taking
        string sTable = TemplateHandler.CopyBlock(_s, "tbl");
        for (int i = 1; i < 5; i++)
        {
            if (roleID > i) TemplateHandler.RemoveBlock(ref sTable, "r" + i.ToString());
        }
        string curRow = string.Empty, _sTable = string.Empty;
        StringBuilder _mainTable = new StringBuilder();
        int curRoleID;
        foreach (DataRow r in dtb.Rows)
        {
            curRoleID = CastAide.AsInt16(rInfo["roleid"].ToString());
            _sTable = sTable;
            TemplateHandler.Assign(ref _sTable, "sportName", "Bingo");
            TemplateHandler.Assign(ref _sTable, "GameKind", "Bingo");
            TemplateHandler.Assign(ref _sTable, "GameType", "Bingo");

            // super
            TemplateHandler.Assign(ref _sTable, "s4", NumberFormat.DecFormat(r["s4"], 2));
            TemplateHandler.Assign(ref _sTable, "m4", NumberFormat.DecFormat(r["m4"], 2));
            TemplateHandler.Assign(ref _sTable, "a4", NumberFormat.DecFormat(r["a4"], 2));
            TemplateHandler.Assign(ref _sTable, "MinPT4", NumberFormat.DecFormat(r["n4"], 2));            
            TemplateHandler.Assign(ref _sTable, "isAutoPT4", (1 == 1) ? " checked='checked'" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "AutoPT4", NumberFormat.DecFormat(0, 2));
            TemplateHandler.Assign(ref _sTable, "Com4", IBCUtils.FormatHandler.FormatNumber4(0));

            // master
            TemplateHandler.Assign(ref _sTable, "s3", NumberFormat.DecFormat(r["s3"], 2));
            TemplateHandler.Assign(ref _sTable, "m3", NumberFormat.DecFormat(r["m3"], 2));
            TemplateHandler.Assign(ref _sTable, "a3", NumberFormat.DecFormat(r["u3"], 2));
            TemplateHandler.Assign(ref _sTable, "MinPT3", NumberFormat.DecFormat(r["n3"], 2));
            TemplateHandler.Assign(ref _sTable, "isAutoPT3", (CastAide.AsBoolean(r["i3"], false)) ? " checked='checked'" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "AutoPT3", NumberFormat.DecFormat(0, 2));
            TemplateHandler.Assign(ref _sTable, "Com3", IBCUtils.FormatHandler.FormatNumber4(0));

            // agent                                                            
            TemplateHandler.Assign(ref _sTable, "s2", NumberFormat.DecFormat(0, 2));
            TemplateHandler.Assign(ref _sTable, "m2", NumberFormat.DecFormat(r["m2"], 2));
            TemplateHandler.Assign(ref _sTable, "a2", NumberFormat.DecFormat(r["a2"], 2));
            TemplateHandler.Assign(ref _sTable, "MinPT2", NumberFormat.DecFormat(r["n2"], 2));
            TemplateHandler.Assign(ref _sTable, "isAutoPT2", (CastAide.AsBoolean(r["im2"], false)) ? " checked='checked'" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "AutoPT2", NumberFormat.DecFormat(r["im2"], 2));
            TemplateHandler.Assign(ref _sTable, "Com2", IBCUtils.FormatHandler.FormatNumber4(0));

            // member                                                           
            TemplateHandler.Assign(ref _sTable, "s1", NumberFormat.DecFormat(r["s1"], 2));
            TemplateHandler.Assign(ref _sTable, "m1", NumberFormat.DecFormat(r["m1"], 2));
            TemplateHandler.Assign(ref _sTable, "a1", NumberFormat.DecFormat(r["a1"], 2));
            TemplateHandler.Assign(ref _sTable, "MinPT1", NumberFormat.DecFormat(0, 2));
            TemplateHandler.Assign(ref _sTable, "isAutoPT1", (CastAide.AsBoolean(r["i1"], false)) ? " checked='checked'" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "AutoPT1", NumberFormat.DecFormat(r["im1"], 2));
            TemplateHandler.Assign(ref _sTable, "Com1", IBCUtils.FormatHandler.FormatNumber4(0));

            if (roleID == curRoleID)
                _mainTable.Append(_sTable);
        }
        TemplateHandler.ReplaceBlock(ref _s, "tbl", _mainTable.ToString());
        return _s;
    }
    #endregion


    public static string GenerateHTMLPosiontaking(DbResult dr)
    {
        string _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/ViewPT_PT.htm");
        DataRow rInfo = dr.ResultSet.Tables[0].Rows[0];
        DataTable tblContent = dr.ResultSet.Tables[1];
        
        int roleID = CastAide.AsInt32(rInfo["roleid"], 1);        

        // information
        string _sUpline = TemplateHandler.CopyBlock(_s, "upline");
        string curUpline = string.Empty;
        StringBuilder _sbUpline = new StringBuilder();
        if (roleID < 2)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "AGENT");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["Ag"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["AgID"].ToString());
            _sbUpline.Append(curUpline);
        }
        if (roleID < 3)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "MASTER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["MAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["MAgID"].ToString());
            _sbUpline.Append(curUpline);


        }
        if (roleID < 4)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "SUPER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["SAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["SAgID"].ToString());
            _sbUpline.Append(curUpline);
        }
        TemplateHandler.ReplaceBlock(ref _s, "upline", _sbUpline.ToString());
        TemplateHandler.Assign(ref _s, "CustName", rInfo["username"].ToString());
        TemplateHandler.Assign(ref _s, "CustID", rInfo["custid"].ToString());

        int _sportID = CastAide.AsInt32(HttpContext.Current.Request["sid"], 0);

        // IBC position taking for HR
        string _ibcPThr = string.Empty;
        if (_sportID == 151)
        {
            if (dr.ResultSet.Tables[2].Rows.Count>0)
            {
                int _bettype = CastAide.AsInt32(HttpContext.Current.Request["bid"], 0);
                _ibcPThr = TemplateHandler.CopyBlock(_s, "IBC_PT_HR");
                TemplateHandler.Assign(ref _ibcPThr, "Tote", Resources.Language.lblTote);
                TemplateHandler.Assign(ref _ibcPThr, "Fixed", Resources.Language.lblOpenTote);
                TemplateHandler.Assign(ref _ibcPThr, "AUTote", Resources.Language.lblAUTote);
                
                // For IBC Auto PT 
                foreach (DataRow r in dr.ResultSet.Tables[2].Rows)
                {
                    TemplateHandler.Assign(ref _ibcPThr, "ibc_pt_HR_31", NumberFormat.DecFormat(r[string.Format("v{0}_AutoIBC_pt_31", _sportID)], 2));
                    TemplateHandler.Assign(ref _ibcPThr, "ibc_pt_HR_41", NumberFormat.DecFormat(r[string.Format("v{0}_AutoIBC_pt_41", _sportID)], 2));
                    TemplateHandler.Assign(ref _ibcPThr, "ibc_pt_HR_51", NumberFormat.DecFormat(r[string.Format("v{0}_AutoIBC_pt_51", _sportID)], 2));

                    // For Actual IBC PT on Member (Win Place)
                    if (_bettype == 0) // All
                    {
                        //TemplateHandler.Assign(ref _ibcPThr, "ToteEWWin", NumberFormat.DecFormat(r["to_31_33"], 2));
                        //TemplateHandler.Assign(ref _ibcPThr, "ToteEWPlace", NumberFormat.DecFormat(r["to_32_33"], 2));
                        //TemplateHandler.Assign(ref _ibcPThr, "FixedEWWin", NumberFormat.DecFormat(r["fo_41_43"], 2));
                        //TemplateHandler.Assign(ref _ibcPThr, "FixedEWPlace", NumberFormat.DecFormat(r["fo_42_43"], 2));

                        TemplateHandler.Assign(ref _ibcPThr, "ToteEWWin", NumberFormat.DecFormat(r[string.Format("v151_ibc_pt_31_33")], 2));
                        TemplateHandler.Assign(ref _ibcPThr, "ToteEWPlace", NumberFormat.DecFormat(r[string.Format("v151_ibc_pt_32_33")], 2));
                        TemplateHandler.Assign(ref _ibcPThr, "FixedEWWin", NumberFormat.DecFormat(r[string.Format("v151_ibc_pt_41_43")], 2));
                        TemplateHandler.Assign(ref _ibcPThr, "FixedEWPlace", NumberFormat.DecFormat(r[string.Format("v151_ibc_pt_42_43")], 2));
                        TemplateHandler.Assign(ref _ibcPThr, "AUEWWin", NumberFormat.DecFormat(r[string.Format("v151_ibc_pt_51_53")], 2));
                        TemplateHandler.Assign(ref _ibcPThr, "AUEWPlace", NumberFormat.DecFormat(r[string.Format("v151_ibc_pt_52_53")], 2));
                    }
                }
                // For Actual IBC PT on Member
                
                if (roleID == 1)
                {
                    TemplateHandler.Assign(ref _ibcPThr, "Win", Resources.Language.lblWin);
                    TemplateHandler.Assign(ref _ibcPThr, "Place", Resources.Language.lblPlace);
                    TemplateHandler.Assign(ref _ibcPThr, "E/W", "E/W");
                    if (_bettype != 0)
                    {
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "EW");
                        if (_bettype == 31)
                        {
                            TemplateHandler.Assign(ref _ibcPThr, "ToleWin", NumberFormat.DecFormat(tblContent.Rows[0]["ibc_pt_hr"], 2));
                            TemplateHandler.RemoveBlock(ref _ibcPThr, "Place");
                        }
                        else if (_bettype == 32)
                        {
                            TemplateHandler.Assign(ref _ibcPThr, "TotePlace", NumberFormat.DecFormat(tblContent.Rows[0]["ibc_pt_hr"], 2));
                            TemplateHandler.RemoveBlock(ref _ibcPThr, "Win");
                        }
                        else if (_bettype == 41)
                        {
                            TemplateHandler.Assign(ref _ibcPThr, "FixedWin", NumberFormat.DecFormat(tblContent.Rows[0]["ibc_pt_hr"], 2));
                            TemplateHandler.RemoveBlock(ref _ibcPThr, "Place");
                        }
                        else if (_bettype == 42)// 42
                        {
                            TemplateHandler.Assign(ref _ibcPThr, "FixedPlace", NumberFormat.DecFormat(tblContent.Rows[0]["ibc_pt_hr"], 2));
                            TemplateHandler.RemoveBlock(ref _ibcPThr, "Win");
                        }
                        else if (_bettype == 51)// 51
                        {
                            TemplateHandler.Assign(ref _ibcPThr, "AUWin", NumberFormat.DecFormat(tblContent.Rows[0]["ibc_pt_hr"], 2));
                            TemplateHandler.RemoveBlock(ref _ibcPThr, "Place");
                        }
                        else if (_bettype == 52)// 52
                        {
                            TemplateHandler.Assign(ref _ibcPThr, "AUPlace", NumberFormat.DecFormat(tblContent.Rows[0]["ibc_pt_hr"], 2));
                            TemplateHandler.RemoveBlock(ref _ibcPThr, "Win");
                        }
                    }
                    else // All
                    {
                        TemplateHandler.Assign(ref _ibcPThr, "ToleWin", NumberFormat.DecFormat(tblContent.Rows[0]["ibc_pt_hr"], 2));
                        TemplateHandler.Assign(ref _ibcPThr, "TotePlace", NumberFormat.DecFormat(tblContent.Rows[1]["ibc_pt_hr"], 2));
                        if (!WebSettings.DisableHRPT_OpenTote)
                        {
                            if (tblContent.Rows.Count > 2)
                            {
                                TemplateHandler.Assign(ref _ibcPThr, "FixedWin", NumberFormat.DecFormat(tblContent.Rows[2]["ibc_pt_hr"], 2));
                                TemplateHandler.Assign(ref _ibcPThr, "FixedPlace", NumberFormat.DecFormat(tblContent.Rows[3]["ibc_pt_hr"], 2));
                            }
                        }

                        TemplateHandler.Assign(ref _ibcPThr, "AUWin", NumberFormat.DecFormat(tblContent.Rows[4]["ibc_pt_hr"], 2));
                        TemplateHandler.Assign(ref _ibcPThr, "AUPlace", NumberFormat.DecFormat(tblContent.Rows[5]["ibc_pt_hr"], 2));
                    }
                }
                else
                    TemplateHandler.RemoveBlock(ref _ibcPThr, "Member_PT_HR");
                                
                if (_bettype != 0)
                {
                    if (_bettype < 33 || WebSettings.DisableHRPT_OpenTote)
                    {
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "Fixed");
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "AUTote");
                    }
                    if ((_bettype > 33 || WebSettings.DisableHRPT_Tote) && _bettype < 43)
                    {
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "Tote");
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "AUTote");
                    }
                    if ((_bettype > 43))
                    {
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "Fixed");
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "Tote");
                    }
                }
                else // All
                {
                    if (WebSettings.DisableHRPT_OpenTote)
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "Fixed");
                    if (WebSettings.DisableHRPT_Tote)
                        TemplateHandler.RemoveBlock(ref _ibcPThr, "Tote");
                }
            }
        }
        TemplateHandler.ReplaceBlock(ref _s, "IBC_PT_HR", _ibcPThr.ToString());
        
        // position taking
        string sTable = TemplateHandler.CopyBlock(_s, "tbl");        
        // Compare WebConfig before
        if (WebSettings.disableAutoM)
            TemplateHandler.RemoveBlock(ref sTable, "AutoMPT");
        else
            TemplateHandler.RemoveBlock(ref sTable, "DisableAutoMPT");
        // Compare Master's Status After
        if (!Sunplus.Accounting.Business.Security.Permissions.IsEnabledAutoMaster(!CastAide.AsBoolean(rInfo["disableautompt"], false)))
        {
            TemplateHandler.Assign(ref sTable, "disableFlag", "disableFlag");
            TemplateHandler.Assign(ref sTable, "disableAutoMPT", "disColorMPT");
        }
        else
        {
            TemplateHandler.Assign(ref sTable, "disableFlag", "OpenFlag");
            TemplateHandler.Assign(ref sTable, "disableAutoMPT", "");
        }
        for (int i = 1; i <5; i++)
        {
            if (roleID > i) TemplateHandler.RemoveBlock(ref sTable, "r" + i.ToString());
        }        
        string curRow = string.Empty;
        StringBuilder _sbTable = new StringBuilder();
        foreach (DataRow r in tblContent.Rows)
        {
            string _sTable = sTable;
            if (_sportID == 151)
            {
                int mBetType = CastAide.AsInt16(r["bettype"]);
                if (((WebSettings.DisableHRPT_Tote) && (mBetType == 31 || mBetType == 32)) || 
                    ((WebSettings.DisableHRPT_OpenTote) && (mBetType == 41 || mBetType == 42)))
                    goto tt;
                if (mBetType < 33)
                    TemplateHandler.Assign(ref _sTable, "typeName", Resources.Language.lblTote + " - " + HRPositionTakingManager.GetBettypeName(mBetType));
                else if(mBetType < 43)
                    TemplateHandler.Assign(ref _sTable, "typeName", Resources.Language.lblOpenTote + " - " + HRPositionTakingManager.GetBettypeName(mBetType));
                else if (mBetType < 53)
                    TemplateHandler.Assign(ref _sTable, "typeName", Resources.Language.lblAUTote + " - " + r["typename"]);
            }
            else                
                TemplateHandler.Assign(ref _sTable, "typeName", r["typename"].ToString());

            TemplateHandler.Assign(ref _sTable, "sportName",
                                   ((CastAide.AsInt32(r["sporttype"]) == Sunplus.Accounting.Core.Common.Constants.Sport.OtherSports &&
                                     CastAide.AsInt32(r["betType"]) == Sunplus.Accounting.Core.Common.Constants.BetType.MixParlay)
                                        ? Resources.Language.lblMixedSports
                                        : r["sportname"].ToString()));
            
            TemplateHandler.Assign(ref _sTable, "sportType", r["sporttype"].ToString());
            TemplateHandler.Assign(ref _sTable, "betType", r["bettype"].ToString());
            
            // super
            TemplateHandler.Assign(ref _sTable, "livegame", (CastAide.AsBoolean(r["islive"])) ? "LIVE" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "s4", NumberFormat.DecFormat(r["s4"], 2));
            TemplateHandler.Assign(ref _sTable, "m4", NumberFormat.DecFormat(r["m4"], 2));
            TemplateHandler.Assign(ref _sTable, "a4", NumberFormat.DecFormat(r["a4"], 2));
            TemplateHandler.Assign(ref _sTable, "n4", NumberFormat.DecFormat(r["n4"], 2));
            TemplateHandler.Assign(ref _sTable, "l4", NumberFormat.DecFormat(r["l4"], 2));
            
            // master
            TemplateHandler.Assign(ref _sTable, "s3", NumberFormat.DecFormat(r["s3"], 2));
            TemplateHandler.Assign(ref _sTable, "m3", NumberFormat.DecFormat(r["m3"], 2));
            TemplateHandler.Assign(ref _sTable, "u3", NumberFormat.DecFormat(r["u3"], 2));
            TemplateHandler.Assign(ref _sTable, "i3", (CastAide.AsBoolean(r["i3"],false)) ? " checked='checked'" :string.Empty);
            TemplateHandler.Assign(ref _sTable, "n3", NumberFormat.DecFormat(r["n3"], 2));
            TemplateHandler.Assign(ref _sTable, "l3", NumberFormat.DecFormat(r["l3"], 2));
                                                                                
            // agent                                                            
            TemplateHandler.Assign(ref _sTable, "m2", NumberFormat.DecFormat(r["m2"], 2));
            TemplateHandler.Assign(ref _sTable, "a2", NumberFormat.DecFormat(r["a2"], 2));
            TemplateHandler.Assign(ref _sTable, "u2", NumberFormat.DecFormat(r["um2"], 2));
            TemplateHandler.Assign(ref _sTable, "i2", (CastAide.AsBoolean(r["im2"], false)) ? " checked='checked'" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "um2", NumberFormat.DecFormat(r["um2"], 2));
            TemplateHandler.Assign(ref _sTable, "im2", (CastAide.AsBoolean(r["im2"], false)) ? " checked='checked'" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "n2", NumberFormat.DecFormat(r["n2"], 2));
            TemplateHandler.Assign(ref _sTable, "l2", NumberFormat.DecFormat(r["l2"], 2));
                                                                                
            // member                                                           
            TemplateHandler.Assign(ref _sTable, "s1", NumberFormat.DecFormat(r["s1"], 2));
            TemplateHandler.Assign(ref _sTable, "m1", NumberFormat.DecFormat(r["m1"], 2));
            TemplateHandler.Assign(ref _sTable, "a1", NumberFormat.DecFormat(r["a1"], 2));
            TemplateHandler.Assign(ref _sTable, "u1", NumberFormat.DecFormat(r["u1"], 2));
            TemplateHandler.Assign(ref _sTable, "i1", (CastAide.AsBoolean(r["i1"], false)) ? " checked='checked'" : string.Empty);
            TemplateHandler.Assign(ref _sTable, "um1", NumberFormat.DecFormat(r["um1"], 2));
            TemplateHandler.Assign(ref _sTable, "im1", (CastAide.AsBoolean(r["im1"], false)) ? " checked='checked'" : string.Empty);            
            _sbTable.Append(_sTable);

        tt:
            continue;
        }        
        TemplateHandler.ReplaceBlock(ref _s, "tbl", _sbTable.ToString());
        
        return _s;
    }

    public static string GenerateHTMLCommission(DbResult dr)
    {
        string _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/ViewPT_Comm.htm");
        DataRow rInfo = dr.ResultSet.Tables[0].Rows[0];
        DataRow rComm = dr.ResultSet.Tables[2].Rows[0];                
        //Phuong.Nguyen <2008-12-23>
        //Assign header 
        if (!WebSettings.Disable1x2Comm)
            TemplateHandler.Assign(ref _s, "lbl1X2", Resources.Language.lbl1x2);
        else
            TemplateHandler.RemoveBlock(ref _s, "1X2");
        
        if (!WebSettings.DisableHRPT_Tote)
            TemplateHandler.Assign(ref _s, "lblTote", Resources.Language.lblTote);
        else
            TemplateHandler.RemoveBlock(ref _s, "Tote");
        
        if (!WebSettings.DisableHRPT_OpenTote)
            TemplateHandler.Assign(ref _s, "lblFixed", Resources.Language.lblOpenTote);        
        else
            TemplateHandler.RemoveBlock(ref _s, "Fixed");
        
        //Kiet Mai - add Game comm
        if (Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled)
            TemplateHandler.Assign(ref _s, "{lblGameComm}", Resources.Language.lblGameComm);
        else
            TemplateHandler.RemoveBlock(ref _s, "numbergame");

        if (WebSettings.disabledFI) TemplateHandler.RemoveBlock(ref _s, "Finan");
            
        int roleID = CastAide.AsInt32(rInfo["roleid"], 1);
                          
        // information
        string _sUpline = TemplateHandler.CopyBlock(_s, "upline");
        string curUpline = string.Empty;
        StringBuilder _sbUpline = new StringBuilder();
        if (roleID < 2)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "AGENT");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["Ag"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["AgID"].ToString());
            _sbUpline.Append(curUpline);
            
        }
        if (roleID < 3)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "MASTER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["MAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["MAgID"].ToString());
            _sbUpline.Append(curUpline);

            
        }
        if (roleID < 4)
        {
            curUpline = _sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "SUPER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["SAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["SAgID"].ToString());
            _sbUpline.Append(curUpline);            
        }
        TemplateHandler.ReplaceBlock(ref _s, "upline", _sbUpline.ToString());
        TemplateHandler.Assign(ref _s, "CustName", rInfo["username"].ToString());
        TemplateHandler.Assign(ref _s, "CustID", rInfo["custid"].ToString());
        
        // commission
        string _sRow = TemplateHandler.CopyBlock(_s, "row");
        string curRow = string.Empty;
        StringBuilder _sbRow = new StringBuilder();
        if (roleID <= 4)
        {
            curRow = _sRow;
            TemplateHandler.Assign(ref curRow, "RoleName", "Super");
            TemplateHandler.Assign(ref curRow, "ga", NumberFormat.DecFormat(rComm["ga4"], 4));
            TemplateHandler.Assign(ref curRow, "gb", NumberFormat.DecFormat(rComm["gb4"], 4));
            TemplateHandler.Assign(ref curRow, "gc", NumberFormat.DecFormat(rComm["gc4"], 4));
            TemplateHandler.Assign(ref curRow, "gd", NumberFormat.DecFormat(rComm["gd4"], 4));
            TemplateHandler.Assign(ref curRow, "#", NumberFormat.DecFormat(rComm["othercomm4"], 4));
            if (!WebSettings.Disable1x2Comm) TemplateHandler.Assign(ref curRow, "1X2", NumberFormat.DecFormat(rComm["comm_5_4"], 4));
            if (!WebSettings.DisableHRPT_Tote) TemplateHandler.Assign(ref curRow, "Tote", NumberFormat.DecFormat(rComm["v151_comm_31_4"], 4));
            if (!WebSettings.DisableHRPT_OpenTote) TemplateHandler.Assign(ref curRow, "Fixed", NumberFormat.DecFormat(rComm["v151_comm_41_4"], 4));
            TemplateHandler.Assign(ref curRow, "ficomm", NumberFormat.DecFormat(rComm["v201_comm_3_4"], 4));    
            //Kiet Mai - add Game comm        
            if (Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled) TemplateHandler.Assign(ref curRow, "gamecomm", NumberFormat.DecFormat(rComm["v161_comm_81_4"], 4));
            _sbRow.Append(curRow);
        }
        if (roleID <= 3)
        {
            curRow = _sRow;
            TemplateHandler.Assign(ref curRow, "RoleName", "Master");
            TemplateHandler.Assign(ref curRow, "ga", NumberFormat.DecFormat(rComm["ga3"], 4));
            TemplateHandler.Assign(ref curRow, "gb", NumberFormat.DecFormat(rComm["gb3"], 4));
            TemplateHandler.Assign(ref curRow, "gc", NumberFormat.DecFormat(rComm["gc3"], 4));
            TemplateHandler.Assign(ref curRow, "gd", NumberFormat.DecFormat(rComm["gd3"], 4));
            TemplateHandler.Assign(ref curRow, "#", NumberFormat.DecFormat(rComm["othercomm3"], 4));
            if (!WebSettings.Disable1x2Comm) TemplateHandler.Assign(ref curRow, "1X2", NumberFormat.DecFormat(rComm["comm_5_3"], 4));
            if (!WebSettings.DisableHRPT_Tote) TemplateHandler.Assign(ref curRow, "Tote", NumberFormat.DecFormat(rComm["v151_comm_31_3"], 4));
            if (!WebSettings.DisableHRPT_OpenTote) TemplateHandler.Assign(ref curRow, "Fixed", NumberFormat.DecFormat(rComm["v151_comm_41_3"], 4));
            TemplateHandler.Assign(ref curRow, "ficomm", NumberFormat.DecFormat(rComm["v201_comm_3_3"], 4));
            //Kiet Mai - add Game comm        
            if (Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled) TemplateHandler.Assign(ref curRow, "gamecomm", NumberFormat.DecFormat(rComm["v161_comm_81_3"], 4));
            _sbRow.Append(curRow);
        }
        if (roleID <= 2)
        {
            curRow = _sRow;
            TemplateHandler.Assign(ref curRow, "RoleName", "Agent");
            TemplateHandler.Assign(ref curRow, "ga", NumberFormat.DecFormat(rComm["ga2"], 4));
            TemplateHandler.Assign(ref curRow, "gb", NumberFormat.DecFormat(rComm["gb2"], 4));
            TemplateHandler.Assign(ref curRow, "gc", NumberFormat.DecFormat(rComm["gc2"], 4));
            TemplateHandler.Assign(ref curRow, "gd", NumberFormat.DecFormat(rComm["gd2"], 4));
            TemplateHandler.Assign(ref curRow, "#", NumberFormat.DecFormat(rComm["othercomm2"], 4));
            if (!WebSettings.Disable1x2Comm) TemplateHandler.Assign(ref curRow, "1X2", NumberFormat.DecFormat(rComm["comm_5_2"], 4));
            if (!WebSettings.DisableHRPT_Tote) TemplateHandler.Assign(ref curRow, "Tote", NumberFormat.DecFormat(rComm["v151_comm_31_2"], 4));
            if (!WebSettings.DisableHRPT_OpenTote) TemplateHandler.Assign(ref curRow, "Fixed", NumberFormat.DecFormat(rComm["v151_comm_41_2"], 4));
            TemplateHandler.Assign(ref curRow, "ficomm", NumberFormat.DecFormat(rComm["v201_comm_3_2"], 4));
            //Kiet Mai - add Game comm        
            if (Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled) TemplateHandler.Assign(ref curRow, "gamecomm", NumberFormat.DecFormat(rComm["v161_comm_81_2"], 4));
            _sbRow.Append(curRow);
        }
        if (roleID == 1)
        {
            curRow = _sRow;
            string _group = rComm["ugroup"].ToString().ToLower();
            if (_group != "") _group = _group.Substring(_group.Length - 1, 1);
            TemplateHandler.Assign(ref curRow, "g" + _group, NumberFormat.DecFormat(rComm["comm"], 4));
            TemplateHandler.Assign(ref curRow, "RoleName", "Member");
            TemplateHandler.Assign(ref curRow, "ga", string.Empty);
            TemplateHandler.Assign(ref curRow, "gb", string.Empty);
            TemplateHandler.Assign(ref curRow, "gc", string.Empty);
            TemplateHandler.Assign(ref curRow, "gd", string.Empty);
            TemplateHandler.Assign(ref curRow, "#", NumberFormat.DecFormat(rComm["othercomm"], 4));
            if (!WebSettings.Disable1x2Comm) TemplateHandler.Assign(ref curRow, "1X2", NumberFormat.DecFormat(rComm["comm_5"], 4));
            if (!WebSettings.DisableHRPT_Tote) TemplateHandler.Assign(ref curRow, "Tote", NumberFormat.DecFormat(rComm["v151_comm_31"], 4));
            if (!WebSettings.DisableHRPT_OpenTote) TemplateHandler.Assign(ref curRow, "Fixed", NumberFormat.DecFormat(rComm["v151_comm_41"], 4));
            TemplateHandler.Assign(ref curRow, "ficomm", NumberFormat.DecFormat(rComm["v201_comm_3"], 4));
            //Kiet Mai - add Game comm    
            if (Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled) TemplateHandler.Assign(ref curRow, "gamecomm", NumberFormat.DecFormat(rComm["v161_comm_81"], 4));
            _sbRow.Append(curRow);
        }
        TemplateHandler.ReplaceBlock(ref _s, "row", _sbRow.ToString());           
        return _s;
    }
    
   

    //Kiet Mai - Generate Parlay Mixed Sport
    public static string GenerateHTMLParlayMixedSportsPT(DbResult dr, int sportType)
    {
        var s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/ViewPT/ViewPT_NumberGame.htm");
        var rInfo = dr.ResultSet.Tables[0].Rows[0];
        var tblContent = dr.ResultSet.Tables[1];

        var roleID = CastAide.AsInt32(rInfo["roleid"], 1);

        // information
        var sUpline = TemplateHandler.CopyBlock(s, "upline");
        string curUpline;
        var sbUpline = new StringBuilder();
        if (roleID < 2)
        {
            curUpline = sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "AGENT");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["Ag"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["AgID"].ToString());
            sbUpline.Append(curUpline);

        }
        if (roleID < 3)
        {
            curUpline = sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "MASTER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["MAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["MAgID"].ToString());
            sbUpline.Append(curUpline);


        }
        if (roleID < 4)
        {
            curUpline = sUpline;
            TemplateHandler.Assign(ref curUpline, "Role", "SUPER");
            TemplateHandler.Assign(ref curUpline, "Name", rInfo["SAg"].ToString());
            TemplateHandler.Assign(ref curUpline, "ID", rInfo["SAgID"].ToString());
            sbUpline.Append(curUpline);
        }
        TemplateHandler.ReplaceBlock(ref s, "upline", sbUpline.ToString());
        TemplateHandler.Assign(ref s, "CustName", rInfo["username"].ToString());
        TemplateHandler.Assign(ref s, "CustID", rInfo["custid"].ToString());

        // position taking
        var sTable = TemplateHandler.CopyBlock(s, "tbl");
        // Compare WebConfig before
        TemplateHandler.RemoveBlock(ref sTable, WebSettings.disableAutoM ? "AutoMPT" : "DisableAutoMPT");
        // Compare Master's Status After
        if (!Sunplus.Accounting.Business.Security.Permissions.IsEnabledAutoMaster(!CastAide.AsBoolean(rInfo["disableautompt"], false)))
        {
            TemplateHandler.Assign(ref sTable, "disableFlag", "disableFlag");
            TemplateHandler.Assign(ref sTable, "disableAutoMPT", "disColorMPT");
        }
        else
        {
            TemplateHandler.Assign(ref sTable, "disableFlag", "OpenFlag");
            TemplateHandler.Assign(ref sTable, "disableAutoMPT", "");
        }
        for (var i = 1; i < 5; i++)
        {
            if (roleID > i) TemplateHandler.RemoveBlock(ref sTable, "r" + i.ToString());
        }
        var sbTable = new StringBuilder();
        foreach (DataRow r in tblContent.Rows)
        {
            var _sTable = sTable;
            if ((sportType == 999 && CastAide.AsInt32(r["bettype"].ToString()) == Sunplus.Accounting.Core.Common.Constants.BetType.MixParlay) ||
                (sportType == Sunplus.Accounting.Core.Common.Constants.Sport.OtherSports && CastAide.AsInt32(r["bettype"].ToString()) != Sunplus.Accounting.Core.Common.Constants.BetType.MixParlay))
            {
                TemplateHandler.Assign(ref _sTable, "typeName", r["typename"].ToString());
                TemplateHandler.Assign(ref _sTable, "sportName", sportType == 99 ? r["sportname"] : Resources.Language.lblMixedSports);
                TemplateHandler.Assign(ref _sTable, "sportType", r["sporttype"].ToString());
                TemplateHandler.Assign(ref _sTable, "betType", r["bettype"].ToString());

                // super
                TemplateHandler.Assign(ref _sTable, "livegame",
                                       (CastAide.AsBoolean(r["islive"])) ? "LIVE" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "s4", NumberFormat.DecFormat(r["s4"], 2));
                TemplateHandler.Assign(ref _sTable, "m4", NumberFormat.DecFormat(r["m4"], 2));
                TemplateHandler.Assign(ref _sTable, "a4", NumberFormat.DecFormat(r["a4"], 2));
                TemplateHandler.Assign(ref _sTable, "n4", NumberFormat.DecFormat(r["n4"], 2));
                TemplateHandler.Assign(ref _sTable, "l4", NumberFormat.DecFormat(r["l4"], 2));

                // master
                TemplateHandler.Assign(ref _sTable, "s3", NumberFormat.DecFormat(r["s3"], 2));
                TemplateHandler.Assign(ref _sTable, "m3", NumberFormat.DecFormat(r["m3"], 2));
                TemplateHandler.Assign(ref _sTable, "u3", NumberFormat.DecFormat(r["u3"], 2));
                TemplateHandler.Assign(ref _sTable, "i3",
                                       (CastAide.AsBoolean(r["i3"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "n3", NumberFormat.DecFormat(r["n3"], 2));
                TemplateHandler.Assign(ref _sTable, "l3", NumberFormat.DecFormat(r["l3"], 2));

                // agent                                                            
                TemplateHandler.Assign(ref _sTable, "m2", NumberFormat.DecFormat(r["m2"], 2));
                TemplateHandler.Assign(ref _sTable, "a2", NumberFormat.DecFormat(r["a2"], 2));
                TemplateHandler.Assign(ref _sTable, "u2", NumberFormat.DecFormat(r["um2"], 2));
                TemplateHandler.Assign(ref _sTable, "i2",
                                       (CastAide.AsBoolean(r["im2"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "um2", NumberFormat.DecFormat(r["um2"], 2));
                TemplateHandler.Assign(ref _sTable, "im2",
                                       (CastAide.AsBoolean(r["im2"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "n2", NumberFormat.DecFormat(r["n2"], 2));
                TemplateHandler.Assign(ref _sTable, "l2", NumberFormat.DecFormat(r["l2"], 2));

                // member                                                           
                TemplateHandler.Assign(ref _sTable, "s1", NumberFormat.DecFormat(r["s1"], 2));
                TemplateHandler.Assign(ref _sTable, "m1", NumberFormat.DecFormat(r["m1"], 2));
                TemplateHandler.Assign(ref _sTable, "a1", NumberFormat.DecFormat(r["a1"], 2));
                TemplateHandler.Assign(ref _sTable, "u1", NumberFormat.DecFormat(r["u1"], 2));
                TemplateHandler.Assign(ref _sTable, "i1",
                                       (CastAide.AsBoolean(r["i1"], false)) ? " checked='checked'" : string.Empty);
                TemplateHandler.Assign(ref _sTable, "um1", NumberFormat.DecFormat(r["um1"], 2));
                TemplateHandler.Assign(ref _sTable, "im1",
                                       (CastAide.AsBoolean(r["im1"], false)) ? " checked='checked'" : string.Empty);
                sbTable.Append(_sTable);    
            }
            
        }
        TemplateHandler.ReplaceBlock(ref s, "tbl", sbTable.ToString());

        return s;
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}

