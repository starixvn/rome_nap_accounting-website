Imports System.Data
Public Class AgentSiteShare
    Inherits System.Web.UI.Page
    Public Shared Function DtRefined(ByRef dt As DataTable) As DataTable
        Dim i, j As Integer
        If Not (dt Is Nothing) Then
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    For j = 0 To dt.Columns.Count - 1
                        If IsDBNull(dt.Rows(i).Item(j)) Then
                            If dt.Columns(j).DataType.ToString = "System.Double" Then
                                dt.Rows(i).Item(j) = 0.0
                            ElseIf dt.Columns(j).DataType.ToString = "System.Integer" Then
                                dt.Rows(i).Item(j) = 0
                            ElseIf dt.Columns(j).DataType.ToString = "System.Decimal" Then
                                dt.Rows(i).Item(j) = 0
                            ElseIf dt.Columns(j).DataType.ToString = "System.Boolean" Then
                                dt.Rows(i).Item(j) = False
                            ElseIf dt.Columns(j).DataType.ToString = "System.String" Then
                                dt.Rows(i).Item(j) = "-"
                            ElseIf dt.Columns(j).DataType.ToString = "System.DateTime" Then
                                dt.Rows(i).Item(j) = "01/01/1900 00:00:00"
                            End If
                        End If
                    Next
                Next
                Return dt
            Else
                dt = Nothing
                Return dt
            End If
        Else
            Return dt
        End If
    End Function
End Class
