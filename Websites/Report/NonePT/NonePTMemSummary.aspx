<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NonePTMemSummary.aspx.cs" Inherits="Report_NonePT_NonePTMemSummary" Theme=""  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css"/>      
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Report/Report.css"/>
    <link href="../../App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" /> 
    
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>

    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/pager_v2.js" type="text/javascript"></script>
    <script src="../../js/Report/NonePT/NonePTMemSummary.js" type="text/javascript"></script>

    
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC"><span id="spTitle"> Members Position Taking Monitoring</span></td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="sHeader">
                    <%=header%>
                    
                </div>
            </div>
            
            <div id="sErrMsg" class="error-message" style="font-weight:bold"></div>                
        </div>
        
        <div style="margin-top:2px; margin-left:1px" class="maincontent_box">
            <div id="sContent"></div>
        </div>    
    </div> 
    <input type="hidden" id="currpage" name="currpage" />
    <iframe id="frm" width="0" height="0"  frameborder="0" style="display:none"></iframe>
    <img id="displayBox" src="../../App_Themes/Images/waiting7.gif" alt="" style="display: none;" />
    <script type="text/javascript">
        GetNonePTMemSummary(1);
    </script>
</body>
</html>
