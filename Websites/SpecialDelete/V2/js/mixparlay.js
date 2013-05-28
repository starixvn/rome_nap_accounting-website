var CURDATE = new Date();

var mixparlay = {
    counter: 100,
    initForm: function () {
        var data = { type: 4 };
        $.ajax({
            type: "POST",
            url: "../Services/SpecialDeleteService.svc/GetBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            timeout: 6000,
            success: function (resp) {
                if (resp) {
                    var resp = { Rows: resp, Count: resp.length };
                    $('#betform_tpl').tmpl(resp).appendTo($('#box_setting_form'));
                    window.setTimeout(function () {
                        // event bindding
                        mixparlay.binddingEvent();
                    }, 500);
                }
            },
            error: function (error, status, xhr) {
                //alert(status);
            }
        });
    },
    binddingEvent: function () {
        SetupCalendar(CURDATE.format('m/d/Y H:i:s'), '#txtFromdate');
        SetupCalendar(CURDATE.format('m/d/Y H:i:s'), '#txtTodate');
        $('div.box_button.list').show();
        $('div.box_header').bind('click', function (e) {
            var ids = $(this).attr('rel').toString().split(':');
            $(this).find('span').toggleClass('up');
            for (i = 0; i < ids.length; i++) {
                $('div#' + ids[i]).slideToggle();
            }
        });
        $('div.advance-header').bind('click', function (e) {
            var ids = $(this).attr('rel').toString().split(':');
            $(this).find('span').toggleClass('up');
            for (i = 0; i < ids.length; i++) {
                $('div#' + ids[i]).slideToggle();
            }
        });
        //
        $('input.btn.showbet').bind('click', function (e) {
            mixparlay.showBet(1, mixparlay.counter, true);
        });
        $('input.btn.delbet').bind('click', function (e) {
            mixparlay.deleteBet();
        });
        $('input#chk-send-email').click(function (e) {
            mixparlay.validMessage($(this).is(':checked'));
        });
        // bettype
        this.bindingBettype();
        // advance mode
        this.bindingAdvance();
        //test
        this.binddingResult();
    },
    bindingBettype: function () {
        $('input.chk-bettype').bind('click', function (e) {
            var _chked = $(this).attr('checked'), _isall = false;
            if ($(this).attr('id').indexOf('all') != -1) {
                _isall = true;
            }
            mixparlay.validChkBettype($(this).val(), _isall, _chked);
        });
        $('span.check.bettype').bind('click', function (e) {
            var _rel = $(this).attr('rel'), _chked = $('input#chk-bettype-' + _rel).attr('checked');
            var _isall = false;
            $('input#chk-bettype-' + _rel).attr('checked', !_chked);
            if (_rel.indexOf('all') != -1) {
                _isall = true;
            }
            mixparlay.validChkBettype($('input#chk-bettype-' + _rel).val(), _isall, !_chked);
        });
    },
    bindingAdvance: function () {
        $('input.chk-score').bind('click', function (e) {
            var _chked = $(this).attr('checked'), _isall = true;
            if ($(this).attr('id').indexOf('all') != -1) {
                $('input.chk-score').attr('checked', _chked);
            } else {
                $.each($('input.chk-score'), function (ind, elm) {
                    if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                });
                $('#chk-score-all').attr('checked', _isall);
            }
        });
        $('span.check.score').bind('click', function (e) {
            var _rel = $(this).attr('rel');
            if (($('input#chk-score-' + _rel).attr('disabled') == false)) {
                var _chked = $('input#chk-score-' + _rel).attr('checked'), _isall = true;
                $('input#chk-score-' + _rel).attr('checked', !_chked);
                if (_rel.indexOf('all') != -1) {
                    $('input.chk-score').attr('checked', !_chked);
                } else {
                    $.each($('input.chk-score'), function (ind, elm) {
                        if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                    });
                    $('#chk-score-all').attr('checked', _isall);
                }
            }
        });
        // htft
        $('input.chk-htft').bind('click', function (e) {
            var _chked = $(this).attr('checked'), _isall = true;
            if ($(this).attr('id').indexOf('all') != -1) {
                $('input.chk-htft').attr('checked', _chked);
            } else {
                $.each($('input.chk-htft'), function (ind, elm) {
                    if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                });
                $('#chk-htft-all').attr('checked', _isall);
            }
        });
        $('span.check.htft').bind('click', function (e) {
            var _rel = $(this).attr('rel');
            if (($('input#chk-htft-' + _rel).attr('disabled') == false)) {
                var _chked = $('input#chk-htft-' + _rel).attr('checked'), _isall = true;
                $('input#chk-htft-' + _rel).attr('checked', !_chked);
                if (_rel.indexOf('all') != -1) {
                    $('input.chk-htft').attr('checked', !_chked);
                } else {
                    $.each($('input.chk-htft'), function (ind, elm) {
                        if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                    });
                    $('#chk-htft-all').attr('checked', _isall);
                }
            }
        });
        //total goal
        $('input.chk-tg').bind('click', function (e) {
            var _chked = $(this).attr('checked'), _isall = true;
            if ($(this).attr('id').indexOf('all') != -1) {
                $('input.chk-tg').attr('checked', _chked);
            } else {
                $.each($('input.chk-tg'), function (ind, elm) {
                    if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                });
                $('#chk-tg-all').attr('checked', _isall);
            }
        });
        $('span.check.tg').bind('click', function (e) {
            var _rel = $(this).attr('rel');
            if (($('input#chk-tg-' + _rel).attr('disabled') == false)) {
                var _chked = $('input#chk-tg-' + _rel).attr('checked'), _isall = true;
                $('input#chk-tg-' + _rel).attr('checked', !_chked);
                if (_rel.indexOf('all') != -1) {
                    $('input.chk-tg').attr('checked', !_chked);
                } else {
                    $.each($('input.chk-tg'), function (ind, elm) {
                        if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                    });
                    $('#chk-tg-all').attr('checked', _isall);
                }
            }
        });
        //First Goal/Last Goal:
        $('input.chk-flg').bind('click', function (e) {
            var _chked = $(this).attr('checked'), _isall = true;
            if ($(this).attr('id').indexOf('all') != -1) {
                $('input.chk-flg').attr('checked', _chked);
            } else {
                $.each($('input.chk-flg'), function (ind, elm) {
                    if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                });
                $('#chk-flg-all').attr('checked', _isall);
            }
        });
        $('span.check.flg').bind('click', function (e) {
            var _rel = $(this).attr('rel'), _isall = true;
            if ($('input#chk-flg-' + _rel).attr('disabled') == false) {
                var _chked = $('input#chk-flg-' + _rel).attr('checked');
                $('input#chk-flg-' + _rel).attr('checked', !_chked);
                if (_rel.indexOf('all') != -1) {
                    $('input.chk-flg').attr('checked', !_chked);
                } else {
                    $.each($('input.chk-flg'), function (ind, elm) {
                        if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                    });
                    $('#chk-flg-all').attr('checked', _isall);
                }
            }
        });
    },
    validChkBettype: function (val, isall, chked) {
        if (isall) {
            $('input.chk-bettype').attr('checked', chked);
            this.showAdvanceBox();
        } else {
            var _chkall = true;
            $.each($('input.chk-bettype'), function (ind, elm) {
                if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _chkall = false; }
            });
            $('input#chk-bettype-all').attr('checked', _chkall);
        }
        mixparlay.highlightMode(val, chked, isall);
    },
    highlightMode: function (val, chked, isall) {
        //alert('isall:' + isall);
        if (isall) {
            mixparlay.enableAllMode(chked);
            return;
        }
        var mode = "";
        var is1378 = null; //Handicap, 1st Handicap , Over/Under  and 1st Over/Under 
        var is515 = null; //1X2 and FT.1X2
        var is430 = null; //Correct Score
        var is4_30 = [0, 0]; //Correct Score
        var is212 = null; //Odd/Even, 1st Odd/Even
        var is6, is14, is16;
        if (val) {
            switch (val) {
                case "1":
                case "3":
                case "7":
                case "8":
                    mode = 'hdp';
                    break;
                case "4":
                case "30":
                    mode = 'score';
                    break;
                case "2": //Odd/Even, 1st Odd/Even
                case "12":
                    mode = 'hdp';
                    break;
                case "5":
                case "15":
                    mode = 'ft'; is515 = chked;
                    break;
                case "6":
                    mode = 'tg'; is6 = chked;
                    break;
                case "14":
                    mode = 'flg'; is14 = chked;
                    break;
                case "16":
                    mode = 'htft'; is16 = chked;
                    break;
                default:
                    break;
            }
            is1378 = this.checkedBet('1,3,7,8');
            is212 = this.checkedBet('2,12');
            is430 = this.checkedBet('4,30');
            if (is1378 || is212) {
                $('#ddlBetTeam1378').removeAttr('disabled');
                if (is1378) {
                    $('#hdp').removeAttr('disabled');
                } else {
                    $('#hdp').attr('disabled', 'disabled').val('');
                }
            } else {
                $('#ddlBetTeam1378').attr('disabled', 'disabled').val('All');
                $('#hdp').attr('disabled', 'disabled').val('');
            }
            is515 = this.checkedBet('5,15');
            if (is515) {
                $('#ddlBetTeam515').removeAttr('disabled');
            } else {
                $('#ddlBetTeam515').attr('disabled', 'disabled').val('All');
            }
            if (mode.indexOf('score') != -1) {
                is4_30[0] = $('#chk-bettype-4').is(':checked') ? 1 : 0;
                is4_30[1] = $('#chk-bettype-30').is(':checked') ? 1 : 0;
                var checkResult430 = parseInt(is4_30[0]) + parseInt(is4_30[1]);
                if (checkResult430 == 2) {
                    $('#chk-score-all').removeAttr('disabled');
                    $('#chk-score-all').attr('checked', true);

                    $('input.chk-score').removeAttr('disabled');
                    $('input.chk-score').attr('checked', true);
                }
                else if (checkResult430 == 1) {
                    var _ischecking = false;
                    $.each($('input.chk-score'), function (_ind, _elm) {
                        if ($(_elm).is(':checked')) { _ischecking = true; }
                    });
                    if (!_ischecking) {
                        $('#chk-score-all').removeAttr('disabled');
                        $('#chk-score-all').attr('checked', true);

                        $('input.chk-score').removeAttr('disabled');
                        $('input.chk-score').attr('checked', true);
                    }
                }
                else if (is430 != null && is430 == false) {
                    $('#chk-score-all').attr('disabled', 'disabled');
                    $('#chk-score-all').attr('checked', false);

                    $('input.chk-score').attr('disabled', 'disabled');
                    $('input.chk-score').attr('checked', false);
                }
            }
            // is total goal
            if (is6 != null && is6 == true) {
                $('input.chk-tg').removeAttr('disabled');
                $('input.chk-tg').attr('checked', true);
            } else if (is6 != null && is6 == false) {
                $('input.chk-tg').attr('disabled', 'disabled');
                $('input.chk-tg').attr('checked', false);
            }
            // is first/last goal
            if (is14 != null && is14 == true) {
                $('input.chk-flg').removeAttr('disabled');
                $('input.chk-flg').attr('checked', true);
            } else if (is14 != null && is14 == false) {
                $('input.chk-flg').attr('disabled', 'disabled');
                $('input.chk-flg').attr('checked', false);
            }
            // is ht/ft
            if (is16 != null && is16 == true) {
                $('input.chk-htft').removeAttr('disabled');
                $('input.chk-htft').attr('checked', true);
            } else if (is16 != null && is16 == false) {
                $('input.chk-htft').attr('disabled', 'disabled');
                $('input.chk-htft').attr('checked', false);
            }
            // set highlight mode
            if (mode && chked) {
                this.showAdvanceBox();
                $('div.advance-item.' + mode).toggleClass('cur');
                window.setTimeout(function () {
                    $('div.advance-item.' + mode).toggleClass('cur');
                }, 3000);
            }
        }
        return false;
    },
    enableAllMode: function (chked) {
        // set disabled
        if (chked) {
            //HDP
            $('#hdp').removeAttr('disabled');
            $('#ddlBetTeam1378').removeAttr('disabled');
            //FT
            $('#ddlBetTeam515').removeAttr('disabled');
            //SCORE
            $('#chk-score-all').removeAttr('disabled');
            $('input.chk-score').removeAttr('disabled');
            //HT/FT
            $('input.chk-htft').removeAttr('disabled');
            //TG
            $('input.chk-tg').removeAttr('disabled');
            //FG/LG
            $('input.chk-flg').removeAttr('disabled');
        } else {
            //HDP
            $('#hdp').attr('disabled', 'disabled').val('');
            $('#ddlBetTeam1378').attr('disabled', 'disabled').val('All');
            //FT
            $('#ddlBetTeam515').attr('disabled', 'disabled').val('All');
            //SCORE
            $('#chk-score-all').attr('disabled', 'disabled');
            $('input.chk-score').attr('disabled', 'disabled');
            //HT/FT
            $('input.chk-htft').attr('disabled', 'disabled');
            //TG
            $('input.chk-tg').attr('disabled', 'disabled');
            //FG/LG
            $('input.chk-flg').attr('disabled', 'disabled');
        }
        // set checked
        //SCORE
        $('#chk-score-all').attr('checked', chked);
        $('input.chk-score').attr('checked', chked);
        //HT/FT
        $('#chk-htft-all').attr('checked', chked);
        $('input.chk-htft').attr('checked', chked);
        //TG
        $('#chk-tg-all').attr('checked', chked);
        $('input.chk-tg').attr('checked', chked);
        //FG/LG
        $('#chk-flg-all').attr('checked', chked);
        $('input.chk-flg').attr('checked', chked);
    },
    showAdvanceBox: function () {
        if ($('div#advance-cont').is(":hidden")) {
            var ids = $('div#advance-header').attr('rel').toString().split(':');
            $('div#advance-header').find('span').toggleClass('up');
            for (i = 0; i < ids.length; i++) {
                $('div#' + ids[i]).slideToggle();
            }
        }
    },
    checkedBet: function (bets) {
        if (bets) {
            var arr_b = bets.split(',');
            for (i = 0; i < arr_b.length; i++) {
                if ($('#chk-bettype-' + arr_b[i]).attr('checked')) { return true; }
            }
        }
        return false;
    },
    showBet: function (page, counter, refresh) {
        try {
            if (!page) { var page = 1; }
            if (!counter) { var counter = mixparlay.counter; }
            if (!refresh) { var refresh = false; }
            $('#spErrCS').html('');
            $('#dvContent').html('');



            //--------------
            var matchid = $("#matchid").val();
            if (!isNumberNotNull('matchid')) {
                showAlertMessage("ErrMsg", "Please enter a valid Match Id (number) !", 1);
                $('#matchid').focus();
                return false;
            }

            if (parseInt(matchid) > 2147483647) {
                showAlertMessage("ErrMsg", "Please enter a valid Match Id (number) !", 1);
                $('#matchid').focus();
                return false;
            }
            if ($('#ddlLiveIndicator').val() == '1') {
                if ($('#txtHomeScore').val() == '' && $('#txtAwayScore').val() != '') {
                    showAlertMessage("ErrMsg", "Please enter a Live Home Score !", 1);
                    $('#txtHomeScore').focus();
                    return false;
                }
                if ($('#txtHomeScore').val() != '' && $('#txtAwayScore').val() == '') {
                    showAlertMessage("ErrMsg", "Please enter a Live Away Score !", 1);
                    $('#txtAwayScore').focus();
                    return false;
                }
            }

            if (!$('#chkMixparlay').attr('checked') && !$('#chkSystemparlay').attr('checked')) {
                showAlertMessage("ErrMsg", "Please select a type of Parlay.", 1);
                return false;
            }

            // valid bettype
            var chk_bettype = $('input.chk-bettype');
            var arr_bettype = [];
            $.each(chk_bettype, function (i, elm) {
                if ($(elm).is(':checked') && $(elm).attr('id').indexOf('all') == -1) { arr_bettype.push($(elm).val()); }
            });

            if (arr_bettype.length == 0) {
                showAlertMessage("ErrMsg", "Please select a bet type !", 1);
                return false;
            }



            $('#bettype').val(arr_bettype.join('^'));

            // valid correct score
            var arr_score = [], scores = '';
            if ($('#chk-bettype-4').is(':checked') || $('#chk-bettype-30').is(':checked')) {
                $.each($('input.chk-score'), function (ind, elm) {
                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { arr_score.push($(elm).val()); }
                });
                if (arr_score.length == 0) {
                    showAlertMessage("ErrMsg", "Please select a score !", 1);
                    return false;
                }
                scores = arr_score.join('^');
                if ($('#chk-score-all').is(':checked')) { scores = 'All'; }
            }
            // valid hdp
            if ($('#hdp').val().length > 0) {
                if (isNaN($('#hdp').val())) {
                    showAlertMessage("ErrMsg", "Please enter a valid To HDP Number !", 1);
                    $('#hdp').focus();
                    return false;
                }
            }
            // valid htft-16
            var all_htft = [], htfts = '';
            if ($('#chk-bettype-16').is(':checked')) {
                $.each($('input.chk-htft'), function (ind, elm) {
                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { all_htft.push($(elm).val()); }
                });
                if (all_htft.length == 0) {
                    showAlertMessage("ErrMsg", "Please select a HT/FT !", 1);
                    return false;
                }
                htfts = all_htft.join('^');
                if ($('#chk-htft-all').is(':checked')) { htfts = 'All'; }
            }
            // valid total goal-6
            var all_tg = [], totalgoals = '';
            if ($('#chk-bettype-6').is(':checked')) {
                $.each($('input.chk-tg'), function (ind, elm) {
                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { all_tg.push($(elm).val()); }
                });
                if (all_tg.length == 0) {
                    showAlertMessage("ErrMsg", "Please select a Total Goal !", 1);
                    return false;
                }
                totalgoals = all_tg.join('^');
                if ($('#chk-tg-all').is(':checked')) { totalgoals = 'All'; }
            }
            // valid f/l goal-14
            var all_flg = [], flgoals = '';
            if ($('#chk-bettype-14').is(':checked')) {
                $.each($('input.chk-flg'), function (ind, elm) {
                    if ($(elm).attr('checked') && $(elm).attr('id').indexOf('all') == -1) { all_flg.push($(elm).val()); }
                });
                if (all_flg.length == 0) {
                    showAlertMessage("ErrMsg", "Please select a First Goal/Last Goal !", 1);
                    return false;
                }
                flgoals = all_flg.join('^');
                if ($('#chk-flg-all').is(':checked')) { flgoals = 'All'; }
            }

            if (par) {
                $.extend(par, { Page: page, Counter: counter, Refresh: refresh });
                par.MatchId = $('#matchid').val();
                par.Hdp = $('#hdp').val().length == 0 ? '9999' : $('#hdp').val();
                par.Bettypes = $('#bettype').val();
                par.Betteam1378 = $('#ddlBetTeam1378').val();
                par.Betteam515 = $('#ddlBetTeam515').val();
                par.Betteam430 = scores;
                par.Betteam6 = totalgoals;
                par.Betteam14 = flgoals;
                par.Betteam16 = htfts;
                par.Type = $('#ddlLiveIndicator').val();
                par.HomeScore = $('#txtHomeScore').val();
                par.AwayScore = $('#txtAwayScore').val();
                par.FromTransdate = $('#txtFromdate').val();
                par.ToTransdate = $('#txtTodate').val();
                par.SelectBettype = ($('#chkMixparlay').attr('checked') && $('#chkSystemparlay').attr('checked') ? 0 : ($('#chkMixparlay').attr('checked') ? 9 : 29));
                if (par.Type != '1') {
                    par.HomeScore = '';
                    par.AwayScore = '';
                    par.FromTransdate = '';
                    par.ToTransdate = '';
                }
                $('#ErrMsg').empty();
                var data = {
                    'param': par
                };
                mixparlay.validAction('list');
                $.ajax({
                    type: "POST",
                    url: "../Services/SpecialDeleteService.svc/GetMPListPager",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function () {
                        try { parent.BlockUI(); } catch (e) { }
                        $('#box_list_cont').html('<p align="center">Loading...</p>');
                    },
                    success: function (rs) {
                        if (rs) {
                            var bettypes = rs.BetTypes;
                            if (parseInt(rs.ErrorCode) > 0) {
                                showAlertMessage("ErrMsg", rs.ErrorMessage, 1);
                                mixparlay.validAction('norow');
                                $('#box_list_cont').empty().html('<p align="center">There is no information available</p>');
                            }
                            else {
                                mixparlay.showBetCallbackPager(rs);
                            }
                        } else {
                            mixparlay.validAction('norow');
                        }
                    },
                    error: function (error, status, xhr) {
                        //alert(status);
                    },
                    complete: function () {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                    }
                });
                $('#username').val(AccUserName);
            }
        } catch (e) {
            //alert(e);
        }
    },
    showBetCallback: function (rs) {
        var no_row = true;
        $('#box_match_info').empty().show();
        $('#row_match').tmpl(rs.MatchInfo).appendTo($('#box_match_info'));
        $('#box_list_cont').empty().show();
        $('#row_bet_tpl').tmpl(null).appendTo($('#box_list_cont'));
        $(bettypes).each(function (ind, bettype) {
            if (bettype.Rows.length > 0) { no_row = false; }
            $('#item_bettype_tpl').tmpl(bettype).appendTo($('#box_list_scroll'));
            var _siteTpl = [];
            for (var ind = 0, n = bettype.Rows.length; ind < n; ind++) {
                var item = bettype.Rows[ind];
                _siteTpl.push('<tr class="betrow">' + $('#item_betrans_tpl').tmpl(item).html() + '</tr>');
                if (_siteTpl.length == 100 || ind == (n - 1)) {
                    $('table#tbl_' + bettype.Type + ' > tbody').append(_siteTpl.join(''));
                    _siteTpl = [];
                }
            }
        });
        window.setTimeout(function () {
            if (no_row) {
                mixparlay.validAction('norow');
                $('#box_list_cont').empty().html('<p align="center">There is no information available</p>');
            } else {
                mixparlay.validAction('hasrow');
                mixparlay.binddingResult();
            }
        }, 500);
    },
    showBetCallbackPager: function (rs) {
        var no_row = true;
        if (rs && rs.Rows.length > 0) { no_row = false; }
        $('#box_match_info').empty().show();
        $('#row_match').tmpl(rs.MatchInfo).appendTo($('#box_match_info'));
        $('#box_list_cont').empty().show();
        $('#row_bet_tpl').tmpl(null).appendTo($('#box_list_cont'));
        var _curGroup = '', _siteTpl = [];
        var ind = 0, n = rs.Rows.length;
        $(rs.Rows).each(function (i, row) {
            var _group = row.Type;
            if (_group != _curGroup) {
                _curGroup = _group;
                var _bettype = { Type: _group, Rows: [row] };
                $('#item_bettype_tpl').tmpl(_bettype).appendTo($('#box_list_scroll'));
            }
            $('table#tbl_' + _group + ' > tbody').append('<tr class="betrow">' + $('#item_betrans_tpl').tmpl(row).html() + '</tr>');
        });
        if (n > 0 && (rs.Total > n || rs.Page != 0)) {
            $('#pagination').pagination(rs.Total, {
                current_page: rs.Page,
                items_per_page: rs.Counter,
                show_pagesize: true,
                maxRow: mixparlay.counter,
                goPage: true,
                pageBegin: 1,
                callback: mixparlay.pagingCallBack
            }).show();
        }
        window.setTimeout(function () {
            if (no_row) {
                mixparlay.validAction('norow');
                $('#box_list_cont').empty().html('<p align="center">There is no information available</p>');
            } else {
                mixparlay.validAction('hasrow');
                mixparlay.binddingResult();
            }
        }, 500);
    },
    pagingCallBack: function (page, counter, jpager) {
        //alert('page: ' + page + ', counter: ' + counter);
        mixparlay.showBet(page, counter);
        $('#pagination').hide();
    },
    binddingResult: function () {
        // row title
        $('div.title_label').bind('click', function (e) {
            var ids = $(this).attr('rel').toString().split(':');
            for (i = 0; i < ids.length; i++) {
                $('div#' + ids[i]).slideToggle();
            }
            $(this).find('span').toggleClass('up');
        });
        // row line
        $('tr.betrow').hover(function () {
            $(this).addClass('cur');
        },
        function () {
            $(this).removeClass('cur');
        });
        $('div.matchmp').hover(function () {
            $(this).addClass('cur');
        },
        function () {
            $(this).removeClass('cur');
        });
        $('span.zoom.in').bind('click', function (e) {
            if ($(this).attr('data') == '1') {
                var _refno = $(this).attr('refno');
                $('#expand-all-' + _refno).slideUp('fast', function () {
                    $('#expand-cont-' + _refno).slideDown('fast');
                });
            } else {
                mixparlay.expandChoice(null, $(this).attr('refno'));
            }
        });
        // expand all
        $('a.txt-expand-all').bind('click', function (e) {
            if (mixparlay.expanding == null) {
                mixparlay.expanding = true;
                mixparlay.curChoice = this;
                var elm = this, _status = $(elm).attr('status');
                _status = (_status == 0) ? 1 : 0;
                try { parent.BlockUI(); } catch (e) { }
                $(elm).attr('status', _status).toggleClass('load');
                window.setTimeout(function () { mixparlay.expandChoice(elm, null); }, 20);
                if (_status == 1 && $('#bettype_' + $(elm).attr('type')).is(':hidden')) {
                    $('#betrow_title_' + $(elm).attr('type')).find('div.title_label').find('span').removeClass('up');
                    $('#bettype_' + $(elm).attr('type')).show();
                }
            }
        });
    },
    deleteBet: function () {
        $('#ErrMsg').empty();
        $('#ErrorMessage').html('');
        $('#trMsg2').hide();

        if ($('#reason').val() == "") {
            $('#trMsg2').show();
            showAlertMessage("ErrorMessage", "Please enter reason", 1);
            $('#reason').focus();
            return false;
        }
        else if ($('#username').val() == "") {
            $('#trMsg2').show();
            showAlertMessage("ErrorMessage", "Please enter username", 1);
            $('#username').focus();
            return false;
        }
        else if ($('#userpwd').val() == "") {
            $('#trMsg2').show();
            showAlertMessage("ErrorMessage", "Please enter password", 1);
            $('#userpwd').focus();
            return false;
        }
        // valid message
        if (msg) { $.extend(msg, { IsSend: 0 }); }
        if ($('input#chk-send-email').is(':checked')) {
            msg.IsSend = 1;
            if ($('#men').val() == "") {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must enter English message", 1);
                $('#men').focus();
                return false;
            }
            else if ($('#men').val().indexOf('@@') == -1) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in  English message for replacing ticket number", 1);
                $('#men').focus();
                return false;
            }
            else if ($('#mch').val() == "") {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must enter Chinese message", 1);
                $('#mch').focus();
                return false;
            }
            else if ($('#mch').val().indexOf('@@') == -1) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in Chinese message for replacing ticket number", 1);
                $('#mch').focus();
                return false;
            }
            else if (($('#mcs').val() != "") && ($('#mcs').val().indexOf('@@') == -1)) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in Simple Chinese message for replacing ticket number", 1);
                $('#mcs').focus();
                return false;
            }
            else if (($('#mth').val() != "") && ($('#mth').val().indexOf('@@') == -1)) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in Thai message for replacing ticket number", 1);
                $('#mcs').focus();
                $('#mth').focus();
                return false;
            }
            else if (($('#mit').val() != "") && ($('#mit').val().indexOf('@@') == -1)) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in Italian message for replacing ticket number", 1);
                $('#mit').focus();
                return false;
            }
            else if (($('#mjp').val() != "") && ($('#mjp').val().indexOf('@@') == -1)) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in Japanese message for replacing ticket number", 1);
                $('#mjp').focus();
                return false;
            }
            else if (($('#mes').val() != "") && ($('#mes').val().indexOf('@@') == -1)) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in Spanish message for replacing ticket number", 1);
                $('#mes').focus();
                return false;
            }
            else if (($('#mko').val() != "") && ($('#mko').val().indexOf('@@') == -1)) {
                $('#trMsg2').show();
                showAlertMessage("ErrorMessage", "You must include @@ in Korean message for replacing ticket number", 1);
                $('#mko').focus();
                return false;
            }
        }
        try {

            var data_partner = { Normal: { opt: 0, ids: [] },
                Deposit: { opt: 0, ids: [] },
                BetDaq: { opt: 0, ids: [] },
                EU: { opt: 0, ids: [] }, // 12EU
                MHC: { opt: 0, ids: [] }
            };
            var sl_partner = $('select.sel-partner');

            $(sl_partner).each(function (ind, sel) {
                var _partner = $(sel).attr('partner'), _field = _partner.replace(/12/, ''), delOpt = $(sel).val();
                delOpt = (delOpt == 1) ? 2 : delOpt;
                eval("data_partner." + _field + ".opt=" + delOpt + ";");
                //if (delOpt == '2') {
                var bet_checks = $('input.chk' + _partner + '-1');
                $(bet_checks).each(function (i, chk) {
                    if ($(chk).is(':checked')) {
                        eval("data_partner." + _field + ".ids.push(" + $(chk).val() + ");");
                    }
                });
                //}
            });

            //alert(JSON.stringify(data_partner)); return;
            // set message content
            if (msg && msg.IsSend == 1) {
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
            }
            var bettypes = '', betteam430 = '', betteam6 = '', betteam14 = '', betteam16 = '';
            if (par) {
                bettypes = par.Bettypes || '';
                betteam430 = par.Betteam430 || '';
                betteam6 = par.Betteam6 || '';
                betteam14 = par.Betteam14 || '';
                betteam16 = par.Betteam16 || '';
            }
            var HomeScore, AwayScore, FromTransDate, ToTransDate, ParlayType = '0';
            HomeScore = $('#txtHomeScore').val();
            AwayScore = $('#txtAwayScore').val();
            FromTransDate = $('#txtFromdate').val();
            ToTransDate = $('#txtTodate').val();
            if ($('#chkMixparlay').is(':checked') && $('#chkSystemparlay').is(':checked')) {
                ParlayType = '0';
            }
            else if ($('#chkMixparlay').is(':checked') && !$('#chkSystemparlay').is(':checked')) {
                ParlayType = '9';
            }
            else if (!$('#chkMixparlay').is(':checked') && $('#chkSystemparlay').is(':checked')) {
                ParlayType = '29';
            }

            if ($('#ddlLiveIndicator').val() != '1') {
                HomeScore = '';
                AwayScore = '';
                FromTransDate = '';
                ToTransDate = '';
            }

            msg.Reason = $('#reason').val();

            var data = {
                'matchId': $('#matchid').val(),
                'Type': $('#ddlLiveIndicator').val(),
                'HomeScore': HomeScore,
                'AwayScore': AwayScore,
                'bettypes': bettypes,
                'betteam1378': $('#ddlBetTeam1378').val(),
                'betteam515': $('#ddlBetTeam515').val(),
                'hdp': ($('#hdp').val() == '' ? '9999' : $('#hdp').val()),
                'uname': $('#username').val(),
                'userId': $('#userId').val(),
                'pass': $('#userpwd').val(),
                'DelOption': data_partner.Normal.opt,
                'transids': data_partner.Normal.ids.join(','),
                'DelOption_D': data_partner.Deposit.opt,
                'transids_D': data_partner.Deposit.ids.join(','),
                'DelOption_BD': data_partner.BetDaq.opt,
                'transids_BD': data_partner.BetDaq.ids.join(','),
                'DelOption_12EU': data_partner.EU.opt,
                'transids_12EU': data_partner.EU.ids.join(','),
                'DelOption_MHC': data_partner.MHC.opt,
                'transids_MHC': data_partner.MHC.ids.join(','),
                'betteam430': betteam430,
                'betteam6': betteam6,
                'betteam14': betteam14,
                'betteam16': betteam16,
                'Fromtransdate': FromTransDate,
                'Totransdate': ToTransDate,
                'ParlayType': ParlayType,
                'message': msg                
            };

            $.ajax({
                type: "POST",
                url: "../Services/SpecialDeleteService.svc/DelMPList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    try { parent.BlockUI(); } catch (e) { }
                },
                success: function (resp) {
                    mixparlay.completeBet(resp);
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        } catch (e) {
            alert(e);
        }
    },
    completeBet: function (resp) {
        if (resp) {
            if (resp.ErrorCode == 0) {
                $('#box_list_cont').html(resp.ErrorMessage);

                mixparlay.validAction('complete');
            } else {
                $('#ErrMsg').html(resp.ErrorMessage);
                window.scrollTo(0, 0);
            }
        }
    },
    validAction: function (act) {
        if (act) {
            switch (act) {
                case 'list':
                    $('div.box_header.list').show().find('span').removeClass('up');
                    $('div#box_list_cont').show();
                    $('div#box_match_info').hide();
                    $('div.box_header.message').hide();
                    $('div#box_list_message').hide();
                    $('div.box_header.confirm').hide();
                    $('div#box_list_confirm').hide();
                    $('div.box_button.list').hide();
                    $('div.box_button.delete').hide();
                    $('#pagination').hide();
                    this.resetMessage();
                    break;
                case 'norow':
                    $('div.box_button.list').show();
                    $('div.box_button.delete').hide();
                    $('#pagination').hide();
                    break;
                case 'hasrow':
                    $('div.box_header.message').show().find('span').addClass('up'); ;
                    $('div#box_list_message').hide();
                    $('div.box_header.confirm').show();
                    $('div#box_list_confirm').show();
                    $('div.box_button.list').show();
                    $('div.box_button.delete').show();
                    break;
                case 'complete':
                    $('div#box_list_cont').show();
                    $('div#box_match_info').empty();
                    $('div.box_header.message').hide().find('span').removeClass('up'); ;
                    $('div#box_list_message').hide();
                    $('div.box_header.confirm').hide();
                    $('div#box_list_confirm').hide();
                    $('div.box_button.list').show();
                    $('div.box_button.delete').hide();
                    $('input#matchid').val('');
                    $('input.chk-bettype').attr('checked', false);
                    $('#pagination').hide();
                    this.resetMessage();
                    this.enableAllMode(false);
                    $('#txtHomeScore').val('');
                    $('#txtAwayScore').val('');
                    $('#ddlLiveIndicator').val('2');
                    $('td.type').hide();
                default:
                    break;
            }
        }
        return false;
    },
    resetMessage: function () {
        $('textarea').val('');
        $('#username').val('');
        $('#userpwd').val('');
        $('#chk-send-email').attr('checked', false);
        return false;
    },
    validMessage: function (chked) {
        if (chked && chked == true) {
            $('#span-msg-en').show();
            $('#span-msg-ch').show();
            $('div.box_header.message').show()
                .find('span').removeClass('up');
            $('div#box_list_message').slideDown();
        } else {
            $('#span-msg-en').hide();
            $('#span-msg-ch').hide();
            $('div.box_header.message').show()
                .find('span').addClass('up');
            $('div#box_list_message').slideUp();
        }
    },
    expanding: null,
    curChoice: null,
    choices: [],
    expandChoice: function (elm, refno) {
        var transids = [];
        if (elm) {
            var _type = $(elm).attr('type'), _status = $(elm).attr('status'), _chktrans = $('input[name="chk' + _type + '"]');
            if (_chktrans && _chktrans.length > 0) {
                if (_status == '1') {
                    for (var i = 0, n = _chktrans.length; i < n; i++) {
                        var _elm = _chktrans[i], _refno = $(_elm).attr('refno'), _data = $('span#zoom-0-' + _refno).attr('data');
                        if (_data == 0) {
                            transids.push(_refno);
                            if (transids.length == 100 || (i == (n - 1))) {
                                mixparlay.choices.push(transids.join('|'));
                                transids = [];
                            }
                        }
                        else {
                            $('#expand-cont-' + _refno).hide();
                            $('#expand-all-' + _refno).show();
                        }
                    }
                    mixparlay.runChoice();
                } else {
                    $('.expand-cont-' + _type).show();
                    $('.expand-all-' + _type).hide();
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                    if (mixparlay.curChoice) {
                        var elm = mixparlay.curChoice, _status = $(elm).attr('status');
                        var _text = $(elm).html(), _text = (_status == 0) ? _text.replace(/Collapse/, 'Expand') : _text.replace(/Expand/, 'Collapse');
                        $(elm).html(_text).toggleClass('load');
                    }
                    mixparlay.expanding = null;
                }
            }
        }
        if (refno) {
            mixparlay.choices.push(refno);
            mixparlay.runChoice();
        }
    },
    expandCallback: function (resp) {
        if ($.isArray(resp)) {
            $(resp).each(function (ind, ticket) {
                $('span#zoom-0-' + ticket.Key).removeClass('load').addClass('in').attr('data', 1);
                $('#expand-cont-' + ticket.Key).hide();
                $('#expand-all-' + ticket.Key).html(ticket.Value);
            });
            window.setTimeout(function () {
                $(resp).each(function (ind, ticket) {
                    $('div.matchmp.' + ticket.Key).hover(
                        function () {
                            $(this).addClass('cur');
                        },
                        function () {
                            $(this).removeClass('cur');
                        }
                    );
                });
                $('span.zoom.in').click(function (e) {
                    var _refno = $(this).attr('refno');
                    $('#expand-cont-' + _refno).slideUp('fast', function () {
                        $('#expand-all-' + _refno).slideDown('fast');
                    });
                });
                $('span.zoom.out').click(function (e) {
                    var _refno = $(this).attr('refno');
                    $('#expand-all-' + _refno).slideUp('fast', function () {
                        $('#expand-cont-' + _refno).slideDown('fast');
                    });
                });
            }, 1000);
        }
    },
    runChoice: function () {
        if (mixparlay.choices.length > 0) {
            var transids = mixparlay.choices.shift();
            $.each(transids.split('|'), function (ind, _refno) {
                $('span#zoom-0-' + _refno).removeClass('in').addClass('load');
            });
            var data = { transids: transids, matchid: $('#matchid').val() };
            window.clearTimeout(mixparlay.asyncTimeout);
            $.ajax({
                type: "POST",
                url: "../Services/SpecialDeleteService.svc/GetMPTicket",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    //TODO
                },
                success: function (resp) {
                    mixparlay.expandCallback(resp);
                    /*mixparlay.asyncTimeout = window.setTimeout(function () {
                    mixparlay.runChoice();
                    }, 500);*/
                    mixparlay.runChoice();
                },
                complete: function () {
                    //TODO
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        } else {
            window.clearTimeout(mixparlay.asyncTimeout);
            mixparlay.asyncTimeout = null;
            try { parent.jQuery.unblockUI(); } catch (e) { }
            if (mixparlay.curChoice) {
                var elm = mixparlay.curChoice, _status = $(elm).attr('status');
                var _text = $(elm).html(), _text = (_status == 0) ? _text.replace(/Collapse/, 'Expand') : _text.replace(/Expand/, 'Collapse');
                $(elm).html(_text).toggleClass('load');
            }
            mixparlay.expanding = null;
        }
    },
    asyncTimeout: null,
    ddlChange: function (site) {
        var ddlId = 'ddl' + site, chkCls = 'chk' + site;
        if ($('select#' + ddlId).length > 0) {
            var _val = $('select#' + ddlId).val();
            switch (_val.toString()) {
                case "0":
                    $('input.' + chkCls + '-1"').attr('disabled', 'disabled')
                        .attr('checked', false);
                    break;
                case "1":
                    $('input.' + chkCls + '-1').attr('disabled', 'disabled')
                        .attr('checked', true);
                    break;
                case "2":
                    $('input.' + chkCls + '-1').removeAttr('disabled')
                        .attr('checked', false);
                    break;
                default:
                    break;
            }
        }
    },
    LiveindicatorChange: function () {
        var type = $('#ddlLiveIndicator').val();
        switch (type) {
            case '1':
                $('.type').show();
                $('#txtFromdate').val('');
                $('#txtTodate').val('');
                $('#txtHomeScore').val('');
                $('#txtAwayScore').val('');
                $('#chkDateTime').attr("checked", false);
                break;
            default:
                $('.type').hide();
                break;
        }
    },
    eventOnDateTime: function () {
        var IsChecked = $('#chkDateTime').is(':checked');
        if (!IsChecked) {
            $('#txtFromdate').val('');
            $('#txtFromdate').attr('disabled', true);
            $('#txtTodate').val('');
            $('#txtTodate').attr('disabled', true);
        }
        else {
            CURDATE = new Date();
            $('#txtFromdate').val(CURDATE.format('m/d/Y H:i:s'));
            $('#txtFromdate').attr('disabled', false);
            $('#txtTodate').val(CURDATE.format('m/d/Y H:i:s'));
            $('#txtTodate').attr('disabled', false);
        }
    }
};
