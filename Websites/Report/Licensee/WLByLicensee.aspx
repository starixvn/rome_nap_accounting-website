<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WLByLicensee.aspx.cs" Inherits="Report_Deposit_Deposit" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <title id="ReportTitle" runat="server"></title>
 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Style/Deposit.css" rel="stylesheet" type="text/css" />
    
    <script src="../../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/BlockUI.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script> 
    <script src="../../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/LicenseeReport.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body >
   <div class="main"> 
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Win Loss by Licensee
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:1146px;">
                <div id="header_main">
                <table>
                    <tr>
                        <td rowspan="2" style="width:80px"><b>Search by</b> </td>
                        <td style="width:50px; text-align:right">Licensee:</td>
                        <td>
                            <select onchange="SelCurrency();" id="dlSite" style="vertical-align:middle;width:150px" >
                                <option value="0">Please Choose One !</option>
                                <%=strSite%>
                            </select>                            
                        </td>
                        <td style="width:50px; text-align:right">Currency:</td>
                        <td>
                            <select id="dlCurrency" style="vertical-align:middle;width:150px">
                                <option value="0">All</option>
                                <option value="-3">Base Currency-USD</option>
                                <option value="-2">Base Currency-RM</option>                
                                <option value="-6">Base Currency-EUR</option>                
                            </select>
                        </td>
                        <td style="width:50px; text-align:right">Sport:</td>
                        <td>
                            <select id="dlSport" style="vertical-align:middle;width:150px">
                                <option value="-1">All</option>
                                <option value="0">Sportsbook</option>
                                <option value="151">Racing</option>
                                <option value="161">Number Game</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td class="r">Date:</td>
                        <td>
                            <select id="dlDate" style="vertical-align:middle;width:150px" onchange="GetDateTime(this.value)">
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
                        <td colspan="2">
                            <span style="padding-right:8px"><input type="radio" name="RdReportBy" id="rdDaily" checked="checked" value="1" /><label for="rdDaily">Daily</label></span>
                            <span style="padding-right:5px"><input type="radio" name="RdReportBy" id="rdMonthly"  value="2" /><label for="rdMonthly">Monthly</label></span>
                            <span style="padding-right:60px"><input type="radio" name="RdReportBy" id="rdSport"  value="3" /><label for="rdSport">Sport</label></span>                                                       
                        </td>
                        <td class="r">Type:</td>
                        <td >
                            <select id="ddltype" style="vertical-align:middle;width:158px">
                                <option value="1">Normal</option>
                                <option value="3">No Draw for Turnover only</option>
                                <option value="2">No Draw</option>
                            </select>
                        </td>
                        
                        <td colspan="2" >
                        </td>
                        
                    </tr>
                 
                    <tr>
                        <td colspan="2"> <b>Exclude Testing</b></td>
                        <td colspan="2">
                             <span style="padding-right:27px"><input type="radio" name="rdExcludeTesting" id="rdExcludeTestingYes" checked="checked" value="1" /><label for="rdDaily">Yes</label></span>
                             <span style="padding-right:5px"><input type="radio" name="rdExcludeTesting" id="rdExcludeTestingNo" checked="checked" value="0" /><label for="rdDaily">No</label></span>
                        </td>
                        <td colspan="2">
                            <input id="Button2" name="btnReport" type="button" value="REPORT" class="btn" onclick="ResetValue();licenseeReport.renderMainReport(0);" />
                            <input id="Button3" name="btnReset" type="button" value="RESET" class="btn" onclick="DoReset()" />
                        </td>
                    </tr>

                </table>
                 <div id="ErrMsg"></div>
            </div>
            <div style="padding-bottom:3px;">                
            </div>
          </div>
            <div id="mainreport">
                <div id="winlossmain" class="content-warper hidden">
                        <div class="content-toolbox bgtitle">
                            <div id="gridtitle" class="gridtitle"></div>
                            <div class="btngroup">
                                <input type="button" onclick="licenseeReport.goBack();" value="  " name="btnBack"
                                    id="btnBackToMain" class="btnBack" title="Back To Main" />
                                <input type="button" onclick="licenseeReport.exportExcel();" value="  " name="btnExport"
                                    id="btnExport" class="btnExcel" title="Export to Excel" /></div>
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" >
                                    <tr>
                                          <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col100 c" id="colReportBy">Date</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col100 c">Currency</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col100 c">Turnover</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;" >
                                            <div class="col100 c">Total Trans</div>
                                        </td>
                                        <td colspan="3">
                                            <div class="c">Member</div>
                                        </td>
                                        <td colspan="3">
                                            <div class="c">Agent</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col100 c ">Company</div>
                                        </td>                                      
                                    </tr>
                                     <tr>
                                        <td style="border-bottom:none 0px;">
                                            <div class="col100 c">Winloss</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="col100 c">Comm</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="col100 c">Total</div>
                                        </td>  
                                        <td style="border-bottom:none 0px;">
                                            <div class="col100 c">Winloss</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="col100 c">Comm</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="col100 c">Total</div>
                                        </td>                                        
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblwinlossmain" class="tbl-body-scroll">
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
                                <table  style="width:100%" cellpadding="0" cellspacing="1">
                                    <tr>
                                        <td>
                                             <div id="pagination">&nbsp;</div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        
                    </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        getLicensees();
    </script>
    <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
  
</body>
</html>
