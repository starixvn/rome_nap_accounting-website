Imports Sunplus.Accounting.Common

Partial Class Customer_V2_ConfirmEditAgent
    Inherits RequiredAuthorizationPage
    Public strMsg As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        strMsg = Resources.Language.lblConfirmEditAgent

    End Sub
End Class
