var objDetail = new Object();
$(function () {
 
});

var dailyTurnover = {
    date: '',
    width: 981,
    getReport: function () {
        try { parent.BlockUI(); } catch (e) { }
        var arr = [];
        dailyTurnover.date = $("#date").val();
        $('#ActionTurnover').addClass('hidden');
        $('#detailTurnover').addClass('hidden');
        $('#txtMess').hide();
        ClosePopup();
        var data = '{"date":"' + dailyTurnover.date + '"}';
        $("#gridtitle").html("Daily Turnover - Date: " + dailyTurnover.date);
        $.ajax({
            type: "POST",
            url: "Service/DailyTurnoverService.svc/SelDailyTurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            success: function (rs) {

                if (rs.length > 0) {
                    for (var i = 0; i < rs.length; i++) {
                        arr.push(dailyTurnover.renderRow(rs[i]));
                    }
                    $("#tbldailyturnover").html(arr.join(''));
                    $("#dailyturnover").removeClass("hidden").reRenderScroller(dailyTurnover.width, 542, true);

                }
                else {
                    $("#tbldailyturnover").html('<tr><td colspan="7"><div class="c b" style="width:' + (dailyTurnover.width - 3) + 'px">There is no data.</div></td></tr>');
                    $("#dailyturnover").removeClass("hidden").reRenderScroller(dailyTurnover.width, 542, true);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderRow: function (row) {
        var strrow = "";
        strrow += '<tr class="bgeven"><td colspan="7" ><div class="l b" style="width:' + (dailyTurnover.width - 3) + '">' + row.TypeName + '</div></td><tr>';
        strrow += '<tr>';
        strrow += '<td><div class="col120 c">' + row.StrDate + '</div></td>';
        strrow += '<td><div class="col120 r">' + row.Turnover + '</div></td>';
        strrow += '<td><div class="col120 r">' + row.STurnover + '</div></td>';
        strrow += '<td><div class="col120 r">' + row.MTurnover + '</div></td>';
        strrow += '<td><div class="col120 r">' + row.ATurnover + '</div></td>';
        strrow += '<td><div class="col120 r">' + row.IBCTurnover + '</div></td>';
        strrow += '<td><div class="col240 c">' + row.CNT + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },
    exportToExcel: function () {
        var params = 'type=export&export=0&date=' + dailyTurnover.date;
        $('#frm').attr('src', 'DailyTurnover.aspx?' + params);
    }
};

var detailTurnover = {
    date: '',
    getReportDetailTurnover: function () {
        try { parent.BlockUI(); } catch (e) { }
        var arr = [];
        var endRow = false;
        detailTurnover.date = $('#date').val();
        $('#txtMess').show();
        ClosePopup();
        var data = '{"date":"' + detailTurnover.date + '"}';
        $.ajax({
            type: "POST",
            url: "Service/DailyTurnoverService.svc/SelDetailTurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            success: function (rs) {
                objDetail = rs;
                if (rs.length > 0) {
                    for (var i = 0; i < rs.length; i++) {
                        if (i == rs.length - 1)
                            endRow = true;
                        if (i % 2 == 1)
                            arr.push(detailTurnover.renderRowDetail(rs[i], 'bgrow', endRow));
                        else
                            arr.push(detailTurnover.renderRowDetail(rs[i], '', endRow));
                    }
                    $("#tbldetailTurnover").html(arr.join(''));
                    $("#detailTurnover").removeClass("hidden").reRenderScroller(1024, 542, true);

                }
                else {
                    $("#tbldetailTurnover").html('<tr><td colspan="7"><div class="c b" style="width:1021px">There is no data.</div></td></tr>');
                    $("#detailTurnover").removeClass("hidden").reRenderScroller(1024, 542, true);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    renderRowDetail: function (row, cssClass, endRow) {
        var html = "";
        html += '<tr class="' + cssClass + '">';
        if (endRow) {
            html += '<td rowspan="4" style="border-bottom:0px none;"><div class="col140 c" style="border-bottom:none 0px;">' + row.StrDateTime + '</div></td>';
            html += '<td rowspan="4" style="border-bottom:0px none;"><div class="col140 c"><span <u style="color:blue; cursor:pointer" onclick="detailTurnover.ViewDetailAction(\'' + row.ActionName + '\',\'' + row.StrDateTime + '\',\'' + row.Username + '\')">' + row.ActionName + '</u></span></div></td>'
            html += '<td rowspan="4" style="border-bottom:0px none;"><div class="col120 c">' + row.Username + '</div></td>';
        }
        else {
            html += '<td rowspan="4"><div class="col140 c" style="border-bottom:none 0px;">' + row.StrDateTime + '</div></td>';
            html += '<td rowspan="4"><div class="col140 c"><span <u style="color:blue; cursor:pointer" onclick="detailTurnover.ViewDetailAction(\'' + row.ActionName + '\',\'' + row.StrDateTime + '\',\'' + row.Username + '\')">' + row.ActionName + '</u></span></div></td>'
            html += '<td rowspan="4"><div class="col120 c">' + row.Username + '</div></td>';
        }

        html += '<td  colspan="5"><div class="b"> Virtual Currency</div></td>';
        html += '<tr class="' + cssClass + '"><td><div class="col120 c">' + row.ActionDetail.TurnoverVirtual + '</div></td><td><div class="col120 c">' + row.ActionDetail.STurnoverVirtual + '</div></td><td><div class="col120 c">' + row.ActionDetail.MTurnoverVirtual + '</div></td><td><div class="col120 c">' + row.ActionDetail.ATurnoverVirtual + '</div></td><td><div class="col120 c">' + row.ActionDetail.IBCTurnoverVirtual + '</div></td></tr>';
        html += '<tr class="' + cssClass + '"><td  colspan="5"><div class="b"> Actual Currency</div></td></tr>';
        html += '<tr class="' + cssClass + '"><td><div class="col120 c">' + row.ActionDetail.TurnoverActual + '</div></td><td><div class="col120 c">' + row.ActionDetail.STurnoverActual + '</div></td><td><div class="col120 c">' + row.ActionDetail.MTurnoverActual + '</div></td><td><div class="col120 c">' + row.ActionDetail.ATurnoverActual + '</div></td><td><div class="col120 c">' + row.ActionDetail.IBCTurnoverActual + '</div></td></tr>';
        return html;

    },
    ViewDetailAction: function (action, strDateTime, username) {
        try { parent.BlockUI(); } catch (e) { }
        var title = action + " - " + strDateTime;
        var url = 'DetailActionTurnover.aspx?action=' + action + '&StrDateTime=' + strDateTime + '&Username=' + username;
        var width = 965;
        var height = 200;
        OpenPopup(url, title, width, height, false, false, window, 0, null, null);
        try { parent.jQuery.unblockUI() } catch (e) { };
    },
    exportToExcel: function () {
        var params = 'type=export&export=1&date=' + dailyTurnover.date;
        $('#frm').attr('src', 'DailyTurnover.aspx?' + params);
    }

};

var detailActionTurnover = {
    date: '',
    action: '',
    getdetailActionTurnover: function (action, datetime) {
        detailActionTurnover.date = datetime;
        detailActionTurnover.action = action;
        try { parent.BlockUI(); } catch (e) { }
        var arrDetail = [];
        var flag = true;
        var endRow = false;
        if (action.indexOf("Void") != -1) {
            $('#txtMatch').html('TransID');
            flag = false
        }
        else
            $('#txtMatch').html('Match');
        alert(objDetail);
        //var records = jlinq.from(objDetail).equals("StrDateTime", datetime).equals("ActionName", action).select();
        for (var index = 0; index < objDetail.length; index++) {
            if (objDetail[index].StrDateTime == datetime && objDetail[index].ActionName == action) {
                var records = objDetail[index].Detail;
                for (var j = 0; j < records.length; j++) {
                    if (j == records.length - 1)
                        endRow = true;
                    if (j % 2 == 1)
                        arrDetail.push(detailActionTurnover.renderDetailRow(records[j], 'bgrow', flag, endRow));
                    else
                        arrDetail.push(detailActionTurnover.renderDetailRow(records[j], '', flag, endRow));
                }

            }
        }
        //        if (records.length > 0) {
        //            for (var i = 0; i < records.length; i++) {
        //                var temp = records[i].Detail;
        //                for (var j = 0; j < temp.length; j++) {
        //                    if (j == temp.length - 1)
        //                        endRow = true;
        //                    if (j % 2 == 1)
        //                        arrDetail.push(detailActionTurnover.renderDetailRow(temp[j], 'bgrow', flag, endRow));
        //                    else
        //                        arrDetail.push(detailActionTurnover.renderDetailRow(temp[j], '', flag, endRow));

        //                }
        //            }
        //        }
        $("#tblActionTurnover").html(arrDetail.join(''));
        $("#ActionTurnover").removeClass("hidden").reRenderScroller(918, 542, true);
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    renderDetailRow: function (row, cssClass, flag, endRow) {
        var html = "";
        html += '<tr class="' + cssClass + '">';
        if (row.Detail == "")
            html += '<td rowspan="4" style="border-bottom:0px none"><div class="col300 c"></div></td>';
        else if (flag && endRow) {
            var match = row.Detail.split('$');
            html += '<td rowspan="4" style="border-bottom:0px none"><div class="col300 c"><span class="c">' + match[0] + '</span><br /><span class="c">' + match[1] + '</span></div></td>';
        }
        else if (!flag && endRow) {

            html += '<td rowspan="4" style="border-bottom:0px none"><div class="col300 c">' + row.Detail + '</div></td>';
        }
        else if (flag && !endRow) {
            var match = row.Detail.split('$');
            html += '<td rowspan="4"><div class="col300 c"><span class="c">' + match[0] + '</span><br /><span class="c">' + match[1] + '</span></div></td>';
        }
        else {
            html += '<td rowspan="4"><div class="col300 c">' + row.Detail + '</div></td>';
        }
        html += '<td  colspan="5"><div class="b"> Virtual Currency</div></td>';
        html += '<tr class="' + cssClass + '"><td><div class="col120 c">' + row.TurnoverVirtual + '</div></td><td><div class="col120 c">' + row.STurnoverVirtual + '</div></td><td><div class="col120 c">' + row.MTurnoverVirtual + '</div></td><td><div class="col120 c">' + row.ATurnoverVirtual + '</div></td><td><div class="col120 c">' + row.IBCTurnoverVirtual + '</div></td></tr>';
        html += '<tr class="' + cssClass + '"><td  colspan="5"><div class="b"> Actual Currency</div></td></tr>';
        html += '<tr class="' + cssClass + '"><td><div class="col120 c">' + row.TurnoverActual + '</div></td><td><div class="col120 c">' + row.STurnoverActual + '</div></td><td><div class="col120 c">' + row.MTurnoverActual + '</div></td><td><div class="col120 c">' + row.ATurnoverActual + '</div></td><td><div class="col120 c">' + row.IBCTurnoverActual + '</div></td></tr>';
        return html;
    },

    exportToExcel: function () {

        var params = 'type=export&export=2&datetime=' + detailActionTurnover.date + '&action=' + detailActionTurnover.action;
        $('#frm').attr('src', 'DailyTurnover.aspx?' + params);
    }

};

