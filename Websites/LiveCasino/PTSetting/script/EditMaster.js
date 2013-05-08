var casinoManager = {
    gameType: function (id, name, bettingLimit, betTypeId) {
        this.Id = id;
        this.Name = name;
        this.BettingLimit = bettingLimit;
        this.BetTypeId = betTypeId;
    },
    BettingLimit: function (s) {
        this.IsSelected = ko.observable(s.IsSelected);
        this.Text = s.Text;
        this.Id = s.Id;
    },
    PTSetting: function (b) {
        this.MinPT = ko.observable(b.MinPT);
        this.MaxPT = ko.observable(b.MaxPT);
        this.Comm = ko.observable(b.Comm);

        this.Upline = b.Upline;
        this.Upline.PresetPT = ko.observable(b.Upline.PresetPT);
        this.Upline.AutoPresetPT = ko.observable(b.Upline.AutoPresetPT);
        this.Upline.IsAutoPresetPT = ko.observable(b.Upline.IsAutoPresetPT);
        this.Upline.IsUpdateAllMember = 0;
        this.Upline.NewMemberPT = 0;

    },
    WinLimitSetting: function (w) {
        this.IsAutoResetLiveCasino = ko.observable(w.IsAutoResetLiveCasino);
        this.UplineIsAutoResetLiveCasino = w.UplineIsAutoResetLiveCasino;

        var limits = [];
        var totalItems = w.Limits.length;
        for (var i = 0; i < totalItems; i++) {
            var item = w.Limits[i];
            if (item.IsSelected) {
                this.MaxWinLiveCasino = ko.observable(item.Id);
            }
            limits.push(new casinoManager.Limit(item));
        }
        this.Limits = ko.observableArray(limits);
    },
    Limit: function (l) {
        this.Id = l.Id;
        this.IsSelected = l.IsSelected;
        this.Text = l.Text;
    },
    casinoVM: {
        CustomerInfo: {
            CustomerId: '',
            RoleId: '',
            UserName: '',
            NickName: '',
            FirstName: '',
            LastName: '',
            Phone: '',
            Mobile: '',
            IsSyncLiveCasino: true
        },
        requestContext: {
            UserId: 0,
            Ip: '',
            Language: '',
            CustId: 0,
            SubAccId: 0,
            UplineId: 0,
            AgentId: 0,
            MasterId: 0,
            SuperId: 0
        },
        PTSetting: {},
        GameSetting: ko.observableArray(),
        WinLimitSetting: {}
    },
    isDisabled: false,
    casinoDefaultData: {},
    createDependentObservable: function () {
        casinoManager.casinoVM.isAllowUpdatePtSetting = ko.dependentObservable(function () {
            if (!casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
                return true;
            }
            if (parseFloat(casinoManager.casinoVM.PTSetting.MinPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.MinPT) ||
                parseFloat(casinoManager.casinoVM.PTSetting.MaxPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.MaxPT) ||
                    parseFloat(casinoManager.casinoVM.PTSetting.Comm()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Comm) ||
                        parseFloat(casinoManager.casinoVM.PTSetting.Upline.PresetPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Upline.PresetPT) ||
                            parseFloat(casinoManager.casinoVM.PTSetting.Upline.AutoPresetPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Upline.AutoPresetPT) ||
                                casinoManager.casinoVM.PTSetting.Upline.IsAutoPresetPT() != casinoManager.casinoDefaultData.PTSetting.Upline.IsAutoPresetPT)
            { return true; }

            return false;

        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAllowUpdateGameSetting = ko.dependentObservable(function () {
            if (!casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
                return true;
            }
            if (casinoManager.casinoVM.GameSetting()) {
                for (var i = 0; i < casinoManager.casinoVM.GameSetting().length; i++) {
                    var item = casinoManager.casinoVM.GameSetting()[i];
                    var itemDefaut = casinoManager.casinoDefaultData.GameSetting[i];
                    for (var j = 0; j < item.BettingLimit.length; j++) {
                        if (item.BettingLimit[j].IsSelected() != itemDefaut.BettingLimit[j].IsSelected)
                            return true;
                    }
                }
                return false;
            }
            return false;
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAutoReset = ko.dependentObservable(function () {
            return (casinoManager.casinoVM.PTSetting.Upline.IsAutoPresetPT());
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isNotAutoReset = ko.dependentObservable(function () {
            return !(casinoManager.casinoVM.PTSetting.Upline.IsAutoPresetPT());
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAllowUpdateWinLimit = ko.dependentObservable(function () {
            if (!casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
                return true;
            }
            if (casinoManager.casinoVM.WinLimitSetting.Limits() && casinoManager.casinoVM.WinLimitSetting.Limits().length > 0) {
                var totalLimits = casinoManager.casinoVM.WinLimitSetting.Limits().length;
                var MaxWinLiveCasino = casinoManager.casinoVM.WinLimitSetting.MaxWinLiveCasino();
                for (var i = 0; i < totalLimits; i++) {
                    var item = casinoManager.casinoVM.WinLimitSetting.Limits()[i];
                    if (item.IsSelected && item.Id != MaxWinLiveCasino) {
                        return true;
                    }
                }
            }
            if (casinoManager.casinoVM.WinLimitSetting.IsAutoResetLiveCasino() != casinoManager.casinoDefaultData.WinLimitSetting.IsAutoResetLiveCasino)
                return true;
            return false;
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAllowUpdate = ko.dependentObservable(function () {
            if (casinoManager.casinoVM.isAllowUpdateGameSetting() || casinoManager.casinoVM.isAllowUpdatePtSetting() || casinoManager.casinoVM.isAllowUpdateWinLimit())
                return true;
            return false;
        }, casinoManager.casinoVM);
    },
    mappingcasinoData: function (data, custid) {
        casinoManager.casinoDefaultData = $.extend(true, {}, data);
        casinoManager.casinoVM.CustomerInfo = data.CustomerInfo;
        casinoManager.casinoVM.CustomerInfo.CustomerId = custid;

        casinoManager.casinoVM.PTSetting = new casinoManager.PTSetting(data.PTSetting);
        casinoManager.casinoVM.WinLimitSetting = new casinoManager.WinLimitSetting(data.WinLimitSetting);

        casinoManager.casinoVM.GameSetting.removeAll();
        for (var j = 0; j < data.GameSetting.length; j++) {
            var _stakeLevels = [];
            var _gameType = data.GameSetting[j];
            for (var k = 0; k < _gameType.BettingLimit.length; k++) {
                _stakeLevels.push(new casinoManager.BettingLimit(_gameType.BettingLimit[k]));
            }
            casinoManager.casinoVM.GameSetting.push(new casinoManager.gameType(_gameType.Id, _gameType.Name, _stakeLevels, _gameType.BetTypeId));
        }
    },
    initcasino: function () {
        $("#err").empty();
        ko.applyBindings(casinoManager.casinoVM, document.getElementById("SettingContent"));

    }
};

var editMaster = {
    custId: 0,
    uplineId: 0,
    isReloaded: false,
    isFirstSuccess: true,
    msgUncheckBettingLimit: '',
    requestContext: { UserId: 0, CustId: 0, SubAccId: 0, UplineId: 0, Ip: '', Language: '', IseditMaster: false, AgentId: 0, MasterId: 0, SuperId: 0, UplineRoleId: 4 },
    getSettings: function () {
        try { parent.BlockUI(); } catch (e) { }
        var param = { custId: editMaster.custId, uplineId: editMaster.uplineId };
        $.ajax({
            type: "POST",
            url: "service/SettingService.svc/GetMasterSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs && rs.PTSetting) {
                    casinoManager.mappingcasinoData(rs, editMaster.custId);
                    casinoManager.createDependentObservable();
                    casinoManager.initcasino();
                }
                else {
                    showAlertMessage('err', 'There is no infomation available.', 1);
                }
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });

    },
    updateLiveCasino: function (typeUpdate) {
        var confirmPopup = false;
        switch (typeUpdate) {
            case 0: //Update All
                confirmPopup = editMaster.confirmChangeAutoPt(typeUpdate);
                break;
            case 1: //isAllowUpdatePtSetting                
                confirmPopup = editMaster.confirmChangeAutoPt(typeUpdate);
                break;
        }

        if (confirmPopup || confirmPopup == undefined)
            return;

        editMaster.updatePT(typeUpdate);
    },
    updatePT: function (typeUpdate) {
        if (!casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
            typeUpdate = 0; //Update All 
        }
        switch (typeUpdate) {
            case 0: //Update All
                break;
            case 1: //isAllowUpdatePtSetting
                casinoManager.casinoVM.isAllowUpdateWinLimit = ko.dependentObservable(function () { return false });
                casinoManager.casinoVM.isAllowUpdateGameSetting = ko.dependentObservable(function () { return false });
                break;
            case 2: //isAllowUpdateWinLimit
                casinoManager.casinoVM.isAllowUpdatePtSetting = ko.dependentObservable(function () { return false });
                casinoManager.casinoVM.isAllowUpdateGameSetting = ko.dependentObservable(function () { return false });
                break;
            case 3: //isAllowUpdateGameSetting
                casinoManager.casinoVM.isAllowUpdatePtSetting = ko.dependentObservable(function () { return false });
                casinoManager.casinoVM.isAllowUpdateWinLimit = ko.dependentObservable(function () { return false });
                break;
        }

        casinoManager.casinoVM.requestContext.UserId = userId;
        casinoManager.casinoVM.requestContext.Ip = ip;
        casinoManager.casinoVM.requestContext.CustId = editMaster.custId;
        casinoManager.casinoVM.requestContext.UplineId = editMaster.uplineId;
        casinoManager.casinoVM.requestContext.SuperId = editMaster.requestContext.SuperId;
        casinoManager.casinoVM.requestContext.MasterId = editMaster.requestContext.MasterId;
        casinoManager.casinoVM.requestContext.AgentId = editMaster.requestContext.AgentId;
        casinoManager.casinoVM.requestContext.UplineRoleId = editMaster.requestContext.UplineRoleId;

        var setting = ko.toJSON(casinoManager.casinoVM);
        var param = '{"setting":' + setting + '}';
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "service/SettingService.svc/UpdateMasterLiveCasino",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: param,
            success: function (rs) {
                showEnrichMessage('err', rs.ErrorHandler);
                editMaster.afterUpdate(rs.ErrorHandler);
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    afterUpdate: function (errs) {
        var ok = true;
        for (var i = 0; i < errs.length; i++) {
            if (errs[i].Key > 0) {
                ok = false;
                break;
            }
        }
        if (ok) {

            var t = setTimeout(function () {
                editMaster.getSettings();
            }, 3000);
        }
    },
    confirmChangeAutoPt: function (typeUpdate) {

        if (casinoManager.casinoDefaultData.PTSetting.Upline.IsAutoPresetPT &&
            !casinoManager.casinoVM.PTSetting.Upline.IsAutoPresetPT()) {
            var maxPt = roundNumber(parseFloat(casinoManager.casinoVM.PTSetting.Upline.MaxPT) - parseFloat(casinoManager.casinoVM.PTSetting.MaxPT()), 2);
            CreateOption($("#MemberPT"), maxPt, maxPt, 0, 0.01, 2);

            $("#dialogMemberPT").dialog({
                title: "Confirm Update Member's PT",
                autoOpen: false,
                height: 180,
                width: 400,
                modal: true,
                buttons: {
                    "YES": function () {
                        casinoManager.casinoVM.PTSetting.Upline.IsUpdateAllMember = true;
                        casinoManager.casinoVM.PTSetting.Upline.NewMemberPT = $('#MemberPT').val();
                        editMaster.updatePT(typeUpdate);
                        $(this).dialog("close");
                        return true;
                    },
                    "NO": function () {
                        casinoManager.casinoVM.PTSetting.Upline.IsUpdateAllMember = false;
                        casinoManager.casinoVM.PTSetting.Upline.NewMemberPT = $('#MemberPT').val();
                        editMaster.updatePT(typeUpdate);
                        $(this).dialog("close");
                        return true;
                    }
                },
                close: function (event, ui) {
                }
            });
            $('a.ui-dialog-titlebar-close').show();
            $("#dialogMemberPT").dialog("open");
        }
        else {
            editMaster.updatePT(typeUpdate);
            return true;
        }
    },
    confirmUpdateAll: function () {
        if (casinoManager.casinoDefaultData.PTSetting.Upline.IsAutoPresetPT &&
            !casinoManager.casinoVM.PTSetting.Upline.IsAutoPresetPT()) {
            var maxPt = roundNumber(parseFloat(casinoManager.casinoVM.PTSetting.Upline.MaxPT) - parseFloat(casinoManager.casinoVM.PTSetting.MaxPT()), 2);
            CreateOption($("#MemberPT"), maxPt, maxPt, 0, 0.01, 2);

            $("#dialogMemberPT").dialog({
                title: "Confirm Update Member's PT",
                autoOpen: false,
                height: 180,
                width: 400,
                modal: true,
                buttons: {
                    "YES": function () {
                        casinoManager.casinoVM.PTSetting.Upline.IsUpdateAllMember = true;
                        casinoManager.casinoVM.PTSetting.Upline.NewMemberPT = $('#MemberPT').val();
                        editMaster.updateCasino();
                        $(this).dialog("close");
                    },
                    "NO": function () {
                        casinoManager.casinoVM.PTSetting.Upline.IsUpdateAllMember = false;
                        casinoManager.casinoVM.PTSetting.Upline.NewMemberPT = $('#MemberPT').val();
                        editMaster.updateCasino();
                        $(this).dialog("close");
                    }
                },
                close: function (event, ui) {
                    //editMaster.updateCasino();
                }
            });
            $('.ui-dialog-titlebar-close').show();
            $("#dialogMemberPT").dialog("open");
        }
        else {
            editMaster.updateCasino();
        }
    },
    notifyUnCheckAllBettingLimit: function (name) {
        var chks = $('input[name=chkBettingLimit_' + name + ']');
        var itemChecked = 0;
        var divErr = '';
        if ($('#err_' + name).length == 0) {
            divErr = document.createElement("DIV");
            divErr.id = 'err_' + name;
            divErr.style.margin = '2px';
            $('#err').append(divErr);
        }
        $.each(chks, function (index, item) {
            itemChecked += $(item).is(':checked') ? 1 : 0;
        });

        if (itemChecked == 0) {
            showAlertMessage('err_' + name, editMaster.msgUncheckBettingLimit.replace("{0}", name), 2);
        }
        else {
            clearMessage('err_' + name);
        }
    }
};

$(document).ready(function () {
    editMaster.getSettings();
    try { parent.jQuery.unblockUI(); } catch (e) { }

});
