<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="DailyWL_Sporttype.aspx.cs" Inherits="DailyWL_Sporttype" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/> 
<link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css"></link>
<script src ="../js/common.js" type ="text/javascript"></script>    
<script src="../js/jQuery/jquery.js" type="text/javascript"></script>
<script type ="text/javascript">
    function TopLocation() {
       //window.location.href = window.location.href;
    }

    function ViewReport() {
        try{parent.BlockUI();}catch(e){}    
        $('#IsExport').val('0');
        $('#btnReport').attr('disabled', true);
        $('#form').submit();
    }
    function ExportToExcel() {
        $('#IsExport').val('1');
        $('#form').submit();
    }
    
    function ViewReport(date){
        var d;
         $('#IsExport').val('0');
        if(date == 1)
        {
            $('#reportDate').val($('#btnToday').val());
            $('#btnToday').attr('disabled', true);
        }
        else if(date == 2){
            $('#reportDate').val($('#btnYesterday').val());
            $('#btnYesterday').attr('disabled', true);
        }
        else
            $('#reportDate').val($('#fdate').val());
         $('#form').submit();
    }
</script>        
</head>
<body style="margin:5px 2px 5px 5px">
    <form id ="form" action ="DailyWL_Sporttype.aspx" method ="post">
        <asp:Literal ID ="headerDatetime" runat ="server"></asp:Literal>        
        <asp:Literal ID ="content" runat ="server"></asp:Literal>
        <asp:Literal ID ="Footer" runat ="server"></asp:Literal>     
        <input type="hidden" value="" id="reportDate" name="reportDate" />
        
        <input type="hidden" id="IsExport" name="IsExport" />
        <script type ="text/javascript">
            TopLocation();
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

    



