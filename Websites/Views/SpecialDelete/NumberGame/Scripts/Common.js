// Modal popup library is compressed
var ModalPopupsDefaults = { shadow: true, shadowSize: 5, shadowColor: "#999999", backgroundColor: "#FAFBFC", borderColor: "#999999", titleBackColor: "#46a3ca", titleFontColor: "#ffffff", popupBackColor: "#ECECEC", popupFontColor: "black", footerBackColor: "#ECECEC", footerFontColor: "#15428B", okButtonText: "OK", yesButtonText: "Yes", noButtonText: "No", cancelButtonText: "Cancel", fontFamily: "Tahoma,Arial,helvetica,sans-serif", fontSize: "8pt" }; var ModalPopups = { Init: function () { }, SetDefaults: function (a) { a = a || {}; ModalPopupsDefaults.shadow = a.shadow != null ? a.shadow : ModalPopupsDefaults.shadow; ModalPopupsDefaults.shadowSize = a.shadowSize != null ? a.shadowSize : ModalPopupsDefaults.shadowSize; ModalPopupsDefaults.shadowColor = a.shadowColor != null ? a.shadowColor : ModalPopupsDefaults.shadowColor; ModalPopupsDefaults.backgroundColor = a.backgroundColor != null ? a.backgroundColor : ModalPopupsDefaults.backgroundColor; ModalPopupsDefaults.borderColor = a.borderColor != null ? a.borderColor : ModalPopupsDefaults.borderColor; ModalPopupsDefaults.okButtonText = a.okButtonText != null ? a.okButtonText : ModalPopupsDefaults.okButtonText; ModalPopupsDefaults.yesButtonText = a.yesButtonText != null ? a.yesButtonText : ModalPopupsDefaults.yesButtonText; ModalPopupsDefaults.noButtonText = a.noButtonText != null ? a.noButtonText : ModalPopupsDefaults.noButtonText; ModalPopupsDefaults.cancelButtonText = a.cancelButtonText != null ? a.cancelButtonText : ModalPopupsDefaults.cancelButtonText; ModalPopupsDefaults.titleBackColor = a.titleBackColor != null ? a.titleBackColor : ModalPopupsDefaults.titleBackColor; ModalPopupsDefaults.titleFontColor = a.titleFontColor != null ? a.titleFontColor : ModalPopupsDefaults.titleFontColor; ModalPopupsDefaults.popupBackColor = a.popupBackColor != null ? a.popupBackColor : ModalPopupsDefaults.popupBackColor; ModalPopupsDefaults.popupFontColor = a.popupFontColor != null ? a.popupFontColor : ModalPopupsDefaults.popupFontColor; ModalPopupsDefaults.footerBackColor = a.footerBackColor != null ? a.footerBackColor : ModalPopupsDefaults.footerBackColor; ModalPopupsDefaults.footerFontColor = a.footerFontColor != null ? a.footerFontColor : ModalPopupsDefaults.footerFontColor; ModalPopupsDefaults.fontFamily = a.fontFamily != null ? a.fontFamily : ModalPopupsDefaults.fontFamily; ModalPopupsDefaults.fontSize = a.fontSize != null ? a.fontSize : ModalPopupsDefaults.fontSize }, Alert: function (f, d, b, a) { a = a || {}; if (!d) { d = "Alert" } a.buttons = "ok"; a.okButtonText = a.okButtonText != null ? a.okButtonText : ModalPopupsDefaults.okButtonText; var e = ModalPopups._createAllLayers(f, d, b, a); var c = e[4]; c.innerHTML = b; ModalPopups._styleAllLayers(f, a, e) }, Confirm: function (f, d, a, b) { b = b || {}; if (!d) { d = "Confirm" } b.buttons = "yes,no"; b.yesButtonText = b.yesButtonText != null ? b.yesButtonText : ModalPopupsDefaults.yesButtonText; b.noButtonText = b.noButtonText != null ? b.noButtonText : ModalPopupsDefaults.noButtonText; var e = ModalPopups._createAllLayers(f, d, a, b); var c = e[4]; c.innerHTML = a; ModalPopups._styleAllLayers(f, b, e) }, YesNoCancel: function (f, d, a, b) { b = b || {}; if (!d) { d = "YesNoCancel" } b.buttons = "yes,no,cancel"; b.yesButtonText = b.yesButtonText != null ? b.yesButtonText : ModalPopupsDefaults.yesButtonText; b.noButtonText = b.noButtonText != null ? b.noButtonText : ModalPopupsDefaults.noButtonText; b.cancelButtonText = b.cancelButtonText != null ? b.cancelButtonText : ModalPopupsDefaults.cancelButtonText; var e = ModalPopups._createAllLayers(f, d, a, b); var c = e[4]; c.innerHTML = a; ModalPopups._styleAllLayers(f, b, e) }, Prompt: function (h, f, a, d) { d = d || {}; if (!f) { f = "Prompt" } d.buttons = "ok,cancel"; d.okButtonText = d.okButtonText != null ? d.okButtonText : "OK"; d.cancelButtonText = d.cancelButtonText != null ? d.cancelButtonText : "Cancel"; var g = ModalPopups._createAllLayers(h, f, a, d); var e = g[4]; var c = ""; if (d.width != null) { c = "width:95%;" } var b = a + "<br/>"; b += "<input type=text id='" + h + "_promptInput' value='' style='border: solid 1px #859DBE; " + c + "'>"; e.innerHTML = b; ModalPopups._styleAllLayers(h, d, g); ModalPopupsSupport.findControl(h + "_promptInput").focus() }, GetPromptInput: function (b) { var a = ModalPopupsSupport.findControl(b + "_promptInput"); return a }, GetPromptResult: function (b) { var a = ModalPopupsSupport.findControl(b + "_promptInput"); return a }, GetCustomControl: function (a) { return ModalPopupsSupport.findControl(a) }, Indicator: function (f, d, b, a) { a = a || {}; if (!d) { d = "Indicator" } if (a.buttons == null) { a.buttons = "" } var e = ModalPopups._createAllLayers(f, d, b, a); var c = e[4]; c.innerHTML = b; ModalPopups._styleAllLayers(f, a, e) }, Custom: function (f, d, b, a) { a = a || {}; if (!d) { d = "Custom" } if (a.buttons == null) { alert("buttons is a required parameter. ie: buttons: 'yes,no' or buttons: 'ok'.\nPossible buttons are yes, no, ok, cancel"); return } var e = ModalPopups._createAllLayers(f, d, b, a); var c = e[4]; c.innerHTML = b; ModalPopups._styleAllLayers(f, a, e) }, Close: function (a) { window.onresize = null; window.onscroll = null; document.body.removeChild(ModalPopupsSupport.findControl(a + "_background")); document.body.removeChild(ModalPopupsSupport.findControl(a + "_popup")); document.body.removeChild(ModalPopupsSupport.findControl(a + "_shadow")) }, Cancel: function (a) { ModalPopups.Close(a) }, _zIndex: 10000, _createAllLayers: function (l, t, h, e) { var i = ModalPopupsSupport.makeLayer(l + "_background", true, null); var r = ModalPopupsSupport.makeLayer(l + "_popup", true, null); var b = ModalPopupsSupport.makeLayer(l + "_shadow", true, null); var n = ModalPopupsSupport.makeLayer(l + "_popupTitle", true, r); var d = ModalPopupsSupport.makeLayer(l + "_popupBody", true, r); var m = ModalPopupsSupport.makeLayer(l + "_popupFooter", true, r); var j = e.okButtonText != null ? e.okButtonText : ModalPopupsDefaults.okButtonText; var p = e.yesButtonText != null ? e.yesButtonText : ModalPopupsDefaults.yesButtonText; var c = e.noButtonText != null ? e.noButtonText : ModalPopupsDefaults.noButtonText; var o = e.cancelButtonText != null ? e.cancelButtonText : ModalPopupsDefaults.cancelButtonText; var s = e.onOk != null ? e.onOk : 'ModalPopups.Close("' + l + '");'; var a = e.onYes != null ? e.onYes : 'ModalPopups.Close("' + l + '");'; var k = e.onNo != null ? e.onNo : 'ModalPopups.Close("' + l + '");'; var g = e.onCancel != null ? e.onCancel : 'ModalPopups.Close("' + l + '");'; n.innerHTML = "<div style='float: left'><b>" + t + "</b></div><div class='closePopup' onclick='javascript:Popups.Close();'></div>"; m.innerHTML = ""; e.fontFamily = e.fontFamily != null ? e.fontFamily : ModalPopupsDefaults.fontFamily; var q = e.buttons.split(","); for (x in q) { if (q[x] == "ok") { m.innerHTML += "<input name='" + l + "_okButton' id='" + l + "_okButton' type=button value='" + j + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px;' onclick='" + s + "'/>" } if (q[x] == "yes") { m.innerHTML += "<input name='" + l + "_yesButton' id='" + l + "_yesButton' type=button value='" + p + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px; cursor: pointer;' onclick='" + a + "'/>" } if (q[x] == "no") { m.innerHTML += "<input name='" + l + "_noButton' id='" + l + "_noButton' type=button value='" + c + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px; cursor: pointer;' onclick='" + k + "'/>" } if (q[x] == "cancel") { m.innerHTML += "<input name='" + l + "_cancelButton' id='" + l + "_cancelButton' type=button value='" + o + "' style='font-family:Verdana,Arial; font-size:8pt; border: solid 1px #859DBE; background-color: white; width:75px; height:20px; margin-right: 5px; margin-left: 5px;' onclick='" + g + "'/>" } } var f = new Array(i, r, b, n, d, m); if (e.autoClose != null) { setTimeout('ModalPopups.Close("' + l + '");', e.autoClose) } return f }, _styleAllLayers: function (n, e, h) { var i = h; var k = i[0]; var t = i[1]; var a = i[2]; var r = i[3]; var d = i[4]; var p = i[5]; ModalPopups._zIndex += 3; var m = ModalPopups._zIndex; e.borderColor = e.borderColor != null ? e.borderColor : ModalPopupsDefaults.borderColor; var w = "display:inline; position:absolute; z-index: " + (m) + "; left:0px; top:0px; width:100%; height:100%; filter:alpha(opacity=70); opacity:0.7;"; if (ModalPopupsSupport.isOlderIE()) { var s = ModalPopupsSupport.getViewportDimensions(); w = "display:inline; position:absolute; z-index: 10; left:0px; top:0px; width:" + s.width + "px; height:" + s.height + "px; filter:alpha(opacity=70); opacity:0.7; overflow:hidden;" } var f = "display:inline; position:absolute; z-index: " + (m + 1) + ";"; var u = "display:inline; position:absolute; z-index: " + (m + 2) + "; background-color:white; color:black; border:solid 1px " + e.borderColor + "; padding:1px;"; e.backgroundColor = e.backgroundColor != null ? e.backgroundColor : ModalPopupsDefaults.backgroundColor; w += " background-color:" + e.backgroundColor + ";"; e.fontFamily = e.fontFamily != null ? e.fontFamily : ModalPopupsDefaults.fontFamily; e.fontSize = e.fontSize != null ? e.fontSize : ModalPopupsDefaults.fontSize; var c = "position: absolute; font-family:" + e.fontFamily + "; font-size:" + e.fontSize + "; padding: 5px; text-align:left;"; var l = "position: absolute; font-family:" + e.fontFamily + "; font-size:" + e.fontSize + "; padding: 5px; text-align:left;"; var v = "position: absolute; font-family:" + e.fontFamily + "; font-size:" + e.fontSize + "; padding: 5px; text-align:center;"; if (ModalPopupsSupport.isIE) { r.style.cssText = c; d.style.cssText = l; p.style.cssText = v } else { r.setAttribute("style", c); d.setAttribute("style", l); p.setAttribute("style", v) } e.titleBackColor = e.titleBackColor != null ? e.titleBackColor : ModalPopupsDefaults.titleBackColor; e.titleFontColor = e.titleFontColor != null ? e.titleFontColor : ModalPopupsDefaults.titleFontColor; e.popupBackColor = e.popupBackColor != null ? e.popupBackColor : ModalPopupsDefaults.popupBackColor; e.popupFontColor = e.popupFontColor != null ? e.popupFontColor : ModalPopupsDefaults.popupFontColor; e.footerBackColor = e.footerBackColor != null ? e.footerBackColor : ModalPopupsDefaults.footerBackColor; e.footerFontColor = e.footerFontColor != null ? e.footerFontColor : ModalPopupsDefaults.footerFontColor; c += " background-color:" + e.titleBackColor + ";"; c += " color:" + e.titleFontColor + ";"; l += " background-color:" + e.popupBackColor + ";"; l += " color:" + e.popupFontColor + ";"; v += " background-color:" + e.footerBackColor + ";"; v += " color:" + e.footerFontColor + ";"; var j = 0; if (ModalPopupsSupport.getLayerWidth(r.id) > j) { j = ModalPopupsSupport.getLayerWidth(r.id) } if (ModalPopupsSupport.getLayerWidth(d.id) > j) { j = ModalPopupsSupport.getLayerWidth(d.id) } if (ModalPopupsSupport.getLayerWidth(p.id) > j) { j = ModalPopupsSupport.getLayerWidth(p.id) } var b = ModalPopupsSupport.getLayerHeight(r.id) + ModalPopupsSupport.getLayerHeight(d.id) + ModalPopupsSupport.getLayerHeight(p.id); e.width = e.width != null ? e.width : (j + 4); e.height = e.height != null ? e.height : b; var o = ModalPopupsSupport.getLayerHeight(d.id); if (e.height > b) { o = e.height - ModalPopupsSupport.getLayerHeight(r.id) - ModalPopupsSupport.getLayerHeight(p.id); l += " height:" + o + "px;"; b = ModalPopupsSupport.getLayerHeight(r.id) + o + ModalPopupsSupport.getLayerHeight(p.id) } c += " top:1px;"; l += " top:" + ModalPopupsSupport.getLayerHeight(r.id) + "px;"; v += " top:" + (ModalPopupsSupport.getLayerHeight(r.id) + (o)) + "px;"; c += " width:" + (e.width - 10) + "px;"; l += " width:" + (e.width - 10) + "px;"; v += " width:" + (e.width - 10) + "px;"; var q = ModalPopupsSupport.getFrameWidth(); var g = ModalPopupsSupport.getFrameHeight(); if (e.height < b) { e.height = b } e.top = e.top != null ? e.top : ((g / 2) - (e.height / 2)); e.left = e.left != null ? e.left : ((q / 2) - (e.width / 2)); c += " left:1px;"; l += " left:1px;"; v += " left:1px;"; if (e.width) { u += " width:" + e.width + "px;" } else { u += " width:" + e.maxWidth + "px;" } if (e.height) { u += " height:" + (e.height - 1) + "px;" } else { u += " height:" + (b - 1) + "px;" } if (ModalPopupsSupport.isIE) { r.style.cssText = c; d.style.cssText = l; p.style.cssText = v } else { r.setAttribute("style", c); d.setAttribute("style", l); p.setAttribute("style", v) } e.shadow = e.shadow != null ? e.shadow : ModalPopupsDefaults.shadow; e.shadowSize = e.shadowSize != null ? e.shadowSize : ModalPopupsDefaults.shadowSize; if (e.shadow) { e.shadowSize = e.shadowSize != null ? e.shadowSize : ModalPopupsDefaults.shadowSize; e.shadowColor = e.shadowColor != null ? e.shadowColor : ModalPopupsDefaults.shadowColor; f += "background-color:" + e.shadowColor + ";"; if (e.width) { f += " width:" + e.width + "px;" } else { f += " width:" + maxWidth + "px;" } if (e.height) { f += " height:" + (e.height - 1) + "px;" } else { f += " height:" + (b) + "px;" } } else { f += " display:none;" } if (ModalPopupsSupport.isIE) { t.style.cssText = u; a.style.cssText = f; k.style.cssText = w } else { t.setAttribute("style", u); a.setAttribute("style", f); k.setAttribute("style", w) } if (!ModalPopupsSupport.isOlderIE()) { ModalPopupsSupport.centerElement(document.getElementById(n + "_background"), 0, true) } else { var s = ModalPopupsSupport.getViewportDimensions(); k.innerHTML = "<div><iframe style='z-index:-1; position:absolute; top:0;left:0 display:none; display/**/:block; position:absolute; filter:mask(); width:" + s.width + "px; height:" + s.height + "px;' id='corr_bug_ie' src='../common/imgLay/spinner.gif'></iframe></div>" } ModalPopupsSupport.centerElement(document.getElementById(n + "_popup"), 0, false); if (e.shadow) { ModalPopupsSupport.centerElement(document.getElementById(n + "_shadow"), e.shadowSize, false) } e.loadTextFile = e.loadTextFile != null ? e.loadTextFile : ""; if (e.loadTextFile != "") { ModalPopups._loadTextFile(n, e, h, e.loadTextFile) } window.onresize = function () { ModalPopupsSupport.centerElement(document.getElementById(n + "_background"), 0, true); ModalPopupsSupport.centerElement(document.getElementById(n + "_popup"), 0, false); if (e.shadow) { ModalPopupsSupport.centerElement(document.getElementById(n + "_shadow"), e.shadowSize, false) } }; window.onscroll = function () { ModalPopupsSupport.centerElement(document.getElementById(n + "_background"), 0, true); ModalPopupsSupport.centerElement(document.getElementById(n + "_popup"), 0, false); if (e.shadow) { ModalPopupsSupport.centerElement(document.getElementById(n + "_shadow"), e.shadowSize, false) } } }, _loadTextFile: function (e, d, c, a) { var b = ModalPopupsSupport.getXmlHttp(); b.open("GET", a, true); b.onreadystatechange = function () { if (b.readyState == 4) { var f = b.responseText.replace("\r\n", "<br>").replace("\n\r", "<br>").replace("\n", "<br>").replace("\r", "<br>"); var g = "<div style='overflow-y: scroll; position:absolute; top:5px; left:5px; height:" + (d.height - 65) + "px; width:" + (d.width - 10) + "px;'>"; g += f; g += "</div>"; ModalPopups.GetCustomControl(e + "_popupBody").innerHTML = g; d.loadTextFile = ""; ModalPopups._styleAllLayers(e, d, c) } }; b.send(null) } }; var ModalPopupsSupport = { isIE: function () { return (window.ActiveXObject) ? true : false }, isOlderIE: function () { var a = -1; if (navigator.appName == "Microsoft Internet Explorer") { var b = navigator.userAgent; var c = new RegExp("MSIE ([0-9]{1,}[.0-9]{0,})"); if (c.exec(b) != null) { a = parseFloat(RegExp.$1) } } if (a > -1 && a < 7) { return true } else { return false } }, makeLayer: function (d, b, c) { var a = document.createElement("div"); a.id = d; if (c) { c.appendChild(a) } else { document.body.appendChild(a) } return a }, deleteLayer: function (b) { var a = findLayer(b); if (a) { document.body.removeChild(a) } }, findLayer: function (a) { return document.all ? document.all[a] : document.getElementById(a) }, findControl: function (b, a) { if (a == null) { return document.all ? document.all[b] : document.getElementById(b) } else { return document.all ? document.all[b] : document.getElementById(b) } }, getLayerHeight: function (a) { if (document.all) { gh = document.getElementById(a).offsetHeight } else { gh = document.getElementById(a).offsetHeight } return gh }, getLayerWidth: function (a) { gw = document.getElementById(a).offsetWidth; return gw }, getViewportDimensions: function () { var b = 0, a = 0; if (self.innerHeight) { b = window.innerHeight; a = window.innerWidth } else { if (document.documentElement && document.documentElement.clientHeight) { b = document.documentElement.clientHeight; a = document.documentElement.clientWidth } else { if (document.body) { b = document.body.clientHeight; a = document.body.clientWidth } } } return { height: parseInt(b, 10), width: parseInt(a, 10)} }, getScrollXY: function () { var b = 0, a = 0; if (typeof (window.pageYOffset) == "number") { a = window.pageYOffset; b = window.pageXOffset } else { if (document.body && (document.body.scrollLeft || document.body.scrollTop)) { a = document.body.scrollTop; b = document.body.scrollLeft } else { if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) { a = document.documentElement.scrollTop; b = document.documentElement.scrollLeft } } } return [b, a] }, centerElement: function (d, g, c) { var b = ModalPopupsSupport.getViewportDimensions(); var f = (b.width == 0) ? 50 : parseInt((b.width - d.offsetWidth) / 2, 10); var e = (b.height == 0) ? 50 : parseInt((b.height - d.offsetHeight) / 2, 10); var a = ModalPopupsSupport.getScrollXY(); if (!c) { d.style.left = (f + g) + "px" } d.style.top = (e + g + a[1]) + "px"; b, f, e, d = null }, readFile: function (c, a) { var b = getXmlHttp(); var d = c + "?r=" + Math.random(); b.open("GET", d, true); b.onreadystatechange = function () { if (b.readyState == 4) { a.innerHTML = b.responseText } }; b.send(null) }, getFrameWidth: function () { var a = document.documentElement.clientWidth; if (self.innerWidth) { a = self.innerWidth } else { if (document.documentElement && document.documentElement.clientWidth) { a = document.documentElement.clientWidth } else { if (document.body) { a = document.body.clientWidth } else { return } } } return a }, getFrameHeight: function () { var a = document.documentElement.clientHeight; if (self.innerWidth) { a = self.innerHeight } else { if (document.documentElement && document.documentElement.clientWidth) { a = document.documentElement.clientHeight } else { if (document.body) { a = document.body.clientHeight } else { return } } } return a }, getXmlHttp: function () { var a; try { a = new XMLHttpRequest() } catch (b) { try { a = new ActiveXObject("Msxml2.XMLHTTP") } catch (b) { try { a = new ActiveXObject("Microsoft.XMLHTTP") } catch (b) { alert("Your browser does not support AJAX!"); return false } } } return a } };

var Popups = {
    Confirm: function (msg) {
        var title = 'CONFIRMATION';
        ModalPopups.Confirm("idConfirm",
                title,
                "<div style='padding: 10px;'>" +
                "<div class='popup_confirm'>" + msg + "</div>" +
                "<div style='height:10px'></div>" +
                "</div>",
                {
                    yesButtonText: 'YES',
                    noButtonText: 'NO',
                    onYes: "ConfirmYes()",
                    onNo: "ConfirmNo()"
                }
            );        
    },
    Close: function () {
        ModalPopups.Close("idConfirm");
    },

    Alert: function (msg) {
        var title = 'CAUTION';
        var height = 100;
        ModalPopups.Alert("idALert",
                title,
                "<div style='padding: 10px;'>" +
                "<div class='popup_confirm'>" + msg + "</div>" +
                "<div style='height:10px'></div>" +
                "</div>",
                {
                    okButtonText: 'OK'
                }
            );
    }

};



function clearMessage(containerid) {

    $("#" + containerid).html('');
}

function showAlertMessage(msg, type) {
    // Success=0, Error=1, Warning=2
    var html = '';

    switch (type) {
        case 0:
            html = "<div class='alertboxscc'><div>" + msg + "</div></div>";
            break;
        case 1:
            html = "<div class='alertboxerr'><div>" + msg + "</div></div>";
            break;
        case 2:
            html = "<div class='alertboxwrn'><div>" + msg + "</div></div>";
            break;
        default:
            break;
    }
    return html;
}


function displayAlertMessage(containerid, msg, type) {
    // Success=0, Error=1, Warning=2
    $('#' + containerid).show();
    if (document.getElementById(containerid) != null) {
        switch (type) {
            case 0:
                $("#" + containerid).html("<div class='alertboxscc'><div>" + msg + "</div></div>");
                var t = setTimeout("clearMessage('" + containerid + "')", 6000);
                break;
            case 1:
                $("#" + containerid).html("<div class='alertboxerr'><div>" + msg + "</div></div>");
                break;
            case 2:
                $("#" + containerid).html("<div class='alertboxwrn'><div>" + msg + "</div></div>");
                break;
            default:
                break;
        }
        window.scroll(0, 0);
        parent.scroll(0, 0);

    }
}

function showEnrichMessage(containerid, ErrorHandler) {
    var isError500 = true;
    var isSuccess = true;
    var result = '';
    if (ErrorHandler != null && ErrorHandler.length > 0) {
        $.each(ErrorHandler, function (index, value) {
            if (value.Key > 0) {
                if (isError500) {
                    result += showAlertMessage(value.Value, 1);
                }
                isSuccess = false;
            }
            else {
                result += showAlertMessage(value.Value, 0);
            }
            if (value.Key == 500) isError500 = false;
        });
    }

    if (isSuccess) {
        t1 = setTimeout("clearMessage('" + containerid + "')", 8000);
    }
    else {
        t2 = setTimeout("clearSuccessMessages()", 8000);
    }

    $("#".concat(containerid)).html(result);

    window.scroll(0, 0);
    parent.scroll(0, 0);
    return isSuccess;
}

function clearSuccessMessages() {

    $("#err .msgcorrect").remove();
    clearTimeout(t2);
}

var timeoutID = 0;
function hideLanguage(obj) {
    if (String(obj.innerHTML).indexOf("-") != -1) {
        //current opened
        $("#otherLanguage_title").hide();
        $("#otherLanguage_content").hide();
        $("#otherLanguage_title2").hide();
        $("#otherLanguage_content2").hide();
        obj.innerHTML = "<span style=\"color: Black;\">Other Language</span> [+]";
    }
    else {
        //current closed
        $("#otherLanguage_title").show();
        $("#otherLanguage_content").show();
        $("#otherLanguage_title2").show();
        $("#otherLanguage_content2").show();
        obj.innerHTML = "<span style=\"color: Black;\">Other Language</span> [-]";
    }

    window.clearTimeout(timeoutID);
    timeoutID = setTimeout('gotoBottom()', 1);
}

function gotoBottom() {
    window.location = '#men';
}

//Thai @20130528
function countChar(txtarea, l, charNum) {
    var len = $(txtarea).val().length;
    if (len > l) $(txtarea).val($(txtarea).val().slice(0, l));
    else $(charNum).text("Characters Left:" + (l - len));
}