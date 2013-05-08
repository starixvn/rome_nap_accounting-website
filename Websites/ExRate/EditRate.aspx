<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditRate.aspx.cs" Inherits="EditRate" Title="Untitled Page" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>        
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>   
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../js/ExChangeRate.js?ver=<%=WebSettings.Version%>"></script>
    <script type="text/javascript" src="../js/common.js?ver=<%=WebSettings.Version%>"></script>  

    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    
</head>
<body style="margin:5px 10px 5px 5px">
<div class="main">
        <div class="maincontent">
              <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                     <asp:Literal ID="exrateEditHeader" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <asp:Literal ID="exrateEditContent" runat="server"></asp:Literal>
                <iframe name="frameUpdate" id="frameUpdate" style="height:0px;width:0px;display:none; margin:0px; padding:0px;"></iframe> 
            </div>
        </div>
    </div>
    <select id="selectStandardCurrency" class="w120" style="display: none" runat="server">
    </select>
    <input type="text" id="tbxStandardCurrency" class="w120" readonly="readonly" disabled="disabled" runat="server" />
    <script type="text/javascript">
        $(window).load(
            function ()
            {
                var IsNumeric = function (sText)
                {
                    var ValidChars = "0123456789.";
                    var IsNumber = true;
                    var Char;

                    for (i = 0; i < sText.length && IsNumber == true; i++)
                    {
                        Char = sText.charAt(i);
                        if (ValidChars.indexOf(Char) == -1)
                        {
                            IsNumber = false;
                        }
                    }
                    return IsNumber;
                };

                //$("#<%=selectStandardCurrency.ClientID %>").appendTo("#tdStandardCurrency");
                $("#<%=tbxStandardCurrency.ClientID %>").appendTo("#tdStandardCurrency");
                //$("#<%=selectStandardCurrency.ClientID %>").show();
                $("#tbxMultipliedBy").attr("onkeypress", "return CheckDecNumber_ExRate(event);");
            }
        );
    </script>
     <script type="text/javascript">
         var isEdited = false;
         function UpdateExrate(id) {
             if (InputValid() == true) {
                 var strC = confirm("Are you sure you want to update exchange rate of this currency?");
                 if (strC) {
                     document.fEdit.submit();
                 }
                 else {
                     return false;
                 }
             }
             return false;
         }
         function InputValid() {
             if (isEmpty("txtRate") || isString("txtRate")) {
                 alert_error("Invalid Rate.", "errMsg");
                 obj("txtRate").focus();
                 return false;
             }
             if (isEmpty("txtMinbet") || isString("txtMinbet")) {
                 alert_error("Invalid Min Bet.", "errMsg");
                 obj("txtMinbet").focus();
                 return false;
             }
             return true;
         }
         function UdpSuccess(Msg) {
             showAlertMessage("errMsg", Msg, 0);
             obj("divEdit").style.display = "none";
             obj("divClose").style.display = "";
             parent.reloadpage();
         }
         function UdpFail(Msg) {
             showAlertMessage("errMsg", Msg, 1);
             obj("divEdit").style.display = "none";
             obj("divClose").style.display = "";
         }
         
    </script>  
</body>
</html>