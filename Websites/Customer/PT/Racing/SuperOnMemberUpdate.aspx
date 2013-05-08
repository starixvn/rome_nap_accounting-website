<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuperOnMemberUpdate.aspx.cs"
    Inherits="Customer_Racing_SuperOnMemberUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Super On Member</title>
    <script src="JS/StartupScript.js" type="text/javascript"></script>
    <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="Style/RacingSetting.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC fixfont">
                            Update Racing Setting Super On Member
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box" style="width: 800px;">
                <div id="err">
                </div>
            </div>
            <div id="SettingContent">
                Loading...</div>
        </div>
    </div>
    <script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/Tpl/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../../js/BlockUI.js" type="text/javascript"></script>
    <script src="../../../js/Common/Functions.js" type="text/javascript"></script>
    <script src="JS/CommonPT.js" type="text/javascript"></script>
    <script src="JS/Racing.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script src="JS/RacingData.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script src="JS/SuperOnMemberUpdate.js?v=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">
        var Roles = <%=PageRoles%>;
    </script>
</body>
</html>
