var cachedRefResourceData;

function SearchRefResource(pageSize, pageIndex) {
    parent.BlockUI();
    $.ajax({
        url: 'SelectRefResource',
        type: 'GET',
        data: {
            RefId: $('#txtRefId').val(),
            ResourceId: $('#txtResourceId').val(),
            ResourceEN: $('#txtResourceEN').val(),
            PageSize: pageSize,
            PageIndex: pageIndex
        },
        success: function (data) {
            cachedRefResourceData = data;
            RenderRefResourceData(data, pageSize, pageIndex);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function RenderRefResourceData(data, pageSize, pageIndex) {
    var bodyRows = [];
    var rowCount = data.RefResources.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'RefId', Value: data.RefResources[i].RefId });
        row.Columns.push({ Name: 'ResourceId', Value: data.RefResources[i].ResourceId });
        row.Columns.push({ Name: 'ResourceEN', Value: data.RefResources[i].ResourceEN });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditRefRes(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteRefRes(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "RefId", Width: 300, Name: "RefId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ResourceId", Width: 100, Name: "ResourceId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Resource_EN", Width: 400, Name: "ResourceEN", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 100, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
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
            pagerSelectCallBack: reRenderRefResAfterPaged,
            totalItem: data.TotalRows
        }
    };

    $('#gridRefResId').fanexGrid(gridOptions);
}

function reRenderRefResAfterPaged(currentPageIndex, itemsPerPage) {
    parent.BlockUI();
    SearchRefResource(itemsPerPage, currentPageIndex + 1);
}

function EditRefRes(index) {
    $('#updateRefResErrMsg').html('');
    $('#RefId').text(cachedRefResourceData.RefResources[index].RefId);
    $('#ResourceId').text(cachedRefResourceData.RefResources[index].ResourceId);
    $('#Resource_EN').val(cachedRefResourceData.RefResources[index].ResourceEN);
    $('#Resource_CH').val(cachedRefResourceData.RefResources[index].ResourceCH);
    $('#Resource_CS').val(cachedRefResourceData.RefResources[index].ResourceCS);
    $('#Resource_JP').val(cachedRefResourceData.RefResources[index].ResourceJP);
    $('#Resource_IT').val(cachedRefResourceData.RefResources[index].ResourceIT);
    $('#Resource_TH').val(cachedRefResourceData.RefResources[index].ResourceTH);
    $('#Resource_KO').val(cachedRefResourceData.RefResources[index].ResourceKO);
    $('#Resource_ES').val(cachedRefResourceData.RefResources[index].ResourceES);
    $('#Resource_EL').val(cachedRefResourceData.RefResources[index].ResourceEL);
    $('#Resource_VN').val(cachedRefResourceData.RefResources[index].ResourceVN);
    $('#Resource_PL').val(cachedRefResourceData.RefResources[index].ResourcePL);
    $('#Resource_ID').val(cachedRefResourceData.RefResources[index].ResourceID);
    $('#Resource_HI').val(cachedRefResourceData.RefResources[index].ResourceHI);
    $('#Resource_MM').val(cachedRefResourceData.RefResources[index].ResourceMM);
    $('#Resource_FR').val(cachedRefResourceData.RefResources[index].ResourceFR);
    $('#Resource_RU').val(cachedRefResourceData.RefResources[index].ResourceRU);
    $('#Resource_PT').val(cachedRefResourceData.RefResources[index].ResourcePT);
    $('#Resource_DE').val(cachedRefResourceData.RefResources[index].ResourceDE);
    $('#ResourceSort1').val(cachedRefResourceData.RefResources[index].Sort1);
    $('#ResourceSort2').val(cachedRefResourceData.RefResources[index].Sort2);
    $('#edit-refresource-form').dialog({
        title: 'Edit RefResource',
        modal: true,
        width: 'auto',
        height: 'auto',
    });
}

function UpdateRefResource() {
    parent.BlockUI();
    $.ajax({
        url: 'UpdateRefResource',
        type: 'POST',
        dataType: 'json',
        data: {
            RefId: $('#RefId').text(),
            Id: $('#ResourceId').text(),
            ResourceEN: $('#Resource_EN').val(),
            ResourceCH: $('#Resource_CH').val(),
            ResourceCS: $('#Resource_CS').val(),
            ResourceJP: $('#Resource_JP').val(),
            ResourceIT: $('#Resource_IT').val(),
            ResourceTH: $('#Resource_TH').val(),
            ResourceKO: $('#Resource_KO').val(),
            ResourceES: $('#Resource_ES').val(),
            ResourceEL: $('#Resource_EL').val(),
            ResourceVN: $('#Resource_VN').val(),
            ResourcePL: $('#Resource_PL').val(),
            ResourceID: $('#Resource_ID').val(),
            ResourceHI: $('#Resource_HI').val(),
            ResourceMM: $('#Resource_MM').val(),
            ResourceFR: $('#Resource_FR').val(),
            ResourceRU: $('#Resource_RU').val(),
            ResourcePT: $('#Resource_PT').val(),
            ResourceDE: $('#Resource_DE').val(),
            Sort1: $('#ResourceSort1').val(),
            Sort2: $('#ResourceSort2').val()
        },
        success: function (result) {
            showAlertMessage('updateRefResErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-refresource-form').dialog('close');
                    SearchRefResource(50, 1);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateRefResErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function DeleteRefRes(index) {
    if (confirm('Are you sure you want to delete this row?')) {
        $.ajax({
            url: 'DeleteRefResource',
            type: 'POST',
            dataType: 'json',
            data: {
                RefId: cachedRefResourceData.RefResources[index].RefId,
                ResourceId: cachedRefResourceData.RefResources[index].ResourceId
            },
            success: function (result) {
                if (result.ErrorCode == 0) {
                    showAlertMessage('errRefResMsg', 'Deleted successfully.', 0);
                    setTimeout(function () {
                        hideAlertMessage('errRefResMsg');
                        SearchRefResource(50, 1);
                    }, 3000);
                } else {
                    alert(result.ErrorMessage);
                }
            },
            error: function (xhr) {
                showAlertMessage('errRefResMsg', 'Internal Server Error.', 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    }
}