var cachedSubTabId = "sub_tab_1";
var isTest = false;
var cachedDWLList = [];
var cachedDWLListExtraPT = [];
var cachedDWLListTest = [];
var cachedDWLListExtraPTTest = [];
function SwitchSubTab(id, contentId) {
    cachedSubTabId = id;
    $('#sub_tabs').find('li').removeClass('current');
    $('div[id^="sub_tab_content_"]').hide();
    $('#' + id).addClass('current');
    $('#' + contentId).show();
    UpdateResetWinningSampleUrl();
}


function UpdateResetWinningSampleUrl() {
    switch (cachedSubTabId) {
        case "sub_tab_1":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/ResetWinning/DWLList.xls");
            isTest = false;
            break;
        case "sub_tab_2":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/ResetWinning/DWLListExtraPT.xls");
            isTest = false;
            break;
        case "sub_tab_3":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/ResetWinning/DWLListTest.xls");
            isTest = true;
            break;
        case "sub_tab_4":
            $('#sampleSettingLink').prop("href", rootUrl + "Views/Admin/ManageProductSetting/ResetWinning/DWLListExtraPTTest.xls");
            isTest = true;
            break;
        default:
            break;
    }
}

function GetInsertResetWinningAction() {
    switch (cachedSubTabId) {
        case "sub_tab_1":
            return 'InsertDWLList';
        case "sub_tab_2":
            return 'InsertDWLListExtraPT';
        case "sub_tab_3":
            return 'InsertDWLListTest';
        case "sub_tab_4":
            return 'InsertDWLListExtraPTTest';
        default:
            return '';
    }
}

function SearchResetWinning() {
    switch (cachedSubTabId) {
        case "sub_tab_1":
            SearchDWLList();
            break;
        case "sub_tab_2":
            SearchDWLListExtra();
            break;
        case "sub_tab_3":
            SearchDWLListTest();
            break;
        case "sub_tab_4":
            SearchDWLListExtraTest();
            break;
        default:
            break;
    }
}

function SearchDWLList() {
    parent.BlockUI();
    var custId = $('#txtDWLListCustId').val();
    var productId = $('#txtDWLListProductId').val();
    if (custId !== '' && isNaN(custId)
        || productId !== '' && isNaN(productId)) {
        RenderDWLListData([]);
        parent.jQuery.unblockUI();
        return;
    }

    var isDisabled = false;
    if ($('#txtDWLListIsDisabled').val() === '') {
        isDisabled = null;
    }

    if ($('#txtDWLListIsDisabled').val() === '1') {
        isDisabled = true;
    }

    $.ajax({
        url: 'SelectDWLList',
        type: 'GET',
        data: {
            CustId: $('#txtDWLListCustId').val(),
            Username: $('#txtDWLListUsername').val(),
            ProductId: $('#txtDWLListProductId').val(),
            Product: $('#txtDWLListProduct').val(),
            IsDisabled: isDisabled,
            IsTest: isTest
        },
        success: function (data) {
            cachedDWLList = data;
            RenderDWLListData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });
}

function SearchDWLListTest() {
    var custId = $('#txtDWLListTestCustId').val();
    var productId = $('#txtDWLListTestProductId').val();
    if (custId !== '' && isNaN(custId)
        || productId !== '' && isNaN(productId)) {
        RenderDWLListData([]);
        parent.jQuery.unblockUI();
        return;
    }

    parent.BlockUI();

    var isDisabled = false;
    if ($('#txtDWLListTestIsDisabled').val() === '') {
        isDisabled = null;
    }

    if ($('#txtDWLListTestIsDisabled').val() === '1') {
        isDisabled = true;
    }

    $.ajax({
        url: 'SelectDWLList',
        type: 'GET',
        data: {
            CustId: $('#txtDWLListTestCustId').val(),
            Username: $('#txtDWLListTestUsername').val(),
            ProductId: $('#txtDWLListTestProductId').val(),
            Product: $('#txtDWLListTestProduct').val(),
            IsDisabled: isDisabled,
            IsTest: isTest
        },
        success: function (data) {
            cachedDWLListTest = data;
            RenderDWLListData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });
}

function RenderDWLListData(data) {
    var bodyRows = [];
    var rowCount = data.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'CustId', Value: data[i].CustId });
        row.Columns.push({ Name: 'Username', Value: data[i].Username });
        row.Columns.push({ Name: 'ProductId', Value: data[i].ProductId });
        row.Columns.push({ Name: 'Product', Value: data[i].Product });
        row.Columns.push({ Name: 'Seq', Value: data[i].Seq });
        row.Columns.push({ Name: 'IsDisabled', Value: data[i].IsDisabled === "True" ? 1 : 0 });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditDWLList(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteDWLList(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "CustId", Width: 100, Name: "CustId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Username", Width: 200, Name: "Username", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "ProductId", Width: 100, Name: "ProductId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Product", Width: 200, Name: "Product", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Seq", Width: 80, Name: "Seq", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "IsDisabled", Width: 100, Name: "IsDisabled", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 100, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    if (isTest) {
        $('#gridDWLListTestId').fanexGrid(gridOptions);
    } else {
        $('#gridDWLListId').fanexGrid(gridOptions);
    }
}

function EditDWLList(index) {
    var item = cachedDWLList[index];
    if (isTest) {
        item = cachedDWLListTest[index];
    }
    $('#updateDWLListErrMsg').html('');
    $('#DWLListCustId').text(item.CustId);
    $('#DWLListUsername').text(item.Username);
    $('#DWLListProduct').text(item.Product);
    $('#DWLListProductId').val(item.ProductId);
    $('#DWLListSeq').val(item.Seq);
    $('#DWLListIsDisabled').prop('checked', item.IsDisabled === "True");
    $('#btnDWLListUpdate').off();
    $('#btnDWLListUpdate').click(function () { return UpdateDWLList(index); });
    if (isTest) {
        title = "Update Acc_DWLList_xtest";
    } else {
        title = "Update Acc_DWLList";
    }

    $('#edit-dwllist-form').dialog({
        title: title,
        modal: true,
        width: 'auto',
        height: 'auto'
    });
}

function UpdateDWLList(index) {
    var productID = $('#DWLListProductId').val();
    if (productID === '') {
        showAlertMessage('updateDWLListErrMsg', 'ProductID cannot be null.', 1);
        return;
    }
    var seq = $('#DWLListSeq').val();
    if (seq === '') {
        showAlertMessage('updateDWLListErrMsg', 'Seq cannot be null.', 1);
        return;
    }
    parent.BlockUI();
    var item = cachedDWLList[index];
    if (isTest) {
        item = cachedDWLListTest[index];
    }
    $.ajax({
        url: 'UpdateDWLList',
        type: 'POST',
        dataType: 'json',
        data: {
            CustId: item.CustId,
            Username: item.Username,
            Product: item.Product,
            ProductID: $('#DWLListProductId').val(),
            Seq: $('#DWLListSeq').val(),
            IsDisabled: $('#DWLListIsDisabled').is(":checked"),
            IsTest: isTest
        },
        success: function (result) {
            showAlertMessage('updateDWLListErrMsg', result.ErrorCode === 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode === 0 ? 0 : 1);
            if (result.ErrorCode === 0) {
                setTimeout(function () {
                    $('#edit-dwllist-form').dialog('close');
                    SearchResetWinning();
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateDWLListErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function DeleteDWLList(index) {
    if (confirm('Are you sure you want to delete this row?')) {
        parent.BlockUI();
        var item = cachedDWLList[index];
        if (isTest) {
            item = cachedDWLListTest[index];
        }
        $.ajax({
            url: 'DeleteDWLList',
            type: 'POST',
            dataType: 'json',
            data: {
                CustId: item.CustId,
                Product: item.Product,
                IsTest: isTest
            },
            success: function (result) {
                parent.jQuery.unblockUI();
                if (result.ErrorCode === 0) {
                    alert('Deleted successfully.');
                    SearchResetWinning();
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

function SearchDWLListExtra() {
    parent.BlockUI();

    var custId = $('#txtDWLListExtraCustId').val();
    var groupId = $('#txtDWLListExtraGroupId').val();
    if (custId !== '' && isNaN(custId)
        || groupId !== '' && isNaN(groupId)) {
        parent.jQuery.unblockUI();
        RenderDWLListExtraPTData([]);
        return;
    }

    var isDisabled = false;
    if ($('#txtDWLListExtraIsDisabled').val() === '') {
        isDisabled = null;
    }

    if ($('#txtDWLListExtraIsDisabled').val() === '1') {
        isDisabled = true;
    }
    $.ajax({
        url: 'SelectDWLListExtraPT',
        type: 'GET',
        data: {
            CustId: $('#txtDWLListExtraCustId').val(),
            Username: $('#txtDWLListExtraUsername').val(),
            GroupId: $('#txtDWLListExtraGroupId').val(),
            GroupName: $('#txtDWLListExtraGroupName').val(),
            IsDisabled: isDisabled,
            IsTest: isTest
        },
        success: function (data) {
            cachedDWLListExtraPT = data;
            RenderDWLListExtraPTData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });
}

function RenderDWLListExtraPTData(data) {
    var bodyRows = [];
    var rowCount = data.length;
    for (i = 0; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'CustId', Value: data[i].CustId });
        row.Columns.push({ Name: 'Username', Value: data[i].Username });
        row.Columns.push({ Name: 'GroupId', Value: data[i].GroupId });
        row.Columns.push({ Name: 'GroupName', Value: data[i].GroupName });
        row.Columns.push({ Name: 'IsDisabled', Value: data[i].IsDisabled === "True" ? 1 : 0 });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer" onclick="EditDWLListExtraPT(' + i + ')" >&nbsp;'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteDWLListExtraPT(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "CustId", Width: 100, Name: "CustId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Username", Width: 200, Name: "Username", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "GroupId", Width: 100, Name: "GroupId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "GroupName", Width: 200, Name: "GroupName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "IsDisabled", Width: 100, Name: "IsDisabled", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 100, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    if (isTest) {
        $('#gridDWLListExtraPTTest').fanexGrid(gridOptions);
    } else {
        $('#gridDWLListExtraPT').fanexGrid(gridOptions);
    }
}

function SearchDWLListExtraTest() {
    parent.BlockUI();
    var custId = $('#txtDWLListExtraTestCustId').val();
    var groupId = $('#txtDWLListExtraTestGroupId').val();
    if (custId !== '' && isNaN(custId)
        || groupId !== '' && isNaN(groupId)) {
        parent.jQuery.unblockUI();
        RenderDWLListExtraPTData([]);
        return;
    }

    var isDisabled = false;
    if ($('#txtDWLListExtraTestIsDisabled').val() === '') {
        isDisabled = null;
    }

    if ($('#txtDWLListExtraTestIsDisabled').val() === '1') {
        isDisabled = true;
    }
    $.ajax({
        url: 'SelectDWLListExtraPT',
        type: 'GET',
        data: {
            CustId: $('#txtDWLListExtraTestCustId').val(),
            Username: $('#txtDWLListExtraTestUsername').val(),
            GroupId: $('#txtDWLListExtraTestGroupId').val(),
            GroupName: $('#txtDWLListExtraTestGroupName').val(),
            IsDisabled: isDisabled,
            IsTest: isTest
        },
        success: function (data) {
            cachedDWLListExtraPTTest = data;
            RenderDWLListExtraPTData(data);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
        }
    });
}

function EditDWLListExtraPT(index) {
    var item = cachedDWLListExtraPT[index];
    if (isTest) {
        item = cachedDWLListExtraPTTest[index];
    }
    $('#updateDWLListExtraPTErrMsg').html('');
    $('#DWLListExtraPTCustId').text(item.CustId);
    $('#DWLListExtraPTUsername').text(item.Username);
    $('#DWLListExtraPTGroupId').val(item.GroupId);
    $('#DWLListExtraPTGroupName').val(item.GroupName);
    $('#DWLListExtraPTIsDisabled').prop('checked', item.IsDisabled === "True");
    $('#btnDWLListExtraPTUpdate').off();
    $('#btnDWLListExtraPTUpdate').click(function () { return UpdateDWLListExtraPT(index); });
    if (isTest) {
        title = "Update Acc_DWLList_ExtraPT_xtest";
    } else {
        title = "Update Acc_DWLList_ExtraPT";
    }

    $('#edit-dwllistextrapt-form').dialog({
        title: title,
        modal: true,
        width: 'auto',
        height: 'auto'
    });
}

function UpdateDWLListExtraPT(index) {
    var item = cachedDWLListExtraPT[index];
    if (isTest) {
        item = cachedDWLListExtraPTTest[index];
    }
    var groupId = $('#DWLListExtraPTGroupId').val();
    if (groupId === '') {
        showAlertMessage('updateDWLListExtraPTErrMsg', 'GroupId cannot be null.', 1);
        return;
    }
    var groupName = $('#DWLListExtraPTGroupName').val();
    if (groupName === '') {
        showAlertMessage('updateDWLListExtraPTErrMsg', 'GroupName cannot be null.', 1);
        return;
    }
    parent.BlockUI();
    $.ajax({
        url: 'UpdateDWLListExtraPT',
        type: 'POST',
        dataType: 'json',
        data: {
            CustId: item.CustId,
            Username: item.Username,
            GroupId: $('#DWLListExtraPTGroupId').val(),
            GroupName: $('#DWLListExtraPTGroupName').val(),
            IsDisabled: $('#DWLListExtraPTIsDisabled').is(":checked"),
            IsTest: isTest
        },
        success: function (result) {
            showAlertMessage('updateDWLListExtraPTErrMsg', result.ErrorCode === 0 ? 'Updated successfully.' : result.ErrorMessage, result.ErrorCode === 0 ? 0 : 1);
            if (result.ErrorCode === 0) {
                setTimeout(function () {
                    $('#edit-dwllistextrapt-form').dialog('close');
                    SearchResetWinning();
                }, 3000);
            }
        },
        error: function (xhr) {
            showAlertMessage('updateDWLListExtraPTErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function DeleteDWLListExtraPT(index) {
    if (confirm('Are you sure you want to delete this row?')) {
        parent.BlockUI();
        var item = cachedDWLListExtraPT[index];
        if (isTest) {
            item = cachedDWLListExtraPTTest[index];
        }
        $.ajax({
            url: 'DeleteDWLListExtraPT',
            type: 'POST',
            dataType: 'json',
            data: {
                CustId: item.CustId,
                IsTest: isTest
            },
            success: function (result) {
                parent.jQuery.unblockUI();
                if (result.ErrorCode === 0) {
                    alert('Deleted successfully.');
                    SearchResetWinning();
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
    UpdateResetWinningSampleUrl();
    $("#txtDWLListIsDisabled").select2({ width: "138px" });
    $("#txtDWLListExtraIsDisabled").select2({ width: "138px" });
    $("#txtDWLListExtraTestIsDisabled").select2({ width: "138px" });
    $("#txtDWLListTestIsDisabled").select2({ width: "138px" });
});