function formatNumber(id, val, isFloat) {
    var obj = document.getElementById(id);
    var value = val;

    if (value == '') return true;

    var num;
    var dotPosition = 0;
    var afterDot;

    num = value.toString().replace(/\$|\,/g, '');
    dotPosition = num.indexOf(".");
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        afterDot = num.substring(dotPosition + 1, num.length).replace(/[^0-9]/g, '');
        num = num.substring(0, dotPosition);
    }
    num = num.formatFloat();
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        eval(obj).value = num + "." + afterDot;
        num = obj.value;
    } else {
        eval(obj).value = num;
    }

    return num;
}

function formatExt(obj, isFloat) {
    var value = obj.value;

    if (value == '') return true;

    var num;
    var dotPosition = 0;
    var afterDot;
    var caretPosition = getCaret(obj);

    num = value.toString().replace(/\$|\,/g, '');
    dotPosition = num.indexOf(".");
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        afterDot = num.substring(dotPosition + 1, num.length).replace(/[^0-9]/g, '');
        num = num.substring(0, dotPosition);
    }
    num = num.formatFloat();
    if ((typeof (isFloat) != 'undefined') && isFloat && dotPosition > 0) {
        eval(obj).value = num + "." + afterDot;
        num = obj.value;
    } else {
        eval(obj).value = num;
    }

    if (num.length > value.length) {
        caretPosition = caretPosition + 1;
    }

    setCaret(obj, caretPosition);

    return true;
}

function setCaret(oField, iCaretPos) {
    // IE Support
    if (document.selection) {
        oField.focus();
        // Create empty selection range
        var oSel = document.selection.createRange();
        // Move selection start and end to desired position
        oSel.moveStart('character', iCaretPos);
        oSel.moveEnd('character', iCaretPos - oField.value.length);
        oSel.select();
    }
    // Firefox support
    else if (oField.selectionStart || oField.selectionStart == '0') {
        oField.selectionStart = iCaretPos;
        oField.selectionEnd = iCaretPos;
        oField.focus();
    }
};

function getCaret(oField) {
    var iCaretPos = 0;
    // IE Support
    if (document.selection) {
        oField.focus();
        // To get cursor position, get empty selection range
        var oSel = document.selection.createRange();
        // Move selection start to 0 position
        oSel.moveStart('character', -oField.value.length);
        iCaretPos = oSel.text.length;
    }
    // Firefox support
    else if (oField.selectionStart || oField.selectionStart == '0')
        iCaretPos = oField.selectionStart;
    // Return results
    return (iCaretPos);
};

String.prototype.formatFloat = function () {
    var number = this;

    if (number == '') return 0;
    var sign;
    if (isNaN(number)) {
        number = '';
    }

    sign = (number == (number = Math.abs(number)));
    number = Math.floor(number * 100 + 0.50000000001);
    number = Math.floor(number / 100).toString();

    for (var i = 0; i < Math.floor((number.length - (1 + i)) / 3); i++) {
        number = number.substring(0, number.length - (4 * i + 3)) + ',' + number.substring(number.length - (4 * i + 3));
    }

    return (((sign) ? '' : '-') + number);
};