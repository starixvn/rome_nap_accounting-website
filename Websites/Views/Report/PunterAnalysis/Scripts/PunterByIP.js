var punterSelIPByCustomer = {

    renderReport: function (rs) {
        var mainreport = $("#mainreport");
        mainreport.addClass("hidden");

        var PunterSelIPByCustomerreport = $("#PunterSelIPByCustomerreport");
        PunterSelIPByCustomerreport.resetGrid("grid5");

        $("#grid5 tbody").empty().append(punterSelIPByCustomer.genBodyScrollSummary(rs));
        PunterSelIPByCustomerreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="c table-cell">&nbsp;</div></th></tr>');
        var gridOpts = {
            tblContentId: 'grid5',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            freezeWidth: 100,
            numberFreezeCol: 1
        };

        PunterSelIPByCustomerreport.removeClass("hidden");
        PunterSelIPByCustomerreport.gridScroller(gridOpts);

    },

    SelTopPunterByCustId: function (userName, custId) {
        try { parent.BlockUI(); } catch (e) { }

        var oldCustName = punter.param.CustName;
        var oldCustId = punter.param.CustId;

        // punter.setParam();
        punter.param.CustName = userName;
        punter.param.CustId = custId;

        //hard code for testing
        //        punter.param.FromDate = '01/01/2012';
        //        punter.param.ToDate = '31/12/2012';
        //        punter.param.CustId = 28104;

        PunterAnalysisExportExcel.FromDateCustIP = punter.param.FromDate;
        PunterAnalysisExportExcel.ToDateCustIP = punter.param.ToDate;
        PunterAnalysisExportExcel.CustIdCustIP = punter.param.CustId;
        PunterAnalysisExportExcel.CustNameCustIP = userName;
        PunterAnalysisExportExcel.PeriodTypeByBettype = punter.PeriodType;

        var param = {
            param: punter.param
        };

        $('#msg').empty();

        // set title
        var title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';
        title += '<span>Punter ' + userName + ' by IP from ' + punter.param.FromDate + ' to ' + punter.param.ToDate + '</span>';
        $('#gridtitleSelIPByCustomer').html(title);

        var action = 'PunterAnalysis/SelIPByCustomer';

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                punterSelIPByCustomer.renderReport(rs);
                punter.param.CustName = oldCustName;
                punter.param.CustId = oldCustId;
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var custId = punter.param.CustId;
        var cnt = data.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                html.push('<tr style="height: 17px;"><td style="height: 17px;" class="c"><div>' + data[i].Month + '</div></td>');
                html.push('<td><div class="c">' + data[i].Year + '</div></td>');
                html.push('<td><div class="c">' + data[i].IP + '</div></td>');
                html.push('<td><div class="c">' + data[i].Country + '</div></td></tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="3">There is no information available.</td></tr>');
        }
        return html.join('');
    }



};