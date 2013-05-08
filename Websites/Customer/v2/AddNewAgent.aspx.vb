Imports System.Resources
Imports Sunplus.Accounting.Common
Imports DNAide.Web
Imports DNAide.Core
Imports System.Text
Imports System.Data

Imports IBCUtils
Imports IBCUtils.DAL
Imports IBCDLL.Customer
Imports IBCUtils.Web
Imports DNAide.Data
Imports BLL.Customer
Imports Sunplus.Core.LoggingLib
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Core.Web
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Business.Customer
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Extensions

Partial Class Customer_V2_AddNewAgent
    Inherits RequiredAuthorizationPage
    Dim bodyTpl, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl As String
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis, strHidInfo As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidNumberGame, strHidOther, strHidFinancial, strTranferTime, strMixedSports As String
    Dim groupBtn As String
    Dim selectTpl As String
    Protected masterId As String
    Dim selDisabledTpl As String
    Dim isEnabledAutoMaster As Boolean

    Protected Overrides ReadOnly Property CustId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.MasterId)
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

            htmlAddAgentContent.Text = LoadData()
        Catch ex As Exception
            'Response.Write("<script type=""text/javascript"">ShowError(" & ReplaceData(ex.Message) & ",'err');</script>")
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

        script += " var lblChooseDaily='" & Resources.Language.lblConfirmChooseDaily & "';" & vbNewLine
        script += " var lblChooseOneDay='" & Resources.Language.lblChooseOneDay & "';" & vbNewLine

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

#Region "Load Data"
    Function LoadData() As String
        Dim html As New StringBuilder
        Try
            Dim rs As DataSet
            rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.CustomerSelMAgentInfo(Request("masterId"))
            masterId = Request("masterId")
            If Not rs Is Nothing Then
                If rs.Tables(0).Rows.Count > 0 Then
                    Dim row As DataRow = rs.Tables(0).Rows(0)
                    CustomerUtils.GetSportTitle(bodyTpl, False)
                    TemplateHandler.Assign(bodyTpl, "accinfo", GetDataInfo(row))
                    TemplateHandler.Assign(bodyTpl, "add-text", Resources.Language.Add)
                    TemplateHandler.Assign(bodyTpl, "add-id", "btnAdd")
                    TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row))
                    TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl))
                    TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, footballTpl))
                    TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, tennisTpl))
                    TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8))
                    TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, golfTpl))
                    TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11))

                    Dim numbergame As SportBooksBase = New NumberGameManager()
                    numbergame.GetNumberCom(row, 2, True, bodyTpl)

                    If Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled Then
                        TemplateHandler.Assign(bodyTpl, "NumberGamePT", GetDataNumberGame(row))
                    Else
                        TemplateHandler.RemoveBlock(bodyTpl, "NumberGame")
                    End If


                    TemplateHandler.Assign(bodyTpl, "MixedSportsPT", GetDataMixedSports(row))
                    TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, otherSportsTpl))


                    If Not Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                        TemplateHandler.Assign(bodyTpl, "hdauto", "hidden")
                        TemplateHandler.ReplaceBlock(bodyTpl, "auto-preset", "")
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
                    bodyTpl &= strHidOther
                    bodyTpl &= strHidInfo
                    bodyTpl &= strHidNumberGame
                    bodyTpl &= strMixedSports
                    html.Append(bodyTpl)
                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
        LoadData = html.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow) As String
        Dim strAccInfo As String = String.Empty
        Dim isInternalMaster As Boolean = False
        Dim hashInfo As New Hashtable
        Try
            strAccInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/AgentInfo.htm")
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-group")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")
            TemplateHandler.RemoveBlock(strAccInfo, "none-add")
            TemplateHandler.RemoveBlock(strAccInfo, "none-member")

            TemplateHandler.Assign(strAccInfo, "cols-untext", "4")

            Dim MemMaxCredit As String = TemplateHandler.CopyBlock(strAccInfo, "Member-Max-Credit2")
            'TemplateHandler.RemoveBlock(strAccInfo, "Member-Max-Credit")
            'TemplateHandler.ReplaceBlock(strAccInfo, "Member-Max-Credit1", MemMaxCredit)
            TemplateHandler.RemoveBlock(strAccInfo, "member-credit-min")
            TemplateHandler.Assign(strAccInfo, "{member-max-credit}", "Member Max Credit")
            TemplateHandler.Assign(strAccInfo, "{mem-max-credit-val}", 0)
            TemplateHandler.Assign(strAccInfo, "{mMaxCredit}", "minmaxbet")


            Dim masterUname As String = ""
            If Request("username") Is Nothing Or Request("username") = String.Empty Then
                masterUname = Session("CustUname").ToString()
            Else
                masterUname = Request("username")
            End If

            TemplateHandler.Assign(strAccInfo, "title", Resources.Language.AddNewAgent & " to " & masterUname)
            TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Language.lblAgentMaxCredit)

            Dim isSuspended As Boolean = CastAide.AsBoolean(dr("suspended"))
            'TemplateHandler.Assign(strAccInfo, "chkSuspended", HtmlHelper.GenCheckBox("chkSuspended", False, isSuspended, "", Globals.CLASS_CHECKBOX_HIDDEN))
            hashInfo.Add("chkSuspended", IIf(isSuspended, 1, 0))


            If DataUtils.IsExistKey(dr, "isinternalmaster") Then
                isInternalMaster = CastAide.AsBoolean(dr("isinternalmaster"), False)
            End If

            'If (WebSettings.DisableInternal Or isInternalMaster) Then
            TemplateHandler.ReplaceBlock(strAccInfo, "internal", HtmlHelper.GenTD(2, 0, ""))
            'Else
            'TemplateHandler.Assign(strAccInfo, "{lblInternalUser}", Resources.Language.lblInternalUser)
            'TemplateHandler.Assign(strAccInfo, "chkInternal", HtmlHelper.GenCheckBox("chkInternal", False, isInternalMaster, "", ""))
            ' End If

            hashInfo.Add("hidinternal", isInternalMaster.ToString().ToLower())
            strHidInfo = CustomerUtils.AssignMultipleHidden(hashInfo)

            TemplateHandler.Assign(strAccInfo, "hdAllowedCurr-value", CastAide.AsInt16(dr("AllowedCurr")))

            TemplateHandler.Assign(strAccInfo, "fname-val", "")
            TemplateHandler.Assign(strAccInfo, "lname-val", "")
            TemplateHandler.Assign(strAccInfo, "phone-val", "")
            TemplateHandler.Assign(strAccInfo, "mobile-val", "")
            CustomerUtils.GetCredit(dr, strAccInfo, False)
            TemplateHandler.Assign(strAccInfo, "check-val", "")
            TemplateHandler.Assign(strAccInfo, "fax-val", "")

            TemplateHandler.Assign(strAccInfo, "groupa-val", CustomerUtils.GetListGroup(CDbl(dr("groupa")), CDbl(dr("groupa"))))
            TemplateHandler.Assign(strAccInfo, "groupb-val", CustomerUtils.GetListGroup(CDbl(dr("groupb")), CDbl(dr("groupb"))))
            TemplateHandler.Assign(strAccInfo, "groupc-val", CustomerUtils.GetListGroup(CDbl(dr("groupc")), CDbl(dr("groupc"))))
            TemplateHandler.Assign(strAccInfo, "groupd-val", CustomerUtils.GetListGroup(CDbl(dr("groupd")), CDbl(dr("groupd"))))
            TemplateHandler.Assign(strAccInfo, "discountcs-val", CustomerUtils.GetListOtherComm(CDbl(dr("discountcs")), CDbl(dr("discountcs"))))



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

            '''''''''ignore direct master''''''''''
            'If Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.IsDirectMaster(CastAide.AsInt32(masterId, -1)) = True Then                
            ''''''''admin can set transfer condition for agent if his master can transfer everyday''''''
            If CustomerUtils.IsTransferDaily(masterId) Then
                TemplateHandler.Assign(strTranferTime, "{referupline}", "0")
                TemplateHandler.Assign(strTranferTime, "{showtransfer}", "")
                CustomerUtils.GetTranferTime(strTranferTime, False)
            Else
                TemplateHandler.Assign(strTranferTime, "{referupline}", "1")
                TemplateHandler.Assign(strTranferTime, "{showtransfer}", "display:none;")
            End If


            TemplateHandler.Assign(strAccInfo, "tranfer-time", strTranferTime)

            strAccInfo += HtmlHelper.GenHidden("uplineusername", masterUname)

            CustomerUtils.GetTitleInfo(strAccInfo, False)

            isEnabledAutoMaster = Not CastAide.AsBoolean(dr("disableautompt"))
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
            TemplateHandler.RemoveBlock(soccerTpl, "none-edit")
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
            TemplateHandler.Assign(soccerTpl, "chk-hdpd", "")
            TemplateHandler.Assign(soccerTpl, "chk-1x2", "")
            TemplateHandler.Assign(soccerTpl, "chk-hdpl", "")

            'assign value of Minbet, Maxbet , ...
            CustomerUtils.GetMinMaxAddValue(soccerTpl, 1, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, soccerTpl, "")

            'assign value position taking for template
            Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
            Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

            Dim maHdpMin, maOUMin, ma1stHdpMin, ma1stOUMin, maOEMin, ma1x2Min, maCSMin, maTGMin, maMPMin, maORMin As Double
            Dim maLHdpMin, maLOUMin, maL1stHdpMin, maL1stOUMin As Double

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

            aHdpMax2 = CastAide.AsDouble(dr("apositiontaking_1_max"))
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

            mHdpMax = CastAide.AsDouble(dr("mpositiontaking_1"))
            mOUMax = CastAide.AsDouble(dr("mpositiontaking_3"))
            m1stHdpMax = CastAide.AsDouble(dr("mpositiontaking_7"))
            m1stOUMax = CastAide.AsDouble(dr("mpositiontaking_8"))
            mOEMax = CastAide.AsDouble(dr("mpositiontaking_2"))
            m1x2Max = CastAide.AsDouble(dr("mpositiontaking_5"))
            mCSMax = CastAide.AsDouble(dr("mpositiontaking_4"))
            mTGMax = CastAide.AsDouble(dr("mpositiontaking_6"))
            mMPMax = CastAide.AsDouble(dr("mpositiontaking_9"))
            mORMax = CastAide.AsDouble(dr("mpositiontaking_10"))
            mLHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_1"))
            mLOUMax = CastAide.AsDouble(dr("mlivepositiontaking_3"))
            mL1stHdpMax = CastAide.AsDouble(dr("mlivepositiontaking_7"))
            mL1stOUMax = CastAide.AsDouble(dr("mlivepositiontaking_8"))
            mHdp = mHdpMax
            mOU = mOUMax
            m1stHdp = m1stHdpMax
            m1stOU = m1stOUMax
            mOE = mOEMax
            m1x2 = m1x2Max
            mCS = mCSMax
            mTG = mTGMax
            mMP = mMPMax
            mOR = mORMax
            mLHdp = mLHdpMax
            mLOU = mLOUMax
            mL1stHdp = mL1stHdpMax
            mL1stOU = mL1stOUMax

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
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
            aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
            a1stHdpMax = IIf(m1stHdpMax - m1stHdp > a1stHdpMax2, a1stHdpMax2, m1stHdpMax - m1stHdp)
            a1stOUMax = IIf(m1stOUMax - m1stOU > a1stOUMax2, a1stOUMax2, m1stOUMax - m1stOU)
            aOEMax = IIf(mOEMax - mOE > aOEMax2, aOEMax2, mOEMax - mOE)
            a1x2Max = IIf(m1x2Max - m1x2 > a1x2Max2, a1x2Max2, m1x2Max - m1x2)
            aCSMax = IIf(mCSMax - mCS > aCSMax2, aCSMax2, mCSMax - mCS)
            aTGMax = IIf(mTGMax - mTG > aTGMax2, aTGMax2, mTGMax - mTG)
            aMPMax = IIf(mMPMax - mMP > aMPMax2, aMPMax2, mMPMax - mMP)
            aORMax = IIf(mORMax - mOR > aORMax2, aORMax2, mORMax - mOR)
            aLHdpMax = IIf(mLHdpMax - mLHdp > aLHdpMax2, aLHdpMax2, mLHdpMax - mLHdp)
            aLOUMax = IIf(mLOUMax - mLOU > aLOUMax2, aLOUMax2, mLOUMax - mLOU)
            aL1stHdpMax = IIf(mL1stHdpMax - mL1stHdp > aL1stHdpMax2, aL1stHdpMax2, mL1stHdpMax - mL1stHdp)
            aL1stOUMax = IIf(mL1stOUMax - mL1stOU > aL1stOUMax2, aL1stOUMax2, mL1stOUMax - mL1stOU)
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

            hiddenHash.Add("uplineid", masterId) 'master hidden field

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
            hiddenHash.Add("isAutoMPT", Permissions.IsEnabledAutoMaster(isEnabledAutoMaster))

            hiddenHash.Add("maxA1HdpD", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
            hiddenHash.Add("maxA1OUD", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
            hiddenHash.Add("maxA11stHdpD", IIf(m1stHdpMax > a1stHdpMax2, a1stHdpMax2, m1stHdpMax))
            hiddenHash.Add("maxA11stOUD", IIf(m1stOUMax > a1stOUMax2, a1stOUMax2, m1stOUMax))
            hiddenHash.Add("maxA1OE", IIf(mOEMax > aOEMax2, aOEMax2, mOEMax))

            hiddenHash.Add("maxA11x2", IIf(m1x2Max > a1x2Max2, a1x2Max2, m1x2Max))
            hiddenHash.Add("maxA1CS", IIf(mCSMax > aCSMax2, aCSMax2, mCSMax))
            hiddenHash.Add("maxA1TG", IIf(mTGMax > aTGMax2, aTGMax2, mTGMax))
            hiddenHash.Add("maxA1MP", IIf(mMPMax > aMPMax2, aMPMax2, mMPMax))
            hiddenHash.Add("maxA1OR", IIf(mORMax > aORMax2, aORMax2, mORMax))

            hiddenHash.Add("maxA1HdpL", IIf(mLHdpMax > aLHdpMax2, aLHdpMax2, mLHdpMax))
            hiddenHash.Add("maxA1OUL", IIf(mLOUMax > aLOUMax2, aLOUMax2, mLOUMax))
            hiddenHash.Add("maxA11stHdpL", IIf(mL1stHdpMax > aL1stHdpMax2, aL1stHdpMax2, mL1stHdpMax))
            hiddenHash.Add("maxA11stOUL", IIf(mL1stOUMax > aL1stOUMax2, aL1stOUMax2, mL1stOUMax))
            '----------------------------------------------------
            hiddenHash.Add("maxM1HdpD", mHdpMax)
            hiddenHash.Add("maxM1OUD", mOUMax)
            hiddenHash.Add("maxM11stHdpD", m1stHdpMax)
            hiddenHash.Add("maxM11stOUD", m1stOUMax)
            hiddenHash.Add("maxM1OE", mOEMax)

            hiddenHash.Add("maxM11x2", m1x2Max)
            hiddenHash.Add("maxM1CS", mCSMax)
            hiddenHash.Add("maxM1TG", mTGMax)
            hiddenHash.Add("maxM1MP", mMPMax)
            hiddenHash.Add("maxM1OR", mORMax)

            hiddenHash.Add("maxM1HdpL", mLHdpMax)
            hiddenHash.Add("maxM1OUL", mLOUMax)
            hiddenHash.Add("maxM11stHdpL", mL1stHdpMax)
            hiddenHash.Add("maxM11stOUL", mL1stOUMax)

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
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOU))
            TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(a1stHdpMin, a1stHdpMax, a1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(a1stOUMin, a1stOUMax, a1stOU))
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOEMin, aOEMax, aOE))
            TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(a1x2Min, a1x2Max, a1x2))
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aCSMin, aCSMax, aCS))
            TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aTGMin, aTGMax, aTG))
            TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMPMin, aMPMax, aMP))
            TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aORMin, aORMax, aOR))
            TemplateHandler.Assign(soccerTpl, "a-or-val", pt)

            'get PT deadball value for super preset PT
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mCSMin, mCSMax, mCS))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mTGMin, mTGMax, mTG))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mORMin, mORMax, mOR))
            TemplateHandler.Assign(soccerTpl, "p-or-val", pt)

            'get PT value for super preset PT
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aLHdpMin, aLHdpMax, aLHdp))
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aLOUMin, aLOUMax, aLOU))
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aL1stHdpMin, aL1stHdpMax, aL1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aL1stOUMin, aL1stOUMax, aL1stOU))
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
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
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mHdpMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mOUMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, m1stHdpMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, m1stOUMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mOEMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, m1x2Max, 0))
            TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mCSMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mTGMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mMPMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mORMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)




            'For live ball
            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mLHdpMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mLOUMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mL1stHdpMax, 0))
            TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mL1stOUMax, 0))
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
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 9:Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
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
            TemplateHandler.Assign(tpl, "chk-hdp", "")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            TemplateHandler.RemoveBlock(tpl, "no-mixparlay")

            'assign value for position taking
            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMPMin, mMLMin As Double
            Dim mHdpMax, mOUMax, mOEMax, mMPMax, mMLMax As Double
            Dim mHdp, mOU, mOE, mMP, mML As Double

            Dim aHdpMin, aOUMin, aOEMin, aMPMin, aMLMin As Double
            Dim aHdpMax, aOUMax, aOEMax, aMPMax, aMLMax As Double
            Dim aHdp, aOU, aOE, aMP, aML As Double
            Dim aHdpMax2, aOUMax2, aOEMax2, aMPMax2, aMLMax2 As Double
            Dim maHdpMin, maOUMin, maOEMin, maMPMin, maMLMin As Double

            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
            aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'aMPMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_9_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

            mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'mMPMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9"))
            mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
            mHdp = mHdpMax
            mOU = mOUMax
            mOE = mOEMax
            mMP = mMPMax
            mML = mMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'maMPMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9_min"))
            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
            aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
            aOEMax = IIf(mOEMax - mOE > aOEMax2, aOEMax2, mOEMax - mOE)
            aMPMax = IIf(mMPMax - mMP > aMPMax2, aMPMax2, mMPMax - mMP)
            aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
            aHdp = aHdpMax
            aOU = aOUMax
            aOE = aOEMax
            aMP = aMPMax
            aML = aMLMax

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
            hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)
            hiddenHash.Add("minM" & sportTypeId & "MP", maMPMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            'hiddenHash.Add("maxA" & sportTypeId & "Hdp", aHdpMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "OU", aOUMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "OE", aOEMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "MP", aMPMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "ML", aMLMax2)
            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
            hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
            hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(mOEMax > aOEMax2, aOEMax2, mOEMax))
            hiddenHash.Add("maxA" & sportTypeId & "MP", IIf(mMPMax > aMPMax2, aMPMax2, mMPMax))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))
            '--------------------------------------------------------------------------------------

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax)
            hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax)
            hiddenHash.Add("maxM" & sportTypeId & "MP", mMPMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)
            strHidBasket = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdpMax))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOUMax))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOEMin, aOEMax, aOEMax))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'pt = selectTpl
            'TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "MP")
            'TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMPMin, aMPMax, aMPMax))
            'TemplateHandler.Assign(tpl, "a-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aMLMax))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'pt = selectTpl
            'TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "MP")
            'TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
            'TemplateHandler.Assign(tpl, "p-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", btnTpl)
            '----------------------------------------------------------------------------------------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '/----------------------------------------------------------------------------------------------------------------
            '----------------------------------------------------------------------------------------------------------------
            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mHdpMax, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mOUMax, 0))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mOEMax, 0))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OE", String.Empty)

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'ptAuto = selDisabledTpl
            'TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mMPMax, 0))
            'TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)
            'TemplateHandler.Assign(tpl, "checked-MP", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mMLMax, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)
            '/----------------------------------------------------------------------------------------------------------------
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
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "title-col", "6")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "chk-hdp", "")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'assign value for position taking
            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
            Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double
            Dim mHdp, mOU, mOE, mML As Double

            Dim aHdpMin, aOUMin, aOEMin, aMLMin As Double
            Dim aHdpMax, aOUMax, aOEMax, aMLMax As Double
            Dim aHdp, aOU, aOE, aML As Double
            Dim aHdpMax2, aOUMax2, aOEMax2, aMLMax2 As Double
            Dim maHdpMin, maOUMin, maOEMin, maMLMin As Double

            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
            aOEMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

            mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOEMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
            mHdp = mHdpMax
            mOU = mOUMax
            mOE = mOEMax
            mML = mMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            maOEMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2_min"))
            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
            aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
            aOEMax = IIf(mOEMax - mOE > aOEMax2, aOEMax2, mOEMax - mOE)
            aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
            aHdp = aHdpMax
            aOU = aOUMax
            aOE = aOEMax
            aML = aMLMax

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
            hiddenHash.Add("minM" & sportTypeId & "OE", maOEMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            'hiddenHash.Add("maxA" & sportTypeId & "Hdp", aHdpMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "OU", aOUMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "OE", aOEMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "ML", aMLMax2)
            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
            hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
            hiddenHash.Add("maxA" & sportTypeId & "OE", IIf(mOEMax > aOEMax2, aOEMax2, mOEMax))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))
            '--------------------------------------------------------------------------------------

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax)
            hiddenHash.Add("maxM" & sportTypeId & "OE", mOEMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)

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
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdpMax))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOUMax))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOEMin, aOEMax, aOEMax))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aMLMax))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOUMax))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOEMax))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)

            '----------------------------------------------------------------------------------------------------------------
            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mHdpMax, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mOUMax, 0))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mOEMax, 0))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OE", String.Empty)


            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mMLMax, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)
            '/--------------------------------------------------------------------------------------------------------------

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)
            '--------------------------------------------------------------------------------------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '/--------------------------------------------------------------------------------------------------------------
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
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BaseballPT.htm")
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "title-col", "5")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "chk-hdp", "")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'assign value for position taking
            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mMLMin As Double
            Dim mHdpMax, mOUMax, mMLMax As Double
            Dim mHdp, mOU, mML As Double

            Dim aHdpMax, aOUMax, aMLMax As Double
            Dim aHdpMin, aOUMin, aMLMin As Double
            Dim aHdp, aOU, aML As Double
            Dim aHdpMax2, aOUMax2, aMLMax2 As Double
            Dim maHdpMin, maOUMin, maMLMin As Double

            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aOUMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

            mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOUMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
            mHdp = mHdpMax
            mOU = mOUMax
            mML = mMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maOUMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3_min"))
            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
            aOUMax = IIf(mOUMax - mOU > aOUMax2, aOUMax2, mOUMax - mOU)
            aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
            aHdp = aHdpMax
            aOU = aOUMax
            aML = aMLMax

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "OU", maOUMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            'hiddenHash.Add("maxA" & sportTypeId & "Hdp", aHdpMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "OU", aOUMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "ML", aMLMax2)
            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
            hiddenHash.Add("maxA" & sportTypeId & "OU", IIf(mOUMax > aOUMax2, aOUMax2, mOUMax))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))
            '--------------------------------------------------------------------------------------

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "OU", mOUMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)
            strHidBaseball = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aOUMin, aOUMax, aOU))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)

            '----------------------------------------------------------------------------------------------------------------

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mHdpMax, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mOUMax, 0))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-OU", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mMLMax, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


            Dim btnTpl As String = groupBtn

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '/-------------------------------------------------------------------------------------------------------------------

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
        Dim ptAuto As String = String.Empty
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MotorSportsPT.htm")
            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "title-col", "4")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "chk-hdp", "")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)            
            CustomerUtils.GetMinMaxAddValue(tpl, 11, dr)
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

            aHdpMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1_max"))
            aMLMax2 = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20_max"))

            mHdpMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mMLMax = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))
            mHdp = mHdpMax
            mML = mMLMax

            maHdpMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1_min"))
            maMLMin = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20_min"))

            aHdpMax = IIf(mHdpMax - mHdp > aHdpMax2, aHdpMax2, mHdpMax - mHdp)
            aMLMax = IIf(mMLMax - mML > aMLMax2, aMLMax2, mMLMax - mML)
            aHdp = aHdpMax
            aML = aMLMax

            hiddenHash.Add("minM" & sportTypeId & "Hdp", maHdpMin)
            hiddenHash.Add("minM" & sportTypeId & "ML", maMLMin)

            'hiddenHash.Add("maxA" & sportTypeId & "Hdp", aHdpMax2)
            'hiddenHash.Add("maxA" & sportTypeId & "ML", aMLMax2)
            hiddenHash.Add("maxA" & sportTypeId & "Hdp", IIf(mHdpMax > aHdpMax2, aHdpMax2, mHdpMax))
            hiddenHash.Add("maxA" & sportTypeId & "ML", IIf(mMLMax > aMLMax2, aMLMax2, mMLMax))
            '------------------------------------------------------------------------------

            hiddenHash.Add("maxM" & sportTypeId & "Hdp", mHdpMax)
            hiddenHash.Add("maxM" & sportTypeId & "ML", mMLMax)
            strHidMoto = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aHdpMin, aHdpMax, aHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(aMLMin, aMLMax, aML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", pt)

            '----------------------------------------------------------------------------------------------------------------

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mHdpMax, 0))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-Hdp", String.Empty)

            ptAuto = selDisabledTpl
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mMLMax, 0))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)
            TemplateHandler.Assign(tpl, "checked-ML", String.Empty)


            Dim btnTpl As String = groupBtn

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '----------------------------------------------------------------------------------------------------------------
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

#Region "Assign info for NumberGame"
    Function GetDataNumberGame(ByVal dr As DataRow) As String
        TemplateHandler.Assign(bodyTpl, "NumberGame-text", Resources.Language.lblNumberGame)
        Dim numbergame As SportBooksBase = New NumberGameManager()
        numbergame.GetNumberCom(dr, 2, True, bodyTpl)
        Dim numberGameSetting As String
        numberGameSetting = numbergame.GetAgentPtSetting_AddNew(dr, strHidNumberGame)
        numbergame.GetMaxPerBall(dr, 2, False, numberGameSetting)
        Return numberGameSetting
    End Function
#End Region

#Region "Assign info for Mixed Sports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        TemplateHandler.Assign(bodyTpl, "MixedSports-text", Resources.Language.lblMixedSports)
        Dim mixedSports As SportBooksBase = New MixedSportsManager()
        Return mixedSports.GetAgentPtSetting_AddNew(dr, strMixedSports)
    End Function

#End Region

#Region "Get Title Common"
    Function GetTitleCommon(ByRef tpl As String)

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
    Public Shared Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
End Class
