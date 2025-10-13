<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewEVolleyballResult.aspx.cs" Inherits="Report_ViewResult" Theme=""  Async="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>eVolleyball</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" type ="text/css" href ="../App_Themes/BetList/Betlist.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>
    <link rel="stylesheet" type="text/css" href="../Content/Fanex.Result/fanex.result.css?v=1.1"/>
    <link rel="stylesheet" type="text/css" href="../Content/Fanex.Result/fanex.result-customized.css"/>
</head>
<body>
    <div style="margin-bottom:5px;"></div>
    <input id="resultTitle" style="display: none;" runat="server"/>
    <asp:Literal ID="lResult" runat="server"></asp:Literal>
</body>
    <script type="text/javascript">
        function updateDialogTitle() {
            window.parent.$("#ui-dialog-title-dialog-modalEVolleyball").text(document.getElementById('resultTitle').value);
        };
        window.onload = updateDialogTitle;
    </script>
</html>