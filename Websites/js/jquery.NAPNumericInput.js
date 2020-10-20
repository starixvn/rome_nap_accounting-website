(function ($) {
    $.fn.NAPNumericInput = function (options) {
        var allowedKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'Enter', 'Delete', 'Backspace', '.', '-', 'ArrowLeft', 'ArrowRight'];
        return this.each(function () {
            var settings = $.extend({
                numOfDecimal: 0,
                minValue: null,
                maxValue: null,
                checkRange: true,
                showTooltip: true
            }, options);

            var $this = $(this);
            $this.off();
            $this.attr('autocomplete', 'off');
            $this.on('paste', function (e) {
                var pastedData = e.originalEvent.clipboardData.getData('text');
                if (!isNaN(settings.numOfDecimal) && settings.numOfDecimal > 0 && pastedData.toString().split('.').length > 1 && pastedData.toString().split('.')[1].length > settings.numOfDecimal)
                    return false;
                if (pastedData !== '' && !$.isNumeric(pastedData))
                    return false;
                if (settings.checkRange && (parseFloat(pastedData) < parseFloat(settings.minValue) || parseFloat(pastedData) > parseFloat(settings.maxValue)))
                    return false;
            });
            if (settings.showTooltip === true)
                $this.attr('title', '[' + settings.minValue + ', ' + settings.maxValue + ']');
            $this.keydown(function (e) {
                if ((e.ctrlKey && e.keyCode === 67)
                    || (e.ctrlKey && e.keyCode === 86)
                    || (e.ctrlKey && e.keyCode === 65)
                    || e.key === 'Tab')
                    return true;
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

                if (e.key === '.' && e.target.value.indexOf('.') === -1 && !isNaN(settings.numOfDecimal) && settings.numOfDecimal > 0)
                    return true;
                if (!isNaN(settings.numOfDecimal) && settings.numOfDecimal > 0 && newValue.toString().split('.').length > 1 && newValue.toString().split('.')[1].length > settings.numOfDecimal)
                    return false;
                if (newValue !== '' && !$.isNumeric(newValue))
                    return false;
                if (settings.checkRange && (parseFloat(newValue) < parseFloat(settings.minValue) || parseFloat(newValue) > parseFloat(settings.maxValue)))
                    return false;
            });
            return this;
        });
    };
}(jQuery));