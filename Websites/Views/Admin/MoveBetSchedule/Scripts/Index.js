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
                var option = defaultMoveBetSchedule[days[day]][row];
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
        var fromDate = this.$fromDate.datepicker('getDate'),
            toDate = this.$toDate.datepicker('getDate');
        this.$tblMoveBetSchedule.renderMoveBetSchedule({ fromDate, toDate });
    };
}

$(function () {
    var moveBetSchedule = new MoveBetSchedule($);
    moveBetSchedule.init();
});