<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyWL.aspx.cs" Inherits="Report_DailyWL_DailyWL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/spliter/style.css" rel="stylesheet" type="text/css" />
    <link href="style/dailywl.css" rel="stylesheet" type="text/css" />
    
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/spliter/splitter.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="script/dailywl.js" type="text/javascript"></script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Daily WinLoss Report
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:576px;">
                <table>
                    <tr>
                        <td>
                            Date:
                        </td>
                        <td>
                            <%=strDate%>
                        </td>
                        <td>
                            <input type="button" onclick="dailyWL.ViewReport(3)" class="btn" value="REPORT" name="btnReport"
                                id="btnReport" />
                        </td>
                        <td>
                            <input type="button" onclick="dailyWL.ViewReport(1)" id="btnToday" name="Today" class="btn"/>
                        </td>
                        <td>
                            <input type="button" onclick="dailyWL.ViewReport(2)" id="btnYesterday" name="Yesterday" class="btn"/>
                        </td>
                    </tr>
                </table>
                <div id="err">
                </div>
            </div>
            <div id="splitterContainer">
	            <div id="leftPane">
                      <table class="tblcollapse" id="dailyWLRacing" style="width: 560px;">
                         <tr class="header bgtitle" style="border-bottom:1px;">
                            <td colspan="7">
                                <span class="gridtitle" id="dailyWLRacingTitle">Daily WinLoss Racing</span>
                                 <span class="btngroup">
                                    <input type="button" onclick="dailyWL.backToMain();" value="  " name="btnBack" id="btnBackToMain" class="btnBack" title="Back To Main" />
                                    <input type="button" onclick="dailyWL.ExportToExcel('1')" value="  " name="btnExport" id="Button3" class="btnExcel" title="Export to Excel" />
                                 </span>
                            </td>
                        </tr>           
                        <tr class="header title">
                            <td>
                                <div class="w80">
                                    Bettype</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Winloss</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Position</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Comm</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Line Total</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Turnover</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Margin</div>
                            </td>
                        </tr>
                    </table>
                      <table class="tblcollapse hidden" id="dailyWLBySport" style="width: 560px;">
                        <tr class="header bgtitle" style="border-bottom:solid 1px;">
                            <td colspan="7">
                                <span class="gridtitle" id="dailyWLBySportTitle"></span>
                                 <span class="btngroup">
                                    <input type="button" onclick="dailyWL.showMainReport(true);" value="  " name="btnBack" id="btnBack" class="btnBack" title="Back To Main" />
                                    <input type="button" onclick="dailyWL.ExportToExcel('2')" value="  " name="btnExport" id="btnExport" class="btnExcel" title="Export to Excel" />
                                 </span>
                            </td>
                        </tr>
                        <tr class="header title">
                            <td>
                                <div class="w80">
                                    Bettype</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Winloss</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Position</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Comm</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Line Total</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Turnover</div>
                            </td>
                            <td>
                                <div class="w80">
                                    Margin</div>
                            </td>
                        </tr>
                    </table>
	            </div>
	            <div id="rightPane">
                    <div id="gridbettype" class="content-warper hidden">
                        <div class="content-toolbox bgtitle">
                            <div id="gridtitlebettype" class="gridtitle">
                            </div>
                            <div class="btngroup">
                                    <input type="button" onclick="dailyWL.ExportToExcel('3')" value="  " name="btnExport" id="Button1" class="btnExcel" title="Export to Excel" />
                                 </div>
                        </div>
                        <div class="content-header clearfix">
                            <div id="headerscrollbettype" class="header-scroll">
                                <table class="tbl-header-scroll">
                                    <tr>
                                       <td style="border-bottom:none 0px;">
                                            <div class="w80 c">Bettype</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="w80 c">Winloss</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="w80 c">Position</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="w80 c">Comm</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="w80 c">Line Total</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="w80 c">Turnover</div>
                                        </td>
                                        <td style="border-bottom:none 0px;">
                                            <div class="w80 c">Margin</div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div id="bodyscrollbetlist" class="body-scroll">
                                <table id="tblbodyscrollbettype" class="tbl-body-scroll">
                                </table>
                            </div>
                        </div>
                        <div class="content-footer">
                            <div id="footerscrollbettype" class="footer-scroll">
                                <table class="tbl-footer-scroll" id="tblfooterscrollbettype">
                                </table>
                            </div>
                        </div>
                    </div>
	            </div>
            </div>
        </div>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <script type="text/javascript">
        var curdate = '<%=currdate %>'
        var isExpExcel = '<%=isExpExcel %>'
        var today = '<%=today %>';
        var yesterday = '<%= yesterday %>';
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        if(isExpExcel == "0"){
            dailyWL.getDailyWLRacing(curdate);
            resize_spliter()
            $("#splitterContainer").splitter({ minAsize: 10, splitVertical: true, A: $('#leftPane'), B: $('#rightPane'), closeableto: 0 });
            $(".splitbarV").addClass("hidden");
         
            $(window).resize(function () {
                resize_spliter();
            });
        }
    </script>
</body>
</html>
