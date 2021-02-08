var days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
    months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    defaultMoveBetSchedule = {
        'Mon': [
            { checked: false, disabled: false, label: 'Current date - 1 (Sun)' },
            { checked: false, disabled: false, label: 'Current date - 2 (Sat)' },
            { checked: true, disabled: false, label: 'Current date - 3 (Fri)' },
            { checked: true, disabled: false, label: 'Current date - 4 (Thu)' },
            { checked: false, disabled: false, label: 'Current date - 5 (Wed)' },
            { checked: false, disabled: false, label: 'Current date - 6 (Tue)' },
            { checked: false, disabled: false, label: 'Current date - 7 (Mon)' }
        ],
        'Tue': [
            { checked: false, disabled: false, label: 'Current date - 1 (Mon)' },
            { checked: false, disabled: false, label: 'Current date - 2 (Sun)' },
            { checked: true, disabled: false, label: 'Current date - 3 (Sat)' },
            { checked: false, disabled: false, label: 'Current date - 4 (Fri)' },
            { checked: false, disabled: false, label: 'Current date - 5 (Thu)' },
            { checked: false, disabled: false, label: 'Current date - 6 (Wed)' },
            { checked: false, disabled: false, label: 'Current date - 7 (Tue)' }
        ],
        'Wed': [
            { checked: false, disabled: false, label: 'Current date - 1 (Tue)' },
            { checked: false, disabled: false, label: 'Current date - 2 (Mon)' },
            { checked: true, disabled: false, label: 'Current date - 3 (Sun)' },
            { checked: false, disabled: false, label: 'Current date - 4 (Sat)' },
            { checked: false, disabled: false, label: 'Current date - 5 (Fri)' },
            { checked: false, disabled: false, label: 'Current date - 6 (Thu)' },
            { checked: false, disabled: false, label: 'Current date - 7 (Wed)' }
        ],
        'Thu': [
            { checked: false, disabled: false, label: 'Current date - 1 (Wed)' },
            { checked: false, disabled: false, label: 'Current date - 2 (Tue)' },
            { checked: false, disabled: false, label: 'Current date - 3 (Mon)' },
            { checked: false, disabled: false, label: 'Current date - 4 (Sun)' },
            { checked: false, disabled: false, label: 'Current date - 5 (Sat)' },
            { checked: false, disabled: false, label: 'Current date - 6 (Fri)' },
            { checked: false, disabled: false, label: 'Current date - 7 (Tue)' }
        ],
        'Fri': [
            { checked: false, disabled: false, label: 'Current date - 1 (Thu)' },
            { checked: false, disabled: false, label: 'Current date - 2 (Wed)' },
            { checked: true, disabled: false, label: 'Current date - 3 (Tue)' },
            { checked: true, disabled: false, label: 'Current date - 4 (Mon)' },
            { checked: false, disabled: false, label: 'Current date - 5 (Sun)' },
            { checked: false, disabled: false, label: 'Current date - 6 (Sat)' },
            { checked: false, disabled: false, label: 'Current date - 7 (Fri)' }
        ],
        'Sat': [
            { checked: false, disabled: true, label: 'Current date - 1 (Fri)' },
            { checked: false, disabled: true, label: 'Current date - 2 (Thu)' },
            { checked: true, disabled: true, label: 'Current date - 3 (Wed)' },
            { checked: false, disabled: true, label: 'Current date - 4 (Tue)' },
            { checked: false, disabled: true, label: 'Current date - 5 (Mon)' },
            { checked: false, disabled: true, label: 'Current date - 6 (Sun)' },
            { checked: false, disabled: true, label: 'Current date - 7 (Sat)' }
        ],
        'Sun': [
            { checked: false, disabled: true, label: 'Current date - 1 (Sat)' },
            { checked: false, disabled: true, label: 'Current date - 2 (Fri)' },
            { checked: false, disabled: true, label: 'Current date - 3 (Thu)' },
            { checked: false, disabled: true, label: 'Current date - 4 (Wed)' },
            { checked: false, disabled: true, label: 'Current date - 5 (Tue)' },
            { checked: false, disabled: true, label: 'Current date - 6 (Mon)' },
            { checked: false, disabled: true, label: 'Current date - 7 (Sun)' }
        ]
    };

(function ($) {
    'use strict';

    var monthAndYearEl = null,
        previousIconEl = null,
        nextIconEl = null;

    var getDaysInMonth = function (month, year) {
        return 32 - new Date(year, month, 32).getDate();
    };

    var getWeekDay = function (date, month, year) {
        return days[new Date(year, month, date).getDay()];
    };

    var getWeeksBetweenTwoDate = function (fdate, fmonth, fyear, tdate, tmonth, tyear) {
        var totalDays = 0;
        if (fdate === tdate && fmonth === tmonth && fyear === tyear) {
            totalDays = 1;
        }

        if (fdate !== tdate && fmonth === tmonth && fyear === tyear) {
            totalDays = tdate - fdate + 1;
        }

        if (fmonth !== tmonth && fyear === tyear) {
            totalDays = getDaysInMonth(fmonth, fyear) - fdate + 1 + tdate;
            for (var month = fmonth + 1; month < tmonth; month++) {
                totalDays += getDaysInMonth(month, fyear);
            }
        }

        if (fyear !== tyear) {
            totalDays = getDaysInMonth(fmonth, fyear) - fdate + 1 + tdate;
            for (var fm = fmonth + 1; fm < 12; fm++) {
                totalDays += getDaysInMonth(fm, fyear);
            }
            for (var year = fyear + 1; year < tyear; year++) {
                for (var m = 0; m < 12; m++) {
                    totalDays += getDaysInMonth(m, year);
                }
            }
            for (var tm = 0; tm < tmonth; tm++) {
                totalDays += getDaysInMonth(tm, tyear);
            }
        }

        return Math.ceil(totalDays / 7);
    };

    var isToday = function (date, month, year) {
        var currentDate = currentDay ? new Date(currentDay) : new Date();
        return date === currentDate.getDate()
            && month === currentDate.getMonth()
            && year === currentDate.getFullYear();
    };

    var transformElId = function (elId) {
        return '#' + elId;
    };

    var renderCheckBox = function (classes, key, checked = false, disabled = false, label = null, labelPosition = 'end') {
        return `${label && labelPosition === 'start' ? `<label>${label}</label>` : ''}
                <input
                    type="checkbox"
                    class="${classes}"
                    data-key="${key}"
                    ${checked ? 'checked' : ''}
                    ${disabled ? 'disabled' : ''}
                />
                ${label && labelPosition === 'end' ? `<label>${label}</label>` : ''}`;
    };

    var renderMonthAndYearEl = function (month, year) {
        monthAndYearEl.html(`${months[month]} ${year}`);
    };

    var renderIconElements = function () {
        var currentGroup = parseInt(this.data('currentgroup')),
            lastGroup = parseInt(this.data('lastgroup'));
        if (currentGroup < lastGroup) {
            nextIconEl.removeClass('invisible');
        } else {
            nextIconEl.addClass('invisible');
        }

        if (currentGroup > 0) {
            previousIconEl.removeClass('invisible');
        } else {
            previousIconEl.addClass('invisible');
        }
    };

    var next = function () {
        var currentGroup = parseInt(this.data('currentgroup'));
        this.find(`tr[data-group="${currentGroup}"]`).addClass('hidden');
        this.find(`tr[data-group="${currentGroup + 1}"]`).removeClass('hidden');
        this.data('currentgroup', currentGroup + 1);
        this.attr('data-currentgroup', currentGroup + 1);

        renderIconElements.call(this);
    };

    var previous = function () {
        var currentGroup = parseInt(this.data('currentgroup'));
        this.find(`tr[data-group="${currentGroup}"]`).addClass('hidden');
        this.find(`tr[data-group="${currentGroup - 1}"]`).removeClass('hidden');
        this.data('currentgroup', currentGroup - 1);
        this.attr('data-currentgroup', currentGroup - 1);

        renderIconElements.call(this);
    };

    var onChange = function (event) {
        var el = event.currentTarget,
            key = $(el).data('key');
        this.find(`.secondary-checkbox[data-key="${key}"]`).prop('disabled', !el.checked);
    };

    var registerEvents = function () {
        nextIconEl.off('click').on('click', next.bind(this));
        previousIconEl.off('click').on('click', previous.bind(this));
        this.find('.primary-checkbox').off('change').on('change', onChange.bind(this));
    };

    var renderMoveBetSchedule = function (fromDate, toDate) {
        var fdate = fromDate.getDate(),
            fmonth = fromDate.getMonth(),
            fyear = fromDate.getFullYear(),
            tdate = toDate.getDate(),
            tmonth = toDate.getMonth(),
            tyear = toDate.getFullYear();

        var tbody = this.find('tbody');
        tbody.empty();

        var date = fdate,
            month = fmonth,
            year = fyear,
            weeks = getWeeksBetweenTwoDate(fdate, fmonth, fyear, tdate, tmonth, tyear),
            lastGroup = parseInt(weeks / 4);

        this.data({ 'currentgroup': 0, 'lastgroup': lastGroup });
        this.attr({ 'data-currentgroup': 0, 'data-lastgroup': lastGroup });
        renderMonthAndYearEl(month, year);
        renderIconElements.call(this);

        for (var week = 0; week < weeks; week++) {
            var group = parseInt(week / 4),
                hidden = week > 3,
                tr = $(`<tr class="tr-date ${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`),
                rows = [
                    $(`<tr class="${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`),
                    $(`<tr class="${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`),
                    $(`<tr class="${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`),
                    $(`<tr class="${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`),
                    $(`<tr class="${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`),
                    $(`<tr class="${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`),
                    $(`<tr class="${hidden ? 'hidden' : ''}" data-group="${group}"></tr>`)];

            if (week % 4 === 0) {
                var rowSpan = weeks - week < 4 ? (weeks - week) * 8 - 1 : 31;
                tr.append('<td>Move Bet Day <br /> (GMT -4)</td>');
                rows[0].append(`<td align="center" rowspan="${rowSpan}">For Days</td>`);
            }

            for (var day = 0; day < 7; day++) {
                var key = date;

                if (date > getDaysInMonth(month, year)) {
                    date = 1;
                    month++;
                }

                if (month > 11) {
                    month = 0;
                    year++;
                }

                var disabled = year === fyear && month === tmonth && date > tdate
                    || year === fyear && month > tmonth
                    || year > fyear,
                    weekDay = getWeekDay(date, month, year),
                    checked = weekDay !== days[0] && weekDay !== days[6];
                tr.append(`<td class="td-date">
                             ${renderCheckBox('primary-checkbox', key, checked && !disabled, disabled, weekDay, 'start')}
                             <div class="date-box ${isToday(date, month, year) ? 'current-day' : ''}">
                                <span>${date}</span>
                             </div>
                           </td >`);

                for (var i = 0; i < 7; i++) {
                    var option = defaultMoveBetSchedule[weekDay][i];
                    rows[i].append(`<td>${renderCheckBox('secondary-checkbox', key, option.checked, option.disabled || disabled, option.label)}</td>`);
                }

                date++;
            }

            tbody.append(tr, rows);
        }

        registerEvents.call(this);
    };

    $.fn.renderMoveBetSchedule = function (options) {
        var defaultOptions = {
            fromDate: null,
            toDate: null,
            monthAndYearElId: 'month-year-label',
            previousIconElId: 'previous-icon',
            nextIconElId: 'next-icon'
        };
        $.extend(defaultOptions, options);

        var fromDate = defaultOptions.fromDate || new Date(),
            toDate = defaultOptions.toDate || fromDate.setDate(fromDate.getDate() + 6);

        monthAndYearEl = this.find(transformElId(defaultOptions.monthAndYearElId));
        previousIconEl = this.find(transformElId(defaultOptions.previousIconElId));
        nextIconEl = this.find(transformElId(defaultOptions.nextIconElId));

        renderMoveBetSchedule.call(this, fromDate, toDate);

        return this;
    };
}(jQuery));