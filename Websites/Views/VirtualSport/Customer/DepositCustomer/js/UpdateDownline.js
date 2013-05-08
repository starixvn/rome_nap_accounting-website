var vsDownlineSettings = {
    roleId: 0,
    isUpdateCompleted: false,
    getDefaultSettings: function (customerSettings) {        
        this.roleId = parseInt($("#hiddenRoleID").val());
        $('#tblSettingLimit tbody').html('');
        var htmlTilte = '<tr class="title">\
                                    <td colspan="4" align="center">\
                                        <span style="font-weight: bold">Bet Setting</span>\
                                    </td>\
                                </tr>\
                                <tr class="title">\
                                    <td style="width: 100px">&nbsp;\
                                    </td>\
                                    <td style="width: 185px" align="center">\
                                        <span style="font-weight: bold">Min Bet</span>\
                                    </td>\
                                    <td style="width: 185px" align="center">\
                                        <span style="font-weight: bold">Max Bet</span>\
                                    </td>\
                                    <td style="width: 185px" align="center">\
                                        <span style="font-weight: bold">Max Per Match</span>\
                                    </td>\
                                </tr>'
                                ;
        $('#tblSettingLimit tbody').append(htmlTilte);

        //init common setting
        vsCommon.init();
        //re-assign constrain
        var totalSports = window.virtualSports.length;
        for (var i = 0; i < totalSports; i++) {
            var sport = window.virtualSports[i];
            var minBetv = "minbetv" + sport.Id;
            var maxBetv = "maxbetv" + sport.Id;
            var maxPerMatchv = "maxpermatchv" + sport.Id;

            var minBet = parseFloat(customerSettings[minBetv]);
            var maxBet = parseFloat(customerSettings[maxBetv]);
            var maxPerMatch = parseFloat(customerSettings[maxPerMatchv]);
            //current line
            var html = '<tr>\
                                <td align="left" style="padding-left: 4px">\
                                    <span>' + sport.Name + '</span>\
                                </td>\
                                <td>\
                                    <input id="' + minBetv + '" minbet="true" type="text"  style="width: 120px; text-align: right" value="' + minBet.formatMoneyFromVal() + '" />\
                                    <span></span>\
                                </td>\
                                <td>\
                                    <input id="' + maxBetv + '" maxbet="true" type="text"  style="width: 120px; text-align: right" value="' + maxBet.formatMoneyFromVal() + '" />\
                                    <span></span>\
                                </td>\
                                <td>\
                                    <input id="' + maxPerMatchv + '" maxpermatch="true" type="text"  style="width: 120px; text-align: right" value="' + maxPerMatch.formatMoneyFromVal() + '" />\
                                    <span></span>\
                                </td>\
                            </tr>'
                            ;
            $('#tblSettingLimit tbody').append(html);
            //upline
            if (vsDownlineSettings.roleId == 3) {
                $("#" + minBetv).get(0).uplineVal = parseFloat(customerSettings[minBetv + "_min"]);
                $("#" + maxBetv).get(0).uplineVal = parseFloat(customerSettings[maxBetv + "_max"]);
            }
            else {
                $("#" + minBetv).get(0).uplineVal = parseFloat(customerSettings[minBetv + "_max"]);
                $("#" + maxBetv).get(0).uplineVal = parseFloat(customerSettings[maxBetv + "_min"]);
            }
            $("#" + maxPerMatchv).get(0).uplineVal = parseFloat(customerSettings[maxPerMatchv + "_max"]);
        }

        //PT
        var uplineValMax, seletedValue, minPt;
        if (vsDownlineSettings.roleId == 3) {
            uplineValMax = parseFloat(customerSettings["v180mpositiontaking_1201_max"]);
            minPt = parseFloat(customerSettings["v180mpositiontaking_1201_min"]);
            seletedValue = parseFloat(customerSettings["v180mpositiontaking_1201"]).formatMoneyFromVal();
        }
        else {
            uplineValMax = parseFloat(customerSettings["v180apositiontaking_1201_max"]);
            minPt = 0;
            seletedValue = parseFloat(customerSettings["v180apositiontaking_1201"]).formatMoneyFromVal();
        }

        if (isNaN(uplineValMax)) uplineValMax = 0;
        if (isNaN(minPt)) minPt = 0;
        $("#selVirtualSportsPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
            if (parseFloat($(this).val()) < minPt) $(this).remove();
        });
        $("#selVirtualSportsPT").removeAttr("selected").find("option[value='" + seletedValue + "']").attr("selected", "selected");

        vsCommon.initConstraint();

    },
    assignVirtualSportsSettings: function (params) {
        var totalSports = window.virtualSports.length;
        if (totalSports > 0) {
            for (var i = 0; i < totalSports; ++i) {
                var sport = window.virtualSports[i];
                var bettypes = sport.Bettype;
                var id = sport.Id;

                params.push({
                    Name: "v" + id + "minbet"
                    , Value: $("#" + "minbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "v" + id + "maxbet"
                    , Value: $("#" + "maxbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "v" + id + "maxpermatch"
                    , Value: $("#" + "maxpermatchv" + id).getNumberValue()
                });

                //VS position taking
                params.push({
                    Name: "v" + id + "spositiontaking"
                    , Value: 0
                });
                params.push({
                    Name: "v" + id + "mpositiontaking"
                    , Value: (vsDownlineSettings.roleId >= 3 ? $("#selVirtualSportsPT").val() : 0)
                });
                params.push({
                    Name: "v" + id + "apositiontaking"
                    , Value: $("#selVirtualSportsPT").val()
                });
            }
        }
    },
    assignVirtualSportsSettingsAndUpdate: function () {
        var totalSports = window.virtualSports.length;
        if (totalSports > 0) {
            for (var i = 0; i < totalSports; ++i) {
                var sport = window.virtualSports[i];
                var bettypes = sport.Bettype;
                var id = sport.Id;
                var params = [];

                params.push({
                    Name: "userid"
                , Value: $("#hiddenAccID").val()
                });
                params.push({
                    Name: "custid"
                , Value: $("#hiddenCustID").val()
                });
                params.push({
                    Name: "sporttype"
                , Value: id
                });

                params.push({
                    Name: "minbet"
            , Value: $("#" + "minbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "maxbet"
            , Value: $("#" + "maxbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "maxpermatch"
            , Value: $("#" + "maxpermatchv" + id).getNumberValue()
                });

                //VS position taking
                params.push({
                    Name: "spositiontaking"
                , Value: 0
                });
                params.push({
                    Name: "mpositiontaking"
                , Value: (vsDownlineSettings.roleId >= 3 ? $("#selVirtualSportsPT").val() : 0)
                });
                params.push({
                    Name: "apositiontaking"
                , Value: $("#selVirtualSportsPT").val()
                });

                vsDownlineSettings.updateSetting(params);

                if (i == totalSports - 1) {
                    vsDownlineSettings.isUpdateCompleted = true;

                }
            }


        }
    },
    updateSetting: function (params) {
        var urlRequest = '../VirutalSportsDepositCustomer/UpdateMasterSettings';

        if (this.roleId == 3) {
            urlRequest = '../VirutalSportsDepositCustomer/UpdateMasterSettings';
        }
        else if (this.roleId == 2) {
            urlRequest = '../VirutalSportsDepositCustomer/UpdateAgentSettings';
        }
        else if (this.roleId == 1) {
            urlRequest = '../VirutalSportsDepositCustomer/UpdateMemberSettings';
        }

        $.ajax({
            url: urlRequest,
            type: 'POST',
            dataType: 'json',
            data: $.toJSON(params),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    vsDownlineSettings.isSuccess = false;
                    common.showErr(data.ErrorMessage);
                    common.unBlock();
                }
                else {
                    if (vsDownlineSettings.isUpdateCompleted) {
                        common.unBlock();
                        common.showSuccess("Update successfully!");
                        common.reloadPage("DepositCustomer/EditSettingAgent");
                    }
                }
            },
            error: function () {
                common.unBlock();
            }
        });
    },
    updateMultipleSports: function () {
        this.isUpdateCompleted = false;
        this.assignVirtualSportsSettingsAndUpdate();
    }
};

$(document).ready(function () {
    vsDownlineSettings.getDefaultSettings(window.dataJson);
});