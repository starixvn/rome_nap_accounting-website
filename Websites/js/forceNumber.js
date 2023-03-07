/* forceNumber()
 * ==========
 * Allow input Number only.
 *
 * @usage: $(selector).forceNumber();
 *
 */
$.fn.forceNumber = function (keyupCallback, isDecimalSupport, isNegativeNumberSupport) {
    isDecimalSupport = isDecimalSupport || false;
    isNegativeNumberSupport = isNegativeNumberSupport || false;
    var number_0 = 48,
        number_9 = 57,
        numpad_0 = 96,
        numpad_9 = 105,
        numpad_point = 110,
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
        x = 88,
        point = 190,
        negativeCode=[173,189,109];

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
            code === del ||
            (isDecimalSupport && code === numpad_point) ||
            (isDecimalSupport && code === point) ||
            (isNegativeNumberSupport && (negativeCode.includes(code)))) {
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
        if (isNegativeNumberSupport && text === '-') {
            return;
        }

        text = text.replace(/\,/g, "");
        var isEndWithOneDot = text.slice(-1) === '.' && text.slice(-2) !== '..';
        var isEndWithDotOrZero = text.slice(-2) == '0.' || text.slice(-2) == '.0' || text.slice(-3) == '0.0';
        if (text.replace(/^\.+|\.+$/g, "") === "") {
            text = "0";
        }

        var number = parseFloat(text.replace(/^\.+|\.+$/g, "")).toLocaleString("en-US", { minimumFractionDigits: 0, maximumFractionDigits: 2 });
        if (number !== 0 && !isEndWithDotOrZero) {
            this.value = number + (isEndWithOneDot ? "." : "");
        }
        if (number==="NaN") {
            this.value="";
        }
        
        if (keyupCallback && typeof keyupCallback === "function") {
            keyupCallback(this);
        }
    }

    this.keydown(keydownHandler)
        .keyup(keyupHandler);
};