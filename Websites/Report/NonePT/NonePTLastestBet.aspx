<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NonePTLastestBet.aspx.cs" Inherits="Report_NonePT_NonePTLastestBet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    
    <link href="../../App_Themes/jquery-ui.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/ui.jqgrid.css" rel="stylesheet" type="text/css"></link>
    
    
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/>          
    <link href="../../App_Themes/BetList/BetList.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css"></link>
    <link href="../../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" /> 
    
    
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>

    <script src="../../js/common.js" type="text/javascript"></script>    
    <script src="../../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script src="../../js/pager_v2.js" type="text/javascript"></script>
    <script src="../../js/Report/NonePT/NonePTLastestBet.js" type="text/javascript"></script>    
    
    </head>
<body onload="GetCurrency();" >
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">Members Position Taking Lastest Bet Monitoring</td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                <table border="0">
                    <tr>
                        <td colspan="3" class="b">Search by:</td>
                        <td rowspan="5">
                            <input type="button" class="btn" value="Submit" onclick="GetData();" />
                        </td>
                    </tr>
                    <tr>
                        <td>Username</td>
                        <td><input type="text" style="width:80px" id="txtUsername" name="txtUsername" maxlength="50" /> </td>                        
                    </tr>    
                    <tr>
                       <td>
                            Stake: From
                        </td>            
                        <td>
                             <input type="text" style="width:80px" id="txtFromStake" name="txtFromStake" maxlength="10" onkeypress="return CheckDecNumber(event);" /> 
                            To <input type="text" style="width:80px" id="txtToStake" name="txtToStake" maxlength="10" onkeypress="return CheckDecNumber(event);" />
                        </td>            
                    </tr>               
                    <tr>
                        <td>Currency</td>
                        <td><div id="dvCurrency"></div></td>  
                    </tr> 
                    <tr>
                       <td>Bet type</td>
                       <td><input class="hand" type="checkbox" id="chkBettypes" name="chkBettypes" onclick="ShowAllBettypes();" /> </td>                       
                    </tr> 
                 
                             
                </table>
                <div id="dvBettypes"></div>
            </div>
            <div id="dvMainTable" class="bg_table" style="display:none">
                <div id="dvControl" style="width: 888px; text-align:center">
                    <span id="spReload">
                        Refresh every
                        <select id="ddlReloadTime" name="ddlReloadTime">
                            <option value="10">10 s</option> 
                            <option selected="selected" value="30">30 s</option> 
                            <option value="45">45 s</option> 
                            <option value="60">60 s</option> 
                        </select>
                        Remaining in <span id="spRemainTime" class="b">(s)</span>
                        <input class="hand" type="checkbox" id="IsPause" name="IsPause" onclick="CalcRemainTime();" /> <span id="spPause">Pause</span> 
                    </span> 
                    <span id="spPageSize" style="padding-left:50px">
                    Records per page 
                        <select id="ddlPageSize" name="ddlPageSize">
                            <option selected="selected" value="20">20</option>
                            <option value="50">50</option>
                            <option  value="100">100</option>
                            <option value="200">200</option>
                            <option value="500">500</option>
                        </select>
                    </span>
                </div>
                <div id="errMsg"></div>
                <div id="sContent"></div>                
            </div>
        </div>
    </div> 
    
</body>
</html>
