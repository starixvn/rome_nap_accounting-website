<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCustomerInfo.aspx.cs"  Inherits="Customer_EditCustomerInfo" Title="Update Customer Info" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
</head>
<body style="margin:5px">
    <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    Edit <%=username%> Info
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg" class="error-message"></div>
                <form action="../Handlers/UpdateCustInfo.ashx" method="post" target="frUpdate" style="margin:0px; padding:0px;">
                    <asp:Literal ID="EditCustInfoContentHtml" runat="server"></asp:Literal>
                </form>
                <iframe name="frUpdate" id="frUpdate" height="0" width="0" frameborder="0" style="display:none;"></iframe>
            </div>
        </div>
    </div>
</body>
</html>
