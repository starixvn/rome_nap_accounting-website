<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuperUpdate.aspx.cs" Inherits="Customer_PT_Poker_SuperUpdate" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Helper" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Super</title>
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />    
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="Style/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC fixfont">
                            <%=Resources.Language.lblUpdateP2PSetting%>
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box" style="width:600px;" >
                <div id="err"></div>
            </div>
            <div id="SettingContent"  data-bind='template: "p2ptmpl"' ></div>
            <script id="p2ptmpl" type="text/html">
                <table class="tblcollapse w605 marginT6">
                    <tr class="title1" >
                        <td colspan="4" class="c">Super: ${CustInfo.UserName}</td>
                    </tr>
                    <tr>
                        <td width="25%" class="l">User Name</td>
                        <td width="25%" class="l">${CustInfo.UserName}</td>
                        <td width="25%" class="l">Screen name</td>
                        <td width="25%" class="l">${CustInfo.ScreenName}</td>
                    </tr>
                    <tr>
                        <td class="l">First Name</td> 
                        <td class="l">${CustInfo.FirstName}</td>
                        <td class="l">Last Name</td>
                        <td class="l">${CustInfo.LastName}</td>
                    </tr>
                    <tr>
                        <td class="l">Phone</td>
                        <td class="l">${CustInfo.Phone}</td>
                        <td class="l">Mobile Phone</td>
                        <td class="l">${CustInfo.MobilePhone}</td>
                    </tr>
                     <tr class="title1">
                        <td  class="l">Bet Setting </td>
                        <td colspan="3" class="l"><input type="button" title="Save" class="btnUpdate" data-bind="enable: isAllowUpdateBetSetting, click: function(){pokerManager.pokerVM.updateBetSetting(true)}" /></td>
                    </tr>
                     <tr>
                        <td class="l">Member max. transfer credit</td>
                        <td class="l"><input id="txtmaxtransfer"  data-bind="value: BetSetting.TransferCredit, valueUpdate: 'afterkeydown'" /></td>
                        <td colspan="2" class="l"><input type="checkbox" id="chkdailyautoreset" data-bind="checked: BetSetting.IsAutoReset" />Daily auto reset limit (at 12 noon GMT+8)</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="l">(${autoFormatNumber(BetSetting.MinTransferCredit)}-${autoFormatNumber(BetSetting.MaxTransferCredit)})</td>
                        <td colspan="2" class="l">(Full multiple transfer will reset max. transfer limit)</td>
                    </tr>
                    <tr class="title1">
                        <td  class="l">Game Setting </td>
                        <td colspan="3" class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateGameSetting, click: function(){pokerManager.pokerVM.updateGameSetting(false)}" /></td>
                    </tr>
                    <tr>
                        <td class="l b">Game Type</td>
                        <td colspan="3" class="l b">Stake Level</td>
                    </tr>
                    {{each GameSettings}}
                    <tr>
                        <td class="l">${Name}</td>
                        <td colspan="3" class="l">
                            {{each StakeLevels}}
                            <input type="checkbox" data-bind="checked: IsChecked" />${Name}&nbsp;&nbsp;
                            {{/each}}
                        </td>
                    </tr>
                    {{/each}}
                    <tr class="title1">
                        <td  class="l">Commission</td>
                        <td colspan="3" class="l"><input type="button" title="Save" class="btnUpdate" data-bind="enable: isAllowUpdateCommission, click: function(){pokerManager.pokerVM.updateCommission(true)}" /></td>
                    </tr>
                     <tr>
                        <td></td>
                        <td class="l">Super Comm.</td>
                        <td colspan="2"></td>
                    <tr>   
                    {{each GamesComm}}
                    <tr>
                        <td class="l">${_Name}</td>
                        <td class="l"><select class="pt comm" data-bind="options: getCommissionValues(0,MaxComm,Comm()), value:Comm" /></td>
                        <td colspan="2"></td>
                    <tr>   
                    {{/each}}                
                </table>
                  {{if(isHideUpdate == 0)}}
                     <input type="button" class="btnUpdateSetting" value="Update" data-bind="enable: isAllowUpdate, event:{click:updatePocker}" />
                {{/if}}
               
            </script>
        </div>
    </div>
    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/Number/jquery.number_format.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/Number/autoNumeric-1.4.5.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/Tpl/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/knockout-1.2.0.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../../js/Common/Functions.js" type="text/javascript"></script>
    <script src="JS/CommonPT.js" type="text/javascript"></script>
    <script src="JS/PokerData.js" type="text/javascript"></script>
    <script src="JS/PokerSetting.js" type="text/javascript"></script>
    <script type='text/javascript'>
        var custId=<%=custId%>;
        var roleId=<%=roleId%>;
        var isDisabled=<%=isDisabled%>;
        var msgDisabled='<%=msgDisabled%>';
        var currentSystemName = '<%=AppSession.CurrentSystemNAME %>';
       var isHideUpdate = (currentSystemName == '<%=Enumerations.AccountingSystem.EXTERNAL %>' ? 1 : 0); 
        $(function () {
            pokerData.getCustomerSetting(custId,roleId);
        });
    </script>
</body>
</html>
