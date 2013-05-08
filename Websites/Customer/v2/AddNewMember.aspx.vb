'Imports IBCDB
Imports System.Resources
Imports Sunplus.Accounting.Core.Extensions
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
Imports System.Linq


Public Class AddNewMember
    Inherits RequiredAuthorizationPage
    Public lminbet, lmaxbet, lminpermatch, lmaxpermatch, lminbetb, lmaxbetb, lminpermatchb, lmaxpermatchb As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.


    Private designerPlaceholderDeclaration As System.Object

    Protected groupa As String = String.Empty
    Protected groupb As String = String.Empty
    Protected groupc As String = String.Empty
    Protected WithEvents lblOutRight As System.Web.UI.WebControls.Label
    Protected WithEvents ddl19 As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents chkDeadballOther As System.Web.UI.HtmlControls.HtmlInputCheckBox
    Protected WithEvents lblCopyFromHandicap As System.Web.UI.WebControls.Label
    Protected WithEvents lblCopyFrom1x2 As System.Web.UI.WebControls.Label
    Protected groupd As String = String.Empty
    'Dim lblPasswordRequire As HtmlInputHidden
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Dim res As New ResManager
    Dim bodyTpl, strAccInfo, tennisTpl, basketballTpl, footballTpl, otherSportsTpl, golfTpl, financialTpl, groupBtn, selectTpl, strHidSoccer As String
    Dim strHidBaseball, strHidGolf, strHidMoto, strHidNumberGame, strHidOther, strHidBasket, strHidFootball, strHidTennis, strHidVariable As String
    Public script, content As New StringBuilder
    Public agentID As String = "0"

    Protected Overrides ReadOnly Property CustId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.AgentId)
        End Get
    End Property

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Dim cud As Shaunctions = New Shaunctions()
        'Put user code to initialize the page here
        Try

            If Not IsPostBack Then
                Dim ht As New Hashtable
                agentID = Request("agentId")
                ht.Add("custid", agentID)
                Dim rs As DataSet = Select4Edit.acc_CustomerSelAgentInfo(ht)
                Dim dt As DataTable = rs.Tables(0)
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

                    'TemplateHandler.RemoveBlock(selectTpl, "none-member")
                    CustomerUtils.GetSportTitle(bodyTpl, False)

                    TemplateHandler.Assign(bodyTpl, "{add-text}", Resources.Language.btnAdd)
                    TemplateHandler.Assign(bodyTpl, "{add-id}", "btnSubmit")
                    TemplateHandler.Assign(bodyTpl, "{copy-Basketball}", Resources.Language.lblCopyBasketHdp)
                    TemplateHandler.Assign(bodyTpl, "{accinfo}", AssignDataInfo(dr))
                    TemplateHandler.Assign(bodyTpl, "{soccerPT}", GetDataSoccer(dr))
                    TemplateHandler.Assign(bodyTpl, "{basketPT}", GetData4DDl(dr, 2, basketballTpl))
                    TemplateHandler.Assign(bodyTpl, "{footballPT}", GetData4DDl(dr, 3, footballTpl))
                    TemplateHandler.Assign(bodyTpl, "{tennisPT}", GetData4DDl(dr, 5, tennisTpl))
                    TemplateHandler.Assign(bodyTpl, "{baseballPT}", GetDataBaseball(dr, 8))
                    TemplateHandler.Assign(bodyTpl, "{golfPT}", GetData4DDl(dr, 10, golfTpl))
                    TemplateHandler.Assign(bodyTpl, "{motoPT}", GetDataMotoSports(dr, 11))

                    Dim numbergame As SportBooksBase = New NumberGameManager()
                    numbergame.GetNumberCom(dr, 1, True, bodyTpl)



                    If Sunplus.Bingo.Core.BingoSettings.NumberGame.Enabled Then
                        TemplateHandler.Assign(bodyTpl, "{NumberGamePT}", GetDataNumberGame(dr))
                    Else
                        TemplateHandler.RemoveBlock(bodyTpl, "NumberGame")
                    End If

                    TemplateHandler.Assign(bodyTpl, "{MixedSportsPT}", GetDataMixedSports(dr))
                    TemplateHandler.Assign(bodyTpl, "{otherPT}", GetData4DDl(dr, 99, otherSportsTpl))

                    TemplateHandler.Assign(bodyTpl, "{widthpt}", "920px")
                    content.Append(bodyTpl)
                    content.Append(strHidVariable)
                    VariableScript(dr)
                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub


#Region "Declare javascript parameter"
    Function VariableScript(ByVal dr As DataRow) As String
        Dim script As String = ""

        groupa = Format(dr("groupa"), "#,##0.####")
        groupb = Format(dr("groupb"), "#,##0.####")
        groupc = Format(dr("groupc"), "#,##0.####")
        groupd = Format(dr("groupd"), "#,##0.####")

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
        script += " var lblPasswordRequire='" & Resources.Language.lblPasswordRequire & "';" & vbNewLine
        script += " var alertUserName='" & Resources.Language.alertUserName & "';" & vbNewLine

        Dim lblCreditInvalid As String, lblMemberMaxCreditInvalid As String = String.Empty

        lblCreditInvalid = Resources.Language.lblCreditInvalid.Replace("{0}", Resources.Language.lblMemberMaxCredit)
        lblMemberMaxCreditInvalid = Resources.Language.lblCreditInvalid.Replace("{0}", Resources.Language.lblMemberMaxCredit)

        script += " var alertCreditInvalid='" & lblCreditInvalid & "';" & vbNewLine
        script += " var alertMemberMaxCreditInvalid='" & lblMemberMaxCreditInvalid & "';" & vbNewLine

        script += "  var valGroupa='" + groupa + "';"
        script += "  var valGroupb='" + groupb + "';"
        script += "  var valGroupc='" + groupc + "';"
        script += "  var valGroupd='" + groupd + "';"
        Dim strAlertMinPT As String = Resources.Language.alertLessthanMinPT
        strAlertMinPT = strAlertMinPT.Replace("{1}", Resources.Language.lblMasterPresetPosition)
        strAlertMinPT = strAlertMinPT.Replace("{2}", Resources.Language.lblSuperPresetPosition)
        script += " var alertLessthanMinPT='" & strAlertMinPT & "';" & vbNewLine
        script += "function SetValueMemMaxCredit(){}" & vbNewLine
        script += "</script>"
        Response.Write(script)
        Return script
    End Function
#End Region

#Region "Load AccInfoMem template"
    Function AssignDataInfo(ByVal dr As DataRow) As String
        Try

            TemplateHandler.RemoveBlock(strAccInfo, "none-super")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            'TemplateHandler.RemoveBlock(strAccInfo, "none-single")
            TemplateHandler.RemoveBlock(strAccInfo, "none-cell-member")
            TemplateHandler.Assign(strAccInfo, "{tranfer-time}", "")
            TemplateHandler.Assign(strAccInfo, "{cols-untext}", "4")

            TemplateHandler.RemoveBlock(strAccInfo, "Member-Max-Credit")

            Dim agentUname As String = ""
            If Request("username") Is Nothing Or Request("username") = String.Empty Then
                agentUname = Session("CustUname").ToString()
            Else
                agentUname = Request("username")
            End If
            TemplateHandler.Assign(strAccInfo, "{title}", Resources.Language.lblAddNewMember + " " + Resources.Language.To.ToLower() + " " + agentUname)
            TemplateHandler.Assign(strAccInfo, "{max-credit-text}", Resources.Language.lblMemberMaxCredit)

            Dim isSuspended As Boolean = CastAide.AsBoolean(dr("suspended"))
            TemplateHandler.Assign(strAccInfo, "chkSuspended", HtmlHelper.GenCheckBox("chkSuspended", isSuspended, isSuspended, "", ""))

            Dim isClosed As Boolean = CastAide.AsBoolean(dr("closed"))
            TemplateHandler.Assign(strAccInfo, "chkClosed", HtmlHelper.GenCheckBox("chkClosed", isClosed, isClosed, "", ""))



            TemplateHandler.Assign(strAccInfo, "{fname-val}", "")
            TemplateHandler.Assign(strAccInfo, "{lname-val}", "")
            TemplateHandler.Assign(strAccInfo, "{phone-val}", "")
            TemplateHandler.Assign(strAccInfo, "{mobile-val}", "")
            TemplateHandler.Assign(strAccInfo, "{credit-val}", "0")
            CustomerUtils.GetCredit(dr, strAccInfo, False)
            TemplateHandler.Assign(strAccInfo, "{check-val}", "")
            TemplateHandler.Assign(strAccInfo, "{fax-val}", "")

            Dim discount As Double = FormatHandler.GetDouble(dr("groupa"))
            Dim discountcs As Double = FormatHandler.GetDouble(dr("discountcs"))
            Dim discountcs1x2 As Double = 0


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
            TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenSelectOption(discount, discount, Globals.INTERVAL_COMM, False, "ddrCommissionH_OU_OE", "", "selectcomm"))
            dataHDP_OU_OE.Append(valueBetType)

            'Other
            'Agent
            valueBetType = tplPTValue
            TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenInput(Globals.InputType.Text, "txtDiscountcs", discountcs.ToString(), "selectcomm", True, False, ""))
            dataOther.Append(valueBetType)
            'Member
            valueBetType = tplPTValue
            TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenSelectOption(discountcs, discountcs, Globals.INTERVAL_COMM, False, "ddrCommissionCS_TG", "", "selectcomm"))
            dataOther.Append(valueBetType)

            If (WebSettings.Disable1x2Comm) Then
                TemplateHandler.RemoveBlock(tplPTData, "1x2")
            Else
                Try
                    discountcs1x2 = FormatHandler.GetDouble(dr("discount1x2"))
                Catch ex As Exception
                    discountcs1x2 = 0
                End Try

                '1x2
                'Agent
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenInput(Globals.InputType.Text, "txtDiscountcs1x2", discountcs1x2.ToString(), "selectcomm", True, False, ""))
                data1x2.Append(valueBetType)
                'Member
                valueBetType = tplPTValue
                TemplateHandler.Assign(valueBetType, "{value}", HtmlHelper.GenSelectOption(discountcs1x2, discountcs1x2, Globals.INTERVAL_COMM, False, "ddrCommissionCS_1x2", "", "selectcomm"))
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

            TemplateHandler.Assign(strAccInfo, "{discount-val}", CStr(discount))
            TemplateHandler.Assign(strAccInfo, "{discountcs-val}", CStr(discountcs))

            Dim username_parent As String = Session("username")
            Dim sel As String = selectTpl

            TemplateHandler.Assign(strAccInfo, "{group-text}", Resources.Language.lblGroup)
            TemplateHandler.Assign(strAccInfo, "{lblCommissionForAgent}", Resources.Language.CommissionForAgent)
            TemplateHandler.Assign(strAccInfo, "{lblAsianHDC_OU_OE}", Resources.Language.lblAsianHDC_OU_OE)
            TemplateHandler.Assign(strAccInfo, "{lbl1x2_CS_TG}", Resources.Language.lblOtherTypes)

            TemplateHandler.Assign(strAccInfo, "{lblOther}", Resources.Language.lblOthersMeeting)

            TemplateHandler.Assign(strAccInfo, "{lblCommissionForMember}", Resources.Language.CommissionForMember)
            TemplateHandler.Assign(strAccInfo, "{CommissionH_OU_OE}", AssignData4Commission(dr, "ddrCommissionH_OU_OE"))
            TemplateHandler.Assign(strAccInfo, "{CommissionCS_TG}", AssignData4Commission(dr, "ddrCommissionCS_TG"))

            'End If
            TemplateHandler.Assign(strAccInfo, "{UGroup}", AssignData4Group())

            CustomerUtils.GetTitleInfo(strAccInfo, False)

            Dim hiddenHash As Hashtable = New Hashtable

            hiddenHash.Add("hidden_discount", discount)
            hiddenHash.Add("acomhdp", Format(dr("groupa"), "#,##0.####"))
            hiddenHash.Add("hidden_discountcs", discountcs)
            hiddenHash.Add("hidden_uparent", username_parent)
            hiddenHash.Add("hidSuspended", IIf(isSuspended, 1, 0))
            hiddenHash.Add("hidClosed", IIf(isClosed, 1, 0))
            strHidVariable = CustomerUtils.AssignMultipleHidden(hiddenHash)

            strAccInfo += HtmlHelper.GenHidden("uplineusername", agentUname)

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
        Dim csm As Decimal = 0
        Dim vstep As Integer = 5
        If (ddl = "ddrCommissionH_OU_OE") Then
            csm = FormatNumber(dr("groupa"), d) * 10000

        ElseIf ddl = "ddrCommissionOther" Then
            d = 3
            vstep = 25
            csm = FormatNumber(dr("discountcsOther"), 5) * 10000
        Else
            d = 3
            'vstep = 25
            vstep = 5
            'csm = FormatNumber(dr("discountcs"), 5) * 100000
            csm = FormatNumber(dr("discountcs"), 5) * 10000
        End If
        Dim str As String = "<SELECT class='selectcomm' id='" & ddl & "' name='" & ddl & "'>"
        Dim index As Integer = 0
        While csm > 0
            'str += "<option value='" & csm / IIf(d = 4, 10000, 100000) & "'" + IIf(index = 0, "selected='true'", "") + " >" & csm / IIf(d = 4, 10000, 100000) & "</option>"
            str += "<option value='" & csm / IIf(d = 4, 10000, 10000) & "'" + IIf(index = 0, "selected='true'", "") + " >" & csm / IIf(d = 4, 10000, 10000) & "</option>"
            csm = csm - vstep
            index = index + 1
        End While
        str += "<option value='0'>0</option>"
        str += "</select>"
        Return str
    End Function
#End Region

#Region "AssignData4Group(ByVal dr As DataRow)"
    Function AssignData4Group() As String


        Dim str As String = "<select id = 'ddlUGruop' name ='ddlUGruop'onchange='javascript:updateCommission(this.form)' >"
        Dim ht2 As New Hashtable
        ht2.Add("custid", Request("agentId"))
        Dim dt As DataTable = Select4Edit.CustomerSelCustomerInfo(ht2).ResultSet.Tables(0)
        Dim currenciesGroup = CustomerSection.Instance(CustomerSection.CustomerCollections.Currencies)
     
        Dim currencies As String
        currencies = "a,b,c,d"

        If dt.Rows.Count > 0 Then
            Dim sitesGroup = CustomerSection.Instance(CustomerSection.CustomerCollections.Sites)
            If dt.Rows(0).Item("site").ToString().ToLower() = "ibsbet" then
                Dim userGroupBySite = (From currency In sitesGroup Where currency.Name = "ibsbet" Select currency).Single() 
                currencies = userGroupBySite.Group
            Else If dt.Rows(0).Item("currency").ToString().ToLower() = "won" then
                Dim userGroupByCurrency = (From currency In currenciesGroup Where currency.Name = "won" Select currency).Single()
                currencies = userGroupByCurrency.Group
            Else 
                Dim userGroupByOtherCurrency = (From currency In currenciesGroup Where currency.Name = "other" Select currency).Single()
                currencies = userGroupByOtherCurrency.Group
            End If
        Else
            Dim userGroupByDefaultCurrency = (From currency In currenciesGroup Where currency.Name = "default" Select currency).Single()
                currencies = userGroupByDefaultCurrency.Group
        End If

        Dim arr = currencies.Split(",")
        For Each currentCur As String In arr
           str += ("<option value='"+currentCur+"'>"+ currentCur +"</option>")
        Next


        str += "</select>"
        Return str
    End Function
#End Region

#Region "AssignDataUserName"
    Function AssignDataUserName(ByVal ddl As String, ByVal val As String) As String
        Dim str As String = ""
        str += "<select class='Sel40' id = '" + ddl + "' name ='" + ddl + "'>"
        'str += "<option value='" + CStr(val) + "'>" + CStr(val) + "</option>"
        For i As Integer = 0 To 9
            If (i = CInt(val)) Then
                str += "<option selected='true' value='" + CStr(i) + "'>" + CStr(i) + "</option>"
            Else
                str += "<option value='" + CStr(i) + "'>" + CStr(i) + "</option>"
            End If
        Next
        str += "</select>"
        Return str
    End Function
#End Region

#Region "Assign Info for Soccer"
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

            GetTitleCommon(soccerTpl)
            TemplateHandler.Assign(soccerTpl, "live-text", Resources.Language.Live)
            TemplateHandler.Assign(soccerTpl, "copy-1x2-text", Resources.Language.CopyFrom1x2)
            TemplateHandler.Assign(soccerTpl, "deadball-text", Resources.Language.Deadball)
            TemplateHandler.Assign(soccerTpl, "dead-row", "2")
            TemplateHandler.Assign(soccerTpl, "live-row", "1")
            TemplateHandler.Assign(soccerTpl, "copy-live-col", "1")
            TemplateHandler.Assign(soccerTpl, "col-title", 11)
            TemplateHandler.Assign(soccerTpl, "col-auto", 9)

            'assign value of Minbet, Maxbet , ...
            'TemplateHandler.Assign(soccerTpl, "minbet1-val", "0")
            'TemplateHandler.Assign(soccerTpl, "maxbet1-val", "0")
            'TemplateHandler.Assign(soccerTpl, "maxpermatch1-val", "0")
            CustomerUtils.GetMinMaxAddValue(soccerTpl, 1, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, soccerTpl, "")

            'assign value position taking for template
            Dim mHdpMin, mOUMin, m1stHdpMin, m1stOUMin, mOEMin, m1x2Min, mCSMin, mTGMin, mMPMin, mORMin As Double
            Dim mLHdpMin, mLOUMin, mL1stHdpMin, mL1stOUMin As Double

            Dim mHdpMax, mOUMax, m1stHdpMax, m1stOUMax, mOEMax, m1x2Max, mCSMax, mTGMax, mMPMax, mORMax As Double
            Dim mLHdpMax, mLOUMax, mL1stHdpMax, mL1stOUMax As Double

            mHdpMax = CastAide.AsDouble(dr("apositiontaking_1"))
            mOUMax = CastAide.AsDouble(dr("apositiontaking_3"))
            m1stHdpMax = CastAide.AsDouble(dr("apositiontaking_7"))
            m1stOUMax = CastAide.AsDouble(dr("apositiontaking_8"))
            mOEMax = CastAide.AsDouble(dr("apositiontaking_2"))
            m1x2Max = CastAide.AsDouble(dr("apositiontaking_5"))
            mCSMax = CastAide.AsDouble(dr("apositiontaking_4"))
            mTGMax = CastAide.AsDouble(dr("apositiontaking_6"))
            mMPMax = CastAide.AsDouble(dr("apositiontaking_9"))
            mORMax = CastAide.AsDouble(dr("apositiontaking_10"))
            mLHdpMax = CastAide.AsDouble(dr("alivepositiontaking_1"))
            mLOUMax = CastAide.AsDouble(dr("alivepositiontaking_3"))
            mL1stHdpMax = CastAide.AsDouble(dr("alivepositiontaking_7"))
            mL1stOUMax = CastAide.AsDouble(dr("alivepositiontaking_8"))

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

            ''get PT value for super preset PT

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
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try

        Return soccerTpl
    End Function
#End Region

#Region "Assign info for  Basketball, Football, Tennis, Golf, OtherSports, MixParlay"
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

            GetTitleCommon(tpl)
            TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "mp-text", Resources.Language.MixParlay)
            TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblML)

            TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'If (sportTypeId = "2") Then
            'TemplateHandler.Assign(tpl, "title-col", "6")
            'Else
            'TemplateHandler.Assign(tpl, "title-col", "5")
            'End If
            TemplateHandler.Assign(tpl, "title-col", "5")

            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMin, mOUMin, mOEMin, mMPMin, mMLMin As Double

            Dim mHdpMax, mOUMax, mOEMax, mMPMax, mMLMax As Double
            Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

            mHdpMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            mOUMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            mOEMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_2"))

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'If (sportTypeId = "2") Then
            'mMPMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_9"))
            'End If
            mMLMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdpMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
            mOUMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
            mOEMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_2_min"))

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'If (sportTypeId = "2") Then
            'mMPMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_9_min"))
            'End If
            mMLMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

            mMinbet = FormatHandler.GetDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
            mMaxbet = FormatHandler.GetDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Min = FormatHandler.GetDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Max = FormatHandler.GetDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

            'assign data for bet setting
            TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
            TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
            TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
            TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

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

            'Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
            'If (sportTypeId = "2") Then
            '    pt = selectTpl
            '    TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "MP")
            '    TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMPMin, mMPMax, mMPMax))
            '    TemplateHandler.Assign(tpl, "a-mp-val", pt)
            'End If

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
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
            'TemplateHandler.RemoveBlock(tpl, "none-member")

            GetTitleCommon(tpl)

            TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblML)

            TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
            TemplateHandler.Assign(tpl, "title-col", "4")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            Dim mHdpMax, mOUMax, mMLMax As Double
            Dim mHdpMin, mOUMin, mMLMin As Double

            Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

            mMinbet = FormatHandler.GetDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
            mMaxbet = FormatHandler.GetDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Min = FormatHandler.GetDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Max = FormatHandler.GetDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

            'assign data for bet setting
            TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
            TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
            TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
            TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

            mHdpMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            mOUMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_3"))
            mMLMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdpMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
            mOUMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_3_min"))
            mMLMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))


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

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)

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
            'TemplateHandler.RemoveBlock(tpl, "none-member")

            GetTitleCommon(tpl)

            TemplateHandler.Assign(tpl, "hdp-text", Resources.Language.Handicap)
            TemplateHandler.Assign(tpl, "ou-text", Resources.Language.OverUnder)
            TemplateHandler.Assign(tpl, "oe-text", Resources.Language.OddEven)
            TemplateHandler.Assign(tpl, "ml-text", Resources.Language.lblML)

            TemplateHandler.Assign(tpl, "copy-hdp-col", "2")
            TemplateHandler.Assign(tpl, "title-col", "3")
            TemplateHandler.Assign(tpl, "chkAuto-" & sportTypeId, "chkAuto" & sportTypeId)

            CustomerUtils.GetMinMaxAddValue(tpl, sportTypeId, dr)
            CustomerUtils.GetLimitMinMaxBet(dr, tpl, sportTypeId.ToString())

            'get data and assign for template Position taking
            Dim mHdpMax, mMLMax As Double
            Dim mHdpMin, mMLMin As Double

            Dim mMinbet, mMaxbet, mMaxPerMatch_Min, mMaxPerMatch_Max As Double

            mMinbet = FormatHandler.GetDouble(dr("minbet" + "v" & sportTypeId & "_Max"))
            mMaxbet = FormatHandler.GetDouble(dr("maxbet" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Min = FormatHandler.GetDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Min"))
            mMaxPerMatch_Max = FormatHandler.GetDouble(dr("MaxPerMatch" + "v" & sportTypeId & "_Max"))

            'assign data for bet setting
            TemplateHandler.Assign(tpl, "minbet-min-" & sportTypeId, mMinbet)
            TemplateHandler.Assign(tpl, "maxbet-max-" & sportTypeId, mMaxbet)
            TemplateHandler.Assign(tpl, "maxpermatch-min-" & sportTypeId, mMaxPerMatch_Min)
            TemplateHandler.Assign(tpl, "maxpermatch-max-" & sportTypeId, mMaxPerMatch_Max)

            mHdpMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_1"))
            mMLMax = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_20"))

            mHdpMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_1_min"))
            mMLMin = FormatHandler.GetDouble(dr("v" & sportTypeId & "apositiontaking_20_min"))

            'assign data for Position taking dropdowns
            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mHdpMin, mHdpMax, mHdpMax))
            TemplateHandler.Assign(tpl, "a-hdp-val", pt)

            pt = selectTpl
            TemplateHandler.Assign(pt, "ddl-Id", "listM" & sportTypeId & "ML")
            TemplateHandler.Assign(pt, "ddl-val", CustomerUtils.GetListPositionTaking(mMLMin, mMLMax, mMLMax))
            TemplateHandler.Assign(tpl, "a-ml-val", pt)

            Dim btnTpl As String = groupBtn
            TemplateHandler.Assign(btnTpl, "ddl-Id", "listM" & sportTypeId & "Hdp")
            TemplateHandler.Assign(tpl, "a-hdp-btn", btnTpl)
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
        Dim numberGameSetting As String
        numberGameSetting = numbergame.GetMemberPtSetting(dr, True)
        numbergame.GetMaxPerBall(dr, 1, True, numberGameSetting)
        Return numberGameSetting
    End Function
#End Region

#Region "Assign info for Mixed Sports"
    Function GetDataMixedSports(ByVal dr As DataRow) As String
        Dim mixedSport As SportBooksBase = New MixedSportsManager()
        TemplateHandler.Assign(bodyTpl, "MixedSports-text", Resources.Language.lblMixedSports)
        Return mixedSport.GetMemberPtSetting(dr, True)
    End Function
#End Region

#Region "Get Title Common"
    Sub GetTitleCommon(ByRef tpl As String)
        'TemplateHandler.RemoveBlock(tpl, "auto-preset")
        TemplateHandler.Assign(tpl, "{bet-text}", Resources.Language.lblBetSetting)
        TemplateHandler.Assign(tpl, "{minbet-text}", Resources.Language.lblMinBet)
        TemplateHandler.Assign(tpl, "{maxbet-text}", Resources.Language.lblMaxBet)
        TemplateHandler.Assign(tpl, "{maxpermatch-text}", Resources.Language.lblMaxPerMatch)
        TemplateHandler.Assign(tpl, "{pt-text}", Resources.Language.PositionTaking)
        TemplateHandler.Assign(tpl, "{auto-super-text}", Resources.Language.lblAutoSuperPresetPosition)
        TemplateHandler.Assign(tpl, "{a-presetpt-text}", Resources.Language.lblMasterPresetPosition)
        TemplateHandler.Assign(tpl, "{p-presetpt-text}", Resources.Language.lblSuperPresetPosition)
        TemplateHandler.Assign(tpl, "{hdp-text}", Resources.Language.Handicap)
        TemplateHandler.Assign(tpl, "{ml-text}", Resources.Language.lblML)
        TemplateHandler.Assign(tpl, "{copy-hdp-text}", Resources.Language.CopyFromHdp)
        TemplateHandler.Assign(tpl, "{copy-oe-text}", Resources.Language.lblCopyFromOE)
        TemplateHandler.Assign(tpl, "{copy-hdp-col}", "1")
    End Sub
#End Region

End Class
