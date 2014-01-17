var casinoManager = {
    gameType: function (id, name, bettingLimit, selectedDefault, IsUplineUnCheckAll, betTypeId) {
        this.Id = id;
        this.Name = name;
        this.BettingLimit = bettingLimit;
        this.settingSelected = ko.observable(selectedDefault);
        this.IsUplineUnCheckAll = IsUplineUnCheckAll;
        this.BetTypeId = betTypeId;
    },
    BettingLimit: function (s) {
        this.IsSelected = s.IsSelected;
        this.Text = s.Text;
        this.Id = s.Id;
    },
    PTSetting: function (b) {
        this.Comm = ko.observable(b.Comm);

        this.Upline = b.Upline;
        this.Upline.MinPT = b.Upline.MinPT;
        this.Upline.MaxPT = b.Upline.MaxPT;
        this.Upline.PresetPT = ko.observable(b.Upline.PresetPT);
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
    CustomerInfo: function (c) {
        this.CustomerId = c.CustomerId;
        this.RoleId = c.RoleId;
        this.UserName = c.UserName;
        this.NickName = c.NickName;
        this.FirstName = c.FirstName;
        this.LastName = c.LastName;
        this.Phone = c.Phone;
        this.Mobile = c.Mobile;
        this.IsSyncLiveCasino = c.IsSyncLiveCasino;
        this.UplineDisableLiveCasino = !c.UplineDisableLiveCasino;
        this.DisableLiveCasino = ko.observable(!c.DisableLiveCasino);
    },
    casinoVM: {
        CustomerInfo: {
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
        WinLimitSetting: {},
        updatePtSetting: function () {
            if (casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
                editMember.updatePtSetting();
            }
            else {
                editMember.updateCasino();
            }
        },
        updateGameSetting: function () {
            if (casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
                editMember.updateGameSetting();
            }
            else {
                editMember.updateCasino();
            }
        },
        updateCasino: function () {
            if (casinoManager.casinoVM.isAllowUpdateGameSetting() && !casinoManager.casinoVM.isAllowUpdatePtSetting()) {
                editMember.updateGameSetting();
            }
            else if (casinoManager.casinoVM.isAllowUpdatePtSetting() && !casinoManager.casinoVM.isAllowUpdateGameSetting()) {
                editMember.updatePtSetting();
            }
            else if (casinoManager.casinoVM.isAllowUpdatePtSetting() && casinoManager.casinoVM.isAllowUpdateGameSetting()) {
                editMember.updateCasino();
            }

            else if (casinoManager.casinoVM.isAllowStatus()) {
                editMember.UpdateStatusLiveCasino();
            }
        }
    },
    isDisabled: false,
    casinoDefaultData: {},
    createDependentObservable: function () {
        casinoManager.casinoVM.isAllowUpdatePtSetting = ko.dependentObservable(function () {
            if (!casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
                return true;
            }
            if (parseFloat(casinoManager.casinoVM.PTSetting.Comm()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Comm) ||
                parseFloat(casinoManager.casinoVM.PTSetting.Upline.PresetPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Upline.PresetPT))
            { return true; }

            return false;

        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAllowUpdateGameSetting = ko.dependentObservable(function () {
            if (!casinoManager.casinoVM.CustomerInfo.IsSyncLiveCasino) {
                return true;
            }
            var gameTypes = [];

            for (var i = 0; i < casinoManager.casinoVM.GameSetting().length; i++) {
                if (casinoManager.casinoVM.GameSetting()[i].IsUplineUnCheckAll) {
                    gameTypes[i] = casinoManager.casinoVM.GameSetting()[i].Name;
                }
            }
            if (gameTypes.length > 0) {
                //showAlertMessage('err1', gameTypes.join(', ') + ' ' + editMember.msgUncheckBettingLimit, 2);
                showAlertMessage('err1', gameTypes.clean(undefined).clean("").join(', ') + ' ' + editMember.msgUncheckBettingLimit, 2);
                return true;
            }
            else {
                clearMessage('err1');
            }
            if (casinoManager.casinoVM.GameSetting()) {

                for (var i = 0; i < casinoManager.casinoVM.GameSetting().length; i++) {
                    var item = casinoManager.casinoVM.GameSetting()[i];
                    var itemDefaut = casinoManager.casinoDefaultData.GameSetting[i];
                    for (var j = 0; j < item.BettingLimit.length; j++) {
                        if (itemDefaut.BettingLimit[j].IsSelected && itemDefaut.BettingLimit[j].Text != item.settingSelected()) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAllowStatus = ko.dependentObservable(function () {
            return !(casinoManager.casinoVM.CustomerInfo.DisableLiveCasino() != casinoManager.casinoDefaultData.CustomerInfo.DisableLiveCasino);
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
            if (casinoManager.casinoVM.isAllowUpdateGameSetting() || casinoManager.casinoVM.isAllowUpdatePtSetting() || casinoManager.casinoVM.isAllowStatus() || casinoManager.casinoVM.isAllowUpdateWinLimit())
                return true;
            return false;
        }, casinoManager.casinoVM);
    },
    mappingcasinoData: function (data, custid) {
        casinoManager.casinoDefaultData = $.extend(true, {}, data);
        casinoManager.casinoVM.CustomerInfo = new casinoManager.CustomerInfo(data.CustomerInfo);
        casinoManager.casinoVM.PTSetting = new casinoManager.PTSetting(data.PTSetting);

        casinoManager.casinoVM.WinLimitSetting = new casinoManager.WinLimitSetting(data.WinLimitSetting);

        casinoManager.casinoVM.GameSetting.removeAll();
        for (var j = 0; j < data.GameSetting.length; j++) {
            var _stakeLevels = [];
            var _gameType = data.GameSetting[j];
            var selectedSetting = '';
            for (var k = 0; k < _gameType.BettingLimit.length; k++) {
                if (_gameType.BettingLimit[k].IsSelected)
                    selectedSetting = _gameType.BettingLimit[k].Text;
                _stakeLevels.push(new casinoManager.BettingLimit(_gameType.BettingLimit[k]));

            }
            casinoManager.casinoVM.GameSetting.push(new casinoManager.gameType(_gameType.Id, _gameType.Name, _stakeLevels, selectedSetting, _gameType.IsUplineUnCheckAll, _gameType.BetTypeId));

        }
    },
    initcasino: function () {
        $("#err").empty();
        ko.applyBindings(casinoManager.casinoVM, document.getElementById("SettingContent"));

    }
};

var editMember = {
    custId: 0,
    uplineId: 0,
    isReloaded: false,
    isFirstSuccess: true,
    msgUncheckBettingLimit: '',
    requestContext: { UserId: 0, CustId: 0, SubAccId: 0, UplineId: 0, Ip: '', Language: '', IseditMember: false, AgentId: 0, MasterId: 0, SuperId: 0, UplineRoleId: 2 },
    getSettings: function () {
        try { parent.BlockUI(); } catch (e) { }
        var param = { custId: editMember.custId, uplineId: editMember.uplineId };
        $.ajax({
            type: "POST",
            url: "service/SettingService.svc/GetMemberSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs && rs.PTSetting) {
                    casinoManager.mappingcasinoData(rs, editMember.custId);
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
        casinoManager.casinoVM.requestContext.CustId = editMember.custId;
        casinoManager.casinoVM.requestContext.UplineId = editMember.uplineId;
        casinoManager.casinoVM.requestContext.SuperId = editMember.requestContext.SuperId;
        casinoManager.casinoVM.requestContext.MasterId = editMember.requestContext.MasterId;
        casinoManager.casinoVM.requestContext.AgentId = editMember.requestContext.AgentId;
        casinoManager.casinoVM.requestContext.UplineRoleId = editMember.requestContext.UplineRoleId;

        for (var i = 0; i < casinoManager.casinoVM.GameSetting().length; i++) {
            var item = casinoManager.casinoVM.GameSetting()[i];
            var itemDefaut = casinoManager.casinoDefaultData.GameSetting[i];
            for (var j = 0; j < itemDefaut.BettingLimit.length; j++) {

                if (item.settingSelected() == item.BettingLimit[j].Text) {
                    casinoManager.casinoVM.GameSetting()[i].BettingLimit[j].IsSelected = true;
                }
                else {
                    casinoManager.casinoVM.GameSetting()[i].BettingLimit[j].IsSelected = false;
                }
            }
        }
        var setting = ko.toJSON(casinoManager.casinoVM);
        var param = '{"setting":' + setting + '}';
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "service/SettingService.svc/UpdateMemberLiveCasino",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: param,
            success: function (rs) {
                showEnrichMessage('err', rs.ErrorHandler);
                editMember.afterUpdate(rs.ErrorHandler);
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    UpdateStatusLiveCasino: function () {
        casinoManager.casinoVM.requestContext.UserId = userId;
        casinoManager.casinoVM.requestContext.Ip = ip;
        casinoManager.casinoVM.requestContext.CustId = editMember.custId;
        casinoManager.casinoVM.requestContext.UplineId = editMember.uplineId;
        casinoManager.casinoVM.requestContext.SuperId = editMember.requestContext.SuperId;
        casinoManager.casinoVM.requestContext.MasterId = editMember.requestContext.MasterId;
        casinoManager.casinoVM.requestContext.AgentId = editMember.requestContext.AgentId;
        casinoManager.casinoVM.requestContext.UplineRoleId = editMember.requestContext.UplineRoleId;

        var setting = ko.toJSON(casinoManager.casinoVM);
        var param = '{"setting":' + setting + '}';
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "service/SettingService.svc/UpdateStatusLiveCasino",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: param,
            success: function (rs) {
                showEnrichMessage('err', rs.ErrorHandler);
                editMember.afterUpdate(rs.ErrorHandler);
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
                editMember.getSettings();
            }, 3000);
        }
    },
    viewCurrentWinLimit: function () {
        $('#trClicktoViewCurrentWinLimit').hide();
        $('#trCurrentWinLimit').removeClass('hide');

        var limit = $('#ddlWinLimit').val();
        if (limit == 0) {
            $('#spWinPercent').html('Unlimited');
            return;
        }

        var param = {
            custId: casinoManager.casinoVM.CustomerInfo.CustomerId
        };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "service/SettingService.svc/SelCurrentWinLimit",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {

                if (isNaN(rs.WinPercent.replace(",", ""))) {
                    $('#spWinPercent').html(rs.WinPercent);
                }
                else {
                    if (parseFloat(rs.WinPercent.replace(",", "")) > 100)
                        $('#spWinPercent').html(rs.WinPercent + '%').css('color', 'red');
                    else
                        $('#spWinPercent').html(rs.WinPercent + '%');
                }
                if (parseFloat(rs.MaxWinLCS.replace(",", "")) == 0)
                    $('#spMaxWinCredit').html('');
                else
                    $('#spMaxWinCredit').html('of ' + rs.MaxWinCredit);
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    }

};

$(document).ready(function () {
    editMember.getSettings();

    try { parent.jQuery.unblockUI(); } catch (e) { }

});
