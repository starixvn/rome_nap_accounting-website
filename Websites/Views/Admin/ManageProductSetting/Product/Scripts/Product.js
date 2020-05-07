var cachedProductData;

function SearchProduct() {
    parent.BlockUI();
    $.ajax({
        url: 'SelectProduct',
        type: 'GET',
        data: {
            ProductId: $('#txtProductId').val(),
            ProductName: $('#txtProductName').val(),
            ProductPrefix: $('#txtProductPrefix').val(),
            PageSize: $('#PageProductSize').val(),
            PageIndex: $('#PageProductIndex').val()
        },
        success: function (data) {
            cachedProductData = data;
            RenderProductData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderProductData(data) {
    var bodyRows = [];
    var rowCount = data.ProductEntities.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'ProductID', Value: data.ProductEntities[i].ProductID });
        row.Columns.push({ Name: 'ProductName', Value: data.ProductEntities[i].ProductName });
        row.Columns.push({ Name: 'Description', Value: data.ProductEntities[i].Description });
        row.Columns.push({ Name: 'ProductPrefix', Value: data.ProductEntities[i].ProductPrefix });
        row.Columns.push({ Name: 'Enabled', Value: data.ProductEntities[i].Enabled === true ? 1 : 0 });
        row.Columns.push({ Name: 'EndOfSupportDate', Value: data.ProductEntities[i].EndOfSupportDate });
        row.Columns.push({ Name: 'BalanceType', Value: data.ProductEntities[i].BalanceType });
        row.Columns.push({ Name: 'SubProductBalanceType', Value: data.ProductEntities[i].SubProductBalanceType.replace(/</g, '&lt;') });
        row.Columns.push({ Name: 'GroupType', Value: data.ProductEntities[i].GroupType });
        row.Columns.push({ Name: 'CasinoType', Value: data.ProductEntities[i].CasinoType });
        row.Columns.push({ Name: 'IsOfferPlayForFun', Value: data.ProductEntities[i].IsOfferPlayForFun === true ? 1 : 0 });
        row.Columns.push({ Name: 'VendorId', Value: data.ProductEntities[i].VendorId });
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
            itemsPerPage: parseInt($('#PageProductSize').val()),
            currentPage: parseInt($('#PageProductIndex').val()) - 1,
            itemsPerPageArray: [50, 100, 150, 200],
            pagerSelectCallBack: reRenderProductAfterPaged,
            totalItem: data.TotalRows
        }
    };

    $('#gridProductId').fanexGrid(gridOptions);
}

function reRenderProductAfterPaged(currentPageIndex, itemsPerPage) {
    $('#PageProductSize').val(itemsPerPage);
    $('#PageProductIndex').val(currentPageIndex + 1);
    parent.BlockUI();
    SearchProduct();
}

function EditProduct(index) {
    $('#updateProductErrMsg').html('');
    $('#ProductId').text(cachedProductData.ProductEntities[index].ProductID);
    $('#ProductName').val(cachedProductData.ProductEntities[index].ProductName);
    $('#Description').val(cachedProductData.ProductEntities[index].Description);
    $('#ProductPrefix').val(cachedProductData.ProductEntities[index].ProductPrefix);
    $('#Enabled').val(cachedProductData.ProductEntities[index].Enabled === true ? 1 : 0);
    $('#EndOfSupportDate').val(cachedProductData.ProductEntities[index].EndOfSupportDate);
    $('#BalanceType').val(cachedProductData.ProductEntities[index].BalanceType);
    $('#SubProductBalanceType').val(cachedProductData.ProductEntities[index].SubProductBalanceType);
    $('#GroupType').val(cachedProductData.ProductEntities[index].GroupType);
    $('#CasinoType').val(cachedProductData.ProductEntities[index].CasinoType);
    $('#IsOfferPlayForFun').val(cachedProductData.ProductEntities[index].IsOfferPlayForFun === true ? 1 : 0);
    $('#VendorId').val(cachedProductData.ProductEntities[index].VendorId);
    $('#OrderNo').val(cachedProductData.ProductEntities[index].OrderNo);
    $('#edit-product-form').dialog({
        title: 'Edit Product',
        modal: true,
        width: 700,
        height: 700,
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
            ProductName: $('#ProductName').val(),
            Description: $('#Description').val(),
            ProductPrefix: $('#ProductPrefix').val(),
            Enabled: $('#Enabled').val() === '1' ? true : false,
            EndOfSupportDate: $('#EndOfSupportDate ').val(),
            BalanceType: $('#BalanceType').val(),
            SubProductBalanceType: encodeURI($('#SubProductBalanceType').val()),
            GroupType: $('#GroupType').val(),
            CasinoType: $('#CasinoType').val(),
            IsOfferPlayForFun: $('#IsOfferPlayForFun').val() === '1' ? true : false,
            VendorId: $('#VendorId').val(),
            OrderNo: $('#OrderNo').val(),
        },
        success: function (result) {
            showAlertMessage('updateProductErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-product-form').dialog('close');
                    SearchProduct();
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
    var selectionDate = year + month + dates;

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