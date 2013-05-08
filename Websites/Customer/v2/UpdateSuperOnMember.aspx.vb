Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Common

Imports IBCUtils.Encryption
Imports BLL.Customer
Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Core.Extensions


Partial Class Customer_V2_UpdateSuperOnMember
    Inherits RequiredAuthorizationPage
    Dim param As New Hashtable
    Dim isCopyAll As Boolean
    Dim isCopyBasket As Boolean



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
            Return Request("saveAgentid")
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
                UpdateData()
            End If

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)

        End Try
    End Sub
#Region "Update Data"
    Function UpdateData()
        Dim isUpdate As Boolean = True
        Try
            Dim arCID() As String = Request("saveCustid").Split("-")
            If Not Request("chkCopyAll") Is Nothing Then
                isCopyAll = True
            End If
            If Not Request("chkCopyBasket") Is Nothing Then
                isCopyBasket = True
            End If
            Dim isMult As Boolean = False
            isMult = IIf(Request("isMult") Is Nothing, 0, CBool(Request("isMult")))
            For i As Integer = 0 To arCID.Length - 1 Step 1
                If (arCID(i) <> "") Then
                    param.Clear()
                    param.Add("custid", arCID(i)) 'int  
                    If Not Request("chkClosed") Is Nothing Then
                        param.Add("closed", 1) 'bit  
                    Else
                        param.Add("closed", 0) 'bit  
                    End If
                    param.Add("userid", Session("AccId")) 'int  
                    If Session("lang") = "en-US" Then
                        param.Add("language", "E")
                    ElseIf Session("lang") = "zh-CN" Then
                        param.Add("language", "C")
                    ElseIf Session("lang") = "ja-JP" Then
                        param.Add("language", "J")
                    End If

                    param.Add("subaccid", 0) 'int  

                    param.Add("ip", Request.UserHostAddress.ToString())
                    param.Add("errcode", "") 'int  
                    param.Add("errmsg", "") 'nvarchar  
                    AddParaSoccer(isMult)
                    AddParaFootball(2, isMult)
                    AddParaFootball(3, isMult)
                    AddParaFootball(5, isMult)
                    AddParaBaseball(isMult)
                    AddParaFootball(10, isMult)
                    AddParaMotosports(isMult)
                    AddParaFootball(99, isMult)
                    AddParaMixedSports()

                    Dim nunmbergame As NumberGameManager = New NumberGameManager()
                    nunmbergame.GetParamSuperOnMember(param, HttpContext.Current)

                    Dim dError As IBCUtils.IbcError = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.UpdSuperOnMember(param)
                    If Not dError Is Nothing Then
                        If dError.ErrorCode <> 0 Then
                            isUpdate = False
                            Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI();parent.ShowError('" & RemoveSpecialChars(dError.ErrorMsg) & "','err');</script>")
                        Else

                            Response.Write("<script type=""text/javascript"">parent.doReload('Update position taking successfully.');</script>")
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            isUpdate = False
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function
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
            Dim sHdpD, sOUD, s1stHdpD, s1stOUD, sOE, s1x2, sCS, sTG, sMp, sOR As Double
            Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
            Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMp, isAutoOR As Boolean

            Dim sHdpL, sOUL, s1stHdpL, s1stOUL As Double
            Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double
            Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean

            'Updating Positon Taking in Single Edit or Allow update in Multiple Edit
            'If Not isMult Then
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
            'End If

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

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
    Function AddParaFootball(ByVal sportId As String, ByVal isMult As Boolean)
        Try
            '***** Start Inserting for Football *****
            Dim sHdp, sOU, sOE, sMP, sML As Double
            Dim autoHdp, autoOU, autoOE, autoMP, autoML As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Double

            'If Not Request("chkUpdPT" & sportId) Is Nothing Or Not isMult Then
            isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
            isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)
            If sportId = "2" Then
                isAutoMP = IIf(Request("chkAuto" & sportId & "MP") Is Nothing, 0, 1)
            End If

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
                isCopyHdp = True
                isAutoOU = isAutoHdp
                isAutoOE = isAutoHdp
                isAutoML = isAutoHdp
                isAutoMP = isAutoHdp
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
                    isAutoML = CInt(isAuto2Hdp)
                    isAutoMP = CInt(isAuto2Hdp)
                Else
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "Hdp")), 0)
                    autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OU")), 0)
                    autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "OE")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "ML")), 0)

                    sHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "Hdp")))
                    sOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OU")))
                    sOE = IIf(isAutoOE, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "OE")))
                    sML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "ML")))

                    If sportId = "2" Then
                        autoMP = IIf(isAutoMP, IIf(isCopyHdp, Request("autoS" & sportId & "Hdp"), Request("autoS" & sportId & "MP")), 0)
                        sMP = IIf(isAutoMP, 0, IIf(isCopyHdp, Request("listS" & sportId & "Hdp"), Request("listS" & sportId & "MP")))
                    End If
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
            'End If
            param.Add("v" & sportId & "spositiontaking_1", sHdp) 'smallmoney  
            param.Add("v" & sportId & "spositiontaking_3", sOU) 'smallmoney  
            param.Add("v" & sportId & "spositiontaking_2", sOE) 'smallmoney  
            param.Add("v" & sportId & "spositiontaking_20", sML) 'smallmoney  

            param.Add("auto_v" & sportId & "spositiontaking_1", autoHdp) 'smallmoney  
            param.Add("auto_v" & sportId & "spositiontaking_2", autoOE) 'smallmoney  
            param.Add("auto_v" & sportId & "spositiontaking_3", autoOU) 'smallmoney  
            param.Add("auto_v" & sportId & "spositiontaking_20", autoML) 'smallmoney 

            param.Add("isauto_v" & sportId & "spositiontaking_1", isAutoHdp) 'bit  
            param.Add("isauto_v" & sportId & "spositiontaking_2", isAutoOE) 'bit  
            param.Add("isauto_v" & sportId & "spositiontaking_3", isAutoOU) 'bit  
            param.Add("isauto_v" & sportId & "spositiontaking_20", isAutoML) 'bit  

            If sportId = "2" Then
                param.Add("v2spositiontaking_7", sHdp) 'smallmoney 
                param.Add("v2spositiontaking_8", sOU) 'smallmoney  
                param.Add("v2spositiontaking_9", sMP) 'smallmoney  

                param.Add("auto_v2spositiontaking_7", autoHdp) 'smallmoney  
                param.Add("auto_v2spositiontaking_8", autoOU) 'smallmoney 
                param.Add("auto_v2spositiontaking_9", autoMP) 'smallmoney 

                param.Add("isauto_v2spositiontaking_7", isAutoHdp) 'bit  
                param.Add("isauto_v2spositiontaking_8", isAutoOU) 'bit  
                param.Add("isauto_v2spositiontaking_9", isAutoMP) 'bit  
            End If
            '***** End Inserting for Football *****
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function

#End Region

#Region "Insert parameters for Baseball Position Taking"
    Function AddParaBaseball(ByVal isMult As Boolean)
        Try
            '***** Start Inserting for Baseball *****
            Dim sHdp, sOU, sML As Double
            Dim autoHdp, autoOU, autoML As Double
            Dim isAutoHdp, isAutoOU, isAutoML As Double
            'If Not Request("chkUpdPT8") Is Nothing Or Not isMult Then
            '    param.Add("UpdatePositiontakingv8", 1) 'bit 

            isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy8Hdp") Is Nothing Then
                isCopyHdp = True
                isAutoOU = isAutoHdp
                isAutoML = isAutoHdp
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
            'Else
            '    param.Add("UpdatePositiontakingv8", 0) 'bit  
            'End If
            param.Add("v8spositiontaking_1", sHdp) 'smallmoney  
            param.Add("v8spositiontaking_3", sOU) 'smallmoney 
            param.Add("v8spositiontaking_20", sML) 'smallmoney 

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
            '***** Start Inserting for MotoSports *****
            Dim sHdp, sML As Double
            Dim autoHdp, autoML As Double
            Dim isAutoHdp, isAutoML As Double

            'If Not Request("chkUpdPT11") Is Nothing Or Not isMult Then
            isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)
            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy11Hdp") Is Nothing Then
                isCopyHdp = True
                isAutoML = isAutoHdp
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
            'End If
            param.Add("v11spositiontaking_1", sHdp) 'smallmoney         
            param.Add("v11spositiontaking_20", sML) 'smallmoney  

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


#Region "Insert parameters for Mixed sports postion taking"
    Function AddParaMixedSports()
        Dim mixedSports As MixedSportsManager = New MixedSportsManager()
        Dim context As HttpContext = HttpContext.Current
        Dim auto As String = ""
        Dim s As String = ""
        Dim isMPAuto As Boolean
        If (isCopyAll) Then
            isMPAuto = IIf(Request("chkAuto1HdpD") Is Nothing, False, True)
            auto = IIf(isMPAuto, Request("autoS1HdpD"), 0)
            s = IIf(isMPAuto, 0, Request("listS1HdpD"))
        ElseIf isCopyBasket Then
            isMPAuto = IIf(Request("chkAuto2Hdp") Is Nothing, False, True)
            auto = IIf(isMPAuto, Request("autoS2Hdp"), 0)
            s = IIf(isMPAuto, 0, Request("listS2Hdp"))
        Else
            isMPAuto = IIf(Request("chkAutoMixParlay") Is Nothing, False, True)
            auto = IIf(isMPAuto, IIf(Request("autoSMixParlay") Is Nothing, 0, Request("autoSMixParlay")), 0)
            s = IIf(isMPAuto, 0, IIf(Request("listSMixParlay") Is Nothing, 0, Request("listSMixParlay")))
        End If
        mixedSports.AddParamSuperPTOnMember(param, auto, s, isMPAuto)
    End Function
#End Region
End Class
