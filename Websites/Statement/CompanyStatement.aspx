<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyStatement.aspx.cs" Inherits="Statement_CompanyStatement" Title="Statement" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title id="ReportTitle" runat="server"></title>
<link href="~/App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
<link href="~/App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
<link href="~/App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
<link href="~/App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
<link rel="Stylesheet" type ="text/css" href ="~/App_Themes/Report/Statement.css" />
<link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
</head> 
<body>
       <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <asp:Literal ID="reportHeader" runat="server"></asp:Literal> 
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <asp:Literal ID="searchContent" runat="server"></asp:Literal>
            </div>
            <div class="bg_table" style="display:none;" id="statementCotent">
                 <div id="sData">
                 <%--Loading...--%>
                 </div>                                       
            </div>
        </div>
    </div>
    
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src ="WebResources/js/CompanyStatement.js?ver=<%=WebSettings.Version%>" type="text/javascript" ></script>    
    
    <script src="../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>
</body>
</html>

