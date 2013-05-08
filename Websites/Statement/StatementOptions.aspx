<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatementOptions.aspx.cs" Inherits="Statement_StatementOptions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Statement report options</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
<link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
<link rel="Stylesheet" type ="text/css" href ="../App_Themes/Report/Statement.css?v=2" />

</head>
<body class="no-margin">
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
            </div>
            <div class="bg_table">
                 <asp:Literal ID="html" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
   
</body>
</html>
