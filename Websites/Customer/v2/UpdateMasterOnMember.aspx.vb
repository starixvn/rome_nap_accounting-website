Imports BLL.Customer
Imports Sunplus.Accounting.Common

Imports Sunplus.Accounting.Business.PT.SportBooks
Imports Sunplus.Accounting.Core.Helper
Imports Sunplus.Accounting.Business.Security
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.Core.Extensions


Partial Class Customer_V2_UpdateMasterOnMember
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
                Response.Write("<script type=""text/javascript"">parent.parent.jQuery.unblockUI(); parent.ShowError('" & AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) & "','err');</script>")
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


                    param.Add("allowupdatepositiontaking", 1) 'always allow update in account site

                    param.Add("ip", Request.UserHostAddress.ToString())
                    param.Add("subaccid", 0) 'int  
                    'bit  
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

                    Dim numbergame As NumberGameManager = New NumberGameManager()
                    numbergame.GetParamMasterOnMember(param, HttpContext.Current)

                    Dim dError As IBCUtils.IbcError
                    dError = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.UpdMasterOnMember(param)
                    If Not dError Is Nothing Then
                        If dError.ErrorCode <> 0 Then
                            isUpdate = False
                            Response.Write("<script type=""text/javascript"">parent.PTUnBlockUI();parent.ShowError('" & IBCUtils.Encryption.RemoveSpecialChars(dError.ErrorMsg) & "','err');</script>")
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
            Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
            Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
            'Updating Positon Taking in Single Edit or Allow update in Multiple Edit

            Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double
            Dim isAutoHdpD, isAutoOUD, isAuto1stHdpD, isAuto1stOUD, isAutoOE, isAuto1x2, isAutoCS, isAutoTG, isAutoMp, isAutoOR As Boolean

            Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double
            Dim isAutoHdpL, isAutoOUL, isAuto1stHdpL, isAuto1stOUL As Boolean


            'auto master on member -huy add
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
            'end


            Dim isCopyHdpD As Boolean = False
            If Not Request("chkCopy1HdpDeadball") Is Nothing Then
                isCopyHdpD = True
                'auto master on member -huy add
                isAutoOUD = isAutoHdpD
                isAuto1stHdpD = isAutoHdpD
                isAuto1stOUD = isAutoHdpD
                isAutoOE = isAutoHdpD

            End If
            Dim isCopy1x2 As Boolean = False
            If Not Request("chkCopy11x2") Is Nothing Then
                isCopy1x2 = True
                'auto master on member -huy add
                isAutoCS = isAuto1x2
                isAutoTG = isAuto1x2
                isAutoMp = isAuto1x2
                isAutoOR = isAuto1x2

            End If
            Dim isCopyHdpL As Boolean = False
            If Not Request("chkCopy1HdpLive") Is Nothing Then
                isCopyHdpL = True
                'auto master on member -huy add
                isAutoOUL = isAutoHdpL
                isAuto1stHdpL = isAutoHdpL
                isAuto1stOUL = isAutoHdpL
            End If
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked              
                mHdpD = IIf(isCopyHdpD, IIf(Request("isAutoHdp"), 0, Request("listM1HdpD")), Request("listM1HdpD"))
                mOUD = IIf(isCopyHdpD, IIf(Request("isAutoOU"), 0, Request("listM1HdpD")), Request("listM1OUD"))
                m1stHdpD = IIf(isCopyHdpD, IIf(Request("isAuto1stHdp"), 0, Request("listM1HdpD")), Request("listM11stHdpD"))
                m1stOUD = IIf(isCopyHdpD, IIf(Request("isAuto1stOU"), 0, Request("listM1HdpD")), Request("listM11stOUD"))
                mOE = IIf(isCopyHdpD, IIf(Request("isAutoOE"), 0, Request("listM1HdpD")), Request("listM1OE"))

                m1x2 = IIf(isCopy1x2, IIf(Request("isAuto1x2"), 0, Request("listM11x2")), Request("listM11x2"))
                mCS = IIf(isCopy1x2, IIf(Request("isAutoCS"), 0, Request("listM11x2")), Request("listM1CS"))
                mTG = IIf(isCopy1x2, IIf(Request("isAutoTG"), 0, Request("listM11x2")), Request("listM1TG"))
                mMp = IIf(isCopy1x2, IIf(Request("isAutoMP"), 0, Request("listM11x2")), Request("listM1MP"))
                mOR = IIf(isCopy1x2, IIf(Request("isAutoOR"), 0, Request("listM11x2")), Request("listM1OR"))

                mHdpL = IIf(isCopyHdpL, IIf(Request("isAutoLHdp"), 0, Request("listM1HdpL")), Request("listM1HdpL"))
                mOUL = IIf(isCopyHdpL, IIf(Request("isAutoLOU"), 0, Request("listM1HdpL")), Request("listM1OUL"))
                m1stHdpL = IIf(isCopyHdpL, IIf(Request("isAutoL1stHdp"), 0, Request("listM1HdpL")), Request("listM11stHdpL"))
                m1stOUL = IIf(isCopyHdpL, IIf(Request("isAutoL1stOU"), 0, Request("listM1HdpL")), Request("listM11stOUL"))

                'auto master on member -huy add
                autoHdpD = IIf(isAutoHdpD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM1HdpD")), 0)
                autoOUD = IIf(isAutoOUD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM1OUD")), 0)
                auto1stHdpD = IIf(isAuto1stHdpD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM11stHdpD")), 0)
                auto1stOUD = IIf(isAuto1stOUD, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM11stOUD")), 0)
                autoOE = IIf(isAutoOE, IIf(isCopyHdpD, Request("autoM1HdpD"), Request("autoM1OE")), 0)

                auto1x2 = IIf(isAuto1x2, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM11x2")), 0)
                autoCS = IIf(isAutoCS, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1CS")), 0)
                autoTG = IIf(isAutoTG, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1TG")), 0)
                autoMp = IIf(isAutoMp, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1MP")), 0)
                autoOR = IIf(isAutoOR, IIf(isCopy1x2, Request("autoM11x2"), Request("autoM1OR")), 0)

                autoHdpL = IIf(isAutoHdpL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM1HdpL")), 0)
                autoOUL = IIf(isAutoOUL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM1OUL")), 0)
                auto1stHdpL = IIf(isAuto1stHdpL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM11stHdpL")), 0)
                auto1stOUL = IIf(isAuto1stOUL, IIf(isCopyHdpL, Request("autoM1HdpL"), Request("autoM11stOUL")), 0)

            Else
                'auto master on member -huy add
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If

                autoHdpD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoOUD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                auto1stHdpD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                auto1stOUD = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

                auto1x2 = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoCS = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoTG = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoMp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoOR = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

                autoHdpL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoOUL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                auto1stHdpL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                auto1stOUL = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)


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
                'end auto master on member


                mHdpD = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mOUD = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                m1stHdpD = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                m1stOUD = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mOE = IIf(isAutoSoccer, 0, Request("listM1HdpD"))

                m1x2 = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mCS = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mTG = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mMp = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mOR = IIf(isAutoSoccer, 0, Request("listM1HdpD"))

                mHdpL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mOUL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                m1stHdpL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                m1stOUL = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
            End If
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

            'auto master on member -huy add
            param.Add("auto_mpositiontaking_mem_1", autoHdpD) 'smallmoney    
            param.Add("auto_mpositiontaking_mem_3", autoOUD) 'smallmoney    
            param.Add("auto_mpositiontaking_mem_7", auto1stHdpD) 'smallmoney 
            param.Add("auto_mpositiontaking_mem_8", auto1stOUD) 'smallmoney  
            param.Add("auto_mpositiontaking_mem_2", autoOE) 'smallmoney                  

            param.Add("auto_mpositiontaking_mem_5", auto1x2) 'smallmoney 
            param.Add("auto_mpositiontaking_mem_4", autoCS) 'smallmoney
            param.Add("auto_mpositiontaking_mem_6", autoTG) 'smallmoney             
            param.Add("auto_mpositiontaking_mem_9", autoMp) 'smallmoney  
            param.Add("auto_mpositiontaking_mem_10", autoOR) 'smallmoney 

            param.Add("auto_mlivepositiontaking_mem_1", autoHdpL) 'smallmoney     
            param.Add("auto_mlivepositiontaking_mem_3", autoOUL) 'smallmoney     
            param.Add("auto_mlivepositiontaking_mem_7", auto1stHdpL) 'smallmoney   
            param.Add("auto_mlivepositiontaking_mem_8", auto1stOUL) 'smallmoney  

            param.Add("isauto_mpositiontaking_mem_1", isAutoHdpD) 'bit  
            param.Add("isauto_mpositiontaking_mem_3", isAutoOUD) 'bit 
            param.Add("isauto_mpositiontaking_mem_7", isAuto1stHdpD) 'bit  
            param.Add("isauto_mpositiontaking_mem_8", isAuto1stOUD) 'bit  

            param.Add("isauto_mpositiontaking_mem_2", isAutoOE) 'bit 
            param.Add("isauto_mpositiontaking_mem_5", isAuto1x2) 'bit 
            param.Add("isauto_mpositiontaking_mem_4", isAutoCS) 'bit  
            param.Add("isauto_mpositiontaking_mem_6", isAutoTG) 'bit        
            param.Add("isauto_mpositiontaking_mem_9", isAutoMp) 'bit  
            param.Add("isauto_mpositiontaking_mem_10", isAutoOR) 'bit  

            param.Add("isauto_mlivepositiontaking_mem_1", isAutoHdpL) 'bit  
            param.Add("isauto_mlivepositiontaking_mem_3", isAutoOUL) 'bit  
            param.Add("isauto_mlivepositiontaking_mem_7", isAuto1stHdpL) 'bit  
            param.Add("isauto_mlivepositiontaking_mem_8", isAuto1stOUL) 'bit  

        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function
#End Region

#Region "Insert parameters for Basketball, Football, Tennis, Golf, Other Sports Position Taking"
    Function AddParaFootball(ByVal sportId As String, ByVal isMult As Boolean)
        Try
            '***** Start Inserting for Football *****
            Dim mHdp, mOU, mOE, mMP, mML As Double

            'auto master on member -huy add
            Dim autoHdp, autoOU, autoOE, autoMP, autoML As Double
            Dim isAutoHdp, isAutoOU, isAutoOE, isAutoMP, isAutoML As Double

            isAutoHdp = IIf(Request("chkAuto" & sportId & "Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto" & sportId & "OU") Is Nothing, 0, 1)
            isAutoOE = IIf(Request("chkAuto" & sportId & "OE") Is Nothing, 0, 1)
            isAutoMP = IIf(Request("chkAuto" & sportId & "MP") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto" & sportId & "ML") Is Nothing, 0, 1)

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy" & sportId & "Hdp") Is Nothing Then
                isCopyHdp = True

                'auto master on member -huy add
                isAutoOU = isAutoHdp
                isAutoOE = isAutoHdp
                isAutoMP = isAutoHdp
                isAutoML = isAutoHdp

            End If
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then

                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If

                    'auto master on member -huy add
                    autoHdp = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                    autoOU = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                    autoOE = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                    If sportId = "2" Then
                        autoMP = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                        mMP = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))
                    End If

                    mHdp = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))
                    mOU = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))
                    mOE = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))

                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoOU = CInt(isAuto2Hdp)
                    isAutoOE = CInt(isAuto2Hdp)
                    isAutoMP = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)

                Else
                    mHdp = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "Hdp"))
                    mOU = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OU"))
                    mOE = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "OE"))
                    mML = IIf(isCopyHdp, Request("listM" & sportId & "Hdp"), Request("listM" & sportId & "ML"))
                    If sportId = "2" Then
                        mMP = IIf(isCopyHdp, IIf(Request("isAuto_V" & sportId & "MP"), 0, Request("listM" & sportId & "Hdp")), Request("listM" & sportId & "MP"))
                    End If

                    'auto master on member -huy add
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "Hdp")), 0)
                    autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "OU")), 0)
                    autoOE = IIf(isAutoOE, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "OE")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "ML")), 0)

                    mHdp = IIf(isAutoHdp, 0, mHdp)
                    mOU = IIf(isAutoOU, 0, mOU)
                    mOE = IIf(isAutoOE, 0, mOE)
                    mML = IIf(isAutoMP, 0, mML)
                    If sportId = "2" Then
                        autoMP = IIf(isAutoMP, IIf(isCopyHdp, Request("autoM" & sportId & "Hdp"), Request("autoM" & sportId & "MP")), 0)
                        mMP = IIf(isAutoMP, 0, mMP)
                    End If
                End If
            Else

                If sportId = "2" Then
                    mMP = IIf(Request("isAuto_V" & sportId & "MP"), 0, Request("listM1HdpD"))
                End If

                'auto master on member -huy add
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

                If sportId = "2" Then
                    autoMP = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                    mMP = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                End If

                mHdp = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mOU = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mOE = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listM1HdpD"))


                isAutoHdp = CInt(isAutoSoccer)
                isAutoOU = CInt(isAutoSoccer)
                isAutoOE = CInt(isAutoSoccer)
                isAutoMP = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
            End If
            param.Add("v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
            param.Add("v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
            param.Add("v" & sportId & "mpositiontaking_2", mOE) 'smallmoney  
            param.Add("v" & sportId & "mpositiontaking_20", mML) 'smallmoney  

            'auto master on member -huy add
            param.Add("auto_v" & sportId & "mpositiontaking_mem_1", autoHdp) 'smallmoney 
            param.Add("auto_v" & sportId & "mpositiontaking_mem_3", autoOU) 'smallmoney  
            param.Add("auto_v" & sportId & "mpositiontaking_mem_2", autoOE) 'smallmoney  
            param.Add("auto_v" & sportId & "mpositiontaking_mem_20", autoML) 'smallmoney 

            param.Add("isauto_v" & sportId & "mpositiontaking_mem_1", isAutoHdp) 'bit 
            param.Add("isauto_v" & sportId & "mpositiontaking_mem_3", isAutoOU) 'bit  
            param.Add("isauto_v" & sportId & "mpositiontaking_mem_2", isAutoOE) 'bit 
            param.Add("isauto_v" & sportId & "mpositiontaking_mem_20", isAutoML) 'bit

            If sportId = "2" Then
                param.Add("v2mpositiontaking_7", mHdp) 'smallmoney 
                param.Add("v2mpositiontaking_8", mOU) 'smallmoney                  
                param.Add("v2mpositiontaking_9", mMP) 'smallmoney   

                param.Add("auto_v2mpositiontaking_mem_7", autoHdp) 'smallmoney  
                param.Add("auto_v2mpositiontaking_mem_8", autoOU) 'smallmoney
                param.Add("auto_v2mpositiontaking_mem_9", autoMP) 'smallmoney 

                param.Add("isauto_v2mpositiontaking_mem_7", isAutoHdp) 'bit  
                param.Add("isauto_v2mpositiontaking_mem_8", isAutoOU) 'bit
                param.Add("isauto_v2mpositiontaking_mem_9", isAutoMP) 'bit 
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
            Dim mHdp, mOU, mML As Double

            'auto master on member -huy add
            Dim autoHdp, autoOU, autoML As Double
            Dim isAutoHdp, isAutoOU, isAutoML As Double

            isAutoHdp = IIf(Request("chkAuto8Hdp") Is Nothing, 0, 1)
            isAutoOU = IIf(Request("chkAuto8OU") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto8ML") Is Nothing, 0, 1)

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy8Hdp") Is Nothing Then
                isCopyHdp = True
                'auto master on member -huy add
                isAutoOU = isAutoHdp
                isAutoML = isAutoHdp

            End If
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then


                    'auto master on member -huy add
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If
                    autoHdp = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                    autoOU = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)

                    mHdp = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))
                    mOU = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))

                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoOU = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)
                Else

                    'Auto master on member -huy add
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoM8Hdp"), Request("autoM8Hdp")), 0)
                    autoOU = IIf(isAutoOU, IIf(isCopyHdp, Request("autoM8Hdp"), Request("autoM8OU")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoM8Hdp"), Request("autoM8ML")), 0)

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8Hdp")))
                    mOU = IIf(isAutoOU, 0, IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8OU")))
                    mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listM8Hdp"), Request("listM8ML")))


                End If
            Else

                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

                mHdp = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mOU = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listM1HdpD"))


                isAutoHdp = CInt(isAutoSoccer)
                isAutoOU = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)


            End If
            param.Add("v8mpositiontaking_1", mHdp) 'smallmoney  
            param.Add("v8mpositiontaking_3", mOU) 'smallmoney 
            param.Add("v8mpositiontaking_20", mML) 'smallmoney 

            param.Add("auto_v8mpositiontaking_mem_1", autoHdp) 'smallmoney 
            param.Add("auto_v8mpositiontaking_mem_3", autoOU) 'smallmoney  
            param.Add("auto_v8mpositiontaking_mem_20", autoML) 'smallmoney        

            param.Add("isauto_v8mpositiontaking_mem_1", isAutoHdp) 'bit  
            param.Add("isauto_v8mpositiontaking_mem_3", isAutoOU) 'bit  
            param.Add("isauto_v8mpositiontaking_mem_20", isAutoML) 'bit  
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
            Dim mHdp, mML As Double
            Dim autoHdp, autoML As Double
            Dim isAutoHdp, isAutoML As Double
            'auto master on member -huy add
            isAutoHdp = IIf(Request("chkAuto11Hdp") Is Nothing, 0, 1)
            isAutoML = IIf(Request("chkAuto11ML") Is Nothing, 0, 1)

            Dim isCopyHdp As Boolean = False
            If Not Request("chkCopy11Hdp") Is Nothing Then
                isCopyHdp = True
                isAutoML = isAutoHdp
            End If
            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then

                    'auto master on member -huy add
                    Dim isAuto2Hdp As Integer = 0
                    If Not Request("chkAuto2Hdp") Is Nothing Then
                        isAuto2Hdp = 1
                    End If
                    autoHdp = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)
                    autoML = IIf(isAuto2Hdp, Request("autoM2Hdp"), 0)

                    mHdp = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))
                    mML = IIf(isAuto2Hdp, 0, Request("listM2Hdp"))

                    isAutoHdp = CInt(isAuto2Hdp)
                    isAutoML = CInt(isAuto2Hdp)

                Else
                    'auto master on member -huy add
                    autoHdp = IIf(isAutoHdp, IIf(isCopyHdp, Request("autoM11Hdp"), Request("autoM11Hdp")), 0)
                    autoML = IIf(isAutoML, IIf(isCopyHdp, Request("autoM11Hdp"), Request("autoM11ML")), 0)

                    mHdp = IIf(isAutoHdp, 0, IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11Hdp")))
                    mML = IIf(isAutoML, 0, IIf(isCopyHdp, Request("listM11Hdp"), Request("listM11ML")))
                End If
            Else
                'auto master on member -huy add
                Dim isAutoSoccer As Boolean = False
                If Not Request("chkAuto1HdpD") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)
                autoML = IIf(isAutoSoccer, Request("autoM1HdpD"), 0)

                mHdp = IIf(isAutoSoccer, 0, Request("listM1HdpD"))
                mML = IIf(isAutoSoccer, 0, Request("listM1HdpD"))

                isAutoHdp = CInt(isAutoSoccer)
                isAutoML = CInt(isAutoSoccer)
            End If
            param.Add("v11mpositiontaking_1", mHdp) 'smallmoney         
            param.Add("v11mpositiontaking_20", mML) 'smallmoney 

            param.Add("auto_v11mpositiontaking_mem_1", autoHdp) 'smallmoney          
            param.Add("auto_v11mpositiontaking_mem_20", autoML) 'smallmoney      

            param.Add("isauto_v11mpositiontaking_mem_1", isAutoHdp) 'bit  
            param.Add("isauto_v11mpositiontaking_mem_20", isAutoML) 'bit  
            '***** End Inserting for MotoSports *****
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
        End Try
    End Function
#End Region


#Region "Add parameters for Mixed sports position taking"
    Function AddParaMixedSports()
        Dim auto, m As Double
        Dim isMAuto As Boolean
        Dim mixedSport As MixedSportsManager = New MixedSportsManager()

        If isCopyAll Then
            isMAuto = IIf(Request("chkAuto1HdpD") Is Nothing, False, True)
            auto = IIf(isMAuto, Request("autoM1HdpD"), 0)
            m = IIf(isMAuto, 0, Request("listM1HdpD"))
        ElseIf isCopyBasket Then
            isMAuto = IIf(Request("chkAuto2Hdp") Is Nothing, False, True)
            auto = IIf(isMAuto, Request("autoM2Hdp"), 0)
            m = IIf(isMAuto, 0, Request("listM2Hdp"))
        Else
            isMAuto = IIf(Request("chkAutoMixParlay") Is Nothing, False, True)
            auto = IIf(isMAuto, IIf(Request("autoMMixParlay") Is Nothing, 0, Request("autoMMixParlay")), 0)
            m = IIf(isMAuto, 0, IIf(Request("listMMixParlay") Is Nothing, 0, Request("listMMixParlay")))
        End If
        mixedSport.AddParamMasterPTOnMember(param, auto, m, isMAuto)
    End Function
#End Region

End Class
