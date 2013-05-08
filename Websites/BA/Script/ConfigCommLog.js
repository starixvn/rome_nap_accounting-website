
$(window).load(function () {
     try { parent.jQuery.unblockUI(); } catch (e) { }  
});
var ConfigCommLog = {
    renderRow: function (row) {
        var html = '<tr class="lic_' + row.Name + '">\
                        <td><div class="col200 c">\
                               <table width="100%">\
                                 <tr>\
                                    <td style="border:none 0px; width:49px" class="c">Group A:</td>\
                                    <td style="border:none 0px; width:49px">' + row.ValueGroupA + '</td>\
                                    <td style="border:none 0px; width:49px" class="c">Group B:</td>\
                                    <td style="border:none 0px; width:49px">' + row.ValueGroupB + '</td>\
                                  </tr>\
                                  <tr>\
                                    <td style="border:none 0px;" class="c">Group C:</td>\
                                    <td style="border:none 0px;">' + row.ValueGroupC + '</td>\
                                    <td style="border:none 0px;" class="c">Group D:</td>\
                                    <td style="border:none 0px;">' + row.ValueGroupD + '</td>\
                                  </tr>\
                               </table>\
                        </td>\
                        <td><div class="w60 c">' + row.Value1X2Comm + '</div></td>\
                        <td><div class="w60 c">' + row.ValueOtherComm + '</div></td>\
                        <td><div class="w60 c">' + row.ValueGroup + '</div></td>\
                        <td><div class="w60 c"><span class="' + (row.EnableBa ? 'chkchecked' : 'chkuncheck') + '">&nbsp;</span></div></td>\
                        <td><div class="w100 c">' + row.ModifyBy + '</div></td>\
                        <td><div class="col100 c"><a href="' + iplocation.replace(/{ip}/gi, row.IP) + '" target="_blank" >' + row.IP + '</a></div></td>\
                        <td><div class="w120 c">' + row.ModifyDate + '</div></td>\
                        <td><div class="w160 c">' + row.Description + '</div></td>\
                    </tr>';
        return html;
    },
    viewGrid: function () {
        var licenseeId = parseInt($("#dllSite").val());
        ConfigCommLog.initGrid(licenseeId);
    },
    initGrid: function (licenseeId) {
        var param = {
            licenseeId: licenseeId
        };
        var arr = [];
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LIcenseeConfig.svc/SelBaLicenseeLogList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs != null) {
                    var len = rs.length;
                    for (i = 0; i < len; i++) {
                        var items = rs[i].Items;
                        var len1 = items.length;
                        if (len1 > 0) {
                            arr.push('<tr class="site"><td colspan="9"><div class="b" style="width:944px"><span class="tree-expanded" onclick="ConfigCommLog.toggleDetail(this,\'' + rs[i].Name + '\')"></span>' + rs[i].Name + '</div></td></tr>');
                            for (j = 0; j < len1; j++) {
                                arr.push(ConfigCommLog.renderRow(items[j]));
                            }
                        }
                    }
                    if (arr.length > 0) {
                        $("#ErrMsg").empty();
                        $("#tblcommconfiglog").empty().html(arr.join(''));
                        $("#commConfigLog").removeClass("hidden").reRenderScroller(947, 522, true);
                    }
                    else {
                        showAlertMessage("ErrMsg", "There is no data", 1);
                    }
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    toggleDetail: function (obj, licenseeId) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#tblcommconfiglog tr.lic_" + licenseeId).removeClass("hidden");
        }
        else {
            $("#tblcommconfiglog tr.lic_" + licenseeId).addClass("hidden");
            $(obj).addClass("tree-expand");
        }
        $("#commConfigLog").reRenderScroller(947, 522, true);
    }
};

$(function () {
    GetAllSite();
    if (licenseeId > 0) {
        ConfigCommLog.initGrid(licenseeId);
        try { parent.parent.jQuery.unblockUI(); } catch (e) { }
    }
});
function GetAllSite() {
    var obj = $('#dllSite');
    //obj.find('option').remove();
    $.ajax({
        type: "POST",
        url: "Service/LIcenseeConfig.svc/SelAllLicensee",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (rs) {
            for (i = 0; i < rs.length; i++) {
                if (licenseeId > 0 && rs[i].Id == licenseeId) {
                    obj.append('<option selected="selected" value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                }
                else {
                    obj.append('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                }
            }
        }
    });
}
