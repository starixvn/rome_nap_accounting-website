<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PositionTakingCS.aspx.cs" Inherits="PositionTakingCS" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/PTSetting/casino.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jshashtable.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/common.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/PT/Casino/BettingLimit.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/PT/Casino/CasinoPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/showhidepopup.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    
    <script type="text/javascript">

        function BtnOKClick() {
            PTBlockUI();
            document.forms[0].submit();
            return true;
        }

        function ShowMessage(msg) {
            document.getElementById('err').innerHTML = msg;
            window.scroll(0, 0);
            PTUnBlockUI();
         
        }

        function ResetMaxTransfer(id) {
            
            var arrayCustID = document.getElementById('arrayCustID');

            if (arrayCustID && arrayCustID.value != '') {
                id = arrayCustID.value.replace('^', ',');
                while (id.indexOf('^') != -1) {
                    id = id.replace('^', ',');
                }
                id = id.substr(0, id.length - 1);
            }

            $.ajax(
             {
                 type: "POST",
                 url: "../Handlers/ResetMaxTransfer.ashx",
                 async: true,
                 data: "custid=" + id,
                 success: function (res) {
                     if (res == 'Ok') {

                         document.getElementById('lblAccumulatedWL').innerHTML = document.getElementById('hidTransferCredit').value;
                         alert("Successfully");
                     }
                     else {
                         alert(res);
                     }
                 }
                }
             );
        }        
    </script>

</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC"><%--<%=PageTitle%>--%><%=Resources.Language.lblUpdatePTCasino %></td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
           <div class="maincontent_box">
                <span id="err" class="errMsg"></span>
            </div>
            <div class="bg_table">
                <iframe name="InsertData" id="InsertData" style="display: none;" onload="PTUnBlockUI();"></iframe>
                <form id="frmUpdatePT" action="../Handlers/SynCasino.ashx?insert=<%= Request["insert"] %>&custId=<%=Request["custId"] %>"
                method="post" target="InsertData">
                <asp:Literal ID="ctrHTML" EnableViewState="false" runat="server"></asp:Literal>
                </form>
            </div>
        </div>
    </div>
   
</body>
</html>
