<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuperBalance.aspx.cs" Inherits="SuperBalance"
    Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <script type="text/javascript" src="../../js/common.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery.js"></script>
    <script src="../../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../../js/FrozenHeader.js" type="text/javascript"></script>
    <script src="../../js/Report/DownLineBalance/DownLineBalance.js" type="text/javascript"></script>
</head>
<body>
    <div class="main" style="">
        <div class="maincontent">
            <div id="divMainHeader" class="">
                <div id="left_tab1">
                    <ul>
                        <li class="current"><span>Super Balance</span></li>
                    </ul>
                </div>
            </div>
            <div class="maincontent_box">
                <asp:literal id="header" runat="server"></asp:literal>
                <asp:literal id="content" runat="server"></asp:literal>
            </div>
            <div class="bg_table">
                <div id="sData" style="width: 600px">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
