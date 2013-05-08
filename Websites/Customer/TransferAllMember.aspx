<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferAllMember.aspx.cs"
    Inherits="Customer_TransferAllMember" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
     <link href="../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/common.js?vers=<%=AccCommon.Version() %>"></script>
    <script src="../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
    <script type="text/javascript">
        function KeyPress(e) {
            e = (e) ? e : window.event;
            var key = (e.keyCode) ? e.keyCode : e.which;
            if (key == 13 && flag == 0) {
                CheckInputValue();
                return false;
            }
            return;
        }
        function OnOffButton() {
            if (!isEmpty("txtUname"))
                EnableSubmit();
            else
                DisableSubmit();

        }
        function CheckInputValue() {
            if (isEmpty("txtUname")) {
                
                showAlertMessage("errMsg", "Please enter Agent User Name.", 1);
                $("#txtUname").focus();
                return;
            }
            $("#errMsg").html("");
            var agentname = $("#txtUname").val();
            var type = $("input[name=chkTransferAll]:checked").val();
            if (!confirm("Are you sure you want to transfer balance for all " + type + " under user " + agentname.trim() + "?")) {
                return;
            }

            try{parent.BlockUI();}catch(e){}
            $.ajax({
                type: "POST",
                url: "../Handlers/UpdateTransferAllMember.ashx",
                async: true,
                data: 'username=' + $('#txtUname').val() + "&type=" + type,
                success: AfterUpdate
            });
        }

        function AfterUpdate(rs) {
            var aRs = rs.split('|');
            if (aRs[0] == '1') {//success
                $('#_sContent').html(aRs[1]);
            }
            else {
                $("#errMsg").html(aRs[1]);
            }
            try{parent.jQuery.unblockUI();}catch(e){}
        }
        function DisableSubmit() {
            $("#btnSubmit").attr("disabled",true);
        }
        function EnableSubmit() {
            $("#btnSubmit").attr("disabled", false);
        }
    </script>

</head>
<body style="margin: 0px;">
    <div class="main">
        <div class="maincontent">
             <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC"><asp:Literal ID="sTitle" runat="server"></asp:Literal></td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg"></div>
                <div id="clear2"></div>
                <div id="_sContent">
                    <asp:Literal ID="sContent" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    <script>
        Custom.init();
    </script>
</body>
</html>
