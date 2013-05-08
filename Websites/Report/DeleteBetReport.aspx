<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeleteBetReport.aspx.cs" Inherits="DeleteBetReport" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title id="ReportTitle" runat="server"></title>
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
<script type="text/javascript" src="../js/jQuery/jquery.js"></script>
<script type ="text/javascript">
        function ViewReport() {
            var params = '?fdate=' + $('#fdate').val();
            try{parent.BlockUI();}catch(e){}
            $.ajax({
            url: '../Handlers/DeleteBetReport.ashx' + params,
                dataType: 'html',
                success: function(data) {
                    $('#sData').html(data);
                    $(".tbllayout tr:not(:.title.report-footer)").mouseover(function() { rowover(this) });
                    $(".tbllayout tr:not(:.title.report-footer)").mouseout(function() { rowout() });
                    parent.jQuery.unblockUI();
                }
            });
        }
        
        function ExportToExcel() {
            var params = '?fdate=' + $('#fdate').val() + "&isexport=1";
            document.location.href = "DeleteBetReport.aspx" + params;
        }
        
        function rowover(obj) { $(".tbllayout .overrow").removeClass("overrow"); $(obj).addClass("overrow"); }
        function rowout() { $(".tbllayout .overrow").removeClass("overrow"); }
</script> 
</head>
<body>
    <div class="main">
        <div class="maincontent">
             <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC"><asp:Literal ID ="header" runat ="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <asp:Literal ID="SearchStandard" runat="server"></asp:Literal>                
            </div>
            <div class="bg_table">
                <div id="sData">
                    <asp:Literal ID="content" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>  
</body>
</html>


