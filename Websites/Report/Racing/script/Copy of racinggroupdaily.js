var obj = {};
$(function () {
    var racinggrid = $("#racinggrid");
    racinggrid.scroller(); //width:7*100+7*1+7*2|height:605px
});


var racingGroup = {
    arr: [],
    arrfrozen: [],
    fromDate: '',
    toDate: '',
    mails: [],
    renderFrozenRow: function (total, index, row, classCss) {
        var html = "";
        if (index == 0) {
            html = '<tr class="' + classCss + '">\
                            <td rowspan="' + total + '">\
                                <div class="col100 c">' + row.WinlostDate + '</div>\
                            </td>\
                            <td >\
                                <div class="col100 c">' + row.Name + '</div>\
                            </td>\
                        </tr>';
        }
        else {
            html = '<tr>\
                            <td >\
                                <div class="col100 c">' + row.Name + '</div>\
                            </td>\
                        </tr>';
        }
        return html;
    },
    renderScrollRow: function (row, classCss) {
        var html = '<tr class="' + classCss + '">\
                                <td><div class="col100 r">' + row.TurnoverAgency + '</div></td>\
                                <td><div class="col100 r">' + row.TurnoverIBC + '</div></td>\
                                <td><div class="col100 r">' + row.Turnover3rd + '</div></td>\
                                <td><div class="col100 r">' + row.TurnoverTotalTO + '</div></td>\
                                <td><div class="col100 r">' + row.IBCPT + '</div></td>\
                                <td><div class="col100 r">' + row.IBCNetComm + '</div></td>\
                                <td><div class="col100 r">' + row.IBCTotalIncome + '</div></td>\
                                <td><div class="col100 r">' + row.One2BetPT + '</div></td>\
                                <td><div class="col100 r">' + row.One2BetComm + '</div></td>\
                                <td><div class="col100 r">' + row.One2BetTotalIncome + '</div></td>\
                                <td><div class="col100 r">' + row.GroupTotalIncome + '</div></td>\
                                <td><div class="col100 r">' + row.PartnerTurnover + '</div></td>\
                                <td><div class="col100 r">' + row.PartnerDividendWon + '</div></td>\
                                <td><div class="col100 r">' + row.PartnerCommExpense + '</div></td>\
                                <td><div class="col100 r">' + row.PartnerFees + '</div></td>\
                                <td><div class="col100 r">' + row.PartnerAmountDueto + '</div></td>\
                            </tr>';
        return html;
    },
    getDataReport: function () {
        parent.BlockUI();
        racingGroup.fromDate = $("#fdate").val();
        racingGroup.toDate = $("#tdate").val();
        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/GetRacingWinlossGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"fromdate":"' + racingGroup.fromDate + '","todate":"' + racingGroup.toDate + '"}',
            success: function (rs) {
                if (rs != null && rs.Items.length > 0) {
                    racingGroup.chart.setData(rs);
                    $("#err").html('');
                    racingGroup.arr = [];
                    racingGroup.arrfrozen = [];
                    $("#gridtitle").html(title + ' - From ' + racingGroup.fromDate + ' To ' + racingGroup.toDate);
                    var totalItem = rs.Items.length;

                    for (var i = 0; i < totalItem; i++) {
                        var item = rs.Items[i];
                        var totalItem1 = item.Items.length;

                        for (var j = 0; j < totalItem1; j++) {
                            var item1 = item.Items[j];
                            racingGroup.arrfrozen.push(racingGroup.renderFrozenRow(totalItem1, j, item1, ''));
                            racingGroup.arr.push(racingGroup.renderScrollRow(item1, ''));
                        }
                        //Total here
                        racingGroup.arrfrozen.push('<tr><td colspan="2" ><div class="b c">Total</div></td></tr>');
                        racingGroup.arr.push(racingGroup.renderScrollRow(item.Total, 'b'));

                    }
                    $("#tblfrozen").empty().html(racingGroup.arrfrozen.join(''));
                    $("#tblscroll").empty().html(racingGroup.arr.join(''));

                    //Grand Total
                    var grandtotal = rs.Total.length;
                    if (grandtotal > 0) {
                        racingGroup.arr = [];
                        racingGroup.arrfrozen = [];

                        for (var n = 0; n < grandtotal; n++) {
                            var granditem = rs.Total[n];
                            if (n == (grandtotal - 1)) {
                                racingGroup.arrfrozen.push('<tr class="grandtotal"><td colspan="2"><div class="b c" style="width:203px;">Grand Total</div></td></tr>');
                                racingGroup.arr.push(racingGroup.renderScrollRow(granditem, 'grandtotal'));
                            }
                            else {
                                racingGroup.arrfrozen.push('<tr class="grandtotalpartner"><td colspan="2"><div class="b c" style="width:203px;">' + granditem.Name + '</div></td></tr>');
                                racingGroup.arr.push(racingGroup.renderScrollRow(granditem, 'grandtotalpartner'));
                            }
                        }

                        $("#tblfooterfrozen").empty().html(racingGroup.arrfrozen.join(''));
                        $("#tblfooterscroll").empty().html(racingGroup.arr.join(''));
                    }

                    var racinggrid = $("#racinggrid");
                    racinggrid.removeClass("hidden").reRenderScroller(721, 563, false); //width:7*100+7*1+7*2|height:605px


                }
                else {
                    $("#racinggrid").addClass("hidden");
                    $("#err").html("There is no data");
                }
            },
            complete: function () {

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    exportExcel: function () {
        var url = 'RacingExport.aspx?type=export&reporttype=groupdaily&fdate=' + this.fromDate + '&tdate=' + this.toDate;
        $('#frm').attr('src', url);
    },
    sendMail: function () {
        $("#btnSendMail")[0].sendMailFormData = $("#btnSendMail").sendMailFormData ? $("#btnSendMail").sendMailFormData :
            {
                "fromDate": this.fromDate
                , "toDate": this.toDate
                , "mails": racingGroup.mails
                , "Subject": "Accounting System - Racing Group Daily Report from " + this.fromDate + " to " + this.toDate
                , "Content": mailContent
            };

        $("#scriptSendMailTemplate").tmpl($("#btnSendMail")[0].sendMailFormData).appendTo($("#divSendMailContainer").empty().dialog(
            {
                width: 610, position: "top", modal: true
                , buttons: { "Send":
                    function () {
                        parent.BlockUI();
                        $.ajax({
                            type: "Post",
                            url: "Services/ServiceReportRacing.svc/SendMailWinLossRacingGroup2",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify($("#btnSendMail")[0].sendMailFormData),
                            success: function (rs) {
                                if (rs != null && rs.IsFail && rs.Recivers != null) {
                                    var arrErr = [];
                                    var recivers = rs.Recivers;
                                    for (var i = 0; i < recivers.length; i++) {
                                        arrErr.push(recivers[i].ErrorMessage + "<br/>");
                                    }
                                    showAlertMessage("err", arrErr.join(''), 1);
                                }
                                else {
                                    $("#err").html("");
                                }
                            },
                            complete: function () {
                                $("#divSendMailContainer").dialog("close");
                                parent.jQuery.unblockUI();
                            }
                        });
                    }
                }
                , title: "Racing Group Daily"
            }));
        $($("#btnSendMail")[0].sendMailFormData)
                               .linkBoth("Subject", "#tbxSubject", "val")
                               .linkBoth("Content", "#tbxSendMailContent", "html");
    },
    getMails: function () {
        parent.BlockUI();
        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/GetDefaultToEmailAddress",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"horseId": "0" }',
            success: function (rs) {
                if (rs != null && rs.Recivers != null) {
                    racingGroup.mails = rs.Recivers;
                    racingGroup.sendMail();
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    }
};
$.extend(racingGroup, { chart: {
    data: null,
    categories: [
                { field: 'TurnoverAgency', label: 'Agency Turnover', enable: true },
                { field: 'TurnoverIBC', label: 'IBC Turnover', enable: true },
                { field: 'Turnover3rd', label: 'Turnover Partner', enable: true },
                { field: 'TurnoverTotalTO', label: 'Total Turnover', enable: true },
                { field: 'IBCPT', label: 'IBC PT', enable: true },
                { field: 'IBCNetComm', label: 'IBC Net Comm', enable: true },
                { field: 'IBCTotalIncome', label: 'IBC Total Comm', enable: true },
                { field: 'One2BetPT', label: '12Bet PT', enable: true },
                { field: 'One2BetComm', label: '12Bet Comm', enable: true },
                { field: 'One2BetTotalIncome', label: '12Bet Total Income', enable: true },
                { field: 'GroupTotalIncome', label: 'Group Total Income', enable: true },
                { field: 'PartnerDividendWon', label: 'Partner Dividend Won', enable: true },
                { field: 'PartnerCommExpense', label: 'Partner Comm Expense', enable: true },
                { field: 'PartnerFees', label: 'Partner Fee', enable: true },
                { field: 'PartnerAmountDueto', label: 'Partner Amount Due', enable: true }
        ],
    setData: function (_data) {
        this.data = _data;
    },
    viewByTotal: function () {
        if (this.data != null && this.data.Total) {
            var data_items = this.data.Total;
            var serial_data = []; var categories = [];
            for (var i = 0, len = data_items.length; i < len - 1; i++) {
                var item = data_items[i];
                var toData = [];
                $.each(this.categories, function (ind, cate) {
                    if (cate.enable) {
                        var val = eval("item." + cate.field + "");
                        toData.push(racingGroup.chart.toNumber(val));
                        if (i == 0) { categories.push(cate.label); }
                    }
                });
                serial_data.push({
                    name: item.Name,
                    data: toData,
                    cursor: 'pointer'
                });
            }
            // init chart total
            var opts = {
                container: 'chart_content',
                type: 'column',
                title: $('#gridtitle').html(),
                width: 1000,
                height: 550,
                categories: categories,
                series: serial_data
            };
            if ($('#chart_content').length == 0) {
                this.initViewChart(opts);
            }
            accChart.renderSVG(opts);
        }
    },
    initViewChart: function (opts) {
        $('<div />').attr('id', 'chart_shadow').addClass('chart_shadow').appendTo('body').show();
        $('<div />').attr('id', 'chart_popup').addClass('chart_popup')
            .css({ width: 1000 })
            .appendTo('body');
        $('<div />').attr('id', 'chart_setting').addClass('chart_setting').appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_title')
             .addClass('chart_title')
             .append('<div class="setting" title="Setting">&nbsp;</div><div class="title">' + opts.title + '</div><div class="close" title="Close">&nbsp;</div>')
             .appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_content').addClass('chart_content')
             .css({ width: opts.width, height: opts.height })
             .appendTo('#chart_popup');
        $('#chart_popup').css("left", 100);
        $('#chart_popup').show();
        $('#chart_popup').draggable({
            handle: '#chart_title'
        });
        $('div.setting').bind('click', function (e) {
            if ($('#chart_setting').is(':hidden')) {
                $('#chart_setting').slideDown();
                $(this).attr('title', 'Hide setting');
            } else {
                $('#chart_setting').slideUp();
                $(this).attr('title', 'Display setting');
            }
        });
        $('div.close').bind('click', function (e) {
            racingGroup.chart.close();
        });
        this.renderSetting();
    },
    renderSetting: function () {
        $('#chart_setting').html('<ul id="ul_setting"></ul>');
        var tpl_cate = '{{each categories}}<li><input class="setcheck" type="checkbox" {{if $value.enable}}checked="${$value.enable}"{{/if}} value="${$index}"/>${$value.enable}&nbsp;${$value.label}</li>{{/each}}';
        //$('<div />').append(tpl_cate).tmpl(this).appendTo("#ul_setting");
        $.each(this.categories, function (ind, cate) {
            var is_checked = (cate.enable == true) ? 'checked' : '';
            var _itemTpl = '<li><input class="setcheck" ' + is_checked + ' type="checkbox" value="' + ind + '"/>&nbsp;' + cate.label + '</li>';
            $("#ul_setting").append(_itemTpl);
        });
        $('#chart_setting').css({ width: 185, 'margin-top': 27 });
        if ($.browser.msie) {
            if ($.browser.version.indexOf('7') != -1) {
                $('#chart_setting').css({ 'margin-left': -1000 });
                $('#ul_setting').css({ 'margin-left': 10 });
            }
        }
        window.setTimeout(function () {
            $('input.setcheck').bind('click', function (e) {
                var checked = $(this).attr("checked");
                var index = $(this).val();
                racingGroup.chart.categories[index].enable = checked;
                racingGroup.chart.viewByTotal();
            });
        });
    },
    close: function () {
        $('#chart_shadow').fadeOut();
        $('#chart_popup').fadeOut();
        window.setTimeout(function () {
            $('#chart_shadow').remove();
            $('#chart_popup').remove();
        }, 1000);
    },
    toNumber: function (numText) {
        numText = "".concat(numText);
        return parseFloat(numText.replace(/,/gi, ''));
    }
}
});
