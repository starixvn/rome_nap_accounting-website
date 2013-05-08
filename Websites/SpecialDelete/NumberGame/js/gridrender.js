var specialdelete = {
    betteam4: '',
    renderTitleRow: function (row) {
        var html = '';
        var ddldelete = '';
        if (row.IsAllowOptionDelete) {
            ddldelete = '<select onchange="ddl_change(\'' + row.Type + '\');" name="ddl' + row.Type + '" id="ddl' + row.Type + '">\
                            <option selected="selected" value="0" ' + (row.OptionType == '0' ? 'selected="selected"' : '') + ' >Not Delete</option>\
                            <option value="1" ' + (row.OptionType == '1' ? 'selected="selected"' : '') + '> Delete All</option>\
                            <option value="2" ' + (row.OptionType == '2' ? 'selected="selected"' : '') + '> Optional</option>\
                        </select>';
        }
        //width:1040 + 13+13*2
        html = '<tr style="background-color:#6495ed;"><td colspan="12"><div class="c b" style="color:#fff">' + row.Type + '</div></td><td><div class="col80 c">' + ddldelete + '</div></td></tr>'
        return html;
    },
    renderRowNG: function (row, IsAllowOptionDelete, OptionType) {
        var html = '';
        var optional = '';
        if (IsAllowOptionDelete) {
            optional = '<input type="checkbox" value="' + row.Refno + '" name="chk' + row.Type + '" id="' + row.Refno + '-' + row.Type + '-' + row.IsDelete + '" ' + (OptionType != '2' ? 'disabled="disabled"' : '') + ' ' + ((OptionType == '1' && row.IsDelete != "0") ? 'checked="true"' : '') + '>';
        }
        html = '<tr>\
                <td>\
                    <div class="col40 c">' + row.Order + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.Account + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + row.Refno + '</div>\
                </td>\
                <td >\
                    <div class="col160 c">' + row.Bettype + '</div>\
                </td>\
                <td style="">\
                    <div style="overflow:hidden!important;" class="col40 c">' + row.Betteam + '</div>\
                </td>\
                <td>\
                    <div class="col60 r b"><span style="color:blue;">' + row.Odds + '</span><br>' + row.OddType + '</div>\
                </td>\
                <td>\
                    <div class="col60 r red b">' + row.Amount + '</div>\
                </td>\
                    <td>\
                    <div class="col60 c">' + row.Currency + '</div>\
                </td>\
                    <td>\
                    <div class="col40 c">' + row.Hdp + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.TransTime + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.Creator + '</div>\
                </td>\
                <td>\
                    <div class="col60 c">' + row.Status + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + optional + '</div>\
                </td>\
            </tr>';

        return html;
    },
    renderRow: function (row, IsAllowOptionDelete, OptionType) {
        var html = '';
        var optional = '';
        if (IsAllowOptionDelete) {
            optional = '<input value="' + row.Refno + '" type="checkbox" name="chk' + row.Type + '" id="' + row.Refno + '-' + row.Type + '-' + row.IsDelete + '" ' + (OptionType != '2' ? 'disabled="disabled"' : '') + ' ' + ((OptionType == '1' && row.IsDelete != "0") ? 'checked="true"' : '') + '>';
        }
        
        html = '<tr>\
                <td>\
                    <div class="col40 c">' + row.Order + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.Account + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.Refno + '</div>\
                </td>\
                <td >\
                    <div class="col160 c">' + row.Bettype + '</div>\
                </td>\
                <td style="">\
                    <div style="" class="col40 c">' + row.Betteam + '</div>\
                </td>\
                <td>\
                    <div class="col60 r b"><span style="color:blue;">' + row.Odds + '</span><br>' + row.OddType + '</div>\
                </td>\
                <td>\
                    <div class="col60 r red b">' + row.Amount + '</div>\
                </td>\
                    <td>\
                    <div class="col60 c">' + row.Currency + '</div>\
                </td>\
                    <td>\
                    <div class="col40 c">' + row.Hdp + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.TransTime + '</div>\
                </td>\
                <td>\
                    <div class="col40 c" >' + row.Score.split(' - ')[0] + '</div>\
                </td>\
                 <td>\
                    <div class="col40 c">' + row.Score.split(' - ')[1] + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.Creator + '</div>\
                </td>\
                <td>\
                    <div class="col60 c">' + row.Status + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + optional + '</div>\
                </td>\
            </tr>';
        return html;
    },
    renderSpecialDeleteGrid: function (p, isNumberGame) {
        var arr = [];

        $.ajax({
            type: "POST",
            url: "../Services/SpecialDeleteService.svc/GetSpecialDeleteNG",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                var bettypes = rs.Items;
                if (bettypes != null && bettypes.length > 0) {
                    var isallnull = true;
                    var len = bettypes.length;
                    for (var i = 0; i < len; i++) {
                        var bettype = bettypes[i];
                        var items = bettype.Items;
                        var len1 = items.length;
                        if (len1 > 0) {
                            isallnull = false;
                        }
                        if (bettype.Type != "Normal" && len1 > 0) {
                            arr.push(specialdelete.renderTitleRow(bettype));
                        }
                        if (isNumberGame) {
                            for (var j = 0; j < len1; j++) {
                                arr.push(specialdelete.renderRowNG(items[j], bettype.IsAllowOptionDelete, bettype.OptionType));
                            }
                        }
                        else {
                            for (var j = 0; j < len1; j++) {
                                arr.push(specialdelete.renderRow(items[j], bettype.IsAllowOptionDelete, bettype.OptionType));
                            }
                        }
                    }
                    if (isallnull) {
                        $("#tblspecialdelete").empty().html('<tr><td><div class="c" style="width:1162px;">There is no information available</div></td></tr>');
                    }
                    else {
                        $("#tblspecialdelete").empty().html(arr.join(''));
                    }

                    $("#mainreport").removeClass("hidden");
                    $("#gridspecialdelete").reRenderScroller(1079, 562, true);
                    $('#btnDeleteBets').show();
                }
                else {

                    $("#tblspecialdelete").empty().html('<tr><td><div class="c" style="width:1076px;">There is no information available</div></td></tr>');
                    $("#mainreport").removeClass("hidden");
                    $("#gridspecialdelete").reRenderScroller(1079, 562, true);
                    //Render no data
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        });
    }
};