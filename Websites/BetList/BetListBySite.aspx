<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BetListBySite.aspx.cs" Inherits="BetList_BetListBySite" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="Stylesheet" type ="text/css" href ="../App_Themes/Report/Betlist.css?v=4" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <%--<asp:Literal ID="reportHeader" runat="server"></asp:Literal>--%>
    <asp:Literal ID="reportContent" runat="server"></asp:Literal>
<%--    <div id="sData">
    </div>--%>


    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ViewDNS(ip) {
            url = 'http://anonym.to/?http://legacytools.dnsstuff.com/tools/ipall.ch?tool_id=67&token=&toolhandler_redirect=0&ip=' + ip + '&j=1';
            window.open(url, 'DNS', ' scrollbars=yes, toolbar=yes, menubar=yes,location=yes,status=yes,resizable=yes');
        }
        function obj(id) {
            return document.getElementById(id);
        }

        function showMP(refno) {
            var spEvent = obj('spEvent_' + refno);

            if (spEvent.className == 'hide') {
                if (spEvent.innerHTML == "") {
                    $.ajax({
                        url: '../Handlers/GenMixParlay.ashx?refno=' + refno + '&type=2',
                        dataType: 'html',
                        success: function (data) {
                            spEvent.innerHTML = data;
                            spEvent.className = '';

                        }
                    });
                }
                else {
                    spEvent.className = '';
                }
            }
            else {
                spEvent.className = 'hide';
            }
        }
    </script>
</body>
</html>
