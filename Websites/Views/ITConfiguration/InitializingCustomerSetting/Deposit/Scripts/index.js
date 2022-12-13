var configSettings = [];
var dcsproducts = [];
var selectedProduct = null;
var selectedSports = [];
var selectedConfig = null;
var isAddConfig = false;
var isNeedRenderBettype = false;
var isLoadCachedFilter = false;
let headlineRegex = /^[0-9a-zA-Z\s\\r\\n~.,?:;!|@#$%&*\(\)\{\}+-\\"]+$/;
var betSettingBySettingType = {};
var rootUrl = window.location.origin + prefixUrl + 'InitDepSetting/';
function GetSetting() {
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: rootUrl + "GetSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (rs) {
            configSettings = rs;
            RenderSettings();
        },
        error: function (event, jqXHR, ajaxSettings) {
            console.log("asdasd");
            showAlertMessage('errMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RefreshGrid() {
    hideAlertMessage('errMsg');
    GetSetting();
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
            + ' </br> <a class="info-link" href="javascript:void(0);" onclick="OpenBetTypeDialogInformation(' + pos + ')">View all ' + bettypes.length + ' bet types</a>';
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

function RenderBetSetting(configId) {
    return '<a href="javascript:void(0);" alt = "View" style = "cursor: pointer" onclick = "ViewBetSetting(' + configId + ')" >[View]</a>';
}

function GetStartTimeClass(starttime) {
    if (starttime && !IsFutureTime(starttime)) {
        return "previous-time";
    }

    return "";
}

function IsFutureTime(starttime) {
    var selectDate = new Date(starttime);
    var todayDate = new Date(today);

    return +selectDate > +todayDate;

}

function RenderSettings() {
    var bodyRows = [];
    var steps = configSettings.Steps;
    for (var i = 0; i < configSettings.ConfigSettings.length; i++) {
        var setting = configSettings.ConfigSettings[i];
        var startTimeClass = GetStartTimeClass(setting.StartTime);
        var row = { Columns: [] };
        row.Columns.push({ Name: 'ConfigId', TdClass: startTimeClass, Value: setting.ConfigId });
        row.Columns.push({ Name: 'Product', TdClass: startTimeClass, Value: setting.ProductName });
        row.Columns.push({ Name: 'Sport', TdClass: startTimeClass, Value: RenderSports(i) });
        row.Columns.push({ Name: 'BetType', TdClass: startTimeClass, Value: RenderBettypes(i) });
        row.Columns.push({ Name: 'Site', TdClass: startTimeClass, Value: RenderSite(i) });
        row.Columns.push({ Name: 'Currency', TdClass: startTimeClass, Value: RenderCurrency(i) });
        row.Columns.push({ Name: 'BetSetting', TdClass: startTimeClass, Value: setting.IsExistBetSetting ? RenderBetSetting(setting.ConfigId) : ""});
        row.Columns.push({ Name: 'Status', TdClass: startTimeClass, Value: RenderStatus(setting.Status) });
        row.Columns.push({ Name: 'Steps', TdClass: startTimeClass, Value: RenderStep(setting, steps) });
        row.Columns.push({ Name: 'StartTime', TdClass: startTimeClass, Value: setting.StartTime });
        row.Columns.push({ Name: 'EndTime', TdClass: startTimeClass, Value: setting.EndTime });
        row.Columns.push({ Name: 'IsTest', TdClass: startTimeClass, Value: setting.IsTest ? 1 : 0 });
        row.Columns.push({ Name: 'Remark', TdClass: startTimeClass + " remark", Value: setting.Remark.replaceAll('<', '&lt').replaceAll('>', '&gt') });
        row.Columns.push({ Name: 'Action', TdClass: startTimeClass, Value: RenderAction(setting.Status, setting.ConfigId) });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Config ID", Width: 25, Name: "ConfigId", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Product", Width: 100, Name: "Product", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Sport", Width: 100, Name: "Sport", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Bet Type", Width: 100, Name: "BetType", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Site", Width: 90, Name: "Site", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Currency", Width: 90, Name: "Currency", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Bet Setting", Width: 100, Name: "BetSetting", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Width: 80, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Steps", Width: 250, Name: "Steps", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Start Time (GMT-4)", Width: 65, Name: "StartTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "End Time (GMT-4)", Width: 65, Name: "EndTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Is Test", Width: 40, Name: "IsTest", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Remark", Width: 100, Name: "Remark", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 80, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
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
        url: rootUrl + "GetProductSportBettype",
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                $("#popupContainer").html(rs);
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }

            $('#popupContainer').dialog({
                title: 'Add Config',
                modal: true,
                width: '1100',
                height: '600'
            });
            $("#editErrMsg").html("");
            $("#btn-add").addClass("hidden");
            $("#btn-edit").addClass("hidden");
            $("#filterDiv").removeClass("hidden");
            InitFilter();
        }
    });
}

function InitFilter() {
    isLoadCachedFilter = true;

    InitFilterBehavior();
    $("#EditProductSelect").select2({ width: "135px" });
    if (selectedConfig && selectedConfig.ConfigDetail) {
        $("#EditProductSelect").val(selectedConfig.ConfigDetail.Config.ProductId).trigger("change");
    } else {
        RenderSportType();
    }
    UpdateSelectSport();
    RenderBetTypeOptions();

    isLoadCachedFilter = false;
} 

function InitFilterBehavior() {
    var productFilter = $("#EditProductSelect");
    productFilter.change(function () {
        var selectedProductId = $("#EditProductSelect").val();
        selectedProduct = dcsproducts.find(element => element.ProductId.toString() === selectedProductId);
        RenderSportType();
        UpdateSelectSport();
        RenderBetTypeOptions();
    });
}

function EditConfig(configId) {
    isAddConfig = false;
    $("#errMsg").html("");
    $("#popupContainer").html("");
    betSettingBySettingType = {};
    GetConfigDetail(configId);
}

function ViewConfig(configId) {
    $("#popupContainer").html("");
    GetViewConfigDetail(configId);
}

function ViewBetSetting(configId) {
    $("#popupContainer").html("");
    GetViewBetSetting(configId);
}

function OpenAddConfig() {
    isAddConfig = true;
    selectedConfig = null;
    selectedProduct = null;
    selectedSports = [];
    betSettingBySettingType = {};
    $("#errMsg").html("");
    $("#popupContainer").html("");
    RenderDcsMatrix();
}

function GetConfigDetail(configId, isDialogOpened = false) {
    selectedConfig = null;
    $("#editErrMsg").html("");
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: rootUrl + "GetConfigDetail?configId=" + configId,
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                $("#popupContainer").html(rs);
                if (!isDialogOpened) {
                    $('#popupContainer').dialog({
                        title: 'Config ' + configId,
                        modal: true,
                        width: '1100',
                        height: '600'
                    });
                } else {
                    showAlertMessage('editErrMsg', "Updated Successfully.", 0);
                }

                $("#btn-add").addClass("hidden");
                $("#btn-edit").removeClass("hidden");
                RenderEditConfigDetail();
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

function GetViewConfigDetail(configId) {
    $("#errMsg").html("");
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: rootUrl + "ViewConfigDetail?configId=" + configId,
        contentType: "application/json; charset=utf-8",
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                $("#popupContainer").html(rs);
                $('#popupContainer').dialog({
                    title: 'Config ' + configId,
                    modal: true,
                    width: '1100',
                    height: '600'
                });

                RenderSelectView();
                RenderViewConfigDetail();
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

function GetViewBetSetting(configId) {
    $("#errMsg").html("");
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: rootUrl + "ViewBetSetting?configId=" + configId,
        contentType: "application/json; charset=utf-8",
        success: function (rs) {
            $("#popupContainer").html();
            BuildBetSettingGrid(rs);
            $('#popupContainer').dialog({
                title: 'Bet Setting - File Upload',
                modal: true,
                width: 'auto',
                height: 400
            });
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function BuildBetSettingGrid(betSettings){
    var bodyRows = [];
    var $table = $('<table>').addClass('freegrid');
    var $th = $('<tr>').append(
        $('<th>').text('Sport Type'),
        $('<th>').text('Bet Type'),
        $('<th>').text('Currency Code'),
        $('<th>').text('Currency Name'),
        $('<th>').text('Min Bet'),
        $('<th>').text('Max Bet'),
        $('<th>').text('Max per Match'),
        $('<th>').text('Max per Number'),
        $('<th>').text('Max Payout per Match')
    );
    $table.append($th);
    if (betSettings.length === 0) {
        $table.append($('<tr>').append(
            $('<td>').attr('colspan', 3).text('No data information.')
        ));
    }

    for (var i = 0; i < betSettings.length; i++) {
        var setting = betSettings[i];
        var $tr = $('<tr>').append(
            $('<td>').text(setting.SportTypeId),
            $('<td>').text(setting.BetTypeId),
            $('<td>').text(setting.CurrencyCode),
            $('<td>').text(setting.CurrencyName),
            $('<td>').text(setting.MinBet),
            $('<td>').text(setting.MaxBet),
            $('<td>').text(setting.MaxPerMatch == null ? "" : setting.MaxPerMatch),
            $('<td>').text(setting.MaxPerNumber == null ? "" : setting.MaxPerNumber),
            $('<td>').text(setting.MaxPayoutPerMatch == null ? "" : setting.MaxPayoutPerMatch)
        );

        $table.append($tr);
    }

    $('#popupContainer').append($table);
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

    SetupCalendar("EditStartTime");
    $("#EditStartTime").val(selectedConfig.ConfigDetail.Config.StartTime).trigger("change");
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
    var option = {
        minWidth: 140,
        maxHeight: 250,
        shadow: true,
        showHeader: true,
        isCheckAll: selectedConfig && selectedConfig.ConfigDetail.Config.SiteIdList.indexOf(0) >= 0,
        showFooter: false,
        showFilter: true,
        disabled: !enableInput,
        checkAllText: "[0] All Sites",
        listData: multiselectData,
        noItemText: "Select Sites"
    };

    siteSelect.multiSelect(option);
    $('#EditSiteSelect').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').on('keypress', function (e) {
        if (e.which == 13) {
            var filterValue = $('#EditSiteSelect').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val().toLowerCase();
            var selectedSites = filterValue.split(/(?:,| )+/);
            option.listData.forEach(function (cur) {
                var curOption = cur.Text.split(" ")[1].toLowerCase();
                $('#' + cur.Id).prop('checked', selectedSites.indexOf(curOption) >= 0);
            });

            $('#EditSiteSelect').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val('').keyup();
            $('#EditSiteSelect').trigger('change');
        }
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
    var option = {
        minWidth: 140,
        maxHeight: 250,
        shadow: true,
        showHeader: true,
        isCheckAll: selectedConfig && selectedConfig.ConfigDetail.Config.CurrencyIdList.indexOf(0) >= 0,
        showFooter: false,
        showFilter: true,
        disabled: !enableInput,
        checkAllText: "[0] All currencies",
        listData: multiselectData,
        noItemText: "Select Currencies"
    };

    currencySelect.multiSelect(option);
    $('#EditCurrencySelect').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').on('keypress', function (e) {
        if (e.which == 13) {
            var filterValue = $('#EditCurrencySelect').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val().toLowerCase();
            var selectedCurrency = filterValue.split(/(?:,| )+/);
            option.listData.forEach(function (cur) {
                var curOption = cur.Text.split(" ")[1].toLowerCase();
                $('#' + cur.Id).prop('checked', selectedCurrency.indexOf(curOption) >= 0);
            });

            $('#EditCurrencySelect').next('div.ui-multiselect-options').find('input.ui-multiselect-widget-header-filter').val('').keyup();
            $('#EditCurrencySelect').trigger('change');
        }
    });
}

function RenderSelectView() {
    $(".max-winloss").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        if (!isNaN(selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper)) {
            $(this).val(parseInt(selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper).toString());
        }
    });
    $(".sel-sport-copy").each(function () {
        $(this).select2({ width: "100%" });
    });
    $(".sel-bettype-copy").each(function () {
        $(this).select2({ width: "100%" });
    });
    $(".sel-position-taking").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        var pt = selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper;
        $(this).val(parseFloat(pt)).change();
        $(this).select2({ width: "100%" });
    });

    $(".input-com-super").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        var comm = selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper;
        $(this).val(parseFloat(comm)).change();
        $(this).select2({ width: "100%", disabled: true });
    });

    $(".input-com-master").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        var comm = selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster;
        $(this).val(parseFloat(comm)).change();
        $(this).select2({ width: "100%", disabled: true });
    });
    $(".input-com-agent").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        var comm = selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent;
        $(this).val(parseFloat(comm)).change();
        $(this).select2({ width: "100%", disabled: true });
    });
    $(".input-com-member").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        var comm = selectedConfig.ConfigDetail.Settings[position].DefaultValueMember;
        $(this).val(parseFloat(comm)).change();
        $(this).select2({ width: "100%", disabled: true });
    });

}

function RenderEventEdit() {

    SetupCalendar("EditStartTime");
    $("#EditStartTime").val(selectedConfig.ConfigDetail.Config.StartTime).trigger("change");
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
                $(".input-status-default-" + pos).each(function () {
                    $(this).removeClass("input-status-disable");
                });
            } else {
                $(".input-status-default-" + pos).each(function () {
                    $(this).addClass("input-status-disable");
                });
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
                $(".input-status-default-" + pos).each(function () {
                    $(this).addClass("input-status-disable");
                });
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

    $(".sel-position-taking").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        $(this).select2({ width: "100%", disabled: !selectedConfig.ConfigDetail.Settings[position].IsDefault });
        $(this).val(parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper)).trigger("change");
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper = $(this).val();
        });
    });

    $(".input-file-bet-setting").each(function () {
        var settingType = $(this).data("setting-type");
        var errBetSettingDiv = 'err-bet-setting-' + settingType;
        $(this).prop("disabled", true);
        $(".group-bet-setting-" + settingType).each(function () {
            if ($(this).is(":checked")) {
                $("#uploadFile-" + settingType).prop("disabled", false);
            }
        });

        $(this).change(function () {
            hideAlertMessage(errBetSettingDiv);
            var fileData = $("#uploadFile-" + settingType)[0].files[0];
            var data = new FormData();
            data.append("uploadFile", fileData);
            if (!ValidateUploadFile("#uploadFile-" + settingType, errBetSettingDiv)) {
                return;
            }
            var filePath = $("#uploadFile-" + settingType).val();
            var fileName = filePath.substring(filePath.lastIndexOf('\\') + 1, filePath.length);
            $("#uploadFile-" + settingType).val("");
            $.ajax({
                type: "POST",
                url: rootUrl + "ParseBetSetting?settingType=" + settingType,
                data: data,
                processData: false,
                enctype: "multipart/form-data",
                contentType: false,
                success: function (result) {
                    if (result.ErrorMessage !== "") {
                        showAlertMessage(errBetSettingDiv, result.ErrorMessage, 1);
                    } else {
                        showAlertMessage(errBetSettingDiv, fileName + " is selected.", 0);
                        betSettingBySettingType[settingType] = result.BetSettings;
                    }
                }
            });
        });
    });
    $(".ckb-default-bet-setting").each(function () {
        $(this).change(function () {
            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            var isDefault = $(this).prop('checked');
            selectedConfig.ConfigDetail.Settings[position].IsDefault = isDefault;
            var settingType = selectedConfig.ConfigDetail.Settings[position].SettingType;
            var errBetSettingDiv = 'err-bet-setting-' + settingType;
            if (isDefault) {
                $("#uploadFile-" + settingType).prop("disabled", false);
            } else {
                var isDisabled = true;
                $(".group-bet-setting-" + settingType).each(function () {
                    if ($(this).is(":checked")) {
                        isDisabled = false;
                    }
                });
                $("#uploadFile-" + settingType).prop("disabled", isDisabled);
                if (isDisabled) {
                    betSettingBySettingType[settingType] = [];
                    hideAlertMessage(errBetSettingDiv);
                }
                
            }

        });
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
        EnableStatusDiv(statusDiv);
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper = "0";
        DisableStatusDiv(statusDiv);
        var targetMaster = statusDiv.data("master");
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = "0";
        DisableStatusDiv($("#" + targetMaster));
        var targetAgent = statusDiv.data("agent");
        selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = "0";
        DisableStatusDiv($("#" + targetAgent));
        var targetMember = statusDiv.data("member");
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = "0";
        DisableStatusDiv($("#" + targetMember));
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
        if (selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper !== "" && parseInt(selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper) === 0) {
            return;
        }
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = "1";
        EnableStatusDiv(statusDiv);
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = "0";
        DisableStatusDiv(statusDiv);
        var targetAgent = statusDiv.data("agent");
        selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = "0";
        DisableStatusDiv($("#" + targetAgent));
        var targetMember = statusDiv.data("member");
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = "0";
        DisableStatusDiv($("#" + targetMember));
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
        if (selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster !== "" && parseInt(selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster) === 0) {
            return;
        }
        selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = "1";
        EnableStatusDiv(statusDiv);
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = "0";
        DisableStatusDiv(statusDiv);
        var targetMember = statusDiv.data("member");
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = "0";
        DisableStatusDiv($("#" + targetMember));
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
        if (selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent !== "" && parseInt(selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent) === 0) {
            return;
        }
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = "1";
        EnableStatusDiv(statusDiv);
    } else {
        selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = "0";
        DisableStatusDiv(statusDiv);
    }
}

function EnableStatusDiv(targetDiv) {
    targetDiv.html("Enable");
    targetDiv.removeClass("status-disable");
    targetDiv.addClass("status-enable");
}

function DisableStatusDiv(targetDiv) {
    targetDiv.html("Disable");
    targetDiv.addClass("status-disable");
    targetDiv.removeClass("status-enable");
}

function SetupCalendar(divId) {
    $('#' + divId).datetimepicker({
        format: 'm/d/Y H:i:00',
        formatDate: 'm/d/Y',
        formatTime: 'H:i',
        defaultDate: minDate,
        defaultTime: minTime,
        minDate: minDate,
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

    $("#EditSportSelect").next('div.ui-multiselect-options').on('close', function () { return false; });
    $("#EditSportSelect").next('div.ui-multiselect-options').off('close');
    $("#EditSportSelect").next('div.ui-multiselect-options').on('close',
        function () {
            if (isNeedRenderBettype) RenderBetTypeOptions();
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
}

function RenderBetTypeOptions() {
    isNeedRenderBettype = false;
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
                        Checked: isLoadCachedFilter && selectedConfig && selectedConfig.ConfigDetail.Config.BetTypeIdList &&selectedConfig.ConfigDetail.Config.BetTypeIdList.indexOf(bt.BettypeId) >= 0,
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

    if (request.ProductId === "") {
        showAlertMessage('editErrMsg', "Product cannot be null.", 1);
        return;
    }

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
        url: rootUrl + "GenerateConfigSetting",
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

function ValidateBeforeAddUpdate() {
    var settingTypesKeys = Object.getOwnPropertyNames(betSettingBySettingType);
    selectedConfig.ConfigDetail.BetSettings = [];
    settingTypesKeys.forEach(function (key) {
        selectedConfig.ConfigDetail.BetSettings.push(...betSettingBySettingType[key]);
    });
    
    selectedConfig.ConfigDetail.Config.Remark = $("#EditRemark").val().encodeHtml();
    if (selectedConfig.ConfigDetail.Config.Remark && selectedConfig.ConfigDetail.Config.Remark !== "" && !headlineRegex.test(selectedConfig.ConfigDetail.Config.Remark)) {
        showAlertMessage('editErrMsg', "Invalid Remark.", 1);

        return false;
    }
    selectedConfig.ConfigDetail.Config.SiteIdList = $("#EditSiteSelect").multiSelect("getCheckedItems").map(Number);
    if (selectedConfig.ConfigDetail.Config.SiteIdList.length === 0) {
        showAlertMessage('editErrMsg', "Site cannot be null.", 1);

        return false;
    }
    selectedConfig.ConfigDetail.Config.CurrencyIdList = $("#EditCurrencySelect").multiSelect("getCheckedItems").map(Number);
    if (selectedConfig.ConfigDetail.Config.CurrencyIdList.length === 0) {
        showAlertMessage('editErrMsg', "Currency cannot be null.", 1);

        return false;
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

        return false;
    }
    selectedConfig.ConfigDetail.Config.IsTest = $("#ckbEditIsTest").prop('checked');

    return true;
}

function ValidateUpdateConfigModel() {
    $("#editErrMsg").html("");
    if (!ValidateBeforeAddUpdate()) {
        $("#popupContainer").scrollTop(0);
        return;
    }

    parent.BlockUI();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        url: rootUrl + "ValidateUpdateConfigSetting",
        data: JSON.stringify(selectedConfig.ConfigDetail),
        success: function (rs) {
            if (rs.IsWarning) {
                if (confirm("There already exists a configuration that has the same product/sport type/bet type, currency, setting type in the system with status \"Completed\". Are you sure you want to edit this config ?")) {
                    UpdateConfigModel();
                }
            } else {
                if (rs && rs.Errors.length > 0) {
                    ShowAddEditError(rs.Errors);
                    $("#popupContainer").scrollTop(0);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                } else {
                    $("#popupContainer").scrollTop(0);
                    GetConfigDetail(selectedConfig.ConfigDetail.Config.ConfigId, true);
                    GetSetting();
                }
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            $("#popupContainer").scrollTop(0);
            showAlertMessage('editErrMsg', "Internal server error.", 1);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function UpdateConfigModel() {
    $("#editErrMsg").html("");
    parent.BlockUI();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        url: rootUrl + "UpdateConfigSetting",
        dataType: 'json',
        data: JSON.stringify(selectedConfig.ConfigDetail),
        success: function (rs) {
            if (rs && rs.length > 0) {
                ShowAddEditError(rs);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            } else {
                GetConfigDetail(selectedConfig.ConfigDetail.Config.ConfigId, true);
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        },
        complete: function () {
            $("#popupContainer").scrollTop(0);
            GetSetting();
        }
    });
}

function ValidateNewConfigModel() {
    $("#editErrMsg").html("");
    if (!ValidateBeforeAddUpdate()) {
        $("#popupContainer").scrollTop(0);
        return;
    }

    parent.BlockUI();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        url: rootUrl + "ValidateAddConfigSetting",
        dataType: 'json',
        data: JSON.stringify(selectedConfig.ConfigDetail),
        success: function (rs) {
            if (rs.IsWarning) {
                if (confirm("There already exists a configuration that has the same product/sport type/bet type, currency, setting type in the system with status \"Completed\". Are you sure you want to add this config ?")) {
                    AddConfigModel();
                }
            } else {
                if (rs.Errors && rs.Errors.length > 0) {
                    ShowAddEditError(rs.Errors);
                    $("#popupContainer").scrollTop(0);
                } else if (rs.BetSettingErrorMessage !== null && rs.BetSettingErrorMessage !== "") {
                    showAlertMessage('err-bet-setting-' + rs.SettingType, rs.BetSettingErrorMessage, 1);
                } else {
                    $('#popupContainer').html("");
                    $('#popupContainer').dialog('close');
                    showAlertMessage('errMsg', "Added Config Successfully.", 0);
                    GetSetting();
                }
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            $("#popupContainer").scrollTop(0);
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
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        url: rootUrl + "AddConfigSetting",
        dataType: 'json',
        data: JSON.stringify(selectedConfig.ConfigDetail),
        success: function (rs) {
            if (rs && rs.length > 0) {
                ShowAddEditError(rs);
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

function ShowAddEditError(result) {
    if (result[0].SettingType > 0) {
        var betSettingGroup = {};
        result.forEach(function (err) {
            if (betSettingGroup[err.SettingType] == null || betSettingGroup[err.SettingType] == undefined) {
                betSettingGroup[err.SettingType] = "";
            }

            betSettingGroup[err.SettingType] = betSettingGroup[err.SettingType] + err.ErrorMsg + "</br>";
        });

        var settingTypesKeys = Object.getOwnPropertyNames(betSettingGroup);
        settingTypesKeys.forEach(function (key) {
            showAlertMessage('err-bet-setting-' + key, betSettingGroup[key], 1);
        });

        return;
    }

    var message = "";
    result.forEach(function (err) {
        message = message + err.ErrorMsg + "</br>";
    });

    showAlertMessage('editErrMsg', message, 1);
}

function AbortConfig(configId) {
    $("#errMsg").html("");
    if (confirm("Are you sure you want to abort this config?")) {
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: rootUrl + "AbortSetting",
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
            url: rootUrl + "DeleteSetting",
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

function ValidateUploadFile(fileId, errDiv) {
    var filename = $(fileId).val();
    if (filename === '' || filename == undefined) {
        showAlertMessage(errDiv, 'Please select file to upload.', 1);
        return false;
    }

    var extfilename = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
    if (extfilename !== 'xls' && extfilename !== 'xlsx') {
        showAlertMessage(errDiv, 'Sorry, You choose invalid file extension. Please choose an Excel file (.xls or .xlsx)', 1);
        return false;
    }

    if (GetFileSize(fileId) > 1024000) {
        showAlertMessage(errDiv, "Your uploaded file's size can't be larger than 1 MB", 1);
        return false;
    }
    
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

String.prototype.encodeHtml = function () {
    var str = this;
    return $('<div/>').text(str).html();
};

$(document).ready(function () {
    GetSetting();
});