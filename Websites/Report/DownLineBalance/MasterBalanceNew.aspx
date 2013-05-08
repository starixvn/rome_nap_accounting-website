<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterBalanceNew.aspx.cs"
    Inherits="Report_DownLineBalance_MasterBalanceNew" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Styles/downlincebalance.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="js/MasterBalance.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body>
    <div class="main" style="">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Master Balance</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent" style="">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tbody>
                                        <tr>
                                            <!--ReportType-->
                                            <td>
                                                <input type="radio" id="alltrans" value="all" name="type" onclick="masterBalance.isYesterday = 0" />Total
                                                Balance|<input type="radio" checked="checked" id="yestrans" value="yes" name="type"
                                                    onclick="masterBalance.isYesterday = 1" />Yesterday's Balance
                                            </td>
                                            <!--/ReportType-->
                                            <td>
                                            </td>
                                        </tr>
                                        <!--db-->
                                        <%--<tr>
                            <td>
                                GET DATA FROM<input type="radio" onclick="IsRep='0';" value="Main" name="db" id="dbMain" />MAIN<input
                                    type="radio" onclick="IsRep='1';" checked="checked" value="Rep" name="db" id="dbRep" />REPLICATION
                            </td>
                            <!-- <td><input type ="hidden" name ="custid" value ="{custid}"></input><input type="hidden" id="isYesterday" name="isYesterday" value="{isYesterday}" /></td>-->
                        </tr>--%>
                                        <!--/db-->
                                    </tbody>
                                </table>
                                <span class="errMsg" id="errMsg"></span>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <font class="b">Keyword</font>
                                            </td>
                                            <td>
                                                <input type="text" onkeypress="return masterBalance.onKeyPress(event);" onblur="masterBalance.onBlur();"
                                                    onfocus="masterBalance.onFocus();" value="All" id="txtSearch" name="txtSearch"
                                                    style="width: 120px;" />
                                            </td>
                                            <td>
                                                <font class="b">First Letter</font>
                                            </td>
                                            <td>
                                                <select id="listAlpha" name="listAlpha" class="Sel60" style="width: 65px;">
                                                    <option value="All">All</option>
                                                    <option value="0-9">0-9</option>
                                                    <option value="A">A</option>
                                                    <option value="B">B</option>
                                                    <option value="C">C</option>
                                                    <option value="D">D</option>
                                                    <option value="E">E</option>
                                                    <option value="F">F</option>
                                                    <option value="G">G</option>
                                                    <option value="H">H</option>
                                                    <option value="I">I</option>
                                                    <option value="J">J</option>
                                                    <option value="K">K</option>
                                                    <option value="L">L</option>
                                                    <option value="M">M</option>
                                                    <option value="N">N</option>
                                                    <option value="O">O</option>
                                                    <option value="P">P</option>
                                                    <option value="Q">Q</option>
                                                    <option value="R">R</option>
                                                    <option value="S">S</option>
                                                    <option value="T">T</option>
                                                    <option value="U">U</option>
                                                    <option value="V">V</option>
                                                    <option value="W">W</option>
                                                    <option value="X">X</option>
                                                    <option value="Y">Y</option>
                                                    <option value="Z">Z</option>
                                                </select>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="hidden" value="{lfrom}" id="bycom" name="bycom" /><font class="b">Currency</font>
                                            </td>
                                            <td>
                                                <select id="listCurrency" name="listCurrency" class="Sel60" style="width: 124px;">
                                                </select>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <input type="button" onclick="masterBalance.getMasterBalanceReport();" value="REPORT"
                                                    id="btnGo" name="btnGo" class="btn" style="width: 65px;" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div class="bg_table" style="width: 615px;">
                                <div id="masterBalanceGrid" class="content-warper hidden">
                                    <div class="content-toolbox bgtitle">
                                        <div id="masterBalanceTitle" class="gridtitle">
                                        </div>
                                        <div class="btngroup">
                                            <input type="button" onclick="masterBalance.exportExcel();" value="  " id="btnMasterExport"
                                                class="btnExcel" title="Export to Excel" />
                                        </div>
                                    </div>
                                    <div class="content-header clearfix">
                                        <div class="header-scroll">
                                            <table class="tbl-header-scroll">
                                                <tr>
                                                    <td colspan="5">
                                                        <div class="c">
                                                            Master Agent</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col40 c">
                                                            No</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col200 c">
                                                            Master</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col60 c">
                                                            Curr</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col140 c">
                                                            Balance</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col140 c">
                                                            Base(RM)</div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-body clearfix">
                                        <div class="body-scroll">
                                            <table id="tbMasterBalance" class="tbl-body-scroll">
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-footer">
                                        <div class="footer-scroll">
                                            <table class="tbl-footer-scroll" id="tbFooterMasterBalance">
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div id="agentBalanceGrid" class="content-warper hidden">
                                    <div class="content-toolbox bgtitle">
                                        <div id="agentBalanceTitle" class="gridtitle">
                                        </div>
                                        <div class="btngroup">
                                            <input type="button" onclick="agentBalance.goBack()" value="  " id="btnAgentBack"
                                                class="btnBack" title="Go Back" />
                                            <input type="button" onclick="agentBalance.exportExcel();" value="  " id="btnAgentExport"
                                                class="btnExcel" title="Export to Excel" />
                                        </div>
                                    </div>
                                    <div class="content-header clearfix">
                                        <div class="header-scroll">
                                            <table class="tbl-header-scroll">
                                                <tr>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col40 c">
                                                            No</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col200 c">
                                                            Agent</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col60 c">
                                                            Curr</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col140 c">
                                                            Balance</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col140 c">
                                                            Base(RM)</div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-body clearfix">
                                        <div class="body-scroll">
                                            <table id="tbAgentBalance" class="tbl-body-scroll">
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-footer">
                                        <div class="footer-scroll">
                                            <table class="tbl-footer-scroll" id="tbFooterAgentBalance">
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div id="memberBalanceGrid" class="content-warper hidden">
                                    <div class="content-toolbox bgtitle">
                                        <div id="memberBalanceTitle" class="gridtitle">
                                        </div>
                                        <div class="btngroup">
                                            <input type="button" onclick="memberBalance.goBack()" value="  " id="btnMemberBack"
                                                class="btnBack" title="Go Back" />
                                            <input type="button" onclick="memberBalance.exportExcel();" value="  " id="btnMemberExport"
                                                class="btnExcel" title="Export to Excel" />
                                        </div>
                                    </div>
                                    <div class="content-header clearfix">
                                        <div class="header-scroll">
                                            <table class="tbl-header-scroll">
                                                <tr>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col40 c">
                                                            No</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col200 c">
                                                            Agent</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col60 c">
                                                            Curr</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col140 c">
                                                            Balance</div>
                                                    </td>
                                                    <td style="border-bottom: none 0px;">
                                                        <div class="col140 c">
                                                            Base(RM)</div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-body clearfix">
                                        <div class="body-scroll">
                                            <table id="tblMemberBalance" class="tbl-body-scroll">
                                            </table>
                                        </div>
                                    </div>
                                    <div class="content-footer">
                                        <div class="footer-scroll">
                                            <table class="tbl-footer-scroll" id="tblFooterMemberBalance">
                                            </table>
                                        </div>
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
</html>
