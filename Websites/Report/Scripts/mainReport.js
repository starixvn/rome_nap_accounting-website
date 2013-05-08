function trimSpace(str) {
    return str.replace(/\s/g, "");
};

var mainMenu = '';
var _isHideIBCPTRacing = false; 

function gotoURL(frmName, url) {
    $('#' + frmName).attr({ src: url });
}

function BlockUI(msg) {
    if (msg == null) {
        jQuery.blockUI({
            message: $('img#displayBox'),
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 100) / 2 + 'px',
                left: ($(window).width() - 100) / 2 + 'px',
                width: '100px',
                border: 'none'
            }
        });
    } else {
        jQuery.blockUI({
            message: '<table style="border:solid 2px #000;background-color:#fff;"><tr><td align="center"><img id="displayBox" src="../App_Themes/Images/loading51.gif" alt=""/></td></tr><tr><td><b>' + msg + '</b></td></tr></table>',
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 136) / 2 + 'px',
                left: ($(window).width() - 136) / 2 + 'px',
                border: 'none'
            }
        });
    }
}

function resizeIframe() {
    fheight = $('.wrapper').height() - topheight;
    $('#centerFr').height(fheight + "px");
    $('#leftPanel').height(fheight + "px");
}


function update_online_status() {
    UpdateOnlineStatus.location.href = "../Handlers/OnlineUpd.ashx";
}
function QueryData(queryString, preserveDuplicates) {

    // if a query string wasn't specified, use the query string from the URL
    if (queryString == undefined) {
        queryString = location.search ? location.search : '';
    }

    // remove the leading question mark from the query string if it is present
    if (queryString.charAt(0) == '?') queryString = queryString.substring(1);

    // check whether the query string is empty
    if (queryString.length > 0) {

        // replace plus signs in the query string with spaces
        queryString = queryString.replace(/\+/g, ' ');

        // split the query string around ampersands and semicolons
        var queryComponents = queryString.split(/[&;]/g);

        // loop over the query string components
        for (var index = 0; index < queryComponents.length; index++) {

            // extract this component's key-value pair
            var keyValuePair = queryComponents[index].split('=');
            var key = decodeURIComponent(keyValuePair[0]);
            var value = keyValuePair.length > 1
                       ? decodeURIComponent(keyValuePair[1])
                       : '';

            // check whether duplicates should be preserved
            if (preserveDuplicates) {

                // create the value array if necessary and store the value
                if (!(key in this)) this[key] = [];
                this[key].push(value);

            } else {

                // store the value
                this[key] = value;

            }

        }

    }

}
$(function () {
    (function ShowUrl() {
        var getData = new QueryData(top.location.search, true);
        if (typeof getData['menuUrl'] != 'undefined') {
            var fullDate = new Date()
            var currentDate = (fullDate.getMonth() + 1) + "/" + fullDate.getDate() + "/" + fullDate.getFullYear();
            $('#centerFr').attr("src", getData['menuUrl'] + "?fdate=" + currentDate + "&tdate=" + currentDate);
        }
    })();

    _isHideIBCPTRacing = $('#IsHideIBCPTRacing').val() == 'True';

    //Gen menu
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceMenu.svc/GetReportMenu",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            if (rs != null) {
                //LoadData(rs);
                renderMenu(rs, rs, 0);
                $('#mainmenu').html(mainMenu);
            }
            else {
                LogOut();
                setTimeout('RequestLogin()', 2000);
            }
        }
    });
});

function MainMenu_Click(item, url) {
    url = trimSpace(url);

    $("#mainmenu .mcurrent").removeClass("mcurrent");
    $("#" + item.id).addClass("mcurrent");
    $("#" + item.id + " .mcurrent").css("outline", "0");
    if (url.indexOf(".aspx") > 0) {
        gotoURL('centerFr', url);
    }
}

function Submenu_Click(url) {
    gotoURL('centerFr', url);
}

var currentParentId = 0;
function renderMenu(parent, all, level) {

    if (parent && parent.length > 0) {
        var url = '';
        var target = '';
        var parentId = '';
        var parentIndex = 0;
        var menuClass = '';
        var downIcon = '';


        if (level == 0)
            mainMenu += '<ul id="nav">';
        else
            mainMenu += '<ul>';

        var total = parent.length;
        for (var i = 0; i < total; i++) {
            var menu = parent[i];

            if (_isHideIBCPTRacing && (menu.FunctionName == '[HorseRacingforTote]' || menu.FunctionName == '[HorseRacingforOpenTote]')) {
                continue;
            }

            if (CheckIsMenuIgnore(menu.Name) == false) {
                var totalChild = menu.SubMenu.length;
                menuClass = '';
                parentId = 'id="pId' + parentIndex + '"';

                url = trimSpace(menu.Url);
                url = url.toLowerCase().indexOf('n/a') > -1 ? 'javascript:void(0)' : url;

                target = menu.Target == '' ? 'centerFr' : menu.Target;

                if (level >= 1) {
                    if (totalChild > 1) {
                        menuClass = ' class="detail"';
                    }
                    else if (totalChild == 1) {
                        menuClass = ' class="single detail"';
                    }
                }


                mainMenu += ('<li ' + (level == 0 ? parentId : '') + menuClass + '>\
                        <a target="' + target +
                        '" onclick="SubMenuClick(\'pId' + currentParentId + '\',\'' +
                        menu.Id + '\',\'' + menu.FunctionName + '\',\'' +
                        url + '\',\'' + menu.Name + '\')"><span>' +
                        menu.Name + '</span></a>');

                renderMenu(menu.SubMenu, all, level + 1);
                mainMenu += '</li>';


                if (level != 0) continue;
                parentIndex++;
                currentParentId = parentIndex;
            }

        }
        mainMenu += '</ul>';
    }
}

function SubMenuClick(parentId, menuId, functionName, menuUrl, menuName) {
    addParentActiveClass(parentId);
    var iframe = $("#centerFr");
    iframe.unbind("load");
    iframe.load(function () {
        jQuery.unblockUI();
        fanexLegendManager.initPage(menuId, functionName, menuName, "#previewReportLegendDialog");
        fanexLegendManager.getReportLegend(
            $("#centerFr").contents().find(".filterContent,.maincontent_box"),
            false);
        //        fanexLegendManager.getReportLegend(
        //            $("#centerFr").contents().find(".filterContent,.maincontent_box"),
        //            true);
    });

    iframe.attr("src", menuUrl);
};

function addParentActiveClass(parentId) {
    //BlockUI();
    $('#nav li').removeClass('mcurrent');
    $('#nav #' + parentId).addClass('mcurrent');
    //jQuery.unblockUI();
}

function LogOut() {
    $.ajax({
        type: "POST",
        url: "../PreLogin/LogOut.ashx",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {

        }
    });
}

function CheckIsMenuIgnore(nameName) {
    try {
        if (arrayMenuNameIgnore && arrayMenuNameIgnore.length > 0) {
            for (var k = 0; k < arrayMenuNameIgnore.length; k++) {
                if (arrayMenuNameIgnore[k] == nameName)
                    return true;
            }
        }
    } catch (e) {
    }

    return false;
};
