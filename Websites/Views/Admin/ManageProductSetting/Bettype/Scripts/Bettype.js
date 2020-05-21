var cachedBettypeData;

function SearchBettype(pageSize, pageIndex) {
    parent.BlockUI();
    $.ajax({
        url: 'SelectBettype',
        type: 'GET',
        data: {
            TypeId: $('#txtBettypeId').val(),
            TypeName: $('#txtBettypeNamee').val(),
            PageSize: pageSize,
            PageIndex: pageIndex
        },
        success: function (data) {
            cachedBettypeData = data;
            RenderBettypeData(data, pageSize, pageIndex);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderBettypeData(data, pageSize, pageIndex) {
    var bodyRows = [];
    var rowCount = data.BettypeEntities.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'TypeID', Value: data.BettypeEntities[i].TypeId });
        row.Columns.push({ Name: 'TypeNamee', Value: data.BettypeEntities[i].TypeNamee });
        row.Columns.push({
            Name: 'Edit',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditBettype(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "TypeID", Width: 50, Name: "TypeID", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "TypeNameE", Width: 500, Name: "TypeNamee", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Edit", Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center" }
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
                pagerSelectCallBack: reRenderAfterPaged,
                totalItem: data.TotalRows
            }
        };

    $('#gridBettypeId').fanexGrid(gridOptions);
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    parent.BlockUI();
    SearchBettype(itemsPerPage, currentPageIndex + 1);
}

function EditBettype(index) {
    $('#updateBettypeErrMsg').html('');
    $('#TypeId').text(cachedBettypeData.BettypeEntities[index].TypeId);
    $('#TypeNamee').val(cachedBettypeData.BettypeEntities[index].TypeNamee);
    $('#TypeNamec').val(cachedBettypeData.BettypeEntities[index].TypeNamec);
    $('#TypeDesc').val(cachedBettypeData.BettypeEntities[index].TypeDesc);
    $('#TypeNamec_uni').val(cachedBettypeData.BettypeEntities[index].TypeNamec_uni);
    $('#TypeNamej').val(cachedBettypeData.BettypeEntities[index].TypeNamej);
    $('#TypeNamcn').val(cachedBettypeData.BettypeEntities[index].TypeNamcn);
    $('#TypeNamecn').val(cachedBettypeData.BettypeEntities[index].TypeNamecn);
    $('#TypeNameth').val(cachedBettypeData.BettypeEntities[index].TypeNameth);
    $('#TypeNameit').val(cachedBettypeData.BettypeEntities[index].TypeNameit);
    $('#TypeNameko').val(cachedBettypeData.BettypeEntities[index].TypeNameko);
    $('#TypeNameES').val(cachedBettypeData.BettypeEntities[index].TypeNameES);
    $('#TypeNameVN').val(cachedBettypeData.BettypeEntities[index].TypeNameVN);
    $('#TypeNameEL').val(cachedBettypeData.BettypeEntities[index].TypeNameEL);
    $('#TypeNamee_Eu').val(cachedBettypeData.BettypeEntities[index].TypeNamee_Eu);
    $('#TypeNameRU').val(cachedBettypeData.BettypeEntities[index].TypeNameRU);
    $('#TypeNameID').val(cachedBettypeData.BettypeEntities[index].TypeNameID);
    $('#TypeNamePT').val(cachedBettypeData.BettypeEntities[index].TypeNamePT);
    $('#TypeNameFR').val(cachedBettypeData.BettypeEntities[index].TypeNameFR);
    $('#ParentTypeid').val(cachedBettypeData.BettypeEntities[index].ParentTypeid);
    $('#TypeNameMMR').val(cachedBettypeData.BettypeEntities[index].TypeNameMMR);
    $('#TypeNameHI').val(cachedBettypeData.BettypeEntities[index].TypeNameHI);
    $('#DisplayedTypeID').val(cachedBettypeData.BettypeEntities[index].DisplayedTypeID);
    $('#edit-bettype-form').dialog({
        title: 'Edit Bettype',
        modal: true,
        width: 700,
        height: 700,
    });
}

function UpdateBettype() {
    parent.BlockUI();
    $.ajax({
        url: 'UpdateBettype',
        type: 'POST',
        dataType: 'json',
        data: {
            TypeId: $('#TypeId').text(),
            TypeNamee: $('#TypeNamee').val(),
            TypeNamec: $('#TypeNamec').val(),
            TypeDesc: $('#TypeDesc').val(),
            TypeNamec_uni: $('#TypeNamec_uni').val(),
            TypeNamej: $('#TypeNamej').val(),
            TypeNamcn: $('#TypeNamcn').val(),
            TypeNamecn: $('#TypeNamecn').val(),
            TypeNameth: $('#TypeNameth').val(),
            TypeNameit: $('#TypeNameit').val(),
            TypeNameko: $('#TypeNameko').val(),
            TypeNameES: $('#TypeNameES').val(),
            TypeNameVN: $('#TypeNameVN').val(),
            TypeNameEL: $('#TypeNameEL').val(),
            TypeNamee_Eu: $('#TypeNamee_Eu').val(),
            TypeNameRU: $('#TypeNameRU').val(),
            TypeNameID: $('#TypeNameID').val(),
            TypeNamePT: $('#TypeNamePT').val(),
            TypeNameFR: $('#TypeNameFR').val(),
            ParentTypeid: $('#ParentTypeid').val(),
            TypeNameMMR: $('#TypeNameMMR').val(),
            TypeNameHI: $('#TypeNameHI').val(),
            DisplayedTypeID: $('#DisplayedTypeID ').val()
        },
        success: function (result) {
            showAlertMessage('updateBettypeErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-bettype-form').dialog('close');
                    SearchBettype(50, 1);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateBettypeErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}