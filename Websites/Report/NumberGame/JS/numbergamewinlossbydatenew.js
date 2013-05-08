$(document).ready(function () {
    numberGameWinlossByDate.getCompanies();
    numberGameWinlossByDate.getCurrencies();
    numberGameWinlossByDate.getBettyes();
});

var numberGameWinlossByDate = {
    currencies: '',
    companies: '',
    bettypes: '',
    servicePath: '../../Services/WCF/Report/ServiceReportNumberGame.svc/',
    fdate: '',
    tdate: '',
    title: '',
    getServicePath: function (type) {
        if (typeof type == "string" && type == "bettype") {
            numberGameWinlossByDate.servicePath = "../../../Services/WCF/Report/ServiceReportNumberGame.svc/";
        }
    },
    getCurrencies: function () {
        $.ajax({
            type: "POST",
            url: numberGameWinlossByDate.servicePath + "GetCurrencyList",
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
                        numberGameWinlossByDate.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
                        if (numberGameWinlossByDate.currencies.length <= 0) {
                            showAlertMessage("err", "Please choose Currency!", 1);
                            numberGameWinlossByDate.currencies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWinlossByDate.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
            },
            error: function () {
                numberGameWinlossByDate.currencies = "-100";
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
            url: numberGameWinlossByDate.servicePath + "GetBettypeList",
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
                        numberGameWinlossByDate.bettypes = $('#selectBettypes').multiSelect('getCheckedItems');
                        if (numberGameWinlossByDate.bettypes.length <= 0) {
                            showAlertMessage("err", "Please choose Bettype!", 1);
                            numberGameWinlossByDate.bettypes = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWinlossByDate.bettypes = $('#selectBettypes').multiSelect('getCheckedItems');
             
            },
            error: function () {
                numberGameWinlossByDate.bettypes = "-100";
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
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: numberGameWinlossByDate.servicePath + "GetSiteList",
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
                        numberGameWinlossByDate.companies = $('#selectCompany').multiSelect('getCheckedItems');
                        if (numberGameWinlossByDate.companies.length <= 0) {
                            showAlertMessage("err", "Please choose Company!", 1);
                            numberGameWinlossByDate.companies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                numberGameWinlossByDate.companies = $('#selectCompany').multiSelect('getCheckedItems');

            },
            error: function () {
                numberGameWinlossByDate.companies = '-100';
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
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    getReport: function () {
        $("#err").empty();
        $("#matchwinlost").hide();

        var comSeleted = numberGameWinlossByDate.companies;
        var curSeleted = numberGameWinlossByDate.currencies;
        var bettypeSeleted = numberGameWinlossByDate.bettypes;

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
        //--------------------------
        numberGameWinlossByDate.companiesChoosed();
        numberGameWinlossByDate.currenciesChoosed();
        numberGameWinlossByDate.bettypesChoosed();
        //--------------------------
        numberGameWinlossByDate.fdate = $('#fdate').val();
        numberGameWinlossByDate.tdate = $('#tdate').val();
        var arrCompanies = numberGameWinlossByDate.companies.split(',');
        var arrCurrencies = numberGameWinlossByDate.currencies.split(',');
        var arrBettypes = numberGameWinlossByDate.bettypes.split(',');
        if (arrCompanies && arrCompanies.length > 0) {
            if (arrCompanies[0] == '0')
                numberGameWinlossByDate.companies = '';
        }

        if (arrCurrencies && arrCurrencies.length > 0) {
            if (arrCurrencies[0] == '0')
                numberGameWinlossByDate.currencies = '';
        }
        if (arrBettypes && arrBettypes.length > 0) {
            if (arrBettypes[0] == '0')
                numberGameWinlossByDate.bettypes = '';
        }

        var p = {
            companies: numberGameWinlossByDate.companies,
            currencies: numberGameWinlossByDate.currencies,
            bettypes: numberGameWinlossByDate.bettypes,
            fromdate: numberGameWinlossByDate.fdate,
            todate: numberGameWinlossByDate.tdate
        };

        try { parent.BlockUI(); } catch (e) { }

        var arr = [];
        $.ajax({
            type: "POST",
            url: numberGameWinlossByDate.servicePath + "NumberGameWinLossByDate",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs.rows != null && rs.rows.length > 0) {
                    numberGameWinlossByDate.title = "Number Game Win Loss By Date - From " + numberGameWinlossByDate.fdate + " To " + numberGameWinlossByDate.tdate;
                    $("#gridtitle").html(numberGameWinlossByDate.title);
                    var tbl = $("#tblmatchwinlost");
                    tbl.empty();
                    $.each(rs.rows, function (i, n) {
                        arr.push(numberGameWinlossByDate.renderMatchRow(n));
                        if (n.children.length > 0) {
                            $.each(n.children, function (i1, n1) {
                                arr.push(numberGameWinlossByDate.renderLiveRow(n.Id, n1));
                                if (n1.children.length > 0) {
                                    $.each(n1.children, function (i2, n2) {
                                        arr.push(numberGameWinlossByDate.renderBettypeRow(n.Id, n1.Id, n2));
                                    });
                                }
                            });
                        }
                    });
                    tbl.html(arr.join(''));
                    numberGameWinlossByDate.assignFooter(rs.totalrow);
                    $('#matchwinlost').show();
                    $('#matchwinlost').reRenderScroller(987, 563, true);
                }
                else {
                    $("#matchwinlost").hide();
                    showAlertMessage("err", "There is no data.", 1);
                }

            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
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
        $('#matchwinlost').reRenderScroller(987, 563, true);
    },
    renderMatchRow: function (n) {
        var strrow = "";
        strrow += '<tr>';
        strrow += '<td ><div class="col160 b"><span class="tree-expanded" onclick="numberGameWinlossByDate.showhiderow(this, \'' + n.Id + '\')"></span>' + n.Name + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.VirtualLineTotal) < 0 ? ' red' : '') + '">' + n.VirtualLineTotal + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Variance) < 0 ? ' red' : '') + '">' + n.Variance + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
        strrow += '<td><div class="col100 r b' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    renderLiveRow: function (matchid, n) {
        var strrow = "";
        strrow += '<tr class="' + matchid + '">';
        strrow += '<td ><div class="col160"><span class="indent"></span><span class="tree-expanded" onclick="numberGameWinlossByDate.showhiderow(this, \'' + n.Id + '\')" ></span>' + n.Name + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.VirtualLineTotal) < 0 ? ' red' : '') + '">' + n.VirtualLineTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Variance) < 0 ? ' red' : '') + '">' + n.Variance + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    renderBettypeRow: function (matchid, liveId, n) {
        var strrow = "";
        strrow += '<tr class="' + matchid + ' ' + liveId + '">';
        strrow += '<td ><div class="col160"><span class="indent"></span><span class="indent"></span>' + n.Name + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.VirtualLineTotal) < 0 ? ' red' : '') + '">' + n.VirtualLineTotal + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Variance) < 0 ? ' red' : '') + '">' + n.Variance + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
        strrow += '<td><div class="col100 r' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    assignFooter: function (row) {
        if (parseFloat(row.Winlost) < 0)
            $("#totalWinlost").html('<span style="color:red;">' + row.Winlost + '</span>');
        else
            $("#totalWinlost").html(row.Winlost);

        if (parseFloat(row.Position) < 0)
            $("#totalPosition").html('<span style="color:red;">' + row.Position + '</span>');
        else
            $("#totalPosition").html(row.Position);

        if (parseFloat(row.Commission) < 0)
            $("#totalCommission").html('<span style="color:red;">' + row.Commission + '</span>');
        else
            $("#totalCommission").html(row.Commission);

        if (parseFloat(row.LineTotal) < 0)
            $("#totalLineTotal").html('<span style="color:red;">' + row.LineTotal + '</span>');
        else
            $("#totalLineTotal").html(row.LineTotal);

        if (parseFloat(row.VirtualLineTotal) < 0)
            $("#totalVirtualLineTotal").html('<span style="color:red;">' + row.VirtualLineTotal + '</span>');
        else
            $("#totalVirtualLineTotal").html(row.VirtualLineTotal);

        if (parseFloat(row.Variance) < 0)
            $("#totalVariance").html('<span style="color:red;">' + row.Variance + '</span>');
        else
            $("#totalVariance").html(row.Variance);

        if (parseFloat(row.TurnOver) < 0)
            $("#totalTurnOver").html('<span style="color:red;">' + row.TurnOver + '</span>');
        else
            $("#totalTurnOver").html(row.TurnOver);

        if (parseFloat(row.Margin) < 0)
            $("#totalMargin").html('<span style="color:red;">' + row.Margin + '</span>');
        else
            $("#totalMargin").html(row.Margin);
    },
    exportExcel: function (type) {
        var params = 'type=' + type;
        params += '&fdate=' + numberGameWinlossByDate.fdate;
        params += '&tdate=' + numberGameWinlossByDate.tdate;
        params += '&companies=' + numberGameWinlossByDate.companies;
        params += '&currencies=' + numberGameWinlossByDate.currencies;
        params += '&bettypes=' + numberGameWinlossByDate.bettypes;

        $("#frm").attr('src', 'DateWinLossNew.aspx?' + params);
    },
    companiesChoosed: function () {
        numberGameWinlossByDate.companies = $('#selectCompany').multiSelect('getCheckedItems');
        numberGameWinlossByDate.companies = numberGameWinlossByDate.companies.join(",");
    },
    currenciesChoosed: function () {
        numberGameWinlossByDate.currencies = $('#selectCurrency').multiSelect('getCheckedItems');
        numberGameWinlossByDate.currencies = numberGameWinlossByDate.currencies.join(",");
    },
    bettypesChoosed: function () {
        numberGameWinlossByDate.bettypes = $('#selectBettypes').multiSelect('getCheckedItems');
        numberGameWinlossByDate.bettypes = numberGameWinlossByDate.bettypes.join(",");
    }

}

