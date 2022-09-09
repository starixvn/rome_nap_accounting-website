var filter3rdData = {
    ProductId: "",
    ProductName: "",
    Description: "",
    ProductPrefix: "",
    OfferSingle: "",
    ApplyDCS: "",
    OfferSite: "0",
    PageIndex: 1,
    PageSize: 50
};
var cachedProduct3rd = [];


function SearchProduct3rd(pageindex, pagesize) {
    parent.BlockUI();
    filter3rdData.ProductId = $('#txtProduct3rdID').val();
    filter3rdData.ProductName = $('#txtProduct3rdName').val();
    filter3rdData.Description = $('#txt3rdDescription').val();
    filter3rdData.ProductPrefix = $('#txtProduct3rdPrefix').val();
    filter3rdData.OfferSingle = $('#txt3rdOfferSingle').val();
    filter3rdData.ApplyDCS = $('#txt3rdApplyDCS').val();
    filter3rdData.OfferSite = $('#txt3rdSite').val();
    filter3rdData.PageIndex = pageindex;
    filter3rdData.PageSize = pagesize;

    $.ajax({
        url: window.rootUrl + 'ManageProduct3RD/Select',
        type: 'POST',
        data: filter3rdData,
        success: function (data) {
            cachedProduct3rd = data.Product3RDEntities;
            RenderProduct3rdData(data);
        },
        error: function (err) {
            console.log(err);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function RenderProduct3rdData(data) {
    var bodyRows = [];

    for (var i = 0; i < data.Product3RDEntities.length; i++) {
        var product = data.Product3RDEntities[i];
        var row = { Columns: [], CssClass: "" };
        row.Columns.push({ Name: 'ProductId', Value: product.ProductId });
        row.Columns.push({ Name: 'ProductName', Value: product.ProductName });
        row.Columns.push({ Name: 'ProductDescription', Value: product.Description });
        row.Columns.push({ Name: 'ProductPrefix', Value: product.ProductPrefix });
        row.Columns.push({ Name: 'OfferSingle', Value: product.OfferSingle === "True" ? 1 : 0 });
        row.Columns.push({ Name: 'ApplyDCS', Value: product.ApplyDCS === "True" ? 1 : 0 });
        row.Columns.push({ Name: 'Enabled', Value: product.Enabled === "True" ? 1 : 0 });
        row.Columns.push({ Name: 'EndOfSupportDate', Value: product.EndOfSupportDate });
        row.Columns.push({ Name: 'OfferSite', Value: product.OfferSite });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditProduct3rd(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteProduct3rd(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "ProductId", Width: 80, Name: "ProductId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductName", Width: 180, Name: "ProductName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductDescription", Width: 250, Name: "ProductDescription", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductPrefix", Width: 120, Name: "ProductPrefix", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "OfferSingle", Width: 90, Name: "OfferSingle", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ApplyDCS", Width: 90, Name: "ApplyDCS", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Enabled", Width: 90, Name: "Enabled", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "EndOfSupportDate", Width: 130, Name: "EndOfSupportDate", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "OfferSite", Width: 90, Name: "OfferSite", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showPager: true,
        customPager: true,
        showDistinctRow: false,
        pagerOption: {
            itemsPerPage: filter3rdData.PageSize,
            currentPage: filter3rdData.PageIndex - 1,
            itemsPerPageArray: [50, 100, 150, 200],
            pagerSelectCallBack: change3rdPageHandler,
            totalItem: data.TotalRows
        }
    };

    $('#grid3rdId').fanexGrid(gridOptions);
}

function change3rdPageHandler(currentPageIndex, itemsPerPage) {
    SearchProduct3rd(currentPageIndex + 1, itemsPerPage);
}

function EditProduct3rd(index) {
    $('#update3rdErrMsg').html('');
    $('#UpdProduct3rdId').html(cachedProduct3rd[index].ProductId);
    $('#Upd3rdName').val(cachedProduct3rd[index].ProductName);
    $('#Upd3rdDesc').val(cachedProduct3rd[index].Description);
    $('#Upd3rdPrefix').val(cachedProduct3rd[index].ProductPrefix);
    $('#Upd3rdOffer').prop('checked', cachedProduct3rd[index].OfferSingle === "True");
    $('#Upd3rdApplyDCS').prop('checked', cachedProduct3rd[index].ApplyDCS === 'True');
    $('#Upd3rdEnabled').prop('checked', cachedProduct3rd[index].Enabled === 'True');
    $('#Upd3rdDate').val(cachedProduct3rd[index].EndOfSupportDate);
    $('#Upd3rdSite').val(cachedProduct3rd[index].OfferSite);

    $('#btnUpd3rd').off();
    $('#btnUpd3rd').click(function () { return UpdateProduct3rd(index); });
    $('#edit-3rd-form').dialog({
        title: 'Update Product3rd',
        modal: true,
        width: 'auto',
        height: 'auto'
    });
}

function UpdateProduct3rd(index) {
    var product = { ...cachedProduct3rd[index] };
    product.ProductName = $('#Upd3rdName').val();
    product.Description = $('#Upd3rdDesc').val();
    product.ProductPrefix = $('#Upd3rdPrefix').val();
    product.OfferSingle = $('#Upd3rdOffer').is(":checked");
    product.ApplyDCS = $('#Upd3rdApplyDCS').is(":checked");
    product.Enabled = $('#Upd3rdEnabled').is(":checked");
    product.EndOfSupportDate = $('#Upd3rdDate').val();
    product.OfferSite = $('#Upd3rdSite').val();

    if (product.ProductName === "") {
        showAlertMessage('update3rdErrMsg', 'ProductName cannot be null.', 1);
        return;
    }

    parent.BlockUI();
    $.ajax({
        url: window.rootUrl + 'ManageProduct3RD/Update',
        type: 'POST',
        dataType: 'json',
        data: product,
        success: function (result) {
            showAlertMessage('update3rdErrMsg', result.ErrorCode == 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
            if (result.ErrorCode == 0) {
                setTimeout(function () {
                    $('#edit-3rd-form').dialog('close');
                    SearchProduct3rd(1, 50);
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('update3rdErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function DeleteProduct3rd(index) {
    const productId = cachedProduct3rd[index].ProductId;
    if (confirm("Are you sure you want to delete this row?")) {
        parent.BlockUI();
        $.ajax({
            url: window.rootUrl + 'ManageProduct3RD/Delete',
            type: 'POST',
            dataType: 'json',
            data: {
                productId
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    alert('Deleted successfully.');
                    SearchProduct3rd(1, 50);
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

$(document).ready(function () {

    $('#Upd3rdDate').datetimepicker("destroy");
    $('#Upd3rdDate').datetimepicker({
        format: 'm/d/Y'
    });

    $('#Upd3rdDate_Trigger').click(function () {
        jQuery('#Upd3rdDate').datetimepicker('show');
    });
    $("#txt3rdOfferSingle").select2({ width: "138px" });
    $("#txt3rdApplyDCS").select2({ width: "138px" });
    $("#txt3rdSite").select2({ width: "138px" }); 
    $("#Upd3rdSite").select2({ width: "126px" });
});