var cachedData;

function Search() {
    parent.BlockUI();
    $.ajax({
        url: 'Select',
        type: 'GET',
        data: {
            SportType: $('#txtSportType').val(),
            SportName: $('#txtSportName').val(),
            PageSize: $('#PageSize').val(),
            PageIndex: $('#PageIndex').val()
        },
        success: function (data) {
            cachedData = data;
            RenderData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderData(data) {
    var bodyRows = [];
    var rowCount = data.SportTypeEntities.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportType', Value: data.SportTypeEntities[i].SportType });
        row.Columns.push({ Name: 'SportName', Value: data.SportTypeEntities[i].SportName });
        row.Columns.push({
            Name: 'Edit',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="Edit(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "Sport Type", Width: 50, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Sport Name", Width: 300, Name: "SportName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Edit", Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: bodyRows,
            gridExpandHeight: 200,
            showPager: true,
            customPager: true,
            showDistinctRow: false,
            pagerOption: {
                itemsPerPage: parseInt($('#PageSize').val()),
                currentPage: parseInt($('#PageIndex').val()) - 1,
                itemsPerPageArray: [50, 100, 150, 200],
                pagerSelectCallBack: reRenderAfterPaged,
                totalItem: data.TotalRows
            }
        };

    $('#gridId').fanexGrid(gridOptions);
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    $('#PageSize').val(itemsPerPage);
    $('#PageIndex').val(currentPageIndex + 1);
    parent.BlockUI();
    Search();
}

function Edit(index) {
    $('#updateErrMsg').html('');
    $('#SportType').text(cachedData.SportTypeEntities[index].SportType);
    $('#SportName').val(cachedData.SportTypeEntities[index].SportName);
    $('#SportNameCh').val(cachedData.SportTypeEntities[index].SportNameCh);
    $('#SportNameCs').val(cachedData.SportTypeEntities[index].SportNameCs);
    $('#SportNameJp').val(cachedData.SportTypeEntities[index].SportNameJp);
    $('#SportNameIt').val(cachedData.SportTypeEntities[index].SportNameIt);
    $('#SportNameTh').val(cachedData.SportTypeEntities[index].SportNameTh);
    $('#ParlayMaxPayout').val(cachedData.SportTypeEntities[index].ParlayMaxPayout);
    $('#SportNameKo').val(cachedData.SportTypeEntities[index].SportNameKo);
    $('#SportNameES').val(cachedData.SportTypeEntities[index].SportNameES);
    $('#SportNameVN').val(cachedData.SportTypeEntities[index].SportNameVN);
    $('#SportNameEL').val(cachedData.SportTypeEntities[index].SportNameEL);
    $('#SportNameRU').val(cachedData.SportTypeEntities[index].SportNameRU);
    $('#SportNameID').val(cachedData.SportTypeEntities[index].SportNameID);
    $('#SportNamePT').val(cachedData.SportTypeEntities[index].SportNamePT);
    $('#SportNameFR').val(cachedData.SportTypeEntities[index].SportNameFR);
    $('#SportNameMMR').val(cachedData.SportTypeEntities[index].SportNameMMR);
    $('#SportNamePL').val(cachedData.SportTypeEntities[index].SportNamePL);
    $('#SportNameHI').val(cachedData.SportTypeEntities[index].SportNameHI);
    $('#edit-form').dialog({
        title: 'Edit Sport',
        modal: true,
        width: 700,
        height: 600,
    });
}

function Update() {
    parent.BlockUI();
    $.ajax({
        url: 'Update',
        type: 'POST',
        dataType: 'json',
        data: {
            SportType: $('#SportType').text(),
            SportName: $('#SportName').val(),
            SportNameCh: $('#SportNameCh').val(),
            SportNameCs: $('#SportNameCs').val(),
            SportNameJp: $('#SportNameJp').val(),
            SportNameIt: $('#SportNameIt').val(),
            SportNameTh: $('#SportNameTh').val(),
            ParlayMaxPayout: $('#ParlayMaxPayout').val(),
            SportNameKo: $('#SportNameKo').val(),
            SportNameES: $('#SportNameES').val(),
            SportNameVN: $('#SportNameVN').val(),
            SportNameEL: $('#SportNameEL').val(),
            SportNameRU: $('#SportNameRU').val(),
            SportNameID: $('#SportNameID').val(),
            SportNamePT: $('#SportNamePT').val(),
            SportNameFR: $('#SportNameFR').val(),
            SportNameMMR: $('#SportNameMMR').val(),
            SportNamePL: $('#SportNamePL').val(),
            SportNameHI: $('#SportNameHI').val()
        },
        success: function (result) {
            showAlertMessage('updateErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-form').dialog('close');
                    Search();
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}