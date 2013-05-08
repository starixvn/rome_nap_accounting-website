<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MatchWinloss.aspx.cs" Theme=""
    Inherits="Report_MatchWinloss" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/numbergamewinloss.css" rel="stylesheet" type="text/css" />
  
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="JS/numbergamewinlost.js" type="text/javascript"></script>
    <script src="JS/matchwinloss.js" type="text/javascript"></script>
  
    <script type="text/javascript">
        var _pageIndex = 1;
        var _pageSize = 50;
        var id = "";
        var bettypename = "";
        var matchid = 0;
        var fromcache = false;
        var preTop = 0;
        var preLeft = 0;
     
        matchwinloss.init();
    </script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Number Game Win Loss By Match
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <table>
                    <tr>
                        <td style="padding-right:4px">
                            <div>Company:</div>
                        </td>
                        <td>
                            <div>
                                <select id="selectCompany" class="txt" style="width:100px;" runat="server"></select>
                                <input type="hidden" id="hiddenCompnayFilter" runat="server" value="0" />
                            </div>
                        </td>
                        <td style="padding-left:8px">
                            <div>Currency:</div>
                        </td>
                        <td style="padding-left:4px">
                            <select id="selectCurrency" class="txt" style="width:100px" runat="server"></select>
                            <input type="hidden" id="hiddenCurrencyFilter" runat="server" value="0" />
                        </td>
                        <td style="padding-left:8px">
                            <div>
                                Date:
                            </div>
                        </td>
                        <td>
                            <%=strDate%>
                        </td>
                        <td style="padding-left:8px">
                            <input type="button" onclick="GetDataReport()" class="btn" value="REPORT" name="btnReport" id="btnReport" />
                        </td>
                    </tr>
                    <tr id="backbtn" style="display: none;">
                        <td colspan="2">
                            <a href="#" onclick="backtomain();">Back To Main</a>
                        </td>
                    </tr>
                </table>

                
            </div>
            <div class="bg_table">
                <div id="mainreport">
                    <div id="matchwinlost" class="content-warper"  style="display:none;">
                        <div class="content-toolbox">
                            <div id="gridtitle" class="gridtitle"></div><div class="btngroup"><input type="button" onclick="ExportToExcel('1');" value="  " name="btnExport" id="btnExport" class="btnExcel" title="Export to Excel"></div>
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll" >
                                    <tr class="bgborder">
                                        <td >
                                            <div class="col160 c">Match - Bet Type</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Winloss</div>
                                        </td>
                                        <td>
                                            <div class="col100 c">Position</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Commission</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Line Total</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Virtual Total</div>
                                        </td>
                                        <td>
                                            <div class="col100 c">Variance</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Turnover</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Margin</div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll">
                                <table id="tblmatchwinlost" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                        <div class="content-footer">
                            <div class="footer-scroll">
                                <table class="tbl-footer-scroll">
                                    <tr>
                                        <td >
                                            <div class="col160 l">Grand Total</div>
                                        </td>
                                        <td >
                                            <div id="totalWinlost" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="totalPosition" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="totalCommission" class="col100 r"></div>
                                        </td>
                                        <td > 
                                            <div id="totalLineTotal" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="totalVirtualLineTotal" class="col100 r"></div>
                                        </td>
                                        <td>
                                            <div id="totalVariance" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="totalTurnOver" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="totalMargin" class="col100 r"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="detailreport" >
                   <div id="matchwinlostdetail" class="content-warper " style="display:none;">
                        <div class="content-toolbox">
                            <div id="gridtitledetail" class="gridtitle"></div><div class="btngroup"><input type="button" onclick="backtomain();" value="  " name="btnBack" id="btnBack" class="btnBack" title="Back To Main"><input type="button" onclick="ExportToExcel('2');" value="  " name="btnExportDetail" id="btnExportDetail" class="btnExcel" title="Export to Excel"></div>
                        </div>
                        <div class="content-header clearfix">
                            <div id="header-frozen-detail" class="header-frozen">
                                <table class="tbl-header-frozen">
                                    <tr>
                                        <td  style="border-bottom:none 0px;">
                                           <div class="col40 c">No.</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                           <div class="col160 c">Username</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                           <div class="col100 c">Turnover</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                           <div class="col100 c">Comm.</div>
                                        </td>
                                    </tr>
                                 
                                </table>
                            </div>
                            <div id="header-scroll-detail" class="header-scroll">
                                <table class="tbl-header-scroll">
                                  <tr>
                                        <td colspan="3">
                                            <div class="col300 c">Member</div>
                                        </td>
                                         <td colspan="3">
                                            <div class="col300 c">Agent</div>
                                        </td>
                                         <td colspan="3">
                                            <div class="col300 c">Master</div>
                                        </td>
                                         <td colspan="3">
                                            <div class="col300 c">Super</div>
                                        </td>
                                    </tr>
                                    <tr class="bgborder">
                                        <td >
                                            <div class="col100 c">Winloss</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Comm.</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Total</div>
                                        </td>
                                       <td >
                                            <div class="col100 c">Winloss</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Comm.</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Total</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Winloss</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Comm.</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Total</div>
                                        </td>
                                        <td >
                                            <div class="col100  c">Winloss</div>
                                        </td>
                                        <td >
                                            <div class="col100 c">Comm.</div>
                                        </td>
                                        <td>
                                            <div class="col100 c">Total</div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div  class="content-body clearfix">
                             <div id="body-frozen-detail" class="body-frozen">
                                <table id="tblfrozendetail" class="tbl-body-frozen">
                                       
                                </table>
                             </div>
                            <div id="body-scroll-detail" class="body-scroll">
                                <table id="tblscrolldetail" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                        <div class="content-footer" >
                            <div style="background-color:#B5C4D0; height:22px; border:1px solid #99BBE8">
                            <div id="footer-frozen-detail" class="footer-frozen">
                                <table class="tbl-footer-frozen">
                                     <tr style="display:none">
                                        <td>
                                           <div  class="col40">&nbsp;</div>
                                        </td>
                                        <td >
                                           <div class="col160">Total</div>
                                        </td>
                                        <td >
                                           <div class="col100 r">&nbsp;</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                           <div class="col100 r">&nbsp;</div>
                                        </td>
                                    </tr>
                                    <tr style="display:none">
                                        <td >
                                           <div  class="col40">&nbsp;</div>
                                        </td>
                                        <td  >
                                           <div class="col160">Total</div>
                                        </td>
                                        <td  >
                                           <div class="col100 r">&nbsp;</div>
                                        </td>
                                        <td >
                                           <div class="col100 r">&nbsp;</div>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td  style="border-bottom:none 0px;">
                                           <div  class="col40">&nbsp;</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                           <div class="col160">Total</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                           <div id="detailtotalTurnover" class="col100 r">&nbsp;</div>
                                        </td>
                                        <td  style="border-bottom:none 0px;">
                                           <div id="detailtotalComm" class="col100 r">&nbsp;</div>
                                        </td>
                                    </tr>

                                </table>
                            </div>
                            <div id="footer-scroll-detail" class="footer-scroll">
                                <table class="tbl-footer-scroll">
                                    <tr>
                                        <td >
                                            <div id="mdetailtotalwinloss" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="mdetailtotalcomm" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="mdetailtotal" class="col100 r"></div>
                                        </td>
                                        <td > 
                                            <div id="adetailtotalwinloss" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="adetailtotalcomm" class="col100 r"></div>
                                        </td>
                                        <td>
                                            <div id="adetailtotal" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="madetailtotalwinloss" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="madetailtotalcomm" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="madetailtotal" class="col100 r"></div>
                                        </td>
                                         <td>
                                            <div id="sdetailtotalwinloss" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="sdetailtotalcomm" class="col100 r"></div>
                                        </td>
                                        <td >
                                            <div id="sdetailtotal" class="col100 r"></div>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
</html>
