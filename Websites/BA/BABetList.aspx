<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BABetList.aspx.cs" Inherits="BA_BABetList" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <title id="ReportTitle" runat="server"></title>
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Style/LatestBet.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>

    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>
    <script src="../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../js/stdlib.js" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../Customer/PT/Racing/JS/CommonPT.js" type="text/javascript"></script>
    <script src="Script/BABetList.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body >
    
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        BA Bet List 
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:1110px">
                <div style="display: none;" id="trMsg">
                    <div id="dvMsg">
                    </div>
                </div>
                <div style="padding-bottom:2px">
                    <span style="font-weight:bold"><input id="rdTransID" type="radio" onclick="LatestBet.BL_changeOption();" value="1" name="rdb" /><label for="rdTransID">Ref No</label></span>
                    <span style="padding-left:17px;"><input id="txtTransID" type="text" maxlength="13" style="width:120px" name="txtTransID" disabled="disabled" onkeyup="javascript:onKeyEnter(event);"  /></span>
                </div>
                <div style="padding-bottom:4px">
                    <span style="font-weight:bold;"><input id="rdBetType" type="radio" onclick="LatestBet.BL_changeOption();" checked="checked" value="2" name="rdb" /><label for="rdBetType">Advance</label></span>
                </div>
                <div style="padding-left:15px" class="content">
                    <table id="tblAdvance">
                        <tr>
                            <td style="width:60px; font-weight:bold; text-align:right">Username</td>
                            <td><input type="text" id="txtUsername" style="width:268px"/></td>
                            <td style="font-weight:bold; width:60px; text-align:right">Currency</td>
                            <td>
                                <select id="dlCurrency" style="width:100px">   
                                    <option value="0">All</option>
                                </select>
                            </td>
                            <td style="font-weight:bold; width:60px; text-align:right">Bet type</td>
                            <td>
                                 <select id="dlBettype" style="width:180px">
                                    <option value="0">All</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight:bold; text-align:left; padding-left:7px">Stake</td>
                            <td>
                                <span style="font-weight:bold; width:50px">From</span>
                                <span><input type="text" id="txtfromstake" style="width:104px" /></span>
                                <span style="font-weight:bold; padding-left:5px; width:30px">To</span>
                                <span><input type="text" id="txttostake" style="width:104px;" /></span>
                            </td>
                            <td colspan="4">
                                <span style="padding-left:20px"><input type="checkbox" id="cbExclude" /></span>
                                <span style="padding-right:2px; font-weight:bold"><label for = "cbExclude" />Exclude test currency (UUS)</span>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="font-weight:bold; text-align:left; padding-left:7px">Date</td>
                            <td>
                                <span style="font-weight:bold; width:50px">From</span>
                                <span><input id="txtfromdate" class="txtdate" readonly="readonly" /></span>
                                <span style="font-weight:bold; padding-left:5px; width:30px">To</span>
                                <span><input id="txttodate" class="txtdate" readonly="readonly" /></span>
                            </td>
                            <td colspan="4" style="padding-left:40px">
                                <span><input id="Button1" name="btnReport" type="button" value="SUBMIT" class="btn" onclick="LatestBet.viewBetList();" /></span>
                            </td>
                        </tr>
                        
                    </table>
                </div>
                <%--<div style="text-align:center">
                    <span><input id="btnReport" name="btnReport" type="button" value="SUBMIT" class="btn" onclick="LatestBet.viewBetList();" /></span>
                </div>--%>
               <div id="mainreport" style="margin-top:2px;">
                <div id="latestbet" class="content-warper hidden">
                        <div class="content-toolbox bgtitle">
                             <div id="gridtitle" class="gridtitle"></div>   
                             <div class="btngroup">
                                <input type="button" value="  " id="btnRefresh" class="btnRefresh" title="Refresh" onclick="LatestBet.manualRefresh();" />
                             </div>                      
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll">
                                    <tr>
                                        <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col40 c">
                                            #</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col100 c">
                                            Member
                                            </div>
                                        </td>
                                        <td rowspan="2" style="border-bottom: none 0px;">
                                            <div class="col100 c">
                                                Trans.Time</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom: none 0px;">
                                            <div class="col360 c">
                                                Choice</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom: none 0px;">
                                            <div class="col60 c">
                                                Odds</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom: none 0px;">
                                            <div class="col60 c">
                                                Stake</div>
                                        </td>
                                         <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col100 c">Win Loss</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col100 c">Licensee Comm</div>
                                        </td>
                                        <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col60 c">Virtual</div>
                                        </td>
                                         <td rowspan="2" style="border-bottom: none 0px;">
                                            <div class="col100 c">
                                                Status</div>
                                        </td>
                                            <td rowspan="2" style="border-bottom:none 0px;">
                                            <div class="col60 c">Currency</div>
                                        </td>                                                                  
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll" id="latestbetcontent">
                                <table class="tbl-body-scroll" >
                                    <tbody id="tblatestbet"></tbody>
                                </table>
                            </div>
                        </div>
                        <div id="contentPagination" class="content-pagination bgtitle clearfix">
                            <div class="pagination-scroll">
                                <table style="width:100%" cellspacing="0" cellpadding="1">
                                    <tr>
                                        <td> <div id="pagination">&nbsp;</div></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                   <div id="mpwarper" class="hidden r"></div>
                   <div id="dialogCombinationDetailMP" class="hidden">
                       <div id="gridcombination" class="content-warper ">
                           <div class="content-header clearfix">
                               <div id="headerscrollbetlist" class="header-scroll">
                                   <table class="tbl-header-scroll">
                                       <tr>
                                           <td class="col100 c" style="border-bottom: none 0px;">
                                               <div class="col100 c">
                                                   Date</div>
                                           </td>
                                           <td class="col200 c" style="border-bottom: none 0px;">
                                               <div class="col200  c">
                                                   Event</div>
                                           </td>
                                           <td class="col100 c" style="border-bottom: none 0px;">
                                               <div class="col100  c">
                                                   Odds</div>
                                           </td>
                                           <td class="col100 c" style="border-bottom: none 0px;">
                                               <div class="col100 c">
                                                   Stake</div>
                                           </td>
                                           <td class="col100 c" style="border-bottom: none 0px;">
                                               <div class="col100 c">
                                                   Winloss</div>
                                           </td>
                                           <td class="col100 c" style="border-bottom: none 0px;">
                                               <div class="col100 c">
                                                   Status</div>
                                            </td>
                                       </tr>
                                   </table>
                               </div>
                           </div>
                           <div class="content-body clearfix">
                               <div id="bodyscrollcombination" class="body-scroll">
                                   <table id="tblbodyscrollcombination" class="tbl-body-scroll">
                                   </table>
                               </div>
                           </div>
                           <div class="content-footer">
                               <div id="scrolltotalWLComm" class="footer-scroll">
                                   <table class="tbl-footer-scroll c" id="totalWLComm">
                                   </table>
                               </div>
                           </div>
                       </div>
                   </div>
                </div>
            </div>
          </div>
        </div>
       
</body>
</html>
