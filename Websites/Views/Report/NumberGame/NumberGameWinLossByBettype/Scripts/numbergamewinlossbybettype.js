var numberGameWLByBettype = {
    currencyLoaded: false,
    companyLoaded: false,
    currencies: '0',
    companies: '0',
    liveStatus: -1,
    fdate: '',
    title: '',
    isFullLoad: function () {
        return (numberGameWLByBettype.currencyLoaded && numberGameWLByBettype.companyLoaded) ? true : false;
    },
    getCurrencies: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByBettype/Getcurrencies",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                numberGameWLByBettype.currencyLoaded = true;
                var tempData = [];
                var id = "selectCurrency";
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
                    },
                    noItemText: 'No currency found.'
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
                    listData: [],
                    noItemText: 'No currency found.'
                });
            },
            complete: function () {
                if (numberGameWLByBettype.isFullLoad()) {
                    try { parent.jQuery.unblockUI() } catch (e) { };
                }
            }
        });
    },
    getCompanies: function () {

        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByBettype/GetLicensees",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                numberGameWLByBettype.companyLoaded = true;
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
                    },
                    noItemText: 'No company found.'
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
                    listData: [],
                    noItemText: 'No company found.'
                });
            },
            complete: function () {
                if (numberGameWLByBettype.isFullLoad()) {
                    try { parent.jQuery.unblockUI() } catch (e) { };
                } 
            }
        });
    },
    getReport: function () {
        $('#err').html('');
        numberGameWLByBettype.title = 'NUMBER GAME PRODUCT WIN/LOSS REPORT';
        $("#numbergamewinloss").addClass("hidden");
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
        numberGameWLByBettype.liveStatus = $('#ddlLiveStatus').val();
        var p = {
            companies: numberGameWLByBettype.companies,
            currencies: numberGameWLByBettype.currencies,
            wlDate: numberGameWLByBettype.fdate,
            liveStatus: numberGameWLByBettype.liveStatus
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByBettype/SelNumberGameWinLossByBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs && rs.WinLossDetails && rs.WinLossDetails.length > 0) {
                    if (numberGameWLByBettype.liveStatus == 0)
                        numberGameWLByBettype.title = 'NUMBER GAME "NON - LIVE" PRODUCT WIN/LOSS REPORT';
                    else if (numberGameWLByBettype.liveStatus == 1)
                        numberGameWLByBettype.title = 'NUMBER GAME "LIVE" PRODUCT WIN/LOSS REPORT';

                    numberGameWLByBettype.renderWinLossReport(rs);
                }
                else {
                    showAlertMessage("err", "No information is available.", 1);
                }
            },
            error: function () {
                showAlertMessage("err", "No information is available.", 1);
            },
            complete: function () { try { parent.jQuery.unblockUI() } catch (e) { }; }
        });
    },
    renderWinLossReport: function (rs) {
        var arr = [];
        var arrFrozen = [];
        var totalRows = rs.WinLossDetails.length;
        for (var i = 0; i < totalRows; i++) {
            var item = rs.WinLossDetails[i];
            var css = i % 2 == 0 ? "bgeven" : "";
            arr.push(numberGameWLByBettype.renderRow(item, css, numberGameWLByBettype.liveStatus));
            arrFrozen.push(numberGameWLByBettype.renderFrozenRow(item, css, numberGameWLByBettype.liveStatus));
        }
        $('#reporttile').html(numberGameWLByBettype.title);
        $('#tblContent tbody').empty().append(arr.join(''));
        $('#tblContentHeader tbody').empty().append(arrFrozen.join(''));

        $('#numbergamewinloss').find(".col_footer_content thead").empty().append(numberGameWLByBettype.renderFooter(rs.GrandTotal));
        $('#numbergamewinloss').find(".row_footer_content thead").empty().append(numberGameWLByBettype.renderFooterFrozenRow(rs.GrandTotal));

        var gridOpts = {
            tblContentId: 'tblContent',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 140,
            freezeWidth: 415,
            numberFreezeCol: 4,
            gridWidth: 730
        };
        $('#numbergamewinloss').removeClass("hidden").gridScroller(gridOpts);
    },
    renderRow: function (row, css, isLive) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div style="width: 100px; height: 17px" class="c">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">' + row.Turnover + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></td>\
                </tr>';
        return html;
    },
    renderFrozenRow: function (row, css, isLive) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div style="width: 100px; height: 17px" class="c "><a class="blue" onclick="numberGameEventWinLossDetail.getReportDetail(\'' + numberGameWLByBettype.fdate + '\',' + isLive + ',' + row.BettypeId + ',\'' + row.Bettype + '\',\'' + numberGameWLByBettype.companies + '\', \'' + numberGameWLByBettype.currencies + '\'' + ')">' + row.Bettype + '</a></div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">' + row.Position + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">' + row.Comm + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row) {
        var html = '';
        html += '<tr>\
                    <th><div style="width: 100px; height: 17px" class="c">Total</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">&nbsp;</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">&nbsp;</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">&nbsp;</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">' + row.Turnover + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></th>\
                </tr>';
        return html;
    },
    renderFooterFrozenRow: function (row) {
        var html = '';
        html += '<tr>\
                    <th><div style="width: 100px; height: 17px" class="c">Total</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">' + row.Position + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">' + row.Comm + '</div></th>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = window.rootUrl + 'NumberGameWinLossByBettype/Index?isexport=true&type=1&currencies=' +
            numberGameWLByBettype.currencies + '&companies=' + numberGameWLByBettype.companies +
            '&date=' + numberGameWLByBettype.fdate + '&title=' + numberGameWLByBettype.title +
            '&liveStatus=' + numberGameWLByBettype.liveStatus;
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
            wlDate: date,
            liveStatus: isLive,
            bettype: bettypeid,
            companies: companies,
            currencies: currencies
        };
        numberGameEventWinLossDetail.title = bettype;

        $("#dialogNgWLDetail").dialog('option', 'title', 'Number Game Event Win/Loss Report');

        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByBettype/SelNumberGameWinLossByBettypeDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs && rs.WinLoss && rs.WinLoss.length > 0) {
                    var arr = []; var arrFrozen = [];
                    var totalLeague = rs.WinLoss.length;
                    for (var i = 0; i < totalLeague; i++) {
                        var leagueItem = rs.WinLoss[i];
                        arr.push(numberGameEventWinLossDetail.renderLeagueRow(leagueItem.Name));
                        arrFrozen.push(numberGameEventWinLossDetail.renderLeagueFrozenRow(leagueItem.Name));
                        var totalRows = leagueItem.WinLossDetails.length;
                        for (var j = 0; j < totalRows; j++) {
                            var item = leagueItem.WinLossDetails[j];
                            var css = j % 2 == 0 ? "bgeven" : "";
                            arr.push(numberGameEventWinLossDetail.renderRow(item, css));
                            arrFrozen.push(numberGameEventWinLossDetail.renderRowFrozen(item, css));
                        }
                        arr.push(numberGameEventWinLossDetail.renderRow(leagueItem.GrandTotal, 'total'));
                        arrFrozen.push(numberGameEventWinLossDetail.renderRowFrozen(leagueItem.GrandTotal, 'total'));

                    }

                    $('#numbergamewinlossdetailtile').html(numberGameEventWinLossDetail.title);
                    var content = arr.join('');
                    var contentLeft = arrFrozen.join('');
                    if ($.browser.msie && parseInt($.browser.version) == 9) {
                        content = numberGameEventWinLossDetail.replaceSpaceOnIE9(content);
                        contentLeft = numberGameEventWinLossDetail.replaceSpaceOnIE9(contentLeft);
                    }
                    $('#tblDetailContent tbody').empty().append(content);
                    $('#tblDetailContentHeader tbody').empty().append(contentLeft);

                    $('#numbergamewinlossdetail').find(".col_footer_content thead").empty().append(numberGameEventWinLossDetail.renderFooter(rs.GrandTotal, isLive));
                    $('#numbergamewinlossdetail').find(".row_footer_content thead").empty().append(numberGameEventWinLossDetail.renderFooterFrozenRow(rs.GrandTotal, isLive));

                    var gridOpts = {
                        tblContentId: 'tblDetailContent',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: false,
                        gridHeight: 500,
                        freezeWidth: 415,
                        numberFreezeCol: 4
                    };

                    $("#dialogNgWLDetail").removeClass("hidden").dialog("open");
                    $('#numbergamewinlossdetail').removeClass("hidden").gridScroller(gridOpts);

                }
                else {
                    $("#dialogNgWLDetail").removeClass("hidden").dialog("open");
                    $('#errMsgWLDetail').empty().html('No information is available.');
                }
            },
            error: function () {
                $("#dialogNgWLDetail").removeClass("hidden").dialog("open");
                $('#errMsgWLDetail').empty().html('No information is available.');
            },
            complete: function () {
                try { parent.jQuery.unblockUI() } catch (e) { };


            }
        });
    },
    renderLeagueRow: function (leagueName) {
        var html = '<tr>\
                <td colspan="9"><div style="height: 17px" class="b">' + leagueName + '</div></td>\
                </tr>';
        return html;
    },
    renderLeagueFrozenRow: function (leagueName) {
        var html = '<tr>\
                <td colspan="4"><div style="height: 17px" class="b">' + leagueName + '</div></td>\
                </tr>';
        return html;
    },
    renderRow: function (row, css) {
        var html = '';

        html += '<tr class="' + css + '">\
                    <td><div style="width: 100px; height: 17px" class="c">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">&nbsp;</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ">' + row.Turnover + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></td>\
                </tr>';
        return html;
    },
    renderRowFrozen: function (row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div style="width: 100px; height: 17px" class=" c">' + row.Bettype + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class=" r ' + (parseFloat(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class=" r ">' + row.Position + '</div></td>\
                    <td><div style="width: 100px; height: 17px" class=" r ">' + row.Comm + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, isLive) {
        var html = '';
        if (row) {
            var totalTitle = isLive == 1 ? '<font style="color: red">Live!</font> Total' : 'Total';
            html += '<tr>\
                    <th><div style="width: 100px; height: 17px" class=" c">&nbsp;</div></th>\
                    <th><div style="width: 100px; height: 17px" class=" r ">&nbsp;</div></th>\
                    <th><div style="width: 100px; height: 17px" class=" r ">&nbsp;</div></th>\
                    <th><div style="width: 100px; height: 17px" class=" r ">&nbsp;</div></th>\
                    <th><div style="width: 100px; height: 17px" class=" r ' + (parseFloat(row.LineTotalReal) > 0 ? 'red' : '') + '">' + row.LineTotalReal + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class=" r ">' + row.Turnover + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class=" r ' + (parseFloat(row.Margin) > 0 ? 'red' : '') + '">' + row.Margin + '%</div></th>\
                </tr>';
        }
        return html;
    },
    renderFooterFrozenRow: function (row, isLive) {
        var html = '';
        if (row) {
            var totalTitle = isLive == 1 ? '<font style="color: red">Live!</font> Total' : 'Total';
            html += '<tr>\
                    <th><div style="width: 100px; height: 17px" class="c">' + totalTitle + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ' + (parseFloat(row.WinLoss) > 0 ? 'red' : '') + '">' + row.WinLoss + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">' + row.Position + '</div></th>\
                    <th><div style="width: 100px; height: 17px" class="r ">' + row.Comm + '</div></th>\
                </tr>';
        }
        return html;
    },
    exportExcel: function () {
        var url = window.rootUrl + 'NumberGameWinLossByBettype/Index?isexport=true&type=2&currencies=' +
            numberGameEventWinLossDetail.currencies + '&companies=' + numberGameEventWinLossDetail.companies +
            '&date=' + numberGameEventWinLossDetail.fdate + '&liveStatus=' + numberGameEventWinLossDetail.isLive +
            '&bettype=' + numberGameEventWinLossDetail.bettype + '&bettypename=' + numberGameEventWinLossDetail.bettypeName +
            '&title' + numberGameEventWinLossDetail.title;
        $("#frm").attr('src', url);
    },
    replaceSpaceOnIE9: function (text) {
        var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
        var html = text.replace(expr, '><');
        return html;
    }
};

$(document).ready(function () {
    var limitFromDate = {
        minDate: minDay,
        maxDate: toDay
    };
    var options = {
        showOtherMonths: true,
        selectOtherMonths: true,
        checkMinMax: true
    };
    calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);

    try { parent.BlockUI() } catch (e) { }
    numberGameWLByBettype.getCompanies();
    numberGameWLByBettype.getCurrencies();

    var gridOpts = {
        tblContentId: 'tblContent',
        isShowPager: false
    };
    $("#numbergamewinloss").initGrid(gridOpts);

    var gridOpts = {
        tblContentId: 'tblDetailContent',
        isShowPager: false
    };
    $("#numbergamewinlossdetail").initGrid(gridOpts);

    $("#dialogNgWLDetail").dialog({
        title: "",
        autoOpen: false,
        height: 570,
        width: 775,
        modal: true,
        resizable: false
    });
    
});