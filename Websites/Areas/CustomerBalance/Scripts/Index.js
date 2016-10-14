$(document).ready(function () {
    if (roleId == 4) {
        cachedInputData.superId = custId;
        cachedInputData.superName = userName;
    }
    else {
        cachedInputData.masterId = custId;
        cachedInputData.masterName = userName;
    }

    GetData(custId, userName, roleId, 1, 50);
});

function GetData(custId, userName, roleId, pageIndex, pageSize) {
    if (roleId == 3) {
        cachedInputData.masterId = custId;
        cachedInputData.masterName = userName;
    }

    $.ajax({
        type: "POST",
        url: "GetCustomerBalance",
        contentType: "application/json; charset=utf-8",
        data: '{ CustId:"' + custId + '", PageIndex:' + pageIndex + ', PageSize:' + pageSize + ' }',
        beforeSend: function () {
            try { parent.BlockUI(); } catch (e) { }
        },
        success: function (data) {

            RenderGrid(data, roleId == 4 ? $gridSuper : $gridMaster, roleId, pageIndex, pageSize);
            if (roleId == 4) {
                $gridSuperContainer.show();
                $gridMasterContainer.hide();
            }
            else {
                $gridSuperContainer.hide();
                $gridMasterContainer.show();
            }
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderGrid(data, $gridId, roleId, pageIndex, pageSize) {
    var bodyRows = [];
    var rowCount = data.CustomerBalance.length;
    for (var i = 0 ; i < rowCount; i++) {
        var row = {
            Columns: [
                    { Name: "No", Value: data.CustomerBalance[i].RowNum },
                    {
                        Name: "UserName", Value: roleId == 4 ?
                          "<a href='javascript:GetData(" + data.CustomerBalance[i].CustId + ", \"" + data.CustomerBalance[i].UserName + "\", 3, 1, 50)'>" + data.CustomerBalance[i].UserName + "</a>" :
                          "<a href='javascript:parent.resetMenu();parent.gotoURL(\"centerFr\",\"Customer/CustomerBalance.aspx?custid=" + data.CustomerBalance[i].CustId + "\")'>" + data.CustomerBalance[i].UserName + "</a>"
                    },
                    { Name: "FirstName", Value: data.CustomerBalance[i].FirstName },
                    { Name: "LastName", Value: data.CustomerBalance[i].LastName },
                    { Name: "GivenCredit", Value: data.CustomerBalance[i].GivenCredit },
                    { Name: "TotalBalance", Value: data.CustomerBalance[i].TotalBalance },
                    { Name: "YesterdayBalance", Value: data.CustomerBalance[i].YesterdayBalance },
                    { Name: "TotalCash", Value: data.CustomerBalance[i].TotalCash },
                    { Name: "YesterdayCash", Value: data.CustomerBalance[i].YesterdayCash },
                    { Name: "AvailableCredit", Value: data.CustomerBalance[i].AvailableCredit },
                    { Name: "Outstanding", Value: data.CustomerBalance[i].Outstanding }
            ]
        };
        bodyRows.push(row);
    }

    var gridOptions =
       {
           columns: [
               { HeaderText: "No", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "User Name", Width: 150, Name: "UserName", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "First Name", Width: 80, Name: "FirstName", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "Last Name", Width: 80, Name: "LastName", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "Given Credit", Width: 120, Name: "GivenCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
               { HeaderText: "Total Balance", Width: 120, Name: "TotalBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
               { HeaderText: "Yesterday Balance", Width: 120, Name: "YesterdayBalance", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
               { HeaderText: "Total Cash", Width: 120, Name: "TotalCash", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
               { HeaderText: "Yesterday Cash", Width: 120, Name: "YesterdayCash", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
               { HeaderText: "Available Credit", Width: 120, Name: "AvailableCredit", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' },
               { HeaderText: "Outstanding", Width: 120, Name: "Outstanding", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0.00", FormatNegative: '$' }
           ],
           bodyRows: bodyRows,
           gridExpandHeight: 70,
           showTitle: true,
           titleInfo: {
               text: GetBreadScrum(roleId)
           },
           showPager: true,
           customPager: true,
           pagerOption: {
               itemsPerPage: pageSize,
               currentPage: pageIndex - 1,
               itemsPerPageArray: [50, 100, 150, 200],
               pagerSelectCallBack: reRenderAfterPaged,
               totalItem: data.TotalRows
           }
       };

    $gridId.fanexGrid(gridOptions);
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    //// Set grid option for re-render pager
    var custId = cachedInputData.masterId == undefined ? cachedInputData.superId : cachedInputData.masterId;
    var userName = cachedInputData.masterName == undefined ? cachedInputData.superName : cachedInputData.masterName;

    GetData(custId, userName, cachedInputData.masterId == null ? 4 : 3, currentPageIndex + 1, itemsPerPage);
}

function GetBreadScrum(roleId) {
    if (roleId == 4) {
        return cachedInputData.superName;
    }
    else {

        return cachedInputData.superName == undefined ? cachedInputData.masterName : "<a href='javascript:GetBack()'>" + cachedInputData.superName + "<a/> > " + cachedInputData.masterName;
    }
}

function GetBack() {
    $gridSuperContainer.show();
    $gridMasterContainer.hide();
    cachedInputData.masterId = null;
    cachedInputData.masterName = null;
}