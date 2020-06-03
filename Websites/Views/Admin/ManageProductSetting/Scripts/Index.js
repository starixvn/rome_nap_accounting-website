var cachedTabId;
$(document).ready(function () {
    cachedTabId = "tab_1";

    $('.numeric-input-int').NAPNumericInput({
        numOfDecimal: 0,
        minValue: 0,
        maxValue: 2147483647
    });
    $('.numeric-input-smallint').NAPNumericInput({
        numOfDecimal: 0,
        minValue: 0,
        maxValue: 32767
    });
    $('.numeric-input-tinyint').NAPNumericInput({
        numOfDecimal: 0,
        minValue: 0,
        maxValue: 255
    });
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
            SearchProduct(50, 1);
            break;
        case "tab_2":
            SearchSport(50, 1);
            break;
        case "tab_3":
            SearchBettype(50, 1);
            break;
        case "tab_4":
            SearchSBM(50, 1);
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
    $('#errMsg').html('');
    var $uploadFile = $('#uploadFile');
    $uploadFile.replaceWith($uploadFile.val('').clone(true));
    $('#insert-setting').dialog({
        title: 'Import From Excel',
        modal: true,
        width: 700,
        height: 200,
    });
}