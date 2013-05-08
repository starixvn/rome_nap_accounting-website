Imports System.Collections
Imports Sunplus.Accounting.WcfServiceLibrary.Customer
Imports Sunplus.Accounting.WcfServiceLibrary.PT.Casino
Imports Sunplus.Accounting.WcfServiceLibrary.Bingo
Imports Sunplus.Accounting.Business.PT.Bingo
Imports Sunplus.Accounting.Entities.Casino
Imports Sunplus.Accounting.Business.PT.Casino
Imports Sunplus.Accounting.Business.BootStrappers
Imports Sunplus.Accounting.DataAccess.IBCDLL.Customer
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Common
Imports DAL
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports IBCDLL.Customer
Imports IBCUtils.Encryption
Imports DNAide.Core
Imports System.Collections.Generic
Imports System.Linq
Imports System.Data
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Core.Helper



Partial Class Customer_V2_UpdateNewMaster
    Inherits RequiredAuthorizationPage
    Dim param As New Hashtable
    Dim isCopyAll As Boolean = False
    Dim isCopyBasket As Boolean = False

    Protected Overrides Function HasPermission() As Boolean
        Return True
    End Function

    Protected Overrides ReadOnly Property CustId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.SuperId)
        End Get
    End Property

    Protected Overrides Sub RedirectError()

        Response.Write("<script type=""text/javascript"">parent.showAlertMessage('err','" & StringUtils.ParseToJSMsg(Resources.ErrorMsg.error_InvalidRequest) & "',1);parent.PTUnBlockUI();</script>")

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim errmsg As String = ""
        Try
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            If Permissions.CheckPermission(CastAide.AsInt32(Session("AccId")), Enumerations.Functions.Customer.OtherCust, errmsg) <> 0 Then
                Response.Write("<script type=""text/javascript"">parent.ShowError('" & AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) & "','err'); parent.PTUnBlockUI();</script>")
            Else
                InsertData()
            End If
        Catch ex As Exception

            Response.Write("<script type=""text/javascript"">parent.ShowError('" & AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) & "','err'); parent.PTUnBlockUI();</script>")


        End Try
    End Sub

#Region "Update data"
    Sub InsertData()
        Try
            Dim resultUpdated As New Hashtable
            Dim isInternal As Boolean = CastAide.AsBoolean(Request("hidinternal"), False)
            
            If (isInternal = False) And (Not Request("chkInternal") Is Nothing) Then
                isInternal = IIf(Request("chkInternal") = "on", True, False)
            End If

            Dim custDetail = Sunplus.Accounting.Business.Customer.CustomerService.GetCustInfo(Request)
            Dim groupa = Request("groupa")
            Dim ip = Request.UserHostAddress.ToString()

            param.Add("custid", 0) 'int  
            param.Add("firstname", custDetail.FirstName) 'nvarchar  
            param.Add("lastname", custDetail.LastName) 'nvarchar  
            param.Add("creator", AppSession.AccId) 'int  
            param.Add("username", custDetail.UserName)  'varchar  
            param.Add("userpwd", custDetail.Password) 'varchar  
            param.Add("phone", custDetail.Phone) 'nvarchar  
            param.Add("mobilephone", custDetail.MobilePhone) 'nvarchar  
            param.Add("fax", custDetail.Fax) 'nvarchar  
            param.Add("email", custDetail.Email) 'nvarchar  
            param.Add("address", custDetail.Address) 'nvarchar  
            param.Add("city", custDetail.City) 'nvarchar  
            param.Add("state", custDetail.State) 'nvarchar  
            param.Add("country", custDetail.Country) 'nvarchar  
            param.Add("postal", custDetail.Postal) 'nvarchar 


            If Not WebSettings.DisableInternal Then
                param.Add("isinternal", isInternal)
            End If

            If (Not WebSettings.Disable1x2Comm) Then
                param.Add("discount1x2", Request("discountcs1x2"))
            End If

            'param.Add("disablefinancial", IIf(Request("isDisableFI") = "yes", True, False))
            Dim IsSuspened As Boolean = False
            IsSuspened = IIf(Request("chkSuspended").Equals("1"), True, False)
            param.Add("suspended", IsSuspened)
            param.Add("suspendedflag", "Acc")

            Dim strUpdTranfer As String = ""
            If Request("referupline") = "1" Then
                strUpdTranfer = ""
            Else If Request("tranfer") = "daily" Then
                strUpdTranfer = "00000000" '
            Else
                If Not Request("chkSun") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkMon") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkTue") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkWed") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkThurs") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkFri") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                If Not Request("chkSat") Is Nothing Then
                    strUpdTranfer &= "1"
                Else
                    strUpdTranfer &= "0"
                End If
                strUpdTranfer &= "1"
            End If
            param.Add("transferoption", strUpdTranfer) 'varchar  
            If Not Request("chkCopyAll") Is Nothing Then
                isCopyAll = True
            End If
            If Not Request("chkCopyBasket") Is Nothing Then
                isCopyBasket = True
            End If
            param.Add("credit", Request("txtCredit")) 'money  
            param.Add("srecommend", Request("superId")) 'int 

            param.Add("groupa", groupa) 'decimal  
            param.Add("groupb", Request("groupb")) 'decimal  
            param.Add("groupc", Request("groupc")) 'decimal  
            param.Add("groupd", Request("groupd")) 'decimal  
            param.Add("discountcs", Request("discountcs")) 'decimal       
            If (Permissions.IsEnabledAutoMaster(True)) Then
                param.Add("disableautompt", IIf(Request("chkautompt") = "on", "False", "True"))
            End If


            If Session("lang") = "en-US" Then
                param.Add("language", "E")
            ElseIf Session("lang") = "zh-CN" Then
                param.Add("language", "C")
            ElseIf Session("lang") = "ja-JP" Then
                param.Add("language", "J")
            End If
            param.Add("subaccid", 0) 'int     
            param.Add("ip", ip)

            param.Add("errcode", 0) 'int  
            param.Add("errmsg", "") 'nvarchar  
            Dim strSite As String = IIf(Request("hidSite") Is Nothing, "", Request("hidSite"))
            param.Add("site", strSite) 'varchar  
            AddParaSoccer()
            AddParaBasketball(2)
            AddParaFootball(3)
            AddParaFootball(5)
            AddParaBaseball()
            AddParaFootball(10)
            AddParaMotosports()
            AddParaFootball(99)

            Dim numbergame As NumberGameManager = New NumberGameManager()
            Dim context As HttpContext = HttpContext.Current
            numbergame.AddParamMasterPT(param, context, isCopyAll, isCopyBasket, False)

            Dim mixedSports As MixedSportsManager = New MixedSportsManager()
            mixedSports.AddParamMasterPT(param, context)
            resultUpdated = UpdateCustomer.CustomerInsMemberSAgRpt(param)
            If Not resultUpdated("errCode") Is Nothing Then
                If CInt(resultUpdated("errCode")) <> 0 Then
                    Response.Write("<script type=""text/javascript"">parent.ShowError('" & AlertMessage.GetMsg(resultUpdated("errMsg"), AlertMessage.MsgType.Error) & "','err'); parent.PTUnBlockUI();</script>")
                Else
                    
                    Dim strQuery As String = "&strSearch=search&txtSearch=" & Request("txtUsername")
                    Dim strUrl As String = "parent.location.href"
                    Dim isInsert As String = "false"
                    If Not Request("set") Is Nothing And Request("set") = "1" Then
                        strUrl = "location.href"
                        isInsert = "true"
                    End If
                    Dim uplineId As Integer = CastAide.AsInt32(Request("uplineid"), 0)
                    Dim custId = resultUpdated("superId")

                    If Not WebSettings.disabledFI Then
                        strUrl = "UpdatePT.aspx?custid=" & custId & "&insert=" & isInsert & "&roleid=3" & "&uplineid=" & uplineId
                        Response.Write("<script> if (parent.name=='centerFr') {parent.DelayReloadPage('" + strUrl + "',3000)} else if (parent.parent.name=='centerFr') {parent.DelayReloadPage('" + strUrl + "',3000)}  else {parent.DelayReloadPage('', 3000); setTimeout('ReLoad()', 3000)}; function ReLoad() {parent.PTUnBlockUI(); parent.location.replace('" + strUrl + "')}</script>")
                    Else
                        If CInt(Request("superId")) < 100 Then
                            Response.Write("<script type=""text/javascript"">parent.DelayReloadPage('../MasterAgentList.aspx?bycom=1" & strQuery & "&hr=" & (WebSettings.disabledHR = False).ToString() & "',2000,'" & strUrl & "');</script>")
                        Else
                            Response.Write("<script type=""text/javascript"">parent.DelayReloadPage('../MasterAgentList.aspx?superId=" + context.Request("superId") + "" & strQuery & "&hr=" & ((Not (Request("hdAllowedCurr") = 0)) And (WebSettings.disabledHR = False)).ToString() & "',2000,'" & strUrl & "');</script>")
                        End If
                    End If

                    'Auto insert casino - bingo account
                    custDetail.MasterId = custId
                    Dim csCustomerInfo = New CSCustomerDetail()
                    csCustomerInfo.CasinoNo = Globals.CASINO_GROUP1
                    csCustomerInfo.IBCCustID = custId
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
                    csCustomerInfo.IsEnablePT = True
                    csCustomerInfo.SIBCCustID = custDetail.SuperId
                    csCustomerInfo.MIBCCustID = custDetail.MasterId
                    csCustomerInfo.AIBCCustID = custDetail.AgentId
                    csCustomerInfo.RoleId = Globals.AccountType.Master
                    csCustomerInfo.CasinoDisabled = custDetail.IsUplineDisableCasino
                    
                    Dim custService = New ServiceCustomerInfo()
                    custService.AutoInsertThirdPartyAccount(csCustomerInfo, custDetail, AppSession.AccId, AppSession.Username, Sunplus.Accounting.Business.BaseManager.CurrentUserIp)
                End If
            End If
        Catch ex As Exception

            Dim entLibLogger As Sunplus.Core.LoggingLib.EntLibLogger = New Sunplus.Core.LoggingLib.EntLibLogger()
            entLibLogger.Error(ex.Message, ex)
            Throw

        End Try
    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Sub AddParaSoccer()
        'Add Bet Setting parameters
        param.Add("minbet", Request("txtMinbet1")) 'money  
        param.Add("maxbet", Request("txtMaxbet1")) 'money  
        param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money 
        '**************************
        '***** Add Position Taking paramaters*****
        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim sHdpD, sOUD, s1stHdpD, s1stOUD, sOE, s1x2, sCS, sTG, sMp, sOR As Double
        Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double

        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
        Dim sHdpL, sOUL, s1stHdpL, s1stOUL As Double
        Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto1") Is Nothing Then
        '    isAuto = True
        'End If
        '-------------------------------- Check IsAuto posiiontaking for All Bettype------------------Tho Nguyen Add
        Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Boolean
        Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean

        isAutoHdpD = IIf(Request("chkAuto1HdpD") Is Nothing, 0, 1)
        isAutoOUD = IIf(Request("chkAuto1OUD") Is Nothing, 0, 1)
        isAuto1stHdpD = IIf(Request("chkAuto11stHdpD") Is Nothing, 0, 1)
        isAuto1stOUD = IIf(Request("chkAuto11stOUD") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto1OE") Is Nothing, 0, 1)

        isAuto1x2 = IIf(Request("chkAuto11x2") Is Nothing, 0, 1)
        isAutoCS = IIf(Request("chkAuto1CS") Is Nothing, 0, 1)
        isAutoTG = IIf(Request("chkAuto1TG") Is Nothing, 0, 1)
        isAutoMP = IIf(Request("chkAuto1MP") Is Nothing, 0, 1)
        isAutoOR = IIf(Request("chkAuto1OR") Is Nothing, 0, 1)

        isAutoHdpL = IIf(Request("chkAuto1HdpL") Is Nothing, 0, 1)
        isAutoOUL = IIf(Request("chkAuto1OUL") Is Nothing, 0, 1)
        isAuto1stHdpL = IIf(Request("chkAuto11stHdpL") Is Nothing, 0, 1)
        isAuto1stOUL = IIf(Request("chkAuto11stOUL") Is Nothing, 0, 1)

        '---------------------------------------------------------------------------------------------------------


        Dim isCopyHdpD As Boolean = False
        If Not Request("chkCopy1HdpDeadball") Is Nothing Then
            isCopyHdpD = True
            isAutoOUD = isAutoHdpD
            isAuto1stHdpD = isAutoHdpD
            isAuto1stOUD = isAutoHdpD
            isAutoOE = isAutoHdpD
        End If
        Dim isCopy1x2 As Boolean = False
        If Not Request("chkCopy11x2") Is Nothing Then
            isCopy1x2 = True
            isAutoCS = isAuto1x2
            isAutoTG = isAuto1x2
            isAutoMP = isAuto1x2
            isAutoOR = isAuto1x2
        End If
        Dim isCopyHdpL As Boolean = False
        If Not Request("chkCopy1HdpLive") Is Nothing Then
            isCopyHdpL = True
            isAutoOUL = isAutoHdpL
            isAuto1stHdpL = isAutoHdpL
            isAuto1stOUL = isAutoHdpL
        End If

        mHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1HdpD")))
        mOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OUD")))
        m1stHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stHdpD")))
        m1stOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stOUD")))
        mOE = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OE")))

        m1x2 = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM11x2")))
        mCS = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1CS")))
        mTG = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1TG")))
        mMp = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1MP")))
        mOR = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1OR")))

        mHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1HdpL")))
        mOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1OUL")))
        m1stHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stHdpL")))
        m1stOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stOUL")))

        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            autoHdpD = IIf(isAutoHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1HdpD")), 0)
            autoOUD = IIf(isAutoOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OUD")), 0)
            auto1stHdpD = IIf(isAuto1stHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stHdpD")), 0)
            auto1stOUD = IIf(isAuto1stOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stOUD")), 0)
            autoOE = IIf(isAutoOE, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OE")), 0)

            auto1x2 = IIf(isAuto1x2, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS11x2")), 0)
            autoCS = IIf(isAutoCS, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1CS")), 0)
            autoTG = IIf(isAutoTG, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1TG")), 0)
            autoMp = IIf(isAutoMP, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1MP")), 0)
            autoOR = IIf(isAutoOR, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1OR")), 0)

            autoHdpL = IIf(isAutoHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1HdpL")), 0)
            autoOUL = IIf(isAutoOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1OUL")), 0)
            auto1stHdpL = IIf(isAuto1stHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stHdpL")), 0)
            auto1stOUL = IIf(isAuto1stOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stOUL")), 0)

            sHdpD = IIf(isAutoHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1HdpD")))
            sOUD = IIf(isAutoOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OUD")))
            s1stHdpD = IIf(isAuto1stHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stHdpD")))
            s1stOUD = IIf(isAuto1stOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stOUD")))
            sOE = IIf(isAutoOE, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OE")))

            s1x2 = IIf(isAuto1x2, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS11x2")))
            sCS = IIf(isAutoCS, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1CS")))
            sTG = IIf(isAutoTG, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1TG")))
            sMp = IIf(isAutoMP, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1MP")))
            sOR = IIf(isAutoOR, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1OR")))

            sHdpL = IIf(isAutoHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1HdpL")))
            sOUL = IIf(isAutoOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1OUL")))
            s1stHdpL = IIf(isAuto1stHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stHdpL")))
            s1stOUL = IIf(isAuto1stOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stOUL")))
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdpD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOUD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            auto1stHdpD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            auto1stOUD = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            auto1x2 = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoCS = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoTG = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoMp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOR = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            autoHdpL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOUL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            auto1stHdpL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            auto1stOUL = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

            s1x2 = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sCS = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sTG = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sMp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOR = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

            sHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            s1stOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("spositiontaking_1", sHdpD) 'smallmoney  
        param.Add("spositiontaking_3", sOUD) 'smallmoney  
        param.Add("spositiontaking_7", s1stHdpD) 'smallmoney  
        param.Add("spositiontaking_8", s1stOUD) 'smallmoney  
        param.Add("spositiontaking_2", sOE) 'smallmoney  

        param.Add("spositiontaking_5", s1x2) 'smallmoney 
        param.Add("spositiontaking_4", sCS) 'smallmoney
        param.Add("spositiontaking_6", sTG) 'smallmoney            
        param.Add("spositiontaking_9", sMp) 'smallmoney  
        param.Add("spositiontaking_10", sOR) 'smallmoney  

        param.Add("slivepositiontaking_1", sHdpL) 'smallmoney  
        param.Add("slivepositiontaking_3", sOUL) 'smallmoney  
        param.Add("slivepositiontaking_7", s1stHdpL) 'smallmoney  
        param.Add("slivepositiontaking_8", s1stOUL) 'smallmoney  

        param.Add("mpositiontaking_1", mHdpD) 'smallmoney   
        param.Add("mpositiontaking_3", mOUD) 'smallmoney   
        param.Add("mpositiontaking_7", m1stHdpD) 'smallmoney  
        param.Add("mpositiontaking_8", m1stOUD) 'smallmoney   
        param.Add("mpositiontaking_2", mOE) 'smallmoney   

        param.Add("mpositiontaking_5", m1x2) 'smallmoney  
        param.Add("mpositiontaking_4", mCS) 'smallmoney
        param.Add("mpositiontaking_6", mTG) 'smallmoney             
        param.Add("mpositiontaking_9", mMp) 'smallmoney   
        param.Add("mpositiontaking_10", mOR) 'smallmoney  

        param.Add("mlivepositiontaking_1", mHdpL) 'smallmoney    
        param.Add("mlivepositiontaking_3", mOUL) 'smallmoney    
        param.Add("mlivepositiontaking_7", m1stHdpL) 'smallmoney   
        param.Add("mlivepositiontaking_8", m1stOUL) 'smallmoney  

        param.Add("auto_spositiontaking_1", autoHdpD) 'smallmoney    
        param.Add("auto_spositiontaking_3", autoOUD) 'smallmoney    
        param.Add("auto_spositiontaking_7", auto1stHdpD) 'smallmoney 
        param.Add("auto_spositiontaking_8", auto1stOUD) 'smallmoney  
        param.Add("auto_spositiontaking_2", autoOE) 'smallmoney                  

        param.Add("auto_spositiontaking_5", auto1x2) 'smallmoney 
        param.Add("auto_spositiontaking_4", autoCS) 'smallmoney
        param.Add("auto_spositiontaking_6", autoTG) 'smallmoney             
        param.Add("auto_spositiontaking_9", autoMp) 'smallmoney  
        param.Add("auto_spositiontaking_10", autoOR) 'smallmoney 

        param.Add("auto_slivepositiontaking_1", autoHdpL) 'smallmoney     
        param.Add("auto_slivepositiontaking_3", autoOUL) 'smallmoney     
        param.Add("auto_slivepositiontaking_7", auto1stHdpL) 'smallmoney   
        param.Add("auto_slivepositiontaking_8", auto1stOUL) 'smallmoney 

        '***** End Inserting for Soccer *****
    End Sub
#End Region

#Region "Insert parameters for Basketball Position Taking"
    Sub AddParaBasketball(ByVal sportId As String)
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" + sportId)))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" + sportId)))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" + sportId)))
        param.Add("minbetv" + sportId, minb) 'money  
        param.Add("maxbetv" + sportId, maxb) 'money  
        param.Add("maxpermatchv" + sportId, maxpermatch) 'money  
        '***** Start Inserting for Basketball *****
        Dim mHdp, mOU, mOE, mMP, mML As Double
        Dim sHdp, sOU, sOE, sMP, sML As Double
        Dim autoHdp, autoOU, autoOE, autoMP, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto" & sportId) Is Nothing Then
        '    isAuto = True
        'End If
        '-----------------------------------------------------------------------------------
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Boolean


        isAutoHdp = IIf(Request("chkAuto2Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto2OU") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto2OE") Is Nothing, 0, 1)
        isAutoMP = IIf(Request("chkAuto2MP") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto2ML") Is Nothing, 0, 1)
        '--------------------------------------------------------------------------------
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy2Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoOE = isAutoHdp
            isAutoMP = isAutoHdp
            isAutoML = isAutoHdp
        End If

        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOE = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mMP = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2Hdp"))
            mOU = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2OU"))
            mOE = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2OE"))
            mMP = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2MP"))
            mML = IIf(isCopyHdp, Request("listM2Hdp"), Request("listM2ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True

                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOE = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoMP = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOE = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sMP = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2OU")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2OE")), 0)
                autoMP = IIf(isAutoMP, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2MP")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS2Hdp"), Request("autoS2ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2Hdp")))
                sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2OU")))
                sOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2OE")))
                sMP = IIf(isAutoMP, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2MP")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS2Hdp"), Request("listS2ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoMP = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sMP = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v2spositiontaking_1", sHdp) 'smallmoney  
        param.Add("v2spositiontaking_3", sOU) 'smallmoney  
        param.Add("v2spositiontaking_7", sHdp) 'smallmoney 
        param.Add("v2spositiontaking_8", sOU) 'smallmoney  
        param.Add("v2spositiontaking_2", sOE) 'smallmoney  
        param.Add("v2spositiontaking_9", sMP) 'smallmoney 
        param.Add("v2spositiontaking_20", sML) 'smallmoney          

        param.Add("v2mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v2mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
        param.Add("v2mpositiontaking_8", mOU) 'smallmoney 
        param.Add("v2mpositiontaking_2", mOE) 'smallmoney
        param.Add("v2mpositiontaking_9", mMP) 'smallmoney 
        param.Add("v2mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v2spositiontaking_1", autoHdp) 'smallmoney  
        param.Add("auto_v2spositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v2spositiontaking_7", autoHdp) 'smallmoney  
        param.Add("auto_v2spositiontaking_8", autoOU) 'smallmoney 
        param.Add("auto_v2spositiontaking_2", autoOE) 'smallmoney  
        param.Add("auto_v2spositiontaking_9", autoMP) 'smallmoney 
        param.Add("auto_v2spositiontaking_20", autoML) 'smallmoney 

        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Football, Tennis, Golf, Other Sports Position Taking"
    Sub AddParaFootball(ByVal sportId As String)
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" + sportId)))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" + sportId)))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" + sportId)))
        param.Add("minbetv" + sportId, minb) 'money  
        param.Add("maxbetv" + sportId, maxb) 'money  
        param.Add("maxpermatchv" + sportId, maxpermatch) 'money  
        '***** Start Inserting for Football *****
        Dim mHdp, mOU, mOE, mML As Double
        Dim sHdp, sOU, sOE, sML As Double
        Dim autoHdp, autoOU, autoOE, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto" & sportId) Is Nothing Then
        '    isAuto = True
        'End If

        '-----------------------------------------------------------------------------------
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Boolean


        isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)
        '--------------------------------------------------------------------------------

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoOE = isAutoHdp
            isAutoML = isAutoHdp
        End If

        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOE = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
            mOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
            mOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
            mML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOE = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOE = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                sOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v" & sportId & "spositiontaking_1", sHdp) 'smallmoney  
        param.Add("v" & sportId & "spositiontaking_3", sOU) 'smallmoney  
        param.Add("v" & sportId & "spositiontaking_2", sOE) 'smallmoney  
        param.Add("v" & sportId & "spositiontaking_20", sML) 'smallmoney  

        param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney
        param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v" & sportId & "spositiontaking_1", autoHdp) 'smallmoney  
        param.Add("auto_v" & sportId & "spositiontaking_2", autoOE) 'smallmoney  
        param.Add("auto_v" & sportId & "spositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v" & sportId & "spositiontaking_20", autoML) 'smallmoney 

        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Baseball Position Taking"
    Sub AddParaBaseball()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet8")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet8")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch8")))
        param.Add("minbetv8", minb)  'money  
        param.Add("maxbetv8", maxb) 'money  
        param.Add("maxpermatchv8", maxpermatch) 'money  
        '***** Start Inserting for Baseball *****
        Dim mHdp, mOU, mML As Double
        Dim sHdp, sOU, sML As Double
        Dim autoHdp, autoOU, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto8") Is Nothing Then
        '    isAuto = True
        'End If

        '-----------------------------------------------------------------------------------
        Dim isAutoHdp, isAutoOU, isAutoML As Boolean

        isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)
        '--------------------------------------------------------------------------------

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy8Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoML = isAutoHdp
        End If
        'this case for Copy All from Soccer or Basketball checkbox is checked
        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp"))
            mOU = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU"))
            mML = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8OU")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8Hdp")))
                sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8OU")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v8spositiontaking_1", sHdp) 'smallmoney  
        param.Add("v8spositiontaking_3", sOU) 'smallmoney 
        param.Add("v8spositiontaking_20", sML) 'smallmoney  

        param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
        param.Add("v8mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v8spositiontaking_1", autoHdp) 'smallmoney 
        param.Add("auto_v8spositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v8spositiontaking_20", autoML) 'smallmoney 

        '***** End Inserting for Baseball *****
    End Sub
#End Region

#Region "Insert parameters for Motorsports Position Taking"
    Sub AddParaMotosports()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet11")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet11")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch11")))
        param.Add("minbetv11", minb)  'money  
        param.Add("maxbetv11", maxb) 'money  
        param.Add("maxpermatchv11", maxpermatch) 'money  
        '***** Start Inserting for MotoSports *****
        Dim mHdp, mML As Double
        Dim sHdp, sML As Double
        Dim autoHdp, autoML As Double

        'Dim isAuto As Boolean = False
        'If Not Request("chkAuto11") Is Nothing Then
        '    isAuto = True
        'End If
        '-----------------------------------------------------------------------------------
        Dim isAutoHdp, isAutoML As Boolean

        isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
        '--------------------------------------------------------------------------------

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy11Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoML = isAutoHdp
        End If
        'this case for Copy All from Soccer or Basketball checkbox is checked
        If isCopyAll Or isCopyBasket Then
            mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
        Else
            mHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
            mML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))
        End If

        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)

                sHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                sML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11Hdp")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11ML")), 0)

                sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11Hdp")))
                sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11ML")))
            End If
        Else
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

            sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("v11spositiontaking_1", sHdp) 'smallmoney         
        param.Add("v11spositiontaking_20", sML) 'smallmoney  

        param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
        param.Add("v11mpositiontaking_20", mML) 'smallmoney 

        param.Add("auto_v11spositiontaking_1", autoHdp) 'smallmoney          
        param.Add("auto_v11spositiontaking_20", autoML) 'smallmoney         
        '***** End Inserting for MotoSports *****
    End Sub
#End Region


End Class
