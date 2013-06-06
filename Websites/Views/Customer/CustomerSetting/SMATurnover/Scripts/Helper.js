var Helper = {
//    addCommas: function (n) {
//        var rx = /(\d+)(\d{3})/;
//        return String(n).replace(/^\d+/, function (w) {
//            while (rx.test(w)) {
//                w = w.replace(rx, '$1,$2');
//            }
//            return w;
//        });
//    },

    SeparateNumricByThousand: function (number) {
        var value = "";
        stringarr = number.split(',');
        for (var i = 0; i < stringarr.length; i++) {
            value += stringarr[i];
        }
        return value;
    },


    CheckDecNumberValue: function (e) {
        var code = "";
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;
        if (code == 9) {
            return code;
        }
        if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 45 && code != 37 && code != 39) {
            return false;
        }
        return true;
    },

    onKeyEnter: function (e) {
        if (e.keyCode == 13) {
            SMATurnover.GetInfoByMatch();
        }
    }

}