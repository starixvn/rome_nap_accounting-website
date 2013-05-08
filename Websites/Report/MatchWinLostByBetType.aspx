<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MatchWinLostByBetType.aspx.cs" Inherits="MatchWinLostByBetType" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
<script type="text/javascript" src="../js/jQuery/jquery.js"></script>
<script type ="text/javascript">
    function GetData() {      
        var params='?fdate='+$('#fdate').val() + '&tdate='+$('#tdate').val();
        parent.BlockUI();
        $.ajax({
                    url: 'MatchWinLostByBetType.ashx' + params,
                    dataType: 'html',     
                    success: function(data){
                        $('#sData').html(data);
//                        $(".tbllayout tr:not(:.title.report-footer)").mouseover(function(){rowover(this)});
//                        $(".tbllayout tr:not(:.title.report-footer)").mouseout(function(){rowout()});
                        parent.jQuery.unblockUI();
                    }
                });
    }        
        
    function ExportToExcel()
    {
        var params='?fdate='+$('#fdate').val() + '&tdate='+$('#tdate').val() + "&isexport=1";
        document.location.href = "MatchWinLostByBetType.aspx" + params;
    }
    
//    function rowover(obj) {$(".tbllayout .overrow").removeClass("overrow");$(obj).addClass("overrow");}
//    function rowout() {$(".tbllayout .overrow").removeClass("overrow");}
</script>
</head>
<body>
      <asp:Literal ID="content" runat="server"></asp:Literal>
</body>
</html>


