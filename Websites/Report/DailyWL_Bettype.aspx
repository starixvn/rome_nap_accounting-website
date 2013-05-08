<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyWL_Bettype.aspx.cs" Inherits="DailyWL_Bettype" Theme="" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/> 
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
     <%=jsfunction%>  
    <script type="text/javascript">
       
        function obj(id) {
            return document.getElementById(id);
        }
        function OpenDetails(bType, bLive, site, fdate, tdate, sporttype, sportname, path, IsRepDB, IsHedge, mode) {
            var frm = parent.document.getElementById('WinLostDetails');
            frm.src = 'DailyWL_Detail.aspx?B=' + bType + '&Live=' + bLive + '&site=' + site + '&fdate=' + fdate + '&tdate=' + tdate + '&sporttype=' + sporttype + '&sportname=' + sportname + '&path=' + path + "&db=" + IsRepDB + "&IsHedge=" + IsHedge + '&mode=' + mode;
        }
        function SplitTable() {            
            if (count != 2) {
                //obj('spSplit').className = 'hide';
            }
        }
        function ViewReport() {
            $('#IsExport').val('2');
            $('#btnReport').attr('disabled', true);
            $('#form').submit();
        }
        function ExportToExcel() {
            $('#IsExport').val('1');
            $('#form').submit();
        }
        function ViewReport(date){
            $('#dateReport').val(date);
            $('#IsExport').val('2');
            $('#form').submit();
        }
         
    </script>

</head>
<body >
    <form id="form" action="DailyWL_Bettype.aspx" method="post">
        <div class="font-small"><asp:Literal ID="headerDatetime"  runat="server"></asp:Literal></div>
        <asp:Literal ID="content" runat="server"></asp:Literal>
        <asp:Literal ID="Footer" runat="server"></asp:Literal>
        <script type="text/javascript">
            SplitTable();
             var today = '<%=today %>';
             var yesterday = '<%=yesterday %>';
            $(document).ready(function () {
                    
                var strtoday = '<input type="button" onclick="ViewReport(1)" id="btnToday" name="Today" class="btn"/>';
                var stryesterday = '<input type="button" onclick="ViewReport(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
                $('#spMoreElement').append(strtoday);
                $('#spMoreElement').append(stryesterday);
                $('#btnToday').val(today);
                $('#btnYesterday').val(yesterday);
            });
        </script>
        <input style="display:none" id="IsExport" name="IsExport" type="hidden" />
        <input style="display:none" id="mode" name="mode" value="<%=mode %>" type="hidden" />
        <input style="display:none" id="dateReport" name="dateReport" type="hidden" />
           
    </form>
</body>
</html>
