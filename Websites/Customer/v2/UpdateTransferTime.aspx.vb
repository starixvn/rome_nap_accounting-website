Imports System.Collections
Imports IBCUtils.DAL
Imports DNAide.Data
Partial Class Customer_V2_UpdateTransferTime
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            UpdateData()
        Catch ex As Exception

            Response.Write("<script type=""text/javascript"">parent.alert_error('" & AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) & "','errTransfer');</script>")

        End Try
    End Sub
    Function UpdateData()
        Try
            Dim param As New Hashtable
            param.Add("custid", Request("custId")) 'int 
            param.Add("userid", Session("AccId")) 'int 
            Dim strUpdTranfer As String = ""
            If Request("tranfer") = "daily" Then
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
            param.Add("subaccid", 0)
            param.Add("language", "E")

            param.Add("errcode", 0)
            param.Add("errmsg", "")
            Dim rs = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.UpdTransferOption(param)
            If rs.ErrorCode <> 0 Then
                Response.Write("<script type=""text/javascript"">parent.alert_error('" + AlertMessage.GetMsg(rs.ErrorMessage, AlertMessage.MsgType.Error) + "','errTransfer'); </script>")
            Else
                If Not Request("set") Is Nothing And Request("set") = "1" Then
                    Response.Write("<script type=""text/javascript"">parent.alert_error('" + AlertMessage.GetMsg("Update transfer condition successfully.", AlertMessage.MsgType.Success) + "','errTransfer'); </script>")
                Else
                    Response.Write("<script type=""text/javascript"">parent.parent.ibcWnd.close(); </script>")
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function ReplaceData(ByVal str As String) As String
        Return str.Replace("""", "&quot;").Replace("'", "&#39;").Replace(vbNewLine, "")
    End Function
End Class
