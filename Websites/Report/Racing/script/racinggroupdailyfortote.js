var obj = {};
var _isHideIBCPTRacing = false; 


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
    renderFrozenRow: function (row, classCss) {
        var html = "";
        html = '<tr>\
                        <td >\
                            <div class="col100 c">' + row.WinlostDate + '</div>\
                        </td>\
                        <td >\
                            <div class="col100 c">' + row.Name + '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    renderScrollRow: function (row, classCss) {
        var html = '<tr class="' + classCss + '">\
                                <td><div class="col100 r">' + row.TurnoverAgency + '</div></td>\
                                <td><div class="col100 r">' + row.TurnoverIBC + '</div></td>\
                                <td><div class="col100 r">' + row.Turnover3rd + '</div></td>\
                                <td><div class="col100 r">' + row.TurnoverTotalTO + '</div></td>\
                                <td class="hideIBC"><div class="col100 r">' + row.IBCPT + '</div></td>\
                                <td><div class="col100 r">' + row.IBCNetComm + '</div></td>\
                                <td class="hideIBC"><div class="col100 r">' + row.IBCTotalIncome + '</div></td>\
                                <td class="hideIBC"><div class="col100 r">' + row.One2BetPT + '</div></td>\
                                <td class="hideIBC"><div class="col100 r">' + row.One2BetComm + '</div></td>\
                                <td class="hideIBC"><div class="col100 r">' + row.One2BetTotalIncome + '</div></td>\
                                <td class="hideIBC"><div class="col100 r">' + row.GroupTotalIncome + '</div></td>\
                            </tr>';
        return html;
    },
    getDataReport: function () {
        parent.BlockUI();
        racingGroup.fromDate = $("#fdate").val();
        racingGroup.toDate = $("#tdate").val();

        _isHideIBCPTRacing = $('#IsHideIBCPTRacing').val() == 'True';

        $.ajax({
            type: "Post",
            url: "Services/ServiceReportRacing.svc/GetRacingWinlossGroupForTote",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"fromdate":"' + racingGroup.fromDate + '","todate":"' + racingGroup.toDate + '"}',
            success: function (rs) {
                if (rs != null && rs.Items.length > 0) {
                    $("#err").html('');
                    racingGroup.arr = [];
                    racingGroup.arrfrozen = [];
                    $("#gridtitle").html(title + ' - From ' + racingGroup.fromDate + ' To ' + racingGroup.toDate);
                    var totalItem = rs.Items.length;

                    for (var i = 0; i < totalItem; i++) {
                        var item = rs.Items[i];
                        racingGroup.arrfrozen.push(racingGroup.renderFrozenRow(item, ''));
                        racingGroup.arr.push(racingGroup.renderScrollRow(item, ''));

                    }
                    $("#tblfrozen").empty().html(racingGroup.arrfrozen.join(''));
                    $("#tblscroll").empty().html(racingGroup.arr.join(''));

                    //Grand Total
                    var grandtotal = rs.Total;
                    if (rs.Total != null) {
                        $("#tblfooterfrozen").empty().html('<tr class="grandtotal"><td colspan="2"><div class="b c" style="width:203px;">' + grandtotal.Name + '</div></td></tr>');
                        $("#tblfooterscroll").empty().html(racingGroup.renderScrollRow(grandtotal, 'grandtotal'));
                    }

                    var racinggrid = $("#racinggrid");
                    var width = 721;
                    if (_isHideIBCPTRacing) {
                        width = 515;
                    }
                    racinggrid.removeClass("hidden").reRenderScroller(width, 563, false); //width:7*100+7*1+7*2|height:605px

                    if (_isHideIBCPTRacing) {
                        $('.hideIBC').addClass('hide');
                        $('.hideIBCExcel').addClass('hide');
                        $('.colspanIBC').attr('colspan', 1);
                    }

                    if (navigator.appName == 'Netscape') {

                        if ($('#racinggrid').height() <= 124) {
                            $('#racinggrid .header-scroll').css('width', $('#racinggrid .body-scroll').width());
                            $('#racinggrid .footer-scroll').css('width', $('#racinggrid .body-scroll').width());
                        }
                    }
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
        var url = 'RacingExport.aspx?type=export&reporttype=groupdailyfortote&fdate=' + this.fromDate + '&tdate=' + this.toDate;
        $('#frm').attr('src', url);
    },
    sendMail: function () {
        $("#btnSendMail")[0].sendMailFormData = $("#btnSendMail").sendMailFormData ? $("#btnSendMail").sendMailFormData :
            {
                "fromDate": this.fromDate
                , "toDate": this.toDate
                , "mails": racingGroup.mails
                , "Subject": "Accounting System - Racing Group Daily for Tote Report from " + this.fromDate + " to " + this.toDate
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
                            url: "Services/ServiceReportRacing.svc/SendMailWinLossRacingGroupForTote2",
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
                , title: "Racing Group Daily for Tote"
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
            data: '{ "horseId": "151" }',
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
