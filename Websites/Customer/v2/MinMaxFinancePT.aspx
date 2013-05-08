<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="false" CodeFile="MinMaxFinancePT.aspx.cs" Inherits="Customer_V2_MinMaxFinancePT" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Min/Max PT</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../../js/jshashtable.js" type="text/javascript"></script>
    <script src="../../js/agent/FinancePT.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/agent/Customer.js" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    
</head>
<body>
    <div class="maincontent">
          <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                         <%=strtitle%>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
        <div class="maincontent_box">
            <div id="errMsg" class="errMsg"></div>
        </div>
        <div class="bg_table">
             <form id="form1" runat="server">
            <div align="left" style="padding-left:1px;">
                <asp:Literal ID="htmlContent" runat="server"></asp:Literal>
            </div>
            </form>
        </div>
    </div>
   
</body>
</html>
