$(document).ready(function () {
    $('#listPlayer').tokenInput("", {
        theme: "facebook",
        allowFreeTagging: true,
        hintText: "",
        noResultsText: "",
        searchingText: "",
        tokenLimit: 500
    });

    $('#searchByIDs').click(function () {
        $('#searchByIdSample').removeClass('disabled');
        $('#searchByUsernameSample').addClass('disabled');
    });

    $('#searchByUsername').click(function () {
        $('#searchByIdSample').addClass('disabled');
        $('#searchByUsernameSample').removeClass('disabled');
    });

    $('#btnSubmit').click(function () {
        $('#errMsg').html('');
        parent.BlockUI();
        var params = {
            ProductID: $('#cmbProduct').find('option:selected').val(),
            PlayerIDs: null,
            Usernames: null
        };

        var listPlayer = '';
        $("#trListPlayer").find("input").each(function (i) {
            var val = $(this).val();
            if ($.trim(val) != '' && val.indexOf('N/A') == -1)
                listPlayer += $(this).val() + ',';
        });

        if (listPlayer == '') {
            if ($("input[name='optSearchType']:checked").val() == 0) {
                RenderErrMsg('PlayerID (3rd) is required');
            } else {
                RenderErrMsg('Username is required');
            }

            parent.jQuery.unblockUI();
            return;
        }

        if ($("input[name='optSearchType']:checked").val() == 0) {
            params.PlayerIDs = listPlayer;
        } else {
            params.Usernames = listPlayer;
        }

        $.ajax({
            type: "POST",
            url: "Search",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.indexOf('class="alertboxerr"') > 0)
                {
                    RenderErrMsg(lang.AccessDenied);
                    return;
                }

                var bodyRows = [];
                var rowCount = result.length;
                for (var i = 0 ; i < rowCount; i++) {
                    var row = { Columns: [] };
                    row.Columns.push({ Name: 'RowNum', Value: i + 1 });
                    row.Columns.push({ Name: 'Username', Value: '<span class="b">' + result[i].MemberName + '</span><br />' + result[i].MemberId });
                    row.Columns.push({ Name: 'Agent', Value: '<span class="b">' + result[i].AgentName + '</span><br />' + result[i].AgentId });
                    row.Columns.push({ Name: 'Master', Value: '<span class="b">' + result[i].MasterName + '</span><br />' + result[i].MasterId });
                    row.Columns.push({ Name: 'Super', Value: '<span class="b">' + result[i].SuperName + '</span><br />' + result[i].SuperId });
                    row.Columns.push({ Name: 'PlayerID', Value: '<span class="b">' + (result[i].PlayerID3rd == '0' ? '-' : result[i].PlayerID3rd) + '</span>' });
                    bodyRows.push(row);
                }

                var gridOptions =
                {
                    columns: [
                        { HeaderText: "#", Width: 40, Name: "RowNum", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Username", Width: 200, Name: "Username", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Agent", Width: 200, Name: "Agent", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Master", Width: 200, Name: "Master", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Super", Width: 200, Name: "Super", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "PlayerID (3rd)", Width: 150, Name: "PlayerID", HeaderAlign: "center", CellAlign: "center" }
                    ],
                    bodyRows: bodyRows,
                    gridExpandHeight: 200,
                    showPager: false,
                    showDistinctRow: false,
                };

                $('#gridContainer').show();
                $('#gridId').fanexGrid(gridOptions);
            },
            error: function () {
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    });
});

function RenderErrMsg(errMsg) {
    $('#errMsg').append('<div class="alertboxerr"><div>' + errMsg + '</div></div>');
}