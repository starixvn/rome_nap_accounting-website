<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExchangeRate.aspx.cs" Inherits="ExchangeRate" Title="Untitled Page" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/image.css"/>   
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />   
  
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/ExChangeRate.js?ver=<%=WebSettings.Version%>"></script>
    <script type="text/javascript" src="../js/common.js?ver=<%=WebSettings.Version%>"></script>  
    <script src="../js/showhidepopup.js" type="text/javascript"></script>  

    <script src="../js/jQuery/ajaxfileupload.js" type="text/javascript"></script>

    

    <script type="text/javascript">
        function ExportToExcel() {
            $('#IsExport').val('1');
            $('#form').submit();
        }
    </script>
</head>
<body>
<div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                     <asp:Literal ID="ExRateHeaderStr" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="ErrMsg"></div>
                <asp:Literal ID="ExRateAddContentHtml" runat="server"></asp:Literal> 
            </div>
            <div class="bg_table" id="sContent">
                <asp:Literal ID="ExRateListContentHtml" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        InitFixedHeaderEx();
    </script>
    <form id="form" method="post" action="ExchangeRate.aspx">
        <input type="hidden" id="IsExport" name="IsExport" value="0" />
    </form>
</body>
</html>