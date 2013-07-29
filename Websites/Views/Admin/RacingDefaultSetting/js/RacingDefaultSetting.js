

var racingDefaultSetting = {
    params: {
        SportType: '',
        Bettype: '',
        Currency: ''
    },
    gridOpts: {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 710, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 80,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    },
    selRacingDefaultSetting: function () {
        racingDefaultSetting.params.SportType = $('#selectSport').val();
        racingDefaultSetting.params.Bettype = $('#selectTote').val();
        racingDefaultSetting.params.Currency = $('#txtCurrency').val();
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "RacingDefaultSetting/SelRacingDefaultSetting",
            data: racingDefaultSetting.params,
            success: function (result) {
                $("#racingContent").html(result);
                if ($.browser.msie && parseInt($.browser.version, 10) === 9) {
                    $("#racingContent").html(racingDefaultSetting.removeSpecialCharactersForIE9($("#racingContent").html()));
                }
                $("#Rarcing").initGrid(racingDefaultSetting.gridOpts);
                $('#Rarcing').removeClass("display-none").gridScroller(racingDefaultSetting.gridOpts);
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    checkEnableSubmit: function (value, index) {
        var valueTmp = $("#txtCurrency_" + index).val();
        if (valueTmp == value) {
            $("#btnSave_" + index).attr("disabled", "disable");
            _countSaveQuality--;
            racingDefaultSetting.checkEnableMultiSubmit();
        }
        else {
            $("#btnSave_" + index).removeAttr("disabled");
            _countSaveQuality++;
            racingDefaultSetting.checkEnableMultiSubmit();
        }
    },
    checkEnableMultiSubmit: function () {
        if (_countSaveQuality == 0) {
            $("#btnSaveMultiple").attr("disabled", "disable");
        }
        else {
            $("#btnSaveMultiple").removeAttr("disabled");
        }
    },
    updateSingle: function (index) {
        var racing = null;
        $.each(racingListModel, function (i, item) {
            if (item.Index == index) {
                racing = item;
                return item;
            }
        });
        racing.MinBet = $("#txtCurrency_" + index).val();

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "RacingDefaultSetting/UpdateRacingDefaultSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(racing),
            success: function (Result) {
                var errorFlag = false;
                if (Result.ErrorCode != 0) {
                    errorFlag = true;
                }

                if (!errorFlag) {
                    $('#err').show();
                    showAlertMessage("err", "Updated successful.", 0);
                    setTimeout("racingDefaultSetting.selRacingDefaultSetting()", 2000);
                }
                else {
                    $('#err').show();
                    showAlertMessage("err", "Updated fail.", 1);
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    updateMultiple: function () {
        var arrRacing = [];
        var racing = null;
        $('#tblContent').find('input.btnUpdate').each(function () {
            if (!this.disabled) {
                tmpId = this.id.split("_");
                $.each(racingListModel, function (i, item) {
                    if (item.Index.toString() == tmpId[1]) {
                        racing = item;
                        racing.MinBet = $("#txtCurrency_" + tmpId[1]).val();
                        arrRacing.push(racing);
                        return;
                    }
                });
            }
        });

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "RacingDefaultSetting/UpdateMultipleRacingDefaultSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(arrRacing),
            success: function (Result) {
                if (Result.length > 0) {
                    var errorFlag = false;
                    var length = Result.length;
                    for (var i = 0; i < length; i++) {
                        if (Result[i].ErrorCode != 0) {
                            errorFlag = true;
                            break;
                        }
                    }

                    if (!errorFlag) {
                        $('#err').show();
                        showAlertMessage("err", "Updated successful.", 0);
                        setTimeout("racingDefaultSetting.selRacingDefaultSetting()", 2000);
                    }
                    else {
                        $('#err').show();
                        showAlertMessage("err", "Updated fail.", 1);
                    }
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    removeSpecialCharactersForIE9: function (content) {
        var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
        var html = content.replace(expr, '><');
        return html;
    }
};

$(document).ready(function () {
    $("#selectSport").change(function () {
        var index = $("#selectSport").val();
        $("#selectTote").empty();
        var sport = null;

        $.each(sportListModel, function (i, item) {
            if (item.SportID == index) {
                sport = item;
                return item;
            }
        });

        $.each(sport.ToteList, function (i, item) {
            $('#selectTote').append('<option value="'
                                 + item.ToteID
                                 + '">'
                                 + item.ToteName
                                 + '</option>');
        });
    });

    racingDefaultSetting.selRacingDefaultSetting();
});