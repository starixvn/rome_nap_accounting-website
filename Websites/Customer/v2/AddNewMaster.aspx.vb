Imports System.Resources
Imports Sunplus.Accounting.Core.Extensions
Imports Sunplus.Accounting.Common
Imports DNAide.Web
Imports System.Text
Imports DNAide.Core
Imports System.Data

Imports IBCUtils.DAL
Imports IBCDLL
Imports IBCUtils
Imports IBCUtils.Web
Imports DNAide.Data
Imports BLL.Customer
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Web
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Business.Customer

Partial Class Customer_V2_AddNewMaster
    Inherits RequiredAuthorizationPage
    Public style As String
    'Dim res As New ResManager
    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis, strHidFinancial, strHidInfo As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidNumberGame, strHidOther, strTranferTime, strHidMixedSport As String
    Dim groupBtn As String
    Dim selectTpl As String
    Dim selDisabledTpl As String
    Protected superId As String = String.Empty

    Protected Overrides ReadOnly Property CustId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.SuperId)
        End Get
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MenuPT.htm")
            groupBtn = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BtnGroup.htm")
            selectTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/dropdownPT.htm")
            tennisTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/TennisPT.htm")
            basketballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BasketballPT.htm")
            footballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/FootballPT.htm")
            otherSportsTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/OtherSportsPT.htm")
            golfTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/GolfPT.htm")
            strTranferTime = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/EditTranferTime.htm")

            selDisabledTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/DdlDisablePT.htm")

            htmlAddMasterContent.Text = LoadData()
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub

#Region "Declare parameter for Script"
    'parameters is used in client script functions
    Public Sub VariableOfScript()
        Dim script As String = ""
        script += "<script language='javascript'>" & vbNewLine
        script += " var alertcredit='" & Resources.Language.alertIncorrectAmaxcredit & "';" & vbNewLine
        script += " var alertMinbet='" & Resources.Language.alertIncorrectMinBet & "'; " & vbNewLine
        script += " var alertMaxbet='" & Resources.Language.alertIncorrectMaxBet & "';" & vbNewLine
        script += " var alertMaxpermatch='" & Resources.Language.alertIncorrectMaxPerMatch & "';" & vbNewLine
        script += " var alertCompare3='" & Resources.Language.alertMaxLessMin & "'; " & vbNewLine
        script += " var alertCompare='" & Resources.Language.alertMaxLessMaxPer & "'; " & vbNewLine
        script += " var alertminbetC='" & Resources.Language.lblalertMinbetCompare & "';" & vbNewLine
        script += " var alertMaxbetC='" & Resources.Language.lblalerMaxbetCompare & "';" & vbNewLine
        script += " var alertMaxpermatchC='" & Resources.Language.lblalertMaxPerMatchCompare & "';" & vbNewLine
        script += " var alertMaxpermatchCMin='" & Resources.Language.lblalertMaxPerMatchCompareMin & "';" & vbNewLine
        script += " var alertMaxpermatchCMax='" & Resources.Language.lblalertMaxPerMatchCompareMax & "';" & vbNewLine
        script += " var alertUserName='" & Resources.Language.alertUserName & "';" & vbNewLine
        script += " var lblPasswordRequire='" & Resources.Language.lblPasswordRequire & "';" & vbNewLine
        script += " var alertPasswordEmpty='" & Resources.Language.alertPasswordEmpty & "';" & vbNewLine
        script += " var alertIncorrectCredit='" & Resources.Language.alertIncorrectCredit & "';" & vbNewLine

        script += " var lblChooseDaily='" & Resources.Language.lblConfirmChooseDaily & "';" & vbNewLine
        script += " var lblChooseOneDay='" & Resources.Language.lblChooseOneDay & "';" & vbNewLine
        Dim lblCreditInvalid As String
        lblCreditInvalid = Resources.Language.lblCreditInvalid.Replace("{0}", Resources.Language.lblMasterMaxCredit)
        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        Dim strAlertMinPT As String = Resources.Language.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Language.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Language.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "function SetValueMemMaxCredit(){}"
        script += "</script>"
        Response.Write(script)
    End Sub
#End Region

#Region "Load Information for Add Account"
    Function LoadData() As String
        Dim strAddContent As New StringBuilder
        Try
            Dim rs As DataSet
            rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.CustomerSelSAgentInfoRpt(Request("superId"))
            superId = Request("superId")

            If Not rs Is Nothing Then
                If rs.Tables(0).Rows.Count > 0 Then
                    Dim row As DataRow = rs.Tables(0).Rows(0)
                    CustomerUtils.GetSportTitle(bodyTpl, False)
                    TemplateHandler.Assign(bodyTpl, "accinfo", GetDataInfo(row))
                    TemplateHandler.Assign(bodyTpl, "add-text", Resources.Language.btnAdd)
                    TemplateHandler.Assign(bodyTpl, "add-id", "btnAdd")

                    TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row))
                    TemplateHandler.Assign(bodyTpl, "basketPT", GetBasketball(row, 2, basketballTpl))
                    TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl))
                    TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl))
                    TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8))
                    TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl))
                    TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11))
                    TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl))

                    If Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled Then
                        TemplateHandler.Assign(bodyTpl, "NumberGamePT", GetDataNumberGame(row))
                    Else
                        TemplateHandler.RemoveBlock(bodyTpl, "NumberGame")
                    End If

                    TemplateHandler.Assign(bodyTpl, "MixedSportsPT", GetDataMixedSports(row))

                    bodyTpl &= strHidInfo
                    bodyTpl &= strHidSoccer
                    bodyTpl &= strHidBasket
                    bodyTpl &= strHidFootball
                    bodyTpl &= strHidTennis
                    bodyTpl &= strHidBaseball
                    bodyTpl &= strHidGolf
                    bodyTpl &= strHidMoto
                    bodyTpl &= strHidNumberGame
                    bodyTpl &= strHidMixedSport
                    bodyTpl &= strHidOther

                    strAddContent.Append(bodyTpl)
                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        LoadData = strAddContent.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow) As String
        Dim strAccInfo As String = String.Empty
        Dim hashInfo As New Hashtable
        Dim isInternal As Boolean = False
        Try
            strAccInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/AccInfoSuper.htm", True)
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-add")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
            TemplateHandler.RemoveBlock(strAccInfo, "Member-Max-Credit")

            TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
            Dim superUName As String = ""
            If Request("username") Is Nothing Then
                superUName = Session("CustUname").ToString()
            Else
                superUName = Request("username")
            End If
            TemplateHandler.Assign(strAccInfo, "title", Resources.Language.lblAddNewMaster & " to " & superUName)
            TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Language.lblMasterMaxCredit)

            'If (CastAide.AsBoolean(dr("suspended"))) Then
            '    TemplateHandler.Assign(strAccInfo, "suspend-check", "checked='checked'")
            'Else
            '    TemplateHandler.Assign(strAccInfo, "suspend-check", "")
            'End If

            Dim isSuspended As Boolean = CastAide.AsBoolean(dr("suspended"))
            'TemplateHandler.Assign(strAccInfo, "chkSuspended", HtmlHelper.GenCheckBox("chkSuspended", False, isSuspended, "", Globals.CLASS_CHECKBOX_HIDDEN))
            hashInfo.Add("chkSuspended", IIf(isSuspended, 1, 0))


            If (Not Permissions.IsEnabledAutoMaster(True)) Then
                TemplateHandler.RemoveBlock(strAccInfo, "automasterpt")
            Else
                TemplateHandler.Assign(strAccInfo, "{autompt}", "checked='true'")
            End If
            If DataUtils.IsExistKey(dr, "isinternal") Then
                isInternal = CastAide.AsBoolean(dr("isinternal"), False)
            End If
            'If (WebSettings.DisableInternal Or isInternal) Then
            TemplateHandler.ReplaceBlock(strAccInfo, "internal", HtmlHelper.GenTD(1, 2, ""))
            'Else
            ' TemplateHandler.Assign(strAccInfo, "{lblInternalUser}", Resources.Language.lblInternalUser)
            ' TemplateHandler.Assign(strAccInfo, "chkInternal", HtmlHelper.GenCheckBox("chkInternal", False, isInternal, "", ""))
            ' End If
            hashInfo.Add("hidinternal", isInternal.ToString().ToLower())
            strHidInfo = CustomerUtils.AssignMultipleHidden(hashInfo)

            TemplateHandler.Assign(strAccInfo, "fname-val", "")
            TemplateHandler.Assign(strAccInfo, "lname-val", "")
            TemplateHandler.Assign(strAccInfo, "phone-val", "")
            TemplateHandler.Assign(strAccInfo, "mobile-val", "")
            'display value,limit of credit
            CustomerUtils.GetCredit(dr, strAccInfo, False)
            TemplateHandler.Assign(strAccInfo, "check-val", "")
            TemplateHandler.Assign(strAccInfo, "fax-val", "")

            TemplateHandler.Assign(strAccInfo, "groupa-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupa")), CastAide.AsDouble(dr("groupa"))))
            TemplateHandler.Assign(strAccInfo, "groupb-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupb")), CastAide.AsDouble(dr("groupb"))))
            TemplateHandler.Assign(strAccInfo, "groupc-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupc")), CastAide.AsDouble(dr("groupc"))))
            TemplateHandler.Assign(strAccInfo, "groupd-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupd")), CastAide.AsDouble(dr("groupd"))))
            TemplateHandler.Assign(strAccInfo, "discountcs-val", CustomerUtils.GetListOtherComm(CastAide.AsDouble(dr("discountcs")), CastAide.AsDouble(dr("discountcs"))))
            TemplateHandler.Assign(strAccInfo, "sel-dis", "")
            If WebSettings.Disable1x2Comm Then
                TemplateHandler.RemoveBlock(strAccInfo, "none-1x2-Comm")
            Else
                TemplateHandler.Assign(strAccInfo, "sel-1x2", "")
                Try
                    TemplateHandler.Assign(strAccInfo, "discountcs1x2-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("discount1x2")), CastAide.AsDouble(dr("discount1x2"))))
                Catch ex As Exception
                    LogExtensions.ExceptionHandler(ex)

                End Try

            End If
            strAccInfo += HtmlHelper.GenHidden("uplineusername", superUName)
            Dim strCheckText() As String = {"{sun-chk}", "{mon-chk}", "{tue-chk}", "{wed-chk}", "{thurs-chk}", "{fri-chk}", "{sat-chk}"}
            Dim rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.Select4Edit.TransferOptionSel(CustId)
            
            If Not rs Is Nothing Then
                If Not rs.Outputs("transferoption") Is Nothing And rs.Outputs("transferoption").ToString() <> ""
                    If rs.Outputs("transferoption").ToString().Equals("00000000")
                        TemplateHandler.Assign(strTranferTime, "{referupline}", "0")                        
                        TemplateHandler.Assign(strTranferTime, "{showtransfer}", "")
                    Else
                        TemplateHandler.Assign(strTranferTime, "{referupline}", "1")
                        TemplateHandler.Assign(strTranferTime, "{showtransfer}", "display:none;")
                    End If
                    

                End If
            End If


            CustomerUtils.GetTranferTime(strTranferTime, False)
            TemplateHandler.Assign(strAccInfo, "tranfer-time", strTranferTime)
            CustomerUtils.GetTitleInfo(strAccInfo, False)
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        Dim canupdatecredit As Boolean
        canupdatecredit = Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit)

        If canupdatecredit Then
            TemplateHandler.Assign(strAccInfo, "{disable_credit}", "")
            TemplateHandler.Assign(strAccInfo, "{diable_credit}", "")
            TemplateHandler.Assign(strAccInfo, "{mouseover_credit}", "")
            TemplateHandler.Assign(strAccInfo, "{mouseout_credit}", "")
        Else

            TemplateHandler.Assign(strAccInfo, "{disable_color}", "background-color:#E8E8E8; border:solid 1px #7f9db9;color:#A0A0A0;")
            TemplateHandler.Assign(strAccInfo, "{diable_credit}", "readonly")
            TemplateHandler.Assign(strAccInfo, "{mouseout_credit}", "onmouseout='UnTip()'")
            TemplateHandler.Assign(strAccInfo, "{mouseover_credit}", "onmouseover=""Tip('You do not have permission on update credit.', BALLOON, true, ABOVE, true, OFFSETX, -100, PADDING, 4)""")
        End If

        Dim numbergame As SportBooksBase = New NumberGameManager()
        numbergame.GetNumberCom(dr, 3, True, strAccInfo)


        GetDataInfo = strAccInfo
    End Function
#End Region

#Region "Assign Info for Soccer"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataSoccer(ByVal dr As DataRow) As String
        Dim soccerTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty
        Try
            soccerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SoccerPT.htm")
            headerSTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SBettypeHeader.htm")
            ' assign all text according language
            'TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
            TemplateHandler.RemoveBlock(soccerTpl, "none-edit")
            GetTitleCommon(soccerTpl)
            TemplateHandler.Assign(soccerTpl, "col-title", "12")
            TemplateHandler.Assign(soccerTpl, "bettype-header", CustomerUtils.LoadSoccerPTHeader(False))
            TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Language.Deadball)
            TemplateHandler.Assign(soccerTpl, "live-text", Resources.Language.Live)
            TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Language.lblMasterPresetPosition)
            TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Language.lblSuperPresetPosition)
            TemplateHandler.Assign(soccerTpl, "dead-row", "5")
            TemplateHandler.Assign(soccerTpl, "live-row", "4")
            TemplateHandler.Assign(soccerTpl, "copy-live-col", "4")
            TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Language.CopyFrom1x2)

            TemplateHandler.Assign(soccerTpl, "chkAuto-1", "chkAuto1HdpD")
            TemplateHandler.Assign(soccerTpl, "chkAuto-2", "chkAuto11x2")
            TemplateHandler.Assign(soccerTpl, "chkAuto-3", "chkAuto1HdpL")

            TemplateHandler.Assign(soccerTpl, "au-presetpt-text", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.Assign(soccerTpl, "au-preset-live", Resources.Language.lblAutoSuperPresetPosition)

            'assign value of Minbet, Maxbet , ...
            CustomerUtils.GetMinMaxAddValue(soccerTpl, 1, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, soccerTpl, "")

            'assign value position taking for template
            Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
            Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

            Dim sHdpMin, sOUMin, s1stHdpMin, s1stOUMin, sOEMin, s1x2Min, sCSMin, sTGMin, sMPMin, sORMin As Double
            Dim sLHdpMin, sLOUMin, sL1stHdpMin, sL1stOUMin As Double

            Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
            Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

            Dim mHdpMax2, mOUMax2, m1stHdpMax2, m1stOUMax2, mOEMax2, m1x2Max2, mCSMax2, mTGMax2, mMPMax2, mORMax2 As Double
            Dim mLHdpMax2, mLOUMax2, mL1stHdpMax2, mL1stOUMax2 As Double

            Dim sHdpMax, sOUMax, s1stHdpMax, s1stOUMax, sOEMax, s1x2Max, sCSMax, sTGMax, sMPMax, sORMax As Double
            Dim sLHdpMax, sLOUMax, sL1stHdpMax, sL1stOUMax As Double

            Dim msHdpMax, msOUMax, ms1stHdpMax, ms1stOUMax, msOEMax, ms1x2Max, msCSMax, msTGMax, msMPMax, msORMax As Double
            Dim msLHdpMax, msLOUMax, msL1stHdpMax, msL1stOUMax As Double

            Dim msHdpMin, msOUMin, ms1stHdpMin, ms1stOUMin, msOEMin, ms1x2Min, msCSMin, msTGMin, msMPMin, msORMin As Double
            Dim msLHdpMin, msLOUMin, msL1stHdpMin, msL1stOUMin As Double

            Dim sAutoHdp, sAutoOU, sAuto1stHdp, sAuto1stOU, sAutoOE, sAuto1x2, sAutoCS, sAutoTG, sAutoMP, sAutoOR As Double
            Dim sAutoLHdp, sAutoLOU, sAutoL1stHdp, sAutoL1stOU As Double

            mHdpMax2 = CastAide.AsDouble(dr("mpositiontaking_1_max"))
            mOUMax2 = CastAide.AsDouble(dr("mpositiontaking_3_max"))
            m1stHdpMax2 = CastAide.AsDouble(dr("mpositiontaking_7_max"))
            m1stOUMax2 = CastAide.AsDouble(dr("mpositiontaking_8_max"))
            mOEMax2 = CastAide.AsDouble(dr("mpositiontaking_2_max"))
            m1x2Max2 = CastAide.AsDouble(dr("mpositiontaking_5_max"))
            mCSMax2 = CastAide.AsDouble(dr("mpositiontaking_4_max"))
            mTGMax2 = CastAide.AsDouble(dr("mpositiontaking_6_max"))
            mMPMax2 = CastAide.AsDouble(dr("mpositiontaking_9_max"))
            mORMax2 = CastAide.AsDouble(dr("mpositiontaking_10_max"))
            mLHdpMax2 = CastAide.AsDouble(dr("mlivepositiontaking_1_max"))
            mLOUMax2 = CastAide.AsDouble(dr("mlivepositiontaking_3_max"))
            mL1stHdpMax2 = CastAide.AsDouble(dr("mlivepositiontaking_7_max"))
            mL1stOUMax2 = CastAide.AsDouble(dr("mlivepositiontaking_8_max"))

            sHdpMax = CastAide.AsDouble(dr("spositiontaking_1"))
            sOUMax = CastAide.AsDouble(dr("spositiontaking_3"))
            s1stHdpMax = CastAide.AsDouble(dr("spositiontaking_7"))
            s1stOUMax = CastAide.AsDouble(dr("spositiontaking_8"))
            sOEMax = CastAide.AsDouble(dr("spositiontaking_2"))
            s1x2Max = CastAide.AsDouble(dr("spositiontaking_5"))
            sCSMax = CastAide.AsDouble(dr("spositiontaking_4"))
            sTGMax = CastAide.AsDouble(dr("spositiontaking_6"))
            sMPMax = CastAide.AsDouble(dr("spositiontaking_9"))
            sORMax = CastAide.AsDouble(dr("spositiontaking_10"))
            sLHdpMax = CastAide.AsDouble(dr("slivepositiontaking_1"))
            sLOUMax = CastAide.AsDouble(dr("slivepositiontaking_3"))
            sL1stHdpMax = CastAide.AsDouble(dr("slivepositiontaking_7"))
            sL1stOUMax = CastAide.AsDouble(dr("slivepositiontaking_8"))

            'this is used to save into hidden fields to check in client script '''''''
            msHdpMax = CastAide.AsDouble(dr("mspositiontaking_1_max"))
            msOUMax = CastAide.AsDouble(dr("mspositiontaking_3_max"))
            ms1stHdpMax = CastAide.AsDouble(dr("mspositiontaking_7_max"))
            ms1stOUMax = CastAide.AsDouble(dr("mspositiontaking_8_max"))
            msOEMax = CastAide.AsDouble(dr("mspositiontaking_2_max"))
            ms1x2Max = CastAide.AsDouble(dr("mspositiontaking_5_max"))
            msCSMax = CastAide.AsDouble(dr("mspositiontaking_4_max"))
            msTGMax = CastAide.AsDouble(dr("mspositiontaking_6_max"))
            msMPMax = CastAide.AsDouble(dr("mspositiontaking_9_max"))
            msORMax = CastAide.AsDouble(dr("mspositiontaking_10_max"))
            msLHdpMax = CastAide.AsDouble(dr("mslivepositiontaking_1_max"))
            msLOUMax = CastAide.AsDouble(dr("mslivepositiontaking_3_max"))
            msL1stHdpMax = CastAide.AsDouble(dr("mslivepositiontaking_7_max"))
            msL1stOUMax = CastAide.AsDouble(dr("mslivepositiontaking_8_max"))

            msHdpMin = CastAide.AsDouble(dr("mspositiontaking_1_min"))
            msOUMin = CastAide.AsDouble(dr("mspositiontaking_3_min"))
            ms1stHdpMin = CastAide.AsDouble(dr("mspositiontaking_7_min"))
            ms1stOUMin = CastAide.AsDouble(dr("mspositiontaking_8_min"))
            msOEMin = CastAide.AsDouble(dr("mspositiontaking_2_min"))
            ms1x2Min = CastAide.AsDouble(dr("mspositiontaking_5_min"))
            msCSMin = CastAide.AsDouble(dr("mspositiontaking_4_min"))
            msTGMin = CastAide.AsDouble(dr("mspositiontaking_6_min"))
            msMPMin = CastAide.AsDouble(dr("mspositiontaking_9_min"))
            msORMin = CastAide.AsDouble(dr("mspositiontaking_10_min"))
            msLHdpMin = CastAide.AsDouble(dr("mslivepositiontaking_1_min"))
            msLOUMin = CastAide.AsDouble(dr("mslivepositiontaking_3_min"))
            msL1stHdpMin = CastAide.AsDouble(dr("mslivepositiontaking_7_min"))
            msL1stOUMin = CastAide.AsDouble(dr("mslivepositiontaking_8_min"))

            sAutoHdp = CastAide.AsDouble(dr("auto_spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_spositiontaking_3"))
            sAuto1stHdp = CastAide.AsDouble(dr("auto_spositiontaking_7"))
            sAuto1stOU = CastAide.AsDouble(dr("auto_spositiontaking_8"))
            sAutoOE = CastAide.AsDouble(dr("auto_spositiontaking_2"))
            sAuto1x2 = CastAide.AsDouble(dr("auto_spositiontaking_5"))
            sAutoCS = CastAide.AsDouble(dr("auto_spositiontaking_4"))
            sAutoTG = CastAide.AsDouble(dr("auto_spositiontaking_6"))
            sAutoMP = CastAide.AsDouble(dr("auto_spositiontaking_9"))
            sAutoOR = CastAide.AsDouble(dr("auto_spositiontaking_10"))
            sAutoLHdp = CastAide.AsDouble(dr("auto_slivepositiontaking_1"))
            sAutoLOU = CastAide.AsDouble(dr("auto_slivepositiontaking_3"))
            sAutoL1stHdp = CastAide.AsDouble(dr("auto_slivepositiontaking_7"))
            sAutoL1stOU = CastAide.AsDouble(dr("auto_slivepositiontaking_8"))
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            mHdpMax = msHdpMax - sHdpMax
            mOUMax = msOUMax - sOUMax
            m1stHdpMax = ms1stHdpMax - s1stHdpMax
            m1stOUMax = ms1stOUMax - s1stOUMax
            mOEMax = msOEMax - sOEMax
            m1x2Max = ms1x2Max - s1x2Max
            mCSMax = msCSMax - sCSMax
            mTGMax = msTGMax - sTGMax
            mMPMax = msMPMax - sMPMax
            mORMax = msORMax - sORMax
            mLHdpMax = msLHdpMax - sLHdpMax
            mLOUMax = msLOUMax - sLOUMax
            mL1stHdpMax = msL1stHdpMax - sL1stHdpMax
            mL1stOUMax = msL1stOUMax - sL1stOUMax

            'hiddenHash.Add("autoS1HdpD", sAutoHdp)
            'hiddenHash.Add("autoS1OUD", sAutoOU)
            'hiddenHash.Add("autoS11stHdpD", sAuto1stHdp)
            'hiddenHash.Add("autoS11stOUD", sAuto1stOU)
            'hiddenHash.Add("autoS1OE", sAutoOE)

            'hiddenHash.Add("autoS11x2", sAuto1x2)
            'hiddenHash.Add("autoS1CS", sAutoCS)
            'hiddenHash.Add("autoS1TG", sAutoTG)
            'hiddenHash.Add("autoS1MP", sAutoMP)
            'hiddenHash.Add("autoS1OR", sAutoOR)

            'hiddenHash.Add("autoS1HdpL", sAutoLHdp)
            'hiddenHash.Add("autoS1OUL", sAutoLOU)
            'hiddenHash.Add("autoS11stHdpL", sAutoL1stHdp)
            'hiddenHash.Add("autoS11stOUL", sAutoL1stOU)
            hiddenHash.Add("uplineid", superId) 'superid hidden field

            hiddenHash.Add("maxMS1HdpD", msHdpMax)
            hiddenHash.Add("maxMS1OUD", msOUMax)
            hiddenHash.Add("maxMS11stHdpD", ms1stHdpMax)
            hiddenHash.Add("maxMS11stOUD", ms1stOUMax)
            hiddenHash.Add("maxMS1OE", msOEMax)

            hiddenHash.Add("maxMS11x2", ms1x2Max)
            hiddenHash.Add("maxMS1CS", msCSMax)
            hiddenHash.Add("maxMS1TG", msTGMax)
            hiddenHash.Add("maxMS1MP", msMPMax)
            hiddenHash.Add("maxMS1OR", msORMax)

            hiddenHash.Add("maxMS1HdpL", msLHdpMax)
            hiddenHash.Add("maxMS1OUL", msLOUMax)
            hiddenHash.Add("maxMS11stHdpL", msL1stHdpMax)
            hiddenHash.Add("maxMS11stOUL", msL1stOUMax)

            hiddenHash.Add("minMS1HdpD", msHdpMin)
            hiddenHash.Add("minMS1OUD", msOUMin)
            hiddenHash.Add("minMS11stHdpD", ms1stHdpMin)
            hiddenHash.Add("minMS11stOUD", ms1stOUMin)
            hiddenHash.Add("minMS1OE", msOEMin)

            hiddenHash.Add("minMS11x2", ms1x2Min)
            hiddenHash.Add("minMS1CS", msCSMin)
            hiddenHash.Add("minMS1TG", msTGMin)
            hiddenHash.Add("minMS1MP", msMPMin)
            hiddenHash.Add("minMS1OR", msORMin)

            hiddenHash.Add("minMS1HdpL", msLHdpMin)
            hiddenHash.Add("minMS1OUL", msLOUMin)
            hiddenHash.Add("minMS11stHdpL", msL1stHdpMin)
            hiddenHash.Add("minMS11stOUL", msL1stOUMin)

            hiddenHash.Add("maxM1HdpD", mHdpMax2)
            hiddenHash.Add("maxM1OUD", mOUMax2)
            hiddenHash.Add("maxM11stHdpD", m1stHdpMax2)
            hiddenHash.Add("maxM11stOUD", m1stOUMax2)
            hiddenHash.Add("maxM1OE", mOEMax2)

            hiddenHash.Add("maxM11x2", m1x2Max2)
            hiddenHash.Add("maxM1CS", mCSMax2)
            hiddenHash.Add("maxM1TG", mTGMax2)
            hiddenHash.Add("maxM1MP", mMPMax2)
            hiddenHash.Add("maxM1OR", mORMax2)

            hiddenHash.Add("maxM1HdpL", mLHdpMax2)
            hiddenHash.Add("maxM1OUL", mLOUMax2)
            hiddenHash.Add("maxM11stHdpL", mL1stHdpMax2)
            hiddenHash.Add("maxM11stOUL", mL1stOUMax2)

            strHidSoccer = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign Increase and Reduce button for template
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(soccerTpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(soccerTpl, "a-1x2-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(soccerTpl, "p-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(soccerTpl, "p-1x2-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", btnTpl)

            'get PT deadball value for master preset PT
            Dim pt As String = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
            TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdpMax))
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOUMax))
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
            TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1x2Min, m1x2Max, m1x2Max))
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mCSMin, mCSMax, mCSMax))
            TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mTGMin, mTGMax, mTGMax))
            TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
            TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mORMin, mORMax, mORMax))
            TemplateHandler.Assign(soccerTpl, "a-or-val", pt)

            'get PT deadball value for super preset PT
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1stHdpMin, s1stHdpMax, s1stHdpMax))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOUMax))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOEMax))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1x2Min, s1x2Max, s1x2Max))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sCSMin, sCSMax, sCSMax))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sTGMin, sTGMax, sTGMax))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMPMin, sMPMax, sMPMax))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sORMin, sORMax, sORMax))
            TemplateHandler.Assign(soccerTpl, "p-or-val", pt)

            'get PT value for super preset PT
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdpMax))
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mLOUMin, mLOUMax, mLOUMax))
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdpMax))
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOUMax))
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sLHdpMin, sLHdpMax, sLHdpMax))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sLOUMin, sLOUMax, sLOUMax))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sL1stHdpMin, sL1stHdpMax, sL1stHdpMax))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sL1stOUMin, sL1stOUMax, sL1stOUMax))
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", pt)

            ' Get value for Auto Postion taking-------------------------------------------------------------------------------

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)

            'For Deadball

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdp, 0))
            TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOU, 0))
            TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stHdp, 0))
            TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stOU, 0))
            TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOE, 0))
            TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1x2, 0))
            TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoCS, 0))
            TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoTG, 0))
            TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMP, 0))
            TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOR, 0))
            TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)




            'For live ball
            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdp, 0))
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOU, 0))
            TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stHdp, 0))
            TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stOU, 0))
            TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)

            '-------------------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try

        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even,9: Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball)
    Function GetBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty
        Try
            'assign text for position taking
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "mp-text", Resources.Language.MixParlay)
            TemplateHandler.Assign(tpl, "title-col", "7")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            TemplateHandler.RemoveBlock(tpl, "no-mixparlay")

            'assign value for position taking
            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMPMin, mMLMin As Double
            Dim sHdpMin, sOUMin, sOEMin, sMPMin, sMLMin As Double

            Dim mHdpMax, mOUMax, mOEMax, mMPMax, mMLMax As Double
            Dim mHdpMax2, mOUMax2, mOEMax2, mMPMax2, mMLMax2 As Double
            Dim sHdpMax, sOUMax, sOEMax, sMPMax, sMLMax As Double

            Dim msHdpMax, msOUMax, msOEMax, msMPMax, msMLMax As Double
            Dim msHdpMin, msOUMin, msOEMin, msMPMin, msMLMin As Double
            Dim sAutoHdp, sAutoOU, sAutoOE, sAutoMP, sAutoML As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
            mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'mMPMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'sMPMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_9"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
            msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
            msOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_max"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'msMPMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_9_max"))
            msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

            msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
            msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
            msOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_min"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'msMPMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_9_min"))
            msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
            sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'sAutoMP = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_9"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            mHdpMax = msHdpMax - sHdpMax
            mOUMax = msOUMax - sOUMax
            mOEMax = msOEMax - sOEMax
            mMPMax = msMPMax - sMPMax
            mMLMax = msMLMax - sMLMax

            'hiddenHash.Add("autoS" & sportTypeId & "Hdp", sAutoHdp)
            'hiddenHash.Add("autoS" & sportTypeId & "OU", sAutoOU)
            'hiddenHash.Add("autoS" & sportTypeId & "OE", sAutoOE)
            'hiddenHash.Add("autoS" & sportTypeId & "MP", sAutoMP)
            'hiddenHash.Add("autoS" & sportTypeId & "ML", sAutoML)

            hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
            hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
            hiddenHash.Add("maxMS" & sportTypeId & "OE", msOEMax)
            hiddenHash.Add("maxMS" & sportTypeId & "MP", msMPMax)
            hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

            hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
            hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
            hiddenHash.Add("minMS" & sportTypeId & "OE", msOEMin)
            hiddenHash.Add("minMS" & sportTypeId & "MP", msMPMin)
            hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
            hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
            hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax2)
            hiddenHash.Add("maxM" & sportTypeId & "MP", mMPMax2)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

            strHidBasket = CustomerUtils.AssignMultipleHidden(hiddenHash)
            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
            TemplateHandler.Assign(tpl, "a-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOEMax))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'pt = selectTpl
            'TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "MP")
            'TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMPMin, sMPMax, sMPMax))
            'TemplateHandler.Assign(tpl, "p-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '----------------------------------------------------------------------------------------------------------------

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdp, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOU, 0))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOE, 0))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OE", String.Empty)

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'ptAuto = selDisabledTpl
            'TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMP, 0))
            'TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)
            'TemplateHandler.Assign(tpl, "checked-MP", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoML, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball, 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty

        Try
            'assign text for position taking
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "title-col", "6")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

            TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.RemoveBlock(tpl, "none-edit")

            'assign value for position taking
            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
            Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double

            Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
            Dim mHdpMax2, mOUMax2, mOEMax2, mMLMax2 As Double
            Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

            Dim msHdpMax, msOUMax, msOEMax, msMLMax As Double
            Dim msHdpMin, msOUMin, msOEMin, msMLMin As Double
            Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
            mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
            msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
            msOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_max"))
            msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

            msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
            msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
            msOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_min"))
            msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
            sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            mHdpMax = msHdpMax - sHdpMax
            mOUMax = msOUMax - sOUMax
            mOEMax = msOEMax - sOEMax
            mMLMax = msMLMax - sMLMax

            'hiddenHash.Add("autoS" & sportTypeId & "Hdp", sAutoHdp)
            'hiddenHash.Add("autoS" & sportTypeId & "OU", sAutoOU)
            'hiddenHash.Add("autoS" & sportTypeId & "OE", sAutoOE)
            'hiddenHash.Add("autoS" & sportTypeId & "ML", sAutoML)

            hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
            hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
            hiddenHash.Add("maxMS" & sportTypeId & "OE", msOEMax)
            hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

            hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
            hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
            hiddenHash.Add("minMS" & sportTypeId & "OE", msOEMin)
            hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
            hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
            hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax2)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

            Select Case sportTypeId
                Case 3
                    strHidFootball = CustomerUtils.AssignMultipleHidden(hiddenHash)
                Case 5
                    strHidTennis = CustomerUtils.AssignMultipleHidden(hiddenHash)
                Case 10
                    strHidGolf = CustomerUtils.AssignMultipleHidden(hiddenHash)
                Case 99
                    strHidOther = CustomerUtils.AssignMultipleHidden(hiddenHash)
            End Select

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOEMax))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
            '----------------------------------------------------------------------------------------------------------------


            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdp, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOU, 0))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOE, 0))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OE", String.Empty)


            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoML, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)

            Dim btnTpl As String = groupBtn
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

        Catch ex As Exception
           LogExtensions.ExceptionHandler(ex)
        End Try
        GetData4DDl = tpl
    End Function
#End Region

#Region "Assign info for  Baseball"
    'this function is used to assign data into template for Baseball
    'dr: row contains data get from db
    'sportTypeId: id of sporttype
    'Baseball have only Handicap, Over/Under & Money Line bettype
    Function GetDataBaseball(ByVal dr As DataRow, ByVal sportTypeId As Integer) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BaseballPT.htm")
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "title-col", "5")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

            TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'assign value for position taking
            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMax, mOUMax, mMLMax As Double
            Dim mHdpMax2, mOUMax2, mMLMax2 As Double
            Dim mHdpMin, mOUMin, mMLMin As Double

            Dim sHdpMax, sOUMax, sMLMax As Double
            Dim sHdpMin, sOUMin, sMLMin As Double
            Dim sAutoHdp, sAutoOU, sAutoML As Double

            Dim msHdpMax, msOUMax, msMLMax As Double
            Dim msHdpMin, msOUMin, msMLMin As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
            msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
            msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

            msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
            msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
            msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            mHdpMax = msHdpMax - sHdpMax
            mOUMax = msOUMax - sOUMax
            mMLMax = msMLMax - sMLMax

            'hiddenHash.Add("autoS" & sportTypeId & "Hdp", sAutoHdp)
            'hiddenHash.Add("autoS" & sportTypeId & "OU", sAutoOU)
            'hiddenHash.Add("autoS" & sportTypeId & "ML", sAutoML)

            hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
            hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
            hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

            hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
            hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
            hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
            hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

            strHidBaseball = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOUMax))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)

            '----------------------------------------------------------------------------------------------------------------

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdp, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOU, 0))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoML, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)

            Dim btnTpl As String = groupBtn

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataBaseball = tpl
    End Function
#End Region

#Region "Assign info for MotoSports"
    'this function is used to assign data into template for MotoSports
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (11: motosports)
    'Motorsports have only Handicap & Money Line bettype
    Function GetDataMotoSports(ByVal dr As DataRow, ByVal sportTypeId As Integer) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable
        Dim ptAuto As String = String.Empty
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MotorSportsPT.htm")
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "title-col", "4")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.RemoveBlock(tpl, "none-edit")

            'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)            
            CustomerUtils.GetMinMaxAddValue(tpl, 11, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            'get data and assign for template Position taking
            Dim mHdpMax, mMLMax As Double
            Dim mHdpMax2, mMLMax2 As Double
            Dim mHdpMin, mMLMin As Double

            Dim sHdpMax, sMLMax As Double
            Dim sHdpMin, sMLMin As Double

            Dim msHdpMax, msMLMax As Double
            Dim msHdpMin, msMLMin As Double
            Dim sAutoHdp, sAutoML As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
            msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

            msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
            msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            mHdpMax = msHdpMax - sHdpMax
            mMLMax = msMLMax - sMLMax

            'hiddenHash.Add("autoS" & sportTypeId & "Hdp", sAutoHdp)
            'hiddenHash.Add("autoS" & sportTypeId & "ML", sAutoML)

            hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
            hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

            hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
            hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

            strHidMoto = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdpMax))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sMLMax))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)

            '----------------------------------------------------------------------------------------------------------------

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdp, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoML, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)

            Dim btnTpl As String = groupBtn

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign info for Number Game"
    Function GetDataNumberGame(ByVal dr As DataRow) As String
        Dim numbergame As SportBooksBase = New NumberGameManager()
        TemplateHandler.Assign(bodyTpl, "NumberGame-text", Resources.Language.lblNumberGame)
        numbergame.GetNumberCom(dr, 3, True, bodyTpl)
        Dim numberGameSetting As String
        numberGameSetting = numbergame.GetMasterPtSetting_AddNew(dr, strHidNumberGame)
        numbergame.GetMaxPerBall(dr, 3, True, numberGameSetting)
        Return numberGameSetting
    End Function
#End Region

#Region "Assign info for Mixed Sports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim mixedSport As SportBooksBase = New MixedSportsManager()
        TemplateHandler.Assign(bodyTpl, "MixedSports-text", Resources.Language.lblMixedSports)
        Return mixedSport.GetMasterPtSetting_AddNew(dr, strHidMixedSport)
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        'TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.Assign(tpl, "bet-text", Resources.Language.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Language.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Language.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Language.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
        TemplateHandler.Assign(tpl, "auto-super-text", Resources.Language.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Language.lblSuperPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblML)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Language.CopyFromHdp)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "4")

    End Sub
#End Region

End Class
