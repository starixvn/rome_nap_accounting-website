<%@ Page Language="C#" EnableViewState="false" AutoEventWireup="true" CodeFile="LogAction.aspx.cs" Inherits="LogDisplay_LogAction" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/Table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/Tab.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/ViewLog.css" rel="stylesheet" type="text/css" />
    <link href="../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/LogAction.js"></script>
    <script type="text/javascript">        var rootpath = '../';</script>
    <script src="../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="lContent" runat="server"></asp:Literal>
    </form>
    <script type="text/javascript">
        $(document).ready(function() {
            GetLogActionContent();
        });
    </script>
</body>
</html>
