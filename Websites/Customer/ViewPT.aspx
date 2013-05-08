<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPT.aspx.cs" Inherits="Customer_ViewPT" Title="Untitled Page" Theme="" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
 <title>View PT</title>
    <link href="../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/PT.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ChangeType(rdo) {
            if (rdo.id == 'rdoPT' && $('#tblPT').css('display','')) return;
            if (rdo.id == 'rdoComm' && $('#tblPT').css('display', 'none')) return;
            $('#tblPT').css('display',$('#rdoPT').css('display')='none' ? '' : 'none');
            $('#sContent').html('');
        }

        function ChangeSport() {
            var sid = $("#ddlSporttype :selected").val();
            $.ajax({
                type: "POST",
                url: "../Handlers/ViewPT.ashx",
                async: true,
                data: 'type=changesport&sid=' + sid,
                success: UpdateBettype
            });
        }

        function UpdateBettype(rs) {
           
            $('#tdBettype').html(rs);

            //For Casino
            var sid = $("ddlSporttype :selected").val();
            if (sid == 71)
                $('#showChkLive').css('display','none');
            else
                $('#showChkLive').css('display','block');
        }

        function KP(e) {
            e = (e) ? e : window.event;
            var key = (e.keyCode) ? e.keyCode : e.which;
            if (key == 13) DoSubmit();
            return;
        }

        function DoSubmit() {
           $("#err").html("");
          
            var username =$.trim($('#txtUsername').val());
           
            if (username=="") {
                showAlertMessage("err","Please input username!",1);
                return;
            }
            var params = 'username=' + username;
            if ($('#rdoPT').attr('checked')) {
                params += '&type=pt';
                params += '&sid=' + $("#ddlSporttype :selected").val();
                params += '&bid=' + $("#ddlBettype :selected").val();
                params += '&isLive=' + (($('#chkLive').attr("checked")) ? '1' : '0');
            }
            else {
                params += '&type=comm&sid=1&bid=1&isLive=0';
            }
            try{parent.BlockUI();}catch(e){}
            $.ajax({
                type: "POST",
                url: "../Handlers/ViewPT.ashx",
                async: true,
                data: params,
                success: BindData
            });
        }

        function BindData(rs) {
            $('#sContent').html(rs);
            $("#divcontainer").removeClass("allhidden");
            try{parent.jQuery.unblockUI();}catch(e){}
        }

        window.onload = function() {
            $('#rdoPT').attr('checked', true);
            if (gup('username') != "") {
                $('#txtUsername').val(gup('username'));
                DoSubmit();
            }
            Custom.init();
        }
        function gup(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.href);
            if (results == null)
                return "";
            else
                return results[1];
        }
    </script>
</head>
<body>
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
                <div id="err"></div>
                <asp:Literal ID="sContent" runat="server"></asp:Literal>
            </div>
            <div class="bg_table allhidden" id="divcontainer">
               <div id="sContent"></div>
            </div>
        </div>
    </div>
</body>
</html>


