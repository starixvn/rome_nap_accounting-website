var isNeedRenderBettype = false;
var selectedProductId = null;
var requestFilter = {};

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
    var productFilter = $("#FilterProduct");
    productFilter.empty();
    products.forEach(function (product, _) {
        var option = "<option value=\"" + product.Id + "\">" + product.Name +"</option>";
        productFilter.append(option);
    });
    productFilter.select2({ width: "200px" });
    productFilter.change(function () {
        selectedProductId = $("#FilterProduct").val();
        RenderSportType();
    });
    productFilter.trigger("change");
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
                        isNeedRenderBettype = true;
                    }
                });

                $("#FilterSportType").next('div.ui-multiselect-options').on('close', function () { return false; });
                $("#FilterSportType").next('div.ui-multiselect-options').off('close');
                $("#FilterSportType").next('div.ui-multiselect-options').on('close',
                    function () {
                        if (isNeedRenderBettype) RenderBetType();
                    });
                RenderBetType();
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
    isNeedRenderBettype = false;
    parent.BlockUI();
    var selectedSportIds = $("#FilterSportType").multiSelect("getCheckedItems").join();
    var request = {
        ProductId: selectedProductId,
        SportTypes: selectedSportIds
    };

    $.ajax({
        type: "POST",
        url: "GetBetTypes",
        data: request,
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                var bettypeFilter = $("#FilterBetType");
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

function Search() {
    UpdateFilterValue();

    if (ValidateRequest()) {
        GetStakeLevels(50, 0);
    }
}

function UpdateFilterValue() {
    var selectedSportIds = $("#FilterSportType").multiSelect("getCheckedItems").join();
    var selectedBetTypeIds = $("#FilterBetType").multiSelect("getCheckedItems").join();
    requestFilter = {
        ProductID: selectedProductId,
        SportTypes: selectedSportIds,
        BetTypes: selectedBetTypeIds,
        DepositCurrency: '0',
        CreditCurrency: '0'
    };

    var isCheckAllDepositCurrency = $("#FilterDeposit").multiSelect("getUncheckItems").length === 0;
    var isCheckAllCreditCurrency = $("#FilterCredit").multiSelect("getUncheckItems").length === 0;
    if (!isCheckAllDepositCurrency) {
        requestFilter.DepositCurrency = $("#FilterDeposit").multiSelect("getCheckedItems").join();
    }

    if (!isCheckAllCreditCurrency) {
        requestFilter.CreditCurrency = $("#FilterCredit").multiSelect("getCheckedItems").join();
    }
}

function ValidateRequest(request) {
    $("#filterError").html("");
    if (requestFilter.SportTypes === null || requestFilter.SportTypes.length === 0) {
        showAlertMessage("filterError", "Sport Type cannot be null.", 1);
        return false;
    }

    if (requestFilter.BetTypes === null || requestFilter.BetTypes.length === 0) {
        showAlertMessage("filterError", "Bet Type cannot be null.", 1);
        return false;
    }

    return true;
}

function GetStakeLevels(pageSize, pageIndex) {
    parent.BlockUI();
    $("#errMsg").html("");
    requestFilter.PageSize = pageSize;
    requestFilter.PageIndex = pageIndex;
    $.ajax({
        type: "POST",
        url: "GetStakeLevels",
        data: requestFilter,
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                RenderStakeLevels(rs);
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

function RenderStakeLevels(data) {
    var bodyRows = [];
    for (var i = 0; i < data.StakeLevels.length; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportTypeId', Value: data.StakeLevels[i].SportTypeId });
        row.Columns.push({ Name: 'SportType', Value: data.StakeLevels[i].SportType });
        row.Columns.push({ Name: 'BetTypeId', Value: data.StakeLevels[i].BetTypeId });
        row.Columns.push({ Name: 'BetType', Value: data.StakeLevels[i].BetType });
        row.Columns.push({ Name: 'Site', Value: data.StakeLevels[i].Site });
        row.Columns.push({ Name: 'CreditCurrency', Value: data.StakeLevels[i].CreditCurrency });
        row.Columns.push({ Name: 'DepositCurrency', Value: data.StakeLevels[i].DepositCurrency });
        row.Columns.push({ Name: 'NewMinBet', Value: data.StakeLevels[i].NewMinBet });
        row.Columns.push({ Name: 'NewMaxBet', Value: data.StakeLevels[i].NewMaxBet });
        row.Columns.push({ Name: 'MinBet', Value: data.StakeLevels[i].MinBet });
        row.Columns.push({ Name: 'MaxBet', Value: data.StakeLevels[i].MaxBet });
        row.Columns.push({ Name: 'GameGroupId', Value: data.StakeLevels[i].GameGroupId });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Sport Type ID", Width: 100, Name: "SportTypeId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Sport Type", Width: 150, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bet Type ID", Width: 100, Name: "BetTypeId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bet Type", Width: 150, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Credit Currency", Width: 110, Name: "CreditCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Deposit Currency", Width: 110, Name: "DepositCurrency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "New Min Bet", Width: 100, Name: "NewMinBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "New Max Bet", Width: 100, Name: "NewMaxBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Current Min Bet", Width: 100, Name: "MinBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Current Max Bet", Width: 100, Name: "MaxBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Game Group ID", Width: 100, Name: "GameGroupId", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false,
        showPager: true,
        customPager: true,
        pagerOption: {
            itemsPerPage: data.PageSize,
            currentPage: data.PageIndex,
            itemsPerPageArray: [50, 100, 150, 200],
            pagerSelectCallBack: reRenderAfterPaged,
            totalItem: data.TotalRows
        }

    };

    $('#gridStakeLevel').fanexGrid(gridOptions);
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    GetStakeLevels(itemsPerPage, currentPageIndex);
}

function ExportExcel() {
    UpdateFilterValue();

    if (ValidateRequest()) {
        GetExcelFile();
    }
}

function GetExcelFile() {
    parent.BlockUI();
    $("#errMsg").html("");
    $.ajax({
        type: "POST",
        url: "ExportExcel",
        data: requestFilter,
        xhr: function () {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 2) {
                    if (xhr.status === 200) {
                        xhr.responseType = "blob";
                    } else {
                        xhr.responseType = "text";
                    }
                }
            };
            return xhr;
        },
        success: function (data) {
            try { parent.jQuery.unblockUI(); } catch (e) { }
            //Convert the Byte Data to BLOB object.
            var blob = new Blob([data], { type: "application/octetstream" });

            var url = window.URL || window.webkitURL;
            let link = url.createObjectURL(blob);
            var a = $("<a />");
            a.attr("download", "StakeLevels.xls");
            a.attr("href", link);
            $("body").append(a);
            a[0].click();
            $("body").remove(a);
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsg', "Internal server error.", 1);
        }
    });
}


$(document).ready(function () {
    RenderDcsMatrix();
});