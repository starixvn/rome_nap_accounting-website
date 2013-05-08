var casinoManager = {
    gameType: function (id, name, bettingLimit, selectedDefault, IsUplineUnCheckAll) {
        this.Id = id;
        this.Name = name;
        this.BettingLimit = bettingLimit;
        this.settingSelected = ko.observable(selectedDefault != '' ? selectedDefault : '');
        this.IsUplineUnCheckAll = IsUplineUnCheckAll;
    },
    BettingLimit: function (s) {
        this.IsSelected = s.IsSelected;
        this.Text = s.Text;
        this.Id = s.Id;
    },
    PTSetting: function (b) {
        //agent
        this.Upline = b.Upline;
        this.Upline.PresetPT = b.Upline.PresetPT;

        //master
        this.Upline.Upline = b.Upline.Upline;
        this.Upline.Upline.PresetPT = ko.observable(b.Upline.Upline.PresetPT);
        this.Upline.Upline.MinPT = b.Upline.Upline.MinPT;
        this.Upline.Upline.MaxPT = b.Upline.Upline.MaxPT;
        this.Upline.Upline.IsAutoPresetPT = ko.observable(b.Upline.Upline.IsAutoPresetPT);
        this.Upline.Upline.AutoPresetPT = ko.observable(b.Upline.Upline.AutoPresetPT);
        this.Upline.Upline.IsDisabledAutoPresetPT = b.Upline.Upline.IsDisabledAutoPresetPT;
        this.Upline.Upline.MaxAutoPresetPT = b.Upline.Upline.MaxAutoPresetPT;
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
            Mobile: ''
        },
        requestContext: {
            UserId: 0,
            Ip: '',
            Language: '',
            CustId: 0,
            SubAccId: 0,
            UplineId: 0
        },
        PTSetting: {},
        GameSetting: ko.observableArray(),
        WinLimitSetting: {}
    },
    isDisabled: false,
    casinoDefaultData: {},
    createDependentObservable: function () {
        casinoManager.casinoVM.isAllowUpdatePtSetting = ko.dependentObservable(function () {

            if (casinoManager.casinoVM.PTSetting.Upline.Upline.IsDisabledAutoPresetPT) {
                if (parseFloat(casinoManager.casinoVM.PTSetting.Upline.Upline.PresetPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Upline.Upline.PresetPT))
                    return true;
            }

            if (casinoManager.casinoVM.PTSetting.Upline.Upline.IsAutoPresetPT() != casinoManager.casinoDefaultData.PTSetting.Upline.Upline.IsAutoPresetPT)
                return true;

            if (casinoManager.casinoDefaultData.PTSetting.Upline.Upline.IsAutoPresetPT == true) {

                if (parseFloat(casinoManager.casinoVM.PTSetting.Upline.Upline.AutoPresetPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Upline.Upline.AutoPresetPT))
                    return true;
            }
            else {
                if (parseFloat(casinoManager.casinoVM.PTSetting.Upline.Upline.PresetPT()) != parseFloat(casinoManager.casinoDefaultData.PTSetting.Upline.Upline.PresetPT))
                    return true;
            }

            return false;

        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAllowUpdateGameSetting = ko.dependentObservable(function () {
            var gameTypes = [];

            for (var i = 0; i < casinoManager.casinoVM.GameSetting().length; i++) {
                if (casinoManager.casinoVM.GameSetting()[i].IsUplineUnCheckAll) {
                    gameTypes[i] = casinoManager.casinoVM.GameSetting()[i].Name;
                }
            }
            if (gameTypes.length > 0) {
                showAlertMessage('err1', gameTypes.join(', ') + ' ' + masterEditOnMember.msgUncheckBettingLimit, 2);
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

                        if (itemDefaut.BettingLimit[j].IsSelected && itemDefaut.BettingLimit[j].Text != item.settingSelected())
                            return true;

                    }
                }
                return false;
            }
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAutoReset = ko.dependentObservable(function () {
            return (casinoManager.casinoVM.PTSetting.Upline.Upline.IsAutoPresetPT());
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isUplineDisableAutoPresetPt = ko.dependentObservable(function () {
            return (casinoManager.casinoVM.PTSetting.Upline.Upline.IsDisabledAutoPresetPT);
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isNotAutoReset = ko.dependentObservable(function () {
            return !(casinoManager.casinoVM.PTSetting.Upline.Upline.IsAutoPresetPT());
        }, casinoManager.casinoVM);
        casinoManager.casinoVM.isAllowUpdateWinLimit = ko.dependentObservable(function () {
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
            var selectedSetting = '';
            for (var k = 0; k < _gameType.BettingLimit.length; k++) {
                if (_gameType.BettingLimit[k].IsSelected)
                    selectedSetting = _gameType.BettingLimit[k].Text;
                _stakeLevels.push(new casinoManager.BettingLimit(_gameType.BettingLimit[k]));

            }
            //ko.applyBindings(_stakeLevels.BettingLimit);
            casinoManager.casinoVM.GameSetting.push(new casinoManager.gameType(_gameType.Id, _gameType.Name, _stakeLevels, selectedSetting, _gameType.IsUplineUnCheckAll));

        }
    },
    initcasino: function () {
        $("#err").empty();
        ko.applyBindings(casinoManager.casinoVM, document.getElementById("SettingContent"));

    }
};

var masterEditOnMember = {
    custId: 0,
    isReloaded: false,
    isFirstSuccess: true,
    uplineId: 0,
    msgUncheckBettingLimit:'',
    requestContext: { UserId: 0, CustId: 0, SubAccId: 0, UplineId: 0, Ip: '', Language: '', IsmasterEditOnMember: false, UplineRoleId: 3 },
    getSettings: function () {
        try { parent.BlockUI(); } catch (e) { }
        var param = { custId: masterEditOnMember.custId, uplineId: masterEditOnMember.uplineId };
        $.ajax({
            type: "POST",
            url: "service/SettingService.svc/GetMasterOnMemberSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                casinoManager.mappingcasinoData(rs, masterEditOnMember.custId);
                casinoManager.createDependentObservable();
                casinoManager.initcasino();
            },
            error: function () { },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    updateLiveCasino: function (typeUpdate) {
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
        casinoManager.casinoVM.requestContext.CustId = masterEditOnMember.custId;
        casinoManager.casinoVM.requestContext.UplineId = masterEditOnMember.uplineId;
        casinoManager.casinoVM.requestContext.UplineRoleId = masterEditOnMember.requestContext.UplineRoleId;

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
            url: "service/SettingService.svc/UpdateMasterOnMemberLiveCasino",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: param,
            success: function (rs) {
                showEnrichMessage('err', rs.ErrorHandler);
                masterEditOnMember.afterUpdate(rs.ErrorHandler);
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
                masterEditOnMember.getSettings();
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
    masterEditOnMember.getSettings();
});

