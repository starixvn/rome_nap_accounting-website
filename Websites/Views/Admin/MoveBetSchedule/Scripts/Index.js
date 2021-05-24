function MoveBetSchedule($) {
    'use strict';

    var self = this;

    this.$ = $;

    this.$fromDate = null;
    this.$toDate = null;
    this.$tblDefaultMoveBetSchedule = null;
    this.$tblMoveBetSchedule = null;
    this.$resultMessage = null;

    this.currentDay = currentDay;
    this.sevenDaysFromToday = sevenDaysFromToday;

    this.days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    this.daysPerWeek = 7;
    this.defaultMoveBetSchedule = defaultMoveBetSchedule;
    this.getMoveBetScheduleUrl = getMoveBetScheduleUrl;
    this.updateMoveBetScheduleUrl = updateMoveBetScheduleUrl;

    this.init = function () {
        this.$.ajaxSetup({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            beforeSend: function (xhr, settings) {
                window.parent.BlockUI();
            },
            error: function (xhr, textStatus, thrownError) { return; },
            complete: function (xhr, textStatus) {
                window.parent.$.unblockUI();
            }
        });

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
        this.$resultMessage = this.$('#result-message');
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
        for (var row = 0; row < this.daysPerWeek; row++) {
            var tr = $('<tr></tr>');
            if (row === 0) {
                tr.append('<td rowspan="7" align="center">For Days</td>');
            }

            for (var column = 0; column < this.daysPerWeek; column++) {
                var day = column + 1 === this.daysPerWeek ? 0 : column + 1;
                var backDay = day - row - 1 < 0 ? this.daysPerWeek + day - row - 1 : day - row - 1;
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

        this.$.ajax({
            url: this.getMoveBetScheduleUrl,
            data: JSON.stringify(param),
            success: function (data) {
                self.$tblMoveBetSchedule.renderMoveBetSchedule({ data, onSave: self.onSave.bind(self) });
                self.$resultMessage.text('');
            }
        });
    };

    this.onSave = function (data) {
        var param = {
            File: data,
            FromDate: this.$fromDate.val(),
            ToDate: this.$toDate.val()
        };

        this.$.ajax({
            url: this.updateMoveBetScheduleUrl,
            data: JSON.stringify(param),
            success: function (result) {
                if (result && result.ErrorCode === 0) {
                    self.showMessage.call(self, 'Successful.', 'success');
                    setTimeout(self.renderMoveBetSchedule.bind(self), 1000);
                } else {
                    var message = result ? result.ErrorMessage : 'Error.';
                    self.showMessage.call(self, message, 'error');
                }
            }
        });
    };

    this.showMessage = function (message, className) {
        this.$resultMessage.removeClass().addClass(className);
        this.$resultMessage.text(message);
    };
}

$(function () {
    var moveBetSchedule = new MoveBetSchedule($);
    moveBetSchedule.init();
});