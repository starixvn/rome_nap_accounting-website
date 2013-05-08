var vsDownlineSettings = {
    roleId: 0,
    roleName: '',
    isUpdateCompleted: false,
    getDefaultSettings: function (uplineData) {
        this.roleId = parseInt($("#hiddenRoleID").val());
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

            var minBet = parseFloat(uplineData[minBetv]);
            var maxBet = parseFloat(uplineData[maxBetv]);
            var maxPerMatch = parseFloat(uplineData[maxPerMatchv]);
            //current line
            if (minBet == 0) {
                if (this.roleId == 2) {
                    minBet = parseFloat(uplineData[minBetv + '_min']);
                }
                else {
                    minBet = parseFloat(uplineData[minBetv + '_max']);
                }
            }
            if (maxBet == 0) {
                if (this.roleId == 2) {
                    minBet = parseFloat(uplineData[minBetv + '_max']);
                }
                else {
                    maxBet = parseFloat(uplineData[maxBetv + '_min']);
                }

            }
            if (maxPerMatch == 0) {
                maxPerMatch = parseFloat(uplineData[maxPerMatchv + '_max']);
            }

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
                $("#" + minBetv).get(0).uplineVal = parseFloat(uplineData[minBetv]);
                $("#" + maxBetv).get(0).uplineVal = parseFloat(uplineData[maxBetv]);
                $("#" + maxPerMatchv).get(0).uplineVal = parseFloat(uplineData[maxPerMatchv]);

            }
            else {
                if (vsDownlineSettings.roleId == 2) {
                    $("#" + minBetv).get(0).uplineVal = parseFloat(uplineData[minBetv + '_min']);
                    $("#" + maxBetv).get(0).uplineVal = parseFloat(uplineData[maxBetv + '_max']);
                }
                else {
                    $("#" + minBetv).get(0).uplineVal = parseFloat(uplineData[minBetv + '_max']);
                    $("#" + maxBetv).get(0).uplineVal = parseFloat(uplineData[maxBetv + '_min']);
                }
                $("#" + maxPerMatchv).get(0).uplineVal = parseFloat(uplineData[maxPerMatchv + '_max']);
            }

            if (minBet < ($("#" + minBetv).get(0).uplineVal)) {
                $("#" + minBetv).val($("#" + minBetv).get(0).uplineVal.formatMoneyFromVal());
            }
            if (maxBet > ($("#" + maxBetv).get(0).uplineVal)) {
                $("#" + maxBetv).val($("#" + maxBetv).get(0).uplineVal.formatMoneyFromVal());
            }
            if (maxPerMatch > ($("#" + maxPerMatchv).get(0).uplineVal)) {
                $("#" + maxPerMatchv).val($("#" + maxPerMatchv).get(0).uplineVal.formatMoneyFromVal());
            }
        }

        //PT
        var uplineValMax, selectedValue;
        if (vsDownlineSettings.roleId == 3) {
            uplineValMax = parseFloat(uplineData["v180spositiontaking_1201"]);
        }
        else if (vsDownlineSettings.roleId == 2) {
            uplineValMax = parseFloat(uplineData["v180mpositiontaking_1201"]);
        }
        else {
            uplineValMax = parseFloat(uplineData["v180apositiontaking_1201"]);
        }

        if (isNaN(uplineValMax)) uplineValMax = 0;
        selectedValue = uplineValMax.formatMoneyFromVal();
        $("#selVirtualSportsPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selVirtualSportsPT").removeAttr("selected").find("option[value='" + selectedValue + "']").attr("selected", "selected");

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
    assignVirtualSportsSettingsAndAddNew: function (customerId) {
        var totalSports = window.virtualSports.length;
        if (totalSports > 0) {
            for (var i = 0; i < totalSports; ++i) {
                var sport = window.virtualSports[i];
                var bettypes = sport.Bettype;
                var id = sport.Id;
                var params = [];

                params.push({
                    Name: "creator"
                    , Value: $("#hiddenAccID").val()
                });
                params.push({
                    Name: "custid"
                    , Value: customerId
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

                vsDownlineSettings.addSetting(params);

                if (i == totalSports - 1) {
                    vsDownlineSettings.isUpdateCompleted = true;
                }
            }


        }
    },
    addSetting: function (params) {
        var paramName = 'srecommend';
        var urlRequest = '../VirutalSportsDepositCustomer/AddNewMasterSettings';
        if (this.roleId == 3) {
            paramName = 'srecommend';
            urlRequest = '../VirutalSportsDepositCustomer/AddNewMasterSettings';
            this.roleName = 'master';
        }
        else if (this.roleId == 2) {
            paramName = 'mrecommend';
            urlRequest = '../VirutalSportsDepositCustomer/AddNewAgentSettings';
            this.roleName = 'agent';
        }
        else if (this.roleId == 1) {
            paramName = 'recommend';
            urlRequest = '../VirutalSportsDepositCustomer/AddNewMemberSettings';
            this.roleName = 'member';
        }

        params.push({
            Name: paramName
            , Value: $("#hiddenCustID").val()
        });

        $.ajax({
            url: urlRequest,
            type: 'POST',
            dataType: 'json',
            data: $.toJSON(params),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    common.showErr(data.ErrorMessage);
                    vsDownlineSettings.isSuccess = false;
                    common.unBlock();
                }
                else {
                    if (vsDownlineSettings.isUpdateCompleted) {
                        common.unBlock();
                        common.showSuccess("Add new " + vsDownlineSettings.roleName + ' ' + $("#tbxUserNameAddNew").val() + " successfully!");
                        $("#tbxUserNameAddNew").val("");
                    }
                }
            },
            error: function () {
                common.unBlock();
            }
        });

    },
    updateMultipleSports: function (customerId) {
        this.isUpdateCompleted = false;
        this.assignVirtualSportsSettingsAndAddNew(customerId);
    }
};

$(document).ready(function () {
    vsDownlineSettings.getDefaultSettings(window.dataJson);
});