



var PunterAnalysisExportExcel = {
    saveParamForExportExcel: function (useSporttype) {
        PunterAnalysisExportExcel.FromDate = punter.param.FromDate;
        PunterAnalysisExportExcel.ToDate = punter.param.ToDate;
        PunterAnalysisExportExcel.UserName = punter.param.UserName;
        PunterAnalysisExportExcel.OrderBy = punter.param.OrderBy;
        PunterAnalysisExportExcel.OrderType = punter.param.OrderType;
        PunterAnalysisExportExcel.TopRow = punter.param.TopRow;
        PunterAnalysisExportExcel.OrderByName = punter.param.OrderByName;
        PunterAnalysisExportExcel.OrderTypeName = punter.param.OrderTypeName;
        PunterAnalysisExportExcel.LastSearchBy = punter.SearchBy;
        PunterAnalysisExportExcel.CustId = punter.param.CustId;
        PunterAnalysisExportExcel.CustName = punter.param.CustName;
        PunterAnalysisExportExcel.PeriodType = punter.PeriodType;
        PunterAnalysisExportExcel.ProductIds = punter.param.ProductIds;

        if (useSporttype && useSporttype == true) {
            PunterAnalysisExportExcel.LeagueId = punter.param.LeagueId;
            if (PunterAnalysisExportExcel.LeagueId == 0) {
                PunterAnalysisExportExcel.LeagueName = "All";
            }
            else {
                PunterAnalysisExportExcel.LeagueName = punter.param.LeagueName;
            }
        }
        else {
            PunterAnalysisExportExcel.SportId = punter.param.SportId;
            PunterAnalysisExportExcel.SportName = punter.param.SportName;
            PunterAnalysisExportExcel.LeagueId = punter.param.LeagueId;
            PunterAnalysisExportExcel.LeagueName = punter.param.LeagueName;
        }

    },

    exportExcelTopPunterSummary: function () {
        var url = 'PunterAnalysisReport?ExportType=';
        switch (PunterAnalysisExportExcel.LastSearchBy) {
            case 0:
                url += 'TopPunterSummary';
                url += '&SportId=' + Helper.getSportId(PunterAnalysisExportExcel.SportId) + "," + Helper.getSportId(PunterAnalysisExportExcel.ProductIds, true);
                url += '&ProductIds=' + Helper.getSportId(PunterAnalysisExportExcel.ProductIds);
                break;
            case 1:
                url += 'TopPunterSummaryByLeague';
                url += '&SportId=' + Helper.getSportId(PunterAnalysisExportExcel.SportId);
                break;
        }

        url += '&FromDate=' + PunterAnalysisExportExcel.FromDate;
        url += '&ToDate=' + PunterAnalysisExportExcel.ToDate;

        url += '&UserName=' + PunterAnalysisExportExcel.UserName;
        url += '&OrderBy=' + PunterAnalysisExportExcel.OrderBy;
        url += '&OrderType=' + PunterAnalysisExportExcel.OrderType;
        url += '&TopRow=' + PunterAnalysisExportExcel.TopRow;
        url += '&LeagueId=' + PunterAnalysisExportExcel.LeagueId;
        url += '&OrderByName=' + PunterAnalysisExportExcel.OrderByName;
        url += '&OrderTypeName=' + PunterAnalysisExportExcel.OrderTypeName;
        url += '&PeriodType=' + PunterAnalysisExportExcel.PeriodType;

        $('#frm').attr('src', url);
    },
    exportExcelTopPunterByCustID: function () {
        var url = 'PunterAnalysisReport?ExportType=';
        switch (PunterAnalysisExportExcel.LastSearchBy) {
            case 0:
                url += 'TopPunterByCustID';
                break;
            case 1:
                url += 'TopPunterBySporttypeSum';
                break;
        }

        url += '&FromDate=' + PunterAnalysisExportExcel.FromDate;
        url += '&ToDate=' + PunterAnalysisExportExcel.ToDate;
        url += '&OrderBy=' + PunterAnalysisExportExcel.OrderBy;
        url += '&OrderType=' + PunterAnalysisExportExcel.OrderType;
        url += '&TopRow=' + PunterAnalysisExportExcel.TopRow;
        url += '&LeagueId=' + PunterAnalysisExportExcel.LeagueId;
        url += '&OrderByName=' + PunterAnalysisExportExcel.OrderByName;
        url += '&OrderTypeName=' + PunterAnalysisExportExcel.OrderTypeName;
        url += '&CustId=' + PunterAnalysisExportExcel.CustId;
        url += '&CustName=' + PunterAnalysisExportExcel.CustName;
        url += '&PeriodType=' + PunterAnalysisExportExcel.PeriodType;
        url += '&SearchBy=' + PunterAnalysisExportExcel.LastSearchBy;

        if (punter.param.SearchBy == 0) {
            url += '&SportId=' + Helper.getSportId(PunterAnalysisExportExcel.SportId) + "," + Helper.getSportId(PunterAnalysisExportExcel.ProductIds, true);
            url += '&ProductIds=' + Helper.getSportId(PunterAnalysisExportExcel.ProductIds);
        }
        else {
            url += '&SportId=' + $('#ddlSportLeague').val();
        }

        $('#frm').attr('src', url);
    },
    exportExcelTopPunterBySporttype: function () {
        var url = 'PunterAnalysisReport?ExportType=TopPunterBySporttype';

        url += '&FromDate=' + PunterAnalysisExportExcel.FromDate;
        url += '&ToDate=' + PunterAnalysisExportExcel.ToDate;
        url += '&OrderBy=' + PunterAnalysisExportExcel.OrderBy;
        url += '&OrderType=' + PunterAnalysisExportExcel.OrderType;
        url += '&OrderByName=' + PunterAnalysisExportExcel.OrderByName;
        url += '&OrderTypeName=' + PunterAnalysisExportExcel.OrderTypeName;
        url += '&TopRow=' + PunterAnalysisExportExcel.TopRow;
        url += '&LeagueId=' + PunterAnalysisExportExcel.LeagueId;
        url += '&CustId=' + PunterAnalysisExportExcel.CustId;
        url += '&CustName=' + PunterAnalysisExportExcel.CustName;
        url += '&SportTypeId=' + PunterAnalysisExportExcel.SportTypeId;
        url += '&SearchBy=' + PunterAnalysisExportExcel.LastSearchBy;
        url += '&PeriodType=' + PunterAnalysisExportExcel.PeriodType;
        url += '&ProductIds=' + Helper.getSportId(PunterAnalysisExportExcel.ProductIds);

        $('#frm').attr('src', url);
    },
    exportExcelTopPunterByBettype: function () {
        PunterAnalysisExportExcel.PeriodType = punter.param.PeriodType;

        var url = 'PunterAnalysisReport?ExportType=TopPunterByBettype';
        url += '&FromDate=' + PunterAnalysisExportExcel.FromDateByBettype;
        url += '&ToDate=' + PunterAnalysisExportExcel.ToDateByBettype;
        url += '&OrderBy=' + PunterAnalysisExportExcel.OrderBy;
        url += '&OrderType=' + PunterAnalysisExportExcel.OrderType;
        url += '&OrderByName=' + PunterAnalysisExportExcel.OrderByName;
        url += '&OrderTypeName=' + PunterAnalysisExportExcel.OrderTypeName;
        url += '&CustId=' + PunterAnalysisExportExcel.CustId;
        url += '&CustName=' + PunterAnalysisExportExcel.CustName;
        url += '&SportTypeId=' + PunterAnalysisExportExcel.SportIdByBettype;
        url += '&BettypeId=' + PunterAnalysisExportExcel.BettypeIdByBettype;
        url += '&LiveIndicator=' + PunterAnalysisExportExcel.LiveIndicator;
        url += '&PeriodType=' + PunterAnalysisExportExcel.PeriodTypeByBettype;
        url += '&SearchBy=' + PunterAnalysisExportExcel.LastSearchBy;
        url += '&LeagueId=' + PunterAnalysisExportExcel.LeagueId;
        url += '&ProductIds=' + Helper.getSportId(PunterAnalysisExportExcel.ProductIds);

        $('#frm').attr('src', url);

    },
    exportExcelTopPunterSelIPByCustomer: function () {
        var url = 'PunterAnalysisReport?ExportType=IPByCustId_TopPunterSummary';
        url += '&FromDate=' + PunterAnalysisExportExcel.FromDateCustIP;
        url += '&ToDate=' + PunterAnalysisExportExcel.ToDateCustIP;
        url += '&CustId=' + PunterAnalysisExportExcel.CustIdCustIP;
        url += '&CustName=' + PunterAnalysisExportExcel.CustNameCustIP;
        url += '&PeriodType=' + PunterAnalysisExportExcel.PeriodTypeByBettype;
        url += '&SearchBy=' + PunterAnalysisExportExcel.LastSearchBy;
        url += '&SportId=' + Helper.getSportId(PunterAnalysisExportExcel.SportId);
        url += '&ProductIds=' + Helper.getSportId(PunterAnalysisExportExcel.ProductIds);
        $('#frm').attr('src', url);
    },
    exportExcelTopPunterSelBetCountByCustomer: function () {
        var url = 'PunterAnalysisReport?ExportType=BetCountByCustId_TopPunterSummary';
        url += '&FromDate=' + PunterAnalysisExportExcel.FromDateCustBetCount;
        url += '&ToDate=' + PunterAnalysisExportExcel.ToDateCustBetCount;
        url += '&CustId=' + PunterAnalysisExportExcel.CustIdCustBetCount;
        url += '&CustName=' + PunterAnalysisExportExcel.CustNameCustBetCount;
        url += '&LeagueId=' + PunterAnalysisExportExcel.LeagueId;
        url += '&PeriodType=' + PunterAnalysisExportExcel.PeriodTypeByBetCount;
        url += '&SearchBy=' + PunterAnalysisExportExcel.LastSearchBy;
        switch (PunterAnalysisExportExcel.BetCountType) {
            case 0:
                url += '&SportTypeId=' + Helper.getSportId(PunterAnalysisExportExcel.SportId);
                if (PunterAnalysisExportExcel.LastSearchBy == 0) {
                    url += '&ProductIds=' + Helper.getSportId(PunterAnalysisExportExcel.ProductIds, true);
                }

                break;
            case 1:
                url += '&SportTypeId=' + PunterAnalysisExportExcel.SportIdBetCount;
                break;
            case 2:
                url += '&SportTypeId=' + PunterAnalysisExportExcel.SportIdBetCount;
                url += '&BettypeId=' + PunterAnalysisExportExcel.BettypeIdBetCount;
                url += '&LiveIndicator=' + PunterAnalysisExportExcel.LiveIndicatorBetCount;
                break;
        }

        $('#frm').attr('src', url);
    },
    getSelectedSportsName: function (searchBy) {
        var id = 'ddlSportLeague';
        switch (searchBy) {
            case 0:
                id = 'ddlSportUser';
                break;
            case 1:
                id = 'ddlSportLeague';
                break;
        }
        var selectedList = $('#' + id).multiSelect('getCheckedItemsByText');
        var result = "";
        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }
        result = result.replace(/,*$/, "");
        return result;
    }

};