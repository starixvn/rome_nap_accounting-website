<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateRacebooksStatus.aspx.cs" Inherits="Customer_v2_Racing_UpdateRacebooksStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Racing Status</title>
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <style>
           .marginT6 {
            margin-top: 6px;
            }
    </style>
</head>
<body>
    <div class="main">
        <div class="maincontent_box" style="width:669px;" >
            <div id="err"></div>
            <div id="SettingContent">
                <div class="">
                    <table class="tblcollapse w1000 marginT6">
                        <tbody>
                            <tr class="title1">
                                <td class="l">
                                    <span class="expanded" target="setting_151">Racing</span>
                                </td>
                            </tr>
                            <tr id="setting_151">
                                <td>
                                    <div>
                                        <div class="">
                                            <table cellspacing="0" cellpadding="0" border="0" class="tblPTheader">
                                                <tbody>
                                                    <tr id="btntop" style="cursor: pointer;">
                                                        <td class="btnleft">
                                                        </td>
                                                        <td width="120" class="l btnmiddle" target="setting_151_41">
                                                            <span class="mp">UK/Irish Tote</span>
                                                        </td>
                                                        <td width="20" class="setting_151_41 r  md2">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="151_41_errmsg" class="w800 errmsg">
                                            </div>
                                            <div id="setting_151_41" class="l" style="">
                                                <table class="tblcollapse w800">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="3">
                                                                Enable:&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" checked="true" value="true"
                                                                    id="151_41_chkEnable">&nbsp;Super &nbsp;&nbsp;&nbsp;<input type="checkbox" id="151_41_chkEnableDownline"
                                                                        value="false" disabled="disabled">&nbsp;Super's downlines
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="">
                                            <table cellspacing="0" cellpadding="0" border="0" class="tblPTheader">
                                                <tbody>
                                                    <tr id="btntop" style="cursor: pointer;">
                                                        <td class="btnleft">
                                                        </td>
                                                        <td width="120" class="l btnmiddle" target="setting_151_31">
                                                            <span class="mp">SA Tote</span>
                                                        </td>
                                                        <td width="20" class="setting_151_31 r  md3">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="151_31_errmsg" class="w800 errmsg">
                                            </div>
                                            <div id="setting_151_31" class="l" style="display: none;">
                                                <table class="tblcollapse w800">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="3">
                                                                Enable:&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" value="false" id="151_31_chkEnable">&nbsp;Super
                                                                &nbsp;&nbsp;&nbsp;<input type="checkbox" id="151_31_chkEnableDownline" value="false"
                                                                    disabled="disabled">&nbsp;Super's downlines
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="">
                                            <table cellspacing="0" cellpadding="0" border="0" class="tblPTheader">
                                                <tbody>
                                                    <tr id="btntop" style="cursor: pointer;">
                                                        <td class="btnleft">
                                                        </td>
                                                        <td width="120" class="l btnmiddle" target="setting_151_51">
                                                            <span class="mp">AU Tote</span>
                                                        </td>
                                                        <td width="20" class="setting_151_51 r  md3">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="151_51_errmsg" class="w800 errmsg">
                                            </div>
                                            <div id="setting_151_51" class="l" style="display: none;">
                                                <table class="tblcollapse w800">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="3">
                                                                Enable:&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" checked="true" value="true"
                                                                    id="151_51_chkEnable">&nbsp;Super &nbsp;&nbsp;&nbsp;<input type="checkbox" id="151_51_chkEnableDownline"
                                                                        value="false" disabled="disabled">&nbsp;Super's downlines
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div class="">
                    <table class="tblcollapse w1000 marginT6">
                        <tbody>
                            <tr class="title1">
                                <td class="l">
                                    <span class="expanded" target="setting_152">Grey Hound</span>
                                </td>
                            </tr>
                            <tr id="setting_152">
                                <td>
                                    <div>
                                        <div class="">
                                            <table cellspacing="0" cellpadding="0" border="0" class="tblPTheader">
                                                <tbody>
                                                    <tr id="Tr1" style="cursor: pointer;">
                                                        <td class="btnleft">
                                                        </td>
                                                        <td width="120" class="l btnmiddle" target="setting_152_41">
                                                            <span class="mp">UK/Irish Tote</span>
                                                        </td>
                                                        <td width="20" class="setting_152_41 r  md3">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="152_41_errmsg" class="w800 errmsg">
                                            </div>
                                            <div id="setting_152_41" class="l" style="display: none;">
                                                <table class="tblcollapse w800">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="3">
                                                                Enable:&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" checked="true" value="true"
                                                                    id="152_41_chkEnable">&nbsp;Super &nbsp;&nbsp;&nbsp;<input type="checkbox" id="152_41_chkEnableDownline"
                                                                        value="false" disabled="disabled">&nbsp;Super's downlines
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="">
                                            <table cellspacing="0" cellpadding="0" border="0" class="tblPTheader">
                                                <tbody>
                                                    <tr id="Tr2" style="cursor: pointer;">
                                                        <td class="btnleft">
                                                        </td>
                                                        <td width="120" class="l btnmiddle" target="setting_152_51">
                                                            <span class="mp">AU Tote</span>
                                                        </td>
                                                        <td width="20" class="setting_152_51 r  md3">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="152_51_errmsg" class="w800 errmsg">
                                            </div>
                                            <div id="setting_152_51" class="l" style="display: none;">
                                                <table class="tblcollapse w800">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="3">
                                                                Enable:&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" checked="true" value="true"
                                                                    id="152_51_chkEnable">&nbsp;Super &nbsp;&nbsp;&nbsp;<input type="checkbox" id="152_51_chkEnableDownline"
                                                                        value="false" disabled="disabled">&nbsp;Super's downlines
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div class="">
                    <table class="tblcollapse w1000 marginT6">
                        <tbody>
                            <tr class="title1">
                                <td class="l">
                                    <span class="expanded" target="setting_153">Harness</span>
                                </td>
                            </tr>
                            <tr id="setting_153">
                                <td>
                                    <div>
                                        <div class="">
                                            <table cellspacing="0" cellpadding="0" border="0" class="tblPTheader">
                                                <tbody>
                                                    <tr id="Tr3" style="cursor: pointer;">
                                                        <td class="btnleft">
                                                        </td>
                                                        <td width="120" class="l btnmiddle" target="setting_153_51">
                                                            <span class="mp">AU Tote</span>
                                                        </td>
                                                        <td width="20" class="setting_153_51 r  md3">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <div id="153_51_errmsg" class="w800 errmsg">
                                            </div>
                                            <div id="setting_153_51" class="l" style="display: none;">
                                                <table class="tblcollapse w800">
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="3">
                                                                Enable:&nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" checked="true" value="true"
                                                                    id="153_51_chkEnable">&nbsp;Super &nbsp;&nbsp;&nbsp;<input type="checkbox" id="153_51_chkEnableDownline"
                                                                        value="false" disabled="disabled">&nbsp;Super's downlines
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <table cellspacing="0" cellpadding="0" border="0" class="marginT6">
                    <tbody>
                        <tr>
                            <td class="custBtn1">
                            </td>
                            <td onclick="return BtnOnClick();" id="btnAdd" class="custBtn2 hand">
                                Update
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/Tpl/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../../js/PT/CommonPT.js" type="text/javascript"></script>
    <script src="../../PT/Racing/JS/Racing.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var TplEnableRacingBox = "<div class='{{if !Visible}}hidden{{/if}}'>";
        TplEnableRacingBox += "<table class='tblcollapse w1000 marginT6'>";
        TplEnableRacingBox += "<tr class='title1'>";
        TplEnableRacingBox += "<td class='l'><span class='expanded' target='setting_${Id}'>${Name}</span></td>";
        TplEnableRacingBox += "</tr>";
        TplEnableRacingBox += "{{if Totes}}";
        TplEnableRacingBox += "<tr id='setting_${Id}'>";
        TplEnableRacingBox += "<td><div> {{tmpl($RacebooksStatus) TplEnableRacingType}} <div></td>";
        TplEnableRacingBox += "</tr></table></div>";

        var TplEnableRacingType = "<div class='{{if !Visible}}hidden{{/if}}'>";
        TplEnableRacingType += "<table cellspacing='0' cellpadding='0' border='0' class='tblPTheader'><tbody>";
        TplEnableRacingType += "<tr id='btntop' style='cursor: pointer;'>";
        TplEnableRacingType += "<td class='btnleft'></td>";
        TplEnableRacingType += "<td width='120' class='l btnmiddle' target='setting_${Id}'>";
        TplEnableRacingType += "<span class='mp' >${Name}</span></td>";
        TplEnableRacingType += "<td width='20' class='setting_${Id} r {{if IsTop}} md2{{else}} md3{{/if}}'></td></tr></tbody></table>";
        TplEnableRacingType += "<div id='${Id}_errmsg' class='w800 errmsg'></div>";
        TplEnableRacingType += "<div id='setting_${Id}_${Id}' class='l' style='{{if IsTop}}{{else}} display:none;{{/if}}'>{{tmpl($data) TplEnableRacingSetting}}</div>";
        TplEnableRacingType += "</div>";

        var TplEnableRacingSetting = "<table class='tblcollapse w800'>";
        TplEnableRacingSetting += "<tr><td colspan='3'>";
        TplEnableRacingSetting += "Enable:\&nbsp;<input type='checkbox' onclick='ChkEnalbe(this)' {{if !EnableUpline}}disabled='disabled'{{/if}} {{if Enable}}checked='true'{{/if}} value='${Enable}' id='${getId(0)}_chkEnable' />&nbsp;Agent";
        TplEnableRacingSetting += "&nbsp;&nbsp;&nbsp;<input type='checkbox' id='${getId(0)}_chkEnableDownline' value='${EnableDownline}' disabled='disabled' />&nbsp;Agent\'s downlines\'";
        TplEnableRacingSetting += "</td></tr></table>";

        function getEnableRacingStatus(custId, roleId) {
            $.ajax({
                url: ''

            });
        }
    </script>
</body>
</html>
