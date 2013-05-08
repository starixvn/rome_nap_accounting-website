<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RacingStatusUpdate.aspx.cs"
    Inherits="Customer_Racing_RacingStatusUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Racing Status</title>
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="Style/RacingSetting.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="maincontent">
            <div id="err">
            </div>
            <div id="SettingContent">
                Loading...</div>
        </div>
    </div>
    </form>
    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/Tpl/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../../js/Common/Functions.js" type="text/javascript"></script>
    <script src="JS/CommonPT.js" type="text/javascript"></script>
    <script src="JS/Racing.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script src="JS/RacingData.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script src="JS/StatusUpdate.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">var Roles=<%=PageRoles%>;</script>
</body>
</html>
