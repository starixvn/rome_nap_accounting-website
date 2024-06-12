$(document).ready(function () {
    var DOMElements = {
        txtUsername: $('#txtUsername'),
        btnSearch: $('#btnSearch'),
        btnLock: $('#btnLock'),
        btnUnlock: $('#btnUnlock'),
        gridContainer: $('#gridContainer'),
        gridId: $('#gridId'),
        errMsg: $('#errMsg')
    };

    BindingEvents = function () {
        DOMElements.btnSearch.click(Search);
        DOMElements.btnLock.click(function () { return Update(true) });
        DOMElements.btnUnlock.click(function () { return Update(false) });
    }

    Init = function () {
        BindingEvents();
        DOMElements.btnSearch.click();
    }

    Search = function () {
        parent.BlockUI();
        $.get('Search', null, function (data) {
            var bodyRows = [];
            for (i = 0; i < data.length; i++) {

                var row = { Columns: [] };
                row.Columns.push({ Name: 'RowNum', Value: i + 1 });
                row.Columns.push({ Name: 'Username', Value: data[i].UserName });
                row.Columns.push({ Name: 'Level', Value: data[i].Level });
                row.Columns.push({ Name: 'Status', Value: data[i].Status });
                bodyRows.push(row);
            }

            var gridOptions =
                {
                    columns: [
                        { HeaderText: "#", Width: 40, Name: "RowNum", HeaderAlign: "center", CellAlign: "center" },
                        { HeaderText: "Username", Width: 300, Name: "Username", HeaderAlign: "center" },
                        { HeaderText: "Level", Width: 80, Name: "Level", HeaderAlign: "center" },
                        { HeaderText: "Status", Width: 80, Name: "Status", HeaderAlign: "Status" }
                    ],
                    bodyRows: bodyRows,
                    gridExpandHeight: 200,
                    showDistinctRow: false
                };

            DOMElements.gridId.fanexGrid(gridOptions);
            DOMElements.gridContainer.show();
        }).always(parent.jQuery.unblockUI);
    }

    Update = function (isLock) {
        DOMElements.errMsg.html();
        DOMElements.errMsg.hide();
        if ($.trim(DOMElements.txtUsername.val()) == '') {

            DOMElements.errMsg.html("<div class='alertboxerr'><div>Please input Username(s).</div></div>");
            DOMElements.errMsg.show();
        }

        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "Update",
            contentType: "application/json",
            data: JSON.stringify({ custList: DOMElements.txtUsername.val(), isLock: isLock }),
            dataType: "json",
            success: function (data) {
                if (data.ErrorCode == 0) {
                    DOMElements.errMsg.html("<div class='alertboxscc'><div>Updated successfully.</div></div>");
                    DOMElements.errMsg.show();
                    setTimeout(function () {
                        DOMElements.errMsg.html();
                        DOMElements.errMsg.hide();
                        Search();
                    }, 3000);
                } else {
                    DOMElements.errMsg.html("<div class='alertboxerr'><div>" + data.ErrorMessage + "</div></div>");
                    DOMElements.errMsg.show();
                }
            },
            error: function (err) {    
                if (err.status == 403) {
                    DOMElements.errMsg.html("<div class='alertboxerr'><div>You don't have permission to access.</div></div>");                
                }
                else {
                    DOMElements.errMsg.html("<div class='alertboxerr'><div>" + data.ErrorMessage + "</div></div>");
                }              
                DOMElements.errMsg.show();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });   
    }

    Init();
});
