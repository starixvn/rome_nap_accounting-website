<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinanceTransfer.aspx.cs" Inherits="Finance_FinanceTransfer" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/ViewLog.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Log/Tab.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form onsubmit="return ResetWinning('<%=strDate %>')">
    <div class="main">
        <div class="maincontent" style="padding: 5px;">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <%=strDate %>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="errMsg"></div>
                <div id="alertboxscc"></div>
                <table class="tbllayout" cellspacing="1">
                    <tr class="title">
                        <td style="width: 15px">&nbsp;</td>
                        <td style="width: 80px">
                            Type
                        </td>
                        <td>
                            Remark
                        </td>
                        <td style="width: 100px">
                            Amount
                        </td>                   
                    </tr>
                    <asp:Literal ID="ltrContent" runat="server"></asp:Literal>                    
                </table>
                <br />
                <div style="float:right; width: 300px; text-align: right;">
                    <input type="submit" value="Submit" class="btn" <%=strAllowUpdate %> />
                </div> 
            </div>
        </div>
    </div>
    </form>
    
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../js/common.js?ver=<%=WebSettings.Version%>"></script>  
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        function ResetWinning(dateValue) {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: "ResetWinning.ashx",
                data: "date=" + dateValue + "&txt1=" + $("#txt1").val() + "&txt2=" + $("#txt2").val() + "&txt3=" + $("#txt3").val() + "&txt4=" + $("#txt4").val(),
                dataType: "text",
                success: function(data) {

                    if (data == "OK") {
                        $("#errMsg").html("");
                        $("#alertboxscc").html("<div class='alertboxscc'><div id='succMsg'>Transfer successfully!</div></div>");
                        //afterUpdate(true, 2000);
                        if (parent.currentExecFrame.isreload) {
                            setTimeout('parent.currentExecFrame.location.reload(true);', 2000);
                        }
                        setTimeout('hidePopup()', 2000); 
                    }
                    else {
                        $("#errMsg").html(data);
                        $("#alertboxscc").html("");
                        parent.jQuery.unblockUI();
                    }
                }
            });
            return false;
        }
    </script>
</body>
</html>
