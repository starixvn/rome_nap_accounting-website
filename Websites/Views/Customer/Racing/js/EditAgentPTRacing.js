var customerPTRacingSetting =
{
    context: {},
    ptSetting: null,
    bettingCommSetting: null,
    oldSetting: {},
    currentSetting: {},
    otherSettings: null,
    init: function () {
        this.getContextObject();
    },
    getContextObject: function () {
        var param = {
            targetCustId: custId,
            targetRoleId: ptRacingCommon.agentRoleId,
            custId: uplineId, //upline
            roleId: ptRacingCommon.masterRoleId
        };

        $.ajax({
            type: "POST",
            url: "GetContextObject",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (stringContext) {
                customerPTRacingSetting.context = JSON.parse(stringContext);
                customerPTRacingSetting.getCustomerInfo();
            }
            , error: function (e) {
                showAlertMessage("err", "Init Context Error", 1);
            }
        });
    },
    getCustomerInfo: function () {
        $.ajax({
            type: "POST",
            url: "GetCustomerInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"requestContext":' + JSON.stringify(customerPTRacingSetting.context) + '}',
            success: function (stringCustomer) {
                customerPTRacingSetting.currentSetting = JSON.parse(stringCustomer);
                customerPTRacingSetting.oldSetting = JSON.parse(stringCustomer);
                customerPTRacingSetting.initHtml();
            }
            , error: function (e) {
                showAlertMessage("err", "Init Customer Error", 1);
            }
        });
    },
    initHtml: function () {
        ptRacingCommon.renderCustomerInfo("Agent", customerPTRacingSetting.currentSetting);

        $('#bettingCommContainer').html('');
        if (this.currentSetting && this.currentSetting.settings &&
            this.currentSetting.settings.sportTypes && this.currentSetting.settings.sportTypes.length > 0) {
            Fanex.PTRacing.CURRENT_CONTEXT = this.context;
            Fanex.PTRacing.CURRENT_CONTEXT.ia3 = !this.currentSetting.DisableAutoMasterPt;

            /*region render betting & comm*/
            this.bettingCommSetting = new Fanex.PTRacing.BettingCommSetting(this.currentSetting.settings.sportTypes);
            var bettingCommContainer = document.getElementById('bettingCommContainer');
            bettingCommContainer.innerHTML = "";
            this.bettingCommSetting.Render(bettingCommContainer, ptRacingCommon.agentRoleId);
            /*endregion render betting & comm*/

            /*region render pt*/
            this.ptSetting = new Fanex.PTRacing.PTGrid(this.currentSetting.settings);
            var ptContainer = document.getElementById('ptContainer');
            ptContainer.innerHTML = "";
            this.ptSetting.Render(ptContainer);
            /*endregion render pt*/

            $('#tbCustomerInfo').css('width', ($('#tbBettingCommSetting').width() + 'px'));
        } else {
            $('#bettingCommContainer').append('There is no information available.');
            $('#ptContainer').append('There is no information available.');
        }
    },
    updateCustomerSettings: function () {
        if (this.bettingCommSetting.Validate()) {
            var isUpdateAllMember = this.ptSetting.Validate(this.currentSetting.settings, false);
            this.currentSetting.settings = this.ptSetting.Save();
            if (!isUpdateAllMember) {
                customerPTRacingSetting.updateAllSettings();
            }
        }
    },
    updateAllSettings: function () {
        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "POST",
            url: "UpdateCustomerSettings",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"oldCustomerSettings":' + JSON.stringify(JSON.stringify(this.oldSetting)) +
                ', "newCustomerSettings":' + JSON.stringify(JSON.stringify(this.currentSetting)) +
                ', "currentContext":' + JSON.stringify(customerPTRacingSetting.context) +
                ', "otherSettings":' + JSON.stringify(customerPTRacingSetting.otherSettings) +
                '}',
            success: function (result) {
                if (typeof (result.ErrorCode) != 'undefined') {
                    showAlertMessage("err", result.ErrorMessage, 1);
                }
                else {
                    $('#divShowInfo').show();
                    ptRacingCommon.renderErrorMessage('err', result);
                }                
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                try { parent.jQuery.unblockUI(); } catch (e) { }
                showAlertMessage("err", "Update Agent Unsuccessfully!", 1);
            }
        });
    },
    setNewValueToMember: function (isUpdateAllMember) {
        this.otherSettings = { "isupdateallmembers": isUpdateAllMember, "newmpositiontaking": Popups.GetNewPt() };

        this.updateAllSettings();
        Popups.Close();
    }
};

$(function () {
    customerPTRacingSetting.init();
});