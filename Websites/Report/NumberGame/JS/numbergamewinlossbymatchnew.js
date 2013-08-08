var _pageIndex = 1;
var _pageSize = 50;
var id = "";
var bettypename = "";
var matchid = 0;
var fromcache = false;
var preTop = 0;
var preLeft = 0;

$(document).ready(function () {
    var options = {
        showOtherMonths: true,
        selectOtherMonths: true,
        checkMinMax: true
    };
    calendarUtils.renderCalendar("#date", null, "#date", "", fDay, options);

    numberGameWinLossByMatch.getCompanies();
    numberGameWinLossByMatch.getCurrencies();
    numberGameWinLossByMatch.getBettyes();

    $("#matchwinlostdetail").scroller();
});

var numberGameWinLossByMatch = {
    currencies: '',
    companies: '',
    bettypes: '',
    servicePath: '../../Services/WCF/Report/ServiceReportNumberGame.svc/',
    fdate: '',
    title: '',
    getServicePath: function (type) {
        if (typeof type == "string" && type == "bettype") {
            numberGameWinLossByMatch.servicePath = "../../../Services/WCF/Report/ServiceReportNumberGame.svc/";
        }
    },
    getCurrencies: function () {
        $.ajax({
            type: "POST",
            url: numberGameWinLossByMatch.servicePath + "GetCurrencyList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                var tempData = [];
                var id = "selectCurrency";
                for (var i = 0; i < rs.length; i++) {
                    var detail = rs[i];
                    tempData.push({
                        Id: id + detail.Value.toString(),
                        Value: detail.Value,
                        Text: detail.Name,
                        Checked: true,
                        Enable: true,
                        CssClass: "",
                        Items: []
                    });
                }          
                $("#selectCurrency").multiSelect(
                {
                    minWidth: 140,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: 'All',
                    listData: tempData,
                    onCheck: function () {
                        numberGameWinLossByMatch.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
                        if (numberGameWinLossByMatch.currencies.length <= 0) {
                            showAlertMessage("err", "Please choose Currency!", 1);
                            numberGameWinLossByMatch.currencies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWinLossByMatch.currencies = $('#selectCurrency').multiSelect('getCheckedItems');

            },
            error: function () {
                numberGameWinLossByMatch.currencies = "-100";
                $("#selectCurrency").multiSelect(
                {
                    minWidth: 140,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: 'All',
                    listData: []
                });
            },
            complete: function () { }
        });
    },
    getBettyes: function () {
        $.ajax({
            type: "POST",
            url: numberGameWinLossByMatch.servicePath + "GetBettypeList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                var tempData = [];
                var id = "selectBettypes";
                for (var i = 0; i < rs.length; i++) {
                    var detail = rs[i];
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
                $("#selectBettypes").multiSelect(
                {
                    minWidth: 140,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: 'All',
                    listData: tempData,
                    onCheck: function () {
                        numberGameWinLossByMatch.bettypes = $('#selectBettypes').multiSelect('getCheckedItems');
                        if (numberGameWinLossByMatch.bettypes.length <= 0) {
                            showAlertMessage("err", "Please choose Bettype!", 1);
                            numberGameWinLossByMatch.bettypes = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWinLossByMatch.bettypes = $('#selectBettypes').multiSelect('getCheckedItems');

            },
            error: function () {
                numberGameWinLossByMatch.bettypes = "-100";
                $("#selectBettypes").multiSelect(
                {
                    minWidth: 140,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: 'All',
                    listData: []
                });
            },
            complete: function () { }
        });
    },
    getCompanies: function () {
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: numberGameWinLossByMatch.servicePath + "GetSiteList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                var companyGroup = [];
                companyGroup.push({
                    Id: "0IBCGroup",
                    Value: -1,
                    Text: "IBC Group",
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: []
                });
                var tempData = [];
                var id = "selectCompany";
                for (var i = 0; i < rs.length; i++) {
                    var detail = rs[i];
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
                companyGroup.push({
                    Id: "0Licensee",
                    Value: -1,
                    Text: "Licensee",
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: tempData
                });
                $("#selectCompany").multiSelect(
                {
                    minWidth: 140,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: 'All',
                    listData: companyGroup,
                    onCheck: function () {
                        numberGameWinLossByMatch.companies = $('#selectCompany').multiSelect('getCheckedItems');
                        if (numberGameWinLossByMatch.companies.length <= 0) {
                            showAlertMessage("err", "Please choose Company!", 1);
                            numberGameWinLossByMatch.companies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWinLossByMatch.companies = $('#selectCompany').multiSelect('getCheckedItems');
            },
            error: function () {
                numberGameWinLossByMatch.companies = '-100';
                $("#selectCompany").multiSelect(
                {
                    minWidth: 140,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: 'All',
                    listData: []
                });
            },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    GetDataReport: function () {
        $("#err").empty();
        $("#backbtn").hide();
        $("#matchwinlost").hide();
        $("#matchwinlostdetail").hide();

        var comSeleted = numberGameWinLossByMatch.companies;
        var curSeleted = numberGameWinLossByMatch.currencies;
        var bettypeSeleted = numberGameWinLossByMatch.bettypes;
        if (comSeleted == '-100' && curSeleted == '-100' && bettypeSeleted == '-100') {
            showAlertMessage("err", "Please choose Company, Bettype and Currency!", 1);
            return false;
        }
        else if (comSeleted == '-100' && curSeleted == '-100') {
            showAlertMessage("err", "Please choose Company and Currency!", 1);
            return false;
        }
        else if (comSeleted == '-100' && bettypeSeleted == '-100') {
            showAlertMessage("err", "Please choose Company and Bettype!", 1);
            return false;
        }
        else if (curSeleted == '-100' && bettypeSeleted == '-100') {
            showAlertMessage("err", "Please choose Currency and Bettype!", 1);
            return false;
        }
        else if (comSeleted == '-100') {
            showAlertMessage("err", "Please choose Company!", 1);
            return false;
        }
        else if (curSeleted == '-100') {
            showAlertMessage("err", "Please choose Currency!", 1);
            return false;
        }
        else if (bettypeSeleted == '-100') {
            showAlertMessage("err", "Please choose Bettype!", 1);
            return false;
        }

        try { parent.BlockUI(); } catch (e) { }
        fromcache = false;
        _pageIndex = 1;
        _pageSize = 50;
        //--------------------------
        numberGameWinLossByMatch.companiesChoosed();
        numberGameWinLossByMatch.currenciesChoosed();
        numberGameWinLossByMatch.bettypesChoosed();
        //--------------------------
        numberGameWinLossByMatch.fdate = $('#date').val();
        var arrCompanies = numberGameWinLossByMatch.companies.split(',');
        var arrCurrencies = numberGameWinLossByMatch.currencies.split(',');
        var arrBettypes = numberGameWinLossByMatch.bettypes.split(',');
        if (arrCompanies && arrCompanies.length > 0) {
            if (arrCompanies[0] == '0')
                numberGameWinLossByMatch.companies = '';
        }

        if (arrCurrencies && arrCurrencies.length > 0) {
            if (arrCurrencies[0] == '0')
                numberGameWinLossByMatch.currencies = '';
        }
        if (arrBettypes && arrBettypes.length > 0) {
            if (arrBettypes[0] == '0')
                numberGameWinLossByMatch.bettypes = '';
        }

        var arr = [];
        //string date, string companies, string currencies, string bettypes
        var p = { date: numberGameWinLossByMatch.fdate, companies: numberGameWinLossByMatch.companies, currencies: numberGameWinLossByMatch.currencies, bettypes: numberGameWinLossByMatch.bettypes };
        $.ajax({
            type: "Post",
            url: "../../Services/WCF/Report/ServiceReportNumberGame.svc/NumberGameWinLossByMatch",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                //var start = new Date().getTime();
                if (rs.rows != null && rs.rows.length > 0) {
                    $("#gridtitle").html("Number Game Win Loss By Match - Date " + $("#date").val());
                    var tbl = $("#tblmatchwinlost");
                    tbl.empty();
                    $.each(rs.rows, function (i, n) {
                        arr.push(numberGameWinLossByMatch.renderMatchRow(n));
                        if (n.children.length > 0) {
                            $.each(n.children, function (i1, n1) {
                                arr.push(numberGameWinLossByMatch.renderLiveRow(n.Id, n1));
                                if (n1.children.length > 0) {
                                    $.each(n1.children, function (i2, n2) {
                                        arr.push(numberGameWinLossByMatch.renderBettypeRow(n.Id, n1.Id, n2));
                                    });
                                }
                            });
                        }
                    });

                    tbl.html(arr.join(''));
                    numberGameWinLossByMatch.assignFooter(rs.totalrow);
                    var matchwinlost = $("#matchwinlost");
                    matchwinlost.show();

                    $('#matchwinlost').removeClass("hidden").reRenderScroller(781, 563, true);
                    try { parent.jQuery.unblockUI(); } catch (e) { }

                }
                else {
                    $("#matchwinlost").hide();                 
                    showAlertMessage("err", "There is no data.", 1);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }

            }
        });
    },
    renderMatchRow: function (n) {
        var strrow = "";
        strrow += '<tr>';
        strrow += '<td ><div class="col160 b"><span class="tree-expanded" onclick="numberGameWinLossByMatch.showhiderow(this, \'' + n.Id + '\')"></span>' + n.Name + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    renderLiveRow: function (matchid, n) {
        var strrow = "";
        strrow += '<tr class="' + matchid + '">';
        strrow += '<td ><div class="col160"><span class="indent"></span><span class="tree-expanded" onclick="numberGameWinLossByMatch.showhiderow(this, \'' + n.Id + '\')" ></span>' + n.Name + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    renderBettypeRow: function (matchid, liveId, n) {
        var strrow = "";
        strrow += '<tr class="' + matchid + ' ' + liveId + '">';
        strrow += '<td ><div class="col160"><span class="indent"></span><span class="indent"></span><span class="canchange" onclick="numberGameWinLossByMatch.viewdetail(\'' + n.Id + '\',\'' + n.Name + '\',' + n.MatchId + ');">' + n.Name + '</span></div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    assignFooter: function (row) {
        if (parseFloat(row.Winlost) < 0) {
            $("#totalWinlost").html('<span style="color:red;">' + row.Winlost + '</span>');
        }
        else {
            $("#totalWinlost").html(row.Winlost);
        }

        if (parseFloat(row.Position) < 0) {
            $("#totalPosition").html('<span style="color:red;">' + row.Position + '</span>');
        }
        else {
            $("#totalPosition").html(row.Position);
        }

        if (parseFloat(row.Commission) < 0) {
            $("#totalCommission").html('<span style="color:red;">' + row.Commission + '</span>');
        }
        else {
            $("#totalCommission").html(row.Commission);
        }

        if (parseFloat(row.LineTotal) < 0) {
            $("#totalLineTotal").html('<span style="color:red;">' + row.LineTotal + '</span>');
        }
        else {
            $("#totalLineTotal").html(row.LineTotal);
        }

//        if (parseFloat(row.VirtualLineTotal) < 0) {
//            $("#totalVirtualLineTotal").html('<span style="color:red;">' + row.VirtualLineTotal + '</span>');
//        }
//        else {
//            $("#totalVirtualLineTotal").html(row.VirtualLineTotal);
//        }

//        if (parseFloat(row.Variance) < 0) {
//            $("#totalVariance").html('<span style="color:red;">' + row.Variance + '</span>');
//        }
//        else {
//            $("#totalVariance").html(row.Variance);
//        }

        if (parseFloat(row.TurnOver) < 0) {
            $("#totalTurnOver").html('<span style="color:red;">' + row.TurnOver + '</span>');
        }
        else {
            $("#totalTurnOver").html(row.TurnOver);
        }

        if (parseFloat(row.Margin) < 0) {
            $("#totalMargin").html('<span style="color:red;">' + row.Margin + '</span>');
        }
        else {
            $("#totalMargin").html(row.Margin);
        }
    },
    viewdetail: function (_id, _bettypename, _matchid) {
        id = _id;
        bettypename = _bettypename;
        matchid = _matchid;
        numberGameWinLossByMatchDetail.GetDataReportDetail();
    },
    showhiderow: function (obj, className) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#tblmatchwinlost tr." + className).removeClass("hidden");
            $("#tblmatchwinlost tr." + className + " span.tree-expanded").removeClass("tree-expand");
        }
        else {
            $("#tblmatchwinlost tr." + className).addClass("hidden");
            $(obj).addClass("tree-expand");
        }
    },
    exportExcel: function () {

        var _id = id.split("_");
        var matchcode = _id[0];
        var islive = _id[1];
        var bettype = _id[2];

        var params = 'type=1';
        params += '&date=' + $('#date').val();
        params += '&bettypes=' + bettype;
        params += '&companies=' + bettype;
        params += '&currencies=' + bettype;
        $("#frm").attr('src', 'MatchWinlossNew.aspx?' + params);

    },
    companiesChoosed: function () {
        numberGameWinLossByMatch.companies = $('#selectCompany').multiSelect('getCheckedItems');
        numberGameWinLossByMatch.companies = numberGameWinLossByMatch.companies.join(",");
    },
    currenciesChoosed: function () {
        numberGameWinLossByMatch.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
        numberGameWinLossByMatch.currencies = numberGameWinLossByMatch.currencies.join(",");
    },
    bettypesChoosed: function () {
        numberGameWinLossByMatch.bettypes = $('#selectBettypes').multiSelect('getCheckedItems');
        numberGameWinLossByMatch.bettypes = numberGameWinLossByMatch.bettypes.join(",");
    }

};

var numberGameWinLossByMatchDetail = {
    date: '',
    isLive: false,
    bettype: '',
    matchid: '',
    companies: '',
    currencies: '',
    matchcode: '',
    GetDataReportDetail: function () {
        try { parent.BlockUI(); } catch (e) { }

        $("#err").empty();
        $("#matchwinlost").hide();
        $("#matchwinlostdetail").hide();

        var _id = id.split("_");
        var matchcode = _id[0];
        var islive = _id[1];
        var bettype = _id[2];

        numberGameWinLossByMatchDetail.date = numberGameWinLossByMatch.fdate;
        numberGameWinLossByMatchDetail.isLive = (islive == "Live" ? true : false);
        numberGameWinLossByMatchDetail.bettype = bettype;
        numberGameWinLossByMatchDetail.matchid = matchid;
        numberGameWinLossByMatchDetail.matchcode = matchcode;
        numberGameWinLossByMatchDetail.companies = numberGameWinLossByMatch.companies;
        numberGameWinLossByMatchDetail.currencies = numberGameWinLossByMatch.currencies;

        var arrfrozen = [];
        var arrscroll = [];
        var p = {
            date: numberGameWinLossByMatchDetail.date,
            islive: numberGameWinLossByMatchDetail.isLive,
            matchid: numberGameWinLossByMatchDetail.matchid,
            companies: numberGameWinLossByMatchDetail.companies,
            currencies: numberGameWinLossByMatchDetail.currencies,
            bettype: numberGameWinLossByMatchDetail.bettype
        };
        $.ajax({
            type: "POST",
            url: "../../Services/WCF/Report/ServiceReportNumberGame.svc/NumberGameWinLossByMatchDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs.rows.length > 0) {
                    $("#gridtitledetail").html("Number Game Win Loss By Match " + numberGameWinLossByMatchDetail.matchcode + " - " + (islive == "Live" ? "Live" : "None Live") + " - " + bettypename + " - Date " + $("#date").val());
                    var tblfrozen = $("#tblfrozendetail");
                    var tblscroll = $("#tblscrolldetail");
                    tblscroll.empty();
                    tblfrozen.empty();

                    $.each(rs.rows, function (i, n) {
                        arrscroll.push(numberGameWinLossByMatchDetail.renderWinLossDetail(n));
                        arrfrozen.push(numberGameWinLossByMatchDetail.renderWinLossDetailFrozen(n));
                    });

                    tblfrozen.html($(arrfrozen.join('')));
                    tblscroll.html($(arrscroll.join('')));

                    numberGameWinLossByMatchDetail.assignFooterDetail(rs.footer[0]);

                    //Adjust the size of grid
                    //var start = new Date().getTime();
                    var matchwinlostdetail = $("#matchwinlostdetail");
                    matchwinlostdetail.show();

                    $('#matchwinlostdetail').removeClass("hidden").reRenderScroller(309, 563, false);

                    if ($.browser.msie)
                        $('#matchwinlostdetail .header-frozen .tbl-header-frozen td').css('height', $('#matchwinlostdetail .header-scroll').height() - 2);
                    else {
                        $('#matchwinlostdetail .header-frozen .tbl-header-frozen td').css('height', $('#matchwinlostdetail .header-scroll').height());
                        if ($('#matchwinlostdetail').height() <= 126) {
                            $('#matchwinlostdetail .header-scroll').css('width', $('#matchwinlostdetail .body-scroll').width());
                            $('#matchwinlostdetail .footer-scroll').css('width', $('#matchwinlostdetail .body-scroll').width());
                        }
                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
                else {
                    $("#matchwinlostdetail").hide();
                    $("#backbtn").show();
                    $('<div id="err"></div>').appendTo($("#detailreport"));
                    showAlertMessage("err", "There is no data.", 1);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            }
        });
    },
    renderWinLossDetail: function (n) {
        var strrow = "";
        strrow += '<tr>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.MemberWinlost) < 0 ? ' red' : '') + '">' + n.MemberWinlost + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.MemberComm) < 0 ? ' red' : '') + '">' + n.MemberComm + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.MemberTotal) < 0 ? ' red' : '') + '">' + n.MemberTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.AgentWinlost) < 0 ? ' red' : '') + '">' + n.AgentWinlost + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.AgentComm) < 0 ? ' red' : '') + '">' + n.AgentComm + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.AgentTotal) < 0 ? ' red' : '') + '">' + n.AgentTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.MasterTotal) < 0 ? ' red' : '') + '">' + n.MasterTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.MasterComm) < 0 ? ' red' : '') + '">' + n.MasterComm + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.MasterTotal) < 0 ? ' red' : '') + '">' + n.MasterTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.SuperWinlost) < 0 ? ' red' : '') + '">' + n.SuperWinlost + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.SuperComm) < 0 ? ' red' : '') + '">' + n.SuperComm + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.SuperTotal) < 0 ? ' red' : '') + '">' + n.SuperTotal + '</div></td>';
        strrow += "</tr>";
        return strrow;
    },
    renderWinLossDetailFrozen: function (n) {
        var strrow = "";
        strrow += '<tr>';
        strrow += '<td><div class="col40 c">' + n.Id + '</div></td>';
        strrow += '<td><div class="col160 c">' + n.Username + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
        strrow += "</tr>";
        return strrow;
    },
    assignFooterDetail: function (row) {
        if (parseFloat(row.TurnOver) < 0) {
            $("#detailtotalTurnover").html('<span style="color:red;">' + row.TurnOver + '</span>');
        }
        else {
            $("#detailtotalTurnover").html(row.TurnOver);
        }

        if (parseFloat(row.Commission) < 0) {
            $("#detailtotalComm").html('<span style="color:red;">' + row.Commission + '</span>');
        }
        else {
            $("#detailtotalComm").html(row.Commission);
        }


        //Member
        if (parseFloat(row.MemberWinlost) < 0) {
            $("#mdetailtotalwinloss").html('<span style="color:red;">' + row.MemberWinlost + '</span>');
        }
        else {
            $("#mdetailtotalwinloss").html(row.MemberWinlost);
        }

        if (parseFloat(row.MemberComm) < 0) {
            $("#mdetailtotalcomm").html('<span style="color:red;">' + row.MemberComm + '</span>');
        }
        else {
            $("#mdetailtotalcomm").html(row.MemberComm);
        }

        if (parseFloat(row.MemberTotal) < 0) {
            $("#mdetailtotal").html('<span style="color:red;">' + row.MemberTotal + '</span>');
        }
        else {
            $("#mdetailtotal").html(row.MemberTotal);
        }
        //End member

        //Agent
        if (parseFloat(row.AgentWinlost) < 0) {
            $("#adetailtotalwinloss").html('<span style="color:red;">' + row.AgentWinlost + '</span>');
        }
        else {
            $("#adetailtotalwinloss").html(row.AgentWinlost);
        }

        if (parseFloat(row.AgentComm) < 0) {
            $("#adetailtotalcomm").html('<span style="color:red;">' + row.AgentComm + '</span>');
        }
        else {
            $("#adetailtotalcomm").html(row.AgentComm);
        }

        if (parseFloat(row.AgentTotal) < 0) {
            $("#adetailtotal").html('<span style="color:red;">' + row.AgentTotal + '</span>');
        }
        else {
            $("#adetailtotal").html(row.AgentTotal);
        }
        //End agent

        //Master
        if (parseFloat(row.MasterWinlost) < 0) {
            $("#madetailtotalwinloss").html('<span style="color:red;">' + row.MasterWinlost + '</span>');
        }
        else {
            $("#madetailtotalwinloss").html(row.MasterWinlost);
        }

        if (parseFloat(row.MasterComm) < 0) {
            $("#madetailtotalcomm").html('<span style="color:red;">' + row.MasterComm + '</span>');
        }
        else {
            $("#madetailtotalcomm").html(row.MasterComm);
        }

        if (parseFloat(row.MasterTotal) < 0) {
            $("#madetailtotal").html('<span style="color:red;">' + row.MasterTotal + '</span>');
        }
        else {
            $("#madetailtotal").html(row.MasterTotal);
        }
        //End master

        //Super
        if (parseFloat(row.SuperWinlost) < 0) {
            $("#sdetailtotalwinloss").html('<span style="color:red;">' + row.SuperWinlost + '</span>');
        }
        else {
            $("#sdetailtotalwinloss").html(row.SuperWinlost);
        }

        if (parseFloat(row.SuperComm) < 0) {
            $("#sdetailtotalcomm").html('<span style="color:red;">' + row.SuperComm + '</span>');
        }
        else {
            $("#sdetailtotalcomm").html(row.SuperComm);
        }

        if (parseFloat(row.SuperTotal) < 0) {
            $("#sdetailtotal").html('<span style="color:red;">' + row.SuperTotal + '</span>');
        }
        else {
            $("#sdetailtotal").html(row.SuperTotal);
        }
        //End super}
    },
    backtomain: function () {
        $("#err").empty();
        $("#backbtn").hide();
        $("#matchwinlostdetail").hide();
        $("#matchwinlost").show();
    },
    exportExcel: function () {

        var _id = id.split("_");
        var matchcode = _id[0];
        var islive = _id[1];
        var bettype = _id[2];

        var params = 'type=2';
        params += '&date=' + numberGameWinLossByMatchDetail.date;
        params += '&isLive=' + numberGameWinLossByMatchDetail.isLive;
        params += '&matchid=' + numberGameWinLossByMatchDetail.matchcode;
        params += '&bettype=' + numberGameWinLossByMatchDetail.bettype;
        params += '&companies=' + numberGameWinLossByMatchDetail.companies;
        params += '&currencies=' + numberGameWinLossByMatchDetail.currencies;
        $("#frm").attr('src', 'MatchWinlossNew.aspx?' + params);

    }
}