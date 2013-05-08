$(document).ready(function () {
    Helper.initCalendar(_today);
    VirtualSportWinLoss.selectLicensees();
    VirtualSportWinLoss.loadSportType();
    var gridOpts = {
        tblContentId: 'tblContent',
        isShowPager: false
    };
    $("#virtualsportwinloss").initGrid(gridOpts);
});
var Helper = {
    initCalendar: function (_today) {
        var lastdate = '';
        lastdate = lastMonth + '/' + lastDay + '/' + lastYear;
        $('#spMonth').monPickr({
            today: lastdate,
            maxDate: _today,
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            log: function (msg) {
                console.warn(msg);
            }
        });
    }

};

var VirtualSportWinLoss = {
    fromDate: '',
    toDate: '',
    normalLicensse: [],
    operators: [],
    sportList: [],
    specificPeriodIndex: 7,
    maxHeightOfLicenseesDropdown: 400,
    minWidthOfLicenseesDropdown: 150,
    selectLicensees: function () {
        $("#InformationMssg").empty();
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "VirtualSportsWinlossByLicensee/VirtualSportsLicensees",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{ "productCode": "VS", "isExcludeBA": 1, "isExcludeTest": 1}',
            success: function (rs) {
                if (rs && rs.Licensee && rs.Licensee.length > 0) {
                    VirtualSportWinLoss.renderLicensees(rs.Licensee);
                }
                else {
                    VirtualSportWinLoss.clearAllDataWhenLincenseeEmpty();
                }
            },
            error: function () {
                VirtualSportWinLoss.clearAllDataWhenLincenseeEmpty();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    clearAllDataWhenLincenseeEmpty: function () {
        $("#ddlLicenseese").multiSelect({
            minWidth: VirtualSportWinLoss.minWidthOfLicenseesDropdown,
            maxHeight: VirtualSportWinLoss.maxHeightOfLicenseesDropdown,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            listData: [],
            noItemText: 'No licensee found.'
        });
        showAlertMessage("InformationMssg", "No Licensee found.", 1);
    },
    renderLicensees: function (licensees) {
        var licenseesTempData = [];
        var totalLicensees = licensees.length;
        var oldResellerId = 0;
        var oldResellerName = '';
        var distributors = [];
        var licenseeByReseller = [];
        var oldOperatorResellerId = 0;
        for (var i = 0; i < totalLicensees; i++) {
            var licensee = licensees[i];
            var currentResellerId = licensee.ResellerId;
            var currentResellerName = licensee.ResellerName;
            var currentResellerType = licensee.ResellerType;
            var licenseeId = licensee.LicenseeId;
            var licenseeName = licensee.LicenseeName;

            if (currentResellerType == 1) {
                if (oldResellerId != 0 && currentResellerId != oldResellerId) {
                    distributors.push({
                        Id: "#ddlLicenseese" + oldResellerId,
                        Value: oldResellerId,
                        Text: oldResellerName,
                        Checked: true,
                        Enable: true,
                        CssClass: "",
                        Items: licenseeByReseller
                    });
                    licenseeByReseller = [];
                    oldResellerId = 0;
                    oldResellerName = '';
                }
                licenseeByReseller.push({
                    Id: "#ddlLicenseeseDetail" + licenseeId,
                    Value: licenseeId,
                    Text: licenseeName,
                    Checked: true,
                    Enable: true,
                    CssClass: ""
                });
                oldResellerId = currentResellerId;
                oldResellerName = currentResellerName;
            }
            else if (currentResellerType == 2) {
                if (currentResellerId != oldOperatorResellerId) {
                    licenseesTempData.push({
                        Id: "#ddlLicenseese" + currentResellerId,
                        Value: currentResellerId,
                        Text: currentResellerName,
                        Checked: true,
                        Enable: true,
                        CssClass: "",
                        Items: []
                    });
                }
                oldOperatorResellerId = currentResellerId;
            }
        }
        //assign latest licensee group
        if (licenseeByReseller && licenseeByReseller.length > 0) {
            distributors.push({
                Id: "#ddlLicenseese" + oldResellerId,
                Value: oldResellerId,
                Text: oldResellerName,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: licenseeByReseller
            });
            licenseeByReseller = [];
        }
        //append the distributors to main data
        licenseesTempData = licenseesTempData.concat(distributors);
        $("#ddlLicenseese").multiSelect(
        {
            minWidth: VirtualSportWinLoss.minWidthOfLicenseesDropdown,
            maxHeight: VirtualSportWinLoss.maxHeightOfLicenseesDropdown,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All Licensees',
            listData: licenseesTempData,
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Operators/Resellers',
            onCheck: function () {
                if ($('#ddlLicenseese').multiSelect('getCheckedItems').length <= 0) {
                    showAlertMessage("InformationMssg", "Please choose Licensee!", 1);
                }
                else {
                    $('#InformationMssg').empty();
                }
            }
        });
    },
    getLicenseesSelected: function () {
        var listChecked = $('#ddlLicenseese').next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $('#ddlLicenseese').attr('id') + "HeaderCheckBox)").filter('[checked]');
        var normalLicensees = [];
        var operators = [];
        for (var i = 0; i < listChecked.length; i++) {
            if (listChecked[i].id.indexOf('Detail') != -1) {
                normalLicensees.push(listChecked[i].value);
            }
            else {
                operators.push(listChecked[i].value);
            }
        }
        var result = { 'normal': normalLicensees, 'operators': operators }
        return result;
    },
    loadSportType: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "VirtualSportsWinlossByLicensee/SelAllVirtualSportName",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                if (rs.length > 0) {
                    VirtualSportWinLoss.renderSportTypeSelected(rs, 'ddlSports');
                }

            }
        });
    },

    renderSportTypeSelected: function (rs, id) {
        var tempData = [];
        for (var i = 0; i < rs.length; i++) {
            tempData.push({
                Id: id + rs[i].SportId.toString(),
                Value: rs[i].SportId,
                Text: rs[i].SportName,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });

        }
        $("#" + id).multiSelect(
        {
            minWidth: 180,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            listData: tempData,
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Selected',
            onCheck: function () {
                if ($('#ddlSports').multiSelect('getCheckedItems').length <= 0) {
                    showAlertMessage("InformationMssg", "Please choose Sport!", 1);
                }
                else {
                    $('#InformationMssg').empty();
                }
            }
        });
    },
    getSelectedSport: function () {
        var id = 'ddlSports';

        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";

        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }

        result = result.replace(/,*$/, "");
        return result;
    },
    selWinLossReport: function () {
        var licenseesSeleted = VirtualSportWinLoss.getLicenseesSelected();
        VirtualSportWinLoss.normalLicensse = licenseesSeleted.normal;
        VirtualSportWinLoss.operators = licenseesSeleted.operators;
        VirtualSportWinLoss.fromDate = $('#spMonth').monPickr("getFromDate", "MM/dd/yyyy");
        VirtualSportWinLoss.toDate = $('#spMonth').monPickr("getToDate", "MM/dd/yyyy");
        VirtualSportWinLoss.sportList = VirtualSportWinLoss.getSelectedSport();
        $("#InformationMssg").empty();
        $('#virtualsportwinloss').addClass('hidden');
        if ($('#ddlLicenseese').multiSelect('getCheckedItems').length <= 0) {
            showAlertMessage("InformationMssg", "Please choose Licensee!", 1);
        }
        else if ($('#ddlSports').multiSelect('getCheckedItems').length <= 0) {
            showAlertMessage("InformationMssg", "Please choose Sport!", 1);
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: window.rootUrl + "VirtualSportsWinlossByLicensee/SelVirtualSportWinLoss",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{ "normalLicensee":"' + VirtualSportWinLoss.normalLicensse.join(',') + '", "operators":"' + VirtualSportWinLoss.operators.join(',') + '", "fromDate":"' + VirtualSportWinLoss.fromDate + '", "toDate":"' + VirtualSportWinLoss.toDate + '", "sportList":"' + VirtualSportWinLoss.sportList + '"}',
                success: function (rs) {
                    if (rs && rs.LicenseesWinLoss && rs.LicenseesWinLoss.length > 0) {
                        $('#reporttile').html('Virtual Sport Win Loss Report from ' + VirtualSportWinLoss.fromDate + ' to ' + VirtualSportWinLoss.toDate);
                        var totalGroups = rs.LicenseesWinLoss.length;
                        var arr = [];
                        var arrLeft = [];
                        for (var i = 0; i < totalGroups; i++) {
                            var groupData = rs.LicenseesWinLoss[i];
                            arr.push(VirtualSportWinLoss.renderTitleInfo(groupData.RevenueInfo));
                            arrLeft.push(VirtualSportWinLoss.renderTitleInfoLeftCols(groupData.RevenueInfo));
                            if (groupData.Licensees && groupData.Licensees.length > 0) {
                                var totalLicensees = groupData.Licensees.length;
                                for (var j = 0; j < totalLicensees; j++) {
                                    var licensee = groupData.Licensees[j];
                                    arr.push(VirtualSportWinLoss.renderTotal(licensee.WinLossTotal));
                                    arrLeft.push(VirtualSportWinLoss.renderTotalLeftCols(licensee.WinLossTotal));
                                    if (licensee.LicenseeWinLossDetail && licensee.LicenseeWinLossDetail.length > 0) {
                                        var totalRows = licensee.LicenseeWinLossDetail.length;
                                        for (var k = 0; k < totalRows; k++) {
                                            var detail = licensee.LicenseeWinLossDetail[k];
                                            arr.push(VirtualSportWinLoss.renderDetail(detail));
                                            arrLeft.push(VirtualSportWinLoss.renderDetailLeftCols(detail));
                                        }
                                    }
                                }
                            }

                        }

                        $('#virtualsportwinloss').find(".col_footer_content thead").empty().append(VirtualSportWinLoss.renderFooter(rs.GrandTotal));
                        $('#virtualsportwinloss').find(".row_footer_content thead").empty().append(VirtualSportWinLoss.renderFooterLeftCols(rs.GrandTotal));

                        $('#tblContent tbody').empty().append(arr.join(''));
                        $('#tblContentHeader tbody').empty().append(arrLeft.join(''));
                        var gridOpts = {
                            tblContentId: 'tblContent',
                            manualCopyHeader: true,
                            fullWidth: true,
                            fullHeight: true,
                            gridHeight: 140,
                            freezeWidth: 395,
                            numberFreezeCol: 4
                        };
                        $('#virtualsportwinloss').removeClass("hidden").gridScroller(gridOpts);
                    }
                    else {
                        showAlertMessage("InformationMssg", "There is no information available.", 1);
                    }
                },
                error: function () {
                    showAlertMessage("InformationMssg", "There is no information available.", 1);
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
       
        
    },
    renderTitleInfo: function (row) {
        var html = '<tr class="revenueInfo">\
            <td>\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
             <td>\
                <div class="c b i" style="width: 130px; height: 17px">' + row.DistributorRevenue + '</div>\
            </td>\
            <td>\
                <div class="c b i" style="width: 120px; height: 17px">' + row.ONEWorksIncome + '</div>\
            </td>\
            <td>\
                <div class="c b i" style="width: 100px; height: 17px">' + row.AmountDueToInspired + '</div>\
            </td>\
            <td>\
                <div class="c b i" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
        </tr>';
        return html;
    },
    renderTitleInfoLeftCols: function (row) {
        var html = '<tr>\
            <td style="border-right:none 0px;">\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td style="border-right:none 0px;>\
                <div class="r" style="width: 80px; height: 17px">&nbsp;</div>\
            </td>\
            <td style="border-right:none 0px;>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
        </tr>';
        return html;
    },
    renderTotal: function (row) {
        var html = '<tr class="winlossreseller">\
            <td>\
                <div class="c b" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 130px; height: 17px">' + row.DistributorRevenue + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">' + row.ONEWorksIncome + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.AmountDueToInspired + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.AmountDueToOneWorks + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderTotalLeftCols: function (row) {
        var html = '<tr class="winlossreseller">\
            <td>\
                <div class="l b" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderDetail: function (row) {
        var html = '<tr>\
            <td>\
                <div class="b c" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 130px; height: 17px">' + row.DistributorRevenue + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">' + row.ONEWorksIncome + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.AmountDueToInspired + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.AmountDueToOneWorks + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderDetailLeftCols: function (row) {
        var html = '<tr>\
            <td>\
                <div class="b r" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderEmptyRow: function () {
        var html = '<tr class="hideborderright">\
            <td>\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 130px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
        </tr>';
        return html;
    },
    renderEmptyRow_LeftCols: function () {
        var html = '<tr class="hideborderright">\
            <td>\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
        </tr>';
        return html;
    },
    renderFooter: function (row) {
        var html = '<tr class="grandtotal">\
           <td>\
                <div class="c" style="width: 100px; height: 17px">Total</div>\
            </td>\
            <td>\
                <div class="r" style="width: 80px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td style="border-right:solid 1px #B87801;">\
                <div class="r" style="width: 100px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 130px; height: 17px">' + row.DistributorRevenue + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 120px; height: 17px">' + row.ONEWorksIncome + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 100px; height: 17px">' + row.AmountDueToInspired + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 100px; height: 17px">' + row.AmountDueToOneWorks + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderFooterLeftCols: function (row) {
        var html = '<tr class="grandtotal">\
             <td>\
                <div class="r b" style="width: 100px; height: 17px">Total</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 80px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 100px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
        </tr>';
        return html;
    },
    exportExcel: function () {
        var urlRequest = window.rootUrl + 'VirtualSportsWinlossByLicensee/VirtualSportWinLoss?IsExport=true&normallicensee=' + VirtualSportWinLoss.normalLicensse.join(',') +
        '&operators=' + VirtualSportWinLoss.operators + '&fdate=' + VirtualSportWinLoss.fromDate + '&tdate=' + VirtualSportWinLoss.toDate +
        '&sportlist=' + VirtualSportWinLoss.sportList + '&reporttile=' + $('#reporttile').html() + '';

        $('#frm').attr('src', urlRequest);
    }
};