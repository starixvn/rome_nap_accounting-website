<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MinMaxExRate.aspx.cs" Inherits="MinMaxExRate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/image.css"/>   
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />   
    <script type="text/javascript" src="../js/common.js"></script>

    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript">          
    
        function DoCal(url) {
            url = "ExRate/" + url;
            if (url.indexOf('Add')>0)
                showPopup('Add Min Max Exchange Rate', url, 0, 0, 500, 180, true, 2000, null);
            else
                showPopup('Edit Min Max Exchange Rate', url, 0, 0, 500, 180, true, 2000, null);
        }
        function checkishistorypage() {
            if (gup("ishis") != "") {
                document.getElementById("divfilter").style.display = "none";
            }
        }
    </script>
</head>
<body onload="checkishistorypage();">    
    <div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    <asp:Literal ID="MinMaxExchangeRate" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" id="divfilter">
                    <input type="button" class="btn"  onclick="DoCal('UpdateMinMaxExRate.aspx?status=Add');" value="Add Min Max Ex.Rate" /> 
            </div>
            <div class="bg_table">
                <asp:Literal ID="MinMaxExRateListContentHtml" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</body>
</html>