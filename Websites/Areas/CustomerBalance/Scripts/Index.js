$(document).ready(function () {
    GetData(custId, userName, roleId, 1, 50, isCached);
});

function GetData(custId, userName, roleId, pageIndex, pageSize, isCached) {
    if (roleId == 3) {
        cachedInputData.masterId = custId;
        cachedInputData.masterName = userName;
    }

    $.ajax({
        type: "POST",
        url: "GetCustomerBalance",
        contentType: "application/json; charset=utf-8",
        data: '{ CustId:"' + custId + '", UserName:"' + userName + '", RoleId:"' + roleId + '", IsCached:' + (isCached == undefined ? false : isCached.toString()) + ', PageIndex:' + pageIndex + ', PageSize:' + pageSize + ' }',
        beforeSend: function () {
            try { parent.BlockUI(); } catch (e) { }
        },
        success: function (data) {
            cachedInputData.currentRoleId = roleId;
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
                          "<a href='javascript:GetData(" + data.CustomerBalance[i].CustId + ", \"" + data.CustomerBalance[i].UserName + "\", 3, 1, 50, true)'>" + data.CustomerBalance[i].UserName + "</a>" :
                          "<a href='javascript:parent.resetMenu();parent.gotoURL(\"centerFr\",\"Customer/CustomerBalance.aspx?custid=" + data.CustomerBalance[i].CustId + "&userName=" + data.CustomerBalance[i].UserName + "&isCached=true\")'>" + data.CustomerBalance[i].UserName + "</a>"
                    },
                    { Name: "FirstName", Value: data.CustomerBalance[i].FirstName },
                    { Name: "LastName", Value: data.CustomerBalance[i].LastName },
                    { Name: "GivenCredit", Value: data.CustomerBalance[i].GivenCredit },
                    { Name: "TotalBalance", Value: data.CustomerBalance[i].Balance },
                    { Name: "YesterdayBalance", Value: data.CustomerBalance[i].YesterdayBalance },
                    { Name: "TotalCash", Value: data.CustomerBalance[i].Cash },
                    { Name: "YesterdayCash", Value: data.CustomerBalance[i].YesterdayCash },
                    { Name: "AvailableCredit", Value: data.CustomerBalance[i].AvailableCredit },
                    { Name: "Outstanding", Value: data.CustomerBalance[i].Outstanding }
            ],
            CssClass: data.CustomerBalance[i].Closed ? 'add-customer-bggray' : ''
        };
        bodyRows.push(row);
    }

    var gridOptions =
       {
           columns: [
               { HeaderText: "No", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "User Name", Width: 150, Name: "UserName", HeaderAlign: "center", CellAlign: "center", AllowTruncate: true },
               { HeaderText: "First Name", Width: 80, Name: "FirstName", HeaderAlign: "center", CellAlign: "center", AllowTruncate: true },
               { HeaderText: "Last Name", Width: 80, Name: "LastName", HeaderAlign: "center", CellAlign: "center", AllowTruncate: true },
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
           allowTruncate: true,
           pagerOption: {
               itemsPerPage: pageSize,
               currentPage: pageIndex - 1,
               itemsPerPageArray: [50, 100, 150, 200],
               pagerSelectCallBack: reRenderAfterPaged,
               totalItem: data.TotalRows
           }
       };

    if (roleId == 4) {
        $('#ReportTitle').html('Master Balance');
    }
    else {
        $('#ReportTitle').html('Agent Balance');
    }
    $gridId.fanexGrid(gridOptions);
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    //// Set grid option for re-render pager
    var custId = cachedInputData.currentRoleId == 4 ? cachedInputData.superId : cachedInputData.masterId;
    var userName = cachedInputData.currentRoleId == 4 ? cachedInputData.superName : cachedInputData.masterName;

    GetData(custId, userName, cachedInputData.currentRoleId, currentPageIndex + 1, itemsPerPage, true);
}

function GetBreadScrum(roleId) {
    if (roleId == 4) {
        return cachedInputData.superName;
    }
    else {

        return cachedInputData.superName == "" ? cachedInputData.masterName : "<a href='javascript:GetBack()'>" + cachedInputData.superName + "<a/> > " + cachedInputData.masterName;
    }
}

function GetBack() {
    GetData(cachedInputData.superId, cachedInputData.superName, 4, 1, 50, true);
}