

var specialDeleteUtils = {
    validMessage: '',
    oldMatch: '',
    toggleDiv: function (e, toogleBtn) {
        var ids = toogleBtn.attr('rel').toString().split(':');
        toogleBtn.find('div').toggleClass('up');
        for (i = 0; i < ids.length; i++) {
            $('div#' + ids[i]).slideToggle();
        }
    },
    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },
    unblockUI: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    },
    validRefno: function () {
        try {
            var matchid = '';
            var fromRefno = '';
            var toRefno = '';
            matchid = $('#matchid').val().trim();
            if (matchid.length > 0 && !isNumberNotNull('matchid')) {
                showAlertMessage("ErrorMessage", "Please enter a valid Match Id (number)!", 1);
                $('#matchid').focus();
                return false;
            }

            fromRefno = $('#frefno').val().trim();
            if (fromRefno.length > 0 && !isNumberNotNull('frefno')) {
                showAlertMessage("ErrorMessage", "Please enter a valid From Ref Number!", 1);
                $('#frefno').focus();
                return false;
            }

            toRefno = $('#trefno').val().trim();
            if (toRefno.length > 0 && !isNumberNotNull('trefno')) {
                showAlertMessage("ErrorMessage", "Please enter a valid To Ref Number!", 1);
                $('#trefno').focus();
                return false;
            }

            var newMatched = ''.concat(matchid).concat(fromRefno).concat(toRefno);
            if (matchid.length > 0 && fromRefno.length > 0 &&
                toRefno.length > 0 && newMatched != specialDeleteUtils.oldMatch) {
                specialDeleteUtils.oldMatch = newMatched;
                var data = {
                    matchid: matchid,
                    fromRefno: fromRefno,
                    toRefno: toRefno
                };
                specialDeleteUtils.validMessage = '';
                $.ajax({
                    type: "POST",
                    url: "../SpecialDelete/CheckRefNo",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    success: function (resp) {
                        if (resp && resp.length > 0) {
                            $(resp).each(function (i, item) {
                                specialDeleteUtils.validMessage += item.ErrorMessage;
                            });
                        }
                        if (specialDeleteUtils.validMessage != '') {
                            showAlertMessage("ErrorMessage", specialDeleteUtils.validMessage, 1);
                            $('#ErrorMessage').removeClass("hidden");
                        } else {
                            $('#ErrorMessage').empty().addClass("hidden");
                        }
                    }
                });
            }
        }
        catch (e) {
            showAlertMessage("ErrorMessage", e, 1);
            $('#ErrorMessage').removeClass("hidden");
        }
        return false;
    },
    validAction: function (act) {
        if (act) {
            switch (act) {
                case 'list':
                    $("#bg_table").css("width", "1063px");
                    $("#box_betlist_info").css("width", "1063px");
                    $(".maincontent_box").css("width", "1054px");
                    $('div.box_header.list').removeClass("hidden");
                    $('div.box_header.list').find('div').removeClass('up');
                    $('div#box_list_cont').removeClass("hidden");
                    $('div#box_match_info').addClass("hidden");
                    if (isShowOtherLangue) {
                        hideLanguage($("#other_display"));
                    }
                    $('div.box_header.message').addClass("hidden");
                    $('div#box_list_message').addClass("hidden");
                    $('div.box_header.confirm').addClass("hidden");
                    $('div#box_list_confirm').addClass("hidden");
                    $('div.box_button.delete').addClass("hidden");
                    $('div.box_button.list').addClass("hidden");
                    specialDeleteUtils.resetMessage();
                    break;
                case 'norow':
                    $('div.box_button.list').removeClass("hidden");
                    $('div.box_button.delete').addClass("hidden");
                    $('#pagination').empty();
                    $('#pagination').addClass("hidden");
                    break;
                case 'hasrow':
                    $('div.box_button.list').removeClass("hidden");
                    $('div.box_header.message').removeClass("hidden");
                    $('div.box_header.message').find('div').removeClass('up');
                    $('div#box_list_message').removeClass("hidden");
                    $('div.box_header.confirm').removeClass("hidden");
                    $('div#box_list_confirm').removeClass("hidden");
                    $('div.box_button.delete').removeClass("hidden");
                    break;
                case 'complete':
                    $("#box_betlist_info").css("width", "910px");
                    $("#bg_table").css("width", "910px");
                    $(".maincontent_box").css("width", "910px");
                    $('div#box_match_info').empty();
                    $('div#box_list_cont').removeClass("hidden");
                    $('div.box_header.list').addClass("hidden");
                    $('div.box_button.list').removeClass("hidden");
                    $('div#box_list_message').addClass("hidden");
                    $('div.box_header.message').addClass("hidden");
                    $('div.box_header.message').find('div').removeClass('up');
                    $('div.box_header.confirm').addClass("hidden");
                    $('div.box_button.delete').addClass("hidden");
                    $('input#matchid').val('');
                    $('#frefno').val('');
                    $('#trefno').val('');
                    $('#ddlLiveIndicator').val('All');
                    $('input.chk-bettype').attr('checked', false);
                    $('#ddlSAFilter').val('All');
                    $('#txtHomeScore').val('');
                    $('#txtAwayScore').val('');
                    $('#chkDateTime').attr('checked', false);
                    $('#txtFromdate').val('');
                    $('#txtTodate').val('');
                    $('#Home').addClass("hidden");
                    $('#Away').addClass("hidden");
                    $('#txtFromDate').addClass("hidden");
                    $('#txtToDate').addClass("hidden");
                    $('#chbDateTime').addClass("hidden");
                    $('#txtFilterScore').addClass("hidden");
                    specialDeleteUtils.resetMessage();
                    specialDeleteEventBinding.enableAllMode(false);
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

        $("#tdErrorMessage").addClass("hidden");
        $('#ErrorMessageDeleteBets').empty();
        return false;
    },
    genCheckOpt: function (row, bettype) {
        try {
            if (bettype) {
                var OptionType = bettype.OptionType, optional = '';
                if (bettype.IsAllowOptionDelete) {
                    optional = '<input type="checkbox" class="chk' + row.Type + '-' + row.IsDelete + '" id="' + row.Refno + '" ' + ((OptionType != '2') ? 'disabled="disabled"' : '') + ' ' + ((OptionType == '1' && row.IsDelete != '0') ? 'checked="true"' : '') + ' value="' + row.Refno + '"/>';
                } else {
                    if (row.IsDelete == '1') {
                        optional = '<input type="hidden" class="chk' + row.Type + '-' + row.IsDelete + '" id="' + row.Refno + '" value="' + row.Refno + '"/>';
                    }
                }
                return optional;
            }
        }
        catch (e) { }
        return '';
    },
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
    showOtherLanguage: function (obj) {
        hideLanguage(obj);
    },
    completeBet: function (resp) {
        if (resp) {
            if (resp.ErrorCode == 0) {
                $("#tdErrorMessage").addClass("hidden");
                $('#ErrorMessageDeleteBets').empty();
                $('#box_list_cont').empty();
                if (resp.ErrorMessage != '') {
                    showAlertMessage("ErrorMessage", resp.ErrorMessage, 0);
                    $('#ErrorMessage').removeClass("hidden");
                }
                specialDeleteUtils.validAction('complete');
            } else if (resp.ErrorMessage != '') {
                showAlertMessage("ErrorMessage", resp.ErrorMessage, 1);
                $('#ErrorMessage').removeClass("hidden");
                $("#ErrorMessage").focus();
                window.scrollTo(0, 0);
            }
        }
    },
    checkedBet: function (bets) {
        if (bets) {
            var arr_b = bets.split(',');
            for (i = 0; i < arr_b.length; i++) {
                if ($('#chk-bettype-' + arr_b[i]).attr('checked')) {
                    return true;
                }
            }
        }
        return false;
    }
}