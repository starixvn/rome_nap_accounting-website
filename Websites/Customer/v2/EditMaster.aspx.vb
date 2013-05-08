Imports Sunplus.Accounting.Common

Imports IBCDLL
Imports IBCUtils.DAL
Imports System.Resources
Imports DNAide.Web
Imports System.Text
Imports DNAide.Core
Imports System.Data
Imports IBCUtils.Encryption
Imports IBCUtils.Web
Imports DNAide.Data
Imports BLL.Customer
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Business.Customer
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Web
Imports Sunplus.Accounting.Core.Extensions

Partial Class Customer_V2_EditMaster
    Inherits RequiredAuthorizationPage
    Public strUser As String
    Public strcustid, superId As String
    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl, financialTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidNumberGame, strHidOther, strHidFinancial, strHidMixedSports As String
    Dim groupBtn As String
    Dim selectTpl As String
    Dim selDisabledTpl As String
    Protected strErrMsg, fromSet, msg As String

    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.SuperId)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MenuPT.htm")
            groupBtn = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BtnGroup.htm")
            selectTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/dropdownPT.htm")
            selDisabledTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/ddlDisablePT.htm")
            tennisTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/TennisPT.htm")
            basketballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BasketballPT.htm")
            footballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/FootballPT.htm")
            otherSportsTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/OtherSportsPT.htm")
            golfTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/GolfPT.htm")
            'financialTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/FinancialPT.htm")
            Page.Title = Request("uname")
            htmlEditMasterContent.Text = GetDataSingle()
            If (Not Request("msg") Is Nothing) Then
                msg = Request("msg")
                msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success)
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub
#Region "Declare parameter for Script"
    'declare parameters is used in client script 
    'this function is call in code inline
    Sub VariableOfScript()
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
        script += " var alertUserName='" & Resources.Language.alertUserName & "';" & vbNewLine
        script += " var lblPasswordRequire='" & Resources.Language.lblPasswordRequire & "';" & vbNewLine
        script += " var alertPasswordEmpty='" & Resources.Language.alertPasswordEmpty & "';" & vbNewLine
        script += " var alertIncorrectCredit='" & Resources.Language.alertIncorrectCredit & "';" & vbNewLine
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

#Region "Get Information in Single Edit case"
    Function GetDataSingle()
        Dim strEditContent As New StringBuilder
        Try
            Dim rs As DbResult
            Dim hash As New Hashtable
            rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.Select4Edit.GetMasterInfo(Request("custId"), Request("superId"))
            fromSet = IIf(Request("set") Is Nothing, 0, Request("set"))
            strcustid = Request("custId")
            superId = Request("superId")
            If Not rs Is Nothing Then
                If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                    Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                    CustomerUtils.GetSportTitle(bodyTpl, False)
                    TemplateHandler.Assign(bodyTpl, "accinfo", GetDataInfo(row))
                    TemplateHandler.Assign(bodyTpl, "add-text", Resources.Language.Submit)
                    TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")

                    ' Casino
                    If (WebSettings.disableCS) Then
                        TemplateHandler.RemoveBlock(bodyTpl, "casino")
                    Else
                        TemplateHandler.Assign(bodyTpl, "{hidSynCS}", row("synccasino").ToString())
                    End If

                    TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row))
                    TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl))
                    TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl))
                    TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl))
                    TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8))
                    TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl))
                    TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11))

                    Dim numbergame As SportBooksBase = New NumberGameManager()
                    numbergame.GetNumberCom(row, 3, False, bodyTpl)

                    If Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled Then
                        TemplateHandler.Assign(bodyTpl, "NumberGamePT", GetDataNumbergame(row))
                    Else
                        TemplateHandler.RemoveBlock(bodyTpl, "NumberGame")
                    End If



                    TemplateHandler.Assign(bodyTpl, "MixedSportsPT", GetDataMixedSports(row))
                    TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl))

                    TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")

                    bodyTpl &= strHidSoccer
                    bodyTpl &= strHidBasket
                    bodyTpl &= strHidFootball
                    bodyTpl &= strHidTennis
                    bodyTpl &= strHidBaseball
                    bodyTpl &= strHidGolf
                    bodyTpl &= strHidMoto
                    bodyTpl &= strHidOther
                    bodyTpl &= strHidNumberGame
                    bodyTpl &= strHidMixedSports
                    'bodyTpl &= strHidFinancial
                    strEditContent.Append(bodyTpl)
                Else
                    msg = AlertMessage.GetMsg(Resources.Language.thereisnodata, AlertMessage.MsgType.Error)
                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataSingle = strEditContent.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim strAccInfo As String = String.Empty
        Try
            strAccInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/AccInfo.htm")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
            TemplateHandler.RemoveBlock(strAccInfo, "Member-Max-Credit")

            TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
            If isMult Then
                TemplateHandler.RemoveBlock(strAccInfo, "none-mult")
                TemplateHandler.Assign(strAccInfo, "cols-untext", "5")
                TemplateHandler.Assign(strAccInfo, "groupa-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupa")), CastAide.AsDouble(dr("groupa"))))
                TemplateHandler.Assign(strAccInfo, "groupb-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupb")), CastAide.AsDouble(dr("groupb"))))
                TemplateHandler.Assign(strAccInfo, "groupc-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupc")), CastAide.AsDouble(dr("groupc"))))
                TemplateHandler.Assign(strAccInfo, "groupd-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupd")), CastAide.AsDouble(dr("groupd"))))

                If WebSettings.Disable1x2Comm Then
                    TemplateHandler.RemoveBlock(strAccInfo, "none-1x2-Comm")
                Else
                    TemplateHandler.Assign(strAccInfo, "sel-1x2", "")
                    Try
                        TemplateHandler.Assign(strAccInfo, "discountcs1x2-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("discountcs1x2")), CastAide.AsDouble(dr("discountcs1x2"))))
                    Catch ex As Exception
                        LogExtensions.ExceptionHandler(ex)


                    End Try

                End If

                TemplateHandler.Assign(strAccInfo, "discountcs-val", CustomerUtils.GetListOtherComm(CastAide.AsDouble(dr("discountcs")), CastAide.AsDouble(dr("discountcs"))))
                TemplateHandler.Assign(strAccInfo, "sel-a", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-b", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-c", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-d", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-dis", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-31", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-32", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-fc", "disabled")
                TemplateHandler.Assign(strAccInfo, "{credit-col}", "2")
                TemplateHandler.Assign(strAccInfo, "check-val", "disabled")
            Else
                TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
                TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
                TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
                ' TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-super-master")
                TemplateHandler.RemoveBlock(strAccInfo, "suspend-member")

                TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
                TemplateHandler.Assign(strAccInfo, "title", "Master: " & CastAide.AsString(dr("username")))
                TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Language.lblMasterMaxCredit)

                TemplateHandler.Assign(strAccInfo, "fname-val", RemoveSpecialChars(CastAide.AsString(dr("firstname"))))
                TemplateHandler.Assign(strAccInfo, "lname-val", RemoveSpecialChars(CastAide.AsString(dr("lastname"))))
                TemplateHandler.Assign(strAccInfo, "phone-val", RemoveSpecialChars(CastAide.AsString(dr("phone"))))
                TemplateHandler.Assign(strAccInfo, "mobile-val", RemoveSpecialChars(CastAide.AsString(dr("mobilephone"))))
                TemplateHandler.Assign(strAccInfo, "fax-val", RemoveSpecialChars(CastAide.AsString(dr("fax"))))
                'display value,limit of credit
                CustomerUtils.GetCredit(dr, strAccInfo, True)
                Dim isSuspended As Boolean = CastAide.AsBoolean(dr("suspended"))
                TemplateHandler.Assign(strAccInfo, "chkSuspended", HtmlHelper.GenCheckBox("chkSuspended", False, isSuspended, "", ""))

                Dim isClosed As Boolean = CastAide.AsBoolean(dr("closed"))
                TemplateHandler.Assign(strAccInfo, "chkClosed", HtmlHelper.GenCheckBox("chkClosed", False, isClosed, "", ""))

                If WebSettings.Disable1x2Comm Then
                    TemplateHandler.RemoveBlock(strAccInfo, "none-1x2-Comm")
                Else
                    TemplateHandler.Assign(strAccInfo, "sel-1x2", "")
                    Try
                        TemplateHandler.Assign(strAccInfo, "discountcs1x2-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("discount1x2_max")), CastAide.AsDouble(dr("discount1x2"))))
                    Catch ex As Exception
                        LogExtensions.ExceptionHandler(ex)

                    End Try
                End If


                'If (Not Permissions.IsEnabledAutoMaster(True)) Then
                '    TemplateHandler.RemoveBlock(strAccInfo, "automasterpt")
                'Else
                '    TemplateHandler.Assign(strAccInfo, "{autompt}", IIf(CastAide.AsBoolean(dr("disableautompt")), "", "checked='true'"))
                'End If



                TemplateHandler.Assign(strAccInfo, "groupa-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupa_max")), CastAide.AsDouble(dr("groupa"))))
                TemplateHandler.Assign(strAccInfo, "groupb-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupb_max")), CastAide.AsDouble(dr("groupb"))))
                TemplateHandler.Assign(strAccInfo, "groupc-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupc_max")), CastAide.AsDouble(dr("groupc"))))
                TemplateHandler.Assign(strAccInfo, "groupd-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupd_max")), CastAide.AsDouble(dr("groupd"))))
                TemplateHandler.Assign(strAccInfo, "discountcs-val", CustomerUtils.GetListOtherComm(CastAide.AsDouble(dr("discountcs_max")), CastAide.AsDouble(dr("discountcs"))))

                TemplateHandler.Assign(strAccInfo, "sel-a", "")
                TemplateHandler.Assign(strAccInfo, "sel-b", "")
                TemplateHandler.Assign(strAccInfo, "sel-c", "")
                TemplateHandler.Assign(strAccInfo, "sel-d", "")
                TemplateHandler.Assign(strAccInfo, "sel-dis", "")
                TemplateHandler.Assign(strAccInfo, "sel-31", "")
                TemplateHandler.Assign(strAccInfo, "sel-32", "")
                TemplateHandler.Assign(strAccInfo, "sel-fc", "")
                TemplateHandler.Assign(strAccInfo, "{credit-col}", "2")

            End If

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

            CustomerUtils.GetTitleInfo(strAccInfo, isMult)
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataInfo = strAccInfo
    End Function
#End Region

#Region "Assign Info for Soccer"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataSoccer(ByVal dr As DataRow, Optional ByVal isMult As Boolean = False) As String
        Dim soccerTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable
        Try
            soccerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SoccerPT.htm")
            headerSTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SBettypeHeader.htm")
            ' assign all text according language
            TemplateHandler.RemoveBlock(soccerTpl, "none-edit")
            TemplateHandler.Assign(soccerTpl, "col-title", "12")
            TemplateHandler.Assign(soccerTpl, "bettype-header", CustomerUtils.LoadSoccerPTHeader(False))
            'Customer.LoadChkUpdate(soccerTpl, 1, isMult)
            GetTitleCommon(soccerTpl)
            TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Language.Deadball)
            TemplateHandler.Assign(soccerTpl, "au-preset-live", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.Assign(soccerTpl, "live-text", Resources.Language.Live)
            TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Language.lblMasterPresetPosition)
            TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Language.lblSuperPresetPosition)
            TemplateHandler.Assign(soccerTpl, "auto-super-text", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.Assign(soccerTpl, "dead-row", "5")
            TemplateHandler.Assign(soccerTpl, "live-row", "4")
            TemplateHandler.Assign(soccerTpl, "copy-live-col", "4")
            TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Language.CopyFrom1x2)

            TemplateHandler.Assign(soccerTpl, "chkAuto-1", "chkAuto1HdpD")
            TemplateHandler.Assign(soccerTpl, "chkAuto-2", "chkAuto11x2")
            TemplateHandler.Assign(soccerTpl, "chkAuto-3", "chkAuto1HdpL")
            'assign value of Minbet, Maxbet , ...
            If Not isMult Then
                TemplateHandler.Assign(soccerTpl, "minbet1-val", DataFormat.FormatNumber1(dr("minbet")))
                TemplateHandler.Assign(soccerTpl, "maxbet1-val", DataFormat.FormatNumber1(dr("maxbet")))
                TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", DataFormat.FormatNumber1(dr("maxpermatch")))
                TemplateHandler.Assign(soccerTpl, "chk-hdpd", "")
                TemplateHandler.Assign(soccerTpl, "chk-1x2", "")
                TemplateHandler.Assign(soccerTpl, "chk-hdpl", "")
            Else
                CustomerUtils.GetMinMaxAddValue(soccerTpl, 1, "0"" disabled='true'")
                TemplateHandler.Assign(soccerTpl, "chk-hdpd", "disabled")
                TemplateHandler.Assign(soccerTpl, "chk-1x2", "disabled")
                TemplateHandler.Assign(soccerTpl, "chk-hdpl", "disabled")
            End If
            CustomerUtils.GetLimitMinMaxBet(dr, soccerTpl, "")

            'assign value position taking for template
            Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
            Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

            Dim sHdpMin, sOUMin, s1stHdpMin, s1stOUMin, sOEMin, s1x2Min, sCSMin, sTGMin, sMPMin, sORMin As Double
            Dim sLHdpMin, sLOUMin, sL1stHdpMin, sL1stOUMin As Double

            Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
            Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

            Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
            Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

            Dim mHdpMax2, mOUMax2, m1stHdpMax2, m1stOUMax2, mOEMax2, m1x2Max2, mCSMax2, mTGMax2, mMPMax2, mORMax2 As Double
            Dim mLHdpMax2, mLOUMax2, mL1stHdpMax2, mL1stOUMax2 As Double

            Dim sHdp, sOU, s1stHdp, s1stOU, sOE, s1x2, sCS, sTG, sMP, sOR As Double
            Dim sLHdp, sLOU, sL1stHdp, sL1stOU As Double

            Dim sHdpMax, sOUMax, s1stHdpMax, s1stOUMax, sOEMax, s1x2Max, sCSMax, sTGMax, sMPMax, sORMax As Double
            Dim sLHdpMax, sLOUMax, sL1stHdpMax, sL1stOUMax As Double

            Dim msHdpMax, msOUMax, ms1stHdpMax, ms1stOUMax, msOEMax, ms1x2Max, msCSMax, msTGMax, msMPMax, msORMax As Double
            Dim msLHdpMax, msLOUMax, msL1stHdpMax, msL1stOUMax As Double

            Dim msHdpMin, msOUMin, ms1stHdpMin, ms1stOUMin, msOEMin, ms1x2Min, msCSMin, msTGMin, msMPMin, msORMin As Double
            Dim msLHdpMin, msLOUMin, msL1stHdpMin, msL1stOUMin As Double

            Dim sAutoHdp, sAutoOU, sAuto1stHdp, sAuto1stOU, sAutoOE, sAuto1x2, sAutoCS, sAutoTG, sAutoMP, sAutoOR As Double
            Dim sAutoLHdp, sAutoLOU, sAutoL1stHdp, sAutoL1stOU As Double

            Dim sAutoHdpMax, sAutoOUMax, sAuto1stHdpMax, sAuto1stOUMax, sAutoOEMax, sAuto1x2Max, sAutoCSMax, sAutoTGMax, sAutoMPMax, sAutoORMax As Double
            Dim sAutoLHdpMax, sAutoLOUMax, sAutoL1stHdpMax, sAutoL1stOUMax As Double

            Dim isAutoHdp, isAutoOU, isAuto1stHdp, isAuto1stOU, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Double
            Dim isAutoLHdp, isAutoLOU, isAutoL1stHdp, isAutoL1stOU As Double

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

            sHdp = CastAide.AsDouble(dr("spositiontaking_1"))
            sOU = CastAide.AsDouble(dr("spositiontaking_3"))
            s1stHdp = CastAide.AsDouble(dr("spositiontaking_7"))
            s1stOU = CastAide.AsDouble(dr("spositiontaking_8"))
            sOE = CastAide.AsDouble(dr("spositiontaking_2"))
            s1x2 = CastAide.AsDouble(dr("spositiontaking_5"))
            sCS = CastAide.AsDouble(dr("spositiontaking_4"))
            sTG = CastAide.AsDouble(dr("spositiontaking_6"))
            sMP = CastAide.AsDouble(dr("spositiontaking_9"))
            sOR = CastAide.AsDouble(dr("spositiontaking_10"))
            sLHdp = CastAide.AsDouble(dr("slivepositiontaking_1"))
            sLOU = CastAide.AsDouble(dr("slivepositiontaking_3"))
            sL1stHdp = CastAide.AsDouble(dr("slivepositiontaking_7"))
            sL1stOU = CastAide.AsDouble(dr("slivepositiontaking_8"))

            ''this is used to save into hidden fields to check in client script '''''''
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

            If Not isMult Then 'this case for Single Edit
                mHdpMin = CastAide.AsDouble(dr("mpositiontaking_1_min"))
                mOUMin = CastAide.AsDouble(dr("mpositiontaking_3_min"))
                m1stHdpMin = CastAide.AsDouble(dr("mpositiontaking_7_min"))
                m1stOUMin = CastAide.AsDouble(dr("mpositiontaking_8_min"))
                mOEMin = CastAide.AsDouble(dr("mpositiontaking_2_min"))
                m1x2Min = CastAide.AsDouble(dr("mpositiontaking_5_min"))
                mCSMin = CastAide.AsDouble(dr("mpositiontaking_4_min"))
                mTGMin = CastAide.AsDouble(dr("mpositiontaking_6_min"))
                mMPMin = CastAide.AsDouble(dr("mpositiontaking_9_min"))
                mORMin = CastAide.AsDouble(dr("mpositiontaking_10_min"))
                mLHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_1_min"))
                mLOUMin = CastAide.AsDouble(dr("mlivepositiontaking_3_min"))
                mL1stHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_7_min"))
                mL1stOUMin = CastAide.AsDouble(dr("mlivepositiontaking_8_min"))

                mHdp = CastAide.AsDouble(dr("mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("mpositiontaking_3"))
                m1stHdp = CastAide.AsDouble(dr("mpositiontaking_7"))
                m1stOU = CastAide.AsDouble(dr("mpositiontaking_8"))
                mOE = CastAide.AsDouble(dr("mpositiontaking_2"))
                m1x2 = CastAide.AsDouble(dr("mpositiontaking_5"))
                mCS = CastAide.AsDouble(dr("mpositiontaking_4"))
                mTG = CastAide.AsDouble(dr("mpositiontaking_6"))
                mMP = CastAide.AsDouble(dr("mpositiontaking_9"))
                mOR = CastAide.AsDouble(dr("mpositiontaking_10"))
                mLHdp = CastAide.AsDouble(dr("mlivepositiontaking_1"))
                mLOU = CastAide.AsDouble(dr("mlivepositiontaking_3"))
                mL1stHdp = CastAide.AsDouble(dr("mlivepositiontaking_7"))
                mL1stOU = CastAide.AsDouble(dr("mlivepositiontaking_8"))

                sHdpMax = CastAide.AsDouble(dr("spositiontaking_1_max"))
                sOUMax = CastAide.AsDouble(dr("spositiontaking_3_max"))
                s1stHdpMax = CastAide.AsDouble(dr("spositiontaking_7_max"))
                s1stOUMax = CastAide.AsDouble(dr("spositiontaking_8_max"))
                sOEMax = CastAide.AsDouble(dr("spositiontaking_2_max"))
                s1x2Max = CastAide.AsDouble(dr("spositiontaking_5_max"))
                sCSMax = CastAide.AsDouble(dr("spositiontaking_4_max"))
                sTGMax = CastAide.AsDouble(dr("spositiontaking_6_max"))
                sMPMax = CastAide.AsDouble(dr("spositiontaking_9_max"))
                sORMax = CastAide.AsDouble(dr("spositiontaking_10_max"))
                sLHdpMax = CastAide.AsDouble(dr("slivepositiontaking_1_max"))
                sLOUMax = CastAide.AsDouble(dr("slivepositiontaking_3_max"))
                sL1stHdpMax = CastAide.AsDouble(dr("slivepositiontaking_7_max"))
                sL1stOUMax = CastAide.AsDouble(dr("slivepositiontaking_8_max"))

                sAutoHdpMax = CastAide.AsDouble(dr("auto_spositiontaking_1_max"))
                sAutoOUMax = CastAide.AsDouble(dr("auto_spositiontaking_3_max"))
                sAuto1stHdpMax = CastAide.AsDouble(dr("auto_spositiontaking_7_max"))
                sAuto1stOUMax = CastAide.AsDouble(dr("auto_spositiontaking_8_max"))
                sAutoOEMax = CastAide.AsDouble(dr("auto_spositiontaking_2_max"))
                sAuto1x2Max = CastAide.AsDouble(dr("auto_spositiontaking_5_max"))
                sAutoCSMax = CastAide.AsDouble(dr("auto_spositiontaking_4_max"))
                sAutoTGMax = CastAide.AsDouble(dr("auto_spositiontaking_6_max"))
                sAutoMPMax = CastAide.AsDouble(dr("auto_spositiontaking_9_max"))
                sAutoORMax = CastAide.AsDouble(dr("auto_spositiontaking_10_max"))
                sAutoLHdpMax = CastAide.AsDouble(dr("auto_slivepositiontaking_1_max"))
                sAutoLOUMax = CastAide.AsDouble(dr("auto_slivepositiontaking_3_max"))
                sAutoL1stHdpMax = CastAide.AsDouble(dr("auto_slivepositiontaking_7_max"))
                sAutoL1stOUMax = CastAide.AsDouble(dr("auto_slivepositiontaking_8_max"))

                isAutoHdp = CastAide.AsInt16(dr("isauto_spositiontaking_1"))
                isAutoOU = CastAide.AsInt16(dr("isauto_spositiontaking_3"))
                isAuto1stHdp = CastAide.AsInt16(dr("isauto_spositiontaking_7"))
                isAuto1stOU = CastAide.AsInt16(dr("isauto_spositiontaking_8"))
                isAutoOE = CastAide.AsInt16(dr("isauto_spositiontaking_2"))
                isAuto1x2 = CastAide.AsInt16(dr("isauto_spositiontaking_5"))
                isAutoCS = CastAide.AsInt16(dr("isauto_spositiontaking_4"))
                isAutoTG = CastAide.AsInt16(dr("isauto_spositiontaking_6"))
                isAutoMP = CastAide.AsInt16(dr("isauto_spositiontaking_9"))
                isAutoOR = CastAide.AsInt16(dr("isauto_spositiontaking_10"))
                isAutoLHdp = CastAide.AsInt16(dr("isauto_slivepositiontaking_1"))
                isAutoLOU = CastAide.AsInt16(dr("isauto_slivepositiontaking_3"))
                isAutoL1stHdp = CastAide.AsInt16(dr("isauto_slivepositiontaking_7"))
                isAutoL1stOU = CastAide.AsInt16(dr("isauto_slivepositiontaking_8"))

                sHdpMax = sHdpMax - mHdpMin
                sOUMax = sOUMax - mOUMin
                s1stHdpMax = s1stHdpMax - m1stHdpMin
                s1stOUMax = s1stOUMax - m1stOUMin
                sOEMax = sOEMax - mOEMin
                s1x2Max = s1x2Max - m1x2Min
                sCSMax = sCSMax - mCSMin
                sTGMax = sTGMax - mTGMin
                sMPMax = sMPMax - mMPMin
                sORMax = sORMax - mORMin
                sLHdpMax = sLHdpMax - mLHdpMin
                sLOUMax = sLOUMax - mLOUMin
                sL1stHdpMax = sL1stHdpMax - mL1stHdpMin
                sL1stOUMax = sL1stOUMax - mL1stOUMin
            Else 'this case for Multiple Edit
                sHdpMax = sHdp
                sOUMax = sOU
                s1stHdpMax = s1stHdp
                s1stOUMax = s1stOU
                sOEMax = sOE
                s1x2Max = s1x2
                sCSMax = sCS
                sTGMax = sTG
                sMPMax = sMP
                sORMax = sOR
                sLHdpMax = sLHdp
                sLOUMax = sLOU
                sL1stHdpMax = sL1stHdp
                sL1stOUMax = sL1stOU

                sAutoHdpMax = sAutoHdp
                sAutoOUMax = sAutoOU
                sAuto1stHdpMax = sAuto1stHdp
                sAuto1stOUMax = sAuto1stOU
                sAutoOEMax = sAutoOE
                sAuto1x2Max = sAuto1x2
                sAutoCSMax = sAutoCS
                sAutoTGMax = sAutoTG
                sAutoMPMax = sAutoMP
                sAutoORMax = sAutoOR
                sAutoLHdpMax = sAutoLHdp
                sAutoLOUMax = sAutoLOU
                sAutoL1stHdpMax = sAutoL1stHdp
                sAutoL1stOUMax = sAutoL1stOU
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'this script to get max PT value of master that master can be preseted
            'max PT value = max ms - current PT value of super
            mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
            mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
            m1stHdpMax = IIf(isAuto1stHdp, ms1stHdpMax, ms1stHdpMax - s1stHdp)
            m1stOUMax = IIf(isAuto1stOU, ms1stOUMax, ms1stOUMax - s1stOU)
            mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
            m1x2Max = IIf(isAuto1x2, ms1x2Max, ms1x2Max - s1x2)
            mCSMax = IIf(isAutoCS, msCSMax, msCSMax - sCS)
            mTGMax = IIf(isAutoTG, msTGMax, msTGMax - sTG)
            mMPMax = IIf(isAutoMP, msMPMax, msMPMax - sMP)
            mORMax = IIf(isAutoOR, msORMax, msORMax - sOR)
            mLHdpMax = IIf(isAutoLHdp, msLHdpMax, msLHdpMax - sLHdp)
            mLOUMax = IIf(isAutoLOU, msLOUMax, msLOUMax - sLOU)
            mL1stHdpMax = IIf(isAutoL1stHdp, msL1stHdpMax, msL1stHdpMax - sL1stHdp)
            mL1stOUMax = IIf(isAutoL1stOU, msL1stOUMax, msL1stOUMax - sL1stOU)

            ' if max ms - current PT > max PT of master then max PT value = max PT of master
            mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
            mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
            m1stHdpMax = IIf(m1stHdpMax > m1stHdpMax2, m1stHdpMax2, m1stHdpMax)
            m1stOUMax = IIf(m1stOUMax > m1stOUMax2, m1stOUMax2, m1stOUMax)
            mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
            m1x2Max = IIf(m1x2Max > m1x2Max2, m1x2Max2, m1x2Max)
            mCSMax = IIf(mCSMax > mCSMax2, mCSMax2, mCSMax)
            mTGMax = IIf(mTGMax > mTGMax2, mTGMax2, mTGMax)
            mMPMax = IIf(mMPMax > mMPMax2, mMPMax2, mMPMax)
            mORMax = IIf(mORMax > mORMax2, mORMax2, mORMax)
            mLHdpMax = IIf(mLHdpMax > mLHdpMax2, mLHdpMax2, mLHdpMax)
            mLOUMax = IIf(mLOUMax > mLOUMax2, mLOUMax2, mLOUMax)
            mL1stHdpMax = IIf(mL1stHdpMax > mL1stHdpMax2, mL1stHdpMax2, mL1stHdpMax)
            mL1stOUMax = IIf(mL1stOUMax > mL1stOUMax2, mL1stOUMax2, mL1stOUMax)
            '''''''''''end script to get max PT value of master that master can be preseted            

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

            hiddenHash.Add("isAuto1HdpD", isAutoHdp)
            hiddenHash.Add("isAuto1OUD", isAutoOU)
            hiddenHash.Add("isAuto11stHdpD", isAuto1stHdp)
            hiddenHash.Add("isAuto11stOUD", isAuto1stOU)
            hiddenHash.Add("isAuto1OE", isAutoOE)

            hiddenHash.Add("isAuto11x2", isAuto1x2)
            hiddenHash.Add("isAuto1CS", isAutoCS)
            hiddenHash.Add("isAuto1TG", isAutoTG)
            hiddenHash.Add("isAuto1MP", isAutoMP)
            hiddenHash.Add("isAuto1OR", isAutoOR)

            hiddenHash.Add("isAuto1HdpL", isAutoLHdp)
            hiddenHash.Add("isAuto1OUL", isAutoLOU)
            hiddenHash.Add("isAuto11stHdpL", isAutoL1stHdp)
            hiddenHash.Add("isAuto11stOUL", isAutoL1stOU)

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

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)

            'this script is used to check or uncheck All Auto Checkbox
            'disable or enable Super Preset Position or Auto Super Preset Position
            Dim ptAuto As String
            Dim strChecked As String
            Dim ptMaster As String
            Dim pt As String = selectTpl
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OUD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(soccerTpl, "a-ou-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAuto1stHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = "checked"
                ptMaster = selectTpl
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-1stHdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stHdpD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1stHdpMin, s1stHdpMax, s1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stHdpMax, sAuto1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAuto1stOU Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-1stOU", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stOUD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOU))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stOUMax, sAuto1stOU))
            TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOE Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-OE", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(soccerTpl, "a-oe-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
            TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAuto1x2 Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-1x2", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1x2Min, s1x2Max, s1x2))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1x2Max, sAuto1x2))
            TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoCS Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-CS", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1CS")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mCSMin, mCSMax, mCS))
            TemplateHandler.Assign(soccerTpl, "a-cs-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sCSMin, sCSMax, sCS))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoCSMax, sAutoCS))
            TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoTG Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-TG", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1TG")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mTGMin, mTGMax, mTG))
            TemplateHandler.Assign(soccerTpl, "a-tg-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sTGMin, sTGMax, sTG))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoTGMax, sAutoTG))
            TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoMP Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-MP", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1MP")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            TemplateHandler.Assign(soccerTpl, "a-mp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMPMin, sMPMax, sMP))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
            TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOR Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-OR", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OR")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mORMin, mORMax, mOR))
            TemplateHandler.Assign(soccerTpl, "a-or-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sORMin, sORMax, sOR))
            TemplateHandler.Assign(soccerTpl, "p-or-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoORMax, sAutoOR))
            TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoLHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-LHdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sLHdpMin, sLHdpMax, sLHdp))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoLHdpMax, sAutoLHdp))
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoLOU Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-LOU", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OUL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sLOUMin, sLOUMax, sLOU))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoLOUMax, sAutoLOU))
            TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoL1stHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-L1stHdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stHdpL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sL1stHdpMin, sL1stHdpMax, sL1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoL1stHdpMax, sAutoL1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoL1stOU Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(soccerTpl, "checked-L1stOU", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stOUL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sL1stOUMin, sL1stOUMax, sL1stOU))
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoL1stOUMax, sAutoL1stOU))
            TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)

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
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'Customer.LoadChkUpdate(tpl, sportTypeId, isMult)
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "mp-text", Resources.Language.MixParlay)
            TemplateHandler.Assign(tpl, "title-col", "7")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            TemplateHandler.RemoveBlock(tpl, "no-mixparlay")

            'assign value for position taking
            If Not isMult Then
                CustomerUtils.GetMinMaxEditValue(tpl, sportTypeId, dr)
                TemplateHandler.Assign(tpl, "chk-hdp", "")
            Else
                CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMPMin, mMLMin As Double
            Dim mHdp, mOU, mOE, mMP, mML As Double
            Dim mHdpMax, mOUMax, mOEMax, mMPMax, mMLMax As Double
            Dim mHdpMax2, mOUMax2, mOEMax2, mMPMax2, mMLMax2 As Double

            Dim sHdpMin, sOUMin, sOEMin, sMPMin, sMLMin As Double
            Dim sHdp, sOU, sOE, sMP, sML As Double
            Dim sHdpMax, sOUMax, sOEMax, sMPMax, sMLMax As Double

            Dim msHdpMax, msOUMax, msOEMax, msMPMax, msMLMax As Double
            Dim msHdpMin, msOUMin, msOEMin, msMPMin, msMLMin As Double

            Dim sAutoHdp, sAutoOU, sAutoOE, sAutoMP, sAutoML As Double
            Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMPMax, sAutoMLMax As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
            mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'mMPMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'sMP = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_9"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

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

            If Not isMult Then
                mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
                mOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'mMPMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9_min"))
                mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'mMP = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
                sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3_max"))
                sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2_max"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'sMPMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_9_max"))
                sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

                sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
                sAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3_max"))
                sAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2_max"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'sAutoMPMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_9_max"))
                sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

                isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
                isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'isAutoMP = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_9"))
                isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

                sHdpMax = sHdpMax - mHdpMin
                sOUMax = sOUMax - mOUMin
                sOEMax = sOEMax - mOEMin
                sMPMax = sMPMax - mMPMin
                sMLMax = sMLMax - mMLMin
            Else
                sHdpMax = sHdp
                sOUMax = sOU
                sOEMax = sOE
                sMPMax = sMP
                sMLMax = sML

                sAutoHdpMax = sAutoHdp
                sAutoOUMax = sAutoOU
                sAutoOEMax = sAutoOE
                sAutoMPMax = sAutoMP
                sAutoMLMax = sAutoML
            End If

            'this script to get max PT value of master that can preset to
            mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
            mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
            mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
            mMPMax = IIf(isAutoMP, msMPMax, msMPMax - sMP)
            mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

            mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
            mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
            mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
            mMPMax = IIf(mMPMax > mMPMax2, mMPMax2, mMPMax)
            mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)
            ''''''''end script''''''''''''''            

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

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
            hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE)
            hiddenHash.Add("isAuto" & sportTypeId & "MP", isAutoMP)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)
            strHidBasket = CustomerUtils.AssignMultipleHidden(hiddenHash)

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            Dim ptAuto As String
            Dim strChecked As String
            Dim ptMaster As String
            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "a-ou-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOE Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-OE", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "a-oe-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoMP Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'TemplateHandler.Assign(tpl, "checked-MP", strChecked)
            'TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            'TemplateHandler.Assign(tpl, "a-mp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMPMin, sMPMax, sMP))
            TemplateHandler.Assign(tpl, "p-mp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "MP")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
            TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMLMax, sAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'Customer.LoadChkUpdate(tpl, sportTypeId, isMult)
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "title-col", "6")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

            'assign value for position taking
            If Not isMult Then
                CustomerUtils.GetMinMaxEditValue(tpl, sportTypeId, dr)
                TemplateHandler.Assign(tpl, "chk-hdp", "")
            Else
                CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
            Dim mHdp, mOU, mOE, mML As Double
            Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
            Dim mHdpMax2, mOUMax2, mOEMax2, mMLMax2 As Double

            Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double
            Dim sHdp, sOU, sOE, sML As Double
            Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

            Dim msHdpMax, msOUMax, msOEMax, msMLMax As Double
            Dim msHdpMin, msOUMin, msOEMin, msMLMin As Double

            Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double
            Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMLMax As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
            mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

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

            If Not isMult Then
                mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
                mOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
                mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
                sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3_max"))
                sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2_max"))
                sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

                sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
                sAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3_max"))
                sAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2_max"))
                sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

                isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
                isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
                isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

                sHdpMax = sHdpMax - mHdpMin
                sOUMax = sOUMax - mOUMin
                sOEMax = sOEMax - mOEMin
                sMLMax = sMLMax - mMLMin
            Else
                sHdpMax = sHdp
                sOUMax = sOU
                sOEMax = sOE
                sMLMax = sML

                sAutoHdpMax = sAutoHdp
                sAutoOUMax = sAutoOU
                sAutoOEMax = sAutoOE
                sAutoMLMax = sAutoML
            End If

            'this script to get max PT value of master that can preset to
            mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
            mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
            mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
            mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

            mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
            mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
            mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
            mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)
            ''''''''end script''''''''''''''            

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

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
            hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)

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

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            Dim ptAuto As String
            Dim strChecked As String
            Dim ptMaster As String
            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "a-ou-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOE Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-OE", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "a-oe-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMLMax, sAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

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
    Function GetDataBaseball(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BaseballPT.htm")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'Customer.LoadChkUpdate(tpl, sportTypeId, isMult)
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "title-col", "5")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

            'assign value for position taking
            If Not isMult Then
                CustomerUtils.GetMinMaxEditValue(tpl, sportTypeId, dr)
                TemplateHandler.Assign(tpl, "chk-hdp", "")
            Else
                CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mMLMin As Double
            Dim mHdp, mOU, mML As Double
            Dim mHdpMax, mOUMax, mMLMax As Double
            Dim mHdpMax2, mOUMax2, mMLMax2 As Double

            Dim sHdpMin, sOUMin, sMLMin As Double
            Dim sHdp, sOU, sML As Double
            Dim sHdpMax, sOUMax, sMLMax As Double

            Dim msHdpMax, msOUMax, msMLMax As Double
            Dim msHdpMin, msOUMin, msMLMin As Double

            Dim sAutoHdp, sAutoOU, sAutoML As Double
            Dim sAutoHdpMax, sAutoOUMax, sAutoMLMax As Double
            Dim isAutoHdp, isAutoOU, isAutoML As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
            msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
            msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

            msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
            msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
            msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            If Not isMult Then
                mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
                mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
                sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3_max"))
                sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

                sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
                sAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3_max"))
                sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

                isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
                isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))

                sHdpMax = sHdpMax - mHdpMin
                sOUMax = sOUMax - mOUMin
                sMLMax = sMLMax - mMLMin
            Else
                sHdpMax = sHdp
                sOUMax = sOU
                sMLMax = sML

                sAutoHdpMax = sAutoHdp
                sAutoOUMax = sAutoOU
                sAutoMLMax = sAutoML
            End If
            'this script to get max PT value of master that can be preseted to
            mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
            mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)
            mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

            mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
            mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)
            mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)
            '''''end script '''''''''''

            hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
            hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)
            hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

            hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
            hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)
            hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
            hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)
            strHidBaseball = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns          
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            Dim ptAuto As String
            Dim strChecked As String
            Dim ptMaster As String
            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "a-ou-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMLMax, sAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

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
    Function GetDataMotoSports(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MotorSportsPT.htm")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'Customer.LoadChkUpdate(tpl, sportTypeId, isMult)
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "title-col", "4")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)
            If Not isMult Then
                CustomerUtils.GetMinMaxEditValue(tpl, sportTypeId, dr)
                TemplateHandler.Assign(tpl, "chk-hdp", "")
            Else
                CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
                TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
            End If
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            'get data and assign for template Position taking
            Dim mHdpMin, mMLMin As Double
            Dim mHdp, mML As Double
            Dim mHdpMax, mMLMax As Double
            Dim mHdpMax2, mMLMax2 As Double

            Dim sHdpMin, sMLMin As Double
            Dim sHdp, sML As Double
            Dim sHdpMax, sMLMax As Double

            Dim msHdpMax, msMLMax As Double
            Dim msHdpMin, msMLMin As Double

            Dim sAutoHdp, sAutoML As Double
            Dim sAutoHdpMax, sAutoMLMax As Double
            Dim isAutoHdp, isAutoML As Double

            mHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_max"))
            mMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_max"))

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            msHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_max"))
            msMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_max"))

            msHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_1_min"))
            msMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_20_min"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            If Not isMult Then
                mHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                mMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1_max"))
                sMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20_max"))

                sAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1_max"))
                sAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20_max"))

                isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))
                sHdpMax = sHdpMax - mHdpMin
                sMLMax = sMLMax - mMLMin
            Else
                sHdpMax = sHdp
                sMLMax = sML

                sAutoHdpMax = sAutoHdp
                sAutoMLMax = sAutoML
            End If

            mHdpMax = IIf(isAutoHdp, msHdpMax, msHdpMax - sHdp)
            mMLMax = IIf(isAutoML, msMLMax, msMLMax - sML)

            mHdpMax = IIf(mHdpMax > mHdpMax2, mHdpMax2, mHdpMax)
            mMLMax = IIf(mMLMax > mMLMax2, mMLMax2, mMLMax)

            hiddenHash.Add("maxMS" & sportTypeId & "Hdp", msHdpMax)
            hiddenHash.Add("maxMS" & sportTypeId & "ML", msMLMax)

            hiddenHash.Add("minMS" & sportTypeId & "Hdp", msHdpMin)
            hiddenHash.Add("minMS" & sportTypeId & "ML", msMLMin)

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax2)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax2)

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)
            strHidMoto = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns     
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            Dim ptAuto As String
            Dim strChecked As String
            Dim ptMaster As String
            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            If isMult Then
                pt = selDisabledTpl
                ptAuto = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                ptMaster = selectTpl
                strChecked = "checked"
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "a-ml-val", ptMaster)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMLMin, sMLMax, sML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMLMax, sAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign info for NumberGame"
    Function GetDataNumbergame(ByVal dr As DataRow) As String
        Dim numbergame As SportBooksBase = New NumberGameManager()
        TemplateHandler.Assign(bodyTpl, "NumberGame-text", Resources.Language.lblNumberGame)
        Dim numberGameSetting As String
        numberGameSetting = numbergame.GetMasterPtSetting_Edit(dr, strHidNumberGame)
        numbergame.GetMaxPerBall(dr, 3, False, numberGameSetting)
        Return numberGameSetting
    End Function
#End Region

#Region "Assign info for Mixed Sports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim mixedSports As SportBooksBase = New MixedSportsManager()
        TemplateHandler.Assign(bodyTpl, "MixedSports-text", Resources.Language.lblMixedSports)
        Return mixedSports.GetMasterPtSetting_Edit(dr, strHidMixedSports)
    End Function
#End Region


#Region "Assign info for Financial"
    'this function is used to assign data into template for Baseball
    'dr: row contains data get from db
    'sportTypeId: id of sporttype
    'Financial have only Odd/Even, Fixed Odds & Floating Odds bettype
    Function GetDataFinancial(ByVal dr As DataRow, ByVal sportTypeId As Integer, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable
        Try


            Dim mOUMin As Double
            Dim mOU As Double
            Dim mOUMax As Double
            Dim mOUMax2 As Double

            Dim sOUMin As Double
            Dim sOU As Double
            Dim sOUMax As Double

            Dim msOUMax As Double
            Dim msOUMin As Double

            Dim sAutoOU As Double
            Dim sAutoOUMax As Double
            Dim isAutoOU As Double

            'Odd/Even
            Dim mOEMin As Double
            Dim mOE As Double
            Dim mOEMax As Double
            Dim mOEMax2 As Double

            Dim sOEMin As Double
            Dim sOE As Double
            Dim sOEMax As Double

            Dim msOEMax As Double
            Dim msOEMin As Double

            Dim sAutoOE As Double
            Dim sAutoOEMax As Double
            Dim isAutoOE As Double
            'END
            mOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_max"))
            sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            msOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_max"))
            msOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_3_min"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))

            'Odd/Even
            mOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_max"))
            sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
            msOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_max"))
            msOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mspositiontaking_2_min"))
            sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
            'END

            If Not isMult Then
                mOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))

                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))

                sOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3_max"))

                sAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3_max"))

                isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_3"))

                sOUMax = sOUMax - mOUMin

                'Odd/Even
                mOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
                mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                sOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2_max"))
                sAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2_max"))
                isAutoOE = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
                sOEMax = sOEMax - mOEMin
                'End
            Else
                sOUMax = sOU

                sAutoOUMax = sAutoOU

                'Odd/Even
                sOEMax = sOE
                sAutoOEMax = sAutoOE
                'End

            End If
            'this script to get max PT value of master that can be preseted to
            mOUMax = IIf(isAutoOU, msOUMax, msOUMax - sOU)

            mOUMax = IIf(mOUMax > mOUMax2, mOUMax2, mOUMax)

            'Odd/Even
            mOEMax = IIf(isAutoOE, msOEMax, msOEMax - sOE)
            mOEMax = IIf(mOEMax > mOEMax2, mOEMax2, mOEMax)
            'End
            '''''end script '''''''''''
            If WebSettings.disabledFI Then
                hiddenHash.Add("hiddenMinbet201", dr("minbetv" + sportTypeId.ToString()))
                hiddenHash.Add("hiddenMaxbet201", dr("maxbetv" + sportTypeId.ToString()))
                hiddenHash.Add("hiddenMaxpermatch201", dr("maxpermatchv" + sportTypeId.ToString()))

                hiddenHash.Add("hiddenSOUB201", sOU)
                hiddenHash.Add("hiddenMOUB201", mOU)
                hiddenHash.Add("hiddenAOUB201", sAutoOU)
                hiddenHash.Add("hiddenSOEB201", sOE)
                hiddenHash.Add("hiddenMOEB201", mOE)
                hiddenHash.Add("hiddenAOEB201", sAutoOE)
                tpl = CustomerUtils.AssignMultipleHidden(hiddenHash)
            Else
                'assign text for position taking
                tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/FinancialPT.htm")
                TemplateHandler.RemoveBlock(tpl, "none-edit")
                'Customer.LoadChkUpdate(tpl, sportTypeId, isMult)
                'GetTitleCommon(tpl)
                TemplateHandler.Assign(tpl, "bet-text", Resources.Language.lblBetSetting)
                TemplateHandler.Assign(tpl, "minbet-text", Resources.Language.lblMinBet)
                TemplateHandler.Assign(tpl, "maxbet-text", Resources.Language.lblMaxBet)
                TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Language.lblMaxPerMatch)

                TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
                TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.lblMasterPresetPosition)
                TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Language.lblSuperPresetPosition)
                TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.lblAutoSuperPresetPosition)
                TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
                TemplateHandler.Assign(tpl, "copy-hdp-col", "4")
                TemplateHandler.Assign(tpl, "title-col", "5")
                TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "OU")
                TemplateHandler.Assign(tpl, "150px", "150px")

                'Odd/Even label
                TemplateHandler.Assign(tpl, "copy-ou-span", "4")
                TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven) 'ODD/EVEN label
                TemplateHandler.Assign(tpl, "chkCopy-1", "chkCopy" & sportTypeId & "OU")
                TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "OE") 'OE bettype
                TemplateHandler.Assign(tpl, "lblCopyFromBettype", String.Format(Resources.Language.lblCopyFromBettype, Resources.Language.OverUnder)) 'Copy from label
                'END

                'assign value for position taking
                If Not isMult Then
                    CustomerUtils.GetMinMaxEditValue(tpl, sportTypeId, dr)
                    TemplateHandler.Assign(tpl, "chk-hdp", "")
                Else
                    CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, "0"" disabled='true'")
                    TemplateHandler.Assign(tpl, "chk-hdp", "disabled")
                End If
                CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())


                hiddenHash.Add("maxMS" & sportTypeId & "OU", msOUMax)

                hiddenHash.Add("minMS" & sportTypeId & "OU", msOUMin)

                hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax2)

                hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)

                'Assign min/max PT for Odd/Even
                hiddenHash.Add("maxMS" & sportTypeId & "OE", msOEMax)

                hiddenHash.Add("minMS" & sportTypeId & "OE", msOEMin)

                hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax2)

                hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE)
                'END
                strHidFinancial = CustomerUtils.AssignMultipleHidden(hiddenHash)

                Dim ptAuto As String
                Dim strChecked As String
                Dim ptMaster As String
                If isMult Then
                    pt = selDisabledTpl
                    ptAuto = selDisabledTpl
                    ptMaster = selDisabledTpl
                    strChecked = "disabled"
                ElseIf isAutoOU Then
                    pt = selDisabledTpl
                    ptAuto = selectTpl
                    ptMaster = selectTpl
                    strChecked = "checked"
                Else
                    pt = selectTpl
                    ptAuto = selDisabledTpl
                    ptMaster = selectTpl
                    strChecked = ""
                End If



                TemplateHandler.Assign(tpl, "checked-OU", strChecked)
                TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
                TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
                TemplateHandler.Assign(tpl, "a-ou-val", ptMaster)

                TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
                TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
                TemplateHandler.Assign(tpl, "p-ou-val", pt)
                TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
                TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
                TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

                Dim btnTpl As String = groupBtn
                btnTpl = groupBtn
                TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "OU")
                TemplateHandler.Assign(tpl, "au-ou-btn", btnTpl)

                btnTpl = groupBtn
                TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "OU")
                TemplateHandler.Assign(tpl, "a-ou-btn", btnTpl)

                btnTpl = groupBtn
                TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "OU")
                TemplateHandler.Assign(tpl, "p-ou-btn", btnTpl)

                'Assign Value for PT Odd/Even
                If isMult Then
                    pt = selDisabledTpl
                    ptAuto = selDisabledTpl
                    ptMaster = selDisabledTpl
                    strChecked = "disabled"
                ElseIf isAutoOE Then 'need change to OE
                    pt = selDisabledTpl
                    ptAuto = selectTpl
                    ptMaster = selectTpl
                    strChecked = "checked"
                Else
                    pt = selectTpl
                    ptAuto = selDisabledTpl
                    ptMaster = selectTpl
                    strChecked = ""
                End If
                TemplateHandler.Assign(tpl, "checked-OE", strChecked)
                TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OE")
                TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
                TemplateHandler.Assign(tpl, "a-oe-val", ptMaster)

                TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
                TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOE))
                TemplateHandler.Assign(tpl, "p-oe-val", pt)
                TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
                TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
                TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
                'END
            End If

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataFinancial = tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        TemplateHandler.Assign(tpl, "bet-text", Resources.Language.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Language.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Language.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Language.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
        TemplateHandler.Assign(tpl, "auto-super-text", Resources.Language.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Language.lblSuperPresetPosition)
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblML)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Language.CopyFromHdp)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "4")
    End Sub
#End Region
End Class

