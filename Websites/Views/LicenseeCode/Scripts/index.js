var filterValues = {
    LicenseeIds: '',
    StatusIds: '',
    PageIndex: 0,
    PageSize: 50
};

$(document).ready(function () {
    RenderMultiSelect("selectLicensee");
    RenderMultiSelect("selectStatus");
    SearchLicenseeCode(0, 50);
});

function RenderMultiSelect(id) {
    var multiselectData = [];
    $('#' + id + ' > option').each(function () {
        var $this = $(this);
        multiselectData.push({
            Id: id + $this.attr('value'),
            Value: $this.attr('value'),
            Text: $this.html(),
            Checked: true,
            Enable: true,
            CssClass: "",
            Items: []
        });
    });


    $('#' + id).multiSelect({
        minWidth: 150,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: true,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "No item selected."
    });
}

function SearchLicenseeCode(pageIndex, pageSize) {
    parent.BlockUI();
    var lienseeIds = $("#selectLicensee").multiSelect("getCheckedItems").join(',');
    var status = $("#selectStatus").multiSelect("getCheckedItems").join(',');
    filterValues.LicenseeIds = lienseeIds;
    filterValues.StatusIds = status;
    filterValues.PageSize = pageSize;
    filterValues.PageIndex = pageIndex + 1;

    $.ajax({
        type: "POST",
        url: "SearchLicenseeCode",
        contentType: "application/json",
        data: JSON.stringify(filterValues),
        success: function (data) {
            RenderLicenseeCode(data, pageSize, pageIndex + 1);
        },
        error: function (err) {
            resolve('Internal server error.');
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderLicenseeCode(data, pageSize, pageIndex) {
    var bodyRows = [];
    var licenseeCodes = data.LicenseeCodes;
    for (i = 0; i < licenseeCodes.length; i++) {
        var licenseeCode = licenseeCodes[i];
        var row = { Columns: [] };
        row.Columns.push({ Name: 'No', Value: licenseeCode.RowNumber });
        row.Columns.push({ Name: 'Licensee', Value: licenseeCode.Name });
        row.Columns.push({ Name: 'LicenseeCode', Value: licenseeCode.Code });
        row.Columns.push({ Name: 'FromDate', Value: licenseeCode.FromDate });
        row.Columns.push({ Name: 'ToDate', Value: licenseeCode.ToDate });
        row.Columns.push({
            Name: 'AddNew',
            Value: '<img class="add-entity" alt="Add" title="Add" src="../App_Themes/Images/icn_add.png" style="cursor: pointer" onclick="addLicenseeCode(\''
                + licenseeCode.Name
                + '\')">'
        });
        if (licenseeCode.Status === 'OFF' || licenseeCode.Status === 'ON') {
            row.Columns.push({
                Name: 'Edit',
                Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="showLicenseeCodeStatusDialog(\''
                    + licenseeCode.Name
                    + "\',\'"
                    + licenseeCode.Code
                    + "\',\'"
                    + licenseeCode.FromDate
                    + "\',\'"
                    + licenseeCode.ToDate
                    + "\',\'"
                    + licenseeCode.Status
                    + '\')">'
            });
            row.Columns.push({ Name: 'Status', Value: '<label>' + licenseeCode.Status + '</label>', CssClass: 'status' + licenseeCode.Status });
        } else if (licenseeCode.Status === 'N/A') {
            row.Columns.push({ Name: 'Edit', Value: '<label></label>' });
            row.Columns.push({ Name: 'Status', Value: '<label>N/A</label>', CssClass: 'statusNA' });
        } else if (licenseeCode.Status === 'Code Needed') {
            row.Columns.push({ Name: 'Edit', Value: '<label></label>' });
            row.Columns.push({ Name: 'Status', Value: '<label>Code Needed</label>', CssClass: 'statusCN' });
        }
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "#", Width: 25, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Licensee", Width: 100, Name: "Licensee", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Licensee Code", Width: 200, Name: "LicenseeCode", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "From Date", Width: 100, Name: "FromDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "To Date", Width: 100, Name: "ToDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Add New", Width: 50, Name: "AddNew", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Edit", Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Status", Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showPager: true,
        customPager: true,
        showDistinctRow: false,
        pagerOption: {
            itemsPerPage: pageSize,
            currentPage: pageIndex - 1,
            itemsPerPageArray: [50, 100, 150, 200],
            pagerSelectCallBack: SearchLicenseeCode,
            totalItem: data.Totals
        }
    };

    $('#gridLicenseeCode').fanexGrid(gridOptions);

    $("#billingMonth").html('Billing Month: ' + data.CurrentBillingMonth);
}

function showLicenseeCodeStatusDialog(licensee, licenseeCode, fromDate, toDate, status) {
    var updRequest =
    {
        LicenseeName: licensee,
        LicenseeCode: licenseeCode,
        FromDate: fromDate,
        ToDate: toDate,
        FromDateOld: fromDate,
        ToDateOld: toDate,
        CodeStatus: status === 'ON'
    };

    validateLicenseeCodeBeforeUpdate(updRequest);

    hideAlertMessage("updErrorMsg1");
    hideAlertMessage("updErrorMsg2");
    hideAlertMessage("updWarningMsg1");
    hideAlertMessage("updWarningMsg2");

    $('#NewStatus').text(status);

    var displayFromDate = GetDisplayMonthOnly(updRequest.FromDate);
    var displayToDate = GetDisplayMonthOnly(updRequest.ToDate);
    SetupCalendar('UpdateFromDate', displayFromDate);
    SetupCalendar('UpdateToDate', displayToDate);
    $('#valueUpdateFromDate').val(updRequest.FromDate);
    $('#valueUpdateToDate').val(updRequest.ToDate);

    $('#updateLicenseeCode').dialog({
        title: 'Confirmation',
        modal: true,
        resizable: false,
        width: 'auto',
        height: 'auto',
        buttons: [
            {
                text: "Cancel",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "Save",
                click: function () {
                    updRequest.FromDate = GetFromDate($("#valueUpdateFromDate").val());
                    updRequest.ToDate = GetToDate($("#valueUpdateToDate").val());
                    validateLicenseeCodeBeforeUpdate(updRequest, updateLicenseeCodeStatus);
                }
            }
        ]
    });
}

function validateLicenseeCodeBeforeUpdate(request, updateFunction = DefaultFunction) {
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "ValidateLicenseeCode",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (data) {
            var errors = data.filter(IsError);
            if (errors.length > 0) {
                for (var i = 0; i < errors.length; i++) {
                    showAlertMessage('updErrorMsg' + (i + 1), errors[i].ErrorMessage, 1);
                }
            } else {
                var warnings = data.filter(IsWarning);
                for (var i = 0; i < warnings.length; i++) {
                    showAlertMessage('updWarningMsg' + (i + 1), warnings[i].ErrorMessage, 2);
                }

                updateFunction(request);
            }
        },
        error: function (err) {
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function DefaultFunction() {
    return false;
}

function updateLicenseeCodeStatus(request) {
    hideAlertMessage('errMsg');
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "UpdateLicenseeCode",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (data) {
            if (data.ValidateResponse.length > 0) {
                var message = [];
                for (var i = 0; i < data.ValidateResponse.length; i++) {
                    message.push(data.ValidateResponse[i].ErrorMessage);
                }
                showAlertMessage('errMsg', message.join("<br/>"), 1);
            } else if (data.NonQueryResult.ErrorCode === 0) {
                showAlertMessage('errMsg', 'Update Successfully.', 0);
                setTimeout(function () {
                    hideAlertMessage('errMsg');
                }, 3000);
                SearchLicenseeCode(filterValues.PageIndex - 1, filterValues.PageSize);
            } else {
                showAlertMessage('errMsg', data.NonQueryResult.ErrorMessage, 1);
            }
        },
        error: function (err) {
            showAlertMessage('errMsg', 'Internal server error.', 1);
        },
        complete: function () {
            $("#updateLicenseeCode").dialog("close");
            parent.jQuery.unblockUI();
        }
    });
}

function newLicenseeCodeStatus(request) {
    hideAlertMessage('errMsg');
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "NewLicenseeCode",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (data) {
            if (data.ValidateResponse.length > 0) {
                var message = [];
                for (var i = 0; i < data.ValidateResponse.length; i++) {
                    message.push(data.ValidateResponse[i].ErrorMessage);
                }
                showAlertMessage('errMsg', message.join("<br/>"), 1);
            } else if (data.NonQueryResult.ErrorCode === 0) {
                showAlertMessage('errMsg', 'Add Successfully.', 0);
                setTimeout(function () {
                    hideAlertMessage('errMsg');
                }, 3000);
                SearchLicenseeCode(filterValues.PageIndex - 1, filterValues.PageSize);
                
            } else {
                showAlertMessage('errMsg', data.NonQueryResult.ErrorMessage, 1);
            }
        },
        error: function (err) {
            showAlertMessage('errMsg', 'Internal server error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function addLicenseeCode(licenseeSite) {
    hideAlertMessage('addLicenseeCodeErrMsg');
    $('#LicenseeName').text(licenseeSite); 
    $('#LicenseeCode').val('');
    var currentMonth = GetDisplayMonthOnly(new Date());
    SetupCalendar('NewFromDate', currentMonth);
    SetupCalendar('NewToDate', 'Jan 2050');
    $('#addLicenseeCode').dialog({
        title: 'Add New Code',
        modal: true,
        resizable: false,
        width: 'auto',
        height: 'auto',
        close: function (event, ui) {
            $(this).dialog('destroy');
        },
        buttons: [
            {
                text: "Cancel",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "Save",
                click: function () {
                    var licenseeCode = $('#LicenseeCode').val();
                    var status = $('#LicenseeCodeStatus').prop('checked');
                    ValidateLicenseeCode(licenseeSite, licenseeCode, status, $(this));
                }
            }
            ]
    });
}

function IsError(result) {
    return result.ErrorCode === 201 || result.ErrorCode === 202 || result.ErrorCode === 203 || result.ErrorCode === 405;
}

function IsWarning(result) {
    return result.ErrorCode === 101 || result.ErrorCode === 102;
}

function ValidateLicenseeCode(licensee, licenseeCode, status, diablog) {
    var request =
    {
        LicenseeName: licensee,
        LicenseeCode: licenseeCode,
        CodeStatus: status,
        FromDate: GetFromDate($("#valueNewFromDate").val()),
        ToDate: GetToDate($("#valueNewToDate").val())
    };

    if (CheckFromToDate(request.FromDate, request.ToDate)) {
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "ValidateLicenseeCode",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                var errors = data.filter(IsError);
                if (errors.length > 0) {
                    var message = [];
                    for (var i = 0; i < errors.length; i++) {
                        message.push(errors[i].ErrorMessage);
                    }
                    showAlertMessage('addLicenseeCodeErrMsg', message.join("<br/>"), 1);
                } else {
                    diablog.dialog("close");
                    var warnings = data.filter(IsWarning);
                    showAddConfirmBox('Are you sure you want to save the change?', warnings, newLicenseeCodeStatus, request);
                }
            },
            error: function (err) {
                showAlertMessage('addLicenseeCodeErrMsg', 'Internal server error.', 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    } else {
        showAlertMessage('addLicenseeCodeErrMsg', 'From Date could not be later than To Date', 1);
    }
}

function showAddConfirmBox(message, warnings, fncCallback, params) {
    var dialoghtml = jQuery('<div id="dialog-confirm" title="Confirmation" style=" overflow:hidden"><div id="confirmWarningMsg1"></div><div id="confirmWarningMsg2"></div><div class="confirmMsg">' + message + '</div></div>');
    dialoghtml.appendTo(jQuery("body"));
    $('#dialog-confirm').dialog({
        modal: true,
        resizable: false,
        width: 'auto',
        height: 'auto',
        zIndex: 9999,
        close: function (event, ui) {
            $(this).dialog('destroy').remove();
        },
        buttons: [
            {
                text: "Cancel",
                click: function () {
                    $(this).dialog("close");
                }
            },
            {
                text: "Save",
                click: function () {
                    $(this).dialog("close");
                    fncCallback(params);
                }
            }]
    });
    hideAlertMessage('confirmWarningMsg1');
    hideAlertMessage('confirmWarningMsg2');

    for (var i = 0; i < warnings.length; i++) {
        showAlertMessage('confirmWarningMsg' + (i + 1), warnings[i].ErrorMessage, 2);
    }
}

function SetupCalendar(id, selectedMonth) {
    $("#" + id).MonthPicker({
        MonthFormat: 'M yy',
        AltFormat: 'mm/01/yy',
        AltField: '#value' + id,
        SelectedMonth: selectedMonth,
        Button: '<img class="img-date-selector" alt="" id="' + id + '_Trigger" title="Month selector" width="18" height="17" src="../App_Themes/Images/calendar.gif" type="image" /> '
    });
}

function CheckFromToDate(fromDate, toDate) {
    const x = new Date(fromDate);
    const y = new Date(toDate);

    return +x <= +y;
}

function GetDisplayMonthOnly(dateString) {
    const date = new Date(dateString);

    return date.toLocaleDateString('en-US', { month: 'short', year: 'numeric' });
}

function GetFromDate(dateString) {
    const date = new Date(dateString);
    const fromDate = new Date(date.getFullYear(), date.getMonth(), 1);

    return fromDate.toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' });
}

function GetToDate(dateString) {
    const date = new Date(dateString);
    const endDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);

    return endDate.toLocaleDateString('en-US', { month: '2-digit', day: '2-digit', year: 'numeric' });
}

function Search() {
    hideAlertMessage("errMsg");
    SearchLicenseeCode(0, filterValues.PageSize);
}