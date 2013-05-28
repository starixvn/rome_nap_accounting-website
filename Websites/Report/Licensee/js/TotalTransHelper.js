var TotalTransHelper = (function ($) {
    return {

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
                $('#chkRNGCasino').attr('checked', true).addClass('chksport ');
            }
            else {
                $('#chkRNGCasino').attr('checked', false).removeClass('chksport ');
                $('#spRNGCasino').hide();
            }*/
        }
    };
})(jQuery);