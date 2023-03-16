/* forceNumber()
 * ==========
 * Allow input Number only.
 *
 * @usage: $(selector).forceNumber();
 *
 */
$.fn.forceNumber = function (keyupCallback, options) {
    isDecimalSupport = options.isDecimalSupport || false;
    isNegativeNumberSupport = options.isNegativeNumberSupport || false;
    minimumFractionDigits = options.minimumFractionDigits || 0;
    maximumFractionDigits = options.maximumFractionDigits || 2;
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
        if((isDecimalSupport && code === numpad_point) || (isDecimalSupport && code === point) ){
            return !($(this).val() === "" || $(this).val().indexOf(".") >= 0);
        }
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
            
            (isNegativeNumberSupport && (negativeCode.includes(code)))) {
            return true;
        }
        var ctrlKey = e.ctrlKey;
        var metaKey = e.metaKey;
        return (code === a || code === c || code === v || code === x) && (ctrlKey || metaKey);
    }

    function keyupHandler(e) {
        var text = this.value;
        var backupValue = $(this).val();
        var currentPosition =  $(this).caret().start;
        if (text.length === 0) {
            return;
        }
        if (isNegativeNumberSupport && text === '-') {
            return;
        }

        text = text.replace(/\,/g, "");
        var isEndWithOneDot = text.slice(-1) === '.' && text.slice(-2) !== '..';
        var isEndWithDotOrZero = text.slice(-2) == '0.' || text.slice(-2) == '.0' || text.slice(-3) == '0.0' || text.split(".")[1]==0 ;
        if (text.replace(/^\.+|\.+$/g, "") === "") {
            text = "0";
        }
        
        var number = parseFloat(text.replace(/^\.+|\.+$/g, "")).toLocaleString("en-US", { minimumFractionDigits, maximumFractionDigits });
        if (number !== 0 && !isEndWithDotOrZero) {
            this.value = number + (isEndWithOneDot ? "." : "");
        }
        
        
        if (number==="NaN") {
            this.value="";
        }
        var numberIncreasedChars = this.value.length - backupValue.length;
        if (numberIncreasedChars >= 0) {
            $(this).setCaretPosition(currentPosition + numberIncreasedChars);
        }
        else {
            $(this).setCaretPosition(currentPosition);
        }
        if (keyupCallback && typeof keyupCallback === "function") {
            keyupCallback(this);
        }
    }

    this.keydown(keydownHandler)
        .keyup(keyupHandler);
};