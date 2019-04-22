$(document).ready(function () {
    var DOMElements = {
        btnUnsuspend: $('#btnUnsuspend'),
        custId: $('#custId'),
        successMsg: $('#successMsg'),
        errMsg: $('#errMsg'),
        btnBack: $('#btnBack'),
        tabDetail: $('#tab-detail'),
        tabMain: $('#tab-main'),
        informationBox: $('#information-box'),
    };

    DOMElements.btnUnsuspend.click(function () {
        parent.ConfirmPopup('Confirmation', LanguageResources.UnsuspendSingleConfirmMesssage, function () {
            try { parent.BlockUI(); } catch (e) { }
            DOMElements.btnUnsuspend.attr("disabled", true);
            clearMessage();
            var params = {
                custId: DOMElements.custId.val()
            };

            $.ajax({
                type: 'POST',
                url: "Update",
                dataType: "json",
                data: params,
                success: function (result) {
                    if (result.ErrorCode == 0) {
                        DOMElements.successMsg.html(LanguageResources.UpdateSuspendSuccess).show();
                        setTimeout(function () {
                            top.window.frames['content'].submitSearch();
                            parent.$('#dialog-modalupdate-unsuspend').dialog('close');
                            parent.$('#dialog-modalupdate-unsuspend').dialog('destroy');
                        }, 3000)
                    }
                    else {
                        DOMElements.errMsg.html(result.ErrorMessage).show();
                        DOMElements.btnUnsuspend.removeAttr("disabled");
                    }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        });
    });

    var clearMessage = function () {
        DOMElements.successMsg.empty().hide();
        DOMElements.errMsg.empty().hide();
    }

    DOMElements.btnBack.click(function () {
        DOMElements.tabDetail.fadeOut(function () { DOMElements.tabMain.fadeIn(); });
    });

    DOMElements.informationBox.click(function () {
        parent.AlertPopup('Information', LanguageResources.SuspendInfoMessage);
    });
});

var GetDownlinesDetailInfo = function (custId, isAffected, downlineRoleId, pageSize, pageIndex) {
    try { parent.BlockUI(); } catch (e) { }
    var params = {
        custId: custId,
        isAffected: isAffected,
        downlineRoleId: downlineRoleId,
        pageSize: pageSize,
        pageIndex: pageIndex
    };

    cachedDownlinesDetailInfoData.custId = custId;
    cachedDownlinesDetailInfoData.isAffected = isAffected;
    cachedDownlinesDetailInfoData.downlineRoleId = downlineRoleId;
    $.ajax({
        type: 'POST',
        url: "GetDownlinesInfo",
        dataType: "json",
        data: params,
        success: function (result) {
            var bodyRows = [];
            var rowCount = result.AffectedDownlines.length;
            var startMergedRowIndex = 0;
            var endMergedRowIndex = 0;
            var prevLevel = '';
            var currentLevel = '';
            for (var i = 0 ; i < rowCount; i++) {
                var row = { Columns: [] };
                currentLevel = result.AffectedDownlines[i].LevelName;
                endMergedRowIndex = i;
                row.Columns.push({ Name: 'No', Value: pageSize * pageIndex + i + 1 });
                if (prevLevel != currentLevel) {
                    if (startMergedRowIndex != endMergedRowIndex) {
                        bodyRows[startMergedRowIndex].Columns[1].MergeRowNumber = endMergedRowIndex - startMergedRowIndex;
                    }

                    startMergedRowIndex = endMergedRowIndex;
                    row.Columns.push({ Name: 'Level', Value: result.AffectedDownlines[i].LevelName });
                }

                row.Columns.push({ Name: 'Username', Value: result.AffectedDownlines[i].UserName });
                bodyRows.push(row);
                prevLevel = currentLevel;
            }

            if (startMergedRowIndex != endMergedRowIndex) {
                bodyRows[startMergedRowIndex].Columns[1].MergeRowNumber = endMergedRowIndex - startMergedRowIndex + 1;
            }

            var gridOptions =
            {
                columns: [
                    { HeaderText: "No", Width: 50, Name: "No", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Level", Width: 100, Name: "Level", HeaderAlign: "center" },
                    { HeaderText: "Username", Width: 400, Name: "Username", HeaderAlign: "center" }
                ],
                bodyRows: bodyRows,
                gridExpandHeight: 100,
                showDistinctRow: false,
                showNumberColumn: true,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: pageSize,
                    currentPage: pageIndex,
                    itemsPerPageArray: [50, 100, 150, 200],
                    pagerSelectCallBack: reRenderAfterPaged,
                    totalItem: result.TotalRows
                }
            };

            $('#tab-main').fadeOut(function () {
                $('#tab-detail').fadeIn(function () {
                    $('#tab-detail').show();
                    $('#gridContainer').show();
                    $('#gridId').fanexGrid(gridOptions);
                });
            });
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    GetDownlinesDetailInfo(cachedDownlinesDetailInfoData.custId, cachedDownlinesDetailInfoData.isAffected, cachedDownlinesDetailInfoData.downlineRoleId, itemsPerPage, currentPageIndex);
}