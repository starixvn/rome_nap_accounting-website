<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditOperator.aspx.cs" Inherits="NewCustomer_EditOperator" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" src="../js/common.js"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    </head>
<body style="margin:5px 10px 5px 5px">
    <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
            </div>
            <div class="bg_table">
                 <iframe name="frUpdate" height="0" width="0" frameborder="0"></iframe>
                <form action="../Handlers/UpdateOperator.ashx" method="post" target="frUpdate">
                    <asp:Literal ID="EditOperatorContentHtml" runat="server"></asp:Literal>    
                </form>
            </div>
        </div>
    </div>
</body>
</html>

