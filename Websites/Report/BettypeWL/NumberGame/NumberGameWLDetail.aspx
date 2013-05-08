<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NumberGameWLDetail.aspx.cs"
    Inherits="NumberGameWLDetail" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/input.css" />
    <link href="../../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />

    <script src="../../../js/common.js" type="text/javascript"></script>

    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../NumberGame/JS/matchwinloss.js"></script>

    <%=jsfunction%>

    <script type="text/javascript">
       
        function obj(id) {
            return document.getElementById(id);
        }
        function OpenDetails(bType, bLive, site, fdate, tdate, sporttype, sportname, path, IsRepDB, IsHedge, mode) {
            parent.parent.BlockUI();
            var frm = parent.document.getElementById('WinLostDetails');
            frm.src = 'NumberGameWLBettypeDetail.aspx?B=' + bType 
            + '&Live=' + bLive + '&site=' + site + '&fdate=' + fdate + '&tdate=' + tdate + '&sporttype=161' 
            +  '&sportname=Number Game' + '&path=' + path + "&db=" + IsRepDB + "&IsHedge=" + IsHedge + '&mode=' + mode
            + '&SiteID=' + $("#selectCompany").val() + '&Currency=' + $("#selectCurrency").val();
        }
        function SplitTable() {            
            if (count != 2) {
                //obj('spSplit').className = 'hide';
            }
        }
        function ViewReport() {
            parent.parent.BlockUI();
            $('#IsExport').val('0');
            $('#btnReport').attr('disabled', true);
            $('#form').submit();
        }
        function ExportToExcel() {
            $('#IsExport').val('1');
            $('#form').submit();
        }
        matchwinloss.init("bettype");
        $(document).ready(function(){
            parent.parent.$.unblockUI();
        });
    </script>

</head>
<body>
    <form id="form" action="NumberGameWLDetail.aspx" method="post">
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC">
                            Number Game Win Loss by Bet Type
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="bg_table">
            <div>
                <table border="0" width="340">
                    <tr>
                        <td style="padding-right: 4px">
                            <div>
                                Company:</div>
                        </td>
                        <td>
                            <div>
                                <select id="selectCompany" class="txt" style="width: 100px;" runat="server">
                                </select>
                                <input type="hidden" id="hiddenCompnayFilter" runat="server" value="0" />
                            </div>
                        </td>
                        <td style="padding-left: 8px">
                            <div>
                                Currency:</div>
                        </td>
                        <td style="padding-left: 4px">
                            <select id="selectCurrency" class="txt" style="width: 100px" runat="server">
                            </select>
                            <input type="hidden" id="hiddenCurrencyFilter" runat="server" value="0" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="font-small">
                <asp:Literal ID="headerDatetime" runat="server"></asp:Literal></div>
            <asp:Literal ID="content" runat="server"></asp:Literal>
            <asp:Literal ID="Footer" runat="server"></asp:Literal>

            <script type="text/javascript">
                SplitTable();
            </script>

            <input type="hidden" id="IsExport" name="IsExport" />
            <input type="hidden" id="mode" name="mode" value="<%=mode %>" />
        </div>
    </div>
    </form>
</body>
</html>
