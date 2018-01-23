var helper = {
    Validate: function (BetSettings, RoleId) {
        var c = BetSettings.length;
        $('#err').html('');
        var rs = true;
        for (var i = 0; i < c; i++) {
            var item = BetSettings[i];
            var subfix = '_' + item.SportId + '_' + item.BettypeId;
            var minBet = parseFloat($('#txtMinBet' + subfix).val(), true);
            var maxBet = parseFloat($('#txtMaxBet' + subfix).val(), true);
            var maxPerMatch = parseFloat($('#txtMaxPerMatch' + subfix).val(), true);
            var sportName = item.SportName;

            // Validate each sport type
            if (item != null) {

                // Check min bet < max bet < max per match
                if (minBet == 0 && maxBet == 0 && maxPerMatch == 0) { // all equal zero -> no limit, follow upline setting
                    continue;
                }

                if (minBet > maxBet) {
                    showAlertMessage("err", sportName + " - The Min Bet must be less than or equal to Max Bet", 1);
                    $('#txtMinBet' + subfix).focus();
                    return false;
                }
                if (minBet > maxPerMatch) {
                    showAlertMessage("err", sportName + " - The Min Bet must be less than or equal to Max Per Match", 1);
                    $('#txtMinBet' + subfix).focus();
                    return false;
                }
                if (maxBet > maxPerMatch) {
                    showAlertMessage("err", sportName + " - The Max Bet must be less than or equal to Max Per Match", 1);
                    $('#txtMaxBet' + subfix).focus();
                    return false;
                }

                //At member level: agent/admin cannot edit member’s min bet to 0 if his max bet/max pet match is greater than 0.
                if (RoleId == 1) {
                    // Check minbet < minbetmax
                    if (typeof (item.MinBetMax) != 'undefined' && parseFloat(item.MinBetMax) > 0 && minBet < parseFloat(item.MinBetMax)) {
                        showAlertMessage("err", sportName + " - Min Bet must be greater than or equal to " + ''.FormatNumber(item.MinBetMax), 1);
                        $('#txtMinBet' + subfix).focus();
                        return false;
                    }
                }

                // Check minbet < minbetmax
                if (typeof (item.MinBetMax) != 'undefined' && parseFloat(item.MinBetMax) > 0 && minBet < parseFloat(item.MinBetMax)) {
                    showAlertMessage("err", sportName + " - Min Bet must be greater than or equal to " + ''.FormatNumber(item.MinBetMax), 1);
                    $('#txtMinBet' + subfix).focus();
                    return false;
                }
                // Check maxbet < maxbetmin
                if (typeof (item.MaxBetMin) != 'undefined' && parseFloat(item.MaxBetMin) > 0 && maxBet > parseFloat(item.MaxBetMin)) {
                    showAlertMessage("err", sportName + " - Max Bet must be less than or equal to " + ''.FormatNumber(item.MaxBetMin), 1);
                    $('#txtMaxBet' + subfix).focus();
                    return false;
                }
                // Check maxpermatch_min < maxpermatch < maxpermatch_max
                if (typeof (item.MaxPerMatchMin) != 'undefined' && parseFloat(item.MaxPerMatchMin) > 0 && maxPerMatch > parseFloat(item.MaxPerMatchMin)) {
                    showAlertMessage("err", sportName + " - Max Per Match must be less than or equal to " + ''.FormatNumber(item.MaxPerMatchMin), 1);
                    $('#txtMaxPerMatch' + subfix).focus();
                    return false;
                }
            }

        }
        // Success
        return rs;
    }
};

var vsCustomer = {
    defaultSetting: {
        Enable: false,
        ApplyAllDownlines: false
    },
    currentSetting: {},
    isValidData: false,
    loadDefault: function () {
        $('#btnSubmit').attr('disabled', true);
        $('.BettingLimitTextBox').attr('disabled', true);

    },
    init: function () {
        vsCustomer.loadDefault();
    },
    search: function () {
        $('#err').html('');

        if (!vsCustomer.checkInputData())
            return;
        vsCustomer.customerSelStatus();

    },
    enableOnlick: function () {
        if (!vsCustomer.isValidData) {
            $('#btnSubmit').attr('disabled', true);
            return;
        }
        var checked = $('#chkEnable').attr('checked') ? true : false;               

        $('.BettingLimitTextBox').attr('disabled', !checked);
        $('#btnSubmit').attr('disabled', !checked);
    },
    checkInputData: function () {
        if ($('#txtUsername').val() == '') {
            showAlertMessage("err", "Please input Username.", 1);
            $('#txtUsername').focus();
            $('#btnSubmit').attr('disabled', true);
            return false;
        }

        return true;
    },
    customerSelStatus: function () {
        var param = {
            userName: $('#txtUsername').val()
        };

        $('#chkEnable').attr('checked', false);
        $('#chkApplyAllDownlines').attr('checked', false);

        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "POST",
            url: "CustomerSelStatusVS",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (typeof (rs.ErrorCode) == 'undefined') {
                    vsCustomer.defaultSetting = rs;
                    if (rs.RoleId == 1) {
                        $('#spApplyAllDownLines').hide();
                    }
                    else {
                        $('#spApplyAllDownLines').show();
                    }
                    $('#tblBettingLimit').html('');
                    $('#tblBettingLimit').html(vsCustomer.mappingBetSetting(rs.BetSettings));
                    vsCustomer.enableOnlick();
                }
                else {
                    vsCustomer.isValidData = false;
                    $('#chkEnable').attr('checked', false);
                    vsCustomer.enableOnlick();
                    $('#tblBettingLimit').html('');
                    showAlertMessage("err", rs.ErrorMessage, 1);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        , error: function (e) {
            $('.bg_table').addClass('hidden');
            try { parent.jQuery.unblockUI(); } catch (e) { }
            showAlertMessage("err", "Init Context Error", 1);
        }
        });
    },
    mappingBetSetting: function (BetSettings) {
        var html = '';
        var cnt = BetSettings.length;
        if (cnt == 0) {
            vsCustomer.isValidData = false;
            html += '<tr><td colspan="4"><span class="b" style="color:red">There is no information available.</span></td></tr>';
            return html;
        }

        html = '<tr class="title">\
        <td></td>\
        <td>Min Bet</td>\
        <td>Max Bet</td>\
        <td>Max Per Match</td></tr>';

        vsCustomer.isValidData = true;
        for (var i = 0; i < cnt; i++) {
            var item = BetSettings[i];
            var subfix = '_' + item.SportId + '_' + item.BettypeId;
            var classMinBetMax = '', classMaxBetMin = '', classMaxPerMatchMin = '';
            if (parseFloat(item.MinBetMax) == 0)
                classMinBetMax = 'hide';
            if (parseFloat(item.MaxBetMin) == 0)
                classMaxBetMin = 'hide';
            if (parseFloat(item.MaxPerMatchMin) == 0)
                classMaxPerMatchMin = 'hide';

            html += '<tr>\
            <td class="bgSports b l">' + item.SportName + '</td>\
            <td class="l"><input value="' + "".FormatNumber(item.MinBet) + '" maxlength="14" class="BettingLimitTextBox" type="text" id="txtMinBet' + subfix + '" onkeyup="return Fanex.FormatNumber(event, true)" /> <span class="pleft10 ' + classMinBetMax + '" id="spMinBetMax' + subfix + '">>=' + "".FormatNumber(item.MinBetMax) + '</span></td>\
            <td class="l"><input value="' + "".FormatNumber(item.MaxBet) + '" maxlength="14" class="BettingLimitTextBox" type="text" id="txtMaxBet' + subfix + '" onkeyup="return Fanex.FormatNumber(event, true)" /> <span class="pleft10 ' + classMaxBetMin + '" id="spMaxBetMin' + subfix + '"><=' + "".FormatNumber(item.MaxBetMin) + '</span></td>\
            <td class="l"><input value="' + "".FormatNumber(item.MaxPerMatch) + '" maxlength="14" class="BettingLimitTextBox" type="text" id="txtMaxPerMatch' + subfix + '" onkeyup="return Fanex.FormatNumber(event, true)" /> <span class="pleft10 ' + classMaxPerMatchMin + '" id="spMaxPerMatchMin' + subfix + '"><=' + "".FormatNumber(item.MaxPerMatchMin) + '</span></td></tr>\
            ';
        }
        return html;
    },
    save: function () {
        vsCustomer.currentSetting = vsCustomer.defaultSetting;

        vsCustomer.currentSetting.Enable = $('#chkEnable').attr('checked') ? true : false;
        vsCustomer.currentSetting.ApplyAllDownlines = $('#chkApplyAllDownlines').attr('checked') ? true : false;
        var cnt = vsCustomer.currentSetting.BetSettings.length;
        for (var i = 0; i < cnt; i++) {
            var item = vsCustomer.currentSetting.BetSettings[i];
            var subfix = '_' + item.SportId + '_' + item.BettypeId;
            vsCustomer.currentSetting.BetSettings[i].MinBet = parseFloat($('#txtMinBet' + subfix).val(), true);
            vsCustomer.currentSetting.BetSettings[i].MaxBet = parseFloat($('#txtMaxBet' + subfix).val(), true);
            vsCustomer.currentSetting.BetSettings[i].MaxPerMatch = parseFloat($('#txtMaxPerMatch' + subfix).val(), true);
        }
    },
    submit: function () {
        if (!vsCustomer.checkInputData())
            return;

        if (!vsCustomer.isValidData)
            return;

        var BetSettings = vsCustomer.defaultSetting.BetSettings;
        var RoleId = vsCustomer.defaultSetting.RoleId;
        var ok = helper.Validate(BetSettings, RoleId);

        if (ok) {
            vsCustomer.save();
            var param = {
                settings: vsCustomer.currentSetting
            };
            try { parent.BlockUI() } catch (e) { }
            $.ajax({
                type: "POST",
                url: "CustomerUpdStatusVS",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (result) {
                    showEnrichMessage('err', result);
                    vsCustomer.afterUpdate(result);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
                    , error: function (e) {
                        $('.bg_table').addClass('hidden');
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        showAlertMessage("err", "Error", 1);
                    }
            });
        }
    },
    afterUpdate: function (errs) {
        var ok = true;
        for (var i = 0; i < errs.length; i++) {
            if (errs[i].ErrorCode > 0) {
                ok = false;
                break;
            }
        }
        if (ok) {
            var t = setTimeout(function () {
                vsCustomer.init();
            }, 3000);
        }
    }
};



$(document).ready(function () {
    vsCustomer.init();
});
