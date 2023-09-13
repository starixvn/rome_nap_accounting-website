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
    let rowCount = data.BettypeEntities.length;
    for (var i = 0; i < rowCount; i++) {
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
                { HeaderText: "TypeID", Width: 100, Name: "TypeID", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "TypeNameE", Width: 500, Name: "TypeNamee", HeaderAlign: "center", CellAlign: "center" },
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
    $('#TypeNamePTBR').val(cachedBettypeData.BettypeEntities[index].TypeNamePTBR);
    $('#TypeNameDE').val(cachedBettypeData.BettypeEntities[index].TypeNameDE);
    $('#TypeNameBENG').val(cachedBettypeData.BettypeEntities[index].TypeNameBENG);
    $('#TypeNameURD').val(cachedBettypeData.BettypeEntities[index].TypeNameURD);
    $('#TypeNameMSA').val(cachedBettypeData.BettypeEntities[index].TypeNameMSA);
    $('#TypeNameKH').val(cachedBettypeData.BettypeEntities[index].TypeNameKH);
    $('#TypeNameLAO').val(cachedBettypeData.BettypeEntities[index].TypeNameLAO);
    $('#TypeNameAR').val(cachedBettypeData.BettypeEntities[index].TypeNameAR);
    $('#TypeNameTAM').val(cachedBettypeData.BettypeEntities[index].TypeNameTAM);
    $('#DisplayedTypeID').val(cachedBettypeData.BettypeEntities[index].DisplayedTypeID);
    $('#LicSpreadBetTypeGroupId').val(cachedBettypeData.BettypeEntities[index].LicSpreadBetTypeGroupId);
    $('#Is3rdParty').prop('checked', cachedBettypeData.BettypeEntities[index].Is3rdParty === 'True' ? true : false);
    $('#edit-bettype-form').dialog({
        title: 'Edit Bettype',
        modal: true,
        width: 'auto',
        height: 580
    });
}

function UpdateBettype() {
    if ($('#LicSpreadBetTypeGroupId ').val() === '') {
        showAlertMessage('updateBettypeErrMsg', "LicSpreadBetTypeGroupId cannot be null.", 1);
        return;
    }

    parent.BlockUI();
    $.ajax({
        url: 'UpdateBettype',
        type: 'POST',
        dataType: 'json',
        data: {
            TypeId: $('#TypeId').text(),
            TypeNamee: $('#TypeNamee').val().escapeHTML(),
            TypeNamec: $('#TypeNamec').val().escapeHTML(),
            TypeDesc: $('#TypeDesc').val().escapeHTML(),
            TypeNamec_uni: $('#TypeNamec_uni').val().escapeHTML(),
            TypeNamej: $('#TypeNamej').val().escapeHTML(),
            TypeNamcn: $('#TypeNamcn').val().escapeHTML(),
            TypeNamecn: $('#TypeNamecn').val().escapeHTML(),
            TypeNameth: $('#TypeNameth').val().escapeHTML(),
            TypeNameit: $('#TypeNameit').val().escapeHTML(),
            TypeNameko: $('#TypeNameko').val().escapeHTML(),
            TypeNameES: $('#TypeNameES').val().escapeHTML(),
            TypeNameVN: $('#TypeNameVN').val().escapeHTML(),
            TypeNameEL: $('#TypeNameEL').val().escapeHTML(),
            TypeNamee_Eu: $('#TypeNamee_Eu').val().escapeHTML(),
            TypeNameRU: $('#TypeNameRU').val().escapeHTML(),
            TypeNameID: $('#TypeNameID').val(),
            TypeNamePT: $('#TypeNamePT').val().escapeHTML(),
            TypeNameFR: $('#TypeNameFR').val().escapeHTML(),
            ParentTypeid: $('#ParentTypeid').val(),
            TypeNameMMR: $('#TypeNameMMR').val().escapeHTML(),
            TypeNameHI: $('#TypeNameHI').val().escapeHTML(),
            DisplayedTypeID: $('#DisplayedTypeID ').val(),
            TypeNamePTBR: $('#TypeNamePTBR').val().escapeHTML(),
            TypeNameDE: $('#TypeNameDE').val().escapeHTML(),
            TypeNameBENG: $('#TypeNameBENG').val().escapeHTML(),
            TypeNameURD: $('#TypeNameURD').val().escapeHTML(),
            TypeNameMSA: $('#TypeNameMSA').val().escapeHTML(),
            TypeNameKH: $('#TypeNameKH').val().escapeHTML(),
            TypeNameLAO: $('#TypeNameLAO').val().escapeHTML(),
            TypeNameAR: $('#TypeNameAR').val().escapeHTML(),
            TypeNameTAM: $('#TypeNameTAM').val().escapeHTML(),
            LicSpreadBetTypeGroupId: $('#LicSpreadBetTypeGroupId ').val(),
            Is3rdParty: $('#Is3rdParty').is(":checked")
        },
        success: function (result) {
            showAlertMessage('updateBettypeErrMsg', result.ErrorCode === 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode === 0 ? 0 : 1);
            if (result.ErrorCode === 0) {
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