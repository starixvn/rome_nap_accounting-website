var cachedTabId;

$(document).ready(function () {
    cachedTabId = "tab_1";
});

function SwitchTab(id, contentId) {
    cachedTabId = id;
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#' + id).addClass('current');
    $('#' + contentId).show();
    InitTab();
}

function InitTab() {
    switch (cachedTabId) {
        case "tab_1":
            break;
        case "tab_2":
            break;
        default:
            break;
    }
}