


var specialDeleteEventBinding = {
    binddingEvent: function () {
        $('div.box_header').bind('click', function (e) {
            specialDeleteUtils.toggleDiv(e, $(this));
        });
        $('div.advance-header').bind('click', function (e) {
            specialDeleteUtils.toggleDiv(e, $(this));
        });

        $("#matchid").ForceDecimalOnly();
        $("#frefno").ForceDecimalOnly();
        $("#trefno").ForceDecimalOnly();
        // valid refno
        $('input.#matchid').blur(function (e) {
            specialDeleteUtils.validRefno();
        }).bind('paste', function (e) {
            window.setTimeout(function () {
                specialDeleteUtils.validRefno();
            }, 100);
        });
        $('input.#frefno').blur(function (e) {
            specialDeleteUtils.validRefno();
        }).bind('paste', function (e) {
            window.setTimeout(function () {
                specialDeleteUtils.validRefno();
            }, 100);
        });
        $('input.#trefno').blur(function (e) {
            specialDeleteUtils.validRefno();
        }).bind('paste', function (e) {
            window.setTimeout(function () {
                specialDeleteUtils.validRefno();
            }, 100);
        });

        // show bet
        $('input.btn.showbet').bind('click', function (e) {
            specialDeleteVirtualSport.showBet();
        });

        $('input.btn.delbet').bind('click', function (e) {
            specialDeleteVirtualSport.deleteBet();
        });

        // bettype
        specialDeleteEventBinding.bindingBettypeEvent();
        //advance mode
        specialDeleteEventBinding.bindingAdvanceBettypeEvent();
    },
    bindingBettypeEvent: function () {
        $('input.chk-bettype').bind('click', function (e) {
            var _chked = $(this).attr('checked');
            if (_chked == null) {
                _chked = false;
            }
            var _isall = false;
            if ($(this).attr('id').indexOf('all') != -1) {
                _isall = true;
            }
            specialDeleteEventBinding.validChkBettype($(this).val(), _isall, _chked, e);
        });
        $('div.check.bettype').bind('click', function (e) {
            var _rel = $(this).attr('rel');
            var _chked = $('input#chk-bettype-' + _rel).attr('checked');
            var _isall = false;
            $('input#chk-bettype-' + _rel).attr('checked', !_chked);
            if (_rel.indexOf('all') != -1) {
                _isall = true;
            }
            specialDeleteEventBinding.validChkBettype($('input#chk-bettype-' + _rel).val(), _isall, !_chked, e);
        });
    },
    bindingAdvanceBettypeEvent: function () {
        $('input.chk-score').bind('click', function (e) {
            var _chked = ($(this).attr('checked') != null ? $(this).attr('checked') : false);
            var _isall = true;
            if ($(this).attr('id').indexOf('all') != -1) {
                $('input.chk-score').attr('checked', _chked);
            } else {
                $.each($('input.chk-score'), function (ind, elm) {
                    if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                });
                $('#chk-score-all').attr('checked', _isall);
            }
        });
        $('div.check.score').bind('click', function (e) {
            var _rel = $(this).attr('rel');
            if ($('input#chk-score-' + _rel).attr('disabled') == null ||
                $('input#chk-score-' + _rel).attr('disabled') == false) {
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
            var _chked = ($(this).attr('checked') != null ? $(this).attr('checked') : false);
            var _isall = true;
            if ($(this).attr('id').indexOf('all') != -1) {
                $('input.chk-tg').attr('checked', _chked);
            } else {
                $.each($('input.chk-tg'), function (ind, elm) {
                    if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) { _isall = false; }
                });
                $('#chk-tg-all').attr('checked', _isall);
            }
        });
        $('div.check.tg').bind('click', function (e) {
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
    validChkBettype: function (val, isall, chked, e) {
        if (isall) {
            $('input.chk-bettype').attr('checked', chked);
            specialDeleteEventBinding.showAdvanceBox(e);
        } else {
            var _chkall = true;
            $.each($('input.chk-bettype'), function (ind, elm) {
                if ($(elm).attr('id').indexOf('all') == -1 && !$(elm).attr('checked')) {
                    _chkall = false;
                }
            });
            $('input#chk-bettype-all').attr('checked', _chkall);
        }
        specialDeleteEventBinding.highlightMode(val, chked, isall, e);
    },
    highlightMode: function (val, chked, isall, e) {
        if (isall) {
            specialDeleteEventBinding.enableAllMode(chked);
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
                case "1201":
                case "1203":
                case "1207":
                case "1208":
                    mode = 'hdp';
                    break;
                case "1204":
                case "1230":
                    mode = 'score';
                    is430 = chked;
                    break;
                case "1202": //Odd/Even, 1st Odd/Even
                case "1212":
                    mode = 'hdp';
                    break;
                case "1205":
                case "1215":
                    mode = 'ft';
                    is515 = chked;
                    break;
                case "1206":
                    mode = 'tg';
                    is6 = chked;
                    break;
                case "1214":
                    mode = 'flg';
                    is14 = chked;
                    break;
                case "1216":
                    mode = 'htft';
                    is16 = chked;
                    break;
                default:
                    break;
            }
            is1378 = specialDeleteUtils.checkedBet('1201,1203,1207,1208');
            is212 = specialDeleteUtils.checkedBet('1202,1212');
            is430 = specialDeleteUtils.checkedBet('1204,1230');
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
            is515 = specialDeleteUtils.checkedBet('1205,1215');
            if (is515) {
                $('#ddlBetTeam515').removeAttr('disabled');
            } else {
                $('#ddlBetTeam515').attr('disabled', 'disabled').val('All');
            }

            if (mode.indexOf('score') != -1) {
                is4_30[0] = $('#chk-bettype-1204').is(':checked') ? 1 : 0;
                is4_30[1] = $('#chk-bettype-1230').is(':checked') ? 1 : 0;
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
            if (is6 != null && (is6 == true || is6 == 'checked')) {
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
                specialDeleteEventBinding.showAdvanceBox(e);
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
    showAdvanceBox: function (e) {
        if ($('div#advance-cont').is(":hidden")) {
            specialDeleteUtils.toggleDiv(e, $('div#advance-header'));
        }
    }
}