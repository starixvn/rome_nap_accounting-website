<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MatchAnalysis.aspx.cs" Inherits="Report_MatchAnalysis_MatchAnalysis" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />   
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" /> 

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="js/MatchAnalysis.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <style>
        .red 
        {
            color:red;            
         }
    </style>
    
</head>
<body>
     <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Special Match Analysis (Base on RM)
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:837px;">
                <table>
                    <tr>
                        <td class="r">
                            Report Type:
                        </td>
                        <td >
                           <select id="ddlReportType" style="width: 120px" onchange="grid.onChangeReportType()">
                            <option value="0">Currency</option>
                            <option value="1">Bet type</option>
                            <option value="2">Super</option>
                            <option value="3">Member</option>
                           </select>
                        </td>
                        <td style="width:80px" class="r">
                            Match ID:
                        </td>
                        <td>
                            <input type="text" id="txtMatchId" maxlength="10" style="width: 120px" onkeypress="return AllowNumber(event)" onkeyup="grid.allowViewReport()" />
                        </td>
                        <td style="width:80px" class="r">
                            Status:
                        </td>
                        <td>
                            <select id="ddlStatus" style="width: 80px">
                            <option value="0">All</option>
                            <option value="112">Won</option>
                            <option value="113">Lose</option>
                            <option value="111">Draw</option>
                           </select>
                        </td>
                        <td>
                            <input type="button" class="btn" onclick="grid.getData()" value="View Report" id="btnShow" disabled="disabled" />
                        </td>
                    </tr>
                   
                </table>
            </div>         
            <div id="mainreport" style="margin-top:6px">
                <div id="matchAnalysis" class="content-warper hidden">
                    <div class="content-toolbox bgtitle">
                        <div id="gridtitle" class="gridtitle"></div>
                        <div class="btngroup">
                            <input type="button" onclick="grid.exportToExcel();" value="  " name="btnExport" id="btnExport" class="btnExcel" title="Export to Excel" />
                            </div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll" id="tblHeader" >
                                <tr>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c" id="ttlreportType">Currency</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col110 c">Member Turnover</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">Member Winloss</div>
                                    </td>
                                     <td style="border-bottom:none 0px;">
                                        <div class="col100 c">Bet Count</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">House Turnover</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">House Winloss</div>
                                    </td>
                                      <td style="border-bottom:none 0px;">
                                        <div class="col100 c">SMA Turnover</div>
                                    </td>
                                    <td style="border-bottom:none 0px;">
                                        <div class="col100 c">SMA Winloss</div>
                                    </td>
                                    <td style="border-bottom:none 0px;" id="colCurrency" class="hidden">
                                        <div class="col100 c">Currency</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table id="tblContent" class="tbl-body-scroll">
                                <tbody></tbody>
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
           <%-- </div>--%>
        </div>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
</html>
