/* global hasHostPermission, model, rootUrl, showAlertMessage */
/* jshint unused: vars */
var updateStatus;
$(document).ready(function () {
    updateStatus.initPage();
});

var UpdateStatus = function () {
    var self = this;
    self.enums = {
        RoleId: {
            Super: 4,
            Master: 3,
            Agent: 2,
            Member: 1
        }
    };

    self.domElements = {
        chkSetting: null,
        labelSetting: null,
        chkDownlineSetting: null,
        labelDownlineSetting: null,
        customerInfoTitle: null,
        resultMessage: null
    };
    var dirty = false;


    self.blockUI = function () {
        try {
            var blocker = parent.parent.BlockUI ? parent.parent : parent;
            blocker.BlockUI();
        }
        catch (e) {
        }
    };
    self.unblockUI = function () {
        try {
            var blocker = parent.parent.$ && parent.parent.$.unblockUI ?
             parent.parent.$ :
             parent.$;
            blocker.unblockUI();
        }
        catch (e) {
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
    self.initPage = function () {
        if (!model) {
            $('#setting-container').addClass('display-none');
            showAlertMessage('result-message', viewResources.Invalid_Customer, 1);
            $('#result-message').removeClass("display-none");
            return;
        }

        self.cacheElements();
        self.initElements();
        $('#setting-container').removeClass('display-none');
        if (hasHostPermission === 0) {
            return;
        }

        self.bindEvents();
    };
    self.cacheElements = function () {
        self.domElements.chkSetting = $("#chkSetting");
        self.domElements.labelSetting = $('label-for-checkbox[for="chkSetting"]');
        self.domElements.chkDownlineSetting = $("#chkDownlineSetting");
        self.domElements.labelDownlineSetting = $('label-for-checkbox[for="chkDownlineSetting"]');
        self.domElements.customerInfoTitle = $('#customer-info-title');
        self.domElements.resultMessage = $('#result-message');
    };
    self.initElements = function () {
        if (model.Enable) {
            self.domElements.chkSetting.attr('checked', 'checked');
        }

        self.domElements.customerInfoTitle.html(self.getRoleName(model.RoleId) + ': ' + model.Username);
        self.domElements.resultMessage.html('').addClass('display-none');
        if (hasHostPermission === 0) {
            self.domElements.chkSetting.attr('disabled', 'disabled');
            self.domElements.chkDownlineSetting.attr('disabled', 'disabled');
        }
    };
    self.bindEvents = function () {
        $('#chkSetting').click(function () {
            var $this = $(this);
            self.statusToggle($this.attr('checked'), $this.attr('data-target'));
        });

        $('#button-update').click(function () {
            self.updateStatus();
        });
    };
    self.statusToggle = function (status) {
        var oldStatus = model.Enable;
        var downlineCheckbox = $('#chkDownlineSetting');
        var downlineLabel = $('#lblDownlineSetting');

        if (oldStatus && !status) {
            downlineCheckbox.removeAttr('checked').attr('disabled', 'disabled');
            downlineLabel.attr('disabled', 'disabled');
        } else if (!oldStatus && status) {
            downlineCheckbox.removeAttr('disabled');
            downlineLabel.removeAttr('disabled');
        }

        self.checkFormDirty();
    };
    self.checkFormDirty = function () {
        var isEnable = $('#chkSetting').attr('checked');
        dirty = model.Enable !== isEnable;

        var custBtn1 = $('.custBtn1'),
            custBtn2 = $('.custBtn2');
        if (dirty) {
            custBtn1.removeAttr('disabled');
            custBtn2.removeAttr('disabled');
        }
        else {
            custBtn1.attr('disabled', 'disabled');
            custBtn2.attr('disabled', 'disabled');
        }

        return dirty;
    }
    self.getRoleName = function (roleId) {
        switch (roleId) {
            case self.enums.RoleId.Super:
                return viewResources.Super;
            case self.enums.RoleId.Master:
                return viewResources.Master;
            case self.enums.RoleId.Agent:
                return viewResources.Agent;
            case self.enums.RoleId.Member:
                return viewResources.Member;
            default:
                return "";
        }
    };
    self.updateStatus = function () {
        if (!hasHostPermission || !dirty) {
            return;
        }

        self.blockUI();
        var criteria = {
            CustId: model.CustId,
            IsEnable: self.domElements.chkSetting.attr('checked'),
            ApplyToMember: self.domElements.chkDownlineSetting.attr('checked')
        };

        self.CallAjax(self,
            window.rootUrl + "BetTrade/UpdateStatus",
            criteria,
            self.renderUpdateResult,
            self.renderUpdateResult,
            self.unblockUI);
    };
    self.renderUpdateResult = function (result) {
        var code = result.ErrorCode === 0 ? 0 : 1;
        showAlertMessage('result-message', result.ErrorMessage, code);
        $('#result-message').removeClass("display-none");
        if (code === 0) {
            window.setTimeout(function () {
                try { parent.ClosePopup(); } catch (e) { }
            }, 3000);
        }
    };
};

var updateStatus = new UpdateStatus();