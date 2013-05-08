

Imports System.Collections
Imports IBCUtils.Encryption
Imports DNAide.Core
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Entities.Casino
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Configurations
Imports Sunplus.Accounting.Core.Extensions



Partial Class Customer_V2_UpdateEditMaster
    Inherits RequiredAuthorizationPage
    Dim param As New Hashtable
    Dim isCopyAll As Boolean = False
    Dim isCopyBasket As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim errmsg As String = ""

            If Permissions.CheckPermission(CastAide.AsInt32(Session("AccId")), Enumerations.Functions.Customer.OtherCust, errmsg) <> 0 Then
                Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI(); parent.ShowError('" & AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) & "','err');parent.afterUpdate(false,0);</script>")
            Else
                Response.Write("<script type=""text/javascript"">")
                UpdateData()
                Response.Write("</script>")
            End If

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub


    Protected Overrides Function HasPermission() As Boolean
        Return True
    End Function
    Protected Overrides ReadOnly Property CustId() As Integer
        Get
            Return Request("saveCustid")
        End Get
    End Property

    Protected Overrides ReadOnly Property UplineId() As Integer
        Get
            Return Request(Sunplus.Accounting.Core.Common.Constants.QueryStringKey.SuperId)
        End Get
    End Property

    Protected Overrides Sub RedirectError()
        Response.Write("<script type=""text/javascript"">parent.showAlertMessage('err','" & StringUtils.ParseToJSMsg(Resources.ErrorMsg.error_InvalidRequest) & "',1);parent.PTUnBlockUI();</script>")
    End Sub
#Region "Update data"
    Sub UpdateData()
        Dim isUpdate As Boolean = True
        Try
            Dim isMult As Boolean = False
            Dim disableCS As Boolean = WebSettings.disableCS
            Dim closed = False
            Dim suspend = False
            'isMult = IIf(Request("isMult") Is Nothing, 0, CBool(Request("isMult")))
            If Request("isMult") Is Nothing Then
                isMult = False
            Else
                isMult = CBool(Request("isMult"))
            End If

            Dim custId As String = Request("saveCustid")
            param.Clear()
            param.Add("custid", custId) 'int 
            param.Add("firstname", RemoveSpecialChars(Request("txtFName"))) 'nvarchar  
            param.Add("lastname", RemoveSpecialChars(Request("txtLName")))  'nvarchar  
            param.Add("phone", RemoveSpecialChars(Request("txtPhone")))  'nvarchar  
            param.Add("mobilephone", RemoveSpecialChars(Request("txtMobile")))  'nvarchar  
            param.Add("updatemultiple", 0) 'bit  
            param.Add("credit", Request("txtCredit")) 'money  
            param.Add("fax", RemoveSpecialChars(Request("txtFax"))) 'nvarchar             
            param.Add("srecommend", Request("superId")) 'int                 
            param.Add("userid", Session("AccId")) 'int  
            param.Add("language", "E")
            param.Add("updatecommission", 1) 'bit  
            param.Add("groupa", Request("groupa")) 'decimal  
            param.Add("groupb", Request("groupb")) 'decimal  
            param.Add("groupc", Request("groupc")) 'decimal  
            param.Add("groupd", Request("groupd")) 'decimal  
            param.Add("discountcs", Request("discountcs")) 'decimal      

            If (Not WebSettings.Disable1x2Comm) Then
                param.Add("discount1x2", Request("discountcs1x2"))
            End If

            Dim isUpdateStatus As Integer = 1 'IIf(disableCS, 0, 1)
            param.Add("discasino", IIf(disableCS, 1, 0)) 'bit

            param.Add("updateclosed", 1) 'bit
            If Not Request("chkClosed") Is Nothing Then
                param.Add("closed", 1) 'bit 
                closed = True
            Else
                param.Add("closed", 0) 'bit 
            End If

            param.Add("updatesuspended", 1) 'bit
            If Not Request("chkSuspended") Is Nothing Then
                param.Add("suspended", 1) 'bit 
                suspend = True
            Else
                param.Add("suspended", 0) 'bit 
            End If

            Dim isUpdAll As Integer = CInt(Request("isUpdateMember"))
            param.Add("updateallmembers", isUpdAll) 'bit  
            param.Add("newspositiontaking", Request("memberPT")) 'smallmoney      
            param.Add("subaccid", Session("SubAccID"))  'int    
            param.Add("ip", Request.UserHostAddress.ToString())
            If Not Request("chkCopyAll") Is Nothing Then
                isCopyAll = True
            End If
            If Not Request("chkCopyBasket") Is Nothing Then
                isCopyBasket = True
            End If
            AddParaSoccer(isMult)
            AddParaBasketball(2, isMult)
            AddParaFootball(3, isMult)
            AddParaFootball(5, isMult)
            AddParaBaseball(isMult)
            AddParaFootball(10, isMult)
            AddParaMotosports(isMult)
            AddParaFootball(99, isMult)

            Dim numbergame As NumberGameManager = New NumberGameManager()
            Dim context As HttpContext = HttpContext.Current
            numbergame.AddParamMasterPT(param, context, isCopyAll, isCopyBasket, True)

            Dim mixedSports As MixedSportsManager = New MixedSportsManager()
            mixedSports.AddParamMasterPT(param, context)


            Dim dError As IBCUtils.IbcError = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdEditCustomerSAgRpt(param)
            If dError.IsSuccess Then

                Response.Write("parent.scroll('0','0');parent.alert_error('" + AlertMessage.GetMsg("Update Master successfully", AlertMessage.MsgType.Success) + "','err'); parent.afterUpdate(true,2000);")

                ' Update for casino
                If (Permissions.IsAllowCasino() And (context.Request("hidSynCS") <> "False") And Not isMult) Then
                    ' Store data for casino section
                    Dim custInfo As CSCustomerDetail = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetCustInfoIBC(param("custid").ToString(), param)
                    ' Sync data to Casino
                    Dim csCustomer As CasinoLibrary.SBAgent.Customer = New CasinoLibrary.SBAgent.Customer()
                    'csCustomer.SiteName = New Sunplus.Accounting.Business.Customer.CustomerManager().GetRNGSiteNameByCustId(custId)
                    csCustomer.SiteName = custInfo.SiteName
                    Dim status As Boolean = csCustomer.UpdateCustInfo(custInfo.IBCCustID, custInfo.FirstName, custInfo.LastName, custInfo.Phone, custInfo.MobilePhone, custInfo.Fax, custInfo.Closed, custInfo.Suspend, Request.UserHostAddress, AppSession.CustID, AppSession.Username, 3)
                End If

            Else
                Response.Write("parent.scroll('0','0');parent.PTUnBlockUI(); parent.showEnrichMsg('" + dError.ErrorMsg + "');")
            End If
        Catch ex As Exception
            isUpdate = False
            Response.Write("parent.scroll('0','0');parent.PTUnBlockUI(); parent.ShowError('" + AlertMessage.GetMsg(ReplaceData(ex.Message), AlertMessage.MsgType.Error) + "','err');")
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    'this function is used to add parameters
    'in Single Edit case get all parameters for Bet Setting and Positiontaking
    'in Multiple Edit case: 
    '       if Bet Setting checkbox is checked , value is updated db that is requested from Form 
    '       if Bet Setting checkbox is unchecked , value is updated db that is 0
    '       if Position Taking checkbox is checked , value is updated db that is requested from Form 
    '       if Position Taking checkbox is unchecked , value is updated db that is 0
    Function AddParaSoccer(ByVal isMult As Boolean)
        '***** Start Inserting for Soccer *****
        Try

            param.Add("updateminmaxbet", 1) 'bit  
            param.Add("minbet", Request("txtMinbet1")) 'money  
            param.Add("maxbet", Request("txtMaxbet1")) 'money  
            param.Add("maxpermatch", Request("txtMaxPerMatch1")) 'money              

            Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
            Dim sHdpD, sOUD, s1stHdpD, s1stOUD, sOE, s1x2, sCS, sTG, sMp, sOR As Double
            Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
            Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMp, isAutoOR As Boolean

            Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
            Dim sHdpL, sOUL, s1stHdpL, s1stOUL As Double
            Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double
            Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean

            'Updating Positon Taking in Single Edit or Allow update in Multiple Edit
            If Not Request("chkUpdPT1") Is Nothing Or Not isMult Then
                param.Add("updatepositiontaking", 1) 'bit  

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
                    isAutoMp = isAuto1x2
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
                    autoMp = IIf(isAutoMp, IIf(isCopy1x2, Request("autoS11x2"), Request("autoS1MP")), 0)
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
                    sMp = IIf(isAutoMp, 0, IIf(isCopy1x2, Request("listS11x2"), Request("listS1MP")))
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
                End If
            Else '' this case for Not allow update in Multiple edit
                param.Add("updatepositiontaking", 0)
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

            param.Add("isauto_spositiontaking_1", isAutoHdpD) 'bit  
            param.Add("isauto_spositiontaking_3", isAutoOUD) 'bit 
            param.Add("isauto_spositiontaking_7", isAuto1stHdpD) 'bit  
            param.Add("isauto_spositiontaking_8", isAuto1stOUD) 'bit  
            param.Add("isauto_spositiontaking_2", isAutoOE) 'bit  

            param.Add("isauto_spositiontaking_5", isAuto1x2) 'bit 
            param.Add("isauto_spositiontaking_4", isAutoCS) 'bit  
            param.Add("isauto_spositiontaking_6", isAutoTG) 'bit        
            param.Add("isauto_spositiontaking_9", isAutoMp) 'bit  
            param.Add("isauto_spositiontaking_10", isAutoOR) 'bit  

            param.Add("isauto_slivepositiontaking_1", isAutoHdpL) 'bit  
            param.Add("isauto_slivepositiontaking_3", isAutoOUL) 'bit  
            param.Add("isauto_slivepositiontaking_7", isAuto1stHdpL) 'bit  
            param.Add("isauto_slivepositiontaking_8", isAuto1stOUL) 'bit  
            '***** End Inserting for Soccer *****
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function
#End Region

#Region "Insert parameters for Basketball Position Taking"
    Function AddParaBasketball(ByVal sportId As String, ByVal isMult As Boolean)
        Try
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
            Dim sHdp, sOU, sOE, sMP, sML As Double
            Dim autoHdp, autoOU, autoOE, autoMP, autoML As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Double

            If Not Request("chkUpdPT" & sportId) Is Nothing Or Not isMult Then
                param.Add("updatepositiontakingv" & sportId, 1) 'bit  

                isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
                isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
                isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
                isAutoMP = IIf(Request("chkAuto" & sportId & "MP") Is Nothing, 0, 1)
                isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)

                Dim isCopyHdp As Boolean = False
                If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
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
                    mHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                    mOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                    mOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                    mMP = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "MP"))
                    mML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))
                End If

                If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                    If isCopyBasket Then
                        Dim isAuto2Hdp As Integer = 0
                        If Not Request("chkAuto2Hdp") Is Nothing Then
                            isAuto2Hdp = 1
                        End If
                        autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoOU = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoOE = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoMP = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)

                        sHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sOU = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sOE = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sMP = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                        isAutoHdp = CInt(isAuto2Hdp)
                        isAutoOU = CInt(isAuto2Hdp)
                        isAutoOE = CInt(isAuto2Hdp)
                        isAutoMP = CInt(isAuto2Hdp)
                        isAutoML = CInt(isAuto2Hdp)
                    Else
                        autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                        autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                        autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                        autoMP = IIf(isAutoMP, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "MP")), 0)
                        autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)

                        sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                        sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                        sOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                        sMP = IIf(isAutoMP, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "MP")))
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
                    autoMP = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)
                    autoML = IIf(isAutoSoccer, Request("autoS1HdpD"), 0)

                    sHdp = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                    sOU = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                    sOE = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                    sMP = IIf(isAutoSoccer, 0, Request("listS1HdpD"))
                    sML = IIf(isAutoSoccer, 0, Request("listS1HdpD"))

                    isAutoHdp = CInt(isAutoSoccer)
                    isAutoOU = CInt(isAutoSoccer)
                    isAutoOE = CInt(isAutoSoccer)
                    isAutoMP = CInt(isAutoSoccer)
                    isAutoML = CInt(isAutoSoccer)
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, 0) 'bit  
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

            param.Add("isauto_v2spositiontaking_1", isAutoHdp) 'bit 
            param.Add("isauto_v2spositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v2spositiontaking_7", isAutoHdp) 'bit  
            param.Add("isauto_v2spositiontaking_8", isAutoOU) 'bit  
            param.Add("isauto_v2spositiontaking_2", isAutoOE) 'bit 
            param.Add("isauto_v2spositiontaking_9", isAutoMP) 'bit  
            param.Add("isauto_v2spositiontaking_20", isAutoML) 'bit  

            '***** End Inserting for Football *****
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function

#End Region

#Region "Insert parameters for Football, Tennis, Golf, Other Sports Position Taking"
    Function AddParaFootball(ByVal sportId As String, ByVal isMult As Boolean)
        Try
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
            Dim mHdp, mOU, mOE, mML As Double
            Dim sHdp, sOU, sOE, sML As Double
            Dim autoHdp, autoOU, autoOE, autoML As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoML As Double

            If Not Request("chkUpdPT" & sportId) Is Nothing Or Not isMult Then
                param.Add("updatepositiontakingv" & sportId, 1) 'bit  

                isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
                isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
                isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
                isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)

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

                If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                    If isCopyBasket Then
                        Dim isAuto2Hdp As Integer = 0
                        If Not Request("chkAuto2Hdp") Is Nothing Then
                            isAuto2Hdp = 1
                        End If
                        autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoOU = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoOE = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)

                        sHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sOU = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sOE = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                        isAutoHdp = CInt(isAuto2Hdp)
                        isAutoOU = CInt(isAuto2Hdp)
                        isAutoOE = CInt(isAuto2Hdp)
                        isAutoML = CInt(isAuto2Hdp)
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

                    isAutoHdp = CInt(isAutoSoccer)
                    isAutoOU = CInt(isAutoSoccer)
                    isAutoOE = CInt(isAutoSoccer)
                    isAutoML = CInt(isAutoSoccer)
                End If
            Else
                param.Add("updatepositiontakingv" & sportId, 0) 'bit  
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

            param.Add("isauto_v" & sportId & "spositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v" & sportId & "spositiontaking_2", isAutoOE) 'bit  
            param.Add("isauto_v" & sportId & "spositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v" & sportId & "spositiontaking_20", isAutoML) 'bit             
            '***** End Inserting for Football *****
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function

#End Region

#Region "Insert parameters for Baseball Position Taking"
    Function AddParaBaseball(ByVal isMult As Boolean)
        Try
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
            Dim sHdp, sOU, sML As Double
            Dim autoHdp, autoOU, autoML As Double
            Dim isAutoHdp, isAutoOU, isAutoML As Double
            If Not Request("chkUpdPT8") Is Nothing Or Not isMult Then
                param.Add("updatepositiontakingv8", 1) 'bit 

                isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
                isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
                isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)

                Dim isCopyHdp As Boolean = False
                If Not Request("chkCopy8Hdp") Is Nothing Then
                    isCopyHdp = True
                    isAutoOU = isAutoHdp
                    isAutoML = isAutoHdp
                End If
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
                        Dim isAuto2Hdp As Integer = 0
                        If Not Request("chkAuto2Hdp") Is Nothing Then
                            isAuto2Hdp = 1
                        End If
                        autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoOU = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)

                        sHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sOU = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                        isAutoHdp = CInt(isAuto2Hdp)
                        isAutoOU = CInt(isAuto2Hdp)
                        isAutoML = CInt(isAuto2Hdp)
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

                    isAutoHdp = CInt(isAutoSoccer)
                    isAutoOU = CInt(isAutoSoccer)
                    isAutoML = CInt(isAutoSoccer)
                End If
            Else
                param.Add("updatepositiontakingv8", 0) 'bit  
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

            param.Add("isauto_v8spositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v8spositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v8spositiontaking_20", isAutoML) 'bit  
            '***** End Inserting for Baseball *****
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function
#End Region

#Region "Insert parameters for Motorsports Position Taking"
    Function AddParaMotosports(ByVal isMult As Boolean)
        Try
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
            Dim sHdp, sML As Double
            Dim autoHdp, autoML As Double
            Dim isAutoHdp, isAutoML As Double

            If Not Request("chkUpdPT11") Is Nothing Or Not isMult Then
                param.Add("updatepositiontakingv11", 1) 'bit  

                isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
                isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
                Dim isCopyHdp As Boolean = False
                If Not Request("chkCopy11Hdp") Is Nothing Then
                    isCopyHdp = True
                    isAutoML = isAutoHdp
                End If
                If isCopyAll Or isCopyBasket Then
                    mHdp = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                    mML = IIf(isCopyAll, Request("listM1HdpD"), Request("listM2Hdp"))
                Else
                    mHdp = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp"))
                    mML = IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML"))
                End If
                If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                    If isCopyBasket Then
                        Dim isAuto2Hdp As Integer = 0
                        If Not Request("chkAuto2Hdp") Is Nothing Then
                            isAuto2Hdp = 1
                        End If
                        autoHdp = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)
                        autoML = IIf(isAuto2Hdp, Request("autoS2Hdp"), 0)

                        sHdp = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))
                        sML = IIf(isAuto2Hdp, 0, Request("listS2Hdp"))

                        isAutoHdp = CInt(isAuto2Hdp)
                        isAutoML = CInt(isAuto2Hdp)
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

                    isAutoHdp = CInt(isAutoSoccer)
                    isAutoML = CInt(isAutoSoccer)
                End If
            Else
                param.Add("updatepositiontakingv11", 0) 'bit  
            End If
            param.Add("v11spositiontaking_1", sHdp) 'smallmoney         
            param.Add("v11spositiontaking_20", sML) 'smallmoney  

            param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
            param.Add("v11mpositiontaking_20", mML) 'smallmoney 

            param.Add("auto_v11spositiontaking_1", autoHdp) 'smallmoney          
            param.Add("auto_v11spositiontaking_20", autoML) 'smallmoney      

            param.Add("isauto_v11spositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v11spositiontaking_20", isAutoML) 'bit  
            '***** End Inserting for MotoSports *****
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function
#End Region

    Public Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")

    End Function
End Class
