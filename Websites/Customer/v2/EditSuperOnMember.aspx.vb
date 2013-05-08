Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Core.Helper
Imports DNAide.Data

Imports IBCUtils.Encryption
Imports DNAide.Web
Imports DNAide.Core
Imports System.Data
Imports IBCUtils.FormatHandler
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Web
Imports Sunplus.Accounting.Business.Customer
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Extensions

Partial Class Customer_V2_EditSuperOnMember
    Inherits RequiredAuthorizationPage
    Public style As String
    Dim bodyTpl, for4ddlTpl, for3ddlTpl, basketballTpl As String
    Dim groupBtn As String
    Dim selectTpl As String
    Public strUser As String
    Protected m As Integer
    Dim strHidSoccer, strHidBasket, strHidFootball, strHidTennis, strHidFinancial, strMixedSport As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidNumberGame, strHidOther As String

    Dim selDisabledTpl As String
    Dim CopyAll As Boolean = True
    Dim CopyBasket As Boolean = True
    Protected msg As String = String.Empty



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MenuPT.htm")
            groupBtn = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BtnGroup.htm")
            selectTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/dropdown-MemPT.htm")
            for4ddlTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SuperOnMemberFootball.htm")
            basketballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SuperOnMemberBasketball.htm")
            selDisabledTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/ddlDisabledMem.htm")

            saveCustid.Value = Request.QueryString("custId").Replace("-", "")
            strUser = Request.QueryString("uname").Replace("-", "")
            saveAgentid.Value = CustId  'Request.QueryString("agentId")
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

    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.AgentId)
        End Get
    End Property




#Region "Load Information for Single Edit Member"
    Function LoadDataSingle() As String
        Dim strEditContent As New StringBuilder
        Try
            'Dim rs As DataResult
            'rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.Select4Edit.EditSuperOnMemberSel(saveCustid.Value, Request.QueryString("agentid"))
            Dim rs As DbResult
            rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.Select4Edit.EditSuperOnMemberSel(saveCustid.Value, Request.QueryString("agentid"))
            If Not rs Is Nothing Then
                If rs.ResultSet.Tables(0).Rows.Count > 0 Then
                    Dim row As DataRow = rs.ResultSet.Tables(0).Rows(0)
                    strEditContent.Append(GetDataInfo(row, m))
                    CustomerUtils.GetSportTitle(bodyTpl, True)
                    TemplateHandler.RemoveBlock(bodyTpl, "editsuper")
                    TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")
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

                    'mixed sport
                    TemplateHandler.Assign(bodyTpl, "MixedSportsPT", GetDataMixedSports(row))

                    TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(row, 99, for4ddlTpl, m))
                    If Not CopyAll Then
                        'TemplateHandler.RemoveBlock(bodyTpl, "limit-copyAll")
                        TemplateHandler.Assign(bodyTpl, "{disabled-copyall}", "disabled")
                    End If
                    If Not CopyBasket Then
                        TemplateHandler.Assign(bodyTpl, "disabled-copybasketall", "disabled")
                    End If

                    bodyTpl &= strHidSoccer
                    bodyTpl &= strHidBasket
                    bodyTpl &= strHidFootball
                    bodyTpl &= strHidTennis
                    bodyTpl &= strHidBaseball
                    bodyTpl &= strHidGolf
                    bodyTpl &= strHidMoto
                    bodyTpl &= strMixedSport
                    bodyTpl &= strHidNumberGame
                    bodyTpl &= strHidOther
                    strEditContent.Append(bodyTpl)
                Else
                    msg = AlertMessage.GetMsg(Resources.Language.thereisnodata, AlertMessage.MsgType.Error)
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
                TemplateHandler.RemoveBlock(strAccInfo, "none-Single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-Super")
                TemplateHandler.RemoveBlock(strAccInfo, "none-Super")
                GetTitleMember(strAccInfo)
                TemplateHandler.Assign(strAccInfo, "title", "Member: " + CastAide.AsString(dr("username")))
                TemplateHandler.Assign(strAccInfo, "uname-val", CastAide.AsString(dr("username")))
                TemplateHandler.Assign(strAccInfo, "fname-val", CastAide.AsString(dr("firstname")))
                TemplateHandler.Assign(strAccInfo, "lname-val", CastAide.AsString(dr("lastname")))
                TemplateHandler.Assign(strAccInfo, "email-val", CastAide.AsString(dr("fax")))
                TemplateHandler.Assign(strAccInfo, "group-val", CastAide.AsString(dr("ugroup")))
                TemplateHandler.Assign(strAccInfo, "credit-val", DataFormat.FormatNumber1(dr("credit")))
                TemplateHandler.Assign(strAccInfo, "phone-val", CastAide.AsString(dr("phone")))
                TemplateHandler.Assign(strAccInfo, "mobile-val", CastAide.AsString(dr("mobilephone")))
                If CastAide.AsBoolean(dr("closed")) Then
                    TemplateHandler.Assign(strAccInfo, "check-val", "checked")
                Else
                    TemplateHandler.Assign(strAccInfo, "check-val", "")
                End If

                TemplateHandler.Assign(strAccInfo, "ma-comm-val", FormatNumber4(dr("adiscount")))
                TemplateHandler.Assign(strAccInfo, "ma-commo-val", FormatNumber5(dr("adiscountcs")))
                TemplateHandler.Assign(strAccInfo, "a-comm-val", FormatNumber4(dr("discount")))
                TemplateHandler.Assign(strAccInfo, "a-commo-val", FormatNumber5(dr("discountcs")))
                TemplateHandler.Assign(strAccInfo, "comm-val", FormatNumber4(dr("playerdiscount")))
                TemplateHandler.Assign(strAccInfo, "commo-val", FormatNumber5(dr("playerdiscountcs")))

                'Comm
                Dim tplPTData As String = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer\v2\NewPTCommissionData.htm")
                Dim tplPTValue As String = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer\v2\NewPTCommissionValue.htm")

                Dim dataHeader As StringBuilder = New StringBuilder()
                Dim dataHDP_OU_OE As StringBuilder = New StringBuilder()
                Dim data1x2 As StringBuilder = New StringBuilder()
                Dim dataOther As StringBuilder = New StringBuilder()

                Dim valueHeader As String = tplPTValue
                Dim valueBetType As String = tplPTValue

                valueHeader = tplPTValue
                TemplateHandler.Assign(valueHeader, "{value}", HtmlHelper.GenLabel("", String.Format(Resources.Language.lblCommFor, Resources.Language.Master), ""))
                dataHeader.Append(valueHeader)

                valueHeader = tplPTValue
                TemplateHandler.Assign(valueHeader, "{value}", HtmlHelper.GenLabel("", String.Format(Resources.Language.lblCommFor, Resources.Language.Agent), ""))
                dataHeader.Append(valueHeader)

                valueHeader = tplPTValue
                TemplateHandler.Assign(valueHeader, "{value}", HtmlHelper.GenLabel("", String.Format(Resources.Language.lblCommFor, Resources.Language.Member), ""))
                dataHeader.Append(valueHeader)


                'HDP 
                'Master
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("adiscount")), ""))
                dataHDP_OU_OE.Append(valueBetType)
                'Agent
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("discount")), ""))
                dataHDP_OU_OE.Append(valueBetType)
                'Member
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("playerdiscount")), ""))
                dataHDP_OU_OE.Append(valueBetType)

                'Other
                'Master
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("adiscountcs")), ""))
                dataOther.Append(valueBetType)
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
                        'Master
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("adiscount1x2")), ""))
                        data1x2.Append(valueBetType)
                        'Agent
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("discount1x2")), ""))
                        data1x2.Append(valueBetType)
                        'Member
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", FormatNumber4(dr("playerdiscount1x2")), ""))
                        data1x2.Append(valueBetType)
                    Catch ex As Exception
                        LogExtensions.ExceptionHandler(ex)
                        '1x2
                        'Master
                        valueBetType = tplPTValue
                        TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenLabel("", "0", ""))
                        data1x2.Append(valueBetType)
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
                CustomerUtils.GetMinMaxEditValue(strAccInfo, 161, dr)
                MixedSportsManager.GetEditMinMaxValue(dr, strAccInfo)
                Dim comm As NumberGameManager = New NumberGameManager()
                comm.GetSuperOnMemberComm(dr, strAccInfo)

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
            soccerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SuperOnMemberSoccer.htm")
            headerSTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SBettypeHeader.htm")
            If isMult Then
                TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")
                TemplateHandler.RemoveBlock(soccerTpl, "none-Multi")
                TemplateHandler.Assign(soccerTpl, "dead-row", "4")
                TemplateHandler.Assign(soccerTpl, "live-row", "3")
            Else
                TemplateHandler.Assign(soccerTpl, "dead-row", "6")
                TemplateHandler.Assign(soccerTpl, "live-row", "5")
            End If
            TemplateHandler.Assign(soccerTpl, "col-title", "12")
            TemplateHandler.Assign(soccerTpl, "cols-live", "2")
            TemplateHandler.Assign(soccerTpl, "bettype-header", CustomerUtils.LoadSoccerPTHeader(False))
            GetTitleCommon(soccerTpl)
            TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Language.Deadball)
            TemplateHandler.Assign(soccerTpl, "live-text", Resources.Language.Live)
            TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Language.Agent)
            TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Language.Super)
            TemplateHandler.Assign(soccerTpl, "au-presetpt-text", Resources.Language.SuperAuto)
            TemplateHandler.Assign(soccerTpl, "au-preset-live", Resources.Language.SuperAuto)
            TemplateHandler.Assign(soccerTpl, "master-text", Resources.Language.Master)
            TemplateHandler.Assign(soccerTpl, "copy-live-col", "3")
            TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Language.CopyFrom1x2)
            'assign value of Minbet, Maxbet , ...           

            'assign value position taking for template          
            Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
            Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double

            Dim aHdp, aOU, a1stHdp, a1stOU, aOE, a1x2, aCS, aTG, aMP, aOR As Double
            Dim aLHdp, aLOU, aL1stHdp, aL1stOU As Double

            Dim sHdpMin, sOUMin, s1stHdpMin, s1stOUMin, sOEMin, s1x2Min, sCSMin, sTGMin, sMPMin, sORMin As Double
            Dim sLHdpMin, sLOUMin, sL1stHdpMin, sL1stOUMin As Double

            Dim sHdp, sOU, s1stHdp, s1stOU, sOE, s1x2, sCS, sTG, sMP, sOR As Double
            Dim sLHdp, sLOU, sL1stHdp, sL1stOU As Double

            Dim sHdpMax, sOUMax, s1stHdpMax, s1stOUMax, sOEMax, s1x2Max, sCSMax, sTGMax, sMPMax, sORMax As Double
            Dim sLHdpMax, sLOUMax, sL1stHdpMax, sL1stOUMax As Double

            Dim sAutoHdp, sAutoOU, sAuto1stHdp, sAuto1stOU, sAutoOE, sAuto1x2, sAutoCS, sAutoTG, sAutoMP, sAutoOR As Double
            Dim sAutoLHdp, sAutoLOU, sAutoL1stHdp, sAutoL1stOU As Double

            Dim sAutoHdpMax, sAutoOUMax, sAuto1stHdpMax, sAuto1stOUMax, sAutoOEMax, sAuto1x2Max, sAutoCSMax, sAutoTGMax, sAutoMPMax, sAutoORMax As Double
            Dim sAutoLHdpMax, sAutoLOUMax, sAutoL1stHdpMax, sAutoL1stOUMax As Double

            Dim isAutoHdp_Mem, isAutoOU_Mem, isAuto1stHdp_Mem, isAuto1stOU_Mem, isAutoOE_Mem, isAuto1x2_Mem, isAutoCS_Mem, isAutoTG_Mem, isAutoMP_Mem, isAutoOR_Mem As Double
            Dim isAutoLHdp_Mem, isAutoLOU_Mem, isAutoL1stHdp_Mem, isAutoL1stOU_Mem As Double

            'Phuong.Nguyen, Is Auto on Master
            Dim isAutoHdp_Master, isAutoOU_Master, isAuto1stHdp_Master, isAuto1stOU_Master, isAutoOE_Master, isAuto1x2_Master, isAutoCS_Master, isAutoTG_Master, isAutoMP_Master, isAutoOR_Master As Double
            Dim isAutoLHdp_Master, isAutoLOU_Master, isAutoL1stHdp_Master, isAutoL1stOU_Master As Double
            'END

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
                sHdpMin = CastAide.AsDouble(dr("minpos_1"))
                sOUMin = CastAide.AsDouble(dr("minpos_3"))
                s1stHdpMin = CastAide.AsDouble(dr("minpos_7"))
                s1stOUMin = CastAide.AsDouble(dr("minpos_8"))
                sOEMin = CastAide.AsDouble(dr("minpos_2"))
                s1x2Min = CastAide.AsDouble(dr("minpos_5"))
                sCSMin = CastAide.AsDouble(dr("minpos_4"))
                sTGMin = CastAide.AsDouble(dr("minpos_6"))
                sMPMin = CastAide.AsDouble(dr("minpos_9"))
                sORMin = CastAide.AsDouble(dr("minpos_10"))
                sLHdpMin = CastAide.AsDouble(dr("minlpos_1"))
                sLOUMin = CastAide.AsDouble(dr("minlpos_3"))
                sL1stHdpMin = CastAide.AsDouble(dr("minlpos_7"))
                sL1stOUMin = CastAide.AsDouble(dr("minlpos_8"))

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

                sHdpMin = CastAide.AsDouble(dr("minpos_1"))
                sOUMin = CastAide.AsDouble(dr("minpos_3"))
                s1stHdpMin = CastAide.AsDouble(dr("minpos_7"))
                s1stOUMin = CastAide.AsDouble(dr("minpos_8"))
                sOEMin = CastAide.AsDouble(dr("minpos_2"))
                s1x2Min = CastAide.AsDouble(dr("minpos_5"))
                sCSMin = CastAide.AsDouble(dr("minpos_4"))
                sTGMin = CastAide.AsDouble(dr("minpos_6"))
                sMPMin = CastAide.AsDouble(dr("minpos_9"))
                sORMin = CastAide.AsDouble(dr("minpos_10"))
                sLHdpMin = CastAide.AsDouble(dr("minlpos_1"))
                sLOUMin = CastAide.AsDouble(dr("minlpos_3"))
                sL1stHdpMin = CastAide.AsDouble(dr("minlpos_7"))
                sL1stOUMin = CastAide.AsDouble(dr("minlpos_8"))

                sHdpMax = CastAide.AsDouble(dr("maxpos_1"))
                sOUMax = CastAide.AsDouble(dr("maxpos_3"))
                s1stHdpMax = CastAide.AsDouble(dr("maxpos_7"))
                s1stOUMax = CastAide.AsDouble(dr("maxpos_8"))
                sOEMax = CastAide.AsDouble(dr("maxpos_2"))
                s1x2Max = CastAide.AsDouble(dr("maxpos_5"))
                sCSMax = CastAide.AsDouble(dr("maxpos_4"))
                sTGMax = CastAide.AsDouble(dr("maxpos_6"))
                sMPMax = CastAide.AsDouble(dr("maxpos_9"))
                sORMax = CastAide.AsDouble(dr("maxpos_10"))
                sLHdpMax = CastAide.AsDouble(dr("maxlpos_1"))
                sLOUMax = CastAide.AsDouble(dr("maxlpos_3"))
                sL1stHdpMax = CastAide.AsDouble(dr("maxlpos_7"))
                sL1stOUMax = CastAide.AsDouble(dr("maxlpos_8"))

                sAutoHdpMax = CastAide.AsDouble(dr("maxautopos_1"))
                sAutoOUMax = CastAide.AsDouble(dr("maxautopos_3"))
                sAuto1stHdpMax = CastAide.AsDouble(dr("maxautopos_7"))
                sAuto1stOUMax = CastAide.AsDouble(dr("maxautopos_8"))
                sAutoOEMax = CastAide.AsDouble(dr("maxautopos_2"))
                sAuto1x2Max = CastAide.AsDouble(dr("maxautopos_5"))
                sAutoCSMax = CastAide.AsDouble(dr("maxautopos_4"))
                sAutoTGMax = CastAide.AsDouble(dr("maxautopos_6"))
                sAutoMPMax = CastAide.AsDouble(dr("maxautopos_9"))
                sAutoORMax = CastAide.AsDouble(dr("maxautopos_10"))
                sAutoLHdpMax = CastAide.AsDouble(dr("maxautolpos_1"))
                sAutoLOUMax = CastAide.AsDouble(dr("maxautolpos_3"))
                sAutoL1stHdpMax = CastAide.AsDouble(dr("maxautolpos_7"))
                sAutoL1stOUMax = CastAide.AsDouble(dr("maxautolpos_8"))

                isAutoHdp_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_3"))
                isAuto1stHdp_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_7"))
                isAuto1stOU_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_8"))
                isAutoOE_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_2"))
                isAuto1x2_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_5"))
                isAutoCS_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_4"))
                isAutoTG_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_6"))
                isAutoMP_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_9"))
                isAutoOR_Mem = CastAide.AsInt16(dr("isauto_spositiontaking_mem_10"))
                isAutoLHdp_Mem = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_1"))
                isAutoLOU_Mem = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_3"))
                isAutoL1stHdp_Mem = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_7"))
                isAutoL1stOU_Mem = CastAide.AsInt16(dr("isauto_slivepositiontaking_mem_8"))


                isAutoHdp_Master = CastAide.AsInt16(dr("isauto_spositiontaking_1"))
                isAutoOU_Master = CastAide.AsInt16(dr("isauto_spositiontaking_3"))
                isAuto1stHdp_Master = CastAide.AsInt16(dr("isauto_spositiontaking_7"))
                isAuto1stOU_Master = CastAide.AsInt16(dr("isauto_spositiontaking_8"))
                isAutoOE_Master = CastAide.AsInt16(dr("isauto_spositiontaking_2"))
                isAuto1x2_Master = CastAide.AsInt16(dr("isauto_spositiontaking_5"))
                isAutoCS_Master = CastAide.AsInt16(dr("isauto_spositiontaking_4"))
                isAutoTG_Master = CastAide.AsInt16(dr("isauto_spositiontaking_6"))
                isAutoMP_Master = CastAide.AsInt16(dr("isauto_spositiontaking_9"))
                isAutoOR_Master = CastAide.AsInt16(dr("isauto_spositiontaking_10"))
                isAutoLHdp_Master = CastAide.AsInt16(dr("isauto_slivepositiontaking_1"))
                isAutoLOU_Master = CastAide.AsInt16(dr("isauto_slivepositiontaking_3"))
                isAutoL1stHdp_Master = CastAide.AsInt16(dr("isauto_slivepositiontaking_7"))
                isAutoL1stOU_Master = CastAide.AsInt16(dr("isauto_slivepositiontaking_8"))

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
            '''''''''''end script to get max PT value of master that master can be preseted                       
            hiddenHash.Add("isAuto1HdpD", isAutoHdp_Mem)
            hiddenHash.Add("isAuto1OUD", isAutoOU_Mem)
            hiddenHash.Add("isAuto11stHdpD", isAuto1stHdp_Mem)
            hiddenHash.Add("isAuto11stOUD", isAuto1stOU_Mem)
            hiddenHash.Add("isAuto1OE", isAutoOE_Mem)

            hiddenHash.Add("isAuto11x2", isAuto1x2_Mem)
            hiddenHash.Add("isAuto1CS", isAutoCS_Mem)
            hiddenHash.Add("isAuto1TG", isAutoTG_Mem)
            hiddenHash.Add("isAuto1MP", isAutoMP_Mem)
            hiddenHash.Add("isAuto1OR", isAutoOR_Mem)

            hiddenHash.Add("isAuto1HdpL", isAutoLHdp_Mem)
            hiddenHash.Add("isAuto1OUL", isAutoLOU_Mem)
            hiddenHash.Add("isAuto11stHdpL", isAutoL1stHdp_Mem)
            hiddenHash.Add("isAuto11stOUL", isAutoL1stOU_Mem)

            'Agent hidden
            hiddenHash.Add("isAuto1HdpD_Master", isAutoHdp_Master)
            hiddenHash.Add("isAuto11x2_Master", isAuto1x2_Master)
            hiddenHash.Add("isAuto1HdpL_Master", isAutoLHdp_Master)

            strHidSoccer = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign Increase and Reduce button for template
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(soccerTpl, "p-hdp-btn", IIf(isAutoHdp_Master, "", btnTpl))

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(soccerTpl, "p-1x2-btn", IIf(isAuto1x2_Master, "", btnTpl))

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-btn", IIf(isAutoLHdp_Master, "", btnTpl))

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(soccerTpl, "au-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(soccerTpl, "au-1x2-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-btn", btnTpl)


            'Copy All Checkbox
            If isAutoHdp_Master Then
                TemplateHandler.Assign(soccerTpl, "{disabled-copyhdpd}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
                CopyAll = IIf(isAutoHdp_Mem, True, False)
            Else
                CopyAll = True
            End If

            If isAuto1x2_Master Then
                TemplateHandler.Assign(soccerTpl, "{disabled-copy1x2}", IIf(isAuto1x2_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If

            If isAutoLHdp_Master Then
                TemplateHandler.Assign(soccerTpl, "{disabled-copyhdpl}", IIf(isAutoLHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If
            'this script is used to check or uncheck All Auto Checkbox
            'disable or enable Super Preset Position or Auto Super Preset Position

            Dim ptAuto As String
            Dim strChecked As String
            Dim pt As String = selectTpl
            If isAutoHdp_Master Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1HdpD", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetSoccer(this)")

            TemplateHandler.Assign(soccerTpl, "{chkAutoHdpD}", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(soccerTpl, "m-hdp-val", mHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(soccerTpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(soccerTpl, "au-hdp-val", ptAuto)

            'OU
            If isAutoOU_Master Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOU, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OUD", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")

            TemplateHandler.Assign(soccerTpl, "chkAutoOUD", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-ou-val", aOU)
            TemplateHandler.Assign(soccerTpl, "m-ou-val", mOU)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(soccerTpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(soccerTpl, "au-ou-val", ptAuto)

            '1st HDP
            If isAuto1stHdp_Master Then
                ptAuto = IIf(isAuto1stHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", s1stHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAuto1stHdp_Mem, True, False)
            ElseIf isAuto1stHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stHdpD", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")
            TemplateHandler.Assign(soccerTpl, "chkAuto1stHdpD", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", a1stHdp)
            TemplateHandler.Assign(soccerTpl, "m-1sthdp-val", m1stHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1stHdpMin, s1stHdpMax, s1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-1sthdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stHdpMax, sAuto1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-1sthdp-val", ptAuto)
            '1st OU
            If isAuto1stOU_Master Then
                ptAuto = IIf(isAuto1stOU_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", s1stOU, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAuto1stOU_Mem, True, False)
            ElseIf isAuto1stOU_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stOUD", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")
            TemplateHandler.Assign(soccerTpl, "chkAuto1stOUD", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", a1stOU)
            TemplateHandler.Assign(soccerTpl, "m-1stou-val", m1stOU)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1stOUMin, s1stOUMax, s1stOU))
            TemplateHandler.Assign(soccerTpl, "p-1stou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stOUD")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1stOUMax, sAuto1stOU))
            TemplateHandler.Assign(soccerTpl, "au-1stou-val", ptAuto)

            'OE
            If isAutoOE_Master Then
                ptAuto = IIf(isAutoOE_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOE, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOE_Mem, True, False)
            ElseIf isAutoOE_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OE", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpDeadball')")
            TemplateHandler.Assign(soccerTpl, "chkAutoOE", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-oe-val", aOE)
            TemplateHandler.Assign(soccerTpl, "m-oe-val", mOE)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(soccerTpl, "p-oe-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
            TemplateHandler.Assign(soccerTpl, "au-oe-val", ptAuto)

            '1x2
            If isAuto1x2_Master Then
                ptAuto = IIf(isAuto1x2_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", s1x2, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAuto1x2_Mem, True, False)
            ElseIf isAuto1x2_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11x2", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetSoccer(this,'chkCopy1HdpDeadball')")
            TemplateHandler.Assign(soccerTpl, "chkAuto1x2", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", a1x2)
            TemplateHandler.Assign(soccerTpl, "m-1x2-val", m1x2)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(s1x2Min, s1x2Max, s1x2))
            TemplateHandler.Assign(soccerTpl, "p-1x2-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11x2")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAuto1x2Max, sAuto1x2))
            TemplateHandler.Assign(soccerTpl, "au-1x2-val", ptAuto)

            'CS
            If isAutoCS_Master Then
                ptAuto = IIf(isAutoCS_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sCS, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoCS_Mem, True, False)
            ElseIf isAutoCS_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1CS", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")

            TemplateHandler.Assign(soccerTpl, "chkAutoCS", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-cs-val", aCS)
            TemplateHandler.Assign(soccerTpl, "m-cs-val", mCS)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sCSMin, sCSMax, sCS))
            TemplateHandler.Assign(soccerTpl, "p-cs-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1CS")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoCSMax, sAutoCS))
            TemplateHandler.Assign(soccerTpl, "au-cs-val", ptAuto)

            'TG
            If isAutoTG_Master Then
                ptAuto = IIf(isAutoTG_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sTG, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoTG_Mem, True, False)
            ElseIf isAutoTG_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1TG", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")
            TemplateHandler.Assign(soccerTpl, "chkAutoTG", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-tg-val", aTG)
            TemplateHandler.Assign(soccerTpl, "m-tg-val", mTG)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sTGMin, sTGMax, sTG))
            TemplateHandler.Assign(soccerTpl, "p-tg-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1TG")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoTGMax, sAutoTG))
            TemplateHandler.Assign(soccerTpl, "au-tg-val", ptAuto)

            'Mix Parlay
            If isAutoMP_Master Then
                ptAuto = IIf(isAutoMP_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sMP, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoMP_Mem, True, False)
            ElseIf isAutoMP_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1MP", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")
            TemplateHandler.Assign(soccerTpl, "chkAutoMP", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-mp-val", aMP)
            TemplateHandler.Assign(soccerTpl, "m-mp-val", mMP)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMPMin, sMPMax, sMP))
            TemplateHandler.Assign(soccerTpl, "p-mp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1MP")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
            TemplateHandler.Assign(soccerTpl, "au-mp-val", ptAuto)

            'Out Right
            If isAutoOR_Master Then
                ptAuto = IIf(isAutoOR_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOR, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOR_Mem, True, False)
            ElseIf isAutoOR_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OR", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy11x2')")

            TemplateHandler.Assign(soccerTpl, "chkAutoOR", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-or-val", aOR)
            TemplateHandler.Assign(soccerTpl, "m-or-val", mOR)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sORMin, sORMax, sOR))
            TemplateHandler.Assign(soccerTpl, "p-or-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OR")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoORMax, sAutoOR))
            TemplateHandler.Assign(soccerTpl, "au-or-val", ptAuto)

            'Live Hdp
            If isAutoLHdp_Master Then
                ptAuto = IIf(isAutoLHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sLHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoLHdp_Mem, True, False)
            ElseIf isAutoLHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1HdpL", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetSoccer(this)")
            TemplateHandler.Assign(soccerTpl, "chkAutoHdpL", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", aLHdp)
            TemplateHandler.Assign(soccerTpl, "m-Lhdp-val", mLHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sLHdpMin, sLHdpMax, sLHdp))
            TemplateHandler.Assign(soccerTpl, "p-Lhdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1HdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoLHdpMax, sAutoLHdp))
            TemplateHandler.Assign(soccerTpl, "au-Lhdp-val", ptAuto)

            'live OU
            If isAutoLOU_Master Then
                ptAuto = IIf(isAutoLOU_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sLOU, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoLOU_Mem, True, False)
            ElseIf isAutoLOU_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto1OUL", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpLive')")
            TemplateHandler.Assign(soccerTpl, "chkAutoOUL", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", aLOU)
            TemplateHandler.Assign(soccerTpl, "m-Lou-val", mLOU)

            TemplateHandler.Assign(pt, "ddl-Id", "listS1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sLOUMin, sLOUMax, sLOU))
            TemplateHandler.Assign(soccerTpl, "p-Lou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS1OUL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoLOUMax, sAutoLOU))
            TemplateHandler.Assign(soccerTpl, "au-Lou-val", ptAuto)

            '1st Hdp Live
            If isAutoL1stHdp_Master Then
                ptAuto = IIf(isAutoL1stHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sL1stHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoL1stHdp_Mem, True, False)
            ElseIf isAutoL1stHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stHdpL", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpLive')")
            TemplateHandler.Assign(soccerTpl, "chkAuto1stHdpL", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", aL1stHdp)
            TemplateHandler.Assign(soccerTpl, "m-L1stHdp-val", mL1stHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sL1stHdpMin, sL1stHdpMax, sL1stHdp))
            TemplateHandler.Assign(soccerTpl, "p-L1stHdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS11stHdpL")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoL1stHdpMax, sAutoL1stHdp))
            TemplateHandler.Assign(soccerTpl, "au-L1stHdp-val", ptAuto)

            'Live 1st OU
            If isAutoL1stOU_Master Then
                ptAuto = IIf(isAutoL1stOU_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sL1stOU, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoL1stOU_Mem, True, False)
            ElseIf isAutoL1stOU_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto11stOUL", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this,'chkCopy1HdpLive')")
            TemplateHandler.Assign(soccerTpl, "chkAuto1stOUL", strChecked)
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", aL1stOU)
            TemplateHandler.Assign(soccerTpl, "m-L1stou-val", mL1stOU)

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
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 9: Mix Parlay, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball )
    Function GetDataBasketball(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            GetTitleCommon(tpl)
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "sportId", sportTypeId)
            TemplateHandler.Assign(tpl, "cols-a", "2")
            TemplateHandler.Assign(tpl, "master-text", Resources.Language.Master)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            'Kiet Mai - remove mix parlay
            'TemplateHandler.Assign(tpl, "mp-text", Resources.Language.MixParlay)
            TemplateHandler.Assign(tpl, "copy-hdp-col", "3")
            'Kiet Mai - remove mix parlay (change 7 to 6)
            TemplateHandler.Assign(tpl, "title-col", "6")
            TemplateHandler.RemoveBlock(tpl, "no-mixparlay")

            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

            'assign value for position taking           
            Dim mHdp, mOU, mOE, mMP, mML As Double
            Dim aHdp, aOU, aOE, aMP, aML As Double

            Dim sHdpMin, sOUMin, sOEMin, sMPMin, sMLMin As Double
            Dim sHdp, sOU, sOE, sMP, sML As Double
            Dim sHdpMax, sOUMax, sOEMax, sMPMax, sMLMax As Double

            Dim sAutoHdp, sAutoOU, sAutoOE, sAutoMP, sAutoML As Double
            Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMPMax, sAutoMLMax As Double
            Dim isAutoHdp_Mem, isAutoOU_Mem, isAutoOE_Mem, isAutoMP_Mem, isAutoML_Mem As Double
            Dim isAutoHdp_Master, isAutoOU_Master, isAutoOE_Master, isAutoMP_Master, isAutoML_Master As Double

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
            'Kiet Mai - remove mix parlay
            'sMP = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_9"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
            sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
            'Kiet Mai - remove mix parlay
            'sAutoMP = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_9"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
                'Kiet Mai - remove mix parlay
                'aMP = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_9"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                'Kiet Mai - remove mix parlay
                'mMP = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_9"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                sOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
                sOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
                'Kiet Mai - remove mix parlay
                'sMPMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_9"))
                sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                sOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
                sOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
                'Kiet Mai - remove mix parlay
                'sMPMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_9"))
                sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
                sAutoOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_3"))
                sAutoOEMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_2"))
                'Kiet Mai - remove mix parlay
                'sAutoMPMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_9"))
                sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

                isAutoHdp_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_3"))
                isAutoOE_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_2"))
                'Kiet Mai - remove mix parlay
                'isAutoMP_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_9"))
                isAutoML_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))

                isAutoHdp_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoOU_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
                isAutoOE_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
                'Kiet Mai - remove mix parlay
                'isAutoMP_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_9"))
                isAutoML_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_20"))
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
            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "MP", isAutoMP_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML_Mem)

            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Master", isAutoHdp_Master)
            strHidBasket = CustomerUtils.AssignMultipleHidden(hiddenHash)

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Master, "", btnTpl))

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            'Copy from basket 
            If isAutoHdp_Master Then
                TemplateHandler.Assign(tpl, "{disabled-copyhdp}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
                CopyBasket = IIf(isAutoHdp_Mem, True, False)
            Else
                'If isAutoOU_Mem And isAutoOE_Mem And isAutoMP_Mem And isAutoML_Mem Then
                ' TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
                'End If
                CopyBasket = True
            End If

            Dim ptAuto As String
            Dim strChecked As String
            'Hdp
            If isAutoHdp_Master Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "Hdp", strChecked)
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            'OU
            If isAutoOU_Master Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOU, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OU", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OU", strChecked)
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(tpl, "m-ou-val", mOU)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

            'OE
            If isAutoOE_Master Then
                ptAuto = IIf(isAutoOE_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOE, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOE_Mem, True, False)
            ElseIf isAutoOE_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OE", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")

            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OE", strChecked)
            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(tpl, "m-oe-val", mOE)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

            'Kiet Mai - remove mix parlay
            'Mix Parlay
            'If isAutoMP_Master Then
            '    ptAuto = IIf(isAutoMP_Mem, selectTpl, selDisabledTpl)
            '    pt = HtmlHelper.GenLabel("", sMP, "")
            '    'ptMaster = selectTpl
            '    strChecked = IIf(isAutoMP_Mem, True, False)
            'ElseIf isAutoMP_Mem Then
            '    pt = selDisabledTpl
            '    ptAuto = selectTpl
            '    strChecked = True
            'Else
            '    ptAuto = selDisabledTpl
            '    pt = selectTpl
            '    strChecked = False
            'End If
            'strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "MP", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            'TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "MP", strChecked)
            'TemplateHandler.Assign(tpl, "a-mp-val", aMP)
            'TemplateHandler.Assign(tpl, "m-mp-val", mMP)

            'TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "MP")
            'TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sMPMin, sMPMax, sMP))
            'TemplateHandler.Assign(tpl, "p-mp-val", pt)
            'TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "MP")
            'TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoMPMax, sAutoMP))
            'TemplateHandler.Assign(tpl, "au-mp-val", ptAuto)

            'Money line
            If isAutoML_Master Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sML, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "m-ml-val", mML)

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

#Region "Assign info for  Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String, Optional ByVal isMult As Boolean = False) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            GetTitleCommon(tpl)
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "sportId", sportTypeId)
            TemplateHandler.Assign(tpl, "cols-a", "2")
            TemplateHandler.Assign(tpl, "master-text", Resources.Language.Master)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "copy-hdp-col", "3")
            TemplateHandler.Assign(tpl, "title-col", "6")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

            'assign value for position taking           
            Dim mHdp, mOU, mOE, mML As Double
            Dim aHdp, aOU, aOE, aML As Double

            Dim sHdpMin, sOUMin, sOEMin, sMLMin As Double
            Dim sHdp, sOU, sOE, sML As Double
            Dim sHdpMax, sOUMax, sOEMax, sMLMax As Double

            Dim sAutoHdp, sAutoOU, sAutoOE, sAutoML As Double
            Dim sAutoHdpMax, sAutoOUMax, sAutoOEMax, sAutoMLMax As Double
            Dim isAutoHdp_Mem, isAutoOU_Mem, isAutoOE_Mem, isAutoML_Mem As Double
            Dim isAutoHdp_Master, isAutoOU_Master, isAutoOE_Master, isAutoML_Master As Double

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sOE = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_2"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
            sAutoOE = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_2"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))

            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aOE = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_2"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_2"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                sOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
                sOEMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_2"))
                sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                sOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
                sOEMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_2"))
                sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
                sAutoOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_3"))
                sAutoOEMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_2"))
                sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

                isAutoHdp_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_3"))
                isAutoOE_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_2"))
                isAutoML_Mem = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))

                isAutoHdp_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoOU_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
                isAutoOE_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_2"))
                isAutoML_Master = CastAide.AsInt16(dr("isauto_v" & sportTypeId & "spositiontaking_20"))
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
            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "OE", isAutoOE_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Master", isAutoHdp_Master)

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
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Master, "", btnTpl))

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            'Copy from HDP checkbox
            If isAutoHdp_Master Then
                'TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
                TemplateHandler.Assign(tpl, "disabled-copyhdp", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If

            Dim ptAuto As String
            Dim strChecked As String

            'Hdp
            If isAutoHdp_Master Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "Hdp", strChecked)
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            'OU
            If isAutoOU_Master Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOU, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OU", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OU", strChecked)
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(tpl, "m-ou-val", mOU)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

            'OE
            If isAutoOE_Master Then
                ptAuto = IIf(isAutoOE_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOE, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOE_Mem, True, False)
            ElseIf isAutoOE_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OE", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OE", strChecked)
            TemplateHandler.Assign(tpl, "a-oe-val", aOE)
            TemplateHandler.Assign(tpl, "m-oe-val", mOE)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOEMin, sOEMax, sOE))
            TemplateHandler.Assign(tpl, "p-oe-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OE")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOEMax, sAutoOE))
            TemplateHandler.Assign(tpl, "au-oe-val", ptAuto)

            'ML
            If isAutoML_Master Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sML, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                ptAuto = selDisabledTpl
                pt = selectTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "m-ml-val", mML)

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
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SuperOnMemberBaseball.htm")
            GetTitleCommon(tpl)
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "cols-a", "2")
            TemplateHandler.Assign(tpl, "sportId", sportTypeId)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "copy-hdp-col", "3")
            TemplateHandler.Assign(tpl, "title-col", "5")
            TemplateHandler.Assign(tpl, "master-text", Resources.Language.Master)
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "chk1-Id", "chkAuto8Hdp")
            TemplateHandler.Assign(tpl, "chk2-Id", "chkAuto8OU")
            TemplateHandler.Assign(tpl, "chk3-Id", "chkAuto8ML")

            Dim aHdp, aOU, aML As Double
            Dim mHdp, mOU, mML As Double

            Dim sHdpMin, sOUMin, sMLMin As Double
            Dim sHdp, sOU, sML As Double
            Dim sHdpMax, sOUMax, sMLMax As Double

            Dim sAutoHdp, sAutoOU, sAutoML As Double
            Dim sAutoHdpMax, sAutoOUMax, sAutoMLMax As Double
            Dim isAutoHdp_Mem, isAutoOU_Mem, isAutoML_Mem As Double
            Dim isAutoHdp_Master, isAutoOU_Master, isAutoML_Master As Double

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sOU = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_3"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoOU = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_3"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))
            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aOU = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_3"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_3"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                sOUMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_3"))
                sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                sOUMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_3"))
                sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
                sAutoOUMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_3"))
                sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

                isAutoHdp_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
                isAutoOU_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_3"))
                isAutoML_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))

                isAutoHdp_Master = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoOU_Master = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_3"))
                isAutoML_Master = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))
            Else
                sHdpMax = sHdp
                sOUMax = sOU
                sMLMax = sML

                sAutoHdpMax = sAutoHdp
                sAutoOUMax = sAutoOU
                sAutoMLMax = sAutoML
            End If

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "OU", isAutoOU_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Master", isAutoHdp_Master)
            strHidBaseball = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns          
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Master, "", btnTpl))

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            If isAutoHdp_Master Then
                'TemplateHandler.RemoveBlock(tpl, "limit-CopyHdp")
                TemplateHandler.Assign(tpl, "{disabled-copyhdp}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If
            Dim ptAuto As String
            Dim strChecked As String
            'HDP
            If isAutoHdp_Master Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "Hdp", strChecked)
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            'OU
            If isAutoOU_Master Then
                ptAuto = IIf(isAutoOU_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sOU, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoOU_Mem, True, False)
            ElseIf isAutoOU_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "OU", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "OU", strChecked)
            TemplateHandler.Assign(tpl, "a-ou-val", aOU)
            TemplateHandler.Assign(tpl, "m-ou-val", mOU)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sOUMin, sOUMax, sOU))
            TemplateHandler.Assign(tpl, "p-ou-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "OU")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoOUMax, sAutoOU))
            TemplateHandler.Assign(tpl, "au-ou-val", ptAuto)

            'ML
            If isAutoML_Master Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sML, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "m-ml-val", mML)

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
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SuperOnMemberMoto.htm")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            GetTitleCommon(tpl)
            If isMult Then
                TemplateHandler.RemoveBlock(tpl, "none-Multi")
            End If
            TemplateHandler.Assign(tpl, "sportId", sportTypeId)
            TemplateHandler.Assign(tpl, "cols-a", "2")
            TemplateHandler.Assign(tpl, "copy-hdp-col", "3")
            TemplateHandler.Assign(tpl, "title-col", "4")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId & "Hdp")

            'get data and assign for template Position taking
            Dim aHdp, aML As Double
            Dim mHdp, mML As Double

            Dim sHdpMin, sMLMin As Double
            Dim sHdp, sML As Double
            Dim sHdpMax, sMLMax As Double

            Dim sAutoHdp, sAutoML As Double
            Dim sAutoHdpMax, sAutoMLMax As Double
            Dim isAutoHdp_Mem, isAutoML_Mem As Double
            Dim isAutoHdp_Master, isAutoML_Master As Double

            sHdp = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_1"))
            sML = CastAide.AsDouble(dr("v" & sportTypeId & "spositiontaking_20"))

            sAutoHdp = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_1"))
            sAutoML = CastAide.AsDouble(dr("auto_v" & sportTypeId & "spositiontaking_20"))
            If Not isMult Then
                aHdp = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_1"))
                aML = CastAide.AsDouble(dr("v" & sportTypeId & "apositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_1"))
                mML = CastAide.AsDouble(dr("v" & sportTypeId & "mpositiontaking_20"))

                sHdpMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_1"))
                sMLMin = CastAide.AsDouble(dr("minv" & sportTypeId & "pos_20"))

                sHdpMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_1"))
                sMLMax = CastAide.AsDouble(dr("maxv" & sportTypeId & "pos_20"))

                sAutoHdpMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_1"))
                sAutoMLMax = CastAide.AsDouble(dr("maxautov" & sportTypeId & "pos_20"))

                isAutoHdp_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_1"))
                isAutoML_Mem = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_mem_20"))

                isAutoHdp_Master = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_1"))
                isAutoML_Master = CastAide.AsDouble(dr("isauto_v" & sportTypeId & "spositiontaking_20"))
            Else
                sHdpMax = sHdp
                sMLMax = sML

                sAutoHdpMax = sAutoHdp
                sAutoMLMax = sAutoML
            End If

            hiddenHash.Add("isAuto" & sportTypeId & "Hdp", isAutoHdp_Mem)
            hiddenHash.Add("isAuto" & sportTypeId & "ML", isAutoML_Mem)
            hiddenHash.Add("isAuto_V" & sportTypeId & "Hdp_Master", isAutoHdp_Master)
            strHidMoto = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns     
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "p-hdp-btn", IIf(isAutoHdp_Master, "", btnTpl))

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "au-hdp-btn", btnTpl)

            If isAutoHdp_Master Then
                TemplateHandler.Assign(tpl, "{disabled-copyhdp}", IIf(isAutoHdp_Mem, "", Sunplus.Accounting.Core.Common.Constants.ATTR_DISABLED))
            End If
            Dim ptAuto As String
            Dim strChecked As String
            'HDP
            If isAutoHdp_Master Then
                ptAuto = IIf(isAutoHdp_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sHdp, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoHdp_Mem, True, False)
            ElseIf isAutoHdp_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "Hdp", False, CastAide.AsBoolean(strChecked), "CheckAutoPresetOthers(this)")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "Hdp", strChecked)
            TemplateHandler.Assign(tpl, "a-hdp-val", aHdp)
            TemplateHandler.Assign(tpl, "m-hdp-val", mHdp)

            TemplateHandler.Assign(pt, "ddl-Id", "listS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(sHdpMin, sHdpMax, sHdp))
            TemplateHandler.Assign(tpl, "p-hdp-val", pt)
            TemplateHandler.Assign(ptAuto, "ddl-Id", "autoS" & sportTypeId & "Hdp")
            TemplateHandler.Assign(ptAuto, "ddl-val", CustomerUtils.GetListPositionTaking(0, sAutoHdpMax, sAutoHdp))
            TemplateHandler.Assign(tpl, "au-hdp-val", ptAuto)

            'ML
            If isAutoML_Master Then
                ptAuto = IIf(isAutoML_Mem, selectTpl, selDisabledTpl)
                pt = HtmlHelper.GenLabel("", sML, "")
                'ptMaster = selectTpl
                strChecked = IIf(isAutoML_Mem, True, False)
            ElseIf isAutoML_Mem Then
                pt = selDisabledTpl
                ptAuto = selectTpl
                strChecked = True
            Else
                pt = selectTpl
                ptAuto = selDisabledTpl
                strChecked = False
            End If
            strChecked = HtmlHelper.GenCheckBox("chkAuto" & sportTypeId & "ML", False, CastAide.AsBoolean(strChecked), "CheckAutoPreset(this, 'chkCopy" & sportTypeId & "Hdp')")
            TemplateHandler.Assign(tpl, "chkAuto" & sportTypeId & "ML", strChecked)
            TemplateHandler.Assign(tpl, "a-ml-val", aML)
            TemplateHandler.Assign(tpl, "m-ml-val", mML)

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
    'this function is used to assign data into template for MotoSports
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (11: motosports)
    'Motorsports have only Handicap & Money Line bettype
    Function GetDataNumberGame(ByVal dr As DataRow) As String
        TemplateHandler.Assign(bodyTpl, "NumberGame-text", Resources.Language.lblNumberGame)
        Dim numbergame As SportBooksBase = New NumberGameManager()
        Return numbergame.GetSuperOnMemberPtSetting(dr, strHidNumberGame)
    End Function
#End Region

#Region "Assign info for Mixedsports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim mixedSport As MixedSportsManager = New MixedSportsManager()
        GetDataMixedSports = mixedSport.GetSuperEditOnMemberInfo(dr, strMixedSport)
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
        'TemplateHandler.Assign(tpl, "commissionHR-text", Resources.Language.CommissionHR)
        'TemplateHandler.Assign(tpl, "commissionFC-text", Resources.Language.CommissionFinancial)

        'TemplateHandler.Assign(tpl, "commissionHR-text", Resources.Language.CommissionHR)
        'TemplateHandler.Assign(tpl, "a-commUK-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblUK & " </u>")
        'TemplateHandler.Assign(tpl, "a-commoOther-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblOther & " </u>")
        'TemplateHandler.Assign(tpl, "commUK-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblUK & " </u>")
        'TemplateHandler.Assign(tpl, "commoOther-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblOther & " </u>")

        TemplateHandler.Assign(tpl, "commissionFC-text", Resources.Language.CommissionFinancial)
        TemplateHandler.Assign(tpl, "a-commFC-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblFinancial & " </u>")
        TemplateHandler.Assign(tpl, "ma-commFC-text", Resources.Language.CommissionForMaster & " <u>" & Resources.Language.lblFinancial & " </u>")
        TemplateHandler.Assign(tpl, "commFC-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblFinancial & " </u>")

        TemplateHandler.Assign(tpl, "horseracing-text", Resources.Language.lblHorseRacing)

        TemplateHandler.Assign(tpl, "ma-comm-text", Resources.Language.CommissionForMaster & " <u>" & Resources.Language.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "ma-commo-text", Resources.Language.CommissionForMaster & " <u>" & Resources.Language.lblOtherTypes & " </u>")
        TemplateHandler.Assign(tpl, "a-comm-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "a-commo-text", Resources.Language.CommissionForAgent & " <u>" & Resources.Language.lblOtherTypes & " </u>")

        TemplateHandler.Assign(tpl, "comm-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblAsianHDC_OU_OE & " </u>")
        TemplateHandler.Assign(tpl, "commo-text", Resources.Language.CommissionForMember & " <u>" & Resources.Language.lblOtherTypes & " </u>")

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
        TemplateHandler.Assign(tpl, "financial-text", Resources.Language.lblFinancial)
        TemplateHandler.Assign(tpl, "mixedsports-text", Resources.Language.lblMixedSports)
        TemplateHandler.Assign(tpl, "numbergame-text", Resources.Language.lblNumberGame)
    End Function

#Region "Get Title Common"
    Function GetTitleCommon(ByRef tpl As String)
        TemplateHandler.RemoveBlock(tpl, "no-header")
        TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
        TemplateHandler.Assign(tpl, "au-presetpt-text", Resources.Language.SuperAuto)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.Agent)

        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Language.Super)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblMoneyLine)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Language.CopyFromHdp)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "3")
        TemplateHandler.Assign(tpl, "master-text", Resources.Language.Master)
    End Function
#End Region
End Class
