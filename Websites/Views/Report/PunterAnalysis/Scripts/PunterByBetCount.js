var punterSelBetCountByCustomer = {
    renderReport: function (rs) {
        var mainreport = $("#mainreport");
        mainreport.addClass("hidden");
        var PunterSelBetCountByCustomerreport = $("#PunterSelBetCountByCustomerreport");
        PunterSelBetCountByCustomerreport.resetGrid("grid6");

        $("#grid6 tbody").empty().append(punterSelBetCountByCustomer.genBodyScrollSummary(rs));
        PunterSelBetCountByCustomerreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="r table-cell">Total</div></th></tr>');
        PunterSelBetCountByCustomerreport.find(".col_footer_content thead").empty().append(punterSelBetCountByCustomer.genFooterScrollSummary(rs.Total));
        var gridOpts = {
            tblContentId: 'grid6',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 200,
            freezeWidth: 100,
            numberFreezeCol: 1
        };

        PunterSelBetCountByCustomerreport.removeClass("hidden");
        PunterSelBetCountByCustomerreport.gridScroller(gridOpts);

    },

    setTitlebyReportType: function (userName, type, bettypeName, liveindicator, sportName) {
        var title = '';
        var titleLive = '';
        switch (liveindicator) {
            case 0:
                titleLive = 'None Live';
                break;
            case 1:
                titleLive = 'Live';
                break;
            case 2:
                titleLive = 'None Live & Live';
                break;
        }

        switch (type) {
            case 'SelTopPunterSummary':
                title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';
                title += '<span>Punter ' + userName + ' by bet count</span>';
                break;
            case 'SelTopPunterByCustId':
                $('#PunterByCustIdreport').addClass("hidden");
                title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';
                if (punter.param.UserName.length == 0) {
                    title += '<span class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + userName + '</span><span> > </span>';
                }
                title += '<span>Punter ' + userName + ' by bet count of ' + sportName + '</span>';
                break;
            case 'SelTopPunterBySporttype':
                $('#PunterBySporttypereport').addClass("hidden");
                title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';

                if (punter.param.UserName.length == 0) {
                    title += '<span class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + userName + '</span><span> > </span>';
                }

                title += '<span class="link" onclick="punterByBettype.callBackSelTopPunterBySporttype()">' + $('#spSportName_custId').html() + '</span><span> > </span>';
                title += '<span>' + bettypeName + ' - ' + titleLive + '</span>';
                break;
        }
        return title;
    },

    SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId, sportName) {
        try { parent.BlockUI(); } catch (e) { }

        if (sportId == 0) {
            switch (punter.param.SearchBy) {
                case 0: //User
                    //  sportId = punter.getSelectedSport();
                    break;
                case 1: // League
                    //  sportId = $('#ddlSportLeague').val();
                    break;
            }
        }

        var oldCustName = punter.param.CustName;
        var oldCustId = punter.param.CustId;
        var oldLeagueId = punter.param.LeagueId;
        var _LiveIndicator = punter.param.LiveIndicator;
        var _SportId = punter.param.SportId;
        var _SportName = punter.param.SportName;
        var _BettypeId = punter.param.BettypeId;
        var _ProductIds = punter.param.ProductIds;


        //  punter.setParam();
        punter.param.CustName = userName;
        punter.param.CustId = custId;
        punter.param.LiveIndicator = liveindicator;

        if (punter.param.SearchBy == 0) //Usr
            punter.param.LeagueId = 0;

        PunterAnalysisExportExcel.FromDateCustBetCount = punter.param.FromDate;
        PunterAnalysisExportExcel.ToDateCustBetCount = punter.param.ToDate;
        PunterAnalysisExportExcel.CustIdCustBetCount = custId;
        PunterAnalysisExportExcel.CustNameCustBetCount = userName;
        PunterAnalysisExportExcel.PeriodTypeByBetCount = punter.PeriodType;
        PunterAnalysisExportExcel.LeagueId = punter.param.LeagueId;
        PunterAnalysisExportExcel.LeagueName = punter.param.LeagueName;
        PunterAnalysisExportExcel.LiveIndicatorBetCount = liveindicator;

        if (PunterAnalysisExportExcel.LeagueId == 0) {
            PunterAnalysisExportExcel.LeagueName = "All";
        }

        if (sportId != null && sportId != undefined) {
            if (sportId != 0) {
                punter.param.SportId = sportId;
                punter.param.ProductIds += "," + _SportId;
            }

            PunterAnalysisExportExcel.SportIdBetCount = sportId;
            if (sportName != null && sportName != undefined) {
                punter.param.SportName = sportName;
                PunterAnalysisExportExcel.SportNameBetCount = sportName;
            }
        }
        PunterAnalysisExportExcel.BettypeIdBetCount = bettype;
        PunterAnalysisExportExcel.BettypeNameBetCount = bettypeName;


        var param = {
            param: punter.param
        };

        $('#msg').empty();

        // set title        
        $('#gridtitleSelBetCountByCustomer').html(punterSelBetCountByCustomer.setTitlebyReportType(userName, type, bettypeName, liveindicator, sportName));

        var action = 'PunterAnalysis/SelBetCountByCustomer';
        if (IsDetail) {
            action = 'PunterAnalysis/SelBetCountByCustomer_Detail';
            punter.param.BettypeId = bettype;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                punterSelBetCountByCustomer.renderReport(rs);
                punter.param.CustName = oldCustName;
                punter.param.CustId = oldCustId;
                punter.param.LeagueId = oldLeagueId;
                punter.param.LiveIndicator = _LiveIndicator;

                punter.param.SportId = _SportId;
                punter.param.SportName = _SportName;
                punter.param.BettypeId = _BettypeId;
                punter.param.ProductIds = _ProductIds;
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var custId = punter.param.CustId;
        var cnt = data.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="c">' + data.Punters[i].Date + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].BetCount + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].WinCount + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].DrawCount + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Winlost) + '</div></td></tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="4">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];

        html.push('<tr>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >Total</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? total.BetCount : '') + '</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? total.WinCount : '') + '</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? total.DrawCount : '') + '</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? Helper.format(total.Winlost) : '') + '</div></th>');
        html.push('</tr>');

        return html.join('');
    }
};