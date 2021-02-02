$(function () {
    $('#txtFromDate').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        minDate: new Date(minDay),
        defaultDate: currentDay,
        onSelect: function (dateText, inst) {
            var maxDate = new Date(dateText);
            maxDate.setDate(maxDate.getDate() + 6);
            $('#txtToDate').datepicker("option", "minDate", dateText);
            $('#txtToDate').datepicker("option", "maxDate", maxDate);
        }
    }).datepicker('setDate', currentDay);

    $('#txtToDate').datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        minDate: currentDay,
        maxDate: sevenDaysFromToday,
        defaultDate: currentDay
    }).datepicker("setDate", currentDay);
});