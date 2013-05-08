

$(document).ready(function () {
    var limitFromDate = {
        minDate: minDay,
        maxDate: toDay
    };
    var options = {
        showOtherMonths: true,
        selectOtherMonths: true
    };
    calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);
    calendarUtils.renderCalendar("#tdate", limitFromDate, "#tdate", "", tDay, options);
});

var winLostSummary = {
    updateCalendar: function () {
        var limitFromDate = {
            minDate: minDay,
            maxDate: toDay
        };
        var options = {
            showOtherMonths: true,
            selectOtherMonths: true
        };
        calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "#tdate", fDay, options);
        calendarUtils.renderCalendar("#tdate", limitFromDate, "#fdate", "#tdate", tDay, options);
    }
}