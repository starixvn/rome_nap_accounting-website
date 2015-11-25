<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/main_content.css" rel="stylesheet" type="text/css" />
    <link href="Styles/mainReport.css?version=<%=WebSettings.Version%>" rel="stylesheet"
        type="text/css" />
    <link href="Styles/Menu/menu.css?version=<%=WebSettings.Version%>" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" />
    <link rel="stylesheet" type="text/css" href="../js/jQuery/CustomScrollbar/Styles/scrollbars.css" />
    <link rel="stylesheet" type="text/css" href="../js/jQuery/CustomScrollbar/Styles/scrollbars-itunes.css" />
    <link rel="stylesheet" type="text/css" href="../js/jQuery/CustomScrollbar/Styles/scrollbars-custom.css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="Scripts/mainReport.js?version=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery-ui-1.8.20.custom.min.js" type="text/javascript">
    </script>
    <script src="../js/jQuery/Namespace/Namespace.js" type="text/javascript"></script>
    <script src="../js/jQuery/CrossdomainAjax/jquery.xdomainajax.js" type="text/javascript"></script>
    <script src="../js/jQuery/Cookie/jquery.cookie.js" type="text/javascript"></script>
    <script src="../js/jQuery/SHAHash/jquery.sha1.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jQuery/CustomScrollbar/jquery.event.drag-2.0.js"></script>
    <script type="text/javascript" src="../js/jQuery/CustomScrollbar/jquery.ba-resize.js"></script>
    <script type="text/javascript" src="../js/jQuery/CustomScrollbar/mousehold.js"></script>
    <script type="text/javascript" src="../js/jQuery/CustomScrollbar/jquery.mousewheel.js"></script>
    <script type="text/javascript" src="../js/jQuery/CustomScrollbar/aplweb.scrollbars.js"></script>
    <script type="text/javascript">
        //menu setting
        var listMenuIgnore = "<%=listMenuIgnore%>";
        var arrayMenuNameIgnore = listMenuIgnore.split(";");       
        var noteViewCountToCollapse = "<%=noteViewCountToCollapse%>";
        var isLegendEnable = "<%=isLegendEnable%>";
        var currentUserId = "<%=currentUserId%>";
        var inlineImageExpand = "<%=inlineImageExpand%>";
        var inlineImageCollapse = "<%=inlineImageCollapse%>";

        jQuery(document).ready(function () {
            pre_resize();
            $(window).resize(function () {
                pre_resize();
            });
        });

        function pre_resize() {
            var fullheight = $(".fullsize").height();
            var contentheight = fullheight - 44;
            $('#centerFr').height(contentheight + "px");
        }      
    </script>
</head>
<body>
    <script type="text/javascript">
        setInterval('update_online_status()', 30000);
    </script>
    <div id="fullsize" class="fullsize">
    </div>
    <div class="">
        <div id="iom-header" class="enhanced globalheader-loaded globalheader-js" aria-label="iom"
            role="navigation" style="visibility: visible;">
            <div id="mainmenu" style="margin-left: 2px;">
            </div>
            <div style="right: 6px; position: absolute; top: 8px; font-weight: bold; color: #FFFFFF;">
                <span class="userIcon">&nbsp</span><span style="color: #F68D55; text-align: left;">
                    <%=accUsername%></span>
            </div>
        </div>
        <div class="contentbox1">
            <iframe id="centerFr" name="centerFr" src="../_blankpage.htm" scrolling="auto" marginwidth="0"
                marginheight="0" frameborder="0" vspace="0" hspace="0" style="width: 100%; height: 0px;"
                allowtransparency="true"></iframe>
        </div>
        <div id="previewReportLegendDialog" class="hidden">
        </div>
    </div>

    <input type="hidden" id="IsHideIBCPTRacing" value="<%=Fanex.Tarus.Infrastructure.Configurations.WebSettings.IsHideIBCPTRacing %>" />

    <iframe id="UpdateOnlineStatus" name="UpdateOnlineStatus" width="0" height="0" style="display: none">
    </iframe>
</body>
</html>
