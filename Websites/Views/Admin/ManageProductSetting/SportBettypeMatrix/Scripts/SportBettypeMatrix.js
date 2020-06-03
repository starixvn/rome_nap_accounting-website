var cachedSBMData;
function SearchSBM(pageSize, pageIndex) {
    parent.BlockUI();
    $.ajax({
        url: 'SelectSportBettypeMatrix',
        type: 'GET',
        data: {
            SportType: $('#txtSBMSportType').val(),
            SportName: $('#txtSBMSportTypeName').val(),
            BetType: $('#txtSBMBetType').val(),
            BetTypeName: $('#txtSBMBetTypeName').val(),
            ProductId: $('#ddlSBMProductId').multiSelect("getCheckedItems").join(),
            EventType: $('#ddlSBMEventType').val(),
            RefTable: $('#ddlSBMRefTable').val(),
            PageSize: pageSize,
            PageIndex: pageIndex
        },
        success: function (data) {
            cachedSBMData = data;
            RenderSBMData(data, pageSize, pageIndex);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function RenderSBMData(data, pageSize, pageIndex) {
    var bodyRows = [];
    var rowCount = data.SportBettypeMatrixEntities.length;

    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'ProductId', Value: data.SportBettypeMatrixEntities[i].ProductId });
        row.Columns.push({ Name: 'ProductName', Value: data.SportBettypeMatrixEntities[i].ProductName });
        row.Columns.push({ Name: 'SportType', Value: data.SportBettypeMatrixEntities[i].SportType });
        row.Columns.push({ Name: 'SportTypeName', Value: data.SportBettypeMatrixEntities[i].SportTypeName });
        row.Columns.push({ Name: 'BetType', Value: data.SportBettypeMatrixEntities[i].BetType });
        row.Columns.push({ Name: 'BetTypeName', Value: data.SportBettypeMatrixEntities[i].BetTypeName });
        row.Columns.push({
            Name: 'IsLive',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.SportBettypeMatrixEntities[i].IsLive === 'False' ? '' : 'checked') + ' />'
        });
        row.Columns.push({
            Name: 'Enabled',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.SportBettypeMatrixEntities[i].Enabled === 'False' ? '' : 'checked') + ' />'
        });
        row.Columns.push({
            Name: 'IsTest',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.SportBettypeMatrixEntities[i].IsTest === 'False' ? '' : 'checked') + ' />'
        });
        row.Columns.push({ Name: 'EventType', Value: data.SportBettypeMatrixEntities[i].EventType });
        row.Columns.push({ Name: 'SubProduct', Value: data.SportBettypeMatrixEntities[i].SubProduct });
        row.Columns.push({ Name: 'RefTable', Value: data.SportBettypeMatrixEntities[i].RefTable });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditSBM(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteSBM(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "ProductId", Width: 50, Name: "ProductId", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "ProductName", Width: 100, Name: "ProductName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType", Width: 50, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportTypeName", Width: 100, Name: "SportTypeName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType", Width: 50, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetTypeName", Width: 200, Name: "BetTypeName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "IsLive", Width: 50, Name: "IsLive", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Enabled", Width: 50, Name: "Enabled", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "IsTest", Width: 50, Name: "IsTest", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "EventType", Width: 50, Name: "EventType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SubProduct", Width: 150, Name: "SubProduct", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "RefTable", Width: 50, Name: "RefTable", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
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
                pagerSelectCallBack: reRenderSBMAfterPaged,
                totalItem: data.TotalRows
            }
        };

    $('#gridSBMId').fanexGrid(gridOptions);
}

function reRenderSBMAfterPaged(currentPageIndex, itemsPerPage) {
    parent.BlockUI();
    SearchSBM(itemsPerPage, currentPageIndex + 1);
}

function EditSBM(index) {
    $('#updateSBMErrMsg').html('');
    $('#SBMProductId').val(cachedSBMData.SportBettypeMatrixEntities[index].ProductId);
    $('#SBMSportType').html(cachedSBMData.SportBettypeMatrixEntities[index].SportType);
    $('#SBMSportTypeName').html(cachedSBMData.SportBettypeMatrixEntities[index].SportTypeName);
    $('#SBMBetType').html(cachedSBMData.SportBettypeMatrixEntities[index].BetType);
    $('#SBMBetTypeName').html(cachedSBMData.SportBettypeMatrixEntities[index].BetTypeName);
    $('#SBMIsLive').prop('checked', cachedSBMData.SportBettypeMatrixEntities[index].IsLive === 'True' ? true : false);
    $('#SBMEnabled').prop('checked', cachedSBMData.SportBettypeMatrixEntities[index].Enabled === 'True' ? true : false);
    $('#SBMIsTest').prop('checked', cachedSBMData.SportBettypeMatrixEntities[index].IsTest === 'True' ? true : false);
    $('#SBMEventType').val(cachedSBMData.SportBettypeMatrixEntities[index].EventType);
    $('#SBMSubProduct').val(cachedSBMData.SportBettypeMatrixEntities[index].SubProduct);
    $('#SBMRefTable').val(cachedSBMData.SportBettypeMatrixEntities[index].RefTable);
    $('#btnSBMUpdate').off();
    $('#btnSBMUpdate').click(function () { return UpdateSBM(index); });
    $('#edit-sbm-form').dialog({
        title: 'Edit Sport Bettype Matrix',
        modal: true,
        width: 600,
        height: 400,
    });
}

function DeleteSBM(index) {
    if (confirm('Are you sure you want to delete this SportBettypeMatrix ?')) {
        $.ajax({
            url: 'DeleteSportBettypeMatrix',
            type: 'POST',
            dataType: 'json',
            data: {
                SportType: cachedSBMData.SportBettypeMatrixEntities[index].SportType,
                BetType: cachedSBMData.SportBettypeMatrixEntities[index].BetType,
                IsLive: cachedSBMData.SportBettypeMatrixEntities[index].IsLive
            },
            success: function (result) {
                if (result.ErrorCode == 0) {
                    alert('Deleted successfully.');
                    SearchSBM(50, 1);
                } else {
                    alert(result.ErrorMessage);
                }
            },
            error: function (xhr) {
                alert('Internal Server Error.');
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    }
}

function UpdateSBM(index) {
    parent.BlockUI();
    $.ajax({
        url: 'UpdateSportBettypeMatrix',
        type: 'POST',
        dataType: 'json',
        data: {
            ProductId: $('#SBMProductId').val(),
            SportType: cachedSBMData.SportBettypeMatrixEntities[index].SportType,
            BetType: cachedSBMData.SportBettypeMatrixEntities[index].BetType,
            IsLive: cachedSBMData.SportBettypeMatrixEntities[index].IsLive,
            Enabled: $('#SBMEnabled').is(":checked"),
            IsTest: $('#SBMIsTest').is(":checked"),
            EventType: $('#SBMEventType').val(),
            SubProduct: $('#SBMSubProduct').val(),
            RefTable: $('#SBMRefTable').val()
        },
        success: function (result) {
            showAlertMessage('updateSBMErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-sbm-form').dialog('close');
                    SearchSBM(50, 1);
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

function RenderMultiSelect(id) {
    var multiselectData = [];
    var options = $('#' + id + ' > option');
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
        minWidth: 255,
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

$(document).ready(function () {
    RenderMultiSelect('ddlSBMProductId');
});