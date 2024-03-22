var getNotAllowedNewSuperPrefixes = function () {
    if (parent.level !== "super") { return; }

    $.ajax({
        type: "GET",
        url: parent.getSuperPrefixesUrl,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var $notAllowedSuperPrefixes = $("#notAllowedSuperPrefixes");
            var $notAllowedSuperPrefixesStepOne = $("#notAllowedSuperPrefixes_step1");
            var $notAllowedSuperPrefixesStepTwo = $("#notAllowedSuperPrefixes_step2");
            if (data && data.length > 0) {
                var warningStepTwo = "For 789y site, the following username prefixes are not allowed for new accounts anymore";
                var warningStepOne = `<span class="start">${warningStepTwo}</span>`;
                $.each(data, function (index, item) {
                    warningStepOne += `<br /><span style="margin-left: 20px">- ${item.groupName}: ${item.prefixes}</span>`;
                    warningStepTwo += `<br />- ${item.groupName}: ${item.prefixes}`;
                });
                $notAllowedSuperPrefixes.addClass("headlineBelongSiteMsg");
                $notAllowedSuperPrefixes.html(warningStepTwo);
                $notAllowedSuperPrefixesStepOne.html(warningStepOne);
                $notAllowedSuperPrefixesStepTwo.html(warningStepTwo);
                $notAllowedSuperPrefixesStepTwo.addClass("headlineBelongSiteMsg");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { }
    });
};

$(function () {
    getNotAllowedNewSuperPrefixes();
});