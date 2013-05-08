


var specialDeleteVirtualSport = {
    initPage: function () {
        specialDeleteUtils.blockUI();
        $(".maincontent_box").css("width", "907px");
        specialDeleteVirtualSport.initBettypeList();
        $('div.box_button.list').removeClass("hidden");
    },
    initBettypeList: function () {
        var data = { type: 5 };
        $.ajax({
            type: "POST",
            url: "../DeleteBets/GetBettypes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            timeout: 5000,
            success: function (result) {
                specialDeleteVirtualSport.renderBettypeList(result);
            },
            error: function (error, status, xhr) {
                $("#bettype-cont").empty();
            },
            complete: function () {
                specialDeleteEventBinding.binddingEvent();
                specialDeleteUtils.unblockUI();
            }
        });
    },
    renderBettypeList: function (result) {
        var betList = "";
        var totalBetType = result.length;
        var betTypeLastRowClass = "";
        for (var i = 0; i < totalBetType; i++) {
            var betType = result[i];
            var betTypeCssClass = "";
            var indexMod5 = (i + 1) % 5;
            if (indexMod5 == 0) {
                betTypeCssClass = "brnone";
            }
            betList += '<div class="bettype-item ' + betTypeCssClass + betTypeLastRowClass + '">\
                            <input id="chk-bettype-' + betType.Id + '" class="chk-bettype" value="' +
                                    betType.Id + '" type="checkbox" />&nbsp;<div class="check bettype" rel="' +
                                    betType.Id + '">' + betType.Name + '</div>\
                        </div>';
            if (indexMod5 == 0) {
                if (i + 6 > totalBetType) {
                    betTypeLastRowClass = " bbnone";
                }
            }
        }
        $("#bettype-cont").empty();
        $("#bettype-cont").html(betList);
    },
    showBet: function () {
        try {
            // check valid refno 
            if (specialDeleteUtils.validMessage.length > 0) {
                showAlertMessage("ErrorMessage", specialDeleteUtils.validMessage, 1);
                $('#ErrorMessage').removeClass("hidden");
                window.scrollTo(0, 0);
                return false;
            }

            //--------------
            if (!isNumberNotNull('matchid')) {
                showAlertMessage("ErrorMessage", "Please enter a valid Match Id (number)!", 1);
                $('#ErrorMessage').removeClass("hidden");
                $('#matchid').focus();
                return false;
            }
            par.MatchId = $.trim($('#matchid').val());
            if (parseInt(par.MatchId) > 2147483647) {
                showAlertMessage("ErrorMessage", "Please enter a valid Match Id (number)!", 1);
                $('#ErrorMessage').removeClass("hidden");
                $('#matchid').focus();
                return false;
            }
            if (!isNumberNotNull('frefno')) {
                showAlertMessage("ErrorMessage", "Please enter a valid From Ref Number!", 1);
                $('#ErrorMessage').removeClass("hidden");
                $('#frefno').focus();
                return false;
            }
            if (!isNumberNotNull('trefno')) {
                showAlertMessage("ErrorMessage", "Please enter a valid To Ref Number!", 1);
                $('#ErrorMessage').removeClass("hidden");
                $('#trefno').focus();
                return false;
            }
            if ($('#ddlLiveIndicator').val() == '1') {
                if ($('#txtHomeScore').val() == '' && $('#txtAwayScore').val() != '') {
                    showAlertMessage("ErrorMessage", "Please enter a Live Home Score!", 1);
                    $('#ErrorMessage').removeClass("hidden");
                    $('#txtHomeScore').focus();
                    return false;
                }
                if ($('#txtHomeScore').val() != '' && $('#txtAwayScore').val() == '') {
                    showAlertMessage("ErrorMessage", "Please enter a Live Away Score!", 1);
                    $('#ErrorMessage').removeClass("hidden");
                    $('#txtAwayScore').focus();
                    return false;
                }
            }

            // valid bettype
            var chk_bettype = $('input.chk-bettype');
            var arr_bettype = [];
            $.each(chk_bettype, function (i, elm) {
                if ($(elm).is(':checked') && $(elm).attr('id').indexOf('all') == -1) {
                    arr_bettype.push($(elm).val());
                }
            });

            if ($('input.chk-bettype').length <= 1) {
                showAlertMessage("ErrorMessage", "There is no bet type.", 1);
                $('#ErrorMessage').removeClass("hidden");
                return false;
            }
            if (arr_bettype.length == 0) {
                showAlertMessage("ErrorMessage", "Please select a bet type!", 1);
                $('#ErrorMessage').removeClass("hidden");
                return false;
            }
            $('#bettype').val(arr_bettype.join('^'));

            // valid correct score
            var arr_score = [], scores = '';
            if ($('#chk-bettype-1204').is(':checked')) {
                $.each($('input.chk-score'), function (ind, elm) {
                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { arr_score.push($(elm).val()); }
                });
                if (arr_score.length == 0) {
                    showAlertMessage("ErrorMessage", "Please select a score!", 1);
                    $('#ErrorMessage').removeClass("hidden");
                    return false;
                }
                scores = arr_score.join('^');
                if ($('#chk-score-all').is(':checked')) { scores = 'All'; }
            }
            // valid hdp
            if ($('#hdp').val().length > 0) {
                if (isNaN($('#hdp').val())) {
                    showAlertMessage("ErrorMessage", "Please enter a valid To HDP Number!", 1);
                    $('#ErrorMessage').removeClass("hidden");
                    $('#hdp').focus();
                    return false;
                }
            }
            //            // valid htft-16
            //            var all_htft = [], htfts = '';
            //            if ($('#chk-bettype-16').is(':checked')) {
            //                $.each($('input.chk-htft'), function (ind, elm) {
            //                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { all_htft.push($(elm).val()); }
            //                });
            //                if (all_htft.length == 0) {
            //                    showAlertMessage("ErrorMessage", "Please select a HT/FT!", 1);
            //                    return false;
            //                }
            //                htfts = all_htft.join('^');
            //                if ($('#chk-htft-all').is(':checked')) { htfts = 'All'; }
            //            }
            // valid total goal-1206
            var all_tg = [], totalgoals = '';
            if ($('#chk-bettype-1206').is(':checked')) {
                $.each($('input.chk-tg'), function (ind, elm) {
                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { all_tg.push($(elm).val()); }
                });
                if (all_tg.length == 0) {
                    showAlertMessage("ErrorMessage", "Please select a Total Goal!", 1);
                    $('#ErrorMessage').removeClass("hidden");
                    return false;
                }
                totalgoals = all_tg.join('^');
                if ($('#chk-tg-all').is(':checked')) { totalgoals = 'All'; }
            }
            //            // valid f/l goal-14
            //            var all_flg = [], flgoals = '';
            //            if ($('#chk-bettype-14').is(':checked')) {
            //                $.each($('input.chk-flg'), function (ind, elm) {
            //                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { all_flg.push($(elm).val()); }
            //                });
            //                if (all_flg.length == 0) {
            //                    showAlertMessage("ErrorMessage", "Please select a First Goal/Last Goal!", 1);
            //                    return false;
            //                }
            //                flgoals = all_flg.join('^');
            //                if ($('#chk-flg-all').is(':checked')) { flgoals = 'All'; }
            //            }

            var HomeScore = $('#txtHomeScore').val();
            var AwayScore = $('#txtAwayScore').val();
            var FromTransDate = $('#txtFromdate').val();
            var ToTransDate = $('#txtTodate').val();

            if ($('#ddlLiveIndicator').val() != '1') {
                HomeScore = '';
                AwayScore = '';
                FromTransDate = '';
                ToTransDate = '';
            }

            //par.MatchId = $.trim($('#matchid').val());
            par.FromRefNo = $.trim($('#frefno').val());
            par.ToRefNo = $.trim($('#trefno').val());
            par.Hdp = $('#hdp').val().length == 0 ? '9999' : $('#hdp').val();
            par.LiveIndicator = $('#ddlLiveIndicator').val() == 'All' ? '2' : $('#ddlLiveIndicator').val();
            par.Bettypes = $('#bettype').val();
            par.Betteam1378 = $('#ddlBetTeam1378').val();
            par.Betteam515 = $('#ddlBetTeam515').val();
            par.Betteam430 = scores;
            par.Betteam6 = totalgoals;
            //            par.Betteam14 = flgoals;
            //            par.Betteam16 = htfts;
            par.HomeScore = HomeScore;
            par.AwayScore = AwayScore;
            par.FromTransDate = FromTransDate;
            par.ToTransDate = ToTransDate
            //par.SAFilter = $('#ddlSAFilter').val() == 'All' ? '2' : $('#ddlSAFilter').val();
            par.SAFilter = $('#ddlSAFilter').val();
            var data = {
                'param': par
            };
            specialDeleteUtils.validAction('list');
            specialDeleteUtils.blockUI();
            $.ajax({
                type: "POST",
                url: "../DeleteBets/BettransSelSpecialDel",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    $('#box_list_cont').removeClass("hidden");
                    $('#box_list_cont').html('<p align="center" class="b">Loading...</p>');
                },
                success: function (rs) {
                    $('#ErrorMessage').addClass("hidden");
                    if (rs) {
                        var bettypes = rs.Items;
                        if (parseInt(rs.ErrorCode) > 0) {
                            specialDeleteVirtualSport.noTransactionInfo(true);
                        }
                        else {
                            $('#box_match_info').empty();
                            $('#box_match_info').removeClass("hidden");
                            $('#row_match').tmpl(rs).appendTo($('#box_match_info'));
                            var no_row = true;
                            $.each(bettypes, function (ind, bettype) {
                                var rel = ind;
                                if (bettype.TransactionInfo.length > 0) {
                                    no_row = false;
                                }
                            });
                            $('#box_list_cont').empty();
                            if (no_row) {
                                $('#row_nobet_tpl').tmpl(null).appendTo($('#box_list_cont'));
                            }
                            else {
                                $('#row_bet_tpl').tmpl({ NoRow: false }).appendTo($('#box_list_cont'));
                            }

                            for (var b = 0; b < bettypes.length; b++) {
                                var bettype = bettypes[b];
                                if (bettype.TransactionInfo.length > 0) {
                                    $('#item_bettype_tpl').tmpl(bettype).appendTo($('#box_list_scroll'));
                                    for (var i = 0; i < bettype.TransactionInfo.length; i++) {
                                        var _item = bettype.TransactionInfo[i];
                                        $('#item_betrans_tpl').tmpl(_item, {
                                            data: _item,
                                            bettype: { IsAllowOptionDelete: bettype.IsAllowOptionDelete, OptionType: bettype.OptionType }
                                        }).appendTo($('table#tbl_' + bettype.Type + ' > tbody'));
                                    }
                                }
                            }
                            window.setTimeout(function () {
                                if (no_row) {
                                    specialDeleteVirtualSport.noTransactionInfo(false);
                                } else {
                                    specialDeleteUtils.validAction('hasrow');
                                    // row title
                                    $('div.title_label').bind('click', function (e) {
                                        specialDeleteUtils.toggleDiv(e, $(this));
                                    });
                                    // row line
                                    $('tr.betrow.line').bind('mouseover', function (e) {
                                        $(this).addClass('cur');
                                    });
                                    $('tr.betrow.line').bind('mouseout', function (e) {
                                        $(this).removeClass('cur');
                                    });
                                }
                            }, 500);
                        }
                    } else {
                        specialDeleteVirtualSport.noTransactionInfo(true);
                    }
                },
                error: function () {
                    specialDeleteVirtualSport.noTransactionInfo(true);
                },
                complete: function () {
                    $("#bg_table").removeClass("hidden");
                    $('#username').val(accUserName);
                    specialDeleteUtils.unblockUI();
                }
            });
        } catch (e) { alert(e); }
    },
    noTransactionInfo: function (isClearMatchInfo) {
        if (isClearMatchInfo) {
            $('#box_match_info').empty();
            $('#box_match_info').addClass("hidden");
        }
        specialDeleteUtils.validAction('norow');
        $('#box_list_cont').empty();
        $('#box_list_cont').html(specialDeleteVirtualSport.renderNoTransactionInfo());
    },
    renderNoTransactionInfo: function () {
        return $('#row_nobet_tpl').tmpl(null);
    },
    deleteBet: function () {
        $('#ErrorMessage').html('');
        $('#ErrorMessage').addClass("hidden");
        $("#tdErrorMessage").addClass("hidden");
        $('#ErrorMessageDeleteBets').html('');
        if ($('#username').val() == "") {
            showAlertMessage("ErrorMessageDeleteBets", "Please enter username!", 1);
            $("#tdErrorMessage").removeClass("hidden");
            $('#username').focus();
            return false;
        }
        if ($('#userpwd').val() == "") {
            showAlertMessage("ErrorMessageDeleteBets", "Please enter password!", 1);
            $("#tdErrorMessage").removeClass("hidden");
            $('#userpwd').focus();
            return false;
        }
        if ($('#men').val() == "") {
            showAlertMessage("ErrorMessageDeleteBets", "You must enter English message!", 1);
            $("#tdErrorMessage").removeClass("hidden");
            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#men').focus();
            return false;
        }
        if ($('#men').val().indexOf('@@') == -1) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in  English message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");
            $('div#box_list_message').show();
            $('div.box_header.message').show().find('span').removeClass('up');
            $('#men').focus();
            return false;
        }
        if ($('#mch').val() == "") {
            showAlertMessage("ErrorMessageDeleteBets", "You must enter Chinese message!", 1);
            $("#tdErrorMessage").removeClass("hidden");
            $('div#box_list_message').show();
            $('div.box_header.message').show().find('span').removeClass('up');
            $('#mch').focus();
            return false;
        }
        if ($('#mch').val().indexOf('@@') == -1) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in Chinese message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");
            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#mch').focus();
            return false;
        }
        if (($('#mcs').val() != "") && ($('#mcs').val().indexOf('@@') == -1)) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in Simple Chinese message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");

            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#mcs').focus();
            return false;
        }
        if (($('#mth').val() != "") && ($('#mth').val().indexOf('@@') == -1)) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in Thai message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");

            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#mth').focus();
            return false;
        }
        if (($('#mit').val() != "") && ($('#mit').val().indexOf('@@') == -1)) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in Italian message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");

            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#mit').focus();
            return false;
        }
        if (($('#mjp').val() != "") && ($('#mjp').val().indexOf('@@') == -1)) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in Japanese message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");

            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#mjp').focus();
            return false;
        }
        if (($('#mes').val() != "") && ($('#mes').val().indexOf('@@') == -1)) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in Spanish message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");

            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#mes').focus();
            return false;
        }
        if (($('#mko').val() != "") && ($('#mko').val().indexOf('@@') == -1)) {
            showAlertMessage("ErrorMessageDeleteBets", "You must include @@ in Korean message for replacing ticket number!", 1);
            $("#tdErrorMessage").removeClass("hidden");

            $('div#box_list_message').show();
            $('div.box_header.message').show().find('div').removeClass('up');
            $('#mko').focus();
            return false;
        }

        //try {
        var data_partner = {
            Deposit: { opt: 0, ids: [] },
            BetDaq: { opt: 0, ids: [] },
            EU: { opt: 0, ids: [] },
            MHC: { opt: 0, ids: [] }
        };
        var sl_partner = $('select.sel-partner');
        $(sl_partner).each(function (ind, sel) {
            var _partner = $(sel).attr('partner'), _field = _partner.replace(/12/, ''), delOpt = $(sel).val();
            eval("data_partner." + _field + ".opt=" + delOpt + ";");
            if (delOpt == '2') {
                var bet_checks = $('input.chk' + _partner + '-1');
                $(bet_checks).each(function (i, chk) {
                    if ($(chk).is(':checked')) {
                        eval("data_partner." + _field + ".ids.push(" + $(chk).val() + ");");
                    }
                });
            }
        });
        // set message content
        msg.Content_en = $('#men').val();
        msg.Content_ch = $('#mch').val();
        msg.Content_cs = $('#mcs').val();
        msg.Content_it = $('#mit').val();
        msg.Content_jp = $('#mjp').val();
        msg.Content_th = $('#mth').val();
        msg.Content_es = $('#mes').val();
        msg.Content_ko = $('#mko').val();
        msg.UserName = $('#username').val();
        msg.UserId = $('#accId').val();
        msg.userPwd = $('#userpwd').val();
        msg.CreatorId = $('#userId').val();
        msg.CreatorName = $('#username').val();

        var bettypes = '', betteam430 = '', betteam6 = '', betteam14 = '', betteam16 = '';
        if (typeof (par) !== undefined) {
            bettypes = par.Bettypes || '';
            betteam430 = par.Betteam430 || '';
            betteam6 = par.Betteam6 || '';
            betteam14 = par.Betteam14 || '';
            betteam16 = par.Betteam16 || '';
        }

        var HomeScore = $('#txtHomeScore').val();
        var AwayScore = $('#txtAwayScore').val();
        var FromTransdate = $('#txtFromdate').val();
        var ToTransdate = $('#txtTodate').val();
        if ($('#ddlLiveIndicator').val() != '1') {
            HomeScore = '';
            AwayScore = '';
            FromTransdate = '';
            ToTransdate = '';
        }

        var data = {
            param: {
                'MatchId': par.MatchId,
                'FromRefNo': par.FromRefNo,
                'ToRefNo': par.ToRefNo,
                'Bettypes': par.Bettypes,
                'Betteam1378': par.Betteam1378,
                'Betteam515': par.Betteam515,
                'Hdp': par.Hdp,
                'UserName': $('#username').val(),
                'UserId': $('#userId').val(),
                'UserPassword': $('#userpwd').val(),
                'DelOptions': data_partner.Deposit.opt,
                'TransIds': data_partner.Deposit.ids.join(','),
                'DelOption_BD': data_partner.BetDaq.opt,
                'TransIds_BD': data_partner.BetDaq.ids.join(','),
                'DelOption_12EU': data_partner.EU.opt,
                'TransIds_12EU': data_partner.EU.ids.join(','),
                'DelOption_MHC': data_partner.MHC.opt,
                'TransIds_MHC': data_partner.MHC.ids.join(','),
                'Betteam430': par.Betteam430,
                'Betteam6': par.Betteam6,
                'Betteam14': par.Betteam14,
                'Betteam16': par.Betteam16,
                'SpecialDeleteMessage': msg,
                'HomeScore': par.HomeScore,
                'AwayScore': par.AwayScore,
                'FromTransDate': par.FromTransdate,
                'ToTransDate': par.ToTransdate,
                'SAFilter': par.SAFilter,
                'LiveIndicator': par.Liveindicator
            }
        };

        $.ajax({
            type: "POST",
            url: "../DeleteBets/DeleteSpecialTransaction",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function () {
                specialDeleteUtils.blockUI();
            },
            success: function (resp) {
                specialDeleteUtils.completeBet(resp);
            },
            error: function (error, status, xhr) {
            },
            complete: function () {
                specialDeleteUtils.unblockUI();
            }
        });
    }
}

$(document).ready(function () {
    specialDeleteVirtualSport.initPage();
});