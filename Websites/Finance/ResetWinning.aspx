<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetWinning.aspx.cs" Inherits="Finance_ResetWinning" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/ViewLog.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/Tab.css?v=<%= Sunplus.Accounting.Core.Configurations.WebSettings.Version %>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
  
    <style type="text/css">
        .red{color: #f00;}
    </style>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Reset Winning
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">                
                <div id="alertboxscc"></div>
                <div id="errMsg"></div>
                
                <table class="tbllayout" cellspacing="1" style="width:600px !important;">
                    <tr class="title">
                        <td>       
                            Date                     
                        </td>
                        <td>
                            Amount
                        </td>
                        <td>
                            Balance
                        </td>
                        <td>
                            Transactions
                        </td>
                        <td>
                            Action
                        </td>                        
                    </tr>
                    <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
                </table>
            </div>
            <br />
            <div class="red b"> * Note: After resetting winning, if the Amount is still different from 0, please contact IT department for support!</div>
        </div>
    </div>
    
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../js/common.js?ver=<%=WebSettings.Version%>"></script>  
    <script src="../js/showhidepopup.js" type="text/javascript"></script>    
    <script type="text/javascript">
        $(window).load(function() {
        try { parent.jQuery.unblockUI(); } catch (e) { }
        });
        
        function ViewBalance(dateValue, amount,isreset) {
            var url = "Finance/FinanceTransfer.aspx?date=" + dateValue + "&Amount=" + amount + "&isreset=" + isreset;
            showPopup("Balance Transfer", url, "", "", 630, 330, true, 2000, null)
        }
        function ResetWinning(dateValue) {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: "ResetWinning.ashx",
                data: "date=" + dateValue,
                dataType: "text",
                success: function(data) {

                    if (data == "OK") {
                        $("#errMsg").html("");
                        $("#alertboxscc").html("<div class='alertboxscc'><div id='succMsg'>Transfer successfully!</div></div>");
                        setTimeout('location.reload(true);', 2000);
                    }
                    else {
                        $("#errMsg").html(data);
                        $("#alertboxscc").html("");
                        parent.jQuery.unblockUI();
                    }
                }
            });            
        }
    </script>
</body>
</html>
