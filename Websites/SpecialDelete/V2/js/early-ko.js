var CURDATE = new Date();
var earlyko = {
    initForm: function () {
        var data = { type: 2 };
        $.ajax({
            type: "POST",
            url: "../Services/SpecialDeleteService.svc/GetBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            timeout: 5000,
            success: function (resp) {
                if (resp) {
                    var resp = { Rows: resp, Count: resp.length };
                    $('#betform_tpl').tmpl(resp).appendTo($('#box_setting_form'));
                    window.setTimeout(function () {
                        // event bindding
                        earlyko.binddingEvent();
                    }, 500);
                }
            },
            error: function (error, status, xhr) {
                //alert(status);
            }
        });
    },
    binddingEvent: function () {
        // setup starting time
        SetupCalendar(CURDATE.format('m/d/Y H:i:s'), document.getElementById('txtdate'));
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
            earlyko.showBet();
        });
        $('input.btn.delbet').bind('click', function (e) {
            earlyko.deleteBet();
        });
        // bettype
        this.bindingBettype();
        //advance mode
        this.bindingAdvance();
    },
    bindingBettype: function () {
        $('input.chk-bettype').bind('click', function (e) {
            var _chked = $(this).attr('checked'), _isall = false;
            if ($(this).attr('id').indexOf('all') != -1) {
                _isall = true;
            }
            earlyko.validChkBettype($(this).val(), _isall, _chked);
        });
        $('span.check.bettype').bind('click', function (e) {
            var _rel = $(this).attr('rel'), _chked = $('input#chk-bettype-' + _rel).attr('checked');
            var _isall = false;
            $('input#chk-bettype-' + _rel).attr('checked', !_chked);
            if (_rel.indexOf('all') != -1) {
                _isall = true;
            }
            earlyko.validChkBettype($('input#chk-bettype-' + _rel).val(), _isall, !_chked);
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
        earlyko.highlightMode(val, chked, isall);
    },
    highlightMode: function (val, chked, isall) {
        //alert('isall:' + isall);
        if (isall) {
            earlyko.enableAllMode(chked);
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
            $('#chkAllScores').removeAttr('disabled');
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
            $('#chkAllScores').attr('disabled', 'disabled');
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
        $('#chkAllScores').attr('checked', chked);
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
    showBet: function () {

        try {
            $('#ErrMsg').empty();
            $('#spErrCS').html('');
            $('#dvContent').html('');

            //--------------
            par.MatchId = $('#matchid').val();
            if (!isNumberNotNull('matchid') && parseInt(par.MatchId) > 2147483647) {
                showAlertMessage("ErrMsg", "Please enter a valid Match Id (number) !", 1);
                $('#matchid').focus();
                return false;
            }
            if (parseInt(par.MatchId) > 2147483647) {
                showAlertMessage("ErrMsg", "Please enter a valid Match Id (number) !", 1);
                $('#matchid').focus();
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

            //par.MatchId = $('#matchid').val();
            par.FromRefNo = '0';
            par.ToRefNo = '999999999999';
            par.Hdp = $('#hdp').val().length == 0 ? '9999' : $('#hdp').val();
            par.Liveindicator = '0';
            par.Bettypes = $('#bettype').val();
            par.Betteam1378 = $('#ddlBetTeam1378').val();
            par.Betteam515 = $('#ddlBetTeam515').val();
            par.Betteam430 = scores;
            par.KichOffDate = $('#txtdate').val();
            par.Betteam6 = totalgoals;
            par.Betteam14 = flgoals;
            par.Betteam16 = htfts;
            //par.SAFilter = $('#ddlSAFilter').val() == 'All' ? '2' : $('#ddlSAFilter').val();
            par.SAFilter = $('#ddlSAFilter').val();

            var data = {
                'param': par
            };
            earlyko.validAction('list');
            $.ajax({
                type: "POST",
                url: "../Services/SpecialDeleteService.svc/GetEarlyKOBetList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    try { parent.BlockUI(); } catch (e) { }
                    $('#box_list_cont').html('<p align="center">Loading...</p>');
                },
                success: function (rs) {
                    if (rs) {
                        var bettypes = rs.Items;

                        if (parseInt(rs.ErrorCode) > 0) {
                            showAlertMessage("ErrMsg", rs.ErrorMessage, 1);
                            earlyko.validAction('norow');
                            $('#box_list_cont').empty().html('<p align="center">There is no information available</p>');
                        }
                        else {
                            $('#box_match_info').empty().show();
                            $('#row_match').tmpl(rs).appendTo($('#box_match_info'));
                            var no_row = true;
                            $.each(bettypes, function (ind, bettype) {
                                var rel = ind;
                                if (bettype.KoItems.length > 0) {
                                    no_row = false;
                                }
                            });
                            $('#box_list_cont').empty().show();
                            $('#row_bet_tpl').tmpl({ NoRow: no_row }).appendTo($('#box_list_cont'));
                            for (var b = 0; b < bettypes.length; b++) {
                                var bettype = bettypes[b];
                                if (bettype.KoItems.length > 0) {
                                    $('#item_bettype_tpl').tmpl(bettype).appendTo($('#box_list_scroll'));
                                    for (var i = 0; i < bettype.KoItems.length; i++) {
                                        var _item = bettype.KoItems[i];
                                        $('#item_betrans_tpl').tmpl(_item, {
                                            data: _item,
                                            bettype: { IsAllowOptionDelete: bettype.IsAllowOptionDelete, OptionType: bettype.OptionType }
                                        }).appendTo($('#bettype_' + bettype.Type));
                                    }
                                }
                            }
                            window.setTimeout(function () {
                                if (no_row) {
                                    earlyko.validAction('norow');
                                    $('#box_list_cont').empty().html('<p align="center">There is no information available</p>');
                                } else {
                                    earlyko.validAction('hasrow');
                                    // row title
                                    $('div.title_label').bind('click', function (e) {
                                        var ids = $(this).attr('rel').toString().split(':');
                                        for (i = 0; i < ids.length; i++) {
                                            $('div#' + ids[i]).slideToggle();
                                        }
                                        $(this).find('span').toggleClass('up');
                                    });
                                    // row line
                                    $('div.betrow.line').bind('mouseover', function (e) {
                                        $(this).addClass('cur');
                                    });
                                    $('div.betrow.line').bind('mouseout', function (e) {
                                        $(this).removeClass('cur');
                                    });
                                    $('a.mixparlay').bind('click', function (e) {
                                        earlyko.showMixparlay($(this).attr('ref').toString());
                                    });
                                }
                            }, 500);
                        }
                    } else {
                        earlyko.validAction('norow');
                        $('#box_list_cont').empty().html('<p align="center">There is no information available</p>').show();
                    }
                },
                error: function () {

                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
            $('#username').val(AccUserName);
        } catch (e) {
            //alert(e); 
        }
    },
    deleteBet: function () {
        $('#ErrMsg').empty();
        $('#ErrorMessage').html('');
        $('#trMsg2').hide();

        if ($('#reason').val() == "") {
            $('#trMsg2').show();
            showAlertMessage("ErrorMessage", "Please enter reason", 1);
            //$('#ErrorMessage').html("*Please enter username");
            $('#reason').focus();
            return false;
        }
        else if ($('#username').val() == "") {
            $('#trMsg2').show();
            showAlertMessage("ErrorMessage", "Please enter username", 1);
            //$('#ErrorMessage').html("*Please enter username");
            $('#username').focus();
            return false;
        }
        else if ($('#userpwd').val() == "") {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*Please enter password");
            showAlertMessage("ErrorMessage", "Please enter password", 1);
            $('#userpwd').focus();
            return false;
        }
        else if ($('#men').val() == "") {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must enter English message");
            showAlertMessage("ErrorMessage", "You must enter English message", 1);
            $('#men').focus();
            return false;
        }
        else if ($('#men').val().indexOf('@@') == -1) {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must include @@ in  English message for replacing ticket number");
            showAlertMessage("ErrorMessage", "You must include @@ in  English message for replacing ticket number", 1);
            $('#men').focus();
            return false;
        }
        else if ($('#mch').val() == "") {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must enter Chinese message");
            showAlertMessage("ErrorMessage", "You must enter Chinese message", 1);
            $('#mch').focus();
            return false;
        }
        else if ($('#mch').val().indexOf('@@') == -1) {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must include @@ in Chinese message for replacing ticket number");
            showAlertMessage("ErrorMessage", "You must include @@ in Chinese message for replacing ticket number", 1);
            $('#mch').focus();
            return false;
        }
        else if (($('#mcs').val() != "") && ($('#mcs').val().indexOf('@@') == -1)) {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must include @@ in Simple Chinese message for replacing ticket number");
            showAlertMessage("ErrorMessage", "You must include @@ in Simple Chinese message for replacing ticket number", 1);
            $('#mcs').focus();
            return false;
        }
        else if (($('#mth').val() != "") && ($('#mth').val().indexOf('@@') == -1)) {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must include @@ in Thai message for replacing ticket number");
            showAlertMessage("ErrorMessage", "You must include @@ in Thai message for replacing ticket number", 1);
            $('#mcs').focus();
            $('#mth').focus();
            return false;
        }
        else if (($('#mit').val() != "") && ($('#mit').val().indexOf('@@') == -1)) {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must include @@ in Italian message for replacing ticket number");
            showAlertMessage("ErrorMessage", "You must include @@ in Italian message for replacing ticket number", 1);
            $('#mit').focus();
            return false;
        }
        else if (($('#mjp').val() != "") && ($('#mjp').val().indexOf('@@') == -1)) {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must include @@ in Japanese message for replacing ticket number");
            showAlertMessage("ErrorMessage", "You must include @@ in Japanese message for replacing ticket number", 1);
            $('#mjp').focus();
            return false;
        }
        else if (($('#mes').val() != "") && ($('#mes').val().indexOf('@@') == -1)) {
            $('#trMsg2').show();
            //$('#ErrorMessage').html("*You must include @@ in Spanish message for replacing ticket number");
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

        try {
            var data_partner = { Deposit: { opt: 0, ids: [] },
                BetDaq: { opt: 0, ids: [] },
                EU: { opt: 0, ids: [] }, //12EU
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
            //alert(JSON.stringify(data_partner)); return;
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
            msg.Reason = $('#reason').val();

            var bettypes = '', betteam430 = '', betteam6 = '', betteam14 = '', betteam16 = '';
            if (typeof (par) !== undefined) {
                bettypes = par.Bettypes || '';
                betteam430 = par.Betteam430 || '';
                betteam6 = par.Betteam6 || '';
                betteam14 = par.Betteam14 || '';
                betteam16 = par.Betteam16 || '';
            }

            //            var data = {
            //                'matchId': $('#matchid').val(),
            //                'bettype': bettypes,
            //                'betteam1378': $('#ddlBetTeam1378').val(),
            //                'betteam515': $('#ddlBetTeam515').val(),
            //                'hdp': ($('#hdp').val() == '' ? '9999' : $('#hdp').val()),
            //                'uname': $('#username').val(),
            //                'userId': $('#userId').val(),
            //                'pass': $('#userpwd').val(),
            //                'DelOption': data_partner.Deposit.opt,
            //                'transids': data_partner.Deposit.ids.join(','),
            //                'kickoffdate': $.trim($('#txtdate').val()),
            //                'DelOption_BD': data_partner.BetDaq.opt,
            //                'transids_BD': data_partner.BetDaq.ids.join(','),
            //                'DelOption_12EU': data_partner.EU.opt,
            //                'transids_12EU': data_partner.EU.ids.join(','),
            //                'DelOption_MHC': data_partner.MHC.opt,
            //                'transids_MHC': data_partner.MHC.ids.join(','),
            //                'betteam430': betteam430,
            //                'betteam6': betteam6,
            //                'betteam14': betteam14,
            //                'betteam16': betteam16,
            //                'message': msg,
            //                'SAFilter': par.SAFilter
            //            };
            var data = {
                'matchId': par.MatchId,
                'bettype': par.Bettypes,
                'betteam1378': par.Betteam1378,
                'betteam515': par.Betteam515,
                'hdp': par.Hdp,
                'uname': $('#username').val(),
                'userId': $('#userId').val(),
                'pass': $('#userpwd').val(),
                'DelOption': data_partner.Deposit.opt,
                'transids': data_partner.Deposit.ids.join(','),
                'kickoffdate': par.KichOffDate,
                'DelOption_BD': data_partner.BetDaq.opt,
                'transids_BD': data_partner.BetDaq.ids.join(','),
                'DelOption_12EU': data_partner.EU.opt,
                'transids_12EU': data_partner.EU.ids.join(','),
                'DelOption_MHC': data_partner.MHC.opt,
                'transids_MHC': data_partner.MHC.ids.join(','),
                'betteam430': par.Betteam430,
                'betteam6': par.Betteam6,
                'betteam14': par.Betteam14,
                'betteam16': par.Betteam16,
                'message': msg,
                'SAFilter': par.SAFilter
            };

            $.ajax({
                type: "POST",
                url: "../Services/SpecialDeleteService.svc/DeleteSpecialEarlyKO",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    try { parent.BlockUI(); } catch (e) { }
                },
                success: function (resp) {
                    earlyko.completeBet(resp);
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        } catch (e) {
            //alert(e);
        }
    },
    completeBet: function (resp) {
        if (resp) {
            if (resp.ErrorCode == 0) {
                $('#box_list_cont').html(resp.ErrorMessage);
                earlyko.validAction('complete');
            } else {
                $('#ErrMsg').html(resp.ErrorMessage);
                window.scrollTo(0, 0);
            }
        }
    },
    showMixparlay: function (refno) {
        var info = refno.split('-'), data = { refno: info[0], transid: info[3], currency: info[2], subtransid: info[1] };
        var mTitle = info[4] == '29' ? 'System Parlay' : 'Mix Parlay';
        var opts = { title: mTitle + info[3], width: 700, height: 500 };
        if ($('#popup_popup').length == 0) {
            $('<div />').attr('id', 'popup_shadow').addClass('popup_shadow').appendTo('body').show();
            $('<div />').attr('id', 'popup_popup').addClass('popup_popup')
                .css({ width: 760 })
                .appendTo('body');
        } else {
            $('#popup_shadow').show();
            $('#popup_popup').empty();
        }
        $('<div />').attr('id', 'popup_setting').addClass('popup_setting').appendTo('#popup_popup');
        $('<div />').attr('id', 'popup_title')
                 .addClass('popup_title')
                 .append('<div class="title">' + opts.title + '</div><div class="close" title="Close">&nbsp;</div>')
                 .appendTo('#popup_popup');
        $('<div />').attr('id', 'popup_loading').addClass('popup_loading')
                 .appendTo('#popup_popup').html('<p align="center">Loading...</p>').show();
        $('<div />').attr('id', 'popup_content').addClass('popup_content')
                 .css({ width: 749 })
                 .appendTo('#popup_popup').hide();
        $('#popup_popup').css("left", (($(window).width() / 2) - (750 / 2)));
        $('#popup_popup').show();

        window.setTimeout(function () {
            $('#popup_popup').draggable({
                handle: '#popup_title'
            });
            $('div.close').bind('click', function (e) {
                earlyko.closePopup();
            });

        }, 500);

        $.ajax({
            type: "POST",
            url: "../Services/SpecialDeleteService.svc/GetEarlyKOMixparlay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (resp) {
                $('#popup_loading').hide();
                if (resp) {
                    $.extend(resp, {
                        Account: $('span#account-' + resp.TransId).html(),
                        TransTime: $('span#transtime-' + resp.TransId).html(),
                        Stake: $('span#amount-' + resp.TransId).html(),
                        Currency: $('span#currency-' + resp.TransId).html()
                    });
                    $('#popup_content').append($('#row_bet_mixparlay').tmpl(resp)).slideDown();
                } else {
                    $('#popup_content').html('<p align="center">There is no information available</p>');
                }
            }
        });
    },
    closePopup: function () {
        $('#popup_shadow').hide();
        $('#popup_popup').fadeOut();
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
                    $('div.box_button.list').hide();
                    $('div.box_button.delete').hide();
                    this.resetMessage();
                    break;
                case 'norow':
                    $('div.box_button.list').show();
                    $('div.box_button.delete').hide();
                    break;
                case 'hasrow':
                    $('div.box_header.message').show().find('span').removeClass('up');
                    $('div#box_list_message').show();
                    $('div.box_button.list').show();
                    $('div.box_button.delete').show();
                    break;
                case 'complete':
                    $('div#box_list_cont').show();
                    $('div#box_match_info').empty();
                    $('div.box_header.message').hide();
                    $('div#box_list_message').hide();
                    $('div.box_button.list').show();
                    $('div.box_button.delete').hide();
                    $('input#matchid').val('');
                    $('input.chk-bettype').attr('checked', false);
                    $('ddlSAFilter').val('All');
                    $('#Home').hide();
                    $('#Away').hide();
                    $('#txtDateTime').hide();
                    $('#chbDateTime').hide();
                    $('#txtFilterScore').hide();
                    SetupCalendar(CURDATE.format('m/d/Y H:i:s'), document.getElementById('txtdate'));
                    this.resetMessage();
                    this.enableAllMode(false);
                default:
                    break;
            }
        }
        return false;
    },
    genCheckOpt: function (row, bettype) {
        try {
            if (bettype) {
                var OptionType = bettype.OptionType, optional = '';
                if (bettype.IsAllowOptionDelete) {
                    optional = '<input type="checkbox" class="chk' + row.Type + '-' + row.IsDelete + '" id="' + row.TransId + '" ' + ((OptionType != '2') ? 'disabled="disabled"' : '') + ' ' + ((OptionType == '1' && row.IsDelete != '0') ? 'checked="true"' : '') + ' value="' + row.SubTransId + '"/>';
                } else {
                    if (row.IsDelete == '1') {
                        optional = '<input type="hidden" class="chk' + row.Type + '-' + row.IsDelete + '" id="' + row.TransId + '" value="' + row.SubTransId + '"/>';
                    }
                }
                return optional;
            }
        }
        catch (e) { }
        return '';
    },
    genBetTitle: function (index, bettype) {
        var tpl_title = '', title = '';
        title = bettype.Type.toString();
        //if (index % 2 == 0) {
        tpl_title = '<div class="title_label" style="float:left; width: 850px;cursor: pointer;" rel="bettype_' + (index) + '"><span>&nbsp;</span>' + title + '</div>';
        //}
        return tpl_title;
    },
    resetMessage: function () {
        $('textarea').val('');
        $('#username').val('');
        $('#userpwd').val('');
        return false;
    },
    ddlChange: function (site) {
        var ddlId = 'ddl' + site, chkCls = 'chk' + site;
        if ($('select#' + ddlId).length > 0) {
            var _val = $('select#' + ddlId).val();
            switch (_val.toString()) {
                case "0":
                    $('input.' + chkCls + '-1').attr('disabled', 'disabled')
                        .attr('checked', false);
                    break;
                case "1":
                    $('input.' + chkCls + '-1').attr('disabled', 'disabled')
                        .attr('checked', true);
                    break;
                case "2":
                    $('input.' + chkCls + '-1').attr('disabled', false)
                        .attr('checked', false);
                    break;
                default:
                    break;
            }
        }
    }
};
