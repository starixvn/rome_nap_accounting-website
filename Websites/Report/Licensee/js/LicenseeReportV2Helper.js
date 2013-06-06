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
        },

        CheckLicenseeOfferRNGCasino: function (LicenseeId) {
            var ListLicenseeOffer = [19, 20, 22, 23, 24, 25, 30];
            var isOffer = false;
            for (var i = 0; i < ListLicenseeOffer.length; i++) {
                if (ListLicenseeOffer[i] == LicenseeId) {
                    isOffer = true;
                    break;
                }
            }

            // no need to check
            /*
            if (isOffer) {
                $('#spRNGCasino').show();
                $('#chbRNGCasino').attr('checked', true).addClass('chksport ');
            }
            else {
                $('#chbRNGCasino').attr('checked', false).removeClass('chksport ');
                $('#spRNGCasino').hide();
                
            }*/
        }
    };
})(jQuery);