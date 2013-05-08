var recoverBet = {
    matchGridWidth: 649,
    matchGridHeight: 100,
    ticketsGridWidth: 1193,
    ticketsGridHeight: 800,
    isHavingBetsInfo: false,
    initPage: function () {
        $("#tbxTransId").ForceDecimalOnly();
        $("#btnSearch").click(function () {
            recoverBet.searchTransaction();
        });
        var gridOptsMatchInfo = {
            tblContentId: 'matchInfo_Container',
            isShowPager: false
        };

        var gridOptsBets = {
            tblContentId: 'betListInfo_Container',
            isShowPager: false
        };
        $("#matchInfo").initGrid(gridOptsMatchInfo);
        $("#betListInfo").initGrid(gridOptsBets);
    },
    searchTransaction: function () {
        if ($("#tbxTransId").val() == "") {
            showAlertMessage("ErrorMessage", "Please enter transid!", 1);
            $("#ErrorMessage").removeClass("hidden");
            return;
        }
        recoverBet.blockUI();
        $.ajax({
            type: "POST",
            url: "../RecoverBets/SelRecoverBets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"TransId":' + $("#tbxTransId").val() + '}',
            success: function (rs) {
                if (rs.ErrorCode == 0) {
                    $("#ErrorMessage").empty().addClass("hidden");
                    $("#betListInfo_Container tbody").empty().append(recoverBet.displayBets(rs));
                    var gridOptsMatchInfo = {
                        tblContentId: 'matchInfo_Container',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: 300,
                        gridWidth: 585,
                        freezeWidth: 585,
                        freezeHeight: 200,
                        numberFreezeCol: 3
                    };

                    var gridOptsBets = {
                        tblContentId: 'betListInfo_Container',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: 300,
                        gridWidth: 1010,
                        freezeWidth: 230,
                        freezeHeight: 200,
                        numberFreezeCol: 3
                    };
                    $("#dvContent").removeClass("hidden");

                    if (recoverBet.isHavingBetsInfo) {
                        $("#matchInfo").removeClass("hidden");
                        $("#matchInfo").gridScroller(gridOptsMatchInfo);
                    } else {
                        $("#matchInfo").addClass("hidden");
                        $("#matchInfo_RowHeader tbody").empty();
                        $("#matchInfo_Container tbody").empty();
                    }
                    $("#betListInfo").removeClass("hidden");
                    $("#betListInfo").gridScroller(gridOptsBets);
                    recoverBet.make2TableBodySameHeight($("#betListInfo_RowHeader_Content"), $("#betListInfo_Container"));
                }
                else {
                    showAlertMessage("ErrorMessage", rs.ErrorMessage, 1);
                    $("#ErrorMessage").removeClass("hidden");
                }
            },
            error: function () {
            },
            complete: function () {
                recoverBet.unblockUI();
            }
        });
    },
    rederNoDataMatchInfo: function () {
        html = '<tr><td class="b c nonBorderBottom top" style="height: 19px;" colspan="3">There is no data.</td></tr>';
        return html;
    },
    rederNoDataBets: function () {
        html = '<tr><td class="b c nonBorderBottom top" style="height: 19px;" colspan="14">There is no data.</td></tr>';
        var frozenHeader = '<tr><td class="b c nonBorderBottom top" style="height: 19px;" colspan="3">&nbsp</td></tr>';
        $("#betListInfo_RowHeader tbody").empty().append(frozenHeader);
        return html;
    },
    displayMatchInfo: function (rs) {
        var html = "";
        if (rs) {
            html += '<tr class="h3 c">\
                        <td class="nonBorderBottom top"><div style="height: 17px;" class="col120">' + rs.MatchCode + '</div></td>\
                        <td class="nonBorderBottom top" nowrap=""><div style="height: 17px;" class="col300">' + rs.HomeName + (rs.AwayName != '' ? '-vs-' + rs.AwayName : '') + '</div></td>\
                        <td class="nonBorderBottom top"><div style="height: 17px;" style="width: 150px;">' + rs.EventDate + '</div></td>\
                     </tr>';
        }
        else {
            html = recoverBet.rederNoDataMatchInfo();
        }
        return html;
    },
    displayBets: function (rs) {
        var html = "";
        var frozenHeader = "";
        if (rs && rs.TransactionInfo && rs.TransactionInfo.length > 0) {
            var totalInfo = rs.TransactionInfo.length;
            recoverBet.isHavingBetsInfo = true;
            var matchInfoHtml = recoverBet.displayMatchInfo(rs);
            $("#matchInfo_RowHeader tbody").empty().append(matchInfoHtml);
            $("#matchInfo_Container tbody").empty().append(matchInfoHtml);
            var classSuffix = "";
            for (var i = 0; i < totalInfo; i++) {
                var detail = rs.TransactionInfo[i];
                if (i == totalInfo - 1) {
                    classSuffix = 'class="nonBorderBottom top"';
                }
                else {
                    classSuffix = 'class="top"';
                }
                html += '<tr bgcolor="white" class="c">\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col30 table-cell">' + detail.Order + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col110 table-cell">' + detail.Account + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col80 table-cell">' + detail.Refno + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col80 table-cell">' + detail.Bettype + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col30 table-cell">' + detail.Betteam + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="b col40 table-cell">\
                                <font color="blue">' + detail.Odds + '</font>\
                                <div class="bl-leaguename">' + detail.OddType + '</div>\
                            </div></td>\
                            <td ' + classSuffix + '><div style="text-align: right;height: 25px;" class="b col80 table-cell">\
                                <font color="red">' + detail.Amount + '</font></div>\
                            </div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col70 table-cell">' + detail.Currency + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col30 table-cell">' + detail.Hdp + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col120 table-cell">' + detail.TransTime + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col40 table-cell">' + detail.Score + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col110 table-cell">' + detail.Creator + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col60 table-cell">' + detail.Status + '</div></td>\
                            <td ' + classSuffix + '><div style="height: 25px;" class="col60 table-cell">\
                                <img class="hand" src="../App_Themes/Images/undo.png" style="border:0px;" onclick="recoverBet.recoverBet(\'' + detail.Refno + '\');" />\
                            </div></td>\
                        </tr>';

                frozenHeader += '<tr bgcolor="white" class="c">\
                                        <td ' + classSuffix + '><div style="height: 25px;" class="col30 table-cell">' + detail.Order + '</div></td>\
                                        <td ' + classSuffix + '><div style="height: 25px;" class="col110 table-cell">' + detail.Account + '</div></td>\
                                        <td ' + classSuffix + '><div style="height: 25px;" class="col80 table-cell">' + detail.Refno + '</div></td>\
                                    </tr>';
            }
            $("#betListInfo_RowHeader tbody").empty().append(frozenHeader);
        }
        else {
            recoverBet.isHavingBetsInfo = false;
            html = recoverBet.rederNoDataBets();
        }
        return html;
    },
    recoverBet: function (transid) {
        if (!confirm('Are you sure you want to recover transaction ' + transid + ' ?')) {
            return;
        }

        try { parent.BlockUI(); } catch (e) { }

        var data =
        {
            'transid': transid
        };
        $.ajax({
            type: "POST",
            url: "../RecoverBets/RecoverBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $("#dvContent").addClass("hidden");
                $("#matchInfo").addClass("hidden");
                $("#betListInfo").addClass("hidden");
                if (rs.ErrorCode > 0) {
                    showAlertMessage("ErrorMessage", rs.ErrorMessage, 1);
                }
                else if (rs.ErrorMessage != '') {
                    showAlertMessage("ErrorMessage", rs.ErrorMessage, 0);
                }
                $("#ErrorMessage").removeClass("hidden");
                recoverBet.unblockUI();
            }
        });
    },
    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },
    unblockUI: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    },
    checkDecNumber: function (e) {
        var code = "";
        if (e.which) {
            code = e.which;
        }
        else if (e.keyCode) {
            code = e.keyCode;
        }

        if (code == 37 || code == 39 || code == 36 || code == 35 || code == 118) {
            //e or E or "-"
            return true;
        }
        if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9) {
            return false;
        }
        return true;
    },
    make2TableBodySameHeight: function (tableA, tableB) {
        var rowsA = tableA.find("tbody tr");
        var rowsB = tableB.find("tbody tr");
        var numberOfRows = (rowsA.length < rowsB.length) ? rowsA.length : rowsB.length;
        for (i = 0; i < numberOfRows; i++) {
            var currentRowA = $(rowsA[i]);
            var currentRowB = $(rowsB[i]);
            var cellsA = currentRowA.find("td");
            var cellsB = currentRowB.find("td");
            var cellsALength = (cellsA ? cellsA.length : 0);
            var cellsBLength = (cellsB ? cellsB.length : 0);
            var numberOfCells = (cellsALength < cellsBLength) ? cellsALength : cellsBLength;
            for (j = 0; j < numberOfCells; j++) {
                var currentHeightA = $(cellsA[j]).height();
                var currentHeightB = $(cellsB[j]).height();
                if (currentHeightA > currentHeightB && currentHeightA > 4) {
                    $(cellsB[j]).css("height", currentHeightA + "px");
                }
                else if (currentHeightA != currentHeightB) {
                    $(cellsA[j]).css("height", currentHeightB + "px");
                }
            }
        }
    }
};


$(document).ready(function () {
    recoverBet.initPage();
});