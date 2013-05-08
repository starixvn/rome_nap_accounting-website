<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MarginReport_14.aspx.cs" Inherits="MarginReport_14" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title id="ReportTitle" runat="server"></title>
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
<link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <asp:Literal ID ="header" runat ="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="bg_table">
                <div id="sData" style="width:700px">
                    <asp:Literal ID="content" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>  
</body>
</html>
