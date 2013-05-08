<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAgent.aspx.cs" Inherits="LiveCasino_PTSetting_EditAgent" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Helper" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
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
            <div class="maincontent_box" style="width: 600px;">
                <div id="err">
                </div>
            </div>
            <div id="SettingContent" data-bind='template: "p2ptmpl"'>
            </div>
            <script id="p2ptmpl" type="text/html">
                <table class="tblcollapse w605 marginT6">
                    <tr class="title1" >
                        <td colspan="5" class="c">Customer Information</td>
                    </tr>
                    <tr>
                        <td width="120px" class="r b">Username:</td>
                        <td width="120px" class="l">${CustomerInfo.UserName}</td>
                        <td width="120px" class="r b">Nick name:</td>
                        <td width="240px" class="l" colspan="2">${CustomerInfo.NickName}</td>
                    </tr>
                    <tr>
                        <td class="r b">First Name:</td> 
                        <td class="l">${CustomerInfo.FirstName}</td>
                        <td class="r b">Last Name:</td>
                        <td class="l" colspan="2">${CustomerInfo.LastName}</td>
                    </tr>
                    <tr>
                        <td class="r b">Phone:</td>
                        <td class="l">${CustomerInfo.Phone}</td>
                        <td class="r b">Mobile Phone:</td>
                        <td class="l" colspan="2">${CustomerInfo.Mobile}</td>
                    </tr>
                    <tr>
                        <td class="r b">Fax:</td>
                        <td class="l">${CustomerInfo.Fax}</td>
                        <td class="r b"></td>
                        <td class="l" colspan="2"></td>
                    </tr>
                     <tr class="title1">
                        {{if (!PTSetting.Upline.IsDisabledAutoPresetPT)}}
                        <td  class="l" colspan="4">Position Taking/Commission </td>
                        {{else}}
                        <td  class="l" colspan="3">Position Taking/Commission </td>
                        {{/if}}
                        <td class="l"><input type="button" title="Save" class="btnUpdate" data-bind="enable: isAllowUpdatePtSetting, click: function(){editAgent.updateLiveCasino(1)}" h="${isAllowUpdatePtSetting}" /></td>
                    </tr>
                     <tr>
                        <td class="c b">Master Preset PT</td>
                        {{if (!PTSetting.Upline.IsDisabledAutoPresetPT)}}
                        <td class="c b">Master Auto Preset PT</td>
                        {{/if}}
                        <td class="c b">Agent Max PT</td>
                        <td width="120px" class="c b">Agent Min PT</td>
                        <td width="120px" class="c b">Agent Comm</td>
                    </tr>
                    <tr>
                        <td><select class="pt comm" data-bind="enable:isNotAutoReset, options: getPtValues(0,PTSetting.Upline.MaxPT, PTSetting.Upline.PresetPT()), value:PTSetting.Upline.PresetPT" v='${PTSetting.Upline.PresetPT}' /></td>
                        {{if (!PTSetting.Upline.IsDisabledAutoPresetPT)}}
                        <td><input type="checkbox" data-bind="checked: PTSetting.Upline.IsAutoPresetPT" /><select class="pt comm" data-bind="enable:isAutoReset, options: getPtValues(0,PTSetting.Upline.MaxPT, PTSetting.Upline.AutoPresetPT()), value:PTSetting.Upline.AutoPresetPT"  k='${PTSetting.Upline.AutoPresetPT}'/></td>
                        {{/if}}

                        {{if (isNotAutoReset) }}
                            <td><select class="pt comm" data-bind="options: getPtValues(PTSetting.MinPT(), roundNumber(PTSetting.Upline.MaxPT - PTSetting.Upline.PresetPT(), 2), PTSetting.MaxPT()), value:PTSetting.MaxPT" mx='${PTSetting.MaxPT}'/></td>
                        {{else}}
                            <td><select class="pt comm" data-bind="options: getPtValues(PTSetting.MinPT(), roundNumber(PTSetting.Upline.MaxPT - 0, 2), PTSetting.MaxPT()), value:PTSetting.MaxPT" mx='${PTSetting.MaxPT}'/></td>
                        {{/if}}
                        
                        <td><select class="pt comm" data-bind="options: getPtValues(0,PTSetting.MaxPT(), PTSetting.MinPT()), value:PTSetting.MinPT"  mn='${PTSetting.MaxPT}'/></td>
                        <td><select class="pt comm" data-bind="options: getCommissionValues(0,PTSetting.Upline.Comm, PTSetting.Comm()), value:PTSetting.Comm"  c='${PTSetting.Upline.AutoPresetPT}' /></td>
                    </tr>
                      <tr class="title1">
                        <td  class="l" colspan="3">Member Win Limit </td>
                        <td/>
                        <td class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateWinLimit, click: function(){editAgent.updateLiveCasino(2)}" h="${isAllowUpdateWinLimit}" /></td>
                    </tr>
                    <tr>
                        <td class="b r">Maximum Win</td>
                        <td colspan="2" class="l">
                            <select data-bind="options: WinLimitSetting.Limits, optionsValue: 'Id', optionsText: 'Text', value: WinLimitSetting.MaxWinLiveCasino"></select>
                            <span id="lblWinLimit" style="padding-left:15px">on given credit</span>
                        </td>
                        <td colspan="2">
                            <input type="checkbox" data-bind='checked:casinoManager.casinoVM.WinLimitSetting.IsAutoResetLiveCasino,enable:casinoManager.casinoVM.WinLimitSetting.UplineIsAutoResetLiveCasino' h='${casinoManager.casinoVM.WinLimitSetting.IsDailyReset()}' >
                            <span>Daily auto reset limit (at 12 noon GMT + 8)</span>
                        </td>
                    </tr>  
                    <tr class="title1">
                        {{if (!PTSetting.Upline.IsDisabledAutoPresetPT)}}
                        <td  class="l" colspan="4">Game Setting </td>
                        {{else}}
                        <td  class="l" colspan="3">Game Setting </td>
                        {{/if}}
                        <td class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateGameSetting, click: function(){editAgent.updateLiveCasino(3)}" h="${isAllowUpdateGameSetting}" /></td>
                    </tr>
                    <tr>
                        <td class="r b">Game Type</td>

                        <td colspan="4" class="l b">Betting Limit</td>
                    </tr>
                    {{each GameSetting}}
                    <tr>
                        <td class="r">${Name}</td>
                        <td colspan="4" class="l">
                            {{each BettingLimit}}
                                {{if $index % 6 == 0 && $index > 0 }}
                                    </br>
                                {{/if}}
                                <div style="word-wrap: break-word; display: inline-block; max-width: 450px;">
                                   <input name="chkBettingLimit_${Name}" type="checkbox" data-bind="checked: IsSelected" onclick='editAgent.notifyUnCheckAllBettingLimit("${Name}");' />${Text}&nbsp;&nbsp;
                                </div>
                            {{/each}}
                        </td>
                    </tr>
                    {{/each}}
                </table>
                <input type="button" class="btnUpdateSetting" value="Update" data-bind="enable: isAllowUpdate, click: function(){editAgent.updateLiveCasino(0)}" />
            </script>
        </div>
    </div>
    <div id="dialogMemberPT" style="display: none;">
        <table border="0px">
            <tr>
                <td>
                    You have changed this Agent not to take auto position taking.
                </td>
            </tr>
            <tr>
                <td>
                    Do you want to update all member's position taking to:
                </td>
            </tr>
            <tr>
                <td>
                    <select id="MemberPT" class="pt">
                    </select>
                </td>
            </tr>
        </table>
    </div>
    <script src="../../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="script/CommonPT.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/knockout-1.2.0.js" type="text/javascript"></script>
    <script src="script/EditAgent.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        editAgent.custId = <%=CustId %>;
        editAgent.uplineId = <%=UplineId %>;
        var userId = <%=AppSession.AccId %>;
        var ip = '<%=Utils.GetClientIP() %>';
        editAgent.msgUncheckBettingLimit = '<%= Resources.Language.UnCheckBettingLimit%>' ;
        editAgent.requestContext.SuperId = <%=AppSession.SuperID %>;
        editAgent.requestContext.MasterId = <%=AppSession.MasterID %>;
        editAgent.requestContext.AgentId = <%=AppSession.AgentID %>;
    </script>
</body>
</html>
