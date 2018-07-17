<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoveCustomer.aspx.cs" Inherits="Customer_MoveCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Move Customer</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/common.js"></script>


    <script type="text/javascript">


        function MoveCustomer() {
            if (CheckInputValue()) {
                var param = '';
                param = 'downline=' + obj("txtDownline").value;
                param += '&upline=' + obj("txtUpline").value;
                param += '&agent=' + gup("agent");
                $.ajax({
                    type: "POST",
                    url: '../Handlers/UpdateMoveCust.ashx',
                    data: param,
                    success: function(data) {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        alert_error(data, 'errMsg');
                    },
                    error: function (xhr) {
                        if (xhr.status === 403) {
                            var denyObject = $.parseJSON(xhr.responseText);
                            alert_error(denyObject.ErrorMessage, 'errMsg');
                        }
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                    }
                });
            }
        }
        
    
        function CheckInputValue() {
            var type = gup("agent");
            var str = "";
            if (type == "1") {
                str = "Agent";
                if (isEmpty("txtDownline")) {
                    //alert_error("Please enter valid Agent Username.")
                    showAlertMessage("errMsg", "Please enter valid Agent Username.", 1);
                    obj("txtDownline").focus();
                    return false;
                }
                if (isEmpty("txtUpline")) {
                    //alert_error("Please enter valid Master Username.")
                    showAlertMessage("errMsg", "Please enter valid Master Username.", 1);
                    obj("txtUpline").focus();
                    return false;
                }
            }
            else {
                str = "Master";
                if (isEmpty("txtDownline")) {
                    //alert_error("Please enter valid Master Username.")
                    showAlertMessage("errMsg", "Please enter valid Master Username.", 1);
                    obj("txtDownline").focus();
                    return false;
                }
                if (isEmpty("txtUpline")) {
                    //alert_error("Please enter valid Super Username.")
                    showAlertMessage("errMsg", "Please enter valid Super Username.", 1);
                    obj("txtUpline").focus();
                    return false;
                }
            }
            var strC = confirm("Are you sure you want to move this " + str + "?");
            if (strC) {
               
                obj("errMsg").innerHTML = "";
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
                    <%=Title %>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg" class="error-message"></div>
                <div id="clear2"></div>
                <table cellspacing="1" cellpadding="0" style="width: 250px; line-height: 21px ! important;" class="tbllayout">
	                <tbody>
	                <tr class="title">
		                <td colspan="2"><%=TableTitle%></td>
	                </tr>
	                <tr>
		                <td nowrap="" style="width: 45%;" class="l b"><%=LblDownline%></td>
		                <td style="padding: 2px;" class="l">
			                <input type="text" id="txtDownline" name="txtDownline" class="txt">
		                </td>
	                </tr>
	                <tr>
		                <td nowrap="" class="l b"><%=LblUpline%></td>
		                <td style="padding: 2px;" class="l">
			                <input type="text" id="txtUpline" name="txtUpline" class="txt">
			                
		                </td>
	                </tr>
	                <tr>
		                <td style="padding: 2px;" class="c" colspan="2">
			                <input class="btn"  type="button" onclick="MoveCustomer();"  name="btnMove" id="btnMove" value="Submit"/>
		                </td>		
	                </tr>
                </tbody></table>
            </div>
        </div>
    </div>
</body>
</html>
