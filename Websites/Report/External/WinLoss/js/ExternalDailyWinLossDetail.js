var GridWidth = 681;
var objglb = null;
var objmaster = null;
var objagent = null;
var objmember = null;

$(document).ready(function () {

    var option = '111111';

    $("#chkAll").attr('checked', (option.substring(0, 1) == 1) ? true : false);
    var i = 1;
    $("#chkItems input:checkbox").each(function () {
        this.checked = (option.substring(i, i + 1) == 1 || option.substring(0, 1) == 1) ? true : false;
        i++;
    });
    $("#chkAll").click(function () {
        $("#chkItems input:checkbox").attr('checked', this.checked);
    });

    $("#chkItems input:checkbox").click(function () {
        var checked = true;
        $("#chkItems input:checkbox").each(function () {
            if (!$(this).prop('checked') && !$(this).hasClass("hidden")) {
                checked = false;
                return;
            }
        });
        $("#chkAll").attr("checked", checked);

    });

    $('#superWLGrid').scroller();
    $('#masterWLGrid').scroller();
    $('#agentWLGrid').scroller();
    $('#memberWLGrid').scroller();
});
Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
    return this;
}

//function initMonthlyCalendar() {

function initMonthlyCalendar() {
    $('#monPickr1').monPickr({
        today: serverDate,
        minDate: '2010/1/1',
        //maxDate: maxdate,
        //toDay: toDay.getDate(),
        showDateRange: true,
        fromDayId: 'fromDay1',
        toDayId: 'toDay1',
        log: function (msg) {
            console.warn(msg);
        }
    });

    var today = new Date($('#btnCurrentDateReport').val());

    $('#monPickr1').monPickr("setMonth", today);
    $('#monPickr1').monPickr("setToDay", today.getDate());

    var yesterday = new Date($('#btnCurrentYesterdayReport').val());

    if (yesterday.getDate() <= today.getDate()) {
        $('#monPickr1').monPickr("setFromDay", yesterday.getDate());
    }
}

function CheckSelectAll() {
    var flag = true;
    $("#chkItems input:checkbox").each(function () {
        if (this.checked == false)
            flag = false;
    });
    $("#chkAll").attr('checked', flag);
}

var superWinLossReportDetail = {
    title: '',
    fdate: '',
    tdate: '',
    showSB: '1',
    showCasino: '1',
    showP2P: '1',
    showRB: '1',
    showFI: '1',
    showNG: '1',
    showBI: '1',
    showLC: '1',
    showVS: '1',
    getWinLossReport: function (type) {
        $("div").each(function () {
            if ($(this).hasClass("desc"))
                $(this).removeClass("desc");
            else if ($(this).hasClass("asc"))
                $(this).removeClass("asc");
        });
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        $('#err').html('');

        superWinLossReportDetail.getReportDate(type);

        superWinLossReportDetail.showSB = $('#chkItems_0').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showCasino = $('#chkItems_1').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showRB = $('#chkItems_2').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showFI = $('#chkItems_3').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showBI = $('#chkItems_4').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showNG = $('#chkItems_5').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showLC = $('#chkItems_7').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showVS = $('#chkItems_8').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showP2P = $('#chkItems_6').is(':checked') ? 1 : 0;
        var pInput = 'fromdate=' + superWinLossReportDetail.fdate + '-todate=' + superWinLossReportDetail.tdate +
                     '-showsb=' + superWinLossReportDetail.showSB + '-showcasino=' + superWinLossReportDetail.showCasino +
                     '-showrb=' + superWinLossReportDetail.showRB + '-showfi=' + superWinLossReportDetail.showFI +
                     '-showbi=' + superWinLossReportDetail.showBI + '-showng=' + superWinLossReportDetail.showNG + 
                     '-showlivecasino=' + superWinLossReportDetail.showLC + '-showp2p=' + superWinLossReportDetail.showP2P + '-showvs=' + superWinLossReportDetail.showVS;

        var p = {
            param: pInput,
            sorting: null
        };

        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailByCom",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    superWinLossReportDetail.title = 'Super Win/Loss Detail From ' + superWinLossReportDetail.fdate + ' To ' + superWinLossReportDetail.tdate;
                    $('#superWLGridTitle').html(superWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(superWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(superWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = superWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = superWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblSuperWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblSuperWL').empty().html(arr.join(''));
                    $('#tblfooterSuperWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterSuperWL').empty().html(footer);

                    $("#superWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);
                    //$('#superWLGrid .header-frozen').css('height', $('#superWLGrid .header-scroll').height());
                    if ($.browser.msie)
                        $('#superWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#superWLGrid .header-scroll').height() - 2);
                    else {
                        $('#superWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#superWLGrid .header-scroll').height());
                        if ($('#superWLGrid').height() <= 124) {
                            $('#superWLGrid .header-scroll').css('width', $('#superWLGrid .body-scroll').width());
                            $('#superWLGrid .footer-scroll').css('width', $('#superWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });

    },
    getReportDate: function (type) {
        switch (type) {
            case 1:
                if (dateMode == "FromTo") {
                    superWinLossReportDetail.fdate = $('#fdate').val();
                    superWinLossReportDetail.tdate = $('#tdate').val();
                }
                else {
                    superWinLossReportDetail.fdate = $('#monPickr1').monPickr("getFromDate", "MM/dd/yyyy");
                    superWinLossReportDetail.tdate = $('#monPickr1').monPickr("getToDate", "MM/dd/yyyy");
                }
                break;
            case 2:
                if (dateMode == "FromTo") {
                    $('#fdate').val($('#btnCurrentDateReport').val());
                    $('#tdate').val($('#btnCurrentDateReport').val());
                }
                else {
                    var today = new Date($('#btnCurrentDateReport').val());
                    $('#monPickr1').monPickr("setMonth", today);
                    $('#monPickr1').monPickr("setFromDay", today.getDate());
                    $('#monPickr1').monPickr("setToDay", today.getDate());
                }

                superWinLossReportDetail.fdate = $('#btnCurrentDateReport').val();
                superWinLossReportDetail.tdate = $('#btnCurrentDateReport').val();
                break;
            case 3:
                if (dateMode == "FromTo") {
                    $('#fdate').val($('#btnCurrentYesterdayReport').val());
                    $('#tdate').val($('#btnCurrentYesterdayReport').val());
                }
                else {
                    var yesterday = new Date($('#btnCurrentYesterdayReport').val());
                    $('#monPickr1').monPickr("setMonth", yesterday);
                    $('#monPickr1').monPickr("setFromDay", yesterday.getDate());
                    $('#monPickr1').monPickr("setToDay", yesterday.getDate());
                }

                superWinLossReportDetail.fdate = $('#btnCurrentYesterdayReport').val();
                superWinLossReportDetail.tdate = $('#btnCurrentYesterdayReport').val();
                break;
            default:
                if (dateMode == "FromTo") {
                    superWinLossReportDetail.fdate = $('#fdate').val();
                    superWinLossReportDetail.tdate = $('#tdate').val();
                }
                else {
                    superWinLossReportDetail.fdate = $('#monPickr1').monPickr("getFromDate", "MM/dd/yyyy");
                    superWinLossReportDetail.tdate = $('#monPickr1').monPickr("getToDate", "MM/dd/yyyy");
                }
                break;
        }
    },
    renderWLFrozenRow: function (row, css) {
        var html = '';
        html = '<tr class="' + css + '">\
                    <td><div class="col100 c"><span class="blue" onclick="superWinLossReportDetail.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</span></div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.MWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.MComm + '</div></td>\
                    <td><div class="col120 r">' + row.MTotal + '</div></td>\
                    <td><div class="col120 r">' + row.SWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.SComm + '</div></td>\
                    <td><div class="col120 r">' + row.STotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.MWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.MComm + '</div></td>\
                    <td><div class="col120 r">' + row.MTotal + '</div></td>\
                    <td><div class="col120 r">' + row.SWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.SComm + '</div></td>\
                    <td><div class="col120 r">' + row.STotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterFrozenRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.UserName + '</div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossDetailNew.aspx?isexport=true&rtitle=' + superWinLossReportDetail.title +
                  '&type=1&rfdate=' + superWinLossReportDetail.fdate + '&rtdate=' + superWinLossReportDetail.tdate +
                  '&showSB=' + superWinLossReportDetail.showSB + '&showCasino=' + superWinLossReportDetail.showCasino +
                  '&showP2P=' + superWinLossReportDetail.showP2P + '&showRB=' + superWinLossReportDetail.showRB +
                  '&showFI=' + superWinLossReportDetail.showFI + '&showNG=' + superWinLossReportDetail.showNG +
                  '&showBI=' + superWinLossReportDetail.showBI + '&showLC=' + superWinLossReportDetail.showLC +
                  '&showVS=' + superWinLossReportDetail.showVS;
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        masterWinLossReportDetail.getWinLossReport(uplineId);
    },
    sortingReport: function (obj, sortCol, type) {
        if (obj != objglb) {
            $(objglb).removeClass('asc');
            $(objglb).removeClass('desc');
        }
        var sortType = 0;
        if ($(obj).is('.desc')) {
            $(obj).removeClass('desc');
            $(obj).addClass('asc');
            sortType = 1;
        }
        else {
            $(obj).removeClass('asc');
            $(obj).addClass('desc');
            sortType = 2;
        }
        //$('#superWLGrid').addClass('hidden');
        //$('#masterWLGrid').addClass('hidden');
        //$('#agentWLGrid').addClass('hidden');
        //$('#memberWLGrid').addClass('hidden');
        $('#err').html('');

        //superWinLossReportDetail.getReportDate(type);

        superWinLossReportDetail.showSB = $('#chkItems_0').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showCasino = $('#chkItems_1').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showRB = $('#chkItems_2').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showFI = $('#chkItems_3').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showBI = $('#chkItems_4').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showNG = $('#chkItems_5').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showLC = $('#chkItems_7').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showP2P = $('#chkItems_6').is(':checked') ? 1 : 0;
        superWinLossReportDetail.showVS = $('#chkItems_8').is(':checked') ? 1 : 0;

        var sort = { fieldName: sortCol, IsDecimal: (sortCol.toLowerCase() != 'username' ? true : false), sortType: sortType };
       
      
        var pInput = 'fromdate=' + superWinLossReportDetail.fdate + '-todate=' + superWinLossReportDetail.tdate +
                     '-showsb=' + superWinLossReportDetail.showSB + '-showcasino=' + superWinLossReportDetail.showCasino +
                     '-showrb=' + superWinLossReportDetail.showRB + '-showfi=' + superWinLossReportDetail.showFI +
                     '-showbi=' + superWinLossReportDetail.showBI + '-showng=' + superWinLossReportDetail.showNG +
                     '-showlivecasino=' + superWinLossReportDetail.showLC + '-showp2p=' + superWinLossReportDetail.showP2P + '-showvs=' + superWinLossReportDetail.showVS;

        var p = {
            param: pInput,
            sorting: sort
        };

        //try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailByCom",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    superWinLossReportDetail.title = 'Super Win/Loss Detail From ' + superWinLossReportDetail.fdate + ' To ' + superWinLossReportDetail.tdate;
                    $('#superWLGridTitle').html(superWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(superWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(superWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = superWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = superWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblSuperWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblSuperWL').empty().html(arr.join(''));
                    $('#tblfooterSuperWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterSuperWL').empty().html(footer);

                    $("#superWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);
                    //$('#superWLGrid .header-frozen').css('height', $('#superWLGrid .header-scroll').height());
                    if ($.browser.msie)
                        $('#superWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#superWLGrid .header-scroll').height() - 2);
                    else {
                        $('#superWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#superWLGrid .header-scroll').height());
                        if ($('#superWLGrid').height() <= 124) {
                            $('#superWLGrid .header-scroll').css('width', $('#superWLGrid .body-scroll').width());
                            $('#superWLGrid .footer-scroll').css('width', $('#superWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    $('#superWLGrid').addClass('hidden');
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { $('#superWLGrid').addClass('hidden'); },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
        objglb = obj;
    }
};

var masterWinLossReportDetail = {
    title: '',
    uplineId: 0,
    fdate: '',
    tdate: '',
    showSB: '1',
    showCasino: '1',
    showP2P: '1',
    showRB: '1',
    showFI: '1',
    showNG: '1',
    showBI: '1',
    showLC: '1',
    showVS: '1',
    getWinLossReport: function (uplineId) {
        $("div").each(function () {
            if ($(this).hasClass("desc"))
                $(this).removeClass("desc");
            else if ($(this).hasClass("asc"))
                $(this).removeClass("asc");
        });
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        $('#err').html('');

        masterWinLossReportDetail.uplineId = uplineId;
        masterWinLossReportDetail.fdate = superWinLossReportDetail.fdate;
        masterWinLossReportDetail.tdate = superWinLossReportDetail.tdate;

        masterWinLossReportDetail.showSB = superWinLossReportDetail.showSB;
        masterWinLossReportDetail.showCasino = superWinLossReportDetail.showCasino;
        masterWinLossReportDetail.showRB = superWinLossReportDetail.showRB;
        masterWinLossReportDetail.showFI = superWinLossReportDetail.showFI;
        masterWinLossReportDetail.showBI = superWinLossReportDetail.showBI;
        masterWinLossReportDetail.showNG = superWinLossReportDetail.showNG;
        masterWinLossReportDetail.showP2P = superWinLossReportDetail.showP2P;
        masterWinLossReportDetail.showLC = superWinLossReportDetail.showLC;
        masterWinLossReportDetail.showVS = superWinLossReportDetail.showVS;

        var pInput = 'fromdate=' + masterWinLossReportDetail.fdate + '-todate=' + masterWinLossReportDetail.tdate +
                     '-showsb=' + masterWinLossReportDetail.showSB + '-showcasino=' + masterWinLossReportDetail.showCasino +
                     '-showrb=' + masterWinLossReportDetail.showRB + '-showfi=' + masterWinLossReportDetail.showFI +
                     '-showbi=' + masterWinLossReportDetail.showBI + '-showng=' + masterWinLossReportDetail.showNG +
                     '-showlivecasino=' + masterWinLossReportDetail.showLC + '-showp2p=' + superWinLossReportDetail.showP2P + '-showvs=' + masterWinLossReportDetail.showVS;
        var p = {
            custid: parseInt(uplineId),
            param: pInput,
            sorting: null,
            pageIndex: 0,
            pageSize: 0
        };

        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailBySuper",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    masterWinLossReportDetail.title = 'Master Win/Loss Detail From ' + masterWinLossReportDetail.fdate + ' To ' + masterWinLossReportDetail.tdate;
                    $('#masterWLGridTitle').html(masterWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(masterWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(masterWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = masterWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = masterWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblMasterWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblMasterWL').empty().html(arr.join(''));
                    $('#tblfooterMasterWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterMasterWL').empty().html(footer);

                    $("#masterWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);



                    if ($.browser.msie)
                        $('#masterWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#masterWLGrid .header-scroll').height() - 2);
                    else {
                        $('#masterWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#masterWLGrid .header-scroll').height());
                        if ($('#masterWLGrid').height() <= 124) {
                            $('#masterWLGrid .header-scroll').css('width', $('#masterWLGrid .body-scroll').width());
                            $('#masterWLGrid .footer-scroll').css('width', $('#masterWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });

    },
    renderWLFrozenRow: function (row, css) {
        var html = '';
        html = '<tr class="' + css + '">\
                    <td><div class="col100 c"><span class="blue" onclick="masterWinLossReportDetail.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</span></div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.MWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.MComm + '</div></td>\
                    <td><div class="col120 r">' + row.MTotal + '</div></td>\
                    <td><div class="col120 r">' + row.SWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.SComm + '</div></td>\
                    <td><div class="col120 r">' + row.STotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.MWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.MComm + '</div></td>\
                    <td><div class="col120 r">' + row.MTotal + '</div></td>\
                    <td><div class="col120 r">' + row.SWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.SComm + '</div></td>\
                    <td><div class="col120 r">' + row.STotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterFrozenRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.UserName + '</div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossDetailNew.aspx?isexport=true&rtitle=' + masterWinLossReportDetail.title +
                  '&type=2&rfdate=' + masterWinLossReportDetail.fdate + '&rtdate=' + masterWinLossReportDetail.tdate +
                  '&showSB=' + masterWinLossReportDetail.showSB + '&showCasino=' + masterWinLossReportDetail.showCasino +
                  '&showP2P=' + masterWinLossReportDetail.showP2P + '&showRB=' + masterWinLossReportDetail.showRB +
                  '&showFI=' + masterWinLossReportDetail.showFI + '&showNG=' + masterWinLossReportDetail.showNG +
                  '&showBI=' + masterWinLossReportDetail.showBI + '&showLC=' + masterWinLossReportDetail.showLC +
                  '&showvs=' + masterWinLossReportDetail.showVS + '&uplineId=' + masterWinLossReportDetail.uplineId;
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {

        agentWinLossReportDetail.getWinLossReport(uplineId);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLGrid').removeClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
    },
    sortingReport: function (obj, sortCol) {
        $('#err').html('');
        if (obj != objmaster) {
            $(objmaster).removeClass('asc');
            $(objmaster).removeClass('desc');
        }
        var sortType = 0;
        if ($(obj).is('.desc')) {
            $(obj).removeClass('desc');
            $(obj).addClass('asc');
            sortType = 1;
        }
        else {
            $(obj).removeClass('asc');
            $(obj).addClass('desc');
            sortType = 2;
        }

        var sort = { fieldName: sortCol, IsDecimal: (sortCol.toLowerCase() != 'username' ? true : false), sortType: sortType };

        var pInput = 'fromdate=' + masterWinLossReportDetail.fdate + '-todate=' + masterWinLossReportDetail.tdate +
                     '-showsb=' + masterWinLossReportDetail.showSB + '-showcasino=' + masterWinLossReportDetail.showCasino +
                     '-showrb=' + masterWinLossReportDetail.showRB + '-showfi=' + masterWinLossReportDetail.showFI +
                     '-showbi=' + masterWinLossReportDetail.showBI + '-showng=' + masterWinLossReportDetail.showNG +
                     '-showlivecasino=' + masterWinLossReportDetail.showLC+ '-showp2p=' + superWinLossReportDetail.showP2P + '-showvs=' + masterWinLossReportDetail.showVS;
        var p = {
            custid: parseInt(masterWinLossReportDetail.uplineId),
            param: pInput,
            sorting: sort,
            pageIndex: 0,
            pageSize: 0
        };

        //try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailBySuper",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    masterWinLossReportDetail.title = 'Master Win/Loss Detail From ' + masterWinLossReportDetail.fdate + ' To ' + masterWinLossReportDetail.tdate;
                    $('#masterWLGridTitle').html(masterWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(masterWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(masterWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = masterWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = masterWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblMasterWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblMasterWL').empty().html(arr.join(''));
                    $('#tblfooterMasterWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterMasterWL').empty().html(footer);

                    $("#masterWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);



                    if ($.browser.msie)
                        $('#masterWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#masterWLGrid .header-scroll').height() - 2);
                    else {
                        $('#masterWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#masterWLGrid .header-scroll').height());
                        if ($('#masterWLGrid').height() <= 124) {
                            $('#masterWLGrid .header-scroll').css('width', $('#masterWLGrid .body-scroll').width());
                            $('#masterWLGrid .footer-scroll').css('width', $('#masterWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
        objmaster = obj;
    }
};

var agentWinLossReportDetail = {
    title: '',
    uplineId: 0,
    fdate: '',
    tdate: '',
    showSB: '1',
    showCasino: '1',
    showP2P: '1',
    showRB: '1',
    showFI: '1',
    showNG: '1',
    showBI: '1',
    showLC: '1',
    showVS: '1',
    getWinLossReport: function (uplineId) {
        $("div").each(function () {
            if ($(this).hasClass("desc"))
                $(this).removeClass("desc");
            else if ($(this).hasClass("asc"))
                $(this).removeClass("asc");
        });
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        $('#err').html('');

        agentWinLossReportDetail.uplineId = uplineId;
        agentWinLossReportDetail.fdate = superWinLossReportDetail.fdate;
        agentWinLossReportDetail.tdate = superWinLossReportDetail.tdate;

        agentWinLossReportDetail.showSB = superWinLossReportDetail.showSB;
        agentWinLossReportDetail.showCasino = superWinLossReportDetail.showCasino;
        agentWinLossReportDetail.showRB = superWinLossReportDetail.showRB;
        agentWinLossReportDetail.showFI = superWinLossReportDetail.showFI;
        agentWinLossReportDetail.showBI = superWinLossReportDetail.showBI;
        agentWinLossReportDetail.showNG = superWinLossReportDetail.showNG;
        agentWinLossReportDetail.showP2P = superWinLossReportDetail.showP2P;
        agentWinLossReportDetail.showLC = superWinLossReportDetail.showLC;
        agentWinLossReportDetail.showVS = superWinLossReportDetail.showVS;
        var pInput = 'fromdate=' + agentWinLossReportDetail.fdate + '-todate=' + agentWinLossReportDetail.tdate +
                     '-showsb=' + agentWinLossReportDetail.showSB + '-showcasino=' + agentWinLossReportDetail.showCasino +
                     '-showrb=' + agentWinLossReportDetail.showRB + '-showfi=' + agentWinLossReportDetail.showFI +
                     '-showbi=' + agentWinLossReportDetail.showBI + '-showng=' + agentWinLossReportDetail.showNG + 
                     '-showlivecasino=' + agentWinLossReportDetail.showLC+ '-showp2p=' + superWinLossReportDetail.showP2P + '-showvs=' + agentWinLossReportDetail.showVS;
        var p = {
            custid: parseInt(uplineId),
            param: pInput,
            sorting: null,
            pageIndex: 0,
            pageSize: 0
        };

        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailByMaster",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    agentWinLossReportDetail.title = 'Agent Win/Loss Detail From ' + agentWinLossReportDetail.fdate + ' To ' + agentWinLossReportDetail.tdate;
                    $('#agentWLGridTitle').html(agentWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(agentWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(agentWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = agentWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = agentWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblAgentWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblAgentWL').empty().html(arr.join(''));
                    $('#tblfooterAgentWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterAgentWL').empty().html(footer);

                    $("#agentWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);

                    if ($.browser.msie)
                        $('#agentWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#agentWLGrid .header-scroll').height() - 2);
                    else {
                        $('#agentWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#agentWLGrid .header-scroll').height());
                        if ($('#agentWLGrid').height() <= 124) {
                            $('#agentWLGrid .header-scroll').css('width', $('#agentWLGrid .body-scroll').width());
                            $('#agentWLGrid .footer-scroll').css('width', $('#agentWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });

    },
    renderWLFrozenRow: function (row, css) {
        var html = '';
        html = '<tr class="' + css + '">\
                    <td><div class="col100 c "><span class="blue" onclick="agentWinLossReportDetail.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</span></div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.MWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.MComm + '</div></td>\
                    <td><div class="col120 r">' + row.MTotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.MWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.MComm + '</div></td>\
                    <td><div class="col120 r">' + row.MTotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterFrozenRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.UserName + '</div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossDetailNew.aspx?isexport=true&rtitle=' + agentWinLossReportDetail.title +
                  '&type=3&rfdate=' + agentWinLossReportDetail.fdate + '&rtdate=' + agentWinLossReportDetail.tdate +
                  '&showSB=' + agentWinLossReportDetail.showSB + '&showCasino=' + agentWinLossReportDetail.showCasino +
                  '&showP2P=' + agentWinLossReportDetail.showP2P + '&showRB=' + agentWinLossReportDetail.showRB +
                  '&showFI=' + agentWinLossReportDetail.showFI + '&showNG=' + agentWinLossReportDetail.showNG +
                  '&showBI=' + agentWinLossReportDetail.showBI + '&showLC=' + agentWinLossReportDetail.showLC +
                  '&showvs=' + agentWinLossReportDetail.showVS + '&uplineId=' + agentWinLossReportDetail.uplineId;
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        memberWinLossReportDetail.getWinLossReport(uplineId);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').removeClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
    },
    sortingReport: function (obj, sortCol) {
        $('#err').html('');
        if (obj != objagent) {
            $(objagent).removeClass('asc');
            $(objagent).removeClass('desc');
        }
        var sortType = 0;
        if ($(obj).is('.desc')) {
            $(obj).removeClass('desc');
            $(obj).addClass('asc');
            sortType = 1;
        }
        else {
            $(obj).removeClass('asc');
            $(obj).addClass('desc');
            sortType = 2;
        }

        var sort = { fieldName: sortCol, IsDecimal: (sortCol.toLowerCase() != 'username' ? true : false), sortType: sortType };
        var pInput = 'fromdate=' + agentWinLossReportDetail.fdate + '-todate=' + agentWinLossReportDetail.tdate +
                     '-showsb=' + agentWinLossReportDetail.showSB + '-showcasino=' + agentWinLossReportDetail.showCasino +
                     '-showrb=' + agentWinLossReportDetail.showRB + '-showfi=' + agentWinLossReportDetail.showFI +
                     '-showbi=' + agentWinLossReportDetail.showBI + '-showng=' + agentWinLossReportDetail.showNG + 
                     '-showlivecasino=' + agentWinLossReportDetail.showLC + '-showp2p=' + superWinLossReportDetail.showP2P + '-showvs=' + agentWinLossReportDetail.showVS;
        var p = {
            custid: parseInt(agentWinLossReportDetail.uplineId),
            param: pInput,
            sorting: sort,
            pageIndex: 0,
            pageSize: 0
        };

        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailByMaster",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    agentWinLossReportDetail.title = 'Agent Win/Loss Detail From ' + agentWinLossReportDetail.fdate + ' To ' + agentWinLossReportDetail.tdate;
                    $('#agentWLGridTitle').html(agentWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(agentWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(agentWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = agentWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = agentWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblAgentWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblAgentWL').empty().html(arr.join(''));
                    $('#tblfooterAgentWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterAgentWL').empty().html(footer);

                    $("#agentWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);

                    if ($.browser.msie)
                        $('#agentWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#agentWLGrid .header-scroll').height() - 2);
                    else {
                        $('#agentWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#agentWLGrid .header-scroll').height());
                        if ($('#agentWLGrid').height() <= 124) {
                            $('#agentWLGrid .header-scroll').css('width', $('#agentWLGrid .body-scroll').width());
                            $('#agentWLGrid .footer-scroll').css('width', $('#agentWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
        objagent = obj;
    }
};

var memberWinLossReportDetail = {
    title: '',
    uplineId: 0,
    fdate: '',
    tdate: '',
    showSB: '1',
    showCasino: '1',
    showP2P: '1',
    showRB: '1',
    showFI: '1',
    showNG: '1',
    showBI: '1',
    showLC: '1',
    showVS: '1',
    getWinLossReport: function (uplineId) {
        $("div").each(function () {
            if ($(this).hasClass("desc"))
                $(this).removeClass("desc");
            else if ($(this).hasClass("asc"))
                $(this).removeClass("asc");
        });
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        $('#err').html('');

        memberWinLossReportDetail.uplineId = uplineId;
        memberWinLossReportDetail.fdate = superWinLossReportDetail.fdate;
        memberWinLossReportDetail.tdate = superWinLossReportDetail.tdate;

        memberWinLossReportDetail.showSB = superWinLossReportDetail.showSB;
        memberWinLossReportDetail.showCasino = superWinLossReportDetail.showCasino;
        memberWinLossReportDetail.showRB = superWinLossReportDetail.showRB;
        memberWinLossReportDetail.showFI = superWinLossReportDetail.showFI;
        memberWinLossReportDetail.showBI = superWinLossReportDetail.showBI;
        memberWinLossReportDetail.showNG = superWinLossReportDetail.showNG;
        memberWinLossReportDetail.showP2P = superWinLossReportDetail.showP2P;
        memberWinLossReportDetail.showLC = superWinLossReportDetail.showLC;
        memberWinLossReportDetail.showVS = superWinLossReportDetail.showVS;
        var pInput = 'fromdate=' + memberWinLossReportDetail.fdate + '-todate=' + memberWinLossReportDetail.tdate +
                     '-showsb=' + memberWinLossReportDetail.showSB + '-showcasino=' + memberWinLossReportDetail.showCasino +
                     '-showrb=' + memberWinLossReportDetail.showRB + '-showfi=' + memberWinLossReportDetail.showFI +
                     '-showbi=' + memberWinLossReportDetail.showBI + '-showng=' + memberWinLossReportDetail.showNG + 
                     '-showlivecasino=' + memberWinLossReportDetail.showLC+ '-showp2p=' + superWinLossReportDetail.showP2P + '-showvs=' + memberWinLossReportDetail.showVS;
        var p = {
            custid: parseInt(uplineId),
            param: pInput,
            sorting: null,
            pageIndex: 0,
            pageSize: 0
        };

        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailByAgent",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    memberWinLossReportDetail.title = 'Member Win/Loss Detail From ' + memberWinLossReportDetail.fdate + ' To ' + memberWinLossReportDetail.tdate;
                    $('#memberWLGridTitle').html(memberWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(memberWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(memberWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = memberWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = memberWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblMemberWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblMemberWL').empty().html(arr.join(''));
                    $('#tblfooterMemberWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterMemberWL').empty().html(footer);

                    $("#memberWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);

                    if ($.browser.msie)
                        $('#memberWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#memberWLGrid .header-scroll').height() - 2);
                    else {
                        $('#memberWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#memberWLGrid .header-scroll').height());

                        if ($('#memberWLGrid').height() <= 124) {
                            $('#memberWLGrid .header-scroll').css('width', $('#memberWLGrid .body-scroll').width());
                            $('#memberWLGrid .footer-scroll').css('width', $('#memberWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });

    },
    renderWLFrozenRow: function (row, css) {
        var html = '';
        html = '<tr class="' + css + '">\
                    <td><div class="col100 c" >' + row.UserName + '</div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col120 r">' + row.WinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.Comm + '</div></td>\
                    <td><div class="col120 r">' + row.Total + '</div></td>\
                    <td><div class="col120 r">' + row.AWinLoss + '</div></td>\
                    <td><div class="col120 r">' + row.AComm + '</div></td>\
                    <td><div class="col120 r">' + row.ATotal + '</div></td>\
                    <td><div class="col120 r">' + row.Company + '</div></td>\
                </tr>';
        return html;
    },
    renderWLFooterFrozenRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.UserName + '</div></td>\
                    <td><div class="col120 r">' + row.Turnover + '</div></td>\
                    <td><div class="col120 r">' + row.GrossComm + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossDetailNew.aspx?isexport=true&rtitle=' + memberWinLossReportDetail.title +
                  '&type=4&rfdate=' + memberWinLossReportDetail.fdate + '&rtdate=' + memberWinLossReportDetail.tdate +
                  '&showSB=' + memberWinLossReportDetail.showSB + '&showCasino=' + memberWinLossReportDetail.showCasino +
                  '&showP2P=' + memberWinLossReportDetail.showP2P + '&showRB=' + memberWinLossReportDetail.showRB +
                  '&showFI=' + memberWinLossReportDetail.showFI + '&showNG=' + memberWinLossReportDetail.showNG +
                  '&showBI=' + memberWinLossReportDetail.showBI + '&showLC=' + memberWinLossReportDetail.showLC + 
                  '&showVS=' + memberWinLossReportDetail.showVS + '&uplineId=' + memberWinLossReportDetail.uplineId;
        $("#frm").attr('src', url);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').removeClass('hidden');
        $('#memberWLGrid').addClass('hidden');
    },
    sortingReport: function (obj, sortCol, uplineId) {
        $('#err').html('');
        if (obj != objmember) {
            $(objmember).removeClass('asc');
            $(objmember).removeClass('desc');
        }
        var sortType = 0;
        if ($(obj).is('.desc')) {
            $(obj).removeClass('desc');
            $(obj).addClass('asc');
            sortType = 1;
        }
        else {
            $(obj).removeClass('asc');
            $(obj).addClass('desc');
            sortType = 2;
        }

        var sort = { fieldName: sortCol, IsDecimal: (sortCol.toLowerCase() != 'username' ? true : false), sortType: sortType };
        var pInput = 'fromdate=' + memberWinLossReportDetail.fdate + '-todate=' + memberWinLossReportDetail.tdate +
                     '-showsb=' + memberWinLossReportDetail.showSB + '-showcasino=' + memberWinLossReportDetail.showCasino +
                     '-showrb=' + memberWinLossReportDetail.showRB + '-showfi=' + memberWinLossReportDetail.showFI +
                     '-showbi=' + memberWinLossReportDetail.showBI + '-showng=' + memberWinLossReportDetail.showNG + 
                     '-showlivecasino=' + memberWinLossReportDetail.showLC + '-showp2p=' + superWinLossReportDetail.showP2P + '&showvs=' + memberWinLossReportDetail.showVS;
        var p = {
            custid: parseInt(memberWinLossReportDetail.uplineId),
            param: pInput,
            sorting: sort,
            pageIndex: 0,
            pageSize: 0
        };

        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDetailByAgent",
            data: JSON.stringify(p),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    memberWinLossReportDetail.title = 'Member Win/Loss Detail From ' + memberWinLossReportDetail.fdate + ' To ' + memberWinLossReportDetail.tdate;
                    $('#memberWLGridTitle').html(memberWinLossReportDetail.title);
                    var arrForzen = [];
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var item = rs.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arrForzen.push(memberWinLossReportDetail.renderWLFrozenRow(item, css));
                        arr.push(memberWinLossReportDetail.renderWLRow(item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';
                    footerFrozen = memberWinLossReportDetail.renderWLFooterFrozenRow(rs.Total, 'footer');
                    footer = memberWinLossReportDetail.renderWLFooterRow(rs.Total, 'footer');
                    $('#tblMemberWLfrozen').empty().html(arrForzen.join(''));
                    $('#tblMemberWL').empty().html(arr.join(''));
                    $('#tblfooterMemberWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterMemberWL').empty().html(footer);

                    $("#memberWLGrid").removeClass("hidden").reRenderScroller(GridWidth, 380, false);

                    if ($.browser.msie)
                        $('#memberWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#memberWLGrid .header-scroll').height() - 2);
                    else {
                        $('#memberWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#memberWLGrid .header-scroll').height());

                        if ($('#memberWLGrid').height() <= 124) {
                            $('#memberWLGrid .header-scroll').css('width', $('#memberWLGrid .body-scroll').width());
                            $('#memberWLGrid .footer-scroll').css('width', $('#memberWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
        objmember = obj;
    }
};

var isInitedMonthlyCalendar = false;
var dateMode = "FromTo";
function SwitchCalendarType(type) {
    if (type == 1) {
        $("#tblDate1").hide();
        $("#tblDate2").show();
        if (!isInitedMonthlyCalendar) {
            initMonthlyCalendar();
            isInitedMonthlyCalendar = true;
        }
        $("#spMsg").empty().html("<a href='#' onclick='javascript:SwitchCalendarType(2);'>Back To Current Report</a>");
        dateMode = "Monthly";
    }
    else {
        $("#tblDate1").show();
        $("#tblDate2").hide();
        $("#spMsg").empty().html("You can see the report data up to 3 months. For more historic data, please click to <a onclick='javascript:SwitchCalendarType(1);' href='#'>View History Report</a>");
        dateMode = "FromTo";
    }
};
