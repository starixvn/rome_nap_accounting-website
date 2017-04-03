<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="App_Themes/Main/main_content.css?v=1" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Main/menu.css?v=1" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/input.css?v=1" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="Stylesheet" href="js/jQuery/autocomplete/styles.css?v=1" />
    <link href="js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="Areas/CustomerSetting/Views/EnableThirdPartyProductStatus/Styles/EnableThirdPartyProductStatus.css?vs=<%=WebSettings.Version%>" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-widget-overlay {
            background: none #fff;
            opacity: 0.1;
        }
    </style>
    <script src="js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="js/jQuery/autocomplete/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="js/dialogbox.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/BlockUI.js" type="text/javascript"></script>
    <script src="js/mainpage.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/mainmenu.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/jscookies.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <link href="js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <script src="js/IBCWnd/IBCWnd.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/showhidepopup.js" type="text/javascript"></script>
    <script src="js/json2.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="js/jQuery/highlight/jquery.highlight-3.js" type="text/javascript"></script>
</head>
<body>
    <div id="fullsize" class="fullsize">
    </div>
    <div>
        <div class="bannerbox">
            <!--Banner-->
            <div class="top_banner2">
                <div id="left_topbanner2" style="float: left;">
                    <div class="welcomebox">
                        Welcome <span class="text_org">
                            <%=accUsername%></span>
                    </div>
                    <div id="Search_box">
                        <input name="txtuserName" value="" onkeypress="return KeyPress(event);" class="form_top autocomp"
                            type="text" id="txtuserName" value="" autocomplete="off" />
                        <input class="btn" id="btnSendMsg" onclick="submitSearch();" type="button" value="Search"
                            name="btnSendMsg" />
                        <input type="button" name="btnleftright" id="btnleftright" class="leftright" onclick="leftright();" />
                    </div>
                </div>
                <div id="button_main">
                    <!--Main Menu-->
                </div>
                <div class="accOption">
                    <a onclick="ShowHideSearchBox()" href="javascript:void(0);" class="text_user01" id="searchLink">Quick Link </a><span class="text_dif">|</span> <a onclick="gotoURL('centerFr','VerifyPassword.aspx')"
                        href="javascript:void(0);" class="text_user01">User Info</a> <span class="text_dif">|</span> <a href="PreLogin/LogOut.ashx" class="text_user01">Log out</a>
                </div>
            </div>
            <!--/Baner-->
        </div>
        <%--suggestion for search here--%>
        <div class="siteseachbox" style="display: none; z-index: 100;">
            <input type="text" id="tbxSiteSearchMenu" class="form_top" style="float: right; margin-right: 2px; width: 132px; padding-right: 16px;" />
            <span id="sitesearchIcon" class="ui-icon ui-icon-search" style="position: absolute; right: 4px; margin-top: 1px; cursor: pointer;"></span>
        </div>
        <div id="suggestionDiv" class="sitesearchboxsuggestion radiuscornerall" style="display: none;">
        </div>
        <div class="contentbox">
            <div class="leftcontent">
                <iframe id="leftPanel" name="leftPanel" src="_blankpage.htm" marginwidth="0" scrolling="auto"
                    marginheight="0" frameborder="0" vspace="0" hspace="0" style="width: 273px; height: 0px;"></iframe>
            </div>
            <div class="rightcontent">
                <iframe id="centerFr" name="centerFr" src="_blankpage.htm" scrolling="auto" marginwidth="0"
                    marginheight="0" frameborder="0" vspace="0" hspace="0" style="width: 100%; height: 0px;"></iframe>
            </div>
        </div>
    </div>
    <iframe id="UpdateOnlineStatus" name="UpdateOnlineStatus" width="0" height="0" style="display: none"></iframe>
    <%--template for quick link suggestion--%>
    <script id="suggestionSiteSearchTemplate" type="text/html">
        <div class="childInforDiv" style="margin-left: 3px; margin-bottom: 0px; border-bottom: 1px dashed #7C98A4;">
            <a class="linkDirectToPage" href="${Url}" isreportmenu="${IsReportMenu}" linktarget="${Target}">${Name} </a>
            <input type="hidden" value="${AncestorId}" />
            <br />
            <span style="color: #009933; font-size: normal;">${ServerPath} </span>
        </div>
    </script>
    <script type="text/javascript">
        //menu setting
        setInterval('update_online_status()', 45000);
        var accid = "<%=accUsername%>"; //use for write and read cookie
        var timerId = "";
        var listMenuIgnore = "<%=listMenuIgnore%>";
        var arrayMenuNameIgnore = listMenuIgnore.split(";");
        var isonauto = false;
        var showleft = false;
        var fullheight, contentheight, fullwidth, contentwidth;
        var bannerbox = 43;
        var leftcontent = 0;
        var autosearch;

        //Top menu        
        var accMenu = new Array();
        var subaccMenu = new Array();

        jQuery(document).ready(function () {

            pre_resize();
            $(".subtopmenu").width((fullwidth - 280) + "px");

            //Get cookies
            var username = Get_Cookie(accid + "_custname");
            var txtusername = $('#txtuserName');
            if (username != null && username != "") {
                //txtusername.val(jQuery.trim(username));
                $('#leftPanel').attr({ src: 'CustInfo.aspx?username=' + jQuery.trim(escape(username)) });
                if (!showleft) {
                    leftright();
                }
            }

            //Auto complete code
            autosearch = txtusername.autocomplete({ serviceUrl: 'Handlers/QueryUserName.ashx', minChars: 2 });
            $('.autocomplete').mouseover(function () {
                isonauto = true;
            }).mouseout(function () {
                isonauto = false;
            });
            //Add events to page
            $(window).resize(function () {
                pre_resize();
            });

            txtusername.blur(function () {
                if (!isonauto) {
                    $('#Autocomplete_1').css('display', 'none');
                }
                else {
                    txtusername.focus();
                }

            });


            $("#btnleftright").mouseover(function () {
                $(this).attr('class', $(this).attr('class').replace("rightleft", "rightleft_over").replace("leftright", "leftright_over"));
            });
            $("#btnleftright").mouseout(function () {
                $(this).attr('class', '');
                if (showleft)
                    $(this).addClass("rightleft");
                else
                    $(this).addClass("leftright");

            });

            //Gen menu

            $.ajax({
                type: "POST",
                url: "Services/WCF/ServiceMenu.svc/GetMenuList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '',
                success: function (rs) {
                    if (rs != null) {
                        LoadData(rs);
                    }
                    else {
                        LogOut();
                        setTimeout('RequestLogin()', 2000);
                    }
                }
            });


        });

        function RequestLogin() {
            window.location.href = "SignIn.aspx";
        }

        function LogOut() {
            $.ajax({
                type: "POST",
                url: "PreLogin/LogOut.ashx",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '',
                success: function (rs) {

                }
            });
        }

        function pre_resize() {
            fullheight = $(".fullsize").height();
            fullwidth = $(".fullsize").width();
            contentheight = fullheight - bannerbox;
            contentwidth = fullwidth - leftcontent;


            $(".contentbox").height(contentheight + "px");
            $(".rightcontent").width((contentwidth - 3) + "px");

            $('#centerFr').height(contentheight + "px");
            $('#leftPanel').height(contentheight + "px");
        }


        (function ($) {
            var cache = [];
            // Arguments are image paths relative to the current page.
            $.preLoadImages = function () {
                var args_len = arguments.length;
                for (var i = args_len; i--;) {
                    var cacheImage = document.createElement('img');
                    cacheImage.src = arguments[i];
                    cache.push(cacheImage);
                }
            }
        })(jQuery)
        jQuery.preLoadImages("App_Themes/Images/Banner/out.gif", "App_Themes/Images/Banner/in.gif");

        function CheckIsMenuIgnore(nameName) {
            for (var k = 0; k < arrayMenuNameIgnore.length; k++) {
                if (arrayMenuNameIgnore[k] == nameName)
                    return true;
            }
            return false
        };

        function LoadData(rs) {
            var SettingContent = $("#button_main");
            SettingContent.setTemplateURL("App_Templates/Default/MainMenu/MenuTop.htm");
            SettingContent.processTemplate(rs);
            var arr;
            var sarr;


            var total = rs.length;
            if (total > 0) {
                for (var i = 0; i < total; i++) //Top menu
                {
                    var mainid = rs[i].Id;
                    if (rs[i].SubMenu != null && rs[i].SubMenu.length > 0) {
                        var sub1 = rs[i].SubMenu;
                        var total1 = sub1.length;
                        arr = 'item_' + mainid;
                        accMenu[arr] = new Array();
                        for (var j = 0; j < total1; j++) {
                            if (CheckIsMenuIgnore(sub1[j].Name) == false) {
                                if (sub1[j].SubMenu == null || sub1[j].SubMenu.length == 0) {
                                    accMenu[arr].push(sub1[j].Name + "|" + trimSpace(sub1[j].Url));
                                }
                                else {
                                    accMenu[arr].push(sub1[j].Name + "|sMenu" + mainid + "_" + sub1[j].Id);
                                    var sub2 = sub1[j].SubMenu;
                                    var total2 = sub2.length;

                                    sarr = 'sMenu' + mainid + "_" + sub1[j].Id;
                                    subaccMenu[sarr] = new Array();
                                    for (var z = 0; z < total2; z++) {
                                        if (CheckIsMenuIgnore(sub2[z].Name) == false) {
                                            subaccMenu[sarr].push(sub2[z].Name + "|" + trimSpace(sub2[z].Url));
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
        $(document).ready(function () {
            var getURLParam = function (strParamName) {
                var strReturn = null;
                var strHref = window.location.href;
                if (strHref.indexOf("?") > -1) {
                    var strQueryString = strHref.substr(strHref.indexOf("?")).toLowerCase();
                    var aQueryString = strQueryString.split("&");
                    for (var iParam = 0; iParam < aQueryString.length; iParam++) {
                        if (aQueryString[iParam].indexOf(strParamName.toLowerCase() + "=") > -1) {
                            var aParam = aQueryString[iParam].split("=");
                            strReturn = aParam[1];
                            break;
                        }
                    }
                }
                if (strReturn == null)
                    return null;
                return unescape(strReturn);
            };
            var uname = getURLParam('uname');
            if (uname != null && uname != '') {
                submitSearch(uname);
            }
        });
    </script>
</body>
</html>
