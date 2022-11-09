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

    SearchProduct(50, 1);
});

function SwitchTab(id, contentId) {
    cachedTabId = id;
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#' + id).addClass('current');
    $('#' + contentId).show();
    UpdateSampleUrl();
    InitTab();
}

function InitTab() {
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
        case "tab_5":
            GetGroupsAndSearchCSSBM(50, 1);
            break;
        case "tab_6":
            SearchRefResource(50, 1);
            break;
        case "tab_7":
            SearchCanSee();
            break;
        case "tab_8":
            SearchResetWinning();
            break;
        case "tab_9":
            GetInitDataAndSearchAllUMConfig();
            break;
        case "tab_10":
            GetInitVendor();
            break;
        case "tab_11":
            GetInitDataProduct3rd();
            break;
        default:
            break;
    }
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
        case "tab_5":
            GetGroupsAndSearchCSSBM(50, 1);
            break;
        case "tab_6":
            SearchRefResource(50, 1);
            break;
        case "tab_7":
            SearchCanSee();
            break;
        case "tab_8":
            SearchResetWinning();
            break;
        case "tab_9":
            GetInitDataAndSearchAllUMConfig();
            break;
        case "tab_10":
            SearchVendor(1, 50);
            break;
        case "tab_11":
            SearchProduct3rd(1, 50);
            break;
        default:
            break;
    }
}

function UpdateSampleUrl() {
    $("#btnUpdateExcel").addClass("hide");
    switch (cachedTabId) {
        case "tab_1":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Product/Product.xls");
            break;
        case "tab_2":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Sport/SportType.xls");
            break;
        case "tab_3":
            $("#btnUpdateExcel").removeClass("hide");
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Bettype/Bettype.xls");
            break;
        case "tab_4":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/SportBettypeMatrix/SportBettypeMatrix.xls");
            break;
        case "tab_5":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/CSSportBettypeMatrix/CS_SportBettypeMatrix.xls");
            break;
        case "tab_6":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/RefResource/RefResource.xls");
            break;
        case "tab_7":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/CanSee/CanSee.xls");
            break;
        case "tab_8":
            UpdateResetWinningSampleUrl();
            break;
        case "tab_9":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/UMConfig/UMConfig.xls");
            break;
        case "tab_10":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/VendorGlobalSetting/VendorGlobalSetting.xls");
            break;
        case "tab_11":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Product3rd/Product3rd.xls");
            break;
        default:
            break;
    }
}