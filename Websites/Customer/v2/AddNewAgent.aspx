<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddNewAgent.aspx.vb" Inherits="Customer_V2_AddNewAgent"
    Title="Untitled Page" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/common.js?v=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="../../js/agent/Customer.js?v=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="../../js/agent/MEditCustomer.js?v=<%=AccCommon.Version() %>"></script>
    <%VariableOfScript()%>

    <script src="../../js/PT/NumberGame/NumberGamePT.js" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js" type="text/javascript"></script>
</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC fixfont">
                    Add New Agent
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="err" class="errMsg">
            </div>
        </div>
        <div class="bg_table">
            <script src="../../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>
            <script src="../../js/ToolTip/wz_tooltip/tip_balloon2.js" type="text/javascript"></script>
            <form id="fUpdate" name="fUpdate" method="post" action="UpdateNewAgent.aspx" target="frInsert">
            <asp:Literal ID="htmlAddAgentContent" runat="server"></asp:Literal>
            <iframe id="frInsert" name="frInsert" style="display: none"></iframe>
            <input type="hidden" name="roleId" id="roleId" value="2" />
            <input type="hidden" name="roleId2" id="roleId2" value="2" />
            <input type="hidden" id="isAllowUpd" name="isAllowUpd" value="1" />
            <input type="hidden" name="masterId" id="masterId" value="<%=masterId %>" />
            <input type="hidden" name="set" id="set" value="<%=Request("set") %>" />
            <script type="text/javascript">
                selectTranferTime();
            </script>
        </div>
    </div>
</body>
</html>
