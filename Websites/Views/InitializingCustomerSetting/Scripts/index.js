var configSettings = [];
var dcsproducts = [];
var selectedProduct = null;
var selectedSports = [];
var selectedConfig = null;
function GetSetting() {
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "GetSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (rs) {
            configSettings = rs;
            RenderSettings();
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderSports(pos) {
    var sports = configSettings.ConfigSettings[pos].SportTypeList;
    if (sports.length > 3) {
        return sports[0] + ' </br>' + sports[1] + ' </br>' + sports[2]
            + ' </br> <a class="info-link" href="#" onclick="OpenSportsDialogInformation(' + pos + ')">View all ' + sports.length + ' sports</a>';
    } else {
        return sports.join('</br>');
    }
}

function OpenSportsDialogInformation(pos) {
    var extraSports = configSettings.ConfigSettings[pos].SportTypeList.join('</br>');
    OpenDialogInformation("Sports", extraSports);
}

function RenderBettypes(pos) {
    var bettypes = configSettings.ConfigSettings[pos].BetTypeList;
    if (bettypes.length > 3) {
        return bettypes[0] + ' </br>' + bettypes[1] + ' </br>' + bettypes[2]
            + ' </br> <a class="info-link" href="#" onclick="OpenBetTypeDialogInformation(' + pos + ')">View all ' + bettypes.length + ' currencies</a>';
    } else {
        return bettypes.join('</br>');
    }
}

function OpenBetTypeDialogInformation(pos) {
    var extraBetType = configSettings.ConfigSettings[pos].BetTypeList.join('</br>');
    OpenDialogInformation("BetTypes", extraBetType);
}

function RenderSite(pos) {
    var sites = configSettings.ConfigSettings[pos].SiteList;
    if (sites.length > 3) {
        return sites[0] + ' </br>' + sites[1] + ' </br>' + sites[2]
            + ' </br> <a class="info-link" href="#" onclick="OpenSiteDialogInformation(' + pos + ')">View all ' + sites.length + ' sites</a>';
    } else {
        return sites.join('</br>');
    }
}

function OpenSiteDialogInformation(pos){
    var extraSites = configSettings.ConfigSettings[pos].SiteList.join('</br>');
    OpenDialogInformation("Sites", extraSites);
}

function RenderCurrency(pos) {
    var currencies = configSettings.ConfigSettings[pos].CurrencyList;
    if (currencies.length > 3) {
        return currencies[0] + ' </br>' + currencies[1] + ' </br>' + currencies[2]
            + ' </br> <a class="info-link" href="#" onclick="OpenCurrencyDialogInformation(' + pos + ')">View all ' + currencies.length +' currencies</a>';
    } else {
        return currencies.join('</br>');
    }
}

function OpenCurrencyDialogInformation(pos) {
    var extraCurrency = configSettings.ConfigSettings[pos].CurrencyList.join('</br>');
    OpenDialogInformation("Currencies", extraCurrency);
}

function OpenDialogInformation(title, divhtml) {
    $('#information').html(divhtml);
    $('#detail-information').dialog({
        title: title,
        modal: true,
        resizable: false,
        width: '250',
        height: 'auto'
    });
}

function RenderStep(setting, steps) {
    return "";
}

function RenderStatus(status) {
    if (status === 0) {
        return '<span class="status_planned">Planned</span>';
    }

    if (status === 1) {
        return '<span class="status_running">Running</span>';
    }

    if (status === 2) {
        return '<span class="status_completed">Completed</span>';
    }

    if (status === 3) {
        return '<span class="status_aborted">Aborted</span>';
    }
}

function RenderAction(status, configId) {
    if (status === 0) {
        return '<a href="#" alt = "Edit" style = "cursor: pointer" onclick = "EditConfig(' + configId + ')" >[Edit]</a><a href="#" alt = "Delete" style = "cursor: pointer" onclick = "DeleteConfig(' + configId + ')" >[Delete]</a>';
    }

    if (status === 1) {
        return '<a href="#" alt = "Abort" style = "cursor: pointer" onclick = "AbortConfig(' + configId + ')" >[Abort]</a>';
    }

    return '<a href="#" alt = "View" style = "cursor: pointer" onclick = "ViewConfig(' + configId + ')" >[View]</a>';
}

function RenderSettings() {
    var bodyRows = [];
    var steps = configSettings.Steps;
    for (var i = 0; i < configSettings.ConfigSettings.length; i++) {
        var setting = configSettings.ConfigSettings[i];
        var row = { Columns: [] };
        row.Columns.push({ Name: 'ConfigId', TdClass: null, Value: setting.ConfigId });
        row.Columns.push({ Name: 'Product', TdClass: null, Value: setting.ProductName });
        row.Columns.push({ Name: 'Sport', TdClass: null, Value: RenderSports(i) });
        row.Columns.push({ Name: 'BetType', TdClass: null, Value: RenderBettypes(i) });
        row.Columns.push({ Name: 'Site', TdClass: null, Value: RenderSite(i) });
        row.Columns.push({ Name: 'Currency', TdClass: null, Value: RenderCurrency(i) });
        row.Columns.push({ Name: 'Status', TdClass: null, Value: RenderStatus(setting.Status) });
        row.Columns.push({ Name: 'Steps', TdClass: null, Value: RenderStep(setting, steps) });
        row.Columns.push({ Name: 'StartTime', TdClass: null, Value: setting.StartTime });
        row.Columns.push({ Name: 'EndTime', TdClass: null, Value: setting.EndTime });
        row.Columns.push({ Name: 'IsTest', TdClass: null, Value: setting.IsTest? 1 : 0 });
        row.Columns.push({ Name: 'Remark', TdClass: null, Value: setting.Remark });
        row.Columns.push({ Name: 'Action', TdClass: null, Value: RenderAction(setting.Status, setting.ConfigId)});
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Config ID", Width: 25, Name: "ConfigId", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Product", Width: 100, Name: "Product", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Sport", Width: 100, Name: "Sport", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Bet Type", Width: 100, Name: "BetType", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Currency", Width: 150, Name: "Currency", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Status", Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Steps", Width: 250, Name: "Steps", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Start Time(GMT-4)", Width: 150, Name: "StartTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "End Time(GMT-4)", Width: 150, Name: "EndTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Is Test", Width: 50, Name: "IsTest", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Remark", Width: 200, Name: "Remark", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 100, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    $('#gridCustomerSetting').fanexGrid(gridOptions);
}

function RenderDcsMatrix(type, callbackfunc, params) {
    dcsproducts = [];
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "GetProductSportBettype",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (rs) {
            dcsproducts = rs;
    InitFilter("Edit");
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
            if (callbackfunc) {
                callbackfunc(params);
            }
        }
    });
}

function InitFilter(type) {
    var productFilter = $("#" + type + "ProductSelect");
    if (dcsproducts) {
        productFilter.empty();
        productFilter.append(new Option("Please select", ""));
        dcsproducts.forEach(function (item, index) {
            productFilter.append(new Option('[' + item.ProductId + '] ' + item.ProductName, item.ProductId));
        });
    }

    var sportFilter = $("#" + type + "SportSelect");
    sportFilter.empty();
    sportFilter.append(new Option("Please select", ""));
    sportFilter.attr('disabled', 'disabled');
    var bettypeFilter = $("#" + type + "BettypeSelect");
    bettypeFilter.empty();
    bettypeFilter.append(new Option("Please select", ""));
    bettypeFilter.attr('disabled', 'disabled');
}

function InitFilterBehavior(type) {
    var productFilter = $("#" + type + "ProductSelect");
    productFilter.change(function () {
        var selectedProductId = $("#" + type + "ProductSelect").val();
        selectedProduct = dcsproducts.find(element => element.ProductId.toString() === selectedProductId);
        RenderSportType(type);
    });
}

function EditConfig(configId) {
    RenderDcsMatrix("Edit", GetConfigDetail, configId);
    $('#detail-information-popup').dialog({
        title: 'Config ' + configId,
        modal: true,
        width: '800',
        height: '700'
    });
}

function GetConfigDetail(configId) {
    selectedConfig = null;
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "GetConfigDetail?configId=" + configId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (rs) {
            selectedConfig = rs;
            $("#grid-information").html(rs.ConfigSettingHtml);
            RenderEditConfigDetail();
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('errMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderEditConfigDetail() {
    $("#EditProductSelect").val(selectedConfig.ConfigDetail.Config.ProductId).trigger("change");
    //selectedConfig.ConfigDetail.Config.BetTypeIdList.forEach(function (item, _) {
    //    $("#bettype" + item).prop('checked', true);
    //});
    //$("#EditBettypeSelect").trigger("change");
    RenderSportType("Edit");
    UpdateSelectSport("Edit");
}

function RenderSportType(type) {
    var sportFilter = $("#" + type + "SportSelect");
    sportFilter.empty();
    sportFilter.removeAttr('disabled');
    var multiselectData = [];
    if (selectedProduct) {
        selectedProduct.Sports.forEach(function (item, index) {
            multiselectData.push({
                Id: 'sport' + item.SportId,
                Value: item.SportId,
                Text: '[' + item.SportId + '] ' + item.SportName,
                Checked: selectedConfig && selectedConfig.ConfigDetail.Config.SportTypeIdList.indexOf(item.SportId) >= 0,
                Enable: true,
                CssClass: "",
                Items: []
            });
        });
    }

    sportFilter.multiSelect({
        minWidth: 160,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: false,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "Select Sport",
        onCheck: function () { UpdateSelectSport(type);}
    });
}

function UpdateSelectSport(type) {
    selectedSports = [];
    var selectedSportIds = $("#" + type + "SportSelect").multiSelect("getCheckedItems");
    if (selectedSportIds) {
        selectedSportIds.forEach(function (item, index) {
            selectedSports.push(selectedProduct.Sports.find(element => element.SportId.toString() === item));
        });
    }
    RenderBetType(type);
}

function RenderBetType(type) {
    var bettypeFilter = $("#" + type + "BettypeSelect");
    bettypeFilter.empty();
    bettypeFilter.removeAttr('disabled');
    var multiselectData = [];
    if (selectedSports) {
        selectedSports.forEach(function (item, index) {
            item.Bettypes.forEach(function (bt) {
                var existbt = multiselectData.find(element => element.Id === 'bettype' + bt.BettypeName);
                if (!existbt) {
                    multiselectData.push({
                        Id: 'bettype' + bt.BettypeId,
                        Value: bt.BettypeId,
                        Text: '[' + bt.BettypeId + '] ' + bt.BettypeName,
                        Checked: selectedConfig && selectedConfig.ConfigDetail.Config.BetTypeIdList.indexOf(bt.BettypeId) >= 0,
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

$(document).ready(function () {
    InitFilterBehavior("Add");
    InitFilterBehavior("Edit");
    GetSetting();
});