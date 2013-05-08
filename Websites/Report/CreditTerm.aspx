<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditTerm.aspx.cs" Inherits="Report_CreditTerm"
    Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <script type="text/javascript" src="../js/prototype-full.js"></script>
    <style>
        .hand
        {
            cursor: point;
        }
        .edit
        {
            text-align: right;
            font-size: 7pt;
            color: grey;
        }
    </style>
    <script type="text/javascript">
        var click = 0;
        function EditRemark(obj) {
            var id = obj.id;
            $('dv' + id).hide();
            var txt = $('txt' + id);
            txt.show();
            txt.style.width = '98%';
            txt.focus();
        }

        function UpdateEditRemark(custid) {
            if (click > 0) {
                click = 0;
                return;
            }
            var id = custid;

            var txt = $('txt' + id);
            var div = $('dv' + id);
            var remark = txt.value;
            var param = '?mode=3&remark=' + remark + '&custid=' + id;
            var myajax = new Ajax.Request("GetData.ashx", {
                asynchronous: true,
                method: 'get',
                parameters: param,
                onSuccess: function (rs) {
                    var str = rs.responseText.split('|');
                    if (str[0] == 'no') {
                        $('err').innerHTML = '*' + str[1];
                        txt.value = div.innerHTML;
                        txt.hide();
                        div.show();
                    }
                    else {
                        $('err').innerHTML = '';
                        txt.hide();
                        div.innerHTML = remark;
                        div.show();
                        $('dvTime_' + id).innerHTML = str[1];
                    }
                }
            });
        }

        function on_UpdateEditRemark(e, custid) {
            var code = (window.Event) ? e.which : e.keyCode;
            if (code == 13) {

                UpdateEditRemark(custid);
                click++;
            }

        }

        function LoadCreditTerm() {
            var pre1 = $('txtPre1').value;
            var pre2 = $('txtPre2').value;
            $('err').innerHTML = '';
            if (isNaN(pre1) || parseFloat(pre1) < 0) {
                $('err').innerHTML = '* Invalid Pre-Agreed Amount!';
                $('txtPre1').focus();
                return;
            }
            if (isNaN(pre2) || parseFloat(pre2) < 0) {
                $('err').innerHTML = '* Invalid Pre-Agreed Amount!';
                $('txtPre2').focus();
                return;
            }

            $('inProgressBar').show();
            $('btnSubmit').disabled = true;

            var param = '?mode=4';
            param += "&checkname=" + $('txtCheckname').value;
            param += "&pre1=" + $('txtPre1').value;
            param += "&pre2=" + $('txtPre2').value;

            var myajax = new Ajax.Request("GetData.ashx", {
                asynchronous: true,
                method: 'get',
                parameters: param,
                onSuccess: function (rs) {
                    $('sData').innerHTML = rs.responseText;
                    $('inProgressBar').hide();
                    $('btnSubmit').disabled = false;
                }
            });
        }

        function On_LoadCreditTerm(e) {
            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which;
            if (code == 13) {
                LoadCreditTerm();
            }
        }

        function ExportToExcel() {
            var param = '?btnExport=1';
            var displayRQ = $('displayRQ').innerHTML;
            if (displayRQ == 'pre') {
                var pre1 = $('txtPre1').value;
                var pre2 = $('txtPre2').value;
                $('err').innerHTML = '';
                if (isNaN(pre1) || parseFloat(pre1) < 0) {
                    $('err').innerHTML = '* Invalid Pre-Agreed Amount!';
                    $('txtPre1').focus();
                    return;
                }
                if (isNaN(pre2) || parseFloat(pre2) < 0) {
                    $('err').innerHTML = '* Invalid Pre-Agreed Amount!';
                    $('txtPre2').focus();
                    return;
                }

                param += '&display=pre';
                param += '&mode=4';
                param += "&checkname=" + $('txtCheckname').value;
                param += "&pre1=" + $('txtPre1').value;
                param += "&pre2=" + $('txtPre2').value;
            }
            else
                param += '&display=' + displayRQ;
            var url = 'CreditTerm.aspx' + param;
            $('mainfr').src = url;
        }                             
    </script>
</head>
<body>
    <div class="main">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Credit Term </span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <asp:literal id="link" runat="server"></asp:literal>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="sData">
                                <%= sData.ToString() %></div>
                            <div id="displayRQ" style="display: none">
                                <%=display.ToString().Trim()%></div>
                            <iframe frameborder="0" name="mainfr" id="mainfr" style="width: 100%; height: 90%;">
                            </iframe>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
