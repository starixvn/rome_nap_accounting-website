/* File Created: December 14, 2012 */
var Core = {};
var _monthNames = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
var currentTheme = '';
var DATE_FORMAT = "dd M yy";

$(document).ready(function () {
    Core.maxDate = new Date($('#maxDate').val());
    Core.minDate = new Date();
});

Core.registerWeekPicker = function () {
    // week picker plugin
    var configs = {};

    $.fn.weekpicker = function (options, func) {

        var $this = $(this);
        $this.css("margin", "2px");
        var $calendar = $this.nextAll("div:first");

        var id = $this.attr("id");
        if (!configs[id])
            configs[id] = { date: Core.maxDate };
        var defaults = configs[id];

        var displayYear = defaults.date.getFullYear();
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                $calendar.find('.ui-datepicker-current-day a').addClass('ui-state-active')
            }, 1);
        };

        var selectWeek = function (date, hideOnSelect) {

            if (!date) {
                date = $calendar.datepicker('getDate');
                if (!date)
                    date = Core.maxDate;
            }
            var dat = date.getDate();
            var day = date.getDay();

            defaults.startDate = new Date(date.getFullYear(), date.getMonth(), 1);
            if (day == 0)
                day = 7;
            defaults.startDate.setDate(dat - day + 1);
            defaults.endDate = new Date(date.getFullYear(), date.getMonth(), 1);
            defaults.endDate.setDate(dat - day + 7);


            if (date) {
                //var week = date.getWeekOfYear();
                var week = $.datepicker.iso8601Week(defaults.startDate);
                $this.val(week + " (" + defaults.startDate.format() + ")");
            }

            if (defaults.selectHandlers) {
                for (var i in defaults.selectHandlers)
                    defaults.selectHandlers[i](defaults.startDate, defaults.endDate, $this[0]);
            }

            if (hideOnSelect)
                $this.weekpicker("hide");
            //if (defaults.startDate == date) {
            $calendar.datepicker('setDate', defaults.startDate);
            //}
            displayYear = defaults.startDate.getFullYear();
            selectCurrentWeek();
            Core.setWeekRangeConstraint();
        };

        if (typeof options == "string") {

            if (options == "hide") {
                $calendar.fadeOut("fast");
            }
            else if (options == "show") {
                if ($calendar.is(":visible"))
                    return;
                $calendar.show();
            }
            else if (options == "onSelect") {
                if (!defaults.selectHandlers)
                    defaults.selectHandlers = [];
                defaults.selectHandlers.push(func);
            }
            else if (options == "getDates") {
                return { start: defaults.startDate, end: defaults.endDate.compareTo(Core.maxDate) == 1 ? Core.maxDate : defaults.endDate };
            }
            else if (options == "setDate") {
                selectWeek(func);
            }
        }
        else {

            $.extend(defaults, options);
            var offset = $this.offset();

            $this.addClass("datepicker hasDatepicker");
            $this.after("<div class='weekcalendar'></div>"); //<img style='position: relative;margin-top: 1px' class='ui-datepicker-trigger' src='" + $("#currentDomain").val() + "/App_Themes/" + currentTheme + "/Images/calendar1.gif' alt='' title=''>
            $calendar = $this.next();
            $calendar.css({ "position": "absolute", "z-index": 1000, "margin-top": offset.top, "float": "left" });
            var ishover = false;
            $.extend(defaults, {
                dateFormat: DATE_FORMAT,
                showOn: null,
                buttonImageOnly: true,
                buttonText: "",
                //buttonImage: $("#currentDomain").val() + "/App_Themes/" + currentTheme + "/Images/calendar.gif",
                firstDay: 1,
                showOtherMonths: true,
                selectOtherMonths: true,
                showWeek: true,
                changeYear: true,
                changeMonth: true,
                //                calculateWeek: function (date, a, b, c) {
                //                    return date.getWeekOfYear();
                //                },
                onSelect: function (dateText, inst) {
                    selectWeek(null, true);
                    punter.loadLeagueBySport();
                },
                beforeShowDay: function (date) {
                    var cssClass = '';
                    if (date >= defaults.startDate && date <= defaults.endDate)
                        cssClass = 'ui-datepicker-current-day';
                    return [true, cssClass];
                },
                onChangeMonthYear: function (year, month, inst) {
                    selectCurrentWeek();
                    displayYear = year;
                }
            });



            $calendar.datepicker(defaults);


            var show = function () {
                ishover = true;
                $this.weekpicker("show");
            }

            $calendar.hide();

            $this.width(144).mousedown(show);

            function enter() {
                ishover = true;
            }
            function leave() {
                ishover = false;
            }

            $calendar.mouseenter(enter).mouseleave(leave);
            $this.mouseenter(enter).mouseleave(leave);

            $("html").bind("click", function (e) {
                if (!ishover) {
                    $this.weekpicker("hide");
                }
            });
            $this.nextAll("img.ui-datepicker-trigger:first").click(function () {
                if ($calendar.is(":visible"))
                    $this.weekpicker("hide");
                else
                    show();
            });

            // set initial value    
            selectWeek(defaults.date);
        }
    }

    if ($('.weekpicker').length > 0) {
        $('.weekpicker').each(function () {
            $(this).weekpicker({
                date: new Date($(this).attr('data-value')),
                minDate: new Date($(this).attr('data-mindate')),
                maxDate: new Date($(this).attr('data-maxdate'))
            });
        });
    }
    $('.weekcalendar table.ui-datepicker-calendar tr').die().live({
        mousemove: function () { $(this).find('td a').addClass('ui-state-hover'); },
        mouseleave: function () { $(this).find('td a').removeClass('ui-state-hover'); }
    });
}


Date.prototype.format = function () {
    return _monthNames[this.getMonth()] + " " + this.getDate() + ", " + this.getFullYear();
};

Date.prototype.getWeek = function () {
    // Copy date so don't modify original
    d = new Date(this);
    d.setHours(0, 0, 0);
    // Set to nearest Thursday: current date + 4 - current day number
    // Make Sunday's day number 7
    d.setDate(d.getDate() + 4 - (d.getDay() || 7));
    // Get first day of year
    var yearStart = new Date(d.getFullYear(), 0, 1);
    // Calculate full weeks to nearest Thursday
    var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7) + 1;
    // Return array of year and week number
    return weekNo;
};

Date.prototype.MonthSubtract = function (d) {
    if (this.getFullYear() == d.getFullYear()) {
        return this.getMonth() - d.getMonth();
    } else {
        return this.getMonth() - d.getMonth() + 12 * (this.getFullYear() - d.getFullYear());
    }

    return 1;
};

// Date time constraints



Core.setWeekRangeConstraint = function (fromWeekId, toWeekId) {

    // alert('hi');

    fromWeekId = fromWeekId || "Week";
    toWeekId = toWeekId || "ToWeek";
    var $from = $("#" + fromWeekId);
    var $to = $("#" + toWeekId);

    $from.weekpicker("onSelect", setConstraint);
    $to.weekpicker("onSelect", setConstraint);

    setConstraint();
    function setConstraint(start, end, element) {

        if (!element) return;

        var from = $from.weekpicker("getDates");
        var to = $to.weekpicker("getDates");

        if (from.start > to.start) {
            if (element.id == fromWeekId) {
                $to.weekpicker("setDate", start);
            }
            else {
                $from.weekpicker("setDate", start);
            }
        }
        //        else {
        //            $to.weekpicker("setDate", start);
        //        }
    }



};




Core.setFromToDateConstraint = function (dateStartId, dateEndId, callback) {


    dateStartId = dateStartId || "DateStart";
    dateEndId = dateEndId || "DateEnd";
    var $dateStart = $("#" + dateStartId);
    var $dateEnd = $("#" + dateEndId);

    $dateStart.change(setConstraint);
    $dateEnd.change(setConstraint);

    function setConstraint() {

        alert('here daily');

        var startdate = $dateStart.datepicker("getDate");
        var enddate = $dateEnd.datepicker("getDate");

        if (enddate < startdate) {

            if (this.id == dateStartId) {
                $dateEnd.datepicker("setDate", startdate);
            }
            else {

                $dateStart.datepicker("setDate", enddate);
            }
            //  return;
        }
        else if ((enddate.getYear() != startdate.getYear()) ||
                (enddate.getMonth() != startdate.getMonth())) {

            if (enddate.getMonth() - 1 != startdate.getMonth() || enddate.getYear() != startdate.getYear()) {

                if (this.id == dateStartId) {
                    var month = startdate.getMonth() + 1;
                    if (month > 11)
                        month = 0;

                    var end = new Date(startdate);
                    end.setMonth(end.getMonth() + 1);

                    if (end.getMonth() > month ||
                    (enddate.getMonth() != end.getMonth() || enddate.getYear() != end.getYear())) {

                        if (end.getMonth() > month)
                            end.setDate(0);
                        if (enddate - end > 0)
                            enddate = end;

                        $dateEnd.datepicker("setDate", enddate);
                    }
                }
                else {
                    var month = enddate.getMonth() - 1;
                    if (month < 0)
                        month = 12;

                    var start = new Date(enddate);
                    start.setMonth(start.getMonth() - 1);

                    if (start.getMonth() > month ||
                        (startdate.getMonth() != start.getMonth() || startdate.getYear() != start.getYear())) {
                        if (start.getMonth() > month)
                            start.setDate(0);
                        if (startdate - start < 0)
                            startdate = start;
                        $dateStart.datepicker("setDate", startdate);
                    }
                }
            }
        }

        if (callback)
            callback();
    }

},

Core.renderCalendarFromTo = function (ids, opts) {
    var dateFull = $(ids);
    $.datepicker._defaults.dateFormat = "M dd, yy";
    var defaults = {
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        onSelect: function (selectedDate) {
            var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
            dateFull.not(this).datepicker("option", option, date);
            punter.loadLeagueBySport();
        }
    };
    var new_opts = $.extend(true, defaults, opts);

    var fdate = ids.split(',')[0];
    var tdate = ids.split(',')[1];

    var dates = $(fdate).datepicker(new_opts);
    var dates = $(tdate).datepicker(new_opts);

    $(fdate).datepicker("setDate", new Date(opts.maxDate));
    $(tdate).datepicker("setDate", new Date(opts.maxDate));

},

 Core.convertStringToDate = function (stringDate) {
     var temp = stringDate.split('/');
     if (temp.length != 3) {
         return stringDate + " is not valid format";
     }

     var day = temp[0];
     var month = temp[1];
     var year = temp[2];

     return new Date(Number(year), Number(month) - 1, Number(day));
 },

 Core.convertDateToString = function (date) {
     var day = date.getDate();
     var month = date.getMonth() + 1;
     var year = date.getFullYear();

     var returnDate = "";

     if (month < 10)
         returnDate += "0" + month + "/";
     else
         returnDate += month + "/";

     if (day < 10)
         returnDate += "0" + day + "/" + year;
     else
         returnDate += day + "/" + year;

     return returnDate;
 }