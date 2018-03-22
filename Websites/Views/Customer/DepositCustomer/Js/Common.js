var common = {
    moneyMax: 912337203685477.5807,
    errMsg: ""
};

common.init = function () {
    $("#tblTabSBNG").click(function () {
        $("#divSBNGContainer").slideToggle("fast", function () {
            if (!$("#divSBNGContainer").is(":visible")) {
                $("#tblTabSBNG").find("td[arrow]").removeClass("md2").addClass("md3");
            }
            else {
                $("#tblTabSBNG").find("td[arrow]").removeClass("md3").addClass("md2");
            }
        });
    });    

    this.arrInputsReaminSportMinBet_SB = $("#divSBNGContainer").find("input[type='text'][remainsport][minbet]");
    this.arrInputsReaminSportMaxBet_SB = $("#divSBNGContainer").find("input[type='text'][remainsport][maxbet]");
    this.arrInputsReaminSportMaxperMatch_SB = $("#divSBNGContainer").find("input[type='text'][remainsport][maxpermatch]");
  

    $("#divSBNGContainer").find("input[type='text']").ForceDecimalOnly().ForceCurrencyOnly();
    $("#divSBNGContainer").find("input[type='text'][soccer][minbet]").keyup(function () {
        if ($("#cbxCopyAllSportsBook").is(":checked")) {
            common.arrInputsReaminSportMinBet_SB.val($(this).val());
        }
    });
    $("#divSBNGContainer").find("input[type='text'][soccer][maxbet]").keyup(function () {
        if ($("#cbxCopyAllSportsBook").is(":checked")) {
            common.arrInputsReaminSportMaxBet_SB.val($(this).val());
        }
    });
    $("#divSBNGContainer").find("input[type='text'][soccer][maxpermatch]").keyup(function () {
        if ($("#cbxCopyAllSportsBook").is(":checked")) {
            common.arrInputsReaminSportMaxperMatch_SB.val($(this).val());
        }
    });
   

    //Sportbook
    var pt = 0.9;
    var val;
    while (pt > 0) {
        val = parseFloat(pt.toFixed(2));
        $("#selSBPT").append("<option value='" + val + "'>" + val + "</option>");
        pt = pt - 0.01;
    }
    $("#selSBPT").append("<option selected='selected' value='0'>0</option>");

    //NumberGame
    PTRenderManager.renderNumberGameOptions();    

    $("#cbxCopyAllSportsBook").change(function () {
        if ($(this).is(":checked")) {
            $("#divSBNGContainer").find("input[remainsport]").attr("disabled", "disabled");
            var soccerMinBet = $("#divSBNGContainer").find("input[soccer][minbet]").val();
            var soccerMaxBet = $("#divSBNGContainer").find("input[soccer][maxbet]").val();
            var soccerMaxPerMatch = $("#divSBNGContainer").find("input[soccer][maxpermatch]").val();
            $("#divSBNGContainer").find("input[remainsport][minbet]").val(soccerMinBet);
            $("#divSBNGContainer").find("input[remainsport][maxbet]").val(soccerMaxBet);
            $("#divSBNGContainer").find("input[remainsport][maxpermatch]").val(soccerMaxPerMatch);
        }
        else {
            $("#divSBNGContainer").find("input[remainsport]").removeAttr("disabled");
        }
    });   
};

common.resetValues = function () {
    $("#tbxSuperName").val("");
    $("#tbxMasterName").val("");
    $("#tbxAgentName").val("");
    $("#tbxUserNameAddNew").val("");

    $("#tbxDummyMasterName").val("");
    $("#tbxDummyAgentName").val("");
    $("#tbxDummyMemberName").val("");

    $("#divSBNGContainer").find("input[type='text']").val("0");
    $("#divSBNGContainer").find("select").find("option[value='0']").attr("selected", "selected");
   
    if (typeof vsCommon != 'undefined') {
        vsCommon.resetValues();
    }

    if (window.kenoSettingsManager) {
        window.kenoSettingsManager.resetValues();
    }

    $(document).find("input[type='checkbox']").each(function () {
        this.checked = false;
        $(this).removeAttr("disabled");
    });
    $(document).find("input[type='text']").removeAttr("disabled");
    $(document).find("select").removeAttr("disabled");
};

common.initConstraint = function () {
    $("#divSBNGContainer").find("input[type='text']").each(function () {
        if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html(">=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxperball") != null && typeof $(this).attr("maxperball") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
    });  
    
    if (window.kenoSettingsManager) {
        kenoSettingsManager.initConstraints();
    }
};

common.isNumber = function (n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
};

common.hideDivMsg = function () {
    $("#divErr").hide();
    $("#divSuccess").hide();
};

common.showErr = function (text) {
    showAlertMessage("divErr", text, 1);
    $("#divErr").show().find("div:first");
};

common.showSuccess = function (text) {
    showAlertMessage("divErr", text, 0);
    $("#divErr").show().find("div:first");
};

common.blockUI = function () {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

common.unBlock = function () {
    if (parent.parent.$ && parent.parent.$.unblockUI) {
        try { parent.parent.$.unblockUI(); } catch (e) { }
    }
    else {
        try { parent.$.unblockUI(); } catch (e) { }
    }
};

common.isEmpty = function (val) {
    if (typeof (val) == "undefined" || val == null) return false;
    return val.replace(/ /g, '') == "";
};

common.validateUserName = function (str) {
    return str.indexOf("<") == -1 ||
           str.indexOf(">") == -1 ||
           str.indexOf(",") == -1;
};

common.verifyDataContrain = function () {
    this.errMsg = "";
    var valid = true;
    var job = function (obj) {
        valid = false; $(obj).css("border-color", "red");
    };
    var setMsg1 = function (val) {
        common.errMsg = "Min Bet must be less than " + val + " (Max Bet)";
    };
    var setMsg2 = function (val) {
        common.errMsg = "Min Bet must be greater than or equal " + val;
    };
    var setMsg3 = function (val) {
        common.errMsg = "Max Bet must be less than or equal " + val;
    };
    var setMsg4 = function (val) {
        common.errMsg = "Max Per Match must be less than or equal " + val;
    };
    var setMsg5 = function (val) {
        common.errMsg = "Max Per Ball must be less than or equal " + val;
    };
    var setMsg6 = function (val) {
        common.errMsg = "Max Bet must be less than " + val + " (Max Per Match)";
    };
    var setMsg7 = function (val) {
        common.errMsg = "Please input value!";
    };
    var setMsg8 = function (val) {
        common.errMsg = "Value is not a number!";
    };
    var setMsg9 = function (val) {
        common.errMsg = "Value can not be nagative!";
    };
    var setMsg10 = function (val) {
        common.errMsg = "Value is so larger!";
    };
    var setMsg11 = function (val) {
        common.errMsg = "Min Bet must be less than " + val + " (Max Per Match)";
    };

    //virutal sports
    if (typeof vsCommon != 'undefined') {
        vsCommon.verifyDataContrain(valid, job, setMsg1, setMsg2, setMsg3, setMsg4, setMsg5, setMsg6, setMsg7, setMsg8, setMsg9, setMsg10, setMsg11);
    }

    if (window.kenoSettingsManager) {
        window.kenoSettingsManager.verifyDataContrains(valid, job, setMsg1, setMsg2, setMsg3, setMsg4, setMsg5, setMsg6, setMsg7, setMsg8, setMsg9, setMsg10, setMsg11);
    }

    $("#divSBNGContainer").find("input[type='text']").css("border-color", "").each(function () {
        var currentVal = parseFloat($(this).getNumberValue());
        if (valid) {
            if (common.isEmpty($(this).val())) {
                job(this);
                setMsg7();
                return;
            }
            if (!common.isNumber($(this).getNumberValue())) {
                job(this);
                setMsg8();
                return;
            }
            if (currentVal < 0) {
                job(this);
                setMsg9();
                return;
            }
            if (currentVal > common.moneyMax) {
                job(this);
                setMsg10();
                return;
            }

            var minBet, maxBet, maxPerMatch;
            if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
                if (isNaN(maxBet)) return;
                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) return;
                if (currentVal > maxBet) {
                    job(this);
                    setMsg1(maxBet);
                    return;
                }
                if (currentVal > maxPerMatch) {
                    job(this);
                    setMsg11(maxPerMatch);
                    return;
                }
            }
            else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) return;
                if (currentVal > maxPerMatch) {
                    job(this); setMsg6(maxPerMatch); return;
                }
            }

            if (typeof this.uplineVal == "undefined") {
                return;
            }

            var uplineMinBet, uplineMaxBet, uplineMaxPermatch;
            uplineMinBet = $(this).parents("tr:first").find("input[minbet]").get(0).uplineVal;
            uplineMaxBet = $(this).parents("tr:first").find("input[maxbet]").get(0).uplineVal;
            uplineMaxPermatch = $(this).parents("tr:first").find("input[maxpermatch]").get(0).uplineVal;
            if (uplineMinBet == 0 && uplineMaxBet == 0 && uplineMaxPermatch == 0) return; //valid case

            minBet = parseFloat($(this).parents("tr:first").find("input[minbet]").getNumberValue());
            if (isNaN(minBet)) minBet = 0;
            maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
            if (isNaN(maxBet)) maxBet = 0;
            maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
            if (isNaN(maxPerMatch)) maxPerMatch = 0;
            if (minBet == 0 && maxBet == 0 && maxPerMatch == 0) return; //valid case

            if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                if (currentVal < this.uplineVal) { job(this); setMsg2(this.uplineVal); return; }
            }
            else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg3(this.uplineVal); return; }
            }
            else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg4(this.uplineVal); return; }
            }
            else if ($(this).attr("maxperball") != null && typeof $(this).attr("maxperball") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg5(this.uplineVal); return; }
            }
        }
    });  

    return valid;
};

common.reloadPage = function (url) {
    var timerId = setTimeout(function () {
        parent.gotoURL('centerFr', url);
        clearTimeout(timerId);
    }, 5000);
};

$(document).ready(function () {
    common.init();
});
