var filterVDData = {
    VendorId: "",
    VendorName: "",
    NumberValue: "",
    BetType: "",
    SportType: "",
    PageIndex: 1,
    PageSize: 50
};
var cachedVendor = [];

function SearchVendor(pageindex, pagesize) {
    parent.BlockUI();
    filterVDData.VendorId = $('#txtVendorID').val();
    filterVDData.VendorName = $('#txtVendorName').val();
    filterVDData.NumberValue = $('#txtNumberValue').val();
    filterVDData.BetType = $('#txtVDBetType').multiSelect("getCheckedItems").join();
    filterVDData.SportType = $('#txtVDSportType').multiSelect("getCheckedItems").join();
    filterVDData.PageIndex = pageindex;
    filterVDData.PageSize = pagesize;

    $.ajax({
        url: window.rootUrl + 'ManageVendorGlobalSetting/Select',
        type: 'POST',
        data: filterVDData,
        success: function (data) {
            cachedVendor = data.VendorGlobalEntities;
            RenderVendorData(data);
        },
        error: function (err) {
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderVendorData(data) {
    var bodyRows = [];

    for (var i = 0; i < data.VendorGlobalEntities.length; i++) {
        var vendor = data.VendorGlobalEntities[i];
        var row = { Columns: [], CssClass: "" };
        row.Columns.push({ Name: 'VendorId', Value: vendor.VendorId });
        row.Columns.push({ Name: 'VendorName', Value: vendor.VendorName });
        row.Columns.push({ Name: 'NumberValue', Value: vendor.NumberValue });
        row.Columns.push({ Name: 'SportType', Value: vendor.SportType });
        row.Columns.push({ Name: 'SportTypeName', Value: vendor.SportTypeName });
        row.Columns.push({ Name: 'BetType', Value: vendor.BetType });
        row.Columns.push({ Name: 'BetTypeName', Value: vendor.BetTypeName});
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditVendor(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteVendor(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "VendorId", Width: 50, Name: "VendorId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "VendorName", Width: 200, Name: "VendorName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "NumberValue", Width: 150, Name: "NumberValue", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SportType", Width: 50, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SportTypeName", Width: 200, Name: "SportTypeName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BetType", Width: 50, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BetTypeName", Width: 200, Name: "BetTypeName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showPager: true,
        customPager: true,
        showDistinctRow: false,
        pagerOption: {
            itemsPerPage: filterVDData.PageSize,
            currentPage: filterVDData.PageIndex - 1,
            itemsPerPageArray: [50, 100, 150, 200],
            pagerSelectCallBack: changeVendorPageHandler,
            totalItem: data.TotalRows
        }
    };

    $('#gridVDId').fanexGrid(gridOptions);
}

function changeVendorPageHandler(currentPageIndex, itemsPerPage) {
    SearchVendor(currentPageIndex + 1, itemsPerPage);
}

function EditVendor(index) {
    $('#updateVendorErrMsg').html('');
    $('#UpdVendorId').html(cachedVendor[index].VendorId);
    $('#UpdVendorName').html(cachedVendor[index].VendorName);
    $('#UpdVendorBetType').val(cachedVendor[index].BetType).trigger("change");
    $('#UpdVendorSportType').val(cachedVendor[index].SportType).trigger("change");
    $('#UpdVendorNumberValue').val(cachedVendor[index].NumberValue);

    $('#btnUpdVendor').off();
    $('#btnUpdVendor').click(function () { return UpdateVendor(index); });
    $('#edit-vendor-form').dialog({
        title: 'Update VendorGlobalSetting',
        modal: true,
        width: 'auto',
        height: 'auto'
    });
}

function UpdateVendor(index) {
    var vendor = { ...cachedVendor[index] };
    vendor.NumberValue = $('#UpdVendorNumberValue').val();
    vendor.SportType = $('#UpdVendorSportType').val();
    vendor.BetType = $('#UpdVendorBetType').val();
    parent.BlockUI();
    $.ajax({
        url: window.rootUrl + 'ManageVendorGlobalSetting/Update',
        type: 'POST',
        dataType: 'json',
        data: vendor,
        success: function (result) {
            showAlertMessage('updateVendorErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-vendor-form').dialog('close');
                    SearchVendor(1, 50);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateVendorErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function DeleteVendor(index) {
    const vendor = cachedVendor[index];
    if (confirm("Are you sure you want to delete this row?")) {
        parent.BlockUI();
        $.ajax({
            url: window.rootUrl + 'ManageVendorGlobalSetting/Delete',
            type: 'POST',
            dataType: 'json',
            data: vendor,
            success: function (result) {
                parent.jQuery.unblockUI();
                if (result.ErrorCode === 0) {
                    alert('Deleted successfully.');
                    SearchVendor(1, 50);
                } else {
                    alert(result.ErrorMessage);
                }
            },
            error: function (xhr) {
                parent.jQuery.unblockUI();
                alert('Internal Server Error.');
            }
        });
    }
}

function RenderSportTypes() {
    $.ajax({
        url: window.rootUrl + 'ManageProductSetting/SelectSport',
        type: 'GET',
        data: {
            SportType: null,
            SportName: null,
            PageSize: 0,
            PageIndex: 0
        },
        success: function (data) {
            RenderSportTypeOptions(data.SportTypeEntities);
        },
        error: function (err) {
        }
    });
}

function RenderSportTypeOptions(sportTypes) {
    $('#UpdVendorSportType').find('option').remove().end();

    var multiselectData = [];
    multiselectData.push({
        Id: 'txtVDSportTypeNull',
        Value: '',
        Text: 'Null',
        Checked: true,
        Enable: true,
        CssClass: "",
        Items: []
    });

    for (const sportType of sportTypes) {
        multiselectData.push({
            Id: 'txtVDSportType' + sportType.SportType,
            Value: sportType.SportType,
            Text: '[' + sportType.SportType + '] ' + sportType.SportName,
            Checked: true,
            Enable: true,
            CssClass: "",
            Items: []
        });

        const o = new Option('[' + sportType.SportType + '] ' + sportType.SportName, sportType.SportType);
        $(o).html('[' + sportType.SportType + '] ' + sportType.SportName);
        $("#UpdVendorSportType").append(o);
    }

    $('#txtVDSportType').multiSelect({
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

    $("#UpdVendorSportType").select2({
        minimumInputLength: 0,
        width: '200px'
    });
}

function RenderFilterBettype() {
    parent.BlockUI();
    $.ajax({
        url: window.rootUrl + 'ManageProductSetting/SelectBettype',
        type: 'GET',
        data: {
            TypeId: null,
            TypeNamee: null,
            PageSize: 0,
            PageIndex: 0
        },
        success: function (data) {
            RenderBettypeOptions(data.BettypeEntities);
        },
        error: function (err) {
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderBettypeOptions(bettypes) {
    $('#UpdVendorBetType').find('option').remove().end();

    var multiselectData = [];
    multiselectData.push({
        Id: 'txtVDBetTypeNull',
        Value: '',
        Text: 'Null',
        Checked: true,
        Enable: true,
        CssClass: "",
        Items: []
    });

    for (const bettype of bettypes) {
        multiselectData.push({
            Id: 'txtVDBetType' + bettype.TypeId,
            Value: bettype.TypeId,
            Text: '[' + bettype.TypeId + '] ' + bettype.TypeNamee,
            Checked: true,
            Enable: true,
            CssClass: "",
            Items: []
        });

        const o = new Option('[' + bettype.TypeId + '] ' + bettype.TypeNamee, bettype.TypeId);
        $(o).html('[' + bettype.TypeId + '] ' + bettype.TypeNamee);
        $("#UpdVendorBetType").append(o);
    }

    $('#txtVDBetType').multiSelect({
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

    $("#UpdVendorBetType").select2({
        minimumInputLength: 0,
        width: '200px'
    });
}

$(document).ready(function(){
    RenderSportTypes();
    RenderFilterBettype();
});