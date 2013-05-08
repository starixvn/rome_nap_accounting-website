$(function () {
    $("#" + RacingSetting.ContainerId).delegate(".expanded", "click", function () {
        var obj = $(this);
        if (obj.hasClass("expand")) {
            obj.removeClass("expand");
            $("#" + obj.attr("target")).show();
        }
        else {
            obj.addClass("expand");
            $("#" + obj.attr("target")).hide();
        }
    });
    $("#" + RacingSetting.ContainerId).delegate(".btnmiddle", "click", function () {
        var obj = $(this);
        var target = obj.attr("target");
        var targetobj = $("." + target);
        if (targetobj.hasClass("md3")) {
            targetobj.removeClass("md3");
            targetobj.addClass("md2");
            $("#" + target).show();
        }
        else {
            targetobj.addClass("md3");
            targetobj.removeClass("md2");
            $("#" + target).hide();
        }
    });
    // assign racing message
    if (typeof Roles != 'undefined' && Roles.Message != null) {
        RacingMsg = $.extend(RacingMsg, Roles.Message);
    }
});
var sposition = "spositiontaking";
var mposition = "mpositiontaking";
var aposition = "apositiontaking";
var spositiontaking_isauto = "spositiontaking_isauto";
var spositiontaking_auto = "spositiontaking_auto";
var mpositiontaking_isauto = "mpositiontaking_isauto";
var mpositiontaking_auto = "mpositiontaking_auto";
var chkEnable = "chkEnable";
var chkEnableDownline = "chkEnableDownline";
var IBCPositiontaking = "IBCPositiontaking";
var chkCopyFromWin = "chkCopyFromWin";
var chkCopyAllSetting = "chkCopyAllSetting";
var txtMinbet = "txtMinbet";
var txtMaxbet = "txtMaxbet";
var txtMaxPerMatch = "txtMaxPerMatch";
var chkApplyToMember = "chkApplyToMember";
var commission = "commission";

//ErrorMsg
var RacingMsg = null;

//*******Define Global template for Racing Setting
var TplMain = '<table class="tblcollapse w1000 marginT6"><tbody>\
                <tr class="title1">\
                    <td colspan="4">${getRoleName(CustInfo.CustLevel)}: ${CustInfo.UserName}</td>\
                </tr>\
                <tr>\
                    <td width="20%" class="l">FirstName:</td>\
                    <td width="30%" class="l"><input type="text" id="txtFName" name="txtFName" value="${CustInfo.FirstName}" disabled="disabled" readonly="True">\
                    </td><td width="20%" class="l">Last Name:</td>\
                    <td width="30%" class="l"><input type="text" id="txtLName" name="txtLName" value="${CustInfo.LastName}" disabled="disabled" readonly="True"></td>\
                </tr>\
                <tr>\
                    <td class="l">Phone:</td>\
                    <td class="l"><input type="text" id="txtPhone" name="txtPhone" value="${CustInfo.Phone}" disabled="disabled" readonly="True"></td>\
                    <td class="l">Mobile Phone:</td>\
                    <td class="l"><input type="text" id="txtMobile" name="txtMobile" value="${CustInfo.MobilePhone}" disabled="disabled" readonly="True"></td>\
                </tr>\
                <tr>\
                    <td class="l">Fax:</td>\
                    <td class="l"><input type="text" id="txtFax" name="txtFax" value="${CustInfo.Fax}" disabled="disabled" readonly="True"></td>\
                    <td ></td>\
                    <td ></td>\
                </tr>\
            </tbody></table>\
            <br>\
            <div class="l"><input type="checkbox" onclick="ChkCopyAllSetting(this)" id="chkCopyAllSetting">&nbsp;<b>Copy all {{if CustInfo.CustLevel>0}}Commission & {{/if}}Position Taking  from ${DefaultRacingSportType} - Win</b></div>\
            {{if IsHideCopyBetSetting==false}}<div class="l"><input type="checkbox" onclick="ChkCopyAllBetSetting(this)" id="chkCopyAllBetting">&nbsp;<b>Copy all Bet Setting from ${DefaultRacingSportType} - Win</b></div>{{/if}}\
            <div class="l"></div>\
            {{tmpl(RacingSportType) TplRacingBox}}\
            <table cellspacing="0" cellpadding="0" border="0" class="marginT6">\
                <tr>\
                    <td class="custBtn1"></td>\
                    <td onclick="return BtnOnClick();" id="btnAdd" class="custBtn2 hand">Update</td>\
                </tr>\
                <tr><td colspan="2">&nbsp;</td></tr>\
            </table><br>';

var TplRacingBox = '<div class="l {{if !Visible}}hidden{{/if}}"><table class="tblcollapse w1000 marginT6">\
                        <tr class="title1">\
                            <td class="l"><span class="expanded" target="setting_${Id}">${Name}</span></td>\
                        </tr>\
                        {{if Totes}}\
                        <tr id="setting_${Id}">\
                            <td class="l"><div> {{tmpl($data.Totes) TplRacingType}} </div></td>\
                        </tr>\
                        {{/if}}\
                    </table>\
                    </div>';

var TplRacingType = '<div class="{{if !Visible}}hidden{{/if}}"><table cellspacing="0" cellpadding="0" border="0" class="tblPTheader"><tbody>\
                            <tr id="btntop" style="cursor: pointer;">\
                                <td class="btnleft"></td>\
                                <td width="120" class="l btnmiddle" target="setting_${Id}" >\
                                    <span class="mp" >${Name}</span>\
                                </td>\
                                <td width="20" class="setting_${Id} r {{if IsTop}} md2{{else}} md3{{/if}}"></td>\
                            </tr>\
                        </tbody></table>\
                        <div id="${Id}_errmsg" class="w800 errmsg"></div>\
                        {{if Bettypes!=null}}\
                            <div id="setting_${Id}" class="l" style="{{if IsTop}}{{else}} display:none;{{/if}}">{{tmpl($data) RacingSetting.TplRacingTypeSetting}}</div>\
                        {{/if}}\
                    </div>';
//************* End Global template*******************

var RacingSettingObj = {
     //Global params
    CustInfo: {
        CustLevel:4,
        CustId: 0,
        UserName: "",
        FirstName: "",
        LastName: "",
        Phone: "",
        MobilePhone: "",
        Fax: ""
    },
    MaxPosition: 0.5,
    IsHideCopyBetSetting: true,
    RacingSportType:null 
};

var RacingSetting = {
    ContainerId: "SettingContent",
    TplRacingTypeSetting: "",
    newMemberPT: 0,
    maxNewMemberPT: 0,
    isNewPT: false,
    //Global Functions
    updatePT: function () {
        RacingSetting.setParamUpdatePT();
        RacingData.updateCustomerSetting(RacingSettingObj);
    },
    updateStatus: function () {
        RacingSetting.setParamUpdateStatus();
        RacingData.updateCustomerStatus(RacingSettingObj);
    },
    init: function (resp) {

        if (typeof resp == 'object') {
            $.extend(RacingSettingObj, resp);
        }

        $("#" + RacingSetting.ContainerId).empty();
        if (RacingSettingObj.RacingSportType != null && RacingSettingObj.RacingSportType.length > 0) {
            $.tmpl(TplMain, RacingSettingObj).appendTo("#" + RacingSetting.ContainerId);
            AssignCommission();
            AssignPT();
            if (Roles.CurrRole == Roles.SuperOnMember || Roles.CurrRole == Roles.MasterOnMember) {
                RacingSetting.validCopyAll();
            }
            PTUnBlockUI();
        }
        else {
            showAlertMessage("err", RacingMsg.noRacingData, 1);
            PTUnBlockUI();
        }
    },
    cloneSetting: function (setting) {
        if (typeof $.jCache != 'undefined')
            $.jCache.setItem("setting", setting);
    },
    getSetting: function () {
        if (typeof $.jCache != 'undefined')
            return $.jCache.getItem("setting");
        return RacingSettingObj;
    },
    initStatus: function (resp) {
        if (typeof resp == 'object') {
            $.extend(RacingSettingObj, resp);
        }
        $("#" + RacingSetting.ContainerId).empty();
        if (RacingSettingObj.RacingSportType != null && RacingSettingObj.RacingSportType.length > 0) {
            $.tmpl(TplMain, RacingSettingObj).appendTo("#" + RacingSetting.ContainerId);
            PTUnBlockUI();
        }
        else {
            showAlertMessage("err", RacingMsg.noRacingData, 1);
            PTUnBlockUI();
        }
    },
    initConfirmUpdateMember: function () {
        $("#dialogMemberPT").dialog({
            title: RacingMsg.confirmUpdateMember,
            autoOpen: false,
            height: 180,
            width: 400,
            modal: true,
            buttons: {
                "YES": function () {
                    RacingSetting.isNewPT = true;
                    RacingSetting.newMemberPT = $("#MemberPT").val();
                    //RacingSetting.updatePT(roleId);
                    $(this).dialog("close");
                },
                "NO": function () {
                    RacingSetting.isNewPT = false;
                    RacingSetting.newMemberPT = 0;
                    //RacingSetting.updatePT(roleId);
                    $(this).dialog("close");
                }
            },
            close: function (event, ui) {
                RacingSetting.updatePT();
            }
        });
        $('a.ui-dialog-titlebar-close').hide();
    },
    getIdPlace: function (IdWin) {
        return parseInt(IdWin) + 1;
    },
    checkValidBetSetting: function () {
        if (Roles.CurrRole == 1) {
            return this.checkValidBetMemberSetting();
        }
        var toteId;
        var minbet;
        var maxbet;
        var maxpermatch;
        var validminbet;
        var validmaxbet;
        var validmaxpermatch;
        var validvalue;
        var isValid = true;
        for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
            for (var j = 0; j < RacingSettingObj.RacingSportType[i].Totes.length; j++) {
                if (RacingSettingObj.RacingSportType[i].Totes[j].Visible) {
                    toteId = RacingSettingObj.RacingSportType[i].Totes[j].Id;
                    // check tote is hidden
                    var cur_tote = RacingSettingObj.RacingSportType[i].Totes[j];
                    if (!cur_tote.Visible) { continue; }
                    //Check Minbet
                    minbet = $("#" + toteId + "_txtMinbet").val();
                    validminbet = $("#" + toteId + "_txtMinbet_Min").html();
                    maxbet = $("#" + toteId + "_txtMaxbet").val();
                    validmaxbet = $("#" + toteId + "_txtMaxbet_Max").html();
                    maxpermatch = $("#" + toteId + "_txtMaxPerMatch").val();
                    validmaxpermatch = $("#" + toteId + "_txtMaxPerMatch_Max").html();

                    if (minbet == "" || !isNumber(minbet)) {
                        showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet, 1);
                        $("#" + toteId + "_txtMinbet").focus();
                        isValid = false;
                    }
                    else {
                        //alert(minbet + "|" + validminbet);
                        if (minbet > 0 && validminbet != null && parseFloat(minbet) < parseFloat(validminbet)) {
                            showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet2 + " " + validminbet, 1);
                            $("#" + toteId + "_txtMinbet").focus();
                            isValid = false;
                        }
                        //validvalue = Math.max(parseFloat(maxbet), parseFloat(validmaxbet));
                        if (isValid && minbet > 0 && parseFloat(minbet) > parseFloat(maxbet)) {
                            showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet1, 1);
                            $("#" + toteId + "_txtMinbet").focus();
                            isValid = false;
                        }
                    }

                    //Check Maxbet
                    if (maxbet == "" || !isNumber(maxbet)) {
                        showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet, 1);
                        $("#" + toteId + "_txtMaxbet").focus();
                        isValid = false;
                    }
                    else {
                        if (maxbet > 0 && validmaxbet != null && parseFloat(maxbet) > parseFloat(validmaxbet)) {
                            showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet2 + " " + validmaxbet, 1);
                            $("#" + toteId + "_txtMaxbet").focus();
                            isValid = false;
                        }

                        if (isValid && maxbet > 0 && parseFloat(maxbet) > parseFloat(maxpermatch)) {
                            showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet1, 1);
                            $("#" + toteId + "_txtMaxbet").focus();
                            isValid = false;
                        }
                    }

                    //Check Maxpermatch
                    if (maxpermatch == "" || !isNumber(maxpermatch)) {
                        showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxpermatch, 1);
                        $("#" + toteId + "_txtMaxPerMatch").focus();
                        isValid = false;
                    }
                    else {
                        if (maxpermatch > 0 && validmaxpermatch != null && parseFloat(maxpermatch) > parseFloat(validmaxpermatch)) {
                            showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxpermatch1 + " " + validmaxpermatch, 1);
                            $("#" + toteId + "_txtMaxPerMatch").focus();
                            isValid = false;
                        }
                    }
                }
            }
        }
        return isValid;
    },
    checkValidBetSettingSuper: function () {
        var toteId;
        var minbet;
        var maxbet;
        var maxpermatch;
        var validvalue;
        var isValid = true;
        for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
            for (var j = 0; j < RacingSettingObj.RacingSportType[i].Totes.length; j++) {
                if (RacingSettingObj.RacingSportType[i].Totes[j].Visible) {
                    toteId = RacingSettingObj.RacingSportType[i].Totes[j].Id;
                    // check tote is hidden
                    var cur_tote = RacingSettingObj.RacingSportType[i].Totes[j];
                    if (!cur_tote.Visible) { continue; }
                    //Check Minbet
                    minbet = $("#" + toteId + "_txtMinbet").val();
                    maxbet = $("#" + toteId + "_txtMaxbet").val();
                    maxpermatch = $("#" + toteId + "_txtMaxPerMatch").val();

                    if (minbet == "" || !isNumber(minbet)) {
                        showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet, 1);
                        $("#" + toteId + "_txtMinbet").focus();
                        isValid = false;
                    }
                    else {

                        if (parseFloat(minbet) > parseFloat(maxbet)) {
                            showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet1, 1);
                            $("#" + toteId + "_txtMinbet").focus();
                            isValid = false;
                        }
                    }

                    //Check Maxbet
                    if (maxbet == "" || !isNumber(maxbet)) {
                        showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet, 1);
                        $("#" + toteId + "_txtMaxbet").focus();
                        isValid = false;
                    }
                    else {
                        if (parseFloat(maxbet) > parseFloat(maxpermatch)) {
                            showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet1, 1);
                            $("#" + toteId + "_txtMaxbet").focus();
                            isValid = false;
                        }
                    }

                    //Check Maxpermatch
                    if (maxpermatch == "" || !isNumber(maxpermatch)) {
                        showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxpermatch, 1);
                        $("#" + toteId + "_txtMaxPerMatch").focus();
                        isValid = false;
                    }
                }
            }
        }
        return isValid;
    },
    setParamUpdateStatus: function () {
        var tote, sport;
        var id, status151_41 = { Enable: false, EnableDownline: false, UpdateDisable: false };
        for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
            sport = RacingSettingObj.RacingSportType[i];
            if (!sport.Visible) { sportHidden.addSport(sport); }
            for (var j = 0; j < sport.Totes.length; j++) {
                tote = sport.Totes[j];
                tote.Enable = $("#" + tote.Id + "_" + chkEnable).attr("checked");
                tote.EnableDownline = $("#" + tote.Id + "_" + chkEnableDownline).attr("checked");

                var oldvalue = $("#" + tote.Id + "_" + chkEnable).attr("value") == "true" ? true : false;
                tote.UpdateDisable = false;
                if (tote.Enable != oldvalue) {
                    tote.UpdateDisable = true;
                }
                if (tote.Id.toString().indexOf('151_41') != -1) {
                    status151_41.Enable = tote.Enable;
                    status151_41.EnableDownline = tote.EnableDownline;
                    status151_41.UpdateDisable = tote.UpdateDisable;
                }
                // check sport | tote hidden
                if (!sport.Visible || !tote.Visible) {
                    tote.Enable = status151_41.Enable;
                    tote.EnableDownline = status151_41.EnableDownline;
                    tote.UpdateDisable = status151_41.UpdateDisable;
                }
            }
        }
    },
    setParamUpdatePT: function () {
        var bettype, tote, sport;
        var id, status151_41 = { Enable: false, EnableDownline: false, UpdateDisable: false };
        var isCopyAll = $("#" + chkCopyAllSetting).attr("checked");
        var isCopyfromWin;
        var housept, comm, minbet, maxbet, maxpermatch, pt, mpt, apt, isauto, uplinept, autopt, ptUK, mptUK, aptUK, isautoUK, uplineptUK, autoptUK;
        if (Roles.CurrRole == Roles.Master || Roles.CurrRole == Roles.Agent) {
            RacingSettingObj.IsUpdateAllMember = RacingSetting.isNewPT;
            RacingSettingObj.NewPt = RacingSetting.newMemberPT;
        }

        for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
            sport = RacingSettingObj.RacingSportType[i];
            for (var j = 0; j < sport.Totes.length; j++) {
                tote = sport.Totes[j];
                tote.Enable = $("#" + tote.Id + "_" + chkEnable).attr("checked");
                tote.EnableDownline = $("#" + tote.Id + "_" + chkEnableDownline).attr("checked");

                // check is update status
                var oldvalue = $("#" + tote.Id + "_" + chkEnable).attr("value") == "true" ? true : false;
                if (tote.Enable != oldvalue) {
                    tote.UpdateDisable = true;
                }

                //check for copy status
                if (tote.Id.toString().indexOf('151_41') != -1) {
                    status151_41.Enable = tote.Enable;
                    status151_41.EnableDownline = tote.EnableDownline;
                    status151_41.UpdateDisable = tote.UpdateDisable;
                }
                // check sport | tote hidden
                if (!sport.Visible || !tote.Visible) {
                    tote.Enable = status151_41.Enable;
                    tote.EnableDownline = status151_41.EnableDownline;
                    tote.UpdateDisable = status151_41.UpdateDisable;
                }

                //set deafault update PT
                tote.UpdatePT = false;

                if (i == 0 && j == 0) {
                    housept = $("#" + tote.Id + "_" + IBCPositiontaking).val();
                    comm = $("#" + tote.Id + "_" + commission).val();
                }

                if (isCopyAll) {
                    if (!this.isSATote(tote)) {
                        tote.Comm = comm;
                        tote.HousePt = housept;
                    }
                }
                else {
                    tote.Comm = $("#" + tote.Id + "_" + commission).val();
                    tote.HousePt = $("#" + tote.Id + "_" + IBCPositiontaking).val();
                }

                // check old house PT
                var oldhousept = $("#" + tote.Id + "_" + IBCPositiontaking).attr('old');
                if (tote.HousePt != oldhousept) {
                    tote.UpdatePT = true;
                }

                // check is update commission
                var oldcomm = $("#" + tote.Id + "_" + commission).attr("old");
                if (tote.Comm != oldcomm) {
                    tote.UpdateComm = true;
                }


                // get min|max|maxpermatch
                tote.MinBet = $("#" + tote.Id + "_" + txtMinbet).val();
                tote.MaxBet = $("#" + tote.Id + "_" + txtMaxbet).val(); ;
                tote.MaxPerMatch = $("#" + tote.Id + "_" + txtMaxPerMatch).val();

                // check is update min|max|maxpermatch
                var oldmin = $("#" + tote.Id + "_" + txtMinbet).attr("old");
                var oldmax = $("#" + tote.Id + "_" + txtMaxbet).attr("old");
                var oldmatch = $("#" + tote.Id + "_" + txtMaxPerMatch).attr("old");
                if (tote.MinBet != oldmin || tote.MaxBet != oldmax || tote.MaxPerMatch != oldmatch) {
                    tote.UpdateMinMax = true;
                }


                //Check Apply PT to all Member at Master and Level
                if (Roles.CurrRole == Roles.Master || Roles.CurrRole == Roles.Agent) {
                    tote.IsApplyPresetPT = $("#" + tote.Id + "_" + chkApplyToMember).attr("checked");
                    if (tote.IsApplyPresetPT) {
                        tote.UpdatePT = true;
                    }
                }
                isCopyfromWin = $("#" + tote.Id + "_" + chkCopyFromWin).attr("checked");

                for (var k = 0; k < tote.Bettypes.length; k++) {
                    bettype = tote.Bettypes[k];
                    id = sport.Id + "_" + bettype.Id;
                    switch (Roles.CurrRole) {
                        case Roles.Super: //*******Super params*****
                            if (i == 0 && j == 0 && k == 0) {
                                pt = $("#" + id + "_" + sposition).val();
                                mpt = $("#" + id + "_" + mposition).val();
                                apt = $("#" + id + "_" + aposition).val();
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.Pt = pt;
                                    bettype.PositionTaking.MasterPt = mpt;
                                    bettype.PositionTaking.AgentPt = apt;
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    ptUK = $("#" + id + "_" + sposition).val();
                                    mptUK = $("#" + id + "_" + mposition).val();
                                    aptUK = $("#" + id + "_" + aposition).val();
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.Pt = ptUK;
                                    bettype.PositionTaking.MasterPt = mptUK;
                                    bettype.PositionTaking.AgentPt = aptUK;
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.Pt = $("#" + id + "_" + sposition).val();
                                    bettype.PositionTaking.MasterPt = $("#" + id + "_" + mposition).val();
                                    bettype.PositionTaking.AgentPt = $("#" + id + "_" + aposition).val();
                                    // check is update pt
                                    var oldpt = $("#" + id + "_" + sposition).attr('old');
                                    var oldmpt = $("#" + id + "_" + mposition).attr('old');
                                    var oldapt = $("#" + id + "_" + aposition).attr('old');
                                    if (bettype.PositionTaking.Pt != oldpt || bettype.PositionTaking.MasterPt != oldmpt || bettype.PositionTaking.AgentPt != oldapt) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.Master: //*******Master params*****

                            if (i == 0 && j == 0 && k == 0) {
                                pt = $("#" + id + "_" + mposition).val();
                                isauto = $("#" + id + "_" + spositiontaking_isauto).attr("checked");
                                if (isauto) {
                                    autopt = $("#" + id + "_" + spositiontaking_auto).val();
                                }
                                else {
                                    uplinept = $("#" + id + "_" + sposition).val();
                                }
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.Pt = pt;
                                    bettype.PositionTaking.IsAutoPt = isauto;
                                    if (isauto) {
                                        bettype.PositionTaking.AutoPt = autopt;
                                    }
                                    else {
                                        bettype.PositionTaking.UplinePt = uplinept;
                                    }
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    ptUK = $("#" + id + "_" + mposition).val();
                                    isautoUK = $("#" + id + "_" + spositiontaking_isauto).attr("checked");
                                    if (isautoUK) {
                                        autoptUK = $("#" + id + "_" + spositiontaking_auto).val();
                                    }
                                    else {
                                        uplineptUK = $("#" + id + "_" + sposition).val();
                                    }
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.Pt = ptUK;
                                    bettype.PositionTaking.IsAutoPt = isautoUK;
                                    if (isautoUK) {
                                        bettype.PositionTaking.AutoPt = autoptUK;
                                    }
                                    else {
                                        bettype.PositionTaking.UplinePt = uplineptUK;
                                    }
                                    tote.UpdatePT = true;

                                }
                                else {
                                    bettype.PositionTaking.Pt = $("#" + id + "_" + mposition).val();
                                    bettype.PositionTaking.IsAutoPt = $("#" + id + "_" + spositiontaking_isauto).attr("checked");
                                    if (bettype.PositionTaking.IsAutoPt) {
                                        bettype.PositionTaking.AutoPt = $("#" + id + "_" + spositiontaking_auto).val();
                                    }
                                    else {
                                        bettype.PositionTaking.UplinePt = $("#" + id + "_" + sposition).val();
                                    }
                                    var oldisautopt = $("#" + id + "_" + spositiontaking_isauto).attr('old');
                                    if (bettype.PositionTaking.IsAutoPt.toString() != oldisautopt.toString()) {
                                        tote.UpdatePT = true;
                                    }
                                    // check is update master pt
                                    var oldmpt = $("#" + id + "_" + mposition).attr('old');
                                    var oldspt = $("#" + id + "_" + sposition).attr('old');
                                    var oldautopt = $("#" + id + "_" + spositiontaking_auto).attr('old');
                                    if (bettype.PositionTaking.Pt != oldmpt || bettype.PositionTaking.UplinePt != oldspt || bettype.PositionTaking.AutoPt != oldautopt) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.Agent: //*******Agent params*****
                            if (i == 0 && j == 0 && k == 0) {
                                pt = $("#" + id + "_" + aposition).val();
                                isauto = $("#" + id + "_" + mpositiontaking_isauto).attr("checked");
                                if (isauto) {
                                    autopt = $("#" + id + "_" + mpositiontaking_auto).val();
                                }
                                else {
                                    uplinept = $("#" + id + "_" + mposition).val();
                                }
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.Pt = pt;
                                    bettype.PositionTaking.IsAutoPt = isauto;
                                    if (isauto) {
                                        bettype.PositionTaking.AutoPt = autopt;
                                    }
                                    else {
                                        bettype.PositionTaking.UplinePt = uplinept;
                                    }
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    ptUK = $("#" + id + "_" + aposition).val();
                                    isautoUK = $("#" + id + "_" + mpositiontaking_isauto).attr("checked");
                                    if (isautoUK) {
                                        autoptUK = $("#" + id + "_" + mpositiontaking_auto).val();
                                    }
                                    else {
                                        uplineptUK = $("#" + id + "_" + mposition).val();
                                    }
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.Pt = ptUK;
                                    bettype.PositionTaking.IsAutoPt = isautoUK;
                                    if (isautoUK) {
                                        bettype.PositionTaking.AutoPt = autoptUK;
                                    }
                                    else {
                                        bettype.PositionTaking.UplinePt = uplineptUK;
                                    }
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.Pt = $("#" + id + "_" + aposition).val();
                                    bettype.PositionTaking.IsAutoPt = $("#" + id + "_" + mpositiontaking_isauto).attr("checked");
                                    if (bettype.PositionTaking.IsAutoPt) {
                                        bettype.PositionTaking.AutoPt = $("#" + id + "_" + mpositiontaking_auto).val();
                                    }
                                    else {
                                        bettype.PositionTaking.UplinePt = $("#" + id + "_" + mposition).val();
                                    }
                                    var oldisautopt = $("#" + id + "_" + mpositiontaking_isauto).attr('old');
                                    if (bettype.PositionTaking.IsAutoPt.toString() != oldisautopt.toString()) {
                                        tote.UpdatePT = true;
                                    }
                                    // check is update agent pt
                                    var oldapt = $("#" + id + "_" + aposition).attr('old');
                                    var oldmpt = $("#" + id + "_" + mposition).attr('old');
                                    var oldautopt = $("#" + id + "_" + mpositiontaking_auto).attr('old');
                                    if (bettype.PositionTaking.Pt != oldapt || bettype.PositionTaking.UplinePt != oldmpt || bettype.PositionTaking.AutoPt != oldautopt) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.Member: //*******Member params*****
                            if (i == 0 && j == 0 && k == 0) {
                                pt = $("#" + id + "_" + aposition).val();
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.Pt = pt;
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    ptUK = $("#" + id + "_" + aposition).val();
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.Pt = ptUK;
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.Pt = $("#" + id + "_" + aposition).val();
                                    var oldpt = $("#" + id + "_" + aposition).attr('old');
                                    if (bettype.PositionTaking.Pt != oldpt) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.SuperOnMember: //*******Super on Member params*****
                            if (i == 0 && j == 0 && k == 0) {
                                isauto = $("#" + id + "_" + spositiontaking_isauto).attr("checked");
                                if (isauto) {
                                    autopt = $("#" + id + "_" + spositiontaking_auto).val();
                                }
                                else {
                                    pt = $("#" + id + "_" + sposition).val();
                                }
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.IsAutoPt = isauto;
                                    if (isauto) {
                                        bettype.PositionTaking.AutoPt = autopt;
                                    }
                                    else {
                                        bettype.PositionTaking.Pt = pt;
                                    }
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    isautoUK = $("#" + id + "_" + spositiontaking_isauto).attr("checked");
                                    if (isautoUK) {
                                        autoptUK = $("#" + id + "_" + spositiontaking_auto).val();
                                    }
                                    else {
                                        ptUK = $("#" + id + "_" + sposition).val();
                                    }
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.IsAutoPt = isautoUK;
                                    if (isautoUK) {
                                        bettype.PositionTaking.AutoPt = autoptUK;
                                    }
                                    else {
                                        bettype.PositionTaking.Pt = ptUK;
                                    }
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.IsAutoPt = $("#" + id + "_" + spositiontaking_isauto).attr("checked");
                                    if (bettype.PositionTaking.IsAutoPt) {
                                        bettype.PositionTaking.AutoPt = $("#" + id + "_" + spositiontaking_auto).val();
                                    }
                                    else {
                                        bettype.PositionTaking.Pt = $("#" + id + "_" + sposition).val();
                                    }
                                    var oldisautopt = $("#" + id + "_" + spositiontaking_isauto).attr('old');
                                    if (bettype.PositionTaking.IsAutoPt.toString() != oldisautopt.toString()) {
                                        tote.UpdatePT = true;
                                    }
                                    // check is update super on member
                                    var oldpt = $("#" + id + "_" + sposition).attr('old');
                                    var oldautopt = $("#" + id + "_" + spositiontaking_auto).attr('old');
                                    if (bettype.PositionTaking.Pt != oldpt || bettype.PositionTaking.AutoPt != oldautopt) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.MasterOnMember: //*******Master on Member params*****
                            if (i == 0 && j == 0 && k == 0) {
                                isauto = $("#" + id + "_" + mpositiontaking_isauto).attr("checked");
                                if (isauto) {
                                    autopt = $("#" + id + "_" + mpositiontaking_auto).val();
                                }
                                else {
                                    pt = $("#" + id + "_" + mposition).val();
                                }
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.IsAutoPt = isauto;
                                    if (isauto) {
                                        bettype.PositionTaking.AutoPt = autopt;
                                    }
                                    else {
                                        bettype.PositionTaking.Pt = pt;
                                    }
                                    bettype.PositionTaking.Pt = pt;
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    isautoUK = $("#" + id + "_" + mpositiontaking_isauto).attr("checked");
                                    if (isautoUK) {
                                        autoptUK = $("#" + id + "_" + mpositiontaking_auto).val();
                                    }
                                    else {
                                        ptUK = $("#" + id + "_" + mposition).val();
                                    }
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.IsAutoPt = isautoUK;
                                    if (isautoUK) {
                                        bettype.PositionTaking.AutoPt = autoptUK;
                                    }
                                    else {
                                        bettype.PositionTaking.Pt = ptUK;
                                    }
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.IsAutoPt = $("#" + id + "_" + mpositiontaking_isauto).attr("checked");
                                    if (bettype.PositionTaking.IsAutoPt) {
                                        bettype.PositionTaking.AutoPt = $("#" + id + "_" + mpositiontaking_auto).val();
                                    }
                                    else {
                                        bettype.PositionTaking.Pt = $("#" + id + "_" + mposition).val();
                                    }
                                    var oldisautopt = $("#" + id + "_" + mpositiontaking_isauto).attr('old');
                                    if (bettype.PositionTaking.IsAutoPt.toString() != oldisautopt.toString()) {
                                        tote.UpdatePT = true;
                                    }
                                    // check is update super on member
                                    var oldpt = $("#" + id + "_" + mposition).attr('old');
                                    var oldautopt = $("#" + id + "_" + mpositiontaking_auto).attr('old');
                                    if (bettype.PositionTaking.Pt != oldpt || bettype.PositionTaking.AutoPt != oldautopt) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.SuperMin: //*******Super Min params*****
                            if (i == 0 && j == 0 && k == 0) {
                                pt = $("#" + id + "_minpt").val();
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.Pt = pt;
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    ptUK = $("#" + id + "_minpt").val();
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.Pt = ptUK;
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.Pt = $("#" + id + "_minpt").val();
                                    // check is update super min
                                    var oldpt = $("#" + id + "_minpt").attr('old');
                                    if (bettype.PositionTaking.Pt != oldpt) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.MasterMinMax: //*******Master Min/Max params*****
                            if (i == 0 && j == 0 && k == 0) {
                                pt = $("#" + id + "_minpt").val();
                                uplinept = $("#" + id + "_maxpt").val();
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.Pt = pt;
                                    bettype.PositionTaking.UplinePt = uplinept;
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    ptUK = $("#" + id + "_minpt").val();
                                    uplineptUK = $("#" + id + "_maxpt").val();
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.Pt = ptUK;
                                    bettype.PositionTaking.UplinePt = uplineptUK;
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.Pt = $("#" + id + "_minpt").val();
                                    bettype.PositionTaking.UplinePt = $("#" + id + "_maxpt").val();
                                    // check is update master min/max
                                    var oldmin = $("#" + id + "_minpt").attr('old');
                                    var oldmax = $("#" + id + "_maxpt").attr('old');
                                    if (bettype.PositionTaking.Pt != oldmin || bettype.PositionTaking.UplinePt != oldmax) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        case Roles.AgentMinMax: //*******Agent Min/Max params*****
                            if (i == 0 && j == 0 && k == 0) {
                                pt = $("#" + id + "_minpt").val();
                                uplinept = $("#" + id + "_maxpt").val();
                            }
                            if (isCopyAll) {
                                if (!this.isSATote(tote)) {
                                    bettype.PositionTaking.Pt = pt;
                                    bettype.PositionTaking.UplinePt = uplinept;
                                }
                                tote.UpdatePT = true;
                            }
                            else {
                                if (k == 0) {
                                    ptUK = $("#" + id + "_minpt").val();
                                    uplineptUK = $("#" + id + "_maxpt").val();
                                }

                                if (isCopyfromWin) {
                                    bettype.PositionTaking.Pt = ptUK;
                                    bettype.PositionTaking.UplinePt = uplineptUK;
                                    tote.UpdatePT = true;
                                }
                                else {
                                    bettype.PositionTaking.Pt = $("#" + id + "_minpt").val();
                                    bettype.PositionTaking.UplinePt = $("#" + id + "_maxpt").val();
                                    // check is update master min/max
                                    var oldmin = $("#" + id + "_minpt").attr('old');
                                    var oldmax = $("#" + id + "_maxpt").attr('old');
                                    if (bettype.PositionTaking.Pt != oldmin || bettype.PositionTaking.UplinePt != oldmax) {
                                        tote.UpdatePT = true;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    // set racing 151
                    if (i == 0 && j == 0) {
                        sport151.setBettype(bettype);
                    }
                    // check racing is hidden 
                    if (!sport.Visible || !tote.Visible) {
                        if (!sport.Visible) { sportHidden.addSport(sport); }
                        bettype = sport151.getBettype(bettype);
                    }
                }
                // set comm racing 151
                if (i == 0 && j == 0) {
                    tote151.setComm(tote);
                }
                // check copy comm to hidden tote
                if (!sport.Visible || !tote.Visible) {
                    tote.Comm = tote151.getComm(tote);
                    tote.HousePt = tote151.getHousePt(tote);
                }
            }
        }
    },
    checkUpdateToAllMember: function () {//Used at master and agent level
        var bettype, tote, sport, id;
        for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
            sport = RacingSettingObj.RacingSportType[i];
            for (var j = 0; j < sport.Totes.length; j++) {
                tote = sport.Totes[j];
                for (var k = 0; k < tote.Bettypes.length; k++) {
                    bettype = tote.Bettypes[k];
                    id = sport.Id + "_" + bettype.Id;
                    if (Roles.CurrRole == Roles.Master) {
                        if ($("#" + id + "_" + spositiontaking_isauto).attr("value") == "true" && $("#" + id + "_" + spositiontaking_isauto).attr("checked") == false) {
                            var maxPT = roundNumber($("#" + id + "_" + sposition).attr("maxLine") - $("#" + id + "_" + mposition).val(), 2);
                            CreateOption($("#MemberPT"), maxPT, maxPT, 0, 0.1, 2);
                            $("#dialogMemberPT").dialog("open");
                            return;
                        }
                    }
                    else {
                        if ($("#" + id + "_" + mpositiontaking_isauto).attr("value") == "true" && $("#" + id + "_" + mpositiontaking_isauto).attr("checked") == false) {
                            var maxPT = roundNumber($("#" + id + "_" + mposition).attr("maxLine") - $("#" + id + "_" + aposition).val(), 2);
                            CreateOption($("#MemberPT"), maxPT, maxPT, 0, 0.1, 2);
                            $("#dialogMemberPT").dialog("open");
                            return;
                        }
                    }
                }
            }
        }
        //Update PT
        RacingSetting.updatePT();
    },
    checkValidBetMemberSetting: function () {
        var toteId = null;
        var minbet = null;
        var maxbet = null;
        var maxpermatch = null;
        var validminbet = null;
        var validmaxbet = null;
        var validmaxpermatch = null;
        var isValid = true;
        for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
            for (var j = 0; j < RacingSettingObj.RacingSportType[i].Totes.length; j++) {
                toteId = RacingSettingObj.RacingSportType[i].Totes[j].Id;
                // check tote is hidden
                var cur_tote = RacingSettingObj.RacingSportType[i].Totes[j];
                if (!cur_tote.Visible) { continue; }
                //Check Minbet
                minbet = $("#" + toteId + "_txtMinbet").val();
                validminbet = $("#" + toteId + "_txtMinbet_Min").html();
                maxbet = $("#" + toteId + "_txtMaxbet").val();
                validmaxbet = $("#" + toteId + "_txtMaxbet_Max").html();
                maxpermatch = $("#" + toteId + "_txtMaxPerMatch").val();
                validmaxpermatch = $("#" + toteId + "_txtMaxPerMatch_Max").html();

                //Check Minbet
                if (minbet == "" || !isNumber(minbet)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet, 1);
                    $("#" + toteId + "_txtMinbet").focus();
                    isValid = false;
                }

                //Check Maxbet
                if (maxbet == "" || !isNumber(maxbet)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet, 1);
                    $("#" + toteId + "_txtMaxbet").focus();
                    isValid = false;
                }

                //Check Maxpermatch
                if (maxpermatch == "" || !isNumber(maxpermatch)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxpermatch, 1);
                    $("#" + toteId + "_txtMaxPerMatch").focus();
                    isValid = false;
                }
                if (!isValid) { break; }
                if (minbet == 0 && maxbet == 0 && maxpermatch == 0) {
                    isValid = true;
                    break;
                }
                // valid min bet & 0
                /*if (minbet <= 0) {
                showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet, 1);
                $("#" + toteId + "_txtMinbet").focus();
                isValid = false;
                }*/
                // valid minbet & mimbet_min
                if (isValid && validminbet != null && parseFloat(minbet) < parseFloat(validminbet)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet2 + " " + validminbet, 1);
                    $("#" + toteId + "_txtMinbet").focus();
                    isValid = false;
                }
                // valid minbet & maxbet
                if (isValid && minbet > 0 && parseFloat(minbet) > parseFloat(maxbet)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMinbet1, 1);
                    $("#" + toteId + "_txtMinbet").focus();
                    isValid = false;
                }

                if (!isValid) { break; }
                // valid maxbet & 0
                /*if (maxbet <= 0) {
                showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet, 1);
                $("#" + toteId + "_txtMaxbet").focus();
                isValid = false;
                }*/
                // valid maxbet & maxbet_max
                if (isValid && validmaxbet != null && parseFloat(maxbet) > parseFloat(validmaxbet)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet2 + " " + validmaxbet, 1);
                    $("#" + toteId + "_txtMaxbet").focus();
                    isValid = false;
                }
                // valid maxbet & maxpermatch
                if (isValid && parseFloat(maxbet) > parseFloat(maxpermatch)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxbet1, 1);
                    $("#" + toteId + "_txtMaxbet").focus();
                    isValid = false;
                }
                if (!isValid) { break; }
                //valid maxpermatch & 0
                if (maxpermatch <= 0) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxpermatch, 1);
                    $("#" + toteId + "_txtMaxPerMatch").focus();
                    isValid = false;
                }
                //valid maxpermatch & maxpermatch_max
                if (isValid && validmaxpermatch != null && parseFloat(maxpermatch) > parseFloat(validmaxpermatch)) {
                    showAlertMessage(toteId + "_errmsg", RacingMsg.alertMaxpermatch1 + " " + validmaxpermatch, 1);
                    $("#" + toteId + "_txtMaxPerMatch").focus();
                    isValid = false;
                }
            }
        }
        return isValid;
    },
    isSATote: function (tote) {
        if (typeof tote != 'undefined') {
            if (!tote.Visible && (tote.Id.toString().indexOf("31") != -1 || tote.Id.toString().indexOf("32") != -1)) {
                return true;
            }
        }
        return false;
    },
    validCopyAll: function () {
        try {
            var sport151 = RacingSettingObj.RacingSportType[0].Totes[0];
            var bettypes = sport151.Bettypes;
            if (typeof bettypes != 'undefined') {
                var _betWin = bettypes[0].PositionTaking;
                var _betPlace = bettypes[1].PositionTaking;
                if (_betWin.IsUplineAutoPt && !_betWin.IsAutoPt) {
                    $("#chkCopyAllSetting").attr("disabled", "disabled");
                }
                if (_betWin.IsAutoPt) {
                    $("#chkCopyAllSetting").removeAttr("disabled");
                }
                for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
                    var sport = RacingSettingObj.RacingSportType[i];
                    for (var j = 0; j < sport.Totes.length; j++) {
                        var tote = sport.Totes[j];
                        if (!tote.Visible) continue;
                        var bettype_win = tote.Bettypes[0].PositionTaking; // check bettype win
                        if (bettype_win.IsAutoPt) {
                            $("#" + tote.Id + "_chkCopyFromWin").removeAttr("disabled");
                        }
                    }
                }
            }
        } catch (e) { }
    }
};
// sport 151
var sport151 = {
    bettype: [{ 41: null }, { 42: null}],
    setBettype: function (bettype) {
        var _bettype_id = bettype.Id;
        var _bettype_name = (_bettype_id % 2 != 0) ? 'win' : 'place';
        switch (_bettype_name) {
            case 'win': //41
                this.bettype[41] = bettype;
                break;
            case 'place': //42
                this.bettype[42] = bettype;
                break;
            default:
                break;
        }
        return false;
    },
    getBettype: function (bettype) {
        var _bettype_id = bettype.Id;
        // not check hidden by curency : SA Tote
        if (_bettype_id == 31 || _bettype_id == 32) { return bettype; }
        var _bettype_name = (_bettype_id % 2 != 0) ? 'win' : 'place';
        switch (_bettype_name) {
            case 'win': //41
                bettype = $.extend(bettype, this.bettype[41]);
                break;
            case 'place': //42
                bettype = $.extend(bettype, this.bettype[42]);
                break;
            default:
                break;
        }
        bettype.Id = _bettype_id;
        return bettype;
    }
};
// tote 151
var tote151 = {
    tote: { Comm: 0, HousePt: 0 },
    setComm: function (tote) {
        if (typeof tote != 'undefined') {
            this.tote.Comm = tote.Comm;
            this.tote.HousePt = tote.HousePt;
        }
        return false;
    },
    getComm: function (tote) {
        // not check hidden by curency : SA Tote
        if (tote.Id.indexOf('31') != -1 || tote.Id.indexOf('32') != -1) { return 0; }
        return this.tote.Comm;
    },
    getHousePt: function (tote) {
        // not check hidden by curency : SA Tote
        if (tote.Id.indexOf('31') != -1 || tote.Id.indexOf('32') != -1) { return 0; }
        return this.tote.HousePt;
    }
};
// sport hidden
var sportHidden = {
    sports: [],
    addSport: function (sport) {
        if ($.inArray(sport.Name, this.sports) == -1)
            this.sports.push(sport.Name);
        return false;
    }
};

function getId(index) {
    return this.parent.parent.data.Id + "_" + this.data.Bettypes[index].Id;
}
function getToteId() {
    return this.data.Id;
}
function reloadData(custId) {
    RacingData.getCustomerSetting(custId, RacingSetting.init);
}
function ChkCopyAllBetSetting(obj) {
    var isCopied = $(obj).attr("checked");
    var sportId;
    var winId;
    var placeId;
    
    for (var i = 0; i < RacingSettingObj.RacingSportType.length; i++) {
        sportId = RacingSettingObj.RacingSportType[i].Id;
        for (var j = 0; j < RacingSettingObj.RacingSportType[i].Totes.length; j++) {
            var obj = RacingSettingObj.RacingSportType[i].Totes[j];
            winId = sportId + "_" + obj.Bettypes[0].Id;
            placeId = sportId + "_" + obj.Bettypes[1].Id;
            if (obj.Bettypes.length > 0 && (winId == RacingSettingObj.DefaultRacingSportTypeId)) {
                if (isCopied) {
                    $("#" + winId + "_" + txtMinbet).attr("disabled", "disabled");
                    $("#" + winId + "_" + txtMaxbet).attr("disabled", "disabled");
                    $("#" + winId + "_" + txtMaxPerMatch).attr("disabled", "disabled");
                }
                else {
                    $("#" + placeId + "_" + txtMinbet).removeAttr("disabled");
                    $("#" + placeId + "_" + txtMaxbet).removeAttr("disabled");
                    $("#" + placeId + "_" + txtMaxPerMatch).removeAttr("disabled");
                }
            }
        }
    }
}

