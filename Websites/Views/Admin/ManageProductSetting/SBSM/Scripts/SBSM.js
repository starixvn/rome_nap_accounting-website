var cachedSBSMData;
var isNeedUpdateSportTypes = false;
var isNeedUpdateBetTypes = false;

function SearchSBSM(pageSize, pageIndex) {
    parent.BlockUI();
    $.ajax({
        url: 'SelectSBSM',
        type: 'POST',
        dataType: 'json',
        data: {
            ProductId: $('#ddlSBSMProductId').multiSelect("getCheckedItems").join(),
            SportTypes: $('#ddlSBSMSportType').multiSelect("getCheckedItems").join(),
            BetTypes: $('#ddlSBSMBetType').multiSelect("getCheckedItems").join(),
            IsLicensee: $('#ckbFilterIsLicensee').prop('checked') ? true : false,
            IsLive: $('#ckbFilterIsLive').prop('checked') ? true : false,
            PageSize: pageSize,
            PageIndex: pageIndex
        },
        success: function (data) {
            cachedSBSMData = data;
            RenderSBSMData(data, pageSize, pageIndex);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });
}

function RenderSBSMData(data, pageSize, pageIndex) {
    var bodyRows = [];
    const rowCount = data.SbsmEntities.length;

    for (let i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportType', Value: '[' + data.SbsmEntities[i].SportType + '] ' + data.SbsmEntities[i].SportTypeName });
        row.Columns.push({ Name: 'BetType', Value: '[' + data.SbsmEntities[i].BetType + '] ' + data.SbsmEntities[i].BetTypeName});
        row.Columns.push({ Name: 'IsLicensee', Value: data.SbsmEntities[i].IsLicensee });
        row.Columns.push({ Name: 'IsLive', Value: data.SbsmEntities[i].IsLive });
        row.Columns.push({ Name: 'Site', Value: data.SbsmEntities[i].Site });
        row.Columns.push({ Name: 'SportTypeRefPT', Value: data.SbsmEntities[i].SportTypeRefPT });
        row.Columns.push({ Name: 'BetTypeRefPT', Value: data.SbsmEntities[i].BetTypeRefPT });
        row.Columns.push({ Name: 'SportTypeRefComm', Value: data.SbsmEntities[i].SportTypeRefComm });
        row.Columns.push({ Name: 'BetTypeRefComm', Value: data.SbsmEntities[i].BetTypeRefComm });
        row.Columns.push({ Name: 'SportTypeRefBetSetting', Value: data.SbsmEntities[i].SportTypeRefBetSetting });
        row.Columns.push({ Name: 'BetTypeRefBetSetting', Value: data.SbsmEntities[i].BetTypeRefBetSetting });
        row.Columns.push({ Name: 'TableBettrans', Value: data.SbsmEntities[i].TableBettrans });
        row.Columns.push({ Name: 'SportTypeRefWinlossLimit', Value: data.SbsmEntities[i].SportTypeRefWinlossLimit });
        row.Columns.push({ Name: 'BetTypeRefWinlossLimit', Value: data.SbsmEntities[i].BetTypeRefWinlossLimit });
        row.Columns.push({ Name: 'Product', Value: '[' + data.SbsmEntities[i].ProductId + '] ' + data.SbsmEntities[i].ProductName });
        row.Columns.push({ Name: 'Edit', Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditSBSM(' + i + ')" >' });
        row.Columns.push({
            Name: 'Delete',
            Value: '<input type="checkbox" class="ckbDeleteSBSM" data-pos="' + i + '" />'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "SportType", Width: 100, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType", Width: 100, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Is Licensee", Width: 100, Name: "IsLicensee", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "IsLive", Width: 100, Name: "IsLive", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType RefPT", Width: 100, Name: "SportTypeRefPT", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType RefPT", Width: 100, Name: "BetTypeRefPT", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType RefComm", Width: 100, Name: "SportTypeRefComm", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType RefComm", Width: 100, Name: "BetTypeRefComm", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType RefBetSetting", Width: 100, Name: "SportTypeRefBetSetting", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType RefBetSetting", Width: 100, Name: "BetTypeRefBetSetting", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Table Bettrans", Width: 100, Name: "TableBettrans", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SportType RefWinlossLimit", Width: 100, Name: "SportTypeRefWinlossLimit", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BetType RefWinlossLimit", Width: 100, Name: "BetTypeRefWinlossLimit", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Product", Width: 100, Name: "Product", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Edit", Width: 50, Name: "Edit", HeaderAlign: "center", CellAlign: "center" },
                {
                    HeaderText: 'Delete <br/> <img class="edit-entity" title="Delete All" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteAllSBSMSetting()" >' +
                        '&nbsp;&nbsp;<input type="checkbox" id="ckbDeleteAll" onclick="checkAllRow(this)" />', Width: 50, Name: "Delete", HeaderAlign: "center", CellAlign: "center" }
            ],
            bodyRows: bodyRows,
            gridExpandHeight: 200,
            showPager: true,
            customPager: true,
            showDistinctRow: false,
            pagerOption: {
                itemsPerPage: pageSize,
                currentPage: pageIndex,
                itemsPerPageArray: [50, 100, 150, 200],
                pagerSelectCallBack: reRenderSBSMAfterPaged,
                totalItem: data.TotalRows
            }
        };

    $('#gridSBSMId').fanexGrid(gridOptions);
}

function reRenderSBSMAfterPaged(currentPageIndex, itemsPerPage) {
    parent.BlockUI();
    SearchSBSM(itemsPerPage, currentPageIndex);
}


function checkAllRow(checkboxall) {
    $('#gridSBSMId').find('.ckbDeleteSBSM').prop('checked', checkboxall.checked);
}

function DeleteAllSBSMSetting() {
    var request = [];
    var checkboxs = $('#gridSBSMId').find('.ckbDeleteSBSM');
    checkboxs.each(function (index, option) {
        if ($(option).prop("checked")) {
            request.push(cachedSBSMData.SbsmEntities[$(option).data("pos")]);
        }
    });
    if (request.length > 0) {
        if (confirm('Are you sure you want to delete these rows?')) {
            CallDeleteRequest(request);
        }
    }
}

function CallDeleteRequest(request) {
    parent.BlockUI();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        url: 'DeleteSBSM',
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify(request),
        success: function (result) {
            if (result.ErrorCode === 0) {
                alert('Deleted successfully.');
                SearchSBSM(50, 0);
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

function EditSBSM(index) {
    $('#updateSBSMErrMsg').html('');
    $('#SBSMProductId').val(cachedSBSMData.SbsmEntities[index].ProductId).trigger('change');
    $('#SBSMSportType').val(cachedSBSMData.SbsmEntities[index].SportType);
    $('#SBSMBetType').val(cachedSBSMData.SbsmEntities[index].BetType);
    $('#SBSMIsLive').prop('checked', cachedSBSMData.SbsmEntities[index].IsLive === 'True' ? true : false);
    $('#SBSMIsLicensee').prop('checked', cachedSBSMData.SbsmEntities[index].IsLicensee === 'True' ? true : false);
    $('#SBSMSite').val(cachedSBSMData.SbsmEntities[index].Site);
    $('#SBSMSportTypeRefPT').val(cachedSBSMData.SbsmEntities[index].SportTypeRefPT).trigger('change');
    $('#SBSMBetTypeRefPT').val(cachedSBSMData.SbsmEntities[index].BetTypeRefPT);
    $('#SBSMTablePT').val(cachedSBSMData.SbsmEntities[index].TablePT).trigger('change');
    $('#SBSMIsLivePt').prop('checked', cachedSBSMData.SbsmEntities[index].IsLivePt === 'True' ? true : false);
    $('#SBSMSportTypeRefBetSetting').val(cachedSBSMData.SbsmEntities[index].SportTypeRefBetSetting);
    $('#SBSMBetTypeRefBetSetting').val(cachedSBSMData.SbsmEntities[index].BetTypeRefBetSetting);
    $('#SBSMTableBetSetting').val(cachedSBSMData.SbsmEntities[index].TableBetSetting).trigger('change');
    $('#SBSMSportTypeRefComm').val(cachedSBSMData.SbsmEntities[index].SportTypeRefComm);
    $('#SBSMBetTypeRefComm').val(cachedSBSMData.SbsmEntities[index].BetTypeRefComm);
    $('#SBSMTableComm').val(cachedSBSMData.SbsmEntities[index].TableComm).trigger('change');
    $('#SBSMSportTypeRefWinlossLimit').val(cachedSBSMData.SbsmEntities[index].SportTypeRefWinlossLimit);
    $('#SBSMBetTypeRefWinlossLimit').val(cachedSBSMData.SbsmEntities[index].BetTypeRefWinlossLimit);
    $('#SBSMTableBettrans').val(cachedSBSMData.SbsmEntities[index].TableBettrans).trigger('change');
    $('#btnSBSMUpdate').off();
    $('#btnSBSMUpdate').click(function () { return UpdateSBSM(index); });
    $('#edit-SBSM-form').dialog({
        title: 'Edit Sport Bettype Setting Matrix',
        modal: true,
        width: 1000,
        height: 'auto'
    });
}

function UpdateSBSM(index) {
    parent.BlockUI();
    var request = cachedSBSMData.SbsmEntities[index];
    request.ProductId = $('#SBSMProductId').val();
    request.SportTypeRefPT = $('#SBSMSportTypeRefPT').val();
    request.BetTypeRefPT = $('#SBSMBetTypeRefPT').val();
    request.IsLivePt = $('#SBSMIsLivePT').is(":checked") ? 1 : 0;
    request.TablePT = $('#SBSMTablePT').val();
    request.SportTypeRefBetSetting = $('#SBSMSportTypeRefBetSetting').val();
    request.BetTypeRefBetSetting = $('#SBSMBetTypeRefBetSetting').val();
    request.TableBetSetting = $('#SBSMTableBetSetting').val();
    request.SportTypeRefComm = $('#SBSMSportTypeRefComm').val();
    request.BetTypeRefComm = $('#SBSMBetTypeRefComm').val();
    request.TableComm = $('#SBSMTableComm').val();
    request.SportTypeRefWinlossLimit = $('#SBSMSportTypeRefWinlossLimit').val();
    request.BetTypeRefWinlossLimit = $('#SBSMBetTypeRefWinlossLimit').val();
    request.TableBettrans = $('#SBSMTableBettrans').val();
    $.ajax({
        url: 'UpdateSBSM',
        type: 'POST',
        dataType: 'json',
        data: request,
        success: function (result) {
            showAlertMessage('updateSBSMErrMsg', result.ErrorCode === 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode === 0 ? 0 : 1);
            if (result.ErrorCode === 0) {
                setTimeout(function () {
                    $('#edit-SBSM-form').dialog('close');
                    SearchSBSM(50, 0);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateSBSMErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderProductDdl() {
    var multiselectData = [];
    $('#ddlSBSMProductId > option').each(function () {
        var $this = $(this);
        multiselectData.push({
            Id: "filterProduct_" + $this.attr('value'),
            Value: $this.attr('value'),
            Text: $this.html(),
            Checked: true,
            Enable: true,
            CssClass: "",
            Items: []
        });
    });


    $('#ddlSBSMProductId').multiSelect({
        minWidth: 180,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: true,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "No item selected.",
        onCheck: function () {
            isNeedUpdateSportTypes = true;
        },
        onLeave: function () {
            if (isNeedUpdateSportTypes) {
                isNeedUpdateSportTypes = false;
                GetSportType();
            }
        }
    });
    
    GetSportType();
}

function GetSportType() {
    parent.BlockUI();
    $.ajax({
        url: 'GetSportType',
        type: 'POST',
        dataType: 'json',
        data: {
            ProductIds: $('#ddlSBSMProductId').multiSelect("getCheckedItems").join()
        },
        success: function (result) {
            var multiselectData = [];
            result.forEach(function (item) {
                multiselectData.push({
                    Id: "filterSport_" + item.Value,
                    Value: item.Value,
                    Text: item.Text,
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: []
                });
            });

            $('#ddlSBSMSportType').multiSelect({
                minWidth: 180,
                maxHeight: 450,
                shadow: true,
                showHeader: true,
                isCheckAll: true,
                showFooter: false,
                showFilter: true,
                checkAllText: "All",
                listData: multiselectData,
                noItemText: "No item selected.",
                onCheck: function () {
                    isNeedUpdateBetTypes = true;
                },
                onLeave: function () {
                    if (isNeedUpdateBetTypes) {
                        isNeedUpdateBetTypes = false;
                        GetBetType();
                    }
                }
            });

            GetBetType();
        },
        error: function (xhr) {
            showAlertMessage('updateSBSMErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function GetBetType() {
    parent.BlockUI();
    $.ajax({
        url: 'GetBetTypes',
        type: 'POST',
        dataType: 'json',
        data: {
            ProductIds: $('#ddlSBSMProductId').multiSelect("getCheckedItems").join(),
            SportTypes: $('#ddlSBSMSportType').multiSelect("getCheckedItems").join()
        },
        success: function (result) {
            var multiselectData = [];
            result.forEach(function (item) {
                multiselectData.push({
                    Id: "filterBetType_" + item.Value,
                    Value: item.Value,
                    Text: item.Text,
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: []
                });
            });

            $('#ddlSBSMBetType').multiSelect({
                minWidth: 180,
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
        },
        error: function (xhr) {
            showAlertMessage('updateSBSMErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

$(document).ready(function () {
    RenderProductDdl();
    $("#SBSMProductId").select2({ width: "185px" });
    $("#SBSMTablePT").select2({ width: "185px" });
    $("#SBSMTableBetSetting").select2({ width: "185px" });
    $("#SBSMTableBettrans").select2({ width: "185px" });
    $("#SBSMTableComm").select2({ width: "185px" });
});