<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeSite.aspx.cs" Inherits="Customer_ChangeSite" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../js/common.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(
            function () {
                var username = gup('UserName');
                if (username != null && username != "") {
                    var divUserName = $('#divUsername');
                    var roleId = gup('RoleId');
                    if (roleId == 4)
                        divUserName.html('Super ');
                    else if (roleId == 3)
                        divUserName.html('Master ');
                    else if (roleId == 2)
                        divUserName.html('Agent ');
                    divUserName.append(username);
                   
                    
                }
                var custId = gup('CustId');
                var currSite = gup('Site');
                if (currSite != null && currSite != "")
                    $("#spCurrSite").html(currSite);
                SiteRenderingCreditAndDeposit('Site', "../");
            }

        );
            

           
    </script>
</head>
<body>
    <div class="maincontent">
        <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
            <tbody>
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                        Move Site
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="maincontent_box" style="width:350px">
            <div style="display: none;" id="trMsg">
                <div id="dvMsg">
                </div>
            </div>
            <table cellspacing="1" cellpadding="0" style="width: 350px; line-height: 18px ! important;"
                class="tbllayout">
                <tbody>
                    <tr>
                        <td class="l b">
                            Move site for:
                        </td>
                        <td class="l b">
                            <div id="divUsername"><input type="text" name="txtUserName" id="txtUserName" title="Please input user name" style="width: 100px;"><input type="button" onclick="GetSiteInfo();" value="Submit" name="btnGetSite" id="btnGetSite" class="btn"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="b l">
                            Current site:
                        </td>
                        <td class="l">
                            <b><span id="spCurrSite"></span></b>
                        </td>
                    </tr>
                    <tr>
                        <td class="b l">
                            Move to new site:
                        </td>
                        <td class="l">
                            <select style="width: 100px;" name="Site" id="Site">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <input type="button" class="btn" id="Move" name="Move" value="Move" onclick="UpdateMoveSite();"><input
                                type="hidden" value="3751654" name="CustId" id="CustId"><input type="hidden" value="1"
                                    name="IsSameLine" id="IsSameLine">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        var cust = new Customer();
        function UpdateMoveSite() {
            var CustId = gup('CustId');
            if(CustId == "")
                CustId = cust.CustId;
            var NewSite = $('#Site').val();
            var CurrSite = $('#spCurrSite').html();
            $('#trMsg').hide();
            $('#dvMsg').html("");
            if (NewSite == 'Select') {
                $('#trMsg').show();
                showAlertMessage('dvMsg', 'Please choose new site to move', 1);
            }
            else if (NewSite == CurrSite) {
                $('#trMsg').show();
                showAlertMessage('dvMsg', 'New site is duplicated the current site', 1);
            }

            else {
                if (!confirm("Are you sure you want to move this customer and downlines to " + NewSite + " site?")) return;

                try { parent.BlockUI(); } catch (e) { }
                var param = { custId: CustId, newSite: NewSite };

                $.ajax(
                 {
                     type: "POST",
                     url: "../Services/WCF/Customer/ServiceCustomerInfo.svc/UpdateCustomerSite",
                     data: $.toJSON(param),
                     contentType: "application/json; charset=utf-8",
                     success: function (rs) {
                         $('#trMsg').show();
                         if(rs.ErrorCode == 0)
                             showAlertMessage('dvMsg', 'Move site successfully!', 0);
                         else
                             showAlertMessage('dvMsg', rs.ErrorMessage, 1);
                         try { parent.jQuery.unblockUI(); } catch (e) { }

                     }

                 }


             );
            }
        }

        function GetSiteInfo() {

            var UserName = $('#txtUserName').val();
            $('#trMsg').hide();
            $('#dvMsg').html("");
            $('#Move').attr('disabled', false);
            if (UserName.length == 0) {
                $('#trMsg').show();
                showAlertMessage('dvMsg', "Please input username.", 1);
                $('#txtUserName').focus();
                $('#Move').attr('disabled', true);
                return;
            }

            try { parent.BlockUI(); } catch (e) { }

            var param = {username: UserName};
            $.ajax(
                 {
                     type: "POST",
                     url: "../Services/WCF/Customer/ServiceCustomerInfo.svc/GetCustomerInfoMoveSite",
                     data: $.toJSON(param),
                     dataType: 'json',
                     contentType: "application/json; charset=utf-8",
                     success: function (custdetail) {
                         
                         cust = custdetail;
                         if (cust.CustId == 0) {
                             $('#trMsg').show();
                             showAlertMessage('dvMsg', "Invalid username. Please try again.", 1);
                             $('#Move').attr('disabled', true);
                         }
                         else {
                             $("#spCurrSite").html(cust.Site);

                         }

                         try { parent.jQuery.unblockUI(); } catch (e) { }

                     }

                 }
             );
        }

        function OnEnter(e) {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which;
            if (code == 13) {
                GetSiteInfo();
            }
        }

        function UpdateCustRoleId() {
            var param = '';
            param += 'mode=2';
            $.ajax(
                 {
                     type: "POST",
                     url: "../Handlers/UpdateMoveSite.ashx",
                     data: param,
                     success: function (rs) {
                         var RoleIdUpline = rs;
                         $('#RoleIdUpline').val(RoleIdUpline);
                         return false;
                     }

                 }
             );
        }

        function changeUserName() {
            $("#spCurrSite").html("");
            $('#Move').attr('disabled', true);
        }

        function Customer() {
            this.CustId = 0;
            this.RoleId = 0;
            this.Site = "";
        }
    </script>
</body>
</html>
