<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CasinoDetail_1.aspx.cs" Inherits="Report_CasinoDetail" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">        
    <script type="text/javascript" src="../js/common.js" ></script> 

    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
</head>
<body>    
    <%=html.ToString()%>
      
    <script type="text/javascript">

        
         function ExportToExcel() {
             var wldate = getPramJs('wldate');
             var bettype = getPramJs('bettype');
             var WL = getPramJs('WL');
             var db = getPramJs('db');

             var url = '?wldate=' + wldate;
             url += '&bettype=' + bettype;
             url += '&WL=' + WL;
             url += '&db=' + db;
             url += '&IsExport=1';

             //alert(url);

             window.location.href = url;
         }
     </script>
</body>
</html>
