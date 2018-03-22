<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Others_Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1">
    <title></title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Log/Tab.css?v=<%= Sunplus.Accounting.Core.Configurations.WebSettings.Version %>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            margin: 2px 0 0 0;
        }
    </style>

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        var isDirectCustomer = <%=IsDirectCustomer%>;
        function PTBlockUI() {

            if (parent.BlockUI) {
                try { parent.BlockUI(); } catch (e) { }
            }
                //Called inside a frame set (#centerFr)
            else {
                if (parent.parent.BlockUI) {
                    parent.parent.BlockUI();
                }
            }
            //Called from popup
            if (opener) {
                if (opener.BlockUI) {
                    opener.BlockUI();
                }
                opener.parent.BlockUI();
            }
        }
        function PTUnBlockUI() {
    
            if (parent.jQuery && parent.jQuery.unblockUI) {
                parent.jQuery.unblockUI();
            }
            //Called inside a frame set (#centerFr)
            if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
       
                parent.parent.jQuery.unblockUI();
            }
            //Called from popup

            if (opener) {
                if (opener.parent.jQuery.unblockUI) {
                    opener.parent.jQuery.unblockUI();
                }
                //parent.jQuery.unblockUI();
            }
        }
        function LinkOnClick(link, url) {
            PTBlockUI();
            $(".current").removeClass("current");
            $("#" + link).addClass("current");
            parent.openPage(url);
        }
    </script>
</head>
<body>
    <asp:Literal ID="sMenu" runat="server"></asp:Literal>

    <script type="text/javascript">
        if (isDirectCustomer)
        {
            alert('Please fulfill the Extra Information via the Information tab on the left panel');
        }

        $(document).ready(function () {
            $('#menu-pt-after-insert-customer').find('li').eq(0).find('a').click();
        });
    </script>
</body>
</html>