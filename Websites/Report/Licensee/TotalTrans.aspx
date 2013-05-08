<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TotalTrans.aspx.cs" Inherits="Report_Deposit_TotalTrans" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Style/TotalTrans.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/Datetime.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/TotalTrans.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <style>
        :focus, :active
        {
            outline: none;
        }
        ::-moz-focus-inner
        {
            border: 0;
        }
        .txtDropdown
        {
            display: none;
            position: absolute;
            border: 0px;
            outline: none;
        }
    </style>
</head>
<body>
    <div class="main">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Total Transactions by Licensee</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td style="width: 60px;">
                                            Select:
                                        </td>
                                        <td>
                                            <select id="ddlSite" style="width: 160px" onchange="$('input#text4').val($(this).val());">
                                            </select>
                                            <input id="txtSite" class="txtDropdown" value="All Resellers" readonly />
                                        </td>
                                        <td class="r" style="width: 100px;">
                                            Select month:
                                        </td>
                                        <td>
                                            <select id="dllMonth" style="width: 100px">
                                            </select>
                                        </td>
                                        <td>
                                            <input id="btnReport" name="btnReport" type="button" value="REPORT" class="btn" disabled="disabled"
                                                onclick="totalTrans.gettotalTrans();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Products:
                                        </td>
                                        <td colspan="3">
                                            <span>
                                                <input type="checkbox" id="chbAll" checked="checked" onclick="change(this);" />
                                                    <label for="chbAll">All</label></span> 
                                                    <span> <input type="checkbox" id="chbSport" class="chksport" checked="checked" onclick="changeSport();" /><label for="chbSport">Sportsbook</label></span> 
                                                    <span> <input type="checkbox" id="chbRacing" class="chksport" checked="checked" onclick="changeSport();" /><label for="chbRacing">Racing</label></span> 
                                                    <span> <input type="checkbox" id="chbNG" checked="checked" class="chksport" onclick="changeSport();" /><label for="chbNG">Number Game</label></span> 
                                                    <span> <input type="checkbox" id="chbLiveCasino" class="chksport" checked="checked" onclick="changeSport();" /><label for="chbLiveCasino">Live Casino</label> </span>
                                                    <span> <input type="checkbox" id="chkVirtualSport" class="chksport" checked="checked" onclick="changeSport();" /><label for="chkVirtualSport">Virutal Sports</label> </span>
                                        </td>
                                    </tr>
                                    <tr><td colspan="4" style="text-align:left;">* For data accuracy,  generate the billing report 5 days after month-end.</td></tr>
                                </table>
                                <div id="ErrMsg">
                                </div>
                            </div>
                        </div>
                        <div id="totalTrans" class="content-warper hidden" style="margin-top: 6px;">
                            <div class="content-toolbox bgtitle">
                                <div id="gridtitle" class="gridtitle">
                                </div>
                                <div class="btngroup">
                                    <input type="button" onclick="totalTrans.exportExcel()" value="  " id="btnExport"
                                        class="btnExcel" title="Export to Excel"></div>
                            </div>
                            <div class="content-header clearfix">
                                <div class="header-scroll">
                                    <table class="tbl-header-scroll">
                                        <tr>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col200 c">
                                                    Licensees</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col140 c">
                                                    Total Transactions</div>
                                            </td>
                                            <td style="border-bottom: none 0px;">
                                                <div class="col140 c">
                                                    Transaction Cost (EUR)</div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="content-body clearfix">
                                <div class="body-scroll">
                                    <table id="tbltotalTrans" class="tbl-body-scroll">
                                    </table>
                                </div>
                            </div>
                            <div class="content-footer clearfix">
                                <div class="footer-scroll">
                                    <table class="tbl-footer-scroll" id="tblFooter">
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    </div>
</body>
</html>
