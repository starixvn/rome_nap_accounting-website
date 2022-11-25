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

    OpenProductSetting();
});

function UpdateSampleUrl() {
    $("#btnUpdateExcel").addClass("hide");
    switch (cachedTabId) {
        case "tab_1":
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
            $("#btnUpdateExcel").removeClass("hide");
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

function GetPartialView(requestUrl) {
    parent.BlockUI();
    $("#setting-content").html("<div id=\"errMsg\"></div>");
    $.ajax({
        type: "GET",
        url: requestUrl,
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                $("#setting-content").html(rs);
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function UpdateTabs() {
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#' + cachedTabId).addClass('current');
}

function OpenProductSetting() {
    cachedTabId = "tab_product";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Product/Product.xls");
    UpdateTabs();
    GetPartialView("ProductIndex");
}


function OpenSportSetting() {
    cachedTabId = "tab_sport";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Sport/SportType.xls");
    UpdateTabs();
    GetPartialView("SportIndex");
}

function OpenBettypeSetting() {
    cachedTabId = "tab_bettype";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Bettype/Bettype.xls");
    UpdateTabs();
    GetPartialView("BettypeIndex");
}

function OpenSBMSetting() {
    cachedTabId = "tab_sbm";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/SportBettypeMatrix/SportBettypeMatrix.xls");
    UpdateTabs();
    GetPartialView("SBMIndex");
}

function OpenCSSBMSetting() {
    cachedTabId = "tab_cssbm";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/CSSportBettypeMatrix/CS_SportBettypeMatrix.xls");
    UpdateTabs();
    GetPartialView("CSSBMIndex");
}

function OpenRefResourceSetting() {
    cachedTabId = "tab_refresource";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/RefResource/RefResource.xls");
    UpdateTabs();
    GetPartialView("RefResourceIndex");
}

function OpenCanSeeSetting() {
    cachedTabId = "tab_cansee";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/CanSee/CanSee.xls");
    UpdateTabs();
    GetPartialView("CanSeeIndex");
}

function OpenResetWinningSetting() {
    cachedTabId = "tab_resetwinning";
    UpdateTabs();
    GetPartialView("ResetWinningIndex");
}

function OpenUMConfigSetting() {
    cachedTabId = "tab_umconfig";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/UMConfig/UMConfig.xls");
    UpdateTabs();
    GetPartialView("UmConfigIndex");
}

function OpenVendorSetting() {
    cachedTabId = "tab_vendor";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/VendorGlobalSetting/VendorGlobalSetting.xls");
    UpdateTabs();
    GetPartialView("VendorIndex");
}

function OpenProduct3rdSetting() {
    cachedTabId = "tab_product3rd";
    $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/Product3rd/Product3rd.xls");
    UpdateTabs();
    GetPartialView("Product3rdIndex");
}