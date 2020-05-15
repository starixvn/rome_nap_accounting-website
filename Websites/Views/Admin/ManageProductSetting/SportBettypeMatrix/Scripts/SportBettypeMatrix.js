var cachedSBMData;

function SearchSBM() {
    parent.BlockUI();
    $.ajax({
        url: 'SelectSportBettypeMatrix',
        type: 'GET',
        data: {
            SportType: $('#txtSBMSportType').val(),
            BetType: $('#txtSBMBetType').val(),
            ProductId: $('#txtSBMProductId').val(),
            EventType: $('#txtSBMEventType').val(),
            RefTable: $('#txtSBMRefTable').val(),
            PageSize: $('#PageSBMSize').val(),
            PageIndex: $('#PageSBMIndex').val()
        },
        success: function (data) {
            cachedSBMData = data;
            RenderSBMData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function RenderSBMData(data) {
    var bodyRows = [];
    var rowCount = data.SportBettypeMatrixEntities.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportType', Value: data.SportBettypeMatrixEntities[i].SportType });
        row.Columns.push({ Name: 'BetType', Value: data.SportBettypeMatrixEntities[i].BetType });
        row.Columns.push({ Name: 'IsLive', Value: data.SportBettypeMatrixEntities[i].IsLive });
        row.Columns.push({ Name: 'ProductId', Value: data.SportBettypeMatrixEntities[i].ProductId });
        row.Columns.push({ Name: 'Enabled', Value: data.SportBettypeMatrixEntities[i].Enabled });
        row.Columns.push({ Name: 'IsTest', Value: data.SportBettypeMatrixEntities[i].IsTest });
        row.Columns.push({ Name: 'EventType', Value: data.SportBettypeMatrixEntities[i].EventType });
        row.Columns.push({ Name: 'SubProduct', Value: data.SportBettypeMatrixEntities[i].SubProduct });
        row.Columns.push({ Name: 'RefTable', Value: data.SportBettypeMatrixEntities[i].RefTable });
        row.Columns.push({
            Name: 'Edit',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditSBM(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "SportType", Width: 50, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BetType", Width: 50, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "IsLive", Width: 50, Name: "IsLive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductId", Width: 50, Name: "ProductId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Enabled", Width: 50, Name: "Enabled", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "IsTest", Width: 50, Name: "IsTest", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "EventType", Width: 50, Name: "EventType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SubProduct", Width: 50, Name: "SubProduct", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "RefTable", Width: 50, Name: "RefTable", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Edit", Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showPager: true,
        customPager: true,
        showDistinctRow: false,
        pagerOption: {
            itemsPerPage: parseInt($('#PageSBMSize').val()),
            currentPage: parseInt($('#PageSBMIndex').val()) - 1,
            itemsPerPageArray: [50, 100, 150, 200],
            pagerSelectCallBack: reRenderSBMAfterPaged,
            totalItem: data.TotalRows
        }
    };

    $('#gridSBMId').fanexGrid(gridOptions);
}

function reRenderSBMAfterPaged(currentPageIndex, itemsPerPage) {
    $('#PageSBMSize').val(itemsPerPage);
    $('#PageSBMIndex').val(currentPageIndex + 1);
    parent.BlockUI();
    SearchSBM();
}

function EditSBM(index) {
    $('#updateSBMErrMsg').html('');
    $('#SBMProductId').text(cachedSBMData.SportBettypeMatrixEntities[index].ProductId);
    $('#SBMSportType').val(cachedSBMData.SportBettypeMatrixEntities[index].SportType);
    $('#SBMBetType').val(cachedSBMData.SportBettypeMatrixEntities[index].BetType);
    $('#SBMIsLive').val(cachedSBMData.SportBettypeMatrixEntities[index].IsLive);
    $('#SBMEnabled').val(cachedSBMData.SportBettypeMatrixEntities[index].Enabled);
    $('#SBMIsTest').val(cachedSBMData.SportBettypeMatrixEntities[index].IsTest);
    $('#SBMEventType').val(cachedSBMData.SportBettypeMatrixEntities[index].EventType);
    $('#SBMSubProduct').val(cachedSBMData.SportBettypeMatrixEntities[index].SubProduct);
    $('#SBMRefTable').val(cachedSBMData.SportBettypeMatrixEntities[index].RefTable);
    $('#edit-sbm-form').dialog({
        title: 'Edit Sport Bettype Matrix',
        modal: true,
        width: 700,
        height: 400,
    });
}

function UpdateSBM() {
    parent.BlockUI();
    $.ajax({
        url: 'UpdateSportBettypeMatrix',
        type: 'POST',
        dataType: 'json',
        data: {
            ProductId: $('#SBMProductId').text(),
            SportType: $('#SBMSportType').val(),
            BetType: $('#SBMBetType').val(),
            IsLive: $('#SBMIsLive').val(),
            Enabled: $('#SBMEnabled').val(),
            IsTest: $('#SBMIsTest').val(),
            EventType: $('#SBMEventType').val(),
            SubProduct: $('#SBMSubProduct').val(),
            RefTable: $('#SBMRefTable').val()
        },
        success: function (result) {
            showAlertMessage('updateSBMErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-sbm-form').dialog('close');
                    SearchSBM();
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateSBMErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}