$(function () {
    negativeBalance.renderLicensees();
});

var hepler = {
    // Sort by price high to low
    //data.sort(sort_by('price', true, parseInt));
    // Sort by city, case-insensitive, A-Z
    //data.sort(sort_by('city', false, function(a){return a.toUpperCase()}));
    //item.Items.sort(hepler.sort_by('Balance', false, parseFloat));
    sort_by: function (field, reverse, primer) {
        reverse = (reverse) ? -1 : 1;
        return function (a, b) {
            a = a[field];
            b = b[field];
            if (typeof (primer) != 'undefined') {
                a = primer(a.replace(/,/g, ''));
                b = primer(b.replace(/,/g, ''));
            }
            if (a < b) return reverse * -1;
            if (a > b) return reverse * 1;
            return 0;
        }
    }
};
var pagerOpts = {
    callback: null,
    start_per_page: 100,
    step_per_page: 100,
    max_items_per_page: 1000,
    items_per_page: 500,
    current_page: 1,
    is_show_detail: false
};
var negativeBalance = {
    pageIndex: 1,
    pageSize: 500,
    totalItem: 0,
    licensee: '',
    title: '',
    mailList: [],
    sortDESC: null,
    data: null,
    sortBalance: function (obj) {
        if (negativeBalance.sortDESC == null) {
            negativeBalance.sortDESC = true;
        }
        if (negativeBalance.sortDESC == true) {
            $("#" + obj).removeClass("desc");
            $("#" + obj).addClass("asc");

        }
        else {
            $("#" + obj).removeClass("asc");
            $("#" + obj).addClass("desc");
        }
        negativeBalance.sortDESC = !negativeBalance.sortDESC;
        negativeBalance.sortData(negativeBalance.sortDESC);
        negativeBalance.reRenderData();
    },
    getLicensees: function () {
        var result = '';
        var obj = $("#dllSite");
        if (obj.val() == "0") {
            result = '';
        }
        else {
            result = obj.val();
        }
        return result;
    },
    initPager: function (total_item, items_per_page, current_page) {
        pagerOpts.callback = negativeBalance.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_item, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(negativeBalance.totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        negativeBalance.pageIndex = page_index + 1;
        negativeBalance.pageSize = _items_per_page;
        negativeBalance.renderNegativeBalance(true);
        return false;
    },
    renderLicensees: function () {
        var param = {
            IsAll: true
        };
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/SelLicensees",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                var dlSite = $('#dllSite');
                for (i = 0; i < rs.length; i++) {
                    dlSite.append("<option value='" + rs[i].Name + "'>" + rs[i].Name + "</option>");
                }
            }
        });
    },
    renderLicenseeRow: function (row) {
        var html = '<tr class="total">\
                    <td>\
                        <div class="col160 l b"><span class="tree-expanded" onclick="negativeBalance.toggleLicenseeDetail(this,\'' + (row.Name || '') + '\')"></span>' + (row.Name || '') + '</div>\
                    </td>\
                    <td>\
                        <div class="col160 r b">&nbsp;</div>\
                    </td>\
                    <td>\
                        <div class="col120 c b">&nbsp;</div>\
                    </td>\
                </tr>';
        return html;
    },
    renderLicenseeDetailRow: function (row, licensee, cssClass) {
        var html = '<tr class="lic_' + licensee + ' ' + cssClass + '">\
                    <td>\
                        <div class="col160 l"><span class="indent" style="margin-right:6px;"></span>' + row.Name + '</div>\
                    </td>\
                    <td>\
                        <div class="col160 r">' + row.Balance + '</div>\
                    </td>\
                    <td>\
                        <div class="col120 c">' + row.Currency + '</div>\
                    </td>\
                </tr>';
        return html;
    },
    renderGrandTotal: function (row) {
        var html = '<tr>\
                    <td>\
                        <div class="col160 b l">' + row.Name + '</div>\
                    </td>\
                    <td>\
                        <div class="col160 b r">' + row.Balance + '</div>\
                    </td>\
                    <td>\
                        <div class="col120 b c"></div>\
                    </td>\
                </tr>';
        return html;
    },
    renderNegativeBalance: function (fromCache) {
        try { parent.BlockUI(); } catch (e) { }
        if (negativeBalance.licensee != negativeBalance.getLicensees()) {
            negativeBalance.pageIndex = 1;
        }
        negativeBalance.licensee = negativeBalance.getLicensees();
        $("#ErrMsg").html("");
        $("#balanceSorter").removeClass("desc");
        $("#balanceSorter").removeClass("asc");
        $("#balanceSorter").bind("click", function () { negativeBalance.sortBalance("balanceSorter"); });
        negativeBalance.sortDESC = null;
        var params = {
            site: negativeBalance.licensee,
            pageIndex: negativeBalance.pageIndex,
            pageSize: negativeBalance.pageSize,
            fromCache: fromCache
        };

        var lsName = $("#dllSite").val().charAt(0).toLocaleUpperCase() + $("#dllSite").val().slice(1);
        negativeBalance.title = String.format(title, $("#dllSite").val() == "0" ? "All Licensees" : lsName);
        $("#gridtitle").html(negativeBalance.title);
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/SelNegativeBalanceMember",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (rs) {
                negativeBalance.mailList = rs.MailList;
                if (rs.Items.length > 0) {
                    $(".btngroup").removeClass("hidden");
                    negativeBalance.data = rs;
                    negativeBalance.totalItem = parseInt(rs.totalitem);

                    var total = rs.Items.length;
                    var arr = [];
                    for (i = 0; i < total; i++) {
                        var item = rs.Items[i];
                        if (!negativeBalance.licensee && item) {
                            arr.push(negativeBalance.renderLicenseeRow(item.Total));
                        }

                        var totalitems = item.Items.length;

                        for (j = 0; j < totalitems; j++) {
                            arr.push(negativeBalance.renderLicenseeDetailRow(item.Items[j], item.Total.Name, (j % 2 == 0 ? "bgeven" : "")));
                        }
                    }
                    $("#tblnegativebalance").empty().html(arr.join(''));
                    //$("#tblFooter").empty().html(negativeBalance.renderGrandTotal(rs.Total));
                    negativeBalance.initPager(negativeBalance.totalItem, negativeBalance.pageSize, negativeBalance.pageIndex - 1);


                }
                else {
                    $(".btngroup").addClass("hidden");
                    $("#tblnegativebalance").empty().html('<tr><td><div class="c" style="width:446px;">There is no data.</div></td></tr>');
                    $("#tblFooter").empty();
                    $("#pagination").empty().html('&nbsp;');

                    $("#balanceSorter").unbind("click");
                }
            },
            complete: function () {
                $("#negativebalance").removeClass("hidden").reRenderScroller(449, 541, true);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    reRenderData: function () {
        if (negativeBalance.data != null) {
            var total = negativeBalance.data.Items.length;
            var arr = [];
            for (i = 0; i < total; i++) {
                var item = negativeBalance.data.Items[i];
                if (!negativeBalance.licensee) {
                    arr.push(negativeBalance.renderLicenseeRow(item.Total));
                }
                var totalitems = item.Items.length;
                for (j = 0; j < totalitems; j++) {
                    arr.push(negativeBalance.renderLicenseeDetailRow(item.Items[j], item.Total.Name, (j % 2 == 0 ? "bgeven" : "")));
                }
            }
            $("#tblnegativebalance").empty().html(arr.join(''));
            $("#negativebalance").removeClass("hidden").reRenderScroller(449, 541, true);
        }
    },
    sortData: function (DESC) {
        if (negativeBalance.data != null) {
            var total = negativeBalance.data.Items.length;
            for (i = 0; i < total; i++) {
                var item = negativeBalance.data.Items[i];
                item.Items.sort(hepler.sort_by('Balance', DESC, parseFloat));
            }
        }
    },
    toggleLicenseeDetail: function (obj, licenseeId) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#tblnegativebalance tr.lic_" + licenseeId).removeClass("hidden");
        }
        else {
            $("#tblnegativebalance tr.lic_" + licenseeId).addClass("hidden");
            $(obj).addClass("tree-expand");
        }
        $("#negativebalance").reRenderScroller(449, 541, true);
    },
    exportExcel: function () {
        var lsName = this.licensee.charAt(0).toLocaleUpperCase() + this.licensee.slice(1);
        var url = 'NegativeBalance.aspx?IsExport=1&reportTitle=' + this.title + '&site=' + lsName + '&pageIndex=' + this.pageIndex + '&pageSize=' + this.pageSize;
        $('#frm').attr('src', url);
    },
    sendMail: function () {
        //create virtual mail object
        $("#btnSendMail")[0].sendMailFormData = $("#btnSendMail").sendMailFormData ? $("#btnSendMail").sendMailFormData :
            {
                "mails": negativeBalance.mailList
                , "subject": mailTitle
                , "mailContent": mailContent
                , "reportTitle": negativeBalance.title
            }

        //assign button click on template
        $("#scriptSendMailTemplate").tmpl($("#btnSendMail")[0].sendMailFormData).appendTo($("#divSendMailContainer").empty().dialog({
            width: 610, position: "top", modal: true
                , buttons: { "Send":
                    function () {
                        parent.BlockUI();
                        $.ajax({
                            type: "Post",
                            url: "Service/LicenseeService.svc/SendMailNegativeBalanceMember",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify($("#btnSendMail")[0].sendMailFormData)
                            ,
                            success: function (rs) {
                                if (rs != null && rs.IsFail == true && rs.Licensees != null) {
                                    var arrErr = [];
                                    var lincensees = rs.Licensees;
                                    for (var i = 0; i < lincensees.length; i++) {
                                        arrErr.push(lincensees[i].ErrorMessage + "<br/>");
                                    }                                    
                                    showAlertMessage("ErrMsg", arrErr.join(''), 1);

                                }
                                else {
                                    $("#ErrMsg").html("");
                                }
                            },
                            complete: function () {
                                $("#divSendMailContainer").dialog("close");
                                parent.jQuery.unblockUI();
                            }
                        });
                    }
                }
                , title: negativeBalance.title

        }));
        //mapping mail oject to template
        $($("#btnSendMail")[0].sendMailFormData)
                               .linkBoth("subject", "#tbxSubject", "val")
                               .linkBoth("mailContent", "#tbxSendMailContent", "val");

        $("#divMailList").removeClass("hidden").reRenderScroller(549, 108, true);
    },
    bindCCMail: function (cc) {
        var mailList = '';
        if (cc && cc.length > 0) {
            var arr = cc.split(";");
            for (var i = 0; i < arr.length; i++) {
                mailList += (arr[i] + "<br/>");
            }
        }
        return mailList;
    }
}