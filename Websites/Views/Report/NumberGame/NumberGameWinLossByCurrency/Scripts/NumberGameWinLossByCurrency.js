var NumberGameWinLossByCur = {
    GetReportData: function (fromCache) {
        MainReport.ReportForMain(fromCache);
    }
};

var MainReport = {
    MainParams: {
        FromDate: '',
        ToDate: '',
        Currencies: '',
        Companies: '',
        Bettypes: '',
        IsShowComm: false,
        LiveStatus: 0,
        TotalItems: 0,
        PageSize: 50,
        PageIndex: 1,
        FromCache: false
    },
    RenderReport: function (rs) {
        var GridNumberGameWinLossByCur;
        var tbodyGrid = '';
        var tbodyRowHeader = '';
        var grid = '';

        if (MainReport.MainParams.IsShowComm) {
            GridNumberGameWinLossByCur = $("#NumberGameWinLossByCur");
            tbodyGrid = "#grid tbody";
            tbodyRowHeader = "#gridrowheader tbody";
            grid = 'grid';
            $("#NumberGameWinLossByCur").addClass("hidden");
            $("#NumberGameWinLossByCurNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetail").addClass("hidden");
        }
        else {
            GridNumberGameWinLossByCur = $("#NumberGameWinLossByCurNotComm");
            tbodyGrid = "#gridNotComm tbody";
            tbodyRowHeader = "#gridrowheaderNotComm tbody";
            grid = 'gridNotComm';
            $("#NumberGameWinLossByCur").addClass("hidden");
            $("#NumberGameWinLossByCurNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetail").addClass("hidden");
        }

        $(tbodyGrid).empty().append(MainReport.RenderBodycontent(rs.ListMember));
        $(tbodyRowHeader).empty().append(MainReport.RenderGridRowHeader(rs.ListMember));
        GridNumberGameWinLossByCur.find(".col_footer_content thead").empty().append(MainReport.RenderFooterColContent(rs.GrandTotal));
        GridNumberGameWinLossByCur.find(".row_footer_content thead").empty().append(MainReport.RenderFooterRowContent(rs.GrandTotal));
        var gridOpts = {
            tblContentId: grid,
            manualCopyHeader: true,
            fullWidth: true,
            gridHeight: 100,
            freezeWidth: 415,
            numberFreezeCol: 4
        };

        GridNumberGameWinLossByCur.removeClass("hidden");
        GridNumberGameWinLossByCur.gridScroller(gridOpts);

        var filterwidth = 0;
        var $filter = $('#filter');
        if (MainReport.MainParams.IsShowComm) {
            filterwidth = $('#NumberGameWinLossByCur').outerWidth();
        }
        else {
            filterwidth = $('#NumberGameWinLossByCurNotComm').outerWidth();
        }
//        $filter.css({ width: filterwidth - 6 });
    },
    RenderBodycontent: function (ListMember) {

        var html = [];
        var length = ListMember.length;

        for (var i = 0; i < length; i++) {//Rows
            html.push('<tr >');
            html.push('<td ><div style="height: 17px;" class="c" >&nbsp;</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >&nbsp;</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >&nbsp;</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >&nbsp;</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >' + ListMember[i].NetTurnover + '</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >' + Helper.formatCSS(ListMember[i].PTWinLoss) + '</div></td>');
            if (MainReport.MainParams.IsShowComm) {
                html.push('<td ><div style="height: 17px;" class="r" >' + ListMember[i].CommExpense + '</div></td>');
            }
            html.push('<td ><div style="height: 17px;" class="r" >' + Helper.formatCSS(ListMember[i].NetIncome) + '</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >' + Helper.formatCSS(ListMember[i].Margin + '%') + '</div></td>');
            html.push('</tr>');
        }

        return html.join('');

    },
    RenderGridRowHeader: function (ListMember) {
        var html = [];
        var length = ListMember.length;

        for (var i = 0; i < length; i++) {//Rows
            var onlick = "onclick=\"DetailReport.ReportForDetail(false," + "'" + ListMember[i].Currency + "'" + ", 0 , 0 );\"";
            html.push('<tr >');
            html.push('<td ><div style="height: 17px;" value="' + ListMember[i].Currency + '" class="c link" ' + onlick + '>' + ListMember[i].Currency + '</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >' + ListMember[i].Turnover + '</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >' + Helper.formatCSS(ListMember[i].MemberWinLoss) + '</div></td>');
            html.push('<td ><div style="height: 17px;" class="r" >' + Helper.formatCSS(ListMember[i].GamePayout + '%') + '</div></td>');
            html.push('</tr>');
        }


        return html.join('');
    },
    RenderFooterColContent: function (rs) {
        var html = [];

        html.push('<tr><th><div style="width:100px;height:17px;" class="c">Grand Total</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.Turnover + '</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.MemberWinLoss) + '</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.GamePayout + '%') + '</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.NetTurnover + '</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.PTWinLoss) + '</div></th>');
        if (MainReport.MainParams.IsShowComm) {
            html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.CommExpense + '</div></th>');
        }
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.NetIncome) + '</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.Margin + '%') + '</div></th>');
        html.push('</tr>');

        return html.join('');
    },
    RenderFooterRowContent: function (rs) {
        var html = [];

        html.push('<tr><th><div style="width:100px;height:17px;" class="c">Grand Total</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.Turnover + '</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.MemberWinLoss) + '</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.GamePayout + '%') + '</div></th>');
        html.push('</tr>');

        return html.join('');
    },
    ReportForMain: function (fromCache) {
        $("#err").removeClass("hidden");
        $("#err").empty();
        $("#NumberGameWinLossByCur").addClass("hidden");
        $("#NumberGameWinLossByCurNotComm").addClass("hidden");
        $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
        $("#NumberGameWinLossByCurDetail").addClass("hidden");

        var comSeleted = NumberGameWinLossByCur.Companies;
        var curSeleted = NumberGameWinLossByCur.Currencies;
        var Bettypeseleted = NumberGameWinLossByCur.Bettypes;

        if (comSeleted == -100 && curSeleted == -100 && Bettypeseleted == -100) {
            showAlertMessage("err", "Please choose Company, Bettype and Currency!", 1);
            return false;
        }
        else if (comSeleted == -100 && curSeleted == -100) {
            showAlertMessage("err", "Please choose Company and Currency!", 1);
            return false;
        }
        else if (comSeleted == -100 && Bettypeseleted == -100) {
            showAlertMessage("err", "Please choose Company and Bettype!", 1);
            return false;
        }
        else if (curSeleted == -100 && Bettypeseleted == -100) {
            showAlertMessage("err", "Please choose Currency and Bettype!", 1);
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
        else if (Bettypeseleted == -100) {
            showAlertMessage("err", "Please choose Bettype!", 1);
            return false;
        }

        Unility.GetParamFilter();

        MainReport.MainParams.Bettypes = Unility.Bettypes;
        MainReport.MainParams.Companies = Unility.Companies;
        MainReport.MainParams.Currencies = Unility.Currencies;
        MainReport.MainParams.FromDate = Unility.FromDate;
        MainReport.MainParams.ToDate = Unility.ToDate;
        MainReport.MainParams.IsShowComm = Unility.IsShowComm;
        MainReport.MainParams.LiveStatus = Unility.LiveStatus;
        MainReport.MainParams.FromCache = fromCache;

        if (!fromCache)
            MainReport.MainParams.PageIndex = 1;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByCurrency/GetNumberGameWinLossByCurMain",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(MainReport.MainParams),
            success: function (result) {
                if (result.Quantity > 0) {
                    MainReport.RenderReport(result);

                }
                else {
                    showAlertMessage("err", "There is no data.", 1);
                    $("#NumberGameWinLossByCur").addClass("hidden");
                    $("#NumberGameWinLossByCurNotComm").addClass("hidden");
                    $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
                    $("#NumberGameWinLossByCurDetail").addClass("hidden");
                    $("#paging").addClass("hidden");
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    ReportForMainBack: function (fromCache) {
        $("#err").addClass("hidden");
        $("#NumberGameWinLossByCur").addClass("hidden");
        $("#NumberGameWinLossByCurNotComm").addClass("hidden");
        $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
        $("#NumberGameWinLossByCurDetail").addClass("hidden");


        MainReport.MainParams.Bettypes = Unility.Bettypes;
        MainReport.MainParams.Companies = Unility.Companies;
        MainReport.MainParams.Currencies = Unility.Currencies;
        MainReport.MainParams.FromDate = Unility.FromDate;
        MainReport.MainParams.ToDate = Unility.ToDate;
        MainReport.MainParams.IsShowComm = Unility.IsShowComm;
        MainReport.MainParams.LiveStatus = Unility.LiveStatus;
        MainReport.MainParams.FromCache = fromCache;

        if (!fromCache)
            MainReport.MainParams.PageIndex = 1;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByCurrency/GetNumberGameWinLossByCurMain",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(MainReport.MainParams),
            success: function (result) {
                if (result.Quantity > 0) {
                    MainReport.RenderReport(result);
                }
                else {
                    showAlertMessage("err", "There is no data.", 1);
                    $("#NumberGameWinLossByCur").addClass("hidden");
                    $("#NumberGameWinLossByCurNotComm").addClass("hidden");
                    $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
                    $("#NumberGameWinLossByCurDetail").addClass("hidden");
                    $("#paging").addClass("hidden");
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    ExportExcelforMain: function (fromCache) {
        var p = MainReport.MainParams;
        var url = window.rootUrl + 'NumberGameWinLossByCurrency?IsExport=true&FDate=' + p.FromDate + '&TDate=' + p.ToDate +
         '&Companies=' + p.Companies + '&BetTypes=' + p.Bettypes + '&Currencies=' + p.Currencies + '&LiveStatus=' + p.LiveStatus + '&FromCache=' + fromCache + '&IsShowComm=' + p.IsShowComm;
        $('#frm').attr('src', url);
    }
};

var DetailReport = {
    DetailParams: {
        CustID: 0,
        RoleID: 0,
        FromDate: '',
        ToDate: '',
        Currencies: '',
        Companies: '',
        Bettypes: '',
        IsShowComm: false,
        LiveStatus: 0,
        TotalItems: 0,
        PageSize: 50,
        PageIndex: 1,
        FromCache: false,
        DetailName: ''
    },
    MainOnclick: '',
    CurrencyOnclick: '',
    Super: '',
    SuperOnclick: '',
    Master: '',
    MasterOnclick: '',
    Agent: '',
    AgentOnclick: '',
    Member: '',
    Title: '',
    GetLinkForTitle: function (fromCache, Currency, CustID, RoleID, DetailName) {
        DetailReport.MainOnclick = "onclick=\"MainReport.ReportForMainBack(false);\"";
        DetailReport.CurrencyOnclick = "onclick=\"DetailReport.ReportForDetail(false," + "'" + Currency + "'" + "," + 0 + "," + 0 + ", true );\"";

        if (RoleID == 4) {
            DetailReport.Super = DetailName;
            DetailReport.SuperOnclick = "onclick=\"DetailReport.ReportForDetail(false," + "'" + Currency + "'" + "," + CustID + "," + RoleID + "," + "'" + DetailName + "'" + ", true );\"";
            DetailReport.Master = '';
            DetailReport.Agent = '';

            DetailReport.Title = "<span id='main' class='text' " + DetailReport.MainOnclick + " >Main</span>  >  <span id='currenci' class='text' " + DetailReport.CurrencyOnclick + ">" + Currency + "</span>  >  <span id='super'>" + DetailReport.Super + "</span>";

        }
        else if (RoleID == 3) {
            DetailReport.Master = DetailName;
            DetailReport.MasterOnclick = "onclick=\"DetailReport.ReportForDetail(false," + "'" + Currency + "'" + "," + CustID + "," + RoleID + "," + "'" + DetailName + "'" + ", true );\"";
            DetailReport.Agent = '';

            DetailReport.Title = "<span id='main' class='text' " + DetailReport.MainOnclick + " >Main</span>  >  <span class='text' id='currenci'" + DetailReport.CurrencyOnclick + ">" + Currency + "</span>  >  <span class='text' id='super' " + DetailReport.SuperOnclick + ">" + DetailReport.Super + "</span>  >  <span id='master' >" + DetailReport.Master + "</span>";
        }
        else if (RoleID == 2) {
            DetailReport.Agent = DetailName;
            DetailReport.AgentOnclick = "onclick=\"DetailReport.ReportForDetail(false," + "'" + Currency + "'" + "," + CustID + "," + RoleID + "," + "'" + DetailName + "'" + ", true );\"";

            DetailReport.Title = "<span id='main' class='text' " + DetailReport.MainOnclick + " >Main</span>  >  <span id='currenci' class='text'" + DetailReport.CurrencyOnclick + ">" + Currency + "</span>  >  <span class='text' id='super' " + DetailReport.SuperOnclick + ">" + DetailReport.Super + "</span>  >  <span id='master' class='text' " + DetailReport.MasterOnclick + ">" + DetailReport.Master + "</span>  >  <span id='agent'>" + DetailReport.Agent + "</span>";
        }
        else if (RoleID == 1) {
            DetailReport.Member = DetailName;

            DetailReport.Title = "<span id='main' class='text' " + DetailReport.MainOnclick + " >Main</span>  >  <span id='currenci' class='text'" + DetailReport.CurrencyOnclick + ">" + Currency + "</span>  >  <span class='text' id='super' " + DetailReport.SuperOnclick + ">" + DetailReport.Super + "</span>  >  <span id='master' class='text' " + DetailReport.MasterOnclick + ">" + DetailReport.Master + "</span>  >  <span id='agent' " + DetailReport.AgentOnclick + ">" + DetailReport.Agent + "</span>  >  <span id='member' >" + DetailReport.Member + "</span>";
        }
        else {
            DetailReport.Title = "<span id='main' class='text' " + DetailReport.MainOnclick + " >Main</span>  >  <span id='currenci'>" + Currency + "</span> ";

        }
    },
    ReportForDetail: function (fromCache, Currency, CustID, RoleID, DetailName, isBack) {
        if (isBack == false) {
            Unility.GetParamFilter();
        }
        DetailReport.DetailParams.Bettypes = Unility.Bettypes;
        DetailReport.DetailParams.Companies = Unility.Companies;
        if (RoleID == 0) {
            DetailReport.DetailParams.Currencies = Currency;
            DetailReport.DetailParams.CustID = CustID;
            DetailReport.DetailParams.RoleID = RoleID;
        }
        else {
            DetailReport.DetailParams.Currencies = Currency;
            DetailReport.DetailParams.CustID = CustID;
            DetailReport.DetailParams.RoleID = RoleID - 1;
        }

        DetailReport.GetLinkForTitle(fromCache, Currency, CustID, RoleID, DetailName);
        DetailReport.DetailParams.FromDate = Unility.FromDate;
        DetailReport.DetailParams.ToDate = Unility.ToDate;
        DetailReport.DetailParams.IsShowComm = Unility.IsShowComm;
        DetailReport.DetailParams.LiveStatus = Unility.LiveStatus;
        DetailReport.DetailParams.FromCache = fromCache;
        DetailReport.DetailParams.DetailName = DetailName;

        if (!fromCache)
            DetailReport.DetailParams.PageIndex = 1;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByCurrency/GetNumberGameWinLossByCurDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(DetailReport.DetailParams),
            success: function (result) {
                if (result.Quantity > 0) {
                    DetailReport.RenderReportDetail(result);

                    DetailReport.DetailParams.TotalItems = result.Quantity;

                    if (DetailReport.DetailParams.IsShowComm) {
                        $('#gridtitleBySporttypeDetailNotComm').empty().append(DetailReport.Title);
                    }
                    else {
                        $('#gridtitleBySporttypeDetail').empty().append(DetailReport.Title);
                    }
                    DetailReport.initPager(DetailReport.DetailParams.TotalItems, DetailReport.DetailParams.PageSize, DetailReport.DetailParams.PageIndex - 1, DetailReport.DetailParams.IsShowComm);
                }
                else {
                    $("#NumberGameWinLossByCur").addClass("hidden");
                    $("#NumberGameWinLossByCurNotComm").addClass("hidden");
                    $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
                    $("#NumberGameWinLossByCurDetail").addClass("hidden");

                    showAlertMessage("err", "There is no data.", 1);
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    RenderReportDetail: function (rs) {
        var GridNumberGameWinLossByCur;
        var tbodyGrid = '';
        var tbodyRowHeader = '';
        var grid = '';
        var freeWidth = 0;

        if (DetailReport.DetailParams.IsShowComm) {
            GridNumberGameWinLossByCur = $("#NumberGameWinLossByCurDetail");
            freeWidth = 310;
            tbodyGrid = "#gridDetail tbody";
            tbodyRowHeader = "#gridrowheaderDetail tbody";
            grid = 'gridDetail';
            $("#NumberGameWinLossByCur").addClass("hidden");
            $("#NumberGameWinLossByCurNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetail").addClass("hidden");
        }
        else {
            GridNumberGameWinLossByCur = $("#NumberGameWinLossByCurDetailNotComm");
            freeWidth = 205;
            tbodyGrid = "#gridDetailNotComm tbody";
            tbodyRowHeader = "#gridrowheaderDetailNotComm tbody";
            grid = 'gridDetailNotComm';
            $("#NumberGameWinLossByCur").addClass("hidden");
            $("#NumberGameWinLossByCurNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetailNotComm").addClass("hidden");
            $("#NumberGameWinLossByCurDetail").addClass("hidden");
        }

        $(tbodyGrid).empty().append(DetailReport.RenderBodycontent(rs.ListUsername));
        $(tbodyRowHeader).empty().append(DetailReport.RenderGridRowHeader(rs.ListUsername, rs.Currencies));
        GridNumberGameWinLossByCur.find(".col_footer_content thead").empty().append(DetailReport.RenderFooterColContent(rs.Total));
        GridNumberGameWinLossByCur.find(".row_footer_content thead").empty().append(DetailReport.RenderFooterRowContent(rs.Total));

        var gridOpts = {
            tblContentId: grid,
            manualCopyHeader: true,
            fullWidth: true,
            gridHeight: 100,
            freezeWidth: freeWidth,
            numberFreezeCol: 3
        };

        GridNumberGameWinLossByCur.removeClass("hidden");
        GridNumberGameWinLossByCur.gridScroller(gridOpts);

        var filterwidth = 0;
        var $filter = $('#filter');
        if (DetailReport.DetailParams.IsShowComm) {
            filterwidth = $('#NumberGameWinLossByCurDetail').outerWidth();
        }
        else {
            filterwidth = $('#NumberGameWinLossByCurDetailNotComm').outerWidth();
        }
//        $filter.css({ width: filterwidth - 6 });
    },
    RenderBodycontent: function (ListMember) {
        var html = [];
        var length = ListMember.length;

        for (var i = 0; i < length; i++) {//Rows
            html.push('<tr >');
            html.push('<td ><div style="width:100px; height: 17px;" class="c" >&nbsp;</div></td>');
            if (DetailReport.DetailParams.IsShowComm) {
                html.push('<td ><div style="width:100px; height: 17px;" class="r" >&nbsp;</div></td>');
            }

            html.push('<td ><div style="width:100px; height: 17px;" class="r" >&nbsp;</div></td>');

            html.push('<td><div style="width:100px; height: 17px;" class="r">' + Helper.formatCSS(ListMember[i].MemberWinLoss) + '</div></td>');
            if (DetailReport.DetailParams.IsShowComm) {
                html.push('<td><div style="width:100px; height: 17px;" class="r">' + ListMember[i].MemberComm + '</div></td>');
                html.push('<td><div style="width:100px; height: 17px;" class="r">' + Helper.formatCSS(ListMember[i].MemberTotal) + '</div></td>');
            }


            html.push('<td><div style="width:100px; height: 17px;" class="r">' + Helper.formatCSS(ListMember[i].AgentWinLoss) + '</div></td>');
            if (DetailReport.DetailParams.IsShowComm) {
                html.push('<td><div style="width:100px; height:17px;" class="r">' + ListMember[i].AgentComm + '</div></td>');
                html.push('<td><div style="width:100px; height:17px;" class="r">' + Helper.formatCSS(ListMember[i].AgentTotal) + '</div></td>');
            }


            html.push('<td><div style="width:100px; height:17px;" class="r">' + Helper.formatCSS(ListMember[i].MasterWinLoss) + '</div></td>');
            if (DetailReport.DetailParams.IsShowComm) {
                html.push('<td><div style="width:100px; height:17px;" class="r">' + ListMember[i].MasterComm + '</div></td>');
                html.push('<td><div style="width:100px; height:17px;" class="r">' + Helper.formatCSS(ListMember[i].MasterTotal) + '</div></td>');
            }


            html.push('<td><div style="width:100px; height:17px;" class="r">' + Helper.formatCSS(ListMember[i].SuperWinLoss) + '</div></td>');
            if (DetailReport.DetailParams.IsShowComm) {
                html.push('<td><div style="width:100px; height:17px;" class="r">' + ListMember[i].SuperComm + '</div></td>');
                html.push('<td><div style="width:100px; height:17px;" class="r">' + Helper.formatCSS(ListMember[i].SuperTotal) + '</div></td>');
            }


            html.push('<td><div style="width:100px; height:17px;" class="r">' + Helper.formatCSS(ListMember[i].Company) + '</div></td>');

            html.push('</tr>');
        }


        return html.join('');
    },
    RenderGridRowHeader: function (ListMember, CurrencyDrillDown) {
        var html = [];
        var length = ListMember.length;

        for (var i = 0; i < length; i++) {//Rows
            if (ListMember[i].RoleID > 1) {
                var onlick = "onclick=\"DetailReport.ReportForDetail(false," + "'" + CurrencyDrillDown + "'" + "," + ListMember[i].CustID + "," + ListMember[i].RoleID + "," + "'" + ListMember[i].Username + "'" + " );\"";
                html.push('<tr >');
                html.push('<td ><div style="width:100px; height: 17px;" value="' + ListMember[i].Username + '" class="c link" ' + onlick + '>' + ListMember[i].Username + '</div></td>');
            }
            else {

                html.push('<tr >');
                html.push('<td ><div style="width:100px; height: 17px;" value="' + ListMember[i].Username + '" class="c">' + ListMember[i].Username + '</div></td>');
            }

            html.push('<td ><div style="width:100px; height: 17px;" class="r" >' + ListMember[i].Turnover + '</div></td>');
            if (DetailReport.DetailParams.IsShowComm) {
                html.push('<td ><div style="width:100px; height: 17px;" class="r" >' + Helper.formatCSS(ListMember[i].GrossComm) + '</div></td>');
            }

        }


        return html.join('');
    },
    RenderFooterColContent: function (rs) {
        var html = [];

        html.push('<tr><th><div style="width:100px;height:17px;" class="c">Total</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r"></div></th>');


        if (DetailReport.DetailParams.IsShowComm) {
            html.push('<th><div style="width:100px;height:17px;" class="r"></div></th>');
        }
        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.MemberWinLoss) + '</div></th>');
        if (DetailReport.DetailParams.IsShowComm) {
            html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.MemberComm + '</div></th>');
            html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.MemberTotal) + '</div></th>');
        }


        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.AgentWinLoss) + '</div></th>');
        if (DetailReport.DetailParams.IsShowComm) {
            html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.AgentComm + '</div></th>');
            html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.AgentTotal) + '</div></th>');

        }

        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.MasterWinLoss) + '</div></th>');
        if (DetailReport.DetailParams.IsShowComm) {
            html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.MasterComm + '</div></th>');
            html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.MasterTotal) + '</div></th>');
        }


        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.SuperWinLoss) + '</div></th>');
        if (DetailReport.DetailParams.IsShowComm) {
            html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.SuperComm + '</div></th>');
            html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.SuperTotal) + '</div></th>');
        }


        html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.Company) + '</div></th>');
        html.push('</tr>');

        return html.join('');
    },
    RenderFooterRowContent: function (rs) {
        var html = [];

        html.push('<tr><th><div style="width:100px;height:17px;" class="c">Total</div></th>');
        html.push('<th><div style="width:100px;height:17px;" class="r">' + rs.Turnover + '</div></th>');
        if (DetailReport.DetailParams.IsShowComm) {
            html.push('<th><div style="width:100px;height:17px;" class="r">' + Helper.formatCSS(rs.GrossComm) + '</div></th>');
        }
        html.push('</tr>');

        return html.join('');
    },
    ExportExcelforDetail: function (fromCache) {
        var p = DetailReport.DetailParams;
        var url = window.rootUrl + 'NumberGameWinLossByCurrency?IsExport=true&FDate=' + p.FromDate + '&TDate=' + p.ToDate +
         '&Companies=' + p.Companies + '&BetTypes=' + p.Bettypes + '&Currencies=' + p.Currencies + '&LiveStatus=' + p.LiveStatus + '&FromCache='
         + fromCache + '&IsShowComm=' + p.IsShowComm + '&IsDetail=true&CustID=' + p.CustID + '&RoleID=' + p.RoleID;
        $('#frm').attr('src', url);
    },
    initPager: function (total_items, items_per_page, current_page, isShowComm) {
        pagerOpts.callback = DetailReport.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        var $paging = '';
        $("#NumberGameWinLossByCurDetail").find("#pagination").removeClass("hidden");
        $("#NumberGameWinLossByCurDetailNotComm").find("#pagination").removeClass("hidden");
        if (isShowComm) {
            $paging = $("#NumberGameWinLossByCurDetail").find("#pagination");
        }
        else {
            $paging = $("#NumberGameWinLossByCurDetailNotComm").find("#pagination");
        }
        $($paging).pagination(total_items, pagerOpts);
        if (total_items < items_per_page) {
            $($paging).addClass("hidden");
        }
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        DetailReport.DetailParams.TotalItems = DetailReport.DetailParams.TotalItems;
        var max_page = Math.ceil(DetailReport.DetailParams.TotalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        DetailReport.DetailParams.PageIndex = page_index + 1;
        DetailReport.DetailParams.PageSize = _items_per_page;
        DetailReport.ReportForDetail(true, DetailReport.DetailParams.Currencies, DetailReport.DetailParams.CustID, DetailReport.DetailParams.RoleID);

        return false;
    }
};

var Unility = {
    FromDate: '',
    ToDate: '',
    Companies: '',
    Currencies: '',
    Bettypes: '',
    IsShowComm: false,
    LiveStatus: '',

    GetCurrencies: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByCurrency/GetCurrencies",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
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
                        NumberGameWinLossByCur.Currencies = $('#selectCurrency').multiSelect('getCheckedItems');
                        if (NumberGameWinLossByCur.Currencies.length <= 0) {
                            showAlertMessage("err", "Please choose Currency!", 1);
                            NumberGameWinLossByCur.Currencies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                NumberGameWinLossByCur.Currencies = $('#selectCurrency').multiSelect('getCheckedItems');
            },
            error: function () {
                NumberGameWinLossByCur.Currencies = "-100";
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
    GetBettyes: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByCurrency/GetBettypes",
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
                        NumberGameWinLossByCur.Bettypes = $('#selectBettypes').multiSelect('getCheckedItems');
                        if (NumberGameWinLossByCur.Bettypes.length <= 0) {
                            showAlertMessage("err", "Please choose Bettype!", 1);
                            NumberGameWinLossByCur.Bettypes = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                NumberGameWinLossByCur.Bettypes = $('#selectBettypes').multiSelect('getCheckedItems');

            },
            error: function () {
                NumberGameWinLossByCur.Bettypes = "-100";
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
    GetCompanies: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "NumberGameWinLossByCurrency/GetLicensees",
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
                        NumberGameWinLossByCur.Companies = $('#selectCompany').multiSelect('getCheckedItems');
                        if (NumberGameWinLossByCur.Companies.length <= 0) {
                            showAlertMessage("err", "Please choose Company!", 1);
                            NumberGameWinLossByCur.Companies = '-100';
                        }
                        else {
                            $('#err').empty();
                        }
                    }
                });
                NumberGameWinLossByCur.Companies = $('#selectCompany').multiSelect('getCheckedItems');
            },
            error: function (a, b, c) {
                console.log(a + " -- " + b + " -- " + c);
                NumberGameWinLossByCur.Companies = '-100';
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
    CompaniesChoosed: function () {
        Unility.Companies = $('#selectCompany').multiSelect('getCheckedItems');
        Unility.Companies = Unility.Companies.join(",");
    },
    CurrenciesChoosed: function () {
        Unility.Currencies = $('#selectCurrency').multiSelect('getCheckedItems');
        Unility.Currencies = Unility.Currencies.join(",");
    },
    BettypesChoosed: function () {
        Unility.Bettypes = $('#selectBettypes').multiSelect('getCheckedItems');
        Unility.Bettypes = Unility.Bettypes.join(",");
    },
    GetParamFilter: function () {

        Unility.CompaniesChoosed();
        Unility.CurrenciesChoosed();
        Unility.BettypesChoosed();
        Unility.FromDate = $('#txtfromdate').val();
        Unility.ToDate = $('#txttodate').val();
        Unility.IsShowComm = $('#chkExcludeTestAcc').is(':checked');
        Unility.LiveStatus = $('input[name=livetype]:checked').val();
    }
}

var pagerOpts = {
    callback: null,
    start_per_page: 10,
    step_per_page: 10,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

var Helper = {
    renderCalendarFromTo: function (ids) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
            }
        };

        var dates = $(ids).datepicker(defaults);
        var dt = new Date(_today);
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);
        fdate.datepicker("setDate", dt);
        tdate.datepicker("setDate", dt);
    },
    getShortDateString: function (oDate) {
        // month is 0 based (0 - 11)
        var strDate = gameStatisticsHelper.getDateStr(oDate.getMonth() + 1) + "/";
        strDate += gameStatisticsHelper.getDateStr(oDate.getDate()) + "/";
        strDate += gameStatisticsHelper.getDateStr(oDate.getFullYear());
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
    formatCSS: function (num) {
        if (num.indexOf("-") != -1) {
            return '<span style="color:red">' + num + '</span>'
        }
        return num;
    }
};

$(document).ready(function () {
    Helper.renderCalendarFromTo("#txtfromdate, #txttodate");

    Unility.GetCompanies();
    Unility.GetCurrencies();
    Unility.GetBettyes();

    var gridOptsNumberGameWinLossByCur = {
        tblContentId: 'grid',
        isShowPager: false
    };
    $("#NumberGameWinLossByCur").initGrid(gridOptsNumberGameWinLossByCur);
    var gridOptsNumberGameWinLossByCurNotComm = {
        tblContentId: 'gridNotComm',
        isShowPager: false
    };
    $("#NumberGameWinLossByCurNotComm").initGrid(gridOptsNumberGameWinLossByCurNotComm);
    var gridOptsNumberGameWinLossByCurDetail = {
        tblContentId: 'gridDetail',
        isShowPager: true
    };
    $("#NumberGameWinLossByCurDetail").initGrid(gridOptsNumberGameWinLossByCurDetail);
    var gridOptsNumberGameWinLossByCurDetailNotComm = {
        tblContentId: 'gridDetailNotComm',
        isShowPager: true
    };
    $("#NumberGameWinLossByCurDetailNotComm").initGrid(gridOptsNumberGameWinLossByCurDetailNotComm);

    //    alert($('#main').outerHeight());
});
//-
