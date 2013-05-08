Imports System.Resources
Imports Sunplus.Accounting.Core.Extensions
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Common
Imports DNAide.Web
Imports System.Text
Imports DNAide.Core
Imports System.Data
Imports IBCUtils.Encryption
Imports IBCUtils.DAL
Imports IBCDLL.Customer
Imports IBCUtils.Web
Imports DNAide.Utils
Imports DNAide.Data
Imports BLL.Customer

Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Business.Customer
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.DataAccess.IBCDLL.Customer
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Web


Partial Class Customer_V2_EditAgent
    Inherits RequiredAuthorizationPage
    Public style As String
    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidNumberGame, strHidOther, strHidFinancial, strHidMixedSports As String
    Dim groupBtn As String
    Protected masterId, fromSet, msg As String
    Dim selectTpl, selDisabled As String
    Dim isEnabledAutoMaster As Boolean = True

    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.MasterId)
        End Get
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MenuPT.htm")
            groupBtn = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BtnGroup.htm")
            selectTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/dropdownPT.htm")
            tennisTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/TennisPT.htm")
            basketballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BasketballPT.htm")
            footballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/FootballPT.htm")
            otherSportsTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/OtherSportsPT.htm")
            golfTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/GolfPT.htm")
            selDisabled = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/ddlDisablePT.htm")

            saveCustid.Value = Request.QueryString("custId")
            htmlEditAgentContent.Text = LoadDataSingle()
            'If Not Request("msg") Is Nothing Then
            '    msg = Request("msg")
            '    If (msg.ToLower.IndexOf("successfully") >= 0) Then
            '        msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success)
            '    End If
            'Else
            '    msg = String.Empty
            'End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub

#Region "Declare parameter for Script"
    Sub VariableOfScript()
        Dim script As String = String.Empty
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
        Dim lblCreditInvalid As String, lblMemberMaxCreditInvalid As String = String.Empty

        lblCreditInvalid = Resources.Language.lblCreditInvalid.Replace("{0}", Resources.Language.lblAgentMaxCredit)
        lblMemberMaxCreditInvalid = Resources.Language.lblCreditInvalid.Replace("{0}", Resources.Language.lblMemberMaxCredit)

        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        script += " var alertMemberMaxCreditInvalid='" & lblMemberMaxCreditInvalid & "';" & vbNewLine

        Dim strAlertMinPT As String = Resources.Language.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Language.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Language.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "</script>"
        Response.Write(script)
    End Sub
#End Region

#Region "Load data for single edit"
    Function LoadDataSingle() As String
        Dim html As New StringBuilder
        Try
            Dim rs As DbResult
            rs = Select4Edit.SelAgentForEdit(Request("custId"), Request("masterId"))
            fromSet = IIf(Request("set") Is Nothing, 0, Request("set"))
            masterId = Request("masterId")
            If Not rs Is Nothing Then
                If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                    Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                    isAllowUpd.Value = "1" 'always allow update in account site 
                    'Hanh commented
                    'If CastAide.AsBoolean(row("AllowUpdatePositiontaking")) Then
                    '    isAllowUpd.Value = "1"
                    'Else
                    '    isAllowUpd.Value = "0"
                    '    disabledBtn = "<script type=""text/javascript"">obj(""btnSubmit"").style.color='#cccccc';</script>"
                    'End If


                    ' Casino
                    If (WebSettings.disableCS) Then
                        TemplateHandler.RemoveBlock(bodyTpl, "casino")
                    Else
                        TemplateHandler.Assign(bodyTpl, "{hidSynCS}", row("synccasino").ToString())
                    End If

                    CustomerUtils.GetSportTitle(bodyTpl, False)
                    TemplateHandler.Assign(bodyTpl, "accinfo", GetDataInfo(row))
                    TemplateHandler.Assign(bodyTpl, "add-text", Resources.Language.Submit)
                    TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")
                    TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row))
                    TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl))
                    TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl))
                    TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl))
                    TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8))
                    TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl))
                    TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11))

                    Dim numbergame As SportBooksBase = New NumberGameManager()
                    numbergame.GetNumberCom(row, 2, False, bodyTpl)


                    If Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled Then
                        TemplateHandler.Assign(bodyTpl, "NumberGamePT", GetDataNumberGame(row))
                    Else
                        TemplateHandler.RemoveBlock(bodyTpl, "NumberGame")
                    End If

                    TemplateHandler.Assign(bodyTpl, "MixedSportsPT", GetDataMixedSports(row))
                    TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl))

                    TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")
                    If Not Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                        TemplateHandler.Assign(bodyTpl, "hdauto", "hidden")
                    Else
                        TemplateHandler.Assign(bodyTpl, "hdauto", String.Empty)
                    End If
                    bodyTpl &= strHidSoccer
                    bodyTpl &= strHidBasket
                    bodyTpl &= strHidFootball
                    bodyTpl &= strHidTennis
                    bodyTpl &= strHidBaseball
                    bodyTpl &= strHidGolf
                    bodyTpl &= strHidMoto
                    bodyTpl &= strHidNumberGame
                    bodyTpl &= strHidMixedSports
                    bodyTpl &= strHidOther

                    html.Append(bodyTpl)
                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        LoadDataSingle = html.ToString()
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
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
            TemplateHandler.RemoveBlock(strAccInfo, "suspend-member")
            TemplateHandler.Assign(strAccInfo, "tranfer-time", "")

            If isMult Then
                TemplateHandler.RemoveBlock(strAccInfo, "none-mult")
                TemplateHandler.Assign(strAccInfo, "cols-untext", "5")
                TemplateHandler.Assign(strAccInfo, "title", Resources.Language.EditAgent)
                TemplateHandler.Assign(strAccInfo, "groupa-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupa")), CastAide.AsDouble(dr("groupa"))))
                TemplateHandler.Assign(strAccInfo, "groupb-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupb")), CastAide.AsDouble(dr("groupb"))))
                TemplateHandler.Assign(strAccInfo, "groupc-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupc")), CastAide.AsDouble(dr("groupc"))))
                TemplateHandler.Assign(strAccInfo, "groupd-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupd")), CastAide.AsDouble(dr("groupd"))))

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

                TemplateHandler.Assign(strAccInfo, "discountcs-val", CustomerUtils.GetListOtherComm(CastAide.AsDouble(dr("discountcs")), CastAide.AsDouble(dr("discountcs"))))

                TemplateHandler.Assign(strAccInfo, "sel-a", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-b", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-c", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-d", "disabled")
                TemplateHandler.Assign(strAccInfo, "sel-dis", "disabled")
                TemplateHandler.Assign(strAccInfo, "{credit-col}", "2")
                TemplateHandler.Assign(strAccInfo, "check-val", "disabled")
            Else
                TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
                TemplateHandler.RemoveBlock(strAccInfo, "none-group")
                TemplateHandler.RemoveBlock(strAccInfo, "none-group")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-super-master")
                TemplateHandler.RemoveBlock(strAccInfo, "suspend-member")

                TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
                TemplateHandler.Assign(strAccInfo, "title", "Agent: " + CastAide.AsString(dr("username")))
                TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Language.lblAgentMaxCredit)
                TemplateHandler.Assign(strAccInfo, "uname-parent", Session("UserName"))

                TemplateHandler.Assign(strAccInfo, "fname-val", RemoveSpecialChars(CastAide.AsString(dr("firstname"))))
                TemplateHandler.Assign(strAccInfo, "lname-val", RemoveSpecialChars(CastAide.AsString(dr("lastname"))))
                TemplateHandler.Assign(strAccInfo, "phone-val", RemoveSpecialChars(CastAide.AsString(dr("phone"))))
                TemplateHandler.Assign(strAccInfo, "mobile-val", RemoveSpecialChars(CastAide.AsString(dr("mobilephone"))))
                TemplateHandler.Assign(strAccInfo, "fax-val", RemoveSpecialChars(CastAide.AsString(dr("fax"))))
                'display value, limits of credit
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

                'If CastAide.AsBoolean(dr("disablehorseracing")) Then
                '    TemplateHandler.Assign(strAccInfo, "disableHR-check", "checked='checked' disabled")
                'Else
                '    TemplateHandler.Assign(strAccInfo, "disableHR-check", "")
                'End If
                'TemplateHandler.Assign(strAccInfo, "{hddisableHR-value}", IIf(CastAide.AsBoolean(dr("disablehorseracing")), 1, 0))

                isEnabledAutoMaster = Not CastAide.AsBoolean(dr("disableautompt"))

                TemplateHandler.Assign(strAccInfo, "member-max-credit", "Member Max Credit")
                TemplateHandler.Assign(strAccInfo, "mem-max-credit-val", NumberFormat.DecFormat(CastAide.AsDecimal(dr("Mem_MaxCredit")), 0))
                TemplateHandler.Assign(strAccInfo, "member-credit-min", NumberFormat.DecFormat(CastAide.AsDecimal(dr("MaxCredit_Min")), 0))
                TemplateHandler.Assign(strAccInfo, "member-credit-max", "")
                TemplateHandler.Assign(strAccInfo, "mMinCredit", "minmaxbet")
                TemplateHandler.Assign(strAccInfo, "mMaxCredit", "minmaxbet")

                TemplateHandler.Assign(strAccInfo, "groupa-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupa_max")), CastAide.AsDouble(dr("groupa"))))
                TemplateHandler.Assign(strAccInfo, "groupb-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupb_max")), CastAide.AsDouble(dr("groupb"))))
                TemplateHandler.Assign(strAccInfo, "groupc-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupc_max")), CastAide.AsDouble(dr("groupc"))))
                TemplateHandler.Assign(strAccInfo, "groupd-val", CustomerUtils.GetListGroup(CastAide.AsDouble(dr("groupd_max")), CastAide.AsDouble(dr("groupd"))))
                TemplateHandler.Assign(strAccInfo, "discountcs-val", CustomerUtils.GetListOtherComm(CastAide.AsDouble(dr("discountcs_max")), CastAide.AsDouble(dr("discountcs"))))

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
            '            Customer.LoadChkUpdate(soccerTpl, 1, isMult)
            GetTitleCommon(soccerTpl)
            TemplateHandler.Assign(soccerTpl, "col-title", "12")
            TemplateHandler.Assign(soccerTpl, "bettype-header", CustomerUtils.LoadSoccerPTHeader(False))
            TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Language.Deadball)
            TemplateHandler.Assign(soccerTpl, "live-text", Resources.Language.Live)
            TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Language.lblAgentPosition)
            TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Language.lblMasterPresetPosition)
            If Not Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                TemplateHandler.Assign(soccerTpl, "dead-row", "3")
                TemplateHandler.Assign(soccerTpl, "live-row", "2")
                TemplateHandler.Assign(soccerTpl, "copy-live-col", "2")
                TemplateHandler.Assign(soccerTpl, "hdauto", "hidden")
                TemplateHandler.ReplaceBlock(soccerTpl, "auto-preset", "")
            Else
                TemplateHandler.Assign(soccerTpl, "dead-row", "5")
                TemplateHandler.Assign(soccerTpl, "live-row", "4")
                TemplateHandler.Assign(soccerTpl, "copy-live-col", "4")
                TemplateHandler.Assign(soccerTpl, "hdauto", String.Empty)
            End If
            TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Language.CopyFrom1x2)
            TemplateHandler.Assign(soccerTpl, "chkAuto-1", "chkAuto1HdpD")
            TemplateHandler.Assign(soccerTpl, "chkAuto-2", "chkAuto11x2")
            TemplateHandler.Assign(soccerTpl, "chkAuto-3", "chkAuto1HdpL")
            'assign value of Minbet, Maxbet , ...
            If isMult Then
                CustomerUtils.GetMinMaxAddValue(soccerTpl, 1, "0"" disabled='true'")
                TemplateHandler.Assign(soccerTpl, "chk-hdpd", "disabled")
                TemplateHandler.Assign(soccerTpl, "chk-1x2", "disabled")
                TemplateHandler.Assign(soccerTpl, "chk-hdpl", "disabled")
            Else
                TemplateHandler.Assign(soccerTpl, "minbet1-val", DataFormat.FormatNumber1(dr("minbet")))
                TemplateHandler.Assign(soccerTpl, "maxbet1-val", DataFormat.FormatNumber1(dr("maxbet")))
                TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", DataFormat.FormatNumber1(dr("maxpermatch")))
                TemplateHandler.Assign(soccerTpl, "chk-hdpd", "")
                TemplateHandler.Assign(soccerTpl, "chk-1x2", "")
                TemplateHandler.Assign(soccerTpl, "chk-hdpl", "")
            End If
            CustomerUtils.GetLimitMinMaxBet(dr, soccerTpl, "")

            'assign value position taking for template
            Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
            Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

            Dim maHdpMin, maOUMin, ma1stHdpMin, ma1stOUMin, maOEMin, ma1x2Min, maCSMin, maTGMin, maMPMin, maORMin As Double
            Dim maLHdpMin, maLOUMin, maL1stHdpMin, maL1stOUMin As Double

            Dim maHdpMax, maOUMax, ma1stHdpMax, ma1stOUMax, maOEMax, ma1x2Max, maCSMax, maTGMax, maMPMax, maORMax As Double
            Dim maLHdpMax, maLOUMax, maL1stHdpMax, maL1stOUMax As Double

            Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
            Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

            Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
            Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

            Dim aHdpMax2, aOUMax2, a1stHdpMax2, a1stOUMax2, aOEMax2, a1x2Max2, aCSMax2, aTGMax2, aMPMax2, aORMax2 As Double
            Dim aLHdpMax2, aLOUMax2, aL1stHdpMax2, aL1stOUMax2 As Double

            Dim aHdpMax, aOUMax, a1stHdpMax, a1stOUMax, aOEMax, a1x2Max, aCSMax, aTGMax, aMPMax, aORMax As Double
            Dim aLHdpMax, aLOUMax, aL1stHdpMax, aL1stOUMax As Double

            Dim aHdpMin, aOUMin, a1stHdpMin, a1stOUMin, aOEMin, a1x2Min, aCSMin, aTGMin, aMPMin, aORMin As Double
            Dim aLHdpMin, aLOUMin, aL1stHdpMin, aL1stOUMin As Double

            Dim aHdp, aOU, a1stHdp, a1stOU, aOE, a1x2, aCS, aTG, aMP, aOR As Double
            Dim aLHdp, aLOU, aL1stHdp, aL1stOU As Double

            '------------------------Auto PT ------------------------- Tho Add -------------------------------------
            Dim isAutoHdp, isAutoOU, isAuto1stHdp, isAuto1stOU, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Double
            Dim isAutoLHdp, isAutoLOU, isAutoL1stHdp, isAutoL1stOU As Double

            Dim mAutoHdp, mAutoOU, mAuto1stHdp, mAuto1stOU, mAutoOE, mAuto1x2, mAutoCS, mAutoTG, mAutoMP, mAutoOR As Double
            Dim mAutoLHdp, mAutoLOU, mAutoL1stHdp, mAutoL1stOU As Double

            Dim mAutoHdpMax, mAutoOUMax, mAuto1stHdpMax, mAuto1stOUMax, mAutoOEMax, mAuto1x2Max, mAutoCSMax, mAutoTGMax, mAutoMPMax, mAutoORMax As Double
            Dim mAutoLHdpMax, mAutoLOUMax, mAutoL1stHdpMax, mAutoL1stOUMax As Double
            '-------------------------------------------------------------------------------------------------------
            aHdpMax2 = CastAide.AsDouble(dr("apositiontaking_1_max")) 'min(a4,l2)
            aOUMax2 = CastAide.AsDouble(dr("apositiontaking_3_max"))
            a1stHdpMax2 = CastAide.AsDouble(dr("apositiontaking_7_max"))
            a1stOUMax2 = CastAide.AsDouble(dr("apositiontaking_8_max"))
            aOEMax2 = CastAide.AsDouble(dr("apositiontaking_2_max"))
            a1x2Max2 = CastAide.AsDouble(dr("apositiontaking_5_max"))
            aCSMax2 = CastAide.AsDouble(dr("apositiontaking_4_max"))
            aTGMax2 = CastAide.AsDouble(dr("apositiontaking_6_max"))
            aMPMax2 = CastAide.AsDouble(dr("apositiontaking_9_max"))
            aORMax2 = CastAide.AsDouble(dr("apositiontaking_10_max"))
            aLHdpMax2 = CastAide.AsDouble(dr("alivepositiontaking_1_max"))
            aLOUMax2 = CastAide.AsDouble(dr("alivepositiontaking_3_max"))
            aL1stHdpMax2 = CastAide.AsDouble(dr("alivepositiontaking_7_max"))
            aL1stOUMax2 = CastAide.AsDouble(dr("alivepositiontaking_8_max"))
            If isMult Then ' this case for Multiple Edit
                maHdpMax = CastAide.AsDouble(dr("mpositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("mpositiontaking_3"))
                ma1stHdpMax = CastAide.AsDouble(dr("mpositiontaking_7"))
                ma1stOUMax = CastAide.AsDouble(dr("mpositiontaking_8"))
                maOEMax = CastAide.AsDouble(dr("mpositiontaking_2"))
                ma1x2Max = CastAide.AsDouble(dr("mpositiontaking_5"))
                maCSMax = CastAide.AsDouble(dr("mpositiontaking_4"))
                maTGMax = CastAide.AsDouble(dr("mpositiontaking_6"))
                maMPMax = CastAide.AsDouble(dr("mpositiontaking_9"))
                maORMax = CastAide.AsDouble(dr("mpositiontaking_10"))
                maLHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_1"))
                maLOUMax = CastAide.AsDouble(dr("mlivepositiontaking_3"))
                maL1stHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_7"))
                maL1stOUMax = CastAide.AsDouble(dr("mlivepositiontaking_8"))
                mHdp = maHdpMax
                mOU = maOUMax
                m1stHdp = ma1stHdpMax
                m1stOU = ma1stOUMax
                mOE = maOEMax
                m1x2 = ma1x2Max
                mCS = maCSMax
                mTG = maTGMax
                mMP = maMPMax
                mOR = maORMax
                mLHdp = maLHdpMax
                mLOU = maLOUMax
                mL1stHdp = maL1stHdpMax
                mL1stOU = maL1stOUMax

                mHdpMax = maHdpMax
                mOUMax = maOUMax
                m1stHdpMax = ma1stHdpMax
                m1stOUMax = ma1stOUMax
                mOEMax = maOEMax
                m1x2Max = ma1x2Max
                mCSMax = maCSMax
                mTGMax = maTGMax
                mMPMax = maMPMax
                mORMax = maORMax
                mLHdpMax = maLHdpMax
                mLOUMax = maLOUMax
                mL1stHdpMax = maL1stHdpMax
                mL1stOUMax = maL1stOUMax

                maHdpMin = CastAide.AsDouble(dr("mpositiontaking_1_min"))
                maOUMin = CastAide.AsDouble(dr("mpositiontaking_3_min"))
                ma1stHdpMin = CastAide.AsDouble(dr("mpositiontaking_7_min"))
                ma1stOUMin = CastAide.AsDouble(dr("mpositiontaking_8_min"))
                maOEMin = CastAide.AsDouble(dr("mpositiontaking_2_min"))
                ma1x2Min = CastAide.AsDouble(dr("mpositiontaking_5_min"))
                maCSMin = CastAide.AsDouble(dr("mpositiontaking_4_min"))
                maTGMin = CastAide.AsDouble(dr("mpositiontaking_6_min"))
                maMPMin = CastAide.AsDouble(dr("mpositiontaking_9_min"))
                maORMin = CastAide.AsDouble(dr("mpositiontaking_10_min"))
                maLHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_1_min"))
                maLOUMin = CastAide.AsDouble(dr("mlivepositiontaking_3_min"))
                maL1stHdpMin = CastAide.AsDouble(dr("mlivepositiontaking_7_min"))
                maL1stOUMin = CastAide.AsDouble(dr("mlivepositiontaking_8_min"))

                aHdp = aHdpMax
                aOU = aOUMax
                a1stHdp = a1stHdpMax
                a1stOU = a1stOUMax
                aOE = aOEMax
                a1x2 = a1x2Max
                aCS = aCSMax
                aTG = aTGMax
                aMP = aMPMax
                aOR = aORMax
                aLHdp = aLHdpMax
                aLOU = aLOUMax
                aL1stHdp = aL1stHdpMax
                aL1stOU = aL1stOUMax

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdpMax = maHdpMax 'max u2
                mAutoOUMax = maOUMax
                mAuto1stHdpMax = m1stHdpMax
                mAuto1stOUMax = m1stOUMax
                mAutoOEMax = mOEMax
                mAuto1x2Max = m1x2Max
                mAutoCSMax = mCSMax
                mAutoTGMax = mTGMax
                mAutoMPMax = mMPMax
                mAutoORMax = mORMax
                mAutoLHdpMax = mLHdpMax
                mAutoLOUMax = mLOUMax
                mAutoL1stHdpMax = mL1stHdpMax
                mAutoL1stOUMax = mL1stOUMax
                '/--------------------------------------------------------------------------------------------------------------
            Else ' this case for Single Edit
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

                aHdp = CastAide.AsDouble(dr("apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("apositiontaking_3"))
                a1stHdp = CastAide.AsDouble(dr("apositiontaking_7"))
                a1stOU = CastAide.AsDouble(dr("apositiontaking_8"))
                aOE = CastAide.AsDouble(dr("apositiontaking_2"))
                a1x2 = CastAide.AsDouble(dr("apositiontaking_5"))
                aCS = CastAide.AsDouble(dr("apositiontaking_4"))
                aTG = CastAide.AsDouble(dr("apositiontaking_6"))
                aMP = CastAide.AsDouble(dr("apositiontaking_9"))
                aOR = CastAide.AsDouble(dr("apositiontaking_10"))
                aLHdp = CastAide.AsDouble(dr("alivepositiontaking_1"))
                aLOU = CastAide.AsDouble(dr("alivepositiontaking_3"))
                aL1stHdp = CastAide.AsDouble(dr("alivepositiontaking_7"))
                aL1stOU = CastAide.AsDouble(dr("alivepositiontaking_8"))

                aHdpMin = CastAide.AsDouble(dr("apositiontaking_1_min"))
                aOUMin = CastAide.AsDouble(dr("apositiontaking_3_min"))
                a1stHdpMin = CastAide.AsDouble(dr("apositiontaking_7_min"))
                a1stOUMin = CastAide.AsDouble(dr("apositiontaking_8_min"))
                aOEMin = CastAide.AsDouble(dr("apositiontaking_2_min"))
                a1x2Min = CastAide.AsDouble(dr("apositiontaking_5_min"))
                aCSMin = CastAide.AsDouble(dr("apositiontaking_4_min"))
                aTGMin = CastAide.AsDouble(dr("apositiontaking_6_min"))
                aMPMin = CastAide.AsDouble(dr("apositiontaking_9_min"))
                aORMin = CastAide.AsDouble(dr("apositiontaking_10_min"))
                aLHdpMin = CastAide.AsDouble(dr("alivepositiontaking_1_min"))
                aLOUMin = CastAide.AsDouble(dr("alivepositiontaking_3_min"))
                aL1stHdpMin = CastAide.AsDouble(dr("alivepositiontaking_7_min"))
                aL1stOUMin = CastAide.AsDouble(dr("alivepositiontaking_8_min"))

                maHdpMax = CastAide.AsDouble(dr("max_mapositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("max_mapositiontaking_3"))
                ma1stHdpMax = CastAide.AsDouble(dr("max_mapositiontaking_7"))
                ma1stOUMax = CastAide.AsDouble(dr("max_mapositiontaking_8"))
                maOEMax = CastAide.AsDouble(dr("max_mapositiontaking_2"))
                ma1x2Max = CastAide.AsDouble(dr("max_mapositiontaking_5"))
                maCSMax = CastAide.AsDouble(dr("max_mapositiontaking_4"))
                maTGMax = CastAide.AsDouble(dr("max_mapositiontaking_6"))
                maMPMax = CastAide.AsDouble(dr("max_mapositiontaking_9"))
                maORMax = CastAide.AsDouble(dr("max_mapositiontaking_10"))
                maLHdpMax = CastAide.AsDouble(dr("max_malivepositiontaking_1"))
                maLOUMax = CastAide.AsDouble(dr("max_malivepositiontaking_3"))
                maL1stHdpMax = CastAide.AsDouble(dr("max_malivepositiontaking_7"))
                maL1stOUMax = CastAide.AsDouble(dr("max_malivepositiontaking_8"))

                maHdpMin = CastAide.AsDouble(dr("min_mapositiontaking_1"))
                maOUMin = CastAide.AsDouble(dr("min_mapositiontaking_3"))
                ma1stHdpMin = CastAide.AsDouble(dr("min_mapositiontaking_7"))
                ma1stOUMin = CastAide.AsDouble(dr("min_mapositiontaking_8"))
                maOEMin = CastAide.AsDouble(dr("min_mapositiontaking_2"))
                ma1x2Min = CastAide.AsDouble(dr("min_mapositiontaking_5"))
                maCSMin = CastAide.AsDouble(dr("min_mapositiontaking_4"))
                maTGMin = CastAide.AsDouble(dr("min_mapositiontaking_6"))
                maMPMin = CastAide.AsDouble(dr("min_mapositiontaking_9"))
                maORMin = CastAide.AsDouble(dr("min_mapositiontaking_10"))
                maLHdpMin = CastAide.AsDouble(dr("min_malivepositiontaking_1"))
                maLOUMin = CastAide.AsDouble(dr("min_malivepositiontaking_3"))
                maL1stHdpMin = CastAide.AsDouble(dr("min_malivepositiontaking_7"))
                maL1stOUMin = CastAide.AsDouble(dr("min_malivepositiontaking_8"))
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdp = CastAide.AsDouble(dr("auto_mpositiontaking_1")) 'u2
                mAutoOU = CastAide.AsDouble(dr("auto_mpositiontaking_3"))
                mAuto1stHdp = CastAide.AsDouble(dr("auto_mpositiontaking_7"))
                mAuto1stOU = CastAide.AsDouble(dr("auto_mpositiontaking_8"))
                mAutoOE = CastAide.AsDouble(dr("auto_mpositiontaking_2"))
                mAuto1x2 = CastAide.AsDouble(dr("auto_mpositiontaking_5"))
                mAutoCS = CastAide.AsDouble(dr("auto_mpositiontaking_4"))
                mAutoTG = CastAide.AsDouble(dr("auto_mpositiontaking_6"))
                mAutoMP = CastAide.AsDouble(dr("auto_mpositiontaking_9"))
                mAutoOR = CastAide.AsDouble(dr("auto_mpositiontaking_10"))
                mAutoLHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_1"))
                mAutoLOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_3"))
                mAutoL1stHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_7"))
                mAutoL1stOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_8"))
                '/--------------------------------------------------------------------------------------------------------------

                '------------------------Auto PT ------------------------- Tho Add ---------------------------------------------
                isAutoHdp = CastAide.AsInt16(dr("isauto_mpositiontaking_1")) ' i2
                isAutoOU = CastAide.AsInt16(dr("isauto_mpositiontaking_3"))
                isAuto1stHdp = CastAide.AsInt16(dr("isauto_mpositiontaking_7"))
                isAuto1stOU = CastAide.AsInt16(dr("isauto_mpositiontaking_8"))
                isAutoOE = CastAide.AsInt16(dr("isauto_mpositiontaking_2"))
                isAuto1x2 = CastAide.AsInt16(dr("isauto_mpositiontaking_5"))
                isAutoCS = CastAide.AsInt16(dr("isauto_mpositiontaking_4"))
                isAutoTG = CastAide.AsInt16(dr("isauto_mpositiontaking_6"))
                isAutoMP = CastAide.AsInt16(dr("isauto_mpositiontaking_9"))
                isAutoOR = CastAide.AsInt16(dr("isauto_mpositiontaking_10"))
                isAutoLHdp = CastAide.AsInt16(dr("isauto_mlivepositiontaking_1"))
                isAutoLOU = CastAide.AsInt16(dr("isauto_mlivepositiontaking_3"))
                isAutoL1stHdp = CastAide.AsInt16(dr("isauto_mlivepositiontaking_7"))
                isAutoL1stOU = CastAide.AsInt16(dr("isauto_mlivepositiontaking_8"))

                mAutoHdpMax = CastAide.AsDouble(dr("auto_mpositiontaking_1_max")) 'max u2
                mAutoOUMax = CastAide.AsDouble(dr("auto_mpositiontaking_3_max"))
                mAuto1stHdpMax = CastAide.AsDouble(dr("auto_mpositiontaking_7_max"))
                mAuto1stOUMax = CastAide.AsDouble(dr("auto_mpositiontaking_8_max"))
                mAutoOEMax = CastAide.AsDouble(dr("auto_mpositiontaking_2_max"))
                mAuto1x2Max = CastAide.AsDouble(dr("auto_mpositiontaking_5_max"))
                mAutoCSMax = CastAide.AsDouble(dr("auto_mpositiontaking_4_max"))
                mAutoTGMax = CastAide.AsDouble(dr("auto_mpositiontaking_6_max"))
                mAutoMPMax = CastAide.AsDouble(dr("auto_mpositiontaking_9_max"))
                mAutoORMax = CastAide.AsDouble(dr("auto_mpositiontaking_10_max"))
                mAutoLHdpMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_1_max"))
                mAutoLOUMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_3_max"))
                mAutoL1stHdpMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_7_max"))
                mAutoL1stOUMax = CastAide.AsDouble(dr("auto_mlivepositiontaking_8_max"))

                '-------------------------------------------------------------------------------------------------------
                mHdpMax = maHdpMax - aHdpMin
                mOUMax = maOUMax - aOUMin
                m1stHdpMax = ma1stHdpMax - a1stHdpMin
                m1stOUMax = ma1stOUMax - a1stOUMin
                mOEMax = maOEMax - aOEMin
                m1x2Max = ma1x2Max - a1x2Min
                mCSMax = maCSMax - aCSMin
                mTGMax = maTGMax - aTGMin
                mMPMax = maMPMax - aMPMin
                mORMax = maORMax - aORMin
                mLHdpMax = maLHdpMax - aLHdpMin
                mLOUMax = maLOUMax - aLOUMin
                mL1stHdpMax = maL1stHdpMax - aL1stHdpMin
                mL1stOUMax = maL1stOUMax - aL1stOUMin
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
            aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
            a1stHdpMax = IIf(ma1stHdpMax - m1stHdp > a1stHdpMax2, a1stHdpMax2, ma1stHdpMax - m1stHdp)
            a1stOUMax = IIf(ma1stOUMax - m1stOU > a1stOUMax2, a1stOUMax2, ma1stOUMax - m1stOU)
            aOEMax = IIf(maOEMax - mOE > aOEMax2, aOEMax2, maOEMax - mOE)
            a1x2Max = IIf(ma1x2Max - m1x2 > a1x2Max2, a1x2Max2, ma1x2Max - m1x2)
            aCSMax = IIf(maCSMax - mCS > aCSMax2, aCSMax2, maCSMax - mCS)
            aTGMax = IIf(maTGMax - mTG > aTGMax2, aTGMax2, maTGMax - mTG)
            aMPMax = IIf(maMPMax - mMP > aMPMax2, aMPMax2, maMPMax - mMP)
            aORMax = IIf(maORMax - mOR > aORMax2, aORMax2, maORMax - mOR)
            aLHdpMax = IIf(maLHdpMax - mLHdp > aLHdpMax2, aLHdpMax2, maLHdpMax - mLHdp)
            aLOUMax = IIf(maLOUMax - mLOU > aLOUMax2, aLOUMax2, maLOUMax - mLOU)
            aL1stHdpMax = IIf(maL1stHdpMax - mL1stHdp > aL1stHdpMax2, aL1stHdpMax2, maL1stHdpMax - mL1stHdp)
            aL1stOUMax = IIf(maL1stOUMax - mL1stOU > aL1stOUMax2, aL1stOUMax2, maL1stOUMax - mL1stOU)

            hiddenHash.Add("minM1HdpD", mHdpMin)
            hiddenHash.Add("minM1OUD", mOUMin)
            hiddenHash.Add("minM11stHdpD", m1stHdpMin)
            hiddenHash.Add("minM11stOUD", m1stOUMin)
            hiddenHash.Add("minM1OE", mOEMin)

            hiddenHash.Add("minM11x2", m1x2Min)
            hiddenHash.Add("minM1CS", mCSMin)
            hiddenHash.Add("minM1TG", mTGMin)
            hiddenHash.Add("minM1MP", mMPMin)
            hiddenHash.Add("minM1OR", mORMin)

            hiddenHash.Add("minM1HdpL", mLHdpMin)
            hiddenHash.Add("minM1OUL", mLOUMin)
            hiddenHash.Add("minM11stHdpL", mL1stHdpMin)
            hiddenHash.Add("minM11stOUL", mL1stOUMin)

            'hiddenHash.Add("maxA1HdpD", aHdpMax2)
            'hiddenHash.Add("maxA1OUD", aOUMax2)
            'hiddenHash.Add("maxA11stHdpD", a1stHdpMax2)
            'hiddenHash.Add("maxA11stOUD", a1stOUMax2)
            'hiddenHash.Add("maxA1OE", aOEMax2)

            'hiddenHash.Add("maxA11x2", a1x2Max2)
            'hiddenHash.Add("maxA1CS", aCSMax2)
            'hiddenHash.Add("maxA1TG", aTGMax2)
            'hiddenHash.Add("maxA1MP", aMPMax2)
            'hiddenHash.Add("maxA1OR", aORMax2)

            'hiddenHash.Add("maxA1HdpL", aLHdpMax2)
            'hiddenHash.Add("maxA1OUL", aLOUMax2)
            'hiddenHash.Add("maxA11stHdpL", aL1stHdpMax2)
            'hiddenHash.Add("maxA11stOUL", aL1stOUMax2)
            hiddenHash.Add("maxA1HdpD", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
            hiddenHash.Add("maxA1OUD", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
            hiddenHash.Add("maxA11stHdpD", IIf(a1stHdpMax2 > ma1stHdpMax, ma1stHdpMax, a1stHdpMax2))
            hiddenHash.Add("maxA11stOUD", IIf(a1stOUMax2 > ma1stOUMax, ma1stOUMax, a1stOUMax2))
            hiddenHash.Add("maxA1OE", IIf(aOEMax2 > maOEMax, maOEMax, aOEMax2))

            hiddenHash.Add("maxA11x2", IIf(a1x2Max2 > ma1x2Max, ma1x2Max, a1x2Max2))
            hiddenHash.Add("maxA1CS", IIf(aCSMax2 > maCSMax, maCSMax, aCSMax2))
            hiddenHash.Add("maxA1TG", IIf(aTGMax2 > maTGMax, maTGMax, aTGMax2))
            hiddenHash.Add("maxA1MP", IIf(aMPMax2 > maMPMax, maMPMax, aMPMax2))
            hiddenHash.Add("maxA1OR", IIf(aORMax2 > maORMax, maORMax, aORMax2))

            hiddenHash.Add("maxA1HdpL", IIf(aLHdpMax2 > maLHdpMax, maLHdpMax, aLHdpMax2))
            hiddenHash.Add("maxA1OUL", IIf(aLOUMax2 > maLOUMax, maLOUMax, aLOUMax2))
            hiddenHash.Add("maxA11stHdpL", IIf(aL1stHdpMax2 > maL1stHdpMax, maL1stHdpMax, aL1stHdpMax2))
            hiddenHash.Add("maxA11stOUL", IIf(aL1stOUMax2 > maL1stOUMax, maL1stOUMax, aL1stOUMax2))

            hiddenHash.Add("maxM1HdpD", maHdpMax)
            hiddenHash.Add("maxM1OUD", maOUMax)
            hiddenHash.Add("maxM11stHdpD", ma1stHdpMax)
            hiddenHash.Add("maxM11stOUD", ma1stOUMax)
            hiddenHash.Add("maxM1OE", maOEMax)

            hiddenHash.Add("maxM11x2", ma1x2Max)
            hiddenHash.Add("maxM1CS", maCSMax)
            hiddenHash.Add("maxM1TG", maTGMax)
            hiddenHash.Add("maxM1MP", maMPMax)
            hiddenHash.Add("maxM1OR", maORMax)

            hiddenHash.Add("maxM1HdpL", maLHdpMax)
            hiddenHash.Add("maxM1OUL", maLOUMax)
            hiddenHash.Add("maxM11stHdpL", maL1stHdpMax)
            hiddenHash.Add("maxM11stOUL", maL1stOUMax)

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
            hiddenHash.Add("isAutoMPT", Permissions.IsEnabledAutoMaster(isEnabledAutoMaster))

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

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)
            '/-------------------------------------------------------------------------------------------------------------

            Dim pt As String
            Dim ptMaster As String
            Dim strChecked As String
            Dim ptAuto As String
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabled
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------

            TemplateHandler.Assign(soccerTpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)
            '/-------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOU Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOU))
            TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OUD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)
            '/-------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAuto1stHdp Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(a1stHdpMin, a1stHdpMax, a1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stHdpD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-1stHdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAuto1stHdpMax, mAuto1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)
            '/-------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAuto1stOU Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(a1stOUMin, a1stOUMax, a1stOU))
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stOUD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-1stOU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAuto1stOUMax, mAuto1stOU))
            TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)
            '/-------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOE Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOEMin, aOEMax, aOE))
            TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-OE", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
            TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)
            '/-------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAuto1x2 Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(a1x2Min, a1x2Max, a1x2))
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-1x2", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAuto1x2Max, mAuto1x2))
            TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoCS Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aCSMin, aCSMax, aCS))
            TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1CS")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mCSMin, mCSMax, mCS))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-CS", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoCSMax, mAutoCS))
            TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoTG Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aTGMin, aTGMax, aTG))
            TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1TG")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mTGMin, mTGMax, mTG))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-TG", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoTGMax, mAutoTG))
            TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoMP Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMPMin, aMPMax, aMP))
            TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1MP")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-MP", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
            TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOR Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aORMin, aORMax, aOR))
            TemplateHandler.Assign(soccerTpl, "a-or-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OR")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mORMin, mORMax, mOR))
            TemplateHandler.Assign(soccerTpl, "p-or-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-OR", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoORMax, mAutoOR))
            TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            'get PT value for super preset PT
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoLHdp Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aLHdpMin, aLHdpMax, aLHdp))
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-LHdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoLHdpMax, mAutoLHdp))
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoLOU Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aLOUMin, aLOUMax, aLOU))
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS1OUL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-LOU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoLOUMax, mAutoLOU))
            TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoL1stHdp Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aL1stHdpMin, aL1stHdpMax, aL1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stHdpL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-L1stHdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoL1stHdpMax, mAutoL1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoL1stOU Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aL1stOUMin, aL1stOUMax, aL1stOU))
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS11stOUL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(soccerTpl, "checked-L1stOU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoL1stOUMax, mAutoL1stOU))
            TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataSoccer = soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball, Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball, 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
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
            Dim mHdpMax, mOUMax, mOEMax, mMPMax, mMLMax As Double
            Dim mHdp, mOU, mOE, mMP, mML As Double

            Dim aHdpMin, aOUMin, aOEMin, aMPMin, aMLMin As Double
            Dim aHdpMax, aOUMax, aOEMax, aMPMax, aMLMax As Double
            Dim aHdp, aOU, aOE, aMP, aML As Double
            Dim aHdpMax2, aOUMax2, aOEMax2, aMPMax2, aMLMax2 As Double
            Dim maHdpMin, maOUMin, maOEMin, maMPMin, maMLMin As Double
            Dim maHdpMax, maOUMax, maOEMax, maMPMax, maMLMax As Double
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            Dim mAutoHdp, mAutoOU, mAutoOE, mAutoMP, mAutoML As Double
            Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMPMax, mAutoMLMax As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Double
            '/--------------------------------------------------------------------------------------------------------------
            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
            aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'aMPMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_9_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))
            If isMult Then
                maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                maOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'maMPMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9"))
                maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
                mHdp = maHdpMax
                mOU = maOUMax
                mOE = maOEMax
                mMP = maMPMax
                mML = maMLMax

                mHdpMax = maHdpMax
                mOUMax = maOUMax
                mOEMax = maOEMax
                mMPMax = maMPMax
                mMLMax = maMLMax

                maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
                maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'maMPMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9_min"))
                maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

                aHdp = aHdpMax
                aOU = aOUMax
                aOE = aOEMax
                aML = aMLMax
                aMP = aMPMax

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdpMax = maHdpMax 'max u2
                mAutoOUMax = maOUMax
                mAutoOEMax = mOEMax
                mAutoMPMax = mMPMax
                mAutoMLMax = mMLMax
                '/--------------------------------------------------------------------------------------------------------------
            Else
                maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_3"))
                maOEMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'maMPMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_9"))
                maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'mMP = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'aMP = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_9"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
                aOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
                aOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_min"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'aMPMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_9_min"))
                aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

                maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
                maOUMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_3"))
                maOEMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'maMPMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_9"))
                maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))

                mHdpMax = maHdpMax - aHdpMin
                mOUMax = maOUMax - aOUMin
                mOEMax = maOEMax - aOEMin
                mMPMax = maMPMax - aMPMin
                mMLMax = maMLMax - aMLMin

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1")) ' u2
                mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3"))
                mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'mAutoMP = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_9"))
                mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
                '/--------------------------------------------------------------------------------------------------------------

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
                isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'isAutoMP = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_9"))
                isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

                mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max")) ' max u2
                mAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3_max"))
                mAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2_max"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'mAutoMPMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_9_max"))
                mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))
                '/--------------------------------------------------------------------------------------------------------------
            End If
            aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
            aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
            aOEMax = IIf(maOEMax - mOE > aOEMax2, aOEMax2, maOEMax - mOE)
            aMPMax = IIf(maMPMax - mMP > aMPMax2, aMPMax2, maMPMax - mMP)
            aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
            hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)
            hiddenHash.Add("minM" & sportTypeId & "MP", maMPMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
            hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
            hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(aOEMax2 > maOEMax, maOEMax, aOEMax2))
            hiddenHash.Add("maxA" & sportTypeId & "MP", IIf(aMPMax2 > maMPMax, maMPMax, aMPMax2))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "OU", maOUMax)
            hiddenHash.Add("maxM" & sportTypeId & "OE", maOEMax)
            hiddenHash.Add("maxM" & sportTypeId & "MP", maMPMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
            hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE)
            hiddenHash.Add("isAuto" & sportTypeId & "MP", isAutoMP)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)

            strHidBasket = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOU Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOU))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOE Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOEMin, aOEMax, aOE))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "p-oe-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-OE", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoMP Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "MP")
            'TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMPMin, aMPMax, aMP))
            'TemplateHandler.Assign(tpl, "a-mp-val", pt)
            'TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            'TemplateHandler.Assign(tpl, "p-mp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            'TemplateHandler.Assign(tpl, "checked-MP", strChecked)
            'TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
            'TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoML Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

            '/--------------------------------------------------------------------------------------------------------------
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '/--------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataBasketball = tpl
    End Function
#End Region

#Region "Assign info for Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 9:Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype ( 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
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
            Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
            Dim mHdp, mOU, mOE, mML As Double

            Dim aHdpMin, aOUMin, aOEMin, aMLMin As Double
            Dim aHdpMax, aOUMax, aOEMax, aMLMax As Double
            Dim aHdp, aOU, aOE, aML As Double
            Dim aHdpMax2, aOUMax2, aOEMax2, aMLMax2 As Double
            Dim maHdpMin, maOUMin, maOEMin, maMLMin As Double
            Dim maHdpMax, maOUMax, maOEMax, maMLMax As Double
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            Dim mAutoHdp, mAutoOU, mAutoOE, mAutoML As Double
            Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMLMax As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double
            '/--------------------------------------------------------------------------------------------------------------
            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
            aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))
            If isMult Then
                maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                maOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
                mHdp = maHdpMax
                mOU = maOUMax
                mOE = maOEMax
                mML = maMLMax

                mHdpMax = maHdpMax
                mOUMax = maOUMax
                mOEMax = maOEMax
                mMLMax = maMLMax

                maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
                maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
                maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

                aHdp = aHdpMax
                aOU = aOUMax
                aOE = aOEMax
                aML = aMLMax

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdpMax = maHdpMax 'max u2
                mAutoOUMax = maOUMax
                mAutoOEMax = mOEMax
                mAutoMLMax = maMLMax
                '/--------------------------------------------------------------------------------------------------------------
            Else
                maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_3"))
                maOEMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_2"))
                maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
                aOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
                aOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_min"))
                aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

                maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
                maOUMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_3"))
                maOEMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_2"))
                maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))

                mHdpMax = maHdpMax - aHdpMin
                mOUMax = maOUMax - aOUMin
                mOEMax = maOEMax - aOEMin
                mMLMax = maMLMax - aMLMin

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1"))
                mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3"))
                mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2"))
                mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
                '/--------------------------------------------------------------------------------------------------------------

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoOU = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
                isAutoOE = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
                isAutoML = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

                mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max"))
                mAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3_max"))
                mAutoOEMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_2_max"))
                mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))

                '/--------------------------------------------------------------------------------------------------------------
            End If
            aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
            aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
            aOEMax = IIf(maOEMax - mOE > aOEMax2, aOEMax2, maOEMax - mOE)
            aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
            hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
            hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
            hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(aOEMax2 > maOEMax, maOEMax, aOEMax2))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "OU", maOUMax)
            hiddenHash.Add("maxM" & sportTypeId & "OE", maOEMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)


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

            'assign data for Position taking dropdowns
            pt = selectTpl
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOU Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOU))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOE Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOEMin, aOEMax, aOE))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "p-oe-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-OE", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------

            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoML Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

            '/--------------------------------------------------------------------------------------------------------------
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            '/--------------------------------------------------------------------------------------------------------------
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
            Dim mHdpMax, mOUMax, mMLMax As Double
            Dim mHdp, mOU, mML As Double

            Dim aHdpMax, aOUMax, aMLMax As Double
            Dim aHdpMin, aOUMin, aMLMin As Double
            Dim aHdp, aOU, aML As Double
            Dim aHdpMax2, aOUMax2, aMLMax2 As Double
            Dim maHdpMin, maOUMin, maMLMin As Double
            Dim maHdpMax, maOUMax, maMLMax As Double
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            Dim mAutoHdp, mAutoOU, mAutoML As Double
            Dim mAutoHdpMax, mAutoOUMax, mAutoMLMax As Double
            Dim isAutoHdp, isAutoOU, isAutoML As Double
            '/--------------------------------------------------------------------------------------------------------------
            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))
            If isMult Then
                maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
                mHdp = maHdpMax
                mOU = maOUMax
                mML = maMLMax
                mHdpMax = maHdpMax
                mOUMax = maOUMax
                mMLMax = maMLMax

                maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
                maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

                aHdp = aHdpMax
                aOU = aOUMax
                aML = aMLMax
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdpMax = maHdpMax 'max u2
                mAutoOUMax = maOUMax
                mAutoMLMax = maMLMax
                '/--------------------------------------------------------------------------------------------------------------
            Else
                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
                aOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
                aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

                maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
                maOUMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_3"))
                maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

                maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
                maOUMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_3"))
                maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))

                mHdpMax = maHdpMax - aHdpMin
                mOUMax = maOUMax - aOUMin
                mMLMax = maMLMax - aMLMin

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1"))
                mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3"))
                mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
                '/--------------------------------------------------------------------------------------------------------------

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
                isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

                mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max"))
                mAutoOUMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_3_max"))
                mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))


                '/--------------------------------------------------------------------------------------------------------------
            End If
            aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
            aOUMax = IIf(maOUMax - mOU > aOUMax2, aOUMax2, maOUMax - mOU)
            aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
            hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(aOUMax2 > maOUMax, maOUMax, aOUMax2))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "OU", maOUMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)

            strHidBaseball = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoOU Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOU))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-OU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoML Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '/--------------------------------------------------------------------------------------------------------------
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
            'Customer.LoadChkUpdate(tpl, sportTypeId, isMult)
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "title-col", "4")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

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
            Dim mHdpMax, mMLMax As Double
            Dim mHdp, mML As Double

            Dim aHdpMax, aMLMax As Double
            Dim aHdpMin, aMLMin As Double
            Dim aHdp, aML As Double
            Dim aHdpMax2, aMLMax2 As Double
            Dim maHdpMin, maMLMin As Double
            Dim maHdpMax, maMLMax As Double
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            Dim mAutoHdp, mAutoML As Double
            Dim mAutoHdpMax, mAutoMLMax As Double
            Dim isAutoHdp, isAutoML As Double
            '/--------------------------------------------------------------------------------------------------------------
            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

            If isMult Then
                maHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                maMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
                mHdp = maHdpMax
                mML = maMLMax
                mHdpMax = maHdpMax
                mMLMax = maMLMax

                maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
                maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))
                aHdp = aHdpMax
                aML = aMLMax
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdpMax = maHdpMax 'max u2
                mAutoMLMax = maMLMax
                '/--------------------------------------------------------------------------------------------------------------
            Else
                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                aHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
                aMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

                maHdpMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_1"))
                maMLMax = CastAide.AsDouble(dr("max_v" & sportTypeId & "mapositiontaking_20"))

                maHdpMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_1"))
                maMLMin = CastAide.AsDouble(dr("min_v" & sportTypeId & "mapositiontaking_20"))

                mHdpMax = maHdpMax - aHdpMin
                mMLMax = maMLMax - aMLMin
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1"))
                mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20"))
                '/--------------------------------------------------------------------------------------------------------------

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoML = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))

                mAutoHdpMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_1_max"))
                mAutoMLMax = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_20_max"))

                '/--------------------------------------------------------------------------------------------------------------
            End If
            aHdpMax = IIf(maHdpMax - mHdp > aHdpMax2, aHdpMax2, maHdpMax - mHdp)
            aMLMax = IIf(maMLMax - mML > aMLMax2, aMLMax2, maMLMax - mML)

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(aHdpMax2 > maHdpMax, maHdpMax, aHdpMax2))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(aMLMax2 > maMLMax, maMLMax, aMLMax2))

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", maHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", maMLMax)

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML)

            strHidMoto = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoHdp Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            If isMult Then
                ptAuto = selDisabled
                pt = selDisabled
                ptMaster = selDisabled
                strChecked = "disabled"
            ElseIf isAutoML Then
                ptAuto = selectTpl
                ptMaster = selDisabled
                pt = selectTpl
                strChecked = "checked"
            Else
                ptAuto = selDisabled
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = ""
            End If
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            TemplateHandler.Assign(tpl, "checked-ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            '/--------------------------------------------------------------------------------------------------------------
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            '/--------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign info for NumberGame"
    Function GetDataNumberGame(ByVal dr As DataRow) As String
        Dim numbergame As SportBooksBase = New NumberGameManager()
        TemplateHandler.Assign(bodyTpl, "NumberGame-text", Resources.Language.lblNumberGame)
        Dim numberGameSetting = numbergame.GetAgentPtSetting_Edit(dr, isEnabledAutoMaster, strHidNumberGame)
        numbergame.GetMaxPerBall(dr, 2, False, numberGameSetting)
        Return numberGameSetting

    End Function
#End Region

#Region "Assign info for Mixed Sports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim mixedSports As SportBooksBase = New MixedSportsManager()
        TemplateHandler.Assign(bodyTpl, "MixedSports-text", Resources.Language.lblMixedSports)
        Return mixedSports.GetAgentPtSetting_Edit(dr, isEnabledAutoMaster, strHidMixedSports)
    End Function
#End Region


#Region "Get Title Common"
    Function GetTitleCommon(ByRef tpl As String)
        TemplateHandler.RemoveBlock(tpl, "none-edit")
        TemplateHandler.Assign(tpl, "bet-text", Resources.Language.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Language.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Language.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Language.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.lblAgentPosition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Language.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblML)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Language.CopyFromHdp)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "4")
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.lblAutoMasterResestPosition)
        TemplateHandler.Assign(tpl, "au-preset-live", Resources.Language.lblAutoMasterResestPosition)
    End Function
#End Region
End Class
