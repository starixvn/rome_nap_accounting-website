var configSettings = [];
var dcsproducts = [];
var selectedProduct = null;
var selectedSports = [];
var selectedConfig = null;
var isAddConfig = false;
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
            + ' </br> <a class="info-link" href="javascript:void(0);" onclick="OpenSportsDialogInformation(' + pos + ')">View all ' + sports.length + ' sports</a>';
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
            + ' </br> <a class="info-link" href="javascript:void(0);" onclick="OpenBetTypeDialogInformation(' + pos + ')">View all ' + bettypes.length + ' currencies</a>';
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
            + ' </br> <a class="info-link" href="javascript:void(0);" onclick="OpenSiteDialogInformation(' + pos + ')">View all ' + sites.length + ' sites</a>';
    } else {
        return sites.join('</br>');
    }
}

function OpenSiteDialogInformation(pos) {
    var extraSites = configSettings.ConfigSettings[pos].SiteList.join('</br>');
    OpenDialogInformation("Sites", extraSites);
}

function RenderCurrency(pos) {
    var currencies = configSettings.ConfigSettings[pos].CurrencyList;
    if (currencies.length > 3) {
        return currencies[0] + ' </br>' + currencies[1] + ' </br>' + currencies[2]
            + ' </br> <a class="info-link" href="javascript:void(0);" onclick="OpenCurrencyDialogInformation(' + pos + ')">View all ' + currencies.length + ' currencies</a>';
    } else {
        return currencies.join('</br>');
    }
}

function OpenCurrencyDialogInformation(pos) {
    var extraCurrency = configSettings.ConfigSettings[pos].CurrencyList.join('</br>');
    OpenDialogInformation("Currencies", extraCurrency);
}

function OpenDialogInformation(title, divhtml) {
    $('#extra-information').html(divhtml);
    $('#extra-information-popup').dialog({
        title: title,
        modal: true,
        resizable: false,
        width: '250',
        height: 'auto'
    });
}

function RenderStep(setting, steps) {
    var result = "";
    if (setting.Status === 2) {
        return result;
    }

    if (setting.ProcessedSteps !== "") {
        var processedSteps = setting.ProcessedSteps.split(',');
        processedSteps.forEach(function (item, _) {
            var step = steps.find(st => st.Id.toString() === item);
            var stepString = "<p class=\"step-complete\">+ " + step.Name + ": <span class=\"complete-string\">Completed<span> <p>";
            result += stepString;
        });
    }

    if (setting.CurrentStep !== "") {
        var rnstep = steps.find(st => st.Id.toString() === setting.CurrentStep);
        var rnstepString = "<p class=\"step-running\">+ " + rnstep.Name + ": <span class=\"running-string\">Running<span> <p>";
        if (setting.Status === 3) {
            rnstepString = "<p class=\"step-abort\">+ " + rnstep.Name + ": <span class=\"abort-string\">Aborted<span> <p>";
        }
        
        result += rnstepString;
    }
    return result;
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
        return '<a href="javascript:void(0);" alt = "Edit" style = "cursor: pointer" onclick = "EditConfig(' + configId + ')" >[Edit]</a><a href="javascript:void(0);" alt = "Delete" style = "cursor: pointer" onclick = "DeleteConfig(' + configId + ')" >[Delete]</a>';
    }

    if (status === 1) {
        return '<a href="javascript:void(0);" alt = "Abort" style = "cursor: pointer" onclick = "AbortConfig(' + configId + ')" >[Abort]</a>';
    }

    return '<a href="javascript:void(0);" alt = "View" style = "cursor: pointer" onclick = "ViewConfig(' + configId + ')" >[View]</a>';
}

function GetStartTimeClass(time) {
    if (time && !IsFutureTime(time)) {
        return "previous-time";
    }

    return null;
}

function IsFutureTime(time) {
    var selectDate = new Date(time);
    var todayDate = new Date(today);

    return +selectDate > +todayDate;

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
        row.Columns.push({ Name: 'StartTime', TdClass: GetStartTimeClass(setting.StartTime), Value: setting.StartTime });
        row.Columns.push({ Name: 'EndTime', TdClass: null, Value: setting.EndTime });
        row.Columns.push({ Name: 'IsTest', TdClass: null, Value: setting.IsTest ? 1 : 0 });
        row.Columns.push({ Name: 'Remark', TdClass: null, Value: setting.Remark });
        row.Columns.push({ Name: 'Action', TdClass: null, Value: RenderAction(setting.Status, setting.ConfigId) });
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

function RenderDcsMatrix() {
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "GetProductSportBettype",
        success: function (rs) {
            $("#popupContainer").html(rs);
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }

            $("#editErrMsg").html("");
            $("#btn-add").addClass("hidden");
            $("#btn-edit").addClass("hidden");
            $("#filterDiv").removeClass("hidden");
            InitFilter();
            UpdateSelectSport();
        }
    });
}

function InitFilter() {
    InitFilterBehavior();
    $("#EditProductSelect").select2();
    if (selectedConfig && selectedConfig.ConfigDetail) {
        $("#EditProductSelect").val(selectedConfig.ConfigDetail.Config.ProductId).trigger("change");
    } else {
        RenderSportType();
    }
    UpdateSelectSport();
} 

function InitFilterBehavior() {
    var productFilter = $("#EditProductSelect");
    productFilter.change(function () {
        var selectedProductId = $("#EditProductSelect").val();
        selectedProduct = dcsproducts.find(element => element.ProductId.toString() === selectedProductId);
        RenderSportType();
        UpdateSelectSport();
    });
}

function EditConfig(configId) {
    isAddConfig = false;
    $("#popupContainer").html("");
    $('#popupContainer').dialog({
        title: 'Config ' + configId,
        modal: true,
        width: '850',
        height: '600'
    });
    GetConfigDetail(configId);
}

function ViewConfig(configId) {
    $("#popupContainer").html("");
    $('#popupContainer').dialog({
        title: 'Config ' + configId,
        modal: true,
        width: '850',
        height: '600'
    });
    GetViewConfigDetail(configId);
}

function OpenAddConfig() {
    isAddConfig = true;
    $("#popupContainer").html("");
    $('#popupContainer').dialog({
        title: 'Add Config',
        modal: true,
        width: '850',
        height: '600'
    });
    RenderDcsMatrix();
}

function GetConfigDetail(configId) {
    selectedConfig = null;
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "GetConfigDetail?configId=" + configId,
        success: function (rs) {
            $("#popupContainer").html(rs);
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }

            $("#editErrMsg").html("");
            $("#btn-add").addClass("hidden");
            $("#btn-edit").removeClass("hidden");
            RenderEditConfigDetail();
        }
    });
}

function GetViewConfigDetail(configId) {
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "ViewConfigDetail?configId=" + configId,
        contentType: "application/json; charset=utf-8",
        success: function (rs) {
            $("#popupContainer").html(rs);
            RenderViewConfigDetail();
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderEditConfigDetail() {
    InitFilter();
    RenderSiteSelection();
    RenderCurrencySelection(true);
    RenderEventEdit();
}

function RenderViewConfigDetail() {
    RenderSiteSelection(false);
    RenderCurrencySelection(false);

    $("#EditStartTime").val(selectedConfig.ConfigDetail.Config.StartTime);
    SetupCalendar("EditStartTime");
    $('#EditRemark').val(selectedConfig.ConfigDetail.Config.Remark);
    $('#ckbEditIsTest').prop('checked', selectedConfig.ConfigDetail.Config.IsTest);
    DisableEditInput(true);
}

function DisableEditInput(isDisabled) {
    $("#EditProductSelect").prop('disabled', isDisabled);

    $(".edit-input").each(function () {
        $(this).prop('disabled', isDisabled);
    });
}

function RenderSiteSelection(enableInput = true) {
    var siteSelect = $("#EditSiteSelect");
    siteSelect.empty();
    var multiselectData = [];
    if (selectedConfig) {
        selectedConfig.Sites.forEach(function (item, index) {
            multiselectData.push({
                Id: 'optsiteEdit' + item.Id,
                Value: item.Id,
                Text: item.Name,
                Checked: selectedConfig && selectedConfig.ConfigDetail.Config.SiteIdList.indexOf(item.Id) >= 0,
                Enable: true,
                CssClass: "",
                Items: []
            });
        });
    }
    isAllSite = selectedConfig && selectedConfig.ConfigDetail.Config.SiteIdList.indexOf(0) >= 0;
    siteSelect.multiSelect({
        minWidth: 120,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: isAllSite,
        showFooter: false,
        showFilter: true,
        disabled: !enableInput,
        checkAllText: "[0] All Sites",
        listData: multiselectData,
        noItemText: "Select Sites",
    });
}

function RenderCurrencySelection(enableInput) {
    var currencySelect = $("#EditCurrencySelect");
    currencySelect.empty();
    var multiselectData = [];
    if (selectedConfig) {
        selectedConfig.Currencies.forEach(function (item, index) {
            multiselectData.push({
                Id: 'optcurrencyEdit' + item.Id,
                Value: item.Id,
                Text: item.Name,
                Checked: selectedConfig && selectedConfig.ConfigDetail.Config.CurrencyIdList.indexOf(item.Id) >= 0,
                Enable: true,
                CssClass: "",
                Items: []
            });
        });
    }

    currencySelect.multiSelect({
        minWidth: 120,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: selectedConfig && selectedConfig.ConfigDetail.Config.CurrencyIdList.indexOf(0) >= 0,
        showFooter: false,
        showFilter: true,
        disabled: !enableInput,
        checkAllText: "[0] All currencies",
        listData: multiselectData,
        noItemText: "Select Currencies"
    });
}

function RenderEventEdit() {
    $(".max-winloss").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        if (!isNaN(selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper)) {
            $(this).val(parseInt(selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper).toString());
        }

        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper = $(this).val();
        });
    });

    $(".sel-sport-copy").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        var settingtype = $(this).data("target-settingtype");
        var targetBettype = $(this).data("target-bettype");
        var editsetting = selectedConfig && selectedConfig.SportBettypeBySettings.find(element => element.SettingName.replace(/ /g, '').toLowerCase() === settingtype);
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].CopyFromSportType = $(this).val();
            var bettypeSel = $("#" + targetBettype);
            bettypeSel.empty();
            var selectSportId = $(this).val();
            var editSport = editsetting && editsetting.Sports.find(element => element.SportId.toString() === selectSportId.toString());
            if (editSport) {
                editSport.Bettypes.forEach(function (bt) {
                    bettypeSel.append(new Option(bt.BettypeName, bt.BettypeId));
                });
                selectedConfig.ConfigDetail.Settings[position].CopyFromBetType = editSport.Bettypes[0].BettypeId;
            }
            bettypeSel.select2();
        });
        $(this).select2();
        $(this).prop("disabled", !selectedConfig.ConfigDetail.Settings[position].IsCopy);
    });

    $(".sel-bettype-copy").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        $(this).select2();
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].CopyFromBetType = $(this).val();
        });
        $(this).prop("disabled", !selectedConfig.ConfigDetail.Settings[position].IsCopy);
    });

    $("#EditStartTime").val(selectedConfig.ConfigDetail.Config.StartTime);
    SetupCalendar("EditStartTime");
    $('#EditRemark').val(selectedConfig.ConfigDetail.Config.Remark);
    $('#ckbEditIsTest').prop('checked', selectedConfig.ConfigDetail.Config.IsTest);

    $(".ckb-default").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            var isDefault = $(this).prop('checked');
            selectedConfig.ConfigDetail.Settings[position].IsDefault = isDefault;
            if (isDefault) {
                $("#ckbCopy" + pos).prop("checked", false);
                $(".input-copy-" + pos).each(function () {
                    $(this).prop("disabled", true);
                });
                selectedConfig.ConfigDetail.Settings[position].IsCopy = false;
            }

            $(".input-default-" + pos).each(function () {
                $(this).prop("disabled", !isDefault);
            });
            
        });
    });

    $(".ckb-copy").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            var isCopy = $(this).prop('checked');
            selectedConfig.ConfigDetail.Settings[position].IsCopy = isCopy;
            if (isCopy) {
                $("#ckbDefault" + pos).prop("checked", false);
                $(".input-default-" + pos).each(function () {
                    $(this).prop("disabled", true);
                });
                selectedConfig.ConfigDetail.Settings[position].IsDefault = false;
            }
            $(".input-copy-" + pos).each(function () {
                $(this).prop("disabled", !isCopy);
            });
        });
    }); 
    $(".ckb-auto-reset").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            if ($(this).prop('checked')) {
                selectedConfig.ConfigDetail.Settings[position].SpecialRequirement = "1";
            } else {
                selectedConfig.ConfigDetail.Settings[position].SpecialRequirement = "0";
            }
            
            $("#ckbAutoReset" + pos).prop("checked", selectedConfig.ConfigDetail.Settings[position].SpecialRequirement === "1");
        });
    }); 
    $(".input-headline").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            selectedConfig.ConfigDetail.Settings[position].SpecialRequirement = $(this).val();
        });
    });
    $(".input-com-super").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper = $(this).val();
        });
    });
    $(".input-com-master").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = $(this).val();
        });
    });
    $(".input-com-agent").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = $(this).val();
        });
    });
    $(".input-com-member").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = $(this).val();
        });
    });

    $(".input-status-super").each(function () {
        $(this).on('click', ChangeSuperStatus);
    });
    $(".input-status-master").each(function () {
        $(this).on('click', ChangeMasterStatus);
    });
    $(".input-status-agent").each(function () {
        $(this).on('click', ChangeAgentStatus);
    });
    $(".input-status-member").each(function () {
        $(this).on('click', ChangeMemberStatus);
    });
}

function ChangeSuperStatus() {
    var statusDiv = $(this);
    var pos = statusDiv.data("pos");
    var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
    if (!selectedConfig.ConfigDetail.Settings[position].IsDefault) {
        return;
    }
    var nextEnable = statusDiv.html() !== "Enable";
    if (nextEnable) {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper = "1";
        statusDiv.html("Enable");
        statusDiv.removeClass("status-disable");
        statusDiv.addClass("status-enable");
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper = "0";
        statusDiv.html("Disable");
        statusDiv.addClass("status-disable");
        statusDiv.removeClass("status-enable");
    }

}

function ChangeMasterStatus() {
    var statusDiv = $(this);
    var pos = statusDiv.data("pos");
    var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
    if (!selectedConfig.ConfigDetail.Settings[position].IsDefault) {
        return;
    }
    var nextEnable = statusDiv.html() !== "Enable";
    if (nextEnable) {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = "1";
        statusDiv.html("Enable");
        statusDiv.removeClass("status-disable");
        statusDiv.addClass("status-enable");
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = "0";
        statusDiv.html("Disable");
        statusDiv.addClass("status-disable");
        statusDiv.removeClass("status-enable");
    }
}

function ChangeAgentStatus() {
    var statusDiv = $(this);
    var pos = statusDiv.data("pos");
    var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
    if (!selectedConfig.ConfigDetail.Settings[position].IsDefault) {
        return;
    }
    var nextEnable = statusDiv.html() !== "Enable";
    if (nextEnable) {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = "1";
        statusDiv.html("Enable");
        statusDiv.removeClass("status-disable");
        statusDiv.addClass("status-enable");
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = "0";
        statusDiv.html("Disable");
        statusDiv.addClass("status-disable");
        statusDiv.removeClass("status-enable");
    }
}

function ChangeMemberStatus() {
    var statusDiv = $(this);
    var pos = statusDiv.data("pos");
    var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
    if (!selectedConfig.ConfigDetail.Settings[position].IsDefault) {
        return;
    }
    var nextEnable = statusDiv.html() !== "Enable";
    if (nextEnable) {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = "1";
        statusDiv.html("Enable");
        statusDiv.removeClass("status-disable");
        statusDiv.addClass("status-enable");
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = "0";
        statusDiv.html("Disable");
        statusDiv.addClass("status-disable");
        statusDiv.removeClass("status-enable");
    }
}

function SetupCalendar(divId) {
    $('#' + divId).datetimepicker({
        format: 'm/d/Y H:i:00 A',
        step: 5
    });

    $('#' + divId + '_Trigger').click(function () {
        jQuery('#' + divId).datetimepicker('show');
    });
}

function RenderSportType() {
    var sportFilter = $("#EditSportSelect");
    sportFilter.empty();
    sportFilter.removeAttr('disabled');
    var multiselectData = [];
    if (selectedProduct) {
        selectedProduct.Sports.forEach(function (item, index) {
            multiselectData.push({
                Id: 'sport' + item.SportId,
                Value: item.SportId,
                Text: item.SportName,
                Checked: selectedConfig && selectedConfig.ConfigDetail.Config.SportTypeIdList && selectedConfig.ConfigDetail.Config.SportTypeIdList.indexOf(item.SportId) >= 0,
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
        onCheck: function () { UpdateSelectSport(); }
    });
}

function UpdateSelectSport() {
    selectedSports = [];
    var selectedSportIds = $("#EditSportSelect").multiSelect("getCheckedItems");
    if (selectedSportIds) {
        selectedSportIds.forEach(function (item, index) {
            selectedSports.push(selectedProduct.Sports.find(element => element.SportId.toString() === item));
        });
    }
    RenderBetType();
}

function RenderBetType() {
    var bettypeFilter = $("#EditBettypeSelect");
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
                        Checked: selectedConfig && selectedConfig.ConfigDetail.Config.BetTypeIdList &&selectedConfig.ConfigDetail.Config.BetTypeIdList.indexOf(bt.BettypeId) >= 0,
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

function RenderNewConfig() {
    $("#editErrMsg").html("");
    var request = {
        ProductId: $("#EditProductSelect").val(),
        SportIdList: $("#EditSportSelect").multiSelect("getCheckedItems").map(Number),
        BettypeIdList: $("#EditBettypeSelect").multiSelect("getCheckedItems").map(Number),
        ConfigId: 0
    };

    if (request.SportIdList.length === 0) {
        showAlertMessage('editErrMsg', "Sport Type cannot be null.", 1);
        return;
    }

    if (request.BettypeIdList.length === 0) {
        showAlertMessage('editErrMsg', "Bet Type cannot be null.", 1);
        return;
    }

    if (!isAddConfig) {
        request.ConfigId = selectedConfig.ConfigDetail.Config.ConfigId;
    }

    parent.BlockUI();
    selectedConfig = null;
    $.ajax({
        type: "POST",
        url: "GenerateConfigSetting",
        data: request,
        success: function (rs) {
            $("#grid-information").html(rs);
            RenderEditConfigDetail();
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }

            if (isAddConfig) {
                $("#btn-add").removeClass("hidden");
            }
        }
    });
    
}

function UpdateConfigModel() {
    $("#editErrMsg").html("");
    selectedConfig.ConfigDetail.Config.SiteIdList = $("#EditSiteSelect").multiSelect("getCheckedItems").map(Number);
    if (selectedConfig.ConfigDetail.Config.SiteIdList.length === 0) {
        showAlertMessage('editErrMsg', "Site cannot be null.", 1);
        return;
    }
    selectedConfig.ConfigDetail.Config.CurrencyIdList = $("#EditCurrencySelect").multiSelect("getCheckedItems").map(Number);
    if (selectedConfig.ConfigDetail.Config.CurrencyIdList.length === 0) {
        showAlertMessage('editErrMsg', "Currency cannot be null.", 1);
        return;
    }

    if (selectedConfig.Sites.length === selectedConfig.ConfigDetail.Config.SiteIdList.length) {
        selectedConfig.ConfigDetail.Config.SiteIdList = [0];
    }
    if (selectedConfig.Currencies.length === selectedConfig.ConfigDetail.Config.CurrencyIdList.length) {
        selectedConfig.ConfigDetail.Config.CurrencyIdList = [0];
    }
    selectedConfig.ConfigDetail.Config.StartTime = $("#EditStartTime").val();
    if (selectedConfig.ConfigDetail.Config.StartTime === "") {
        showAlertMessage('editErrMsg', "Start Time cannot be null.", 1);
        return;
    }
    selectedConfig.ConfigDetail.Config.IsTest = $("#ckbEditIsTest").prop('checked');
    selectedConfig.ConfigDetail.Config.Remark = $("#EditRemark").val();

    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "UpdateConfigSetting",
        dataType: 'json',
        data: selectedConfig.ConfigDetail,
        success: function (rs) {
            if (rs && rs.length > 0) {
                var message = "";
                rs.forEach(function (err) {
                    message = message + err.ErrorMsg + "</br>";
                });

                showAlertMessage('editErrMsg', message, 1);
            } else {
                showAlertMessage('editErrMsg', "Updated Successfully.", 0);
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
            GetSetting();
        }
    });
}

function ValidateNewConfigModel() {
    $("#editErrMsg").html("");
    parent.BlockUI();
    selectedConfig.ConfigDetail.Config.SiteIdList = $("#EditSiteSelect").multiSelect("getCheckedItems").map(Number);
    if (selectedConfig.ConfigDetail.Config.SiteIdList.length === 0) {
        showAlertMessage('editErrMsg', "Site cannot be null.", 1);
        return;
    }
    selectedConfig.ConfigDetail.Config.CurrencyIdList = $("#EditCurrencySelect").multiSelect("getCheckedItems").map(Number);
    if (selectedConfig.ConfigDetail.Config.CurrencyIdList.length === 0) {
        showAlertMessage('editErrMsg', "Currency cannot be null.", 1);
        return;
    }
    if (selectedConfig.Sites.length === selectedConfig.ConfigDetail.Config.SiteIdList.length) {
        selectedConfig.ConfigDetail.Config.SiteIdList = [0];
    }
    if (selectedConfig.Currencies.length === selectedConfig.ConfigDetail.Config.CurrencyIdList.length) {
        selectedConfig.ConfigDetail.Config.CurrencyIdList = [0];
    }
    selectedConfig.ConfigDetail.Config.StartTime = $("#EditStartTime").val();

    if (selectedConfig.ConfigDetail.Config.StartTime === "") {
        showAlertMessage('editErrMsg', "Start Time cannot be null.", 1);
        return;
    }
    selectedConfig.ConfigDetail.Config.IsTest = $("#ckbEditIsTest").prop('checked');
    selectedConfig.ConfigDetail.Config.Remark = $("#EditRemark").val();
    $.ajax({
        type: "POST",
        url: "ValidateAddConfigSetting",
        dataType: 'json',
        data: selectedConfig.ConfigDetail,
        success: function (rs) {
            if (rs.IsWarning) {
                if (confirm("There already exists a configuration that has the same product/sport type/bet type, currency, setting type in the system with status \"Completed\".Are you sure you want to add this config ?")) {
                    AddConfigModel();
                }
            } else {
                if (rs && rs.Errors.length > 0) {
                    var message = "";
                    rs.Errors.forEach(function (err) {
                        message = message + err.ErrorMsg + "</br>";
                    });

                    showAlertMessage('editErrMsg', message, 1);
                } else {
                    $('#popupContainer').html("");
                    $('#popupContainer').dialog('close');
                    showAlertMessage('errMsg', "Added Config Successfully.", 0);
                    GetSetting();
                }
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

function AddConfigModel() {
    $("#editErrMsg").html("");
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "AddConfigSetting",
        dataType: 'json',
        data: selectedConfig.ConfigDetail,
        success: function (rs) {
            if (rs && rs.length > 0) {
                var message = "";
                rs.forEach(function (err) {
                    message = message + err.ErrorMsg + "</br>";
                });

                showAlertMessage('editErrMsg', message, 1);
                GetSetting();
            } else {
                $('#popupContainer').html("");
                $('#popupContainer').dialog('close');
                showAlertMessage('errMsg', "Added Config Successfully.", 0);
                GetSetting();
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

function AbortConfig(configId) {
    $("#errMsg").html("");
    if (confirm("Are you sure you want to abort this config?")) {
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "AbortSetting",
            dataType: 'json',
            data: { ConfigId: configId },
            success: function (rs) {
                if (rs.ErrorCode === 0) {
                    showAlertMessage('errMsg', "Aborted Successfully.", 0);
                    GetSetting();
                } else {
                    showAlertMessage('errMsg', rs.ErrorMessage, 1);
                }
            },
            error: function (error) {
                showAlertMessage('errMsg', "Internal server error.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
}

function DeleteConfig(configId) {
    $("#errMsg").html("");
    if (confirm("Are you sure you want to delete this config?")) {
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "DeleteSetting",
            dataType: 'json',
            data: { ConfigId: configId },
            success: function (rs) {
                if (rs.ErrorCode === 0) {
                    showAlertMessage('errMsg', "Deleted Successfully.", 0);
                    GetSetting();
                } else {
                    showAlertMessage('errMsg', rs.ErrorMessage, 1);
                }
            },
            error: function (error) {
                showAlertMessage('errMsg', "Internal server error.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
}

$(document).ready(function () {
    GetSetting();
    setInterval(GetSetting, 5000);
});