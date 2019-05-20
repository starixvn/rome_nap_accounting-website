var BetList = BetList || {};

BetList.LanguageDropdownList = (function () {
    var $callbackFunction;
    var $languageItems;
    var $languageBox;

    function setJqueryMap(callbackFunction) {
        $languageItems = $("#language-ddl li");
        $languageBox = $("#lang-box");
        $callbackFunction = callbackFunction;
    };

    function init(callbackFunction) {
        setJqueryMap(callbackFunction);


        $languageBox.click(function (event) {
            event.stopPropagation();
            $("#language-ddl").show();
        });

        $languageItems.click(function () {
            var languageCode = $(this).find('span').data('language-code');
            var languageName = $(this).find('span').data('language-name');
            $("#languageCode").removeClass();
            $("#languageCode").addClass("flag flag-" + languageCode);
            $("#languageName").html(languageName);
            $("#languageCodeHidden").val(languageCode);
            $("#language-ddl").hide();
            $callbackFunction.call();
        })
        $(window).click(function () {
            $("#language-ddl").hide();
        })
    };

    return {
        init: init
    }

})(jQuery);