<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExchangeRate1.aspx.cs" Inherits="ExchangeRate"
    Title="Untitled Page" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/image.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/Bubblepopup/css/jquery.bubblepopup.v2.3.1.css" rel="stylesheet" type="text/css" />
    <link href="../js/ToolTip/Tipsy-Tooltip/tipsy.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/jQuery/Bubblepopup/jquery.bubblepopup.v2.3.1.min.js" type="text/javascript"></script>
    <script src="../js/ToolTip/Tipsy-Tooltip/jquery.tipsy.js" type="text/javascript"></script>
    <%-- <script src="../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>
     <script type="text/javascript" src="../js/ToolTip/wz_tooltip/tip_balloon.js"></script>--%>
    <script type="text/javascript" src="../js/ExChangeRate.js?ver=<%=WebSettings.Version%>"></script>
    <script type="text/javascript" src="../js/common.js?ver=<%=WebSettings.Version%>"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../js/jQuery/ajaxfileupload.js?v=8" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="Script/ExchangeRate1.js?ver=6" type="text/javascript"></script>
   
    <script type="text/javascript">
        function ExportToExcel()
        {
            $('#IsExport').val('1');
            $('#form').submit();
        }
    </script>
    <style>
        a
        {
            color: #000000;
            outline: 0 none;
            text-decoration: none;
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
                    <td class="maincontent_headerC fixfont">
                        <asp:Literal ID="ExRateHeaderStr" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 650px;">
                <div id="ErrMsg">
                </div>
                <asp:Literal ID="ExRateAddContentHtml" runat="server"></asp:Literal>
            </div>
            <div id="sContent" style="margin-top: 5px">
                <div id="divExhangeRate" class="content-warper" style="width: 654px">
                    <div class="content-toolbox bgtitle">
                        <div id="gridtitle" class="gridtitle">Exchange Rate</div>
                        <div class="btngroup">
                            <input type="button" onclick="ExportToExcel();" value="  " name="btnExport" id="btnExport"
                                class="btnExcel" title="Export to Excel"></div>
                    </div>
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td rowspan="2" style="border-bottom: none 0px;">
                                        <div class="c" style="width:80px">
                                            Currency</div>
                                    </td>
                                    <td rowspan="2" style="border-bottom: none 0px;">
                                        <div class="c" style="width:66px">
                                            Standard Code</div>
                                    </td>
                                    <td rowspan="2" style="border-bottom: none 0px;">
                                        <div class="c" style="width:72px">
                                            Base Rate</div>
                                    </td>
                                    <td rowspan="2" style="border-bottom: none 0px;">
                                        <div class="c" style="width:73px">
                                            Min Bet</div>
                                    </td>
                                    <td rowspan="2" style="border-bottom: none 0px;">
                                        <div class="c" style="width:73px">
                                            Min Bet Live</div>
                                    </td>
                                    <td colspan="5">
                                        <div class="c">
                                            Open Tote Rate</div>
                                    </td>
                                    <td rowspan="2" style="border-bottom: none 0px;">
                                        <div class="c" style="width:48px">
                                            Edit</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width:65px">
                                            GBP</div>
                                    </td>
                                    <%--<td style="border-bottom: none 0px;">
                                        <div class="c" style="width:64px">
                                            AUD</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width:64px">
                                            ZAR</div>
                                    </td>--%>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width:64px">
                                            EURO</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="c" style="width:63px">
                                            US$</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix" style="border-bottom:0px none">
                        <div class="body-scroll">
                            <table id="tblmixparlay" class="tbl-body-scroll" style="display:none">
                                <asp:Literal ID="ExRateListContentHtml" runat="server"></asp:Literal>
                                
                            </table>
                        </div>
                    </div>
                </div>
                <br />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        InitFixedHeaderEx();
        
    </script>
    <form id="form" method="post" action="ExchangeRate1.aspx">
    <input type="hidden" id="IsExport" name="IsExport" value="0" />
    </form>
</body>
</html>
