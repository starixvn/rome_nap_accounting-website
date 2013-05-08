<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index2.aspx.cs" Inherits="Report_index2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../App_Themes/Main/main_content.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/menu.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="Stylesheet" href="../js/jQuery/autocomplete/styles.css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>
    <link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <script src="../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../js/mainpage.js" type="text/javascript"></script>
    <script src="../js/mainmenu.js" type="text/javascript"></script>
</head>
<body>

<div id="button_mainreport">
    <div class="top_banner2">
        <div id="dvMenuTop">
            <ul style="list-style-type:none;padding:0px; margin:0px;">
                <li>
                    <div id="mainmenu">
                        <div style="clear:both;"></div>
                        <ul id="ulMenu">
                            <script type="text/html" id="tmplMenu">
                                <li id="item_${Id}">
                                    <a onclick="menuclick('item_${Id}',this, '${Url}')" href="javascript:void(0)"<span>${Name}</span></a>
                                </li>
                             </script>
                        </ul>
                    </div>
                </li>
                <li>
                    <div class="subtopmenu" id="submenu"></div>
                </li>
            </ul>
        </div>
    </div>
</div>

<script type="text/javascript">
    function GetMenuList() {
        menuList = null;
        
        $.ajax({
            type: "POST",
            url: "../Services/WCF/ServiceMenu.svc/ManageMenuGetMenuList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"system":"' + "ACC-REPORT" + '"}',
            success: function (rs) {
                $("#tmplMenu").tmpl(rs).appendTo("#ulMenu");
            }
        });
    }

    GetMenuList();
</script>


</body>
</html>
