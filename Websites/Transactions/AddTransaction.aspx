<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddTransaction.aspx.cs" Inherits="AddTransaction"
    Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><link href="~/App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/input.css" rel="stylesheet" type="text/css" /><link
        href="~/App_Themes/Public/common.css" rel="stylesheet" type="text/css" /><link href="~/App_Themes/Public/Message.css"
            rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <asp:Literal ID="AddTxnHeaderStr" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box ">
                <div id="errMsg">
                </div>
                <div id="clear2">
                </div>
                <form style="margin: 0px; padding: 0px;" name="fAddTxn" method="post" action="../Handlers/AddTransaction.ashx?CustName=<%= Request.QueryString["CustName"] %>&CustID=<%= Request.QueryString["CustID"] %>&IsInternal=<%= Request.QueryString["IsInternal"]%>"
                target="frameUpdate">
                <asp:Literal ID="AddTxnContentHtml" runat="server"></asp:Literal>
                </form>
                <iframe name="frameUpdate" id="frameUpdate" frameborder="0" height="0" width="0">
                </iframe>
            </div>
        </div>
    </div>

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/common.js" type="text/javascript"></script>

    <script type="text/javascript">
        function ValueAddValid() {
            if (isEmpty("txtDate")) {

                showAlertMessage("errMsg", "Please select date.", 1);
                obj("txtDate").focus();
                return false;
            }
            if (isEmpty("txtamount") || isString("txtamount")) {

                showAlertMessage("errMsg", "Invalid Amount.", 1);
                obj("txtamount").focus();
                return false;
            }
            return true;
        }
         
    </script>

</body>
</html>
