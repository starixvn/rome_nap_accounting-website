<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NegativeBalance.aspx.cs"
    Inherits="Report_Deposit_NagativeBalance" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
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
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="Style/NegativeBalance.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Bubblepopup/css/jquery.bubblepopup.v2.3.1.css" rel="stylesheet"
        type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/jQuery/Template/jquery.tmpl.js"></script>
    <script type="text/javascript" src="../../js/jQuery/Template/jQuery.datalink.js"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/stdlib.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/jQuery/Bubblepopup/jquery.bubblepopup.v2.3.1.min.js" type="text/javascript"></script>
    <script src="js/NegativeBalance.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
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
                                <li class="current"><span>Negative Balance Report</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tr>
                                        <td>
                                            Licensee:
                                        </td>
                                        <td>
                                            <select id="dllSite" style="width: 120px">
                                                <option value="0">All</option>
                                            </select>
                                        </td>
                                        <td>
                                            <input id="btnReport" name="btnReport" type="button" value="REPORT" class="btn" onclick="negativeBalance.renderNegativeBalance(false);" />
                                        </td>
                                    </tr>
                                </table>
                                <div id="ErrMsg">
                                </div>
                            </div>
                        </div>
                        <div id="mainreport">
                            <div id="negativebalance" class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">
                                    </div>
                                    <div class="btngroup">
                                        <input type="button" onclick="negativeBalance.sendMail()" value="  " id="btnSendMail"
                                            class="btnSendMail" title="Send Email" />
                                        <input type="button" onclick="negativeBalance.exportExcel()" value="  " id="btnExport"
                                            class="btnExcel" title="Export to Excel" />
                                    </div>
                                    <div id="divSendMailContainer" style="display: none">
                                    </div>
                                    <script type="text/html" id="scriptSendMailTemplate">
                                    <div id="divMailList" class="content-warper">
	                                    <div class="content-header clearfix">
		                                    <div class="header-scroll">
			                                    <table class="tbl-header-scroll" >
				                                     <tr>
					                                    <td style="border-bottom:none 0px;">
						                                    <div class="col140 c">Licensees</div>
					                                    </td>
					                                    <td style="border-bottom:none 0px;">
						                                    <div class="col200 c" >To</div>
					                                    </td>
					                                    <td style="border-bottom:none 0px;">
						                                    <div class="col200 c">CC</div>
					                                    </td>                         
				                                    </tr>
			                                    </table>
		                                    </div>
	                                    </div>
	                                    <div class="content-body clearfix">
		                                    <div class="body-scroll">
			                                    <table id="tbldivMailList" class="tbl-body-scroll">
                                                {{each(i, current) mails}}
			                                     <tr>
					                                    <td >
						                                    <div class="col140">${Site}</div>
					                                    </td>
					                                    <td >
						                                    <div class="col200" >${To}</div>
					                                    </td>
					                                    <td >
						                                    <div class="col200">${negativeBalance.bindCCMail(Cc)}</div>
					                                    </td>                         
				                                    </tr>               
                                                {{/each}}     
			                                    </table>
		                                    </div>
	                                    </div>
                                    </div>
                                    <div style="margin-top: 3px">
                                        <span unselectable="on" class="textButtonTable composeHeaderButton colorBTK4" transparent="false"
                                            title="Add receiver"><span class="buttonOutter buttonOutterflatRight colorBT3 colorBTK4">
                                                <div style="vertical-align: middle; height: 100%; padding-top: 2px; padding-right: 3px">
                                                    Subject:</div>
                                            </span></span>
                                        <input class="compHeaderField inputField colorK3" id="tbxSubject" value="${Subject}"
                                            type="text" style="width: 496px;width: 496px\9" readonly />
                                    </div>
                                    <div style="margin-top: 3px">
                                        <textarea id="tbxSendMailContent" style="width: 557px; height: 150px; vertical-align: top"
                                            class="compHeaderField inputField colorK3" readonly>${Content}</textarea>
                                    </div>
                                    
                                                                
                                    </script>
                                </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col160 c">
                                                        Username</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div id="balanceSorter" class="col160 c sorter">
                                                        Balance</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Currency</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table id="tblnegativebalance" class="tbl-body-scroll">
                                        </table>
                                    </div>
                                </div>
                                <%--<div class="content-footer clearfix">
                            <div class="footer-scroll">
                                <table class="tbl-footer-scroll" id="tblFooter">                                   
                                </table>
                            </div>
                        </div>--%>
                                <div class="content-pagination bgtitle clearfix">
                                    <div class="pagination-scroll">
                                        <table style="width: 100%" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <div id="pagination">
                                                        &nbsp;</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
<script language="javascript" type="text/javascript">
    var title = '<%=Resources.Language.titleNegativeBalanceReport %>';
    var mailContent = '<%=Resources.Language.lblNagetiveBalanceReportMailContent %>';
    var mailSendby = '<% =string.Format(Resources.Language.lblSendby, AppSession.AccUserName, DateTime.Now)%>'
    var mailContentNote = '<% =Resources.Language.lblThisIsAutoMail%>'
    mailContent += (mailSendby + mailContentNote);
    var mailTitle = '<%=Resources.Language.lblNegativeBalanceReportMailTitle %>';
        
</script>
</html>
