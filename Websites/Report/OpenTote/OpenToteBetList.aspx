<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OpenToteBetList.aspx.cs" Inherits="Report_OptenTote_ReportForSettlement" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/>      
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Report/Report.css"/>
    
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../../js/Report/OpenToteSettlement/OpenToteSettlement.js" type="text/javascript"></script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC"><span id="spTitle"> <%=strTitle%></span></td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>            
        </div>
    </div> 
    <div id="sErrMsg" class="error-message" style="font-weight:bold"></div>
    <div id="sContent"><%=strContent%></div>
    
    <script type="text/javascript">
        InitFixedHeader(850);
      </script> 
</body>
</html>
