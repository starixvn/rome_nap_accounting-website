$(function () {
    var limitDate = {
        minDate: new Date('1/01/2010'),
        maxDate: new Date('1/01/2014')
    };

    calendar.renderCalendar('#txtfromdate,#txttodate', limitDate);
});

var calendar = {
    renderCalendar: function (ids, opts) {
        $.datepicker._defaults.dateFormat = "M d, yy";
        var defaults = {
            onSelect: function (selectedDate) {
                //                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                //                     instance = $(this).data("datepicker"),
                //                     date = $.datepicker.parseDate(
                //                         instance.settings.dateFormat ||
                //                             $.datepicker._defaults.dateFormat,
                //                         selectedDate, instance.settings);
                //                dates.not(this).datepicker("option", option, date);

            }
        };
        var new_opts =  $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids).datepicker("setDate", opts.maxDate);
    }
};
