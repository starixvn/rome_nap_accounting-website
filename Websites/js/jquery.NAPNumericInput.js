(function ($) {
    $.fn.NAPNumericInput = function (options) {
        var allowedKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'Enter', 'Delete', 'Backspace', '.', '-', 'Tab', 'ArrowLeft', 'ArrowRight'];
        return this.each(function () {
            var settings = $.extend({
                numOfDecimal: 0,
                minValue: null,
                maxValue: null,
                showTooltip: true
            }, options);

            var $this = $(this);
            $this.off();
            $this.attr('autocomplete', 'off');
            $this.on('paste', function (e) { return false; });
            if (settings.showTooltip === true)
                $this.attr('title', '[' + settings.minValue + ', ' + settings.maxValue + ']')
            $this.keydown(function (e) {
                if ($.inArray(e.key, allowedKeys) < 0)
                    return false;
                var oldValue = e.target.value;
                var newValue = oldValue + e.key;
                settings.numOfDecimal = parseInt(settings.numOfDecimal);
                var selectionStart = e.target.selectionStart;
                var selectionEnd = e.target.selectionEnd;
                if (selectionEnd > selectionStart) {
                    if (e.key === 'Delete' || e.key === 'Backspace')
                        newValue = oldValue.substring(0, selectionStart) + oldValue.substring(selectionEnd);
                    else
                        newValue = oldValue.substring(0, selectionStart) + e.key + oldValue.substring(selectionEnd);
                }
                else {
                    if (e.key === 'Delete')
                        newValue = oldValue.substring(0, selectionStart) + oldValue.substring(selectionStart + 1);
                    else if (e.key === 'Backspace')
                        newValue = oldValue.substring(0, selectionStart - 1) + oldValue.substring(selectionStart);
                    else
                        newValue = oldValue.substring(0, selectionStart) + e.key + oldValue.substring(selectionStart);
                }

                if (e.key === '.' && e.target.value.indexOf('.') == -1 && settings.numOfDecimal !== NaN && settings.numOfDecimal > 0)
                    return true;
                if (settings.numOfDecimal !== NaN && settings.numOfDecimal > 0 && newValue.toString().split('.').length > 1 && newValue.toString().split('.')[1].length > settings.numOfDecimal)
                    return false;
                if (newValue !== '' && !$.isNumeric(newValue))
                    return false;
                if (parseFloat(newValue) < parseFloat(settings.minValue) || parseFloat(newValue) > parseFloat(settings.maxValue))
                    return false;
            });

            return this;
        });
    }
}(jQuery));