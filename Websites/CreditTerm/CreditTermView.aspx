<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditTermView.aspx.cs" Inherits="CreditTerm_CreditTermView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <script src="../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
    <script src="../js/CreditTerm.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
   
</head>
<body onload="Custom.init();">
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                        Credit term
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box ">
                <div id="errMsg" class="errMsg">
                    <%=msg%></div>
                <%if (string.IsNullOrEmpty(msg)) {%>
                <div class="pad_tb_4">
                    <a href="CreditTermGroupView.aspx">[ View all group ]</a></div>
                    <%} %>
            </div>
            <div class="bg_table">
                <asp:Literal ID="content" runat="server"></asp:Literal>
                <iframe id="frmterm" frameborder="0" height="0" name="frmterm" width="0" style="padding:0px; margin:0px;"></iframe>
            </div>
        </div>
    </div>
</body>
</html>
