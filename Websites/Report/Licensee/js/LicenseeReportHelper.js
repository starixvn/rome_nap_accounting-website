var LicenseeReportHelper = (function ($) {
    return {
        displayLicensee: function () {
            var checkedVals = '';
            checkedVals = $('.group1:checkbox:checked').map(function () {
                return this.value;
            }).get().join('+');

            if (checkedVals == '') {
                checkedVals = $('.group1').map(function () {
                    return this.value;
                }).get().join(' + ');
            }
            $('#titleLicensee').html('Licensee W/L (' + checkedVals + ')');
            $('#titleCompany').html(checkedVals);
        }
    };
})(jQuery);