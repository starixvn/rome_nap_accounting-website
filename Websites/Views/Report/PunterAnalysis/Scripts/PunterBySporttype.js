var punterBySporttype = {

    renderReport: function (rs) {
        var PunterByCustIdreport = $("#PunterByCustIdreport");
        PunterByCustIdreport.addClass("hidden");

        var PunterBySporttypereport = $("#PunterBySporttypereport");
        PunterBySporttypereport.resetGrid("grid3");

        $("#grid3 tbody").empty().append(punterBySporttype.genBodyScrollSummary(rs));
        $("#grid3rowheader tbody").empty().append(punterBySporttype.gen1stColBodyScroll(rs));
        PunterBySporttypereport.find(".row_footer_content thead").empty().append('<tr><th ><div style="height:30px; width:200px;" class="r table-cell">' + rs.Total.Name + '</div></th></tr>');
        PunterBySporttypereport.find(".col_footer_content thead").empty().append(punterBySporttype.genFooterScrollSummary(rs.Total));
        var gridOpts = {
            tblContentId: 'grid3',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            freezeWidth: 200,
            numberFreezeCol: 1
        };

        PunterBySporttypereport.removeClass("hidden");
        PunterBySporttypereport.gridScroller(gridOpts);


    },

    SelTopPunterBySporttype: function (sportId, custId) {
        try { parent.BlockUI(); } catch (e) { }

        //punter.setParam();
        var _SportId = punter.param.SportId;
        var _CustId = punter.param.CustId;
        var _BettypeId = punter.param.BettypeId;

        if ($('#txtUserName').val() != 'All') {
            _CustId = custId;
        }

        PunterAnalysisExportExcel.saveParamForExportExcel(true);
        PunterAnalysisExportExcel.BetCountType = 2;
        punter.param.SportId = sportId;
        punter.param.CustId = custId;

        if (punter.param.SearchBy == 0) {
            punter.param.BettypeId = punter.getSelectedSport();
        }        

        var param = {
            param: punter.param
        };

        $("#puntergridBySporttype").removeClass("hidden");

        $('#msg').empty();

        var action = 'PunterAnalysis/SelTopPunterBySporttype';

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {

                cache.SelTopPunterBySporttype = rs;
                punterBySporttype.renderReport(rs);
            },
            complete: function () {
                punter.param.SportId = _SportId;
                punter.param.CustId = _CustId;
                punter.param.BettypeId = _BettypeId;
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (rs) {
        var html = [];

        var userName = punter.param.CustName;
        var custId = punter.param.CustId;
        var cnt = rs.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');
                html.push('<td ><div style="height:17px; " class="r" >' + rs.Punters[i].Name + '</div></td>');
                for (var j = 0; j < rs.Punters[i].Columns.length; j++) {//Tables
                    var data = rs.Punters[i].Columns[j];
                    var onlick = "onclick=\"punterBySporttype.callSelTopPunterByBettype(" + j + "," + rs.Punters[i].Id + "," + rs.Punters[i].SportType.Id + ",'" + rs.Punters[i].Name + "');\"";
                    html.push('<td><div class="r link" ' + onlick + '>' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></td>');
                    html.push('<td><div class="r ">' + (data.HasData == 1 ? data.Turnover : '') + '</div></td>');
                    html.push('<td><div class="r">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></td>');
                    //SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId)
                    html.push('<td><div class="r link" onclick="punterSelBetCountByCustomer.SelBetCountByCustomer(\'' + userName + '\',' + custId + ',\'SelTopPunterBySporttype\',' + true + ',' + rs.Punters[i].Id + ',\'' + rs.Punters[i].Name + '\',' + j + ',' + punter.param.SportId + ')">' + (data.HasData == 1 ? data.Betcount : '') + '</div></td>');
                }

                html.push('</tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;" colspan="13">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    gen1stColBodyScroll: function (rs) {
        var html = [];
        var cnt = rs.Punters.length;

        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');
                html.push('<td ><div style="width:200px;height:17px;" class="l" >' + rs.Punters[i].Name + '</div></td>');
                html.push('</tr>');
            }
        }
        else {
            html.push('<tr>');
            html.push('<td ><div style="width:200px;height:17px;" class="c" >&nbsp;</div></td>');
            html.push('</tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];

        html.push('<tr>');
        html.push('<th ><div style="width:200px; height:30px;" class="c table-cell" >' + total.Name + '</div></th>');
        for (var j = 0; j < total.Columns.length; j++) {//Tables
            var data = total.Columns[j];
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Turnover : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Betcount : '') + '</div></th>');
        }

        html.push('</tr>');

        return html.join('');
    },

    callBackSelTopPunterByCustId: function () {
        $('#PunterBySporttypereport').addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");

        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
        PunterAnalysisExportExcel.BetCountType = 1;
        var rs = cache.SelTopPunterByCustID;
        punterByCustId.renderReport(rs);
    },

    callSelTopPunterByBettype: function (liveindicator, bettype, sporttype, bettypeName) {
        var arr = [];

        var spMain = '<span id="spMain" class="link" onclick="punterByCustId.callBackSelTopPunterSummary()">Main</span> <span> > </span>';
        arr.push(spMain);

        if (punter.param.UserName.length == 0) {
            var spUserName = '<span id="spUserName" class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + $('#spUserName').html() + '</span> <span> > </span>';
            arr.push(spUserName);
        }

        var spSportName = '<span id="spSportName_sporttype" class="link" onclick="punterByBettype.callBackSelTopPunterBySporttype()">' + $('#spSportName_custId').html() + '</span> <span> > </span>';
        arr.push(spSportName);

        var title = '';
        switch (liveindicator) {
            case 0:
                title = 'None Live - ';
                break;
            case 1:
                title = 'Live - ';
                break;
            case 2:
                title = 'None Live & Live - ';
                break;
        }
        title += bettypeName;
        var spLiveIndicator = '<span id="spLiveindicator">' + title + '</span>';
        PunterAnalysisExportExcel.BettypeNameByBettype = bettypeName;
        arr.push(spLiveIndicator);

        $('#gridtitleByBettype').html(arr.join(''));

        punterByBettype.SelTopPunterByBettype(liveindicator, bettype, sporttype);

        $('#PunterBySporttypereport').addClass("hidden");

    }
};