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
        url: "GetProductMatrix",
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                productMatrix = rs;
                RenderProduct();
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
    productMatrix.forEach(function (product, _) {
        var option = "<option value=\"" + product.ProductId + "\">" + product.ProductName +"</option>";
        productFilter.append(option);
    });
    productFilter.select2({ width: "135px" });
    productFilter.change(function () {
        var selectedProductId = $("#FilterProduct").val();
        selectedProduct = 0;
        RenderSportType();
        UpdateSelectSport();
        RenderBetTypeOptions();
    });

    isLoadCachedFilter = false;

    RenderSportType();
    UpdateSelectSport();
    RenderBetTypeOptions();
}

function RenderSportType() {
    var sportFilter = $("#FilterSportType");
    sportFilter.empty();
    sportFilter.removeAttr('disabled');
    var multiselectData = [];
    if (selectedProduct) {
        selectedProduct.Sports.forEach(function (item, index) {
            multiselectData.push({
                Id: 'sport' + item.SportId,
                Value: item.SportId,
                Text: item.SportName,
                Checked: isLoadCachedFilter && selectedConfig && selectedConfig.ConfigDetail.Config.SportTypeIdList && selectedConfig.ConfigDetail.Config.SportTypeIdList.indexOf(item.SportId) >= 0,
                Enable: true,
                CssClass: "",
                Items: []
            });
        });
    }

    sportFilter.multiSelect({
        minWidth: 140,
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
            isNeedRenderBettype = true;
            UpdateSelectSport();
        }
    });

    $("#FilterSportType").next('div.ui-multiselect-options').on('close', function () { return false; });
    $("#FilterSportType").next('div.ui-multiselect-options').off('close');
    $("#FilterSportType").next('div.ui-multiselect-options').on('close',
        function () {
            if (isNeedRenderBettype) RenderBetTypeOptions();
        });
}

function UpdateSelectSport() {
    selectedSports = [];
    var selectedSportIds = $("#FilterSportType").multiSelect("getCheckedItems");
    if (selectedSportIds) {
        selectedSportIds.forEach(function (item, index) {
            selectedSports.push(selectedProduct.Sports.find(element => element.SportId.toString() === item));
        });
    }
}

function RenderBetTypeOptions() {
    isNeedRenderBettype = false;
    var bettypeFilter = $("#FilterBettype");
    bettypeFilter.empty();
    bettypeFilter.removeAttr('disabled');
    var multiselectData = [];
    if (selectedSports) {
        selectedSports.forEach(function (item, index) {
            item.Bettypes.forEach(function (bt) {
                var existbt = multiselectData.find(element => element.Id === 'bettype' + bt.BettypeId);
                if (!existbt) {
                    multiselectData.push({
                        Id: 'bettype' + bt.BettypeId,
                        Value: bt.BettypeId,
                        Text: bt.BettypeName,
                        Checked: isLoadCachedFilter && selectedConfig && selectedConfig.ConfigDetail.Config.BetTypeIdList && selectedConfig.ConfigDetail.Config.BetTypeIdList.indexOf(bt.BettypeId) >= 0,
                        Enable: true,
                        CssClass: "",
                        Items: []
                    });
                }
            });
        });
    }

    bettypeFilter.multiSelect({
        minWidth: 140,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: false,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "Select Bettype"
    });
}

function RenderCurrency() {
    $("#EditProductSelect").select2({ width: "135px" });
    $("#EditProductSelect").select2({ width: "135px" });
}


$(document).ready(function () {
    RenderDcsMatrix();
});