
$(document).ready(function () {

    var limitDate = {
        minDate: new Date(minDateDaily),
        maxDate: new Date($('#maxDate').val())
    };

    //daily
    Core.renderCalendarFromTo('#txtfromdate,#txttodate', limitDate);

    //weekly
    $('#Week').attr('data-mindate', minDateWeekly);
    $('#ToWeek').attr('data-mindate', minDateWeekly);
    Core.registerWeekPicker();
    Core.setFromToDateConstraint();
    Core.setWeekRangeConstraint();
    $('#Week').weekpicker('setDate', Core.maxDate);
    $('#ToWeek').weekpicker('setDate', Core.maxDate);

    var puntergrid = $("#puntergridSummary");

    var puntergridByCustId = $("#puntergridByCustId");

    var puntergridBySporttype = $("#puntergridBySporttype");

    var puntergridByBettype = $("#puntergridByBettype");

    punter.setSearchBy(0);
    punter.setPeriodType(0);
    punter.loadProducts();
    punter.loadSports();


    punter.SportSelected = punter.getSelectedSport();

    var gridOpts = {
        tblContentId: 'grid1'
    };
    $("#mainreport").initGrid(gridOpts);
    gridOpts.tblContentId = 'grid2';
    $("#PunterByCustIdreport").initGrid(gridOpts);
    gridOpts.tblContentId = 'grid3';
    $("#PunterBySporttypereport").initGrid(gridOpts);
    gridOpts.tblContentId = 'grid4';
    $("#PunterByBettypereport").initGrid(gridOpts);
    gridOpts.tblContentId = 'grid5';
    $("#PunterSelIPByCustomerreport").initGrid(gridOpts);
    gridOpts.tblContentId = 'grid6';
    $("#PunterSelBetCountByCustomerreport").initGrid(gridOpts);

});





var cache = {
    SelTopPunterSummary: [],
    SelTopPunterByCustID: [],
    SelTopPunterBySporttype: []
};



var punter = {
    SearchBy: 0, // 0:User, 1:League
    PeriodType: 0, // 0:Monthly, 1:Annual, 2:Long Range
    arrMonths: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    beginYear: 2007,
    SportSelected: '',
    ReportType: '',
    param: {
        FromDate: '',
        ToDate: '',
        SportId: '',
        BettypeId: '',
        UserName: '',
        TopRow: '',
        OrderBy: '', //@Orderby: 1 winlost, 2 turnover, 3 margin 
        OrderType: '', //@OrderType: 0 Ascending, 1 Descending
        LeagueId: '', //0:All
        CustId: '',
        LiveIndicator: '',
        PeriodType: '',
        SearchBy: '',
        ProductIds: ''
    },
    model: [],

    endYear: function () {
        var d = new Date(maxDate);
        return d.getFullYear();
    },
    genMonth: function (isCurrent) {
        var d = new Date(maxDate);
        var month = '';
        for (var i = 0; i < punter.arrMonths.length; i++) {
            if (i == d.getMonth() + 1 - (isCurrent ? 0 : 1)) {
                month += '<option selected="selected" value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
            else {
                month += '<option value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
        }
        return month;
    },

    genYear: function () {
        var d = new Date(maxDate);
        var year = '';
        for (var i = punter.beginYear; i <= punter.endYear(); i++) {
            if (i == d.getFullYear()) {
                year += '<option selected="selected" value=' + i + '>' + i + '</option>';
            }
            else {
                year += '<option value=' + i + '>' + i + '</option>';
            }
        }

        return year;
    },

    setSearchBy: function (type) {
        punter.SearchBy = type;

        $('#msg').empty();
        switch (type) {
            case 0: // Search by User
                $('#dvUserName').show();
                $('#dvSportByLeague').hide();
                $('#spReport1').show();
                $('#spReport2').hide();
                $('#dvSport').show();
                $('#trSearchByUser').show();
                $('#trSearchByLeague').hide();
                $('#trBettype').hide();
                break;
            case 1: //Search By League
                $('#dvUserName').hide();
                $('#dvSportByLeague').show();
                $('#spReport1').hide();
                $('#spReport2').show();
                $('#dvSport').hide();
                $('#trSearchByUser').hide();
                $('#trSearchByLeague').show();
                $('#trBettype').show();
                punter.loadLeagueBySport();
                punter.loadBettypes();
                break;
        }
    },

    setPeriodType: function (type) {
        punter.PeriodType = type;
        $('#PeriodType').val(type);

    },

    getSelectedSport: function () {
        var id = 'ddlSportUser';

        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";

        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }

        result = result.replace(/,*$/, "");

        var arr = result.split(',');

        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == '1_1') {
                result += ',1_18999';
                break;
            }
        }

        return result;
    },

    getSelectedProduct: function () {
        var id = 'ddlProduct';

        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";

        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }

        result = result.replace(/,*$/, "");

        return result;
    },

    getSelectedBettype: function () {
        var id = 'ddlBettype';

        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";

        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }

        result = result.replace(/,*$/, "");

        var arr = result.split(',');

        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == '1') {
                result += ',18999';
                break;
            }
        }

        return result;
    },

    daysInMonth: function (month, year) {
        return new Date(year, month, 0).getDate();
    },

    setDefaultTopRows: function () {
        if ($('#txtTopRows').val() == '' || isNaN($('#txtTopRows').val())) {
            $('#txtTopRows').val('10');
        }
    },

    setParam: function () {
        var fromdate = '';
        var todate = '';
        var sportId = '';

        switch (punter.SearchBy) {
            case 0: //User
                sportId = punter.getSelectedSport();
                sportName = PunterAnalysisExportExcel.getSelectedSportsName(punter.SearchBy);
                var username = $('#txtUserName').val().replace(/ /g, '');
                punter.param.UserName = username == 'All' ? '' : username;
                break;
            case 1: // League
                sportId = $('#ddlSportLeague').val();
                sportName = $('#ddlSportLeague').find('option:selected').text();
                punter.param.UserName = '';
                break;
        }

        var date = Helper.getFromDateToDate();
        fromdate = date.fromdate;
        todate = date.todate;
        punter.param.FromDate = fromdate;
        punter.param.ToDate = todate;
        punter.param.SportId = sportId;
        punter.param.OrderBy = $('#ddlOrderby').val();
        punter.param.OrderType = $('#ddlOrderType').val();
        punter.param.TopRow = $('#txtTopRows').val();
        punter.param.LeagueId = $('#ddlLeagues').val();
        punter.param.OrderByName = $('#ddlOrderby option:selected').text();
        punter.param.OrderTypeName = $('#ddlOrderType option:selected').text();
        punter.param.SportName = sportName;
        punter.param.LeagueName = $('#ddlLeagues option:selected').text();
        punter.param.SearchBy = punter.SearchBy;
        punter.param.PeriodType = parseInt($('#PeriodType').val());
        punter.param.ProductIds = punter.getSelectedProduct();
    },

    onclickUserName: function () {
        var username = $('#txtUserName').val();
        if (username == 'All') {
            $('#txtUserName').val('');
        }
    },

    onblurUserName: function () {
        var username = $('#txtUserName').val();
        if (username.length == 0) {
            $('#txtUserName').val('All');
        }
    },

    loadSports: function () {
        var p = {
            byleague: punter.SearchBy,
            products: punter.getSelectedProduct()
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "PunterAnalysis/SelSportsGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs.GroupName.length > 0) {
                    punter.renderSportSingleSelected(rs);
                    punter.renderSportMultiSelected(rs.GroupName, 'ddlSportUser');
                }
                else {
                    $('#ddlSportLeague').append('<select><option value="-0">None</option></select>');
                    $('#ddlSportUser').append('<select><option value="-0">None</option></select>');
                }
            }
        });
    },

    loadProducts: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "PunterAnalysis/SelProduct",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: '',
            success: function (rs) {
                if (rs.length > 0) {
                    punter.renderProductMultiSelected(rs, 'ddlProduct');
                }
            }
        });
    },

    loadBettypes: function () {
        var param = {
            SportId: $('#ddlSportLeague').val()
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "PunterAnalysis/SelBettypeBySportId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify(param),
            success: function (rs) {
                $('#spBettype').html('<span id="ddlBettype"></span>');
                punter.renderBettypeMultiSelected(rs, 'ddlBettype');
            }
        });
    },


    loadLeagueBySport: function () {
        if (punter.SearchBy == 0)
            return;

        var _fromDate = punter.param.FromDate;
        var _toDate = punter.param.ToDate;
        var _SportId = punter.param.SportId;
        var _OrderBy = punter.param.OrderBy;
        var _OrderType = punter.param.OrderType;
        var _TopRow = punter.param.TopRow;
        var _LeagueId = punter.param.LeagueId;
        var _OrderByName = punter.param.OrderByName;
        var _OrderTypeName = punter.param.OrderTypeName;
        var _SportName = punter.param.SportName;
        var _LeagueName = punter.param.LeagueName;
        var _SearchBy = punter.param.SearchBy;
        var _PeriodType = punter.param.PeriodType;

        var _punter_SearchBy = punter.SearchBy;
        var _punter_PeriodType = punter.PeriodType;
        var _punter_SportSelected = punter.SportSelected;
        var _punter_ReportType = punter.ReportType;
        var _UserName = punter.param.UserName;


        $('#msg').empty();

        punter.param.SportId = $('#ddlSportLeague').val();

        if (punter.param.SportId.length == 0) {
            showAlertMessage("msg", "Please choose at least one sport.", 1);
            $('#ddlLeagues').empty();
            return;
        }

        punter.setParam();

        var p = {
            param: punter.param
        };



        try { parent.BlockUI(); } catch (e) { }

        $.ajax({
            type: "POST",
            url: window.rootUrl + "PunterAnalysis/SelLeagueBySportId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {

                punter.param.FromDate = _fromDate;
                punter.param.ToDate = _toDate;
                punter.param.SportId = _SportId;
                punter.param.OrderBy = _OrderBy;
                punter.param.OrderType = _OrderType;
                punter.param.TopRow = _TopRow;
                punter.param.LeagueId = _LeagueId;
                punter.param.OrderByName = _OrderByName;
                punter.param.OrderTypeName = _OrderTypeName;
                punter.param.SportName = _SportName;
                punter.param.LeagueName = _LeagueName;
                punter.param.SearchBy = _SearchBy;
                punter.param.PeriodType = _PeriodType;
                punter.param.UserName = _UserName;

                punter.SearchBy = _punter_SearchBy;
                punter.PeriodType = _punter_PeriodType;
                punter.SportSelected = _punter_SportSelected;
                punter.ReportType = _punter_ReportType;


                $('#ddlLeagues').empty();
                if (rs.length > 0) {
                    for (var i = 0; i < rs.length; i++) {
                        $('#ddlLeagues').append('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                    }
                }
                else {
                    $('#ddlLeagues').append('<option value="-100000">There is no information available.</option>');
                }

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    renderSportMultiSelected: function (data, id) {
        $('#spSports').empty().append('<span id="ddlSportUser"></span>');
        var tempData = [];
        var products = punter.getSelectedProduct();
        var isExistsSportbook = products.split(',').length > 0 ? products.split(',')[0] == '1_0' : false;



        for (var i = 0; i < data.length; i++) {
            var groupSports = [];

            for (var j = 0; j < data[i].GroupSports.length; j++) {

                var sport = data[i].GroupSports[j];
                var betTypes = [];

                for (var k = 0; k < sport.Bettypes.length; k++) {
                    var bettype = sport.Bettypes[k];
                    betTypes.push({
                        Id: "#" + id + "Bettype_" + sport.Id + "_" + bettype.Id,
                        Value: sport.Id + "_" + bettype.Id,
                        Text: bettype.Name,
                        Checked: true,
                        Enable: true,
                        CssClass: ""
                    });
                }

                groupSports.push({
                    Id: "#" + id + "Detail_" + data[i].Id + "_" + sport.Id,
                    Value: data[i].Id + "_" + sport.Id,
                    Text: sport.Name,
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: betTypes,
                    Show: false
                });
            }

            if (!isExistsSportbook && data[i].Id == 1)//if products not contain sportbook then sport also remove Soccer and disabled.
            {

                continue;
            }

            if (data[i].Name != "Parlay" || data[i].GroupSports.length > 0) {
                tempData.push({
                    Id: id + data[i].Id.toString(),
                    Value: data[i].Id,
                    Text: data[i].Name,
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: groupSports
                });
            }
        }

        $("#" + id).multiSelect(
        {
            minWidth: 190,
            maxHeight: 350,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All',
            noneSelectedText: 'Select an option',
            disabled: !isExistsSportbook,
            listData: tempData,
            onCheck: function () {
                if (this.value != 1 && this.value != 999 && this.value != 'on' &&
                    this.checked == true) {

                    var v = $('#ddlSportUser').children('input').eq(0).val();
                    if (v == 'All') {
                        $('#ddlSportUserHeaderCheckBox').attr('checked', true);
                    }
                }
            }
        });

        if (!isExistsSportbook) {
            $("#ddlSportUser").multiSelect("unCheckAllItems");
            $('#ddlSportUser').children('input').eq(0).val('Not applicable');
        }

    },

    renderSportSingleSelected: function (data) {
        var html = [];
        html.push('<select style="width:190px" id="ddlSportLeague" onchange="punter.loadLeagueBySport();punter.loadBettypes()">');

        if (data.GroupName.length > 1) {
            html.push('<option value=' + data.GroupName[0].Id + '>' + data.GroupName[0].Name + '</option>');
        }

        for (var i = 0; i < data.GroupName[1].GroupSports.length; i++) {
            html.push('<option value=' + data.GroupName[1].GroupSports[i].Id + '>' + data.GroupName[1].GroupSports[i].Name + '</option>');
        }
        html.push('</select>');
        $('#spSportLeague').html(html.join(''));
    },

    renderProductMultiSelected: function (data, id) {
        var product = [];
        for (var i = 0; i < data.length; i++) {

            var sports = [];
            for (var j = 0; j < data[i].Sports.length; j++) {
                sports.push({
                    Id: "#" + id + "Detail" + data[i].Sports[j].Id.toString(),
                    Value: data[i].Id + '_' + data[i].Sports[j].Id,
                    Text: data[i].Sports[j].Name,
                    Checked: true,
                    Enable: true,
                    CssClass: ""
                });
            }

            product.push({
                Id: id + data[i].Id.toString(),
                Value: data[i].Id + '_0',
                Text: data[i].Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: sports
            });
        }

        $("#" + id).multiSelect(
        {
            minWidth: 190,
            maxHeight: 350,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All',
            noneSelectedText: 'Select an option',
            listData: product,
            onCheck: function () {
                if (this.value == '1_0' || this.id == 'ddlProductHeaderCheckBox') {//if sportsbook seleted then load sports
                    punter.loadSports();
                }
            }
        });
    },

    renderBettypeMultiSelected: function (data, id) {
        var bettype = [];
        for (var i = 0; i < data.length; i++) {

            bettype.push({
                Id: id + data[i].Id,
                Value: data[i].Id,
                Text: data[i].Name,
                Checked: true,
                Enable: true,
                CssClass: ""
            });
        }

        $("#" + id).multiSelect(
        {
            minWidth: 190,
            maxHeight: 300,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All',
            noneSelectedText: 'Select an option',
            listData: bettype,
            onCheck: function () {
            }
        });
    },

    renderReport: function (rs) {
        var mainreport = $("#mainreport");

        mainreport.removeClass("hidden");
        mainreport.resetGrid("grid1");
        $("#grid1 tbody").empty().append(punter.genBodyScrollSummary(rs));
        mainreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="r table-cell">' + rs.Total.Name + '</div></th></tr>');
        mainreport.find(".col_footer_content thead").empty().append(punter.genFooterScrollSummary(rs.Total));

        if (punter.SearchBy == 1) {
            $(".IPHeader").addClass("hidden");
        }
        else {
            $(".IPHeader").removeClass("hidden");
        }
        var grid1Width = 630;
        if (punter.SearchBy == 1) {
            grid1Width = 500;
        }

        var gridOpts = {
            tblContentId: 'grid1',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 160,
            gridWidth: grid1Width,
            freezeWidth: 100,
            numberFreezeCol: 1
        };
        mainreport.gridScroller(gridOpts);

    },

    generateReport: function () {
        punter.setParam();

        var rs = Helper.validInputParam();
        if (!rs) {
            var mainreport = $("#mainreport");
            mainreport.addClass("hidden");
            return;
        }

        try { parent.BlockUI(); } catch (e) { }

        if (punter.param.UserName != '')
            punter.param.CustId = 0;

        PunterAnalysisExportExcel.saveParamForExportExcel(false);

        if (punter.param.UserName != '') {
            PunterAnalysisExportExcel.BetCountType = 1;
        }
        else {
            PunterAnalysisExportExcel.BetCountType = 0;
        }

        var param = {
            param: punter.param
        };


        $('#msg').empty();

        $("#PunterByCustIdreport").addClass("hidden");
        $("#PunterBySporttypereport").addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#PunterSelIPByCustomerreport').addClass("hidden");
        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");


        $('#gridtitleSummary').html("Main - Punter Analysis");

        var action = 'PunterAnalysis/SelTopPunterSummary';
        switch (punter.SearchBy) {
            case 0: //User
                if (punter.param.UserName == '') {
                    action = 'PunterAnalysis/SelTopPunterSummary';
                }
                else {
                    action = 'PunterAnalysis/SelTopPunterByCustID';
                    PunterAnalysisExportExcel.CustName = punter.param.UserName;
                    punter.param.CustName = punter.param.UserName;
                    $('#gridtitleByCustId').html("Main - Punter Analysis");
                }
                break;
            case 1: //League
                punter.param.UserName = '';
                punter.param.CustName = '';
                punter.param.BettypeId = punter.getSelectedBettype();
                PunterAnalysisExportExcel.UserName = "";
                PunterAnalysisExportExcel.CustName = "";

                action = 'PunterAnalysis/SelTopPunterSummaryByLeague';
                break;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (punter.param.UserName == '') {
                    cache.SelTopPunterSummary = rs;
                    punter.renderReport(rs);
                }
                else {
                    cache.SelTopPunterByCustID = rs;
                    punterByCustId.renderReport(rs);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var cnt = data.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="l link" onclick="punter.callSelTopPunterByCustID(' + data.Punters[i].Id, ',\'' + data.Punters[i].Name + '\'' + ')">' + data.Punters[i].Name + '</div></td>')
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Winlost) + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].Turnover + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Margin) + '</div></td>');
                //SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId, sportName)
                html.push('<td><div class="r link" onclick="punterSelBetCountByCustomer.SelBetCountByCustomer(\'' + data.Punters[i].Name + '\',' + data.Punters[i].Id + ',\'SelTopPunterSummary\',' + false + ',' + null + ',' + null + ',' + null + ',' + data.Punters[i].SportType.Id + ',\'' + data.Punters[i].SportType.Name + '\')">' + data.Punters[i].Betcount + '</div></td>');
                if (punter.SearchBy == 0) {
                    if (punter.PeriodType != 0 &&
                        punter.PeriodType != 1) {
                        html.push('<td><div class="c link" onclick="punterSelIPByCustomer.SelTopPunterByCustId(\'' + data.Punters[i].Name + '\',' + data.Punters[i].Id + ')">view by ip</div></td></tr>');
                    }
                    else {
                        html.push('<td><div class="c">view by ip</div></td>');
                    }
                }
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="5">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];
        if (total != null) {
            html.push('<tr><th><div style="width:100px;height:30px;" class="table-cell">&nbsp;</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? Helper.format(total.Winlost) : '') + '</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? total.Turnover : '') + '</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? Helper.format(total.Margin) : '') + '</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? total.Betcount : '') + '</div></th>');
            if (punter.SearchBy == 0) {
                html.push('<th><div style="width:100px;height:30px;" class="r table-cell" >&nbsp;</div></th></tr>');
            }
        }
        return html.join('');
    },

    renderEmptyRow: function (col, width) {
        var html = "";
        html += '<tr>';
        html += '<td class="c" colspan="' + col + '">';
        html += '<div style="width:' + width + 'px">There is no information available.</div>';
        html += '</td>';
        html += '</tr>';
        return html;
    },

    callSelTopPunterByCustID: function (custId, userName) {
        punter.param.CustId = custId;
        punter.param.CustName = userName;
        punterByCustId.SelTopPunterByCustId();
        $('#SummaryLongRange').addClass('hidden');


        var title = '<span id="spMain" class="link" onclick="punterByCustId.callBackSelTopPunterSummary();">Main</span><span> > </span>';
        title += '<span id="spUserName">' + userName + '</span>';
        $('#gridtitleByCustId').html(title);
    }


};

