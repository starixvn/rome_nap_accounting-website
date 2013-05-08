<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetWinning.aspx.cs" Inherits="Report_ResetWinning_ResetWinning" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Css/ResetWinning.css" rel="stylesheet" type="text/css" />

     <script src="../js/jQuery/jquery.js" type="text/javascript"></script>  
    <script src="../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script> 
    <script src="../js/Datetime.js" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="script/ResetWinning.js" type="text/javascript"></script>
   
    <title></title>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Reset Winning
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 525px;">
                <table>
                    <tr>
                        <td>
                            Date:
                        </td>
                        <td style="width: 120px">
                            <%=strDate%>
                        </td>
                        <td align="left">
                            <input type="button" class="btn" onclick="ResetWinning.getContentResetWinning()" value="View Report"
                                name="btnShow" id="btnShow" />
                        </td>
                         <td style="padding-left: 50px"><b>Data</b></td>
                        <td colspan="2">
                            <span style="padding-right:8px"><input type="radio" name="RdData" id="rdMain" checked="checked" value="0" /><label for="rdMain">Main DB</label></span>
                            <span style="padding-right:5px"><input type="radio" name="RdData" id="rdReplicate"  value="1" /><label for="rdReplicate">Replicate DB</label></span>
                                                                              
                        </td>
                    </tr>
                </table>
            </div>
            <div id="mainreport" style="padding-top: 2px">
                <div id="Resetwinning" class="content-warper hidden">
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll" id="tblcontent">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col140 c">
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Win/Loss</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Reset</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Variance</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table id="tblresetwinning" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
   <script type="text/javascript">
       var serverTime = '<%=serverTime%>';
       var currdate = new Date(serverTime);
       var hours = currdate.getHours();
       $(document).ready(function () {
           if (hours >= 6 && hours <= 18) {
               $('#rdReplicate').attr("checked", true);
           }
           else {
               $('#rdMain').attr("checked", true);
           }
       });
   </script>
</body>
</html>
