<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyAllTransaction.aspx.cs"
    Inherits="DailyAllTransaction" Title="Untitled Page" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Daily All Txn Info</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />    
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/DailyTxn/DailyTxn.css" rel="stylesheet" type="text/css" />
   
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC"><%=sTitle%></td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:944px;">
                <table border="0px" cellpadding="0px" cellspacing="0px">
                    <tr>
                        <td class="b">Select Date:&nbsp;</td>
                        <td><%=strCalendar%></td>
                        <td class="b">&nbsp;&nbsp;Type:&nbsp;</td>
                        <td><input type="text" id="txtType" name="txtType" value="" class="txt" /></td>
                        <td class="b">&nbsp;&nbsp;Creator:&nbsp;</td>
                        <td><input type="text" id="txtCreator" name="txtCreator" value="" class="txt" />&nbsp;&nbsp;</td>
                        <td><input type="button" class="btn" onclick="DailyAllTransaction.renderDailyAllTxn(0);" value="Submit" /></td>   
                            
                    </tr>
                </table>
            </div>
          
            <div id="mainreport" style="padding-top:5px">
                <div id="DailyAllTxn" class="content-warper hidden">
                     <div class="content-toolbox bgtitle">
                            <div id="gridtitle" class="gridtitle"></div>
                            <div class="btngroup">
                                <input type="button" onclick="DailyAllTransaction.exportExcel();" value="  " name="btnExport"
                                    id="btnExport" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width: 40px" id="colReportBy">
                                            No</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width: 140px">
                                            Date</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width: 120px">
                                            User Name</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width: 300px">
                                            Description</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width: 80px;">
                                            Type</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width: 80px">
                                            Amount</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width: 150px">
                                            Creator</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table id="tblDailyAllTxn" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
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
    </div>
   <script src="../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/BlockUI.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script> 
    <script src="../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/Pager/jpager.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>

    <script src="WebResources/js/DailyAllTransactionV2.js" type="text/javascript"></script>
    <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
</body>
</html>
