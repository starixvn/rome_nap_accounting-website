<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Deposit.aspx.cs" Inherits="Report_Deposit_Deposit" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/Deposit.css" rel="stylesheet" type="text/css" />

    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    

    <script src="../../js/CommonFxHeader.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/DepositReport.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>

</head>
<body >
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Deposit Reports
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="header_main">

                <table>
                    <tr>
                        <td rowspan="2" style="width:80px"><b>Search by</b> </td>
                        <td style="width:50px; text-align:right">Site:</td>
                        <td>
                            <select onchange="SelCurrency();" id="dlSite" style="vertical-align:middle;width:150px" >
                                <option value="0">Please Choose One !</option>
                                <%=strSite%>
                            </select>                            
                        </td>
                        <td style="width:50px; text-align:right">Currency:</td>
                        <td>
                            <select id="dlCurrency" style="vertical-align:middle;width:150px">
                                <option value="0">Please Choose One !</option>
                                <option value="-1">Base Currency-USD</option>
                                <option value="-2">Base Currency-RM</option>                
                            </select>
                        </td>
                        <td style="width:50px; text-align:right">Sport:</td>
                        <td>
                            <select id="dlSport" style="vertical-align:middle;width:150px">
                                <option value="-1">Please Choose One !</option>
                                <option value="0">Sportsbook</option>
                                <option value="151">Racebook</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="r">Date:</td>
                        <td>
                            <select id="dlDate" style="vertical-align:middle;width:150px" onchange="GetDateTime(this.value)">
                                <option value="0" selected="selected">Please Choose One !</option>
                                <option value="1">Today</option>
                                <option value="2">Yesterday</option>
                                <option value="3">Current Week</option>
                                <option value="4">Last Week</option>
                                <option value="5">Current Month</option>
                                <option value="6">Last Month</option>
                                <option value="7">Since Last Month</option>
                                <option value="8">Specific Date</option>
                            </select>
                        </td>
                        <td class="r">From:</td>
                        <td>
                            <%=strfdate %>
                        </td>
                        <td class="r">To:</td>
                        <td><%=strtdate %></td>
                    </tr>
                    <tr style="height:30px;">
                        <td><b>Report by</b></td>
                        <td colspan="3">
                            <span style="padding-right:5px"><input type="radio" name="RdReportBy" id="rdDaily" checked="checked" value="1" /><label for="rdDaily">Daily</label></span>
                            <span style="padding-right:5px"><input type="radio" name="RdReportBy" id="rdMonthly"  value="2" /><label for="rdMonthly">Monthly</label></span>
                            <span style="padding-right:60px"><input type="radio" name="RdReportBy" id="rdSport"  value="3" /><label for="rdSport">Sport</label></span>
                        </td>
                        <td colspan="3">
                            <input id="btnReport" name="btnReport" type="button" value="REPORT" class="btn" onclick="ResetValue();SelReportDepositBySite(1);" />
                            <input id="Button1" name="btnReset" type="button" value="RESET" class="btn" onclick="DoReset()" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="padding-bottom:3px;">                
            </div>
          </div>
          
            <div id="mainreport">

            <div id="ErrMsg"></div>

                    <div id="mixparlaywon" class="content-warper"  style="display: none;">
                        <div class="content-toolbox bgtitle">
                            <div id="gridtitle" class="gridtitle"></div><div class="btngroup"><input type="button" onclick="ExportToExcel();" value="  " name="btnExport" id="btnExport" class="btnExcel" title="Export to Excel"></div>
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" >
                                    <tr>
                                        <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col100 c" id="dvDate_UserName">Date</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col80 c">Currency</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col80 c">Turnover</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col80 c">Gross Comm</div>
                                        </td>
                                        <td colspan="3">
                                            <div class="c col240">Member</div>
                                        </td>
                                        <td colspan="3">
                                            <div class="c col240">Agent</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col80 c ">Company</div>
                                        </td>                                      
                                    </tr>
                                     <tr>
                                        <td >
                                            <div class="col80 c">Winloss</div>
                                        </td>
                                        <td >
                                            <div class="col80 c">Comm</div>
                                        </td>
                                        <td >
                                            <div class="col80 c">Total</div>
                                        </td>
                                        <td >
                                            <div class="col80 c">Winloss</div>
                                        </td>
                                            <td >
                                            <div class="col80 c">Comm</div>
                                        </td>
                                        <td >
                                            <div class="col80 c">Total</div>
                                        </td>                                        
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblmixparlay" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                         
                        <div class="content-footer clearfix">
                            <div class="footer-scroll">
                                <table class="tbl-footer-scroll" id="tblFooter">                                   
                                </table>
                            </div>
                        </div>
                        <div class="content-pagination bgtitle clearfix">
                            <div class="pagination-scroll">
                                <table  style="width:100%">
                                    <tr>
                                        <td>
                                             <div id="pagination"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        
                    </div>
                </div>
        </div>
    </div>

    

    <input type="hidden" id="balanceUpday" value="<%=ReportUtils.BalanceUp.BalanceUpDay.ToShortDateString() %>" />
    <input type="hidden" id="level" value="1" />
    <input type="hidden" id="currency" value="" />
    <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
    <script type="text/javascript">

        GetLicenseeList();

        SelCurrency();
    </script>
</body>
</html>
