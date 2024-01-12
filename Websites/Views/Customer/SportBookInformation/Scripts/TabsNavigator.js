const numberOfTabs = 3
var TabsNavigator = {};

TabsNavigator.init = function () {
    this.tabs.init();
};

TabsNavigator.tabs = {
    init: function () {
        for (var i = 1; i <= numberOfTabs; i++) {
            $("#left_tab1_" + i).click(function () {
                for (var j = 1; j <= numberOfTabs; j++) {
                    $("#left_tab1_" + j).removeAttr("class");
                }
                $(this).attr("class", "current");
                for (var k = 1; k <= numberOfTabs; k++) {
                    $("#left_tab1_" + k + "_content").hide();
                }
                $("#" + $(this).attr("id") + "_content").show();
            });
        }
        $("#left_tab1_1").click(function () {
            console.log("left_tab1_1 click")
        });
        $("#left_tab1_2").click(function () {
            console.log("left_tab1_2 click")
        });
        $("#left_tab1_3").click(function () {
            console.log("left_tab1_3 click")
        });
    }
};

$(document).ready(function () {
    TabsNavigator.init();
});