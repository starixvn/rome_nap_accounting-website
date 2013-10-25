$(document).ready(function () {
    autoSuspendStatus.initPage();
});

var AutoSuspendStatus = function () {
    var self = this;

    self.Constants = {
        RoleId: {
            SUPER: 4,
            MASTER: 3,
            AGENT: 2,
            MEMBER: 1,
            SuperOnMember: -1,
            MasterOnMember: -2,
            SuperMin: -3,
            MasterMinMax: -4,
            AgentMinMax: -5
        }
    };

    self.Tables = {
        settingContent: null,
        buttonUpdate: null,
        resultMessage: null,
        customerInfoTitle: null,
        tableSettings: null,

        checkboxEnableAutosuspendSoccer: null,
        labelEnableAutosuspendSoccer: null,
        checkboxEnableAutosuspendSoccerDisable: null,

        checkboxEnableAutosuspendOthersports: null,
        labelEnableAutosuspendOthersports: null,
        checkboxEnableAutosuspendOthersportsDisable: null
    };

    self.Params = {
        getCustomerInfo: {
            CustomerId: 0
        },

        updateCustomerInfo: {
            CustomerId: 0,
            Role: null,

            UplineAutoSuspendSoccer: null,
            UplineAutoSuspendOtherSports: null,

            AutoSuspendSoccer: null,
            AutoSuspendOtherSports: null
        }
    };

    self.CacheData = {
        customerInfo: null
    };

    self.blockUI = function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    };

    self.unblockUI = function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    };

    self.CallAjax = function (reportHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(requestParams),
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) == 'function') {
                    successFunction.call(reportHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) == 'function') {
                    errorFunction.call(reportHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) == 'function') {
                    completeFunction();
                }
            }
        });
    };

    self.getRoleName = function (roleId) {
        roleId = parseInt(roleId);
        var name = "";
        switch (roleId) {
            case self.Constants.RoleId.SUPER:
            case self.Constants.RoleId.SuperMin:
                name = "Super";
                break;
            case self.Constants.RoleId.MASTER:
            case self.Constants.RoleId.MasterMinMax:
                name = "Master";
                break;
            case self.Constants.RoleId.AGENT:
            case self.Constants.RoleId.AgentMinMax:
                name = "Agent";
                break;
            case self.Constants.RoleId.MEMBER:
            case self.Constants.RoleId.SuperOnMember:
            case self.Constants.RoleId.MasterOnMember:
                name = "Member";
                break;
            default:
                name = "Member";
                break;
        }

        return name;
    };

    self.initPage = function () {
        self.blockUI();
        self.cachedDOMElements();
        self.bindingEvent();
        self.GetCustomerInfo();
    };

    self.cachedDOMElements = function () {
        self.Tables.settingContent = $("#setting-content");
        self.Tables.buttonUpdate = $("#button-update");
        self.Tables.resultMessage = $("#result-message");
        self.Tables.customerInfoTitle = $("#customer-info-title");

        self.Tables.tableSettings = $("#setting-content .table-settings");
        self.Tables.checkboxEnableAutosuspendSoccer = self.Tables.tableSettings.find("#checkbox-enable-autosuspend-soccer");
        self.Tables.labelEnableAutosuspendSoccer = self.Tables.tableSettings.find("#label-enable-autosuspend-soccer");
        self.Tables.checkboxEnableAutosuspendSoccerDisable = self.Tables.tableSettings.find("#checkbox-enable-autosuspend-soccer-disable");

        self.Tables.checkboxEnableAutosuspendOthersports = self.Tables.tableSettings.find("#checkbox-enable-autosuspend-othersports");
        self.Tables.labelEnableAutosuspendOthersports = self.Tables.tableSettings.find("#label-enable-autosuspend-othersports");
        self.Tables.checkboxEnableAutosuspendOthersportsDisable = self.Tables.tableSettings.find("#checkbox-enable-autosuspend-othersports-disable");

    };
    self.bindingEvent = function () {
        self.Tables.buttonUpdate.unbind("click");
        self.Tables.buttonUpdate.click(function () {
            self.buttonUpdateClick();
        });

        self.Tables.checkboxEnableAutosuspendSoccerDisable.tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'nw'
        });

        self.Tables.checkboxEnableAutosuspendOthersportsDisable.tipsy({
            html: true,
            opacity: 0.9,
            gravity: 'nw'
        });
    };

    self.buttonUpdateClick = function () {
        self.GetUpdateParams();
        var confirmMessage = (self.CacheData.customerInfo.RoleId == self.Constants.RoleId.MEMBER ||
            self.CacheData.customerInfo.RoleId == self.Constants.RoleId.SuperOnMember ||
            self.CacheData.customerInfo.RoleId == self.Constants.RoleId.MasterOnMember) ?
            "Are you sure you want to update auto suspend on this customer?" :
            "Are you sure you want to update auto suspend on this customer and all downlines?";

        if (!confirm(confirmMessage)) {
            return;
        }

        self.blockUI();
        if (self.CacheData.customerInfo.UplineAutoSuspendSoccer &&
            self.CacheData.customerInfo.UplineAutoSuspendOtherSports) {
            showAlertMessage("result-message", "Upline has been auto suspended", 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        self.CallAjax(self,
            "../../../AutoSuspend/UpdateCustomerStatus",
            self.Params.updateCustomerInfo,
            self.RenderUpdateResult,
            self.RenderUpdateResult,
            self.unblockUI);
    };

    self.RenderUpdateResult = function (updateResult) {
        if (updateResult == null) {
            showAlertMessage("result-message", "There is a problem! Please try again!", 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        if (updateResult.ErrorCode > 0) {
            showAlertMessage("result-message", updateResult.ErrorMessage, 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        showAlertMessage("result-message", updateResult.ErrorMessage, 0);
        self.Tables.resultMessage.removeClass("display-none");
        setTimeout(function () {
            self.Tables.resultMessage.empty().addClass("display-none");
        }, 5000);
    };

    self.GetUpdateParams = function () {
        self.Params.updateCustomerInfo.CustomerId = self.Params.getCustomerInfo.CustomerId;
        self.Params.updateCustomerInfo.Role = self.CacheData.customerInfo.RoleId;

        self.Params.updateCustomerInfo.UplineAutoSuspendSoccer = self.CacheData.customerInfo.UplineAutoSuspendSoccer;
        self.Params.updateCustomerInfo.UplineAutoSuspendOtherSports = self.CacheData.customerInfo.UplineAutoSuspendOtherSports;

        self.Params.updateCustomerInfo.AutoSuspendSoccer = !self.Params.updateCustomerInfo.UplineAutoSuspendSoccer ?
            self.Tables.checkboxEnableAutosuspendSoccer.attr("checked") :
            self.CacheData.customerInfo.AutoSuspendSoccer;
        self.Params.updateCustomerInfo.AutoSuspendOtherSports = !self.Params.updateCustomerInfo.UplineAutoSuspendOtherSports ?
            self.Tables.checkboxEnableAutosuspendOthersports.attr("checked") :
            self.CacheData.customerInfo.AutoSuspendOtherSports;
    };

    self.GetCustomerInfo = function () {
        self.Params.getCustomerInfo = {
            CustomerId: viewResources.CustomerId
        };

        self.CallAjax(self,
            "../../../AutoSuspend/GetCustomerStatus",
            self.Params.getCustomerInfo,
            self.RenderCustomerInfo,
            self.RenderCustomerInfo,
            self.unblockUI);
    };

    self.RenderCustomerInfo = function (customerInfo) {
        self.CacheData.customerInfo = customerInfo;
        if (customerInfo == null) {
            self.Tables.settingContent.addClass("display-none");
            showAlertMessage("result-message", "There is no customer info.", 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        self.Tables.resultMessage.addClass("display-none");

        var customerInfoTitle = self.getRoleName(customerInfo.RoleId) + ": " + customerInfo.Username;
        self.Tables.customerInfoTitle.html(customerInfoTitle);

        if (customerInfo.AutoSuspendSoccer) {
            self.Tables.checkboxEnableAutosuspendSoccerDisable.removeClass("checkbox-unchecked")
                                                              .addClass("checkbox-checked");
            self.Tables.checkboxEnableAutosuspendSoccer.attr("checked", "checked");
        } else {
            self.Tables.checkboxEnableAutosuspendSoccerDisable.removeClass("checkbox-checked")
                                                              .addClass("checkbox-unchecked");
            self.Tables.checkboxEnableAutosuspendSoccer.attr("checked", null);
        }

        if (customerInfo.AutoSuspendOtherSports) {
            self.Tables.checkboxEnableAutosuspendOthersportsDisable.removeClass("checkbox-unchecked")
                                                                   .addClass("checkbox-checked");
            self.Tables.checkboxEnableAutosuspendOthersports.attr("checked", "checked");
        } else {
            self.Tables.checkboxEnableAutosuspendOthersportsDisable.removeClass("checkbox-checked")
                                                                   .addClass("checkbox-unchecked");
            self.Tables.checkboxEnableAutosuspendOthersports.attr("checked", null);
        }

        if (customerInfo.UplineAutoSuspendSoccer) {
            self.Tables.checkboxEnableAutosuspendSoccer.attr("disabled", "disabled");
            self.Tables.checkboxEnableAutosuspendSoccer.addClass("display-none");
            self.Tables.labelEnableAutosuspendSoccer.addClass("disabled ");
            self.Tables.checkboxEnableAutosuspendSoccerDisable.removeClass("display-none");
        } else {
            self.Tables.checkboxEnableAutosuspendSoccerDisable.addClass("display-none");
            self.Tables.labelEnableAutosuspendSoccer.removeClass("disabled");
            self.Tables.checkboxEnableAutosuspendSoccer.removeClass("display-none");
            self.Tables.checkboxEnableAutosuspendSoccer.attr("disabled", null);
        }

        if (customerInfo.UplineAutoSuspendOtherSports) {
            self.Tables.checkboxEnableAutosuspendOthersports.attr("disabled", "disabled");
            self.Tables.checkboxEnableAutosuspendOthersports.addClass("display-none");
            self.Tables.labelEnableAutosuspendOthersports.addClass("disabled ");
            self.Tables.checkboxEnableAutosuspendOthersportsDisable.removeClass("display-none");
        } else {
            self.Tables.checkboxEnableAutosuspendOthersportsDisable.addClass("display-none");
            self.Tables.labelEnableAutosuspendOthersports.removeClass("disabled");
            self.Tables.checkboxEnableAutosuspendOthersports.removeClass("display-none");
            self.Tables.checkboxEnableAutosuspendOthersports.attr("disabled", null);
        }

        self.Tables.settingContent.removeClass("display-none");
    };
};

var autoSuspendStatus = new AutoSuspendStatus();
