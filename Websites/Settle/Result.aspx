<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Result.aspx.cs" Inherits="Settle_Result" Title="Result"%>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Settle/Settle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/prototype.js"></script>    
    <script type="text/javascript" src="../js/common.js?vers=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="../js/Result.js?vers=<%=AccCommon.Version() %>"></script>
    <link href="../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        var rootpath = '../';
    </script>
  
    <script src="../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
             <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC"> <asp:Literal ID="sTitle" runat="server"></asp:Literal></td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="searchbox">
                    <asp:Literal ID="sSearch" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="bg_table">
                  <div id="_sContent"><asp:Literal ID="sContent" runat="server"></asp:Literal></div>          
            </div>
        </div>
    </div>
</body>
</html>