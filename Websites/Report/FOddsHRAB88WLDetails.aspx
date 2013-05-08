<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FOddsHRAB88WLDetails.aspx.cs" Inherits="Report_HRMansionWLDetails" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"> 
<head runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="Cache-Control" content="no-cache" />
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
    <script type="text/javascript">
        var widthmainpage = 900;
        var betlistpage = 'FOddsHRAB88BL.aspx';
        var mode = 19;
        var bettypename = 'Racing Daily Report (Fun88 - for FOdds)';
    </script>
    
</head>
<body>
    <div class="main">
        <div class="maincontent">
               <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">Racing Daily Report (Fun88 - for FOdds)
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <%=strDateTimeSingle%>
            </div>
            <div class="bg_table">
                <div id="sErrMsg" class="error-message" style="font-weight:bold"></div>
                <div id="sContent"></div>
                <input type="hidden" id="movebetsday" name="movebetsday" value="<%=ReportUtils.BalanceUp.MoveBetsDay %>" />           
            </div>
        </div>
    </div>

   <script type="text/javascript">
       ViewReport();
     </script>
</body>
</html>
