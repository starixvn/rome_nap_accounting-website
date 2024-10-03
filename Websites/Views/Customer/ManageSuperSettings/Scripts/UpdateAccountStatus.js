var updateAccountStatus = {
    updateData: null,
    superList: '',
    resetSelect: function () {
        updateAccountStatus.updateData = null;
        updateAccountStatus.superList = '';

        $("#MultipleUpdateSuperStatus_superlist").val('');
        $("#MultipleUpdateSuperStatus_superSetting").addClass("hidden");
        $('#MultipleUpdateSuperStatus_errMsg').html('');
        $('#MultipleUpdateSuperStatus_errSupers').html('');
    },
    confirmBox: function (html, successCallback) {
        $("#tab-uas-dialog-confirm").html(html);
        $("#tab-uas-dialog-confirm").dialog({
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
    },
    getAccountStatus: function () {
        parent.BlockUI();
        $('#MultipleUpdateSuperStatus_errMsg').html('');
        $('#MultipleUpdateSuperStatus_errSupers').html('');
        var superList = $("#MultipleUpdateSuperStatus_superlist").val();

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

        var request = {
            SuperList: superList,
            IsCopy: false,
            Mode: 'AccountStatus'
        };

        updateAccountStatus.superList = superList;

        $("#MultipleUpdateSuperStatus_superSetting").addClass("hidden");
        $.ajax({
            type: "POST",
            url: "CheckSuperList",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.ErrorCode === 999) {
                    showAlertMessage('MultipleUpdateSuperStatus_errMsg', data.ErrorMessage, 1);
                    return;
                }

                updateAccountStatus.updateData = data;
                if (data.Result.ErrorCode !== 0) {
                    showAlertMessage('MultipleUpdateSuperStatus_errMsg', data.Result.ErrorMessage, 1);
                }

                if (data.SuperList && data.SuperList.length > 0) {
                    $("#MultipleUpdateSuperStatus_superSetting").removeClass("hidden");
                    updateAccountStatus.renderGrid(data.SuperList);
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("MultipleUpdateSuperStatus_errMsg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
            }
        });
    },
    renderGrid: function (data, errorResults) {
        var bodyRows = [];
        var rowCount = data.length;
        for (var i = 0; i < rowCount; i++) {
            var row = { Columns: [], CssClass: updateAccountStatus.isErrorRow(data[i], errorResults) ? 'highlight-error' : '' };
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
                { HeaderText: "Closed<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Close All\" class=\"btn\" onclick=\"updateAccountStatus.updateClosedStatus(true)\" /><input type=\"button\" value=\"Open All\" class=\"btn\" onclick=\"updateAccountStatus.updateClosedStatus(false)\" /> </div>", Width: 150, Name: "Closed", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Suspended<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Suspend All\" class=\"btn\" onclick=\"updateAccountStatus.updateSuspendedStatus(true)\" /><input type=\"button\" value=\"Unsuspend All\" class=\"btn\" onclick=\"updateAccountStatus.updateSuspendedStatus(false)\" /> </div>", Width: 150, Name: "Suspended", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Protected<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Protect All\" class=\"btn\" onclick=\"updateAccountStatus.updateProtectedStatus(true)\" /><input type=\"button\" value=\"Unprotect All\" class=\"btn\" onclick=\"updateAccountStatus.updateProtectedStatus(false)\" /> </div>", Width: 150, Name: "Protected", HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: bodyRows,
            showDistinctRow: true,
            gridExpandHeight: 0
        };

        $('#MultipleUpdateSuperStatus_grid').fanexGrid(gridOptions);
    },
    isErrorRow: function (dataRow, errorResults) {
        if (!errorResults) {
            return false;
        }

        return errorResults.some(o => o.CustId === dataRow.CustId);
    },
    updateClosedStatus: function (isClosed) {
        updateAccountStatus.confirmBox(isClosed ?
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

                updateAccountStatus.updateData.SuperList.map(o => request.CustList.push({ CustId: o.CustId, UserName: o.UserName }));
                $.ajax({
                    type: "POST",
                    url: "../MultipleUpdateSuperStatus/UpdateClosedStatus",
                    contentType: "application/json",
                    data: JSON.stringify(request),
                    success: function (results) {
                        updateAccountStatus.updateSuccessCallback(results);
                    },
                    complete: function () {
                        parent.jQuery.unblockUI();
                    }
                });
            });
    },
    updateSuspendedStatus: function (isSuspended) {
        updateAccountStatus.confirmBox(isSuspended ?
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

                updateAccountStatus.updateData.SuperList.map(o => request.CustList.push({ CustId: o.CustId, UserName: o.UserName }));
                $.ajax({
                    type: "POST",
                    url: "../MultipleUpdateSuperStatus/UpdateSuspendedStatus",
                    contentType: "application/json",
                    data: JSON.stringify(request),
                    success: function (results) {
                        updateAccountStatus.updateSuccessCallback(results);
                    },
                    complete: function () {
                        parent.jQuery.unblockUI();
                    }
                });
            });
    },
    updateProtectedStatus: function (isProtected) {
        updateAccountStatus.confirmBox(isProtected ?
            'Are you sure you want to <b>Protect All</b> these selected Super Accounts?</br><i>* Please note that Protection Status is supposed just to be turned on in emergency cases.</i>'
            : 'Are you sure you want to <b>Unprotect All</b> these selected Super Accounts?',
            () => {
                parent.BlockUI();
                $('#MultipleUpdateSuperStatus_errMsg').html('');
                $('#MultipleUpdateSuperStatus_errSupers').html('');

                if (!updateAccountStatus.updateData.SuperList.some(o => o.IsOfferedProtectedMode)) {
                    showAlertMessage('MultipleUpdateSuperStatus_errMsg', "Cannot apply Protection status to these Super Accounts.", 1);
                    parent.jQuery.unblockUI();
                    return;
                }

                var request = {
                    CustList: [],
                    IsProtected: isProtected
                };

                updateAccountStatus.updateData.SuperList.map(o => request.CustList.push({ CustId: o.CustId, UserName: o.UserName, Site: o.Site, SiteLabel: o.SiteLabel }));
                $.ajax({
                    type: "POST",
                    url: "../MultipleUpdateSuperStatus/UpdateProtectedStatus",
                    contentType: "application/json",
                    data: JSON.stringify(request),
                    success: function (results) {
                        updateAccountStatus.updateSuccessCallback(results);
                    },
                    complete: function () {
                        parent.jQuery.unblockUI();
                    }
                });
            });
    },
    updateSuccessCallback: function (results) {
        // No permission.
        if (results.ErrorCode === 999) {
            showAlertMessage('MultipleUpdateSuperStatus_errMsg', results.ErrorMessage, 1);
            return;
        }

        updateAccountStatus.reloadAccountStatus(results);

        // No Error.
        if (results.length === 0) {
            showAlertMessage('MultipleUpdateSuperStatus_errMsg', 'Status of these Super Accounts are updated successfully.', 0);
            return;
        }

        // All errors
        if (results.length === updateData.SuperList.length) {
            showAlertMessage('MultipleUpdateSuperStatus_errMsg', 'Cannot update status of these Super Accounts. Please contact IT for support.', 1);
            return;
        }

        showAlertMessage('MultipleUpdateSuperStatus_errMsg', 'Status of these Super Accounts are updated successfully. Except the following:', 0);
        var supersErrMsg = '';
        results.map(result => supersErrMsg += result.UserName + ': ' + result.ErrMsg + '</br>');
        showAlertMessage('MultipleUpdateSuperStatus_errSupers', supersErrMsg.substr(0, supersErrMsg.lastIndexOf('</br>')), 1);
    },
    reloadAccountStatus: function (updateResults) {
        parent.BlockUI();

        var request = {
            SuperList: updateAccountStatus.superList
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

                updateAccountStatus.updateData = data;

                if (data.SuperList.length > 0) {
                    $("#MultipleUpdateSuperStatus_superSetting").removeClass("hidden");
                    updateAccountStatus.renderGrid(data.SuperList, updateResults);
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("MultipleUpdateSuperStatus_errMsg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
            }
        });
    }
};