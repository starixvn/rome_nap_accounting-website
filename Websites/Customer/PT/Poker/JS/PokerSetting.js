var pokerManager = {
    commission: function (c) {
        this.Comm = ko.observable(c.Comm+'');
        this.MaxComm = c.MaxComm + '';
        this.UplineComm = c.UplineComm + '';
        this._Name = c._Name;
    },
    gameType: function (id, name, stakeLevels) {
        this.ID=id;
        this.Name = name;     
        this.StakeLevels = stakeLevels;
    },
    StakeLevel: function (s) {
        this.IsAllow=s.IsAllow;
        this.IsChecked = ko.observable(s.IsChecked);
        this.Name = s.Name;
    },
    BetSetting: function (b) {
        this.IsAutoReset = ko.observable(b.IsAutoReset);
        this.IsUplineAutoReset = b.IsUplineAutoReset;
        this.MaxTransferCredit=b.MaxTransferCredit;
        this.MinTransferCredit=b.MinTransferCredit;
        this.TransferCredit = ko.observable(autoFormatNumber(b.TransferCredit)+'');
    },
    pokerVM: {
        CustInfo: {
            CustId: '',
            RoleId: '',
            UserName: '',
            ScreenName: '',
            FirstName: '',
            LastName: '',
            Phone: '',
            MobilePhone: ''
        },
        BetSetting: {},
        GameSettings: ko.observableArray(),
        GamesComm: ko.observableArray(),
        updateBetSetting: function (isPartial) {
             if (isDisabled)
            {
                showAlertMessage("err",msgDisabled,1);
                return;
            }
            pokerData.isFirstNoPermissionMsg=true;
			var isvalid=false;
            if (isInt($("#txtmaxtransfer").val().replace(/,/g,'')) && isInt(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,'')) && parseInt(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,''))>=parseInt(pokerManager.pokerVM.BetSetting.MinTransferCredit) && parseInt(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,''))<=parseInt(pokerManager.pokerVM.BetSetting.MaxTransferCredit))
            {
                isvalid=true;
            }
           
            if (isvalid)
            {
                pokerManager.pokerVM.BetSetting.TransferCredit(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,''));
                pokerData.updateCustomerSetting(pokerManager.pokerVM,3,isPartial);
            }
            else
            {
                showAlertMessage("err","Invalid input data.",1);
                $("#txtmaxtransfer").focus();
                try {  PTUnBlockUI();  } catch (e) { }
            }
        },
        updateGameSetting: function (isPartial) {
            if (isDisabled)
            {
                showAlertMessage("err",msgDisabled,1);
                return;
            }
			pokerData.isFirstNoPermissionMsg=true;
            $("#err").html('<div id="box" class="boxscc hidden" ></div>');
            pokerManager.pokerVM.BetSetting.TransferCredit(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,''));
            pokerData.updateGameSetting(pokerManager.pokerVM,2,isPartial);
        },
        updateCommission: function (isPartial) {
             if (isDisabled)
            {
                showAlertMessage("err",msgDisabled,1);
                return;
            }
            pokerData.isFirstNoPermissionMsg=true;
            pokerManager.pokerVM.BetSetting.TransferCredit(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,''));
            pokerData.updateCustomerSetting(pokerManager.pokerVM,1,isPartial);
        },
        updatePocker: function () {
            if (isDisabled)
            {
                showAlertMessage("err",msgDisabled,1);
                return;
            }
            $("#err").html('<div id="box" class="boxscc hidden" ></div>');
            pokerData.isReloaded=false;
			pokerData.isFirstNoPermissionMsg=true;
			pokerManager.pokerVM.BetSetting.TransferCredit(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,''));
             if ($("#txtmaxtransfer").val()=="" || !isInt($("#txtmaxtransfer").val().replace(/,/g,'')))
                {
                    $("#box").removeClass("hidden").append(formatAlertMessage("Bet Setting: Invalid value.", 1));
                    pokerManager.pokerVM.BetSetting.TransferCredit("0");
                    $("#txtmaxtransfer").focus();
                }
                else
                {
                   
                    if (pokerManager.pokerVM.isAllowUpdateBetSetting()) {
                        pokerData.updateCustomerSetting(pokerManager.pokerVM,3,false);
                    }
                    else
                    {
                        $("#box").removeClass("hidden").append(formatAlertMessage("Bet Setting: Invalid value.", 1));
                        pokerManager.pokerVM.BetSetting.TransferCredit("0");
                        $("#txtmaxtransfer").focus();
                    }
                }
           
            //if (pokerManager.pokerVM.isAllowUpdateGameSetting()) {
              pokerData.updateGameSetting(pokerManager.pokerVM,2,false);
            //}
            //if (pokerManager.pokerVM.isAllowUpdateCommission()) {
               pokerData.updateCustomerSetting(pokerManager.pokerVM,1,false);
            //}
        }
    },
    isDisabled:false,
    pokerDefaultData: {},
    pokerData: {},
    createDependentObservable: function () {
        pokerManager.pokerVM.isAllowUpdateBetSetting = ko.dependentObservable(function () {
           
            if (isInt(pokerManager.pokerVM.BetSetting.TransferCredit().replace(/,/g,'')) && parseInt(this.BetSetting.TransferCredit().replace(/,/g,''))>=parseInt(this.BetSetting.MinTransferCredit) && parseInt(this.BetSetting.TransferCredit().replace(/,/g,''))<=parseInt(this.BetSetting.MaxTransferCredit))
            {
                return true;
            }
            else
            {
                return false;
            }
       
        }, pokerManager.pokerVM);
        pokerManager.pokerVM.isAllowUpdateGameSetting = ko.dependentObservable(function () {           
            //            if (ko.toJSON(this.GameSettings()) != JSON.stringify(pokerManager.pokerDefaultData.GameSettings))
            //                return true;
            //            return false;
            return true;
        }, pokerManager.pokerVM);
        pokerManager.pokerVM.isAllowUpdateCommission = ko.dependentObservable(function () {
            //            if (ko.toJSON(this.GamesComm()) != JSON.stringify(pokerManager.pokerDefaultData.GamesComm))
            //                return true;
            //            return false;
            return true;
        }, pokerManager.pokerVM);
        pokerManager.pokerVM.isAllowUpdate = ko.dependentObservable(function () {
            //if (this.isAllowUpdateCommission() || this.isAllowUpdateGameSetting() || this.isAllowUpdateBetSetting())
                return true;
            //return false;
        }, pokerManager.pokerVM);
    },
    mappingPokerData: function (data, custid) {
        pokerManager.pokerDefaultData = $.extend(true, {}, data);
        pokerManager.pokerVM.CustInfo = data.CustInfo;
        pokerManager.pokerVM.CustInfo.CustId=custid;
        pokerManager.pokerVM.BetSetting = new pokerManager.BetSetting(data.BetSetting);
        pokerManager.pokerVM.GameSettings.removeAll();
        for (var j = 0; j < data.GameSettings.length; j++) {
            var _stakeLevels = [];
            var _gameType = data.GameSettings[j];
            for (var k = 0; k < _gameType.StakeLevels.length; k++) {
                _stakeLevels.push(new pokerManager.StakeLevel(_gameType.StakeLevels[k]));
            }
            pokerManager.pokerVM.GameSettings.push(new pokerManager.gameType(_gameType.ID, _gameType.Name, _stakeLevels));
        }
        pokerManager.pokerVM.GamesComm.removeAll();
        for (var i = 0; i < data.GamesComm.length; i++) {
            pokerManager.pokerVM.GamesComm.push(new pokerManager.commission(data.GamesComm[i]));
        }
    },
    initPoker: function () {
        $("#err").empty();
        ko.applyBindings(pokerManager.pokerVM, document.getElementById("SettingContent"));
        $('#txtmaxtransfer').bind('keypress',function(e){ 
                allowInt(e);
            });
    }
};

