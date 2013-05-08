<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeUserGroup.aspx.cs"
    Inherits="Customer_ChangeUserGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Clear Credit</title>
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>

    <script type="text/javascript">
        $(function () {
            $("form input").keypress(function (e) {
                if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
                    CheckInputValue();                    
                    return false;       
                } else {
                    return true;
                }
            });
        });  

        function CheckInputValue() {
            if (isEmpty("txtUname")) {
                showAlertMessage("errMsg", "Please enter UserName.", 1);
                obj("txtUname").focus();
                return false;
            }

            var str = "";
            if (obj("clearCr").value == "1") {
                str = "Are you sure you want to clear credit for this User?"
            }
            else {
                str = "Are you sure you want to change User Group for this User?";
            }
            var strC = confirm(str);

            if (strC) {
                document.fChangeGroup.submit();
                obj("errMsg").innerHTML = "";
                return true;
            }
            else {
                return false;
            }
        }
    </script>

</head>
<body>
    <div class="main">
        
        <div class="maincontent"style="width: 250px;">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    <asp:Literal ID="ChangeUGroupHeaderStr" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg"></div>
                <form name="fChangeGroup" method="post" action="../Handlers/UpdateChangeGroup.ashx" target="frUpdate">
                    <input type="hidden" name="clearCr" id="clearCr" value="<%=strClear %>" />
                    <asp:Literal ID="ChangeUGroupContentHtml" runat="server"></asp:Literal>
                </form>
                <iframe class="hidden" name="frUpdate"></iframe>
            </div>
        </div>
        
        <%=script %>
    </div>
</body>
</html>
