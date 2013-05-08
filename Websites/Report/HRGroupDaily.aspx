<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HRGroupDaily.aspx.cs" Inherits="Report_HRGroupDaily" Theme=""  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"> 
    <title>Untitled Page</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>   
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/Report.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <div class="main">
        <div class="maincontent">
                  <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC"><%=strTitle%>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <asp:Literal ID="Header" runat="server"></asp:Literal>
            </div>
            <div class="bg_table">
                <input type="hidden" id="type" name="type" value="<%=Request["type"] %>" />
                <div id="sContent" runat="server"></div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $('#tdToDateCal').width(120 + 'px');
        function ViewReport(IsSendMail) {            
            
            $("#spMsg").html('');
           IsSendMail = (!IsSendMail) ? '0' : IsSendMail;
            var param = '';
            param += 'mode=10';
            param += '&fdate=' + $('#fdate').val();
            param += '&tdate=' + $('#tdate').val();
            param += '&type=' + $('#type').val();
            param += '&IsSendMail=' + IsSendMail;
            parent.BlockUI();
            $.ajax(
                 {
                     type: "POST",
                     url: "GetData.ashx",
                     data: param,
                     success: GetJSONResult
                    
                 }
             );             
        }
        
         function GetJSONResult(resContent) {
            
            var reportInfo = $.evalJSON(resContent);
            var sData = reportInfo.HRReport;
            var sMsg = reportInfo.HRError;

            $("#sContent").html(sData);
            $("#spMsg").html(sMsg);
            

            var widthmainpage = 1100;
            InitFixedHeaderBL();
            parent.jQuery.unblockUI();
        }

        function ExportToExcel() {
            var param = '';
            param += 'fdate=' + $('#fdate').val();
            param += '&tdate=' + $('#tdate').val();
            param += '&Export=1';
            param += '&type=' + getPramJs('type');
            
            window.location.href = 'HRGroupDaily.aspx?' + param;
        }

        var widthmainpage = 1100;
        InitFixedHeaderBL();
    </script>
    
    
</body>
</html>
