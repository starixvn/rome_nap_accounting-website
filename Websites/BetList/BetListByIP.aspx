<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetListByIP.aspx.cs" Inherits="BetListByIP"
    Title="Bet List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link href="../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/BetList/BetList.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <%--<link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />--%>

    <script src="../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>

    <script src="../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>

    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" />

    <script src="../js/BetList.js?ver=2" type="text/javascript"></script>

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/pager_v2.js" type="text/javascript"></script>

    <script src="../js/common.js" type="text/javascript"></script>

    <script src="../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js"></script>

    <script type="text/javascript" src="../js/dialogbox.js"></script>

    <script type="text/javascript" src="../js/BlockUI.js"></script>

    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>

</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Bet list
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg">
                </div>
                <div id="clear2">
                </div>
                <asp:Literal ID="reportHeader" runat="server">
                </asp:Literal>
            </div>
            <div id="divcontainer" class="bg_table allhidden">
                <div id="sContent">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
