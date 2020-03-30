var cachedData;

function Search() {
    parent.BlockUI();
    $.ajax({
        url: 'Select',
        type: 'GET',
        data: {
            TypeId: $('#txtTypeId').val(),
            TypeName: $('#txtTypeNamee').val(),
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
    var rowCount = data.BettypeEntities.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'TypeID', Value: data.BettypeEntities[i].TypeId });
        row.Columns.push({ Name: 'TypeNamee', Value: data.BettypeEntities[i].TypeNamee });
        row.Columns.push({
            Name: 'Edit',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="Edit(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "TypeID", Width: 50, Name: "TypeID", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "TypeNamee", Width: 500, Name: "TypeNamee", HeaderAlign: "center", CellAlign: "center" },
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
    $('#TypeId').text(cachedData.BettypeEntities[index].TypeId);
    $('#TypeNamee').val(cachedData.BettypeEntities[index].TypeNamee);
    $('#TypeNamec').val(cachedData.BettypeEntities[index].TypeNamec);
    $('#TypeDesc').val(cachedData.BettypeEntities[index].TypeDesc);
    $('#TypeNamec_uni').val(cachedData.BettypeEntities[index].TypeNamec_uni);
    $('#TypeNamej').val(cachedData.BettypeEntities[index].TypeNamej);
    $('#TypeNamcn').val(cachedData.BettypeEntities[index].TypeNamcn);
    $('#TypeNamecn').val(cachedData.BettypeEntities[index].TypeNamecn);
    $('#TypeNameth').val(cachedData.BettypeEntities[index].TypeNameth);
    $('#TypeNameit').val(cachedData.BettypeEntities[index].TypeNameit);
    $('#TypeNameko').val(cachedData.BettypeEntities[index].TypeNameko);
    $('#TypeNameES').val(cachedData.BettypeEntities[index].TypeNameES);
    $('#TypeNameVN').val(cachedData.BettypeEntities[index].TypeNameVN);
    $('#TypeNameEL').val(cachedData.BettypeEntities[index].TypeNameEL);
    $('#TypeNamee_Eu').val(cachedData.BettypeEntities[index].TypeNamee_Eu);
    $('#TypeNameRU').val(cachedData.BettypeEntities[index].TypeNameRU);
    $('#TypeNameID').val(cachedData.BettypeEntities[index].TypeNameID);
    $('#TypeNamePT').val(cachedData.BettypeEntities[index].TypeNamePT);
    $('#TypeNameFR').val(cachedData.BettypeEntities[index].TypeNameFR);
    $('#ParentTypeid').val(cachedData.BettypeEntities[index].ParentTypeid);
    $('#TypeNameMMR').val(cachedData.BettypeEntities[index].TypeNameMMR);
    $('#TypeNameHI').val(cachedData.BettypeEntities[index].TypeNameHI);
    $('#DisplayedTypeID').val(cachedData.BettypeEntities[index].DisplayedTypeID);
    $('#edit-form').dialog({
        title: 'Edit Bettype',
        modal: true,
        width: 700,
        height: 700,
    });
}

function Update() {
    parent.BlockUI();
    $.ajax({
        url: 'Update',
        type: 'POST',
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
            showAlertMessage('updateErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-form').dialog('close');
                    Search();
                }, 3000);
            }
        },
        error: function (xhr) {
            if (xhr.status == 500)
                showAlertMessage('updateErrMsg', xhr.statusText, 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}