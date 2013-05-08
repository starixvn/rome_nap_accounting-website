<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Outright.aspx.cs" Inherits="SpecialDelete_Outright" Title="Untitled Page" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />  
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />  
    <link href="../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>

    <script src="../js/jQuery/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>   
    <script src="js/SpecialDelete.js" type="text/javascript"></script>
    
    
    <style type="text/css">
       
        input[type=checkbox]
        {
         cursor:pointer;	
        }
    </style>
</head>
<body>
 <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC">
                    Outright
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="ErrMsg" class="errMsg"></div>
        </div>
        <div class="bg_table">
            <div id="sContent" runat="server"></div>
            <input type="hidden" id="hdbettype" name="hdbettype" value="10" />
            <input type="hidden" id="userId" name="userId" value="<%=AppSession.AccId %>" />
        </div>
     </div>
    
   
</body>
</html>




