Imports Sunplus.Accounting.Common
Imports DNAide.Data
Imports DNAide.Core
Imports DNAide.Web
Imports IBCUtils.Web
Imports IBCUtils.Encryption
Imports System.Data
Imports IBCDLL
Imports IBCUtils.DAL
Imports IBCUtils.FormatHandler
Imports BLL.Customer

Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Core.Web
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Business.Customer
Imports Sunplus.Accounting.Core.Extensions

Partial Class Customer_V2_EditMasterOnMember
    Inherits RequiredAuthorizationPage
    Protected style As String
    Dim bodyTpl, for4ddlTpl, for3ddlTpl, basketballTpl As String
    Dim groupBtn As String
    Dim selectTpl As String
    Public strUser As String
    Protected m As Integer
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis, strHidFinancial, strHidMixedSport As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidNumberGame, strHidOther As String
    Dim selDisabledTpl As String
    Public disabledBtn As String
    Protected msg As String = String.Empty
    Dim CopyAll, CopyBasket As Boolean
    Dim isEnabledAutoMaster As Boolean = True


    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.AgentId)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MenuPT.htm")
            groupBtn = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BtnGroup.htm")
            selectTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/dropdown-MemPT.htm")
            selDisabledTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/ddlDisablePT.htm")
            for4ddlTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/footballMasterOnMember.htm")
            basketballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/basketMasterOnMember.htm")
            m = 0
            saveCustid.Value = Request.QueryString("custid")
            saveAgentid.Value = Request.QueryString("agentid")
            htmlSuperEditMember.Text = LoadDataSingle()
            isMult.Value = "0"
            If (Not Request("msg") Is Nothing) Then
                msg = Request("msg")
                msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success)
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub

#Region "Load Information for Single Edit Member"
    Function LoadDataSingle() As String
        Dim strEditContent As New StringBuilder
        Try
            'Dim rs As DataResult
            'rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.Select4Edit.EditMasterOnMemberSel(saveCustid.Value, Request.QueryString("agentid"))
            Dim rs As DbResult
            rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.Select4Edit.EditMasterOnMemberSel(saveCustid.Value, Request.QueryString("agentid"))
            If Not rs Is Nothing Then
                If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                    Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                    isAllowUpdPT.Value = "1"

                    TemplateHandler.RemoveBlock(bodyTpl, "editsuper")
                    TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")

                    strEditContent.Append(GetDataInfo(row, m))
                    CustomerUtils.GetSportTitle(bodyTpl, True)
                    TemplateHandler.Assign(bodyTpl, "add-text", Resources.Language.Submit)
                    TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")
                    TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(row, m))
                    TemplateHandler.Assign(bodyTpl, "basketPT", GetDataBasketball(row, 2, basketballTpl, m))
                    TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(row, 3, for4ddlTpl, m))
                    TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(row, 5, for4ddlTpl, m))
                    TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(row, 8, m))
                    TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(row, 10, for4ddlTpl, m))
                    TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(row, 11, m))

                    If Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled Then
                        TemplateHandler.Assign(bodyTpl, "NumberGamePT", GetDataNumberGame(row))
                    Else
                        TemplateHandler.RemoveBlock(bodyTpl, "NumberGame")
                    End If

                    'mixed sports
                    TemplateHandler.Assign(bodyTpl, "MixedSportsPT", GetDataMixedSports(row))

                    TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, for4ddlTpl, m))
                    If Not CopyAll Then
                        'TemplateHandler.RemoveBlock(bodyTpl, "limit-copyAll")
                        TemplateHandler.Assign(bodyTpl, "{disabled-copyall}", "disabled")
                    End If
                    If Not CopyBasket Then
                        TemplateHandler.Assign(bodyTpl, "disabled-copybasketall", "disabled")
                    End If

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
                    bodyTpl &= strHidNumberGame
                    bodyTpl &= strHidOther
                    bodyTpl &= strHidFinancial
                    'mixed sport
                    bodyTpl &= strHidMixedSport

                    strEditContent.Append(bodyTpl)
                Else
                    msg = Resources.Language.thereisnodata
                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        LoadDataSingle = strEditContent.ToString()
    End Function
#End Region

#Region "Assign info about UNam, Max Credit, Commission ..."
    Function GetDataInfo(ByVal dr As DataRow, ByVal isMult As Boolean) As String
        Dim strAccInfo As String = String.Empty
        Try
            strAccInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/AccInfoNoneEdit.htm")
            If Not isMult Then
                GetTitleMember(strAccInfo)
                TemplateHandler.RemoveBlock(strAccInfo, "none-Single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-Master")

                TemplateHandler.Assign(strAccInfo, "title", Resources.Language.Edit + " " + CastAide.AsString(dr("username")))
                TemplateHandler.Assign(strAccInfo, "uname-val", CastAide.AsString(dr("username")))
                TemplateHandler.Assign(strAccInfo, "fname-val", CastAide.AsString(dr("firstname")))
                TemplateHandler.Assign(strAccInfo, "lname-val", CastAide.AsString(dr("lastname")))
                TemplateHandler.Assign(strAccInfo, "email-val", CastAide.AsString(dr("fax")))
                TemplateHandler.Assign(strAccInfo, "group-val", CastAide.AsString(dr("ugroup")))
                TemplateHandler.Assign(strAccInfo, "credit-val", FormatNumber1(dr("credit")))
                TemplateHandler.Assign(strAccInfo, "phone-val", CastAide.AsString(dr("phone")))
                TemplateHandler.Assign(strAccInfo, "mobile-val", CastAide.AsString(dr("mobilephone")))

                If CastAide.AsBoolean(dr("closed")) Then
                    TemplateHandler.Assign(strAccInfo, "check-val", "checked")
                Else
                    TemplateHandler.Assign(strAccInfo, "check-val", "")
                End If

                TemplateHandler.Assign(strAccInfo, "a-comm-val", FormatNumber4(dr("discount")))
                TemplateHandler.Assign(strAccInfo, "a-commo-val", FormatNumber5(dr("discountcs")))
                TemplateHandler.Assign(strAccInfo, "comm-val", FormatNumber4(dr("playerdiscount")))
                TemplateHandler.Assign(strAccInfo, "commo-val", FormatNumber5(dr("playerdiscountcs")))


                ''New Commission

                Dim tplPTData As String = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer\v2\NewPTCommissionData.htm")
                Dim tplPTValue As String = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer\v2\NewPTCommissionValue.htm")

                Dim dataHeader As StringBuilder = New StringBuilder()
                Dim dataHDP_OU_OE As StringBuilder = New StringBuilder()
                Dim data1x2 As StringBuilder = New StringBuilder()
                Dim dataOther As StringBuilder = New StringBuilder()

                Dim valueHeader As String = tplPTValue
                Dim valueBetType As String = tplPTValue


                valueHeader = tplPTValue
                TemplateHandler.Assign(valueHeader, "{value}", HtmlHelper.GenLabel("", String.Format(Resources.Language.lblCommFor, Resources.Language.Agent), ""))
                dataHeader.Append(valueHeader)

                valueHeader = tplPTValue
                TemplateHandler.Assign(valueHeader, "{value}", HtmlHelper.GenLabel("", String.Format(Resources.Language.lblCommFor, Resources.Language.Member), ""))
                dataHeader.Append(valueHeader)



                'HDP 
                'Agent
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("discount")), ""))
                dataHDP_OU_OE.Append(valueBetType)
                'Member
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("playerdiscount")), ""))
                dataHDP_OU_OE.Append(valueBetType)

                'Other
                'Agent
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("discountcs")), ""))
                dataOther.Append(valueBetType)
                'Member
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("playerdiscountcs")), ""))
                dataOther.Append(valueBetType)

                If (WebSettings.Disable1x2Comm) Then
                    TemplateHandler.RemoveBlock(tplPTData, "1x2")
                Else
                    Try
                        '1x2
                        'Agent
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("discount1x2")), ""))
                        data1x2.Append(valueBetType)
                        'Member
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("playerdiscount1x2")), ""))
                        data1x2.Append(valueBetType)
                    Catch ex As Exception
                        '1x2
                        'Agent
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", "0", ""))
                        data1x2.Append(valueBetType)
                        'Member
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", "0", ""))
                        data1x2.Append(valueBetType)
                    End Try
                End If

                isEnabledAutoMaster = Not CastAide.AsBoolean(dr("disableautompt"))



                TemplateHandler.Assign(tplPTData, "{header_value}", dataHeader.ToString())
                TemplateHandler.Assign(tplPTData, "{hdpouoe_value}", dataHDP_OU_OE.ToString())
                TemplateHandler.Assign(tplPTData, "{1x2_value}", data1x2.ToString())
                TemplateHandler.Assign(tplPTData, "{other_value}", dataOther.ToString())

                TemplateHandler.Assign(tplPTData, "{lblHDPOUOE}", Resources.Language.lblHDPOUOE)
                TemplateHandler.Assign(tplPTData, "{lbl1x2}", Resources.Language.lbl1x2)
                TemplateHandler.Assign(tplPTData, "{lblother}", Resources.Language.lblOtherBettypes)

                TemplateHandler.Assign(strAccInfo, "{NewCommission}", tplPTData)
                ''End New Commission


                TemplateHandler.Assign(strAccInfo, "minbet-val", FormatNumber1(dr("minbet")))
                TemplateHandler.Assign(strAccInfo, "maxbet-val", FormatNumber1(dr("maxbet")))
                TemplateHandler.Assign(strAccInfo, "maxpermatch-val", FormatNumber1(dr("maxpermatch")))
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 2, dr)
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 3, dr)
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 5, dr)
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 8, dr)
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 10, dr)
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 11, dr)

                CustomerUtils.GetMinMaxEditValue(strAccInfo, 99, dr)
                'CustomerUtils.GetMinMaxEditValue(strAccInfo, 151, dr)
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 161, dr)
                MixedSportsManager.GetEditMinMaxValue(dr, strAccInfo)

                Dim numbercomm As NumberGameManager = New NumberGameManager()
                numbercomm.GetMasterOnMemberComm(dr, strAccInfo)

            Else
                TemplateHandler.RemoveBlock(strAccInfo, "none-Mult")
                TemplateHandler.Assign(strAccInfo, "check-val", "")
                TemplateHandler.Assign(strAccInfo, "status-text", Resources.Language.Status)
                TemplateHandler.Assign(strAccInfo, "closed-text", Resources.Language.lblStatusClosed)
            End If
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
            soccerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/soccerMasterOnMemberPT.htm")
            headerSTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SBettypeHeader.htm")
            'TemplateHandler.RemoveBlock(soccerTpl, "none-Master")
            'TemplateHandler.RemoveBlock(soccerTpl, "none-Master")
            'TemplateHandler.RemoveBlock(soccerTpl, "none-Master")
            'TemplateHandler.RemoveBlock(soccerTpl, "none-Master")
            If isMult Then
                TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")
                TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")
                TemplateHandler.Assign(soccerTpl, "dead-row", "2")
                TemplateHandler.Assign(soccerTpl, "live-row", "1")
            Else
                TemplateHandler.Assign(soccerTpl, "dead-row", IIf(Permissions.IsEnabledAutoMaster(isEnabledAutoMaster), 5, 3))
                TemplateHandler.Assign(soccerTpl, "live-row", IIf(Permissions.IsEnabledAutoMaster(isEnabledAutoMaster), 4, 2))
            End If
            TemplateHandler.Assign(soccerTpl, "col-title", "12")
            TemplateHandler.Assign(soccerTpl, "bettype-header", CustomerUtils.LoadSoccerPTHeader(False))
            GetTitleCommon(soccerTpl)

            TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Language.Deadball)
            TemplateHandler.Assign(soccerTpl, "live-text", Resources.Language.Live)
            TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Language.Master)
            TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Language.Agent)
            TemplateHandler.Assign(soccerTpl, "au-presetpt-text", String.Format(Resources.Language.lblAuto, Resources.Language.Master))
            TemplateHandler.Assign(soccerTpl, "au-preset-live", String.Format(Resources.Language.lblAuto, Resources.Language.Master))
            TemplateHandler.Assign(soccerTpl, "cols-live", "1")
            TemplateHandler.Assign(soccerTpl, "copy-live-col", IIf(Permissions.IsEnabledAutoMaster(isEnabledAutoMaster), 3, 1))
            TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Language.CopyFrom1x2)
            'assign value of Minbet, Maxbet , ...           

            Dim aHdp, aOU, a1stHdp, a1stOU, aOE, a1x2, aCS, aTG, aMP, aOR As Double
            Dim aLHdp, aLOU, aL1stHdp, aL1stOU As Double

            Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
            Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

            Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
            Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

            Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
            Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double



            '------------------------Auto PT ------------------------- Shifu -------------------------------------
            'Master auto on Member
            Dim isAutoHdp_Mem, isAutoOU_Mem, isAuto1stHdp_Mem, isAuto1stOU_Mem, isAutoOE_Mem, isAuto1x2_Mem, isAutoCS_Mem, isAutoTG_Mem, isAutoMP_Mem, isAutoOR_Mem As Double
            Dim isAutoLHdp_Mem, isAutoLOU_Mem, isAutoL1stHdp_Mem, isAutoL1stOU_Mem As Double

            'Master Auto on Agent
            Dim isAutoHdp_Agent, isAutoOU_Agent, isAuto1stHdp_Agent, isAuto1stOU_Agent, isAutoOE_Agent, isAuto1x2_Agent, isAutoCS_Agent, isAutoTG_Agent, isAutoMP_Agent, isAutoOR_Agent As Double
            Dim isAutoLHdp_Agent, isAutoLOU_Agent, isAutoL1stHdp_Agent, isAutoL1stOU_Agent As Double

            Dim mAutoHdp, mAutoOU, mAuto1stHdp, mAuto1stOU, mAutoOE, mAuto1x2, mAutoCS, mAutoTG, mAutoMP, mAutoOR As Double
            Dim mAutoLHdp, mAutoLOU, mAutoL1stHdp, mAutoL1stOU As Double

            Dim mAutoHdpMax, mAutoOUMax, mAuto1stHdpMax, mAuto1stOUMax, mAutoOEMax, mAuto1x2Max, mAutoCSMax, mAutoTGMax, mAutoMPMax, mAutoORMax As Double
            Dim mAutoLHdpMax, mAutoLOUMax, mAutoL1stHdpMax, mAutoL1stOUMax As Double
            '-------------------------------------------------------------------------------------------------------

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
            '-------Auto Value--------
            mAutoHdp = CastAide.AsDouble(dr("auto_mpositiontaking_mem_1"))
            mAutoOU = CastAide.AsDouble(dr("auto_mpositiontaking_mem_3"))
            mAuto1stHdp = CastAide.AsDouble(dr("auto_mpositiontaking_mem_7"))
            mAuto1stOU = CastAide.AsDouble(dr("auto_mpositiontaking_mem_8"))
            mAutoOE = CastAide.AsDouble(dr("auto_mpositiontaking_mem_2"))
            mAuto1x2 = CastAide.AsDouble(dr("auto_mpositiontaking_mem_5"))
            mAutoCS = CastAide.AsDouble(dr("auto_mpositiontaking_mem_4"))
            mAutoTG = CastAide.AsDouble(dr("auto_mpositiontaking_mem_6"))
            mAutoMP = CastAide.AsDouble(dr("auto_mpositiontaking_mem_9"))
            mAutoOR = CastAide.AsDouble(dr("auto_mpositiontaking_mem_10"))
            mAutoLHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_1"))
            mAutoLOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_3"))
            mAutoL1stHdp = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_7"))
            mAutoL1stOU = CastAide.AsDouble(dr("auto_mlivepositiontaking_mem_8"))
            '------------- End -----------
            '----------------------------------------------PT-----------------Tho Add----------------------------------

            If Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                isAutoHdp_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_3"))
                isAuto1stHdp_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_7"))
                isAuto1stOU_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_8"))
                isAutoOE_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_2"))
                isAuto1x2_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_5"))
                isAutoCS_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_4"))
                isAutoTG_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_6"))
                isAutoMP_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_9"))
                isAutoOR_Mem = CastAide.AsInt16(dr("isauto_mpositiontaking_mem_10"))
                isAutoLHdp_Mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_1"))
                isAutoLOU_Mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_3"))
                isAutoL1stHdp_Mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_7"))
                isAutoL1stOU_Mem = CastAide.AsInt16(dr("isauto_mlivepositiontaking_mem_8"))

                'Auto master on agent
                isAutoHdp_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_1"))
                isAutoOU_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_3"))
                isAuto1stHdp_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_7"))
                isAuto1stOU_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_8"))
                isAutoOE_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_2"))
                isAuto1x2_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_5"))
                isAutoCS_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_4"))
                isAutoTG_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_6"))
                isAutoMP_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_9"))
                isAutoOR_Agent = CastAide.AsInt16(dr("isauto_mpositiontaking_10"))
                isAutoLHdp_Agent = CastAide.AsInt16(dr("isauto_mlivepositiontaking_1"))
                isAutoLOU_Agent = CastAide.AsInt16(dr("isauto_mlivepositiontaking_3"))
                isAutoL1stHdp_Agent = CastAide.AsInt16(dr("isauto_mlivepositiontaking_7"))
                isAutoL1stOU_Agent = CastAide.AsInt16(dr("isauto_mlivepositiontaking_8"))
            Else
                isAutoHdp_Mem = False
                isAutoOU_Mem = False
                isAuto1stHdp_Mem = False
                isAuto1stOU_Mem = False
                isAutoOE_Mem = False
                isAuto1x2_Mem = False
                isAutoCS_Mem = False
                isAutoTG_Mem = False
                isAutoMP_Mem = False
                isAutoOR_Mem = False
                isAutoLHdp_Mem = False
                isAutoLOU_Mem = False
                isAutoL1stHdp_Mem = False
                isAutoL1stOU_Mem = False
            End If






            hiddenHash.Add("isAutoHdp", isAutoHdp_Mem)
            hiddenHash.Add("isAutoOU", isAutoOU_Mem)
            hiddenHash.Add("isAuto1stHdp", isAuto1stHdp_Mem)
            hiddenHash.Add("isAuto1stOU", isAuto1stOU_Mem)
            hiddenHash.Add("isAutoOE", isAutoOE_Mem)

            hiddenHash.Add("isAuto1x2", isAuto1x2_Mem)
            hiddenHash.Add("isAutoCS", isAutoCS_Mem)
            hiddenHash.Add("isAutoTG", isAutoTG_Mem)
            hiddenHash.Add("isAutoMP", isAutoMP_Mem)
            hiddenHash.Add("isAutoOR", isAutoOR_Mem)

            hiddenHash.Add("isAutoLHdp", isAutoLHdp_Mem)
            hiddenHash.Add("isAutoLOU", isAutoLOU_Mem)
            hiddenHash.Add("isAutoL1stHdp", isAutoL1stHdp_Mem)
            hiddenHash.Add("isAutoL1stOU", isAutoL1stOU_Mem)

            'Agent
            hiddenHash.Add("isAutoHdp_Agent", isAutoHdp_Agent)
            hiddenHash.Add("isAuto1x2_Agent", isAuto1x2_Agent)
            hiddenHash.Add("isAutoHdpL_Agent", isAutoLHdp_Agent)
            strHidSoccer = AssignMultipleHidden(hiddenHash)
            '/---------------------------------------------------------------------------------------------------------
            If Not isMult Then 'this case for Single Edit              
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

                mHdpMin = CastAide.AsDouble(dr("minpos_1"))
                mOUMin = CastAide.AsDouble(dr("minpos_3"))
                m1stHdpMin = CastAide.AsDouble(dr("minpos_7"))
                m1stOUMin = CastAide.AsDouble(dr("minpos_8"))
                mOEMin = CastAide.AsDouble(dr("minpos_2"))
                m1x2Min = CastAide.AsDouble(dr("minpos_5"))
                mCSMin = CastAide.AsDouble(dr("minpos_4"))
                mTGMin = CastAide.AsDouble(dr("minpos_6"))
                mMPMin = CastAide.AsDouble(dr("minpos_9"))
                mORMin = CastAide.AsDouble(dr("minpos_10"))
                mLHdpMin = CastAide.AsDouble(dr("minlpos_1"))
                mLOUMin = CastAide.AsDouble(dr("minlpos_3"))
                mL1stHdpMin = CastAide.AsDouble(dr("minlpos_7"))
                mL1stOUMin = CastAide.AsDouble(dr("minlpos_8"))

                mHdpMax = CastAide.AsDouble(dr("maxpos_1"))
                mOUMax = CastAide.AsDouble(dr("maxpos_3"))
                m1stHdpMax = CastAide.AsDouble(dr("maxpos_7"))
                m1stOUMax = CastAide.AsDouble(dr("maxpos_8"))
                mOEMax = CastAide.AsDouble(dr("maxpos_2"))
                m1x2Max = CastAide.AsDouble(dr("maxpos_5"))
                mCSMax = CastAide.AsDouble(dr("maxpos_4"))
                mTGMax = CastAide.AsDouble(dr("maxpos_6"))
                mMPMax = CastAide.AsDouble(dr("maxpos_9"))
                mORMax = CastAide.AsDouble(dr("maxpos_10"))
                mLHdpMax = CastAide.AsDouble(dr("maxlpos_1"))
                mLOUMax = CastAide.AsDouble(dr("maxlpos_3"))
                mL1stHdpMax = CastAide.AsDouble(dr("maxlpos_7"))
                mL1stOUMax = CastAide.AsDouble(dr("maxlpos_8"))

                '---Auto Master-----
                mAutoHdpMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_1"))
                mAutoOUMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_3"))
                mAuto1stHdpMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_7"))
                mAuto1stOUMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_8"))
                mAutoOEMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_2"))
                mAuto1x2Max = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_5"))
                mAutoCSMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_4"))
                mAutoTGMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_6"))
                mAutoMPMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_9"))
                mAutoORMax = CastAide.AsDouble(dr("maxauto_mpositiontaking_mem_10"))
                mAutoLHdpMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_1"))
                mAutoLOUMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_3"))
                mAutoL1stHdpMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_7"))
                mAutoL1stOUMax = CastAide.AsDouble(dr("maxauto_mlivepositiontaking_mem_8"))
                '---END----------

            Else 'this case for Multiple Edit
                mHdpMax = mHdp
                mOUMax = mOU
                m1stHdpMax = m1stHdp
                m1stOUMax = m1stOU
                mOEMax = mOE
                m1x2Max = m1x2
                mCSMax = mCS
                mTGMax = mTG
                mMPMax = mMP
                mORMax = mOR
                mLHdpMax = mLHdp
                mLOUMax = mLOU
                mL1stHdpMax = mL1stHdp
                mL1stOUMax = mL1stOU
            End If

            'Copy All Checkbox constraint
            If isAutoHdp_Agent Then
                'TemplateHandler.Assign(soccerTpl, "p-hdp-val", mHdp)
                TemplateHandler.Assign(soccerTpl, "{disabled-copyhdpd}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
                CopyAll = IIf(isAutoHdp_Mem, True, False)
            Else
                'If isAutoOU_Mem And isAuto1stHdp_Mem And isAuto1stOU_Mem And isAutoOE_Mem Then
                'TemplateHandler.RemoveBlock(soccerTpl, "limit-CopyHdp")
                'End If
                CopyAll = True
            End If

            If isAuto1x2_Agent Then
                TemplateHandler.Assign(soccerTpl, "{disabled-copy1x2}", IIf(isAuto1x2_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If

            If isAutoLHdp_Agent Then
                TemplateHandler.Assign(soccerTpl, "{disabled-copyhdpl}", IIf(isAutoLHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If

            'assign Increase and Reduce button for template
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(soccerTpl, "p-hdp-btn", IIf(isAutoHdp_Agent, "", btnTpl))
            '----------Auto Button-----
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM1HdpD")

            TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl) 'hide auto on member if master got auto on agent
            '----------END-------------

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(soccerTpl, "p-1x2-btn", IIf(isAuto1x2_Agent, "", btnTpl))
            '----------Auto Button-----
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM11x2")
            TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)
            '----------END-------------

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", IIf(isAutoLHdp_Agent, "", btnTpl))
            '----------Auto Button-----
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM1HdpL")
            TemplateHandler.Assign(soccerTpl, "{au-Lhdp-btn}", btnTpl)
            '----------END-------------


            Dim pt As String = selectTpl
            Dim strChecked As String
            Dim ptAuto As String
            Dim ptMaster As String
            'TemplateHandler.Assign(soccerTpl, "a-hdp-val", aHdp)
            'TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
            'TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            'TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"

            ElseIf isAutoHdp_Agent Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mHdp, "")
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1HdpD", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetSoccer(this)")
            'Hdp
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", ptMaster)

            TemplateHandler.Assign(soccerTpl, "{chkAutoHdpD}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1HdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(soccerTpl, "{au-hdp-val}", ptAuto)



            'OU
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU_Agent Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOU, "")
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OUD", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")

            TemplateHandler.Assign(soccerTpl, "a-ou-val", aOU)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OUD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAutoOUD}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(soccerTpl, "{au-ou-val}", ptAuto)

            'END

            '1st HDP
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAuto1stHdp_Agent Then
                ptAuto = IIf(isAuto1stHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", m1stHdp, "")
                strChecked = IIf(isAuto1stHdp_Mem, True, False)
            ElseIf isAuto1stHdp_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stHdpD", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")

            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", a1stHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stHdpD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", ptMaster)

            TemplateHandler.Assign(soccerTpl, "{chkAuto1stHdpD}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stHdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAuto1stHdpMax, mAuto1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)

            'END

            '1st OU
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAuto1stOU_Agent Then
                ptAuto = IIf(isAuto1stOU_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", m1stOU, "")
                strChecked = IIf(isAuto1stOU_Mem, True, False)
            ElseIf isAuto1stOU_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stOUD", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")


            TemplateHandler.Assign(soccerTpl, "a-1stou-val", a1stOU)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stOUD")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", ptMaster)

            TemplateHandler.Assign(soccerTpl, "chkAuto1stOUD", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stOUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAuto1stOUMax, mAuto1stOU))
            TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)
            'END

            'OE
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOE_Agent Then
                ptAuto = IIf(isAutoOE_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOE, "")
                strChecked = IIf(isAutoOE_Mem, True, False)
            ElseIf isAutoOE_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OE", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")

            TemplateHandler.Assign(soccerTpl, "a-oe-val", aOE)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", ptMaster)

            TemplateHandler.Assign(soccerTpl, "{chkAutoOE}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
            TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)
            'END

            '1x2
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAuto1x2_Agent Then
                ptAuto = IIf(isAuto1x2_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", m1x2, "")
                strChecked = IIf(isAuto1x2_Mem, True, False)
            ElseIf isAuto1x2_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11x2", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetSoccer(this,'chkCopy1HdpDeadball')")

            TemplateHandler.Assign(soccerTpl, "a-1x2-val", a1x2)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", ptMaster)

            TemplateHandler.Assign(soccerTpl, "{chkAuto1x2}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11x2")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAuto1x2Max, mAuto1x2))
            TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

            'END

            'Correct Score
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoCS_Agent Then
                ptAuto = IIf(isAutoCS_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mCS, "")
                strChecked = IIf(isAutoCS_Mem, True, False)
            ElseIf isAutoCS_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1CS", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")

            TemplateHandler.Assign(soccerTpl, "a-cs-val", aCS)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1CS")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mCSMin, mCSMax, mCS))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAutoCS}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1CS")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoCSMax, mAutoCS))
            TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)
            'END

            'Total Goal
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoTG_Agent Then
                ptAuto = IIf(isAutoTG_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mTG, "")
                strChecked = IIf(isAutoTG_Mem, True, False)
            ElseIf isAutoTG_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1TG", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")

            TemplateHandler.Assign(soccerTpl, "a-tg-val", aTG)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1TG")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mTGMin, mTGMax, mTG))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAutoTG}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1TG")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoTGMax, mAutoTG))
            TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)
            'END

            'Mix Parlay
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoMP_Agent Then
                ptAuto = IIf(isAutoMP_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mMP, "")
                strChecked = IIf(isAutoMP_Mem, True, False)
            ElseIf isAutoMP_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1MP", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")

            TemplateHandler.Assign(soccerTpl, "a-mp-val", aMP)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1MP")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAutoMP}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1MP")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
            TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)
            'END

            'Out right
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOR_Agent Then
                ptAuto = IIf(isAutoOR_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOR, "")
                strChecked = IIf(isAutoOR_Mem, True, False)
            ElseIf isAutoOR_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OR", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")

            TemplateHandler.Assign(soccerTpl, "a-or-val", aOR)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OR")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mORMin, mORMax, mOR))
            TemplateHandler.Assign(soccerTpl, "p-or-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAutoOR}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OR")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoORMax, mAutoOR))
            TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)
            'END

            'Live HDP
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoLHdp_Agent Then
                ptAuto = IIf(isAutoLHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mLHdp, "")
                strChecked = IIf(isAutoLHdp_Mem, True, False)
            ElseIf isAutoLHdp_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1HdpL", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetSoccer(this)")

            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", aLHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAutoHdpL}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1HdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoLHdpMax, mAutoLHdp))
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)
            'END

            'Live OU
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoLOU_Agent Then
                ptAuto = IIf(isAutoLOU_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mLOU, "")
                strChecked = IIf(isAutoLOU_Mem, True, False)
            ElseIf isAutoLOU_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OUL", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpLive')")

            TemplateHandler.Assign(soccerTpl, "a-Lou-val", aLOU)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM1OUL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAutoOUL}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM1OUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoLOUMax, mAutoLOU))
            TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)
            'END

            'Live 1st 
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoL1stHdp_Agent Then
                ptAuto = IIf(isAutoL1stHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mL1stHdp, "")
                strChecked = IIf(isAutoL1stHdp_Mem, True, False)
            ElseIf isAutoL1stHdp_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stHdpL", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpLive')")

            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", aL1stHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stHdpL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAuto1stHdpL}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stHdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoL1stHdpMax, mAutoL1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)
            'END

            'Live 1st OU
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoL1stOU_Agent Then
                ptAuto = IIf(isAutoL1stOU_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mL1stOU, "")
                strChecked = IIf(isAutoL1stOU_Mem, True, False)
            ElseIf isAutoL1stOU_Mem Then
                ptAuto = selectTpl
                pt = selectTpl
                ptMaster = selDisabledTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stOUL", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpLive')")

            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", aL1stOU)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM11stOUL")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
            TemplateHandler.Assign(soccerTpl, "p-L1stou-val", ptMaster)
            TemplateHandler.Assign(soccerTpl, "{chkAuto1stOUL}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM11stOUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoL1stOUMax, mAutoL1stOU))
            TemplateHandler.Assign(soccerTpl, "au-L1stou-val", ptAuto)
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
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            GetTitleCommon(tpl)
            'TemplateHandler.RemoveBlock(tpl, "none-Master")
            'TemplateHandler.RemoveBlock(tpl, "none-Master")
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "sportId", sportTypeId)
            TemplateHandler.Assign(tpl, "master-text", Resources.Language.Master)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            'Kiet Mai - remove mix parlay
            'TemplateHandler.Assign(tpl, "mp-text", Resources.Language.MixParlay)
            TemplateHandler.Assign(tpl, "copy-hdp-row", IIf(Permissions.IsEnabledAutoMaster(isEnabledAutoMaster), 3, 1))
            TemplateHandler.Assign(tpl, "au-presetpt-text", String.Format(Resources.Language.lblAuto, Resources.Language.Master))
            'Kiet Mai - remove mix parlay (change 7 to 6)
            TemplateHandler.Assign(tpl, "title-col", "6")
            TemplateHandler.RemoveBlock(tpl, "no-mixparlay")

            'assign value for position taking           
            Dim aHdp, aOU, aOE, aMP, aML As Double

            Dim mHdpMin, mOUMin, mOEMin, mMPMin, mMLMin As Double
            Dim mHdp, mOU, mOE, mMP, mML As Double
            Dim mHdpMax, mOUMax, mOEMax, mMPMax, mMLMax As Double
            Dim mAutoHdp, mAutoOU, mAutoOE, mAutoMP, mAutoML As Double
            Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMPMax, mAutoMLMax As Double
            '----------------------------------------------PT-----------------Tho Add----------------------------------
            Dim isAutoHdp_Mem, isAutoOU_Mem, isAutoOE_Mem, isAutoMP_Mem, isAutoML_Mem As Double
            Dim isAutoHdp_Agent, isAutoOU_Agent, isAutoOE_Agent, isAutoMP_Agent, isAutoML_Agent As Double
            '/---------------------------------------------------------------------------------------------------------
            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            'Kiet Mai - remove mix parlay
            'mMP = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            '----------Auto--------------
            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_3"))
            mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_2"))
            'Kiet Mai - remove mix parlay
            'mAutoMP = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_9"))
            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))
            'END

            '----------------------------------------------Is Auto PT-----------------Tho Add----------------------------------

            If Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                isAutoHdp_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_3"))
                isAutoOE_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_2"))
                'Kiet Mai - remove mix parlay
                'isAutoMP_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_9"))
                isAutoML_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))

                isAutoHdp_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoOU_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
                isAutoOE_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
                'Kiet Mai - remove mix parlay
                'isAutoMP_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_9"))
                isAutoML_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))
            Else
                isAutoHdp_Mem = False
                isAutoOU_Mem = False
                isAutoOE_Mem = False
                isAutoMP_Mem = False
                isAutoML_Mem = False
            End If


            'END

            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "OU", isAutoOU_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "OE", isAutoOE_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "MP", isAutoMP_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML_Mem)

            'Agent
            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Agent", isAutoHdp_Agent)
            strHidBasket = AssignMultipleHidden(hiddenHash)
            '/---------------------------------------------------------------------------------------------------------
            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
                'Kiet Mai - remove mix parlay
                'aMP = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_9"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                mOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
                mOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
                'Kiet Mai - remove mix parlay
                'mMPMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_9"))
                mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                mOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
                mOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
                'Kiet Mai - remove mix parlay
                'mMPMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_9"))
                mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------    
                mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                mAutoOUMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_3"))
                mAutoOEMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_2"))
                'Kiet Mai - remove mix parlay
                'mAutoMPMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_9"))
                mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))

                '/--------------------------------------------------------------------------------------------------------------
            Else
                mHdpMax = mHdp
                mOUMax = mOU
                mOEMax = mOE
                mMPMax = mMP
                mMLMax = mML
            End If

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Agent, "", btnTpl))

            '----------------------------------------------Auto PT HDP-----------------Tho Add----------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '/--------------------------------------------------------------------------------------------------------------

            'HDP
            pt = selectTpl
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp_Agent Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mHdp, "")
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")

            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            TemplateHandler.Assign(tpl, "{chkAuto" & sportTypeId & "Hdp}", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            If isAutoHdp_Agent Then
                TemplateHandler.Assign(tpl, "{disabled-copyhdp}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
                CopyBasket = IIf(isAutoHdp_Mem, True, False)
            Else
                'If isAutoOU_Mem And isAutoOE_Mem And isAutoMP_Mem And isAutoML_Mem Then
                ' TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
                'End If
                CopyBasket = True
            End If
            'END

            'OU
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU_Agent Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOU, "")
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OU", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            'END

            'OE
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOE_Agent Then
                ptAuto = IIf(isAutoOE_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOE, "")
                strChecked = IIf(isAutoOE_Mem, True, False)
            ElseIf isAutoOE_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OE", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "p-oe-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OE", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            'END

            'Kiet Mai - remove mix parlay
            'Mix Parlay
            'If isMult Then
            '    ptAuto = selDisabledTpl
            '    pt = selDisabledTpl
            '    ptMaster = selDisabledTpl
            '    strChecked = "disabled"
            'ElseIf isAutoMP_Agent Then
            '    ptAuto = IIf(isAutoMP_Mem, selectTpl, selDisabledTpl)
            '    ptMaster = HtmlHelper.GenLabel("", mMP, "")
            '    strChecked = IIf(isAutoMP_Mem, True, False)
            'ElseIf isAutoMP_Mem Then
            '    ptAuto = selectTpl
            '    ptMaster = selDisabledTpl
            '    pt = selectTpl
            '    strChecked = True
            'Else
            '    ptAuto = selDisabledTpl
            '    pt = selectTpl
            '    ptMaster = selectTpl
            '    strChecked = False
            'End If
            'strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "MP", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            'TemplateHandler.Assign(tpl, "a-mp-val", aMP)
            'TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            'TemplateHandler.Assign(tpl, "p-mp-val", ptMaster)
            'TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "MP", strChecked)
            'TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMPMax, mAutoMP))
            'TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)
            'END

            'Money Line
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML_Agent Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mML, "")
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
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
    'sportTypeId: id of sporttype ( 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            GetTitleCommon(tpl)
            'TemplateHandler.RemoveBlock(tpl, "none-Master")
            'TemplateHandler.RemoveBlock(tpl, "none-Master")
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "sportId", sportTypeId)
            TemplateHandler.Assign(tpl, "master-text", Resources.Language.Master)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "copy-hdp-row", IIf(Permissions.IsEnabledAutoMaster(isEnabledAutoMaster), 3, 1))
            TemplateHandler.Assign(tpl, "au-presetpt-text", String.Format(Resources.Language.lblAuto, Resources.Language.Master))
            TemplateHandler.Assign(tpl, "title-col", "6")

            'assign value for position taking           
            Dim aHdp, aOU, aOE, aML As Double

            Dim mHdpMin, mOUMin, mOEMin, mMLMin As Double
            Dim mHdp, mOU, mOE, mML As Double
            Dim mHdpMax, mOUMax, mOEMax, mMLMax As Double

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            Dim mAutoHdp, mAutoOU, mAutoOE, mAutoML As Double
            Dim mAutoHdpMax, mAutoOUMax, mAutoOEMax, mAutoMLMax As Double
            Dim isAutoHdp_Mem, isAutoOU_Mem, isAutoOE_Mem, isAutoML_Mem As Integer
            Dim isAutoHdp_Agent, isAutoOU_Agent, isAutoOE_Agent, isAutoML_Agent As Integer
            '/---------------------------------------------------------------------------------------------------------
            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

            '----------------------------------------------Auto PT---------------------------------------------------
            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_3"))
            mAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_2"))
            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))


            If Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                isAutoHdp_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_3"))
                isAutoOE_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_2"))
                isAutoML_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))

                isAutoHdp_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoOU_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
                isAutoOE_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
                isAutoML_Agent = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))
            Else
                isAutoHdp_Mem = False
                isAutoOU_Mem = False
                isAutoOE_Mem = False
                isAutoML_Mem = False
            End If



            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "OU", isAutoOU_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "OE", isAutoOE_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML_Mem)

            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Agent", isAutoHdp_Agent)
            Select Case sportTypeId
                Case 3
                    strHidFootball = AssignMultipleHidden(hiddenHash)
                Case 5
                    strHidTennis = AssignMultipleHidden(hiddenHash)
                Case 10
                    strHidGolf = AssignMultipleHidden(hiddenHash)
                Case 99
                    strHidOther = AssignMultipleHidden(hiddenHash)
            End Select
            '/---------------------------------------------------------------------------------------------------------


            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                mOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
                mOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
                mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                mOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
                mOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
                mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                '--------- Auto PT Max ---------
                mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                mAutoOUMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_3"))
                mAutoOEMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_2"))
                mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))
                'END
            Else
                mHdpMax = mHdp
                mOUMax = mOU
                mOEMax = mOE
                mMLMax = mML
            End If

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Agent, "", btnTpl))
            '--------Auto PT-------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            'END

            'HDP
            pt = selectTpl
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp_Agent Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mHdp, "")
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")

            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            If isAutoHdp_Agent Then
                'TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
                TemplateHandler.Assign(tpl, "disabled-copyhdp", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If

            'END

            'OU
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU_Agent Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOU, "")
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OU", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            'END

            'OE
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOE_Agent Then
                ptAuto = IIf(isAutoOE_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOE, "")
                strChecked = IIf(isAutoOE_Mem, True, False)
            ElseIf isAutoOE_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OE", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "p-oe-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OE", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOEMax, mAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)
            'END

            'Money Line

            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML_Agent Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mML, "")
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
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
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/baseballMasterOnMember.htm")

            GetTitleCommon(tpl)
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "sportId", sportTypeId)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "copy-hdp-row", IIf(Permissions.IsEnabledAutoMaster(isEnabledAutoMaster), 3, 1))
            TemplateHandler.Assign(tpl, "au-presetpt-text", String.Format(Resources.Language.lblAuto, Resources.Language.Master))
            TemplateHandler.Assign(tpl, "title-col", "5")
            TemplateHandler.Assign(tpl, "cols-a", "5")

            Dim aHdp, aOU, aML As Double
            Dim mHdpMin, mOUMin, mMLMin As Double
            Dim mHdp, mOU, mML As Double
            Dim mHdpMax, mOUMax, mMLMax As Double

            '----------------------------------------------PT-----------------Tho Add----------------------------------
            Dim mAutoHdp, mAutoOU, mAutoML As Double
            Dim mAutoHdpMax, mAutoOUMax, mAutoMLMax As Double
            Dim isAutoHdp_Mem, isAutoOU_Mem, isAutoML_Mem As Integer
            Dim isAutoHdp_Agent, isAutoOU_Agent, isAutoML_Agent As Integer
            '/---------------------------------------------------------------------------------------------------------

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))



            '----------------------------------------------PT-----------------Tho Add----------------------------------

            If Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                isAutoHdp_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_3"))
                isAutoML_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))

                isAutoHdp_Agent = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoOU_Agent = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))
                isAutoML_Agent = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))
            Else
                isAutoHdp_Mem = False
                isAutoOU_Mem = False
                isAutoML_Mem = False
            End If



            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_3"))
            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))

            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "OU", isAutoOU_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML_Mem)

            'Agent
            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Agent", isAutoHdp_Agent)
            strHidBaseball = AssignMultipleHidden(hiddenHash)
            '/---------------------------------------------------------------------------------------------------------

            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                mOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
                mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                mOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
                mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                '-----Auto PT Max ----------
                mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                mAutoOUMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_3"))
                mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))
            Else
                mHdpMax = mHdp
                mOUMax = mOU
                mMLMax = mML

                mAutoHdpMax = mHdpMax 'max u2
                mAutoOUMax = mOUMax
                mAutoMLMax = mMLMax
            End If
            'assign data for Position taking dropdowns          
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Agent, "", btnTpl))
            '----------------------------------------------Auto PT---------------------------------------------------
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            '/--------------------------------------------------------------------------------------------------------------

            'HDP
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp_Agent Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mHdp, "")
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")
            'strChecked = IIf(isAutoHdp_Agent, "", HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')"))

            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            If isAutoHdp_Agent Then
                'TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
                TemplateHandler.Assign(tpl, "{disabled-copyhdp}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If
            'END

            'OU
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoOU_Agent Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mOU, "")
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OU", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "p-ou-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OU", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoOUMax, mAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)
            'END

            'Money Line
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML_Agent Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mML, "")
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
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
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/motoMasterOnMember.htm")
            GetTitleCommon(tpl)
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "copy-hdp-row", IIf(Permissions.IsEnabledAutoMaster(isEnabledAutoMaster), 3, 1))
            TemplateHandler.Assign(tpl, "au-presetpt-text", String.Format(Resources.Language.lblAuto, Resources.Language.Master))
            TemplateHandler.Assign(tpl, "cols-a", "1")
            TemplateHandler.Assign(tpl, "title-col", "4")

            'get data and assign for template Position taking
            Dim aHdp, aML As Double
            Dim mHdpMin, mMLMin As Double
            Dim mHdp, mML As Double
            Dim mHdpMax, mMLMax As Double

            '----------------------------------------------Auto PT---------------------------------------------------
            Dim mAutoHdp, mAutoML As Double
            Dim mAutoHdpMax, mAutoMLMax As Double
            Dim isAutoHdp_Mem, isAutoML_Mem As Integer
            Dim isAutoHdp_Agent, isAutoML_Agent As Integer
            '/---------------------------------------------------------------------------------------------------------

            mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
            mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))



            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            mAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_1"))
            mAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "mpositiontaking_mem_20"))


            If Permissions.IsEnabledAutoMaster(isEnabledAutoMaster) Then
                isAutoHdp_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                isAutoML_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_mem_20"))

                isAutoHdp_Agent = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_1"))
                isAutoML_Agent = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_20"))
            Else
                isAutoHdp_Mem = False
                isAutoML_Mem = False
            End If



            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "ML", isAutoML_Mem)
            'Agent
            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Agent", isAutoHdp_Agent)
            strHidMoto = AssignMultipleHidden(hiddenHash)
            '/---------------------------------------------------------------------------------------------------------
            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                mMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                mHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                mMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                mAutoHdpMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_1"))
                mAutoMLMax = CastAide.AsDouble(dr("maxauto_v" & sportTypeId & "mpositiontaking_mem_20"))
            Else
                mHdpMax = mHdp
                mMLMax = mML
            End If
            'assign data for Position taking dropdowns     
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Agent, "", btnTpl))
            '-------- Autop PT Button
            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)
            'END

            'HDP
            Dim ptMaster As String
            Dim ptAuto As String
            Dim strChecked As String
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoHdp_Agent Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mHdp, "")
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            'strChecked = IIf(isAutoHdp_Agent, "", HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')"))
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "Hdp", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoHdpMax, mAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)
            If isAutoHdp_Agent Then
                TemplateHandler.Assign(tpl, "{disabled-copyhdp}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If
            'END

            'Money Line
            If isMult Then
                ptAuto = selDisabledTpl
                pt = selDisabledTpl
                ptMaster = selDisabledTpl
                strChecked = "disabled"
            ElseIf isAutoML_Agent Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                ptMaster = HtmlHelper.GenLabel("", mML, "")
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                ptAuto = selectTpl
                ptMaster = selDisabledTpl
                pt = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                ptMaster = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(ptMaster, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptMaster, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "p-ml-val", ptMaster)
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoM" & sportTypeId & "ML")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, mAutoMLMax, mAutoML))
            TemplateHandler.Assign(tpl, "au-ml-val", ptAuto)

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataMotoSports = tpl
    End Function
#End Region

#Region "Assign info for NumberGame Sports"
    Function GetDataNumberGame(ByVal dr As DataRow) As String
        Dim numbergame As SportBooksBase = New NumberGameManager()
        TemplateHandler.Assign(bodyTpl, "NumberGame-text", Resources.Language.lblNumberGame)
        Return numbergame.GetMasterOnMemberPtSetting(dr, isEnabledAutoMaster, strHidNumberGame)
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
        tpl = ""
        Dim hiddenHash As New Hashtable
        Try

            Dim aOU As Double
            Dim mOUMin As Double
            Dim mOU As Double
            Dim mOUMax As Double
            'Odd/Even
            Dim aOE As Double
            Dim mOEMin As Double
            Dim mOE As Double
            Dim mOEMax As Double
            Dim isAutoOE As Integer
            'END
            '----------------------------------------------PT-----------------Tho Add----------------------------------
            Dim isAutoOU As Integer
            '/---------------------------------------------------------------------------------------------------------
            mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
            '----------------------------------------------PT-----------------Tho Add----------------------------------
            isAutoOU = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_3"))

            'Get Odd/Even data
            mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
            isAutoOE = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "mpositiontaking_2"))
            'END

            hiddenHash.Add("isAuto_V" & sportTypeId & "OU", isAutoOU)
            'Assign for ODD/EVEN
            hiddenHash.Add("isAuto_V" & sportTypeId & "OE", isAutoOE)
            'END ---------------------------------------------------

            '/---------------------------------------------------------------------------------------------------------
            If Not isMult Then
                'Odd/Even 
                aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
                mOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
                mOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
                'END
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                mOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
                mOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
            Else
                'Odd/Even
                mOEMax = mOE
                'END
                mOUMax = mOU
            End If
            If WebSettings.disabledFI Then
                'hiddenHash.Add("hiddenMOUB201", mOU)
                'hiddenHash.Add("hiddenMOEB201", mOE)
                'strHidFinancial = AssignMultipleHidden(hiddenHash)
                ''tpl = tpl + strHidFinancial
            Else
                strHidFinancial = AssignMultipleHidden(hiddenHash)
                pt = selectTpl
                'TemplateHandler.Assign(tpl, "a-ou-val", aOU)
                'TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
                'TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
                'TemplateHandler.Assign(tpl, "m-ou-val", pt)

                'assign text for position taking
                tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/EditBettypeFC.htm")
                TemplateHandler.Assign(tpl, "sportId", sportTypeId)
                TemplateHandler.Assign(tpl, "cols-a", "1")
                TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
                TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.Agent)
                TemplateHandler.Assign(tpl, "m-presetpt-text", Resources.Language.Master)
                TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
                TemplateHandler.RemoveBlock(tpl, "none_edit_MasterPT")
                'Assign label for Odd/Even PT
                TemplateHandler.Assign(tpl, "copy-ou-rowspan", "2")
                TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven) 'ODD/EVEN label
                TemplateHandler.Assign(tpl, "lblCopyFromBettype", String.Format(Resources.Language.lblCopyFromBettype, Resources.Language.OverUnder)) 'Copy from label
                'END -------------------------------------------------------------------------------


                If isAutoOU Then
                    TemplateHandler.Assign(tpl, "a-ou-val", aOU)
                    TemplateHandler.Assign(tpl, "{m-ou-val}", mOU)
                Else
                    TemplateHandler.Assign(tpl, "a-ou-val", aOU)
                    TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
                    TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
                    TemplateHandler.Assign(tpl, "m-ou-val", pt)
                End If

                Dim btnTpl As String = groupBtn
                'TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "OU")
                'TemplateHandler.Assign(tpl, "p-ou-btn", btnTpl)
                If isAutoOU Then
                    TemplateHandler.Assign(tpl, "m-ou-btn", String.Empty)
                Else
                    TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "OU")
                    TemplateHandler.Assign(tpl, "m-ou-btn", btnTpl)
                End If


                'Assign data for Odd/Even
                pt = selectTpl
                If isAutoOE Then
                    TemplateHandler.Assign(tpl, "a-oe-val", aOE)
                    TemplateHandler.Assign(tpl, "{m-oe-val}", mOE)
                Else
                    TemplateHandler.Assign(tpl, "a-oe-val", aOE)
                    TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
                    TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
                    TemplateHandler.Assign(tpl, "m-oe-val", pt)
                End If

                If isAutoOE Then
                    TemplateHandler.Assign(tpl, "m-oe-btn", String.Empty)
                Else
                    TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "OE")
                    TemplateHandler.Assign(tpl, "m-oe-btn", String.Empty)
                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        GetDataFinancial = tpl
    End Function
#End Region

    Function GetTitleMember(ByRef tpl)

        TemplateHandler.Assign(tpl, "uname-text", Resources.Language.UserName)
        TemplateHandler.Assign(tpl, "fname-text", Resources.Language.FirstName)
        TemplateHandler.Assign(tpl, "lname-text", Resources.Language.LastName)
        TemplateHandler.Assign(tpl, "email-text", Resources.Language.lblFax)
        TemplateHandler.Assign(tpl, "group-text", Resources.Language.lblGroup)
        TemplateHandler.Assign(tpl, "credit-text", Resources.Language.Credit)
        TemplateHandler.Assign(tpl, "phone-text", Resources.Language.Phone)
        TemplateHandler.Assign(tpl, "mobile-text", Resources.Language.Mobile)
        TemplateHandler.Assign(tpl, "status-text", Resources.Language.Status)
        TemplateHandler.Assign(tpl, "closed-text", Resources.Language.lblStatusClosed)

        TemplateHandler.Assign(tpl, "commission-text", Resources.Language.Commission)

        TemplateHandler.Assign(tpl, "a-comm-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "a-commo-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblOtherTypes & " </u>")
        TemplateHandler.Assign(tpl, "comm-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "commo-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblOtherTypes & " </u>")


        TemplateHandler.Assign(tpl, "a-commoOther-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblOther & " </u>")

        TemplateHandler.Assign(tpl, "commoOther-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblOther & " </u>")

        TemplateHandler.Assign(tpl, "minbet-text", Resources.Language.lblMinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Language.lblMaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Language.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "soccer-text", Resources.Language.lblSoccer)
        TemplateHandler.Assign(tpl, "basket-text", Resources.Language.lblBasketball)
        TemplateHandler.Assign(tpl, "football-text", Resources.Language.lblFootball)
        TemplateHandler.Assign(tpl, "tennis-text", Resources.Language.lblTennis)
        TemplateHandler.Assign(tpl, "baseball-text", Resources.Language.lblBaseball)
        TemplateHandler.Assign(tpl, "golf-text", Resources.Language.lblGolf)
        TemplateHandler.Assign(tpl, "moto-text", Resources.Language.lblMotosports)
        TemplateHandler.Assign(tpl, "othersports-text", Resources.Language.lblOthersports)
        TemplateHandler.Assign(tpl, "mixedsports-text", Resources.Language.lblMixedSports)
        TemplateHandler.Assign(tpl, "numbergame-text", Resources.Language.lblNumberGame)
    End Function

#Region "Get Title Common"
    Function GetTitleCommon(ByRef tpl As String)
        TemplateHandler.RemoveBlock(tpl, "no-header")
        TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.Agent)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Language.Master)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblMoneyLine)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Language.CopyFromHdp)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
        TemplateHandler.Assign(tpl, "cols-a", "1")
    End Function
#End Region

#Region "Assign info for Mixed Sports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim mixedSports As MixedSportsManager = New MixedSportsManager()
        GetDataMixedSports = mixedSports.GetMasterEditOnMemberInfo(dr, strHidMixedSport, isEnabledAutoMaster)
    End Function
#End Region

#Region "AssignMultipleHidden"
    'this function is used to assign value into hidden field
    'hash: HashTable contains all value that need to assign hidden field
    'id of hidden field is key of hash table
    'value of hidden field is value of hash table
    Shared Function AssignMultipleHidden(ByVal hash As Hashtable) As String
        Dim html As New StringBuilder
        Try
            Dim hiddenTpl As String = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/HiddenTpl.htm")
            Dim tpl As String = String.Empty
            Dim enumerator As IDictionaryEnumerator = hash.GetEnumerator
            While enumerator.MoveNext()
                tpl = hiddenTpl
                TemplateHandler.Assign(tpl, "hid-id", enumerator.Key())
                TemplateHandler.Assign(tpl, "hid-val", enumerator.Value())
                html.Append(tpl)
            End While
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
        Return html.ToString()
    End Function
#End Region

End Class
