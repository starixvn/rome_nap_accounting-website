<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MiniGameMasterUpdate.aspx.cs"
    Inherits="Customer_MiniGameMasterUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Master</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="../../js/PT/CommonPT.js" type="text/javascript"></script>
    <script src="../../js/PT/MiniGame/MasterUpdate.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC fixfont">
                            Update PT Mini Game
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box">
                <div class="errMsg" id="err">
                </div>
                <div class="error-message" id="errMsg">
                </div>
            </div>
            <div class="bg_table">
                <div id="MasterInfo"></div>
                <div id="MasterMiniGame"></div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
