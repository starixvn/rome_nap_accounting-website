var vsSuperSetting = {
    isUpdateCompleted: false,
    getSuperSettings: function (custSettings) {
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

        var totalSports = window.virtualSports.length;
        for (var i = 0; i < totalSports; i++) {
            var sport = window.virtualSports[i];
            var minBetv = "minbetv" + sport.Id;
            var maxBetv = "maxbetv" + sport.Id;
            var maxPerMatchv = "maxpermatchv" + sport.Id;

            var minBetVal = parseFloat(custSettings[minBetv]).formatMoneyFromVal();
            var maxBetVal = parseFloat(custSettings[maxBetv]).formatMoneyFromVal();
            var maxPerMacthVal = parseFloat(custSettings[maxPerMatchv]).formatMoneyFromVal();

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
        $("#selVirtualSportsPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v180spositiontaking_1201"]).formatMoneyFromVal() + "']").attr("selected", "selected");

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
                    , Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "v" + id + "mpositiontaking"
                    , Value: $("#selVirtualSportsPT").val()
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
            , Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "mpositiontaking"
            , Value: $("#selVirtualSportsPT").val()
                });
                params.push({
                    Name: "apositiontaking"
            , Value: $("#selVirtualSportsPT").val()
                });

                if (i == totalSports - 1)
                    vsSuperSetting.isUpdateCompleted = true;

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