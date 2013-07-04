<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditSuper.aspx.cs" Inherits="LiveCasino_PTSetting_EditSuper" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Helper" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="style/StyleSheet.css" rel="stylesheet" type="text/css" />
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
                            Update Live Casino Setting
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box" style="width:600px;" >
                <div id="err"></div>                
            </div>
            <div id="SettingContent" data-bind='template: "p2ptmpl"' ></div>
            <script id="p2ptmpl" type="text/html">
                <table class="tblcollapse w605 marginT6">
                    <tr class="title1" >
                        <td colspan="4" class="c">Customer Information</td>
                    </tr>
                    <tr>
                        <td width="120px" class="r b">Username:</td>
                        <td width="120px" class="l">${CustomerInfo.UserName}</td>
                        <td width="120px" class="r b">Nick name:</td>
                        <td width="240px" class="l">${CustomerInfo.NickName}</td>
                    </tr>
                    <tr>
                        <td class="r b">First Name:</td> 
                        <td class="l">${CustomerInfo.FirstName}</td>
                        <td class="r b">Last Name:</td>
                        <td class="l">${CustomerInfo.LastName}</td>
                    </tr>
                    <tr>
                        <td class="r b">Phone:</td>
                        <td class="l">${CustomerInfo.Phone}</td>
                        <td class="r b">Mobile Phone:</td>
                        <td class="l">${CustomerInfo.Mobile}</td>
                    </tr>
                    <tr>
                        <td class="r b">Fax:</td>
                        <td class="l">${CustomerInfo.Fax}</td>
                        <td class="r b"></td>
                        <td class="l"></td>
                    </tr>
                     <tr class="title1">
                        <td  class="l" colspan="3">Position Taking/Commission </td>
                        <td class="l"><input type="button" title="Save" class="btnUpdate" data-bind="enable: isAllowUpdatePtSetting, click: function(){editSuper.updateLiveCasino(1)}" h="${isAllowUpdatePtSetting}" /></td>
                    </tr>
                     <tr>
                        <td class="c b">Super Max Position</td>
                        <td class="c b">Super Min Position</td>
                        <td class="c b" colspan="2">Super Commission</td>
                    </tr>
                    <tr>
                        <td><select id="MaxPT" class="pt comm" data-bind="options: getPtValues(0,PTSetting.Upline.MaxPT, PTSetting.MaxPT()), value:PTSetting.MaxPT"  h="${PTSetting.MaxPT}"/></td>
                        <td><select id="MinPT" class="pt comm" data-bind="options: getPtValues(0,PTSetting.MaxPT(), PTSetting.MinPT()), value:PTSetting.MinPT"  h="${PTSetting.MinPT}"/></td>
                        <td colspan="2"><select id="Comm" class="pt comm" data-bind="options: getCommissionValues(0,PTSetting.Upline.Comm, PTSetting.Comm()), value:PTSetting.Comm" /></td>
                    </tr>
                    <tr class="title1">
                        <td  class="l" colspan="3">Member Win Limit </td>
                        <td class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateWinLimit, click: function(){editSuper.updateLiveCasino(2)}" h="${isAllowUpdateWinLimit}" /></td>
                    </tr>
                    <tr>
                        <td class="b r">Maximum Win</td>
                        <td colspan="2" class="l">
                            <select data-bind="options: WinLimitSetting.Limits, optionsValue: 'Id', optionsText: 'Text', value: WinLimitSetting.MaxWinLiveCasino"></select>
                            <span id="lblWinLimit" style="padding-left:15px">on given credit</span>
                        </td>
                        <td colspan="2">
                            <input type="checkbox" data-bind='checked:casinoManager.casinoVM.WinLimitSetting.IsAutoResetLiveCasino' h='${casinoManager.casinoVM.WinLimitSetting.IsAutoResetLiveCasino()}' >
                            <span>Daily auto reset limit (at 12 noon GMT + 8)</span>
                        </td>
                    </tr>
                    <tr class="title1">
                        <td  class="l" colspan="3">Game Setting </td>
                        <td class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateGameSetting, click: function(){editSuper.updateLiveCasino(3)}" h="${isAllowUpdateGameSetting}" /></td>
                    </tr>
                    <tr>
                        <td class="r b">Game Type</td>
                        <td colspan="4" class="l b">Betting Limit</td>
                    </tr>
                    {{each GameSetting}}
                    <tr>
                        <td class="r">${Name}</td>
                        <td colspan="3" class="l">
                            {{each BettingLimit}}
                                {{if $index % 6 == 0 && $index > 0 }}
                                    </br>
                                {{/if}}
                                <div style="word-wrap: break-word; display: inline-block; max-width: 450px;">
                                    <input name="chkBettingLimit_${Name}" type="checkbox" data-bind="checked: IsSelected" onclick='editSuper.notifyUnCheckAllBettingLimit("${Name}");' />${Text}&nbsp;&nbsp;
                                </div>
                            {{/each}}                            
                        </td>
                    </tr>
                    {{/each}}
                </table>
                    <input type="button" class="btnUpdateSetting"  value="Update" data-bind="enable: isAllowUpdate, click: function(){editSuper.updateLiveCasino(0)}, style:{display:isShowBtSave ? '':'none'}" />

            </script>
        </div>
    </div>

    <script src="../../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="script/CommonPT.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/knockout-1.2.0.js" type="text/javascript"></script>        
    <script src="script/CommonPT.js" type="text/javascript"></script>
    <script src="script/EditSuper.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">
        editSuper.superId = <%=CustId %>;
        editSuper.exchangeId = <%=ExchangeId %>;
        var userId = <%=AppSession.AccId %>;
        var isShowBtSave = <%=(Enumerations.AccountingSystem.EXTERNAL==AppSession.CurrentSystemNAME) ? "false":"true" %>; 
        var ip = '<%=Utils.GetClientIP() %>';
        editSuper.requestContext.IsAddNewSuper = <%=IsAddNew %> ;
        editSuper.msgUncheckBettingLimit = '<%= Resources.Language.UnCheckBettingLimit%>' ;
        editSuper.requestContext.SuperId =  <%=CustId %>;        
        
    </script>
</body>
</html>
