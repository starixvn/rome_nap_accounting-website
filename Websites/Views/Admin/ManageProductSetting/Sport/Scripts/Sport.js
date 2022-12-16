var cachedSportData;

function SearchSport(pageSize, pageIndex) {
    parent.BlockUI();
    $.ajax({
        url: 'SelectSport',
        type: 'GET',
        data: {
            SportType: $('#txtSportType').val(),
            SportName: $('#txtSportName').val(),
            PageSize: pageSize,
            PageIndex: pageIndex
        },
        success: function (data) {
            cachedSportData = data;
            RenderSportData(data, pageSize, pageIndex);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });
}

function RenderSportData(data, pageSize, pageIndex) {
    var bodyRows = [];
    var rowCount = data.SportTypeEntities.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportType', Value: data.SportTypeEntities[i].SportType });
        row.Columns.push({ Name: 'SportName', Value: data.SportTypeEntities[i].SportName });
        row.Columns.push({
            Name: 'Edit',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditSport(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "Sport Type", Width: 100, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Sport Name", Width: 500, Name: "SportName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Edit", Width: 100, Name: "Edit", HeaderAlign: "center", CellAlign: "center" }
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
                pagerSelectCallBack: reRenderSportAfterPaged,
                totalItem: data.TotalRows
            }
        };

    $('#gridSportId').fanexGrid(gridOptions);
}

function reRenderSportAfterPaged(currentPageIndex, itemsPerPage) {
    parent.BlockUI();
    SearchSport(itemsPerPage, currentPageIndex + 1);
}

function EditSport(index) {
    $('#updateSportErrMsg').html('');
    $('#SportType').text(cachedSportData.SportTypeEntities[index].SportType);
    $('#SportName').val(cachedSportData.SportTypeEntities[index].SportName);
    $('#SportNameCh').val(cachedSportData.SportTypeEntities[index].SportNameCh);
    $('#SportNameCs').val(cachedSportData.SportTypeEntities[index].SportNameCs);
    $('#SportNameJp').val(cachedSportData.SportTypeEntities[index].SportNameJp);
    $('#SportNameIt').val(cachedSportData.SportTypeEntities[index].SportNameIt);
    $('#SportNameTh').val(cachedSportData.SportTypeEntities[index].SportNameTh);
    $('#ParlayMaxPayout').val(cachedSportData.SportTypeEntities[index].ParlayMaxPayout);
    $('#SportNameKo').val(cachedSportData.SportTypeEntities[index].SportNameKo);
    $('#SportNameES').val(cachedSportData.SportTypeEntities[index].SportNameES);
    $('#SportNameVN').val(cachedSportData.SportTypeEntities[index].SportNameVN);
    $('#SportNameEL').val(cachedSportData.SportTypeEntities[index].SportNameEL);
    $('#SportNameRU').val(cachedSportData.SportTypeEntities[index].SportNameRU);
    $('#SportNameID').val(cachedSportData.SportTypeEntities[index].SportNameID);
    $('#SportNamePT').val(cachedSportData.SportTypeEntities[index].SportNamePT);
    $('#SportNameFR').val(cachedSportData.SportTypeEntities[index].SportNameFR);
    $('#SportNameMMR').val(cachedSportData.SportTypeEntities[index].SportNameMMR);
    $('#SportNamePL').val(cachedSportData.SportTypeEntities[index].SportNamePL);
    $('#SportNameHI').val(cachedSportData.SportTypeEntities[index].SportNameHI);
    $('#edit-sport-form').dialog({
        title: 'Edit Sport',
        modal: true,
        width: 'auto',
        height: 600
    });
}

function UpdateSport() {
    parent.BlockUI();
    $.ajax({
        url: 'UpdateSport',
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
            showAlertMessage('updateSportErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-sport-form').dialog('close');
                    SearchSport(50, 1);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateSportErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}