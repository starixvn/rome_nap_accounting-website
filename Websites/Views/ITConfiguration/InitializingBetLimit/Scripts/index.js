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
var isRenderedCurrencyMatrix = false;
var products = [];

function SwitchTab(type) {
    isNeedRenderBettype = false;
    isNeedRenderExcelBettype = false;
    $('#tabs').find('li').removeClass('current');
    $('div[id^="tab_content_"]').hide();
    $('#tab_' + type).addClass('current');
    $('#tab_content_' + type).show();
    if (type === "product_currency_matrix" && isRenderedCurrencyMatrix === false) {
        RenderAllCurrency();
    }
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
                products = rs.Products;
                RenderProduct();
                RenderProductMatrix();
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

function RenderProduct() {
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

function RenderProductMatrix() {
    var productFilter = $("#FilterProductMatrix");
    productFilter.empty();
    var multiselectData = [];
    products.forEach(function (item) {
        multiselectData.push({
            Id: 'productMatrix' + item.Id,
            Value: item.Id,
            Text: item.Name,
            Checked: true,
            Enable: true,
            CssClass: "",
            Items: []
        });
    });

    productFilter.multiSelect({
        minWidth: 200,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: true,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "Select Product"
    });
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
                    },
                    onCheckAll: function () {
                        sportFilter.val("All");
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
                $("#gridBetLimit").hide();
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

function RenderAllCurrency() {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "GET",
        url: "GetAllCurrency",
        success: function (rs) {
            if (typeof rs === 'string' && rs.includes("DOCTYPE")) {
                showAlertMessage('errMatrixMsg', "You are not authorized on this function", 1);
                $("#gridProductCurrencyMatrix").hide();
            } else {
                isRenderedCurrencyMatrix = true;
                RenderCurrency(rs, "Currency");
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMatrixMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
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
        GetBetLimits(50, 0);
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

function GetBetLimits(pageSize, pageIndex) {
    try { parent.BlockUI(); } catch (e) { }
    HideError();
    requestFilter.PageSize = pageSize;
    requestFilter.PageIndex = pageIndex;
    $.ajax({
        type: "POST",
        url: "GetBetLimits",
        data: requestFilter,
        success: function (rs) {
            if (typeof rs === 'string' && rs.includes("DOCTYPE")) {
                showAlertMessage('errMsg', "You are not authorized on this function", 1);
                $("#filterDiv").hide();
                $("#gridBetLimit").hide();
            } else {
                RenderBetLimits(rs);
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

function RenderBetLimits(data) {
    var bodyRows = [];
    for (var i = 0; i < data.BetLimits.length; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportTypeId', Value: data.BetLimits[i].SportTypeId });
        row.Columns.push({ Name: 'SportType', Value: data.BetLimits[i].SportType });
        row.Columns.push({ Name: 'BetTypeId', Value: data.BetLimits[i].BetTypeId });
        row.Columns.push({ Name: 'BetType', Value: data.BetLimits[i].BetType });
        row.Columns.push({ Name: 'Site', Value: data.BetLimits[i].Site });
        row.Columns.push({ Name: 'CreditCurrency', Value: data.BetLimits[i].CreditCurrency });
        row.Columns.push({ Name: 'DepositCurrency', Value: data.BetLimits[i].DepositCurrency });
        row.Columns.push({ Name: 'MinBet', Value: data.BetLimits[i].MinBet === null ? "" : data.BetLimits[i].MinBet });
        row.Columns.push({ Name: 'MaxBet', Value: data.BetLimits[i].MaxBet === null ? "" : data.BetLimits[i].MaxBet });
        row.Columns.push({ Name: 'MaxPerMatch', Value: data.BetLimits[i].MaxPerMatch === null ? "" : data.BetLimits[i].MaxPerMatch });
        row.Columns.push({ Name: 'MaxPerBall', Value: data.BetLimits[i].MaxPerBall === null ? "" : data.BetLimits[i].MaxPerBall });
        row.Columns.push({ Name: 'MaxGameStake', Value: data.BetLimits[i].MaxGameStake === null ? "" : data.BetLimits[i].MaxGameStake });
        row.Columns.push({ Name: 'GameGroupId', Value: data.BetLimits[i].GameGroupId === null ? "" : data.BetLimits[i].GameGroupId });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: "Sport Type ID", Name: "SportTypeId", MergeRowNumber: 2 },
                    { HeaderText: "Sport Type", Name: "SportType", MergeRowNumber: 2 },
                    { HeaderText: "Bet Type ID", Name: "BetTypeId", MergeRowNumber: 2 },
                    { HeaderText: "Bet Type", Name: "BetType", MergeRowNumber: 2 },
                    { HeaderText: "Site", Name: "Site",  MergeRowNumber: 2 },
                    { HeaderText: "Credit Currency", Name: "CreditCurrency", MergeRowNumber: 2 },
                    { HeaderText: "Deposit Currency", Name: "DepositCurrency",  MergeRowNumber: 2 },
                    { HeaderText: "Current Value", Name: "MinBet", MergeColNumber: 5 },
                    { HeaderText: "Game Group ID", Name: "GameGroupId", MergeRowNumber: 2 }
                ]
            }
        ],
        columns: [
            { HeaderText: "Sport Type ID", Width: 100, Name: "SportTypeId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Sport Type", Width: 150, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bet Type ID", Width: 100, Name: "BetTypeId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bet Type", Width: 150, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Credit Currency", Width: 110, Name: "CreditCurrency", HeaderAlign: "center", CellAlign: "center", CssClass: "word-break-auto" },
            { HeaderText: "Deposit Currency", Width: 110, Name: "DepositCurrency", HeaderAlign: "center", CellAlign: "center", CssClass: "word-break-auto" },
            { HeaderText: "Min Bet", Width: 100, Name: "MinBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Bet", Width: 100, Name: "MaxBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Per Match", Width: 100, Name: "MaxPerMatch", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Per Ball", Width: 100, Name: "MaxPerBall", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Game Stake", Width: 100, Name: "MaxGameStake", HeaderAlign: "center", CellAlign: "center" },
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

    $('#gridBetLimit').fanexGrid(gridOptions);
    $('#gridBetLimit').show();
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    GetBetLimits(itemsPerPage, currentPageIndex);
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
        var isGlobalValue = $("#ckbGlobalValue").prop("checked");
        $.ajax({
            url: 'CheckBetLimits?isGlobalValue=' + isGlobalValue,
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
                    $("#gridExcelBetLimit").hide();
                } else {
                    if (result.Warning && result.Warning.ErrorCode !== 0) {
                        showAlertMessage('errWarningMsgExcel', result.Warning.ErrorMessage, 2);
                    }
                    if (result.Error && result.Error.ErrorCode !== 0) {
                        showAlertMessage('errMsgExcel', result.Error.ErrorMessage, 1);
                    } else {
                        $("#btnInsertExcel").prop('disabled', false);
                        excelContent = result.BetLimits;
                        RenderExcelGrid(result.BetLimits);
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

function RenderExcelGrid(BetLimits) {
    var bodyRows = [];
    for (var i = 0; i < BetLimits.length; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportTypeId', Value: BetLimits[i].SportTypeId });
        row.Columns.push({ Name: 'SportType', Value: BetLimits[i].SportType });
        row.Columns.push({ Name: 'BetTypeId', Value: BetLimits[i].BetTypeId });
        row.Columns.push({ Name: 'BetType', Value: BetLimits[i].BetType });
        row.Columns.push({ Name: 'Site', Value: BetLimits[i].Site });
        row.Columns.push({ Name: 'CreditCurrency', Value: BetLimits[i].CreditCurrency });
        row.Columns.push({ Name: 'DepositCurrency', Value: BetLimits[i].DepositCurrency });
        row.Columns.push({ Name: 'MinBet', Value: BetLimits[i].MinBet === null ? "" : BetLimits[i].MinBet });
        row.Columns.push({ Name: 'MaxBet', Value: BetLimits[i].MaxBet === null ? "" : BetLimits[i].MaxBet});
        row.Columns.push({ Name: 'MaxPerMatch', Value: BetLimits[i].MaxPerMatch === null ? "" : BetLimits[i].MaxPerMatch});
        row.Columns.push({ Name: 'MaxPerBall', Value: BetLimits[i].MaxPerBall === null ? "" : BetLimits[i].MaxPerBall });
        row.Columns.push({ Name: 'MaxGameStake', Value: BetLimits[i].MaxGameStake === null ? "" : BetLimits[i].MaxGameStake });
        row.Columns.push({ Name: 'NewMinBet', Value: BetLimits[i].NewMinBet === null ? "" : BetLimits[i].NewMinBet});
        row.Columns.push({ Name: 'NewMaxBet', Value: BetLimits[i].NewMaxBet === null ? "" : BetLimits[i].NewMaxBet});
        row.Columns.push({ Name: 'NewMaxPerMatch', Value: BetLimits[i].NewMaxPerMatch === null ? "" : BetLimits[i].NewMaxPerMatch });
        row.Columns.push({ Name: 'NewMaxPerBall', Value: BetLimits[i].NewMaxPerBall === null ? "" : BetLimits[i].NewMaxPerBall });
        row.Columns.push({ Name: 'NewMaxGameStake', Value: BetLimits[i].NewMaxGameStake === null ? "" : BetLimits[i].NewMaxGameStake });
        row.Columns.push({ Name: 'GameGroupId', Value: BetLimits[i].GameGroupId === null ? "" : BetLimits[i].GameGroupId });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        customizeHeader: true,
        groupHeaderRows: [
            {
                Columns: [
                    { HeaderText: "Sport Type ID", Name: "SportTypeId", MergeRowNumber: 2 },
                    { HeaderText: "Sport Type", Name: "SportType", MergeRowNumber: 2 },
                    { HeaderText: "Bet Type ID", Name: "BetTypeId", MergeRowNumber: 2 },
                    { HeaderText: "Bet Type", Name: "BetType", MergeRowNumber: 2 },
                    { HeaderText: "Site", Name: "Site", MergeRowNumber: 2 },
                    { HeaderText: "Credit Currency", Name: "CreditCurrency", MergeRowNumber: 2 },
                    { HeaderText: "Deposit Currency", Name: "DepositCurrency", MergeRowNumber: 2 },
                    { HeaderText: "Current Value", Name: "MinBet", MergeColNumber: 5 },
                    { HeaderText: "New Value", Name: "NewMinBet", MergeColNumber: 5 },
                    { HeaderText: "Game Group ID", Name: "GameGroupId", MergeRowNumber: 2 }
                ]
            }
        ],
        columns: [
            { HeaderText: "Sport Type ID", Width: 100, Name: "SportTypeId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Sport Type", Width: 150, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bet Type ID", Width: 100, Name: "BetTypeId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Bet Type", Width: 150, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Credit Currency", Width: 110, Name: "CreditCurrency", HeaderAlign: "center", CellAlign: "center", TdClass: "word-break-auto" },
            { HeaderText: "Deposit Currency", Width: 110, Name: "DepositCurrency", HeaderAlign: "center", CellAlign: "center", TdClass: "word-break-auto" },
            { HeaderText: "Min Bet", Width: 100, Name: "MinBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Bet", Width: 100, Name: "MaxBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Per Match", Width: 100, Name: "MaxPerMatch", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Per Ball", Width: 100, Name: "MaxPerBall", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Game Stake", Width: 100, Name: "MaxGameStake", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Min Bet", Width: 100, Name: "NewMinBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Bet", Width: 100, Name: "NewMaxBet", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Per Match", Width: 100, Name: "NewMaxPerMatch", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Max Per Ball", Width: 100, Name: "NewMaxPerBall", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "New Max Game Stake", Width: 100, Name: "NewMaxGameStake", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Game Group ID", Width: 100, Name: "GameGroupId", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        showDistinctRow: false
    };

    $('#gridExcelBetLimit').fanexGrid(gridOptions);
    $('#gridExcelBetLimit').show();
}

function InsertExcel() {
    HideError();
    try { parent.BlockUI(); } catch (e) { }
    var request = {
        BetLimits: excelContent
    };
    $.ajax({
        type: "POST",
        url: "InsertBetLimits",
        data: request,
        success: function (rs) {
            if (rs && rs.ErrorCode) {
                showAlertMessage('errMsgExcel', rs.ErrorMessage, 1);
                $("#gridExcelBetLimit").hide();
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
    $("#errMatrixMsg").html("");
}

function GetProductCurrencyMatrix() {
    var request = {
        ProductIds: $("#FilterProductMatrix").multiSelect("getCheckedItems").join(),
        Currencies: $("#FilterCurrency").multiSelect("getCheckedItems").join()
    };
    if (request.ProductIds === "") {
        showAlertMessage('errMatrixMsg', "Product cannot be Null.", 1);
        return;
    }

    if (request.Currencies === "") {
        showAlertMessage('errMatrixMsg', "Currency cannot be Null.", 1);
        return;
    }


    var isCheckAllProduct = $("#FilterProductMatrix").multiSelect("getUncheckItems").length === 0;
    var isCheckAllCurrency = $("#FilterCurrency").multiSelect("getUncheckItems").length === 0;
    if (isCheckAllProduct) {
        request.ProductIds = "0";
    }

    if (isCheckAllCurrency) {
        request.Currencies = "0";
    }

    try { parent.BlockUI(); } catch (e) { }
    HideError();
    $.ajax({
        type: "POST",
        url: "GetProductCurrencyMatrix",
        data: request,
        success: function (rs) {
            if (typeof rs === 'string' && rs.includes("DOCTYPE")) {
                showAlertMessage('errMatrixMsg', "You are not authorized on this function", 1);
                $("#gridProductCurrencyMatrix").hide();
            } else {
                RenderProductCurrencyMatrix(rs);
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMatrixMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderProductCurrencyMatrix(data) {
    var bodyRows = [];
    for (var productPos = 0; productPos < data.length; productPos++) {
        var productRowSpan = 0;
        var productNamePos = bodyRows.length;
        var sportList = data[productPos];
        for (var sportPos = 0; sportPos < sportList.length; sportPos++) {
            var itemList = sportList[sportPos];
            productRowSpan += itemList.length;
            var row = { Columns: [] };
            if (sportPos === 0) {
                row.Columns.push({ Name: 'Product', Value: itemList[0].ProductName, MergeRowNumber: 1 });
            }
            row.Columns.push({ Name: 'SportTypeId', Value: itemList[0].SportType, MergeRowNumber: itemList.length });
            row.Columns.push({ Name: 'SportType', Value: itemList[0].SportName, MergeRowNumber: itemList.length });
            row.Columns.push({ Name: 'Credit', Value: itemList[0].CurrencyCredit });
            row.Columns.push({ Name: 'Deposit', Value: itemList[0].CurrencyDeposit });
            bodyRows.push(row);
            for (var i = 1; i < itemList.length; i++) {
                row = { Columns: [] };
                row.Columns.push({ Name: 'Credit', Value: itemList[i].CurrencyCredit });
                row.Columns.push({ Name: 'Deposit', Value: itemList[i].CurrencyDeposit });
                bodyRows.push(row);
            }
        }

        bodyRows[productNamePos].Columns[0].MergeRowNumber = productRowSpan;
    }


    var gridOptions =
    {
        columns: [
            { HeaderText: "Product", Width: 100, Name: "Product", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Sport Type ID", Width: 150, Name: "SportTypeId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Sport Type", Width: 100, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Credit Currency", Width: 150, Name: "Credit", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Deposit Currency", Width: 100, Name: "Deposit", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        showDistinctRow: false
    };

    $("#gridProductCurrencyMatrix").fanexGrid(gridOptions);
    $('#gridProductCurrencyMatrix').show();
}


function ExportMatrixExcel() {
    UpdateFilterValue();

    $("#exportMatrixHref")[0].click();
}

$(document).ready(function () {
    InitSubmitExcelFile();
    RenderDcsMatrix();

    var urlBetLimit = window.rootUrl + "InitBetLimit/ExportExcel";
    $("#exportHref").attr("href", urlBetLimit);
    var urlProductMatrix = window.rootUrl + "InitBetLimit/ExportProductCurrencyExcel";
    $("#exportMatrixHref").attr("href", urlProductMatrix);
});