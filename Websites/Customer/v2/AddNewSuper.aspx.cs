using System;
using System.Data;
using System.Text;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;


using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Business.PT.SportBooks;
using Sunplus.Bingo.Core;

public partial class Customer_V2_AddNewSuper : RequiredAuthorizationPage
{
    protected override bool HasPermission()
    {
        var cust = Request["custid"];
        if (AppSession.SYSTEM.ToLower() != Enumerations.AccountingSystem.Accounting.ToString().ToLower() && (cust == null || (CastAide.AsInt32(cust) <= 0)))
            return false;
        return base.HasPermission();
    }


    public StringBuilder Html = new StringBuilder();
    public string Strtitle = "";

    public bool enableDeposit = WebSettings.IsApplySplitingDepositeCustomer;

    #region decalre variable
    protected string fromSet = "", type = "", msg = "";
    private string strAddNewTpl = "";
    protected string ControlHandler = "InsertNewSuper_v2.ashx";
    protected string username;
    private int custId = 0;
    //super min position taking sFxOMinB
    private double sHdpMin = 0, sOuMin = 0, s1stHdpMin = 0, s1stOuMin = 0, sOeMin = 0, s1x2Min = 0,
        sCsMin = 0, sTgMin = 0, sMpMin = 0, sOtMin = 0, sLiveHdpMin = 0, sLiveOuMin = 0, sLive1stHdpMin = 0,
        sLive1stOuMin = 0, sHdpMinB = 0, sOuMinB = 0, sOeMinB = 0, sMPMinB = 0, sMLMinB = 0;
    //super max position taking
    private double sHdpMax = 0.9, sOuMax = 0.9, s1stHdpMax = 0.9, s1stOuMax = 0.9, sOeMax = 0.9, s1x2Max = 0.9,
        sCsMax = 0.9, sTgMax = 0.9, sMpMax = 0.9, sOtMax = 0.9, sLiveHdpMax = 0.9, sLiveOuMax = 0.9, sLive1stHdpMax = 0.9,
        sLive1stOuMax = 0.9, sHdpMaxB = 0.9, sOuMaxB = 0.9, sOeMaxB = 0.9, sMPMaxB = 0.9, sMLMaxB = 0.9;
    //super position taking
    private double sHdp = 0.9, sOu = 0.9, s1stHdp = 0.9, s1stOu = 0.9, sOe = 0.9, s1x2 = 0.9,
        sCs = 0.9, sTg = 0.9, sMp = 0.9, sOt = 0.9, sLiveHdp = 0.9, sLiveOu = 0.9, sLive1stHdp = 0.9,
        sLive1stOu = 0.9, sHdpB = 0.9, sOuB = 0.9, sOeB = 0.9, sMPB = 0.9, sMLB = 0.9;
    //master position taking
    private double mHdp = 0.9, mOu = 0.9, m1stHdp = 0.9, m1stOu = 0.9, mOe = 0.9, m1x2 = 0.9,
        mCs = 0.9, mTg = 0.9, mMp = 0.9, mOt = 0.9, mLiveHdp = 0.9, mLiveOu = 0.9, mLive1stHdp = 0.9,
        mLive1stOu = 0.9, mHdpB = 0.9, mOuB = 0.9, mOeB = 0.9, mMPB = 0.9, mMLB = 0.9;
    //agent position taking
    private double aHdp = 0.9, aOu = 0.9, a1stHdp = 0.9, a1stOu = 0.9, aOe = 0.9, a1x2 = 0.9,
        aCs = 0.9, aTg = 0.9, aMp = 0.9, aOt = 0.9, aLiveHdp = 0.9, aLiveOu = 0.9, aLive1stHdp = 0.9,
        aLive1stOu = 0.9, aHdpB = 0.9, aOuB = 0.9, aOeB = 0.9, aMPB = 0.9, aMLB = 0.9;

    //variable create interface
    string bodyTpl;
    protected Int16 disHR = CastAide.AsInt16(WebSettings.disabledHR);
    public StringBuilder content = new StringBuilder();
    #endregion



    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            int.TryParse(Request["custid"], out custId);
            username = Request["uname"] + "";
            DataRow dr = null;
            if (custId > 0)
            {
                ControlHandler = "UpdateSuper_v2.ashx";
                Page.Title = username;
                fromSet = Request["set"];
                type = "edit";
                DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Customer.Select4Edit.GetSuperInfo_acc(custId);
                if (ds != null && ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    dr = ds.ResultSet.Tables[0].Rows[0];
                }
                else
                {
                    if (AppSession.SYSTEM.ToLower() != Enumerations.AccountingSystem.Accounting.ToString().ToLower())
                    {
                        content.Append(Resources.Language.thereisnodata);
                        return;
                    }
                }

                Strtitle = "Update Super";
            }
            else
            {
                Strtitle = "Add New Super";
            }

            // add new v2         
            bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/MenuPT.htm");

            TemplateHandler.Assign(ref bodyTpl, "{soccer-text}", Resources.Language.lblSoccer);
            TemplateHandler.Assign(ref bodyTpl, "{basket-text}", Resources.Language.lblBasketball);
            TemplateHandler.Assign(ref bodyTpl, "{football-text}", Resources.Language.lblFootball);
            TemplateHandler.Assign(ref bodyTpl, "{tennis-text}", Resources.Language.lblTennis);
            TemplateHandler.Assign(ref bodyTpl, "{baseball-text}", Resources.Language.lblBaseball);
            TemplateHandler.Assign(ref bodyTpl, "{golf-text}", Resources.Language.lblGolf);
            TemplateHandler.Assign(ref bodyTpl, "{moto-text}", Resources.Language.lblMotosports);
            TemplateHandler.Assign(ref bodyTpl, "{othersports-text}", Resources.Language.lblOthersports);
            TemplateHandler.Assign(ref bodyTpl, "{numbergame-text}", Resources.Language.lblNumberGame);
            //Kiet Mai - assing mixed sports title
            TemplateHandler.Assign(ref bodyTpl, "{mixedsports-text}", Resources.Language.lblMixedSports);

            TemplateHandler.Assign(ref bodyTpl, "{add-text}", (custId > 0) ? Resources.Language.Update : Resources.Language.btnAdd);
            TemplateHandler.Assign(ref bodyTpl, "{add-id}", "btnAdd");
            TemplateHandler.Assign(ref bodyTpl, "{copy-all-text}", Resources.Language.lblCopyAllPTfromHdp);
            TemplateHandler.Assign(ref bodyTpl, "{copy-basket-text}", Resources.Language.lblCopyBasketHdp);

            TemplateHandler.Assign(ref bodyTpl, "{soccerPT}", GenContentSoccerPT_v2(dr));
            TemplateHandler.Assign(ref bodyTpl, "{basketPT}", GenContentBasketballPT_v2("2", dr));
            TemplateHandler.Assign(ref bodyTpl, "{footballPT}", GenContentBasketballPT_v2("3", dr));
            TemplateHandler.Assign(ref bodyTpl, "{tennisPT}", GenContentBasketballPT_v2("5", dr));
            TemplateHandler.Assign(ref bodyTpl, "{baseballPT}", GenContentBaseballPT_v2("8", dr));
            TemplateHandler.Assign(ref bodyTpl, "{golfPT}", GenContentBasketballPT_v2("10", dr));
            TemplateHandler.Assign(ref bodyTpl, "{motoPT}", GenContentMotosportsPT_v2("11", dr));

            if(BingoSettings.NumberGame.Enabled)                
                TemplateHandler.Assign(ref bodyTpl, "{numberGamePT}", GetNumberGameSetting(dr));
            else
                TemplateHandler.RemoveBlock(ref bodyTpl, "NumberGame");


            //Kiet Mai - assign mixed sports panel
            TemplateHandler.Assign(ref bodyTpl, "{mixedsports}", GetMixedSportsSetting(dr));


            TemplateHandler.Assign(ref bodyTpl, "{otherPT}", GenContentBasketballPT_v2("99", dr));
            TemplateHandler.Assign(ref bodyTpl, "{HRMenuHidden}", "hidden");

            TemplateHandler.Assign(ref bodyTpl, "{accinfo}", AssignDataInfo(dr));
            if (custId > 0 && dr != null)
            {
                TemplateHandler.Assign(ref bodyTpl, "{hidSynCS}", dr["synccasino"].ToString());
            }
            //content.Append(AssignDataInfo(dr));
            string strscript = VariableScript();
            content.Append(strscript + Environment.NewLine + bodyTpl);

        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }
    #endregion

    //----------- add new v2 -------------//

    #region VariableScript

    static string VariableScript()
    {
        string script = "";
        script += "<script language='javascript'>";
        script += " var lblPasswordRequire='" + Resources.Language.lblPasswordRequire + "';";
        script += " var alertPasswordEmpty='" + Resources.Language.alertPasswordEmpty + "';";
        script += " var alertUserName='" + Resources.Language.alertUserName + "';";
        script += " var alertCompare3='" + Resources.Language.alertMaxLessMin + "'; ";
        script += " var alertMinbet='" + Resources.Language.alertIncorrectMinBet + "'; ";
        script += " var alertMaxbet='" + Resources.Language.alertIncorrectMaxBet + "';";
        script += " var alertMaxpermatch='" + Resources.Language.alertIncorrectMaxPerMatch + "';";
        script += " var alertCompare='" + Resources.Language.alertMaxLessMaxPer + "'; ";
        script += " var alertIncorrectCredit='" + Resources.Language.alertIncorrectCredit + "';";
        script += " var alertMinLessCredit='" + Resources.Language.alertMinLessCredit + "';";
        script += " var alertSite='" + Resources.Language.alertSite + "';";
        script += " var alertCurrency='" + Resources.Language.alertCurrency + "';";
        string lblCreditInvalid;
        lblCreditInvalid = Resources.Language.lblCreditInvalid.Replace("{0}", Resources.Language.lblMemberMaxCredit);
        script += " var alertCreditInvalid='" + lblCreditInvalid + "';";
        script += "</script>";
        //Response.Write(script);
        return script;
    }
    #endregion

    #region Load AccInfoMem template
    string AssignDataInfo(DataRow dr)
    {
        string strAccInfo = "";
        try
        {
            double groupa = 0, groupb = 0, groupc = 0, groupd = 0, discounts = 0, discountsFC = 0, discountsUK = 0, discountsOther = 0, group1x2 = WebSettings.Group1x2Max;

            double groupa_max = 0, groupb_max = 0, groupc_max = 0, groupd_max = 0, discounts_max = 0, discountsFC_max = 0, discountsUK_max = 0, discountsOther_max = 0, group1x2_max = WebSettings.Group1x2Max;
            groupa = 0.0025;
            groupb = 0.005;
            groupc = 0.0075;
            groupd = 0.01;
            discountsFC = 0.0025;
            group1x2 = WebSettings.Group1x2Max;

            discountsUK = WebSettings.UkMeeting;
            discountsOther = WebSettings.OtherMeeting;
            //discounts = 0.0025;
            discounts = 0.01;
            groupa_max = 0.0025;
            groupb_max = 0.005;
            groupc_max = 0.0075;
            groupd_max = 0.01;
            group1x2_max = WebSettings.Group1x2Max;
            //discounts_max = 0.0025;
            discounts_max = 0.01;
            //discountsFC_max = 0.01;
            discountsFC_max = 0.0025;
            discountsUK_max = WebSettings.UkMeetingMax;
            discountsOther_max = WebSettings.OtherMeetingMax;

            strAccInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/AccInfo.htm");
            //extMaxcredit -- accMaxcredit

            if (WebSettings.DisableInternal || !AppSession.SYSTEM.ToLower().Equals(Enumerations.AccountingSystem.Accounting.ToString().ToLower()))
            {
                TemplateHandler.RemoveBlock(ref strAccInfo, "Internal");
                //TemplateHandler.RemoveBlock(ref strAccInfo, "accMaxcredit");
                //TemplateHandler.Assign(ref strAccInfo, "{isexternal}", 1);
                
            }
            else
            {
                
                //TemplateHandler.RemoveBlock(ref strAccInfo, "extMaxcredit");
                //TemplateHandler.Assign(ref strAccInfo, "{isexternal}", 0);
                TemplateHandler.Assign(ref strAccInfo, "{lblInternalUser}", Resources.Language.lblInternalUser);
            }
            TemplateHandler.RemoveBlock(ref strAccInfo, "extMaxcredit");

            #region edit
            if (custId > 0 && dr != null)//edit
            {
                TemplateHandler.RemoveBlock(ref strAccInfo, "Internal");
                TemplateHandler.Assign(ref strAccInfo, "{colsmaxcredit}", "1");
                TemplateHandler.RemoveBlock(ref strAccInfo, "not-edit");
                TemplateHandler.RemoveBlock(ref strAccInfo, "site-super");
                //TemplateHandler.RemoveBlock(ref strAccInfo, "casino");
                TemplateHandler.RemoveBlock(ref strAccInfo, "currency");
                TemplateHandler.Assign(ref strAccInfo, "{add-edit-agent}", "Super: " + dr["username"]);
                TemplateHandler.Assign(ref strAccInfo, "{firstname}", dr["firstname"].ToString());
                TemplateHandler.Assign(ref strAccInfo, "{lastname}", dr["lastname"].ToString());
                TemplateHandler.Assign(ref strAccInfo, "{phone}", dr["phone"].ToString());
                TemplateHandler.Assign(ref strAccInfo, "{mobile}", dr["mobilephone"].ToString());
                TemplateHandler.Assign(ref strAccInfo, "{maxcredit-val}", FormatHandler.FormatNumber1(dr["credit"]));
                TemplateHandler.Assign(ref strAccInfo, "{fax}", FormatHandler.FormatNumber1(dr["Fax"]));

                var isClosed = Convert.ToBoolean(dr["closed"]);

                if(isClosed && AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
                {
                    TemplateHandler.RemoveBlock(ref strAccInfo, "statusclose");

                    TemplateHandler.Assign(ref strAccInfo, "{strstatus}", "Closed");  
                }
                else
                {
                    TemplateHandler.Assign(ref strAccInfo, "{status}", (Convert.ToBoolean(dr["closed"])) ? "checked='true'" : "");
                    TemplateHandler.Assign(ref strAccInfo, "{strstatus}", "");  
                }
                
                TemplateHandler.Assign(ref strAccInfo, "{Min}", " >= ");
                TemplateHandler.Assign(ref strAccInfo, "{MinCredit}", FormatHandler.FormatNumber2(dr["Credit_Min"]));
                TemplateHandler.Assign(ref strAccInfo, "{chkInternal}", HtmlHelper.GenCheckBox("chkInternal", false, CastAide.AsBoolean(dr["isinternal"], false), "", ""));



                groupa = FormatHandler.GetDouble(dr["groupa"]);
                groupb = FormatHandler.GetDouble(dr["groupb"]);
                groupc = FormatHandler.GetDouble(dr["groupc"]);
                groupd = FormatHandler.GetDouble(dr["groupd"]);
                discounts = FormatHandler.GetDouble(dr["discountcs"]);

                if (WebSettings.Disable1x2Comm)
                {
                    TemplateHandler.RemoveBlock(ref strAccInfo, "none-1x2-Comm");
                }
                else
                {
                    try
                    {
                        group1x2 = FormatHandler.GetDouble(dr["discount1x2"]);
                        group1x2_max = FormatHandler.GetDouble(dr["discount1x2_max"]); ;
                    }
                    catch
                    {
                        group1x2 = WebSettings.Group1x2Max;
                        group1x2_max = WebSettings.Group1x2Max;
                    }
                    TemplateHandler.Assign(ref strAccInfo, "{listGroup1x2Value}", CustomerUtils.GetListGroup(group1x2_max, group1x2));
                }

                groupa_max = FormatHandler.GetDouble(dr["groupa_max"]);
                groupb_max = FormatHandler.GetDouble(dr["groupb_max"]);
                groupc_max = FormatHandler.GetDouble(dr["groupc_max"]);
                groupd_max = FormatHandler.GetDouble(dr["groupd_max"]);
                discounts_max = FormatHandler.GetDouble(dr["discountcs_max"]);

                string IsSuspend = (CastAide.AsBoolean(dr["suspended"])) ? "checked='checked'" : string.Empty;
                TemplateHandler.Assign(ref strAccInfo, "{suspend-check}", IsSuspend);


            }
            #endregion

            #region add new
            else //add new
            {
                TemplateHandler.Assign(ref strAccInfo, "{chkInternal}", HtmlHelper.GenCheckBox("chkInternal", false, false, "", ""));
                if (WebSettings.Disable1x2Comm)
                {
                    TemplateHandler.RemoveBlock(ref strAccInfo, "none-1x2-Comm");
                }
                else
                {
                    TemplateHandler.Assign(ref strAccInfo, "{listGroup1x2Value}", CustomerUtils.GetListGroup(group1x2_max, group1x2));
                }


                //TemplateHandler.Assign(ref strAccInfo, "{lblEnableCS}", Resources.Language.lblEnabledCasino);

                TemplateHandler.RemoveBlock(ref strAccInfo, "status");
                TemplateHandler.RemoveBlock(ref strAccInfo, "not-show-new");
                TemplateHandler.Assign(ref strAccInfo, "{colsmaxcredit}", "2");

                TemplateHandler.Assign(ref strAccInfo, "{add-edit-agent}", Resources.Language.AddNewSuper);
                TemplateHandler.Assign(ref strAccInfo, "{firstname}", string.Empty);
                TemplateHandler.Assign(ref strAccInfo, "{lastname}", string.Empty);
                TemplateHandler.Assign(ref strAccInfo, "{phone}", string.Empty);
                TemplateHandler.Assign(ref strAccInfo, "{mobile}", string.Empty);
                TemplateHandler.Assign(ref strAccInfo, "{maxcredit-val}", "0");
                TemplateHandler.Assign(ref strAccInfo, "{fax}", string.Empty);
                TemplateHandler.Assign(ref strAccInfo, "{status}", string.Empty);
                TemplateHandler.Assign(ref strAccInfo, "{Min}", string.Empty);
                TemplateHandler.Assign(ref strAccInfo, "{MinCredit}", string.Empty);
                CustomerUtils.GetListOtherComm(discountsOther_max, discountsOther, 25);
            }
            #endregion

            //Check permission 
            bool canupdatecredit = Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);

            if (canupdatecredit)
            {
                TemplateHandler.Assign(ref strAccInfo, "{disable_color}", "");
                TemplateHandler.Assign(ref strAccInfo, "{diable_credit}", "");
                TemplateHandler.Assign(ref strAccInfo, "{mouseover_credit}", "");
                TemplateHandler.Assign(ref strAccInfo, "{mouseout_credit}", "");

            }
            else
            {
                TemplateHandler.Assign(ref strAccInfo, "{disable_color}", "background-color:#E8E8E8; border:solid 1px #7f9db9;color:#A0A0A0;");
                TemplateHandler.Assign(ref strAccInfo, "{diable_credit}", "readonly");
                TemplateHandler.Assign(ref strAccInfo, "{mouseout_credit}", "onmouseout='UnTip()'");
                TemplateHandler.Assign(ref strAccInfo, "{mouseover_credit}", "onmouseover=\"Tip('You do not have permission on update credit.', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 4)\"");
            }

            TemplateHandler.Assign(ref strAccInfo, "{role-id}", "4");
            TemplateHandler.Assign(ref strAccInfo, "{edit-hidden}", "");
            TemplateHandler.Assign(ref strAccInfo, "{username}", "");

            TemplateHandler.Assign(ref strAccInfo, "{update-hidden}", "hidden");
            TemplateHandler.Assign(ref strAccInfo, "{add-hidden}", "");
            //Site currency
            // TemplateHandler.Assign(ref strAccInfo, "{site-curr-hidden}", "");
            //TemplateHandler.Assign(ref strAccInfo, "{listSiteValue}", CustomerUtils.GetListSite(true, AppSession.CurrentSystem));
            TemplateHandler.Assign(ref strAccInfo, "{listSiteValue}", string.Empty);
            TemplateHandler.Assign(ref strAccInfo, "{listCurrencyValue}", CustomerUtils.GetListCurrencyBySystem(AppSession.SYSTEM, true, false, true));

            //hide reset button
            TemplateHandler.Assign(ref strAccInfo, "{update-hidden}", "hidden");
            TemplateHandler.Assign(ref strAccInfo, "{add-hidden}", "");

            TemplateHandler.Assign(ref strAccInfo, "{status-hidden}", "hidden");
            TemplateHandler.Assign(ref strAccInfo, "{closed-checked}", string.Empty);
            TemplateHandler.Assign(ref strAccInfo, "{cust-id}", custId.ToString());
            TemplateHandler.Assign(ref strAccInfo, "{maxcredit}", Resources.Language.SuperMaxCredit);

            TemplateHandler.Assign(ref strAccInfo, "{mem-group-hidden}", "hidden");
            TemplateHandler.Assign(ref strAccInfo, "{memb-com-hidden}", "hidden");
            //TemplateHandler.Assign(ref strAccInfo, "{group-com-hidden}", string.Empty);

            //display value Commission dropdowns (default: maxGroupA=0.0025, maxGroupB=0.005, maxGroupC=0,0075, maxGroupD=0,01, Other=0.05) 
            TemplateHandler.Assign(ref strAccInfo, "{Commission}", Resources.Language.Commission);

            TemplateHandler.Assign(ref strAccInfo, "{listGroupAValue}", CustomerUtils.GetListGroup(groupa_max, groupa));
            TemplateHandler.Assign(ref strAccInfo, "{listGroupBValue}", CustomerUtils.GetListGroup(groupb_max, groupb));
            TemplateHandler.Assign(ref strAccInfo, "{listGroupCValue}", CustomerUtils.GetListGroup(groupc_max, groupc));
            TemplateHandler.Assign(ref strAccInfo, "{listGroupDValue}", CustomerUtils.GetListGroup(groupd_max, groupd));
            TemplateHandler.Assign(ref strAccInfo, "{listOtherValue}", CustomerUtils.GetListOtherComm(discounts_max, discounts));

            SportBooksBase numbergame = new NumberGameManager();
            numbergame.GetNumberCom(dr, 4, dr == null, ref strAccInfo);
            //numbergame.GetMaxPerBall(dr, 4, dr == null, ref strAccInfo); 
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return strAccInfo;
    }
    #endregion

    #region GenContentSoccerPT_v2
    private string GenContentSoccerPT_v2(DataRow dr)
    {
        StringBuilder html = new StringBuilder();
        double mHdpMax = 0.9, mOuMax = 0.9, m1stHdpMax = 0.9, m1stOuMax = 0.9, mOeMax = 0.9, m1x2Max = 0.9, mCsMax = 0.9, mTgMax = 0.9, mMpMax = 0.9, mOtMax = 0.9, mLiveHdpMax = 0.9, mLiveOuMax = 0.9, mLive1stHdpMax = 0.9, mLive1stOuMax = 0.9;
        double aHdpMax = 0.9, aOuMax = 0.9, a1stHdpMax = 0.9, a1stOuMax = 0.9, aOeMax = 0.9, a1x2Max = 0.9, aCsMax = 0.9, aTgMax = 0.9, aMpMax = 0.9, aOtMax = 0.9, aLiveHdpMax = 0.9, aLiveOuMax = 0.9, aLive1stHdpMax = 0.9, aLive1stOuMax = 0.9;
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/SoccerPositionTakingRow.htm"); ;
            TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt1");
            TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt2");
            html.Append(CustomerUtils.LoadSoccerPTHeader_v2(false, false, "1", dr, ""));

            TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "3");
            TemplateHandler.Assign(ref strContentTpl, "{width}", "w130");

            if (custId > 0 && dr != null) //edit
            {
                #region super
                //listSHdpDValue
                sHdpMin = FormatHandler.GetDouble(dr["spositiontaking_1_min"]);
                sHdpMax = FormatHandler.GetDouble(dr["spositiontaking_1_max"]);
                sHdp = FormatHandler.GetDouble(dr["spositiontaking_1"]);

                //listSOUDValue
                sOuMin = FormatHandler.GetDouble(dr["spositiontaking_3_min"]);
                sOuMax = FormatHandler.GetDouble(dr["spositiontaking_3_max"]);
                sOu = FormatHandler.GetDouble(dr["spositiontaking_3"]);

                //listS1stHdpDValue
                s1stHdpMin = FormatHandler.GetDouble(dr["spositiontaking_7_min"]);
                s1stHdpMax = FormatHandler.GetDouble(dr["spositiontaking_7_max"]);
                s1stHdp = FormatHandler.GetDouble(dr["spositiontaking_7"]);

                //listS1stOUDValue
                s1stOuMin = FormatHandler.GetDouble(dr["spositiontaking_8_min"]);
                s1stOuMax = FormatHandler.GetDouble(dr["spositiontaking_8_max"]);
                s1stOu = FormatHandler.GetDouble(dr["spositiontaking_8"]);

                //listSOEValue
                sOeMin = FormatHandler.GetDouble(dr["spositiontaking_2_min"]);
                sOeMax = FormatHandler.GetDouble(dr["spositiontaking_2_max"]);
                sOe = FormatHandler.GetDouble(dr["spositiontaking_2"]);

                //listS1x2Value
                s1x2Min = FormatHandler.GetDouble(dr["spositiontaking_5_min"]);
                s1x2Max = FormatHandler.GetDouble(dr["spositiontaking_5_max"]);
                s1x2 = FormatHandler.GetDouble(dr["spositiontaking_5"]);

                //listSCSValue
                sCsMin = FormatHandler.GetDouble(dr["spositiontaking_4_min"]);
                sCsMax = FormatHandler.GetDouble(dr["spositiontaking_4_max"]);
                sCs = FormatHandler.GetDouble(dr["spositiontaking_4"]);

                //listSTGValue
                sTgMin = FormatHandler.GetDouble(dr["spositiontaking_6_min"]);
                sTgMax = FormatHandler.GetDouble(dr["spositiontaking_6_max"]);
                sTg = FormatHandler.GetDouble(dr["spositiontaking_6"]);

                //listSMPValue
                sMpMin = FormatHandler.GetDouble(dr["spositiontaking_9_min"]);
                sMpMax = FormatHandler.GetDouble(dr["spositiontaking_9_max"]);
                sMp = FormatHandler.GetDouble(dr["spositiontaking_9"]);

                //listSOutrightValue
                sOtMin = FormatHandler.GetDouble(dr["spositiontaking_10_min"]);
                sOtMax = FormatHandler.GetDouble(dr["spositiontaking_10_max"]);
                sOt = FormatHandler.GetDouble(dr["spositiontaking_10"]);

                //listSHdpLValue
                sLiveHdpMin = FormatHandler.GetDouble(dr["slivepositiontaking_1_min"]);
                sLiveHdpMax = FormatHandler.GetDouble(dr["slivepositiontaking_1_max"]);
                sLiveHdp = FormatHandler.GetDouble(dr["slivepositiontaking_1"]);

                //listSOULValue
                sLiveOuMin = FormatHandler.GetDouble(dr["slivepositiontaking_3_min"]);
                sLiveOuMax = FormatHandler.GetDouble(dr["slivepositiontaking_3_max"]);
                sLiveOu = FormatHandler.GetDouble(dr["slivepositiontaking_3"]);

                //listS1stHdpLValue
                sLive1stHdpMin = FormatHandler.GetDouble(dr["slivepositiontaking_7_min"]);
                sLive1stHdpMax = FormatHandler.GetDouble(dr["slivepositiontaking_7_max"]);
                sLive1stHdp = FormatHandler.GetDouble(dr["slivepositiontaking_7"]);

                //listS1stOULValue
                sLive1stOuMin = FormatHandler.GetDouble(dr["slivepositiontaking_8_min"]);
                sLive1stOuMax = FormatHandler.GetDouble(dr["slivepositiontaking_8_max"]);
                sLive1stOu = FormatHandler.GetDouble(dr["slivepositiontaking_8"]);
                #endregion

                #region master
                mHdp = FormatHandler.GetDouble(dr["mpositiontaking_1"]);
                mOu = FormatHandler.GetDouble(dr["mpositiontaking_3"]);
                m1stHdp = FormatHandler.GetDouble(dr["mpositiontaking_7"]);
                m1stOu = FormatHandler.GetDouble(dr["mpositiontaking_8"]);
                mOe = FormatHandler.GetDouble(dr["mpositiontaking_2"]);
                m1x2 = FormatHandler.GetDouble(dr["mpositiontaking_5"]);
                mCs = FormatHandler.GetDouble(dr["mpositiontaking_4"]);
                mTg = FormatHandler.GetDouble(dr["mpositiontaking_6"]);
                mMp = FormatHandler.GetDouble(dr["mpositiontaking_9"]);
                mOt = FormatHandler.GetDouble(dr["mpositiontaking_10"]);
                mLiveHdp = FormatHandler.GetDouble(dr["mlivepositiontaking_1"]);
                mLiveOu = FormatHandler.GetDouble(dr["mlivepositiontaking_3"]);
                mLive1stHdp = FormatHandler.GetDouble(dr["mlivepositiontaking_7"]);
                mLive1stOu = FormatHandler.GetDouble(dr["mlivepositiontaking_8"]);

                mHdpMax = sHdp;
                mOuMax = sOu;
                m1stHdpMax = s1stHdp;
                m1stOuMax = s1stOu;
                mOeMax = sOe;
                m1x2Max = s1x2;
                mCsMax = sCs;
                mTgMax = sTg;
                mMpMax = sMp;
                mOtMax = sOt;
                mLiveHdpMax = sLiveHdp;
                mLiveOuMax = sLiveOu;
                mLive1stHdpMax = sLive1stHdp;
                mLive1stOuMax = sLive1stOu;
                #endregion

                #region agent
                aHdp = FormatHandler.GetDouble(dr["apositiontaking_1"]);
                aOu = FormatHandler.GetDouble(dr["apositiontaking_3"]);
                a1stHdp = FormatHandler.GetDouble(dr["apositiontaking_7"]);
                a1stOu = FormatHandler.GetDouble(dr["apositiontaking_8"]);
                aOe = FormatHandler.GetDouble(dr["apositiontaking_2"]);
                a1x2 = FormatHandler.GetDouble(dr["apositiontaking_5"]);
                aCs = FormatHandler.GetDouble(dr["apositiontaking_4"]);
                aTg = FormatHandler.GetDouble(dr["apositiontaking_6"]);
                aMp = FormatHandler.GetDouble(dr["apositiontaking_9"]);
                aOt = FormatHandler.GetDouble(dr["apositiontaking_10"]);
                aLiveHdp = FormatHandler.GetDouble(dr["alivepositiontaking_1"]);
                aLiveOu = FormatHandler.GetDouble(dr["alivepositiontaking_3"]);
                aLive1stHdp = FormatHandler.GetDouble(dr["alivepositiontaking_7"]);
                aLive1stOu = FormatHandler.GetDouble(dr["alivepositiontaking_8"]);

                aHdpMax = mHdp;
                aOuMax = mOu;
                a1stHdpMax = m1stHdp;
                a1stOuMax = m1stOu;
                aOeMax = mOe;
                a1x2Max = m1x2;
                aCsMax = mCs;
                aTgMax = mTg;
                aMpMax = mMp;
                aOtMax = mOt;
                aLiveHdpMax = mLiveHdp;
                aLiveOuMax = mLiveOu;
                aLive1stHdpMax = mLive1stHdp;
                aLive1stOuMax = mLive1stOu;

                #endregion
            }

            #region super display max value Soccer Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listSHdpDValue}", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listSOUDValue}", CustomerUtils.GetListPositionTaking(sOuMin, sOuMax, sOu));
            TemplateHandler.Assign(ref strContentTpl, "{listS1stHdpDValue}", CustomerUtils.GetListPositionTaking(s1stHdpMin, s1stHdpMax, s1stHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listS1stOUDValue}", CustomerUtils.GetListPositionTaking(s1stOuMin, s1stOuMax, s1stOu));
            TemplateHandler.Assign(ref strContentTpl, "{listSOEValue}", CustomerUtils.GetListPositionTaking(sOeMin, sOeMax, sOe));
            TemplateHandler.Assign(ref strContentTpl, "{listS1x2Value}", CustomerUtils.GetListPositionTaking(s1x2Min, s1x2Max, s1x2));
            TemplateHandler.Assign(ref strContentTpl, "{listSCSValue}", CustomerUtils.GetListPositionTaking(sCsMin, sCsMax, sCs));
            TemplateHandler.Assign(ref strContentTpl, "{listSTGValue}", CustomerUtils.GetListPositionTaking(sTgMin, sTgMax, sTg));
            TemplateHandler.Assign(ref strContentTpl, "{listSMPValue}", CustomerUtils.GetListPositionTaking(sMpMin, sMpMax, sMp));
            TemplateHandler.Assign(ref strContentTpl, "{listSOutrightValue}", CustomerUtils.GetListPositionTaking(sOtMin, sOtMax, sOt));
            TemplateHandler.Assign(ref strContentTpl, "{listSHdpLValue}", CustomerUtils.GetListPositionTaking(sLiveHdpMin, sLiveHdpMax, sLiveHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listSOULValue}", CustomerUtils.GetListPositionTaking(sLiveOuMin, sLiveOuMax, sLiveOu));
            TemplateHandler.Assign(ref strContentTpl, "{listS1stHdpLValue}", CustomerUtils.GetListPositionTaking(sLive1stHdpMin, sLive1stHdpMax, sLive1stHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listS1stOULValue}", CustomerUtils.GetListPositionTaking(sLive1stOuMin, sLive1stOuMax, sLive1stOu));
            #endregion

            #region master display max value Soccer Position taking dropdowns of Master (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listMHdpDValue}", CustomerUtils.GetListPositionTaking(0, mHdpMax, mHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listMOUDValue}", CustomerUtils.GetListPositionTaking(0, mOuMax, mOu));
            TemplateHandler.Assign(ref strContentTpl, "{listM1stHdpDValue}", CustomerUtils.GetListPositionTaking(0, m1stHdpMax, m1stHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listM1stOUDValue}", CustomerUtils.GetListPositionTaking(0, m1stOuMax, m1stOu));
            TemplateHandler.Assign(ref strContentTpl, "{listMOEValue}", CustomerUtils.GetListPositionTaking(0, mOeMax, mOe));
            TemplateHandler.Assign(ref strContentTpl, "{listM1x2Value}", CustomerUtils.GetListPositionTaking(0, m1x2Max, m1x2));
            TemplateHandler.Assign(ref strContentTpl, "{listMCSValue}", CustomerUtils.GetListPositionTaking(0, mCsMax, mCs));
            TemplateHandler.Assign(ref strContentTpl, "{listMTGValue}", CustomerUtils.GetListPositionTaking(0, mTgMax, mTg));
            TemplateHandler.Assign(ref strContentTpl, "{listMMPValue}", CustomerUtils.GetListPositionTaking(0, mMpMax, mMp));
            TemplateHandler.Assign(ref strContentTpl, "{listMOutrightValue}", CustomerUtils.GetListPositionTaking(0, mOtMax, mOt));
            TemplateHandler.Assign(ref strContentTpl, "{listMHdpLValue}", CustomerUtils.GetListPositionTaking(0, mLiveHdpMax, mLiveHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listMOULValue}", CustomerUtils.GetListPositionTaking(0, mLiveOuMax, mLiveOu));
            TemplateHandler.Assign(ref strContentTpl, "{listM1stHdpLValue}", CustomerUtils.GetListPositionTaking(0, mLive1stHdpMax, mLive1stHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listM1stOULValue}", CustomerUtils.GetListPositionTaking(0, mLive1stOuMax, mLive1stOu));
            #endregion

            #region agent display max value Soccer Position taking dropdowns of Agent (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listAHdpDValue}", CustomerUtils.GetListPositionTaking(0, aHdpMax, aHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listAOUDValue}", CustomerUtils.GetListPositionTaking(0, aOuMax, aOu));
            TemplateHandler.Assign(ref strContentTpl, "{listA1stHdpDValue}", CustomerUtils.GetListPositionTaking(0, a1stHdpMax, a1stHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listA1stOUDValue}", CustomerUtils.GetListPositionTaking(0, a1stOuMax, a1stOu));
            TemplateHandler.Assign(ref strContentTpl, "{listAOEValue}", CustomerUtils.GetListPositionTaking(0, aOeMax, aOe));
            TemplateHandler.Assign(ref strContentTpl, "{listA1x2Value}", CustomerUtils.GetListPositionTaking(0, a1x2Max, a1x2));
            TemplateHandler.Assign(ref strContentTpl, "{listACSValue}", CustomerUtils.GetListPositionTaking(0, aCsMax, aCs));
            TemplateHandler.Assign(ref strContentTpl, "{listATGValue}", CustomerUtils.GetListPositionTaking(0, aTgMax, aTg));
            TemplateHandler.Assign(ref strContentTpl, "{listAMPValue}", CustomerUtils.GetListPositionTaking(0, aMpMax, aMp));
            TemplateHandler.Assign(ref strContentTpl, "{listAOutrightValue}", CustomerUtils.GetListPositionTaking(0, aOtMax, aOt));
            TemplateHandler.Assign(ref strContentTpl, "{listAHdpLValue}", CustomerUtils.GetListPositionTaking(0, aLiveHdpMax, aLiveHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listAOULValue}", CustomerUtils.GetListPositionTaking(0, aLiveOuMax, aLiveOu));
            TemplateHandler.Assign(ref strContentTpl, "{listA1stHdpLValue}", CustomerUtils.GetListPositionTaking(0, aLive1stHdpMax, aLive1stHdp));
            TemplateHandler.Assign(ref strContentTpl, "{listA1stOULValue}", CustomerUtils.GetListPositionTaking(0, aLive1stOuMax, aLive1stOu));
            #endregion

            TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.SuperMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.MasterMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{agent-position}", Resources.Language.AgentMaxPosition);

            TemplateHandler.Assign(ref strContentTpl, "{super-live-position}", Resources.Language.SuperMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{master-live-position}", Resources.Language.MasterMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{agent-live-position}", Resources.Language.AgentMaxPosition);

            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
    #endregion

    #region GenContentBasketballPT_v2 : this function use for Basketball, Football, Tennis, Golf, OtherSport
    private string GenContentBasketballPT_v2(string sporttype, DataRow dr)
    {
        StringBuilder html = new StringBuilder();
        double mHdpMaxB = 0.9, mOuMaxB = 0.9, mOeMaxB = 0.9, mMLMaxB = 0.9, mMPMaxB = 0.9;
        double aHdpMaxB = 0.9, aOuMaxB = 0.9, aOeMaxB = 0.9, aMLMaxB = 0.9, aMPMaxB = 0.9;
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/BasketballPositionTakingRow.htm"); ;
            html.Append(CustomerUtils.LoadBasketballPTHeader_v2(false, false, sporttype, dr, ""));

            //Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            /*if (sporttype != "2")//basketball
            {
                TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlays");
                TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaym");
                TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaya");
            }*/
            TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlays");
            TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaym");
            TemplateHandler.RemoveBlock(ref strContentTpl, "no-mixparlaya");

            if (custId > 0 && dr != null) //edit
            {
                #region super
                //listSHdpBValue
                sHdpMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1_min"]);
                sHdpMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1_max"]);
                sHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1"]);

                //listSOUBValue
                sOuMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_3_min"]);
                sOuMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_3_max"]);
                sOuB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_3"]);

                //listSOEBValue
                sOeMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_2_min"]);
                sOeMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_2_max"]);
                sOeB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_2"]);

                //Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                /*if (sporttype == "2")//basketball
                {
                    //listSMPBValue
                    sMPMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_9_min"]);
                    sMPMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_9_max"]);
                    sMPB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_9"]);
                }*/

                //listSMLBValue
                sMLMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20_min"]);
                sMLMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20_max"]);
                sMLB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20"]);
                #endregion

                #region master
                mHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_1"]);
                mOuB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_3"]);
                mOeB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_2"]);

                //Kiet Mai - remove mixpalay when sporttype = 2 (basketball
                if (sporttype == "2")
                {
                    //mMPB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_9"]);
                    mMPB = 0;
                }
                mMLB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_20"]);
                mHdpMaxB = sHdpB;
                mOuMaxB = sOuB;
                mOeMaxB = sOeB;
                mMPMaxB = sMPB;
                mMLMaxB = sMLB;
                #endregion

                #region agent
                aHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_1"]);
                aOuB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_3"]);
                aOeB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_2"]);
                
                //Kiet Mai - remove mixpalay when sporttype = 2 (basketball
                if (sporttype == "2")
                {
                    //aMPB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_9"]);
                    aMPB = 0;
                }
                aMLB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_20"]);
                aHdpMaxB = mHdpB;
                aOuMaxB = mOuB;
                aOeMaxB = mOeB;
                aMPMaxB = mMPB;
                aMLMaxB = mMLB;
                #endregion
            }

            //display title of Basketball Position taking
            TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");

            TemplateHandler.Assign(ref strContentTpl, "{sporttype}", sporttype);
            TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.SuperMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.MasterMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{agent-position}", Resources.Language.AgentMaxPosition);

            //display max value Basketball Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)  
            TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(sHdpMinB, sHdpMaxB, sHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listSOUBValue}", CustomerUtils.GetListPositionTaking(sOuMinB, sOuMaxB, sOuB));
            TemplateHandler.Assign(ref strContentTpl, "{listSOEBValue}", CustomerUtils.GetListPositionTaking(sOeMinB, sOeMaxB, sOeB));
            
            //Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            /*if (sporttype == "2")
            {
                TemplateHandler.Assign(ref strContentTpl, "{listSMPBValue}", CustomerUtils.GetListPositionTaking(sMPMinB, sMPMaxB, sMPB));
            }*/

            TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(sMLMinB, sMLMaxB, sMLB));

            //display max value Basketball Position taking dropdowns of Master (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listMHdpBValue}", CustomerUtils.GetListPositionTaking(0, mHdpMaxB, mHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listMOUBValue}", CustomerUtils.GetListPositionTaking(0, mOuMaxB, mOuB));
            TemplateHandler.Assign(ref strContentTpl, "{listMOEBValue}", CustomerUtils.GetListPositionTaking(0, mOeMaxB, mOeB));

            //Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            /*if (sporttype == "2")
            {
                TemplateHandler.Assign(ref strContentTpl, "{listMMPBValue}", CustomerUtils.GetListPositionTaking(0, mMPMaxB, mMPB));
            }*/
            
            TemplateHandler.Assign(ref strContentTpl, "{listMMLBValue}", CustomerUtils.GetListPositionTaking(0, mMLMaxB, mMLB));

            //display max value Basketball Position taking dropdowns of Agent (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listAHdpBValue}", CustomerUtils.GetListPositionTaking(0, aHdpMaxB, aHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listAOUBValue}", CustomerUtils.GetListPositionTaking(0, aOuMaxB, aOuB));
            TemplateHandler.Assign(ref strContentTpl, "{listAOEBValue}", CustomerUtils.GetListPositionTaking(0, aOeMaxB, aOeB));
            
            //Kiet Mai - remove mixpalay when sporttype = 2 (basketball)            
            /*if (sporttype == "2")
            {
                TemplateHandler.Assign(ref strContentTpl, "{listAMPBValue}", CustomerUtils.GetListPositionTaking(0, aMPMaxB, aMPB));
            }*/
            
            TemplateHandler.Assign(ref strContentTpl, "{listAMLBValue}", CustomerUtils.GetListPositionTaking(0, aMLMaxB, aMLB));
            TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "3");
            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
    #endregion

    #region GenContentBaseballPT_v2
    private string GenContentBaseballPT_v2(string sporttype, DataRow dr)
    {
        StringBuilder html = new StringBuilder();
        double mHdpMaxB = 0.9, mOuMaxB = 0.9, mMLMaxB = 0.9;
        double aHdpMaxB = 0.9, aOuMaxB = 0.9, aMLMaxB = 0.9;
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/BaseballPositionTakingRow.htm"); ;
            html.Append(CustomerUtils.LoadBaseballPTHeader_v2(false, false, sporttype, dr, ""));

            //display title of Basketball Position taking
            TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");

            TemplateHandler.Assign(ref strContentTpl, "{sporttype}", sporttype);
            TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.SuperMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.MasterMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{agent-position}", Resources.Language.AgentMaxPosition);

            if (custId > 0 && dr != null) //edit
            {
                #region super
                //listSHdpBValue
                sHdpMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1_min"]);
                sHdpMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1_max"]);
                sHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1"]);

                //listSOUBValue
                sOuMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_3_min"]);
                sOuMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_3_max"]);
                sOuB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_3"]);

                //listSMLBValue
                sMLMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20_min"]);
                sMLMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20_max"]);
                sMLB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20"]);
                #endregion

                #region master
                mHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_1"]);
                mOuB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_3"]);
                mMLB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_20"]);

                mHdpMaxB = sHdpB;
                mOuMaxB = sOuB;
                mMLMaxB = sMLB;
                #endregion

                #region agent
                aHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_1"]);
                aOuB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_3"]);
                aMLB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_20"]);

                aHdpMaxB = mHdpB;
                aOuMaxB = mOuB;
                aMLMaxB = mMLB;
                #endregion
            }


            //display max value Basketball Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)  
            TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(sHdpMinB, sHdpMaxB, sHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listSOUBValue}", CustomerUtils.GetListPositionTaking(sOuMinB, sOuMaxB, sOuB));
            TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(sMLMinB, sMLMaxB, sMLB));

            //display max value Basketball Position taking dropdowns of Master (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listMHdpBValue}", CustomerUtils.GetListPositionTaking(0, mHdpMaxB, mHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listMOUBValue}", CustomerUtils.GetListPositionTaking(0, mOuMaxB, mOuB));
            TemplateHandler.Assign(ref strContentTpl, "{listMMLBValue}", CustomerUtils.GetListPositionTaking(0, mMLMaxB, mMLB));

            //display max value Basketball Position taking dropdowns of Agent (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listAHdpBValue}", CustomerUtils.GetListPositionTaking(0, aHdpMaxB, aHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listAOUBValue}", CustomerUtils.GetListPositionTaking(0, aOuMaxB, aOuB));
            TemplateHandler.Assign(ref strContentTpl, "{listAMLBValue}", CustomerUtils.GetListPositionTaking(0, aMLMaxB, aMLB));
            TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "3");
            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
    #endregion

    #region GenContentMotosportsPT_v2
    private string GenContentMotosportsPT_v2(string sporttype, DataRow dr)
    {
        double mHdpMaxB = 0.9, mMLMaxB = 0.9;
        double aHdpMaxB = 0.9, aMLMaxB = 0.9;
        StringBuilder html = new StringBuilder();
        try
        {
            string strContentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/MotosportsPositionTakingRow.htm"); ;
            html.Append(CustomerUtils.LoadMotosportsPTHeader_v2(false, false, sporttype, dr, ""));

            //display title of Basketball Position taking
            TemplateHandler.RemoveBlock(ref strContentTpl, "preset-pt");

            TemplateHandler.Assign(ref strContentTpl, "{sporttype}", sporttype);
            TemplateHandler.Assign(ref strContentTpl, "{super-position}", Resources.Language.SuperMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{master-position}", Resources.Language.MasterMaxPosition);
            TemplateHandler.Assign(ref strContentTpl, "{agent-position}", Resources.Language.AgentMaxPosition);

            if (custId > 0 && dr != null) //edit
            {
                #region super
                //listSHdpBValue
                sHdpMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1_min"]);
                sHdpMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1_max"]);
                sHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_1"]);

                //listSMLBValue
                sMLMinB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20_min"]);
                sMLMaxB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20_max"]);
                sMLB = FormatHandler.GetDouble(dr["v" + sporttype + "spositiontaking_20"]);
                #endregion

                #region master
                mHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_1"]);
                mMLB = FormatHandler.GetDouble(dr["v" + sporttype + "mpositiontaking_20"]);

                mHdpMaxB = sHdpB;
                mMLMaxB = sMLB;
                #endregion

                #region agent
                aHdpB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_1"]);
                aMLB = FormatHandler.GetDouble(dr["v" + sporttype + "apositiontaking_20"]);

                aHdpMaxB = mHdpB;
                aMLMaxB = mMLB;
                #endregion
            }

            //display max value Basketball Position taking dropdowns of Supper (from 0 to 0.9 for all bettypes)  
            TemplateHandler.Assign(ref strContentTpl, "{listSHdpBValue}", CustomerUtils.GetListPositionTaking(sHdpMinB, sHdpMaxB, sHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listSMLBValue}", CustomerUtils.GetListPositionTaking(sMLMinB, sMLMaxB, sMLB));

            //display max value Basketball Position taking dropdowns of Master (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listMHdpBValue}", CustomerUtils.GetListPositionTaking(0, mHdpMaxB, mHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listMMLBValue}", CustomerUtils.GetListPositionTaking(0, mMLMaxB, mMLB));

            //display max value Basketball Position taking dropdowns of Agent (from 0 to 0.9 for all bettypes)
            TemplateHandler.Assign(ref strContentTpl, "{listAHdpBValue}", CustomerUtils.GetListPositionTaking(0, aHdpMaxB, aHdpB));
            TemplateHandler.Assign(ref strContentTpl, "{listAMLBValue}", CustomerUtils.GetListPositionTaking(0, aMLMaxB, aMLB));
            TemplateHandler.Assign(ref strContentTpl, "{rowspan}", "3");
            html.Append(strContentTpl);
            html.Append("</td></tr></table>");
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
    #endregion

    #region NumberGame
    private string GetNumberGameSetting(DataRow  data)
    {
        double mHdpMaxB = 0.9, mMLMaxB = 0.9;
        double aHdpMaxB = 0.9, aMLMaxB = 0.9;
        SportBooksBase numberGame = new NumberGameManager();
        var numberGameSetting = numberGame.GetSuperPtSetting(data, custId);
        numberGame.GetMaxPerBall(data, 4, data == null, ref numberGameSetting);

        return numberGameSetting;
    }
    #endregion

    #region Mixed Sport
    private string GetMixedSportsSetting(DataRow data) 
    {
        SportBooksBase mixedSports = new MixedSportsManager();
        return mixedSports.GetSuperPtSetting(data, custId);
    }
    #endregion
}
