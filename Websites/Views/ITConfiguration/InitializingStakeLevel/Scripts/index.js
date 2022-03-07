var selectedProduct = null;
var selectedSports = [];
var isLoadCachedFilter = false;
var isNeedRenderBettype = false;
var selectedProductId = null;
var productMatrix = [];

function SwitchTab(id, contentId) {
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#' + id).addClass('current');
    $('#' + contentId).show();
}

function RenderDcsMatrix() {
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "GetFilterData",
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                RenderProduct(rs.Products);
                RenderCreditCurrency(rs.CreditCurrencies);
                RenderDepositCurrency(rs.DepositCurrencies);
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderProduct(products) {
    isLoadCachedFilter = true;
    var productFilter = $("#FilterProduct");
    productFilter.empty();
    products.forEach(function (product, _) {
        var option = "<option value=\"" + product.Id + "\">" + product.Name +"</option>";
        productFilter.append(option);
    });
    productFilter.select2({ width: "135px" });
    productFilter.change(function () {
        selectedProductId = $("#FilterProduct").val();
        RenderSportType();
    });
}

function RenderSportType() {
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "GetSportTypes?productId=" + selectedProductId,
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                var sportFilter = $("#FilterSportType");
                sportFilter.empty();
                sportFilter.removeAttr('disabled');
                var multiselectData = [];
                rs.forEach(function (item, index) {
                    multiselectData.push({
                        Id: 'sport' + item.Id,
                        Value: item.Id,
                        Text: item.Name,
                        Checked: false,
                        Enable: true,
                        CssClass: "",
                        Items: []
                    });
                });

                sportFilter.multiSelect({
                    minWidth: 200,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: false,
                    showFooter: false,
                    showFilter: true,
                    checkAllText: "All",
                    listData: multiselectData,
                    noItemText: "Select Sport",
                    onCheck: function () {
                        RenderBetType();
                    }
                });
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderBetType() {
    parent.BlockUI();
    var selectedSportIds = $("#EditSportSelect").multiSelect("getCheckedItems").join();
    var request = {
        ProductId: selectedProduct,
        SportTypes: selectedSportIds
    };

    $.ajax({
        type: "POST",
        data: request,
        url: "GetBetTypes",
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                var bettypeFilter = $("#FilterBettype");
                bettypeFilter.empty();
                bettypeFilter.removeAttr('disabled');
                var multiselectData = [];
                rs.forEach(function (item, index) {
                    multiselectData.push({
                        Id: 'bettype' + item.Id,
                        Value: item.Id,
                        Text: item.Name,
                        Checked: false,
                        Enable: true,
                        CssClass: "",
                        Items: []
                    });
                });

                bettypeFilter.multiSelect({
                    minWidth: 200,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: false,
                    showFooter: false,
                    showFilter: true,
                    checkAllText: "All",
                    listData: multiselectData,
                    noItemText: "Select Bet Type"
                });
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderCreditCurrency(currencies) {
    RenderCurrency(currencies, "Credit");
}

function RenderDepositCurrency(currencies) {
    RenderCurrency(currencies, "Deposit");
}

function RenderCurrency(currencies, type) {
    var currencyFilter = $("#Filter" + type);
    currencyFilter.empty();
    currencyFilter.removeAttr('disabled');
    var multiselectData = [];
    currencies.forEach(function (currency, index) {
        multiselectData.push({
            Id: type + currency.Id,
            Value: currency.Id,
            Text: currency.Name,
            Checked: false,
            Enable: true,
            CssClass: "",
            Items: []
        });
    });

    currencyFilter.multiSelect({
        minWidth: 200,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: false,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "Select " + type + " currency"
    });
}


$(document).ready(function () {
    RenderDcsMatrix();
});