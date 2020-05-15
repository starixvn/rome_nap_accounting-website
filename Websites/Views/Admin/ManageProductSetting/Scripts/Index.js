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
    UpdateSampleUrl();
}

function Search() {
    switch (cachedTabId) {
        case "tab_1":
            SearchProduct();
            break;
        case "tab_2":
            SearchSport();
            break;
        case "tab_3":
            SearchBettype();
            break;
        case "tab_4":
            SearchSBM();
            break;
    }
}
function UpdateSampleUrl() {
    switch (cachedTabId) {
        case "tab_1":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Product/Product.xls");
            break;
        case "tab_2":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Sport/SportType.xls");
            break;
        case "tab_3":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Bettype/Bettype.xls");
            break;
        case "tab_4":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/SportBettypeMatrix/SportBettypeMatrix.xls");
            break;
    }
}

function OpenInsertSetting() {
    $('#insert-setting').dialog({
        title: 'Import From Excel',
        modal: true,
        width: 700,
        height: 200,
    });
}