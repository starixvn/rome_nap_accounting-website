'Imports IBCDB
Imports System.Resources
Imports Sunplus.Accounting.Common
Imports DNAide.Web
Imports System.Text
Imports DNAide.Core
Imports System.Data.SqlClient
Imports System.Data

Imports IBCUtils.DAL
Imports IBCDLL.Customer
Imports IBCUtils.Web
Imports IBCUtils
Imports DNAide.Data
Imports BLL.Customer
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.DataAccess.IBCDLL.Customer
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Web
Imports Sunplus.Accounting.Business.Customer
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Core.Extensions



Public Class EditCustomerNew
    Inherits RequiredAuthorizationPage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub



#End Region

    Public username As String = String.Empty
    Protected maxPerMatchDisp As String = String.Empty
    Private array_custid As String
    Public CID As String = "", agentID As String = "", formset = ""
    Dim bodyTpl, strAccInfo, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl, groupBtn, selectTpl, strHidSoccer As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidOther, strHidBasket, strHidFootball, strHidTennis, strHidVariable As String
    Public script As New StringBuilder
    Protected groupa As String = String.Empty
    Protected groupb As String = String.Empty
    Protected groupc As String = String.Empty
    Protected groupd As String = String.Empty
    Public AllowUdpPT As String = "0", Mult = "0"
    Protected msg As String = String.Empty

    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.AgentId)
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Dim spName As String = "acc_CustomerSelEditCustomer"
        Try

            If Not IsPostBack Then
                If (Request.QueryString("m") = "0") Then ' edit single
                    script.Append("<script type=""text/javascript""> var isMult=0;</script>")
                    CID = Request.QueryString("custId")
                    agentID = Request.QueryString("agentId")
                    Mult = "0"
                    formset = Request("set")
                    Page.Title = Request("uname")
                End If
            End If
            'If (Not Request("msg") Is Nothing) Then
            '    msg = Request("msg")
            '    If (msg <> "") Then
            '        msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Success)
            '    End If
            '    script.Append("<script type=""text/javascript""> ShowError('" & msg & "', 'err')</script>")
            'End If
            Dim rs As DbResult
            Dim ht As New Hashtable
            ht.Add("custid", CID)
            rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerSelEditCustomer(ht)
            Dim dt As DataTable = rs.ResultSet.Tables(0)
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                bodyTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MenuPT.htm")
                strAccInfo = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/AccInfo.htm")
                groupBtn = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BtnGroup.htm")
                selectTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/dropdown-MemPT.htm")
                tennisTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/TennisPT.htm")
                basketballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BasketballPT.htm")
                footballTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/FootballPT.htm")
                otherSportsTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/OtherSportsPT.htm")
                golfTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/GolfPT.htm")
                TemplateHandler.RemoveBlock(selectTpl, "none-member")
                CustomerUtils.GetSportTitle(bodyTpl, False)
                If Mult = "1" Then
                    TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                    TemplateHandler.RemoveBlock(bodyTpl, "none-Mult")
                End If

                ' Casino
                If (WebSettings.disableCS) Then
                    TemplateHandler.RemoveBlock(bodyTpl, "casino")
                Else
                    TemplateHandler.Assign(bodyTpl, "{hidSynCS}", dr("synccasino").ToString())
                End If


                TemplateHandler.Assign(bodyTpl, "add-text", Resources.Language.Submit)
                TemplateHandler.Assign(bodyTpl, "add-id", "btnSubmit")
                TemplateHandler.Assign(bodyTpl, "copy-all-text", Resources.Language.lblCopyAllPTfromHdp)
                TemplateHandler.Assign(bodyTpl, "copy-basket-text", Resources.Language.lblCopyBasketHdp)
                TemplateHandler.Assign(bodyTpl, "accinfo", AssignDataInfo(dr))
                TemplateHandler.Assign(bodyTpl, "soccerPT", GetDataSoccer(dr))
                TemplateHandler.Assign(bodyTpl, "basketPT", GetData4DDl(dr, 2, basketballTpl))
                TemplateHandler.Assign(bodyTpl, "footballPT", GetData4DDl(dr, 3, footballTpl))
                TemplateHandler.Assign(bodyTpl, "tennisPT", GetData4DDl(dr, 5, tennisTpl))
                TemplateHandler.Assign(bodyTpl, "baseballPT", GetDataBaseball(dr, 8))
                TemplateHandler.Assign(bodyTpl, "golfPT", GetData4DDl(dr, 10, golfTpl))
                TemplateHandler.Assign(bodyTpl, "motoPT", GetDataMotoSports(dr, 11))

                Dim numbergame As SportBooksBase = New NumberGameManager()
                numbergame.GetNumberCom(dr, 1, False, bodyTpl)

                If Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled Then
                    TemplateHandler.Assign(bodyTpl, "NumberGamePT", GetDataNumberGame(dr))
                Else
                    TemplateHandler.RemoveBlock(bodyTpl, "NumberGame")
                End If


                TemplateHandler.Assign(bodyTpl, "MixedSportsPT", GetDataMixedSports(dr))

                TemplateHandler.Assign(bodyTpl, "otherPT", GetData4DDl(dr, 99, otherSportsTpl))
                TemplateHandler.RemoveBlock(bodyTpl, "none-edit-Min")



                htmlEditMemberContent.Text = VariableScript() + Environment.NewLine + bodyTpl
                'content.Append(strHidVariable)

            Else
                script.Append("<script type=""text/javascript""> ShowError('" & AlertMessage.GetMsg(Resources.Language.thereisnodata, AlertMessage.MsgType.Error) & "', 'err')</script>")
            End If
            script.Append("<script type=""text/javascript""> var AllowUdpPT = " + AllowUdpPT + ";</script>")
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub

    '------------------------------------------------
#Region "Declare javascript parameter"
    Function VariableScript() As String
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

        Dim lblCreditInvalid As String
        lblCreditInvalid = Resources.Language.lblCreditInvalid.Replace("{0}", Resources.Language.lblMemberMaxCredit)

        If (Mult = "0") Then
            script += " var alertUserName='" & Resources.Language.alertUserName & "';" & vbNewLine
            script += " var lblPasswordRequire='" & Resources.Language.lblPasswordRequire & "';" & vbNewLine
            script += " var alertPasswordEmpty='" & Resources.Language.alertPasswordEmpty & "';" & vbNewLine
            script += " var alertIncorrectCredit='" & Resources.Language.alertIncorrectCredit & "';" & vbNewLine
            script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        End If

        Dim strAlertMinPT As String = Resources.Language.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Language.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Language.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "function SetValueMemMaxCredit(){}"
        script += "</script>"
        Return script
        'Response.Write(script)

    End Function
#End Region

#Region "Load AccInfoMem template"
    Function AssignDataInfo(ByVal dr As DataRow) As String
        Try
            If Mult = "1" Then ' edit multi
                'TemplateHandler.Assign(strAccInfo, "title", "<span id ='spUserList'></span>")
            Else
                Dim nickname As String = CastAide.AsString(dr("username2"), String.Empty)
                If nickname.Length > 0 Then
                    nickname = " [" + nickname + "]"
                End If
                TemplateHandler.Assign(strAccInfo, "title", "Member: " & Request("uname") + nickname)
            End If
            TemplateHandler.Assign(strAccInfo, "max-credit-text", Resources.Language.lblMemberMaxCredit)

            TemplateHandler.RemoveBlock(strAccInfo, "none-super")
            TemplateHandler.RemoveBlock(strAccInfo, "none-text-user")
            TemplateHandler.RemoveBlock(strAccInfo, "none-edit")
            TemplateHandler.RemoveBlock(strAccInfo, "Member-Max-Credit")
            TemplateHandler.RemoveBlock(strAccInfo, "colspan")


            '
            TemplateHandler.Assign(strAccInfo, "tranfer-time", "")
            If Mult = "1" Then ' mult
                TemplateHandler.RemoveBlock(strAccInfo, "none-mult")
                TemplateHandler.RemoveBlock(strAccInfo, "none-show-info-user-when-edit-mult")
                TemplateHandler.RemoveBlock(strAccInfo, "none-edit-member")
                TemplateHandler.Assign(strAccInfo, "cols-untext", "5")
                Dim str As String = "<input type=""checkbox"" id=""chkUpdStaus"" onclick=""AllowUpdate(this,false);"" name=""chkUpdStaus""/>" + Resources.Language.Status
                TemplateHandler.Assign(strAccInfo, "status-text", str)
                TemplateHandler.Assign(strAccInfo, "colspan", "colspan='2'")
            Else 'single
                TemplateHandler.RemoveBlock(strAccInfo, "none-HRdisable")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-single")
                TemplateHandler.RemoveBlock(strAccInfo, "none-cell-member")
                TemplateHandler.Assign(strAccInfo, "colspan", "")
                TemplateHandler.Assign(strAccInfo, "cols-untext", "4")
                TemplateHandler.Assign(strAccInfo, "fname-val", AccCommon.ReplaceData(CastAide.AsString(dr("firstname"))))
                TemplateHandler.Assign(strAccInfo, "lname-val", AccCommon.ReplaceData(CastAide.AsString(dr("lastname"))))
                TemplateHandler.Assign(strAccInfo, "phone-val", AccCommon.ReplaceData(CastAide.AsString(dr("phone"))))
                TemplateHandler.Assign(strAccInfo, "mobile-val", AccCommon.ReplaceData(CastAide.AsString(dr("mobilephone"))))
                ' display value , limits of credit
                TemplateHandler.Assign(strAccInfo, "credit-val", DataFormat.FormatNumber1(dr("credit")))
                CustomerUtils.GetCredit(dr, strAccInfo, False)

                TemplateHandler.Assign(strAccInfo, "fax-val", dr("fax"))
                TemplateHandler.Assign(strAccInfo, "UGroup", dr("ugroup"))

                Dim isSuspended As Boolean = CastAide.AsBoolean(dr("suspended"))
                TemplateHandler.Assign(strAccInfo, "chkSuspended", HtmlHelper.GenCheckBox("chkSuspended", False, isSuspended, "", ""))

                Dim isClosed As Boolean = CastAide.AsBoolean(dr("closed"))
                TemplateHandler.Assign(strAccInfo, "chkClosed", HtmlHelper.GenCheckBox("chkClosed", False, isClosed, "", ""))



            End If

            Dim discount As Double = CastAide.AsDouble(dr("discount"))
            Dim discountcs As Double = CastAide.AsDouble(dr("discountcs"))
            Dim discount1x2 As Double = CastAide.AsDouble(dr("discount1x2"))
            Dim playerdiscount As Double = CastAide.AsDouble(dr("playerdiscount"))
            Dim playerdiscountcs As Double = CastAide.AsDouble(dr("playerdiscountcs"))
            Dim playerdiscount1x2 As Double = CastAide.AsDouble(dr("playerdiscount1x2"))


            '' 1x2 comm
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
            TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenInput(Globals.InputType.Text, "txtDiscount", discount.ToString(), "selectcomm", True, False, ""))
            dataHDP_OU_OE.Append(valueBetType)
            'Member
            valueBetType = tplPTValue
            TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenSelectOption(discount, playerdiscount, Globals.INTERVAL_COMM, False, "ddrCommissionH_OU_OE", "", "selectcomm"))
            dataHDP_OU_OE.Append(valueBetType)

            'Other
            'Agent
            valueBetType = tplPTValue
            TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenInput(Globals.InputType.Text, "txtDiscountcs", discountcs.ToString(), "selectcomm", True, False, ""))
            dataOther.Append(valueBetType)
            'Member
            valueBetType = tplPTValue
            TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenSelectOption(discountcs, playerdiscountcs, Globals.INTERVAL_COMM, False, "ddrCommissionCS_TG", "", "selectcomm"))
            dataOther.Append(valueBetType)

            If (WebSettings.Disable1x2Comm) Then
                TemplateHandler.RemoveBlock(tplPTData, "1x2")
            Else

                '1x2
                'Agent
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenInput(Globals.InputType.Text, "txtDiscountcs1x2", discount1x2.ToString(), "selectcomm", True, False, ""))
                data1x2.Append(valueBetType)
                'Member
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenSelectOption(discount1x2, playerdiscount1x2, Globals.INTERVAL_COMM, False, "ddrCommissionCS_1x2", "", "selectcomm"))
                data1x2.Append(valueBetType)
            End If



            TemplateHandler.Assign(tplPTData, "{header_value}", dataHeader.ToString())
            TemplateHandler.Assign(tplPTData, "{hdpouoe_value}", dataHDP_OU_OE.ToString())
            TemplateHandler.Assign(tplPTData, "{1x2_value}", data1x2.ToString())
            TemplateHandler.Assign(tplPTData, "{other_value}", dataOther.ToString())

            TemplateHandler.Assign(tplPTData, "{lblHDPOUOE}", Resources.Language.lblHDPOUOE)
            TemplateHandler.Assign(tplPTData, "{lbl1x2}", Resources.Language.lbl1x2)
            TemplateHandler.Assign(tplPTData, "{lblother}", Resources.Language.lblOtherBettypes)

            TemplateHandler.Assign(strAccInfo, "{NewCommission}", tplPTData)
            '' endregion


            TemplateHandler.Assign(strAccInfo, "discount-val", CStr(discount))
            TemplateHandler.Assign(strAccInfo, "discountcs-val", CStr(discountcs))

            TemplateHandler.Assign(strAccInfo, "group-text", Resources.Language.lblGroup)
            TemplateHandler.Assign(strAccInfo, "lblCommissionForAgent", Resources.Language.CommissionForAgent)
            TemplateHandler.Assign(strAccInfo, "lblAsianHDC_OU_OE", Resources.Language.lblAsianHDC_OU_OE)
            TemplateHandler.Assign(strAccInfo, "lbl1x2_CS_TG", Resources.Language.lblOtherTypes)

            'TemplateHandler.Assign(strAccInfo, "lblAsianFC", Resources.Language.lblFinancial)
            'TemplateHandler.Assign(strAccInfo, "discountcsFC-text", Resources.Language.lblFinancial)
            '
            TemplateHandler.Assign(strAccInfo, "lblCommissionForMember", Resources.Language.CommissionForMember)
            TemplateHandler.Assign(strAccInfo, "CommissionH_OU_OE", AssignData4Commission(dr, "ddrCommissionH_OU_OE"))
            TemplateHandler.Assign(strAccInfo, "CommissionCS_TG", AssignData4Commission(dr, "ddrCommissionCS_TG"))

            CustomerUtils.GetTitleInfo(strAccInfo, IIf(Mult = "1", True, False))


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


            Return strAccInfo
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function
#End Region

#Region "AssignData4Commission(ByVal dr As DataRow, ByVal ddl As String)"
    Function AssignData4Commission(ByVal dr As DataRow, ByVal ddl As String) As String
        Dim d As Integer = 4
        Dim csm As Double = 0
        Dim str As String = "<SELECT class='selectcomm' id='" + ddl + "' name='" + ddl + "'>"
        If (ddl = "ddrCommissionH_OU_OE") Then
            If Mult = "1" Then
                'csm = 0.01
                csm = CDbl(FormatNumber(dr("groupa"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscount"), "0.####") + "' selected='true'>" + Format(dr("playerdiscount"), "0.####") + "</option>"
                csm = CDbl(dr("discount"))
            End If
        ElseIf ddl = "ddrCommissionFC" Then
            d = 5
            If (Mult = "1") Then
                'csm = 0.05
                csm = CDbl(FormatNumber(dr("discountcsFinancial"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscountcsFinancial"), "0.#####") + "' selected='true'>" + Format(dr("playerdiscountcsFinancial"), "0.#####") + "</option>"
                csm = CDbl(dr("discountcsFinancial"))
            End If
        ElseIf ddl = "ddrCommissionUK" Then
            d = 5
            If (Mult = "1") Then
                'csm = 0.05
                csm = CDbl(FormatNumber(dr("discountcsUK"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscountcsUK"), "0.#####") + "' selected='true'>" + Format(dr("playerdiscountcsUK"), "0.#####") + "</option>"
                csm = CDbl(dr("discountcsUK"))
            End If
        ElseIf ddl = "ddrCommissionOther" Then
            d = 5
            If (Mult = "1") Then
                'csm = 0.05
                csm = CDbl(FormatNumber(dr("discountcsOther"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscountcsOther"), "0.#####") + "' selected='true'>" + Format(dr("playerdiscountcsOther"), "0.#####") + "</option>"
                csm = CDbl(dr("discountcsOther"))
            End If
        Else
            d = 5
            If (Mult = "1") Then
                'csm = 0.05
                csm = CDbl(FormatNumber(dr("discountcs"), d))
            Else
                str += "<option value='" + Format(dr("playerdiscountcs"), "0.#####") + "' selected='true'>" + Format(dr("playerdiscountcs"), "0.#####") + "</option>"
                csm = CDbl(dr("discountcs"))
            End If
        End If
        Dim index As Integer = 0
        While CDbl(FormatNumber(csm, d)) > 0
            If (d = 4) Then
                str += "<option value='" + Format(csm, "0.####") + "'" + IIf(Mult = "1" And index = 0, "selected='true'", "") + ">" + Format(csm, "0.####") + "</option>"
                csm = csm - 0.0005
            Else
                str += "<option value='" + Format(csm, "0.#####") + "'" + IIf(Mult = "1" And index = 0, "selected='true'", "") + ">" + Format(csm, "0.#####") + "</option>"
                csm = csm - 0.0005
            End If
            index = index + 1
        End While
        str += "<option value='0'>0</option>"
        str += "</select>"
        Return str
    End Function
#End Region

#Region "AssignData4Group(ByVal dr As DataRow)"
    Function AssignData4Group() As String
        Dim str As String = "<select id = 'ddlUGruop' name ='ddlUGruop'onchange='javascript:updateCommission(this.form);' >"
        Dim ht2 As New Hashtable
        'Dim cmd As CommandPool = Application("CommandPool")
        ht2.Add("custid", Session("CustID"))
        Dim dt As DataTable = Select4Edit.CustomerSelCustomerInfo(ht2).ResultSet.Tables(0)

        str += "<option value='a'>a</option>"
        str += "<option value='b'>b</option>"
        str += "<option value='c'>c</option>"
        str += "<option value='d'>d</option>"
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item("site").ToString() = "IBSBET" Then
                str += "<option value='indoa'>indoa</option>"
                str += "<option value='indob'>indob</option>"
                str += "<option value='indoc'>indoc</option>"
                str += "<option value='indod'>indod</option>"
            Else
                str += "<option value='hka'>hka</option>"
                str += "<option value='hkb'>hkb</option>"
                str += "<option value='hkc'>hkc</option>"
                str += "<option value='hkd'>hkd</option>"
            End If
        End If
        str += "</select>"
        Return str
    End Function
#End Region

#Region "Assign Info for Soccer"
    'this function is used to assign data into template for Soccer
    'dr: row contains data get from db
    Function GetDataSoccer(ByVal dr As DataRow) As String
        Dim soccerTpl As String = String.Empty
        Dim headerSTpl As String = String.Empty
        Dim hiddenHash As New Hashtable
        Try
            soccerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SoccerPT.htm")
            headerSTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/SBettypeHeader.htm")
            ' assign all text according language
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "none-member")
            TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
            TemplateHandler.RemoveBlock(soccerTpl, "auto-preset")
            TemplateHandler.RemoveBlock(soccerTpl, "none-edit")

            TemplateHandler.Assign(soccerTpl, "bettype-header", CustomerUtils.LoadSoccerPTHeader(True))

            Dim str As String = "<input type=""checkbox"" id=""chkUpdSoccerPT"" onclick=""AllowUpdate(this,false);"" name=""chkUpdSoccerPT""/>" + Resources.Language.PositionTaking
            If Mult = "0" Then
                str = Resources.Language.PositionTaking
            End If
            TemplateHandler.Assign(soccerTpl, "pt-text", str)

            str = "<input type=""checkbox"" id=""chkUpdSoccerBet"" onclick=""AllowUpdate(this,false);"" name=""chkUpdSoccerBet""/>" + Resources.Language.lblBetSetting
            If Mult = "0" Then
                str = Resources.Language.lblBetSetting
            End If
            TemplateHandler.Assign(soccerTpl, "bet-text", str)

            GetTitleCommon(soccerTpl)

            TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Language.Deadball)
            TemplateHandler.Assign(soccerTpl, "live-text", Resources.Language.Live)
            TemplateHandler.Assign(soccerTpl, "a-preset-live", Resources.Language.lblMasterPresetPosition)
            TemplateHandler.Assign(soccerTpl, "p-preset-live", Resources.Language.lblSuperPresetPosition)
            TemplateHandler.Assign(soccerTpl, "auto-super-text", Resources.Language.lblAutoSuperPresetPosition)
            TemplateHandler.Assign(soccerTpl, "dead-row", "2")
            TemplateHandler.Assign(soccerTpl, "live-row", "1")
            TemplateHandler.Assign(soccerTpl, "copy-live-col", "1")
            TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Language.CopyFrom1x2)
            TemplateHandler.Assign(soccerTpl, "col-title", 11)
            TemplateHandler.Assign(soccerTpl, "col-auto", 9)

            'assign value of Minbet, Maxbet , ...

            CustomerUtils.GetLimitMinMaxBet(dr, soccerTpl, "")

            'assign value position taking for template
            Dim mHdp, mOU, m1stHdp, m1stOU, mOE, m1x2, mCS, mTG, mMP, mOR As Double
            Dim mLHdp, mLOU, mL1stHdp, mL1stOU As Double
            Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
            Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

            Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
            Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

            mHdp = CastAide.AsDouble(dr("apositiontaking_1"))
            mOU = CastAide.AsDouble(dr("apositiontaking_3"))
            m1stHdp = CastAide.AsDouble(dr("apositiontaking_7"))
            m1stOU = CastAide.AsDouble(dr("apositiontaking_8"))
            mOE = CastAide.AsDouble(dr("apositiontaking_2"))
            m1x2 = CastAide.AsDouble(dr("apositiontaking_5"))
            mCS = CastAide.AsDouble(dr("apositiontaking_4"))
            mTG = CastAide.AsDouble(dr("apositiontaking_6"))
            mMP = CastAide.AsDouble(dr("apositiontaking_9"))
            mOR = CastAide.AsDouble(dr("apositiontaking_10"))
            mLHdp = CastAide.AsDouble(dr("alivepositiontaking_1"))
            mLOU = CastAide.AsDouble(dr("alivepositiontaking_3"))
            mL1stHdp = CastAide.AsDouble(dr("alivepositiontaking_7"))
            mL1stOU = CastAide.AsDouble(dr("alivepositiontaking_8"))

            If Mult = "0" Then ' single
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

                TemplateHandler.Assign(soccerTpl, "minbet1-val", FormatNumber(dr("minbet"), 0))
                TemplateHandler.Assign(soccerTpl, "maxbet1-val", FormatNumber(dr("maxbet"), 0))
                TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", FormatNumber(dr("maxpermatch"), 0))

            Else 'mult
                mHdpMin = CastAide.AsDouble(dr("apositiontaking_1_min"))
                mOUMin = CastAide.AsDouble(dr("apositiontaking_3_min"))
                m1stHdpMin = CastAide.AsDouble(dr("apositiontaking_7_min"))
                m1stOUMin = CastAide.AsDouble(dr("apositiontaking_8_min"))
                mOEMin = CastAide.AsDouble(dr("apositiontaking_2_min"))
                m1x2Min = CastAide.AsDouble(dr("apositiontaking_5_min"))
                mCSMin = CastAide.AsDouble(dr("apositiontaking_4_min"))
                mTGMin = CastAide.AsDouble(dr("apositiontaking_6_min"))
                mMPMin = CastAide.AsDouble(dr("apositiontaking_9_min"))
                mORMin = CastAide.AsDouble(dr("apositiontaking_10_min"))
                mLHdpMin = CastAide.AsDouble(dr("alivepositiontaking_1_min"))
                mLOUMin = CastAide.AsDouble(dr("alivepositiontaking_3_min"))
                mL1stHdpMin = CastAide.AsDouble(dr("alivepositiontaking_7_min"))
                mL1stOUMin = CastAide.AsDouble(dr("alivepositiontaking_8_min"))

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

                TemplateHandler.Assign(soccerTpl, "minbet1-val", 0)
                TemplateHandler.Assign(soccerTpl, "maxbet1-val", 0)
                TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", 0)
            End If

            ''assign Increase and Reduce button for template
            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(soccerTpl, "a-hdp-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(soccerTpl, "a-1x2-btn", btnTpl)

            btnTpl = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-btn", btnTpl)

            ''get PT deadball value for master preset PT
            Dim pt As String = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(soccerTpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(soccerTpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1stHdpMin, m1stHdpMax, m1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-1sthdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUD")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1stOUMin, m1stOUMax, m1stOU))
            TemplateHandler.Assign(soccerTpl, "a-1stou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(soccerTpl, "a-oe-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11x2")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(m1x2Min, m1x2Max, m1x2))
            TemplateHandler.Assign(soccerTpl, "a-1x2-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1CS")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mCSMin, mCSMax, mCS))
            TemplateHandler.Assign(soccerTpl, "a-cs-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1TG")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mTGMin, mTGMax, mTG))
            TemplateHandler.Assign(soccerTpl, "a-tg-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1MP")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            TemplateHandler.Assign(soccerTpl, "a-mp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OR")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mORMin, mORMax, mOR))
            TemplateHandler.Assign(soccerTpl, "a-or-val", pt)

            ''get PT value for super preset PT

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1HdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mLHdpMin, mLHdpMax, mLHdp))
            TemplateHandler.Assign(soccerTpl, "a-Lhdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM1OUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mLOUMin, mLOUMax, mLOU))
            TemplateHandler.Assign(soccerTpl, "a-Lou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stHdpL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stHdpMin, mL1stHdpMax, mL1stHdp))
            TemplateHandler.Assign(soccerTpl, "a-L1stHdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM11stOUL")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mL1stOUMin, mL1stOUMax, mL1stOU))
            TemplateHandler.Assign(soccerTpl, "a-L1stou-val", pt)

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try

        Return soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball, Football, Tennis, Golf, OtherSports"
    'this function is used to assign data into template for all sporttype that have 4 bettype
    '(1:Handicap, 3:Over/Under, 2:Odd/Even, 20:Money Line)
    'dr: row contains data get from db
    'sportTypeId: id of sporttype (2: Basketball, 3:Football, 5:Tennis, 10:Golf, 99:OtherSports )
    Function GetData4DDl(ByVal dr As DataRow, ByVal sportTypeId As Integer, ByVal tpl As String) As String
        Dim pt As String
        Dim hiddenHash As New Hashtable
        Try
            'assign text for position taking
            TemplateHandler.RemoveBlock(tpl, "auto-preset")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            TemplateHandler.RemoveBlock(tpl, "no-mixparlay")

            Dim str As String = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "Bet" + """/>" + Resources.Language.lblBetSetting
            If Mult = "0" Then
                str = Resources.Language.lblBetSetting
            End If
            TemplateHandler.Assign(tpl, "bet-text", str)

            str = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "PT" + """/>" + Resources.Language.PositionTaking
            If Mult = "0" Then
                str = Resources.Language.PositionTaking
            End If
            TemplateHandler.Assign(tpl, "pt-text", str)

            GetTitleCommon(tpl)

            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "mp-text", Resources.Language.MixParlay)

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'If (sportTypeId = "2") Then
            '    TemplateHandler.Assign(tpl, "title-col", "6")
            'Else
            '    TemplateHandler.Assign(tpl, "title-col", "5")
            'End If
            TemplateHandler.Assign(tpl, "title-col", "5")

            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMPMin, mMLMin As Double
            Dim mHdpMax, mOUMax, mOEMax, mMPMax, mMLMax As Double
            Dim mHdp, mOU, mOE, mMP, mML As Double
            Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

            If Mult = "0" Then 'single
                mHdpMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_1"))
                mOUMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_3"))
                mOEMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'If (sportTypeId = "2") Then
                '    mMPMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_9"))
                'End If

                mMLMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_20"))

                mHdpMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_1"))
                mOUMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_3"))
                mOEMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'If (sportTypeId = "2") Then
                '    mMPMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_9"))
                'End If

                mMLMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_20"))

                mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))

                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'If (sportTypeId = "2") Then
                '    mMP = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_9"))
                'End If
                mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                'assign value for position taking
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", FormatNumber(dr("MinBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", FormatNumber(dr("MaxBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", FormatNumber(dr("maxpermatchv" + CStr(sportTypeId)), 0))

            Else 'multi
                mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
                mOE = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))

                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'If (sportTypeId = "2") Then
                '    mMP = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_9"))
                'End If

                mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                mHdpMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mOUMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
                mOEMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'If (sportTypeId = "2") Then
                '    mMPMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_9"))
                'End If
                mMLMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                mHdpMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1_min"))
                mOUMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3_min"))
                mOEMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2_min"))
                'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                'If (sportTypeId = "2") Then
                '    mMPMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_9_min"))
                'End If
                mMLMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20_min"))

                'assign value for position taking
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", 0)
            End If

            mMinbet = CastAide.AsDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
            mMaxbet = CastAide.AsDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Min = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Max = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

            'assign data for bet setting
            TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
            TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
            TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
            TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'If (sportTypeId = "2") Then
            '    pt = selectTpl
            '    TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "MP")
            '    TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMP))
            '    TemplateHandler.Assign(tpl, "a-mp-val", pt)
            'End If

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

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
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/BaseballPT.htm")

            TemplateHandler.RemoveBlock(tpl, "auto-preset")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")

            Dim str As String = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "Bet" + """/>" + Resources.Language.lblBetSetting
            If Mult = "0" Then
                str = Resources.Language.lblBetSetting
            End If
            TemplateHandler.Assign(tpl, "bet-text", str)

            str = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "PT" + """/>" + Resources.Language.PositionTaking
            If Mult = "0" Then
                str = Resources.Language.PositionTaking
            End If
            TemplateHandler.Assign(tpl, "pt-text", str)

            GetTitleCommon(tpl)

            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)

            TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Language.CopyFromHdp)

            TemplateHandler.Assign(tpl, "title-col", "4")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMax, mOUMax, mMLMax As Double
            Dim mHdp, mOU, mML As Double
            Dim mHdpMin, mOUMin, mMLMin As Double

            Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

            mMinbet = CastAide.AsDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
            mMaxbet = CastAide.AsDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Min = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Max = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

            'assign data for bet setting
            TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
            TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
            TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
            TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

            If Mult = "0" Then ' single
                mHdpMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_1"))
                mOUMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_3"))
                mMLMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_20"))

                mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
                mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                mHdpMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_1"))
                mOUMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_3"))
                mMLMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_20"))

                'assign value for position taking
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", FormatNumber(dr("MinBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", FormatNumber(dr("MaxBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", FormatNumber(dr("maxpermatchv" + CStr(sportTypeId)), 0))
            Else ' multi
                mHdpMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mOUMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
                mMLMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))
                mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                mHdpMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1_min"))
                mOUMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3_min"))
                mMLMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20_min"))

                'assign value for position taking
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", 0)
            End If

            strHidBaseball = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            Dim btnTpl As String = groupBtn
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
        Try
            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/MotorSportsPT.htm")
            TemplateHandler.RemoveBlock(tpl, "auto-preset")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")

            Dim str As String = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "Bet" + """/>" + Resources.Language.lblBetSetting
            If Mult = "0" Then
                str = Resources.Language.lblBetSetting
            End If
            TemplateHandler.Assign(tpl, "bet-text", str)

            str = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "PT" + """/>" + Resources.Language.PositionTaking
            If Mult = "0" Then
                str = Resources.Language.PositionTaking
            End If
            TemplateHandler.Assign(tpl, "pt-text", str)

            GetTitleCommon(tpl)

            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
            TemplateHandler.Assign(tpl, "title-col", "3")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())


            'get data and assign for template Position taking
            Dim mHdpMax, mMLMax As Double
            Dim mHdp, mML As Double
            Dim mHdpMin, mMLMin As Double

            Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

            mMinbet = CastAide.AsDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
            mMaxbet = CastAide.AsDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Min = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Max = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

            'assign data for bet setting
            TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
            TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
            TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
            TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

            If Mult = "0" Then 'single
                mHdpMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_1"))
                mMLMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_20"))

                mHdpMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_1"))
                mMLMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_20"))

                mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", FormatNumber(dr("MinBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", FormatNumber(dr("MaxBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", FormatNumber(dr("maxpermatchv" + CStr(sportTypeId)), 0))
            Else 'mult
                mHdpMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1_min"))
                mMLMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20_min"))

                mHdpMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mMLMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                mHdp = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_1"))
                mML = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_20"))

                'assign value for minbet, maxbet, maxpermatch (all are 0 for adding case)
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", 0)
            End If

            strHidMoto = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdp))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mML))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            Dim btnTpl As String = groupBtn
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
        'numbergame.GetNumberCom(dr, 1, False, bodyTpl)
        Dim NumberGameSetting As String
        NumberGameSetting = numbergame.GetMemberPtSetting(dr, False)
        numbergame.GetMaxPerBall(dr, 1, False, NumberGameSetting)
        Return NumberGameSetting
    End Function
#End Region

#Region "Assign info for Mixed Sports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim mixedSport As SportBooksBase = New MixedSportsManager()
        TemplateHandler.Assign(bodyTpl, "MixedSports-text", Resources.Language.lblMixedSports)
        Return mixedSport.GetMemberPtSetting(dr, False)
    End Function
#End Region

#Region "Assign info for Financial"
    'this function is used to assign data into template for Baseball
    'dr: row contains data get from db
    'sportTypeId: id of sporttype
    'Baseball have only Handicap, Over/Under & Money Line bettype
    Function GetDataFinancial(ByVal dr As DataRow, ByVal sportTypeId As Integer) As String
        Dim pt As String
        Dim tpl As String
        Dim hiddenHash As New Hashtable
        Try

            'assign text for position taking
            tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/agent/FinancialPT.htm")

            TemplateHandler.RemoveBlock(tpl, "auto-preset")
            TemplateHandler.RemoveBlock(tpl, "none-edit")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.RemoveBlock(tpl, "none-member")
            TemplateHandler.Assign(tpl, "150px", "300px")
            Dim str As String = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "Bet" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "Bet" + """/>" + Resources.Language.lblBetSetting
            If Mult = "0" Then
                str = Resources.Language.lblBetSetting
            End If
            TemplateHandler.Assign(tpl, "bet-text", str)

            str = "<input type=""checkbox"" id=""chkUpd" + CStr(sportTypeId) + "PT" + """ onclick=""AllowUpdate(this,false);"" name=""chkUpd" + CStr(sportTypeId) + "PT" + """/>" + Resources.Language.PositionTaking
            If Mult = "0" Then
                str = Resources.Language.PositionTaking
            End If
            TemplateHandler.Assign(tpl, "pt-text", str)

            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            GetTitleCommon(tpl, "201")

            TemplateHandler.Assign(tpl, "copy-hdp-col", "2")

            'Assign label for Odd/Even PT
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven) 'ODD/EVEN label
            TemplateHandler.Assign(tpl, "lblCopyFromBettype", String.Format(Resources.Language.lblCopyFromBettype, Resources.Language.OverUnder)) 'Copy from label
            TemplateHandler.Assign(tpl, "copy-ou-span", "1")
            'END-----------------------------------------------

            TemplateHandler.Assign(tpl, "title-col", "5")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())
            Dim mOUMax As Double
            Dim mOU As Double
            Dim mOUMin As Double

            'Odd/Even
            Dim mOEMax As Double
            Dim mOE As Double
            Dim mOEMin As Double
            'END-------------
            Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

            mMinbet = CastAide.AsDouble(dr("MinBet" + "v" & sportTypeId & "_Max"))
            mMaxbet = CastAide.AsDouble(dr("MaxBet" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Min = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Max = CastAide.AsDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

            'assign data for bet setting
            TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
            TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
            TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
            TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

            If Mult = "0" Then ' single
                mOUMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_3"))

                mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))

                mOUMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_3"))

                'Get Odd/Even Min/max PT
                mOEMax = CastAide.AsDouble(dr("maxv" + CStr(sportTypeId) + "pos_2"))

                mOE = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))

                mOEMin = CastAide.AsDouble(dr("minv" + CStr(sportTypeId) + "pos_2"))
                'END ------------

                'assign value for position taking
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", FormatNumber(dr("MinBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", FormatNumber(dr("MaxBetv" + CStr(sportTypeId)), 0))
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", FormatNumber(dr("maxpermatchv" + CStr(sportTypeId)), 0))
            Else ' multi
                mOUMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))

                mOU = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3"))

                mOUMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_3_min"))

                'Get Odd/Even Min/max PT
                mOEMax = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))

                mOE = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2"))

                mOEMin = CastAide.AsDouble(dr("v" + CStr(sportTypeId) + "apositiontaking_2_min"))
                'END--------------
                'assign value for position taking
                TemplateHandler.Assign(tpl, "minbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxbet" & sportTypeId & "-val", 0)
                TemplateHandler.Assign(tpl, "maxpermatch" & sportTypeId & "-val", 0)
            End If

            strHidBaseball = CustomerUtils.AssignMultipleHidden(hiddenHash)

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOUMin, mOUMax, mOU))
            TemplateHandler.Assign(tpl, "a-ou-val", pt)
            TemplateHandler.Assign(tpl, "padding-left", "padding-left:55px")

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "OU")
            TemplateHandler.Assign(tpl, "a-ou-btn", btnTpl)

            'Assign Data for Odd/Even PT
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "OE")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mOEMin, mOEMax, mOE))
            TemplateHandler.Assign(tpl, "a-oe-val", pt)
            TemplateHandler.Assign(tpl, "padding-left", "padding-left:55px")
            'END --------------------------------------------------------------------------------------
            If WebSettings.disabledFI Then
                tpl = ""
                hiddenHash.Add("hiddenMinbet201", dr("minbetv" + sportTypeId.ToString()))
                hiddenHash.Add("hiddenMaxbet201", dr("maxbetv" + sportTypeId.ToString()))
                hiddenHash.Add("hiddenMaxpermatch201", dr("maxpermatchv" + sportTypeId.ToString()))

                hiddenHash.Add("hiddenAgOUB201", mOU)
                hiddenHash.Add("hiddenAgOEB201", mOE)

                tpl = CustomerUtils.AssignMultipleHidden(hiddenHash)

            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try

        GetDataFinancial = tpl
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String, Optional ByVal sporttype As String = "0")
        TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.Assign(tpl, "bet-text", Resources.Language.lblBetSetting)
        TemplateHandler.Assign(tpl, "minbet-text", Resources.Language.MinBet)
        TemplateHandler.Assign(tpl, "maxbet-text", Resources.Language.MaxBet)
        TemplateHandler.Assign(tpl, "maxpermatch-text", Resources.Language.MaxPerMatch)
        TemplateHandler.Assign(tpl, "pt-text", Resources.Language.PositionTaking)
        TemplateHandler.Assign(tpl, "auto-super-text", Resources.Language.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "a-presetpt-text", Resources.Language.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "p-presetpt-text", Resources.Language.lblSuperPresetPosition)
        TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
        TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblML)
        TemplateHandler.Assign(tpl, "copy-hdp-text", Resources.Language.CopyFromHdp)
        TemplateHandler.Assign(tpl, "copy-oe-text", Resources.Language.lblCopyFromOE)
        TemplateHandler.Assign(tpl, "copy-hdp-col", "1")
    End Sub
#End Region
End Class
