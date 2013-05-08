using System;
using System.Data;
using System.Collections;
using System.Web.Security;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Business.PT.SportBooks;

public partial class Customer_EditMinMaxPT : RequiredAuthorizationPage
{

    public StringBuilder html = new StringBuilder();
    public string strtitle = "";
    #region decalre variable
    protected string fromSet = "", msg="";
    protected string ControlHandler = "InsertNewSuper_v2.ashx";
    public int custId = 0;
    //variable create interface
    public StringBuilder content = new StringBuilder();
    #endregion

    public string strType = "";
    string HiddenSoccer, HiddenBasketball, HiddenFootball, HiddenTennis, HiddenBaseball, HiddenGolf, HiddenMotosports, HiddenNumberGame, HiddenFinancial, HiddenOtherSports, HiddenMixedSports; 
    protected void Page_Load(object sender, EventArgs e)
    {
        string type = Request["type"] as string ?? string.Empty;
        if (type == "Super")
        {
            Page.Title = "Edit Min Position Taking";
            strtitle = "Update Min Position Taking";
        }
        else
        { 
            Page.Title = "Edit Min, Max Position Taking";
            strtitle = "Update Min/Max PT";
        }
        //--------------------------v2---------------------//
        //msg = Request["msg"] as string ?? string.Empty;
        //if (msg != "")
        //{
        //    msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success);
        //}
        
        fromSet = Request["set"] as string ?? string.Empty;
        if (Session["AccId"] != null) LoadEditMinMaxPT();
        else Response.Redirect(FormsAuthentication.LoginUrl);        
    }


    
    //-------------------------v2 khoa.tran--------------------------// 
    #region LoadEditMinMaxPT()
    void LoadEditMinMaxPT()
    {
        try
        {
            DataRow dr = null;
            DbResult ds = SelectCustomer.GetMaxMinPositionTaking_acc(Request["type"], Request["custId"]);
            if(ds.ResultSet.Tables[0].Rows.Count>0) dr = ds.ResultSet.Tables[0].Rows[0];
            string bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/MenuEditMinMaxPT.htm");
            TemplateHandler.Assign(ref bodyTpl, "{soccer-text}", Resources.Language.lblSoccer);
            TemplateHandler.Assign(ref bodyTpl, "{basket-text}", Resources.Language.lblBasketball);
            TemplateHandler.Assign(ref bodyTpl, "{football-text}", Resources.Language.lblFootball);
            TemplateHandler.Assign(ref bodyTpl, "{tennis-text}", Resources.Language.lblTennis);
            TemplateHandler.Assign(ref bodyTpl, "{baseball-text}", Resources.Language.lblBaseball);
            TemplateHandler.Assign(ref bodyTpl, "{golf-text}", Resources.Language.lblGolf);
            TemplateHandler.Assign(ref bodyTpl, "{moto-text}", Resources.Language.lblMotosports);
            TemplateHandler.Assign(ref bodyTpl, "{othersports-text}", Resources.Language.lblOthersports);
            TemplateHandler.Assign(ref bodyTpl, "{add-text}", Resources.Language.Update );
            TemplateHandler.Assign(ref bodyTpl, "{add-id}", "btnAdd");
            TemplateHandler.Assign(ref bodyTpl, "{copy-all-text}", Resources.Language.lblCopyAllPTfromHdp_MinMax);
            TemplateHandler.Assign(ref bodyTpl, "{copy-basket-text}", Resources.Language.lblCopyBasketHdp_MinMax);
            TemplateHandler.Assign(ref bodyTpl, "{NumberGame-text}", Resources.Language.lblNumberGame );
            //mixed sports
            TemplateHandler.Assign(ref bodyTpl, "{mixedsport-text}", Resources.Language.lblMixedSports);

            strType = Request["type"].ToLower();
            string prefix = string.Empty;
            switch (strType.ToLower())
            {
                case "super":
                    prefix = "s";
                    Page.Title = "Edit Min Position Taking";
                    break;
                case "master":
                    prefix = "m";
                    break;
                case "agent":
                    prefix = "a";
                    break;
                default:
                    break;
            }
            TemplateHandler.Assign(ref bodyTpl, "{soccerPT}", GenContentSoccerPT_v2(dr, prefix));
            TemplateHandler.Assign(ref bodyTpl, "{basketPT}", GenContentBasketballPT_v2("2", dr, prefix)); 
            TemplateHandler.Assign(ref bodyTpl, "{footballPT}", GenContentBasketballPT_v2("3", dr, prefix));
            TemplateHandler.Assign(ref bodyTpl, "{tennisPT}", GenContentBasketballPT_v2("5", dr, prefix));
            TemplateHandler.Assign(ref bodyTpl, "{baseballPT}", GenContentBaseballPT_v2("8", dr, prefix));
            TemplateHandler.Assign(ref bodyTpl, "{golfPT}", GenContentBasketballPT_v2("10", dr, prefix)); 
            TemplateHandler.Assign(ref bodyTpl, "{motoPT}", GenContentMotosportsPT_v2("11", dr, prefix));
            if (Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled)
                TemplateHandler.Assign(ref bodyTpl, "{NumberGamePT}", GenContentNumberGamePT_v2(dr, prefix));
            else
                TemplateHandler.RemoveBlock(ref bodyTpl, "NumberGame");
            //mixed sports
            TemplateHandler.Assign(ref bodyTpl, "{mixedsportPT}", GenMixedSports(dr, prefix));
            
            TemplateHandler.Assign(ref bodyTpl, "{otherPT}", GenContentBasketballPT_v2("99", dr, prefix));

            html.Append(AssignDataInfo(dr));
            html.Append(bodyTpl);

            //------ add hidden variable to show max value in ddl min PT (edit min max PT of master & agent)-----
            html.Append(HiddenSoccer);
            html.Append(HiddenBasketball);
            html.Append(HiddenTennis);
            html.Append(HiddenGolf);
            html.Append(HiddenFootball);
            html.Append(HiddenOtherSports);
            html.Append(HiddenBaseball);
            html.Append(HiddenMotosports);
            //mixed sports
            html.Append(HiddenMixedSports);
            //numbergame
            html.Append(HiddenNumberGame);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }
    #endregion

    #region Load AccInfoMem template
    string AssignDataInfo(DataRow dr)
    {
        string strAccInfo = "";
        try
        {
            strAccInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/AccInfoMinPaxPT.htm");
            TemplateHandler.Assign(ref strAccInfo, "{colsmaxcredit}", "1");
            string type = Request["type"].ToLower();
            switch (type)
            {
                case "super": type = "Super: "; break;
                case "master": type = "Master: "; break;
                case "agent": type = "Agent: "; break;
            }
            TemplateHandler.Assign(ref strAccInfo, "{add-edit-agent}", type + CastAide.AsString( dr["username"]));
            TemplateHandler.Assign(ref strAccInfo, "{firstname}", CastAide.AsString(dr["firstname"]));
            TemplateHandler.Assign(ref strAccInfo, "{lastname}", CastAide.AsString(dr["lastname"]));
            TemplateHandler.Assign(ref strAccInfo, "{phone}", CastAide.AsString(dr["phone"]));
            TemplateHandler.Assign(ref strAccInfo, "{mobile}", CastAide.AsString(dr["mobilephone"]));            
            TemplateHandler.Assign(ref strAccInfo, "{fax}", CastAide.AsString(dr["fax"]));           

            TemplateHandler.Assign(ref strAccInfo, "{edit-hidden}", "");
            TemplateHandler.Assign(ref strAccInfo, "{username}", CastAide.AsString(dr["username"]));

            custId = CastAide.AsInt32(Request["custId"]);
            TemplateHandler.Assign(ref strAccInfo, "{cust-id}", custId.ToString());
            
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return strAccInfo;
    }
    #endregion

    #region GenContentSoccerPT_v2
    private string GenContentSoccerPT_v2(DataRow dr, string prefix)
    {
        StringBuilder html = new StringBuilder();
        string max = "_Max";
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/SoccerPositionTakingRow.htm");
            #region local variables minimum of min PT
            double HdpMin = 0, OuMin = 0, lstHdpMin = 0, lstOuMin = 0, OeMin = 0, lx2Min = 0, CsMin = 0, TgMin = 0, MpMin = 0, OtMin = 0;
            double LiveHdpMin = 0, LiveOuMin = 0, Live1stHdpMin = 0, Live1stOuMin = 0;
            #endregion
            #region local variables maximum of min PT
            double HdpMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_1"]);
            double OuMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_3"]);
            double lstHdpMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_7"]);
            double lstOuMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_8"]);
            double OeMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_2"]);
            double lx2Max = FormatHandler.GetDouble(dr[prefix + "positiontaking_5"]);
            double CsMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_4"]);
            double TgMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_6"]);
            double MpMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_9"]);
            double OtMax = FormatHandler.GetDouble(dr[prefix + "positiontaking_10"]);
            double LiveHdpMax = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_1"]);
            double LiveOuMax = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_3"]);
            double Live1stHdpMax = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_7"]);
            double Live1stOuMax = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_8"]);

            Hashtable hidden = new Hashtable();
            hidden.Add("listMHdpD1" + max, HdpMax);
            hidden.Add("listMOUD1" + max, OuMax);
            hidden.Add("listM1stHdpD1" + max, lstHdpMax);
            hidden.Add("listM1stOUD1" + max, lstOuMax);
            hidden.Add("listMOE1" + max, OeMax);
            hidden.Add("listM1x21" + max, lx2Max);
            hidden.Add("listMCS1" + max, CsMax);
            hidden.Add("listMTG1" + max, TgMax);
            hidden.Add("listMMP1" + max, MpMax);
            hidden.Add("listMOutright1" + max, OtMax);
            hidden.Add("listMHdpL1" + max, LiveHdpMax);
            hidden.Add("listMOUL1" + max, LiveOuMax);
            hidden.Add("listM1stHdpL1" + max, Live1stHdpMax);
            hidden.Add("listM1stOUL1" + max, Live1stOuMax);
            HiddenSoccer = CustomerUtils.AssignMultipleHidden(hidden);

            #endregion
            #region local variables current value of min PT
            double Hdp = FormatHandler.GetDouble(dr[prefix + "positiontaking_1_min"]);
            double Ou = FormatHandler.GetDouble(dr[prefix + "positiontaking_3_min"]);
            double lstHdp = FormatHandler.GetDouble(dr[prefix + "positiontaking_7_min"]);
            double lstOu = FormatHandler.GetDouble(dr[prefix + "positiontaking_8_min"]);
            double Oe = FormatHandler.GetDouble(dr[prefix + "positiontaking_2_min"]);
            double lx2 = FormatHandler.GetDouble(dr[prefix + "positiontaking_5_min"]);
            double Cs = FormatHandler.GetDouble(dr[prefix + "positiontaking_4_min"]);
            double Tg = FormatHandler.GetDouble(dr[prefix + "positiontaking_6_min"]);
            double Mp = FormatHandler.GetDouble(dr[prefix + "positiontaking_9_min"]);
            double Ot = FormatHandler.GetDouble(dr[prefix + "positiontaking_10_min"]);
            double LiveHdp = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_1_min"]);
            double LiveOu = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_3_min"]);
            double Live1stHdp = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_7_min"]);
            double Live1stOu = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_8_min"]);
            #endregion
            if (prefix == "s")
            {
              //  HiddenSoccer = "";
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt1");
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt2");
                TemplateHandler.RemoveBlock(ref strContentTpl, "master-pt1");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt1");
                TemplateHandler.RemoveBlock(ref strContentTpl, "master-pt2");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt2");
                TemplateHandler.RemoveBlock(ref strContentTpl, "super-mix-max");
                TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "1");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMinPosition);
                TemplateHandler.Assign(ref strContentTpl, "{super-live-position}", Resources.Language.lblMinPosition);

                TemplateHandler.Assign(ref strContentTpl, "{none-tlposition}", "none");

                #region display max value Soccer Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpDValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOUDValue}", CustomerUtils.GetListPositionTaking(OuMin, OuMax, Ou));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stHdpDValue}", CustomerUtils.GetListPositionTaking(lstHdpMin, lstHdpMax, lstHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stOUDValue}", CustomerUtils.GetListPositionTaking(lstOuMin, lstOuMax, lstOu));
                TemplateHandler.Assign(ref strContentTpl, "{listSOEValue}", CustomerUtils.GetListPositionTaking(OeMin, OeMax, Oe));
                TemplateHandler.Assign(ref strContentTpl, "{listS1x2Value}", CustomerUtils.GetListPositionTaking(lx2Min, lx2Max, lx2));
                TemplateHandler.Assign(ref strContentTpl, "{listSCSValue}", CustomerUtils.GetListPositionTaking(CsMin, CsMax, Cs));
                TemplateHandler.Assign(ref strContentTpl, "{listSTGValue}", CustomerUtils.GetListPositionTaking(TgMin, TgMax, Tg));
                TemplateHandler.Assign(ref strContentTpl, "{listSMPValue}", CustomerUtils.GetListPositionTaking(MpMin, MpMax, Mp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOutrightValue}", CustomerUtils.GetListPositionTaking(OtMin, OtMax, Ot));
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpLValue}", CustomerUtils.GetListPositionTaking(LiveHdpMin, LiveHdpMax, LiveHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOULValue}", CustomerUtils.GetListPositionTaking(LiveOuMin, LiveOuMax, LiveOu));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stHdpLValue}", CustomerUtils.GetListPositionTaking(Live1stHdpMin, Live1stHdpMax, Live1stHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stOULValue}", CustomerUtils.GetListPositionTaking(Live1stOuMin, Live1stOuMax, Live1stOu));
                #endregion
            }
            else// this case for Edit Master, Agent (it can edit both Min, Max Position Taking)
            {
               TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "2");
                
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt1");
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt1");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt2");
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt2");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMaxPosition);
                TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.lblMinPosition);
                TemplateHandler.Assign(ref strContentTpl, "{super-live-position}", Resources.Language.lblMaxPosition);
                TemplateHandler.Assign(ref strContentTpl, "{master-live-position}", Resources.Language.lblMinPosition);

                #region local variables maximum of Max PT
                double mHdpMax = 0.9, mOuMax = 0.9, mlstHdpMax = 0.9, mlstOuMax = 0.9, mOeMax = 0.9, mlx2Max = 0.9, mCsMax = 0.9, mTgMax = 0.9, mMpMax = 0.9, mOtMax = 0.9;
                double mLiveHdpMax = 0.9, mLiveOuMax = 0.9, mLive1stHdpMax = 0.9, mLive1stOuMax = 0.9;
                #endregion 
                #region local variables current of Max PT
                double mHdp = FormatHandler.GetDouble(dr[prefix + "positiontaking_1_max"]);
                double mOu = FormatHandler.GetDouble(dr[prefix + "positiontaking_3_max"]);
                double mlstHdp = FormatHandler.GetDouble(dr[prefix + "positiontaking_7_max"]);
                double mlstOu = FormatHandler.GetDouble(dr[prefix + "positiontaking_8_max"]);
                double mOe = FormatHandler.GetDouble(dr[prefix + "positiontaking_2_max"]);
                double mlx2 = FormatHandler.GetDouble(dr[prefix + "positiontaking_5_max"]);
                double mCs = FormatHandler.GetDouble(dr[prefix + "positiontaking_4_max"]);
                double mTg = FormatHandler.GetDouble(dr[prefix + "positiontaking_6_max"]);
                double mMp = FormatHandler.GetDouble(dr[prefix + "positiontaking_9_max"]);
                double mOt = FormatHandler.GetDouble(dr[prefix + "positiontaking_10_max"]);
                double mLiveHdp = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_1_max"]);
                double mLiveOu = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_3_max"]);
                double mLive1stHdp = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_7_max"]);
                double mLive1stOu = FormatHandler.GetDouble(dr[prefix + "livepositiontaking_8_max"]);
                #endregion

                #region display  value Soccer Position taking dropdowns of Max Position  (from min Value to 0.9)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpDValue}", CustomerUtils.GetListPositionTaking(Hdp, mHdpMax, mHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOUDValue}", CustomerUtils.GetListPositionTaking(Ou, mOuMax, mOu));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stHdpDValue}", CustomerUtils.GetListPositionTaking(lstHdp, mlstHdpMax, mlstHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stOUDValue}", CustomerUtils.GetListPositionTaking(lstOu, mlstOuMax, mlstOu));
                TemplateHandler.Assign(ref strContentTpl, "{listSOEValue}", CustomerUtils.GetListPositionTaking(Oe, mOeMax, mOe));
                TemplateHandler.Assign(ref strContentTpl, "{listS1x2Value}", CustomerUtils.GetListPositionTaking(lx2, mlx2Max, mlx2));
                TemplateHandler.Assign(ref strContentTpl, "{listSCSValue}", CustomerUtils.GetListPositionTaking(Cs, mCsMax, mCs));
                TemplateHandler.Assign(ref strContentTpl, "{listSTGValue}", CustomerUtils.GetListPositionTaking(Tg, mTgMax, mTg));
                TemplateHandler.Assign(ref strContentTpl, "{listSMPValue}", CustomerUtils.GetListPositionTaking(Mp, mMpMax, mMp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOutrightValue}", CustomerUtils.GetListPositionTaking(Ot, mOtMax, mOt));
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpLValue}", CustomerUtils.GetListPositionTaking(LiveHdp, mLiveHdpMax, mLiveHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOULValue}", CustomerUtils.GetListPositionTaking(LiveOu, mLiveOuMax, mLiveOu));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stHdpLValue}", CustomerUtils.GetListPositionTaking(Live1stHdp, mLive1stHdpMax, mLive1stHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listS1stOULValue}", CustomerUtils.GetListPositionTaking(Live1stOu, mLive1stOuMax, mLive1stOu));
                #endregion
                #region display  value Soccer Position taking dropdowns of Min Position (from 0 to value Position Taking)    
                TemplateHandler.Assign(ref strContentTpl, "{listMHdpDValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listMOUDValue}", CustomerUtils.GetListPositionTaking(OuMin, OuMax, Ou));
                TemplateHandler.Assign(ref strContentTpl, "{listM1stHdpDValue}", CustomerUtils.GetListPositionTaking(lstHdpMin, lstHdpMax, lstHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listM1stOUDValue}", CustomerUtils.GetListPositionTaking(lstOuMin, lstOuMax, lstOu));
                TemplateHandler.Assign(ref strContentTpl, "{listMOEValue}", CustomerUtils.GetListPositionTaking(OeMin, OeMax, Oe));
                TemplateHandler.Assign(ref strContentTpl, "{listM1x2Value}", CustomerUtils.GetListPositionTaking(lx2Min, lx2Max, lx2));
                TemplateHandler.Assign(ref strContentTpl, "{listMCSValue}", CustomerUtils.GetListPositionTaking(CsMin, CsMax, Cs));
                TemplateHandler.Assign(ref strContentTpl, "{listMTGValue}", CustomerUtils.GetListPositionTaking(TgMin, TgMax, Tg));
                TemplateHandler.Assign(ref strContentTpl, "{listMMPValue}", CustomerUtils.GetListPositionTaking(MpMin, MpMax, Mp));
                TemplateHandler.Assign(ref strContentTpl, "{listMOutrightValue}", CustomerUtils.GetListPositionTaking(OtMin, OtMax, Ot));
                TemplateHandler.Assign(ref strContentTpl, "{listMHdpLValue}", CustomerUtils.GetListPositionTaking(LiveHdpMin, LiveHdpMax, LiveHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listMOULValue}", CustomerUtils.GetListPositionTaking(LiveOuMin, LiveOuMax, LiveOu));
                TemplateHandler.Assign(ref strContentTpl, "{listM1stHdpLValue}", CustomerUtils.GetListPositionTaking(Live1stHdpMin, Live1stHdpMax, Live1stHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listM1stOULValue}", CustomerUtils.GetListPositionTaking(Live1stOuMin, Live1stOuMax, Live1stOu));
                #endregion
            }

            html.Append(CustomerUtils.LoadSoccerPTHeader_v2(false, true, "1", null, prefix));
            
            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
    #endregion

    #region GenContentBasketballPT_v2 : this function use for Basketball, Football, Tennis, Golf, OtherSport
    private string GenContentBasketballPT_v2(string sporttype, DataRow dr, string prefix)
    {
        StringBuilder html = new StringBuilder();
        string pre = "_Max";
        Hashtable hidden = new Hashtable();
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/BasketballPositionTakingRow.htm"); ;
            TemplateHandler.Assign(ref strContentTpl, "{sporttype}", sporttype);                       
            html.Append(CustomerUtils.LoadBasketballPTHeader_v2(false, true, sporttype, null, prefix));

            //Kiet Mai - remove mix parlay
            //if (sporttype != "2")
            //{
            //    TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlays");
            //    TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaym");
            //    TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaya");
            //}
            TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlays");
            TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaym");
            TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaya");

            #region local variables minimum of min PT
            double HdpMin = 0, OuMin = 0, OeMin = 0, MPMin = 0, MLMin = 0;
            double HdpMax = 0, OuMax = 0, OeMax = 0, MPMax = 0, MLMax = 0;
            #endregion
            #region local variables maximum of min PT
            if (dr != null)
            {
                HdpMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1"]);
                OuMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_3"]);
                OeMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_2"]);
                //Kiet Mai - remove mix parlay
                //if (sporttype == "2")
                //{
                //    MPMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_9"]);
                //}
                MLMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20"]);
                
                hidden.Add("listMHdpB" + sporttype + pre, HdpMax);
                hidden.Add("listMOUB" + sporttype + pre, OuMax);
                hidden.Add("listMOEB" + sporttype + pre, OeMax);
                hidden.Add("listMMPB" + sporttype + pre, MPMax);
                hidden.Add("listMMLB" + sporttype + pre, MLMax);
            }
            #endregion

            #region local variables current value of min PT
            double Hdp = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1_min"]);
            double Ou = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_3_min"]);
            double Oe = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_2_min"]);
            double MP = 0;
            //Kiet Mai - remove mix parlay
            //if (sporttype == "2")
            //{
            //    MP = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_9_min"]);
            //}
            double ML = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20_min"]);
            #endregion

            if (prefix == "s")
            {
                /*HiddenBasketball = "";
                HiddenTennis = "";
                HiddenGolf = "";
                HiddenFootball = "";
                HiddenOtherSports = "";*/

                switch (sporttype)
                {
                    case "2": HiddenBasketball = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "3": HiddenFootball = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "5": HiddenTennis = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "10": HiddenGolf = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "99": HiddenOtherSports = CustomerUtils.AssignMultipleHidden(hidden); break;
                }

                TemplateHandler.RemoveBlock(ref strContentTpl, "master-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMinPosition);
                TemplateHandler.Assign(ref strContentTpl, "{none-tlposition}", "none");

                #region display max value Soccer Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOUBValue}", CustomerUtils.GetListPositionTaking(OuMin, OuMax, Ou));
                TemplateHandler.Assign(ref strContentTpl, "{listSOEBValue}", CustomerUtils.GetListPositionTaking(OeMin, OeMax, Oe));
                //Kiet Mai - remove mix parlay
                //if (sporttype == "2")
                //{
                //    TemplateHandler.Assign(ref strContentTpl, "{listSMPBValue}", CustomerUtils.GetListPositionTaking(MPMin, MPMax, MP));
                //}
                TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(MLMin, MLMax, ML));
                #endregion
            }
            else
            {
                switch (sporttype)
                {
                    case "2": HiddenBasketball = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "3": HiddenFootball = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "5": HiddenTennis = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "10": HiddenGolf = CustomerUtils.AssignMultipleHidden(hidden); break;
                    case "99": HiddenOtherSports = CustomerUtils.AssignMultipleHidden(hidden); break;
                }

                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMaxPosition);
                TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.lblMinPosition);

                #region local variables maximum of Max PT
                double mHdpMax = 0.9, mOuMax = 0.9, mOeMax = 0.9, mMPMax = 0.9, mMLMax = 0.9;
                #endregion

                #region local variables current of Max PT
                double mHdp = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1_max"]);
                double mOu = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_3_max"]);
                double mOe = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_2_max"]);
                double mMP = 0;
                //Kiet Mai - remove mix parlay
                //if (sporttype == "2")
                //{
                //    mMP = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_9_max"]);
                //}
                double mML = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20_max"]);
                #endregion

                #region display  value  Position taking dropdowns of Max Position  (from min Value to 0.9)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(Hdp, mHdpMax, mHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOUBValue}", CustomerUtils.GetListPositionTaking(Ou, mOuMax, mOu)); 
                TemplateHandler.Assign(ref strContentTpl, "{listSOEBValue}", CustomerUtils.GetListPositionTaking(Oe, mOeMax, mOe));
                //Kiet Mai - remove mix parlay
                //if (sporttype == "2")
                //{
                //    TemplateHandler.Assign(ref strContentTpl, "{listSMPBValue}", CustomerUtils.GetListPositionTaking(MP, mMPMax, mMP));
                //}
                TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(ML, mMLMax, mML));
                #endregion

                #region display  value  Position taking dropdowns of Min Position (from 0 to value Position Taking)
                TemplateHandler.Assign(ref strContentTpl, "{listMHdpBValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listMOUBValue}", CustomerUtils.GetListPositionTaking(OuMin, OuMax, Ou));
                TemplateHandler.Assign(ref strContentTpl, "{listMOEBValue}", CustomerUtils.GetListPositionTaking(OeMin, OeMax, Oe));
                //Kiet Mai - remove mix parlay
                //if (sporttype == "2")
                //{
                //    TemplateHandler.Assign(ref strContentTpl, "{listMMPBValue}", CustomerUtils.GetListPositionTaking(MPMin, MPMax, MP));
                //}
                TemplateHandler.Assign(ref strContentTpl, "{listMMLBValue}", CustomerUtils.GetListPositionTaking(MLMin, MLMax, ML));
                #endregion
            }
            
           TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "2");
            
            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
    #endregion

    #region GenContentBaseballPT_v2
    private string GenContentBaseballPT_v2(string sporttype, DataRow dr, string prefix)
    {
        StringBuilder html = new StringBuilder();
        Hashtable hidden = new Hashtable();
        string pre = "_Max";
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/BaseballPositionTakingRow.htm"); ;
            html.Append(CustomerUtils.LoadBaseballPTHeader_v2(false, true, sporttype, null, prefix));

            TemplateHandler.Assign(ref strContentTpl, "{sporttype}", sporttype);

            #region local variables minimum of min PT
            double HdpMin = 0, OuMin = 0, MLMin = 0;
            #endregion
            #region local variables maximum of min PT
            double HdpMax = 0, OuMax = 0, MLMax = 0;
            HdpMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1"]);
            OuMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_3"]);
            MLMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20"]);

            hidden.Add("listMHdpB" + sporttype + pre, HdpMax);
            hidden.Add("listMOUB" + sporttype + pre, OuMax);
            hidden.Add("listMMLB" + sporttype + pre, MLMax);
            HiddenBaseball = CustomerUtils.AssignMultipleHidden(hidden);
            #endregion
            #region local variables current value of min PT
            double Hdp = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1_min"]);
            double Ou = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_3_min"]);
            double ML = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20_min"]);
            #endregion

            //display title of Basketball Position taking
            if (prefix == "s")
            {
                HiddenBaseball = CustomerUtils.AssignMultipleHidden(hidden);
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "master-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMinPosition);
                TemplateHandler.Assign(ref strContentTpl, "{none-tlposition}", "none");

                #region display max value Soccer Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOUBValue}", CustomerUtils.GetListPositionTaking(OuMin, OuMax, Ou));
                TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(MLMin, MLMax, ML));
                #endregion
            }
            else //this case for Edit Master, Agent (it can edit both Min, Max Position Taking)
            {
                HiddenBaseball = CustomerUtils.AssignMultipleHidden(hidden);
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMaxPosition);
                TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.lblMinPosition);

                #region local variables maximum of Max PT
                double mHdpMax = 0.9, mOuMax = 0.9, mMLMax = 0.9;
                #endregion
                #region local variables current of Max PT
                double mHdp = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1_max"]);
                double mOu = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_3_max"]);
                double mML = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20_max"]);
                #endregion
                #region display  value Soccer Position taking dropdowns of Max Position  (from min Value to 0.9)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(Hdp, mHdpMax, mHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSOUBValue}", CustomerUtils.GetListPositionTaking(Ou, mOuMax, mOu));
                TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(ML, mMLMax, mML));
                #endregion
                #region display  value Soccer Position taking dropdowns of Min Position (from 0 to value Position Taking)
                TemplateHandler.Assign(ref strContentTpl, "{listMHdpBValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listMOUBValue}", CustomerUtils.GetListPositionTaking(OuMin, OuMax, Ou));
                TemplateHandler.Assign(ref strContentTpl, "{listMMLBValue}", CustomerUtils.GetListPositionTaking(MLMin, MLMax, ML));
                #endregion
            }
           TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "2");
            
            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex); 
        }
        return html.ToString();
    }
    #endregion


    #region GenContentMotosportsPT_v2
    private string GenContentMotosportsPT_v2(string sporttype, DataRow dr, string prefix)
    {
        StringBuilder html = new StringBuilder();
        Hashtable hidden = new Hashtable();
        string pre = "_Max";
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/MotosportsPositionTakingRow.htm"); ;
            html.Append(CustomerUtils.LoadMotosportsPTHeader_v2(false, true, sporttype, null, prefix));
            #region local variables minimum of min PT
            double HdpMin = 0, MLMin = 0;
            #endregion
            #region local variables maximum of min PT
            double HdpMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1"]);
            double MLMax = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20"]);

            hidden.Add("listMHdpB" + sporttype + pre, HdpMax);
            hidden.Add("listMMLB" + sporttype + pre, MLMax);

            HiddenMotosports = CustomerUtils.AssignMultipleHidden(hidden);

            #endregion
            #region local variables current value of min PT
            double Hdp = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1_min"]);
            double ML = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20_min"]);
            #endregion

            //display title of Basketball Position taking
            if (prefix == "s")
            {
                //HiddenMotosports = "";
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "master-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMinPosition);
                TemplateHandler.Assign(ref strContentTpl, "{none-tlposition}", "none");

                #region display max value Soccer Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(MLMin, MLMax, ML));
                #endregion
            }
            else //this case for Edit Master, Agent (it can edit both Min, Max Position Taking)
            {
                TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");
                TemplateHandler.RemoveBlock(ref strContentTpl, "agent-pt");
                TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.lblMaxPosition);
                TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.lblMinPosition);

                #region local variables maximum of Max PT
                double mHdpMax = 0.9, mMLMax = 0.9;
                #endregion
                #region local variables current of Max PT
                double mHdp = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_1_max"]);
                double mML = FormatHandler.GetDouble(dr["v" + sporttype + prefix + "positiontaking_20_max"]);
                #endregion
                #region display  value Soccer Position taking dropdowns of Max Position  (from min Value to 0.9)
                TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(Hdp, mHdpMax, mHdp));
                TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(ML, mMLMax, mML));
                #endregion
                #region display  value Soccer Position taking dropdowns of Min Position (from 0 to value Position Taking)
                TemplateHandler.Assign(ref strContentTpl, "{listMHdpBValue}", CustomerUtils.GetListPositionTaking(HdpMin, HdpMax, Hdp));
                TemplateHandler.Assign(ref strContentTpl, "{listMMLBValue}", CustomerUtils.GetListPositionTaking(MLMin, MLMax, ML));
                #endregion
            }
            TemplateHandler.Assign(ref strContentTpl, "{sporttype}", sporttype);
            TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "2");
            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
    #endregion

    #region GenContentNumberGamePT_v2
    string GenContentNumberGamePT_v2(DataRow dr, string prefix)
    {
        SportBooksBase numbergame = new NumberGameManager();
        return numbergame.GetMinPTSetting(dr, ref HiddenNumberGame, prefix);
    }
    #endregion

    #region GenMixedSports
    private string GenMixedSports(DataRow dr, string prefix)
    {
        MixedSportsManager mixedSports = new MixedSportsManager();
        return mixedSports.GetMinMaxPtForEdit(dr, prefix, ref HiddenMixedSports);
    }

    #endregion
}

