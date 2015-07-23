var editSettingManager;

$(document).ready(function () {
    editSettingManager.initPage();
});

var EditSettingManager = function () {
    var self = this;

    self.documentElements = {
        buttonUpdate: null,
        resultMessage: null
    };
    self.params = {
        updateCustomerInfo: null
    };
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
        self.cacheDOMElements();
        self.initElements();

        if (customerInfo.SelectResult.Code || !viewResources.HasHostPermission) {
            return;
        }

        self.bindingEvent();
    };

    self.cacheDOMElements = function () {
        self.documentElements.buttonUpdate = $(".btn-update-setting");
        self.documentElements.resultMessage = $("#result-message");
    };
    self.initElements = function () {
        if (viewResources.HasHostPermission) {
            self.documentElements.buttonUpdate.removeAttr("disabled");
        }

        if (customerInfo.SelectResult.Code) {
            showAlertMessage("result-message", customerInfo.SelectResult.Message, 1);
            return;
        }

        self.renderGameTypeList();
    };
    self.renderGameTypeList = function () {
        var gameTypeListLength = customerInfo.GameTypeList.length;
        var html = '';
        for (var i = 0; i < gameTypeListLength; i++) {
            var gameType = customerInfo.GameTypeList[i];
            html += '<tr class="game-type-row">\
                        <td class="l" colspan="2">' + gameType.Name + '</td>\
                        <td class="l" colspan="2">' + self.renderDropdownPT(gameType) + '</td>\
                    </tr>';
        }

        var ptContainer = $("#setting-content tbody");
        ptContainer.find(".game-type-row").remove();
        ptContainer.append(html);
    };
    self.renderDropdownPT = function (gameType) {
        var html = '<select class="dropdown-positiontaking" bettype="' + gameType.Id + '">';
        html += self.getPTOptions({
            minPT: undefined,
            maxPT: gameType.MaxPositionTaking,
            ptValue: gameType.PositionTaking
        });
        html += '</select>';
        return html;
    };
    self.getPTOptions = function (ptInfo) {
        var minPT = ptInfo.minPT != undefined ?
            parseFloat(ptInfo.minPT.toFixed(2)) :
            0;
        var maxPT = ptInfo.maxPT;
        var ptValue = ptInfo.ptValue;
        var options = "";

        var pt = maxPT;
        var val;
        var hasSelected = false;
        var ptStep = 0.01;
        while (pt > minPT) {
            val = parseFloat(pt.toFixed(2));
            var isSlected = ptValue == val;
            if (isSlected) {
                hasSelected = true;
            }

            options += "<option value='" + val + "'" +
                    (isSlected ? ' selected="selected"' : '') +
                ">" + val + "</option>";
            pt -= ptStep;
        }

        val = minPT;
        options += "<option " + (!hasSelected ? ' selected="selected"' : '') + " value='" + val + "'>" +
            val + "</option>";
        return options;
    };
    self.bindingEvent = function () {
        self.documentElements.buttonUpdate.click(self.buttonUpdateClick);
    };
    self.buttonUpdateClick = function () {
        if (!viewResources.HasHostPermission) {
            return;
        }

        self.blockUI();
        self.getUpdateParams();
        self.CallAjax(self,
            window.rootUrl + "MicroGaming/EditSetting",
            self.params.updateCustomerInfo,
            self.renderUpdateResult,
            self.renderUpdateResult,
            self.unblockUI);
    };
    self.getUpdateParams = function () {
        self.params.updateCustomerInfo = customerInfo;
        var gameTypeListLength = customerInfo.GameTypeList.length;
        for (var i = 0; i < gameTypeListLength; i++) {
            var gameType = self.params.updateCustomerInfo.GameTypeList[i];
            self.params.updateCustomerInfo.GameTypeList[i].PositionTaking = $('.dropdown-positiontaking[bettype="' + gameType.Id + '"]').val();
        }
    };
    self.renderUpdateResult = function (updateResult) {
        if (updateResult == null) {
            showAlertMessage("result-message", viewResources.Error_SystemError, 1);
            return;
        }

        if (updateResult.ErrorCode > 0) {
            showAlertMessage("result-message", updateResult.ErrorMessage, 1);
            return;
        }

        showAlertMessage("result-message", updateResult.ErrorMessage, 0);
        setTimeout(function () {
            self.documentElements.resultMessage.empty();
        }, 3000);
    };
};

editSettingManager = new EditSettingManager();