<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditLog.aspx.cs" Inherits="LogDisplay_Logs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log Display</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/ViewLog.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/Tab.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>

</head>
<body>
    <div class="main">
        <div class="maincontent">
            <div class="maincontent_box">
                <div class="box_header">
                    <div class="left_tab">
                        <ul>
                            <li id="Li2"><a href="javascript:MenuGoTo('DetailLog')"><span>
                                <asp:Literal ID="Literal10" Text="Detail Log" EnableViewState="false" runat="server" />
                            </span></a></li>
                            <li id="Li1" class="current"><a href="javascript:MenuGoTo('CreditLog')"><span>
                                <asp:Literal ID="Literal9" Text="Credit Log" EnableViewState="false" runat="server" />
                            </span></a></li>
                            
                        </ul>
                    </div>
                </div>
                <input type="hidden" id="hdSearch" name="hdSearch" />
                <div id="clear2">
                    &nbsp;</div>
                <div id="errMsg" class="errMsg">
                </div>
                <asp:Literal ID="ltrSearch" runat="server"></asp:Literal>
                <div id="lnkOld">
                    <span style="color:#90979A;font-weight:bold;">* For data after 01-Jan-2009, please <a onclick="NewForm()">click here</a></span>
                </div>
            </div>
            <div id="divContent" runat="server" class="bg_table">
                <table class="tbllayout" cellspacing="1">
                    <tr class="title">
                        <td>
                            Log ID
                        </td>
                        <td style="width: 80px">
                            Logged Date
                        </td>
                        <td>
                            Edited By
                        </td>
                        <td>
                            Username
                        </td>
                        <td>
                            Function
                        </td>
                        <td>
                            Old Value
                        </td>
                        <td>
                            New Value
                        </td>
                        <td>
                            Description
                        </td>
                        <td>
                            IP
                        </td>
                    </tr>
                    <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
                </table>
            </div>
        </div>
    </div>
  <script type="text/javascript">parent.jQuery.unblockUI();</script>
    <script type="text/javascript">
        $(document).ready(function() {
            $('#txtUsername').focus();
            $('#txtUsername').bind('keypress', function(e) {
                var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                var target = e.target.tagName.toLowerCase();
                if (key === 13 && target === 'input') {
                    e.preventDefault();
                    Search();
                }
            });
            $(document).bind('keypress', function(e) {
                var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                if (key === 13) {
                    e.preventDefault();
                }
            });
        }
                );
        function Search() {
            if (isEmpty("txtUsername")) {
                showAlertMessage("errMsg", "Please Enter Username.", 1)
                return false;
            }
            try{parent.BlockUI();}catch(e){}
            obj("hdSearch").value = "Search";
            var para = '?txtUsername=' + encodeURIComponent(obj('txtUsername').value) + '&fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value + '&hdSearch=Search';
            document.location.href = 'CreditLog.aspx' + para;
        }
        function OpenDetail(objClick, id) {
            if (obj(id) && obj(id).style.display == "none") {
                objClick.iLognnerHTML = "Details -";
                obj(id).style.display = "";
            }
            else {
                objClick.innerHTML = "Details +";
                obj(id).style.display = "none";
            }
        }
        function MenuGoTo(tab) {

            if (tab == 'DetailLog') {
                if (isEmpty("txtUsername"))
                    document.location.href = 'Logs.aspx?tabid=DetailLog&txtUsername=' + encodeURIComponent(obj('txtUsername').value) + '&fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value;
                else
                    document.location.href = 'Logs.aspx?tabid=DetailLog&txtUsername=' + encodeURIComponent(obj('txtUsername').value) + '&fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value + '&hdSearch=Search';
            }
            else if (tab == 'Login') {
                document.location.href = 'LogLogin.aspx?tabid=Login&custname=' + encodeURIComponent(obj('txtUsername').value) + '&fromdate=' + obj('fdate').value + '&todate=' + obj('tdate').value;
            }
            else if (tab == 'CreditLog') {
                if (isEmpty("txtUsername"))
                    document.location.href = 'CreditLog.aspx?tabid=CreditLog&txtUsername=' + encodeURIComponent(obj('txtUsername').value) + '&fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value;
                else
                    document.location.href = 'CreditLog.aspx?tabid=CreditLog&txtUsername=' + encodeURIComponent(obj('txtUsername').value) + '&fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value + '&hdSearch=Search';
            }
        }
        function KeyPress(e) {

            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which;

            if (code == 13) {
                try {
                    obj("btnSubmit").click();
                }
                catch (error) {
                    return false;
                }
            }
        }
        function NewForm() {
            document.location.href = "LogCredit.aspx?tabid=Credit&custname=" + obj('txtUsername').value;
        }
     
    </script>

</body>
</html>
