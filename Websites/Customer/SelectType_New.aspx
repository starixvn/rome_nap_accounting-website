<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectType_New.aspx.cs" Inherits="NewCustomer_SelectType_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Customer</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="js/addnew.js"></script>
</head>
<body style="margin: 0px;">
    <iframe name="frCheckUser" frameborder="1" class="hidden" id="frCheckUser" height="0px"
        width="500px"></iframe>
    <form id="fSelectType" method="post" action="SelectType_New.aspx" runat="server">
    <input type="hidden" name="custRoleId" id="custRoleId" value="<%=Session["CustRoleId"]%>" />
    <input type="hidden" name="custId" id="custId" value="<%=Session["CustId"]%>" />
    <asp:Literal ID="SelectTypeContentHtml" runat="server"></asp:Literal>
    </form>
</body>
</html>