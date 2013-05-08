Imports Sunplus.Accounting.WcfServiceLibrary.Customer
Imports Sunplus.Accounting.WcfServiceLibrary.PT.Casino
Imports Sunplus.Accounting.WcfServiceLibrary.Bingo
Imports Sunplus.Accounting.Business.PT.Casino
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Core.Configurations
Imports DNAide.Core
Imports DNAide.Web
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data

Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Entities.Casino
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Extensions



Public Class UpdateNewMember
    Inherits RequiredAuthorizationPage
    Dim param As New Hashtable

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

    Dim agentId As String = "0"


    Protected Overrides Function HasPermission() As Boolean
        Return True
    End Function
    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.AgentId)
        End Get
    End Property

    Protected Overrides ReadOnly Property CustId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.CID)
        End Get
    End Property



    Protected Overrides Sub RedirectError()

        Response.Write("<script type=""text/javascript"">parent.showAlertMessage('err','" & StringUtils.ParseToJSMsg(Resources.ErrorMsg.error_InvalidRequest) & "',1);parent.PTUnBlockUI();</script>")

    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            agentId = Request("agentId")
            Dim errmsg As String = ""

            If Permissions.CheckPermission(CastAide.AsInt32(Session("AccId")), Enumerations.Functions.Customer.OtherCust, errmsg) <> 0 Then
                Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI(); parent.ShowError('" & AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) & "','err'); parent.PTUnBlockUI();parent.afterUpdate(false,2000);</script>")
            Else
                InsertData()
            End If

        Catch ex As Exception

            Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI(); parent.ShowError('" & AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) & "','err');parent.afterUpdate(false,2000);</script>")



        End Try
    End Sub

#Region "InsertData()"
    Sub InsertData()
        Try
            Dim rs As IBCUtils.IbcError = Nothing
            'Dim rs As DNAide.Data.DbResult = Nothing
            Dim msg As String
            msg = ""
            Dim isMult As String = IIf(Not Request("isMult") Is Nothing, Request("isMult"), "0")

            Dim Ids As String = Request("txtIDs")
            Dim str As String = "0-"
            Dim CID() As String = {"1"} ' new

            If (isMult = "1") Then ' edit multi
                CID = Ids.Split("-")
            ElseIf (Not Request("CID") Is Nothing) Then ' edit single
                CID = Request("CID").Split("-")
            End If

            Dim custid As String = String.Empty


            Dim custDetail = Sunplus.Accounting.Business.Customer.CustomerService.GetCustInfo(Request)
            Dim memMiniGameComm = 0.0
            Dim ip = Request.UserHostAddress.ToString()
            Dim group = CastAide.AsString(Request("ddlUGruop"), String.Empty)


            If group.Equals("a") Then
                memMiniGameComm = IIf(Not Request("ddrCommissionH_OU_OE") Is Nothing, CastAide.AsDouble(Request("ddrCommissionH_OU_OE")), 0)
            Else
                memMiniGameComm = Request("acomhdp")
            End If

            Dim isClose As Boolean = False
            Dim IsSuspened As Boolean = False
            For Each custid In CID

                param.Clear()
                param.Add("firstname", IIf(isMult = "1", String.Empty, Request("txtFName"))) 'nvarchar  
                param.Add("lastname", IIf(isMult = "1", String.Empty, Request("txtLName"))) 'nvarchar  
                If (Request("mode") Is Nothing) Then 'add new
                    param.Add("creator", AppSession.AccId) 'int  
                    param.Add("custid", 0) 'int  
                    param.Add("username", custDetail.UserName)  'varchar  
                    param.Add("userpwd", custDetail.Password) '  ShareFunctions.CfsEnCode(Request("txtPwd").Trim)) 'varchar  
                    param.Add("recommend", agentId) 'int 
                    param.Add("ugroup", group) 'varchar  
                    param.Add("answer", "") 'varchar  
                    param.Add("success", 0) 'bit 
                    param.Add("subaccid", 0) 'int     


                Else ' edit

                    param.Add("custid", custid)  'int  
                    param.Add("userid", Session("AccId"))
                    param.Add("subaccid", 0) ' Sub-account                                  
                    param.Add("userName", "") ' varchar(20) = '' OUTPUT
                    param.Add("updatemultiple", CInt(isMult)) 'bit  

                    param.Add("updatecommission", IIf(isMult = "1", IIf(Request("chkUpdComm") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updateminmaxBet", IIf(isMult = "1", IIf(Request("chkUpdSoccerBet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontaking", IIf(isMult = "1", IIf(Request("chkUpdSoccerPT") Is Nothing, 0, 1), 1)) 'bit  

                    param.Add("updateminmaxbetv2", IIf(isMult = "1", IIf(Request("chkUpd2Bet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontakingv2", IIf(isMult = "1", IIf(Request("chkUpd2PT") Is Nothing, 0, 1), 1)) 'bit  

                    param.Add("updateminmaxbetv3", IIf(isMult = "1", IIf(Request("chkUpd3Bet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontakingv3", IIf(isMult = "1", IIf(Request("chkUpd3PT") Is Nothing, 0, 1), 1)) 'bit  

                    param.Add("updateminmaxbetv5", IIf(isMult = "1", IIf(Request("chkUpd5Bet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontakingv5", IIf(isMult = "1", IIf(Request("chkUpd5PT") Is Nothing, 0, 1), 1)) 'bit  

                    param.Add("updateminmaxbetv8", IIf(isMult = "1", IIf(Request("chkUpd8Bet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontakingv8", IIf(isMult = "1", IIf(Request("chkUpd8PT") Is Nothing, 0, 1), 1)) 'bit  

                    param.Add("updateminmaxbetv10", IIf(isMult = "1", IIf(Request("chkUpd10Bet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontakingv10", IIf(isMult = "1", IIf(Request("chkUpd10PT") Is Nothing, 0, 1), 1)) 'bit  

                    param.Add("updateminmaxbetv11", IIf(isMult = "1", IIf(Request("chkUpd11Bet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontakingv11", IIf(isMult = "1", IIf(Request("chkUpd11PT") Is Nothing, 0, 1), 1)) 'bit  

                    param.Add("updateminmaxbetv99", IIf(isMult = "1", IIf(Request("chkUpd99Bet") Is Nothing, 0, 1), 1)) ' bit
                    param.Add("updatepositiontakingv99", IIf(isMult = "1", IIf(Request("chkUpd99PT") Is Nothing, 0, 1), 1)) 'bit  


                End If
                param.Add("phone", custDetail.Phone) 'nvarchar  
                param.Add("mobilephone", custDetail.MobilePhone) 'nvarchar  
                param.Add("fax", custDetail.Fax)  'varchar  
                param.Add("playerdiscount", IIf(Not Request("ddrCommissionH_OU_OE") Is Nothing, Request("ddrCommissionH_OU_OE"), 0))  'decimal  
                param.Add("playerdiscountcs", IIf(Not Request("ddrCommissionCS_TG") Is Nothing, Request("ddrCommissionCS_TG"), 0))  'decimal  
                ''Add param 1x2 comm
                If (Not WebSettings.Disable1x2Comm) Then
                    param.Add("playerdiscount1x2", IIf(Not Request("ddrCommissionCS_1x2") Is Nothing, Request("ddrCommissionCS_1x2"), 0))
                End If

                param.Add("credit", IIf(Not Request("txtCredit") Is Nothing, Request("txtCredit"), 0))  'money  

                If Not Request("hidClosed") Is Nothing Then
                    isClose = CastAide.AsBoolean(CastAide.AsInt32(Request("hidClosed")))
                End If

                If Not (Request("chkClosed")) Is Nothing Then
                    isClose = CastAide.AsBoolean(IIf(Request("chkClosed").Equals("on"), 1, 0))
                End If
                param.Add("closed", isClose) 'bit  


                If Not Request("hidSuspended") Is Nothing Then
                    IsSuspened = CastAide.AsBoolean(CastAide.AsInt32(Request("hidSuspended")))
                End If

                If Not (Request("chkSuspended")) Is Nothing Then
                    IsSuspened = CastAide.AsBoolean(IIf(Request("chkSuspended").Equals("on"), 1, 0))
                End If

                param.Add("suspended", IsSuspened)
                param.Add("suspendedFlag", "Acc")

                Dim numbergame As NumberGameManager = New NumberGameManager()
                Dim context As HttpContext = HttpContext.Current
                numbergame.AddParamMemberPT(param, context)

                AddParaAllSportype()
                param.Add("ip", ip)
                param.Add("language", "E")

                param.Add("errmsg", "") 'nvarchar  
                If (Request("mode") Is Nothing) Then 'insert
                    param.Add("errcode", 0) 'int  
                    rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.AgentInsUdpMember(param, True, custid)
                ElseIf Not (Request("mode") Is Nothing) Then

                    If (Request("mode") = "edit") Then 'edit      
                        param.Add("errcode", "") 'int  
                        rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.AgentInsUdpMember(param, False, custid)
                    End If
                End If
                msg = rs.ErrorMsg
                Dim parent As String = "parent."
                If Not rs.IsSuccess() Then
                    Exit For
                    Exit Sub
                End If
            Next

            If Not rs.IsSuccess() Then
                If (Request("mode") Is Nothing) And custid <> String.Empty Then
                    Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI();parent.ShowError('" & AlertMessage.GetMsg(msg, AlertMessage.MsgType.Error) & "','err');</script>")
                Else 'Enrich message
                    Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI();parent.ShowError('" & msg & "','err');</script>")
                End If


            Else
                If (Request("mode") Is Nothing) Then
                    Dim strUrl As String = ""

                    Dim uplineUserName As String = CastAide.AsString(Request("uplineusername"), "")


                    If (WebSettings.disabledFI) Then

                        Response.Write("<script type=""text/javascript"">parent.DelayReloadPage('../MemberList.aspx?agentId=" & agentId & "&hr=" & ((Not (Request("hdAllowedCurr") = 0)) And (WebSettings.disabledHR = False)).ToString() & "',2000);</script>")
                    Else
                        strUrl = "UpdatePT.aspx?custid=" & custid & "&insert=true" & "&roleid=1" & "&uplineid=" & agentId
                        Response.Write("<script> if (parent.name=='centerFr') {parent.DelayReloadPage('" + strUrl + "',3000)} else {parent.DelayReloadPage('', 4000); setTimeout('ReLoad()', 2000)}; function ReLoad() {parent.location.replace('" + strUrl + "')}</script>")
                    End If


                    'Auto insert casino - bingo account
                    Dim csCustomerInfo = New Sunplus.Accounting.Entities.Casino.CSCustomerDetail()
                    csCustomerInfo.CasinoNo = Globals.CASINO_GROUP1
                    csCustomerInfo.IBCCustID = custid
                    csCustomerInfo.UserName = custDetail.UserName
                    csCustomerInfo.UserName2 = custDetail.UserName
                    csCustomerInfo.FirstName = custDetail.FirstName
                    csCustomerInfo.LastName = custDetail.LastName
                    csCustomerInfo.UserPwd = custDetail.Password
                    csCustomerInfo.Phone = custDetail.Phone
                    csCustomerInfo.MobilePhone = custDetail.MobilePhone
                    csCustomerInfo.ExchangeID = custDetail.CurrencyId
                    csCustomerInfo.Fax = custDetail.Fax
                    csCustomerInfo.Suspend = custDetail.Suspend
                    csCustomerInfo.Closed = custDetail.Closed
                    csCustomerInfo.IsEnablePT = False
                    csCustomerInfo.SIBCCustID = custDetail.SuperId
                    csCustomerInfo.MIBCCustID = custDetail.MasterId
                    csCustomerInfo.AIBCCustID = custDetail.AgentId
                    csCustomerInfo.RoleId = Globals.AccountType.Member
                    csCustomerInfo.CasinoDisabled = custDetail.IsUplineDisableCasino

                Dim custService = New ServiceCustomerInfo()
                custService.AutoInsertThirdPartyAccount(csCustomerInfo, custDetail, AppSession.AccId, AppSession.Username, Sunplus.Accounting.Business.BaseManager.CurrentUserIp)

                Else
                    Response.Write("<script>parent.showAlertMessage('err','Update Member successfully',0);parent.PTUnBlockUI();parent.afterUpdate(true,2000); </script>")
                End If
                Response.Write("</script>")

                ' Synchronize to casino
                If (Request("mode") = "edit") Then 'edit
                    ' Update for casino
                    Try
                        If (Permissions.IsAllowCasino() And (Context.Request("hidSynCS") <> "False") And Not isMult) Then
                            ' Store data for casino section
                            Dim custInfo As CSCustomerDetail = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetCustInfoIBC(param("custid").ToString(), param)
                            ' Sync data to Casino
                            Dim csCustomer As CasinoLibrary.SBAgent.Customer = New CasinoLibrary.SBAgent.Customer()
                            'csCustomer.SiteName = New Sunplus.Accounting.Business.Customer.CustomerManager().GetRNGSiteNameByCustId(custid)
                            csCustomer.SiteName = custInfo.SiteName
                            Dim status As Boolean = csCustomer.UpdateCustInfo(custInfo.IBCCustID, custInfo.FirstName, custInfo.LastName, custInfo.Phone, custInfo.MobilePhone, custInfo.Fax, custInfo.Closed, custInfo.Suspend, Request.UserHostAddress, AppSession.CustID, AppSession.Username, 3)
                        End If
                    Catch ex As Exception
                        LogExtensions.ExceptionHandler(ex)
                    End Try

                End If
            End If
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Function AddParaSoccer()
        If Not (Request("chkCopy1HdpDeadball")) Is Nothing Then 'Dead ball Handicap
            param.Add("apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_7", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_8", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_2", Request("listM1HdpD")) 'smallmoney  
        Else
            param.Add("apositiontaking_1", IIf(Not Request("listM1HdpD") Is Nothing, Request("listM1HdpD"), 0))  'smallmoney  
            param.Add("apositiontaking_3", IIf(Not Request("listM1OUD") Is Nothing, Request("listM1OUD"), 0))  'smallmoney 
            param.Add("apositiontaking_7", IIf(Not Request("listM11stHdpD") Is Nothing, Request("listM11stHdpD"), 0))  'smallmoney  
            param.Add("apositiontaking_8", IIf(Not Request("listM11stOUD") Is Nothing, Request("listM11stOUD"), 0))  'smallmoney  
            param.Add("apositiontaking_2", IIf(Not Request("listM1OE") Is Nothing, Request("listM1OE"), 0)) 'smallmoney  
        End If

        If Not (Request("chkCopy11x2")) Is Nothing Then 'Dead ball 1X2
            param.Add("apositiontaking_5", Request("listM11x2"))  'smallmoney  
            param.Add("apositiontaking_4", Request("listM11x2")) 'smallmoney  
            param.Add("apositiontaking_6", Request("listM11x2")) 'smallmoney  
            param.Add("apositiontaking_9", Request("listM11x2")) 'smallmoney  
            param.Add("apositiontaking_10", Request("listM11x2")) 'smallmoney  
        Else
            param.Add("apositiontaking_5", IIf(Not Request("listM11x2") Is Nothing, Request("listM11x2"), 0))  'smallmoney  
            param.Add("apositiontaking_4", IIf(Not Request("listM1CS") Is Nothing, Request("listM1CS"), 0)) 'smallmoney  
            param.Add("apositiontaking_6", IIf(Not Request("listM1TG") Is Nothing, Request("listM1TG"), 0)) 'smallmoney  
            param.Add("apositiontaking_9", IIf(Not Request("listM1MP") Is Nothing, Request("listM1MP"), 0)) 'smallmoney  
            param.Add("apositiontaking_10", IIf(Not Request("listM1OR") Is Nothing, Request("listM1OR"), 0))  'smallmoney  
        End If

        If Not (Request("chkCopy1HdpLive")) Is Nothing Then ' Live Handicap
            param.Add("alivepositiontaking_1", Request("listM1HdpL")) 'smallmoney  
            param.Add("alivepositiontaking_3", Request("listM1HdpL")) 'smallmoney  
            param.Add("alivepositiontaking_7", Request("listM1HdpL")) 'smallmoney  
            param.Add("alivepositiontaking_8", Request("listM1HdpL")) 'smallmoney  
        Else
            param.Add("alivepositiontaking_1", IIf(Not Request("listM1HdpL") Is Nothing, Request("listM1HdpL"), 0)) 'smallmoney  
            param.Add("alivepositiontaking_3", IIf(Not Request("listM1OUL") Is Nothing, Request("listM1OUL"), 0))  'smallmoney  
            param.Add("alivepositiontaking_7", IIf(Not Request("listM11stHdpL") Is Nothing, Request("listM11stHdpL"), 0)) 'smallmoney  
            param.Add("alivepositiontaking_8", IIf(Not Request("listM11stOUL") Is Nothing, Request("listM11stOUL"), 0)) 'smallmoney  
        End If
    End Function
#End Region

#Region "Insert parameters for AddPara Basketball Position Taking"
    Function AddParaBasketball()
        If Not (Request("chkCopy2Hdp")) Is Nothing Then
            param.Add("v2apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_2", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_7", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_8", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_9", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_20", Request("listM2Hdp")) 'smallmoney  
        Else
            param.Add("v2apositiontaking_1", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_2", IIf(Not Request("listM2OE") Is Nothing, Request("listM2OE"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_3", IIf(Not Request("listM2OU") Is Nothing, Request("listM2OU"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_7", IIf(Not Request("listM2Hdp") Is Nothing, Request("listM2Hdp"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_8", IIf(Not Request("listM2OU") Is Nothing, Request("listM2OU"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_9", IIf(Not Request("listM2MP") Is Nothing, Request("listM2MP"), 0)) 'smallmoney  
            param.Add("v2apositiontaking_20", IIf(Not Request("listM2ML") Is Nothing, Request("listM2ML"), 0)) 'smallmoney  
        End If
    End Function
#End Region

#Region "Insert parameters for AddPara Football Position Taking"
    Function AddParaFootball()
        If Not (Request("chkCopy3Hdp")) Is Nothing Then
            param.Add("v3apositiontaking_1", Request("listM3Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_2", Request("listM3Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_3", Request("listM3Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_20", Request("listM3Hdp")) 'smallmoney  
        Else
            param.Add("v3apositiontaking_1", IIf(Not Request("listM3Hdp") Is Nothing, Request("listM3Hdp"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_2", IIf(Not Request("listM3OE") Is Nothing, Request("listM3OE"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_3", IIf(Not Request("listM3OU") Is Nothing, Request("listM3OU"), 0)) 'smallmoney  
            param.Add("v3apositiontaking_20", IIf(Not Request("listM3ML") Is Nothing, Request("listM3ML"), 0))  'smallmoney  
        End If

    End Function
#End Region

#Region "Insert parameters for AddPara Tennis Position Taking"
    Function AddParaTennis()
        If Not (Request("chkCopy5Hdp")) Is Nothing Then
            param.Add("v5apositiontaking_1", Request("listM5Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_2", Request("listM5Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_3", Request("listM5Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_20", Request("listM5Hdp")) 'smallmoney  
        Else
            param.Add("v5apositiontaking_1", IIf(Not Request("listM5Hdp") Is Nothing, Request("listM5Hdp"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_2", IIf(Not Request("listM5OE") Is Nothing, Request("listM5OE"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_3", IIf(Not Request("listM5OU") Is Nothing, Request("listM5OU"), 0)) 'smallmoney  
            param.Add("v5apositiontaking_20", IIf(Not Request("listM5ML") Is Nothing, Request("listM5ML"), 0)) 'smallmoney  
        End If

    End Function
#End Region

#Region "Insert parameters for AddPara Baseball Position Taking"
    Function AddParaBaseball()
        If Not (Request("chkCopy8Hdp")) Is Nothing Then
            param.Add("v8apositiontaking_1", Request("listM8Hdp")) 'smallmoney  
            param.Add("v8apositiontaking_3", Request("listM8Hdp")) 'smallmoney  
            param.Add("v8apositiontaking_20", Request("listM8Hdp")) 'smallmoney  
        Else
            param.Add("v8apositiontaking_1", IIf(Not Request("listM8Hdp") Is Nothing, Request("listM8Hdp"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_3", IIf(Not Request("listM8OU") Is Nothing, Request("listM8OU"), 0)) 'smallmoney  
            param.Add("v8apositiontaking_20", IIf(Not Request("listM8ML") Is Nothing, Request("listM8ML"), 0)) 'smallmoney  
        End If

    End Function
#End Region

#Region "Insert parameters for AddPara Golf Position Taking"
    Function AddParaGolf()
        If Not (Request("chkCopy10Hdp")) Is Nothing Then
            param.Add("v10apositiontaking_1", Request("listM10Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_2", Request("listM10Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_3", Request("listM10Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_20", Request("listM10Hdp")) 'smallmoney  
        Else
            param.Add("v10apositiontaking_1", IIf(Not Request("listM10Hdp") Is Nothing, Request("listM10Hdp"), 0)) 'smallmoney  
            param.Add("v10apositiontaking_2", IIf(Not Request("listM10OE") Is Nothing, Request("listM10OE"), 0))   'smallmoney  
            param.Add("v10apositiontaking_3", IIf(Not Request("listM10OU") Is Nothing, Request("listM10OU"), 0))   'smallmoney  
            param.Add("v10apositiontaking_20", IIf(Not Request("listM10ML") Is Nothing, Request("listM10ML"), 0))  'smallmoney  
        End If

    End Function
#End Region

#Region "Insert parameters for AddPara Motorsports Position Taking"
    Function AddParaMotosports()
        If Not (Request("chkCopy11Hdp")) Is Nothing Then
            param.Add("v11apositiontaking_1", Request("listM11Hdp")) 'smallmoney  
            param.Add("v11apositiontaking_20", Request("listM11Hdp")) 'smallmoney  
        Else
            param.Add("v11apositiontaking_1", IIf(Not Request("listM11Hdp") Is Nothing, Request("listM11Hdp"), 0)) 'smallmoney  
            param.Add("v11apositiontaking_20", IIf(Not Request("listM11ML") Is Nothing, Request("listM11ML"), 0))  'smallmoney  
        End If

    End Function
#End Region

#Region "Insert parameters for AddPara Other Sports Position Taking"
    Function AddParaOtherSports()
        If Not (Request("chkCopy99Hdp")) Is Nothing Then
            param.Add("v99apositiontaking_1", Request("listM99Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_2", Request("listM99Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_3", Request("listM99Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_20", Request("listM99Hdp")) 'smallmoney  
        Else
            param.Add("v99apositiontaking_1", IIf(Not Request("listM99Hdp") Is Nothing, Request("listM99Hdp"), 0))  'smallmoney  
            param.Add("v99apositiontaking_2", IIf(Not Request("listM99OE") Is Nothing, Request("listM99OE"), 0))   'smallmoney  
            param.Add("v99apositiontaking_3", IIf(Not Request("listM99OU") Is Nothing, Request("listM99OU"), 0))    'smallmoney  
            param.Add("v99apositiontaking_20", IIf(Not Request("listM99ML") Is Nothing, Request("listM99ML"), 0))   'smallmoney  
        End If


    End Function
#End Region

#Region "Assign parameters for Mixed Sports Position Taking"
    Function AddParamMixedSports()
        Dim mixedSports As MixedSportsManager = New MixedSportsManager()
        Dim updatePt As Byte = 1
        Dim updateMinMaxBet As Byte = 1
        Dim minBet, maxBet, maxPerMatch, aPt As Object

        If Not (Request("chkCopyAll")) Is Nothing Then
            minBet = Request("txtMinBet1")
            maxBet = Request("txtMaxBet1")
            maxPerMatch = Request("txtMaxPerMatch1")
            aPt = Request("listM1HdpD")
        ElseIf Not (Request("chkCopyBasket")) Is Nothing Then
            minBet = Request("txtMinBet2")
            maxBet = Request("txtMaxBet2")
            maxPerMatch = Request("txtMaxPerMatch2")
            aPt = Request("listM2Hdp")
        Else
            minBet = IIf(Not Request("txtMinbet99_9") Is Nothing, Request("txtMinbet99_9"), 0)
            maxBet = IIf(Not Request("txtMaxbet99_9") Is Nothing, Request("txtMaxbet99_9"), 0)
            maxPerMatch = IIf(Not Request("txtMaxPerMatch99_9") Is Nothing, Request("txtMaxPerMatch99_9"), 0)
            aPt = Request("listMMixParlay")
        End If
        mixedSports.AddParamMemberPT(param, minBet, maxBet, maxPerMatch, aPt, updatePt, updateMinMaxBet)

    End Function
#End Region

#Region "Insert parameters for All Sports Type Position Taking "
    Function AddParaAllSportype()
        If Not (Request("chkCopyAll")) Is Nothing Then 'when Copy all position taking settings from Soccer Handicap is checked
            'Soccer
            param.Add("apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_7", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_8", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_2", Request("listM1HdpD")) 'smallmoney  

            param.Add("apositiontaking_5", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_4", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_6", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_9", Request("listM1HdpD")) 'smallmoney  
            param.Add("apositiontaking_10", Request("listM1HdpD")) 'smallmoney  

            param.Add("alivepositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("alivepositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("alivepositiontaking_7", Request("listM1HdpD")) 'smallmoney  
            param.Add("alivepositiontaking_8", Request("listM1HdpD")) 'smallmoney  

            'Basketball
            param.Add("v2apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("v2apositiontaking_2", Request("listM1HdpD")) 'smallmoney  
            param.Add("v2apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("v2apositiontaking_7", Request("listM1HdpD")) 'smallmoney  
            param.Add("v2apositiontaking_8", Request("listM1HdpD")) 'smallmoney  
            param.Add("v2apositiontaking_9", Request("listM1HdpD")) 'smallmoney  
            param.Add("v2apositiontaking_20", Request("listM1HdpD")) 'smallmoney  

            'Football
            param.Add("v3apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("v3apositiontaking_2", Request("listM1HdpD")) 'smallmoney  
            param.Add("v3apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("v3apositiontaking_20", Request("listM1HdpD")) 'smallmoney  

            'Tennis
            param.Add("v5apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("v5apositiontaking_2", Request("listM1HdpD")) 'smallmoney  
            param.Add("v5apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("v5apositiontaking_20", Request("listM1HdpD")) 'smallmoney  

            'Baseball
            param.Add("v8apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("v8apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("v8apositiontaking_20", Request("listM1HdpD")) 'smallmoney  

            'Golf
            param.Add("v10apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("v10apositiontaking_2", Request("listM1HdpD")) 'smallmoney  
            param.Add("v10apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("v10apositiontaking_20", Request("listM1HdpD")) 'smallmoney  

            'Motorsports
            param.Add("v11apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("v11apositiontaking_20", Request("listM1HdpD")) 'smallmoney 

            'Other Sports
            param.Add("v99apositiontaking_1", Request("listM1HdpD")) 'smallmoney  
            param.Add("v99apositiontaking_2", Request("listM1HdpD")) 'smallmoney  
            param.Add("v99apositiontaking_3", Request("listM1HdpD")) 'smallmoney  
            param.Add("v99apositiontaking_20", Request("listM1HdpD")) 'smallmoney  

            param.Add("minbet", Request("txtMinBet1")) 'money  
            param.Add("maxbet", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money  

            param.Add("minbetv2", Request("txtMinBet1")) 'money  
            param.Add("maxbetv2", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatchv2", Request("txtMaxPerMatch1")) 'money  

            param.Add("minbetv3", Request("txtMinBet1")) 'money  
            param.Add("maxbetv3", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatchv3", Request("txtMaxPerMatch1")) 'money  

            param.Add("minbetv5", Request("txtMinBet1")) 'money  
            param.Add("maxbetv5", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatchv5", Request("txtMaxPerMatch1")) 'money  

            param.Add("minbetv8", Request("txtMinBet1")) 'money  
            param.Add("maxbetv8", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatchv8", Request("txtMaxPerMatch1")) 'money  

            param.Add("minbetv10", Request("txtMinBet1")) 'money  
            param.Add("maxbetv10", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatchv10", Request("txtMaxPerMatch1")) 'money  

            param.Add("minbetv11", Request("txtMinBet1")) 'money  
            param.Add("maxbetv11", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatchv11", Request("txtMaxPerMatch1")) 'money  


            If (Request("mode") Is Nothing) Then 'add new
                param.Add("minbetv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, Request("txtMinBet1"))) 'money  
                param.Add("maxbetv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, Request("txtMaxBet1"))) 'money  
                param.Add("maxpermatchv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, Request("txtMaxPerMatch1"))) 'money 
            End If

            param.Add("minbetv99", Request("txtMinBet1")) 'money  
            param.Add("maxbetv99", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatchv99", Request("txtMaxPerMatch1")) 'money  

        ElseIf Not (Request("chkCopyBasket")) Is Nothing Then
            AddParaSoccer()

            'Basketball
            param.Add("v2apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_2", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_7", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_8", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_9", Request("listM2Hdp")) 'smallmoney  
            param.Add("v2apositiontaking_20", Request("listM2Hdp")) 'smallmoney  

            'Football
            param.Add("v3apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_2", Request("listM2Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v3apositiontaking_20", Request("listM2Hdp")) 'smallmoney  

            'Tennis
            param.Add("v5apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_2", Request("listM2Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v5apositiontaking_20", Request("listM2Hdp")) 'smallmoney  

            'Baseball
            param.Add("v8apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v8apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v8apositiontaking_20", Request("listM2Hdp")) 'smallmoney  

            'Golf
            param.Add("v10apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_2", Request("listM2Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v10apositiontaking_20", Request("listM2Hdp")) 'smallmoney  

            'Motorsports
            param.Add("v11apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v11apositiontaking_20", Request("listM2Hdp")) 'smallmoney  


            'Other Sports
            param.Add("v99apositiontaking_1", Request("listM2Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_2", Request("listM2Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_3", Request("listM2Hdp")) 'smallmoney  
            param.Add("v99apositiontaking_20", Request("listM2Hdp")) 'smallmoney  

            param.Add("minbet", Request("txtMinBet1")) 'money  
            param.Add("maxbet", Request("txtMaxBet1")) 'money  
            param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money  

            param.Add("minbetv2", Request("txtMinBet2")) 'money  
            param.Add("maxbetv2", Request("txtMaxBet2")) 'money  
            param.Add("maxpermatchv2", Request("txtMaxPerMatch2")) 'money  

            param.Add("minbetv3", Request("txtMinBet2")) 'money  
            param.Add("maxbetv3", Request("txtMaxBet2")) 'money  
            param.Add("maxpermatchv3", Request("txtMaxPerMatch2")) 'money  

            param.Add("minbetv5", Request("txtMinBet2")) 'money  
            param.Add("maxbetv5", Request("txtMaxBet2")) 'money  
            param.Add("maxpermatchv5", Request("txtMaxPerMatch2")) 'money  

            param.Add("minbetv8", Request("txtMinBet2")) 'money  
            param.Add("maxbetv8", Request("txtMaxBet2")) 'money  
            param.Add("maxpermatchv8", Request("txtMaxPerMatch2")) 'money  

            param.Add("minbetv10", Request("txtMinBet2")) 'money  
            param.Add("maxbetv10", Request("txtMaxBet2")) 'money  
            param.Add("maxpermatchv10", Request("txtMaxPerMatch2")) 'money  

            param.Add("minbetv11", Request("txtMinBet2")) 'money  
            param.Add("maxbetv11", Request("txtMaxBet2")) 'money  
            param.Add("maxpermatchv11", Request("txtMaxPerMatch2")) 'money  


            If (Request("mode") Is Nothing) Then 'add new
                param.Add("minbetv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, Request("txtMinBet2"))) 'money  
                param.Add("maxbetv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, Request("txtMaxBet2"))) 'money  
                param.Add("maxpermatchv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, Request("txtMaxPerMatch2"))) 'money 
            End If
            'param.Add("minbetv151", Request("txtMinBet2")) 'money  
            'param.Add("maxbetv151", Request("txtMaxBet2")) 'money  
            'param.Add("maxpermatchv151", Request("txtMaxPerMatch2")) 'money  

            param.Add("minbetv99", Request("txtMinBet2")) 'money  
            param.Add("maxbetv99", Request("txtMaxBet2")) 'money  
            param.Add("maxpermatchv99", Request("txtMaxPerMatch2")) 'money  

        Else
            param.Add("minbet", IIf(Not Request("txtMinBet1") Is Nothing, Request("txtMinBet1"), 0))  'money  
            param.Add("maxbet", IIf(Not Request("txtMaxBet1") Is Nothing, Request("txtMaxBet1"), 0)) 'money  
            param.Add("maxpermatch", IIf(Not Request("txtMaxPerMatch1") Is Nothing, Request("txtMaxPerMatch1"), 0)) 'money  

            param.Add("minbetv2", IIf(Not Request("txtMinBet2") Is Nothing, Request("txtMinBet2"), 0)) 'money  
            param.Add("maxbetv2", IIf(Not Request("txtMaxBet2") Is Nothing, Request("txtMaxBet2"), 0)) 'money  
            param.Add("maxpermatchv2", IIf(Not Request("txtMaxPerMatch2") Is Nothing, Request("txtMaxPerMatch2"), 0))  'money  

            param.Add("minbetv3", IIf(Not Request("txtMinBet3") Is Nothing, Request("txtMinBet3"), 0))  'money  
            param.Add("maxbetv3", IIf(Not Request("txtMaxBet3") Is Nothing, Request("txtMaxBet3"), 0))   'money  
            param.Add("maxpermatchv3", IIf(Not Request("txtMaxPerMatch3") Is Nothing, Request("txtMaxPerMatch3"), 0)) 'money  

            param.Add("minbetv5", IIf(Not Request("txtMinBet5") Is Nothing, Request("txtMinBet5"), 0)) 'money  
            param.Add("maxbetv5", IIf(Not Request("txtMaxBet5") Is Nothing, Request("txtMaxBet5"), 0))  'money  
            param.Add("maxpermatchv5", IIf(Not Request("txtMaxPerMatch5") Is Nothing, Request("txtMaxPerMatch5"), 0))  'money  

            param.Add("minbetv8", IIf(Not Request("txtMinBet8") Is Nothing, Request("txtMinBet8"), 0))  'money  
            param.Add("maxbetv8", IIf(Not Request("txtMaxBet8") Is Nothing, Request("txtMaxBet8"), 0))  'money  
            param.Add("maxpermatchv8", IIf(Not Request("txtMaxPerMatch8") Is Nothing, Request("txtMaxPerMatch8"), 0))  'money  

            param.Add("minbetv10", IIf(Not Request("txtMinBet10") Is Nothing, Request("txtMinBet10"), 0))  'money  
            param.Add("maxbetv10", IIf(Not Request("txtMaxBet10") Is Nothing, Request("txtMaxBet10"), 0))   'money  
            param.Add("maxpermatchv10", IIf(Not Request("txtMaxPerMatch10") Is Nothing, Request("txtMaxPerMatch10"), 0))    'money  

            param.Add("minbetv11", IIf(Not Request("txtMinBet11") Is Nothing, Request("txtMinBet11"), 0))   'money  
            param.Add("maxbetv11", IIf(Not Request("txtMaxBet11") Is Nothing, Request("txtMaxBet11"), 0))    'money  
            param.Add("maxpermatchv11", IIf(Not Request("txtMaxPerMatch11") Is Nothing, Request("txtMaxPerMatch11"), 0))   'money  

            If (Request("mode") Is Nothing) Then 'add new
                param.Add("minbetv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, IIf(Not Request("txtMinBet151") Is Nothing, Request("txtMinBet151"), 0))) 'money  
                param.Add("maxbetv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, IIf(Not Request("txtMaxBet151") Is Nothing, Request("txtMaxBet151"), 0))) 'money  
                param.Add("maxpermatchv151", IIf(Request("hdAllowedCurr") = 0 Or WebSettings.disabledHR = True, 0, IIf(Not Request("txtMaxPerMatch151") Is Nothing, Request("txtMaxPerMatch151"), 0))) 'money 
            End If
            'param.Add("minbetv151", IIf(Not Request("txtMinBet151") Is Nothing, Request("txtMinBet151"), 0))   'money  
            'param.Add("maxbetv151", IIf(Not Request("txtMaxBet151") Is Nothing, Request("txtMaxBet151"), 0))    'money  
            'param.Add("maxpermatchv151", IIf(Not Request("txtMaxPerMatch151") Is Nothing, Request("txtMaxPerMatch151"), 0))   'money 

            param.Add("minbetv99", IIf(Not Request("txtMinBet99") Is Nothing, Request("txtMinBet99"), 0))   'money  
            param.Add("maxbetv99", IIf(Not Request("txtMaxBet99") Is Nothing, Request("txtMaxBet99"), 0))    'money  
            param.Add("maxpermatchv99", IIf(Not Request("txtMaxPerMatch99") Is Nothing, Request("txtMaxPerMatch99"), 0))  'money  

            AddParaSoccer()
            AddParaBasketball()
            AddParaFootball()
            AddParaTennis()

            AddParaBaseball()
            AddParaGolf()
            AddParaMotosports()
            AddParaOtherSports()
        End If

        'mixed sports
        AddParamMixedSports()
    End Function
#End Region
End Class
