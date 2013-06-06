$(function(){
    var ddlSite=$("#ddlSite");
    var pos=ddlSite.offset();
    var top=($.browser.msie && parseInt($.browser.version)==9) ? pos.top+1:pos.top+2;
    
    $("#txtSite").css({width:(ddlSite.width()-20)+'px',
                       height:(ddlSite.height()-3)+'px',
                       top:(top)+'px',
                       left:(pos.left+2)+'px'
                       }).show();    
});


$(function () {
    totalTrans.getMonth();
    totalTrans.getSites();
    $("#ddlSite").change(function () {
        $("#ddlSite option:selected").each(function () {
            //alert($(this).attr("fulltext"));
            $("#txtSite").val($(this).attr("fulltext"));
        });
                
        TotalTransHelper.CheckLicenseeOfferRNGCasino(this.value);
    }).change();

});

var totalTrans = {
    eurCost: 0.13,
    title: '',
    date: '',
    reseller: '0',
    licensee: '0',
    showSB: true,
    showNG: true,
    showRB: true,
    showLC: true,
    ShowVS: true,
    showCS: true,
    getMonth: function () {
        var d = new Date();
        RenderMonthly("dllMonth", 2010, d.getFullYear(), d.getMonth() + 1);
    },
    getSites: function () {
        var arr = [];
        arr.push('<option value="0" reseller="0" fulltext="All Resellers">All Resellers</option>');
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/GetLicenseeByReseller",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs) {
                    var len = rs.length;
                    for (var i = 0; i < len; i++) {
                        var reseller = rs[i];
                        arr.push('<option value="0" reseller="' + reseller.ResellerID + '" fulltext="' + reseller.ResellerName + '">' + reseller.ResellerName + '</option>');
                        var len1 = reseller.Licensees.length;
                        for (var j = 0; j < len1; j++) {
                            var licensee = reseller.Licensees[j];
                            arr.push('<option value="' + licensee.LicenseeID + '" reseller="' + reseller.ResellerID + '" fulltext="' + reseller.ResellerName + " - " + licensee.LicenseeName + '">&nbsp;&nbsp;&nbsp;&nbsp;' + licensee.LicenseeName + '</option>');
                        }
                    }
                    var t = setTimeout(function () {
                        $("#ddlSite").empty().html(arr.join(''));
                        $("#btnReport").removeAttr("disabled");
                    }, 100);

                }
            }
        });
    },
    renderResellerRow: function (row, cssClass) {

        var html = '<tr class="' + cssClass + ' b">\
                        <td>\
                            <div class="col200 l" ><span class="tree-expanded" onclick="totalTrans.toggleGroup(this,\'' + (row.ResellerName || '') + '\')"></span>' + row.ResellerName + '</div>\
                        </td>\
                        <td>\
                            <div class="col140 r">' + row.Total + '</div>\
                        </td>\
                        <td>\
                            <div class="col140 r">' + row.EURCost + '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    renderLicenseeRow: function (row, reseller, cssClass) {
        var html = '<tr class="' + cssClass + ' lic_' + reseller + '" >\
                        <td>\
                            <div class="col200 l"><span class="indent" style="margin-right:6px;"></span>' + row.LicenseeName + '</div>\
                        </td>\
                        <td>\
                            <div class="col140 r">' + row.Total + '</div>\
                        </td>\
                        <td>\
                            <div class="col140 r">' + row.EURCost + '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    renderTotalRow: function (row) {
        var html = '<tr>\
                        <td>\
                            <div class="col200 l b">' + row.ResellerName + '</div>\
                        </td>\
                        <td>\
                            <div class="col140 r b">' + row.Total + '</div>\
                        </td>\
                         <td>\
                            <div class="col140 r b">' + row.EURCost + '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    gettotalTrans: function () {
        try { parent.BlockUI(); } catch (e) { }
        this.title = 'Total Transactions for ' + $("#txtSite").val() + ' in ' + $("#dllMonth option:selected").text();
        $("#gridtitle").html(this.title);
        var arrdate = $("#dllMonth").val().split("_");
        totalTrans.date = arrdate[1] + "-" + arrdate[0] + "-01";
        $("#ddlSite option:selected").each(function () {
            totalTrans.licensee = $(this).val();
            totalTrans.reseller = $(this).attr("reseller");
        });

        if ($("#chbSport").attr("checked"))
            totalTrans.showSB = true;
        else
            totalTrans.showSB = false;

        if ($("#chbNG").attr("checked"))
            totalTrans.showNG = true;
        else
            totalTrans.showNG = false;

        if ($("#chbRacing").attr("checked"))
            totalTrans.showRB = true;
        else
            totalTrans.showRB = false;

        if ($("#chbLiveCasino").attr("checked"))
            totalTrans.showLC = true;
        else
            totalTrans.showLC = false;

        if ($("#chkVirtualSport").attr("checked"))
            totalTrans.ShowVS = true;
        else
            totalTrans.ShowVS = false;

        if ($("#chkRNGCasino").attr("checked"))
            totalTrans.showCS = true;
        else
            totalTrans.showCS = false;

        if (arrdate.length > 1) {
            var data = {
                resellerid: totalTrans.reseller,
                licenseeid: totalTrans.licensee,
                date: totalTrans.date,
                showSB: totalTrans.showSB,
                showNG: totalTrans.showNG,
                showRB: totalTrans.showRB,
                showLC: totalTrans.showLC,
                showVS: totalTrans.ShowVS,
                showCS: totalTrans.showCS
            };
            $.ajax({
                type: "POST",
                url: "Service/LicenseeService.svc/SelTotalTransction",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (rs) {
                        var totalrow = rs.Items.length;
                        if (totalrow > 0) {
                            var arr = [];
                            for (i = 0; i < totalrow; i++) {
                                var reseller = rs.Items[i];
                                arr.push(totalTrans.renderResellerRow(reseller, "total"));
                                var len = reseller.Licensees.length;
                                for (j = 0; j < len; j++) {
                                    arr.push(totalTrans.renderLicenseeRow(reseller.Licensees[j], reseller.ResellerName.replace(/\s+/g, ""), ""));
                                }
                            }
                            $("#tbltotalTrans").empty().html(arr.join(''));
                            $("#tblFooter").empty().html(totalTrans.renderTotalRow(rs.Total));
                        }
                        else {
                            $("#tbltotalTrans").empty().html('<tr><td><div class="c" style="width:486px;">There is no data.</div></td></tr>');
                            $("#tblFooter").empty();
                        }
                        $("#totalTrans").removeClass("hidden").reRenderScroller(489, 470, true);
                    }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
    },
    toggleGroup: function (obj, reseller) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#tbltotalTrans tr.lic_" + reseller).removeClass("hidden");
        }
        else {
            $("#tbltotalTrans tr.lic_" + reseller).addClass("hidden");
            $(obj).addClass("tree-expand");
        }
        $("#totalTrans").reRenderScroller(489, 541, true);
    },
    exportExcel: function () {
        var url = "TotalTrans.aspx?IsExport=1&resellerid=" + this.reseller + "&licenseeid=" + this.licensee + "&fdate=" + this.date + "&showSB=" + this.showSB + "&showNG=" + this.showNG + "&showRB=" + this.showRB + "&showLC=" + this.showLC + "&showVS=" + this.ShowVS + "&reportTitle=" + this.title;
        $('#frm').attr('src', url);
    }
}

function change(obj)
{
    var checked=$(obj).attr("checked");
    $(".chksport").attr("checked",checked);
}

function changeSport(){
    var checked=true;
     $(".chksport").each(function(){
        if (!$(this).attr("checked"))
        {
            checked=false;
            return;
        }
     });
     $("#chbAll").attr("checked",checked);
 }