<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HRGroupDailyforTote.aspx.cs" Inherits="Report_HRGroupDaily" Theme=""  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"> 
    <title><%=Resources.Language.lblAccountingSystem%></title>    
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>   
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <link href="../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" /> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/>      
    
    
    <script src="../../js/Report/HRGroupDaily/HRGroupDaily.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
</head>
<body>
    
    <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
        <tr>
            <td class="maincontent_headerL"></td>
            <td class="maincontent_headerC">
                 Horse Racing Group Daily
            </td>
            <td class="maincontent_headerR"></td>
        </tr>
    </table>
        <div class="maincontent_box">
            <asp:Literal ID="Header" runat="server"></asp:Literal>
        </div>
        <div>
            <input type="hidden" id="type" name="type" value="<%=Request["type"] %>" />
            <div style="margin-top:2px" id="sContent" runat="server"></div>
            <img id="displayBox" src="../../App_Themes/Images/waiting7.gif" alt="" style="display: none;" />
            <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
        </div>
    
    <script type="text/javascript">
        ViewReport(0);
    </script>
</body>
</html>
