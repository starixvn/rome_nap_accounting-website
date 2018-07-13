<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerBalance.aspx.cs" Inherits="CustomerBalance" Title="Untitled Page" %>

<%@ Import Namespace="Fanex.Common" %>
<%@ Import Namespace="Fanex.Tarus.Infrastructure.Constants" %>
<%@ Import Namespace="Fanex.Tarus.Web.Models.Customer.Balance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Daily Txn Info</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <style type="text/css">
        .breadscrumb {
            color: #4a4b4f;
            font-size: 12px;
            height: 18px;
            padding-left: 4px;
            padding-top: 5px;
            font-weight: bold;
            text-align: left;
            border: solid 1px #d2b24d;
            background: #ffcc33;
            background: -moz-linear-gradient(#ffcc33, #ffcc35);
            background: -webkit-linear-gradient(#ffcc33, #ffcc35);
            margin-bottom: 3px;
            vertical-align: middle;
        }
        .breadscrumb > a {
            cursor: pointer;
        }
    </style>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Customer/customerlist.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script src="../js/CustBalance.js" type="text/javascript"></script>
    <script type="text/javascript">
        function updateCheckList(formName) {
            try {
                var _idList = '';
                var f = document.forms[formName];
                var counter = 0;
                cnt = f.elements.length;
                for (var i = 0; i < cnt; i++) {
                    counter++;
                    var e = f.elements[i];

                    if (typeof e != 'undefined' && e.type == 'checkbox' && e.id != '_checkAll' && e.checked == true) {

                        if (counter >= 5) {
                            _idList += e.id + '_';
                            counter = 0;
                        }
                        else {
                            _idList += e.id + ',';
                        }
                    }
                    else {
                        counter--;
                    }
                }
                if (_idList == '') {
                    alert('You must choose at least 1 member to transfer..');
                    return false;
                }

                var msg = 'Are you sure you want to transfer checked members?';
                if (!confirm(msg)) return false;
                document.getElementById('memberIdList').value = _idList.substring(0, _idList.length - 1);
            }
            catch (ex) {
                alert(ex);
                return false;
            }
            return true;
        }
        function Bala_CheckAll(formName, id) {
            try {
                var f = document.forms[formName];
                var isChecked = document.getElementById(id).checked;
                cnt = f.elements.length;
                for (var i = 0; i < cnt; i++) {
                    var e = f.elements[i];
                    if (e.value != 0.00) {
                        if (typeof e != 'undefined' && e.type == 'checkbox')
                            e.checked = isChecked;
                    }
                }
            }
            catch (ex) {
                alert(ex);
            }
        }
        function _CheckItem(formName, id) {
            try {
                var f = document.forms[formName];
                var ischeck = true;
                cnt = f.elements.length;
                for (var i = 1; i < cnt; i++) {
                    var e = f.elements[i];
                    if (e.value != 0) {
                        if (typeof e != 'undefined' && e.type == 'checkbox') {
                            if (e.checked == false) {
                                ischeck = false;
                                break;

                            }
                        }
                    }
                }

                document.getElementById("_checkAll").checked = ischeck;
            }
            catch (ex) {
                alert(ex);
            }
        }
        //        parent.BlockUI("Loading...");
        //        jQuery(document).ready(function() {
        //add event for rows data only, ignore title row
        //            $(".tbllayout tr:not(:.title)").mouseover(function() {
        //                rowover(this)
        //            });
        //            $(".tbllayout tr:not(:.title)").mouseout(function()
        //            { rowout() });
        //        });

    </script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">
                        <asp:Literal ID="sTitle" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div class="breadscrumb">
                    <%
                        bool isCached = Converter.ToBool(Request["isCached"]);
                        if (!isCached)
                        {
                            Session[CustomerBalanceConstants.CustomerBalancePageStatusSessionKeys] = null;
                        }
                        else
                        {
                            var customerBalancePageStatus = (CustomerBalancePageStatus)Session[CustomerBalanceConstants.CustomerBalancePageStatusSessionKeys];
                            if (customerBalancePageStatus.SuperId != 0)
                            {
                    %>
                    <a onclick="parent.resetMenu();parent.gotoURL('centerFr','CustomerBalance/Index?custid=<%=customerBalancePageStatus.SuperId%>&roleId=4&userName=<%=customerBalancePageStatus.SuperName%>&isCached=true');"><%=customerBalancePageStatus.SuperName%></a>
                    &nbsp;>&nbsp;

                    <%
                        }

                        if (customerBalancePageStatus.MasterId != 0)
                        {
                    %>
                    <a onclick="parent.resetMenu();parent.gotoURL('centerFr','CustomerBalance/Index?custid=<%=customerBalancePageStatus.MasterId%>&roleId=3&userName=<%=customerBalancePageStatus.MasterName%>&isCached=true');"><%=customerBalancePageStatus.MasterName%></a>
                    &nbsp;>&nbsp;
                    <%
                            }
                        }
                    %>
                    <%=Request["userName"] %>
                </div>
                <asp:Literal ID="sBody" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</body>
</html>


