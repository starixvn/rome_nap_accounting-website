function get_dmy(strdate) {
    var dmy = {
        day: '',
        month: '',
        year: ''
    };
    //mm//dd/yy;
    var tmp = strdate.split('/');
    dmy.month = tmp[0];
    dmy.day = tmp[1];
    dmy.year = tmp[2]

    return dmy;
}

var Helper = {
    arrMonths: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    beginYear: 2007,
    format: function (num) {
        if (num == null)
            return '';

        var n = num;
        var IsMargin = num.indexOf('%') > 0;
        n = num.replace('%', '');
        if (parseFloat(n) < 0) {
            n = num.replace('-', '');
            return '<span style="color:red">(' + n + ')</span>'
        }
        return num;
    },

    validInputParam: function () {
        var rs = true;
        $('#msg').html('');

        if (parseInt(punter.param.TopRow) <= 0 || parseInt(punter.param.TopRow) > 10000) {
            showAlertMessage("msg", "Top rows must be greater than 0 and less than 10000.", 1);
            rs = false;
        }
        else if (punter.param.SearchBy == 0)//search by User
        {
            var products = punter.getSelectedProduct();
            var ids = punter.getSelectedSport();
            if (ids == '' && products.indexOf('1_0') < 0) {
                ids = '-';
            }

            if (products == '') {
                showAlertMessage("msg", "Please choose at least one product.", 1);
                rs = false;
            }
            else if (ids == '') {
                showAlertMessage("msg", "Please choose at least one sport.", 1);
                rs = false;
            }
        }
        else { //search by League
            var ids = punter.getSelectedBettype();
            if (ids == '') {
                showAlertMessage("msg", "Please choose at least one bettype.", 1);
                rs = false;
            }
        }
        return rs;
    },

    renderCarlendar: function () {
        var type = $('#ddlPeriodType').val();
        punter.setPeriodType(type);
        switch (type) {
            case '0': //Daily
                Helper.renderDailyCalendar();
                break;
            case '1': //Weekly
                Helper.renderWeeklyCalendar();
                break;
            case '2': //Monthly
                Helper.renderMonthlyCalendar();
                break;
            case '3': //Annualy
                Helper.renderAnnualyCalendar();
                break;
            case '4': //Month Range
                Helper.renderLongRangeCalendar();
                break;
        }

        punter.loadLeagueBySport();
    },

    renderDailyCalendar: function () {
        $('.calendar_lable').removeClass('hide');
        $('.calendar_daily').removeClass('hide');
        $('.calendar_weekly').addClass('hide');
        $('.calendar_monthly').addClass('hide');
    },
    renderWeeklyCalendar: function () {
        $('.calendar_lable').removeClass('hide');
        $('.calendar_daily').addClass('hide');
        $('.calendar_weekly').removeClass('hide');
        $('.calendar_monthly').addClass('hide');

        var week = $('#Week');
        week.next().css('left', week.offset().left);

        var toweek = $('#ToWeek');
        toweek.next().css('left', toweek.offset().left);
    },
    renderMonthlyCalendar: function () {
        $('.calendar_lable').addClass('hide');
        $('.calendar_daily').addClass('hide');
        $('.calendar_weekly').addClass('hide');
        $('.calendar_monthly').removeClass('hide');

        $('#sp_calendar_from').html('<span>Month</span><span style="padding-left:27px"><select onchange="punter.loadLeagueBySport();" id="ddlMonth">' + punter.genMonth(false) + '</select></span>\
                <span style="padding-left:10px">Year</span><span style="padding-left:5px"><select onchange="punter.loadLeagueBySport();Helper.bindingMonthly_Calendar();" id="ddlYear">' + punter.genYear() + '</select></span>');
        $('#sp_calendar_to').html('');

        Helper.bindingMonthly_Calendar();
    },
    renderAnnualyCalendar: function () {
        $('.calendar_lable').addClass('hide');
        $('.calendar_daily').addClass('hide');
        $('.calendar_weekly').addClass('hide');
        $('.calendar_monthly').removeClass('hide');

        $('#sp_calendar_from').html('<span>Year</span><span style="padding-left:35px"><select onchange="punter.loadLeagueBySport();" id="ddlYear">' + punter.genYear() + '</select></span></span>');
        $('#sp_calendar_to').html('');
    },
    renderLongRangeCalendar: function () {
        $('.calendar_lable').addClass('hide');
        $('.calendar_daily').addClass('hide');
        $('.calendar_weekly').addClass('hide');
        $('.calendar_monthly').removeClass('hide');

        var from = '<span>From</span><span style="padding-left:30px">\
                <select onchange="punter.loadLeagueBySport();;Helper.bindingLongRange_Calendar();" id="ddlfMonth">' + punter.genMonth(false) + '</select></span>\
                <span><select onchange="punter.loadLeagueBySport();Helper.bindingLongRange_Calendar();" id="ddlfYear">' + punter.genYear() + '</select></span>';
        $('#sp_calendar_from').html(from);

        var to = '<span>To</span><span style="padding-left:44px"></span>\
                <select onchange="punter.loadLeagueBySport();Helper.bindingLongRange_Calendar();" id="ddltMonth">' + punter.genMonth(false) + '</select></span>\
                <span><select onchange="punter.loadLeagueBySport();Helper.bindingLongRange_Calendar();" id="ddltYear">' + punter.genYear() + '</select></span>';
        $('#sp_calendar_to').html(to);

        Helper.bindingLongRange_Calendar();
    },
    getFromDateToDate: function () {
        var date = {
            fromdate: '',
            todate: ''
        };

        var type = $('#ddlPeriodType').val();
        var month, year;

        var d = new Date($('#maxDate').val());
        var current_month = d.getMonth() + 1;
        var current_year = d.getFullYear();
        var current_day = d.getDate();

        switch (type) {
            case '0': //Daily                
                var fromdate = $('#txtfromdate').val();
                var todate = $('#txttodate').val();
                date.fromdate = Core.convertDateToString(new Date(fromdate));
                date.todate = Core.convertDateToString(new Date(todate));
                break;
            case '1': //Weekly
                var fromWeekId = fromWeekId || "Week";
                var toWeekId = toWeekId || "ToWeek";
                var $from = $("#" + fromWeekId);
                var $to = $("#" + toWeekId);

                var from = $from.weekpicker("getDates");
                var to = $to.weekpicker("getDates");

                date.fromdate = Core.convertDateToString(from.start);

                if (Date.parse(to.end) > Date.parse($('#maxDate').val())) {
                    date.todate = $('#maxDate').val();
                }
                else {
                    date.todate = Core.convertDateToString(to.end);
                }

                if (Date.parse(date.fromdate) > Date.parse(date.todate)) {
                    date.fromdate = date.todate;
                }

                break;
            case '2': //Monthly
                month = $('#ddlMonth').val();
                year = $('#ddlYear').val();
                date.fromdate = month + '/01/' + year;

                if (month == current_month && year == current_year) {
                    date.todate = month + '/' + current_day + '/' + year;
                }
                else {
                    date.todate = month + '/' + punter.daysInMonth(month, year) + '/' + year;
                }
                break;
            case '3': //Annualy
                year = parseInt($('#ddlYear').val());

                if (year == current_year) {
                    date.fromdate = '01' + '/01/' + year;
                    date.todate = current_month + '/' + current_day + '/' + year;
                }
                else {
                    date.fromdate = '01' + '/01/' + year;
                    date.todate = '12' + '/31/' + year;
                }
                break;
            case '4': //Month Range
                date.fromdate = $('#ddlfMonth').val() + '/01' + '/' + $('#ddlfYear').val();

                if ($('#ddltMonth').val() == current_month && $('#ddltYear').val() == current_year) {
                    date.todate = $('#ddltMonth').val() + '/' + current_day + '/' + current_year;
                }
                else {
                    date.todate = $('#ddltMonth').val() + '/' + punter.daysInMonth($('#ddltMonth').val(), $('#ddltYear').val()) + '/' + $('#ddltYear').val();
                }
                break;
        }

        return date;
    },

    endYear: function () {
        var d = new Date(maxDate);
        return d.getFullYear();
    },
    genMonth: function (isCurrent) {
        var d = new Date(maxDate);
        var month = '';
        for (var i = 0; i < punter.arrMonths.length; i++) {
            if (i == d.getMonth() + 1 - (isCurrent ? 0 : 1)) {
                month += '<option selected="selected" value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
            else {
                month += '<option value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
        }
        return month;
    },

    genYear: function () {
        var d = new Date(maxDate);
        var year = '';
        for (var i = punter.beginYear; i <= punter.endYear(); i++) {
            if (i == d.getFullYear()) {
                year += '<option selected="selected" value=' + i + '>' + i + '</option>';
            }
            else {
                year += '<option value=' + i + '>' + i + '</option>';
            }
        }

        return year;
    },
    genMonth_Constraint: function (min, max, current) {
        var d = new Date(maxDate);
        var month = '';
        for (var i = min - 1; i <= max - 1; i++) {
            if (i == current - 1) {
                month += '<option selected="selected" value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
            else {
                month += '<option value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
        }
        return month;
    },

    genYear_Constraint: function (min, max, current) {
        var d = new Date(maxDate);
        var year = '';
        for (var i = min; i <= max; i++) {
            if (i == current) {
                year += '<option selected="selected" value=' + i + '>' + i + '</option>';
            }
            else {
                year += '<option value=' + i + '>' + i + '</option>';
            }
        }

        return year;
    },

    bindingLongRange_Calendar: function () {
        var d = new Date(maxDate);
        var current_month = d.getMonth() + 1;
        var current_year = d.getFullYear();


        var fmonth = parseInt($('#ddlfMonth').val());
        var fyear = parseInt($('#ddlfYear').val());
        var tmonth = parseInt($('#ddltMonth').val());
        var tyear = parseInt($('#ddltYear').val());

        var min, max, current;
        //constraint fromdate must be less than from date        
        if (fyear == tyear) {
            min = fyear;
            current = tyear;
            max = current_year;
            $('#ddltYear').empty().append(Helper.genYear_Constraint(min, max, current));
        }
        else {
            min = fyear;
            current = tyear;
            max = current_year;
            $('#ddltYear').empty().append(Helper.genYear_Constraint(min, max, current));
        }
        var tyear = parseInt($('#ddltYear').val());

        if (fyear == current_year) {
            min = 1;
            current = fmonth;
            max = current_month;
            $('#ddlfMonth').empty().append(Helper.genMonth_Constraint(min, max, current));
        }
        else {
            min = 1;
            current = fmonth;
            max = 12;
            $('#ddlfMonth').empty().append(Helper.genMonth_Constraint(min, max, current));
        }
        var fmonth = parseInt($('#ddlfMonth').val());

        if (tyear == current_year) {
            min = 1;
            current = tmonth;
            max = current_month;
            $('#ddltMonth').empty().append(Helper.genMonth_Constraint(min, max, current));
        }
        else if (fyear == tyear) {
            min = fmonth;
            current = tmonth;
            max = 12;
            $('#ddltMonth').empty().append(Helper.genMonth_Constraint(min, max, current));
        }
        else {
            min = 1;
            current = tmonth;
            max = 12;
            $('#ddltMonth').empty().append(Helper.genMonth_Constraint(min, max, current));
        }
    },


    bindingMonthly_Calendar: function () {
        var d = new Date(maxDate);
        var current_month = d.getMonth() + 1;
        var current_year = d.getFullYear();

        var month = parseInt($('#ddlMonth').val());
        var year = parseInt($('#ddlYear').val());

        if (year == current_year) {
            min = 1;
            current = month;
            max = current_month;
            $('#ddlMonth').empty().append(Helper.genMonth_Constraint(min, max, current));
        }
        else {
            min = 1;
            current = month;
            max = 12;
            $('#ddlMonth').empty().append(Helper.genMonth_Constraint(min, max, current));
        }
    },

    getSportId: function (sporttypeId_bettypeId, isProduct) {
        var str = [];
        var arr = sporttypeId_bettypeId.split(',');
        for (var i = 0; i < arr.length; i++) {
            var id = arr[i].split('_')[0];
            if (arr[i] == "0_9" || arr[i] == "0_29") {
                id = arr[i];
            }

            if ($.inArray(id, str) == -1 && (!isProduct || id != 1)) {
                str.push(id);
            }
        }

        return str.join(',');
    }

};
