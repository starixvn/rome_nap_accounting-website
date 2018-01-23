<%@ Page Language="C#" AutoEventWireup="true" CodeFile="__DeleteBet.aspx.cs" Inherits="BetList_DeleteBet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delete Bet</title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>   
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/DeleteBet.js?ver=1"></script>
    
</head>
<body>
    <div class="main">
    <div class="maincontent">
         <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                    <tr>
                        <td class="maincontent_headerL"></td>
                        <td class="maincontent_headerC">Delete Bet</td>
                        <td class="maincontent_headerR"></td>
                    </tr>
         </table>
         <asp:Literal ID="lContent" runat="server"></asp:Literal>
    </div>
    <script type="text/javascript" language="javascript">
        var userid = '<%=userId %>'
        $('#username').val(userid);
    </script>
</div>
</body>
</html>
