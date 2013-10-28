var vsSuperSetting = {
    isUpdateCompleted: false,
    custSettings: null,
    getSettingInfoDictionary: function (custSettings) {
        var settingInfoDictionary = {};
        var virtualSportsLength = custSettings.VirtualSportsSettingInfo.length;
        for (var i = 0; i < virtualSportsLength; i++) {
            var sport = custSettings.VirtualSportsSettingInfo[i];
            var settingLength = sport.SettingInfo.length;
            for (var j = 0; j < settingLength; j++) {
                var settingInfo = sport.SettingInfo[j];
                settingInfoDictionary[settingInfo.Name] = settingInfo.Value;
            }
        }

        return settingInfoDictionary;
    },
    getSuperSettings: function (custSettings) {
        vsSuperSetting.custSettings = custSettings;
        var settingInfoDictionary = vsSuperSetting.getSettingInfoDictionary(custSettings);
        $('#tblSettingLimit tbody').html('');
        var htmlTilte = '<tr class="title">\
                                    <td colspan="4" align="center">\
                                        <span style="font-weight: bold">Bet Setting</span>\
                                    </td>\
                                </tr>\
                                <tr class="title">\
                                    <td style="width: 130px">\
                                    </td>\
                                    <td style="width: 130px" align="center">\
                                        <span style="font-weight: bold">Min Bet</span>\
                                    </td>\
                                    <td style="width: 130px" align="center">\
                                        <span style="font-weight: bold">Max Bet</span>\
                                    </td>\
                                    <td style="width: 130px" align="center">\
                                        <span style="font-weight: bold">Max Per Match</span>\
                                    </td>\
                                </tr>'
                                ;
        $('#tblSettingLimit tbody').append(htmlTilte);

        var virtualSportsLength = custSettings.VirtualSportsSettingInfo.length;
        for (var i = 0; i < virtualSportsLength; i++) {
            var sport = custSettings.VirtualSportsSettingInfo[i];
            var minBetv = "minbetv" + sport.Id;
            var maxBetv = "maxbetv" + sport.Id;
            var maxPerMatchv = "maxpermatchv" + sport.Id;

            var minBetVal = parseFloat(settingInfoDictionary[minBetv]).formatMoneyFromVal();
            var maxBetVal = parseFloat(settingInfoDictionary[maxBetv]).formatMoneyFromVal();
            var maxPerMacthVal = parseFloat(settingInfoDictionary[maxPerMatchv]).formatMoneyFromVal();

            var html = '<tr>\
                                <td align="left" style="padding-left: 4px">\
                                    <span>' + sport.Name + '</span>\
                                </td>\
                                <td>\
                                    <input id="' + minBetv + '" minbet="true" type="text"  style="width: 128px; text-align: right" value="' + minBetVal + '" />\
                                </td>\
                                <td>\
                                    <input id="' + maxBetv + '" maxbet="true" type="text"  style="width: 128px; text-align: right" value="' + maxBetVal + '" />\
                                </td>\
                                <td>\
                                    <input id="' + maxPerMatchv + '" maxpermatch="true" type="text"  style="width: 128px; text-align: right" value="' + maxPerMacthVal + '" />\
                                </td>\
                            </tr>'
                            ;
            $('#tblSettingLimit tbody').append(html);
        }

        vsCommon.init();
        var positionTakingValue = settingInfoDictionary["v180spositiontaking"] ?
            parseFloat(settingInfoDictionary["v180spositiontaking"]).formatMoneyFromVal() :
            0;
        $("#selVirtualSportsPT").removeAttr("selected").find("option[value='" + positionTakingValue + "']").attr("selected", "selected");
    },
    assignVirtualSportsSettings: function (params) {
        var totalSports = window.virtualSports.length;
        if (totalSports > 0) {
            for (var i = 0; i < totalSports; ++i) {
                var sport = window.virtualSports[i];
                var id = sport.Id;

                params.push({
                    Name: "v" + id + "minbet",
                    Value: $("#" + "minbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "v" + id + "maxbet",
                    Value: $("#" + "maxbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "v" + id + "maxpermatch",
                    Value: $("#" + "maxpermatchv" + id).getNumberValue()
                });

                //VS position taking
                params.push({
                    Name: "v" + id + "spositiontaking",
                    Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "v" + id + "mpositiontaking",
                    Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "v" + id + "apositiontaking",
                    Value: $("#selVirtualSportsPT").val()
                });
            }
        }
    },
    getUpdateVirtualSportsSettings: function () {
        if (vsSuperSetting.custSettings == null) {
            return;
        }

        var custSettings = vsSuperSetting.custSettings;
        var virtualSportsLength = custSettings.VirtualSportsSettingInfo.length;
        var settingInfoList = [];

        for (var i = 0; i < virtualSportsLength; i++) {
            var sport = custSettings.VirtualSportsSettingInfo[i];
            var id = sport.Id;

            var sportSettings = [];

            settingName = "v" + id + "minbet";
            sportSettings.push({
                Name: settingName,
                Value: $("#" + "minbetv" + id).getNumberValue()
            });

            settingName = "v" + id + "maxbet";
            sportSettings.push({
                Name: settingName,
                Value: $("#" + "maxbetv" + id).getNumberValue()
            });

            settingName = "v" + id + "maxpermatch";
            sportSettings.push({
                Name: settingName,
                Value: $("#" + "maxpermatchv" + id).getNumberValue()
            });

            //VS position taking
            var positionTakingValue = $("#selVirtualSportsPT").val();

            settingName = "v" + id + "spositiontaking";
            sportSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingName = "v" + id + "mpositiontaking";
            sportSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingName = "v" + id + "apositiontaking";
            sportSettings.push({
                Name: settingName,
                Value: positionTakingValue
            });

            settingInfoList.push({
                Id: id,
                SettingInfo: sportSettings
            });
        }

        return settingInfoList;
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
                    Name: "userid",
                    Value: $("#hiddenAccID").val()
                });
                params.push({
                    Name: "custid",
                    Value: $("#hiddenCustID").val()
                });
                params.push({
                    Name: "sporttype",
                    Value: id
                });

                params.push({
                    Name: "minbet",
                    Value: $("#" + "minbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "maxbet",
                    Value: $("#" + "maxbetv" + id).getNumberValue()
                });

                params.push({
                    Name: "maxpermatch",
                    Value: $("#" + "maxpermatchv" + id).getNumberValue()
                });

                //VS position taking
                params.push({
                    Name: "spositiontaking",
                    Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "mpositiontaking",
                    Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "apositiontaking",
                    Value: $("#selVirtualSportsPT").val()
                });

                if (i == totalSports - 1) {
                    vsSuperSetting.isUpdateCompleted = true;
                }

                vsSuperSetting.updateSetting(params);
            }

        }
    },
    updateSetting: function (params) {
        $.ajax({
            url: '../VirutalSportsDepositCustomer/UpdateSuperSettings',
            type: 'POST',
            dataType: 'json',
            data: $.toJSON(params),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    common.showErr(data.ErrorMessage);
                    vsSuperSetting.isSuccess = false;
                    common.unBlock();
                }
                else {
                    if (vsSuperSetting.isUpdateCompleted) {
                        common.unBlock();
                        common.showSuccess("Update successfully!");
                        common.reloadPage("DepositCustomer/EditSettingSuper");
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
    vsSuperSetting.getSuperSettings(window.dataJson);
});