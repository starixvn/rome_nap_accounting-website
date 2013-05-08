<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeagueWL_Bettype.aspx.cs" Inherits="Report_LeagueWL_Bettype" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>    
    <script src="../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/Report.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Log/Table.css"/>               
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/>
    <link href="../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <script type ="text/javascript">
        function ViewReport() {
            $('#IsExport').val('0');
            ViewReportLeagueWLbySport(24, 0);
        }
        function ExportToExcel() {
            $('#IsExport').val('1');
            $('#form').submit();
        }
    </script>
</head>
<body>
    <form id="form" action="LeagueWL_Bettype.aspx" method="post">        
        <div class="main">
            <div class="maincontent">
                <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC">
                             League Win Lost
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </table>
                <div class="maincontent_box">
                    <asp:Literal ID ="headerDatetime" runat ="server"></asp:Literal>        
                </div>
                <div style="margin-top:2px">               
                    <div id="sErrMsg" class="error-message" style="font-weight:bold"></div>
                    <div id="sContent" runat="server"></div>
                    <input type="hidden" id="IsExport" name="IsExport" />
                </div>
            </div>
        </div>          
        <script type="text/javascript">
            InitFixedHeaderLeagueWL(600);
        </script>                        
    </form> 
</body>
</html>
