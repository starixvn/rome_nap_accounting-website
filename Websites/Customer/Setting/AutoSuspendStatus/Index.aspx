<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Customer_Setting_AutoSuspendStatus_Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Auto Suspend Status</title>
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/Message.css" />
    <link rel="stylesheet" type="text/css" href="../../../js/ToolTip/Tipsy-Tooltip/tipsy.css" />
    <link rel="stylesheet" type="text/css" href="Styles/AutoSuspendStatus.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="maincontent">
            <div id="result-message">
                Loading...
            </div>
            <div id="setting-content" class="display-none">
                <table class="tblcollapse content-width table-settings">
                    <tbody>
                        <tr class="title1">
                            <td id="customer-info-title">
                            </td>
                        </tr>
                        <tr>
                            <td class="align-left">
                                <input id="checkbox-enable-autosuspend-soccer" type="checkbox" />
                                <span id="checkbox-enable-autosuspend-soccer-disable" class="checkbox-checked display-none"
                                    title="Upline has been auto suspended"></span>
                                <label id="label-enable-autosuspend-soccer" class="label-for-checkbox" for="checkbox-enable-autosuspend-soccer">
                                    Auto Suspend Soccer
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="align-left">
                                <input id="checkbox-enable-autosuspend-othersports" type="checkbox" />
                                <span id="checkbox-enable-autosuspend-othersports-disable" class="checkbox-unchecked display-none"
                                    title="Upline has been auto suspended"></span>
                                <label id="label-enable-autosuspend-othersports" class="label-for-checkbox" for="checkbox-enable-autosuspend-othersports">
                                    Auto Suspend Other Sports
                                </label>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td class="custBtn1">
                        </td>
                        <td id="button-update" class="custBtn2 hand">
                            Update
                        </td>
                    </tr>
                </table>
                <br>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="../../../js/common.js"></script>
    <script type="text/javascript" src="../../../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../../../js/jQuery/json2.js"></script>
    <script type="text/javascript" src="../../../js/jQuery/Tpl/jquery.tmpl.js"></script>
    <script type="text/javascript" src="../../../js/jQuery/jquery.blockUI.1.33.js"></script>
    <script type="text/javascript" src="../../../js/BlockUI.js"></script>
    <script type="text/javascript" src="../../../js/Common/Functions.js"></script>
    <script type="text/javascript" src="../../../js/ToolTip/Tipsy-Tooltip/jquery.tipsy.js"></script>
    <script type="text/javascript" src="Scripts/AutoSuspendStatus.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>"></script>
    <script type="text/javascript">
        var viewResources =<%=ViewResources%>;
    </script>
</body>
