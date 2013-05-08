var vsCustomer = {
    oldSetting: {},
    currentSetting: {},
    context: {},
    ptGrid: null,
    bettingLimitGrid: null,
    init: function () {
        this.getContextObject();
    },
    getContextObject: function () {
        var param = {
            custId: superId, //super Id
            targetCustId: MemberId, //MemberId
            roleId: 4,
            targetRoleId: 1
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
                vsCustomer.getMemberInfo();
            }
        , error: function (e) {
            $('.bg_table').addClass('hidden');
            showAlertMessage("err", "Init Context Error", 1);
        }
        });
    },
    getMemberInfo: function () {
        $.ajax({
            type: "POST",
            url: "GetCustomerInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
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
            this.ptGrid = new Fanex.PTVirtualSports.PTGrid(this.currentSetting.ptSetting);
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
            this.bettingLimitGrid.Render(bettingLimitContainer, true);
        } else {
            $('#bettingLimitContainer').append('There is no information available.');
        }
    },
    updateMember: function () {
        this.currentSetting.ptSetting = this.ptGrid.Save();
        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "POST",
            url: "UpdateMemberSettings",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"oldCustomerSettings":' + JSON.stringify(JSON.stringify(vsCustomer.oldSetting)) + ', "newCustomerSettings":' + JSON.stringify(JSON.stringify(vsCustomer.currentSetting)) + ', "currentContext":' + JSON.stringify(vsCustomer.context) + '}',
            success: function (result) {
                $('#divShowInfo').show();
                showEnrichMessage('err', result);
                vsCustomer.afterUpdate(result);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                try { parent.jQuery.unblockUI(); } catch (e) { }
                showAlertMessage("err", "Update Member Unsuccessfully!", 1);
            }
        });

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
        ko.applyBindings(setting);
    }
};

$(document).ready(function () {
    vsCustomer.init();
});