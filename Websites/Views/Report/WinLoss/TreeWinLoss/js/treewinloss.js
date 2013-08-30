/* File Created: December 19, 2012 */
$(document).ready(function () {
     $("#ddlDateSelection").change(function (event) {
        treeWinLossHelper.setReportDateByCurrentSelection();
    });
    var dateRangeDefault = treeWinLossHelper.getReportDateByCurrentSelection();
    treeWinLossHelper.fromDate = dateRangeDefault[0];
    treeWinLossHelper.toDate = dateRangeDefault[1];
    treeWinLossHelper.renderCalendar("#txtfromdate, #txttodate");

    treeWinLoss.init();
});

var treeWinLoss = {
    sortClass: '',
    sortCol: '',
    sortColId: '',
    reportTitle: '',
    reportType: 0,
    params: null,
    currentCustomerLine: [{ Id: 0, Name: '' }, { Id: 1, Name: '' }, { Id: 2, Name: '' }, { Id: 3, Name: ''}],
    init: function () {
        //#region assign events
        $('#btnSubmit').on('click', function () {
            treeWinLoss.sortColId = '';
            treeWinLoss.sortCol = '';
            treeWinLoss.sortClass = '';
            treeWinLoss.getTreeWinLossReportByProducts(true);
        });

        //#endregion
        this.renderCalendar();
        this.getProducts();
    },
    renderCalendar: function () {
        $('#txtDate').datepicker({
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true,
            maxDate: maxdate,
            minDate: mindate
        });
        $('#txtDate').datepicker('setDate', maxdate);
    },
    getProducts: function () {
        var products = [{ Id: 'sb', Name: 'Sportsbook' },
                        { Id: 'ng', Name: 'Number Game' },
                        { Id: 'rb', Name: 'Racing' },
                        { Id: 'bi', Name: 'Bingo' },
                        { Id: 'cs', Name: 'Casino' },
                        { Id: 'lcs', Name: 'Live Casino' },
                        { Id: 'p2p', Name: 'Poker' },
                        { Id: 'vs', Name: 'Virtual Sports' }
                        ];
        treeWinLoss.renderProducts(products);
    },
    renderProducts: function (products) {
        var tempData = [];
        var id = "selectProducts";
        var totalProducts = products.length;
        for (var i = 0; i < totalProducts; i++) {
            var detail = products[i];
            tempData.push({
                Id: id + detail.Id.toString(),
                Value: detail.Id,
                Text: detail.Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }
        $("#ddlProduct").multiSelect(
        {
            minWidth: 200,
            maxHeight: 300,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            listData: tempData,
            onCheck: function () {
                if ($('#ddlProduct').multiSelect('getCheckedItems').length <= 0) {
                    treeWinLoss.showErrorMessage("Please choose Product!");
                }
                else {
                    $('#err').hide();
                }
            },
            noItemText: 'No product found.'
        });
    },
    clearAllDataWhenproductEmpty: function () {
        $("#ddlProduct").multiSelect({
            minWidth: 200,
            maxHeight: 300,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            listData: [],
            noItemText: 'No product found.'
        });
        treeWinLoss.showErrorMessage("Please choose Product!");
    },
    getParams: function () {
        treeWinLoss.params = {
            fromDate: $('#txtfromdate').val(),
            toDate: $('#txttodate').val(),
            isShowSB: $('#selectProductssb').prop('checked'),
            isShowCS: $('#selectProductscs').prop('checked'),
            isShowRB: $('#selectProductsrb').prop('checked'),
            isShowBI: $('#selectProductsbi').prop('checked'),
            isShowNG: $('#selectProductsng').prop('checked'),
            isShowP2P: $('#selectProductsp2p').prop('checked'),
            isShowLCS: $('#selectProductslcs').prop('checked'),
            isShowVS: $('#selectProductsvs').prop('checked')
        };
    },
    getTreeWinLossReportByProducts: function (isGetCurrentFilter) {
        $('#err').hide();
        try { $("#dialog-modal").dialog("destroy").remove(); } catch (e) { }
        $('#divFilterBox').css({ width: '100%' });

        if ($('#ddlProduct').multiSelect('getCheckedItems').length <= 0) {
            treeWinLoss.showErrorMessage("Please choose Product!");
            return false;
        }
        if (isGetCurrentFilter | treeWinLoss.params == null) {
            treeWinLoss.getParams();
        }

        treeWinLoss.reportType = 0;
        treeWinLoss.currentCustomerLine = [{ Id: 0, Name: '' }, { Id: 1, Name: '' }, { Id: 2, Name: '' }, { Id: 3, Name: ''}];
        var request = 'reportType=' + treeWinLoss.reportType +
                        '&uplineId=0' +
                        '&uplineName=all' +
                        '&fromDate=' + treeWinLoss.params.fromDate +
                        '&toDate=' + treeWinLoss.params.toDate +
                        '&isShowSB=' + treeWinLoss.params.isShowSB +
                        '&isShowNG=' + treeWinLoss.params.isShowNG +
                        '&isShowRB=' + treeWinLoss.params.isShowRB +
                        '&isShowLCS=' + treeWinLoss.params.isShowLCS +
                        '&isShowCS=' + treeWinLoss.params.isShowCS +
                        '&isShowP2P=' + treeWinLoss.params.isShowP2P +
                        '&isShowBI=' + treeWinLoss.params.isShowBI +
                        '&isShowVS=' + treeWinLoss.params.isShowVS;
        try { parent.BlockUI(); } catch (e) { };
        var newReq = new Date().getMilliseconds();
        $.ajax({
            type: "GET",
            url: "../TreeWinLoss/ReportContent/?" + request + '&newReq=' + newReq,
            dataType: "html",
            success: function (evt) {
                $('#mainreport').html(evt);
				// highlight row when mouseover
				treeWinLoss.registerGridMouseover();
            },
            error: function (req, status, error) {
                treeWinLoss.showErrorMessage(error);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    getTreeWinLossReportByCustomer: function (uplineId, title) {
        $('#err').hide();
        try { $("#dialog-modal").dialog("destroy").remove(); } catch (e) { }
        $('#divFilterBox').css({ width: "100%" });
        if (treeWinLoss.params == null) {
            if ($('#ddlProduct').multiSelect('getCheckedItems').length <= 0) {
                treeWinLoss.showErrorMessage("Please choose Product!");
                return false;
            }
            treeWinLoss.getParams();
        }

        var request = 'reportType=' + treeWinLoss.reportType +
                        '&uplineId=' + uplineId +
                        '&uplineName=' + title +
                        '&fromDate=' + treeWinLoss.params.fromDate +
                        '&toDate=' + treeWinLoss.params.toDate +
                        '&isShowSB=' + treeWinLoss.params.isShowSB +
                        '&isShowNG=' + treeWinLoss.params.isShowNG +
                        '&isShowRB=' + treeWinLoss.params.isShowRB +
                        '&isShowLCS=' + treeWinLoss.params.isShowLCS +
                        '&isShowCS=' + treeWinLoss.params.isShowCS +
                        '&isShowP2P=' + treeWinLoss.params.isShowP2P +
                        '&isShowBI=' + treeWinLoss.params.isShowBI +
                        '&isShowVS=' + treeWinLoss.params.isShowVS;
        try { parent.BlockUI(); } catch (e) { };
        var newReq = new Date().getMilliseconds();
        $.ajax({
            type: "GET",
            url: "../TreeWinLoss/ReportContent/?" + request + '&newReq=' + newReq,
            dataType: "html",
            success: function (evt) {
                $('#mainreport').html(evt);
            },
            error: function (req, status, error) {
                treeWinLoss.showErrorMessage(error);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    showErrorMessage: function (message) {
        $('#err').show();
        $('#err .alertboxerr div').html(message);
    },
    goToDownLine: function (uplineId, title) {
        treeWinLoss.sortColId = '';
        treeWinLoss.sortCol = '';
        treeWinLoss.sortClass = '';

        treeWinLoss.currentCustomerLine[treeWinLoss.reportType].Id = uplineId;
        treeWinLoss.currentCustomerLine[treeWinLoss.reportType].Name = title;

        treeWinLoss.reportType++;
        treeWinLoss.getTreeWinLossReportByCustomer(uplineId, title);
    },
    goBackUpLine: function () {
        treeWinLoss.sortColId = '';
        treeWinLoss.sortCol = '';
        treeWinLoss.sortClass = '';
        treeWinLoss.reportType -= 1;

        if (treeWinLoss.reportType != 0) {
            var id = treeWinLoss.currentCustomerLine[treeWinLoss.reportType - 1].Id;
            var name = treeWinLoss.currentCustomerLine[treeWinLoss.reportType - 1].Name;
            treeWinLoss.getTreeWinLossReportByCustomer(id, name);
        }
        else {

            treeWinLoss.getTreeWinLossReportByProducts(false);
        }
    },
    removeSpecialCharactersForIE9: function (content) {
        var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
        var html = content.replace(expr, '><');
        return html;
    },
    exportExcel: function () {
        var id = 0;
        var name = '';
        if (treeWinLoss.reportType != 0) {
            id = treeWinLoss.currentCustomerLine[treeWinLoss.reportType - 1].Id;
            name = treeWinLoss.currentCustomerLine[treeWinLoss.reportType - 1].Name;
        }
        var newReq = new Date().getMilliseconds();
        var request = '?reportTitle=' + treeWinLoss.reportTitle + '' +
                        '&reportType=' + treeWinLoss.reportType +
                        '&uplineId=' + id +
                        '&uplineName=' + name +
                        '&fromDate=' + treeWinLoss.params.fromDate +
                        '&toDate=' + treeWinLoss.params.toDate +
                        '&isShowSB=' + treeWinLoss.params.isShowSB +
                        '&isShowNG=' + treeWinLoss.params.isShowNG +
                        '&isShowRB=' + treeWinLoss.params.isShowRB +
                        '&isShowLCS=' + treeWinLoss.params.isShowLCS +
                        '&isShowCS=' + treeWinLoss.params.isShowCS +
                        '&isShowP2P=' + treeWinLoss.params.isShowP2P +
                        '&isShowBI=' + treeWinLoss.params.isShowBI +
                        '&isShowVS=' + treeWinLoss.params.isShowVS +
                        '&newReq=' + newReq;

        $("#frameExportExcel").attr("src", linkExportExcel + request);
    },
    /*numberColor: function (number, isVarial, varianValue) {
        var numberColor = "";
        if (parseFloat(number) > 0) {
            numberColor = 'red';
            if (isVarial && parseFloat(varianValue) == 0) {
                numberColor = '';
            }
        }
        return numberColor;
    },*/
	numberColor: function (number) {
        return number > 0 ? 'red' : '';
    },
	registerGridMouseover: function() {
		// highlight row when mouseover
		$('#tblContentHeader > tbody > tr').hover(function() {
			var className = $(this).attr('class');
			$('#tblContent > tbody > tr.' + className).addClass('highlight');
		}, function() {
			var className = $(this).attr('class');
			$('#tblContent > tbody > tr.' + className).removeClass('highlight');
		});

		$('#tblContent > tbody > tr').hover(function() {
			var className = $(this).attr('class');
			$('#tblContentHeader > tbody > tr.' + className).addClass('highlight');
		}, function() {
			var className = $(this).attr('class');
			$('#tblContentHeader > tbody > tr.' + className).removeClass('highlight');
		});
	}
};

var treeWinLossByProduct = {
    sorting: function (obj, sortCol) {
        if (treeWinLoss.sortClass == '' | treeWinLoss.sortClass == 'desc') {
            treeWinLoss.sortClass = 'asc';
        }
        else {
            treeWinLoss.sortClass = 'desc';
        }
        treeWinLoss.sortCol = sortCol;
        var newReq = new Date().getMilliseconds();
        var request = 'reportType=' + treeWinLoss.reportType +
                        '&uplineName=all' +
                        '&sortCol=' + sortCol +
                        '&sortType=' + treeWinLoss.sortClass +                                             
                        '&isShowSB=' + treeWinLoss.params.isShowSB +
                        '&isShowNG=' + treeWinLoss.params.isShowNG +
                        '&isShowRB=' + treeWinLoss.params.isShowRB +
                        '&isShowLCS=' + treeWinLoss.params.isShowLCS +
                        '&isShowCS=' + treeWinLoss.params.isShowCS +
                        '&isShowP2P=' + treeWinLoss.params.isShowP2P +
                        '&isShowBI=' + treeWinLoss.params.isShowBI +
                        '&isShowVS=' + treeWinLoss.params.isShowVS +
                        '&newReq=' + newReq;
        //        try { parent.BlockUI(); } catch (e) { };
        $.ajax({
            type: "GET",
            url: "../TreeWinLoss/SortingTreeWinLossReport/?" + request,
            dataType: "html",
            success: function (evt) {
                $('#mainreport').html(evt);
				treeWinLoss.registerGridMouseover();
            },
            error: function (req, status, error) {
                treeWinLoss.showErrorMessage(error);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

        treeWinLoss.sortColId = $(obj).attr('id');

    },
    addSortingIconAfterRender: function (obj) {
        treeWinLossByProduct.resetSorting();
        if (treeWinLoss.sortClass == 'asc') {
            $('#' + obj).addClass("asc");
        }
        else {
            $('#' + obj).addClass("desc");
        }
    },
    resetSorting: function () {
        $('.sorter').removeClass('asc').removeClass('desc');
    }
};

var treeWinLossByCustomerLevel = {
  pagerOpts: {
        callback: null,
        start_per_page: 100,
        step_per_page: 100,
        max_items_per_page: 500,
        items_per_page: 100,
        item_per_page_arr: [100, 200, 300, 400, 500],
        current_page: 1
    },
    paramsPaging:{
        PageIndex: 1,
        PageSize: 100,
    },
    renderWlReport: function () {
       var preLeft =  $('#mainreportgrid .x-scrollbar').scrollLeft();
       $('#mainreportgrid').removeClass("hidden");
        $('#reporttile').html(headerText);

        var arrFrozen = [];
        var arrScroll = [];
        var totalRows = wlDetail.length;
        var startRow = treeWinLossByCustomerLevel.paramsPaging.PageSize * (treeWinLossByCustomerLevel.paramsPaging.PageIndex - 1);
        var endRow = treeWinLossByCustomerLevel.paramsPaging.PageSize * treeWinLossByCustomerLevel.paramsPaging.PageIndex;
        endRow = endRow < totalRows ? endRow : totalRows;
        for (var i = startRow; i < endRow; i++) {
            var row = wlDetail[i];
            arrFrozen.push(treeWinLossByCustomerLevel.renderWlReportFrozenRow(row, i + 1));
            arrScroll.push(treeWinLossByCustomerLevel.renderWlReportScrollRow(row, i + 1));
        }

        var htmlScrollContent = arrScroll.join('');
        var htmlFrozenContent = arrFrozen.join('');
        if ($.browser.msie && parseInt($.browser.version, 10) === 9) {
            htmlScrollContent = treeWinLoss.removeSpecialCharactersForIE9(htmlScrollContent);
            htmlFrozenContent = treeWinLoss.removeSpecialCharactersForIE9(htmlFrozenContent);
        }

        $('#tblContent tbody').empty().append(htmlScrollContent);
        $('#tblContentHeader tbody').empty().append(htmlFrozenContent);
        if (treeWinLoss.sortColId) {
            treeWinLossByCustomerLevel.addSortingIconAfterRender(treeWinLoss.sortColId);
        }
        var gridOpts = {
            tblContentId: 'tblContent',
            isShowPager: true,
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridWidth: 1300, //=totalColumnWidth + totalColumn*4 +totalColumn 1260
            gridHeight: 160,
            freezeWidth: 595,
            numberFreezeCol: 6
        };
         
        treeWinLossByCustomerLevel.initPager(wlJson.WinLoss.length,  treeWinLossByCustomerLevel.paramsPaging.PageSize,  treeWinLossByCustomerLevel.paramsPaging.PageIndex - 1);
       
        var scroller = $('#mainreportgrid').removeClass("hidden").gridScroller(gridOpts);
        if($.browser.mozilla && parseFloat($.browser.version) <= 7) {
            $('#mainreportgrid .x-scrollbar').prop("scrollLeft",preLeft);
            $('#mainreportgrid .container').prop("scrollLeft", preLeft);
        }

		// highlight row when mouseover
		treeWinLoss.registerGridMouseover();
    },
    renderWlReportFrozenRow: function (row, no) {
        var downlineLinke = '<div class="c" style="width: 100px; height: 17px"><a href="javascript:;" onclick="treeWinLoss.goToDownLine(' + row.Id + ',\'' + row.Title + '\');">' + row.Title + '</a></div>';
        if (treeWinLoss.reportType == 4) {
            downlineLinke = '<div class="c" style="width: 100px; height: 17px">' + row.Title + '</div>';
        }

        var html = '<tr class="row' + no + '"><td>\
                                    <div class="c" style="width: 30px; height: 17px">'+ no +'</div>\
                                </td>\
                                <td>' + downlineLinke + '\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 120px; height: 17px">' +  numeral(row.Turnover).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.WinLoss)) + '" style="width: 120px; height: 17px">' + numeral(row.WinLoss).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (row.Comm != 0 ? 'red' : '') + '" style="width: 80px; height: 17px">' + numeral(row.Comm).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.Total)) + '" style="width: 120px; height: 17px">' + numeral(row.Total).format('0,0.00') + '</div>\
                                </td>\
                            </tr>';
        return html;
    },
    renderWlReportScrollRow: function (row, no) {
    var turnover = '', turnoverVirtual='';
    var margin = '', baseRMVarianceMargin = '';
    if( treeWinLoss.params.isShowSB == true ||
                        treeWinLoss.params.isShowNG  == true ||
                        treeWinLoss.params.isShowRB == true ||
                       treeWinLoss.params.isShowLCS  == true ||
                       treeWinLoss.params.isShowVS == true)
                       {
                            turnover = numeral(row.BaseRMTurnover).format('0,0.00');
                            turnoverVirtual = numeral(row.BaseRMVirtualTurnover).format('0,0.00');
                       }
                       else{
                            turnover = 'N/A';
                            turnoverVirtual = 'N/A';
                       }
	
	baseRMVarianceMargin = row.BaseRMTOVarianceAbsoluteTotal != 0 ? numeral(row.BaseRMVarianceMargin).format('0,0.00') : 'N/A';
    if(turnoverVirtual == 'N/A' || turnoverVirtual == '0.00')
    {
        margin = 'N/A';
    }
    else{ 
        margin =  numeral(row.Margin).format('0,0.00');
    }
        var html = '<tr class="row' + no + '"><td>\
                                    <div class="c" style="width: 30px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: 100px; height: 17px">&nbsp;</div>\
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
                                    <div class="r" style="width: 110px; height: 17px">' + turnover + '</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 110px; height: 17px">' +  turnoverVirtual + '</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 110px; height: 17px">' + numeral(row.BaseRMTOVariancePlus).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: 110px; height: 17px">' + numeral(row.BaseRMTOVarianceMinus).format('0,0.00') + '</div>\
                                </td>\
								<td>\
                                    <div class="r" style="width: 115px; height: 17px">' + numeral(row.BaseRMTOVarianceAbsoluteTotal).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.BaseRMWLVariancePlus)) + '" style="width: 110px; height: 17px">' + numeral(row.BaseRMWLVariancePlus.toFixed(2)).format('0,0.00') + '</div>\
                                </td>\
                                 <td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.BaseRMWLVarianceMinus)) + '" style="width: 110px; height: 17px">' +  numeral(row.BaseRMWLVarianceMinus.toFixed(2)).format('0,0.00') + '</div>\
                                </td>\
								<td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.BaseRMWLVarianceTotal)) + '" style="width: 110px; height: 17px">' +  numeral(row.BaseRMWLVarianceTotal.toFixed(2)).format('0,0.00') + '</div>\
                                </td>\
								<td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.BaseRMVarianceMargin)) + '" style="width: 110px; height: 17px">' + baseRMVarianceMargin + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.BaseRMTotal)) + '" style="width: 110px; height: 17px">' + numeral(row.BaseRMTotal).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.BaseRMVirtualTotal)) + '" style="width: 110px; height: 17px">' + numeral(row.BaseRMVirtualTotal).format('0,0.00') + '</div>\
                                </td>\
                                <td>\
                                    <div class="r ' + (treeWinLoss.numberColor(row.Margin)) + '" style="width: 110px; height: 17px">' + margin + '</div>\
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
    },
    sorting: function (obj, sortCol) {
        if (treeWinLoss.sortClass == '' | treeWinLoss.sortClass == 'desc') {
            treeWinLoss.sortClass = 'asc';
            sortSymbol = '';
        }
        else {
            treeWinLoss.sortClass = 'desc';
            sortSymbol = '-';
        }
        treeWinLoss.sortCol = sortCol;

        var newReq = new Date().getMilliseconds();
        var request = 'reportType=' + treeWinLoss.reportType +
                        '&uplineName=' + treeWinLoss.currentCustomerLine[treeWinLoss.reportType - 1].Name +
                        '&sortCol=' + sortCol +
                        '&sortType=' + treeWinLoss.sortClass +                      
                        '&isShowSB=' + treeWinLoss.params.isShowSB +
                        '&isShowNG=' + treeWinLoss.params.isShowNG +
                        '&isShowRB=' + treeWinLoss.params.isShowRB +
                        '&isShowLCS=' + treeWinLoss.params.isShowLCS +
                        '&isShowCS=' + treeWinLoss.params.isShowCS +
                        '&isShowP2P=' + treeWinLoss.params.isShowP2P +
                        '&isShowBI=' + treeWinLoss.params.isShowBI +
                        '&isShowVS=' + treeWinLoss.params.isShowVS +
                        '&newReq=' + newReq;
        $.ajax({
            type: "GET",
            url: "../TreeWinLoss/SortingTreeWinLossReport/?" + request,
            dataType: "html",
            success: function (evt) {
                $('#mainreport').html(evt);
            },
            error: function (req, status, error) {
                treeWinLoss.showErrorMessage(error);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

        treeWinLoss.sortColId = $(obj).attr('id');
       
        treeWinLossByCustomerLevel.renderWlReport();

    },
    addSortingIconAfterRender: function (obj) {
        treeWinLossByCustomerLevel.resetSorting();

        if (treeWinLoss.sortClass == 'asc') {
            $('#' + obj).addClass("asc");
        }
        else {
            $('#' + obj).addClass("desc");
        }
    },
    resetSorting: function () {
        $('.sorter').removeClass('asc').removeClass('desc');
        treeWinLoss.sortType = '';
    },
     initPager: function (total_items, items_per_page, current_page) {
        treeWinLossByCustomerLevel.pagerOpts.callback = treeWinLossByCustomerLevel.pageSelectCallback;
        treeWinLossByCustomerLevel.pagerOpts.items_per_page = items_per_page;
        treeWinLossByCustomerLevel.pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, treeWinLossByCustomerLevel.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(wlJson.WinLoss.length / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        treeWinLossByCustomerLevel.paramsPaging.PageIndex = page_index + 1;
        treeWinLossByCustomerLevel.paramsPaging.PageSize = _items_per_page;
        treeWinLossByCustomerLevel.renderWlReport();   
       
        return false;
    }
};

var timeoutId = 0;

var treeWinLossHelper = {
    fromDate: '',
    toDate: '',
    normalLicensse: [],
    operators: [],
    specificPeriodIndex: 7,
    maxHeightOfLicenseesDropdown: 400,
    minWidthOfLicenseesDropdown: 150,
    changeReportDate: function () {
        $("#ddlDateSelection option").eq(treeWinLossHelper.specificPeriodIndex).attr('selected', 'selected');
    },
    renderCalendar: function (ids) {
        var defaults = {
            showOtherMonths: true,
            selectOtherMonths: true,
            maxDate: maxdate,
            minDate: mindate,
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                treeWinLossHelper.changeReportDate();
            }
        };

        var dates = $(ids).datepicker(defaults);
        var fd = new Date(treeWinLossHelper.fromDate);
        var td = new Date(treeWinLossHelper.toDate);

        var fdatePicker = $(ids.split(',')[0]);
        var tdatePicker = $(ids.split(',')[1]);
        fdatePicker.datepicker("setDate", fd);
        tdatePicker.datepicker("setDate", td);
    },
     setReportDateByCurrentSelection: function () {
        var dateRange = treeWinLossHelper.getReportDateByCurrentSelection();
        var fDate = dateRange[0];
        var tDate = dateRange[1];

        $("#txttodate").datepicker("option", "minDate", fDate);
        $("#txtfromdate").datepicker("option", "maxDate", tDate);
        $("#txttodate").datepicker("setDate", tDate);
        $("#txtfromdate").datepicker("setDate", fDate);
    },
    getShortDateString: function (oDate) {
        // month is 0 based (0 - 11)
        var strDate = treeWinLossHelper.getDateStr(oDate.getMonth() + 1) + "/";
        strDate += treeWinLossHelper.getDateStr(oDate.getDate()) + "/";
        strDate += treeWinLossHelper.getDateStr(oDate.getFullYear());
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
        var startDate = treeWinLossHelper.getShortDateString(lastWeek);
        // set the date to last week's Saturday
        lastWeek.setDate(lastWeek.getDate() + 6);
        var endDate = treeWinLossHelper.getShortDateString(lastWeek);

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
                                    treeWinLossHelper.getTotalDayOfMonth(month + 1, currentYear)
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
                dateRange = treeWinLossHelper.getDateRangeOfCurrentWeek();
                break;
            case 3:
                dateRange = treeWinLossHelper.getDateRangeOfLastWeek();
                break;
            case 4:
                dateRange = treeWinLossHelper.getDateRangeOfCurrentMonth();
                break;
            case 5:
                dateRange = treeWinLossHelper.getDateRangeOfMonths(1);
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
};