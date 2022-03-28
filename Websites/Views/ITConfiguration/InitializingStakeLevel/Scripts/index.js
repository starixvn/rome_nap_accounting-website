var isNeedRenderBettype = false;
var isNeedRenderExcelBettype = false;
var requestFilter = {
    ProductID: '',
    SportTypes: '',
    SportTypesIds: [],
    BetTypes: '',
    BetTypeIds: [],
    DepositCurrency: '0',
    DepositCurrencyIds: [],
    CreditCurrency: '0',
    CreditCurrencyIds: []
};

var form;
var excelContent = [];
var creditCurrency = [];
var depositCurrency = [];

function SwitchTab(type) {
    isNeedRenderBettype = false;
    isNeedRenderExcelBettype = false;
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#tab_' + type).addClass('current');
    $('#tab_content_' + type).show();
}

function RenderDcsMatrix() {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "GET",
        url: "GetFilterData",
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                RenderProduct(rs.Products);
                RenderProduct(rs.Products, 'Excel');
                creditCurrency = rs.CreditCurrencies;
                depositCurrency = rs.DepositCurrencies;
                RenderCreditCurrency();
                RenderDepositCurrency();
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

function RenderProduct(products) {
    var productFilter = $("#FilterProduct");
    productFilter.empty();
    products.forEach(function (product, _) {
        var option = "<option value=\"" + product.Id + "\">" + product.Name + "</option>";
        productFilter.append(option);
    });
    productFilter.select2({ width: "200px" });
    productFilter.change(function () {
        requestFilter.ProductID = $("#FilterProduct").val();
        RenderSportType();
    });

    productFilter.trigger("change");
}

function RenderSportType() {
    try { parent.BlockUI(); } catch(e) { }
    var selectedProductId = $("#FilterProduct").val();
    $.ajax({
        type: "GET",
        url: "GetSportTypes?productId=" + selectedProductId,
        success: function (rs, textStatus, jqXHR) {
            if (typeof rs === 'string' && rs.includes("DOCTYPE")) {
                showAlertMessage('errMsg', "You are not authorized on this function", 1);
                $("#filterDiv").hide();
                $("#gridStakeLevel").hide();
            } else {
                var sportFilter = $("#FilterSportType");
                sportFilter.empty();
                sportFilter.removeAttr('disabled');

                var multiselectData = [];
                rs.forEach(function (item) {
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
                        if (isNeedRenderBettype) {
                            isNeedRenderBettype = false;
                            RenderBetType();
                        }
                    });
                RenderBetType();
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsg' + type, "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderBetType() {
    try { parent.BlockUI(); } catch (e) { }
    var selectedSportIds = $("#FilterSportType").multiSelect("getCheckedItems").join();
    var request = {
        ProductId: requestFilter.ProductID,
        SportTypes: selectedSportIds
    };

    $.ajax({
        type: "POST",
        url: "GetBetTypes",
        data: request,
        success: function (rs) {
            if (typeof rs === 'string' && rs.includes("DOCTYPE")) {
                showAlertMessage('errMsg', "You are not authorized on this function", 1);
                $("#filterDiv").hide();
                $("#gridStakeLevel").hide();
            } else {
                var bettypeFilter = $("#FilterBetType");
                bettypeFilter.empty();
                bettypeFilter.removeAttr('disabled');
                var multiselectData = [];
                rs.forEach(function (item) {
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
            showAlertMessage('errMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderCreditCurrency() {
    RenderCurrency(creditCurrency, "Credit");
}

function RenderDepositCurrency() {
    RenderCurrency(depositCurrency, "Deposit");
}

function RenderCurrency(currencies, type) {
    var currencyFilter = $("#Filter" + type);
    currencyFilter.empty();
    currencyFilter.removeAttr('disabled');
    var multiselectData = [];
    currencies.forEach(function (currency) {
        multiselectData.push({
            Id: type + currency.Id,
            Value: currency.Id,
            Text: currency.Name,
            Checked: true,
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
        isCheckAll: true,
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
    var selectedSportIds = $("#FilterSportType").multiSelect("getCheckedItems");
    var selectedBetTypeIds = $("#FilterBetType").multiSelect("getCheckedItems");
    requestFilter = {
        ProductID: $("#FilterProduct").val(),
        SportTypes: selectedSportIds.join(),
        SportTypesIds: selectedSportIds,
        BetTypes: selectedBetTypeIds.join(),
        BetTypeIds: selectedBetTypeIds,
        DepositCurrency: '0',
        CreditCurrency: '0'
    };
    requestFilter.DepositCurrencyIds = $("#FilterDeposit").multiSelect("getCheckedItems");
    requestFilter.CreditCurrencyIds = $("#FilterCredit").multiSelect("getCheckedItems");

    var isCheckAllDepositCurrency = $("#FilterDeposit").multiSelect("getUncheckItems").length === 0;
    var isCheckAllCreditCurrency = $("#FilterCredit").multiSelect("getUncheckItems").length === 0;
    if (!isCheckAllDepositCurrency) {
        requestFilter.DepositCurrency = requestFilter.DepositCurrencyIds.join();
    }

    if (!isCheckAllCreditCurrency) {
        requestFilter.CreditCurrency = requestFilter.CreditCurrencyIds.join();
    }
}

function ValidateRequest() {
    HideError();
    if (requestFilter.SportTypes === null || requestFilter.SportTypes.length === 0) {
        showAlertMessage("errMsg", "Sport Type cannot be null.", 1);
        return false;
    }

    if (requestFilter.BetTypes === null || requestFilter.BetTypes.length === 0) {
        showAlertMessage("errMsg", "Bet Type cannot be null.", 1);
        return false;
    }

    return true;
}

function GetStakeLevels(pageSize, pageIndex) {
    try { parent.BlockUI(); } catch (e) { }
    HideError();
    requestFilter.PageSize = pageSize;
    requestFilter.PageIndex = pageIndex;
    $.ajax({
        type: "POST",
        url: "GetStakeLevels",
        data: requestFilter,
        success: function (rs) {
            if (typeof rs === 'string' && rs.includes("DOCTYPE")) {
                showAlertMessage('errMsg', "You are not authorized on this function", 1);
                $("#filterDiv").hide();
                $("#gridStakeLevel").hide();
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
        row.Columns.push({ Name: 'NewMinBet', Value: data.StakeLevels[i].NewMinBet === 0 ? "" : data.StakeLevels[i].NewMinBet });
        row.Columns.push({ Name: 'NewMaxBet', Value: data.StakeLevels[i].NewMaxBet === 0 ? "" : data.StakeLevels[i].NewMaxBet });
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
    $('#gridStakeLevel').show();
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
    $("#exportHref")[0].click();
}

function ValidateUploadFile() {
    var filename = $('#uploadFile').val();
    if (filename === '' || filename == undefined) {
        showAlertMessage('errMsgExcel', 'Please select file to upload.', 1);
        return false;
    }

    var extfilename = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
    if (extfilename !== 'xls' && extfilename !== 'xlsx') {
        showAlertMessage('errMsgExcel', 'Sorry, You choose invalid file extension. Please choose an Excel file (.xls or .xlsx)', 1);
        return false;
    }

    if (GetFileSize('#uploadFile') > 1024000) {
        showAlertMessage('errMsgExcel', "Your uploaded file's size can't be larger than 1 MB", 1);
        return false;
    }

    HideError();
    return true;
}

function GetFileSize(eName) {
    if ($.browser.msie) {
        return 1;//IE is not support File API, so no way for get file size.
    }
    else {
        return parseFloat($(eName).get(0).files[0].size);
    }
}

function ValidateSetting() {
    if (!ValidateUploadFile()) {
        return;
    }

    try { parent.BlockUI(); } catch (e) { }
    form.submit();
}

function InitSubmitExcelFile() {
    form = $('#mainForm');
    form.submit(function (e) {
        e.preventDefault();
        var formData = new FormData(form[0]);
        $.ajax({
            url: 'CheckStakeLevels',
            type: 'POST',
            datatype: 'json',
            contentType: false,
            processData: false,
            enctype: 'multipart/form-data',
            data: formData,
            success: function (result) {
                if (result && result.ErrorCode) {
                    showAlertMessage('errMsgExcel', result.ErrorMessage, 1);
                    $("#insert-setting").hide();
                    $("#gridExcelStakeLevel").hide();
                } else {
                    if (result.Warning && result.Warning.ErrorCode !== 0) {
                        showAlertMessage('errWarningMsgExcel', result.Warning.ErrorMessage, 2);
                    }
                    if (result.Error && result.Error.ErrorCode !== 0) {
                        showAlertMessage('errMsgExcel', result.Error.ErrorMessage, 1);
                    } else {
                        $("#btnInsertExcel").prop('disabled', false);
                        excelContent = result.StakeLevels;
                        RenderExcelGrid(result.StakeLevels);
                    }
                }
            },
            error: function (xhr) {
                showAlertMessage('errMsgExcel', 'Internal Server Error.', 1);
            },
            complete: function () {
                $('#mainForm')[0].reset();
                parent.jQuery.unblockUI();
            }
        });
    });
}

function RenderExcelGrid(stakeLevels) {
    var bodyRows = [];
    for (var i = 0; i < stakeLevels.length; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportTypeId', Value: stakeLevels[i].SportTypeId });
        row.Columns.push({ Name: 'SportType', Value: stakeLevels[i].SportType });
        row.Columns.push({ Name: 'BetTypeId', Value: stakeLevels[i].BetTypeId });
        row.Columns.push({ Name: 'BetType', Value: stakeLevels[i].BetType });
        row.Columns.push({ Name: 'Site', Value: stakeLevels[i].Site });
        row.Columns.push({ Name: 'CreditCurrency', Value: stakeLevels[i].CreditCurrency });
        row.Columns.push({ Name: 'DepositCurrency', Value: stakeLevels[i].DepositCurrency });
        row.Columns.push({ Name: 'NewMinBet', Value: stakeLevels[i].NewMinBet });
        row.Columns.push({ Name: 'NewMaxBet', Value: stakeLevels[i].NewMaxBet });
        row.Columns.push({ Name: 'MinBet', Value: stakeLevels[i].MinBet });
        row.Columns.push({ Name: 'MaxBet', Value: stakeLevels[i].MaxBet });
        row.Columns.push({ Name: 'GameGroupId', Value: stakeLevels[i].GameGroupId });
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
        showDistinctRow: false
    };

    $('#gridExcelStakeLevel').fanexGrid(gridOptions);
    $('#gridExcelStakeLevel').show();
}

function InsertExcel() {
    HideError();
    try { parent.BlockUI(); } catch (e) { }
    var request = {
        StakeLevels: excelContent
    };
    $.ajax({
        type: "POST",
        url: "InsertStakeLevels",
        data: request,
        success: function (rs) {
            if (rs && rs.ErrorCode) {
                showAlertMessage('errMsgExcel', result.ErrorMessage, 1);
                $("#gridExcelStakeLevel").hide();
            } else {
                if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                    showAlertMessage('errMsgExcel', rs.ErrorMessage, 1);
                } else {
                    showAlertMessage('errMsgExcel', 'Imported successfully.', 0);
                }
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsgExcel', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function HideError() {
    $("#errMsg").html("");
    $("#errMsgExcel").html("");
    $("#errWarningMsgExcel").html("");
}


$(document).ready(function () {
    InitSubmitExcelFile();
    RenderDcsMatrix();

    var url = window.rootUrl + "InitStakeLevel/ExportExcel";
    $("#exportHref").attr("href", url);
});