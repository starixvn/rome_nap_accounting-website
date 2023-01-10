var cachedUMConfigData;
var cachedUMConfigSetting;
var cachedSelectFunctionSetting;
var cachedSelectProductSetting;
var cachedSelectSportSetting;
var cachedSelectSettingType;
var cachedNewUMConfigs;
var cachedCreditSites = [];
var cachedAllSites = [];
var cachedMarkets = [];
var checkedRows = [];

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
                var functionTypeSelection = $("#FilterUMFunctionType");
                var defaultOption = new Option('[0] All', '0', true, true);
                functionTypeSelection.append(defaultOption);
                cachedUMConfigSetting.forEach(function (item, index) {
                    functionTypeSelection.append(new Option('[' + item.FunctionType + '] ' + item.FunctionTypeName, item.FunctionType));
                });
                functionTypeSelection.select2({
                    minimumInputLength: 0,
                    width: '200px'
                });
                SetupFilterCalendar("FilterUMBeginTime");
                SetupFilterCalendar("FilterUMEndTime");
            } else {
                $("#UMConfigAccessDenied").removeClass("display-none");
                $("#UMConfigContent").addClass("display-none");
            }
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });

    $.ajax({
        url: 'GetMarkets',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            if (result !== null && result.length > 0) {
                cachedMarkets = result;
            }
        },
        error: function (xhr) {
            alert('Internal Server Error.');
        }
    });
}

function SearchAllUMConfig() {
    parent.BlockUI();
    var filterValue = {
        FunctionType: $("#FilterUMFunctionType").val(),
        BeginTime: $("#FilterUMBeginTime").val(),
        EndTime: $("#FilterUMEndTime").val()
    };

    $.ajax({
        url: 'GetAllUMConfiguration',
        type: 'POST',
        data: filterValue,
        success: function (data) {
            cachedUMConfigData = data;
            checkedRows = [];
            RenderUMConfigData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });
}

function RenderUMConfigData(data) {
    var bodyRows = [];

    for (i = 0; i < data.length; i++) {
        var umConfig = data[i];
        var row = { Columns: [], CssClass: "" };
        row.Columns.push({ Name: 'ConfigId', Value: umConfig.ConfigId });
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
            Value: '<input type="checkbox" class="ckb-um" data-position="'+ i + '"/>'
        });
        if (IsOverUMTime(umConfig.EndTime)) {
            row.CssClass = "um-overtime";
        }
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "ConfigId", Width: 50, Name: "ConfigId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "FunctionType", Width: 100, Name: "FunctionType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductName", Width: 100, Name: "ProductName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SportName", Width: 100, Name: "SportName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SettingType", Width: 100, Name: "SettingType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Currency", Width: 100, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Username", Width: 160, Name: "Username", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "ShowTime (GMT+8)", Width: 100, Name: "ShowTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BeginTime (GMT+8)", Width: 100, Name: "BeginTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "EndTime (GMT+8)", Width: 100, Name: "EndTime", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Lock Adding New", Width: 60, Name: "LockAddingNew", HeaderAlign: "center", CellAlign: "center" },
            {
                HeaderText: 'Action</br><input type="checkbox" id="ckb-all-um"/>&nbsp;'
                    + '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditUMConfig()" >&nbsp;'
                    + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteUMConfig()" >',
                Width: 70, Name: "Action", HeaderAlign: "center", CellAlign: "center"
            }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    $('#gridUMConfigId').fanexGrid(gridOptions);
    checkedRows = [];

    $('#ckb-all-um').change(function () {
        var newValue = this.checked;
        $(".ckb-um").each(function () {
            $(this).prop('checked', newValue);
        });

        if (newValue) {
            checkedRows = cachedUMConfigData;
        } else {
            checkedRows = [];
        }
    });

    $(".ckb-um").each(function () {
        $(this).change(function () {
            UpdateCheckedRows();
        });
    });
}

function UpdateCheckedRows() {
    checkedRows = [];
    $(".ckb-um").each(function () {
        if (this.checked) {
            checkedRows.push(cachedUMConfigData[$(this).attr('data-position')]);
        }
    });
    var isCheckAll = checkedRows.length === cachedUMConfigData.length;
    $('#ckb-all-um').prop('checked', isCheckAll);
}

function OpenNewUMConfig() {
    InitAddNewUMConfig();
    cachedNewUMConfigs = [];
    $('#new-umconfig-popup').dialog({
        title: 'Add UMConfig',
        modal: true,
        width: 'auto',
        height: 800,
        close: function (event, ui) {
            $(this).dialog('destroy');
        }
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
    $("#UMFunctionType").empty();
    if (cachedUMConfigSetting) {
        var defaultOption = new Option('FunctionType', '', true, true);
        defaultOption.disabled = true;
        functionTypeSelection.append(defaultOption);
        cachedUMConfigSetting.forEach(function (item, index) {
            functionTypeSelection.append(new Option('[' + item.FunctionType + '] ' + item.FunctionTypeName, item.FunctionType));
        });
        functionTypeSelection.select2({
            minimumInputLength: 0,
            width: '200px'
        });
    }

    RenderSiteSelect();

    var UMProductName = $("#UMProductName");
    UMProductName.empty();
    var productDefaultOption = new Option('ProductName', '', true, true);
    productDefaultOption.disabled = true;
    UMProductName.append(productDefaultOption);
    UMProductName.select2({
        minimumInputLength: 0,
        width: '200px'
    });

    var UMSportName = $("#UMSportName");
    UMSportName.empty();
    var sportDefaultOption = new Option('SportName', '', true, true);
    sportDefaultOption.disabled = true;
    UMSportName.append(sportDefaultOption);
    UMSportName.select2({
        minimumInputLength: 0,
        width: '200px'
    });

    var UMSettingType = $("#UMSettingType");
    UMSettingType.empty();
    var settingDefaultOption = new Option('SettingType', '', true, true);
    settingDefaultOption.disabled = true;
    UMSettingType.append(settingDefaultOption);
    UMSettingType.select2({
        minimumInputLength: 0,
        width: '200px'
    });
    
    $('#UMSite').val("0");
    $('#UMCurrency').val("0").trigger('change');
    $("#UMShowTime").val("");
    $("#UMEndTime").val("");
    $("#UMBeginTime").val("");
    $('#UMLockAddingNew').prop('checked', false);
    $("#UMUsername").val("");
    $('#UMUsername').attr('style', 'width: 100%;');
}

function ValidateUMData(divError, beginTime, endTime, showTime, isIgnoreValidateShowTime) {
    if (!isIgnoreValidateShowTime && showTime === "") {
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

    if (!isIgnoreValidateShowTime && showDate >= beginDate) {
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

    if (!$('#UMSite').multiSelect("getCheckedItems") || $('#UMSite').multiSelect("getCheckedItems").length === 0) {
        showAlertMessage('newUMConfigErrMsg', "Site is required.", 1);
        return;
    }

    if (!ValidateUMData('newUMConfigErrMsg', beginTime, endTime, showTime, IsReportsBetListsFunctionType())) {
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
    AppendNewUMConfigs();
    RenderAllNewUMConfigs();
    InitAddNewUMConfig();
}

function AppendNewUMConfigs() {
    var usernamesArray = $("#UMUsername").val().replace(/<[^>]+>/g, "").split(',').filter(function (item, pos, self) {
        return self.indexOf(item) === pos;
    });


    if ($('#UMSite').multiSelect("getUncheckItems").length === 0) {
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
            SiteId: 0,
            Site: '[0] All Sites',
            Currency: $('#UMCurrency').val(),
            CurrencyName: $('#UMCurrency').val() === "0" ? "[0] All" : $('#UMCurrency option:selected').text(),
            BeginTime: $("#UMBeginTime").val(),
            EndTime: $("#UMEndTime").val(),
            ShowTime: $("#UMShowTime").val(),
            LockAddingNew: $('#UMLockAddingNew').is(":checked"),
            Username: usernamesArray.join(',')
        };

        cachedNewUMConfigs.push(umConfig);
    } else {
        var selectedSites = $('#UMSite').multiSelect("getCheckedItems");
        selectedSites.forEach(function (selSite) {
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
                SiteId: selSite,
                Site: $('#sel_opt_newum_' + selSite).attr('title'),
                Currency: $('#UMCurrency').val(),
                CurrencyName: $('#UMCurrency').val() === "0" ? "[0] All" : $('#UMCurrency option:selected').text(),
                BeginTime: $("#UMBeginTime").val(),
                EndTime: $("#UMEndTime").val(),
                ShowTime: $("#UMShowTime").val(),
                LockAddingNew: $('#UMLockAddingNew').is(":checked"),
                Username: usernamesArray.join(',')
            };

            cachedNewUMConfigs.push(umConfig);
        });
    }

    
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
            InitAddNewUMConfig();
            showAlertMessage("newUMConfigErrMsg", "Insert Successfull.", 0);
            cachedNewUMConfigs = [];
            RenderAllNewUMConfigs();
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
        UMProductName.select2({
            minimumInputLength: 0,
            width: '200px'
        });
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
        UMSportName.select2({
            minimumInputLength: 0,
            width: '200px'
        });
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
        UMSettingType.select2({
            minimumInputLength: 0,
            width: '200px'
        });
    }
}

function RenderSiteSelect() {
    var multiselectData = [];
    var cachedSites = [];
    var isSelectedReportsBetLists = false;
    var functionType = $('#UMFunctionType').val();
    if (functionType === "101") {
        cachedSites = cachedAllSites;
    } else if (IsReportsBetListsFunctionType(functionType)) {
        cachedSites = cachedCreditSites;
        isSelectedReportsBetLists = true;
    } else {
        cachedSites = cachedCreditSites;
    }

    cachedSites.forEach(function (item, index) {
        if(item.IsParent){
            htmlOptions.push('<optgroup label="' + item.Name + '">');
            item.ChildSites.forEach(function(child, i){
                htmlOptions.push('<option value="');
                htmlOptions.push(child.Id);
                htmlOptions.push('">');
                htmlOptions.push(child.Name);
                htmlOptions.push('</option>');
            });
            htmlOptions.push('</optgroup>');
        }else{
            htmlOptions.push('<option value="');
            htmlOptions.push(item.Id);
            htmlOptions.push('">');
            htmlOptions.push(item.Name);
            htmlOptions.push('</option>');
        }
    });



    OnSelectReportsBetListsFunctionType(isSelectedReportsBetLists);

    $('#UMSite').multiSelect({
        minWidth: 200,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: false,
        showFooter: false,
        showFilter: true,
        checkAllText: "[0] All Sites",
        noneSelectedText: "Sites",
        listData: multiselectData,
        noItemText: "No item selected."
    });
}

function EditUMConfig() {
    if (checkedRows.length === 0) {
        return;
    }

    var isDisabledShowTime = false;
    for (var i = 0; i < checkedRows.length; i++) {
        if (IsReportsBetListsFunctionType(checkedRows[i].FunctionType)) {
            isDisabledShowTime = true;
            break;
        }
    }

    var umConfig = checkedRows[0];
    var showTimeElement =  $('#UpdUMShowTime').datetimepicker('destroy');
    $('#UpdUMEndTime').datetimepicker('destroy');
    $('#UpdUMBeginTime').datetimepicker('destroy');
    $('#updateUMConfigErrMsg').html('');
    $('#UpdUMFunctionType').html("[" + umConfig.FunctionType + "] " + umConfig.FunctionTypeName);
    $('#UpdUMUsername').html(umConfig.Username);
    $('#UpdUMProductName').html(umConfig.ProductName);
    showTimeElement.val(umConfig.ShowTime).prop("disabled", isDisabledShowTime);
    $('#UpdUMSportName').html(umConfig.SportName);
    $('#UpdUMBeginTime').val(umConfig.BeginTime);
    $('#UpdUMSettingType').html("[" + umConfig.SettingType + "] " + umConfig.SettingName);
    $('#UpdUMEndTime').val(umConfig.EndTime);
    $('#UpdUMSite').html("[" + umConfig.SiteId + "] " + umConfig.Site);
    $('#UpdUMLockAddingNew').prop('checked', umConfig.LockAddingNew).prop("disabled", isDisabledShowTime);
    $('#UpdUMCurrency').html("[" + umConfig.Currency + "] " + umConfig.CurrencyName);

    $('#btnUpdateUMConfig').off();
    $('#btnUpdateUMConfig').click(function () { return UpdateUMConfig(isDisabledShowTime); });
    $.ui.dialog.prototype._focusTabbable = function () { };
    $('#edit-umConfig-form').dialog({
        title: 'Edit UMConfig',
        modal: true,
        width: 'auto',
        height: 'auto',
        open: function () {
            $("#UpdUMShowTime_Trigger").prop("disabled", isDisabledShowTime);
            isDisabledShowTime || SetupUMCalendar('UpdUMShowTime');
            SetupUMCalendar('UpdUMEndTime');
            SetupUMCalendar('UpdUMBeginTime');
        }
    });
}

function CallUpdateUMConfig(umConfig) {
    return $.ajax({
        url: 'UpdateUMConfiguration',
        type: 'POST',
        dataType: 'json',
        data: umConfig
    });
}

function UpdateUMConfig(isDisabledShowTime) {
    parent.BlockUI();
    var beginTime = $('#UpdUMBeginTime').val();
    var endTime = $('#UpdUMEndTime').val();
    var showTime = $('#UpdUMShowTime').val();
    var lockAddingNew = $('#UpdUMLockAddingNew').is(":checked");
    if (!ValidateUMData('updateUMConfigErrMsg', beginTime, endTime, showTime, isDisabledShowTime)) {
        parent.jQuery.unblockUI();
        return;
    }

    var requests = [];
    var divError = '';
    checkedRows.forEach(function (umConfig) {
        umConfig.BeginTime = beginTime;
        umConfig.EndTime = endTime;
        if (!isDisabledShowTime) {
            umConfig.ShowTime = showTime;
            umConfig.LockAddingNew = lockAddingNew;
        }

        requests.push(CallUpdateUMConfig(umConfig));
    });

    $.when.apply(undefined, requests).then(function () {
        var responses = arguments;
        if (checkedRows.length === 1) {
            if (responses[1] === "success") {
                if (responses[0].ErrorCode !== 0) {
                    divError = divError + "Config ID: " + checkedRows[0].ConfigId + ' ' + responses[0].ErrorMessage;
                }
            } else {
                divError = divError + "Internal Error.";
            }
        } else {
            for (var j = 0; j < responses.length; j++) {
                response = responses[j];
                if (response[1] === "success") {
                    if (response[0].ErrorCode !== 0) {
                        divError = divError + "Config ID: " + checkedRows[j].ConfigId + ": " + response[0].ErrorMessage + "</br>";
                    }
                } else {
                    divError = divError + "Config ID: " + checkedRows[j].ConfigId + ": Internal Error.</br>";
                }
            }
        }


        if (divError === "") {
            alert('Update successfully.');
            $('#edit-umConfig-form').dialog('close');
            SearchAllUMConfig();
        } else {
            parent.jQuery.unblockUI();
            showAlertMessage('updateUMConfigErrMsg', divError, 1);
        }
    });
}

function DeleteUMConfig() {
    if (checkedRows.length !== 0) {
        CheckIsInUMTime();
    }
}

function CallCheckUMTime(configId) {
    return $.ajax({
        url: 'CheckIsInUMTime?configId=' + configId,
        type: 'GET'
    });
}

function CheckIsInUMTime() {
    parent.BlockUI();
    var configIdsInUmTime = [];
    var divError = "";
    var requests = [];
    for (var i = 0; i < checkedRows.length; i++) {
        var configId = checkedRows[i].ConfigId;
        requests.push(CallCheckUMTime(configId));
    }

    $.when.apply(undefined, requests).then(function () {
        var responses = arguments;
        if (checkedRows.length === 1) {
            if (responses[1] === "success") {
                if (responses[0]) {
                    configIdsInUmTime.push(checkedRows[0].ConfigId);
                }
            } else {
                divError = divError + "ConfigId " + checkedRows[0].ConfigId +": Internal Error.\n";
            }
        } else {
            for (var j = 0; j < responses.length; j++) {
                response = responses[j];
                if (response[1] === "success") {
                    if (response[0].ErrorCode !== 0) {
                        configIdsInUmTime.push(checkedRows[j].ConfigId);
                    }
                } else {
                    divError = divError + "ConfigId " + checkedRows[j].ConfigId + ": Internal Error.\n";
                }
            }
        }


        parent.jQuery.unblockUI();
        if (divError === "") {
            var message = "Are you sure you want to delete selected row(s)?";
            if (configIdsInUmTime.length > 0) {
                message = "Config ID: " + configIdsInUmTime.join(', ') + " is in UM time. Are you sure you want to delete selected rows?";
            }

            ShowConfirmDelete(message, checkedRows);
        } else {
            alert(divError);
        }
    });
}

function CallDeleteUMConfig(configId) {
    return $.ajax({
        url: 'DeleteUMConfiguration',
        type: 'POST',
        dataType: 'json',
        data: {
            ConfigId: configId
        }
    });
}

function ShowConfirmDelete(message, deleteRows) {
    if (confirm(message)) {
        parent.BlockUI();
        var divError = "";
        var notExistConfigIds = [];
        var requests = [];

        for (var i = 0; i < deleteRows.length; i++) {
            var configId = deleteRows[i].ConfigId;
            requests.push(CallDeleteUMConfig(configId));
        }

        $.when.apply(undefined, requests).then(function () {
            var responses = arguments;
            if (deleteRows.length === 1) {
                if (responses[1] === "success") {
                    if (responses[0].ErrorCode !== 0 && response[0].ErrorCode !== 1) {
                        divError = divError + "Config ID: " + deleteRows[0].ConfigId + ' ' + responses[0].ErrorMessage + "\n";
                    }

                    if (response[0].ErrorCode === 1) {
                        notExistConfigIds.push(deleteRows[0].ConfigId);
                    }
                } else {
                    divError = divError + "Internal Error.";
                }
            } else {
                for (var j = 0; j < responses.length; j++) {
                    response = responses[j];
                    if (response[1] === "success") {
                        if (response[0].ErrorCode !== 0 && response[0].ErrorCode !== 1) {
                            divError = divError + "Config ID: " + deleteRows[j].ConfigId + ": " + response[0].ErrorMessage + "\n";
                        }

                        if (response[0].ErrorCode === 1) {
                            notExistConfigIds.push(deleteRows[j].ConfigId);
                        }
                    } else {
                        divError = divError + "Config ID: " + deleteRows[j].ConfigId + ": Internal Error.\n";
                    }
                }
            }

            parent.jQuery.unblockUI();
            if (notExistConfigIds.length > 0) {
                message = "Config ID: " + notExistConfigIds.join(', ') + " don't exist.\n";
                divError = message + divError;
            }

            if (divError === "" ) {
                alert('Deleted successfully.');
                SearchAllUMConfig();
            } else {
                alert(divError);
                SearchAllUMConfig();
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
    $('#' + divId).datetimepicker("destroy");
    var _today = GetTodayWithOffset('m/d/y', 8);
    var _todayTime = GetTodayWithOffset('HH:mm', 8);
    $('#' + divId).datetimepicker({
        format: 'm/d/Y H:i:00',
        step: 5,
        defaultDate: _today, formatDate: 'm/d/y',
        defaultTime: _todayTime, formatTime: 'H:i'
    });

    $('#' + divId + '_Trigger').click(function () {
        jQuery('#' + divId).datetimepicker('show');
    });
}

function SetupFilterCalendar(divId) {
    var _today = GetTodayWithOffset('m/d/y', 8);
    $('#' + divId).datetimepicker("destroy");
    $('#' + divId).datetimepicker({
        timepicker: false,
        format: 'm/d/Y',
        defaultDate: _today, formatDate: 'm/d/y'
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

function OnSelectReportsBetListsFunctionType(isSelectedReportsBetLists) {
    var currencyElement = $("#UMCurrency").prop("disabled", isSelectedReportsBetLists);
    var showTimeElement = $("#UMShowTime").prop("disabled", isSelectedReportsBetLists);
    $("#UMShowTime_Trigger").prop("disabled", isSelectedReportsBetLists);
    var lockAddingNewElement = $("#UMLockAddingNew").prop("disabled", isSelectedReportsBetLists);
    var usernameElement = $("#UMUsername").prop("disabled", isSelectedReportsBetLists);
    if (isSelectedReportsBetLists) {
        currencyElement.val("0");
        showTimeElement.val("");
        lockAddingNewElement.prop("checked", false);
        usernameElement.val("");
    }
}

function IsReportsBetListsFunctionType(functionTypeValue) {
    var functionType = parseInt(functionTypeValue || $("#UMFunctionType").val());
    return functionType >= 201 && functionType <= 216;
}

$(document).ready(function () {
    GetInitDataAndSearchAllUMConfig();
    $('#UMCurrency').select2({ width: "200px" });
});