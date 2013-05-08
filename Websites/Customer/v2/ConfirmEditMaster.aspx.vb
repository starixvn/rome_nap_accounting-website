Imports Sunplus.Accounting.Common

Partial Class Customer_V2_ConfirmEditMaster
    Inherits RequiredAuthorizationPage
    Public strMsg As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        strMsg = String.Format(Resources.Language.lblConfirmEditMaster, Resources.Language.Master)
    End Sub
End Class
