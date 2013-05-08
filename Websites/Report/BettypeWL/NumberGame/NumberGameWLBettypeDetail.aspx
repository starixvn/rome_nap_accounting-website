<%@ Page Language="C#"  AutoEventWireup="true" CodeFile="NumberGameWLBettypeDetail.aspx.cs" Inherits="NumberGameWLBettypeDetail" Title="Untitled Page" %>

<html>
    <head runat ="server">
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../../../App_Themes/Public/input.css"/> 
    <link href="../../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css"></link>
    <script type="text/javascript" src="../../../js/jQuery/jquery.1.3.1.js"></script>
    <script type="text/javascript">
        $(document).ready(function(){
            parent.parent.$.unblockUI();
        });
    </script>
    </head>
    <body>
        <form id ="frm" method ="post" action ="NumberGameWLBettypeDetail.aspx?<%=param %>">
        <b>Export To Excel</b>
        <input class="btnExcel" type ="submit" id ="btnExport" name="btnExport" value="  " style="width:20px; height:20px;" />
        <asp:Literal ID ="content" runat ="server"></asp:Literal>
        </form>
    </body>
</html>
    



