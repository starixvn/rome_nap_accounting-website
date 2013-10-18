var vsSuperSetting = {
    isUpdateCompleted: false,
    assignVirtualSportsSettings: function (params) {
        var totalSports = window.virtualSports.length;
        params.push({
            Name: "disablevirtualsports",
            Value: !$("#cbxEnableVirtualSports").is(":checked")
        });

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
    assignVirtualSportsSettingsAndUpdate: function (superId, masterId, agentId) {
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
                , Value: superId
                });
                params.push({
                    Name: "masterid"
                , Value: masterId
                });
                params.push({
                    Name: "agentid"
                , Value: agentId
                });
                params.push({
                    Name: "sporttype"
                , Value: id
                });
                params.push({
                    Name: "disablevirtualsports"
            , Value: !$("#cbxEnableVirtualSports").is(":checked")
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

                if (i == (totalSports - 1))
                    vsSuperSetting.isUpdateCompleted = true;
                vsSuperSetting.updateSetting(params);
            }


        }
    },
    updateSetting: function (params) {
        $.ajax({
            url: '../VirutalSportsDepositCustomer/AddNewSuperSettings',
            type: 'POST',
            dataType: 'json',
            data: $.toJSON(params),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    common.showErr(data.ErrorMessage);
                    common.unBlock();
                }
                else {
                    if (vsSuperSetting.isUpdateCompleted) {
                        var msg = "Add new super: " + $("#tbxSuperName").val();
                        if ($("#tbxMasterName").val() != "") {
                            msg += ", master: " + $("#tbxMasterName").val();
                        }

                        if ($("#tbxAgentName").val() != "") {
                            msg += ", agent: " + $("#tbxAgentName").val();
                        }

                        msg += " successfully!";

                        common.unBlock();
                        common.showSuccess(msg);
                        common.resetValues();
                    }
                }
            },
            error: function () {
                common.unBlock();
            }
        });
    },
    updateMultipleSports: function (superId, masterId, agentId) {
        this.isUpdateCompleted = false;
        this.assignVirtualSportsSettingsAndUpdate(superId, masterId, agentId);
    }

};

$(document).ready(function () { vsCommon.init(); });