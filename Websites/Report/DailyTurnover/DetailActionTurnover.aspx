<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetailActionTurnover.aspx.cs" Theme="" Inherits="Report_DailyTurnover_DetailActionTurnover" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />   
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" /> 
    <link href="style/DailyTurnover.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
     <script type="text/javascript" src="../../js/jQuery/Jlinq/jlinq.js"></script>
    <script type="text/javascript" src="../../js/jQuery/Jlinq/jlinq.jquery.js"></script>
    <script src="script/detailaction.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
  
</head>
<body>
    <div class="main">
        <div class="maincontent">
                <div id="ActionTurnover" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="gridTitleAction" class="gridtitle">
                        </div>
                        <div class="btngroup">
                            <input type="button" onclick="detailActionTurnover.exportToExcel();" value="  " name="btnExport"
                                id="Button1" class="btnExcel" title="Export to Excel" />
                        </div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div id="txtMatch" class="col300 c">
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Total Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Super Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Master Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Agent Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            IBC Turnover</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col140 c">
                                            CNT</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table id="tblActionTurnover" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
            </div>
        </div>
    </div>
     <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <script type="text/javascript">
        var action = '<%=action %>';
        var StrDateTime = '<%=strdatetime %>';
        var username = '<%=username %>';
        $(document).ready(function () {
            detailActionTurnover.ViewDetailAction(action, StrDateTime, username);
        });
    </script>

</body>
</html>
