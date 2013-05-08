$(function () {
    filterManager.getFilterData();
});



var filterobj = {
    FromDate: '3/27/2011',
    ToDate: '4/6/2011',
    InYears: [2010, 2011],
    InMonth: 3,
    FromDay: 1,
    ToDay: 25,
    Currency: [{ Id: 1, Name: "All (RM)" }, { Id: 2, Name: "RM" }, { Id: 3, Name: "USS" }, { Id: 4, Name: "EURO" }, { Id: 5, Name: "RBP" }, { Id: 6, Name: "VND"}],
    Product: [{ Id: 0, Name: "All" }, { Id: 2, Name: "SB" }, { Id: 3, Name: "RB" }, { Id: 4, Name: "Casino" }, { Id: 5, Name: "NG" }, { Id: 6, Name: "Bingo"}],
    Sport: [{ Id: 0, Name: "All" }, { Id: 2, Name: "SB" }, { Id: 3, Name: "RB" }, { Id: 4, Name: "Casino" }, { Id: 5, Name: "NG" }, { Id: 6, Name: "Bingo"}],
    BetType: [{ Id: 0, Name: "All" }, { Id: 2, Name: "SB" }, { Id: 3, Name: "RB" }, { Id: 4, Name: "Casino" }, { Id: 5, Name: "NG" }, { Id: 6, Name: "Bingo"}],
    LiveStatus: ["All", "None-Live", "Live"],
    UserName: '',
    Leagues: [{ Id: 0, Name: "All" }, { Id: 2, Name: "Major" }, { Id: 3, Name: "Minor"}],
    SelectLeagues: [{ Id: 0, Name: "All" }, { Id: 2, Name: "SB" }, { Id: 3, Name: "RB" }, { Id: 4, Name: "Casino" }, { Id: 5, Name: "NG" }, { Id: 6, Name: "Bingo" }, { Id: 0, Name: "All" }, { Id: 2, Name: "SB" }, { Id: 3, Name: "RB" }, { Id: 4, Name: "Casino" }, { Id: 5, Name: "NG" }, { Id: 6, Name: "Bingo" }, { Id: 0, Name: "All" }, { Id: 2, Name: "SB" }, { Id: 3, Name: "RB" }, { Id: 4, Name: "Casino" }, { Id: 5, Name: "NG" }, { Id: 6, Name: "Bingo"}],
    ReportBy: [{ Id: 1, Name: "Daily" }, { Id: 2, Name: "Monthly" }, { Id: 3, Name: "UserName" }, { Id: 4, Name: "Bet type" }, { Id: 5, Name: "League" }, { Id: 6, Name: "Game"}]
};
var monitorobj = {
    items: [
    {
        Name: 'test',
        Turnover: '112,22',
        Winloss: '112,22',
        Betcount: '11',
        ATurnover: '112,22',
        AWinloss: '112,22',
        MTurnover: '112,22',
        MWinloss: '112,22',
        STurnover: '112,22',
        SWinloss: '112,22'
    },
     {
         Name: 'test',
         Turnover: '112,22',
         Winloss: '112,22',
         Betcount: '11',
         ATurnover: '112,22',
         AWinloss: '112,22',
         MTurnover: '112,22',
         MWinloss: '112,22',
         STurnover: '112,22',
         SWinloss: '112,22'
     },
     {
         Name: 'test',
         Turnover: '112,22',
         Winloss: '112,22',
         Betcount: '11',
         ATurnover: '112,22',
         AWinloss: '112,22',
         MTurnover: '112,22',
         MWinloss: '112,22',
         STurnover: '112,22',
         SWinloss: '112,22'
     },
     {
         Name: 'test',
         Turnover: '112,22',
         Winloss: '112,22',
         Betcount: '11',
         ATurnover: '112,22',
         AWinloss: '112,22',
         MTurnover: '112,22',
         MWinloss: '112,22',
         STurnover: '112,22',
         SWinloss: '112,22'
     }
    ]
};
var monitorparams = {
    fromdate: '',
    todate: '',
    currency: 0,
    product: [],
    sporttype: 0,
    bettype: 0,
    live: -1,
    usernames: '',
    leagues: '',
    reportby: 0,
    pageIndex: 1,
    pageSize: 50,
    Turnover1: '',
    Turnover2: '',
    Winloss1: '',
    Winloss2: ''
};
var filterManager = {
    currLeagueParams: null,
    isHistory: false,
    sortDESC: null,
    leaguesData: [],
    sortLeagues: function (obj) {

        $("#chkallleagues").attr("checked", true);
        if (filterManager.sortDESC == null) {
            filterManager.sortDESC = true;
        }
        if (filterManager.sortDESC == true) {
            $("#" + obj).removeClass("desc");
            $("#" + obj).addClass("asc");

        }
        else {
            $("#" + obj).removeClass("asc");
            $("#" + obj).addClass("desc");
        }
        filterManager.sortDESC = !filterManager.sortDESC;
        filterManager.sortData(filterManager.sortDESC);

    },
    sortData: function (desc) {

        if (filterManager.leaguesData.length > 0) {
            filterManager.leaguesData.sort(helper.sort_by('Name', desc, function (a) { return a.toUpperCase(); }));
            filterManager.renderSortedSelectLeagues(filterManager.leaguesData);
        }
    },
    resetFilter: function () {
        $("#err").empty();
        $("#gridmonitor").addClass("hidden");
        if (filterManager.isHistory) {
            filterManager.changeReportType();
        }
        $("#rdall").attr("checked", "checked");
        $("#btnselectleagues").attr("disabled", false);
        $("#ddlsport").attr("disabled", false);
        $("#txtusername").val("");
        $("#txtTurnover1").val("");
        $("#txtTurnover2").val("");
        $("#txtWinLoss1").val("");
        $("#txtWinLoss2").val("");



        filterManager.getFilterData();
    },
    renderFilter: function () {
        //Render Datetime
        var fromdate_opts = { minDate: filterobj.FromDate, maxDate: filterobj.ToDate };
        helper.renderCalendarFromTo("#txtfromdate, #txttodate", fromdate_opts);
        helper.renderInMonth(filterobj.InYears, filterobj.InMonth, filterobj.FromDate);
        //Render Currency
        filterManager.renderCurrency(filterobj.Currency);
        //Render Product
        filterManager.renderProduct(filterobj.Product);
        //Render Sport Bettype
        filterManager.renderSport(filterobj.Sport);
        filterManager.rederBettype(filterobj.BetType);
        //Render leagues
        filterManager.renderLeagues(filterobj.Leagues);
        //Render Select leagues
        filterManager.getSelectLeagues();
        //Render Report By
        filterManager.renderReportBy(filterobj.ReportBy);
    },
    renderReportBy: function (data) {
        if (data != null) {
            var len = data.length;
            if (len > 0) {



                var arr = [];
                for (i = 0; i < len; i++) {
                    var item = data[i];
                    if (filterManager.isHistory && item.Id == 1) {//hardcode to hide montly option when view history report
                        continue;
                    }
                    if (!$("#chkproduct_1").attr("checked") && item.Id == 4) {
                        continue;
                    }
                    if (!$("#chkproduct_1").attr("checked") && !$("#chkproduct_2").attr("checked") && !$("#chkproduct_4").attr("checked") && !$("#chkproduct_6").attr("checked") && !$("#chkproduct_7").attr("checked") && item.Id == 5) {
                        continue;
                    }

                    arr.push('<option value="' + item.Id + '">' + item.Name + '</option>');
                }
                $("#ddlreportby").empty().html(arr.join(''));

                //Report by sport only available when product SB is checked
                if (!$('#chkproduct_1').is(':checked')) {
                    $('#ddlreportby option[value="7"]').remove();
                }

                filterManager.getSelectLeagues();
            }
        }
    },
    renderCurrency: function (data) {

        var len = data.length;
        if (len > 0) {
            var arr = [];
            for (i = 0; i < len; i++) {
                var item = data[i];
                arr.push('<option value="' + item.Id + '">' + item.Name + '</option>');
            }
            $("#ddlcurrency").empty().html(arr.join(''));
        }
    },
    renderProduct: function (data) {
        var len = data.length;
        if (len > 0) {
            var arr = [];
            for (var i = 0; i < len; i++) {
                var item = data[i];
                arr.push('<input class="chkproduct ' + (item.Id == 0 ? 'all' : '') + '" type="checkbox" id="chkproduct_' + item.Id + '" checked="checked" onclick="helper.checkProduct(this);" />' + item.Name + '<span class="padr-r30"></span>');
            }
            $("#productcontainer").empty().html(arr.join(''));
        }
    },
    renderSport: function (data) {
        var len = data.length;
        if (len > 0) {
            var arr = [];
            for (var i = 0; i < len; i++) {
                var item = data[i];
                arr.push('<option value="' + item.Id + '">' + item.Name + '</option>');
            }
            $("#ddlsport").empty().html(arr.join(''));
        }
    },
    rederBettype: function (data) {
        var len = data.length;
        if (len > 0) {
            var arr = [];
            for (var i = 0; i < len; i++) {
                var item = data[i];
                arr.push('<option value="' + item.Id + '">' + item.Name + '</option>');
            }
            $("#ddlbettype").empty().html(arr.join(''));
        }
    },
    renderSelectLeagues: function (data) {

        var len = data.length;
        if (len > 0) {
            var arr = [];
            for (i = 0; i < len; i++) {
                var item = data[i];
                var html = '<tr>\
                                <td>\
                                    <div class="w40 c"><input class="chkselectleagues" type="checkbox" checked="checked" id="chkleagues_' + item.Id + '" onclick="helper.checkSelectLeagues(this);" /></div>\
                                </td>\
                                <td>\
                                    <div class="w360 l" style="word-break:break-all">' + item.Name + '</div>\
                                </td>\
                            </tr>';
                arr.push(html);
            }
            $("#chkallleagues").attr("checked", true);
            $("#tblselectleagues").empty().html(arr.join(''));
            $("#gridleagues").reRenderScroller(406, 236, true);
            $("#lblselectleagues").removeClass("desc");
            $("#lblselectleagues").removeClass("asc");

        }
    },
    renderSortedSelectLeagues: function (data) {
        var len = data.length;
        if (len > 0) {
            var arr = [];
            for (i = 0; i < len; i++) {
                var item = data[i];
                var html = '<tr>\
                                <td>\
                                    <div class="w40 c"><input class="chkselectleagues" type="checkbox" checked="checked" id="chkleagues_' + item.Id + '" onclick="helper.checkSelectLeagues(this);" /></div>\
                                </td>\
                                <td>\
                                    <div class="w360 l" style="word-break:break-all">' + item.Name + '</div>\
                                </td>\
                            </tr>';
                arr.push(html);
            }
            $("#tblselectleagues").empty().html(arr.join(''));
            $("#gridleagues").reRenderScroller(406, 236, true);
        }
    },
    renderLeagues: function (data) {
        if (data != null) {
            var len = data.length;
            if (len > 0) {
                var arr = [];
                for (var i = 0; i < len; i++) {
                    var item = data[i];
                    arr.push('<input name="rdleague" onclick="filterManager.getSelectLeagues();" class="rdleague" type="radio" id="rdleague_' + item.Id + '" ' + (i == 0 ? 'checked="checked"' : '') + ' />' + item.Name + '<span class="padr-r30"/>');
                }
                $("#leaguescontainer").empty().html(arr.join(''));
            }
        }
    },
    showSelectLeagues: function () {
        var selectleaguespopup = $("#selectleaguespopup");
        selectleaguespopup.toggleClass("hidden");
        if (!selectleaguespopup.hasClass("hidden")) {
            helper.setPopupPosition($("#btnselectleagues"), selectleaguespopup);
            filterManager.getSelectLeagues();
            $("#btnselectleagues").val("Hide Leagues");
            $('#btnselectleagues').removeAttr('disabled');
        }
        else {
            $("#btnselectleagues").val("Select Leagues");
        }
        try { parent.jQuery.unblockUI(); } catch (et) { }
    },
    getFilterData: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/MonitorService.svc/LoadFilterReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "",
            success: function (rs) {
                $.extend(filterobj, rs);
                filterManager.renderFilter();
                try { parent.jQuery.unblockUI(); } catch (ee) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (et) { }
            }
        });

    },
    changeReportType: function () {
        if (!filterManager.isHistory) {
            $(".historyreport").removeClass("hidden");
            $(".currentreport").addClass("hidden");
            filterManager.isHistory = true;
            $('#spTitleViewBalanceUpDay').html('');
            $("#btnviewhistory").html("Back to Current Report");
        }
        else {
            $(".currentreport").removeClass("hidden");
            $(".historyreport").addClass("hidden");
            filterManager.isHistory = false;
            $('#spTitleViewBalanceUpDay').html('You can see the report data from latest Balance day up to yesterday. For more historic data, please click to');
            $("#btnviewhistory").html("View history report");

        }
        filterManager.renderReportBy(filterobj.ReportBy);
    },
    getBettype: function () {
        var options = filterobj.Product;
        var sportType = 0;

        for (var i = 0; i < filterobj.Product.length; i++) {
            options[i].Value = $("#chkproduct_" + i).attr("checked") == true ? "1" : "0";
        }


        if ($('#chkproduct_1').is(':checked'))
            sportType = $('#ddlsport').val();
        else
            sportType = 0;

        $.ajax({
            type: "POST",
            url: "Service/MonitorService.svc/GetBettypes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"option":' + JSON.stringify(options) + ',"sporttype":' + sportType + '}',
            success: function (rs) {
                if (rs != null && rs.length > 0) {
                    filterManager.rederBettype(rs);
                    filterManager.getSelectLeagues();
                    try { parent.jQuery.unblockUI(); } catch (ee) { }

                    $('.LeagueControl').removeAttr('disabled');
                    $('.LeagueControl').children().removeAttr('disabled');

                    $('#btnselectleagues').attr('disabled', !$('#chkproduct_1').is(':checked'));
                    $('#ddlsport').attr('disabled', !$('#chkproduct_1').is(':checked'));
                    $('.rdleague').attr('disabled', !$('#chkproduct_1').is(':checked'));
                }
            }
        });

    },
    getSelectLeagues: function () {

        if (!$('#chkproduct_1').is(':checked')) {

            return;
        }

        $('.LeagueControl').attr('disabled', 'disabled');
        $('.LeagueControl').children().attr('disabled', 'disabled');

        $("#gridleagues").reRenderScroller(406, 236, true);
        var options = filterobj.Leagues;
        for (var i = 0; i < filterobj.Leagues.length; i++) {
            options[i].Value = $("#rdleague_" + i).attr("checked") == true ? "1" : "0";
        }
        var fromdate = !filterManager.isHistory ? $("#txtfromdate").val() : $("#ddlinmonth").val().split("_")[0] + "/" + $("#ddlfromday").val() + "/" + $("#ddlinmonth").val().split("_")[1];
        var todate = !filterManager.isHistory ? $("#txttodate").val() : $("#ddlinmonth").val().split("_")[0] + "/" + $("#ddltoday").val() + "/" + $("#ddlinmonth").val().split("_")[1];
        var currency = $("#ddlcurrency").val();
        var live = -1;
        if ($("#rdall").attr("checked")) {
            live = -1;
        }
        else if ($("#rdnonelive").attr("checked")) {
            live = 0;
        }
        else {
            live = 1;
        }

        var leagues = -1;
        if ($("#rdleague_0").attr("checked")) {
            leagues = -1;
        }
        else if ($("#rdleague_1").attr("checked")) {
            leagues = 0;
        }
        else {
            leagues = 1;
        }
        var bettype = $("#ddlbettype").val();
        var params = null;
        params = {
            option: options,
            sportId: $("#ddlsport").val(),
            fromdate: fromdate,
            todate: todate,
            currency: currency,
            bettype: bettype,
            live: live,
            leagues: leagues

        };



        var strparams = JSON.stringify(params);
        if (JSON.stringify(filterManager.currLeagueParams) != strparams || filterManager.currLeagueParams == null) {
            $("#tblselectleagues").empty().html(helper.renderLoader());
            $("#gridleagues").reRenderScroller(406, 236, true);
            filterManager.currLeagueParams = params;
            $.ajax({
                type: "POST",
                url: "Service/MonitorService.svc/GetLeague",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: strparams,
                success: function (rs) {
                    filterManager.leaguesData = rs;
                    if (rs != null && rs.length > 0) {
                        filterManager.leaguesData = rs;
                        filterManager.renderSelectLeagues(rs);
                    }
                    else {
                        $("#tblselectleagues").empty().html(helper.renderNodataLeagues());
                        $("#gridleagues").reRenderScroller(406, 236, true);
                    }
                    $('.LeagueControl').removeAttr('disabled');
                    $('.LeagueControl').children().removeAttr('disabled');

                },
                complete: function () {
                    $("#btnReport").removeAttr("disabled");
                    $('.LeagueControl').removeAttr('disabled');
                    $('.LeagueControl').children().removeAttr('disabled');

                }
            });
        }
        else {
            $('.LeagueControl').removeAttr('disabled');
            $('.LeagueControl').children().removeAttr('disabled');

        }
    }
};

var monitorManager = {

    currReportBy: 0,
    currWidth: 1000,
    currCss: 'w120',
    currTitle: '',
    pageIndex: 1,
    pageSize: 50,
    totalItem: 0,
    arr: [],
    checkValidInput: function () {
        var result = true;
        var usernamelength = $("#txtusername").val().length;
        if (usernamelength > 4000) {
            $("#txtusername").focus();
            showAlertMessage("err", "Wrong input Username.", 1);
            result = false;
        }
        if ($("#chkproduct_1").attr("checked")) {
            var isalluncheck = true;
            var leagueobj = $(".chkselectleagues");
            leagueobj.each(function (index) {
                if ($(this).attr("checked")) {
                    isalluncheck = false;
                    return;
                }
            });
            if (isalluncheck) {
                showAlertMessage("err", "Please select at least one league.", 1);
                result = false;
            }
        }

        return result;
    },
    setMonitorParams: function () {
        monitorparams.fromdate = !filterManager.isHistory ? $("#txtfromdate").val() : $("#ddlinmonth").val().split("_")[0] + "/" + $("#ddlfromday").val() + "/" + $("#ddlinmonth").val().split("_")[1];
        monitorparams.todate = !filterManager.isHistory ? $("#txttodate").val() : $("#ddlinmonth").val().split("_")[0] + "/" + $("#ddltoday").val() + "/" + $("#ddlinmonth").val().split("_")[1];
        monitorparams.currency = parseInt($("#ddlcurrency").val());
        monitorparams.sporttype = parseInt($("#ddlsport").val());
        monitorparams.bettype = parseInt($("#ddlbettype").val());
        monitorparams.usernames = $("#txtusername").val();
        monitorparams.product = filterobj.Product;
        for (var i = 0; i < monitorparams.product.length; i++) {
            monitorparams.product[i].Value = $("#chkproduct_" + monitorparams.product[i].Id).attr("checked");
        }

        var live = -1;
        if ($("#rdall").attr("checked")) {
            live = -1;
        }
        else if ($("#rdnonelive").attr("checked")) {
            live = 0;
        }
        else {
            live = 1;
        }

        monitorparams.live = live;
        monitorparams.reportby = parseInt($("#ddlreportby").val());
        monitorparams.leagues = "";

        var leagueobj = $(".chkselectleagues");
        var ischeckedall = true;
        var isalluncheck = true;
        monitorparams.leagues = '';
        leagueobj.each(function (index) {
            if ($(this).attr("checked")) {
                monitorparams.leagues += $(this).attr("id").split('_')[1] + ",";
                isalluncheck = false;
            }
            else {
                ischeckedall = false;
            }
        });
        if (ischeckedall && $("#rdleague_0").attr("checked")) {
            monitorparams.leagues = "";
        }
        if (monitorparams.leagues.length > 0) {
            monitorparams.leagues = monitorparams.leagues.substring(0, monitorparams.leagues.length - 1);
        }

        if (!$('#chkproduct_1').is(':checked')) {//if Sportbook uncheck then @Leagues = ''
            monitorparams.leagues = "";
        }

        monitorparams.Turnover1 = $('#txtTurnover1').val();
        monitorparams.Turnover2 = $('#txtTurnover2').val();
        monitorparams.Winloss1 = $('#txtWinLoss1').val();
        monitorparams.Winloss2 = $('#txtWinLoss2').val();
    },
    OnKeyPress: function (e, nextElement) {
        var code = "";
        if (e.which)
            code = e.which;
        else if (e.keyCode)
            code = e.keyCode;
        if (code == 13) {
            $('#' + nextElement).focus();
        }
    },

    getMonitorReport: function () {
        $("#err").empty();
        var Turnover1 = $('#txtTurnover1').val();
        var Turnover2 = $('#txtTurnover2').val();
        var Winlosss1 = $('#txtWinLoss1').val();
        var Winlosss2 = $('#txtWinLoss2').val();

        if (Turnover1.length > 0 && Turnover2.length == 0) {
            $('#txtTurnover2').focus();
            showAlertMessage("err", "Enter the Turnover range. Eg: 100,000- 200,000", 1);
            return;
        }
        if (Turnover1.length == 0 && Turnover2.length > 0) {
            showAlertMessage("err", "Enter the Turnover range. Eg: 100,000- 200,000", 1);
            $('#txtTurnover1').focus();
            return;
        }
        if (Winlosss1.length > 0 && Winlosss2.length == 0) {
            showAlertMessage("err", "Enter the Win Loss range. Eg: 100,000- 200,000", 1);
            $('#txtWinLoss2').focus();
            return;
        }
        if (Winlosss1.length == 0 && Winlosss2.length > 0) {
            showAlertMessage("err", "Enter the Win Loss range. Eg: 100,000- 200,000", 1);
            $('#txtWinLoss1').focus();
            return;
        }

        if (!monitorManager.checkValidInput()) {
            $("#gridmonitor").addClass("hidden");
            return;
        }

        try { parent.BlockUI(); } catch (e) { }
        monitorManager.setMonitorParams();
        var param = {
            param: monitorparams,
            fromcache: false
        };
        param.param.pageIndex = 1;
        param.param.pageSize = 50;
        $.ajax({
            type: "POST",
            url: "Service/MonitorService.svc/GetMAMonitorReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                helper.setFormatReport();
                if (rs.Total != null && rs.Item.length > 0) {
                    var total = rs.Item.length;
                    monitorManager.arr = [];
                    for (var i = 0; i < total; i++) {
                        monitorManager.renderRow(rs.Item[i], monitorManager.currReportBy, monitorManager.currCss, i);
                    }
                    $("#colTitle").removeClass("w120 w260 w300").addClass(monitorManager.currCss).html(monitorManager.currTitle);
                    $("#tblbodyscrollmonitor").empty().html(monitorManager.arr.join(''));
                    $("#tblfooterscrollmonitor").empty().html(monitorManager.renderTotalRow(rs.Total, monitorManager.currCss));
                    monitorManager.totalItem = rs.TotalRows;
                    monitorManager.initPager(monitorManager.totalItem, 50, 0);
                    $("#gridmonitor").removeClass("hidden").reRenderScroller(monitorManager.currWidth, 499, true);
                }
                else {
                    $("#tblfooterscrollmonitor").empty();
                    $("#pagination").html("&nbsp;");
                    $("#colTitle").removeClass("w120 w260 w300").addClass(monitorManager.currCss).html(monitorManager.currTitle);
                    $("#tblbodyscrollmonitor").empty().html(monitorManager.renderNoData(monitorManager.currWidth - 3));
                    $("#gridmonitor").removeClass("hidden").reRenderScroller(monitorManager.currWidth, 499, true);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    getMonitorReportPaging: function () {
        try { parent.BlockUI(); } catch (e) { }
        var param = {
            param: monitorparams,
            fromcache: true
        };
        param.param.pageIndex = monitorManager.pageIndex;
        param.param.pageSize = monitorManager.pageSize;
        $.ajax({
            type: "POST",
            url: "Service/MonitorService.svc/GetMAMonitorReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.Total != null) {
                    var total = rs.Item.length;
                    monitorManager.arr = [];

                    for (var i = 0; i < total; i++) {
                        monitorManager.renderRow(rs.Item[i], monitorManager.currReportBy, monitorManager.currCss, i);
                    }
                    $("#tblbodyscrollmonitor").empty().html(monitorManager.arr.join(''));
                    $("#gridmonitor").removeClass("hidden").reRenderScroller(monitorManager.currWidth, 499, true);
                }
                else {
                    $("#tblbodyscrollmonitor").empty().html(renderNoData(monitorManager.currWidth - 30));
                    $("#gridmonitor").removeClass("hidden").reRenderScroller(monitorManager.currWidth, 499, true);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderNoData: function (width) {
        var html = '';
        html = '<tr><td colspan="10"><div class="c" style="width:' + width + 'px">There is no information available</div></td></tr>';
        return html;
    },
    renderRow: function (row, reportby, css, index) {
        var colHtml = '';
        if (reportby == 5) {
            switch (row.index) {
                case "151":
                    colHtml = row.Name + ' - ' + row.Home + ' - Race ' + row.Away;
                    break;
                case "161":
                    colHtml = row.Name + ' - Game No ' + row.Away;
                    break;
                case "162":
                    colHtml = row.Name + ' - Game Table - ' + row.Away;
                    break;
                case "1000":
                    colHtml = row.Name;
                    break;
                case "9":
                case "10":
                case "29":
                    colHtml = row.Name;
                    break;
                default:
                    colHtml = '<table border="0" cellspacing="0" cellpadding="0" style="width:100%;">\
                        <tr>\
                            <td style="width:40px; border:none 0px;word-wrap: break-word; word-break:break-all;" class="l">' + row.Name + '</td>\
                            <td style="width:120px; border:none 0px;" class="r ht">' + row.Home + '</td>\
                            <td style="width:20px; border:none 0px;" class="c">-vs-</td>\
                            <td style="width:120px; border:none 0px;" class="l at">' + row.Away + '</td>\
                        </tr>\
                       </table>';
                    break;
            }

        }
        else {
            colHtml = row.Name;
        }
        var html = '<tr class="' + (index % 2 == 0 ? 'evenrow' : '') + '">\
                        <td>\
                            <div class="' + css + '">' + colHtml + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r' + (parseFloat(row.Turnover) < 0 ? 'red' : '') + '">' + row.Turnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.Winloss) < 0 ? 'red' : '') + '">' + row.Winloss + '</div>\
                        </td>\
                        <td>\
                            <div class="w90 r">' + row.Betcount + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.ATurnover) < 0 ? 'red' : '') + '">' + row.ATurnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.AWinloss) < 0 ? 'red' : '') + '">' + row.AWinloss + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.MTurnover) < 0 ? 'red' : '') + '">' + row.MTurnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.MWinloss) < 0 ? 'red' : '') + '">' + row.MWinloss + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.STurnover) < 0 ? 'red' : '') + '">' + row.STurnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.SWinloss) < 0 ? 'red' : '') + '">' + row.SWinloss + '</div>\
                        </td>\
                    </tr>';
        monitorManager.arr.push(html);
    },
    renderTotalRow: function (row, css) {
        var html = '<tr>\
                        <td>\
                            <div class="' + css + ' c">' + row.Name + '</div>\
                        </td>\
                       <td>\
                            <div class="w100 r' + (parseFloat(row.Turnover) < 0 ? 'red' : '') + '">' + row.Turnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.Winloss) < 0 ? 'red' : '') + '">' + row.Winloss + '</div>\
                        </td>\
                        <td>\
                            <div class="w90 r">' + row.Betcount + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.ATurnover) < 0 ? 'red' : '') + '">' + row.ATurnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.AWinloss) < 0 ? 'red' : '') + '">' + row.AWinloss + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.MTurnover) < 0 ? 'red' : '') + '">' + row.MTurnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.MWinloss) < 0 ? 'red' : '') + '">' + row.MWinloss + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.STurnover) < 0 ? 'red' : '') + '">' + row.STurnover + '</div>\
                        </td>\
                        <td>\
                            <div class="w100 r ' + (parseFloat(row.SWinloss) < 0 ? 'red' : '') + '">' + row.SWinloss + '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    initPager: function (total_item, items_per_page, current_page) {
        var opt = { callback: monitorManager.pageselectCallback };
        // Collect options from the text fields - the fields are named like their option counterparts
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageselectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(monitorManager.totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        monitorManager.pageIndex = page_index + 1;
        monitorManager.pageSize = _items_per_page;

        monitorManager.getMonitorReportPaging();
        monitorManager.initPager(monitorManager.totalItem, _items_per_page, page_index);

        return false;
    },
    exportExcel: function () {
        try { parent.BlockUI(); } catch (e) { }
        monitorManager.setMonitorParams();
        var param = {
            param: monitorparams
        };
        param.param.pageIndex = 1;
        param.param.pageSize = 50;

        $.ajax({
            type: "POST",
            url: "Service/MonitorService.svc/CheckMAMonitorReportFromSession",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs != null && rs == true) {
                    var url = "DWMAMonitor.aspx?type=export&reporttitle=MA Monitor&reportby=" + param.param.reportby;
                    $("#frm").attr('src', url);
                }
                else {
                    window.location.href = "../../SignIn.aspx";
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });


    }

};
var helper = {
    // Sort by price high to low
    //data.sort(sort_by('price', true, parseInt));
    // Sort by city, case-insensitive, A-Z
    //data.sort(sort_by('city', false, function(a){return a.toUpperCase()}));
    //item.Items.sort(hepler.sort_by('Balance', false, parseFloat));
    sort_by: function (field, reverse, primer) {
        reverse = (reverse) ? -1 : 1;
        return function (a, b) {
            a = a[field];
            b = b[field];
            if (typeof (primer) != 'undefined') {
                a = primer(a.replace(/,/g, ''));
                b = primer(b.replace(/,/g, ''));
            }
            if (a < b) return reverse * -1;
            if (a > b) return reverse * 1;
            return 0;
        };
    },
    setColumnTitle: function (reportType) {
        var name = '';
        switch (parseInt(reportType)) {
            case 0:
                name = "Date";
                break;
            case 1:
                name = "Month";
                break;
            case 2:
                name = "Username";
                break;
            case 3:
                name = "Bet type";
                break;
            case 4:
                name = "League";
                break;
            case 5:
                name = "Match";
                break;
            default:
                name = "Match";
        }
        $("#colTitle").html(name);
    },
    setFormatReport: function () {
        monitorManager.currReportBy = parseInt($("#ddlreportby").val());
        switch (monitorManager.currReportBy) {
            case 0:
                monitorManager.currWidth = 1040;
                monitorManager.currCss = "w120";
                monitorManager.currTitle = 'Date';
                break;
            case 1:
                monitorManager.currWidth = 1040;
                monitorManager.currCss = "w120";
                monitorManager.currTitle = 'Month';
                break;
            case 2:
                monitorManager.currWidth = 1040;
                monitorManager.currCss = "w120";
                monitorManager.currTitle = 'UserName';
                break;
            case 2:
                monitorManager.currWidth = 1040;
                monitorManager.currCss = "w120";
                monitorManager.currTitle = 'Bet type';
                break;
            case 4:
                monitorManager.currCss = "w260";
                monitorManager.currWidth = 1180;
                monitorManager.currTitle = "League";
                break;
            case 5:
                monitorManager.currTitle = 'Game';
                monitorManager.currWidth = 1220;
                monitorManager.currCss = "w300";
                break;
            case 6:
                monitorManager.currTitle = 'Currency';
                monitorManager.currWidth = 1040;
                monitorManager.currCss = "w120";
                break;
            case 7:
                monitorManager.currTitle = 'Sport';
                monitorManager.currWidth = 1040;
                monitorManager.currCss = "w120";
                break;
            default:
                monitorManager.currWidth = 1040;
                monitorManager.currCss = "w120";
                monitorManager.currTitle = 'Bet type';
        }
    },
    renderLoader: function () {
        var html = '<tr>\
                    <td colspan="2"><div class="tinyloader" style="width:403px;">&nbsp;</div></td>\
                </tr>';
        return html;
    },
    renderNodataLeagues: function () {
        var html = '<tr>\
                    <td colspan="2"><div class="c" style="width:403px;">There is no information available</div></td>\
                </tr>';
        return html;
    },
    renderDay: function (id, curr, min, max) {
        var obj = $("#" + id);
        var opts = [];
        for (var i = min; i <= max; i++) {
            opts.push('<option value="' + i + '" ' + (i == curr ? 'selected="selected"' : '') + ' >' + i + '</option>');
        }
        obj.empty().html(opts.join(''));
    },
    renderCalendar: function (id, opts) {
        $("#" + id).datepicker(opts);
    },
    renderCalendarFromTo: function (ids, opts) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);

                filterManager.getSelectLeagues();
            }
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids.split(',')[0]).datepicker("setDate", new Date(opts.minDate));
        $(ids.split(',')[1]).datepicker("setDate", new Date(opts.maxDate));
    },
    renderInMonth: function (arrYears, currentMonth, balanceUpDay) {
        //alert(arrYears);
        if (arrYears != null && arrYears.length > 1) {
            var d = new Date();
            var startYear = arrYears[0];

            var currentYear = jQuery.browser.mozilla || jQuery.browser.version.indexOf("9") >= 0 ? d.getYear() + 1900 : d.getYear();
            RenderMonthly('ddlinmonth', startYear, currentYear, currentMonth, balanceUpDay);
            helper.changeInMonth(document.getElementById("ddlinmonth"));
        }
    },
    renderFromToDay: function (month, year, currentMonth, balanceUpDay) {
        var d = new Date();
        var endday = daysInMonth(month, year);

        var fday = 1;

        if (month == (d.getMonth() + 1) && year == d.getFullYear()) {
            endday = d.getDate() - 1;
        }

        //alert('month: ' + month + '\nyear: ' + year + '\ncurrentMonth: ' + currentMonth);
        var _balanceUpDay = endday;
        if (d.getFullYear() == year && currentMonth == month) {
            _balanceUpDay = balanceUpDay;
        }

        //alert(_balanceUpDay);

        helper.renderDay("ddlfromday", fday, fday, _balanceUpDay);
        helper.renderDay("ddltoday", _balanceUpDay, fday, _balanceUpDay);
        $("#ddlfromday,#ddltoday").attr("max", _balanceUpDay);

        filterManager.getSelectLeagues();
    },
    changeInMonth: function (obj) {
        var data = $(obj).attr("value").split("_");
        //alert(data[0]);
        helper.renderFromToDay((data[0]), (data[1]), filterobj.InMonth, filterobj.BeforeBalanceUpDay);
    },
    changeFromDay: function () {
        var objtoday = $("#ddltoday");
        var fromday = parseInt($("#ddlfromday").attr("value"));
        var today = parseInt(objtoday.attr("value"));
        var maxtoday = parseInt(objtoday.attr("max"));
        if (today < fromday) {
            today = fromday;
        }
        helper.renderDay("ddltoday", today, fromday, maxtoday);
        filterManager.getSelectLeagues();
    },
    changeToDay: function () {
        var today = parseInt($("#ddltoday").attr("value"));
        var fromday = parseInt($("#ddlfromday").attr("value"));
        if (fromday > today) {
            fromday = today;
        }
        helper.renderDay("ddlfromday", fromday, 1, today);

        filterManager.getSelectLeagues();
    },
    setPopupPosition: function (obj, target) {
        var offset = obj.offset();
        target.css("left", offset.left + "px");
        var top = offset.top + obj.height() + 5;
        target.css("top", top + "px");

    },
    checkProduct: function (obj) {
        var jobj = $(obj);

        if (jobj.hasClass("all")) {
            var checked = $(".all").attr("checked");
            $(".chkproduct").attr("checked", checked);
        }
        else {
            var ischeckedall = true;

            $(".chkproduct").not(".all").each(function (i) {
                var checked = $(this).attr("checked");
                if (!checked) {
                    ischeckedall = false;
                    $(".all").attr("checked", false);
                    return;
                }
            });
            if (ischeckedall) {
                $(".all").attr("checked", true);
            }
        }

        if ($("#chkproduct_1").attr("checked")) {
            $("#ddlsport, #btnselectleagues, .rdleague").removeAttr("disabled");
        }
        else {
            $("#ddlsport, #btnselectleagues, .rdleague").attr("disabled", "disabled");
            $("#selectleaguespopup").addClass("hidden");
            $("#btnselectleagues").val("Select Leagues");
        }

        filterManager.renderReportBy(filterobj.ReportBy);
        filterManager.getBettype();
    },
    checkAllSelectLeagues: function () {
        var checked = $("#chkallleagues").attr("checked");
        $(".chkselectleagues").attr("checked", checked);
    },
    checkSelectLeagues: function (obj) {
        if (!$(obj).attr("checked")) {
            $("#chkallleagues").attr("checked", false);
        }
        else {
            var checkedAll = true;
            $(".chkselectleagues").each(function () {
                if (!$(this).attr("checked")) {
                    checkedAll = false;
                    return;
                }
            });
            if (checkedAll) {
                $("#chkallleagues").attr("checked", true);
            }
        }
    }

};
