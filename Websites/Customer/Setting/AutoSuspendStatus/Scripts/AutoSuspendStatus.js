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

        checkboxEnableAutosuspendSoccer: null,
        checkboxEnableAutosuspendOthersports: null
    };

    self.Params = {
        getCustomerInfo: {
            CustomerId: 0
        }
    };

    self.CacheData = {
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

        self.Tables.checkboxEnableAutosuspendSoccer = $("#checkbox-enable-autosuspend-soccer");
        self.Tables.checkboxEnableAutosuspendOthersports = $("#checkbox-enable-autosuspend-othersports");
    };
    self.bindingEvent = function () {
        self.Tables.buttonUpdate.unbind("click");
        self.Tables.buttonUpdate.click(function () {
            self.buttonUpdateClick();
        });
    };

    self.buttonUpdateClick = function () {

    };

    self.GetCustomerInfo = function () {
        self.Params.getCustomerInfo = {
            CustomerId: viewResourses.CustomerId
        };

        self.CallAjax(self,
            "../../../AutoSuspend/GetCustomerStatus",
            self.Params.getCustomerInfo,
            self.RenderCustomerInfo,
            self.RenderCustomerInfo,
            self.unblockUI);
    };

    self.RenderCustomerInfo = function (customerInfo) {
        if (customerInfo == null) {
            self.Tables.settingContent.addClass("display-none");
            showAlertMessage("result-message", "There is no customer info.", 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        self.Tables.resultMessage.addClass("display-none");

        var customerInfoTitle = self.getRoleName(customerInfo.RoleId) + ": " + customerInfo.Username;
        self.Tables.customerInfoTitle.html(customerInfoTitle);

        self.Tables.checkboxEnableAutosuspendSoccer.attr("disabled",
            customerInfo.UplineAutoSuspendSoccer ? "" : "disabled");
        self.Tables.checkboxEnableAutosuspendOthersports.attr("disabled",
            customerInfo.UplineAutoSuspendOtherSports ? "" : "disabled");


        self.Tables.settingContent.removeClass("display-none");
    };
};

var autoSuspendStatus = new AutoSuspendStatus();
