
var calendarUtils = {
    renderCalendar: function (ids, limitdate, maxDayIds, minDayIds, toDay, options) {
        if (options == null) {
            var options = {
                showOtherMonths: true,
                selectOtherMonths: true,
                checkMinMax: true
            }
        }

        var defaults = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            onSelect: function (selectedDate) {
                if (options.checkMinMax) {
                    var option = this.id == maxDayIds ? "minDate" : "maxDate",
                    instance = $(this).data("datepicker"),
                    date = $.datepicker.parseDate(
                        instance.settings.dateFormat ||
                            $.datepicker._defaults.dateFormat,
                        selectedDate, instance.settings);
                    dates.not(this).datepicker("option", option, date);
                }
            },
            onClose: function () {
            }
        };
        var new_opts = $.extend(true, defaults, limitdate);
        var dates = $(ids).datepicker(new_opts);
        $(ids).datepicker("setDate", new Date(toDay));
    }
} 