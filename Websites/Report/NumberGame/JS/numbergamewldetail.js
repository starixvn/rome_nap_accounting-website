$(document).ready(function () {
    numberGameWLByBettype.getCompanies();
    numberGameWLByBettype.getCurrencies();

    $("#dialogNgWLDetail").dialog({
        title: "",
        autoOpen: false,
        height: 570,
        width: 970,
        modal: true,
        resizable: false
    });
});

var numberGameWLByBettype = {
    currencies: '0',
    companies: '0',
    bettypes: '0',
    servicePath: '../../Services/WCF/Report/ServiceReportNumberGame.svc/',
    fdate: '',
    title: '',
    getServicePath: function (type) {
        if (typeof type == "string" && type == "bettype") {
            numberGameWLByBettype.servicePath = "../../../Services/WCF/Report/ServiceReportNumberGame.svc/";
        }
    },
    getCurrencies: function () {
        $.ajax({
            type: "POST",
            url: numberGameWLByBettype.servicePath + "GetCurrencyList",
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
                        numberGameWLByBettype.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
                        if (numberGameWLByBettype.currencies.length <= 0) {
                            showAlertMessage("err", "Please choose Currency!", 1);
                            numberGameWLByBettype.currencies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWLByBettype.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
            },
            error: function () {
                numberGameWLByBettype.currencies = "-100";
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
    getCompanies: function () {
        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "POST",
            url: numberGameWLByBettype.servicePath + "GetSiteList",
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
                        numberGameWLByBettype.companies = $('#selectCompany').multiSelect('getCheckedItems');
                        if (numberGameWLByBettype.companies.length <= 0) {
                            showAlertMessage("err", "Please choose Company!", 1);
                            numberGameWLByBettype.companies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWLByBettype.companies = $('#selectCompany').multiSelect('getCheckedItems');
            },
            error: function () {
                numberGameWLByBettype.companies = '-100';
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
            complete: function () { try { parent.jQuery.unblockUI() } catch (e) { }; }
        });
    },
    getReport: function () {
        $('#err').html('');
        $("#ngNoneLiveGrid").addClass("hidden");
        $("#ngLiveGrid").addClass("hidden");
        //--------------------
        var comSeleted = numberGameWLByBettype.companies;
        var curSeleted = numberGameWLByBettype.currencies;
        if (comSeleted == -100 && curSeleted == -100) {
            showAlertMessage("err", "Please choose Company and Currency!", 1);
            return false;
        }
        else if (comSeleted == -100) {
            showAlertMessage("err", "Please choose Company!", 1);
            return false;
        }
        else if (curSeleted == -100) {
            showAlertMessage("err", "Please choose Currency!", 1);
            return false;
        }

        numberGameWLByBettype.companiesChoosed();
        numberGameWLByBettype.currenciesChoosed();
        //--------------------
        try { parent.BlockUI() } catch (e) { }

        var arrCompanies = numberGameWLByBettype.companies.split(',');
        var arrCurrencies = numberGameWLByBettype.currencies.split(',');

        if (arrCompanies && arrCompanies.length > 0) {
            if (arrCompanies[0] == '0')
                numberGameWLByBettype.companies = '0';
        }

        if (arrCurrencies && arrCurrencies.length > 0) {
            if (arrCurrencies[0] == '0')
                numberGameWLByBettype.currencies = '0';
        }

        numberGameWLByBettype.fdate = $('#fdate').val();
        var p = {
            companies: numberGameWLByBettype.companies,
            currencies: numberGameWLByBettype.currencies,
            date: numberGameWLByBettype.fdate
        };
        $.ajax({
            type: "POST",
            url: numberGameWLByBettype.servicePath + "NumberGameWinLossByBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs) {
                    numberGameWLByBettype.assignLive(rs.Live);
                    numberGameWLByBettype.assignNoneLive(rs.NoneLive);
                }
            },
            error: function () {
                showAlertMessage("err", "No information is available.", 1);
            },
            complete: function () { try { parent.jQuery.unblockUI() } catch (e) { }; }
        });
    },
    assignNoneLive: function (nonlive) {

        if (nonlive && nonlive.BettypesWinLossRows && nonlive.BettypesWinLossRows.length > 0) {
            var arr = [];
            for (var i = 0; i < nonlive.BettypesWinLossRows.length; i++) {
                var item = nonlive.BettypesWinLossRows[i];
                var css = i % 2 == 0 ? "bgeven" : "";
                arr.push(numberGameWLByBettype.renderRow(item, css, 0));
            }
            $('#tblNgNoneLive').empty().html(arr.join(''));
            $('#tblFooterNgNoneLive').empty().html(numberGameWLByBettype.renderFooter(nonlive.Total));
        }
        else {
            $('#tblNgNoneLive').empty().html('No information is available.');
            $('#tblFooterNgNoneLive').empty();
        }

        $("#ngNoneLiveGrid").removeClass("hidden").reRenderScroller(927, 450, true);
    },
    assignLive: function (live) {
        if (live && live.BettypesWinLossRows && live.BettypesWinLossRows.length > 0) {
            var arr = [];
            for (var i = 0; i < live.BettypesWinLossRows.length; i++) {
                var item = live.BettypesWinLossRows[i];
                var css = i % 2 == 0 ? "bgeven" : "";
                arr.push(numberGameWLByBettype.renderRow(item, css, 1));
            }

            $('#tblNgLive').empty().html(arr.join(''));
            $('#tblFooterNgLive').empty().html(numberGameWLByBettype.renderFooter(live.Total));
        }
        else {
            $('#tblNgLive').empty().html('No information is available.');
            $('#tblFooterNgLive').empty();
        }
        $("#ngLiveGrid").removeClass("hidden").reRenderScroller(927, 450, true);
    },
    renderRow: function (row, css, isLive) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 c blue" onclick="numberGameEventWinLossDetail.getReportDetail(\'' + numberGameWLByBettype.fdate + '\',' + isLive + ',' + row.BettypeId + ',\'' + row.Bettype + '\',\'' + numberGameWLByBettype.companies + '\', \'' + numberGameWLByBettype.currencies + '\'' + ')">' + row.Bettype + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></td>\
                    <td><div class="col100 r ">' + row.Position + '</div></td>\
                    <td><div class="col100 r ">' + row.Comm + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalVirtual) > 0 ? 'red' : '') + '">' + row.LineTotalVirtual + '</div></td>\
                    <td><div class="col100 r ">' + row.Turnover + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Variance) > 0 ? 'red' : '') + '">' + row.Variance + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row) {
        var html = '';
        html += '<tr>\
                    <td><div class="col100 c">Total</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></td>\
                    <td><div class="col100 r ">' + row.Position + '</div></td>\
                    <td><div class="col100 r ">' + row.Comm + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalVirtual) > 0 ? 'red' : '') + '">' + row.LineTotalVirtual + '</div></td>\
                    <td><div class="col100 r ">' + row.Turnover + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Variance) > 0 ? 'red' : '') + '">' + row.Variance + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'NumberGameWLDetailNew.aspx?isexport=true&type=1&currencies=' +
            numberGameWLByBettype.currencies + '&companies=' + numberGameWLByBettype.companies +
            '&date=' + numberGameWLByBettype.fdate;
        $("#frm").attr('src', url);
    },
    companiesChoosed: function () {
        numberGameWLByBettype.companies = $('#selectCompany').multiSelect('getCheckedItems');
        numberGameWLByBettype.companies = numberGameWLByBettype.companies.join(",");
    },
    currenciesChoosed: function () {
        numberGameWLByBettype.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
        numberGameWLByBettype.currencies = numberGameWLByBettype.currencies.join(",");
    }
};

var numberGameEventWinLossDetail = {
    fdate: '',
    isLive: 0,
    bettype: 0,
    bettypeName: '',
    companies: '0',
    currencies: '0',
    title: '',
    getReportDetail: function (date, isLive, bettypeid, bettype, companies, currencies) {
        try { parent.BlockUI(); } catch (e) { }
        $('#err').html('');
        numberGameEventWinLossDetail.fdate = date;
        numberGameEventWinLossDetail.isLive = isLive;
        numberGameEventWinLossDetail.bettype = bettypeid;
        numberGameEventWinLossDetail.companies = companies;
        numberGameEventWinLossDetail.currencies = currencies;
        numberGameEventWinLossDetail.bettypeName = bettype;
        var p = {
            date: date,
            isLive: isLive,
            bettype: bettypeid,
            companies: companies,
            currencies: currencies
        };

        $("#dialogNgWLDetail").dialog('option', 'title', 'Number Game Event Win/Loss Report');
        numberGameEventWinLossDetail.title = bettype;
        $('#NgEventWLGridTitle').html(numberGameEventWinLossDetail.title);
        $.ajax({
            type: "POST",
            url: numberGameWLByBettype.servicePath + "NumberGameWinLossDetailByBettypeNew",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs && rs.Items && rs.Items.length > 0) {
                    var arr = [];
                    for (var i = 0; i < rs.Items.length; i++) {
                        var leagueItem = rs.Items[i];
                        arr.push(numberGameEventWinLossDetail.renderLeagueRow(leagueItem.Name));
                        for (var j = 0; j < leagueItem.BettypesWinLossRows.length; j++) {
                            var item = leagueItem.BettypesWinLossRows[j];
                            var css = j % 2 == 0 ? "bgeven" : "";
                            arr.push(numberGameEventWinLossDetail.renderRow(item, css));
                        }
                        arr.push(numberGameEventWinLossDetail.renderRow(leagueItem.Total, 'total'));

                    }

                    $('#tblNgEvent').empty().html(arr.join(''));
                    $('#tblNgFooterEvent').empty().html(numberGameEventWinLossDetail.renderFooter(rs.Total, isLive));

                }
                else {
                    $('#tblNgFooterEvent').empty().html('No information is available.');
                }

                $("#dialogNgWLDetail").removeClass("hidden").dialog("open");
                $("#NgEventWLGrid").reRenderScroller(927, 450, true);

            },
            error: function () {
                $("#dialogNgWLDetail").removeClass("hidden").dialog("open");
                $("#NgEventWLGrid").reRenderScroller(927, 450, true);
            },
            complete: function () { try { parent.jQuery.unblockUI() } catch (e) { }; }
        });


    },
    renderLeagueRow: function (leagueName) {
        var html = '<tr>\
                <td colspan="9"><div class="b">' + leagueName + '</div></td>\
                </tr>';

        return html;
    },
    renderRow: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 c">' + row.Bettype + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></td>\
                    <td><div class="col100 r ">' + row.Position + '</div></td>\
                    <td><div class="col100 r ">' + row.Comm + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalVirtual) > 0 ? 'red' : '') + '">' + row.LineTotalVirtual + '</div></td>\
                    <td><div class="col100 r ">' + row.Turnover + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Variance) > 0 ? 'red' : '') + '">' + row.Variance + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, isLive) {
        var html = '';
        if (row) {
            var totalTitle = isLive == 1 ? '<font style="color: red">Live!</font> Total' : 'Total';
            html += '<tr>\
                    <td><div class="col100 c">' + totalTitle + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></td>\
                    <td><div class="col100 r ">' + row.Position + '</div></td>\
                    <td><div class="col100 r ">' + row.Comm + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.LineTotalVirtual) > 0 ? 'red' : '') + '">' + row.LineTotalVirtual + '</div></td>\
                    <td><div class="col100 r ">' + row.Turnover + '</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></td>\
                    <td><div class="col100 r ' + (parseInt(row.Variance) > 0 ? 'red' : '') + '">' + row.Variance + '</div></td>\
                </tr>';
        }
        return html;
    },
    exportExcel: function () {
        var url = 'NumberGameWLDetailNew.aspx?isexport=true&type=2&currencies=' +
            numberGameEventWinLossDetail.currencies + '&companies=' + numberGameEventWinLossDetail.companies +
            '&date=' + numberGameEventWinLossDetail.fdate + '&islive=' + numberGameEventWinLossDetail.isLive +
            '&bettype=' + numberGameEventWinLossDetail.bettype + '&bettypename=' + numberGameEventWinLossDetail.bettypeName;
        $("#frm").attr('src', url);
    }
};