function trimSpace(str) {
    return str.replace(/\s/g, "");
};

function submitSearch(username) {
    txtusername = $('#txtuserName');
    if (username == null || username == "") {
        username = encodeURIComponent(escape(jQuery.trim(txtusername.val())));        

        if (username != "") {
            $('#leftPanel').attr({ src: 'CustInfo.aspx?username=' + username });
            Set_Cookie(accid + "_custname", unescape(username));
        }
        else {
            username = Get_Cookie(accid + "_custname");
            if (username != "") {
                $('#leftPanel').attr({ src: 'CustInfo.aspx?username=' + username });
            }
        }
    }
    else {
        
        username = username.replace("&amp;", "&");
        Set_Cookie(accid + "_custname", username);
        $('#leftPanel').attr({ src: 'CustInfo.aspx?username=' + jQuery.trim(escape(username)) });
    }

    if (!showleft) {
        leftright(); 
    }
    txtusername.val("");
}


function gotoURL(frmName, url) {   
   $('#' + frmName).attr({ src: encodeURI(url) });
}
 
function BlockUI(msg) {    
    if (msg == null || msg == "") {
        jQuery.blockUI({
            message: '<table class="blockUI"><tr><td class="tdloading"></td></tr></table>',
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 68) / 2 + 'px',
                left: ($(window).width() - 68) / 2 + 'px',
                border: 'none'
            }
        });
    } else {
        jQuery.blockUI({
            message: '<table class="blockUI"><tr><td class="tdloading"></td></tr><tr><td><b>' + msg + '</b></td></tr></table>',
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 68) / 2 + 'px',
                left: ($(window).width() - 68) / 2 + 'px',
                border: 'none'
            }
        });
    }
}

function MainMenu_Click(item, url) {
    url = trimSpace(url);

    $("#mainmenu .current").removeClass("current");
    $("#" + item.id).addClass("current");
    $("#" + item.id + " .current").css("outline", "0");
    if (url.indexOf(".aspx") > 0 || url.toLowerCase() != "n/a") {
        gotoURL('centerFr', url);
    }
}

function Submenu_Click(url) {
    //if (url.indexOf(".aspx") > 0) {
        gotoURL('centerFr', url);
    //}
}
    function KeyPress(e) {
        e = (e) ? e : window.event;
        var key = (e.keyCode) ? e.keyCode : e.which;
        if (key == 13) {
            submitSearch();
        }
        return;
    }

    function overlay(id) {
        el = $("#" + id);
        el.css("display", el.css("display") == "inline" ? "none" : "inline");
    }


    function changeiconleftright(showlefticon) {
        var btnleftright = $("#btnleftright");
        if (showlefticon) {
            btnleftright.removeClass("leftright");
            btnleftright.addClass("rightleft");
        }
        else {
            btnleftright.removeClass("rightleft");
            btnleftright.addClass("leftright");
        }
    }

    function leftright() {
        var leftpanel = $('.leftcontent');
        var rightpanel = $('.rightcontent');

        if (showleft) {
            leftcontent = 0;
            contentwidth = fullwidth - leftcontent;
            leftpanel.css("width", leftcontent + "px");
            rightpanel.css("width", contentwidth + "px");
            leftpanel.hide();
        }
        else {
            leftcontent = 273;
            contentwidth = fullwidth - leftcontent;
            leftpanel.css("width", leftcontent + "px");
            rightpanel.css("width", (contentwidth-3) + "px");
            leftpanel.show();
        }
        showleft = !showleft;
        changeiconleftright(showleft);
    }

    function update_online_status() {
        UpdateOnlineStatus.location.href = "Handlers/OnlineUpd.ashx";
    }