"use strict";
/* jshint unused: vars,
camelcase: false */
/* global custInfo,
showAlertMessage,
viewResources */
/* exported changeStatusManager */
var changeStatusManager;

$(document).ready(function () {
    changeStatusManager.initPage();
});

var ChangeStatusManager = function () {
    var self = this;
    self.Tables = {
        resultMessage: null
    };

    self.CachedData = {
        successShowTime: 3000,
        gridOpts: {
            tblContentId: 'update-effect-content',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 100,
            freezeWidth: 0,
            freezeHeight: 50,
            numberFreezeCol: 0
        },
        updateEffectLength: 0
    };
    self.Params = {
        loadUpdateEffect: {
            CustomerId: 0,
            Username: '',
            IsCheckedDirectStatus: false
        },

        updateCustomerInfo: {
            CustomerId: 0,
            Username: '',
            IsCheckedDirectStatus: false
        }
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

    self.cachedDOMElements = function () {
        self.Tables.resultMessage = $("#result-message");
        self.Tables.updateEffectContainer = $("#update-effect-container");
    };
    self.initPage = function () {
        self.blockUI();
        parent.window.isUpdateDirectStatus = false;
        self.cachedDOMElements();
        $("#result-message").addClass("display-none");
        $("#setting-content").removeClass("display-none");

        var gridOpts = {
            tblContentId: 'update-effect-content',
            isShowPager: false
        };
        self.Tables.updateEffectContainer.initGrid(gridOpts);
        self.bindingEvent();
        self.loadUpdateEffect();
    };
    self.bindingEvent = function () {
        $("#view-update-effect").click(function () {
            self.showUpdateEffect();
        });

        $("#button-update").click(function () {
            self.updateDirectAccountStatus();
        });

        $("#button-cancel").click(function () {
            self.cancelUpdate();
        });
    };
    self.loadUpdateEffect = function () {
        self.blockUI();
        self.Params.loadUpdateEffect = {
            CustomerId: custInfo.CustomerId,
            Username: custInfo.Username
        };

        self.CallAjax(self,
            window.rootUrl + "ConfigureDirectAccount/UpdateDirectAccountStatusEffect",
            self.Params.loadUpdateEffect,
            self.renderUpdateEffect,
            self.renderUpdateEffect,
            self.unblockUI);
    };
    self.showUpdateEffect = function () {
        self.Tables.updateEffectContainer.removeClass("display-none");
        self.Tables.updateEffectContainer.gridScroller(self.CachedData.gridOpts);
        self.updatePopupSize();
        self.Tables.updateEffectContainer.gridScroller(self.CachedData.gridOpts);
    };
    self.renderUpdateEffect = function (updateEffect) {
        var customerLength = updateEffect && updateEffect.CustomerList ?
            updateEffect.CustomerList.length :
            0;
        self.CachedData.updateEffectLength = customerLength;
        $("#number-effected-cust").html(customerLength);
        if (!updateEffect) {
            showAlertMessage("result-message", viewResources.Error_SystemError, 1);
            self.Tables.resultMessage.removeClass("display-none");
            self.renderNoDataUpdateEffect();
            return;
        }

        self.Tables.resultMessage.empty().addClass("display-none");
        if (customerLength) {
            var html = '';
            for (var i = 0; i < customerLength; i++) {
                var customer = updateEffect.CustomerList[i];
                html += '<tr>\
                            <td class="c">' + (i + 1) + '</td>\
                            <td class="c">' + customer.UserName + '</td>\
                            <td class="c">' + customer.StatusName + '</td>\
                            <td class="c">' + customer.RoleName + '</td>\
                         </tr>';
            }

            $("#update-effect-content tbody").empty().append(html);
        } else {
            self.renderNoDataUpdateEffect();
        }
    };
    self.renderNoDataUpdateEffect = function () {
        var html = '<tr>\
                    <td class="b c" style="height: 19px; width: 375px;" colspan="4">' + viewResources.Message_NoData + '</td>\
                </tr>';
        $("#update-effect-content tbody").empty().append(html);
    };
    self.updatePopupSize = function () {
        var popUpHeight = Math.min(self.CachedData.updateEffectLength * 20 + 300, parent.fullheight - 100);
        parent.$.find("#dialog-modal")[0].style.height = popUpHeight + "px";
        parent.$.find(".ui-dialog")[0].style.width = "420px";

        parent.$.find("#dialog-modal #innerFrame")[0].style.height = popUpHeight + "px";
        parent.$.find("#dialog-modal #innerFrame")[0].style.width = "399px";
        parent.$.find(".ui-widget-content")[0].style.top = (parent.fullheight - popUpHeight - 100) / 2 + "px";
    };

    self.updateDirectAccountStatus = function () {
        self.blockUI();
        self.Params.updateCustomerInfo = {
            CustomerId: custInfo.CustomerId,
            Username: custInfo.Username,
            IsCheckedDirectStatus: !custInfo.IsCheckedDirectStatus
        };

        self.CallAjax(self,
            window.rootUrl + "ConfigureDirectAccount/UpdateDirectAccountStatus",
            self.Params.updateCustomerInfo,
            self.renderUpdateResult,
            self.renderUpdateResult,
            self.unblockUI);
    };
    self.cancelUpdate = function () {
        parent.ClosePopup();
    };
    self.renderUpdateResult = function (updateResult) {
        if (updateResult == null) {
            showAlertMessage("result-message", viewResources.Error_SystemError, 1);
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
            parent.window.isUpdateDirectStatus = true;
            try {
                parent.ClosePopup();
            } catch (e) {
            }
        }, self.CachedData.successShowTime);
    };
};

var changeStatusManager = new ChangeStatusManager();