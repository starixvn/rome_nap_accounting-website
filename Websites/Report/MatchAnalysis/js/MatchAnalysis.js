function AllowNumber(e) {

    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;

    if (code == 13) {
        grid.getData();
    }

    if (code == 37 || code == 39 || code == 36 || code == 35 || code == 118) //e or E or "-"
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9)
        return false;

}
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
var grid = {
    me: null,
    tb: null,
    initGrid: function () {
        grid.me = $("#matchAnalysis");
        grid.th = $("#tblHeader");
        grid.tb = $("#tblContent tbody");
        grid.tf = $("#tblFooter");
    },
    trs: function () {
        return grid.tb.find("tr");
    },
    height: 450,
    width: 927,
    width1: 824,
    toalColumn: 9,
    isMemberReportType: false,
    params: {
        reportType: 0,
        matchId: '123456',
        statusId: 0
    },
    onChangeReportType: function () {
        grid.isMemberReportType = $("#ddlReportType").val() != "3" ? false : true;
        grid.renderStatus(($("#ddlReportType").val() == "3") ? true : false);
    },
    renderStatus: function (isMemberType) {
        if (isMemberType) {
            $("#ddlStatus").html('<option value="112">Won</option><option value="113">Lose</option>');
        }
        else {
            $("#ddlStatus").html('<option value="0">All</option><option value="112">Won</option><option value="113">Lose</option><option value="111">Draw</option>');
        }
    },
    createScroll: function () {
        grid.tf.removeClass("hidden");
        grid.me.removeClass('hidden').reRenderScroller(!grid.isMemberReportType ? grid.width1 : grid.width, grid.height, true);
        if (!grid.isMemberReportType) {
            $(".maincontent_box").css("width", "837px");
        }
        else {
            $(".maincontent_box").css("width", "940px");
        }
    },
    renderNoData: function () {
        grid.tb.html('<tr><td><div style="width:' + (!grid.isMemberReportType ? (grid.width1 - 3) : (grid.width - 3)) + 'px;" colspan=' + (grid.toalColumn - 1) + ' class="b c">There is no information available.</div></td></tr>');
        grid.me.removeClass('hidden').reRenderScroller(!grid.isMemberReportType ? grid.width1 : grid.width, grid.height, true);
        grid.tf.addClass("hidden");
        $("#reportFooter").hide();
    },
    setTitle: function () {
        var reportType = $('#ddlReportType').val();
        var title = '';
        switch (reportType) {
            case '0':
                title = 'Currency';
                break;
            case '1':
                title = 'Bet type';
                break;
            case '2':
                title = 'Super';
                break;
            case '3':
                switch ($('#ddlStatus').val()) {
                    case '112':
                        title = 'Top 100 (WON) Member';
                        break;
                    case '113':
                        title = 'Top 100 (LOSE) Member';
                        break;
                }
                break;
        }

        $('#ttlreportType').html(title);
    },
    getData: function () {
        grid.setParams();
        ///Ajax here
        if (grid.params.matchId != '') {
            //$("#ttlreportType").html($("#ddlReportType option:selected").text());
            grid.setTitle();
            $("#colCurrency").removeClass('hidden');
            if (!grid.isMemberReportType) {
                $("#colCurrency").addClass('hidden');
            }
            $("#gridtitle").html('Report Type: ' + $("#ddlReportType option:selected").text() + ' - Match ID: ' + grid.params.matchId);

            var data = {
                matchId: grid.params.matchId,
                statusId: grid.params.statusId,
                reportType: grid.params.reportType
            };
            try { parent.BlockUI(); } catch (e) { };
            $.ajax({
                type: "POST",
                url: "service/MatchAnalysis.svc/SelMatchAnalysisByType",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    grid.renderData(rs);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });

        }

    },
    allowViewReport: function () {
        if ($("#txtMatchId").val() != "" && isNumber($("#txtMatchId").val())) {
            $("#btnShow").removeAttr("disabled");
        }
        else {
            $("#btnShow").attr("disabled", "disabled");
        }
    },
    setParams: function () {
        grid.params.matchId = $("#txtMatchId").val();
        grid.params.reportType = parseInt($("#ddlReportType option:selected").val());
        grid.params.statusId = parseInt($("#ddlStatus option:selected").val());
    },
    renderData: function (_data) {
        var len = _data.Items.length;
        var arr = [];
        if (len > 0) {
            for (var i = 0; i < len; i++) {
                arr.push(grid.rendRow(_data.Items[i]));
            }
            grid.tb.html(arr.join(''));
            grid.tf.html(grid.rendRow(_data.Total));
            grid.createScroll();
        }
        else {
            grid.renderNoData();
        }
    },
    rendRow: function (row) {
        var html = '<tr>\
            <td>\
                <div class="col100 c">' + row.ItemName + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.MemberTO + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.MemberWL + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.BetCount.replace(/.00/gi, "") + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.HouseTO + '</div>\
            </td>\
            <td>\
                <div class="col100 r ' + (parseFloat(ReplaceAll(row.HouseWL, ',', '')) < 0 ? "red" : "") + '">' + row.HouseWL + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.SMATO + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.SMAWL + '</div>\
            </td>\
            <td ' + (!grid.isMemberReportType ? 'class="hidden"' : '') + '>\
                <div class="col100 c">' + row.Currency + '</div>\
            </td>\
        </tr>';
        return html;
    },
    exportToExcel: function () {
//        $("#frm").attr('src', 'MatchAnalysis.aspx?Export=1&matchId=' + grid.params.matchId + '&statusId=' + grid.params.statusId + '&reportType=' + grid.params.reportType);
    }
};

$(function () {
    grid.initGrid();
    grid.onChangeReportType();
});
