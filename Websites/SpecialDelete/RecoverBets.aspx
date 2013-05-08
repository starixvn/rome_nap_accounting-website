<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecoverBets.aspx.cs" Inherits="SpecialDelete_RecoverBets" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />

    <script type="text/javascript" src="../js/common.js"></script>

    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/stdlib.js" type="text/javascript"></script>
    <script src="js/RecoverBets.js" type="text/javascript"></script>

    <style type="text/css">
        input[type=checkbox]
        {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC">
                    Recover Bet
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="ErrMsg" class="errMsg">
            </div>
        </div>
        <div class="bg_table">            
            <div id="dvHeader">
                <table border="0" width="200px">
                    <tbody>
                        <tr>
                            <td class="b">
                                TransId:
                            </td>
                            <td>
                                <input type="text" onkeypress="return CheckDecNumber(event);" name="transid" id="transid"
                                    maxlength="12" size="12"/>
                            </td>
                            <td><input type="submit" value="  Search  " class="btn" onclick="getRecoverBets();" /></td>
                        </tr>                        
                    </tbody>
                </table>
            </div>    
            <br />    
            <div id="dvmsg"></div>
            <br />
            <div id="dvContent" style="display:none">
                <div id="dvMatchInfo">
                    <table style="width: 500px;" class="tblcollapse">
                        <thead>
                            <tr class="title c">
                                <th nowrap="">
                                    CODE
                                </th>
                                <th>
                                    MATCH EVENT
                                </th>
                                <th width="150">
                                    EVENT DATE
                                </th>
                            </tr>
                            </thead>
                            <tbody id="tbodyMachInfo">
                                <script type="text/html" id="tmplMatchInfo">
                                    <tr class="h3 c">
                                        <td>
                                            ${MatchCode}
                                        </td>
                                        <td nowrap="">
                                            ${HomeName} {{if AwayName != '' }} -vs- ${AwayName} {{/if}}
                                        </td>
                                        <td>
                                            ${EventDate}
                                        </td>
                                    </tr>
                                </script>
                        </tbody>
                    </table>
                </div>

                <br />
                <div id="dvBets">
                    <div id="dvHeaderBets" style="width:983px">
                        <table class="tblcollapse c content" id="Table1" style="width:963px">
                            <thead>
                            <tr style="height: 25px;" id="trHeader" class="title c">
                                <th style="width:30px">
                                    NO
                                </th>
                                <th style="width:110px">
                                    ACCOUNT
                                </th>
                                <th style="width:80px">
                                    REFNO
                                </th>
                                <th style="width:80px">
                                    BET TYPE
                                </th>
                                <th style="width:30px">
                                    BET
                                </th>
                                <th style="width:40px">
                                    ODDS
                                </th>
                                <th style="width:60px">
                                    AMOUNT
                                </th>
                                <th style="width:70px">
                                    CURRENCY
                                </th>
                                <th style="width:30px">
                                    HDP
                                </th>
                                <th style="width:120px">
                                    TRANS.TIME
                                </th>
                                <th style="width:40px">
                                    SCORE
                                </th>
                                <th style="width:110px">
                                    CREATOR
                                </th>
                                <th style="width:60px">
                                    STATUS
                                </th>     
                                <th style="width:60px">
                                    Recover
                                </th>                           
                            </tr>
                        </thead>
                        </table>
                    </div>
                    <div id="dvContentBets" style="width:983px; overflow:auto;">
                        <table class="tblcollapse c content" id="griddelete" style="width:963px">
                        <tbody id="tbodyBetsContent">
                            <script type="text/html" id="tmplBetsConent">
                                 <tr bgcolor="white" class="c h22">
                                    <td style='width:30px'>
                                        ${Order}
                                    </td>
                                    <td style='width:110px'>
                                        ${Account}
                                    </td>
                                    <td style='width:80px'>
                                        ${Refno}
                                    </td>
                                    <td style='width:80px'>
                                        ${Bettype}
                                    </td>
                                    <td style='width:30px'>
                                        <div style='width:30px; word-wrap:break-word; overflow:hidden!important;'>${Betteam}</div>
                                    </td>
                                    <td style="width:40px">
                                        <b><font color="blue">${Odds}</font><div class="bl-leaguename">${OddType}</div></b>
                                    </td>
                                    <td style="text-align: right; width:60px">
                                       <b><font color="red">${Amount}</font></b>
                                    </td>
                                    <td style='width:70px'>
                                        ${Currency}
                                    </td>
                                    <td style='width:30px'>
                                        ${Hdp}
                                    </td>
                                    <td style='width:120px'>
                                        ${TransTime}
                                    </td>
                                    <td style='width:40px'>
                                        ${Score}
                                    </td>                                   
                                    <td style='width:110px'>
                                        ${Creator}
                                    </td>
                                    <td style='width:60px'>
                                        ${Status}
                                    </td>                  
                                    <td style='width:60px'>
                                        <img class='hand' src="../App_Themes/Images/undo.png" style='border:0px' onclick='RecoverBet(${Refno});' />
                                    </td>              
                                </tr>
                            </script>
                        </tbody>
                    </table>
                    </div>
                    
                </div>
            </div>
                        
            
            <input type="hidden" id="userId" name="userId" value="<%=AppSession.AccId %>" />
        
    </div>
</body>
</html>
