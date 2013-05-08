<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerFrame.aspx.cs" Inherits="NewCustomer_CustomerFrame" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=Resources.Language.lblAccountingSystem%></title>
</head>
<frameset rows="30,*">    
        <frame name="topCust" src="SelectType.aspx" frameborder="0" scrolling="no" />
        <frame name="mainCust" frameborder="0" scrolling="auto" />    
</frameset>
</html>
