<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuperEditOnMember.aspx.cs" Inherits="LiveCasino_PTSetting_SuperEditOnMember" %>
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
                <div id="err1"></div>
            </div>
            <div id="SettingContent" data-bind='template: "p2ptmpl"' ></div>
            <script id="p2ptmpl" type="text/html">
                <table class="tblcollapse w605 marginT6">
                    <tr class="title1" >
                        <td colspan="5" class="c">Customer Information</td>
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
                        <td class="l" >${CustomerInfo.LastName}</td>
                    </tr>
                    <tr>
                        <td class="r b">Phone:</td>
                        <td class="l">${CustomerInfo.Phone}</td>
                        <td class="r b">Mobile Phone:</td>
                        <td class="l" colspan="2">${CustomerInfo.Mobile}</td>
                    </tr>
                     <tr class="title1">
                        <td  class="l" colspan="3">Position Taking/Commission </td>
                        <td class="l"><input type="button" title="Save" class="btnUpdate" data-bind="enable: isAllowUpdatePtSetting,  click: function(){superEditOnMember.updateLiveCasino(1)}" h="${isAllowUpdatePtSetting}" /></td>
                    </tr>
                     <tr>
                        <td class="c b">Super PT</td>
                        <td class="c b">Super Auto PT</td>
                        <td class="c b">Master PT</td>
                        <td class="c b">Agent PT</td>
                        
                    </tr>
                   
                    <tr>
                        {{if (PTSetting.Upline.Upline.IsAutoPresetPT)}}
                            <td><lable>${PTSetting.Upline.Upline.Upline.PresetPT()}</lable></td>
                        {{else}}
                            {{if PTSetting.Upline.Upline.Upline.PresetPT() > PTSetting.Upline.Upline.Upline.MaxPT}}
                                <td><select class="pt comm" data-bind="enable:isNotAutoReset, options: getPtValuesEdited(PTSetting.Upline.Upline.Upline.MinPT,PTSetting.Upline.Upline.Upline.MaxPT, PTSetting.Upline.Upline.Upline.PresetPT()), value:PTSetting.Upline.Upline.Upline.PresetPT" h="${PTSetting.Upline.Upline.Upline.PresetPT}" /></td>
                            {{else}}                                
                                <td><select class="pt comm" data-bind="enable:isNotAutoReset, options: getPtValues(PTSetting.Upline.Upline.Upline.MinPT,PTSetting.Upline.Upline.Upline.MaxPT, PTSetting.Upline.Upline.Upline.PresetPT()), value:PTSetting.Upline.Upline.Upline.PresetPT" h="${PTSetting.Upline.Upline.Upline.PresetPT}" /></td>
                            {{/if}}
                        {{/if}}
                        <td><input type="checkbox" data-bind="checked: PTSetting.Upline.Upline.Upline.IsAutoPresetPT" /><select class="pt comm" data-bind="enable:isAutoReset, options: getPtValues(0,PTSetting.Upline.Upline.Upline.MaxAutoPresetPT, PTSetting.Upline.Upline.Upline.AutoPresetPT()), value:PTSetting.Upline.Upline.Upline.AutoPresetPT" h="${PTSetting.Upline.Upline.Upline.AutoPresetPT}" /></td>
                        
                        <td><label>${PTSetting.Upline.Upline.PresetPT}</label></td>
                        <td><label>${PTSetting.Upline.PresetPT}</label></td>                        
                    </tr>
                    <tr class="title1">
                        <td  class="l" colspan="3">Member Win Limit </td>
                        <td class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateWinLimit,  click: function(){superEditOnMember.updateLiveCasino(2)}" h="${isAllowUpdateWinLimit}" /></td>
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
                            <span class="link" onclick="superEditOnMember.viewCurrentWinLimit();">Click to view Current Win Limit</span>
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
                        <td class="l"><input type="button" class="btnUpdate" title="Save" data-bind="enable: isAllowUpdateGameSetting,  click: function(){superEditOnMember.updateLiveCasino(3)}" h="${isAllowUpdateGameSetting}" /></td>
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
                <input type="button" class="btnUpdateSetting" value="Update" data-bind="enable: isAllowUpdate, click: function(){superEditOnMember.updateLiveCasino(0)}" />
            </script>
        </div>
    </div>

    <script src="../../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>    
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/knockout-1.2.0.js" type="text/javascript"></script>
    <script src="script/CommonPT.js" type="text/javascript"></script>
    <script src="script/SuperEditOnMember.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
     <script language="javascript" type="text/javascript">
         superEditOnMember.custId = <%=CustId %>;
         superEditOnMember.uplineId = <%=UplineId %>;
         var userId = <%=AppSession.AccId %>;
         var ip = '<%=Utils.GetClientIP() %>';
         superEditOnMember.msgUncheckBettingLimit = '<%=Resources.Language.UnCheckBettingLimitMember %>';
    </script>
</body>
</html>
