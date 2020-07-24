var cachedCSSBMData;
var cachedSportTypeGroups;

function GetGroupsAndSearchCSSBM(pageSize, pageIndex) {
    GetSportTypeGroups();
    GetBetTypeGroups();
    SearchCSSBM(pageSize, pageIndex);
}

function SearchCSSBM(pageSize, pageIndex) {
    parent.BlockUI();
    $.ajax({
        url: 'SelectCSSportBettypeMatrix',
        type: 'GET',
        data: {
            SportType: $('#txtCSSBMSportType').val(),
            SportName: $('#txtCSSBMSportTypeName').val(),
            BetType: $('#txtCSSBMBetType').val(),
            BetTypeName: $('#txtCSSBMBetTypeName').val(),
            ProductId: $('#ddlCSSBMProductId').multiSelect("getCheckedItems").join(),
            EventType: $('#ddlCSSBMEventType').val(),
            RefTable: $('#ddlCSSBMRefTable').val(),
            PageSize: pageSize,
            PageIndex: pageIndex
        },
        success: function (data) {
            cachedCSSBMData = data;
            RenderCSSBMData(data, pageSize, pageIndex);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function RenderCSSBMData(data, pageSize, pageIndex) {
    var bodyRows = [];
    var rowCount = data.CSSportBettypeMatrixEntities.length;

    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'ProductId', Value: data.CSSportBettypeMatrixEntities[i].ProductId });
        row.Columns.push({ Name: 'ProductName', Value: data.CSSportBettypeMatrixEntities[i].ProductName });
        row.Columns.push({ Name: 'SportType', Value: data.CSSportBettypeMatrixEntities[i].SportType });
        row.Columns.push({ Name: 'SportTypeName', Value: data.CSSportBettypeMatrixEntities[i].SportTypeName });
        row.Columns.push({ Name: 'BetType', Value: data.CSSportBettypeMatrixEntities[i].BetType });
        row.Columns.push({ Name: 'BetTypeName', Value: data.CSSportBettypeMatrixEntities[i].BetTypeName });
        row.Columns.push({
            Name: 'IsLive',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.CSSportBettypeMatrixEntities[i].IsLive === 'False' ? '' : 'checked') + ' />'
        });
        row.Columns.push({
            Name: 'Enabled',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.CSSportBettypeMatrixEntities[i].Enabled === 'False' ? '' : 'checked') + ' />'
        });
        row.Columns.push({
            Name: 'IsTest',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.CSSportBettypeMatrixEntities[i].IsTest === 'False' ? '' : 'checked') + ' />'
        });
        row.Columns.push({ Name: 'EventType', Value: data.CSSportBettypeMatrixEntities[i].EventType });
        row.Columns.push({ Name: 'SubProduct', Value: data.CSSportBettypeMatrixEntities[i].SubProduct });
        row.Columns.push({ Name: 'RefTable', Value: data.CSSportBettypeMatrixEntities[i].RefTable });
        row.Columns.push({ Name: 'SportTypeGroup', Value: data.CSSportBettypeMatrixEntities[i].SportTypeGroupId });
        row.Columns.push({ Name: 'SportTypeGroupName', Value: data.CSSportBettypeMatrixEntities[i].SportTypeGroupName });
        row.Columns.push({ Name: 'BetTypeGroup', Value: data.CSSportBettypeMatrixEntities[i].BetTypeGroupId });
        row.Columns.push({ Name: 'BetTypeGroupName', Value: data.CSSportBettypeMatrixEntities[i].BetTypeGroupName });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditCSSBM(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteCSSBM(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "ProductId", Width: 50, Name: "ProductId", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "ProductName", Width: 100, Name: "ProductName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType", Width: 50, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType Name", Width: 100, Name: "SportTypeName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType", Width: 50, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType Name", Width: 100, Name: "BetTypeName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "IsLive", Width: 40, Name: "IsLive", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Enabled", Width: 50, Name: "Enabled", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "IsTest", Width: 50, Name: "IsTest", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "EventType", Width: 60, Name: "EventType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SubProduct", Width: 65, Name: "SubProduct", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "RefTable", Width: 50, Name: "RefTable", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType Group", Width: 60, Name: "SportTypeGroup", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType GroupName", Width: 100, Name: "SportTypeGroupName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType Group", Width: 50, Name: "BetTypeGroup", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType GroupName", Width: 100, Name: "BetTypeGroupName", HeaderAlign: "center", CellAlign: "center" },
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
                pagerSelectCallBack: reRenderCSSBMAfterPaged,
                totalItem: data.TotalRows
            }
        };

    $('#gridCSSBMId').fanexGrid(gridOptions);
}

function reRenderCSSBMAfterPaged(currentPageIndex, itemsPerPage) {
    parent.BlockUI();
    SearchCSSBM(itemsPerPage, currentPageIndex + 1);
}

function EditCSSBM(index) {
    $('#updateCSSBMErrMsg').html('');
    $('#CSSBMSportType').html(cachedCSSBMData.CSSportBettypeMatrixEntities[index].SportType);
    $('#CSSBMSportTypeName').html(cachedCSSBMData.CSSportBettypeMatrixEntities[index].SportTypeName);
    $('#CSSBMBetType').html(cachedCSSBMData.CSSportBettypeMatrixEntities[index].BetType);
    $('#CSSBMBetTypeName').html(cachedCSSBMData.CSSportBettypeMatrixEntities[index].BetTypeName);
    $('#CSSBMIsLive').prop('checked', cachedCSSBMData.CSSportBettypeMatrixEntities[index].IsLive === 'True' ? true : false);
    $('#CSSBMEnabled').prop('checked', cachedCSSBMData.CSSportBettypeMatrixEntities[index].Enabled === 'True' ? true : false);
    $('#CSSBMIsTest').prop('checked', cachedCSSBMData.CSSportBettypeMatrixEntities[index].IsTest === 'True' ? true : false);
    $('#CSSBMSubProduct').val(cachedCSSBMData.CSSportBettypeMatrixEntities[index].SubProduct);
    $('#btnCSSBMUpdate').off();
    $('#btnCSSBMUpdate').click(function () { return UpdateCSSBM(index); });
    $("#CSSBMProductId").val(cachedCSSBMData.CSSportBettypeMatrixEntities[index].ProductId).trigger('change');
    $("#CSSBMRefTable").val(cachedCSSBMData.CSSportBettypeMatrixEntities[index].RefTable).trigger('change');
    $("#CSSBMEventType").val(cachedCSSBMData.CSSportBettypeMatrixEntities[index].EventType).trigger('change');
    RenderSportTypeGroups(cachedCSSBMData.CSSportBettypeMatrixEntities[index].ProductId, cachedCSSBMData.CSSportBettypeMatrixEntities[index].SportTypeGroupId);
    RenderBetTypeGroups(cachedCSSBMData.CSSportBettypeMatrixEntities[index].BetTypeGroupId);

    $('#edit-cs-sbm-form').dialog({
        title: 'Edit CS_Sport Bettype Matrix',
        modal: true,
        width: 630,
        height: 'auto',
    });
}

function DeleteCSSBM(index) {
    if (confirm('Are you sure you want to delete this CS_SportBettypeMatrix?')) {
        $.ajax({
            url: 'DeleteCSSportBettypeMatrix',
            type: 'POST',
            dataType: 'json',
            data: {
                SportType: cachedCSSBMData.CSSportBettypeMatrixEntities[index].SportType,
                BetType: cachedCSSBMData.CSSportBettypeMatrixEntities[index].BetType,
                IsLive: cachedCSSBMData.CSSportBettypeMatrixEntities[index].IsLive
            },
            success: function (result) {
                if (result.ErrorCode == 0) {
                    alert('Deleted successfully.');
                    GetGroupsAndSearchCSSBM(50, 1);
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

function UpdateCSSBM(index) {
    parent.BlockUI();
    $.ajax({
        url: 'UpdateCSSportBettypeMatrix',
        type: 'POST',
        dataType: 'json',
        data: {
            ProductId: $('#CSSBMProductId').val(),
            SportType: cachedCSSBMData.CSSportBettypeMatrixEntities[index].SportType,
            BetType: cachedCSSBMData.CSSportBettypeMatrixEntities[index].BetType,
            IsLive: cachedCSSBMData.CSSportBettypeMatrixEntities[index].IsLive,
            Enabled: $('#CSSBMEnabled').is(":checked"),
            IsTest: $('#CSSBMIsTest').is(":checked"),
            EventType: $('#CSSBMEventType').val(),
            SubProduct: $('#CSSBMSubProduct').val(),
            RefTable: $('#CSSBMRefTable').val(),
            SportTypeGroupId: $('#CSSBMSportTypeGroup').val(),
            BetTypeGroupId: $('#CSSBMBetTypeGroup').val()
        },
        success: function (result) {
            showAlertMessage('updateCSSBMErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-cs-sbm-form').dialog('close');
                    GetGroupsAndSearchCSSBM(50, 1);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateCSSBMErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function GetSportTypeGroups(needRenderGroup) {
    $.ajax({
        url: 'SelectSportTypeGroups',
        type: 'GET',
        success: function (result) {
            cachedSportTypeGroups = result;
        },
        error: function (xhr) {
        }
    });
}

function RenderSportTypeGroups(productId, sportTypeGroupId) {
    var sportTypeGroupSelection = $("#CSSBMSportTypeGroup");
    sportTypeGroupSelection.empty();
    if (cachedSportTypeGroups != null) {
        cachedSportTypeGroups.forEach(function (item, index) {
            if (item.ProductId === productId || (productId !== '1' && item.ProductId === null)) {
                sportTypeGroupSelection.append(new Option('[' + item.SportTypeGroupId + '] ' + item.SportTypeGroupName, item.SportTypeGroupId));
            }
        });
        $("#CSSBMSportTypeGroup").select2();
        $('#CSSBMSportTypeGroup').val(sportTypeGroupId).trigger('change');
    }
}

function GetBetTypeGroups() {
    $.ajax({
        url: 'SelectBetTypeGroups',
        type: 'GET',
        success: function (result) {
            var betTypeGroupSelection = $("#CSSBMBetTypeGroup");
            betTypeGroupSelection.empty();
            if (result != null) {
                result.forEach(function (item, index) {
                    betTypeGroupSelection.append(new Option('[' + item.BetTypeGroupId + '] ' + item.BetTypeGroupName, item.BetTypeGroupId));
                });

            }
        },
        error: function (xhr) {
        }
    });
}

function RenderBetTypeGroups(betTypeGroupId) {
    $("#CSSBMBetTypeGroup").select2();
    $('#CSSBMBetTypeGroup').val(betTypeGroupId).trigger('change');
}

$(document).ready(function () {
    RenderMultiSelect('ddlCSSBMProductId');
    $("#CSSBMProductId").select2();
    $("#CSSBMRefTable").select2();
    $("#CSSBMEventType").select2();
    $("#CSSBMProductId").change(function () {
        var productId = $('#CSSBMProductId').val();
        RenderSportTypeGroups(productId);
    });
});