var vsCustomer = {
    oldSetting: {},
    currentSetting: {},
    context: {},
    ptGrid: null,
    bettingLimitGrid: null,
    otherSetting: {
        UpdateAllMembers: false,
        s1: 0,
        m1: 0
    },
    init: function () {
        this.getContextObject();
    },
    getContextObject: function () {
        var param = {
            custId: uplineId, //superId
            targetCustId: masterId, //MasterId
            roleId: 4,
            targetRoleId: 3
        };
        $.ajax({
            type: "POST",
            url: "GetContextObject",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify(param),
            success: function (stringContext) {
                vsCustomer.context = JSON.parse(stringContext);
                vsCustomer.getMasterInfo();
            }
        , error: function (e) {
            $('.bg_table').addClass('hidden');
            showAlertMessage("err", "Init Context Error", 1);
        }
        });
    },
    getMasterInfo: function () {
        $.ajax({
            type: "POST",
            url: "GetCustomerInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: '{"requestContext":' + JSON.stringify(vsCustomer.context) + '}',
            success: function (rs) {
                $('.bg_table').removeClass('hidden');

                vsCustomer.currentSetting = JSON.parse(rs);
                vsCustomer.oldSetting = JSON.parse(rs);

                vsCustomer.initHtml();
            }
            , error: function (e) {
                $('.bg_table').addClass('hidden');
                showAlertMessage("err", "Init Customer Error", 1);
            }
        });
    },
    initHtml: function () {
        this.renderCustInfo();
        $('#ptContainer').html('');
        $('#bettingLimitContainer').html('');

        if (this.currentSetting.ptSetting && this.currentSetting.ptSetting.sportTypes && this.currentSetting.ptSetting.sportTypes.length > 0) {
            Fanex.PTVirtualSports.CURRENT_CONTEXT = this.context;
            this.ptGrid = new Fanex.PTVirtualSports.PTGrid(/*Fanex.copy*/(this.currentSetting.ptSetting));
            var ptContainer = document.getElementById('ptContainer');
            this.ptGrid.Render(ptContainer);
        }
        else {
            $('#ptContainer').append('There is no information available.');
        }

        if (this.currentSetting.bettingLimit && this.currentSetting.bettingLimit.bettingLimitItems != null && this.currentSetting.bettingLimit.bettingLimitItems.length > 0) {
            this.bettingLimitGrid = new Fanex.PTVirtualSports.BettingLimit(this.currentSetting.bettingLimit);
            var bettingLimitContainer = document.getElementById('bettingLimitContainer');
            bettingLimitContainer.innerHTML = "";
            this.bettingLimitGrid.Render(bettingLimitContainer, false);
        } else {
            $('#bettingLimitContainer').append('There is no information available.');
        }
    },

    UpdateMasterSettings: function () {
        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "POST",
            url: "UpdateMasterSettings",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"oldCustomerSettings":' + JSON.stringify(JSON.stringify(vsCustomer.oldSetting)) + ', "newCustomerSettings":' + JSON.stringify(JSON.stringify(vsCustomer.currentSetting)) + ', "currentContext":' + JSON.stringify(vsCustomer.context) + ', "otherSetting":' + JSON.stringify(vsCustomer.otherSetting) + '}',
            success: function (result) {
                if (typeof (result.ErrorCode) != 'undefined') {
                    showAlertMessage("err", result.ErrorMessage, 1);
                }
                else {
                    $('#divShowInfo').show();
                    showEnrichMessage('err', result);
                    vsCustomer.afterUpdate(result);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                try { parent.jQuery.unblockUI(); } catch (e) { }
                showAlertMessage("err", "Update Master Unsuccessfully!", 1);
            }
        });
    },

    updateMaster: function () {
        if (this.bettingLimitGrid.Validate()) {
            this.currentSetting.bettingLimit = this.bettingLimitGrid.Save();
            var isUpdateAllMember = this.ptGrid.Validate(this.currentSetting.ptSetting);
            this.currentSetting.ptSetting = this.ptGrid.Save();
            this.currentSetting.statusSetting = customerInfoBinding.save(3, this.oldSetting.statusSetting);

            if (!isUpdateAllMember) {
                vsCustomer.UpdateMasterSettings();
            }
        }

    },

    updateStatusSetting: function () {
        this.currentSetting.statusSetting = customerInfoBinding.save(3, this.oldSetting.statusSetting);
        vsCustomer.UpdateMasterSettings();
    },

    updateBettingLimit: function () {
        if (this.bettingLimitGrid.Validate()) {
            this.currentSetting.bettingLimit = this.bettingLimitGrid.Save();
            vsCustomer.UpdateMasterSettings();
        }

    },

    updatePtSetting: function () {
        var isUpdateAllMember = this.ptGrid.Validate(this.currentSetting.ptSetting);
        this.currentSetting.ptSetting = this.ptGrid.Save();
        
        if (!isUpdateAllMember) {
            vsCustomer.UpdateMasterSettings();
        }
    },

    afterUpdate: function (errs) {
        var ok = true;
        for (var i = 0; i < errs.length; i++) {
            if (errs[i].ErrorCode > 0) {
                ok = false;
                break;
            }
        }
        if (ok) {
            var t = setTimeout(function () {
                vsCustomer.init();
            }, 3000);
        }
    },
    renderCustInfo: function () {
        var setting = { "custSetting": this.currentSetting, "context": this.context };
        customerInfoBinding.createDependentObservable(setting);
        ko.applyBindings(setting);
        $('#chkEnabled').bind('click', function () { customerInfoBinding.enableCheckboxClick(3); });
    },

    setNewValueToMember: function (isUpdateAllMember) {
        vsCustomer.otherSetting.UpdateAllMembers = isUpdateAllMember;
        for (i = 0; i < vsCustomer.currentSetting.ptSetting.sportTypes.length; i++) {
            var sportType = vsCustomer.currentSetting.ptSetting.sportTypes[i];
            for (j = 0; j < sportType.betTypeGroupSets.length; j++) {
                var betTypeGroupSet = sportType.betTypeGroupSets[j];
                for (k = 0; k < betTypeGroupSet.betTypeGroups.length; k++) {
                    var betTypeGroup = betTypeGroupSet.betTypeGroups[k];
                    for (l = 0; l < betTypeGroup.betTypes.length; l++) {
                        var betType = betTypeGroup.betTypes[l];
                        betType.pt.s1 = Popups.GetSportPTValue(sportType.id); ; //parseFloat(document.getElementById('selectedMemberPositionTaking').value);
                    }
                }
            }
        }
        vsCustomer.UpdateMasterSettings();
        Popups.Close();
    }
};


function ConfirmYes() {
    vsCustomer.setNewValueToMember(true);
}

function ConfirmNo() {
    vsCustomer.setNewValueToMember(false);
}


$(document).ready(function () {
    vsCustomer.init();
});