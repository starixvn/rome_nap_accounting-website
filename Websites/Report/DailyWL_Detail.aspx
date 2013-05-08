<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="DailyWL_Detail.aspx.cs" Inherits="DailyWL_Detail" Title="Untitled Page" %>

<html>
    <head runat ="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/> 
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css"></link>
    </head>
    <body>
        <form id ="frm" method ="post" action ="DailyWL_Detail.aspx?<%=param %>">
        <b>Export To Excel</b>
        <input class="btnExcel" type ="submit" id ="btnExport" name="btnExport" value="  " style="width:20px; height:20px;" />
        <asp:Literal ID ="content" runat ="server"></asp:Literal>
        </form>
    </body>
</html>
    



