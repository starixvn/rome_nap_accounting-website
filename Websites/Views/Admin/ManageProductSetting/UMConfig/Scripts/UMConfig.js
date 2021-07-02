var cachedUMConfigData;
var cachedUMConfigSetting;
var cachedSelectFunctionSetting;
var cachedSelectProductSetting;
var cachedSelectSportSetting;
var cachedSelectSettingType;
var cachedNewUMConfigs;
var cachedCreditSites = [];
var cachedAllSites = [];

function GetInitDataAndSearchAllUMConfig() {
    GetInitData();
}

function GetInitData() {
    $.ajax({
        url: 'GetCreditSites',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            if (result !== null && result.length > 0) {
                cachedCreditSites = result;
            }
        },
        error: function (xhr) {
            alert('Internal Server Error.');
        }
    });

    $.ajax({
        url: 'GetAllSites',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            if (result !== null && result.length > 0) {
                cachedAllSites = result;
            }
        },
        error: function (xhr) {
            alert('Internal Server Error.');
        }
    });

    $.ajax({
        url: 'GetProductSettingByFunction',
        type: 'GET',
        success: function (data) {
            if (Array.isArray(data)) {
                $("#UMConfigAccessDenied").addClass("display-none");
                $("#UMConfigContent").removeClass("display-none");
                cachedUMConfigSetting = data;
                SearchAllUMConfig();
            } else {
                $("#UMConfigAccessDenied").removeClass("display-none");
                $("#UMConfigContent").addClass("display-none");
            }
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function SearchAllUMConfig() {
    parent.BlockUI();
    $.ajax({
        url: 'GetAllUMConfiguration',
        type: 'GET',
        success: function (data) {
            cachedUMConfigData = data;
            RenderUMConfigData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function RenderUMConfigData(data) {
    var bodyRows = [];

    for (i = 0; i < data.length; i++) {
        var umConfig = data[i];
        var row = { Columns: [], CssClass: "" };
        row.Columns.push({ Name: 'FunctionType', Value: "[" + umConfig.FunctionType + "] " + umConfig.FunctionTypeName });
        row.Columns.push({ Name: 'ProductName', Value: "[" + umConfig.ProductId + "] " + umConfig.ProductName });
        row.Columns.push({ Name: 'SportName', Value: "[" + umConfig.SportType + "] " + umConfig.SportName });
        row.Columns.push({ Name: 'SettingType', Value: "[" + umConfig.SettingType + "] " + umConfig.SettingName });
        row.Columns.push({ Name: 'Site', Value: "[" + umConfig.SiteId + "] " + umConfig.Site });
        row.Columns.push({ Name: 'Currency', Value: "[" + umConfig.Currency + "] " + umConfig.CurrencyName });
        row.Columns.push({ Name: 'Username', Value: umConfig.Username, CssClass: "word-break" });
        row.Columns.push({ Name: 'ShowTime', Value: umConfig.ShowTime });
        row.Columns.push({ Name: 'BeginTime', Value: umConfig.BeginTime });
        row.Columns.push({ Name: 'EndTime', Value: umConfig.EndTime });
        row.Columns.push({ Name: 'LockAddingNew', Value: umConfig.LockAddingNew ? 1 : 0 });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditUMConfig(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteUMConfig(' + i + ')" >'
        });
        if (IsOverUMTime(umConfig.EndTime)) {
            row.CssClass = "um-overtime";
        }
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "FunctionType", Width: 100, Name: "FunctionType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductName", Width: 100, Name: "ProductName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SportName", Width: 100, Name: "SportName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SettingType", Width: 100, Name: "SettingType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Currency", Width: 100, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Username", Width: 180, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "ShowTime (GMT+8)", Width: 100, Name: "ShowTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BeginTime (GMT+8)", Width: 100, Name: "BeginTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "EndTime (GMT+8)", Width: 100, Name: "EndTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Lock Adding New", Width: 60, Name: "LockAddingNew", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    $('#gridUMConfigId').fanexGrid(gridOptions);
}

function OpenNewUMConfig() {
    InitAddNewUMConfig();
    cachedNewUMConfigs = [];
    $('#new-umconfig-popup').dialog({
        title: 'Add UMConfig',
        modal: true,
        width: 'auto',
        height: 800
    });
    RenderAllNewUMConfigs();
}

function InitAddNewUMConfig() {
    cachedSelectFunctionSetting = null;
    cachedSelectProductSetting = null;
    cachedSelectSportSetting = null;
    cachedSelectSettingType = null;
    hideAlertMessage("newUMConfigErrMsg");
    var functionTypeSelection = $("#UMFunctionType");
    functionTypeSelection.empty();
    if (cachedUMConfigSetting) {
        var defaultOption = new Option('FunctionType', '', true, true);
        defaultOption.disabled = true;
        functionTypeSelection.append(defaultOption);
        cachedUMConfigSetting.forEach(function (item, index) {
            functionTypeSelection.append(new Option('[' + item.FunctionType + '] ' + item.FunctionTypeName, item.FunctionType));
        });
    }

    RenderSiteSelect();

    var UMProductName = $("#UMProductName");
    UMProductName.empty();
    var productDefaultOption = new Option('ProductName', '', true, true);
    productDefaultOption.disabled = true;
    UMProductName.append(productDefaultOption);

    var UMSportName = $("#UMSportName");
    UMSportName.empty();
    var sportDefaultOption = new Option('SportName', '', true, true);
    sportDefaultOption.disabled = true;
    UMSportName.append(sportDefaultOption);

    var UMSettingType = $("#UMSettingType");
    UMSettingType.empty();
    var settingDefaultOption = new Option('SettingType', '', true, true);
    settingDefaultOption.disabled = true;
    UMSettingType.append(settingDefaultOption);
    $('#UMSite').val("0");
    $('#UMCurrency').val("0");
    $("#UMShowTime").val("");
    $("#UMEndTime").val("");
    $("#UMBeginTime").val("");
    $('#UMLockAddingNew').prop('checked', false);
    $("#UMUsername").val("");
    $('#UMUsername').attr('style', 'width: 100%;');
}

function ValidateUMData(divError, beginTime, endTime, showTime) {
    if (showTime === "") {
        showAlertMessage(divError, "ShowTime is required.", 1);
        return false;
    }

    if (beginTime === "") {
        showAlertMessage(divError, "BeginTime is required.", 1);
        return false;
    }

    if (endTime === "") {
        showAlertMessage(divError, "EndTime is required.", 1);
        return false;
    }

    var beginDate = new Date(beginTime);
    var endDate = new Date(endTime);
    var showDate = new Date(showTime);

    if (showDate >= beginDate) {
        showAlertMessage(divError, "The Show Time must be less than Begin Time.", 1);
        return false;
    }

    if (beginDate >= endDate) {
        showAlertMessage(divError, "The Begin Time must be less than End Time.", 1);
        return false;
    }

    if (IsOverUMTime(endTime)) {
        showAlertMessage(divError, "The End Time must be greater than Current time.", 1);
        return false;
    }

    return true;
}

function AddNewUMConfig() {
    $('#newUMConfigErrMsg').html('');
    var beginTime = $("#UMBeginTime").val();
    var endTime = $("#UMEndTime").val();
    var showTime = $("#UMShowTime").val();

    if ($('#UMFunctionType').val() === '') {
        showAlertMessage('newUMConfigErrMsg', "FunctionType is required.", 1);
        return;
    }

    if (!ValidateUMData('newUMConfigErrMsg', beginTime, endTime, showTime)) {
        return;
    }
    var usernames = $('#UMUsername').val().split(",");
    if (usernames.length > 1000) {
        showAlertMessage('newUMConfigErrMsg', "Usernames count must be less than 1000.", 1);
        return;
    }
    for (var i = 0; i < usernames.length; i++) {
        if (usernames[i].length > 30) {
            showAlertMessage('newUMConfigErrMsg', "Username: " + usernames[i] + " must be equal or less than 30 characters.", 1);
            return;
        }
    }
    AppendNewUMConfig();
    RenderAllNewUMConfigs();
    InitAddNewUMConfig();
}

function AppendNewUMConfig() {
    var usernamesArray = $("#UMUsername").val().replace(/<[^>]+>/g, "").split(',').filter(function (item, pos, self) {
        return self.indexOf(item) === pos;
    });

    var umConfig = {
        FunctionType: $('#UMFunctionType').val(),
        FunctionTypeName: $('#UMFunctionType option:selected').text(),
        ProductId: $('#UMProductName').val(),
        ProductName: $('#UMProductName option:selected').text(),
        SportType: $('#UMSportName').val(),
        SportName: $('#UMSportName option:selected').text(),
        SettingType: $('#UMSettingType').val(),
        SettingName: $('#UMSettingType option:selected').text(),
        BetType: cachedSelectSettingType.BetType,
        IsLive: cachedSelectSettingType.IsLive,
        SiteId: $('#UMSite').val(),
        Site: $('#UMSite').val() === "0" ? "[0] All" : $('#UMSite option:selected').text(),
        Currency: $('#UMCurrency').val(),
        CurrencyName: $('#UMCurrency').val() === "0" ? "[0] All" : $('#UMCurrency option:selected').text(),
        BeginTime: $("#UMBeginTime").val(),
        EndTime: $("#UMEndTime").val(),
        ShowTime: $("#UMShowTime").val(),
        LockAddingNew: $('#UMLockAddingNew').is(":checked"),
        Username: usernamesArray.join(',')
    };

    cachedNewUMConfigs.push(umConfig);
}

function RenderAllNewUMConfigs() {
    var bodyRows = [];

    for (i = 0; i < cachedNewUMConfigs.length; i++) {
        umconfig = cachedNewUMConfigs[i];
        var row = { Columns: [] };
        row.Columns.push({ Name: 'Row', Value: i + 1 });
        row.Columns.push({ Name: 'FunctionType', Value: umconfig.FunctionTypeName });
        row.Columns.push({ Name: 'ProductName', Value: umconfig.ProductName });
        row.Columns.push({ Name: 'SportName', Value: umconfig.SportName });
        row.Columns.push({ Name: 'SettingType', Value: umconfig.SettingName });
        row.Columns.push({ Name: 'Site', Value: umconfig.Site });
        row.Columns.push({ Name: 'Currency', Value: umconfig.CurrencyName });
        row.Columns.push({ Name: 'Username', Value: umconfig.Username === "" ? "All" : umconfig.Username, CssClass: "word-break" });
        row.Columns.push({ Name: 'ShowTime', Value: umconfig.ShowTime });
        row.Columns.push({ Name: 'BeginTime', Value: umconfig.BeginTime });
        row.Columns.push({ Name: 'EndTime', Value: umconfig.EndTime });
        row.Columns.push({ Name: 'LockAddingNew', Value: '<input type="checkbox" disabled="disabled" ' + (umconfig.LockAddingNew ? 'checked' : '') + ' />' });
        row.Columns.push({
            Name: 'Remove',
            Value: '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteNewUMConfig(' + i + ')" >'
        });

        if (IsOverUMTime(umconfig.EndTime)) {
            row.CssClass = "um-overtime";
        }
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "No.", Width: 30, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "FunctionType", Width: 100, Name: "FunctionType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductName", Width: 100, Name: "ProductName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SportName", Width: 100, Name: "SportName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SettingType", Width: 100, Name: "SettingType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Currency", Width: 100, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Username", Width: 200, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "ShowTime (GMT+8)", Width: 100, Name: "ShowTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BeginTime (GMT+8)", Width: 100, Name: "BeginTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "EndTime (GMT+8)", Width: 100, Name: "EndTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Lock Adding New", Width: 60, Name: "LockAddingNew", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Remove", Width: 50, Name: "Remove", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    $('#gridUMNewId').fanexGrid(gridOptions);
}

function DeleteNewUMConfig(index) {
    cachedNewUMConfigs.splice(index, 1);
    RenderAllNewUMConfigs();
}

function SubmitAddNewUMConfig() {
    hideAlertMessage("newUMConfigErrMsg");
    if (cachedNewUMConfigs.length === 0) {
        return;
    }
    parent.BlockUI();
    var divError = "";
    var requests = [];
    for (var i = 0; i < cachedNewUMConfigs.length; i++) {
        var umconfig = cachedNewUMConfigs[i];
        requests.push(CallInsertUMConfig(umconfig));
    }

    $.when.apply(undefined, requests).then(function () {
        var responses = arguments;
        if (cachedNewUMConfigs.length === 1) {
            if (responses[1] === "success") {
                if (responses[0].ErrorCode !== 0) {
                    divError = divError + "<div>" + responses[0].ErrorMessage + "</div>";
                }
            } else {
                divError = divError + "<div>Internal Error.</div>";
            }
        } else {
            for (var j = 0; j < responses.length; j++) {
                response = responses[j];
                if (response[1] === "success") {
                    if (response[0].ErrorCode !== 0) {
                        divError = divError + "<div>Row" + (j + 1) + ": " + response[0].ErrorMessage + "</div>";
                    }
                } else {
                    divError = divError + "<div>Row" + (j + 1) + ": Internal Error.</div>";
                }
            }
        }


        if (divError === "") {
            showAlertMessage("newUMConfigErrMsg", "Insert Successfull.", 0);
        } else {
            var errorString = "<div class='alertboxerr'>" + divError + "</div>";
            $("#newUMConfigErrMsg").html(errorString);
        }
        SearchAllUMConfig();
    });
}

function CallInsertUMConfig(umconfig) {
    return $.ajax({
        url: 'InsertUMConfiguration',
        type: 'POST',
        dataType: 'json',
        data: umconfig
    });
}

function RenderProductSelect() {
    var UMProductName = $("#UMProductName");
    if (cachedSelectFunctionSetting) {
        UMProductName.empty();
        cachedSelectFunctionSetting.Products.forEach(function (item, index) {
            UMProductName.append(new Option('[' + item.ProductId + '] ' + item.ProductName, item.ProductId));
        });
        cachedSelectProductSetting = cachedSelectFunctionSetting.Products[0];
        RenderSportSelect();
    }
}

function RenderSportSelect() {
    var UMSportName = $("#UMSportName");
    if (cachedSelectProductSetting) {
        UMSportName.empty();
        cachedSelectProductSetting.Sports.forEach(function (item, index) {
            UMSportName.append(new Option('[' + item.SportType + '] ' + item.SportName, item.SportType));
        });
        cachedSelectSportSetting = cachedSelectProductSetting.Sports[0];
        RenderSettingTypeSelect();
    }
}

function RenderSettingTypeSelect() {
    var UMSettingType = $("#UMSettingType");
    if (cachedSelectSportSetting) {
        UMSettingType.empty();
        cachedSelectSportSetting.Settings.forEach(function (item, index) {
            UMSettingType.append(new Option('[' + item.SettingType + '] ' + item.SettingName, item.SettingType));
        });
        cachedSelectSettingType = cachedSelectSportSetting.Settings[0];
    }
}

function RenderSiteSelect() {
    var siteSelection = $("#UMSite");
    siteSelection.empty();
    var functionType = $('#UMFunctionType').val();

    if (functionType === "101") {
        cachedAllSites.forEach(function (item, index) {
            siteSelection.append(new Option(item.Text, item.Value));
        });
    } else {
        cachedCreditSites.forEach(function (item, index) {
            siteSelection.append(new Option(item.Text, item.Value));
        });
    }

    siteSelection.select2({
        minimumInputLength: 0,
        width: '200px'
    });
}

function EditUMConfig(index) {
    $('#UpdUMShowTime').datetimepicker('destroy');
    $('#UpdUMEndTime').datetimepicker('destroy');
    $('#UpdUMBeginTime').datetimepicker('destroy');
    $('#updateUMConfigErrMsg').html('');
    $('#UpdUMFunctionType').html("[" + cachedUMConfigData[index].FunctionType + "] " + cachedUMConfigData[index].FunctionTypeName);
    $('#UpdUMUsername').html(cachedUMConfigData[index].Username);
    $('#UpdUMProductName').html(cachedUMConfigData[index].ProductName);
    $('#UpdUMShowTime').val(cachedUMConfigData[index].ShowTime);
    $('#UpdUMSportName').html(cachedUMConfigData[index].SportName);
    $('#UpdUMBeginTime').val(cachedUMConfigData[index].BeginTime);
    $('#UpdUMSettingType').html("[" + cachedUMConfigData[index].SettingType + "] " + cachedUMConfigData[index].SettingName);
    $('#UpdUMEndTime').val(cachedUMConfigData[index].EndTime);
    $('#UpdUMSite').html("[" + cachedUMConfigData[index].SiteId + "] " + cachedUMConfigData[index].Site);
    $('#UpdUMLockAddingNew').prop('checked', cachedUMConfigData[index].LockAddingNew);
    $('#UpdUMCurrency').html("[" + cachedUMConfigData[index].Currency + "] " + cachedUMConfigData[index].CurrencyName);

    $('#btnUpdateUMConfig').off();
    $('#btnUpdateUMConfig').click(function () { return UpdateUMConfig(index); });
    $.ui.dialog.prototype._focusTabbable = function () { };
    $('#edit-umConfig-form').dialog({
        title: 'Edit UMConfig',
        modal: true,
        width: 'auto',
        height: 'auto',
        open: function () {
            SetupUMCalendar('UpdUMShowTime');
            SetupUMCalendar('UpdUMEndTime');
            SetupUMCalendar('UpdUMBeginTime');
        }
    });
}

function DeleteUMConfig(index) {
    var message = "Are you sure you want to delete this row?";
    $.ajax({
        url: 'CheckIsInUMTime?configId=' + cachedUMConfigData[index].ConfigId,
        type: 'GET',
        success: function (result) {
            if (result) {
                message = 'This row is in UM time. Are you sure you want to delete this row?';
            }
        },
        error: function (xhr) {
            alert('Internal Server Error.');
        },
        complete: function () {
            ShowConfirmDelete(message, cachedUMConfigData[index].ConfigId);
        }
    });
};

function ShowConfirmDelete(message, configId) {
    if (confirm(message)) {
        parent.BlockUI();
        $.ajax({
            url: 'DeleteUMConfiguration',
            type: 'POST',
            dataType: 'json',
            data: {
                ConfigId: configId
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    alert('Deleted successfully.');
                    SearchAllUMConfig();
                } else {
                    alert(result.ErrorMessage);
                }
            },
            error: function (xhr) {
                alert('Internal Server Error.');
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    }
}

function DeleteOldData() {
    if (confirm("Are you sure you want to delete old data?")) {
        parent.BlockUI();
        $.ajax({
            url: 'DeleteOldData',
            type: 'POST',
            success: function (result) {
                if (result.ErrorCode === 0) {
                    alert('Deleted successfully.');
                    SearchAllUMConfig();
                } else {
                    alert(result.ErrorMessage);
                }
            },
            error: function (xhr) {
                alert('Internal Server Error.');
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    }
}

function UpdateUMConfig(index) {
    parent.BlockUI();
    umConfig = cachedUMConfigData[index];
    umConfig.BeginTime = $('#UpdUMBeginTime').val();
    umConfig.EndTime = $('#UpdUMEndTime').val();
    umConfig.ShowTime = $('#UpdUMShowTime').val();
    umConfig.LockAddingNew = $('#UpdUMLockAddingNew').is(":checked");
    if (!ValidateUMData('updateUMConfigErrMsg', umConfig.BeginTime, umConfig.EndTime, umConfig.ShowTime)) {
        parent.jQuery.unblockUI();
        return;
    }

    $.ajax({
        url: 'UpdateUMConfiguration',
        type: 'POST',
        dataType: 'json',
        data: umConfig,
        success: function (result) {
            showAlertMessage('updateUMConfigErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-umConfig-form').dialog('close');
                    SearchAllUMConfig();
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateUMConfigErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

$(document).ready(function () {
    $("#UMUsername").on('keydown', function (e) {
        var el = this;
        setTimeout(function () {
            el.style.cssText = 'width:100%; height:auto; padding:0';
            el.style.cssText = 'width:100%; height:' + el.scrollHeight + 'px';
        }, 0);
    });

    $("#UMFunctionType").change(function () {
        var functionType = $('#UMFunctionType').val();
        cachedSelectFunctionSetting = cachedUMConfigSetting.find(element => element.FunctionType === functionType);
        RenderProductSelect();
        RenderSiteSelect();
    });

    $("#UMProductName").change(function () {
        var productId = $('#UMProductName').val();
        cachedSelectProductSetting = cachedSelectFunctionSetting.Products.find(element => element.ProductId === productId);
        RenderSportSelect();
    });

    $("#UMSportName").change(function () {
        var sportType = $('#UMSportName').val();
        cachedSelectSportSetting = cachedSelectProductSetting.Sports.find(element => element.SportType === sportType);
        RenderSettingTypeSelect();
    });

    $("#UMSettingType").change(function () {
        var settingType = $('#UMSettingType').val();
        cachedSelectSettingType = cachedSelectSportSetting.Settings.find(element => element.SettingType === settingType);
    });
    SetupUMCalendar('UMShowTime');
    SetupUMCalendar('UMEndTime');
    SetupUMCalendar('UMBeginTime');

    $("#UMUsername").off("change").on("change", function () {
        var usernames = $("#UMUsername");
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
            el.style.cssText = 'width:100%; height:auto; padding:0';
            el.style.cssText = 'width:100%; height:' + el.scrollHeight + 'px';
        }, 0);
    });
});

function SetupUMCalendar(divId) {
    $('#' + divId).datetimepicker({
        format: 'm/d/Y H:i:00',
        step: 5
    });

    $('#' + divId + '_Trigger').click(function () {
        jQuery('#' + divId).datetimepicker('show');
    });
}

function IsOverUMTime(endTime) {
    var endDate = new Date(endTime + " GMT+0800");
    var toDate = new Date();

    return toDate > endDate;
}