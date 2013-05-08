<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketsListing.aspx.cs" Inherits="SpecialDelete_TicketsListing_TicketsListing"
    Async="true" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
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
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../V2/css/style.css" rel="stylesheet" type="text/css" />
    <link href="Style/TicketsListing.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../../js/stdlib.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-ui-timepicker-addon.js" type="text/javascript"></script>
    <script src="JS/ticketslisting.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <style>
        .p15
        {
            padding-left: 15px;
        }
        
        .p30
        {
            padding-left: 30px;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        var title = '<%=Resources.Language.lblRacingWinlossBySite %>'
    </script>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Tickets Listing
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width: 1113px;">
                <table>
                    <tr>
                        <td colspan="7">
                            <div id="err">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <span>Match ID: </span><span style="padding-left: 10px">
                                <input type="text" id="txtmatchid" maxlength="10" style="width: 75px" />
                            </span><span style="padding-left: 17px">Licensee: </span><span style="padding-left: 28px">
                                <select id="ddlsite" class="w120">
                                </select>
                            </span><span style="padding-left: 30px">Group by: </span><span style="padding-left: 71px">
                                <select id="ddlgroup" class="w160" onclick="ticketsListing.changeGroup();">
                                    <option value="0">Licensee and Bet type</option>
                                    <option value="1">Bet type and Licensee</option>
                                </select>
                            </span><span style="padding-left: 30px">Filter by: </span><span style="padding-left: 13px">
                                <select id="ddlSAFilter" class="w120" onclick="">
                                    <option value="0" selected="selected">All</option>
                                    <option value="1">Normal Tickets</option>
                                    <option value="2">SA Tickets</option>
                                </select>
                            </span>
                        </td>
                    </tr>
                    <tr id="typeFilterRow">
                        <td class="l b" colspan="6">
                            <span style="padding-left: 0px" class="liveindicator">Type:</span> <span style="padding-left: 25px">
                                <select class="liveindicator" id="ddlLiveIndicator" name="ddlLiveIndicator" onchange="ticketsListing.LiveindicatorChange();"
                                    style="width: 78px;">
                                    <option selected="selected" value="2">All</option>
                                    <option value="0">Dead Ball</option>
                                    <option value="1">Live Ball</option>
                                </select>
                            </span><span class='type' style='display: none; padding-left: 17px'>Live Score: Home
                                <input style='width: 20px' type='text' onkeypress="return CheckDecNumber(event);"
                                    id='txtHomeScore' maxlength='2' /></span> <span class='p15 type' style='display: none'>
                                        Away
                                        <input style='width: 20px' type='text' onkeypress="return CheckDecNumber(event);"
                                            id='txtAwayScore' maxlength='2' /></span> <span class='type' style='display: none;
                                                padding-left: 20px'>Date Time (GMT +8)</span> <span class='p15 type' style='display: none'>
                                                    <input type='checkbox' id='chkDateTime' onclick='ticketsListing.eventOnDateTime();'></span>
                            <span id="txtFromDate" class='type' style='display: none'>From
                                <input type="text" id="txtFromdate" class="txtcalendar" readonly style="cursor: pointer;" /></span>
                            <span id="txtToDate" class='p15 type' style='display: none'>To
                                <input type="text" id="txtTodate" class="txtcalendar" readonly style="cursor: pointer;" /></span>
                            <span class='p10 type' style='display: none'><span style='color: red'>(Filtering by
                                Score and Time is optional)</span></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <input type="radio" name="rdoSport" id="rdoSportBook" checked="checked" onclick="ticketsListing.changeSport('SportsBook');" />Sportsbook
                            <input type="radio" name="rdoSport" id="rdoNumberGame" onclick="ticketsListing.changeSport('NumberGame');" />Number
                            Game
                            <input type="radio" name="rdoSport" id="rdoVirtualSports" onclick="ticketsListing.changeSport('VirtualSports');" />Virtual
                            Sports
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <table class="tblcollapse hidden" id="tblbettype">
                                <thead>
                                    <tr style="background-color: #c3c3c3">
                                        <td colspan="5" class="l">
                                            <input type="checkbox" checked="checked" id="chkallbettype" onclick="ticketsListing.checkAllBettype();" />&nbsp;All
                                            Bet types
                                        </td>
                                    </tr>
                                </thead>
                                <tbody id="bodybettype">
                                </tbody>
                                <tfoot id="footbettype">
                                </tfoot>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" class="l">
                            <input id="btnReport" onclick="ticketsListing.renderData();" name="btnReport" type="button"
                                value="Show" class="btn" disabled="disabled" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="mainreport">
                <div id="matchInfo" class="hidden" style="margin-top: 6px;">
                    <table class="tblcollapse" style="width: auto">
                        <thead>
                            <tr class="title">
                                <td class="col100">
                                    CODE
                                </td>
                                <td class="col240">
                                    MATCH EVENT
                                </td>
                                <td class="col100">
                                    EVENT DATE
                                </td>
                            </tr>
                        </thead>
                        <tbody id="matchInfoContent">
                        </tbody>
                    </table>
                </div>
                <div id="tickestListing" class="content-warper hidden" style="margin-top: 6px;">
                    <div class="content-header clearfix">
                        <div id="header-scroll" class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col260 c">
                                            LICENSEE</div>
                                    </td>
                                    <td class="ibc" style="border-bottom: none 0px;">
                                        <div class="col100  c">
                                            TOTAL TRANS</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col60 c">
                                            EXPORT</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div id="body-scroll" class="body-scroll">
                            <table id="tblscroll" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                </div>
                <div id="gridbetlist" class="content-warper hidden" style="margin-top: 6px;">
                    <div class="content-toolbox bgtitle">
                        <div id="gridtitle" class="gridtitle">
                        </div>
                        <div class="btngroup">
                            <input type="button" onclick="betList.goBack();" value="  " name="btnBack" id="btnBackToMain"
                                class="btnBack" title="Back To Main" />
                            <input type="button" onclick="betList.exportExcel();" value="  " name="btnExport"
                                id="btnExport" class="btnExcel" title="Export to Excel" /></div>
                    </div>
                    <div class="content-header clearfix">
                        <div id="headerscrollbetlist" class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col100 c">
                                            Ref.no</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col120 c">
                                            Trans.Time</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col100 c">
                                            Username</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col260 c">
                                            Choice</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col80 c">
                                            Odds</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col80 c">
                                            Stake</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col80 c">
                                            WinLost</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col80 c">
                                            Currency</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col80 c">
                                            Bettype</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="col80 c">
                                            Status</div>
                                    </td>
                                    <td id="S.AColumn" style="border-bottom: none 0px;">
                                        <div class="col50 c">
                                            S.A</div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div id="bodyscrollbetlist" class="body-scroll">
                            <table id="tblbodybetlist" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                    <div id="contentPagination" class="content-pagination bgtitle clearfix">
                        <div class="pagination-scroll">
                            <table style="width: 100%" cellspacing="0" cellpadding="1">
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
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
    <br />
</body>
</html>
