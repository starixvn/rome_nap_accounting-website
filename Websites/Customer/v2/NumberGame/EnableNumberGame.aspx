<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EnableNumberGame.aspx.cs" Inherits="Customer_v2_NumberGame_EnableNumberGame" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enable Number Game</title>
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    
    <script src="../../../js/jQuery/jquery.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/json2.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../../js/common.js" type="text/javascript"></script>
    
    <script src="js/EnableNG.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Enable Number Game
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
        </div>
        <div class="maincontent_box" style="width: 700px">
            <div id="ErrMsg" style="display:none"></div>
            <div id="enableNG">
                <table class="tbllayout" cellpadding="0" cellspacing="1" style="line-height: 21px !important;">
                    <tr>
                        <td colspan="4" style="font-weight:bold; color:#c28f06; text-align:left">
                           These settings will be affected to all downlines as well. 
                        </td>
                    </tr>
                    <tr>
                        <td class="l b" style="width: 10%" nowrap>
                            Username:
                        </td>
                        <td class="l" style="padding: 2px; width:40%">
                            <input class="txt" type="text" name="" id="txtUsername" onkeyup="javascript:onKeyEnter(event);" />
                        </td>
                        <td class="l b" style="width: 12%" nowrap>
                            Enable:
                        </td>
                        <td class="l" style="padding: 2px; width:38%">
                            <input id="chbEnable" type="checkbox" onclick="EnableNG.ChangeEnable();" />&nbsp;
                            <span id="divEnable" style="display:none; color:Red; padding-right:10px"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="l b" nowrap>
                            Min bet:
                        </td>
                        <td class="l" style="padding: 2px;">
                            <input class="" style="width:120px" type="text" name="" id="txtMinbet" disabled="disabled" />
                            <span id="divMinbet" style="display:none"></span>
                        </td>
                         <td class="l b" nowrap>
                            Max bet:
                        </td>
                        <td class="l" style="padding: 2px;">
                            <input class="" type="text" name="" id="txtMaxbet" disabled="disabled" />
                            <span id="divMaxbet" style="display:none"></span>
                        </td>
                    </tr>
                    <tr>
                        <td class="l b" nowrap>
                            Max per match:
                        </td>
                        <td class="l" style="padding: 2px;">
                            <input class="" style="width:120px" type="text" name="" id="txtMaxMatch" disabled="disabled" />
                            <span id="divMaxMatch" style="display:none"></span>
                        </td>
                        <td class="l b" nowrap>
                            Max per ball:
                        </td>
                        <td class="l" style="padding: 2px;">
                            <input class="" type="text" name="" id="txtMaxBall" disabled="disabled" />
                            <span id="divMaxball" style="display:none"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="c" style="padding: 2px;">
                            <input class="btn" type="button" onclick="EnableNG.UpdateBetSettingNGEnable();" name="btnSubmit"
                                id="btnSubmit" value="Submit" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    
</body>
</html>
