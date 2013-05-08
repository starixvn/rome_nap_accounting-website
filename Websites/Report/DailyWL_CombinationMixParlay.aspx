<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="DailyWL_CombinationMixParlay.aspx.cs" Async="true" Inherits="DailyWL_CombinationMixParlay" Title="Untitled Page"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat ="server">
        <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
        <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
        <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/> 
        <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
        <script type ="text/javascript" src ="../js/common.js"></script>
        <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
        <script type ="text/javascript">
            function ViewReport() {
                $('#IsExport').val('0');
                $('#btnReport').attr('disabled', true);
                $('#form').submit();
            }

            function ExportToExcel() {
                $('#IsExport').val('1');
                $('#form').submit();
            }
            
            function ViewReport(date){
                $('#dateReport').val(date);
                $('#form').submit();
            }
        </script>
        
    </head>
    <body>
    <form id ="form" action ="DailyWL_CombinationMixParlay.aspx" method ="post">
        <asp:Literal ID ="headerDatetime" runat ="server"></asp:Literal>
        <asp:Literal ID ="content" runat ="server"></asp:Literal>
        <asp:Literal ID ="Footer" runat ="server"></asp:Literal>
        <input type="hidden" id="mode" name="mode" value="<%=mode %>" />        
        <input type="hidden" id="IsExport" name="IsExport" />
        <input type="hidden" id="hiddenBettype" runat="server" />
        <input type="hidden" id="dateReport" name="dateReport"/>
        <script type="text/javascript">
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
     </form>
    </body>
</html>
    
