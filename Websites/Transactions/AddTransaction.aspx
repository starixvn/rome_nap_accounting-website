<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddTransaction.aspx.cs" Inherits="AddTransaction"
    Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="~/js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" />
    <link href="~/App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/LeftPanel/left_panel.css" rel="stylesheet" />
</head>
<body>
    <% var isWhiteLabel = Convert.ToBoolean(Request.QueryString["IsWhiteLabel"]); %>
    <div class="main">
        <div class="maincontent">
            <div id="divMainHeader" style="height: 27px; background-color: #fff; position: fixed; width: 100%; margin-top: -4px; padding-top: 10px;">
                <div class="box_header" style="width: 100%;">
                    <div class="left_tab" style="width: 100%">
                        <ul id="tabs">
                            <li id="tab-add-trans" class="current"><a href="javascript:;" onclick="addWhiteLabelTrans.switchTab(0);"><span><%=Resources.Language.AddTrans%></span></a></li>
                            <%if (isWhiteLabel)
                            {%>
                            <li id="tab-add-trans-white-label"><a href="javascript:;" onclick="addWhiteLabelTrans.switchTab(1);"><span><%=Resources.Language.AddWhiteLabelTrans%></span></a></li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </div>
            <div id="add-trans" class="maincontent_box " style="margin-top: 38px;">
                <div id="errMsg">
                </div>
                <div class="clear">
                </div>
                <form style="margin: 0px; padding: 0px;" id="fAddTxn" name="fAddTxn" method="post" action="../Handlers/AddTransaction.ashx?CustName=<%= Request.QueryString["CustName"] %>&CustID=<%= Request.QueryString["CustID"] %>&IsInternal=<%= Request.QueryString["IsInternal"]%>"
                    target="frameUpdate">
                    <asp:Literal ID="AddTxnContentHtml" runat="server"></asp:Literal>
                </form>
                <iframe name="frameUpdate" id="frameUpdate" frameborder="0" height="0" width="0"></iframe>
            </div>
            <%if (isWhiteLabel)
            {%>
                <div id="add-trans-white-label" class="maincontent_box" style="margin-top: 38px; display: none">
                    <div id="errMsgWL">
                    </div>
                    <div class="clear">
                    </div>
                    <form style="margin: 0px; padding: 0px;" id="fAddWLTxn" name="fAddWLTxn" method="post" action="../Handlers/AddWhiteLabelTransaction.ashx?CustName=<%= Request.QueryString["CustName"] %>&CustID=<%= Request.QueryString["CustID"] %>"
                        target="frameUpdateWL">
                        <asp:Literal ID="AddWLTxnContentHtml" runat="server"></asp:Literal>
                    </form>
                    <iframe name="frameUpdateWL" id="frameUpdateWL" frameborder="0" height="0" width="0"></iframe>
                </div>
            <%} %>
        </div>
    </div>

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../Scripts/_Libs/jQuery/jquery-ui-1.9.1.min.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="js/AddTransaction.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ValueAddValid() {
            if (isEmpty("txtDate")) {
                showAlertMessage("errMsg", "Please select date.", 1);
                obj("txtDate").focus();
                return false;
            }
            if (isEmpty("txtamount") || isString("txtamount")) {
                showAlertMessage("errMsg", "Invalid Amount.", 1);
                obj("txtamount").focus();
                return false;
            }
            return true;
        }

    </script>

</body>
</html>
