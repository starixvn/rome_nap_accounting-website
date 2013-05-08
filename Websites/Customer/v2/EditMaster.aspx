<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditMaster.aspx.vb" Inherits="Customer_V2_EditMaster"
    Title="Untitled Page" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript" src="../../js/agent/Customer.js"></script>
    <script type="text/javascript" src="../../js/agent/EditMaster.js"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>

    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
</head>
<body>
    <div class="maincontent">
         <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    Update Master
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
        <div class="maincontent_box">
            <div id="err" class="errMsg">
            </div>
            <div id="errMsg" class="error-message">
            </div>
        </div>
        <div class="bg_table">

            <script src="../../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>

            <script src="../../js/ToolTip/wz_tooltip/tip_balloon2.js" type="text/javascript"></script>

            <%VariableOfScript()%>
            <iframe name="updateData" id="updateData" height="0" width="0" style="display: none">
            </iframe>
            <form id="form1" name="form1" method="post" action="UpdateEditMaster.aspx" target="updateData">
            <input type="hidden" name="roleId2" id="roleId2" value="3" />
            <input type="hidden" id="hidEdit" name="hidEdit" value="1" />
            <input type="hidden" id="saveControlDisabled" name="saveControlDisabled" />
            <input type="hidden" id="maxMemberPT" name="maxMemberPT" value="0" />
            <input type="hidden" id="saveCustid" name="saveCustid" value="<%=strcustid %>" />
            <input type="hidden" name="superId" id="superId" value="<%=superId%>" />
            <input type="hidden" id="saveUsername" name="saveUsername" runat="server" />
            <input type="hidden" id="isMult" name="isMult" value="0" />
            <input type="hidden" id="isUpdateMember" name="isUpdateMember" value="0" />
            <input type="hidden" id="memberPT" name="memberPT" />
            <input type="hidden" id="set" name="set" value="<%=fromSet %>" />
            <div style="text-align: left;">
                <asp:Literal ID="htmlEditMasterContent" runat="server" EnableViewState="false"></asp:Literal>
            </div>
            </form>
            <%=strErrMsg%>

            <script type="text/javascript">
                var errmsg = '<%=msg%>'
                if (errmsg != '')
                    ShowError(errmsg, 'err');
            </script>

        </div>
    </div>
</body>
</html>
