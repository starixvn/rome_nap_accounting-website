var settings = null;
function CheckSuper() {
    $('#errMsg').html('');
    $('#errSupers').html('');
    var sourceSuper = $("#sourcesuper").val();
    if (sourceSuper === "") {
        showAlertMessage('errMsg', "Please input source super.", 1);
        return;
    }

    var superList = $("#superlist").val();
    if (superList === "") {
        showAlertMessage('errMsg', "Please input destination super.", 1);
        return;
    }

    var request = {
        SourceSuper: sourceSuper,
        SuperList: superList
    };
    $("#superSetting").addClass("hidden");
    $.ajax({
        type: "POST",
        url: "CheckSuper",
        contentType: "application/json",
        data: JSON.stringify(request),
        beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
        success: function (data) {
            settings = data;
            if (data.Result.ErrorCode === 0) {
                EnableDisableInput(true);
                $("#superSetting").removeClass("hidden");
                RenderSports(data.SportTypes);
            } else {
                showAlertMessage('errMsg', data.Result.ErrorMessage, 1);
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

function RenderSports(sports) {
    var bodyRows = [];
    var rowCount = sports.length;
    for (var i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'Name', Value: sports[i].SportTypeName });
        row.Columns.push({
            Name: 'Select',
            Value: '<input type="checkbox" class="ckbSport" id="sportType' + sports[i].SportTypeId +'" onChange="UpdateSports(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Maximun Win Loss Limit", Width: 400, Name: "Name", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Copy<br /><div class=\"selectHeader\"><input type=\"button\" value=\"Select All\" class=\"btn\" onclick=\"SelectAllSports(1)\" /><input type=\"button\" value=\"Deselect All\" class=\"btn\" onclick=\"SelectAllSports(0)\" /> </div>", Width: 150, Name: "Select", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        showDistinctRow: true,
        gridExpandHeight: 0
    };

    $('#settings').fanexGrid(gridOptions);
}

function SelectAllSports(value) {
    settings.SportTypes.forEach(type => type.Selected = value);
    $('.ckbSport').prop('checked', value === 1);
}

function UpdateSports(index) {
    if ($('#sportType' + settings.SportTypes[index].SportTypeId).prop('checked')) {
        settings.SportTypes[index].Selected = 1;
    } else {
        settings.SportTypes[index].Selected = 0;
    }
}

function ApplySetting() {
    $('#errMsg').html('');
    $('#errSupers').html('');
    var request = {
        SportTypes: settings.SportTypes.filter(sport => sport.Selected === 1),
        CustId: settings.CustId,
        SuperIds: settings.SuperListIds,
        SuperList: settings.SuperList
    };

    if (request.SportTypes === null || request.SportTypes.length === 0) {
        return;
    }

    if (confirm("Are you sure you want to copy the Maximum Win Loss Limit from the source super to the destination ones?")) {
        ValidateBeforUpdate(request);
    }
    
}

function ShowUpdateResultMessage(result) {
    if (result.CopyErrors !== undefined && result.CopyErrors !== null && result.CopyErrors.length) {
        if (result.ErrorCode === 0) {
            showAlertMessage('errMsg', 'All accounts have been successfully copied except the following accounts:', 0);
        }
        var errorSupers = result.CopyErrors.map(rs => rs.ErrorMessage).join('<br />');
        showAlertMessage('errSupers', errorSupers, 1);
    } else {
        if (result.ErrorCode === 0) {
            showAlertMessage('errMsg', 'All accounts have been successfully copied.', 0);
        } else {
            showAlertMessage('errMsg', result.ErrorMessage, 1);
        }
    }
}

function ResetSelect() {
    settings = null;
    EnableDisableInput(false);
    $("#superlist").val('');
    $("#sourcesuper").val('');
    $("#superSetting").addClass("hidden");
    $('#settings').html('');
    $('#errMsg').html('');
    $('#errSupers').html('');
}

function EnableDisableInput(flag) {
    $("#sourcesuper").prop('disabled', flag);
    $("#superlist").prop('disabled', flag);
    $("#btnCheckSupers").prop('disabled', flag);
}

function ValidateBeforUpdate(request) {
    var sourceSuper = $("#sourcesuper").val();
    var superList = $("#superlist").val();
    var validateRequest = {
        SourceSuper: sourceSuper,
        SuperList: superList
    };
    $.ajax({
        type: "POST",
        url: "CheckSuper",
        contentType: "application/json",
        data: JSON.stringify(validateRequest),
        beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
        success: function (data) {
            if (data.Result.ErrorCode === 0) {
                CallAPIUpdate(request);
            } else {
                showAlertMessage('errMsg', data.Result.ErrorMessage, 1);
                parent.jQuery.unblockUI();
            }
        },
        error: function (err) {
            showAlertMessage('errMsg', 'Internal server error.', 1);
            parent.jQuery.unblockUI();
        }
    });
}

function CallAPIUpdate(request) {
    $.ajax({
        type: "POST",
        url: "CopyWinLossLimit",
        contentType: "application/json",
        data: JSON.stringify(request),
        beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
        success: function (data) {
            ShowUpdateResultMessage(data);
        },
        error: function (err) {
            showAlertMessage('errMsg', 'Internal server error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

$(document).ready(function () {
    $("#superlist").off("change").on("change", function () {
        var superlist = $("#superlist");
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
            superlist.val(superlistArray.join(","));
        }
    });
});