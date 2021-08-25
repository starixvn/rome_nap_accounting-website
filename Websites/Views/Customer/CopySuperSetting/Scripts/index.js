var settings = null;
function CheckSuper() {
    $('#errMsg').html('');
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
        row.Columns.push({ Name: 'Name', Value: sports[i].Name });
        row.Columns.push({
            Name: 'Select',
            Value: '<input type="checkbox" onChange="UpdateSports(' + i + ')" >'
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
        showDistinctRow: true
    };

    $('#settings').fanexGrid(gridOptions);
}