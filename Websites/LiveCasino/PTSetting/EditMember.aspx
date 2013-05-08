<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditMember.aspx.cs" Inherits="LiveCasino_PTSetting_EditMember" %>
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
                <div id="err1"></div>
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
                        <td class="r b">Enable:</td>
                        <td class="l">
                            <input type='checkbox' data-bind="enable: CustomerInfo.UplineDisableLiveCasino, checked: CustomerInfo.DisableLiveCasino" h="${CustomerInfo.DisableLiveCasino}" />                            
                        </td>
                    </tr>
                     <tr class="title1">
                        <td  class="l" colspan="3">Position Taking/Commission </td>
                        <td class="l"><input type="button" title="Save" h="${isAllowUpdatePtSetting}" class="btnUpdate" data-bind="enable: isAllowUpdatePtSetting, click: function(){editMember.updateLiveCasino(1)}" /></td>
                    </tr>
                     <tr>
                        <td class="c b" colspan="2">Agent Preset PT</td>
                        <td class="c b" colspan="2">Member Commission</td>
                    </tr>
                    <tr>
                        {{if PTSetting.Upline.PresetPT()> PTSetting.Upline.MaxPT}}
                            <td colspan="2"><select class="pt comm" data-bind="options: getPtValuesEdited(PTSetting.Upline.MinPT,PTSetting.Upline.MaxPT, PTSetting.Upline.PresetPT()), value:PTSetting.Upline.PresetPT" h="${PTSetting.Upline.PresetPT}"/></td>
                        {{else}}
                            <td colspan="2"><select class="pt comm" data-bind="options: getPtValues(PTSetting.Upline.MinPT,PTSetting.Upline.MaxPT, PTSetting.Upline.PresetPT()), value:PTSetting.Upline.PresetPT" h="${PTSetting.Upline.PresetPT}"/></td>
                        {{/if}}                        
                        <td colspan="2"><select class="pt comm" data-bind="options: getCommissionValues(0,PTSetting.Upline.Comm, PTSetting.Comm()), value:PTSetting.Comm" /></td>
                    </tr>
                     <tr class="title1">
                        <td  class="l" colspan="3">Member Win Limit </td>
                        <td class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateWinLimit, click: function(){editMember.updateLiveCasino(2)}" h="${isAllowUpdateWinLimit}" /></td>
                    </tr>
                    <tr>
                        <td class="b r">Maximum Win</td>
                        <td colspan="2" class="l">
                            <select id="ddlWinLimit" data-bind="options: WinLimitSetting.Limits, optionsValue: 'Id', optionsText: 'Text', value: WinLimitSetting.MaxWinLiveCasino"></select>
                            <span id="lblWinLimit" style="padding-left:15px">on given credit</span>
                        </td>
                        <td colspan="2">
                            <input type="checkbox" data-bind='checked:casinoManager.casinoVM.WinLimitSetting.IsAutoResetLiveCasino,enable:casinoManager.casinoVM.WinLimitSetting.UplineIsAutoResetLiveCasino' h='${casinoManager.casinoVM.WinLimitSetting.IsAutoResetLiveCasino()}' >
                            <span>Daily auto reset limit (at 12 noon GMT + 8)</span>
                        </td>
                    </tr>  
                    <tr id="trClicktoViewCurrentWinLimit">
                        <td/>
                        <td colspan="2" class="l">
                            <span class="link" onclick="editMember.viewCurrentWinLimit();">Click to view Current Win Limit</span>
                        </td>
                        <td/>
                    </tr>
                    <tr id="trCurrentWinLimit" class="hide">
                        <td class="b r">Current Win Limit</td>
                        <td colspan="2" class="l">
                            <div style="width:73px;display:inline-block;"> <span id="spWinPercent" /> </div>    
                             <span id="spMaxWinCredit"/>
                        </td>
                    </tr>  
                    <tr class="title1">
                        <td  class="l" colspan="3">Game Setting </td>
                        <td class="l"><input type="button" class="btnUpdate" h="${isAllowUpdateGameSetting}" title="Save" data-bind="enable: isAllowUpdateGameSetting, click: function(){editMember.updateLiveCasino(3)}" /></td>
                    </tr>
                    <tr>
                        <td class="r b">Game Type</td>
                        <td colspan="3" class="l b">Betting Limit</td>
                    </tr>
                    
                    {{each(i, setting) GameSetting}}
                    <tr>
                        <td class="r">${Name}</td>
                        <td colspan="3" class="l">
                            {{each BettingLimit}}
                                {{if $index % 6 == 0 && $index > 0 }}
                                    </br>
                                {{/if}}
                                <div style="word-wrap: break-word; display: inline-block; max-width: 450px;">
                                    <input type="radio" name="${setting.Id}" value="${Text}" data-bind="checked:settingSelected" />${Text}&nbsp;&nbsp; 
                                </div>
                            {{/each}}
                        </td>
                    </tr>
                    {{/each}}
                </table>
                <input type="button" class="btnUpdateSetting" value="Update" data-bind="enable: isAllowUpdate, click: function(){editMember.updateLiveCasino(0)}" />
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
    <script src="script/EditMember.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        editMember.custId = <%=CustId %>;
        editMember.uplineId = <%=UplineId %>;
         var userId = <%=AppSession.AccId %>;
        var ip = '<%=Utils.GetClientIP() %>';
        editMember.msgUncheckBettingLimit = '<%=Resources.Language.UnCheckBettingLimitMember %>';
        editMember.requestContext.SuperId = <%=AppSession.SuperID %>;
        editMember.requestContext.MasterId = <%=AppSession.MasterID %>;
        editMember.requestContext.AgentId = <%=AppSession.AgentID %>;
    </script>
</body>
</html>
