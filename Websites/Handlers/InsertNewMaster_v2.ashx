<%@ WebHandler Language="VB" Class="InsertNewMaster_v2" %>

Imports System
Imports System.Web
Imports System.Collections
Imports Sunplus.Accounting.Core.Common
Imports Sunplus.Accounting.DataAccess.IBCDLL.Customer
Imports IBCUtils
Imports IBCDLL.Customer
Imports System.Web.SessionState
Imports Sunplus.Accounting.Core.Extensions


Public Class InsertNewMaster_v2 : Implements IHttpHandler, IRequiresSessionState
    Dim param As New Hashtable
    Dim isCopyAll As Boolean = False
    Dim isCopyBasket As Boolean = False
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/html"
        context.Response.Write("<html><head></head><body>")
        Try
            InsertData(context)
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
            
        End Try
        context.Response.Write("</body></html>")
    End Sub
#Region "Update data"
    Sub InsertData(ByVal context As HttpContext)
        Try
            With context
                Dim resultUpdated As New Hashtable()
                param.Add("@custid", 0) 'int  
                param.Add("@firstname", IBCUtils.Encryption.RemoveSpecialChars(.Request("txtFName"))) 'nvarchar  
                param.Add("@lastname", IBCUtils.Encryption.RemoveSpecialChars(.Request("txtLName"))) 'nvarchar  
                param.Add("@creator", .Session("AccId")) 'int  
                param.Add("@username", .Request("txtUsername").Trim)  'varchar  
                param.Add("@userpwd", IBCUtils.Encryption.CfsEnCode(.Request("txtPwd").Trim)) 'varchar  
                param.Add("@phone", IBCUtils.Encryption.RemoveSpecialChars(.Request("txtPhone"))) 'nvarchar  
                param.Add("@mobilephone", IBCUtils.Encryption.RemoveSpecialChars(.Request("txtMobile"))) 'nvarchar  
                param.Add("@fax", IBCUtils.Encryption.RemoveSpecialChars(.Request("txtFax"))) 'nvarchar  
                param.Add("@email", "") 'nvarchar  
                param.Add("@address", "") 'nvarchar  
                param.Add("@city", "") 'nvarchar  
                param.Add("@state", "") 'nvarchar  
                param.Add("@country", "") 'nvarchar  
                param.Add("@postal", "") 'nvarchar  
                Dim strUpdTranfer As String = "00000000"
                If .Request("tranfer") = "daily" Then
                    strUpdTranfer = "00000000" '
                Else
                    If Not .Request("chkSun") Is Nothing Then
                        strUpdTranfer &= "1"
                    Else
                        strUpdTranfer &= "0"
                    End If
                    If Not .Request("chkMon") Is Nothing Then
                        strUpdTranfer &= "1"
                    Else
                        strUpdTranfer &= "0"
                    End If
                    If Not .Request("chkTue") Is Nothing Then
                        strUpdTranfer &= "1"
                    Else
                        strUpdTranfer &= "0"
                    End If
                    If Not .Request("chkWed") Is Nothing Then
                        strUpdTranfer &= "1"
                    Else
                        strUpdTranfer &= "0"
                    End If
                    If Not .Request("chkThurs") Is Nothing Then
                        strUpdTranfer &= "1"
                    Else
                        strUpdTranfer &= "0"
                    End If
                    If Not .Request("chkFri") Is Nothing Then
                        strUpdTranfer &= "1"
                    Else
                        strUpdTranfer &= "0"
                    End If
                    If Not .Request("chkSat") Is Nothing Then
                        strUpdTranfer &= "1"
                    Else
                        strUpdTranfer &= "0"
                    End If
                    strUpdTranfer &= "1"
                End If
                param.Add("@TransferOption", strUpdTranfer) 'varchar  
                If Not .Request("chkCopyAll") Is Nothing Then
                    isCopyAll = True
                End If
                If Not .Request("chkCopyBasket") Is Nothing Then
                    isCopyBasket = True
                End If
                param.Add("@credit", .Request("txtCredit")) 'money                  
                param.Add("@srecommend", .Session("CustID")) 'int 
                param.Add("@groupa", .Request("groupa")) 'decimal  
                param.Add("@groupb", .Request("groupb")) 'decimal  
                param.Add("@groupc", .Request("groupc")) 'decimal  
                param.Add("@groupd", .Request("groupd")) 'decimal  
                param.Add("@discountcs", .Request("discountcs")) 'decimal          

                If .Session("lang") = "en-US" Then
                    param.Add("@Language", "E")
                ElseIf .Session("lang") = "zh-CN" Then
                    param.Add("@Language", "C")
                ElseIf .Session("lang") = "ja-JP" Then
                    param.Add("@Language", "J")
                End If
                param.Add("@SubAccID", 0) 'int     
                param.Add("@ReturnErrCode", 1) 'bit  
                param.Add("@ErrCode", 0) 'int  
                param.Add("@ErrMsg", "") 'nvarchar  
                param.Add("@site", .Session("sitename")) 'varchar  
                AddParaSoccer(context)
                AddParaBasketball(2, context)
                AddParaFootball(3, context)
                AddParaFootball(5, context)
                AddParaBaseball(context)
                AddParaFootball(10, context)
                AddParaMotosports(context)
                AddParaFinancial(context)
                AddParaFootball(99, context)
                resultUpdated = UpdateCustomer.CustomerInsMemberSAgRpt(param)
                If Not resultUpdated("errCode") Is Nothing Then
                    If CInt(resultUpdated("errCode")) <> 0 Then
                        .Response.Write("<script type=""text/javascript"">parent.ShowError('" & IBCUtils.Encryption.RemoveSpecialChars(resultUpdated("@ErrMsg")) & "','err');</script>")
                    Else
                        If CInt(context.Request("superId")) < 100 Then
                            .Response.Write("parent.parent.location.href='../Customer/MasterAgentList.aspx?bycom=1';")
                        Else
                            .Response.Write("parent.parent.location.href='../Customer/MasterAgentList.aspx?superId=" + context.Request("superId") + "';")
                        End If
                    End If
                End If
            End With
        Catch ex As Exception
            LogExtensions.ExceptionHandler(ex)
            
        End Try
    End Sub
#End Region

#Region "Insert parameters for Soccer Position Taking"
    Sub AddParaSoccer(ByVal context As HttpContext)
        'Add Bet Setting parameters
        With context
            param.Add("@minbet", .Request("txtMinbet1")) 'money  
            param.Add("@maxbet", .Request("txtMaxbet1")) 'money  
            param.Add("@maxpermatch", .Request("txtMaxPerMatch1")) 'money 
            '**************************
            '***** Add Position Taking paramaters*****
            Dim mHdpD, mOUD, m1stHdpD, m1stOUD, mOE, m1x2, mCS, mTG, mMp, mOR As Double
            Dim sHdpD, sOUD, s1stHdpD, s1stOUD, sOE, s1x2, sCS, sTG, sMp, sOR As Double
            Dim autoHdpD, autoOUD, auto1stHdpD, auto1stOUD, autoOE, auto1x2, autoCS, autoTG, autoMp, autoOR As Double

            Dim mHdpL, mOUL, m1stHdpL, m1stOUL As Double
            Dim sHdpL, sOUL, s1stHdpL, s1stOUL As Double
            Dim autoHdpL, autoOUL, auto1stHdpL, auto1stOUL As Double

            Dim isAuto As Boolean = False
            If Not .Request("chkAuto1") Is Nothing Then
                isAuto = True
            End If

            Dim isCopyHdpD As Boolean = False
            If Not .Request("chkCopy1HdpDeadball") Is Nothing Then
                isCopyHdpD = True
            End If
            Dim isCopy1x2 As Boolean = False
            If Not .Request("chkCopy11x2") Is Nothing Then
                isCopy1x2 = True
            End If
            Dim isCopyHdpL As Boolean = False
            If Not .Request("chkCopy1HdpLive") Is Nothing Then
                isCopyHdpL = True
            End If

            mHdpD = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpD, .Request("listM1HdpD"), .Request("listM1HdpD")))
            mOUD = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpD, .Request("listM1HdpD"), .Request("listM1OUD")))
            m1stHdpD = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpD, .Request("listM1HdpD"), .Request("listM11stHdpD")))
            m1stOUD = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpD, .Request("listM1HdpD"), .Request("listM11stOUD")))
            mOE = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpD, .Request("listM1HdpD"), .Request("listM1OE")))

            m1x2 = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopy1x2, .Request("listM11x2"), .Request("listM11x2")))
            mCS = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopy1x2, .Request("listM11x2"), .Request("listM1CS")))
            mTG = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopy1x2, .Request("listM11x2"), .Request("listM1TG")))
            mMp = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopy1x2, .Request("listM11x2"), .Request("listM1MP")))
            mOR = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopy1x2, .Request("listM11x2"), .Request("listM1OR")))

            mHdpL = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpL, .Request("listM1HdpL"), .Request("listM1HdpL")))
            mOUL = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpL, .Request("listM1HdpL"), .Request("listM1OUL")))
            m1stHdpL = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpL, .Request("listM1HdpL"), .Request("listM11stHdpL")))
            m1stOUL = IIf(isCopyAll, .Request("listM1HdpD"), IIf(isCopyHdpL, .Request("listM1HdpL"), .Request("listM11stOUL")))

            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                autoHdpD = IIf(isAuto, IIf(isCopyHdpD, .Request("autoS1HdpD"), .Request("autoS1HdpD")), 0)
                autoOUD = IIf(isAuto, IIf(isCopyHdpD, .Request("autoS1HdpD"), .Request("autoS1OUD")), 0)
                auto1stHdpD = IIf(isAuto, IIf(isCopyHdpD, .Request("autoS1HdpD"), .Request("autoS11stHdpD")), 0)
                auto1stOUD = IIf(isAuto, IIf(isCopyHdpD, .Request("autoS1HdpD"), .Request("autoS11stOUD")), 0)
                autoOE = IIf(isAuto, IIf(isCopyHdpD, .Request("autoS1HdpD"), .Request("autoS1OE")), 0)

                auto1x2 = IIf(isAuto, IIf(isCopy1x2, .Request("autoS11x2"), .Request("autoS11x2")), 0)
                autoCS = IIf(isAuto, IIf(isCopy1x2, .Request("autoS11x2"), .Request("autoS1CS")), 0)
                autoTG = IIf(isAuto, IIf(isCopy1x2, .Request("autoS11x2"), .Request("autoS1TG")), 0)
                autoMp = IIf(isAuto, IIf(isCopy1x2, .Request("autoS11x2"), .Request("autoS1MP")), 0)
                autoOR = IIf(isAuto, IIf(isCopy1x2, .Request("autoS11x2"), .Request("autoS1OR")), 0)

                autoHdpL = IIf(isAuto, IIf(isCopyHdpL, .Request("autoS1HdpL"), .Request("autoS1HdpL")), 0)
                autoOUL = IIf(isAuto, IIf(isCopyHdpL, .Request("autoS1HdpL"), .Request("autoS1OUL")), 0)
                auto1stHdpL = IIf(isAuto, IIf(isCopyHdpL, .Request("autoS1HdpL"), .Request("autoS11stHdpL")), 0)
                auto1stOUL = IIf(isAuto, IIf(isCopyHdpL, .Request("autoS1HdpL"), .Request("autoS11stOUL")), 0)

                sHdpD = IIf(isAuto, 0, IIf(isCopyHdpD, .Request("listS1HdpD"), .Request("listS1HdpD")))
                sOUD = IIf(isAuto, 0, IIf(isCopyHdpD, .Request("listS1HdpD"), .Request("listS1OUD")))
                s1stHdpD = IIf(isAuto, 0, IIf(isCopyHdpD, .Request("listS1HdpD"), .Request("listS11stHdpD")))
                s1stOUD = IIf(isAuto, 0, IIf(isCopyHdpD, .Request("listS1HdpD"), .Request("listS11stOUD")))
                sOE = IIf(isAuto, 0, IIf(isCopyHdpD, .Request("listS1HdpD"), .Request("listS1OE")))

                s1x2 = IIf(isAuto, 0, IIf(isCopy1x2, .Request("listS11x2"), .Request("listS11x2")))
                sCS = IIf(isAuto, 0, IIf(isCopy1x2, .Request("listS11x2"), .Request("listS1CS")))
                sTG = IIf(isAuto, 0, IIf(isCopy1x2, .Request("listS11x2"), .Request("listS1TG")))
                sMp = IIf(isAuto, 0, IIf(isCopy1x2, .Request("listS11x2"), .Request("listS1MP")))
                sOR = IIf(isAuto, 0, IIf(isCopy1x2, .Request("listS11x2"), .Request("listS1OR")))

                sHdpL = IIf(isAuto, 0, IIf(isCopyHdpL, .Request("listS1HdpL"), .Request("listS1HdpL")))
                sOUL = IIf(isAuto, 0, IIf(isCopyHdpL, .Request("listS1HdpL"), .Request("listS1OUL")))
                s1stHdpL = IIf(isAuto, 0, IIf(isCopyHdpL, .Request("listS1HdpL"), .Request("listS11stHdpL")))
                s1stOUL = IIf(isAuto, 0, IIf(isCopyHdpL, .Request("listS1HdpL"), .Request("listS11stOUL")))
            Else
                Dim isAutoSoccer As Boolean = False
                If Not .Request("chkAuto1") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdpD = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOUD = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                auto1stHdpD = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                auto1stOUD = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                auto1x2 = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoCS = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoTG = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoMp = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOR = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                autoHdpL = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOUL = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                auto1stHdpL = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                auto1stOUL = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                sHdpD = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOUD = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                s1stHdpD = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                s1stOUD = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOE = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))

                s1x2 = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sCS = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sTG = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sMp = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOR = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))

                sHdpL = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOUL = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                s1stHdpL = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                s1stOUL = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
            End If
            param.Add("@spositiontaking_1", sHdpD) 'smallmoney  
            param.Add("@spositiontaking_3", sOUD) 'smallmoney  
            param.Add("@spositiontaking_7", s1stHdpD) 'smallmoney  
            param.Add("@spositiontaking_8", s1stOUD) 'smallmoney  
            param.Add("@spositiontaking_2", sOE) 'smallmoney  

            param.Add("@spositiontaking_5", s1x2) 'smallmoney 
            param.Add("@spositiontaking_4", sCS) 'smallmoney
            param.Add("@spositiontaking_6", sTG) 'smallmoney            
            param.Add("@spositiontaking_9", sMp) 'smallmoney  
            param.Add("@spositiontaking_10", sOR) 'smallmoney  

            param.Add("@slivepositiontaking_1", sHdpL) 'smallmoney  
            param.Add("@slivepositiontaking_3", sOUL) 'smallmoney  
            param.Add("@slivepositiontaking_7", s1stHdpL) 'smallmoney  
            param.Add("@slivepositiontaking_8", s1stOUL) 'smallmoney  

            param.Add("@mpositiontaking_1", mHdpD) 'smallmoney   
            param.Add("@mpositiontaking_3", mOUD) 'smallmoney   
            param.Add("@mpositiontaking_7", m1stHdpD) 'smallmoney  
            param.Add("@mpositiontaking_8", m1stOUD) 'smallmoney   
            param.Add("@mpositiontaking_2", mOE) 'smallmoney   

            param.Add("@mpositiontaking_5", m1x2) 'smallmoney  
            param.Add("@mpositiontaking_4", mCS) 'smallmoney
            param.Add("@mpositiontaking_6", mTG) 'smallmoney             
            param.Add("@mpositiontaking_9", mMp) 'smallmoney   
            param.Add("@mpositiontaking_10", mOR) 'smallmoney  

            param.Add("@mlivepositiontaking_1", mHdpL) 'smallmoney    
            param.Add("@mlivepositiontaking_3", mOUL) 'smallmoney    
            param.Add("@mlivepositiontaking_7", m1stHdpL) 'smallmoney   
            param.Add("@mlivepositiontaking_8", m1stOUL) 'smallmoney  

            param.Add("@auto_spositiontaking_1", autoHdpD) 'smallmoney    
            param.Add("@auto_spositiontaking_3", autoOUD) 'smallmoney    
            param.Add("@auto_spositiontaking_7", auto1stHdpD) 'smallmoney 
            param.Add("@auto_spositiontaking_8", auto1stOUD) 'smallmoney  
            param.Add("@auto_spositiontaking_2", autoOE) 'smallmoney                  

            param.Add("@auto_spositiontaking_5", auto1x2) 'smallmoney 
            param.Add("@auto_spositiontaking_4", autoCS) 'smallmoney
            param.Add("@auto_spositiontaking_6", autoTG) 'smallmoney             
            param.Add("@auto_spositiontaking_9", autoMp) 'smallmoney  
            param.Add("@auto_spositiontaking_10", autoOR) 'smallmoney 

            param.Add("@auto_slivepositiontaking_1", autoHdpL) 'smallmoney     
            param.Add("@auto_slivepositiontaking_3", autoOUL) 'smallmoney     
            param.Add("@auto_slivepositiontaking_7", auto1stHdpL) 'smallmoney   
            param.Add("@auto_slivepositiontaking_8", auto1stOUL) 'smallmoney 
        End With
        '***** End Inserting for Soccer *****
    End Sub
#End Region

#Region "Insert parameters for Basketball Position Taking"
    Sub AddParaBasketball(ByVal sportId As String, ByVal context As HttpContext)
        With context
            Dim minb, maxb, maxpermatch As Double
            minb = IIf(isCopyAll, .Request("txtMinbet1"), IIf(isCopyBasket, .Request("txtMinbet2"), .Request("txtMinbet" + sportId)))
            maxb = IIf(isCopyAll, .Request("txtMaxbet1"), IIf(isCopyBasket, .Request("txtMaxbet2"), .Request("txtMaxbet" + sportId)))
            maxpermatch = IIf(isCopyAll, .Request("txtMaxPerMatch1"), IIf(isCopyBasket, .Request("txtMaxPerMatch2"), .Request("txtMaxPerMatch" + sportId)))
            param.Add("@minbetv" + sportId, minb) 'money  
            param.Add("@maxbetv" + sportId, maxb) 'money  
            param.Add("@maxpermatchv" + sportId, maxpermatch) 'money  
            '***** Start Inserting for Basketball *****
            Dim mHdp, mOU, mOE, mMP, mML As Double
            Dim sHdp, sOU, sOE, sMP, sML As Double
            Dim autoHdp, autoOU, autoOE, autoMP, autoML As Double

            Dim isAuto As Boolean = False
            If Not .Request("chkAuto" & sportId) Is Nothing Then
                isAuto = True
            End If
            Dim isCopyHdp As Boolean = False
            If Not .Request("chkCopy2Hdp") Is Nothing Then
                isCopyHdp = True
            End If

            If isCopyAll Or isCopyBasket Then
                mHdp = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mOU = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mOE = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mMP = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mML = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
            Else
                mHdp = IIf(isCopyHdp, .Request("listM2Hdp"), .Request("listM2Hdp"))
                mOU = IIf(isCopyHdp, .Request("listM2Hdp"), .Request("listM2OU"))
                mOE = IIf(isCopyHdp, .Request("listM2Hdp"), .Request("listM2OE"))
                mMP = IIf(isCopyHdp, .Request("listM2Hdp"), .Request("listM2MP"))
                mML = IIf(isCopyHdp, .Request("listM2Hdp"), .Request("listM2ML"))
            End If

            If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
                If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked
                    autoHdp = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoOU = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoOE = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoMP = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto, .Request("autoS2Hdp"), 0)

                    sHdp = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sOU = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sOE = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sMP = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sML = IIf(isAuto, 0, .Request("listS2Hdp"))
                Else
                    autoHdp = IIf(isAuto, IIf(isCopyHdp, .Request("autoS2Hdp"), .Request("autoS2Hdp")), 0)
                    autoOU = IIf(isAuto, IIf(isCopyHdp, .Request("autoS2Hdp"), .Request("autoS2OU")), 0)
                    autoOE = IIf(isAuto, IIf(isCopyHdp, .Request("autoS2Hdp"), .Request("autoS2OE")), 0)
                    autoMP = IIf(isAuto, IIf(isCopyHdp, .Request("autoS2Hdp"), .Request("autoS2MP")), 0)
                    autoML = IIf(isAuto, IIf(isCopyHdp, .Request("autoS2Hdp"), .Request("autoS2ML")), 0)

                    sHdp = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS2Hdp"), .Request("listS2Hdp")))
                    sOU = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS2Hdp"), .Request("listS2OU")))
                    sOE = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS2Hdp"), .Request("listS2OE")))
                    sMP = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS2Hdp"), .Request("listS2MP")))
                    sML = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS2Hdp"), .Request("listS2ML")))
                End If
            Else
                Dim isAutoSoccer As Boolean = False
                If Not .Request("chkAuto1") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoMP = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                sHdp = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOU = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOE = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sMP = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sML = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
            End If
            param.Add("@v2spositiontaking_1", sHdp) 'smallmoney  
            param.Add("@v2spositiontaking_3", sOU) 'smallmoney  
            param.Add("@v2spositiontaking_7", sHdp) 'smallmoney 
            param.Add("@v2spositiontaking_8", sOU) 'smallmoney  
            param.Add("@v2spositiontaking_2", sOE) 'smallmoney  
            param.Add("@v2spositiontaking_9", sMP) 'smallmoney 
            param.Add("@v2spositiontaking_20", sML) 'smallmoney          

            param.Add("@v2mpositiontaking_1", mHdp) 'smallmoney  
            param.Add("@v2mpositiontaking_3", mOU) 'smallmoney  
            param.Add("@v2mpositiontaking_7", mHdp) 'smallmoney 
            param.Add("@v2mpositiontaking_8", mOU) 'smallmoney 
            param.Add("@v2mpositiontaking_2", mOE) 'smallmoney
            param.Add("@v2mpositiontaking_9", mMP) 'smallmoney 
            param.Add("@v2mpositiontaking_20", mML) 'smallmoney 

            param.Add("@auto_v2spositiontaking_1", autoHdp) 'smallmoney  
            param.Add("@auto_v2spositiontaking_3", autoOU) 'smallmoney  
            param.Add("@auto_v2spositiontaking_7", autoHdp) 'smallmoney  
            param.Add("@auto_v2spositiontaking_8", autoOU) 'smallmoney 
            param.Add("@auto_v2spositiontaking_2", autoOE) 'smallmoney  
            param.Add("@auto_v2spositiontaking_9", autoMP) 'smallmoney 
            param.Add("@auto_v2spositiontaking_20", autoML) 'smallmoney 
        End With
        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Football, Tennis, Golf, Other Sports Position Taking"
    Sub AddParaFootball(ByVal sportId As String, ByVal context As HttpContext)
        With context
            Dim minb, maxb, maxpermatch As Double
            minb = IIf(isCopyAll, .Request("txtMinbet1"), IIf(isCopyBasket, .Request("txtMinbet2"), .Request("txtMinbet" + sportId)))
            maxb = IIf(isCopyAll, .Request("txtMaxbet1"), IIf(isCopyBasket, .Request("txtMaxbet2"), .Request("txtMaxbet" + sportId)))
            maxpermatch = IIf(isCopyAll, .Request("txtMaxPerMatch1"), IIf(isCopyBasket, .Request("txtMaxPerMatch2"), .Request("txtMaxPerMatch" + sportId)))
            param.Add("@minbetv" + sportId, minb) 'money  
            param.Add("@maxbetv" + sportId, maxb) 'money  
            param.Add("@maxpermatchv" + sportId, maxpermatch) 'money  
            '***** Start Inserting for Football *****
            Dim mHdp, mOU, mOE, mML As Double
            Dim sHdp, sOU, sOE, sML As Double
            Dim autoHdp, autoOU, autoOE, autoML As Double

            Dim isAuto As Boolean = False
            If Not .Request("chkAuto" & sportId) Is Nothing Then
                isAuto = True
            End If
            Dim isCopyHdp As Boolean = False
            If Not .Request("chkCopy" & sportId & "Hdp") Is Nothing Then
                isCopyHdp = True
            End If

            If isCopyAll Or isCopyBasket Then
                mHdp = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mOU = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mOE = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mML = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
            Else
                mHdp = IIf(isCopyHdp, .Request("listM" & sportId & "Hdp"), .Request("listM" & sportId & "Hdp"))
                mOU = IIf(isCopyHdp, .Request("listM" & sportId & "Hdp"), .Request("listM" & sportId & "OU"))
                mOE = IIf(isCopyHdp, .Request("listM" & sportId & "Hdp"), .Request("listM" & sportId & "OE"))
                mML = IIf(isCopyHdp, .Request("listM" & sportId & "Hdp"), .Request("listM" & sportId & "ML"))
            End If

            If Not isCopyAll Then 'this case for Copy All PT from Soccer checkbox is unchecked
                If isCopyBasket Then 'this case for Copy All PT from Basketball checkbox is checked
                    autoHdp = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoOU = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoOE = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto, .Request("autoS2Hdp"), 0)

                    sHdp = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sOU = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sOE = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sML = IIf(isAuto, 0, .Request("listS2Hdp"))
                Else
                    autoHdp = IIf(isAuto, IIf(isCopyHdp, .Request("autoS" & sportId & "Hdp"), .Request("autoS" & sportId & "Hdp")), 0)
                    autoOU = IIf(isAuto, IIf(isCopyHdp, .Request("autoS" & sportId & "Hdp"), .Request("autoS" & sportId & "OU")), 0)
                    autoOE = IIf(isAuto, IIf(isCopyHdp, .Request("autoS" & sportId & "Hdp"), .Request("autoS" & sportId & "OE")), 0)
                    autoML = IIf(isAuto, IIf(isCopyHdp, .Request("autoS" & sportId & "Hdp"), .Request("autoS" & sportId & "ML")), 0)

                    sHdp = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS" & sportId & "Hdp"), .Request("listS" & sportId & "Hdp")))
                    sOU = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS" & sportId & "Hdp"), .Request("listS" & sportId & "OU")))
                    sOE = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS" & sportId & "Hdp"), .Request("listS" & sportId & "OE")))
                    sML = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS" & sportId & "Hdp"), .Request("listS" & sportId & "ML")))
                End If
            Else
                Dim isAutoSoccer As Boolean = False
                If Not .Request("chkAuto1") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOE = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                sHdp = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOU = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOE = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sML = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
            End If
        
            param.Add("@v" & sportId & "spositiontaking_1", sHdp) 'smallmoney  
            param.Add("@v" & sportId & "spositiontaking_3", sOU) 'smallmoney  
            param.Add("@v" & sportId & "spositiontaking_2", sOE) 'smallmoney  
            param.Add("@v" & sportId & "spositiontaking_20", sML) 'smallmoney  

            param.Add("@v" & sportId & "mpositiontaking_1", mHdp) 'smallmoney  
            param.Add("@v" & sportId & "mpositiontaking_3", mOU) 'smallmoney  
            param.Add("@v" & sportId & "mpositiontaking_2", mOE) 'smallmoney
            param.Add("@v" & sportId & "mpositiontaking_20", mML) 'smallmoney 

            param.Add("@auto_v" & sportId & "spositiontaking_1", autoHdp) 'smallmoney  
            param.Add("@auto_v" & sportId & "spositiontaking_2", autoOE) 'smallmoney  
            param.Add("@auto_v" & sportId & "spositiontaking_3", autoOU) 'smallmoney  
            param.Add("@auto_v" & sportId & "spositiontaking_20", autoML) 'smallmoney 
        End With
        '***** End Inserting for Football *****
    End Sub
#End Region

#Region "Insert parameters for Baseball Position Taking"
    Sub AddParaBaseball(ByVal context As HttpContext)
        With context
            Dim minb, maxb, maxpermatch As Double
            minb = IIf(isCopyAll, .Request("txtMinbet1"), IIf(isCopyBasket, .Request("txtMinbet2"), .Request("txtMinbet8")))
            maxb = IIf(isCopyAll, .Request("txtMaxbet1"), IIf(isCopyBasket, .Request("txtMaxbet2"), .Request("txtMaxbet8")))
            maxpermatch = IIf(isCopyAll, .Request("txtMaxPerMatch1"), IIf(isCopyBasket, .Request("txtMaxPerMatch2"), .Request("txtMaxPerMatch8")))
            param.Add("@minbetv8", minb)  'money  
            param.Add("@maxbetv8", maxb) 'money  
            param.Add("@maxpermatchv8", maxpermatch) 'money  
            '***** Start Inserting for Baseball *****
            Dim mHdp, mOU, mML As Double
            Dim sHdp, sOU, sML As Double
            Dim autoHdp, autoOU, autoML As Double

            Dim isAuto As Boolean = False
            If Not .Request("chkAuto8") Is Nothing Then
                isAuto = True
            End If
            Dim isCopyHdp As Boolean = False
            If Not .Request("chkCopy8Hdp") Is Nothing Then
                isCopyHdp = True
            End If
            'this case for Copy All from Soccer or Basketball checkbox is checked
            If isCopyAll Or isCopyBasket Then
                mHdp = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mOU = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mML = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
            Else
                mHdp = IIf(isCopyHdp, .Request("listM8Hdp"), .Request("listM8Hdp"))
                mOU = IIf(isCopyHdp, .Request("listM8Hdp"), .Request("listM8OU"))
                mML = IIf(isCopyHdp, .Request("listM8Hdp"), .Request("listM8ML"))
            End If

            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then
                    autoHdp = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoOU = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto, .Request("autoS2Hdp"), 0)

                    sHdp = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sOU = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sML = IIf(isAuto, 0, .Request("listS2Hdp"))
                Else
                    autoHdp = IIf(isAuto, IIf(isCopyHdp, .Request("autoS8Hdp"), .Request("autoS8Hdp")), 0)
                    autoOU = IIf(isAuto, IIf(isCopyHdp, .Request("autoS8Hdp"), .Request("autoS8OU")), 0)
                    autoML = IIf(isAuto, IIf(isCopyHdp, .Request("autoS8Hdp"), .Request("autoS8ML")), 0)

                    sHdp = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS8Hdp"), .Request("listS8Hdp")))
                    sOU = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS8Hdp"), .Request("listS8OU")))
                    sML = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS8Hdp"), .Request("listS8ML")))
                End If
            Else
                Dim isAutoSoccer As Boolean = False
                If Not .Request("chkAuto1") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoOU = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                sHdp = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sOU = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sML = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
            End If
        
            param.Add("@v8spositiontaking_1", sHdp) 'smallmoney  
            param.Add("@v8spositiontaking_3", sOU) 'smallmoney 
            param.Add("@v8spositiontaking_20", sML) 'smallmoney  

            param.Add("@v8mpositiontaking_1", mHdp) 'smallmoney  
            param.Add("@v8mpositiontaking_3", mOU) 'smallmoney 
            param.Add("@v8mpositiontaking_20", mML) 'smallmoney 

            param.Add("@auto_v8spositiontaking_1", autoHdp) 'smallmoney 
            param.Add("@auto_v8spositiontaking_3", autoOU) 'smallmoney  
            param.Add("@auto_v8spositiontaking_20", autoML) 'smallmoney 
        End With
        '***** End Inserting for Baseball *****
    End Sub
#End Region

#Region "Insert parameters for Motorsports Position Taking"
    Sub AddParaMotosports(ByVal context As HttpContext)
        With context
            Dim minb, maxb, maxpermatch As Double
            minb = IIf(isCopyAll, .Request("txtMinbet1"), IIf(isCopyBasket, .Request("txtMinbet2"), .Request("txtMinbet11")))
            maxb = IIf(isCopyAll, .Request("txtMaxbet1"), IIf(isCopyBasket, .Request("txtMaxbet2"), .Request("txtMaxbet11")))
            maxpermatch = IIf(isCopyAll, .Request("txtMaxPerMatch1"), IIf(isCopyBasket, .Request("txtMaxPerMatch2"), .Request("txtMaxPerMatch11")))
            param.Add("@minbetv11", minb)  'money  
            param.Add("@maxbetv11", maxb) 'money  
            param.Add("@maxpermatchv11", maxpermatch) 'money  
            '***** Start Inserting for MotoSports *****
            Dim mHdp, mML As Double
            Dim sHdp, sML As Double
            Dim autoHdp, autoML As Double

            Dim isAuto As Boolean = False
            If Not .Request("chkAuto11") Is Nothing Then
                isAuto = True
            End If
            Dim isCopyHdp As Boolean = False
            If Not .Request("chkCopy11Hdp") Is Nothing Then
                isCopyHdp = True
            End If
            'this case for Copy All from Soccer or Basketball checkbox is checked
            If isCopyAll Or isCopyBasket Then
                mHdp = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mML = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
            Else
                mHdp = IIf(isCopyHdp, .Request("listM11Hdp"), .Request("listM11Hdp"))
                mML = IIf(isCopyHdp, .Request("listM11Hdp"), .Request("listM11ML"))
            End If

            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then
                    autoHdp = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoML = IIf(isAuto, .Request("autoS2Hdp"), 0)

                    sHdp = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sML = IIf(isAuto, 0, .Request("listS2Hdp"))
                Else
                    autoHdp = IIf(isAuto, IIf(isCopyHdp, .Request("autoS11Hdp"), .Request("autoS11Hdp")), 0)
                    autoML = IIf(isAuto, IIf(isCopyHdp, .Request("autoS11Hdp"), .Request("autoS11ML")), 0)

                    sHdp = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS11Hdp"), .Request("listS11Hdp")))
                    sML = IIf(isAuto, 0, IIf(isCopyHdp, .Request("listS11Hdp"), .Request("listS11ML")))
                End If
            Else
                Dim isAutoSoccer As Boolean = False
                If Not .Request("chkAuto1") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoHdp = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoML = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                sHdp = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sML = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
            End If
            param.Add("@v11spositiontaking_1", sHdp) 'smallmoney         
            param.Add("@v11spositiontaking_20", sML) 'smallmoney  

            param.Add("@v11mpositiontaking_1", mHdp) 'smallmoney         
            param.Add("@v11mpositiontaking_20", mML) 'smallmoney 

            param.Add("@auto_v11spositiontaking_1", autoHdp) 'smallmoney          
            param.Add("@auto_v11spositiontaking_20", autoML) 'smallmoney       
        End With
        '***** End Inserting for MotoSports *****
    End Sub
#End Region

#Region "Insert parameters for Financial Position Taking"
    Sub AddParaFinancial(ByVal context As HttpContext)
        With context
            Dim minb, maxb, maxpermatch As Double
            minb = IIf(isCopyAll, .Request("txtMinbet1"), IIf(isCopyBasket, .Request("txtMinbet2"), .Request("txtMinbet12")))
            maxb = IIf(isCopyAll, .Request("txtMaxbet1"), IIf(isCopyBasket, .Request("txtMaxbet2"), .Request("txtMaxbet12")))
            maxpermatch = IIf(isCopyAll, .Request("txtMaxPerMatch1"), IIf(isCopyBasket, .Request("txtMaxPerMatch2"), .Request("txtMaxPerMatch12")))
            param.Add("@minbetv12", minb)  'money  
            param.Add("@maxbetv12", maxb) 'money  
            param.Add("@maxpermatchv12", maxpermatch) 'money  
            '***** Start Inserting for Baseball *****
            Dim mOE, mFO, mFLO As Double
            Dim sOE, sFO, sFLO As Double
            Dim autoOE, autoFO, autoFLO As Double

            Dim isAuto As Boolean = False
            If Not .Request("chkAuto12") Is Nothing Then
                isAuto = True
            End If
            Dim isCopyOE As Boolean = False
            If Not .Request("chkCopy12Hdp") Is Nothing Then
                isCopyOE = True
            End If
            'this case for Copy All from Soccer or Basketball checkbox is checked
            If isCopyAll Or isCopyBasket Then
                mOE = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mFO = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
                mFLO = IIf(isCopyAll, .Request("listM1HdpD"), .Request("listM2Hdp"))
            Else
                mOE = IIf(isCopyOE, .Request("listM12OE"), .Request("listM12OE"))
                mFO = IIf(isCopyOE, .Request("listM12OE"), .Request("listM12FO"))
                mFLO = IIf(isCopyOE, .Request("listM12OE"), .Request("listM12FLO"))
            End If

            If Not isCopyAll Then 'this case for Copy All PT checkbox is checked
                If isCopyBasket Then
                    autoOE = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoFO = IIf(isAuto, .Request("autoS2Hdp"), 0)
                    autoFLO = IIf(isAuto, .Request("autoS2Hdp"), 0)

                    sOE = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sFO = IIf(isAuto, 0, .Request("listS2Hdp"))
                    sFLO = IIf(isAuto, 0, .Request("listS2Hdp"))
                Else
                    autoOE = IIf(isAuto, IIf(isCopyOE, .Request("autoS12OE"), .Request("autoS12OE")), 0)
                    autoFO = IIf(isAuto, IIf(isCopyOE, .Request("autoS12OE"), .Request("autoS12FO")), 0)
                    autoFLO = IIf(isAuto, IIf(isCopyOE, .Request("autoS12OE"), .Request("autoS12FLO")), 0)

                    sOE = IIf(isAuto, 0, IIf(isCopyOE, .Request("listS12OE"), .Request("listS12OE")))
                    sFO = IIf(isAuto, 0, IIf(isCopyOE, .Request("listS12OE"), .Request("listS12FO")))
                    sFLO = IIf(isAuto, 0, IIf(isCopyOE, .Request("listS12OE"), .Request("listS12FLO")))
                End If
            Else
                Dim isAutoSoccer As Boolean = False
                If Not .Request("chkAuto1") Is Nothing Then
                    isAutoSoccer = True
                End If
                autoOE = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoFO = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)
                autoFLO = IIf(isAutoSoccer, .Request("autoS1HdpD"), 0)

                sOE = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sFO = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
                sFLO = IIf(isAutoSoccer, 0, .Request("listS1HdpD"))
            End If
        
            param.Add("@v12spositiontaking_2", sOE) 'smallmoney  
            param.Add("@v12spositiontaking_21", sFO) 'smallmoney 
            param.Add("@v12spositiontaking_22", sFLO) 'smallmoney  

            param.Add("@v12mpositiontaking_2", mOE) 'smallmoney  
            param.Add("@v12mpositiontaking_21", mFO) 'smallmoney 
            param.Add("@v12mpositiontaking_22", mFLO) 'smallmoney 

            param.Add("@auto_v12spositiontaking_2", autoOE) 'smallmoney 
            param.Add("@auto_v12spositiontaking_21", autoFO) 'smallmoney  
            param.Add("@auto_v12spositiontaking_22", autoFLO) 'smallmoney 
        End With
        '***** End Inserting for Baseball *****
    End Sub
#End Region
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
