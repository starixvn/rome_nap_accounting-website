<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditUserName.aspx.cs" Inherits="Customer_EditUserName" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../js/common.js"></script>

    <script type="text/javascript">
        function CheckInputValue() {
            if (isEmpty("txtOldUname")) {
                showAlertMessage("errMsg", "Please enter Old User Name.", 1);
                obj("txtOldUname").focus();
                return false;
            }
            if (isEmpty("txtNewUname")) {
                showAlertMessage("errMsg", "Please enter New User Name.", 1);
                obj("txtNewUname").focus();
                return false;
            }
            if (obj("txtOldUname").value == obj("txtNewUname").value) {
                showAlertMessage("errMsg", "New UserName and Old User Name are the same.", 1);
                obj("txtNewUname").focus();
                return false;
            }
            if (obj("txtNewUname").value.length > 30) {
                showAlertMessage("errMsg", "New UserName is too long (<=30 characters).", 1);
                obj("txtNewUname").focus();
                return false;
            }
            var strC = confirm("Are you sure you want to change this User Name?");
            if (strC) {
                obj("errMsg").innerHTML = "";
                document.getElementById('fEditUsername').submit();
                try{parent.BlockUI();}catch(e){}
                return true;
            }
            else {
                return false;
            }
        }
    </script>

</head>
<body style="margin: 0px;">
    <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    <asp:Literal ID="EditUserNameHeaderStr" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
            <div id="errMsg" class="error-message"></div>
            <div id="clear2"></div>
            <iframe class="hidden" name="frUpdate"></iframe>
                    <form name="fEditUsername" id="fEditUsername" method="post" action="../Handlers/EditUsername.ashx" target="frUpdate">
                    <asp:Literal ID="EditUserNameContentHtml" runat="server"></asp:Literal>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
