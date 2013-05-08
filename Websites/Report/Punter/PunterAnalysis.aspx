<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PunterAnalysis.aspx.cs" Inherits="Report_Punter_PunterAnalysis" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />    
    <link rel="stylesheet" type="text/css" href="../../js/IBCWnd/IBCWnd.css"/>    
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />  
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/Report/Punter/PunterAnalysis.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>        
  
    <script type="text/javascript" src="../../js/IBCWnd/IBCWnd.js"></script>
</head>
<body>   
    <input type="hidden" id="FullPath" name="FullPath" value="<%=fullname %>" />
    <input type="hidden" id="searchby" name="searchby" />
    <input type="hidden" id="periodtype" name="periodtype" />    
    <input type="hidden" id="orderby" name="orderby" />    
    <input type="hidden" id="ordertype" name="ordertype" />    
    <input type="hidden" id="fMonth" name="fMonth" />
    <input type="hidden" id="fYear" name="fYear" />
    <input type="hidden" id="tMonth" name="tMonth" />
    <input type="hidden" id="tYear" name="tYear" />
    
    <input type="hidden" id="SelTopPunterSummaryContent" name="SelTopPunterSummaryContent" />
    <input type="hidden" id="SelTopPunterSummaryHeader" name="SelTopPunterSummaryHeader" />
    <input type="hidden" id="SelTopPunterSummarycachepath" name="SelTopPunterSummarycachepath" />
    
    <input type="hidden" id="SelTopPunterByCustIDContent" name="SelTopPunterByCustIDContent" />
    <input type="hidden" id="SelTopPunterByCustIDContentHeader" name="SelTopPunterByCustIDContentHeader" />
    <input type="hidden" id="SelTopPunterByCustIDcachpath" name="SelTopPunterByCustIDcachpath" />
    
    <input type="hidden" id="SelTopPunterBySporttypeContent" name="SelTopPunterBySporttypeContent" />
    <input type="hidden" id="SelTopPunterBySporttypeHeader" name="SelTopPunterBySporttypeHeader" />
    <input type="hidden" id="SelTopPunterBySporttypecatchpath" name="SelTopPunterBySporttypecatchpath" />    
    
    <input type="hidden" id="SelTopPunterSummaryByLeagueContent" name="SelTopPunterSummaryByLeagueContent" />
    <input type="hidden" id="SelTopPunterSummaryByLeaguePath" name="SelTopPunterSummaryByLeaguePath" />   
    
    <input type ="hidden" id="type" name="type" value="" />
    <input type="hidden" id="username" name="username" value="" />
    <input type="hidden" id="custid" name="custid" value="" />
    <input type="hidden" id="sporttype" name="sporttype" value="" />    
    <input type="hidden" id="bettype" name="bettype" value="" />    
    <input type="hidden" id="leagueid" name="leagueid" value="" />    
    <input type="hidden" id="leaguename" name="leaguename" value="" />    
        
    <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
    <img id="displayBox" src="../../App_Themes/Images/waiting7.gif" alt="" style="display: none;" />
   
   <%=html %>
   <%=script%>
    
</body>
</html>
