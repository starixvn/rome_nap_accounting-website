$(document).ready(function () {
    replicateLicenseeSettings.initPage();
});

var ReplicateLicenseeSettings = function () {
    var self = this;
    self.Tables = {
        textboxUsername: null
    };

    self.Params = {
        submitParams: {
            LicenseeName: ''
        }
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

    self.initPage = function () {
        self.cachedDOMElements();
        self.bindingEvent();
    };

    self.cachedDOMElements = function () {
        self.Tables.textboxUsername = $("#textbox-username");
        self.Tables.buttonSubmit = $("#button-submit");
        self.Tables.resultMessage = $("#result-message");
    };

    self.bindingEvent = function () {
        self.Tables.buttonSubmit.unbind("click");
        self.Tables.buttonSubmit.click(function () {
            self.ReplicateLicenseeSettings();
        });
    };

    self.ReplicateLicenseeSettings = function () {
        self.blockUI();
        self.Params.submitParams.LicenseeName = self.Tables.textboxUsername.val();
        if (self.Params.submitParams.LicenseeName == '') {
            showAlertMessage("result-message", "Please input licensee name!", 1);
            self.Tables.resultMessage.removeClass("display-none");
            self.unblockUI();
            return;
        }

        self.Tables.resultMessage.empty().addClass("display-none");
        self.CallAjax(self,
            "DoReplicateLicenseeSettings",
            self.Params.submitParams,
            self.RenderReplicateResult,
            self.RenderReplicateResult,
            self.unblockUI);
    };

    self.RenderReplicateResult = function (replicateResult) {
        if (replicateResult == null) {
            showAlertMessage("result-message", "Something is wrong!", 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        if (replicateResult.ErrorCode > 0) {
            showAlertMessage("result-message", replicateResult.ErrorMessage, 1);
            self.Tables.resultMessage.removeClass("display-none");
            return;
        }

        showAlertMessage("result-message", replicateResult.ErrorMessage, 0);
        self.Tables.resultMessage.removeClass("display-none");
        setTimeout(function () {
            self.Tables.resultMessage.addClass("display-none");
        }, 5000);
    };
};

var replicateLicenseeSettings = new ReplicateLicenseeSettings();