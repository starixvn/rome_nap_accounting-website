var cloneSettingManager;
$(document).ready(function () {
    cloneSettingManager.initPage();
});

var CloneSettingManager = function () {
    var self = this;
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
                if (successFunction && typeof (successFunction) === 'function') {
                    successFunction.call(reportHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) === 'function') {
                    errorFunction.call(reportHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) === 'function') {
                    completeFunction();
                }
            }
        });
    };

    self.initPage = function () {
        self.blockUI();
        self.bindingEvent();
        self.unblockUI();
    };
    self.bindingEvent = function () {
        $('#submit-button').click(function () {
            self.blockUI();
            var criteria = {
                TargetUsername: $.trim($("#TargetUsername").val()),
                DestinationUsername: $.trim($("#DestinationUsername").val())
            };

            if (self.validateInput(criteria)) {
                self.CallAjax(self,
                    window.rootUrl + "TargetAccount/Index",
                    criteria,
                    self.renderUpdateResult,
                    self.renderUpdateResult,
                    self.unblockUI);
            } else {
                self.unblockUI();
            }
        });
    };
    self.validateInput = function (criteria) {
        var resultMessage = $('#result-message');
        if (!criteria.TargetUsername) {
            showAlertMessage("result-message", viewResources.CloneSetting_Error_EmptyTargetUsername, 1);
            resultMessage.removeClass('display-none');
            $("#TargetUsername").addClass('error-input');
            return false;
        } else {
            $("#TargetUsername").removeClass('error-input');
        }

        if (!criteria.DestinationUsername) {
            showAlertMessage("result-message", viewResources.CloneSetting_Error_EmptyDestinationUsername, 1);
            resultMessage.removeClass('display-none');
            $("#DestinationUsername").addClass('error-input');
            return false;
        } else {
            $("#DestinationUsername").removeClass('error-input');
        }

        if (criteria.DestinationUsername.toLowerCase() == criteria.TargetUsername.toLowerCase()) {
            showAlertMessage("result-message", viewResources.CloneSetting_Error_SameTargetDestinationUsername, 1);
            resultMessage.removeClass('display-none');
            $("#DestinationUsername").addClass('error-input');
            return false;
        } else {
            $("#DestinationUsername").removeClass('error-input');
        }

        resultMessage.addClass('display-none');
        return true;
    };
    self.renderUpdateResult = function (updateResult) {
        updateResult.ErrorCode = updateResult.errorCode || updateResult.ErrorCode;
        updateResult.ErrorMessage = updateResult.errorMessage || updateResult.ErrorMessage;

        var resultMessage = $('#result-message');
        if (updateResult == null) {
            showAlertMessage("result-message", viewResources.System_Error, 1);
            resultMessage.removeClass("display-none");
            return;
        }

        if (updateResult.ErrorCode > 0) {
            showAlertMessage("result-message", updateResult.ErrorMessage, 1);
            resultMessage.removeClass("display-none");
            return;
        }

        showAlertMessage("result-message", updateResult.ErrorMessage, 0);
        resultMessage.removeClass("display-none");
        setTimeout(function () {
            resultMessage.addClass("display-none");
        }, 3000);
    };
};

cloneSettingManager = new CloneSettingManager();