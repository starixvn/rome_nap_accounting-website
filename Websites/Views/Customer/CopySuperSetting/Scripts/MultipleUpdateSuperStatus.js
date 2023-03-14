var updateData = {};
var superList = '';
function MultipleUpdateSuperStatus_CheckSuper(isReloadedAfterUpdate, errorResults) {
    parent.BlockUI();
    if (isReloadedAfterUpdate !== true) {
        $('#MultipleUpdateSuperStatus_errMsg').html('');
        $('#MultipleUpdateSuperStatus_errSupers').html('');
        superList = $("#MultipleUpdateSuperStatus_superlist").val();

        if (superList === "") {
            showAlertMessage('MultipleUpdateSuperStatus_errMsg', "Please enter at least 1 Super Account username.", 1);
            parent.jQuery.unblockUI();
            return;
        }

        if (superList.split(',').length > 500) {
            showAlertMessage('MultipleUpdateSuperStatus_errMsg', "There're more than 500 Usernames.", 1);
            parent.jQuery.unblockUI();
            return;
        }
    }

    var request = {
        SuperList: superList
    };
    $("#MultipleUpdateSuperStatus_superSetting").addClass("hidden");
    $.ajax({
        type: "POST",
        url: "../MultipleUpdateSuperStatus/GetSupersStatus",
        contentType: "application/json",
        data: JSON.stringify(request),
        success: function (data) {
            if (data.ErrorCode === 999) {
                showAlertMessage('MultipleUpdateSuperStatus_errMsg', data.ErrorMessage, 1);
                return;
            }

            updateData = data;
            if (isReloadedAfterUpdate) {
                if (data.Supers.length > 0) {
                    $("#MultipleUpdateSuperStatus_superSetting").removeClass("hidden");
                    RenderGrid(data.Supers, isReloadedAfterUpdate, errorResults);
                }

                return;
            }

            if (data.ErrCode !== 0) {
                showAlertMessage('MultipleUpdateSuperStatus_errMsg', data.ErrMsg, 1);
            }

            if (data.Supers.length > 0) {
                $("#MultipleUpdateSuperStatus_superSetting").removeClass("hidden");
                RenderGrid(data.Supers);
            }
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderGrid(data, isReloadedAfterUpdate, errorResults) {
    var bodyRows = [];
    var rowCount = data.length;
    for (var i = 0; i < rowCount; i++) {
        var row = { Columns: [], CssClass: isReloadedAfterUpdate && isErrorRow(data[i], errorResults) ? 'highlight-error' : '' };
        row.Columns.push({ Name: 'No', Value: i + 1 });
        row.Columns.push({ Name: 'UserName', Value: data[i].UserName });
        row.Columns.push({ Name: 'Site', Value: data[i].Site });
        row.Columns.push({ Name: 'Closed', Value: data[i].Closed ? 'Yes' : 'No' });
        row.Columns.push({ Name: 'Suspended', Value: data[i].Suspended ? 'Yes' : 'No' });
        row.Columns.push({ Name: 'Protected', Value: data[i].Protected ? 'Yes' : 'No' });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "No.", Width: 30, Name: "No", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Super Accounts", Width: 200, Name: "UserName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Closed<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Close All\" class=\"btn\" onclick=\"MultipleUpdateSuperStatus_UpdateClosedStatus(true)\" /><input type=\"button\" value=\"Open All\" class=\"btn\" onclick=\"MultipleUpdateSuperStatus_UpdateClosedStatus(false)\" /> </div>", Width: 150, Name: "Closed", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Suspended<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Suspend All\" class=\"btn\" onclick=\"MultipleUpdateSuperStatus_UpdateSuspendedStatus(true)\" /><input type=\"button\" value=\"Unsuspend All\" class=\"btn\" onclick=\"MultipleUpdateSuperStatus_UpdateSuspendedStatus(false)\" /> </div>", Width: 150, Name: "Suspended", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Protected<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Protect All\" class=\"btn\" onclick=\"MultipleUpdateSuperStatus_UpdateProtectedStatus(true)\" /><input type=\"button\" value=\"Unprotect All\" class=\"btn\" onclick=\"MultipleUpdateSuperStatus_UpdateProtectedStatus(false)\" /> </div>", Width: 150, Name: "Protected", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        showDistinctRow: true,
        gridExpandHeight: 0
    };

    $('#MultipleUpdateSuperStatus_grid').fanexGrid(gridOptions);
}

function isErrorRow(dataRow, errorResults) {
    return errorResults.some(o => o.CustId === dataRow.CustId);
}

function MultipleUpdateSuperStatus_UpdateClosedStatus(isClosed) {
    ConfirmBox(isClosed ?
            'Are you sure you want to <b>Close All</b> these selected Super Accounts?</br><i>* Please note that Closing all these Super Accounts will also close all their downlines at once.</i>'
            : 'Are you sure you want to <b>Open All</b> these selected Super Accounts?</br><i>* Please note that only the downlines that were indirectly closed because of closing Super Account level before, shall be re - opened automatically.</i>',
        () => {
            parent.BlockUI();
            $('#MultipleUpdateSuperStatus_errMsg').html('');
            $('#MultipleUpdateSuperStatus_errSupers').html('');

            var request = {
                CustList: [],
                IsClosed: isClosed
            };

            updateData.Supers.map(o => request.CustList.push({ CustId: o.CustId, UserName: o.UserName }));
            $.ajax({
                type: "POST",
                url: "../MultipleUpdateSuperStatus/UpdateClosedStatus",
                contentType: "application/json",
                data: JSON.stringify(request),
                success: function (results) {
                    UpdateSuccessCallback(results);
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        });
}

function MultipleUpdateSuperStatus_UpdateSuspendedStatus(isSuspended) {
    ConfirmBox(isSuspended ?
        'Are you sure you want to <b>Suspend All</b> these selected Super Accounts?'
        : 'Are you sure you want to <b>Unsuspend All</b> these selected Super Accounts?',
        () => {
            parent.BlockUI();
            $('#MultipleUpdateSuperStatus_errMsg').html('');
            $('#MultipleUpdateSuperStatus_errSupers').html('');

            var request = {
                CustList: [],
                IsSuspended: isSuspended
            };

            updateData.Supers.map(o => request.CustList.push({ CustId: o.CustId, UserName: o.UserName }));
            $.ajax({
                type: "POST",
                url: "../MultipleUpdateSuperStatus/UpdateSuspendedStatus",
                contentType: "application/json",
                data: JSON.stringify(request),
                success: function (results) {
                    UpdateSuccessCallback(results);
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        });
}

function MultipleUpdateSuperStatus_UpdateProtectedStatus(isProtected) {
    ConfirmBox(isProtected ?
        'Are you sure you want to <b>Protect All</b> these selected Super Accounts?</br><i>* Please note that Protection Status is supposed just to be turned on in emergency cases.</i>'
        : 'Are you sure you want to <b>Unprotect All</b> these selected Super Accounts?',
        () => {
            parent.BlockUI();
            $('#MultipleUpdateSuperStatus_errMsg').html('');
            $('#MultipleUpdateSuperStatus_errSupers').html('');

            if (!updateData.Supers.some(o => o.IsOfferedProtectedMode)) {
                showAlertMessage('MultipleUpdateSuperStatus_errMsg', "Cannot apply Protection status to these Super Accounts.", 1);
                parent.jQuery.unblockUI();
                return;
            }

            var request = {
                CustList: [],
                IsProtected: isProtected
            };

            updateData.Supers.map(o => request.CustList.push({ CustId: o.CustId, UserName: o.UserName, Site: o.Site, SiteLabel: o.SiteLabel }));
            $.ajax({
                type: "POST",
                url: "../MultipleUpdateSuperStatus/UpdateProtectedStatus",
                contentType: "application/json",
                data: JSON.stringify(request),
                success: function (results) {
                    UpdateSuccessCallback(results);
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        });
}

function UpdateSuccessCallback(results) {
    // No permission.
    if (results.ErrorCode === 999) {
        showAlertMessage('MultipleUpdateSuperStatus_errMsg', results.ErrorMessage, 1);
        return;
    }

    MultipleUpdateSuperStatus_CheckSuper(true, results);

    // No Error.
    if (results.length === 0) {
        showAlertMessage('MultipleUpdateSuperStatus_errMsg', 'Status of these Super Accounts are updated successfully.', 0);
        return;
    }

    // All errors
    if (results.length === updateData.Supers.length) {
        showAlertMessage('MultipleUpdateSuperStatus_errMsg', 'Cannot update status of these Super Accounts. Please contact IT for support.', 1);
        return;
    }

    showAlertMessage('MultipleUpdateSuperStatus_errMsg', 'Status of these Super Accounts are updated successfully. Except the following:', 0);
    var supersErrMsg = '';
    results.map(result => supersErrMsg += result.UserName + ': ' + result.ErrMsg + '</br>');
    showAlertMessage('MultipleUpdateSuperStatus_errSupers', supersErrMsg.substr(0, supersErrMsg.lastIndexOf('</br>')), 1);
}


$(document).ready(function () {
    $("#MultipleUpdateSuperStatus_superlist").off("change").on("change", function () {
        var superlist = $(this);
        var superlistValue = $.trim(superlist.val());
        if (superlistValue.length > 0) {
            if (superlistValue.indexOf("\n") >= 0) {
                superlistValue = superlistValue.replace(/\n/g, " ");
            }

            if (superlistValue.indexOf("\r") >= 0) {
                superlistValue = superlistValue.replace(/\r/g, " ");
            }

            if (superlistValue.indexOf("\t") >= 0) {
                superlistValue = superlistValue.replace(/\t/g, " ");
            }

            superlistValue = superlistValue.replace(/\s/g, ",");
            var superlistArray = superlistValue.split(",").filter(item => item.length > 0);
            superlist.val(Array.from(new Set(superlistArray)).toString());
        }
    });
});

function ConfirmBox(html, successCallback) {
    $("#dialog-confirm").html(html);
    $("#dialog-confirm").dialog({
        resizable: false,
        height: "auto",
        width: 600,
        modal: true,
        buttons: {
            "Yes": function () {
                $(this).dialog("close");
                if (typeof successCallback === 'function')
                    successCallback();
            },
            "No": function () {
                $(this).dialog("close");
            }
        }
    });
}