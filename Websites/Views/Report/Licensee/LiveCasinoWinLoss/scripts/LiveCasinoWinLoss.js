var liveCasinoWinLoss = {
    fromDate: '',
    toDate: '',
    normalLicensse: [],
    operators: [],
    specificPeriodIndex: 7,
    maxHeightOfLicenseesDropdown: 400,
    minWidthOfLicenseesDropdown: 150,
    changeReportDate: function () {
        $("#ddlDateSelection option").eq(liveCasinoWinLoss.specificPeriodIndex).attr('selected', 'selected');
    },

    getReportDateByCurrentSelection: function () {
        var selectValue = $('#ddlDateSelection').val();
        var dt = new Date(_today);
        var fDate = new Date(_today);
        var tDate = new Date(_today);
        var dateRange = [fDate, tDate];
        switch (parseInt(selectValue)) {
            case 0:
                dateRange = [dt, dt];
                break;
            case 1:
                fDate = new Date(_yesterday);
                dateRange = [fDate, fDate];
                break;
            case 2:
                dateRange = liveCasinoWinLossHelper.getDateRangeOfCurrentWeek();
                break;
            case 3:
                dateRange = liveCasinoWinLossHelper.getDateRangeOfLastWeek();
                break;
            case 4:
                dateRange = liveCasinoWinLossHelper.getDateRangeOfCurrentMonth();
                break;
            case 5:
                dateRange = liveCasinoWinLossHelper.getDateRangeOfMonths(1);
                break;
            case 6:
                fDate = new Date(dt.getFullYear(), dt.getMonth() - 1, 1);
                dateRange = [fDate, dt];
                break;
            case 7:
                fDate = new Date($('#txtfromdate').val());
                tDate = new Date($('#txttodate').val());
                dateRange = [fDate, tDate];
                break;
            default:
        }

        return dateRange;
    },

    setReportDateByCurrentSelection: function () {
        var dateRange = liveCasinoWinLoss.getReportDateByCurrentSelection();
        var fDate = dateRange[0];
        var tDate = dateRange[1];

        $("#txttodate").datepicker("option", "minDate", fDate);
        $("#txtfromdate").datepicker("option", "maxDate", tDate);
        $("#txttodate").datepicker("setDate", tDate);
        $("#txtfromdate").datepicker("setDate", fDate);
    },

    selectLicensees: function () {
        $("#InformationMssg").empty();
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LiveCasinoWLByLicensee/SelAllLicensees",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{ "productCode": "LC" }',
            success: function (rs) {
                if (rs && rs.Licensee && rs.Licensee.length > 0) {
                    var arrLicensee = rs.Licensee;

                    //temporary remove PIH reseller 
                    for (var i = 0; i < arrLicensee.length; i++)
                        if (arrLicensee[i].ResellerId === 8) { 
                            arrLicensee.splice(i, 1);
                            break;
                        }

                    liveCasinoWinLoss.renderLicensees(arrLicensee);

                } else {
                    liveCasinoWinLoss.clearAllDataWhenLincenseeEmpty();
                }
            },
            error: function () {
                liveCasinoWinLoss.clearAllDataWhenLincenseeEmpty();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    clearAllDataWhenLincenseeEmpty: function () {
        $("#ddlLicenseese").multiSelect({
            minWidth: liveCasinoWinLoss.minWidthOfLicenseesDropdown,
            maxHeight: liveCasinoWinLoss.maxHeightOfLicenseesDropdown,
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

            if (currentResellerType == 1 && licensee.ResellerId != 6) {
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
            minWidth: liveCasinoWinLoss.minWidthOfLicenseesDropdown,
            maxHeight: liveCasinoWinLoss.maxHeightOfLicenseesDropdown,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All Licensees',
            listData: licenseesTempData,
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

    selWinLossReport: function () {
        var licenseesSeleted = liveCasinoWinLoss.getLicenseesSelected();
        liveCasinoWinLoss.normalLicensse = licenseesSeleted.normal;
        liveCasinoWinLoss.operators = licenseesSeleted.operators;
        liveCasinoWinLoss.fromDate = $('#txtfromdate').val();
        liveCasinoWinLoss.toDate = $('#txttodate').val();

        $("#InformationMssg").empty();
        $('#livecasinowinloss').addClass('hidden');
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LiveCasinoWLByLicensee/SelLiveCasinoWinLoss",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{ "normalLicensee":"' + liveCasinoWinLoss.normalLicensse.join(',') + '", "operators":"' + liveCasinoWinLoss.operators.join(',') + '", "fromDate":"' + liveCasinoWinLoss.fromDate + '", "toDate":"' + liveCasinoWinLoss.toDate + '"}',
            success: function (rs) {
                if (rs && rs.LicenseesWinLoss && rs.LicenseesWinLoss.length > 0) {
                    $('#reporttile').html('Live Casino Win Loss Report from ' + liveCasinoWinLoss.fromDate + ' to ' + liveCasinoWinLoss.toDate);
                    var totalGroups = rs.LicenseesWinLoss.length;
                    var arr = [];
                    var arrLeft = [];
                    for (var i = 0; i < totalGroups; i++) {
                        var groupData = rs.LicenseesWinLoss[i];
                        arr.push(liveCasinoWinLoss.renderRevenueInfo(groupData.RevenueInfo));
                        arrLeft.push(liveCasinoWinLoss.renderRevenueInfoLeftCols(groupData.RevenueInfo));
                        if (groupData.Licensees && groupData.Licensees.length > 0) {
                            var totalLicensees = groupData.Licensees.length;
                            for (var j = 0; j < totalLicensees; j++) {
                                var licensee = groupData.Licensees[j];
                                arr.push(liveCasinoWinLoss.renderTotal(licensee.WinLossTotal));
                                arrLeft.push(liveCasinoWinLoss.renderTotalLeftCols(licensee.WinLossTotal));

                                if (licensee.LicenseeWinLossDetail && licensee.LicenseeWinLossDetail.length > 0) {
                                    var totalRows = licensee.LicenseeWinLossDetail.length;
                                    for (var k = 0; k < totalRows; k++) {
                                        var detail = licensee.LicenseeWinLossDetail[k];
                                        arr.push(liveCasinoWinLoss.renderDetail(detail));
                                        arrLeft.push(liveCasinoWinLoss.renderDetailLeftCols(detail));
                                    }
                                }
                            }
                        }

                        if (i < (totalGroups - 1)) {
                            arr.push(liveCasinoWinLoss.renderEmptyRow());
                            arrLeft.push(liveCasinoWinLoss.renderEmptyRow_LeftCols());
                        }
                    }

                    $('#livecasinowinloss').find(".col_footer_content thead").empty().append(liveCasinoWinLoss.renderFooter(rs.GrandTotal));
                    $('#livecasinowinloss').find(".row_footer_content thead").empty().append(liveCasinoWinLoss.renderFooterLeftCols(rs.GrandTotal));

                    $('#tblContent tbody').empty().append(arr.join(''));
                    $('#tblContentHeader tbody').empty().append(arrLeft.join(''));
                    var gridOpts = {
                        tblContentId: 'tblContent',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: 140,
                        freezeWidth: 435,
                        numberFreezeCol: 4
                    };
                    $('#livecasinowinloss').removeClass("hidden").gridScroller(gridOpts);
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
    },

    renderRevenueInfo: function (row) {
        var html = '<tr class="revenueInfo">\
            <td>\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
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
            <td>\
                <div class="c b i" style="width: 100px; height: 17px">' + row.DistributorRevenue + '</div>\
            </td>\
            <td>\
                <div class="c b i" style="width: 150px; height: 17px">' + row.StarExpressRevenue + '</div>\
            </td>\
            <td>\
                <div class="c b i" style="width: 200px; height: 17px">' + row.AmountDueToOneWorks + '</div>\
            </td>\
        </tr>';
        return html;
    },

    renderRevenueInfoLeftCols: function (row) {
        var html = '<tr>\
            <td style="border-right:none 0px;">\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td style="border-right:none 0px;>\
                <div class="r" style="width: 120px; height: 17px">&nbsp;</div>\
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
        var html = '<tr>\
            <td>\
                <div class="c b" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 100px; height: 17px">' + row.DistributorRevenue + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 150px; height: 17px">' + row.StarExpressRevenue + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 200px; height: 17px">' + row.AmountDueToOneWorks + '</div>\
            </td>\
        </tr>';
        return html;
    },

    renderTotalLeftCols: function (row) {
        var html = '<tr>\
            <td>\
                <div class="l b" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
        </tr>';
        return html;
    },

    renderDetail: function (row) {
        var html = '<tr class="winlossdetail">\
            <td>\
                <div class="c" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.TotalTransaction + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.GrossWin + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.DistributorRevenue + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 150px; height: 17px">' + row.StarExpressRevenue + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 200px; height: 17px">' + row.AmountDueToOneWorks + '</div>\
            </td>\
        </tr>';
        return html;
    },

    renderDetailLeftCols: function (row) {
        var html = '<tr class="winlossdetail">\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.Licensee + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">' + row.Turnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">' + row.TotalTransaction + '</div>\
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
                <div class="r" style="width: 120px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 150px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 200px; height: 17px">&nbsp;</div>\
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

    renderFooter: function (row) {
        var html = '<tr class="grandtotal">\
            <td>\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 120px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td style="border-right:solid 1px #B87801;">\
                <div class="r" style="width: 100px; height: 17px">&nbsp;</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 100px; height: 17px">Total</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 150px; height: 17px">' + row.StarExpressRevenue + '</div>\
            </td>\
            <td>\
                <div class="r b" style="width: 200px; height: 17px">' + row.AmountDueToOneWorks + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderFooterLeftCols: function (row) {
        var html = '<tr class="hideborderright grandtotal">\
            <td>\
                <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
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
    exportExcel: function () {
        var urlRequest = window.rootUrl + 'LiveCasinoWLByLicensee/LiveCasinoWinLoss?IsExport=true&normallicensee=' + liveCasinoWinLoss.normalLicensse.join(',') +
        '&operators=' + liveCasinoWinLoss.operators + '&fdate=' + liveCasinoWinLoss.fromDate + '&tdate=' + liveCasinoWinLoss.toDate +
        '&reporttile=' + $('#reporttile').html() + '';

        $('#frm').attr('src', urlRequest);
    }
}

var liveCasinoWinLossHelper = {
    renderCalendar: function (ids) {
        var defaults = {
            showOtherMonths: true,
            selectOtherMonths: true,
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                liveCasinoWinLoss.changeReportDate();
            }
        };

        var dates = $(ids).datepicker(defaults);
        var fd = new Date(liveCasinoWinLoss.fromDate);
        var td = new Date(liveCasinoWinLoss.toDate);

        var fdatePicker = $(ids.split(',')[0]);
        var tdatePicker = $(ids.split(',')[1]);
        fdatePicker.datepicker("setDate", fd);
        tdatePicker.datepicker("setDate", td);
    },
    getShortDateString: function (oDate) {
        // month is 0 based (0 - 11)
        var strDate = liveCasinoWinLossHelper.getDateStr(oDate.getMonth() + 1) + "/";
        strDate += liveCasinoWinLossHelper.getDateStr(oDate.getDate()) + "/";
        strDate += liveCasinoWinLossHelper.getDateStr(oDate.getFullYear());
        return strDate;
    },
    getDateStr: function (d) {
        if (!d) {
            return '';
        }
        if (d < 10) {
            return '0' + d.toString();
        }
        return d.toString();
    },
    getDateRangeOfCurrentWeek: function () {
        var _date = new Date(_today);
        var _lastDate = new Date(_today);

        var day = _date.getDay();
        diff = _date.getDate() - day; // adjust when day is sunday
        var startDate = new Date(_date.setDate(diff));
        startDate = _date.getMonth() < _lastDate.getMonth() ? new Date(_lastDate.getFullYear(), _lastDate.getMonth(), 1) : startDate;

        return [startDate, _lastDate];
    },
    getDateRangeOfLastWeek: function () {
        var lastWeek = new Date(_today);
        // set the date to last week's Sunday
        lastWeek.setDate(lastWeek.getDate() - lastWeek.getDay() - 7);
        var startDate = liveCasinoWinLossHelper.getShortDateString(lastWeek);
        // set the date to last week's Saturday
        lastWeek.setDate(lastWeek.getDate() + 6);
        var endDate = liveCasinoWinLossHelper.getShortDateString(lastWeek);

        return [new Date(startDate), new Date(endDate)];
    },
    getDateRangeOfCurrentMonth: function () {
        var today = new Date(_today);
        var startday = new Date(_today);
        startday.setDate(1);
        startday.setMonth(today.getMonth());
        var endday = new Date(today.setMonth(today.getMonth()));
        return [startday, endday];
    },
    getDateRangeOfMonths: function (totalMonths) {
        var cDate = new Date(_today);
        var currentMonth = cDate.getMonth();
        var currentYear = cDate.getFullYear();
        var lastYear = cDate.getFullYear() - 1;

        var startDate = new Date();
        var endDate = new Date();
//        if (totalMonths - (currentMonth - 1) > 0) {

//            var month = 12 - (totalMonths - (currentMonth));
//            startDate = new Date(lastYear, month, 1);
//            endDate = new Date(currentYear, (cDate.getMonth() - 1), (liveCasinoWinLossHelper.getTotalDayOfMonth((currentMonth), currentYear)));
//        }
//        else if (totalMonths - (currentMonth - 1) == 0) {
//            startDate = new Date(lastYear, 12, 1);
//            endDate = new Date(lastYear, 12, 31);
//        }
//        else {
//            var month = 12 - (totalMonths - (currentMonth));
//            startDate = new Date(lastYear, month, 1);
//            endDate = new Date(currentYear, (cDate.getMonth() - 1), (liveCasinoWinLossHelper.getTotalDayOfMonth((currentMonth), currentYear)));
//        }
        if (currentMonth == 0) {
            startDate = new Date(lastYear, 11, 1);
            endDate = new Date(lastYear, 11, 31);
        }
        else {
            var month = currentMonth - 1;
            startDate = new Date(currentYear, month, 1);
            endDate = new Date(currentYear,
                                    month,
            //// Add 1 month because month of JS is 0 > 11
                                    liveCasinoWinLossHelper.getTotalDayOfMonth(month + 1, currentYear)
                                    );
        }

        return [startDate, endDate];
    },
    getTotalDayOfMonth: function (month, year) {
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (month != 2) return m[month - 1];
        if (year % 4 != 0) return m[1];
        if (year % 100 == 0 && year % 400 != 0) return m[1];
        return m[1] + 1;
    }
};

$(document).ready(function () {
    $("#ddlDateSelection").change(function (event) {
        liveCasinoWinLoss.setReportDateByCurrentSelection();
    });

    var dateRangeDefault = liveCasinoWinLoss.getReportDateByCurrentSelection();
    liveCasinoWinLoss.fromDate = dateRangeDefault[0];
    liveCasinoWinLoss.toDate = dateRangeDefault[1];
    liveCasinoWinLossHelper.renderCalendar("#txtfromdate, #txttodate");

    liveCasinoWinLoss.selectLicensees();

    var gridOpts = {
        tblContentId: 'tblContent',
        isShowPager: false
    };
    $("#livecasinowinloss").initGrid(gridOpts);
});