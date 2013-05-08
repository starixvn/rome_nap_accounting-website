<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LatestBet.aspx.cs" Inherits="BA_LatestBet" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
    <title id="ReportTitle" runat="server"></title>
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="Style/LatestBet.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common/FlashUtils.js" type="text/javascript"></script>


    <script src="Script/websocket/modernizr-1.7.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>

    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/Tpl/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/knockout-1.2.0.js" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/common.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/gridscroller.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="Script/LatestBet.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body >
    
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Monitor Latest Betlist
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" style="width:1146px">
                <div id="info"></div>
               <div id="mainreport">
                <div id="latestbet" class="content-warper hidden">
                        <div class="content-toolbox bgtitle">
                            <span class="hidden loader"></span>
                            <div class="btngroup">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                     <td class="r" style="padding-left: 6px;">
                                            Stake
                                        </td>
                                        <td style="padding-right: 1px;">
                                            <select class="col80" onchange="LatestBet.changeStake(this)">
                                                <option value='0' selected="selected">>= 0</option>
                                                <option value='100'>>= 100</option>
                                                <option value='500'>>= 500</option>
                                                <option value='1000'>>= 1000</option>
                                                <option value='5000'>>= 5000</option>
                                                <option value='10000'>>= 10000</option>
                                                <option value='20000'>>= 20000</option>
                                            </select>
                                        </td>
                                        <td class="r" style="padding-left: 6px;">
                                            Last
                                        </td>
                                        <td style="padding-right: 1px;">
                                            <select class="col50" onchange="LatestBet.changeDisplay(this)">
                                                <option value='10'>10</option>
                                                <option value='20'>20</option>
                                                <option value='30' selected="selected">30</option>
                                                <option value='40'>40</option>
                                                <option value='50'>50</option>
                                                <option value='60'>60</option>
                                                <option value='70'>70</option>
                                                <option value='80'>80</option>
                                                <option value='90'>90</option>
                                                <option value='100'>100</option>
                                            </select>
                                        </td>
                                        <td class="r" style="padding-left: 6px;">
                                            Auto Refresh
                                        </td>
                                        <td>
                                            <input type="checkbox" id="chkAutoRefresh" checked="checked" onchange="LatestBet.ToggleAutoRefresh();" />
                                        </td>
                                        <td class="r" style="padding-left: 6px;">
                                            Duration
                                        </td>
                                        <td style="padding-right: 1px;">
                                            <select class="col50" onchange="LatestBet.changeDuration(this)">
                                                <option value='2000'>2s</option>
                                                <option value='3000'>3s</option>
                                                <option value='4000'>4s</option>
                                                <option value='5000'>5s</option>
                                            </select>
                                        </td>
                                        <td style="padding-left: 6px;">
                                        <input type="button" value="  " id="btnRefresh" class="btnRefresh" title="Refresh" onclick="LatestBet.manualRefresh();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-header clearfix">
                            <div class="header-scroll">
                                <table class="tbl-header-scroll">
                                    <tr>
                                        <td style="border-bottom: none 0px;">
                                            <div class="col100 c">
                                                Ref.No</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="col120 c">
                                                Username</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="col260 c">
                                                Bet - HDP/Score</div>
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
                                            <div class="col60 c">
                                                Currency</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="col100 c">
                                                Bettype</div>
                                        </td>
                                        <td style="border-bottom: none 0px;">
                                            <div class="col300 c">
                                                Event</div>
                                        </td>                                                                    
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="content-body clearfix">
                            <div class="body-scroll" id="latestbetcontent">
                                <table class="tbl-body-scroll" >
                                    <tbody id="tblatestbet" data-bind='template: { name: "itemtpl", foreach: items}'></tbody>
                                </table>
                                <script id="itemtpl" type="text/html">
                                    <tr class="{{if Liveindicator}}event-live{{/if}}" >
                                      <td><div class="col100 c" title="${TransDate}">${TransId}</div></td>
                                      <td><div class="col120 c">${UserName}</div></td>
                                      <td>
                                        <div class="col260 r">
                                            <span class="bl-choice b" data-bind="html: BetTeam"></span>
                                            <span class="bl-bettype b" data-bind="html: Hdp"></span>
                                            <span class="bl-choice" data-bind="html: LiveScore"></span>
                                          </div> 
                                      </td>
                                      <td><div class="col80 r b {{if parseInt(Odds)<0}}red{{/if}}" >${Odds} {{if OddName!=''}}<span class="bl_oddtype">(${OddName})</span>{{/if}}</div></td>
                                      <td><div class="col80 r bl-choice b">${Stake}</div></td>
                                      <td><div class="col60 c">${CurrencyName}</div></td>
                                      <td><div class="col100 c" data-bind="html: BetType"></div></td>
                                      <td><div class="col300 c" >${Event}</div></td>
                                   </tr>
                                </script>
                            </div>
                        </div>
                        
                    </div>
                    <div id="mpwarper" class="hidden r"></div>
                </div>
            </div>
          
          </div>
        
           
        </div>
    <%--<script src="Script/websocket/swfobject.js" type="text/javascript"></script>
  	<script src="Script/websocket/web_socket.js" type="text/javascript"></script>
	<script src="Script/websocket/alchemy-client.js" type="text/javascript"></script> 
	<script src="Script/websocket/startup.js" type="text/javascript"></script>--%>
</body>
</html>
