<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditMaxExposure.aspx.cs"
    Inherits="Customer_EditMaxExposure" Title="Untitled Page" %>

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
    <script type="text/javascript">
        function CheckInputValue() {
            if (isEmpty("txtValue") || isString("txtValue")) {
                //alert_error("Invalid Max Exposure.", "errMsg");
                showAlertMessage("errMsg", "Invalid Max Exposure.", 1);
                obj("txtValue").focus();
                return false;
            }
            if (isNumeric("txtValue") < 0 || isNumeric("txtValue") > 100) {
                //alert_error("Max exposure must be between 0% and 100%", "errMsg");
                showAlertMessage("errMsg", "Max exposure must be between 0% and 100%", 1);
                obj("txtValue").focus();
                return false;
            }
            obj("matchLimit").value = obj("txtValue").value / 100;
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
<body onload="selectTranferTime();">
    <div class="main">
        <div class="maincontent" style="width:250px">
            <div class="maincontent_box">
                <div><span id="errMsg" class="error-message" style="margin-bottom:2px;"></span></div>  
                <div id="clear2">
                </div>
                 <div>
                    <form action="../Handlers/UpdateMaxExp.ashx" target="frUpdate" method="post">
                    <asp:Literal ID="EditMaxExContentHtml" runat="server"></asp:Literal>
                    </form>
                    <iframe name="frUpdate" height="0" width="0" frameborder="0" style="display:none;"></iframe>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
