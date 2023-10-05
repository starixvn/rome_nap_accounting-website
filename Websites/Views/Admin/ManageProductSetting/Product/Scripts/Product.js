var cachedProductData;

function SearchProduct(pageSize, pageIndex) {
    parent.BlockUI();
    $.ajax({
        url: 'SelectProduct',
        type: 'GET',
        data: {
            ProductId: $('#txtProductId').val(),
            ProductName: $('#txtProductName').val(),
            ProductPrefix: $('#txtProductPrefix').val(),
            PageSize: pageSize,
            PageIndex: pageIndex
        },
        success: function (data) {
            cachedProductData = data;
            RenderProductData(data, pageSize, pageIndex);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderProductData(data, pageSize, pageIndex) {
    var bodyRows = [];
    let rowCount = data.ProductEntities.length;
    for (var i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'ProductID', Value: data.ProductEntities[i].ProductID });
        row.Columns.push({ Name: 'ProductName', Value: data.ProductEntities[i].ProductName });
        row.Columns.push({ Name: 'Description', Value: data.ProductEntities[i].Description });
        row.Columns.push({ Name: 'ProductPrefix', Value: data.ProductEntities[i].ProductPrefix });
        row.Columns.push({
            Name: 'Enabled',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.ProductEntities[i].Enabled === 'True' ? 'checked' : '') + ' />'
        });
        row.Columns.push({ Name: 'EndOfSupportDate', Value: data.ProductEntities[i].EndOfSupportDate });
        row.Columns.push({ Name: 'BalanceType', Value: data.ProductEntities[i].BalanceType });
        row.Columns.push({ Name: 'SubProductBalanceType', Value: data.ProductEntities[i].SubProductBalanceType.replace(/</g, '&lt;') });
        row.Columns.push({ Name: 'GroupType', Value: data.ProductEntities[i].GroupType });
        row.Columns.push({ Name: 'CasinoType', Value: data.ProductEntities[i].CasinoType });
        row.Columns.push({
            Name: 'IsOfferPlayForFun',
            Value: '<input type="checkbox" disabled="disabled" ' + (data.ProductEntities[i].IsOfferPlayForFun === 'True' ? 'checked' : '') + ' />'
        });
        row.Columns.push({ Name: 'VendorId', Value: data.ProductEntities[i].VendorID });
        row.Columns.push({ Name: 'OrderNo', Value: data.ProductEntities[i].OrderNo });
        row.Columns.push({
            Name: 'Edit',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditProduct(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
        {
            columns: [
                { HeaderText: "ProductID", Width: 50, Name: "ProductID", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "ProductName", Width: 200, Name: "ProductName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Description", Width: 200, Name: "Description", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "ProductPrefix", Width: 100, Name: "ProductPrefix", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Enabled", Width: 50, Name: "Enabled", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "EndOfSupportDate", Width: 100, Name: "EndOfSupportDate", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "BalanceType", Width: 50, Name: "BalanceType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "SubProductBalanceType", Width: 200, Name: "SubProductBalanceType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "GroupType", Width: 50, Name: "GroupType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "CasinoType", Width: 50, Name: "CasinoType", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "IsOfferPlayForFun", Width: 100, Name: "IsOfferPlayForFun", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "VendorId", Width: 50, Name: "VendorId", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "OrderNo", Width: 50, Name: "OrderNo", HeaderAlign: "center", CellAlign: "center" },
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
                pagerSelectCallBack: reRenderProductAfterPaged,
                totalItem: data.TotalRows
            }
        };

    $('#gridProductId').fanexGrid(gridOptions);
}

function reRenderProductAfterPaged(currentPageIndex, itemsPerPage) {
    parent.BlockUI();
    SearchProduct(itemsPerPage, currentPageIndex + 1);
}

function EditProduct(index) {
    $('#updateProductErrMsg').html('');
    $('#ProductId').text(cachedProductData.ProductEntities[index].ProductID);
    $('#ProductName').val(cachedProductData.ProductEntities[index].ProductName);
    $('#Description').val(cachedProductData.ProductEntities[index].Description);
    $('#ProductPrefix').val(cachedProductData.ProductEntities[index].ProductPrefix);
    $('#Enabled').prop('checked', cachedProductData.ProductEntities[index].Enabled === 'True' ? true : false);
    $('#EndOfSupportDate').val(cachedProductData.ProductEntities[index].EndOfSupportDate);
    $('#BalanceType').val(cachedProductData.ProductEntities[index].BalanceType);
    $('#SubProductBalanceType').val(cachedProductData.ProductEntities[index].SubProductBalanceType);
    $('#GroupType').val(cachedProductData.ProductEntities[index].GroupType);
    $('#CasinoType').val(cachedProductData.ProductEntities[index].CasinoType);
    $('#IsOfferPlayForFun').prop('checked', cachedProductData.ProductEntities[index].IsOfferPlayForFun === 'True' ? true : false);
    $('#VendorId').val(cachedProductData.ProductEntities[index].VendorID);
    $('#OrderNo').val(cachedProductData.ProductEntities[index].OrderNo);
    $('#edit-product-form').dialog({
        title: 'Edit Product',
        modal: true,
        width: 700,
        height: 400,
    });
    SetupCalendar();
}

function UpdateProduct() {
    parent.BlockUI();
    $.ajax({
        url: 'UpdateProduct',
        type: 'POST',
        dataType: 'json',
        data: {
            ProductId: $('#ProductId').text(),
            ProductName: $('#ProductName').val().escapeHTML(),
            Description: $('#Description').val().escapeHTML(),
            ProductPrefix: $('#ProductPrefix').val(),
            Enabled: $('#Enabled').is(":checked"),
            EndOfSupportDate: $('#EndOfSupportDate ').val(),
            BalanceType: $('#BalanceType').val(),
            SubProductBalanceType: encodeURI($('#SubProductBalanceType').val()),
            GroupType: $('#GroupType').val(),
            CasinoType: $('#CasinoType').val(),
            IsOfferPlayForFun: $('#IsOfferPlayForFun').is(":checked"),
            VendorId: $('#VendorId').val(),
            OrderNo: $('#OrderNo').val(),
        },
        success: function (result) {
            showAlertMessage('updateProductErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-product-form').dialog('close');
                    SearchProduct(50, 1);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateProductErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function SetupCalendar() {
    //  Set up calendar
    var dates = $('#EndOfSupportDate').val().split('/');
    var year = dates[2];
    var month = dates[0];
    var day = dates[1];
    month = month < 10 ? '0' + month : month;
    day = day < 10 ? '0' + day : day;
    var selectionDate = year + month + day;

    var cal = Calendar.setup({
        animation: false,
        showTime: false,
        bottomBar: true,
        trigger: "EndOfSupportDate_Trigger",
        inputField: "EndOfSupportDate",
        dateFormat: "%m/%d/%Y",
        selection: [selectionDate],
        onSelect: function () {
            this.hide();
        }
    });
    cal.selection.set(selectionDate);
}

$(document).ready(function () {
    $("#GroupType").select2({ width: "184px" });
    $("#CasinoType").select2({ width: "184px" });
    $("#BalanceType").select2({ width: "184px" });
});