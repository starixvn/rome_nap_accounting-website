<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="DailyWL_MixParlay.aspx.cs" Inherits="DailyWL_MixParlay" Async="true" Title="Untitled Page"%>
<%@ Import Namespace="DNAide.Core" %>
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
            
             try{parent.BlockUI();}catch(e){}
             function ViewReport() {
                 alert('dfs');
                 try { parent.BlockUI(); } catch (e) { }
                 
                $('#IsExport').val('0');
                $('#btnReport').attr('disabled', true);
                $('#form').submit();
            }

            function ExportToExcel() {
                $('#IsExport').val('1');
                $('#form').submit();
            }

            function ViewReport(date) {
                var winlossdate = '<%=winlostdate %>'
                var datechose = ''; 
                $('#dateReport').val(date);
                if (date == 1)
                    datechose = '<%=today %>'
                else if (date == 2)
                    datechose = '<%=yesterday %>'
                var d1 = new Date(winlossdate);
                datechose = $('#fdate').val();
                var d2 = new Date(datechose);
                
                if (d2.getTime() >= d1.getTime()) {
                    var url = "DailyWL_MixParlayAdvance.aspx?&fdate=" + datechose + "&path=Mix Parlay&db=Rep&bettype=9&ispopup=false";
                    $(location).attr('href', url);
                }
                else {
                    $('#form').submit();
                }
            }
        </script>
        
    </head>
    <body>
    <form id ="form" action ="DailyWL_MixParlay.aspx" method ="post">
        <asp:Literal ID ="headerDatetime" runat ="server"></asp:Literal>
        <asp:Literal ID ="content" runat ="server"></asp:Literal>
        <asp:Literal ID ="Footer" runat ="server"></asp:Literal>
        <input type="hidden" id="mode" name="mode" value="<%=mode %>" />        
        <input type="hidden" id="IsExport" name="IsExport" />
        <input type="hidden" id="hiddenBettype" runat="server" />
        <input type="hidden" id="dateReport" name="dateReport"/>
        <input type="hidden" id="bettype" name="bettype" value="<%=CastAide.AsInt32(Request["bettype"], 0) %>"/>
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
    
