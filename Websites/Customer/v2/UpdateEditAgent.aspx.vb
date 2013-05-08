Imports System.Text
Imports System.Resources
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Common

Imports IBCUtils.Encryption
Imports IBCDLL.Customer
Imports DNAide.Core
Imports DAL
Imports BLL.Customer
Imports System.Collections.Generic
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Entities.Casino
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.DataAccess.IBCDLL.Customer
Imports Sunplus.Accounting.Core.Extensions



Partial Class Customer_V2_UpdateEditAgent
    Inherits RequiredAuthorizationPage
    Dim param As New Hashtable
    Dim isCopyAll As Boolean = False
    Dim isCopyBasket As Boolean = False
    Dim ptStr As String = String.Empty

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
            If Permissions.CheckPermission(CastAide.AsInt32(Session("AccId")), Enumerations.Functions.Customer.OtherCust, errmsg) <> 0 Then
                Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI(); parent.ShowError('" & AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) & "','err');parent.afterUpdate(false,0);</script>")
            Else
                Response.Write("<script type='text/javascript'>")
                UpdateData()
                Response.Write("</script>")
            End If

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub
#Region "Update data"
    Sub UpdateData()
        Dim isUpdate As Boolean = True
        Try
            Dim custId As String = Request("saveCustid")
            Dim isMult As Boolean = False
            isMult = IIf(Request("isMult") Is Nothing, 0, CastAide.AsBoolean(Request("isMult")))
            Dim isAllow As Boolean = False
            isAllow = IIf(Request("isAllowUpd") Is Nothing, 0, CastAide.AsBoolean(Request("isAllowUpd")))
            param.Clear()
            param.Add("custid", custId) 'int 
            param.Add("userid", Session("AccId")) 'int  
            param.Add("mrecommend", Request("masterId")) 'int 

            Dim closed As Boolean = False
            Dim suspend As Boolean = False

            If isMult Then
                param.Add("firstname", "") 'nvarchar  
                param.Add("lastname", "") 'nvarchar  
                param.Add("phone", "") 'nvarchar  
                param.Add("mobilephone", "") 'nvarchar  
                param.Add("fax", "") 'nvarchar 
                param.Add("credit", 0) 'money  
                param.Add("updatemultiple", 1) 'bit  
            Else
                param.Add("firstname", RemoveSpecialChars(Request("txtFName"))) 'nvarchar  
                param.Add("lastname", RemoveSpecialChars(Request("txtLName"))) 'nvarchar  
                param.Add("phone", RemoveSpecialChars(Request("txtPhone"))) 'nvarchar  
                param.Add("mobilephone", RemoveSpecialChars(Request("txtMobile"))) 'nvarchar  
                param.Add("fax", RemoveSpecialChars(Request("txtFax"))) 'nvarchar 
                param.Add("credit", Request("txtCredit")) 'money  
                param.Add("maxcredit", CastAide.AsDecimal(Request("txtMaxCredit"), 0)) 'money  
                param.Add("updatemultiple", 0) 'bit  
            End If

            If Not Request("chkUpdStatus") Is Nothing Or Not isMult Then
                param.Add("updateclosed", 1) 'bit
                If Not Request("chkClosed") Is Nothing Then
                    param.Add("closed", 1) 'bit 
                    closed = True

                Else
                    param.Add("closed", 0) 'bit 
                    closed = False
                End If
            Else
                param.Add("updateclosed", 0) 'bit    
                param.Add("closed", 0) 'bit  
            End If

            If Not Request("chkUpdStatus") Is Nothing Or Not isMult Then
                param.Add("updatesuspended", 1) 'bit
                If Not Request("chkSuspended") Is Nothing Then
                    param.Add("suspended", 1) 'bit 
                    suspend = True
                Else
                    param.Add("suspended", 0) 'bit 
                    suspend = False
                End If
            Else
                param.Add("updatesuspended", 0) 'bit    
                param.Add("suspended", 0) 'bit  
            End If

            If Session("lang") = "en-US" Then
                param.Add("language", "E")
            ElseIf Session("lang") = "zh-CN" Then
                param.Add("language", "C")
            ElseIf Session("lang") = "ja-JP" Then
                param.Add("language", "J")
            End If
            If Not Request("chkUpdComm") Is Nothing Or Not isMult Then
                param.Add("updatecommission", 1) 'bit  
                param.Add("groupa", Request("groupa")) 'decimal  
                param.Add("groupb", Request("groupb")) 'decimal  
                param.Add("groupc", Request("groupc")) 'decimal  
                param.Add("groupd", Request("groupd")) 'decimal  
                param.Add("discountcs", Request("discountcs")) 'decimal 

                If (Not WebSettings.Disable1x2Comm) Then
                    param.Add("discount1x2", Request("discountcs1x2"))
                End If

            Else
                param.Add("updatecommission", 0) 'bit  
                param.Add("groupa", 0) 'decimal  
                param.Add("groupb", 0) 'decimal  
                param.Add("groupc", 0) 'decimal  
                param.Add("groupd", 0) 'decimal  
                param.Add("discountcs", 0) 'decimal 

                If (Not WebSettings.Disable1x2Comm) Then
                    param.Add("discount1x2", 0)
                End If
            End If
            If Not Request("chkCopyAll") Is Nothing Then
                isCopyAll = True
            End If
            If Not Request("chkCopyBasket") Is Nothing Then
                isCopyBasket = True
            End If

            Dim isUpdAll As Integer = CInt(Request("isUpdateMember"))
            param.Add("updateallmembers", isUpdAll) 'bit  
            param.Add("newmpositiontaking", Request("memberPT")) 'smallmoney  
            param.Add("subaccid", 0) 'int   
            param.Add("ip", Request.UserHostAddress.ToString())

            param.Add("errcode", 0) 'int  
            param.Add("errmsg", "") 'nvarchar  
            param.Add("sitename", Session("sitename")) 'varchar  
            AddParaSoccer(isMult)
            AddParaFootball(2, isMult)
            AddParaFootball(3, isMult)
            AddParaFootball(5, isMult)
            AddParaBaseball(isMult)
            AddParaFootball(10, isMult)
            AddParaMotosports(isMult)
            AddParaFootball(99, isMult)

            Dim context As HttpContext = HttpContext.Current
            Dim numbergame As NumberGameManager = New NumberGameManager()
            numbergame.AddParamAgentPT(param, context, isCopyAll, isCopyBasket)

            Dim mixedSports As MixedSportsManager = New MixedSportsManager()
            mixedSports.AddParamAgentPT(param, context)

            Dim rs As IBCUtils.IbcError = UpdateCustomer.UpdateAgent(param)
            If rs.IsSuccess() Then
                'Response.Write("parent.doReload('Update Agent successfully');")
                Response.Write("parent.scroll('0','0');parent.alert_error('" + AlertMessage.GetMsg("Update Agent successfully", AlertMessage.MsgType.Success) + "','err'); parent.afterUpdate(true,2000);")
                ' Update for casino
                If (Permissions.IsAllowCasino() And (context.Request("hidSynCS") <> "False") And Not isMult) Then
                    ' Store data for casino section
                    Dim custInfo As CSCustomerDetail = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetCustInfoIBC(param("custid").ToString(), param)
                    ' Sync data to Casino
                    Try 'Casino cheat
                        Dim csCustomer As CasinoLibrary.SBAgent.Customer = New CasinoLibrary.SBAgent.Customer()
                        'csCustomer.SiteName = New Sunplus.Accounting.Business.Customer.CustomerManager().GetRNGSiteNameByCustId(custId)
                        csCustomer.SiteName = custInfo.SiteName
                        Dim status As Boolean = csCustomer.UpdateCustInfo(custInfo.IBCCustID, custInfo.FirstName, custInfo.LastName, custInfo.Phone, custInfo.MobilePhone, custInfo.Fax, custInfo.Closed, custInfo.Suspend, Request.UserHostAddress, AppSession.CustID, AppSession.Username, 3)
                    Catch ex As Exception
                        LogExtensions.ExceptionHandler(ex)
                    End Try
                End If
            Else
                Response.Write("parent.scroll('0','0');parent.PTUnBlockUI();parent.showEnrichMsg('" & rs.ErrorMsg & "','err')")
            End If
        Catch ex As Exception

            isUpdate = False
            Response.Write("parent.scroll('0','0');parent.PTUnBlockUI(); parent.ShowError('" & AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) & "','err');")
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Sub AddParaSoccer(ByVal isMult As Boolean)
        'Add Bet Setting parameters        
        If Not Request("chkUpdInfo1") Is Nothing Or Not isMult Then
            param.Add("updateminmaxBet", 1) 'bit  
            param.Add("minbet", Request("txtMinbet1")) 'money  
            param.Add("maxbet", Request("txtMaxbet1")) 'money  
            param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money  
        Else
            param.Add("updateminmaxBet", 0) 'bit  
            param.Add("minbet", 0) 'money  
            param.Add("maxbet", 0) 'money  
            param.Add("maxpermatch", 0) 'money  
        End If
        '**************************
        '***** Add Position Taking paramaters*****
        Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
        Dim aHdpD, aOUD, a1stHdpD, a1stOUD, aOE, a1x2, aCS, aTG, aMp, aOR As Double

        Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
        Dim aHdpL, aOUL, a1stHdpL, a1stOUL As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
        Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMp, isAutoOR As Boolean

        Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double
        Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean
        '/--------------------------------------------------------------------------------------------------------------
        If Not Request("chkUpdPT1") Is Nothing Or Not isMult Then
            param.Add("updatepositiontaking", 1) 'bit 

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutoHdpD = IIf(Request("chkAuto1HdpD") Is Nothing, 0, 1)
            isAutoOUD = IIf(Request("chkAuto1OUD") Is Nothing, 0, 1)
            isAuto1stHdpD = IIf(Request("chkAuto11stHdpD") Is Nothing, 0, 1)
            isAuto1stOUD = IIf(Request("chkAuto11stOUD") Is Nothing, 0, 1)
            isAutoOE = IIf(Request("chkAuto1OE") Is Nothing, 0, 1)

            isAuto1x2 = IIf(Request("chkAuto11x2") Is Nothing, 0, 1)
            isAutoCS = IIf(Request("chkAuto1CS") Is Nothing, 0, 1)
            isAutoTG = IIf(Request("chkAuto1TG") Is Nothing, 0, 1)
            isAutoMp = IIf(Request("chkAuto1MP") Is Nothing, 0, 1)
            isAutoOR = IIf(Request("chkAuto1OR") Is Nothing, 0, 1)

            isAutoHdpL = IIf(Request("chkAuto1HdpL") Is Nothing, 0, 1)
            isAutoOUL = IIf(Request("chkAuto1OUL") Is Nothing, 0, 1)
            isAuto1stHdpL = IIf(Request("chkAuto11stHdpL") Is Nothing, 0, 1)
            isAuto1stOUL = IIf(Request("chkAuto11stOUL") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------

            Dim isCopyHdpD As Boolean = False
            If Not Request("chkCopy1HdpDeadball") Is Nothing Then
                isCopyHdpD = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoOUD = isAutoHdpD
                isAuto1stHdpD = isAutoHdpD
                isAuto1stOUD = isAutoHdpD
                isAutoOE = isAutoHdpD
                '/--------------------------------------------------------------------------------------------------------------
            End If
            Dim isCopy1x2 As Boolean = False
            If Not Request("chkCopy11x2") Is Nothing Then
                isCopy1x2 = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoCS = isAuto1x2
                isAutoTG = isAuto1x2
                isAutoMp = isAuto1x2
                isAutoOR = isAuto1x2
                '/--------------------------------------------------------------------------------------------------------------
            End If
            Dim isCopyHdpL As Boolean = False
            If Not Request("chkCopy1HdpLive") Is Nothing Then
                isCopyHdpL = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoOUL = isAutoHdpL
                isAuto1stHdpL = isAutoHdpL
                isAuto1stOUL = isAutoHdpL
                '/--------------------------------------------------------------------------------------------------------------
            End If
            '---------------------------------------Tho Update-----------------------------
            aHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1HdpD")))
            aOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OUD")))
            a1stHdpD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stHdpD")))
            a1stOUD = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM11stOUD")))
            aOE = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpD, Request("listM1HdpD"), Request("listM1OE")))

            a1x2 = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM11x2")))
            aCS = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1CS")))
            aTG = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1TG")))
            aMp = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1MP")))
            aOR = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopy1x2, Request("listM11x2"), Request("listM1OR")))

            aHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1HdpL")))
            aOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM1OUL")))
            a1stHdpL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stHdpL")))
            a1stOUL = IIf(isCopyAll, Request("listM1HdpD"), IIf(isCopyHdpL, Request("listM1HdpL"), Request("listM11stOUL")))
            '/-------------------------------------------------------------------------
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked  
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                autoHdpD = IIf(isAutoHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1HdpD")), 0)
                autoOUD = IIf(isAutoOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OUD")), 0)
                auto1stHdpD = IIf(isAuto1stHdpD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stHdpD")), 0)
                auto1stOUD = IIf(isAuto1stOUD, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS11stOUD")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdpD, Request("autoS1HdpD"), Request("autoS1OE")), 0)

                auto1x2 = IIf(isAuto1x2, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS11x2")), 0)
                autoCS = IIf(isAutoCS, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1CS")), 0)
                autoTG = IIf(isAutoTG, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1TG")), 0)
                autoMp = IIf(isAutoMp, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1MP")), 0)
                autoOR = IIf(isAutoOR, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1OR")), 0)

                autoHdpL = IIf(isAutoHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1HdpL")), 0)
                autoOUL = IIf(isAutoOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS1OUL")), 0)
                auto1stHdpL = IIf(isAuto1stHdpL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stHdpL")), 0)
                auto1stOUL = IIf(isAuto1stOUL, IIf(isCopyHdpL, Request("autoS1HdpL"), Request("autoS11stOUL")), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdpD = IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1HdpD"))
                mOUD = IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OUD"))
                m1stHdpD = IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stHdpD"))
                m1stOUD = IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS11stOUD"))
                mOE = IIf(isCopyHdpD, Request("listS1HdpD"), Request("listS1OE"))

                m1x2 = IIf(isCopy1x2, Request("listS11x2"), Request("listS11x2"))
                mCS = IIf(isCopy1x2, Request("listS11x2"), Request("listS1CS"))
                mTG = IIf(isCopy1x2, Request("listS11x2"), Request("listS1TG"))
                mMp = IIf(isCopy1x2, Request("listS11x2"), Request("listS1MP"))
                mOR = IIf(isCopy1x2, Request("listS11x2"), Request("listS1OR"))

                mHdpL = IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1HdpL"))
                mOUL = IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS1OUL"))
                m1stHdpL = IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stHdpL"))
                m1stOUL = IIf(isCopyHdpL, Request("listS1HdpL"), Request("listS11stOUL"))
            Else
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
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
                '/--------------------------------------------------------------------------------------------------------------
                'mHdpD = Request("listS1HdpD")
                'mOUD = Request("listS1HdpD")
                'm1stHdpD = Request("listS1HdpD")
                'm1stOUD = Request("listS1HdpD")
                'mOE = Request("listS1HdpD")

                'm1x2 = Request("listS1HdpD")
                'mCS = Request("listS1HdpD")
                'mTG = Request("listS1HdpD")
                'mMp = Request("listS1HdpD")
                'mOR = Request("listS1HdpD")

                'mHdpL = Request("listS1HdpD")
                'mOUL = Request("listS1HdpD")
                'm1stHdpL = Request("listS1HdpD")
                'm1stOUL = Request("listS1HdpD")
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

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdpD = CInt(isAutoSoccer)
                isAutoOUD = CInt(isAutoSoccer)
                isAuto1stHdpD = CInt(isAutoSoccer)
                isAuto1stOUD = CInt(isAutoSoccer)
                isAutoOE = CInt(isAutoSoccer)

                isAuto1x2 = CInt(isAutoSoccer)
                isAutoCS = CInt(isAutoSoccer)
                isAutoTG = CInt(isAutoSoccer)
                isAutoMp = CInt(isAutoSoccer)
                isAutoOR = CInt(isAutoSoccer)

                isAutoHdpL = CInt(isAutoSoccer)
                isAutoOUL = CInt(isAutoSoccer)
                isAuto1stHdpL = CInt(isAutoSoccer)
                isAuto1stOUL = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------
            End If
        Else
            param.Add("updatepositiontaking", 0) 'bit 
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

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        '''''''''''Hanh commented for disabling Master Auto PT - 2008-12-28
        If Not WebSettings.disableAutoM Then
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

            param.Add("isauto_mpositiontaking_1", isAutoHdpD) 'bit  
            param.Add("isauto_mpositiontaking_3", isAutoOUD) 'bit 
            param.Add("isauto_mpositiontaking_7", isAuto1stHdpD) 'bit  
            param.Add("isauto_mpositiontaking_8", isAuto1stOUD) 'bit  
            param.Add("isauto_mpositiontaking_2", isAutoOE) 'bit  

            param.Add("isauto_mpositiontaking_5", isAuto1x2) 'bit 
            param.Add("isauto_mpositiontaking_4", isAutoCS) 'bit  
            param.Add("isauto_mpositiontaking_6", isAutoTG) 'bit        
            param.Add("isauto_mpositiontaking_9", isAutoMp) 'bit  
            param.Add("isauto_mpositiontaking_10", isAutoOR) 'bit  

            param.Add("isauto_mlivepositiontaking_1", isAutoHdpL) 'bit  
            param.Add("isauto_mlivepositiontaking_3", isAutoOUL) 'bit  
            param.Add("isauto_mlivepositiontaking_7", isAuto1stHdpL) 'bit  
            param.Add("isauto_mlivepositiontaking_8", isAuto1stOUL) 'bit  
        End If
        '/--------------------------------------------------------------------------------------------------------------
        '***** End Inserting for Soccer *****
    End Sub
#End Region

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
    Sub AddParaFootball(ByVal sportId As String, ByVal isMult As Boolean)
        Dim minb, maxb, maxpermatch As Double
        If Not Request("chkUpdInfo" & sportId) Is Nothing Or Not isMult Then
            param.Add("updateminmaxbetv" & sportId, 1) 'bit  
            minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet" & sportId)))
            maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet" & sportId)))
            maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch" & sportId)))
        Else
            param.Add("updateminmaxbetv" & sportId, 0) 'bit                  
        End If
        param.Add("minbetv" & sportId, minb) 'money  
        param.Add("maxbetv" & sportId, maxb) 'money  
        param.Add("maxpermatchv" & sportId, maxpermatch) 'money  
        '***** Start Inserting for Football *****
        Dim mHdp, mOU, mOE, mMP, mML As Double
        Dim aHdp, aOU, aOE, aMP, aML As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdp, autoOU, autoOE, autoMP, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Double
        '/--------------------------------------------------------------------------------------------------------------
        If Not Request("chkUpdPT" & sportId) Is Nothing Or Not isMult Then
            param.Add("updatepositiontakingv" & sportId, 1) 'bit  
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
            isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
            isAutoMP = IIf(Request("chkAuto" & sportId & "MP") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)
            '/-------------------------------------------------------------------------------------------------------------- 

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
                isCopyHdp = True
                isAutoOU = isAutoHdp
                isAutoOE = isAutoHdp
                isAutoMP = isAutoHdp
                isAutoML = isAutoHdp
            End If
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If isCopyAll Or isCopyBasket Then
                aHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aOE = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aMP = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            Else
                aHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                aOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                aOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                aMP = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "MP"))
                aML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))
            End If
            '/--------------------------------------------------------------------------------------------------------------
            If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
                If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked          
                    'aHdp = Request("listM2Hdp")
                    'aOU = Request("listM2Hdp")
                    'aOE = Request("listM2Hdp")
                    'aMP = Request("listM2Hdp")
                    'aML = Request("listM2Hdp")


                    'mHdp = Request("listS2Hdp")
                    'mOU = Request("listS2Hdp")
                    'mOE = Request("listS2Hdp")
                    'mMP = Request("listS2Hdp")
                    'mML = Request("listS2Hdp")
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If

                    autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoOU = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoOE = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoMP = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mOU = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mOE = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mMP = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoOU = CInt(isAuto2Hdp)
                    isAutoOE = CInt(isAuto2Hdp)
                    isAutoMP = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)
                    '/--------------------------------------------------------------------------------------------------------------
                Else
                    'aHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                    'aOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                    'aOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                    'aML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))

                    'mHdp = IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp"))
                    'mOU = IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU"))
                    'mOE = IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE"))
                    'mML = IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML"))
                    'If sportId = "2" Then
                    '    aMP = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "MP"))
                    '    mMP = IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "MP"))
                    'End If
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                    autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                    autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                    mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                    mOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                    mML = IIf(isAutoMP, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML")))
                    If sportId = "2" Then
                        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                        autoMP = IIf(isAutoMP, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "MP")), 0)
                        '/--------------------------------------------------------------------------------------------------------------

                        mMP = IIf(isAutoMP, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "MP")))
                    End If
                End If
            Else
                'aHdp = Request("listM1HdpD")
                'aOU = Request("listM1HdpD")
                'aOE = Request("listM1HdpD")
                'aMP = Request("listM1HdpD")
                'aML = Request("listM1HdpD")

                'mHdp = Request("listS1HdpD")
                'mOU = Request("listS1HdpD")
                'mOE = Request("listS1HdpD")
                'mMP = Request("listS1HdpD")
                'mML = Request("listS1HdpD")
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoMP = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mMP = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CInt(isAutoSoccer)
                isAutoOU = CInt(isAutoSoccer)
                isAutoOE = CInt(isAutoSoccer)
                isAutoMP = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------
            End If
        Else
            param.Add("updatepositiontakingv" & sportId, 0) 'bit 
        End If
        param.Add("v" & sportId & "apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_3", aOU) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_2", aOE) 'smallmoney  
        param.Add("v" & sportId & "apositiontaking_20", aML) 'smallmoney  

        param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney  
        param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney  

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        '' 2008-12-28: Hanh commented for disabling Master Auto PT
        If Not WebSettings.disableAutoM Then
            param.Add("auto_v" & sportId & "mpositiontaking_1", autoHdp) 'smallmoney 
            param.Add("auto_v" & sportId & "mpositiontaking_3", autoOU) 'smallmoney  
            param.Add("auto_v" & sportId & "mpositiontaking_2", autoOE) 'smallmoney  
            param.Add("auto_v" & sportId & "mpositiontaking_20", autoML) 'smallmoney 

            param.Add("isauto_v" & sportId & "mpositiontaking_1", isAutoHdp) 'bit 
            param.Add("isauto_v" & sportId & "mpositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v" & sportId & "mpositiontaking_2", isAutoOE) 'bit 
            param.Add("isauto_v" & sportId & "mpositiontaking_20", isAutoML) 'bit
        End If
        '/--------------------------------------------------------------------------------------------------------------
        If sportId = "2" Then
            param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
            param.Add("v2mpositiontaking_8", mOU) 'smallmoney  
            param.Add("v2mpositiontaking_9", mMP) 'smallmoney  
            param.Add("v2apositiontaking_7", aHdp) 'smallmoney 
            param.Add("v2apositiontaking_8", aOU) 'smallmoney  
            param.Add("v2apositiontaking_9", aMP) 'smallmoney  

            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            '' 2008-12-28: Hanh commented for disabling Master Auto PT
            If Not WebSettings.disableAutoM Then
                param.Add("auto_v2mpositiontaking_7", autoHdp) 'smallmoney  
                param.Add("auto_v2mpositiontaking_8", autoOU) 'smallmoney
                param.Add("auto_v2mpositiontaking_9", autoMP) 'smallmoney 

                param.Add("isauto_v2mpositiontaking_7", isAutoHdp) 'bit  
                param.Add("isauto_v2mpositiontaking_8", isAutoOU) 'bit
                param.Add("isauto_v2mpositiontaking_9", isAutoMP) 'bit 
            End If
            '/--------------------------------------------------------------------------------------------------------------
        End If

        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Baseball Position Taking"
    Sub AddParaBaseball(ByVal isMult As Boolean)
        Dim minb, maxb, maxpermatch As Double
        If Not Request("chkUpdInfo8") Is Nothing Or Not isMult Then
            param.Add("updateminmaxbetv8", 1) 'bit  
            minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet8")))
            maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet8")))
            maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch8")))
        Else
            param.Add("updateminmaxbetv8", 0) 'bit                  
        End If
        param.Add("minbetv8", minb) 'money  
        param.Add("maxbetv8", maxb) 'money  
        param.Add("maxpermatchv8", maxpermatch) 'money  
        '***** Start Inserting for Baseball *****
        Dim mHdp, mOU, mML As Double
        Dim aHdp, aOU, aML As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdp, autoOU, autoML As Double
        Dim isAutoHdp, isAutoOU, isAutoML As Double
        '/--------------------------------------------------------------------------------------------------------------
        If Not Request("chkUpdPT8") Is Nothing Or Not isMult Then
            param.Add("updatepositiontakingv8", 1) 'bit  
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------
            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy8Hdp") Is Nothing Then
                isCopyHdp = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoOU = isAutoHdp
                isAutoML = isAutoHdp
                '/--------------------------------------------------------------------------------------------------------------
            End If
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If isCopyAll Or isCopyBasket Then
                aHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aOU = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            Else
                aHdp = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp"))
                aOU = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU"))
                aML = IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML"))
            End If
            '/--------------------------------------------------------------------------------------------------------------
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then
                    'aHdp = Request("listM2Hdp")
                    'aOU = Request("listM2Hdp")
                    'aML = Request("listM2Hdp")

                    'mHdp = Request("listS2Hdp")
                    'mOU = Request("listS2Hdp")
                    'mML = Request("listS2Hdp")
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If
                    autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoOU = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mOU = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoOU = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)
                    '/--------------------------------------------------------------------------------------------------------------
                Else
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8Hdp")), 0)
                    autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8OU")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS8Hdp"), Request("autoS8ML")), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8Hdp")))
                    mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8OU")))
                    mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS8Hdp"), Request("listS8ML")))
                End If
            Else
                'aHdp = Request("listM1HdpD")
                'aOU = Request("listM1HdpD")
                'aML = Request("listM1HdpD")

                'mHdp = Request("listS1HdpD")
                'mOU = Request("listS1HdpD")
                'mML = Request("listS1HdpD")
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CInt(isAutoSoccer)
                isAutoOU = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------
            End If
        Else
            param.Add("updatepositiontakingv8", 0) 'bit  

        End If
        param.Add("v8apositiontaking_1", aHdp) 'smallmoney  
        param.Add("v8apositiontaking_3", aOU) 'smallmoney 
        param.Add("v8apositiontaking_20", aML) 'smallmoney  

        param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
        param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
        param.Add("v8mpositiontaking_20", mML) 'smallmoney   

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        ' 2008-12-28: Hanh commented for disabling Master Auto PT
        If Not WebSettings.disableAutoM Then
            param.Add("auto_v8mpositiontaking_1", autoHdp) 'smallmoney 
            param.Add("auto_v8mpositiontaking_3", autoOU) 'smallmoney  
            param.Add("auto_v8mpositiontaking_20", autoML) 'smallmoney        

            param.Add("isauto_v8mpositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v8mpositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v8mpositiontaking_20", isAutoML) 'bit  
        End If
        '/--------------------------------------------------------------------------------------------------------------
        '***** End Inserting for Baseball *****
    End Sub
#End Region

#Region "Insert parameters for Motorsports Position Taking"
    Sub AddParaMotosports(ByVal isMult As Boolean)
        Dim minb, maxb, maxpermatch As Double
        If Not Request("chkUpdInfo11") Is Nothing Or Not isMult Then
            param.Add("updateminmaxbetv11", 1) 'bit  
            minb = IIf(isCopyAll, Request("txtMinbet1"), IIf(isCopyBasket, Request("txtMinbet2"), Request("txtMinbet11")))
            maxb = IIf(isCopyAll, Request("txtMaxbet1"), IIf(isCopyBasket, Request("txtMaxbet2"), Request("txtMaxbet11")))
            maxpermatch = IIf(isCopyAll, Request("txtMaxPerMatch1"), IIf(isCopyBasket, Request("txtMaxPerMatch2"), Request("txtMaxPerMatch11")))
        Else
            param.Add("updateminmaxbetv11", 0) 'bit                  
        End If
        param.Add("minbetv11", minb) 'money  
        param.Add("maxbetv11", maxb) 'money  
        param.Add("maxpermatchv11", maxpermatch) 'money   
        '***** Start Inserting for MotoSports *****
        Dim mHdp, mML As Double
        Dim aHdp, aML As Double
        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        Dim autoHdp, autoML As Double
        Dim isAutoHdp, isAutoML As Double
        '/--------------------------------------------------------------------------------------------------------------
        If Not Request("chkUpdPT11") Is Nothing Or Not isMult Then
            param.Add("updatepositiontakingv11", 1) 'bit  
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
            '/--------------------------------------------------------------------------------------------------------------

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy11Hdp") Is Nothing Then
                isCopyHdp = True
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoML = isAutoHdp
                '/--------------------------------------------------------------------------------------------------------------
            End If
            '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
            If isCopyAll Or isCopyBasket Then
                aHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                aML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
            Else
                aHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
                aML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))
            End If
            '/--------------------------------------------------------------------------------------------------------------
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then
                    'aHdp = Request("listM2Hdp")
                    'aML = Request("listM2Hdp")

                    'mHdp = Request("listS2Hdp")
                    'mML = Request("listS2Hdp")
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If
                    autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)
                    '/--------------------------------------------------------------------------------------------------------------
                Else
                    'aHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
                    'aML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))

                    'mHdp = IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11Hdp"))
                    'mML = IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11ML"))
                    '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11Hdp")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS11Hdp"), Request("autoS11ML")), 0)
                    '/--------------------------------------------------------------------------------------------------------------

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11Hdp")))
                    mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS11Hdp"), Request("listS11ML")))
                End If
            Else
                'aHdp = Request("listM1HdpD")
                'aML = Request("listM1HdpD")

                'mHdp = Request("listS1HdpD")
                'mML = Request("listS1HdpD")
                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                '/--------------------------------------------------------------------------------------------------------------

                mHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
                isAutoHdp = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
                '/--------------------------------------------------------------------------------------------------------------
            End If
        Else
            param.Add("updatepositiontakingv11", 0) 'bit  
        End If
        param.Add("v11apositiontaking_1", aHdp) 'smallmoney         
        param.Add("v11apositiontaking_20", aML) 'smallmoney  

        param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
        param.Add("v11mpositiontaking_20", mML) 'smallmoney     

        '----------------------------------------------Auto PT-----------------Tho Add----------------------------------
        ' 2008-12-28: Hanh commented for disabling Master Auto PT
        If Not WebSettings.disableAutoM Then
            param.Add("auto_v11mpositiontaking_1", autoHdp) 'smallmoney          
            param.Add("auto_v11mpositiontaking_20", autoML) 'smallmoney      

            param.Add("isauto_v11mpositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v11mpositiontaking_20", isAutoML) 'bit  
        End If
        '/--------------------------------------------------------------------------------------------------------------
        '***** End Inserting for MotoSports *****
    End Sub
#End Region

    Public Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")

    End Function
End Class
