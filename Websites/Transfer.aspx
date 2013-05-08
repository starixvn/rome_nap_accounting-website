<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Transfer.aspx.cs" Inherits="Transfer" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/LeftPanel/left_panel.css" rel="stylesheet" type="text/css" />
    <link href="UserGuide/css/userguide.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/jQuery/jquery.js"></script>

    <script type="text/javascript" src="UserGuide/js/userGuide.js"></script>

    <script type="text/javascript" src="UserGuide/js/centreIt-1.1.5.js"></script>

    <script src="js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/common.js"></script>

    <script type="text/javascript" src="js/transfer.js?ver=<%=WebSettings.Version%>"></script>

    <script src="js/jQuery/Number/jquery.caret.1.02.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/BlockUI.js"></script>

    <script src="js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>

    <script type="text/javascript">
        function validateValue() {
            try {
                $('#errMsg').hide();
                var _toUser = document.getElementById("touser").value;
                if (_toUser.isNullOrEmpty()) {
                    $('#errMsg').show();
                    showAlertMessage("errMsg", "Invalid To User", 1);
                    return false;
                }  //return throwErr('Invalid To User');

                var _exRate = document.getElementById("exrate").value;
                if (!_exRate.isNumeric() || parseFloat(_exRate) == 0) {
                    $('#errMsg').show();
                    showAlertMessage("errMsg", "Invalid Exchange Rate", 1);
                    return false;
                } 

                var _amount = document.getElementById('txtamount').value;
                if (!_amount.isNumeric() || parseFloat(_amount) == 0) {
                    $('#errMsg').show();
                    showAlertMessage("errMsg", "Invalid Amount", 1);
                    return false;
                }
            }
            catch (ex) {
                $('#errMsg').show();
                showAlertMessage("errMsg", ex, 1);
                return false;
            }
            
            return true;
        }

        function throwErr(ex) {
            //writeErrMsg(ex, 'errMsg');
            showAlertMessage("errMsg", ex, 1);
            return false;
        }
        var showHelpMultiCrossTran = function (functionName) {
            var url = "UserGuide/View.aspx?functionName=" + functionName;
            ShowPopupSettle(url, "Multiple Cross-Currency Finance Transfer", 720, 600, 50, 100);
        };
    </script>

    <style type="text/css">
        .tblFinance
        {
        }
        .tblFinance tr
        {
            line-height: 12px;
        }
    </style>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <div style="border: 1px solid #7C98A4; width: 700px">
                <div class="box_header" style="width: 100%; margin: 0px">
                    <span style='color: Red; font-size: 12px; margin-left: 4px; display: <%= (!IsHadPermissionOnYesterdayTransfer && !IsHadPermissionOnCrossCurrencyTransfer) ? "block" : "none" %>'>
                        Access Denied. </span>
                    <div id="left_tab1" class="left_tab" style="width: 100%">
                        <ul>
                            <li id="left_tab1_1" style='float:left;display: <%= IsHadPermissionOnYesterdayTransfer ? "block" : "none" %>'
                                <%= IsHadPermissionOnYesterdayTransfer ? "class='current'" : "" %>><a href="javascript:;">
                                    <span>
                                        <asp:Literal ID="sHeader" runat="server"></asp:Literal>
                                    </span></a></li>
                            <li id="left_tab1_2" style='float:left;display: <%= IsHadPermissionOnCrossCurrencyTransfer ? "block" : "none" %>; position:relative'
                                <%= IsHadPermissionOnYesterdayTransfer ? "" : "class='current'" %>>
                                <a href="javascript:;">
                                    <span style="padding-right:22px">Finance Cross-Currency Transfer</span>
                                </a>
                                <span style="background-image:url('UserGuide/image/icon_help.png'); cursor:pointer;
                                        width:14px; height:14px; position:absolute; right:8px; top:3px"
                                        onclick="javascript:userGuide.show('SingleCrossCurrencyFinanceTransfer', 'Single Cross-Currency Finance Transfer', 700, 710);"></span>
                            </li>
                        </ul>
                    </div>
                </div>
                <div>
                    <div id="divMessage" style="display: none; margin-left: 2px; margin-right: 2px">
                    </div>
                    <div id="errMsg" style="display: none">
                    </div>
                    <div id="clear2" style="display: none">
                    </div>
                    <div id="content_tab1" style="margin-top: 10px; margin-left: 10px; display: <%= IsHadPermissionOnYesterdayTransfer ? "block" : "none" %>">
                        <asp:Literal ID="sMain" runat="server"></asp:Literal>
                        <iframe id="frmTransfer" name="frmTransfer" width="350px" height="110px" frameborder="0"
                            style="border: 0px none; margin-top: 5px" scrolling="no"></iframe>
                    </div>
                    <div id="content_tab2" style="margin-top: 10px; margin-left: 10px; margin-bottom: 60px;
                        position: relative; display: <%= IsHadPermissionOnYesterdayTransfer ? "none" : IsHadPermissionOnCrossCurrencyTransfer ? "block" : "none" %>">
                        <div style="border: 1px solid #7C98A4; width: 490px">
                            <table class="tblFinance" border="0" cellpadding="5" cellspacing="0" style="border-collapse: collapse">
                                <tbody>
                                    <tr>
                                        <td align="left">
                                            <div align="left" style="width: 66px; text-align: left; font-weight: bold">
                                                From User:</div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 140px; text-align: left; position: relative">
                                                <div id="divAjaxLoaderImageFromUser" style="display: none; width: 16px; height: 16px;
                                                    position: absolute; background-image: url('App_Themes/Images/ajax-loader.gif');
                                                    right: 0px">
                                                </div>
                                                <input class="txt" type="text" id="tbxFromUser" name="tbxFromUser" size="10" />
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div style="width: 30px;">
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 80px; text-align: left; font-weight: bold">
                                                From Amount:
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 125px; text-align: left">
                                                <input class="txt" type="text" id="tbxFromAmount" name="tbxFromAmount" style="text-align: right" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <div align="left" style="width: 66px; text-align: left;">
                                                Currency:
                                            </div>
                                        </td>
                                        <td>
                                            <div align="left" style="width: 140px; text-align: left">
                                                <span id="spanCurrencyFromUser" style="font-weight: bold"></span>
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div style="width: 30px;">
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 80px; text-align: left;">
                                                <span>Ex.Rate (RM):</span>
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 125px; text-align: left">
                                                <span id="spanExRateFromUser" style="font-weight: bold"></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <div align="left" style="width: 66px; text-align: left; font-weight: bold">
                                                To User:</div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 140px; text-align: left; position: relative">
                                                <div id="divAjaxLoaderImageToUser" style="display: none; width: 16px; height: 16px;
                                                    position: absolute; background-image: url('App_Themes/Images/ajax-loader.gif');
                                                    right: 0px">
                                                </div>
                                                <input class="txt" type="text" id="tbxToUser" name="tbxToUser" size="10" />
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div style="width: 30px;">
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 80px; text-align: left; font-weight: bold">
                                                To Amount:
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 125px; text-align: left">
                                                <input class="txt" type="text" id="tbxToAmount" name="tbxToAmount" style="text-align: right" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trToUserCurrency">
                                        <td align="left">
                                            <div align="left" style="width: 66px; text-align: left;">
                                                Currency:
                                            </div>
                                        </td>
                                        <td>
                                            <div align="left" style="width: 140px; text-align: left">
                                                <span id="spanCurrencyToUser" style="font-weight: bold"></span>
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div style="width: 30px;">
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 80px; text-align: left;">
                                                <span>Ex.Rate (RM):</span>
                                            </div>
                                        </td>
                                        <td align="left">
                                            <div align="left" style="width: 125px; text-align: left">
                                                <span id="spanExRateToUser" style="font-weight: bold"></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                                                <tr>
                                                    <td align="left">
                                                        <div align="left" style="width: 76px; text-align: left;">
                                                            Forex account:
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <div align="left" style="width: 140px; text-align: left">
                                                            <select id="selectForexAccount" style="width: 120px">
                                                            </select>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trSystemTransExRate" style="display: none">
                                        <td colspan="5">
                                            <div>
                                                <span>System Ex.Rate </span>(<span id="spanPerCurrencySystem"></span>)<span>: </span>
                                                <span id="spanExRateSystem" style="font-weight: bold"></span><span style="padding-left: 50px">
                                                    Trans Ex.Rate</span> (<span id="spanPerCurrencyTrans"></span>)<span>: </span>
                                                <span id="spanExRateTrans" style="font-weight: bold"></span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <div align="left" style="width: 66px; text-align: left;">
                                                Remark:
                                            </div>
                                        </td>
                                        <td colspan="4" align="left">
                                            <div align="left">
                                                <input class="txt" type="text" id="tbxDescription" name="tbxRemark" style="width: 400px"
                                                    maxlength="200" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="5">
                                            <div>
                                                <input class="btn" type="button" id="btnTransfer" value="Transfer" />
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <input type="hidden" id="hiddenSearchedName" runat="server" />
                        <input type="hidden" id="hiddenLoginID" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
