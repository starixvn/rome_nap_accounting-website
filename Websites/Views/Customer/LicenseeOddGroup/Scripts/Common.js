/* forceNumber()
 * ==========
 * Allow input Number only.
 *
 * @usage: $(selector).forceNumber();
 *
 */
$.fn.forceNumber = function (keyupCallback) {
    var number_0 = 48,
        number_9 = 57,
        numpad_0 = 96,
        numpad_9 = 105,
        end = 35,
        home = 36,
        leftArrow = 37,
        rightArrow = 39,
        backspace = 8,
        tab = 9,
        enter = 13,
        escape = 27,
        del = 46,
        a = 65,
        c = 67,
        v = 86,
        x = 88;

    function isNumeric(text) {
        var regExp = /^-?(\d+|\d+\.\d*|\d*\.\d+)$/;
        return text.match(regExp);
    }

    function keydownHandler(e) {
        var code = e.which || e.keyCode;
        if (number_0 <= code && code <= number_9 ||
            numpad_0 <= code && code <= numpad_9 ||
            code === end ||
            code === home ||
            code === leftArrow ||
            code === rightArrow ||
            code === backspace ||
            code === tab ||
            code === enter ||
            code === escape ||
            code === del) {
            return true;
        }
        var ctrlKey = e.ctrlKey;
        var metaKey = e.metaKey;
        return (code === a || code === c || code === v || code === x) && (ctrlKey || metaKey);
    }

    function keyupHandler(e) {
        var text = this.value;
        if (text.length === 0) {
            return;
        }
        text = text.replace(/\,/g, "");
        if (isNumeric(text)) {
            var number = parseInt(text).toLocaleString("en-US");
            this.value = number;
            if (keyupCallback && typeof keyupCallback === "function") {
                keyupCallback(this);
            }
        }
    }

    this.keydown(keydownHandler)
        .keyup(keyupHandler);
};

var common = (function ($) {
    "use strict";
    var self = {};
    self.blockUI = function () {
        try { parent.BlockUI(); } catch (e) { console.error(e); }
    };
    self.unblockUI = function () {
        try { parent.jQuery.unblockUI(); } catch (e) { console.error(e); }
    };
    self.showMessage = function (type, message) {
        var html = ['<div class="', type === 0 ? "alertboxscc" : "alertboxerr", '"><div>', message, "</div></div>"].join("");
        var messageDiv = $("#sectionMessage");
        return messageDiv.html(html).removeClass("hide");
    };
    self.hideMessage = function () {
        var messageDiv = $("#sectionMessage");
        return messageDiv.html("").addClass("hide");
    };
    self.showNoChangeMessage = function () {
        self.showMessage(1, "There is no change to update.");
    };
    self.showNotImplementedMessage = function () {
        self.showMessage(1, "Function is not implemented.");
    };
    self.showInternalServerError = function (jqxhr) {
        self.showMessage(500, jqxhr.statusText);
    };
    self.enableElement = function (element) {
        return element.removeAttr("disabled");
    };
    self.disableElement = function (element) {
        return element.attr("disabled", "disabled");
    };
    return self;
})(jQuery);