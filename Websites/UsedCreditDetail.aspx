<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UsedCreditDetail.aspx.cs" Inherits="UsedCreditDetail" Title="Untitled Page" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript">
         function resetClosedConfirm() {
             return confirm("Are you sure you want to reset all closed account credit?");
         }
         function resetAllConfirm() {
             return confirm("Are you sure you want to reset all account credit?");
         }
    </script>
   
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">Credit Detail
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box ">
            </div>
            <div class="bg_table">
                <asp:Literal ID="sBody" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</body>
</html>
