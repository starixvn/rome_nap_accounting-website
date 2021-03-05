function MoveBetSchedule($) {
    'use strict';

    var self = this;

    this.$ = $;

    this.$fromDate = null;
    this.$toDate = null;
    this.$tblDefaultMoveBetSchedule = null;
    this.$tblMoveBetSchedule = null;

    this.currentDay = currentDay;
    this.sevenDaysFromToday = sevenDaysFromToday;

    this.days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    this.defaultMoveBetSchedule = defaultMoveBetSchedule;
    this.url = url;

    this.init = function () {
        this.cacheDOMElements();
        this.renderDateRangePicker();
        this.renderDefaultMoveBetSchedule();
        this.renderMoveBetSchedule();
    };

    this.cacheDOMElements = function () {
        this.$fromDate = this.$('#txtFromDate');
        this.$toDate = this.$('#txtToDate');
        this.$tblDefaultMoveBetSchedule = this.$('#tbl-default-movebet');
        this.$tblMoveBetSchedule = this.$('#tbl-movebet-schedule');
    };

    this.renderDateRangePicker = function () {
        this.$fromDate.datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: this.currentDay,
            defaultDate: this.currentDay,
            onSelect: function (dateText, inst) {
                self.$toDate.datepicker('option', 'minDate', dateText);
                self.renderMoveBetSchedule();
            }
        }).datepicker('setDate', this.currentDay);

        this.$toDate.datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: this.currentDay,
            defaultDate: this.sevenDaysFromToday,
            onSelect: function (dateText, inst) {
                self.renderMoveBetSchedule();
            }
        }).datepicker('setDate', this.sevenDaysFromToday);
    };

    this.renderDefaultMoveBetSchedule = function () {
        var currentWeekDay = new Date(this.currentDay).getDay(),
            tbody = this.$tblDefaultMoveBetSchedule.find('tbody');

        this.$tblDefaultMoveBetSchedule.find(`th[data-weekday="${currentWeekDay}"]`).addClass('current-day');
        for (var row = 0; row < 7; row++) {
            var tr = $('<tr></tr>');
            if (row === 0) {
                tr.append('<td rowspan="7" align="center">For Days</td>');
            }

            for (var column = 0; column < 7; column++) {
                var day = column + 1 === 7 ? 0 : column + 1;
                var backDay = day - row - 1 < 0 ? 7 + day - row - 1 : day - row - 1;
                var option = {
                    checked: this.defaultMoveBetSchedule[day].Options[row],
                    label: `Current date - ${row + 1} (${this.days[backDay]})`
                };

                tr.append(`<td class="${day === currentWeekDay ? 'current-day' : ''}">
                             <input
                                 type = "checkbox"
                                 ${ option.checked ? 'checked' : ''}
                                 disabled
                             />
                             <label>${option.label}</label>
                           </td>`);
            }

            tbody.append(tr);
        }
    };

    this.renderMoveBetSchedule = function () {
        var param = {
            FromDate: this.$fromDate.val(),
            ToDate: this.$toDate.val()
        };

        this.setDatePickerDisabledOption(true);

        this.$.ajax({
            type: 'POST',
            url: this.url,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(param),
            success: function (data) {
                self.$tblMoveBetSchedule.renderMoveBetSchedule({ data });
            },
            error: function (xhr, textStatus, thrownError) { },
            complete: function (xhr, textStatus) {
                self.setDatePickerDisabledOption(false);
            }
        });
    };

    this.setDatePickerDisabledOption = function (disabled) {
        this.$fromDate.datepicker('option', 'disabled', disabled);
        this.$toDate.datepicker('option', 'disabled', disabled);
    };
}

$(function () {
    var moveBetSchedule = new MoveBetSchedule($);
    moveBetSchedule.init();
});