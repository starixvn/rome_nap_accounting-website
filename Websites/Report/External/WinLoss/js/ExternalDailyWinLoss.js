var superDailyWinLoss = {
    title: '',
    reportDate: '',
    getSuperWinLossReport: function (type) {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        superDailyWinLoss.getReportDate(type);

        var param = {
            winlostdate: superDailyWinLoss.reportDate
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyByCom",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (superWL) {
                superDailyWinLoss.title = 'Super Win/Loss Daily On ' + superDailyWinLoss.reportDate;
                $('#superWLGridTitle').html(superDailyWinLoss.title);
                var arr = [];
                var arrFrozen = [];
                if (superWL && superWL.Items && superWL.Items.length > 0) {
                    for (var i = 0; i < superWL.Items.length; i++) {
                        var item = superWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(superDailyWinLoss.renderWLRow(i + 1, item, css));
                        arrFrozen.push(superDailyWinLoss.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';

                    footer = superDailyWinLoss.renderFooter(superWL.Total, 'footer');
                    footerFrozen = superDailyWinLoss.renderFooterFrozen(superWL.Total, 'footer');
                    $('#tblSuperWLfrozen').empty().html(arrFrozen.join(''));
                    $('#tblSuperWL').empty().html(arr.join(''));
                    $('#tblfooterSuperWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterSuperWL').empty().html(footer);

                    $("#superWLGrid").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#superWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#superWLGrid .header-scroll').height() - 2);
                    else {
                        $('#superWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#superWLGrid .header-scroll').height());
                        if ($('#superWLGrid').height() <= 126) {
                            $('#superWLGrid .header-scroll').css('width', $('#superWLGrid .body-scroll').width());
                            $('#superWLGrid .footer-scroll').css('width', $('#superWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });

    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c blue" onclick="superDailyWinLoss.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + superDailyWinLoss.title + '&type=1&rdate=' + superDailyWinLoss.reportDate;
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        masterDailyWinLoss.reportDate = superDailyWinLoss.reportDate;
        masterDailyWinLoss.getMasterWinLossReport(uplineId);
    },
    getReportDate: function (type) {
        switch (type) {
            case 1:
                superDailyWinLoss.reportDate = $('#date').val();
                break;
            case 2:
                $('#date').val($('#btnCurrentDateReport').val());                
                superDailyWinLoss.reportDate = $('#btnCurrentDateReport').val();
                break;
            case 3:
                $('#date').val($('#btnCurrentYesterdayReport').val());
                superDailyWinLoss.reportDate = $('#btnCurrentYesterdayReport').val();
                break;
            default:
                superDailyWinLoss.reportDate = $('#date').val();
                break;
        }
    }
};

var masterDailyWinLoss = {
    title: '',
    reportDate: '',
    uplineId: 0,
    getMasterWinLossReport: function (upline) {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        masterDailyWinLoss.uplineId = upline;

        var param = {
            winlostdate: masterDailyWinLoss.reportDate,
            custid: upline
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyBySuper",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (masterWL) {
                masterDailyWinLoss.title = 'Master Win/Loss Daily On ' + masterDailyWinLoss.reportDate;
                $('#masterWLGridTitle').html(masterDailyWinLoss.title);
                var arr = [];
                var arrFrozen = [];
                if (masterWL && masterWL.Items && masterWL.Items.length > 0) {
                    for (var i = 0; i < masterWL.Items.length; i++) {
                        var item = masterWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(masterDailyWinLoss.renderWLRow(i + 1, item, css));
                        arrFrozen.push(masterDailyWinLoss.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = ''; var footerFrozen = '';
                    footer = masterDailyWinLoss.renderFooter(masterWL.Total, 'footer');
                    footerFrozen = masterDailyWinLoss.renderFooterFrozen(masterWL.Total, 'footer');
                    $('#tblMasterWLfrozen').empty().html(arrFrozen.join(''));
                    $('#tblMasterWL').empty().html(arr.join(''));
                    $('#tblfooterMasterWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterMasterWL').empty().html(footer);

                    $("#masterWLGrid").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#masterWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#masterWLGrid .header-scroll').height() - 2);
                    else {
                        $('#masterWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#masterWLGrid .header-scroll').height());
                        if ($('#masterWLGrid').height() <= 126) {
                            $('#masterWLGrid .header-scroll').css('width', $('#masterWLGrid .body-scroll').width());
                            $('#masterWLGrid .footer-scroll').css('width', $('#masterWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c blue" onclick="masterDailyWinLoss.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + masterDailyWinLoss.title + '&type=2&rdate=' + masterDailyWinLoss.reportDate + '&uplineId=' + masterDailyWinLoss.uplineId;
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        agentDailyWinLoss.reportDate = masterDailyWinLoss.reportDate;
        agentDailyWinLoss.getAgentWinLossReport(uplineId);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLGrid').removeClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
    }
};

var agentDailyWinLoss = {
    title: '',
    reportDate: '',
    uplineId: 0,
    getAgentWinLossReport: function (upline) {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        agentDailyWinLoss.uplineId = upline;
        
        var param = {
            winlostdate: agentDailyWinLoss.reportDate,
            custid: upline
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyByMaster",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (agentWL) {
                agentDailyWinLoss.title = 'Agent Win/Loss Daily On ' + agentDailyWinLoss.reportDate;
                $('#agentWLGridTitle').html(agentDailyWinLoss.title);
                var arr = []; var arrFrozen = [];
                if (agentWL && agentWL.Items && agentWL.Items.length > 0) {
                    for (var i = 0; i < agentWL.Items.length; i++) {
                        var item = agentWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(agentDailyWinLoss.renderWLRow(i + 1, item, css));
                        arrFrozen.push(agentDailyWinLoss.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = ''; var footerFrozen = '';
                    footer = agentDailyWinLoss.renderFooter(agentWL.Total, 'footer');
                    footerFrozen = agentDailyWinLoss.renderFooterFrozen(agentWL.Total, 'footer');
                    $('#tblAgentWLfrozen').empty().html(arrFrozen.join(''));
                    $('#tblAgentWL').empty().html(arr.join(''));
                    $('#tblfooterAgentWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterAgentWL').empty().html(footer);

                    $("#agentWLGrid").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#agentWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#agentWLGrid .header-scroll').height() - 2);
                    else {
                        $('#agentWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#agentWLGrid .header-scroll').height());
                        if ($('#agentWLGrid').height() <= 126) {
                            $('#agentWLGrid .header-scroll').css('width', $('#agentWLGrid .body-scroll').width());
                            $('#agentWLGrid .footer-scroll').css('width', $('#agentWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c blue" onclick="agentDailyWinLoss.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + agentDailyWinLoss.title + '&type=3&rdate=' + agentDailyWinLoss.reportDate + '&uplineId=' + agentDailyWinLoss.uplineId;
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        memberDailyWinLoss.reportDate = agentDailyWinLoss.reportDate;
        memberDailyWinLoss.getMemberWinLossReport(uplineId);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').removeClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
    }
};

var memberDailyWinLoss = {
    title: '',
    reportDate: '',
    uplineId: 0,
    getMemberWinLossReport: function (upline) {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').addClass('hidden');
        $('#memberWLGrid').addClass('hidden');
        memberDailyWinLoss.uplineId = upline;
        //
        var param = {
            winlostdate: agentDailyWinLoss.reportDate,
            custid: upline
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyByAgent",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (memberWL) {
                memberDailyWinLoss.title = 'Member Win/Loss Daily On ' + memberDailyWinLoss.reportDate;
                $('#memberWLGridTitle').html(memberDailyWinLoss.title);
                var arr = []; var arrFrozen = [];
                if (memberWL && memberWL.Items && memberWL.Items.length > 0) {
                    for (var i = 0; i < memberWL.Items.length; i++) {
                        var item = memberWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(memberDailyWinLoss.renderWLRow(i + 1, item, css));
                        arrFrozen.push(memberDailyWinLoss.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = ''; var footerFrozen = '';
                    footer = memberDailyWinLoss.renderFooter(memberWL.Total, 'footer');
                    footerFrozen = agentDailyWinLoss.renderFooterFrozen(memberWL.Total, 'footer');
                    $('#tblMemberWLfrozen').empty().html(arrFrozen.join(''));
                    $('#tblMemberWL').empty().html(arr.join(''));
                    $('#tblfooterMemberWLfrozen').empty().html(footerFrozen);
                    $('#tblfooterMemberWL').empty().html(footer);

                    $("#memberWLGrid").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#memberWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#memberWLGrid .header-scroll').height() - 2);
                    else {
                        $('#memberWLGrid .header-frozen .tbl-header-frozen td').css('height', $('#memberWLGrid .header-scroll').height());
                        if ($('#memberWLGrid').height() <= 126) {
                            $('#memberWLGrid .header-scroll').css('width', $('#memberWLGrid .body-scroll').width());
                            $('#memberWLGrid .footer-scroll').css('width', $('#memberWLGrid .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyMargin + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col60 r">' + row.SuperMargin + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col60 r">' + row.MasterMargin + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                    <td><div class="col60 r">' + row.AgentMargin + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + memberDailyWinLoss.title + '&type=4&rdate=' + memberDailyWinLoss.reportDate + '&uplineId=' + memberDailyWinLoss.uplineId;
        $("#frm").attr('src', url);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLGrid').addClass('hidden');
        $('#masterWLGrid').addClass('hidden');
        $('#agentWLGrid').removeClass('hidden');
        $('#memberWLGrid').addClass('hidden');
    }
};

var superDailyWinLossNotMargin = {
    title: '',
    reportDate: '',
    getSuperWinLossReport: function (type) {
        $('#err').html('');
        $('#superWLNotMargin').addClass('hidden');
        $('#masterWLNotMargin').addClass('hidden');
        $('#agentWLNotMargin').addClass('hidden');
        $('#memberWLNotMargin').addClass('hidden');
        superDailyWinLossNotMargin.getReportDate(type);

        var param = {
            winlostdate: superDailyWinLossNotMargin.reportDate
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyByCom",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (superWL) {
                superDailyWinLossNotMargin.title = 'Super Win/Loss Daily On ' + superDailyWinLossNotMargin.reportDate;
                $('#superWLNotMarginTitle').html(superDailyWinLossNotMargin.title);
                var arr = [];
                var arrFrozen = [];
                if (superWL && superWL.Items && superWL.Items.length > 0) {
                    for (var i = 0; i < superWL.Items.length; i++) {
                        var item = superWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(superDailyWinLossNotMargin.renderWLRow(i + 1, item, css));
                        arrFrozen.push(superDailyWinLossNotMargin.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = '';
                    var footerFrozen = '';

                    footer = superDailyWinLossNotMargin.renderFooter(superWL.Total, 'footer');
                    footerFrozen = superDailyWinLossNotMargin.renderFooterFrozen(superWL.Total, 'footer');
                    $('#tblSuperWLNotMarginFrozen').empty().html(arrFrozen.join(''));
                    $('#tblSuperWLNotMarginScroll').empty().html(arr.join(''));
                    $('#tblSuperWLNotMarginFrozenFooter').empty().html(footerFrozen);
                    $('#tblSuperWLNotMarginScrollFooter').empty().html(footer);

                    $("#superWLNotMargin").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#superWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#superWLNotMargin .header-scroll').height() - 2);
                    else {
                        $('#superWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#superWLNotMargin .header-scroll').height());
                        if ($('#superWLNotMargin').height() <= 126) {
                            $('#superWLNotMargin .header-scroll').css('width', $('#superWLNotMargin .body-scroll').width());
                            $('#superWLNotMargin .footer-scroll').css('width', $('#superWLNotMargin .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });

    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c blue" onclick="superDailyWinLossNotMargin.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + superDailyWinLossNotMargin.title + '&type=1&rdate=' + superDailyWinLossNotMargin.reportDate + '&isExcludeMargin=true';
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        masterDailyWinLossNotMargin.reportDate = superDailyWinLossNotMargin.reportDate;
        masterDailyWinLossNotMargin.getMasterWinLossReport(uplineId);
    },
    getReportDate: function (type) {
        switch (type) {
            case 1:
                superDailyWinLossNotMargin.reportDate = $('#date').val();
                break;
            case 2:
                $('#date').val($('#btnCurrentDateReport').val());
                superDailyWinLossNotMargin.reportDate = $('#btnCurrentDateReport').val();
                break;
            case 3:
                $('#date').val($('#btnCurrentYesterdayReport').val());
                superDailyWinLossNotMargin.reportDate = $('#btnCurrentYesterdayReport').val();
                break;
            default:
                superDailyWinLossNotMargin.reportDate = $('#date').val();
                break;
        }
    }
};

var masterDailyWinLossNotMargin = {
    title: '',
    reportDate: '',
    uplineId: 0,
    getMasterWinLossReport: function (upline) {
        $('#err').html('');
        $('#superWLNotMargin').addClass('hidden');
        $('#masterWLNotMargin').addClass('hidden');
        $('#agentWLNotMargin').addClass('hidden');
        $('#memberWLNotMargin').addClass('hidden');
        masterDailyWinLossNotMargin.uplineId = upline;

        var param = {
            winlostdate: masterDailyWinLossNotMargin.reportDate,
            custid: upline
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyBySuper",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (masterWL) {
                masterDailyWinLossNotMargin.title = 'Master Win/Loss Daily On ' + masterDailyWinLossNotMargin.reportDate;
                $('#masterWLNotMarginTitle').html(masterDailyWinLossNotMargin.title);
                var arr = [];
                var arrFrozen = [];
                if (masterWL && masterWL.Items && masterWL.Items.length > 0) {
                    for (var i = 0; i < masterWL.Items.length; i++) {
                        var item = masterWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(masterDailyWinLossNotMargin.renderWLRow(i + 1, item, css));
                        arrFrozen.push(masterDailyWinLossNotMargin.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = ''; var footerFrozen = '';
                    footer = masterDailyWinLossNotMargin.renderFooter(masterWL.Total, 'footer');
                    footerFrozen = masterDailyWinLossNotMargin.renderFooterFrozen(masterWL.Total, 'footer');
                    $('#tblMasterWLNotMarginFrozeen').empty().html(arrFrozen.join(''));
                    $('#tblMasterWLNotMarginScroll').empty().html(arr.join(''));
                    $('#tblMasterWLNotMarginFrozeenFooter').empty().html(footerFrozen);
                    $('#tblMasterWLNotMarginScrollFooter').empty().html(footer);

                    $("#masterWLNotMargin").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#masterWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#masterWLNotMargin .header-scroll').height() - 2);
                    else {
                        $('#masterWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#masterWLNotMargin .header-scroll').height());
                        if ($('#masterWLNotMargin').height() <= 126) {
                            $('#masterWLNotMargin .header-scroll').css('width', $('#masterWLNotMargin .body-scroll').width());
                            $('#masterWLNotMargin .footer-scroll').css('width', $('#masterWLNotMargin .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c blue" onclick="masterDailyWinLossNotMargin.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + masterDailyWinLossNotMargin.title + '&type=2&rdate=' + masterDailyWinLossNotMargin.reportDate + '&uplineId=' + masterDailyWinLossNotMargin.uplineId + '&isExcludeMargin=true';
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        agentDailyWinLossNotMargin.reportDate = masterDailyWinLossNotMargin.reportDate;
        agentDailyWinLossNotMargin.getAgentWinLossReport(uplineId);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLNotMargin').removeClass('hidden');
        $('#masterWLNotMargin').addClass('hidden');
        $('#agentWLNotMargin').addClass('hidden');
        $('#memberWLNotMargin').addClass('hidden');
    }
};

var agentDailyWinLossNotMargin = {
    title: '',
    reportDate: '',
    uplineId: 0,
    getAgentWinLossReport: function (upline) {
        $('#err').html('');
        $('#superWLNotMargin').addClass('hidden');
        $('#masterWLNotMargin').addClass('hidden');
        $('#agentWLNotMargin').addClass('hidden');
        $('#memberWLNotMargin').addClass('hidden');
        agentDailyWinLossNotMargin.uplineId = upline;

        var param = {
            winlostdate: agentDailyWinLossNotMargin.reportDate,
            custid: upline
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyByMaster",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (agentWL) {
                agentDailyWinLossNotMargin.title = 'Agent Win/Loss Daily On ' + agentDailyWinLossNotMargin.reportDate;
                $('#agentWLNotMarginTitle').html(agentDailyWinLoss.title);
                var arr = []; var arrFrozen = [];
                if (agentWL && agentWL.Items && agentWL.Items.length > 0) {
                    for (var i = 0; i < agentWL.Items.length; i++) {
                        var item = agentWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(agentDailyWinLossNotMargin.renderWLRow(i + 1, item, css));
                        arrFrozen.push(agentDailyWinLossNotMargin.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = ''; var footerFrozen = '';
                    footer = agentDailyWinLossNotMargin.renderFooter(agentWL.Total, 'footer');
                    footerFrozen = agentDailyWinLossNotMargin.renderFooterFrozen(agentWL.Total, 'footer');
                    $('#tblagentWLNotMarginFroozen').empty().html(arrFrozen.join(''));
                    $('#tblagentWLNotMarginScroll').empty().html(arr.join(''));
                    $('#tblagentWLNotMarginFroozenFooter').empty().html(footerFrozen);
                    $('#tblagentWLNotMarginScrollFooter').empty().html(footer);

                    $("#agentWLNotMargin").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#agentWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#agentWLNotMargin .header-scroll').height() - 2);
                    else {
                        $('#agentWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#agentWLNotMargin .header-scroll').height());
                        if ($('#agentWLNotMargin').height() <= 126) {
                            $('#agentWLNotMargin .header-scroll').css('width', $('#agentWLNotMargin .body-scroll').width());
                            $('#agentWLNotMargin .footer-scroll').css('width', $('#agentWLNotMargin .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c blue" onclick="agentDailyWinLossNotMargin.getDownlineReport(' + row.CustId + ')">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + agentDailyWinLoss.title + '&type=3&rdate=' + agentDailyWinLoss.reportDate + '&uplineId=' + agentDailyWinLoss.uplineId + '&isExcludeMargin=true';
        $("#frm").attr('src', url);
    },
    getDownlineReport: function (uplineId) {
        memberDailyWinLossNotMargin.reportDate = agentDailyWinLossNotMargin.reportDate;
        memberDailyWinLossNotMargin.getMemberWinLossReport(uplineId);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLNotMargin').addClass('hidden');
        $('#masterWLNotMargin').removeClass('hidden');
        $('#agentWLNotMargin').addClass('hidden');
        $('#memberWLNotMargin').addClass('hidden');
    }
};

var memberDailyWinLossNotMargin = {
    title: '',
    reportDate: '',
    uplineId: 0,
    getMemberWinLossReport: function (upline) {
        $('#err').html('');
        $('#superWLNotMargin').addClass('hidden');
        $('#masterWLNotMargin').addClass('hidden');
        $('#agentWLNotMargin').addClass('hidden');
        $('#memberWLNotMargin').addClass('hidden');
        memberDailyWinLossNotMargin.uplineId = upline;
        //
        var param = {
            winlostdate: memberDailyWinLossNotMargin.reportDate,
            custid: upline
        };
        try { BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/ExternalService.svc/SelWinLossDailyByAgent",
            data: JSON.stringify(param),
            contentType: "application/json; charset=utf-8",
            success: function (memberWL) {
                memberDailyWinLossNotMargin.title = 'Member Win/Loss Daily On ' + memberDailyWinLossNotMargin.reportDate;
                $('#memberWLNotMarginTitle').html(memberDailyWinLossNotMargin.title);
                var arr = []; var arrFrozen = [];
                if (memberWL && memberWL.Items && memberWL.Items.length > 0) {
                    for (var i = 0; i < memberWL.Items.length; i++) {
                        var item = memberWL.Items[i];
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(memberDailyWinLossNotMargin.renderWLRow(i + 1, item, css));
                        arrFrozen.push(memberDailyWinLossNotMargin.renderWLFrozenRow(i + 1, item, css));
                    }
                    var footer = ''; var footerFrozen = '';
                    footer = memberDailyWinLossNotMargin.renderFooter(memberWL.Total, 'footer');
                    footerFrozen = memberDailyWinLossNotMargin.renderFooterFrozen(memberWL.Total, 'footer');
                    $('#tblmemberWLNotMarginFrozen').empty().html(arrFrozen.join(''));
                    $('#tblmemberWLNotMarginScroll').empty().html(arr.join(''));
                    $('#tblmemberWLNotMarginFrozenFooter').empty().html(footerFrozen);
                    $('#tblmemberWLNotMarginScrollFooter').empty().html(footer);

                    $("#memberWLNotMargin").removeClass("hidden").reRenderScroller(664, 412, false);

                    if ($.browser.msie)
                        $('#memberWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#memberWLNotMargin .header-scroll').height() - 2);
                    else {
                        $('#memberWLNotMargin .header-frozen .tbl-header-frozen td').css('height', $('#memberWLNotMargin .header-scroll').height());
                        if ($('#memberWLNotMargin').height() <= 126) {
                            $('#memberWLNotMargin .header-scroll').css('width', $('#v .body-scroll').width());
                            $('#memberWLNotMargin .footer-scroll').css('width', $('#memberWLNotMargin .body-scroll').width());
                        }
                    }
                }
                else {
                    showAlertMessage("err", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderWLFrozenRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col20 r">' + index + '</div></td>\
                    <td><div class="col100 c">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderWLRow: function (index, row, css) {
        var html = '';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    renderFooterFrozen: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:20px;"></td>\
                    <td  style="height:0px; border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td colspan="2"><div class="r" style="width:103px;">' + row.UserName + '</div></td>\
                    <td><div class="col100 r">' + row.TotalTO + '</div></td>\
                </tr>';
        return html;
    },
    renderFooter: function (row, css) {
        var html = '';
        html += '<tr  class="' + css + '" >\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:60px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                    <td  style="height:0px;border-bottom:none; width:100px;"></td>\
                </tr>';
        html += '<tr class="' + css + '">\
                    <td><div class="col100 r">' + row.CompanyTO + '</div></td>\
                    <td><div class="col60 r">' + row.CompanyPT + '</div></td>\
                    <td><div class="col100 r">' + row.SuperTO + '</div></td>\
                    <td><div class="col60 r">' + row.SuperPT + '</div></td>\
                    <td><div class="col100 r">' + row.MasterTO + '</div></td>\
                    <td><div class="col60 r">' + row.MasterPT + '</div></td>\
                    <td><div class="col100 r">' + row.AgentTO + '</div></td>\
                    <td><div class="col60 r">' + row.AgentPT + '</div></td>\
                    <td><div class="col100 r">' + row.CompanyWL + '</div></td>\
                    <td><div class="col100 r">' + row.SuperWL + '</div></td>\
                    <td><div class="col100 r">' + row.MasterWL + '</div></td>\
                    <td><div class="col100 r">' + row.AgentWL + '</div></td>\
                </tr>';
        return html;
    },
    exportExcel: function () {
        var url = 'DailyWinLossNew.aspx?isexport=true&rtitle=' + memberDailyWinLossNotMargin.title + '&type=4&rdate=' + memberDailyWinLossNotMargin.reportDate + '&uplineId=' + memberDailyWinLossNotMargin.uplineId + '&isExcludeMargin=true';
        $("#frm").attr('src', url);
    },
    goback: function () {
        $('#err').html('');
        $('#superWLNotMargin').addClass('hidden');
        $('#masterWLNotMargin').addClass('hidden');
        $('#agentWLNotMargin').removeClass('hidden');
        $('#memberWLNotMargin').addClass('hidden');
    }
};

function getExternalDailyWL(type) {

    $('#superWLGrid').addClass('hidden');
    $('#masterWLGrid').addClass('hidden');
    $('#agentWLGrid').addClass('hidden');
    $('#memberWLGrid').addClass('hidden');

    $('#superWLNotMargin').addClass('hidden');
    $('#masterWLNotMargin').addClass('hidden');
    $('#agentWLNotMargin').addClass('hidden');
    $('#memberWLNotMargin').addClass('hidden');
    
    if ($('#isExcludeMargin').attr('checked')) {
        superDailyWinLoss.getSuperWinLossReport(type);
    }
    else {
        superDailyWinLossNotMargin.getSuperWinLossReport(type);    
    }

}

$(document).ready(function () {
    $('#superWLGrid').scroller();
    $('#masterWLGrid').scroller();
    $('#agentWLGrid').scroller();
    $('#memberWLGrid').scroller();

    $('#superWLNotMargin').scroller();
    $('#masterWLNotMargin').scroller();
    $('#agentWLNotMargin').scroller();
    $('#memberWLNotMargin').scroller();
});