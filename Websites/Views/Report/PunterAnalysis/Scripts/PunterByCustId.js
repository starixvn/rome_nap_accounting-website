var punterByCustId = {

    renderReport: function (rs) {
        var mainreport = $("#mainreport");
        mainreport.addClass("hidden");
        var PunterByCustIdreport = $("#PunterByCustIdreport");
        PunterByCustIdreport.resetGrid("grid2");

        $("#grid2 tbody").empty().append(punterByCustId.genBodyScrollSummary(rs));
        PunterByCustIdreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="r table-cell">' + rs.Total.Name + '</div></th></tr>');
        PunterByCustIdreport.find(".col_footer_content thead").empty().append(punterByCustId.genFooterScrollSummary(rs.Total));
        var gridOpts = {
            tblContentId: 'grid2',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            freezeWidth: 100,
            numberFreezeCol: 1
        };

        PunterByCustIdreport.removeClass("hidden");
        PunterByCustIdreport.gridScroller(gridOpts);

    },

    SelTopPunterByCustId: function () {
        try { parent.BlockUI(); } catch (e) { }

        //punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(false);
        PunterAnalysisExportExcel.BetCountType = 1;

        var param = {
            param: punter.param
        };

        $('#msg').empty();

        var action = 'PunterAnalysis/SelTopPunterByCustID';

        switch (punter.param.SearchBy) {
            case 0: //User
                action = 'PunterAnalysis/SelTopPunterByCustID';
                break;
            case 1: //League
                action = 'PunterAnalysis/SelTopPunterBySporttypeSum';
                break;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                cache.SelTopPunterByCustID = rs;
                punterByCustId.renderReport(rs);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var cnt = data.Punters.length;
        var userName = punter.param.CustName;
        var custId = 0;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                var isDisplayLink = data.Punters[i].Id != 0;
                // System Parlay and Mix Parlay
                if (!isDisplayLink) {
                    html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="l">' + data.Punters[i].Name + '</div></td>');
                } else {
                    html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="l link" onclick="punterByCustId.callSelTopPunterBySporttype(' + data.Punters[i].Id, ',\'' + data.Punters[i].Name + '\',' + data.Punters[i].CustId + ')">' + data.Punters[i].Name + '</div></td>');
                }

                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Winlost) + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].Turnover + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Margin) + '</div></td>');
                if (!isDisplayLink) {
                    html.push('<td><div class="r">' + data.Punters[i].Betcount + '</div></td>');
                }
                else {
                    //SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId)
                    html.push('<td><div class="r link" onclick="punterSelBetCountByCustomer.SelBetCountByCustomer(\'' + userName + '\',' + data.Punters[i].CustId + ',\'SelTopPunterByCustId\',' + false + ',' + null + ',' + null + ',' + null + ',' + data.Punters[i].Id + ',\'' + data.Punters[i].Name + '\')">' + data.Punters[i].Betcount + '</div></td>');
                }
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="4">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];
        if (total != null) {
            html.push('<tr><th ><div style="width:100px;height:30px;" class="table-cell">&nbsp;</div></th>');
            html.push('<th ><div style="width:100px;height:30px" class="r table-cell">' + Helper.format(total.Winlost) + '</th>');
            html.push('<th><div style="width:100px;height:30px" class="r table-cell">' + total.Turnover + '</th>');
            html.push('<th><div style="width:100px;height:30px" class="r table-cell">' + Helper.format(total.Margin) + '</th>');
            html.push('<th><div style="width:100px;height:30px" class="r table-cell">' + total.Betcount + '</th></tr>');
        }
        return html.join('');
    },

    callBackSelTopPunterSummary: function () {
        $("#mainreport").removeClass("hidden");
        $("#PunterByCustIdreport").addClass("hidden");
        $('#PunterBySporttypereport').addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#PunterSelIPByCustomerreport').addClass("hidden");
        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");

        if (punter.param.UserName != '') {
            PunterAnalysisExportExcel.BetCountType = 1;
        }
        else {
            PunterAnalysisExportExcel.BetCountType = 0;
        }

        var rs = '';
        if (punter.param.UserName == '') {
            rs = cache.SelTopPunterSummary;
            punter.renderReport(rs);
        }
        else {
            rs = cache.SelTopPunterByCustID;
            punterByCustId.renderReport(rs);
        }
    },

    callSelTopPunterBySporttype: function (sportId, sportName, custId) {

        $("#PunterByCustIdreport").addClass("hidden");

        var userName = $('#spUserName').html();
        var title = '';
        if (punter.param.UserName != '') {
            title = '<span id="spMain" class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">Main</span> >';
            title += '<span id="spSportName_custId">' + sportName + '</span>';
        }
        else {
            title = '<span id="spMain" class="link" onclick="punterByCustId.callBackSelTopPunterSummary()">Main</span> >';
            title += '<span id="spUserName" class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + userName + '</span> > ';
            title += '<span id="spSportName_custId">' + sportName + '</span>';
        }
        $('#gridtitleBySporttype').html(title);

        PunterAnalysisExportExcel.SportTypeId = sportId;
        PunterAnalysisExportExcel.SportTypeName = sportName;

        PunterAnalysisExportExcel.SportIdBetCount = sportId;
        PunterAnalysisExportExcel.SportNameBetCount = sportName;

        punterBySporttype.SelTopPunterBySporttype(sportId, custId);
    }
};