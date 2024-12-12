

$(document).ready(function () {
    var tabName = $("#tab-name").val() || 'tab_1';
    SwitchTab(tabName);
    InitManageSettingsPage();
    editSuperInfo.init();
});

function Reset(tabName) {
    location.href = "./Index?tabName=" + tabName;
}

function InitManageSettingsPage() {
    $(".superlist").off("change").on("change", function () {
        var superlist = $(this);
        var superlistValue = $.trim(superlist.val());
        if (superlistValue.length > 0) {
            if (superlistValue.indexOf("\n") >= 0) {
                superlistValue = superlistValue.replace(/\n/g, " ");
            }

            if (superlistValue.indexOf("\r") >= 0) {
                superlistValue = superlistValue.replace(/\r/g, " ");
            }

            if (superlistValue.indexOf("\t") >= 0) {
                superlistValue = superlistValue.replace(/\t/g, " ");
            }

            superlistValue = superlistValue.replace(/\s/g, ",");
            var superlistArray = superlistValue.split(",").filter(item => item.length > 0);
            superlist.val(superlistArray.join(","));
        }
    });

    $(".icon-collapse-expand").click(function (e) {
        var $this = $(this);
        var $filter = $this.parent().find('>div');

        if ($this.hasClass("icon-down")) {
            $this.removeClass("icon-down");
            $this.addClass("icon-up");
            $filter.addClass('hidden');
        } else {
            $this.removeClass("icon-up");
            $this.addClass("icon-down");
            $filter.removeClass('hidden');
        }
    });
}

function scrollPageToTop() {
    window.$('.mCustomScrollbar').mCustomScrollbar("scrollTo", "top");
}