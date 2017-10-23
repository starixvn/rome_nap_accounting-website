<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditDirectCustomerInfo.aspx.cs"  Inherits="Customer_EditDirectCustomerInfo" Title="Edit Information" %>
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
    <style type="text/css">
        input[type=text] {
            width:200px;
        }

    </style>
    <script type="text/javascript">
        function CheckInputValue() {
            if (isString("txtMaxLoss")) {
                showAlertMessage("errMsg", "Invalid Max Loss.", 1);
                $("txtMaxLoss").focus();
                return false;
            } else if (isNumeric("txtMaxLoss") > 0) {
                showAlertMessage("errMsg", "Max Loss should be negative!", 1);
                $("txtMaxLoss").focus();
                return false;
            }

            if ($("#txtGroupHeadlineMaxLoss").length) {
                if (isString("txtGroupHeadlineMaxLoss")) {
                    showAlertMessage("errMsg", "Invalid Group Max Loss.", 1);
                    $("txtGroupHeadlineMaxLoss").focus();
                    return false;
                } else if (isNumeric("txtGroupHeadlineMaxLoss") > 0) {
                    showAlertMessage("errMsg", "Group Max Loss should be negative!", 1);
                    $("txtGroupHeadlineMaxLoss").focus();
                    return false;
                }
            }
            return true;
        }
        function On_KeyPress(e) {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which;
            if (code == 13) {
                return CheckInputValue();
            }
        }
    </script>
</head>
<body style="margin:5px">
    <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                     <%=CustDirectInfoTitle%>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg" class="error-message"></div>
                <form action="../Handlers/UpdateDirectCustInfo.ashx" method="post" target="frUpdate" style="margin:0px; padding:0px;">
                    <asp:Literal ID="EditCustInfoContentHtml" runat="server"></asp:Literal>
                </form>
                <iframe name="frUpdate" id="frUpdate" height="0" width="0" frameborder="0" style="display:none;"></iframe>
            </div>
        </div>
    </div>
</body>
</html>
