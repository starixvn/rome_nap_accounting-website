<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Bet Aggerator</title>
    <link href="Style/style.css" rel="stylesheet" type="text/css" />
    <link href="Style/superfish.css" rel="stylesheet" type="text/css" />
    <link href="Style/Main.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="Script/hoverIntent.js" type="text/javascript"></script>
    <script src="Script/superfish.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>

    <script src="Script/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        //menu setting     
        jQuery(document).ready(function () {
            pre_resize();
            $(window).resize(function () {
                pre_resize();
            });
            $('ul.sf-menu').superfish({ 
            delay:       1,                            // one second delay on mouseout 
            animation:   {opacity:'show',height:'show'},  // fade-in and slide-down animation 
            speed:       45,                          // faster animation speed 
            autoArrows:  false,                           // disable generation of arrow mark-up 
            dropShadows: true                            // disable drop shadows 
        }); 
        });

        function pre_resize() {
            var fullheight = $(".fullsize").height();
            var contentheight = fullheight - 49;
            $('#centerFr').height(contentheight + "px");
        }
        
        function activeMain(id)
        {
            try { BlockUI(); } catch (e) { }  
            $(".content_all .active").removeClass("active");
            $("#" + id).addClass("active");
        }      
    </script>

</head>
<body>
    <div id="fullsize" class="fullsize">
    </div>
    <div id="container">
        <div id="menu_row">
            <div style="left:8px; position:absolute; top:14px;">
                    <span style="color:#52d8f6; font-size:14px; font-weight:bold;">BA</span>
            </div>
            <div class="content_all">
                <%=topMenu%>
            </div>
             <div style="right:6px; position:absolute; top:8px">
                    <span class="text_user" style="color:#52d8f6;">Welcome <span class="text_org"><%=accUsername%></span></span>
            </div>
        </div>
        <div id="content">
            <iframe id="centerFr" name="centerFr" src="../_blankpage.htm" scrolling="auto" marginwidth="0"
                marginheight="0" frameborder="0" vspace="0" hspace="0" style="width: 100%; height: 0px;"
                allowtransparency="true"></iframe>
        </div>
    </div>
    <iframe id="UpdateOnlineStatus" name="UpdateOnlineStatus" width="0" height="0" style="display: none">
    </iframe>

    <script type="text/javascript">
            setInterval('update_online_status()', 30000);
    </script>

</body>
</html>
