<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WinLostSummary.aspx.cs" Inherits="Report_WinLostSummary" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title><%=Resources.Language.lblAccountingSystem%></title>
</head>
<frameset cols="580,*">
    <frame name="leftPanel" frameborder="0" noresize="noresize" scrolling="auto" src="WinLostSummary1.aspx<%=param %>" />
    <frame src="about:blank" name=WinLostDetails id="WinLostDetails"/>
</frameset>
</html>
