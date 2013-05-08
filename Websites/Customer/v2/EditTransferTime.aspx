<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditTransferTime.aspx.cs" Inherits="Customer_v2_EditTransferTime" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <link href="../../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
     <script src="../../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/common.js"></script>
    <%=GetLanguage()%>
</head>
<body onload="selectTranferTime();">    
    <div class="main">
        <div class="maincontent" style="width:420px">
            <div class="maincontent_box">
                <div id="err"></div>
                <div id="clear2"></div>
                 <div>
                    <input type="hidden" id="isEdit" name="isEdit" value="1" />
                    <form id="fUpdate" name="fUpdate" onsubmit="return CheckTime();" action="UpdateTransferTime.aspx" method="post" target="frUpdate">
                    <input type="hidden" id="set" name="set" value="<%=Request["set"]%>" />
                    <input type="hidden" id="custId" name="custId" runat="server" />
                    <asp:Literal ID="htmlTransferTime" runat="server"></asp:Literal></form>
                    <iframe name="frUpdate" id="frUpdate" style="display: none"></iframe>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

