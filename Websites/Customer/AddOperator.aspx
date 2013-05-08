<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddOperator.aspx.cs" Inherits="NewCustomer_AddOperator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">
        function CheckInputValid() {
            if (isEmpty("txtUsername")) {
                showAlertMessage( "errMsg", "Invalid Username.",1);
                obj("txtUsername").focus();
                return false;
            }
            if (isEmpty("txtPwd")) {
                showAlertMessage("errMsg", "Please input password.", 1);
                obj("txtPwd").focus();
                return false;
            }
            else {
                var str = obj("txtPwd").value;
                numbers = new String("0123456789");
                var strError = "Password must have at least 6 characters,including at least 2 numbers and at least 2 letters. (e.g:pass22)";
                if (str.length < 6) {
                    showAlertMessage("errMsg", strError, 1);
                    return false;
                }
                var i = 0;
                var nCount = 0;
                while (i < str.length) {
                    if (numbers.indexOf(str.substring(i, i + 1)) >= 0) {
                        nCount++;
                    }
                    i++;
                }
                if (nCount == str.length || nCount < 2) {
                    showAlertMessage("errMsg", strError, 1);
                    return false;
                }
            }
            if (obj("listRole").value == "Select") {
                showAlertMessage("errMsg", "Please choose a Role.", 1);
                obj("listRole").focus();
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    Add New Operator
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg"></div>
                <div id="clear2"></div>
            </div>
            <div class="bg_table">
             <iframe name="frAdd" height="0" width="0" frameborder="0"></iframe>
            <form method="post" action="../Handlers/InsertOperator.ashx" target="frAdd">
                <asp:Literal ID="AddNewOperatorContentHtml" runat="server"></asp:Literal>
            </form>
            </div>
        </div>
    </div>
</body>
</html>

