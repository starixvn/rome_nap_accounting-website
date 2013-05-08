<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckBet.aspx.cs" Inherits="Report_CheckBet_CheckBet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CheckBet</title>    
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/>      
    <link href="../../js/Calendar2/css/jscal2.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Check Bet
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div id="dvHeader">
                <div class="maincontent_box">
                    From Date: <%=ReportFromDate %>
                    <input type="button" onclick="GetCheckBet()" class="btn" value="Submit" name="btnReport" id="btnReport" />
                </div>
            </div>
            <div class="bg_table">
                <div id="divCheckBetContain"></div>
                <div id="divErr"></div>   
            </div>
        </div>
    </div>

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script> 
    <script src="../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>    
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="JS/CheckBet.js" type="text/javascript"></script>
</body>
</html>
