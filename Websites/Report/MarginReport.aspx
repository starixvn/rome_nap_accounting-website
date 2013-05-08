<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MarginReport.aspx.cs" Inherits="Report_MarginReport"
    Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/commonIOM.css" />
</head>
<body>
    <div class="main">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Margin Report </span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox" style="padding: 0px; border: none;">
                            <%--<div class="filterContent" style="height: 22px;">
                            </div>--%>
                        </div>
                        <div class="contentBox" style="">
                            <div id="sData" style="width: 700px">
                                <asp:literal id="content" runat="server"></asp:literal>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
