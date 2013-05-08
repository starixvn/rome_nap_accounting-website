var treeWinLossByCustomerLevel = {
    numberColor: function (number) {
        if (parseFloat(number) > 0)
            return "red";
        return "";
    },
    renderWlReport: function () {
        $('#reporttile').html(headerText);

        var arrFrozen = [];
        var arrScroll = [];
        var totalRows = wlDetail.length;
        for (var i = 0; i < totalRows; i++) {
            var row = wlDetail[i];
            arrFrozen.push(treeWinLossByCustomerLevel.renderWlReportFrozenRow(row));
            arrScroll.push(treeWinLossByCustomerLevel.renderWlReportScrollRow(row));
        }

        var htmlScrollContent = arrScroll.join('');
        var htmlFrozenContent = arrFrozen.join('');
        if ($.browser.msie && parseInt($.browser.version, 10) === 9) {
            htmlScrollContent = treeWinLoss.removeSpecialCharactersForIE9(htmlScrollContent);
            htmlFrozenContent = treeWinLoss.removeSpecialCharactersForIE9(htmlFrozenContent);
        }

        $('#tblContent tbody').empty().append(htmlScrollContent);
        $('#tblContentHeader tbody').empty().append(htmlFrozenContent);

        var gridOpts = {
            tblContentId: 'tblContent',
            isShowPager: false,
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 100,
            freezeWidth: 560,
            numberFreezeCol: 5
        };
        $('#mainreportgrid').initGrid(gridOpts).removeClass("hidden").gridScroller(gridOpts);
    },
    renderWlReportFrozenRow: function (row) {
        var downlineLinke = '<div class="c" style="width: 100px; height: 17px"><a href="javascript:;" onclick="treeWinLoss.goToDownLine(' + row.Id + ',\'' + row.Title + '\');">' + row.Title + '</a></div>';
        if (treeWinLoss.reportType == 4) {
            downlineLinke = '<div class="c" style="width: 100px; height: 17px">' + row.Title + '</div>';
        }

        var html = '<tr>\
                                <td>' + downlineLinke + '\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 120px; height: 17px">' + numeral(row.Turnover).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLossByCustomerLevel.numberColor(row.WinLoss)) + '" style="width: 120px; height: 17px">' + numeral(row.WinLoss).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 80px; height: 17px">' + row.Comm + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLossByCustomerLevel.numberColor(row.Total)) + '" style="width: 120px; height: 17px">' + row.Total + '</div>\
                                </td>\
                            </tr>';
        return html;
    },
    renderWlReportScrollRow: function (row) {
        var html = '<tr>\
                                <td>\
                                    <div class="c" style="width: 80px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 120px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 120px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 80px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 120px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 110px; height: 17px">' + row.BaseRMTurnover + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLossByCustomerLevel.numberColor(row.BaseRMTotal)) + '" style="width: 110px; height: 17px">' + row.BaseRMTotal + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLossByCustomerLevel.numberColor(row.BaseRMVirtualTotal)) + '" style="width: 110px; height: 17px">' + row.BaseRMVirtualTotal + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLossByCustomerLevel.numberColor(row.BaseRMVariance)) + '" style="width: 110px; height: 17px">' + row.BaseRMVariance + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: 80px; height: 17px"><a href="javascript:;" onclick="treeWinLossByCustomerLevel.getVirtualRateUpdateHistory(' + row.Id + ',\'' + row.Title + '\');">' + row.VirtualRate + '</a></div>\
                                </td>\
                            </tr>';
        return html;
    },
    getVirtualRateUpdateHistory: function (custId, custName) {
        try { parent.BlockUI(); } catch (e) { };
        var url = '../TreeWinLoss/VirtualRateUpdateHistory?customerId=' + custId;
        var title = 'Virtual Rate Update History (' + custName + ')';
        OpenPopup(url, title, 450, 320, false, false, window, 1000, null, null);
    }
};
