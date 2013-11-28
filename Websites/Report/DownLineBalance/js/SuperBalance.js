$(document).ready(function () {
    superBalance.getDifferenceBalance();
    superBalance.getCurrencies();
    
});

var superBalance = {
    isYesterDay: true,
    db: 'rep',
    keyword: '',
    firstLetter: 'all',
    currency: 'all',
    isViewTotalOnly: 0,
    title: '',
    onBlur: function () {
        if (obj('txtSearch').value.length == 0)
            obj('txtSearch').value = 'All';
    },
    onFocus: function () {
        if (obj('txtSearch').value == 'All')
            obj('txtSearch').value = '';
    },
    onKeyPress: function (e) {
        if (!e) e = window.event;
        var code = (e.keyCode) ? e.keyCode : e.which;
        if (code == 13) superBalance.getSuperBalanceReport();
    },
    getCurrencies: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/DownLineBalanceService.svc/GetCurrenciesBySystem",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                var currenciesList = $('#listCurrency');
                $(currenciesList).empty();

                $(currenciesList).append('<option value="0">All</option>');
                if (rs) {
                    $.each(rs.Currencies, function (ind, val) {
                        $(currenciesList).append('<option value="' + val.Id + '">' + val.Value + '</option>');
                    });
                }

            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    getSuperBalanceReport: function () {
        try { parent.BlockUI(); } catch (e) { }
        $('#mainHeader').html('Super Balance');
        $('#superBalanceGrid').addClass('hidden');
        $('#masterBalanceGrid').addClass('hidden');
        $('#agentBalanceGrid').addClass('hidden');
        $('#memberBalanceGrid').addClass('hidden');
        $('#errMsg').html('');

        superBalance.keyword = $('#txtSearch').val();
        superBalance.firstLetter = $('#listAlpha').val();
        superBalance.currency = $('#listCurrency').val();
        superBalance.isViewTotalOnly = $('#cbxviewTotal').attr("checked");

        var data = {
            db: superBalance.db,
            isYesterday: superBalance.isYesterDay,
            usernameSearch: superBalance.keyword,
            firstletter: superBalance.firstLetter,
            currency: superBalance.currency,
            isViewTotalOnly: superBalance.isViewTotalOnly
        };
        if (superBalance.isViewTotalOnly) {

            $.ajax({
                type: "POST",
                url: "../../SuperBalanceReport/GetSuperBalanceTotal",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (parseFloat(rs) != 0) {
                        superBalance.title = 'Super ' + (superBalance.isYesterDay ? 'Yesterday Balance' : 'Total Balance');
                        $('#superBalanceTitle').html(superBalance.title);
                        $('#tbFooterSuperBalance').empty().html(superBalance.renderFooterRowTotal(rs));
                        $('#body-scroll').addClass("hidden");
                        $('#header-scroll').addClass("hidden");
                        $("#superBalanceGrid").removeClass("hidden").reRenderScroller(595, 45, true);
                    }
                    else {
                        showAlertMessage("errMsg", "There is no data.", 1);
                    }
                },
                error: function () { },
                complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
            });
        }
        else {
            $.ajax({
                type: "POST",
                url: "Service/DownLineBalanceService.svc/GetSuperBalance",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (rs && rs.DownLineBalane && rs.DownLineBalane.length > 0) {
                        superBalance.title = 'Super ' + (superBalance.isYesterDay ? 'Yesterday Balance' : 'Total Balance');
                        $('#superBalanceTitle').html(superBalance.title);
                        var arr = [];
                        for (var i = 0; i < rs.DownLineBalane.length; i++) {
                            var css = i % 2 == 0 ? "bgeven" : "";
                            arr.push(superBalance.renderBalanceRow(rs.DownLineBalane[i], css, i + 1));
                        }
                        $('#tbSuperBalance').empty().html(arr.join(''));
                        $('#tbFooterSuperBalance').empty().html(superBalance.renderFooterRow(rs.BaseTotal));
                        $('#body-scroll').removeClass("hidden");
                        $('#header-scroll').removeClass("hidden");
                        $("#superBalanceGrid").removeClass("hidden").reRenderScroller(595, 510, true);


                    }
                    else {
                        showAlertMessage("errMsg", "There is no data.", 1);
                    }
                },
                error: function () { },
                complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
            });
        }
    },
    renderBalanceRow: function (row, css, index) {
        var html = '<tr class="' + css + '">';
        html += '<td><div class="col40 c ">' + index + '</div></td>';
        html += '<td><div class="col200 c"><a href="#" onclick="masterBalance.getMasterBalanceReport(' + row.CustId + ',' + row.RoleId + ',' + superBalance.isYesterDay + ',\'' + superBalance.firstLetter + '\',\'' + superBalance.keyword + '\',\'' + row.CurrencyId + '\');">' + row.Username + '</a></div></td>';
        html += '<td><div class="col60 c">' + row.Currency + '</div></td>';
        html += '<td><div class="col140 r">' + row.Balance + '</div></td>';
        html += '<td><div class="col140 r">' + row.BaseBalance + '</div></td>';
        html += '</tr>';
        return html;
    },
    renderFooterRow: function (row) {
        var html = '<tr class="footer">';
        html += '<td colspan="4"><div class="col20 c" style="width: 449px;">TOTAL</div></td>';
        html += '<td colspan="4"><div class="col140 r ">' + row.BaseBalance + '</div></td>';
        html += '</tr>';

        return html;
    },
    renderFooterRowTotal: function (row) {
        var html = '<tr class="footer">';
        html += '<td colspan="4"><div class="col20 c" style="width: 449px;">TOTAL</div></td>';
        html += '<td colspan="4"><div class="col140 r ">' + row + '</div></td>';
        html += '</tr>';

        return html;
    },
    getDifferenceBalance: function () {
        $('#diffbalinfo').addClass('hidden');
        $.ajax({
            type: "POST",
            url: "Service/DownLineBalanceService.svc/GetDifferenceBalance",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs && rs.DiffBalance && rs.DiffBalance != 0) {
                    var html = ('The balance difference due to Ex.Rates updated for this week (' + rs.DiffDate + ') is: <b><font  color="' + (parseInt(rs.DiffBalance) > 0 ? 'blue' : 'red') + '">' + rs.DiffBalance + '</font> RM</b>&nbsp; <a target="_blank" href="CompBalanceDiff.aspx">View Detail</a>');
                    $('#diffbalinfo').html(html);
                    $('#diffbalinfo').removeClass('hidden');

                }
            },
            error: function () { },
            complete: function () { }
        });
    },
    exportExcel: function () {
        var url = 'SuperBalanceNew.aspx?isexport=true&rtitle=' + superBalance.title +
        '&roleid=4&custid=0&isyesterday=' + superBalance.isYesterDay + '&firstletter=' + superBalance.firstLetter +
        '&username=' + superBalance.keyword + '&currency=' + superBalance.currency + '&isViewTotalOnly=' + superBalance.isViewTotalOnly + '&db=' + superBalance.db; 
        $("#frm").attr('src', url);
    }
};

var masterBalance = {
    custId: 0,
    roleId: 3,
    isYesterday: true,
    keyword: '',
    firstLetter: '',
    currency: '',
    db: 'rep',
    title: '',
    getMasterBalanceReport: function (custId, roleId, isYesterday, firstLetter, username, currency) {
        $('#superBalanceGrid').addClass('hidden');
        $('#masterBalanceGrid').addClass('hidden');
        $('#agentBalanceGrid').addClass('hidden');
        $('#memberBalanceGrid').addClass('hidden');
        $('#errMsg').html('');
        $('#mainHeader').html('Master Balance');
        $('#diffbalinfo').addClass('hidden');

        masterBalance.custId = custId;
        masterBalance.roleId = roleId;
        masterBalance.isYesterday = isYesterday;
        masterBalance.keyword = username;
        masterBalance.firstLetter = firstLetter;
        masterBalance.currency = currency;

        try { parent.BlockUI(); } catch (e) { }

        var data = {
            db: masterBalance.db,
            roleid: masterBalance.roleId,
            custid: masterBalance.custId,
            isYesterday: masterBalance.isYesterday,
            usernameSearch: masterBalance.keyword,
            firstletter: masterBalance.firstLetter,
            currency: masterBalance.currency
        };

        $.ajax({
            type: "POST",
            url: "Service/DownLineBalanceService.svc/GetMasterBalance",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs && rs.DownLineBalane && rs.DownLineBalane.length > 0) {
                    masterBalance.title = 'Master ' + (masterBalance.isYesterDay ? 'Yesterday Balance' : 'Total Balance');
                    $('#masterBalanceTitle').html(masterBalance.title);
                    var arr = [];
                    for (var i = 0; i < rs.DownLineBalane.length; i++) {
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(masterBalance.renderBalanceRow(rs.DownLineBalane[i], css, i + 1));
                    }
                    $('#tbMasterBalance').empty().html(arr.join(''));
                    $('#tbFooterMasterBalance').empty().html(masterBalance.renderFooterRow(rs.BaseTotal));
                    $("#masterBalanceGrid").removeClass("hidden").reRenderScroller(595, 510, true);
                }
                else {
                    showAlertMessage("errMsg", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderBalanceRow: function (row, css, index) {
        var html = '<tr class="' + css + '">';
        html += '<td><div class="col40 c ">' + index + '</div></td>';
        html += '<td><div class="col200 c" ><a href="#" onclick="agentBalance.getAgentBalanceReport(' + row.CustId + ',' + row.RoleId + ',' + masterBalance.isYesterday + ',\'' + masterBalance.firstLetter + '\',\'' + masterBalance.keyword + '\',\'' + row.CurrencyId + '\');">' + row.Username + '</a></div></td>';
        html += '<td><div class="col60 c">' + row.Currency + '</div></td>';
        html += '<td><div class="col140 r">' + row.Balance + '</div></td>';
        html += '<td><div class="col140 r">' + row.BaseBalance + '</div></td>';
        html += '</tr>';
        return html;
    },
    renderFooterRow: function (row) {
        var html = '<tr class="footer">';
        html += '<td colspan="4"><div class="col20 c" style="width: 449px;">TOTAL</div></td>';
        html += '<td colspan="4"><div class="col140 r ">' + row.BaseBalance + '</div></td>';
        html += '</tr>';

        return html;
    },
    goBack: function () {
        $('#superBalanceGrid').removeClass('hidden');
        $('#masterBalanceGrid').addClass('hidden');
        $('#agentBalanceGrid').addClass('hidden');
        $('#memberBalanceGrid').addClass('hidden');
        $('#errMsg').html('');
        $('#mainHeader').html('Super Balance');
    },
    exportExcel: function () {
        var url = 'SuperBalanceNew.aspx?isexport=true&rtitle=' + masterBalance.title +
        '&roleid=3&custid=' + masterBalance.custId + '&isyesterday=' + masterBalance.isYesterDay + '&firstletter=' + masterBalance.firstLetter +
        '&username=' + masterBalance.keyword + '&currency=' + masterBalance.currency;
        $("#frm").attr('src', url);
    }
};

var agentBalance = {
    custId: 0,
    roleId: 2,
    isYesterday: true,
    keyword: '',
    firstLetter: '',
    currency: '',
    db: 'rep',
    title: '',
    getAgentBalanceReport: function (custId, roleId, isYesterday, firstLetter, username, currency) {
        $('#superBalanceGrid').addClass('hidden');
        $('#masterBalanceGrid').addClass('hidden');
        $('#agentBalanceGrid').addClass('hidden');
        $('#memberBalanceGrid').addClass('hidden');
        $('#errMsg').html('');
        $('#mainHeader').html('Agent Balance');
        $('#diffbalinfo').addClass('hidden');

        agentBalance.custId = custId;
        agentBalance.roleId = roleId;
        agentBalance.isYesterday = isYesterday;
        agentBalance.keyword = username;
        agentBalance.firstLetter = firstLetter;
        agentBalance.currency = currency;

        try { parent.BlockUI(); } catch (e) { }

        var data = {
            db: agentBalance.db,
            roleid: agentBalance.roleId,
            custid: agentBalance.custId,
            isYesterday: agentBalance.isYesterday,
            usernameSearch: agentBalance.keyword,
            firstletter: agentBalance.firstLetter,
            currency: agentBalance.currency
        };

        $.ajax({
            type: "POST",
            url: "Service/DownLineBalanceService.svc/GetAgentBalance",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs && rs.DownLineBalane && rs.DownLineBalane.length > 0) {
                    agentBalance.title = 'Agent ' + (agentBalance.isYesterday ? 'Yesterday Balance' : 'Total Balance');
                    $('#agentBalanceTitle').html(agentBalance.title);
                    var arr = [];
                    for (var i = 0; i < rs.DownLineBalane.length; i++) {
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(agentBalance.renderBalanceRow(rs.DownLineBalane[i], css, i + 1));
                    }
                    $('#tbAgentBalance').empty().html(arr.join(''));
                    $('#tbFooterAgentBalance').empty().html(agentBalance.renderFooterRow(rs.BaseTotal));
                    $("#agentBalanceGrid").removeClass("hidden").reRenderScroller(595, 510, true);
                }
                else {
                    showAlertMessage("errMsg", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderBalanceRow: function (row, css, index) {
        var html = '<tr class="' + css + '">';
        html += '<td><div class="col40 c ">' + index + '</div></td>';
        html += '<td><div class="col200 c" ><a href="#" onclick="memberBalance.getMemberBalanceReport(' + row.CustId + ',' + row.RoleId + ',' + agentBalance.isYesterday + ',\'' + agentBalance.firstLetter + '\',\'' + agentBalance.keyword + '\',\'' + row.CurrencyId + '\');">' + row.Username + '</a></div></td>';
        html += '<td><div class="col60 c">' + row.Currency + '</div></td>';
        html += '<td><div class="col140 r">' + row.Balance + '</div></td>';
        html += '<td><div class="col140 r">' + row.BaseBalance + '</div></td>';
        html += '</tr>';
        return html;
    },
    renderFooterRow: function (row) {
        var html = '<tr class="footer">';
        html += '<td colspan="4"><div class="col20 c" style="width: 449px;">TOTAL</div></td>';
        html += '<td colspan="4"><div class="col140 r ">' + row.BaseBalance + '</div></td>';
        html += '</tr>';

        return html;
    },
    goBack: function () {
        $('#superBalanceGrid').addClass('hidden');
        $('#masterBalanceGrid').removeClass('hidden');
        $('#agentBalanceGrid').addClass('hidden');
        $('#memberBalanceGrid').addClass('hidden');
        $('#errMsg').html('');
        $('#mainHeader').html('Agent Balance');
    },
    exportExcel: function () {
        var url = 'SuperBalanceNew.aspx?isexport=true&rtitle=' + agentBalance.title +
        '&roleid=2&custid=' + agentBalance.custId + '&isyesterday=' + agentBalance.isYesterDay + '&firstletter=' + agentBalance.firstLetter +
        '&username=' + agentBalance.keyword + '&currency=' + agentBalance.currency;
        $("#frm").attr('src', url);
    }
};

var memberBalance = {
    custId: 0,
    roleId: 1,
    isYesterday: true,
    keyword: '',
    firstLetter: '',
    currency: '',
    db: 'rep',
    title: '',
    getMemberBalanceReport: function (custId, roleId, isYesterday, firstLetter, username, currency) {
        $('#superBalanceGrid').addClass('hidden');
        $('#masterBalanceGrid').addClass('hidden');
        $('#agentBalanceGrid').addClass('hidden');
        $('#memberBalanceGrid').addClass('hidden');
        $('#errMsg').html('');
        $('#mainHeader').html('Member Balance');
        $('#diffbalinfo').addClass('hidden');

        memberBalance.custId = custId;
        memberBalance.roleId = roleId;
        memberBalance.isYesterday = isYesterday;
        memberBalance.keyword = username;
        memberBalance.firstLetter = firstLetter;
        memberBalance.currency = currency;

        try { parent.BlockUI(); } catch (e) { }

        var data = {
            db: memberBalance.db,
            roleid: memberBalance.roleId,
            custid: memberBalance.custId,
            isYesterday: memberBalance.isYesterday,
            usernameSearch: memberBalance.keyword,
            firstletter: memberBalance.firstLetter,
            currency: memberBalance.currency
        };

        $.ajax({
            type: "POST",
            url: "Service/DownLineBalanceService.svc/GetMemberBalance",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs && rs.DownLineBalane && rs.DownLineBalane.length > 0) {
                    memberBalance.title = 'Member ' + (memberBalance.isYesterDay ? 'Yesterday Balance' : 'Total Balance');
                    $('#memberBalanceTitle').html(memberBalance.title);
                    var arr = [];
                    for (var i = 0; i < rs.DownLineBalane.length; i++) {
                        var css = i % 2 == 0 ? "bgeven" : "";
                        arr.push(memberBalance.renderBalanceRow(rs.DownLineBalane[i], css, i + 1));
                    }
                    $('#tblMemberBalance').empty().html(arr.join(''));
                    $('#tblFooterMemberBalance').empty().html(memberBalance.renderFooterRow(rs.BaseTotal));
                    $("#memberBalanceGrid").removeClass("hidden").reRenderScroller(595, 510, true);
                }
                else {
                    showAlertMessage("errMsg", "No information is available .", 1);
                }
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderBalanceRow: function (row, css, index) {
        var html = '<tr class="' + css + '">';
        html += '<td><div class="col40 c ">' + index + '</div></td>';
        html += '<td><div class="col200 c">' + row.Username + '</div></td>';
        html += '<td><div class="col60 c">' + row.Currency + '</div></td>';
        html += '<td><div class="col140 r">' + row.Balance + '</div></td>';
        html += '<td><div class="col140 r">' + row.BaseBalance + '</div></td>';
        html += '</tr>';
        return html;
    },
    renderFooterRow: function (row) {
        var html = '<tr class="footer">';
        html += '<td colspan="4"><div class="col20 c" style="width: 449px;">TOTAL</div></td>';
        html += '<td colspan="4"><div class="col140 r ">' + row.BaseBalance + '</div></td>';
        html += '</tr>';

        return html;
    },
    goBack: function () {
        $('#superBalanceGrid').addClass('hidden');
        $('#masterBalanceGrid').addClass('hidden');
        $('#agentBalanceGrid').removeClass('hidden');
        $('#memberBalanceGrid').addClass('hidden');
        $('#errMsg').html('');
        $('#mainHeader').html('Member Balance');
    },
    exportExcel: function () {
        var url = 'SuperBalanceNew.aspx?isexport=true&rtitle=' + memberBalance.title +
        '&roleid=1&custid=' + memberBalance.custId + '&isyesterday=' + memberBalance.isYesterDay + '&firstletter=' + memberBalance.firstLetter +
        '&username=' + memberBalance.keyword + '&currency=' + memberBalance.currency;
        $("#frm").attr('src', url);
    }
};