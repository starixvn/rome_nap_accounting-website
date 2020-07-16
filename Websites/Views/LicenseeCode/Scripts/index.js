var filterValues = {
    LicenseeIds: '',
    StatusIds: '',
    PageIndex: 0,
    PageSize: 50
}
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
        row.Columns.push({
            Name: 'AddNew',
            Value: '<img class="add-entity" alt="Add" title="Add" src="../App_Themes/Images/icn_add.png" style="cursor: pointer" onclick="addLicenseeCode(\''
                + licenseeCode.Name
                + '\')">'
        });
        if (licenseeCode.Status === 'OFF') {
            row.Columns.push({
                Name: 'Status', Value: '<input class="update-status" type="checkbox" data-licensee=\''
                + licenseeCode.Name
                + '\' data-licensee-code=\''
                + licenseeCode.Code
                + '\'>'
            });
        } else if (licenseeCode.Status === 'ON') {
            row.Columns.push({ Name: 'Status', Value: '<input class="update-status" type="checkbox" checked data-licensee=\''
                + licenseeCode.Name
                + '\' data-licensee-code=\''
                + licenseeCode.Code
                +'\'>'
            });
        } else if (licenseeCode.Status === 'N/A') {
            row.Columns.push({ Name: 'Status', Value: '<label>N/A</label>', CssClass: 'statusNA' });
        } else if (licenseeCode.Status === 'Code Needed') {
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
            { HeaderText: "Add New", Width: 50, Name: "AddNew", HeaderAlign: "center", CellAlign: "center" },
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
    $('#gridLicenseeCode').find(".update-status").on("click", function (event) {
        event.preventDefault();
        var $this = $(this);
        var licensee = $this.attr("data-licensee");
        var licenseeCode = $this.attr('data-licensee-code');
        var status = this.checked;
        var updRequest =
        {
            LicenseeName: licensee,
            LicenseeCode: licenseeCode,
            CodeStatus: status
        }
        showLicenseeCodeStatusDialog(updRequest);
    });
}

function showLicenseeCodeStatusDialog(updRequest) {
    if (!updRequest.CodeStatus) {
        showConfirmBox('Are you sure you want to save the change?', updateLicenseeCodeStatus, updRequest);
    } else {
        validateLicenseeCodeBeforeUpdate(updRequest);
    }
}

function validateLicenseeCodeBeforeUpdate(request) {
    parent.BlockUI();
    showConfirmBox('Are you sure you want to save the change?', updateLicenseeCodeStatus, request);
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
            }
        },
        error: function (err) {
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
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
            if (data.ErrorCode === 0) {
                showAlertMessage('errMsg', 'Update Successfully.', 0);
                setTimeout(function () {
                    hideAlertMessage('errMsg');
                }, 3000);
                SearchLicenseeCode(filterValues.PageIndex - 1, filterValues.PageSize);
            } else {
                showAlertMessage('errMsg', data.ErrorMessage, 1);
                parent.jQuery.unblockUI();
            }
        },
        error: function (err) {
            showAlertMessage('errMsg', 'Internal server error.', 1);
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
            if (data.ErrorCode === 0) {
                showAlertMessage('errMsg', 'Add Successfully.', 0);
                setTimeout(function () {
                    hideAlertMessage('errMsg');
                }, 3000);
                SearchLicenseeCode(filterValues.PageIndex - 1, filterValues.PageSize);
                
            } else {
                showAlertMessage('errMsg', data.ErrorMessage, 1);
                parent.jQuery.unblockUI();
            }
        },
        error: function (err) {
            showAlertMessage('errMsg', 'Internal server error.', 1);
            parent.jQuery.unblockUI();
        }
    });
}

function addLicenseeCode(licenseeSite) {
    hideAlertMessage('addLicenseeCodeErrMsg');
    $('#LicenseeName').text(licenseeSite); 
    $('#LicenseeCode').val('');
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
    return result.ErrorCode === 201 || result.ErrorCode === 202;
}

function IsWarning(result) {
    return result.ErrorCode === 101 || result.ErrorCode === 102;
}

function ValidateLicenseeCode(licensee, licenseeCode, status, diablog) {
    parent.BlockUI();
    var request =
    {
        LicenseeName: licensee,
        LicenseeCode: licenseeCode,
        CodeStatus: status
    }
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
            $(this).dialog('destroy').remove()
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

function showConfirmBox(message, fncCallback, params) {
    var dialoghtml = jQuery('<div id="dialog-confirm" title="Confirmation" style=" overflow:hidden"><div id="updErrorMsg1"></div><div id="updErrorMsg2"></div><div id="updWarningMsg1"></div><div id="updWarningMsg2"></div><div class="confirmMsg">' + message + '</div></div>');
    dialoghtml.appendTo(jQuery("body"));
    $('#dialog-confirm').dialog({
        modal: true,
        resizable: false,
        width: 'auto',
        height: 'auto',
        zIndex: 9999,
        close: function (event, ui) {
            $(this).dialog('destroy').remove()
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
}

function Search() {
    hideAlertMessage("errMsg");
    SearchLicenseeCode(0, filterValues.PageSize);
}