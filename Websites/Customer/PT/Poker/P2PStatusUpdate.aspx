<%@ Page Language="C#" AutoEventWireup="true" CodeFile="P2PStatusUpdate.aspx.cs"
    Inherits="Customer_Poker_P2PStatusUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Poker Status</title>
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="Style/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style>
        .margin6
        {
            margin: 6px;
        }
        .marginT6
        {
            margin-top: 6px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="maincontent">
            <div id="err">
            </div>
            <div id="SettingContent" class="hidden"> 
                <div >
                    <table class="tblcollapse marginT6">
                        <tbody>
                             <tr class="title1">
                                <td colspan="2">
                                   <span id="custinfo"></span>
                                </td>
                            </tr>
                            <tr class="">
                                <td class="w150 l">
                                    &nbsp;<input type="checkbox" id="chkEnable" value="" onclick="ChkEnalbe(this)" />&nbsp;Enable Poker
                                </td>
                                <td class="w220 l" id="tddl">
                                    <input type="checkbox" disabled="disabled" value="" id="chkEnableDownline" />&nbsp;Enable Poker Downlines
                                </td>
                            </tr>
                             <tr class="">
                                <td class="w150 l">
                                    &nbsp;<input type="checkbox" id="chkEnableFun" value="" onclick="ChkEnalbe(this)" />&nbsp;Enable Poker Play For Fun
                                </td>
                                <td class="w220 l" id="tddlfun">
                                    <input type="checkbox" disabled="disabled" value="" id="chkEnableFunDownline" />&nbsp;Enable Poker Play For Fun Downlines
                                </td>
                            </tr>  
                        </tbody>
                    </table>
                </div>
                <input type="button" value="Update" class="btnUpdateSetting marginT6" id="btnAdd" onclick="return BtnOnClick();" />
                <br />
            </div>
        </div>
    </div>
    </form>
    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="JS/CommonPT.js" type="text/javascript"></script>
    <script src="JS/PokerData.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script src="JS/StatusUpdate.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script type='text/javascript'>
        var custId=<%=custId%>;
        var isDisabled=<%=isDisabled%>;
        var msgDisabled='<%=msgDisabled%>';
    </script>
</body>
</html>
