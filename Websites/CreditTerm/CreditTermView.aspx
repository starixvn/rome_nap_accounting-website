<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreditTermView.aspx.cs" Inherits="CreditTerm_CreditTermView" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/GridScroller/gridscroller.css" rel="stylesheet" type="text/css" />
    <link href="../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CreditTerm/creditterm.css" rel="stylesheet" type="text/css" />
</head>
<body onload="Custom.init();">
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC fixfont">Credit term
                    </td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box ">
                <div id="errMsg" class="errMsg">
                    <%=msg%>
                </div>
                <%if (string.IsNullOrEmpty(msg))
                    {%>
                <div class="pad_tb_4">
                    <a href="CreditTermGroupView.aspx">[ View all group ]</a>
                </div>
                <%} %>
            </div>
            <div class="bg_table">
                <asp:Literal ID="content" runat="server"></asp:Literal>
                <iframe id="frmterm" frameborder="0" height="0" name="frmterm" width="0" style="padding: 0px; margin: 0px;"></iframe>
            </div>
            <div id="confirmUpdate" class="hidden">
                <p>Beside adding super(s) into the selected Headline Group, the following super(s) will be added into External Group with Primary Extra PT Group indicated.</p>
                <p id="headlineName"></p>
                <div id="supermapping" class="freegrid grid_mapping"></div>
            </div>
        </div>
        <div>

        </div>
    </div>
</body>
<script type="text/javascript">
    window.rootUrl = "<%= Page.ResolveUrl("~/") %>";
    var tomorrow = "<%= DateTime.Today.AddDays(1).ToString("MM/dd/yyyy") %>";
</script>
<script src="../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../Scripts/_Libs/jQuery/jquery-ui-1.9.1.min.js" type="text/javascript"></script>
<script src="../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
<script src="../js/CreditTerm.js?v=<%= AccCommon.Version() %>" type="text/javascript"></script>
<script src="../js/common.js" type="text/javascript"></script>
<script src="../js/Popup/confirmbox.js" type="text/javascript"></script>
<script type="text/javascript" src="../Scripts/_Libs/FanexGrid/Fanex.Grid.js?v=<%= AccCommon.Version() %>"></script>
</html>
