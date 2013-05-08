<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccAllStatement.aspx.cs"
    Inherits="Statement_AccAllStatement" Title="Statement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Statement" runat="server">
    <title id="Title1" runat="server"></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link rel="Stylesheet" type="text/css" href="../App_Themes/Report/Statement.css?v=2" />

    <script src="../js/jQuery/jquery.1.3.1.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
    function confirm_delete(refno) {
        if (confirm("Do you want to delete the transaction with Ref.No: " + refno + "?")) {
            if (confirm("To double check, do you want to delete the transaction with Ref.No: " + refno + "?")) {
                //return true;
                callAjax(refno);
            }
            else {
                return false;
            }
        }
        return false;
    }
    function callAjax(refno) {
        $.ajax({
            type: "POST",
            url: "DelTrans.ashx",
            data:'refno=' + refno,
            success: function(data) {
                if (data.indexOf("|")>-1)
                {
                    showAlertMessage("errMsg",data.split("|")[1],parseInt(data.split("|")[0]));
                    setTimeout("window.location.reload();",2000);
                }
            }
        });
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
                    <td class="maincontent_headerC">
                        <asp:Literal ID="ReportTitle" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg"></div>
            </div>
            <div class="bg_table">
                <asp:Literal ID="HtmlBodyLiteral" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</body>
</html>
