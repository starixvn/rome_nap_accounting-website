var getHeadlineBelongSite = function () {
    if (parent.level !== "super") { return; }

    $.ajax({
        type: "POST",
        url: parent.getHeadlineBelongSiteUrl,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var $messageHtml = $("#headlineBelongSite");
            if (data && data.length > 0) {
                var message = "For 789y site group, the following headlines belong to site:";
                $.each(data, function (index, item) {                    
                    message += `<br />- ${item.Site}: ${item.Headline}`;
                });
                $messageHtml.addClass("headlineBelongSiteMsg");
                $messageHtml.html(message);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) { }
    });
};

$(function () {
    getHeadlineBelongSite();
});