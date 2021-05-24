(function ($) {
    'use strict';

    var days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

    var $monthAndYearEl = null,
        $previousIconEl = null,
        $nextIconEl = null,
        $btnSave = null,
        onSaveCallBack = null;

    var daysPerWeek = 7,
        daysPerMonth = 31,
        rowNeededForADay = 8,
        weeksPerMonth = 4;

    var forwardStep = 1,
        backwardStep = -1;

    var isToday = function (date) {
        var currentDate = currentDay ? new Date(currentDay) : new Date();

        return date.getDate() === currentDate.getDate()
            && date.getMonth() === currentDate.getMonth()
            && date.getFullYear() === currentDate.getFullYear();
    };

    var transformElId = function (elId) {
        return '#' + elId;
    };

    var renderCheckBox = function (prop) {
        var startLabel = prop.label && prop.labelPosition === 'start' ? `<label>${prop.label}</label>` : '',
            endLabel = prop.label && prop.labelPosition === 'end' ? `<label>${prop.label}</label>` : '',
            dataWeekDayAttr = prop.weekDayIndex > -1 ? `data-weekday="${prop.weekDayIndex}"` : '',
            dataOptionAttr = prop.optionIndex > -1 ? `data-option="${prop.optionIndex}"` : '',
            checkedAttr = prop.checked ? 'checked' : '',
            disabledAttr = prop.disabled ? 'disabled' : '';

        return `${startLabel}
                <input
                    type="checkbox"
                    class="${prop.classes}"
                    data-key="${prop.key}"
                    ${dataWeekDayAttr}
                    ${dataOptionAttr}
                    data-name="${prop.name}"
                    value="${prop.value}"
                    ${checkedAttr}
                    ${disabledAttr}
                />
                ${endLabel}`;
    };

    var renderMonAndYearElement = function (date) {
        $monthAndYearEl.html(`${months[date.getMonth()]} ${date.getFullYear()}`);
        $monthAndYearEl.hide();
    };

    var renderIconElements = function () {
        var currentGroup = parseInt(this.data('currentgroup')),
            lastGroup = parseInt(this.data('lastgroup'));
        if (currentGroup < lastGroup) {
            $nextIconEl.removeClass('disabled');
        } else {
            $nextIconEl.addClass('disabled');
        }

        if (currentGroup > 0) {
            $previousIconEl.removeClass('disabled');
        } else {
            $previousIconEl.addClass('disabled');
        }
    };

    var move = function (step) {
        if (step === backwardStep && $previousIconEl.hasClass('disabled')
            || step === forwardStep && $nextIconEl.hasClass('disabled')) {
            return;
        }

        var currentGroup = parseInt(this.data('currentgroup'));
        this.find(`tr[data-group="${currentGroup}"]`).addClass('hidden');
        this.find(`tr[data-group="${currentGroup + step}"]`).removeClass('hidden');
        this.data('currentgroup', currentGroup + step);
        this.attr('data-currentgroup', currentGroup + step);

        renderIconElements.call(this);
    };

    var onChange = function (event) {
        var el = event.currentTarget,
            key = $(el).data('key');
        this.find(`.secondary-checkbox[data-key="${key}"]`).prop('disabled', !el.checked);
    };

    var highlightNewChange = function (event) {
        var el = event.currentTarget,
            key = $(el).data('key'),
            weekDay = $(el).data('weekday');

        var isChanged = false,
            secondaryCheckBoxList = this.find(`.secondary-checkbox[data-key="${key}"]`);
        secondaryCheckBoxList.each(function () {
            var option = $(this).data('option');
            if (this.checked !== defaultMoveBetSchedule[weekDay].Options[option]) {
                isChanged = true;
                return false;
            }
        });
        secondaryCheckBoxList.each(function () {
            $(this).parent().css('background-color', isChanged ? 'yellow' : 'unset');
        });
    };

    var highlightNewChangeAfterRendering = function (changedIndexList) {
        var $this = this;
        $.each(changedIndexList, function (index, keyValue) {
            $this.find(`.secondary-checkbox[data-key="${keyValue}"]`).each(function () {
                $(this).parent().css('background-color', 'yellow');
            });
        });
    };

    var onSave = function () {
        var $this = this,
            data = '<ROOT>';
        $this.find('.primary-checkbox').each(function () {
            if (this.checked && !$(this).prop('disabled')) {
                var key = $(this).data('key'),
                    option = `<File ${$(this).data('name')}="${$(this).val()}"`;
                $this.find(`.secondary-checkbox[data-key="${key}"]`).each(function () {
                    option += ` ${$(this).data('name')}="${this.checked ? 1 : 0}"`;
                });
                data += `${option} />`;
            }
        });

        data += '</ROOT>';
        onSaveCallBack(data);
    };

    var registerEvents = function () {
        $nextIconEl.off('click').on('click', move.bind(this, forwardStep));
        $previousIconEl.off('click').on('click', move.bind(this, backwardStep));
        $btnSave.off('click').on('click', onSave.bind(this));
        this.find('.primary-checkbox').off('change').on('change', onChange.bind(this));
        this.find('.secondary-checkbox').off('change').on('change', highlightNewChange.bind(this));
    };

    var renderMoveBetSchedule = function (data) {
        var tbody = this.find('tbody');
        tbody.empty();

        var length = data.length,
            weeks = Math.ceil(length / daysPerWeek),
            lastGroupIndex = Math.ceil(weeks / weeksPerMonth) - 1;

        this.data({ 'currentgroup': 0, 'lastgroup': lastGroupIndex });
        this.attr({ 'data-currentgroup': 0, 'data-lastgroup': lastGroupIndex });

        var fromDate = new Date([data[0].Date]),
            toDate = new Date([data[length - 1].Date]);

        renderMonAndYearElement(fromDate);
        renderIconElements.call(this);

        var dataIndex = 0,
            changedIndexList = [];
        for (var week = 0; week < weeks; week++) {
            var group = parseInt(week / weeksPerMonth),
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

            if (week % weeksPerMonth === 0) {
                var rowSpan = weeks - week < weeksPerMonth ? (weeks - week) * rowNeededForADay - 1 : daysPerMonth;
                tr.append('<td class="td-date">Move Bet Day <br /> (GMT -4)</td>');
                rows[0].append(`<td align="center" rowspan="${rowSpan}">For Days</td>`);
            }

            for (var day = 0; day < daysPerWeek; day++) {
                var stillInRange = dataIndex < length,
                    date = stillInRange ? new Date(data[dataIndex].Date) : new Date(toDate.setDate(toDate.getDate() + 1)),
                    weekDay = stillInRange ? data[dataIndex].WeekDay : date.getDay(),
                    options = stillInRange ? data[dataIndex].Options : defaultMoveBetSchedule[weekDay].Options;

                var disabled = dataIndex >= length,
                    checked = false,
                    weekDayName = days[weekDay],
                    localeDate = date.toLocaleDateString();

                tr.append(
                    `<td class="td-date">
                    ${renderCheckBox({
                        classes: 'primary-checkbox',
                        key: dataIndex,
                        name: 'MBDate',
                        value: localeDate,
                        checked: checked && !disabled,
                        disabled: disabled,
                        label: weekDayName,
                        labelPosition: 'start',
                        weekDayIndex: -1,
                        optionIndex: -1
                    })}
                        <div class="date-box ${isToday(date) ? 'current-day' : ''}">
                            <span>${localeDate}</span>
                        </div>
                    </td>`);

                var isChanged = false;
                for (var row = 0; row < daysPerWeek; row++) {
                    if (!isChanged && options[row] !== defaultMoveBetSchedule[weekDay].Options[row]) {
                        isChanged = true;
                    }

                    var backDay = weekDay - row - 1 < 0 ? daysPerWeek + weekDay - row - 1 : weekDay - row - 1,
                        option = {
                            name: `IsBackDay${row + 1}`,
                            disabled: true,
                            checked: options[row],
                            label: `Current date - ${row + 1} (${days[backDay]})`
                        };

                    rows[row].append(`<td>
                        ${renderCheckBox({
                            classes: 'secondary-checkbox',
                            key: dataIndex,
                            name: option.name,
                            value: '',
                            checked: option.checked,
                            disabled: option.disabled || disabled,
                            label: option.label,
                            labelPosition: 'end',
                            weekDayIndex: weekDay,
                            optionIndex: row
                        })}</td>`);
                }

                if (isChanged) {
                    changedIndexList.push(dataIndex);
                }
                dataIndex++;
            }

            tbody.append(tr, rows);
        }

        highlightNewChangeAfterRendering.call(this, changedIndexList);
        registerEvents.call(this);
    };

    $.fn.renderMoveBetSchedule = function (options) {
        var defaultOptions = {
            data: null,
            monthAndYearElId: 'month-year-label',
            previousIconElId: 'previous-icon',
            nextIconElId: 'next-icon',
            btnSave: 'btn-save',
            onSave: function () { return; }
        };

        $.extend(defaultOptions, options);

        $monthAndYearEl = this.find(transformElId(defaultOptions.monthAndYearElId));
        $previousIconEl = this.find(transformElId(defaultOptions.previousIconElId));
        $nextIconEl = this.find(transformElId(defaultOptions.nextIconElId));
        $btnSave = $(transformElId(defaultOptions.btnSave));
        onSaveCallBack = defaultOptions.onSave;

        renderMoveBetSchedule.call(this, defaultOptions.data || []);

        return this;
    };
}(jQuery));