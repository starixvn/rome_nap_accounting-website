<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewGoldDeluxeResult.aspx.cs" Inherits="Report_ViewResult" Theme=""  Async="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title>Sportbook Result</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" type ="text/css" href ="../App_Themes/BetList/Betlist.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>
    <link rel="stylesheet" type="text/css" href="../Content/Fanex.Result/fanex.result.css"/>
    <link rel="stylesheet" type="text/css" href="../Content/Fanex.Result/fanex.result-customized.css"/>

    <script type="text/javascript">
        function obj(id)
        {
            return document.getElementById(id);
        }
        window.onload = function() {
        ibcWnd = parent.ibcWnd;
        if (typeof ibcWnd == 'undefined') {
            return;
        }
        
        if (ibcWnd.loaded) {
            var height = 250;
            var width = 750;
            
            if (obj('goalcount') && obj('goalcount').value != "") {
                height = height + parseInt(obj('goalcount').value) * 15;
                width = 750;
            }
            ibcWnd.setRect(ibcWnd.left, ibcWnd.top, width, height);
        
        }
    }
    </script>
    
</head>
<body>
    <div style="margin-bottom:5px;"></div>
    <asp:Literal ID="lResult" runat="server"></asp:Literal>  
</body>
</html>
