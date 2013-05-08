var objDetail = new Object();
$(function () {
 
});

var detailActionTurnover = {
    date: '',
    action: '',
    username: '',
    ViewDetailAction: function (action, datetime, username) {
        try { parent.BlockUI(); } catch (e) { }
        var arr = [];
        var arrDetail = [];
        var flag = true;
        var endRow = false;
        detailActionTurnover.date = datetime;
        detailActionTurnover.action = action;
        detailActionTurnover.username = username;
        if (action.indexOf("Void") != -1) {
            $('#txtMatch').html('TransID');
            flag = false
        }
        else
            $('#txtMatch').html('Match');
        var data = '{"date":"' + detailActionTurnover.date + '","action":"' + detailActionTurnover.action + '","username":"' + detailActionTurnover.username + '"}';
        $.ajax({
            type: "POST",
            url: "Service/DailyTurnoverService.svc/SelDetailActionTurnOver",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            success: function (records) {
                if (records.length > 0) {
                    for (var i = 0; i < records.length; i++) {
                        if (i == records.length - 1)
                            endRow = true;
                        if (i % 2 == 1)
                            arrDetail.push(detailActionTurnover.renderDetailRow(records[i], 'bgrow', flag, endRow));
                        else
                            arrDetail.push(detailActionTurnover.renderDetailRow(records[i], '', flag, endRow));

                    }
                }

                $("#tblActionTurnover").html(arrDetail.join(''));
                $("#ActionTurnover").removeClass("hidden").reRenderScroller(918, 350, true);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

        try { parent.BlockUI(); } catch (e) { }



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

        var params = 'type=export&StrDateTime=' + detailActionTurnover.date + '&action=' + detailActionTurnover.action + '&username=' + detailActionTurnover.username;
        $('#frm').attr('src', 'DetailActionTurnover.aspx?' + params);
    }

};

