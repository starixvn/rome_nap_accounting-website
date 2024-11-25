var copyWinLossLimit = {
    enableDisableInput: function (flag) {
        $("#tab-wll-sourcesuper").prop('disabled', flag);
        $("#tab-wll-superlist").prop('disabled', flag);
        $("#tab-wll-btnCheckSupers").prop('disabled', flag);
    },
    resetSelect: function () {
        copyWinLossLimit.settings = null;
        copyWinLossLimit.enableDisableInput(false);

        $("#tab-wll-superlist").val('');
        $("#tab-wll-sourcesuper").val('');
        $("#tab-wll-superSetting").addClass("hidden");
        $('#tab-wll-settings').html('');
        $('#tab-wll-errMsg').html('');
        $('#tab-wll-errSupers').html('');
    },
    checkSuperList: function () {
        $('#tab-wll-errMsg').html('');
        $('#tab-wll-errSupers').html('');
        var sourceSuper = $("#tab-wll-sourcesuper").val();
        if (sourceSuper === "") {
            showAlertMessage('tab-wll-errMsg', "Please input source super.", 1);
            return;
        }

        var superList = $("#tab-wll-superlist").val();
        if (superList === "") {
            showAlertMessage('tab-wll-errMsg', "Please input destination super.", 1);
            return;
        }

        if (superList.split(',').length > 500) {
            showAlertMessage('tab-wll-errMsg', "There're more than 500 Usernames.", 1);
            parent.jQuery.unblockUI();
            return;
        }

        var request = {
            SourceSuper: sourceSuper,
            SuperList: superList,
            ProductList: '',
            IsCopy: true,
            Mode: 'WinLossLimit'
        };

        $("#tab-wll-superSetting").addClass("hidden");

        $.ajax({
            type: "POST",
            url: "CheckSuperList",
            contentType: "application/json",
            data: JSON.stringify(request),
            beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
            success: function (data) {
                copyWinLossLimit.settings = data;
                if (data.Result.ErrorCode === 0) {
                    copyWinLossLimit.enableDisableInput(true);
                    $("#tab-wll-superSetting").removeClass("hidden");
                    copyWinLossLimit.renderSports(data.SportTypes);
                } else {
                    showAlertMessage('tab-wll-errMsg', data.Result.ErrorMessage, 1);
                }
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("tab-wll-errMsg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
                else {
                    showAlertMessage('tab-wll-errMsg', 'Internal server error.', 1);
                }
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    },
    renderSports: function (sports) {
        var bodyRows = [];
        var rowCount = sports.length;
        for (var i = 0; i < rowCount; i++) {
            var row = { Columns: [] };
            row.Columns.push({ Name: 'Name', Value: sports[i].SportTypeName });
            row.Columns.push({
                Name: 'Select',
                Value: '<input type="checkbox" class="ckbSport" id="tab-wll-sportType' + sports[i].SportTypeId + '" onChange="copyWinLossLimit.changeSelectSport(' + i + ')" >'
            });
            bodyRows.push(row);
        }

        var gridOptions =
        {
            columns: [
                { HeaderText: "Maximun Win Loss Limit", Width: 400, Name: "Name", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Copy<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Select All\" class=\"btn-submit\" onclick=\"copyWinLossLimit.selectAllSports(1)\" /><input type=\"button\" value=\"Deselect All\" class=\"btn-reset\" onclick=\"copyWinLossLimit.selectAllSports(0)\" /> </div>", Width: 150, Name: "Select", HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: bodyRows,
            showDistinctRow: true,
            gridExpandHeight: 0
        };

        $('#tab-wll-settings').fanexGrid(gridOptions);
    },
    applySetting: function () {
        $('#tab-wll-errMsg').html('');
        $('#tab-wll-errSupers').html('');
        var request = {
            SportTypes: copyWinLossLimit.settings.SportTypes.filter(sport => sport.Selected === 1),
            CustId: copyWinLossLimit.settings.CustId,
            SuperIds: copyWinLossLimit.settings.SuperListIds,
            SuperList: copyWinLossLimit.settings.SuperList,
            SourceUsername: $("#tab-wll-sourcesuper").val()
        };

        if (request.SportTypes === null || request.SportTypes.length === 0) {
            return;
        }

        if (confirm("Are you sure you want to copy the Maximum Win Loss Limit from the source super to the destination ones?")) {
            copyWinLossLimit.validateBeforUpdate(request);
        }
    },
    validateBeforUpdate: function (request) {
        var sourceSuper = $("#tab-wll-sourcesuper").val();
        var superList = $("#tab-wll-superlist").val();
        var productList = request.SportTypes.map(sport => sport.ProductId).join();
        var validateRequest = {
            SourceSuper: sourceSuper,
            SuperList: superList,
            ProductList: productList
        };
        $.ajax({
            type: "POST",
            url: "GetWinLossLimit",
            contentType: "application/json",
            data: JSON.stringify(validateRequest),
            beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
            success: function (data) {
                if (data.Result.ErrorCode === 0) {
                    copyWinLossLimit.callAPIUpdate(request);
                } else {
                    showAlertMessage('tab-wll-errMsg', data.Result.ErrorMessage, 1);
                    parent.jQuery.unblockUI();
                }
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("tab-wll-errMsg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
                else {
                    showAlertMessage('tab-wll-errMsg', 'Internal server error.', 1);
                }

                parent.jQuery.unblockUI();
            }
        });
    },
    callAPIUpdate: function (request) {
        $.ajax({
            type: "POST",
            url: "CopyWinLossLimit",
            contentType: "application/json",
            data: JSON.stringify(request),
            beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
            success: function (data) {
                copyWinLossLimit.showUpdateResultMessage(data);
            },
            error: function (err) {
                showAlertMessage('tab-wll-errMsg', 'Internal server error.', 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    },
    showUpdateResultMessage: function (result) {
        if (result.CopyErrors !== undefined && result.CopyErrors !== null && result.CopyErrors.length) {
            if (result.ErrorCode === 0) {
                showAlertMessage('tab-wll-errMsg', 'All accounts have been successfully copied except the following accounts:', 0);
            }
            var errorSupers = result.CopyErrors.map(rs => rs.ErrorMessage).join('<br />');
            showAlertMessage('tab-wll-errSupers', errorSupers, 1);
        } else {
            if (result.ErrorCode === 0) {
                showAlertMessage('tab-wll-errMsg', 'All accounts have been successfully copied.', 0);
            } else {
                showAlertMessage('tab-wll-errMsg', result.ErrorMessage, 1);
            }
        }
    },
    selectAllSports: function (value) {
        copyWinLossLimit.settings.SportTypes.forEach(type => type.Selected = value);
        $('#tab-wll-superSetting .ckbSport').prop('checked', value === 1);
    },
    changeSelectSport: function (index) {
        if ($('#tab-wll-sportType' + copyWinLossLimit.settings.SportTypes[index].SportTypeId).prop('checked')) {
            copyWinLossLimit.settings.SportTypes[index].Selected = 1;
        } else {
            copyWinLossLimit.settings.SportTypes[index].Selected = 0;
        }
    }
};