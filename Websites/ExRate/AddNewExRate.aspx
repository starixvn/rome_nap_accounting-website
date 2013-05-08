<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewExRate.aspx.cs" Inherits="ExRate_AddNewExRate" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/image.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/ExChangeRate.js?ver=<%=WebSettings.Version%>"></script>
    <script type="text/javascript" src="../js/common.js?ver=<%=WebSettings.Version%>"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                        Add New Exchange Rate
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg">
                </div>
                <asp:Literal ID="lblExrate" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <select id="selectStandardCurrency" class="w120" style="display: none" runat="server">
    </select>
    <input type="text" id="tbxStandardCurrency" class="w120" runat="server" onkeypress="javascript:return restrictLengthOfStandardCurrency(event);" />
    <script type="text/javascript">
        var isAlphanumeric = function (e) {
            var key;
            if (e.keyCode) {
                key = e.keyCode;
            }
            else if (e.which) {
                key = e.which;
            }
            return /[A-Za-z0-9 ]/.test(String.fromCharCode(key));
        };
        function restrictLengthOfStandardCurrency(event) {
            if (!isAlphanumeric(event)) {
                return true;
            }
            else {
                var target = event.target ? event.target : event.srcElement;
                if ($(target).val().length > 2) {
                    return false;
                }
                else {
                    return true;
                }
            }
        }
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
                $("#<%=selectStandardCurrency.ClientID %>").html("<option value=''>Select</option>" + $("#<%=selectStandardCurrency.ClientID %>").html());
                $("#<%=selectStandardCurrency.ClientID %>").get(0).selectedIndex = 0;
                //$("#<%=selectStandardCurrency.ClientID %>").show();
                $("#tbxMultipliedBy").attr("onkeypress", "return CheckDecNumber_ExRate(event);");
            }
        );
    </script>
</body>
</html>
