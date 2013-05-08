/* File Created: August 17, 2012 */
$(function () {
    BetCount.init();
})

var BetCount = {
    minWidthOfLicenseesDropdown: 150,
    maxHeightOfLicenseesDropdown: 400,
    products: '',
    fromDate: '',
    toDate: '',
    sortType: '',
    sortCol: '',
    isGetHistoryData: false,
    init: function () {
        //#region assign events
        $('#btnviewhistory').on('click', function () { BetCount.changeToViewHistoryData(); }); //view history report link
        $('#btnviewnormal').on('click', function () { BetCount.changeToViewCurrentData(); }); //view current report link
        $('#ddlRangeDate').on('change', function () { BetCount.assignRangeDateEvent(); }); //range of date selection
        $('#btnReport').on('click', function () { BetCount.getBetCountWidthDateRange(); }); //report button
        $('#btnExport').on('click', function () { BetCount.exportToExcelFile(); }); //excel button
        //#region sorting
        $('#divSortingTurnover').on('click', function () { BetCount.sorting(this, 'TotalTurnover'); }); //sorting - Turnover
        $('#divSortingMemberWL').on('click', function () { BetCount.sorting(this, 'MemberWinloss'); }); //sorting - Member WL
        $('#divSortingMargin').on('click', function () { BetCount.sorting(this, 'MarginPercent'); }); //sorting - Margin
        $('#divSortingCompany').on('click', function () { BetCount.sorting(this, 'Company'); }); //sorting - Company WL (net winloss)
        $('#divSortingAgentWL').on('click', function () { BetCount.sorting(this, 'AgentWinLoss'); }); //sorting - Agent WL
        $('#divSortingMasterWL').on('click', function () { BetCount.sorting(this, 'MasterWinLoss'); }); //sorting - Master WL
        $('#divSortingSuperWL').on('click', function () { BetCount.sorting(this, 'SuperWinLoss'); }); //sorting - Super WL
        //#endregion
        //#endregion

        //assign products
        BetCount.assignProduct();

        // Set up Calendar
        dateHelper.renderCalendar();
        dateHelper.renderHistoryRangeDate();

        var gridOpts = {
            tblContentId: 'tblContent',
            isShowPager: false
        };
        $("#betCountReport").initGrid(gridOpts);
    },
    getBetCountWidthDateRange: function () {

        if (BetCount.getProductsSeleted().length <= 0) {
            return false;
        }
        $('.sorter').removeClass('asc').removeClass('desc');
        BetCount.sortType = '';
        $('#err').hide();
        try { parent.BlockUI(); } catch (e) { };
        $.ajax({
            type: "POST",
            url: "GetBetCountWithDateRange",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: BetCount.getInputData(),
            success: function (rs) {
                $('#mainreport').show();
                if (rs.HasData) {
                    BetCount.showForHasData(rs);
                }
                else {
                    BetCount.showNoInfoMessage();
                }
            },
            error: function (error) {
                BetCount.showErrorMessage("Error Occured!");
                $('#betCountReport').addClass('hidden');
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    showErrorMessage: function (message) {
        $('#mainBetCountTable').hide();
        $('#err').show();
        $('#err .alertboxerr div').html(message);
    },
    exportToExcelFile: function () {
        $("#frameExportExcel").attr("src", linkExportExcel + "?fromDate=" + BetCount.fromDate + "&toDate=" + BetCount.toDate + '&products=' + BetCount.products);
    },
    getInputData: function () {
        BetCount.fromDate = $('#txtFromDate').val();
        BetCount.toDate = $('#txtToDate').val();
        BetCount.products = BetCount.getProductsSeleted();

        if (BetCount.isGetHistoryData) {
            BetCount.fromDate = $('#ddlRangeOldDate').monPickr("getFromDate", "MM/dd/yyyy");
            BetCount.toDate = $('#ddlRangeOldDate').monPickr("getToDate", "MM/dd/yyyy");
        }
        var inputData = '{"fromDate":"' + BetCount.fromDate + '", "toDate":"' + BetCount.toDate + '", "products": "' + BetCount.products + '"}';
        return inputData;
    },
    showNoInfoMessage: function () {
        $('#betCountReport').addClass('hidden');
        BetCount.showErrorMessage('There is no information available.');
    },
    showForHasData: function (rs) {
        $('#betCountReport').removeClass('hidden');
        var arr = []; var arrFrozen = [];
        for (var i = 0; i < rs.SiteTicketGroups.length; i++) {
            var groupWinloss = rs.SiteTicketGroups[i];
            var css = 'totalColor'; var titleCss = 'l';
            arr.push(BetCount.renderWinLossRow(groupWinloss.TotalSiteTicket, css));
            arrFrozen.push(BetCount.renderWinLossFrozenRow(groupWinloss.TotalSiteTicket, css, titleCss));
            var totalRows = groupWinloss.SiteTickets.length;
            css = ''; titleCss = 'r';
            for (var j = 0; j < totalRows; j++) {
                var wl = groupWinloss.SiteTickets[j];
                arr.push(BetCount.renderWinLossRow(wl, css));
                arrFrozen.push(BetCount.renderWinLossFrozenRow(wl, css, titleCss));
            }
        }

        $('#betCountReport').find(".col_footer_content thead").empty().append(BetCount.renderWinLossFooterRow(rs.TotalBetCount));
        $('#betCountReport').find(".row_footer_content thead").empty().append(BetCount.renderWinLossFooterFrozenRow(rs.TotalBetCount));

        $('#tblContent tbody').empty().append(arr.join(''));
        $('#tblContentHeader tbody').empty().append(arrFrozen.join(''));
        var gridOpts = {
            tblContentId: 'tblContent',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 100,
            freezeWidth: 540,
            numberFreezeCol: 5
        };
        $('#betCountReport').removeClass("hidden").gridScroller(gridOpts);
    },
    changeToViewHistoryData: function () {
        $('.normalfillter').addClass('hidden');
        $('.historyfillter').removeClass('hidden');
        $('.selectedDayWrapParent').removeClass('hidden');
        $('.monPickrWrapParent').removeClass('hidden');

        BetCount.isGetHistoryData = true;
    },
    changeToViewCurrentData: function () {
        $('.historyfillter').addClass('hidden');
        $('.monPickrWrapParent').addClass('hidden');
        $('.selectedDayWrapParent').addClass('hidden');
        $('.normalfillter').removeClass('hidden');

        BetCount.isGetHistoryData = false;
    },
    assignProduct: function () {
        try { parent.BlockUI(); } catch (e) { };
        $.ajax({
            type: "POST",
            url: "GetAllProducts",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                if (rs) {
                    BetCount.renderProducts(rs);
                }
                else {
                    BetCount.clearAllDataWhenproductEmpty();
                }
            },
            error: function (error) {
                BetCount.clearAllDataWhenproductEmpty();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    clearAllDataWhenproductEmpty: function () {
        $("#ddlProduct").multiSelect({
            minWidth: BetCount.minWidthOfLicenseesDropdown,
            maxHeight: BetCount.maxHeightOfLicenseesDropdown,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            listData: [],
            noItemText: 'No product found.'
        });
        BetCount.showErrorMessage("No Product found.");
    },
    renderProducts: function (products) {
        var tempData = [];
        var id = "selectProducts";
        var totalProducts = products.length;
        for (var i = 0; i < totalProducts; i++) {
            var detail = products[i];
            var checked = detail.Id == 1 ? true : false;
            tempData.push({
                Id: id + detail.Id.toString(),
                Value: detail.Id,
                Text: detail.Name,
                Checked: checked,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }
        $("#ddlProduct").multiSelect(
        {
            minWidth: BetCount.minWidthOfLicenseesDropdown,
            maxHeight: BetCount.maxHeightOfLicenseesDropdown,
            shadow: true,
            showHeader: true,
            isCheckAll: false,
            showFooter: false,
            checkAllText: 'All',
            listData: tempData,
            onCheck: function () {
                BetCount.products = $('#ddlProduct').multiSelect('getCheckedItems');
                if (BetCount.products.length <= 0) {
                    BetCount.showErrorMessage("Please choose Product!");
                    BetCount.products = '-100';
                }
                else {
                    $('#err').hide();
                }
            },
            noItemText: 'No product found.'
        });
        BetCount.products = $('#selectBettypes').multiSelect('getCheckedItems');
    },
    assignRangeDateEvent: function () {
        var selectValue = $('#ddlRangeDate').val();
        var dt = new Date(defaultDate);
        var fDate = new Date(defaultDate);
        var tDate = new Date(defaultDate);
        var dateRange = [fDate, tDate];
        switch (parseInt(selectValue)) {
            case 0:
                dateRange = [fDate, fDate];
                break;
            case 1:
                dateRange = dateHelper.getDateRangeOfCurrentWeek();
                break;
            case 2:
                dateRange = dateHelper.getDateRangeOfLastWeek();
                break;
            case 3:
                dateRange = dateHelper.getDateRangeOfCurrentMonth();
                break;
            case 4:
                dateRange = dateHelper.getDateRangeOfMonths(1);
                break;
            case 5:
                fDate = new Date(dt.getFullYear(), dt.getMonth() - 1, 1);
                dateRange = [fDate, dt];
                break;
            case 6:
                fDate = new Date($('#txtFromDate').val());
                tDate = new Date($('#txtToDate').val());
                dateRange = [fDate, tDate];
                break;
            default:
        }

        $("#txtFromDate").datepicker("setDate", dateRange[0]);
        $("#txtToDate").datepicker("setDate", dateRange[1]);
    },
    renderWinLossRow: function (row, css) {
        var companyCss = parseFloat(row.Company) > 0 ? 'red' : '';
        var html = '<tr class="' + css + '">\
            <td>\
                <div class="r" style="width: 80px; height: 17px">' + row.SiteName + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 90px; height: 17px">' + row.NumOfTicket + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 130px; height: 17px">' + row.TotalTurnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 130px; height: 17px">' + row.MemberWinloss + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 90px; height: 17px">' + row.MarginPercent + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 110px; height: 17px">' + row.AgentWinLoss + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 110px; height: 17px">' + row.MasterWinLoss + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 110px; height: 17px">' + row.SuperWinLoss + '</div>\
            </td>\
            <td>\
                <div class="r ' + companyCss + '" style="width: 110px; height: 17px">' + row.Company + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderWinLossFrozenRow: function (row, css, titleCss) {
        var html = '<tr class="' + css + '">\
            <td>\
                <div class="' + titleCss + '" style="width: 80px; height: 17px">' + row.SiteName + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 90px; height: 17px">' + row.NumOfTicket + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 130px; height: 17px">' + row.TotalTurnover + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 130px; height: 17px">' + row.MemberWinloss + '</div>\
            </td>\
            <td>\
                <div class="r" style="width: 90px; height: 17px">' + row.MarginPercent + '</div>\
            </td>\
        </tr>';
        return html;
    },
    renderWinLossFooterRow: function (row) {
        var companyCss = parseFloat(row.Company) > 0 ? 'red' : '';
        var html = '<tr>\
            <th>\
                <div class="r" style="width: 80px; height: 17px">' + row.SiteName + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 90px; height: 17px">' + row.NumOfTicket + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 130px; height: 17px">' + row.TotalTurnover + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 130px; height: 17px">' + row.MemberWinloss + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 90px; height: 17px">' + row.MarginPercent + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 110px; height: 17px">' + row.AgentWinLoss + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 110px; height: 17px">' + row.MasterWinLoss + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 110px; height: 17px">' + row.SuperWinLoss + '</div>\
            </th>\
            <th>\
                <div class="r ' + companyCss + '" style="width: 110px; height: 17px">' + row.Company + '</div>\
            </th>\
        </tr>';
        return html;
    },
    renderWinLossFooterFrozenRow: function (row) {
        var html = '<tr>\
            <th>\
                <div class="c" style="width: 80px; height: 17px">' + row.SiteName + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 90px; height: 17px">' + row.NumOfTicket + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 130px; height: 17px">' + row.TotalTurnover + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 130px; height: 17px">' + row.MemberWinloss + '</div>\
            </th>\
            <th>\
                <div class="r" style="width: 90px; height: 17px">' + row.MarginPercent + '</div>\
            </th>\
        </tr>';
        return html;
    },
    getProductsSeleted: function () {
        if ($('#ddlProductHeaderCheckBox').attr('checked'))
            return '0';

        var listChecked = $('#ddlProduct').next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $('#ddlProduct').attr('id') + "HeaderCheckBox)").filter('[checked]');
        var tempArray = [];
        for (var i = 0; i < listChecked.length; i++) {
            tempArray.push(listChecked[i].value);
        }
        return tempArray.join(',');
    },
    sorting: function (obj, sortCol) {
        if (BetCount.sortType == '') {
            BetCount.sortType = 'asc';
        }
        if (BetCount.sortType == 'desc') {
            BetCount.sortType = 'asc';
        }
        else {
            BetCount.sortType = 'desc';
        }
        BetCount.sortCol = sortCol;
        var inputData = '{"fromDate":"' + BetCount.fromDate + '", "toDate":"' + BetCount.toDate +
        '", "products": "' + BetCount.products + '", "sortCol": "' + BetCount.sortCol + '", "sortType": "' + BetCount.sortType + '"}';
        $('#err').hide();
        $.ajax({
            type: "POST",
            url: "SortingBetCountReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: inputData,
            success: function (rs) {
                $('#mainreport').show();
                if (rs.HasData) {
                    BetCount.showForHasData(rs);
                    BetCount.addSortingIconAfterRender(obj);
                }
                else {
                    BetCount.showNoInfoMessage();
                }
            },
            error: function (error) {
                BetCount.showErrorMessage("Error Occured!");
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    addSortingIconAfterRender: function (obj) {
        $('.sorter').removeClass('asc').removeClass('desc');
        if (BetCount.sortType == 'asc') {
            $(obj).addClass("asc");
        }
        else {
            $(obj).addClass("desc");
        }
    }
};

//#region helper functions
var dateHelper = {

    renderCalendar: function () {
        $('#txtFromDate, #txtToDate').datepicker({
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            maxDate: "-1D",
            minDate: new Date((mindate.getMonth() + '/' + '1/' + mindate.getFullYear())),
            onSelect: function () {
                $("#ddlRangeDate option").eq(6).attr('selected', 'selected');
            }
        });
        $('#txtFromDate, #txtToDate').datepicker('setDate', defaultDate);
    },
    renderHistoryRangeDate: function () {
        var minday = '1/1/2009';

        $('#ddlRangeOldDate').monPickr({
            today: defaultDate,
            maxDate: defaultDate,
            minDate: minday,
            showDateRange: true,
            fromDayId: 'spFrom',
            toDayId: 'spTo',
            log: function (msg) {
                console.warn(msg);
            }
        });

        $('.monPickrWrapParent').addClass('hidden');
        $('.selectedDayWrapParent').addClass('hidden');
    },
    getShortDateString: function (oDate) {
        // month is 0 based (0 - 11)
        var strDate = dateHelper.getDateStr(oDate.getMonth() + 1) + "/";
        strDate += dateHelper.getDateStr(oDate.getDate()) + "/";
        strDate += dateHelper.getDateStr(oDate.getFullYear());
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
        var _date = new Date(defaultDate);
        var _lastDate = new Date(defaultDate);

        var day = _date.getDay();
        diff = _date.getDate() - day; // adjust when day is sunday
        var startDate = new Date(_date.setDate(diff));
        startDate = _date.getMonth() < _lastDate.getMonth() ? new Date(_lastDate.getFullYear(), _lastDate.getMonth(), 1) : startDate;

        return [startDate, _lastDate];
    },
    getDateRangeOfLastWeek: function () {
        var lastWeek = new Date(defaultDate);
        // set the date to last week's Sunday
        lastWeek.setDate(lastWeek.getDate() - lastWeek.getDay() - 7);
        var startDate = dateHelper.getShortDateString(lastWeek);
        // set the date to last week's Saturday
        lastWeek.setDate(lastWeek.getDate() + 6);
        var endDate = dateHelper.getShortDateString(lastWeek);

        return [new Date(startDate), new Date(endDate)];
    },
    getDateRangeOfCurrentMonth: function () {
        var today = new Date(defaultDate);
        var startday = new Date(defaultDate);
        startday.setDate(1);
        startday.setMonth(today.getMonth());
        var endday = new Date(today.setMonth(today.getMonth()));
        return [startday, endday];
    },
    getDateRangeOfMonths: function (totalMonths) {
        var cDate = new Date(defaultDate);
        var currentMonth = cDate.getMonth();
        var currentYear = cDate.getFullYear();
        var lastYear = cDate.getFullYear() - 1;

        var startDate = new Date();
        var endDate = new Date();
//        if (totalMonths - (currentMonth - 1) > 0) {

//            var month = 12 - (totalMonths - (currentMonth));
//            startDate = new Date(lastYear, month, 1);
//            endDate = new Date(currentYear, (cDate.getMonth() - 1), (dateHelper.getTotalDayOfMonth((currentMonth), currentYear)));
//        }
//        else if (totalMonths - (currentMonth - 1) == 0) {
//            startDate = new Date(lastYear, 12, 1);
//            endDate = new Date(lastYear, 12, 31);
//        }
//        else {
//            var month = 12 - (totalMonths - (currentMonth));
//            startDate = new Date(lastYear, month, 1);
//            endDate = new Date(currentYear, (cDate.getMonth() - 1), (dateHelper.getTotalDayOfMonth((currentMonth), currentYear)));
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
                                    dateHelper.getTotalDayOfMonth(month + 1, currentYear)
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
//#endregion