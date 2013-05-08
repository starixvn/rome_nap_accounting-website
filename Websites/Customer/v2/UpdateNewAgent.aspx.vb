Imports System.Collections
Imports Sunplus.Accounting.Business
Imports Sunplus.Accounting.WcfServiceLibrary.PT.Casino
Imports Sunplus.Accounting.WcfServiceLibrary.Bingo
Imports Sunplus.Accounting.Business.PT.Casino
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Bingo.Services
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Core.Common



Imports DNAide.Core

Imports Sunplus.Accounting.Business.PT.SportBooks
Imports ServiceCustomerInfo = Sunplus.Accounting.WcfServiceLibrary.Customer.ServiceCustomerInfo
Imports Sunplus.Accounting.Core.Extensions


Partial Class Customer_V2_UpdateNewAgent
    Inherits RequiredAuthorizationPage
    Dim param As New Hashtable
    Dim isCopyAll As Boolean = False
    Dim isCopyBasket As Boolean = False


    Protected Overrides Function HasPermission() As Boolean
        Return True
    End Function

    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.MasterId)
        End Get
    End Property

    Protected Overrides Sub RedirectError()

        Response.Write("<script type=""text/javascript"">parent.showAlertMessage('err','" & StringUtils.ParseToJSMsg(Resources.ErrorMsg.error_InvalidRequest) & "',1);parent.PTUnBlockUI();</script>")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim errmsg As String = ""
        Try

            If Permissions.CheckPermission(CInt(Session("AccId")), Enumerations.Functions.Customer.OtherCust, errmsg) <> 0 Then
                Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI(); parent.ShowError('" & AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) & "','err');</script>")
            Else
                InsertData()
            End If

        Catch ex As Exception
            Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI(); parent.ShowError('" & AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) & "','err');</script>")
        End Try
    End Sub
#Region "Update data"
    Sub InsertData()
        Try
            Dim resultUpdated As New Hashtable()
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
            param.Add("answer", "") 'nvarchar  

            If Not Request("chkCopyAll") Is Nothing Then
                isCopyAll = True
            End If
            If Not Request("chkCopyBasket") Is Nothing Then
                isCopyBasket = True
            End If


            If Not WebSettings.DisableInternal Then
                param.Add("isinternal", isInternal)
            End If
            If (Not WebSettings.Disable1x2Comm) Then
                param.Add("discount1x2", Request("discountcs1x2"))
            End If

            Dim IsSuspened As Boolean = False
            IsSuspened = IIf(Request("chkSuspended").Equals("1"), True, False)
            param.Add("suspended", IsSuspened)
            param.Add("suspendedFlag", "Acc")

            param.Add("credit", Request("txtCredit")) 'money  
            param.Add("maxcredit", CastAide.AsDecimal(Request("txtMaxCredit"), 0))
            param.Add("mrecommend", Request("masterId")) 'int 

            param.Add("groupa", Request("groupa")) 'decimal  
            param.Add("groupb", Request("groupb")) 'decimal  
            param.Add("groupc", Request("groupc")) 'decimal  
            param.Add("groupd", Request("groupd")) 'decimal  
            param.Add("discountcs", Request("discountcs")) 'decimal



            param.Add("language", "E")

            param.Add("subaccid", 0) 'int     
            param.Add("ip", Request.UserHostAddress.ToString())

            param.Add("errcode", 0) 'int  
            param.Add("errmsg", "") 'nvarchar  
            'param.Add("sitename", Session("sitename")) 'varchar  
            Dim strUpdTranfer As String = Nothing
            'If Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.IsDirectMaster(CastAide.AsInt32(Request("masterId"), -1)) = True Then
            'for direct agent only
            strUpdTranfer = String.Empty
            If Request("referupline") = "1" Then
                strUpdTranfer = ""
            ElseIf Request("tranfer") = "daily" Then
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

            'End If
            param.Add("transferoption", strUpdTranfer) 'varchar  
            AddParaSoccer()
            AddParaFootball(2)
            AddParaFootball(3)
            AddParaFootball(5)
            AddParaBaseball()
            AddParaFootball(10)
            AddParaMotosports()
            AddParaFootball(99)

            Dim numbergame As NumberGameManager = New NumberGameManager()
            Dim context As HttpContext = HttpContext.Current
            numbergame.AddParamAgentPT(param, context, isCopyAll, isCopyBasket)
            Dim mixedSports As MixedSportsManager = New MixedSportsManager()
            mixedSports.AddParamAgentPT(param, context)

            resultUpdated = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.AddNewAgent(param)
            If CInt(resultUpdated("errCode")) = 0 Then
                Dim strUrl As String = "parent.location.href"
                Dim isInsert As String = "false"
                If Not Request("set") Is Nothing And Request("set") = "1" Then
                    strUrl = "location.href"
                    isInsert = "true"
                End If
                Dim uplineId As Integer = CastAide.AsInt32(Request("uplineid"), 0)
                Dim uplineUserName As String = CastAide.AsString(Request("uplineusername"), "")

                Dim custId = resultUpdated("superId")


                If WebSettings.OpenMiniGame Then
                    'MiniGame
                    Dim maxMHdpD = CastAide.AsDouble(Request("maxM1HdpD"))
                    Dim mpt = CastAide.AsDouble(Request("listS1HdpD"))
                    Dim mAutoPt = CastAide.AsDouble(Request("autoS1HdpD"))
                    Dim apt = CastAide.AsDouble(Request("listM1HdpD"))
                    Dim isAutoHdpD = IIf(Request("chkAuto1HdpD") Is Nothing, False, True)
                    If isAutoHdpD Then
                        mpt = Math.Min(mAutoPt, maxMHdpD - apt)
                    End If

                End If


                If (WebSettings.disabledFI) Then
                    context.Response.Write("<script type=""text/javascript"">parent.DelayReloadPage('../AgentList.aspx?masterId=" + context.Request("masterId") + "&hr=" + ((Not (Request("hdAllowedCurr") = 0)) And (WebSettings.disabledHR = False)).ToString() + "',2000,'" & strUrl & "'); </script>")
                Else
                    strUrl = "UpdatePT.aspx?custid=" & custId & "&insert=" & isInsert & "&roleid=2" & "&uplineid=" & uplineId
                    Response.Write("<script type=""text/javascript""> if (parent.name=='centerFr') {parent.DelayReloadPage('" + strUrl + "',4000)} else {parent.DelayReloadPage('', 4000); setTimeout('ReLoad()', 3000)}; function ReLoad() {parent.location.replace('" + strUrl + "')}</script>")
                End If




                'Auto insert casino - bingo account
                custDetail.AgentId = custId
                Dim csCustomerInfo = New Sunplus.Accounting.Entities.Casino.CSCustomerDetail()
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
                csCustomerInfo.IsEnablePT = False
                csCustomerInfo.SIBCCustID = custDetail.SuperId
                csCustomerInfo.MIBCCustID = custDetail.MasterId
                csCustomerInfo.AIBCCustID = custDetail.AgentId
                csCustomerInfo.RoleId = Globals.AccountType.Agent
                csCustomerInfo.CasinoDisabled = custDetail.IsUplineDisableCasino

                Dim custService = New ServiceCustomerInfo()
                custService.AutoInsertThirdPartyAccount(csCustomerInfo, custDetail, AppSession.AccId, AppSession.Username, Sunplus.Accounting.Business.BaseManager.CurrentUserIp)
            Else
                Dim msg As String = ""
                If (resultUpdated("errMsg") Is Nothing) Then
                    msg = ""
                Else
                    msg = resultUpdated("errMsg")
                    msg = AlertMessage.GetMsg(msg, AlertMessage.MsgType.Error)
                End If
                Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI(); parent.ShowError('" + msg + "','err');</script>")
            End If

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)


        End Try
    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Function AddParaSoccer()
        'Add Bet Setting parameters
        param.Add("minbet", Request("txtMinbet1")) 'money  
        param.Add("maxbet", Request("txtMaxbet1")) 'money  
        param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money 
        '**************************
        '-------------------------------- Check IsAuto posiiontaking for All Bettype------------------Tho Nguyen Add
        Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMP, isAutoOR As Boolean
        Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean

        Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
        Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double

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
        '***** Add Position Taking paramaters*****
        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim aHdpD, aOUD, a1stHdpD, a1stOUD, aOE, a1x2, aCS, aTG, aMp, aOR As Double

        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
        Dim aHdpL, aOUL, a1stHdpL, a1stOUL As Double

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
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked  

            '------------------------------------ Auto PT -------------------------Tho Add----------------------------
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
            '/-------------------------------------------------------------------------------------------------------

            aHdpD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1HdpD"))
            aOUD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OUD"))
            a1stHdpD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stHdpD"))
            a1stOUD = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stOUD"))
            aOE = IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OE"))

            a1x2 = IIf(isCopy1x2, Request("listM11x2"), Request("listM11x2"))
            aCS = IIf(isCopy1x2, Request("listM11x2"), Request("listM1CS"))
            aTG = IIf(isCopy1x2, Request("listM11x2"), Request("listM1TG"))
            aMp = IIf(isCopy1x2, Request("listM11x2"), Request("listM1MP"))
            aOR = IIf(isCopy1x2, Request("listM11x2"), Request("listM1OR"))

            aHdpL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1HdpL"))
            aOUL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1OUL"))
            a1stHdpL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stHdpL"))
            a1stOUL = IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stOUL"))

            mHdpD = IIf(isAutoHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1HdpD")))
            mOUD = IIf(isAutoOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OUD")))
            m1stHdpD = IIf(isAuto1stHdpD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stHdpD")))
            m1stOUD = IIf(isAuto1stOUD, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stOUD")))
            mOE = IIf(isAutoOE, 0, IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OE")))

            m1x2 = IIf(isAuto1x2, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS11x2")))
            mCS = IIf(isAutoCS, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1CS")))
            mTG = IIf(isAutoTG, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1TG")))
            mMp = IIf(isAutoMP, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1MP")))
            mOR = IIf(isAutoOR, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1OR")))

            mHdpL = IIf(isAutoHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1HdpL")))
            mOUL = IIf(isAutoOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1OUL")))
            m1stHdpL = IIf(isAuto1stHdpL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stHdpL")))
            m1stOUL = IIf(isAuto1stOUL, 0, IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stOUL")))
        Else
            '------------------------------------ Auto PT -------------------------Tho Add----------------------------
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
            '/-------------------------------------------------------------------------------------------------------

            aHdpD = Request("listM1HdpD")
            aOUD = Request("listM1HdpD")
            a1stHdpD = Request("listM1HdpD")
            a1stOUD = Request("listM1HdpD")
            aOE = Request("listM1HdpD")

            a1x2 = Request("listM1HdpD")
            aCS = Request("listM1HdpD")
            aTG = Request("listM1HdpD")
            aMp = Request("listM1HdpD")
            aOR = Request("listM1HdpD")

            aHdpL = Request("listM1HdpD")
            aOUL = Request("listM1HdpD")
            a1stHdpL = Request("listM1HdpD")
            a1stOUL = Request("listM1HdpD")

            mHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            m1stHdpD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            m1stOUD = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

            m1x2 = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mCS = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mTG = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mMp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOR = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

            mHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            m1stHdpL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            m1stOUL = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If

        param.Add("apositiontaking_1", aHdpD) 'smallmoney  
        param.Add("apositiontaking_3", aOUD) 'smallmoney 
        param.Add("apositiontaking_7", a1stHdpD) 'smallmoney  
        param.Add("apositiontaking_8", a1stOUD) 'smallmoney  
        param.Add("apositiontaking_2", aOE) 'smallmoney  

        param.Add("apositiontaking_5", a1x2) 'smallmoney  
        param.Add("apositiontaking_4", aCS) 'smallmoney  
        param.Add("apositiontaking_6", aTG) 'smallmoney          
        param.Add("apositiontaking_9", aMp) 'smallmoney  
        param.Add("apositiontaking_10", aOR) 'smallmoney  

        param.Add("alivepositiontaking_1", aHdpL) 'smallmoney  
        param.Add("alivepositiontaking_3", aOUL) 'smallmoney  
        param.Add("alivepositiontaking_7", a1stHdpL) 'smallmoney  
        param.Add("alivepositiontaking_8", a1stOUL) 'smallmoney  

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
        '------------------------------------ Auto PT -------------------------Tho Add----------------------------
        param.Add("auto_mpositiontaking_1", autoHdpD) 'smallmoney    
        param.Add("auto_mpositiontaking_3", autoOUD) 'smallmoney    
        param.Add("auto_mpositiontaking_7", auto1stHdpD) 'smallmoney 
        param.Add("auto_mpositiontaking_8", auto1stOUD) 'smallmoney  
        param.Add("auto_mpositiontaking_2", autoOE) 'smallmoney                  

        param.Add("auto_mpositiontaking_5", auto1x2) 'smallmoney 
        param.Add("auto_mpositiontaking_4", autoCS) 'smallmoney
        param.Add("auto_mpositiontaking_6", autoTG) 'smallmoney             
        param.Add("auto_mpositiontaking_9", autoMp) 'smallmoney  
        param.Add("auto_mpositiontaking_10", autoOR) 'smallmoney 

        param.Add("auto_mlivepositiontaking_1", autoHdpL) 'smallmoney     
        param.Add("auto_mlivepositiontaking_3", autoOUL) 'smallmoney     
        param.Add("auto_mlivepositiontaking_7", auto1stHdpL) 'smallmoney   
        param.Add("auto_mlivepositiontaking_8", auto1stOUL) 'smallmoney 
        '/-------------------------------------------------------------------------------------------------------
        '***** End Inserting for Soccer *****
    End Function
#End Region

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
    Function AddParaFootball(ByVal sportId As String)
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" + sportId)))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" + sportId)))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" + sportId)))
        param.Add("minbetv" + sportId, minb) 'money  
        param.Add("maxbetv" + sportId, maxb) 'money  
        param.Add("maxpermatchv" + sportId, maxpermatch) 'money  
        '***** Start Inserting for Football *****
        Dim mHdp, mOU, mOE, mMP, mML As Double
        Dim aHdp, aOU, aOE, aMP, aML As Double
        '-------------------------------- Check IsAuto posiiontaking for All Bettype------------------Tho Nguyen Add
        Dim autoHdp, autoOU, autoOE, autoMP, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Boolean
        isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
        isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
        isAutoMP = IIf(Request("chkAuto" & sportId & "MP") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)
        '/---------------------------------------------------------------------------------------------------------
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoOE = isAutoHdp
            isAutoMP = isAutoHdp
            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
            If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked    
                '--------------------------------- Tho Nguyen Add ------------------------------
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOE = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoMP = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                '/---------------------------------------------------------------------------------
                aHdp = Request("listM2Hdp")
                aOU = Request("listM2Hdp")
                aOE = Request("listM2Hdp")
                aMP = Request("listM2Hdp")
                aML = Request("listM2Hdp")

                mHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mOE = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mMP = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                '--------------------------------- Tho Nguyen Add ------------------------------
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)
                '/---------------------------------------------------------------------------------
                aHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                aOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                aOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                aML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))

                mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                mOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML")))

                If sportId = "2" Then
                    aMP = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "MP"))
                    mMP = IIf(isAutoMP, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "MP")))
                    '--------------------------------- Tho Nguyen Add ------------------------------
                    autoMP = IIf(isAutoMP, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "MP")), 0)
                    '/---------------------------------------------------------------------------------
                End If
            End If
        Else
            '--------------------------------- Tho Nguyen Add ------------------------------
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoMP = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            '/---------------------------------------------------------------------------------
            aHdp = Request("listM1HdpD")
            aOU = Request("listM1HdpD")
            aOE = Request("listM1HdpD")
            aMP = Request("listM1HdpD")
            aML = Request("listM1HdpD")

            mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mMP = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If
        param.Add("v" & sportId & "apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_3", aOU) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_2", aOE) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_20", aML) 'smallmoney  

        param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney
        param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney      
        '--------------------------------- Tho Nguyen Add ------------------------------
        param.Add("auto_v" & sportId & "mpositiontaking_1", autoHdp) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_2", autoOE) 'smallmoney  
        param.Add("auto_v" & sportId & "mpositiontaking_20", autoML) 'smallmoney 
        '/---------------------------------------------------------------------------------
        If sportId = "2" Then
            param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
            param.Add("v2mpositiontaking_8", mOU) 'smallmoney  
            param.Add("v2mpositiontaking_9", mMP) 'smallmoney  
            param.Add("v2apositiontaking_7", aHdp) 'smallmoney 
            param.Add("v2apositiontaking_8", aOU) 'smallmoney  
            param.Add("v2apositiontaking_9", aMP) 'smallmoney  
            '--------------------------------- Tho Nguyen Add ------------------------------
            param.Add("auto_v2mpositiontaking_7", autoHdp) 'smallmoney  
            param.Add("auto_v2mpositiontaking_8", autoOU) 'smallmoney 
            param.Add("auto_v2mpositiontaking_9", autoMP) 'smallmoney 
            '/---------------------------------------------------------------------------------
        End If
        '***** End Inserting for Football *****
    End Function
#End Region

#Region "Insert parameters for Baseball Position Taking"
    Function AddParaBaseball()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet8")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet8")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch8")))
        param.Add("minbetv8", minb)  'money  
        param.Add("maxbetv8", maxb) 'money  
        param.Add("maxpermatchv8", maxpermatch) 'money  
        '***** Start Inserting for Baseball *****
        Dim mHdp, mOU, mML As Double
        Dim aHdp, aOU, aML As Double
        '-------------------------------- Check IsAuto posiiontaking for All Bettype------------------Tho Nguyen Add
        Dim isAutoHdp, isAutoOU, isAutoML As Boolean
        Dim autoHdp, autoOU, autoML As Double

        isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
        isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)
        '--------------------------------------------------------------------------------------------------------------
        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy8Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoOU = isAutoHdp
            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                '--------------------------------- Tho Nguyen Add ------------------------------
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoOU = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                '------------------------------------------------------------------------------
                aHdp = Request("listM2Hdp")
                aOU = Request("listM2Hdp")
                aML = Request("listM2Hdp")

                mHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mOU = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                '--------------------------------- Tho Nguyen Add ------------------------------
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8Hdp")), 0)
                autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8OU")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8ML")), 0)
                '------------------------------------------------------------------------------

                aHdp = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp"))
                aOU = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU"))
                aML = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML"))

                mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8Hdp")))
                mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8OU")))
                mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8ML")))
            End If
        Else
            '--------------------------------- Tho Nguyen Add ------------------------------
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            '------------------------------------------------------------------------------
            aHdp = Request("listM1HdpD")
            aOU = Request("listM1HdpD")
            aML = Request("listM1HdpD")

            mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If
        param.Add("v8apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v8apositiontaking_3", aOU) 'smallmoney 
        param.Add("v8apositiontaking_20", aML) 'smallmoney  

        param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
        param.Add("v8mpositiontaking_20", mML) 'smallmoney

        '--------------------------------- Tho Nguyen Add ------------------------------
        param.Add("auto_v8mpositiontaking_1", autoHdp) 'smallmoney 
        param.Add("auto_v8mpositiontaking_3", autoOU) 'smallmoney  
        param.Add("auto_v8mpositiontaking_20", autoML) 'smallmoney
        '------------------------------------------------------------------------------
        '***** End Inserting for Baseball *****
    End Function
#End Region

#Region "Insert parameters for Motorsports Position Taking"
    Function AddParaMotosports()
        Dim minb, maxb, maxpermatch As Double
        minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet11")))
        maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet11")))
        maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch11")))
        param.Add("minbetv11", minb)  'money  
        param.Add("maxbetv11", maxb) 'money  
        param.Add("maxpermatchv11", maxpermatch) 'money  
        '***** Start Inserting for MotoSports *****

        '------------------------------------Auto PT----------------------Tho add-----------------
        Dim isAutoHdp, isAutoML As Boolean
        Dim autoHdp, autoML As Double

        isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
        isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
        '/--------------------------------------------------------------------------------

        Dim mHdp, mML As Double
        Dim aHdp, aML As Double

        Dim isCopyHdp As Boolean = False
        If Not Request("chkCopy11Hdp") Is Nothing Then
            isCopyHdp = True
            isAutoML = isAutoHdp
        End If
        If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
            If isCopyBasket Then
                '------------------------------------Auto PT----------------------Tho add-----------------
                Dim isAutoBasket As Boolean = False
                If Not Request("chkAuto2Hdp") Is Nothing Then
                    isAutoBasket = True
                End If
                autoHdp = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                autoML = IIf(isAutoBasket, Request("autoS2Hdp"), 0)
                '/--------------------------------------------------------------------------------
                aHdp = Request("listM2Hdp")
                aML = Request("listM2Hdp")

                mHdp = IIf(isAutoBasket, 0, Request("listS2Hdp"))
                mML = IIf(isAutoBasket, 0, Request("listS2Hdp"))
            Else
                '------------------------------------Auto PT----------------------Tho add-----------------
                autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11Hdp")), 0)
                autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11ML")), 0)
                '/--------------------------------------------------------------------------------
                aHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
                aML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))

                mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11Hdp")))
                mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11ML")))
            End If
        Else
            '------------------------------------Auto PT----------------------Tho add-----------------
            Dim isAutoSoccer As Boolean = False
            If Not Request("chkAuto1HdpD") Is Nothing Then
                isAutoSoccer = True
            End If
            autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
            '/--------------------------------------------------------------------------------
            aHdp = Request("listM1HdpD")
            aML = Request("listM1HdpD")

            mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
            mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
        End If
        param.Add("v11apositiontaking_1", aHdp) 'smallmoney         
        param.Add("v11apositiontaking_20", aML) 'smallmoney  

        param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
        param.Add("v11mpositiontaking_20", mML) 'smallmoney  

        '------------------------------------Auto PT----------------------Tho add-----------------
        param.Add("auto_v11mpositiontaking_1", autoHdp) 'smallmoney          
        param.Add("auto_v11mpositiontaking_20", autoML) 'smallmoney 
        '/--------------------------------------------------------------------------------
        '***** End Inserting for MotoSports *****
    End Function
#End Region


End Class
