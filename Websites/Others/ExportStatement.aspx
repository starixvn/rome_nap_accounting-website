<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportStatement.aspx.cs" Inherits="UploadVirtualRate" theme=""%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>        
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css">
</link>
    
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>    
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    
    <script src="../js/jQuery/ajaxfileupload.js" type="text/javascript"></script>     
    
    <script src="../js/Others/ExportStatement.js" type="text/javascript"></script>
</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL"></td>
                <td class="maincontent_headerC">Export Multi Statement</td>
                <td class="maincontent_headerR"></td>
            </tr>
        </table>
        
        <div class="maincontent_box ">
            <div class="h3" style="margin-top:5px">Choose an Excel file to export: </div>
            <input type="file" onchange="return IsValidFileExtension()" size="40" name="txtfile" id="txtfile"/>
            <input class="btn" style="width: 60px" id="btnUpload" type="submit" onclick="UploadFile()" value="Open File"/>                   
            <div style="color:#808080;margin-bottom:5px">You are allowed to export statements of maximum 100 accounts.</div>    
            <div id="dvCalendar" style="display:none">
                <%=calendar%>  
            </div>
            <div id="msgError" style="display:block">&nbsp</div>
        </div>
     </div>   
        
        <div id="content"></div>
        
        <input type="hidden" id="bolDelDataTmp" value="0" />
        <input type="hidden" id="FileName" value="0" />        
        <input type="hidden" id="userslistvalid" name="userslistvalid" />
        <input type="hidden" id="FullPath" name="FullPath" />        
        <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
    
</body>
</html>