var configSettings = [];
var dcsproducts = [];
var selectedProduct = null;
var selectedSports = [];
var selectedConfig = null;
var isAddConfig = false;
var isNeedRenderBettype = false;
var isLoadCachedFilter = false;
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
        row.Columns.push({ Name: 'Status', TdClass: startTimeClass, Value: RenderStatus(setting.Status) });
        row.Columns.push({ Name: 'Steps', TdClass: startTimeClass, Value: RenderStep(setting, steps) });
        row.Columns.push({ Name: 'StartTime', TdClass: startTimeClass, Value: setting.StartTime });
        row.Columns.push({ Name: 'EndTime', TdClass: startTimeClass, Value: setting.EndTime });
        row.Columns.push({ Name: 'IsTest', TdClass: startTimeClass, Value: setting.IsTest ? 1 : 0 });
        row.Columns.push({ Name: 'Remark', TdClass: startTimeClass + " remark", Value: setting.Remark });
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
            { HeaderText: "Status", Width: 50, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
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
        url: "GetProductSportBettype",
        success: function (rs) {
            if (rs && rs.ErrorCode !== undefined && rs.ErrorCode !== 0) {
                showAlertMessage('errMsg', rs.ErrorMessage, 1);
            } else {
                $("#popupContainer").html(rs);
                $('#popupContainer').dialog({
                    title: 'Add Config',
                    modal: true,
                    width: '1100',
                    height: '600'
                });
            }
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
    GetConfigDetail(configId);
}

function ViewConfig(configId) {
    $("#popupContainer").html("");
    GetViewConfigDetail(configId);
}

function OpenAddConfig() {
    isAddConfig = true;
    selectedConfig = null;
    selectedProduct = null;
    selectedSports = [];
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
        url: "GetConfigDetail?configId=" + configId,
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
            }
        },
        error: function (event, jqXHR, ajaxSettings) {
            showAlertMessage('editErrMsg', "Internal server error.", 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }

            $("#btn-add").addClass("hidden");
            $("#btn-edit").removeClass("hidden");
            RenderEditConfigDetail();
        }
    });
}

function GetViewConfigDetail(configId) {
    $("#errMsg").html("");
    parent.BlockUI();
    $.ajax({
        type: "GET",
        url: "ViewConfigDetail?configId=" + configId,
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
    siteSelect.multiSelect({
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
            $(bettypeSel).select2({ width: "100%"});
        });
        $(this).select2({ width: "100%", disabled: !selectedConfig.ConfigDetail.Settings[position].IsCopy });
    });

    $(".sel-bettype-copy").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        $(this).select2({ width: "100%", disabled: !selectedConfig.ConfigDetail.Settings[position].IsCopy });
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].CopyFromBetType = $(this).val();
        });
    });

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
    $(".input-headline").each(function () {
        $(this).off("change").on("change", function () {
            var usernames = $(this);
            var usernamesValue = $.trim(usernames.val());
            if (usernamesValue.length > 0) {
                if (usernamesValue.indexOf("\n") >= 0) {
                    usernamesValue = usernamesValue.replace(/\n/g, " ");
                }

                if (usernamesValue.indexOf("\r") >= 0) {
                    usernamesValue = usernamesValue.replace(/\r/g, " ");
                }

                if (usernamesValue.indexOf("\t") >= 0) {
                    usernamesValue = usernamesValue.replace(/\t/g, " ");
                }

                usernamesValue = usernamesValue.replace(/\s/g, ",");
                var usernamesArray = usernamesValue.split(",").filter(item => item.length > 0);
                usernames.val(usernamesArray.join(","));
            }

            var el = this;
            setTimeout(function () {
                el.style.cssText = 'height:auto; padding:0';
                el.style.cssText = 'height:' + el.scrollHeight + 'px';
            }, 0);

            var pos = $(this).data("pos");
            var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
            selectedConfig.ConfigDetail.Settings[position].SpecialRequirement = $(this).val();
        });
    });
    $(".input-com-member").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = $(this).val();
        });
    });

    $(".input-com-agent").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = $(this).val();
            RenderCommMember(pos, position);
        });
    });

    $(".input-com-master").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = $(this).val();
            RenderCommAgent(pos, position);
        });
    });

    $(".input-com-super").each(function () {
        var pos = $(this).data("pos");
        var position = selectedConfig.ConfigDetail.Settings.findIndex(x => x.SettingId === pos);
        var comm = selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper;
        $(this).select2({ width: "100%", disabled: !selectedConfig.ConfigDetail.Settings[position].IsDefault });
        $(this).val(parseFloat(comm)).change();
        $(this).change(function () {
            selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper = $(this).val();
            RenderCommMaster(pos, position);
        });
        RenderCommMaster(pos, position);
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

function RenderCommMaster(pos, position) {
    var selDiv = $("#input-com-master-" + pos);
    var options = selectedConfig.ConfigDetail.Settings[position].CommissionOptions;
    selDiv.empty();
    if (options) {
        var currentValue = parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster);
        var maxValue = parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper);
        if (currentValue > maxValue) selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster = selectedConfig.ConfigDetail.Settings[position].DefaultValueSuper;
        options.forEach(function (opt) {
            if (parseFloat(opt.Value) <= maxValue) {
                selDiv.append(new Option(opt.Text, opt.Value));
            }
        });
    }
    selDiv.select2({ width: "100%", disabled: !selectedConfig.ConfigDetail.Settings[position].IsDefault });
    selDiv.val(parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster)).change();
}

function RenderCommAgent(pos, position) {
    var selDiv = $("#input-com-agent-" + pos);
    var options = selectedConfig.ConfigDetail.Settings[position].CommissionOptions;
    selDiv.empty();
    if (options) {
        var currentValue = parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent);
        var maxValue = parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster);
        if (currentValue > maxValue) selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent = selectedConfig.ConfigDetail.Settings[position].DefaultValueMaster;
        options.forEach(function (opt) {
            if (parseFloat(opt.Value) <= maxValue) {
                selDiv.append(new Option(opt.Text, opt.Value));
            }
        });
    }
    selDiv.select2({ width: "100%", disabled: !selectedConfig.ConfigDetail.Settings[position].IsDefault });
    selDiv.val(parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent)).change();
}

function RenderCommMember(pos, position) {
    var selDiv = $("#input-com-member-" + pos);
    var options = selectedConfig.ConfigDetail.Settings[position].CommissionOptions;
    selDiv.empty();
    if (options) {
        var currentValue = parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueMember);
        var maxValue = parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent);
        if (currentValue > maxValue) selectedConfig.ConfigDetail.Settings[position].DefaultValueMember = selectedConfig.ConfigDetail.Settings[position].DefaultValueAgent;
        options.forEach(function (opt) {
            if (parseFloat(opt.Value) <= maxValue) {
                selDiv.append(new Option(opt.Text, opt.Value));
            }
        });
    }
    selDiv.select2({ width: "100%", disabled: !selectedConfig.ConfigDetail.Settings[position].IsDefault });
    selDiv.val(parseFloat(selectedConfig.ConfigDetail.Settings[position].DefaultValueMember)).change();
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

function ValidateBeforeAddUpdate() {
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
    selectedConfig.ConfigDetail.Config.Remark = $("#EditRemark").val();

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
        type: "POST",
        url: "ValidateUpdateConfigSetting",
        data: selectedConfig.ConfigDetail,
        success: function (rs) {
            if (rs.IsWarning) {
                if (confirm("There already exists a configuration that has the same product/sport type/bet type, currency, setting type in the system with status \"Completed\". Are you sure you want to edit this config ?")) {
                    UpdateConfigModel();
                }
            } else {
                if (rs && rs.Errors.length > 0) {
                    ShowAddEditError(rs);
                    $("#popupContainer").scrollTop(0);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                } else {
                    $("#popupContainer").scrollTop(0);
                    GetConfigDetail(selectedConfig.ConfigDetail.Config.ConfigId, true);
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
        type: "POST",
        url: "UpdateConfigSetting",
        dataType: 'json',
        data: selectedConfig.ConfigDetail,
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
        type: "POST",
        url: "ValidateAddConfigSetting",
        dataType: 'json',
        data: selectedConfig.ConfigDetail,
        success: function (rs) {
            if (rs.IsWarning) {
                if (confirm("There already exists a configuration that has the same product/sport type/bet type, currency, setting type in the system with status \"Completed\". Are you sure you want to add this config ?")) {
                    AddConfigModel();
                }
            } else {
                if (rs && rs.Errors.length > 0) {
                    ShowAddEditError(rs);
                    $("#popupContainer").scrollTop(0);
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
        type: "POST",
        url: "AddConfigSetting",
        dataType: 'json',
        data: selectedConfig.ConfigDetail,
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
});