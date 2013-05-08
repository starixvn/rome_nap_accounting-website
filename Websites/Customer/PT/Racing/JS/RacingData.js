// JS - Racing data manager
var RacingData = {
    custId: 0,
    setting: {},
    getCustomerSetting: function (custId, callback) {
        if (typeof custId != 'undefined') {
            var params = { custId: custId, roleId: Roles.CurrRole };
            JAjax.call({
                type: "GET",
                url: "../../../Services/WCF/PT/Racing/Handler.svc/GetCustomerSetting",
                data: params,
                success: function (resp) {
                    if (typeof resp != 'undefined') {
                        if (typeof callback == 'function' && callback) {
                            callback(resp);
                            $.extend(RacingData.setting, resp);
                        }
                    }
                },
                isRefresh: true
            });
        }
    },
    getCustomerStatus: function (custId, callback) {
        if (typeof custId != 'undefined' && parseInt(custId) > 0) {
            var params = { custId: custId };
            JAjax.call({
                type: "GET",
                url: "../../../Services/WCF/PT/Racing/Handler.svc/GetCustomerStatus",
                data: params,
                success: function (resp) {
                    if (typeof resp != 'undefined') {
                        if (typeof callback == 'function' && callback) {
                            callback(resp);
                            $.extend(RacingData.setting, resp);
                        }
                    }
                },
                isRefresh: true
            });
        }
    },
    updateCustomerSetting: function (obj) {
        var crypting = typeof Roles.PageCrypt != 'undefined' ? Roles.PageCrypt : "";
        var data = JSON.stringify({ setting: $.extend(obj, { Crypting: crypting }) });
        clearMessage("err");
        JAjax.call({
            type: "POST",
            url: "../../../Services/WCF/PT/Racing/Handler.svc/UpdateCustomerSetting",
            data: data,
            success: function (resp) {
                if (typeof resp != 'undefined') {
                    RacingData.afterUpdate(resp);
                }
            }
        });
    },
    updateCustomerStatus: function (obj) {
        var params = JSON.stringify({ setting: obj });
        var custId = obj.CustInfo.CustId;
        clearMessage("err");
        JAjax.call({
            type: "POST",
            url: "../../../Services/WCF/PT/Racing/Handler.svc/UpdateCustomerStatus",
            data: params,
            success: function (resp) {
                if (typeof resp != 'undefined') {
                    var isSuccess = false;
                    isSuccess = showEnrichMessage("err", RacingData.filterErrorStatus(resp));
                    if (isSuccess) {
                        window.setTimeout(function () {
                            try { parent.ClosePopup(); } catch (e) { }
                        }, 3000);
                    }
                }
            }
        });
    },
    afterUpdate: function (result) {
        var isSuccess = false;
        isSuccess = showEnrichMessage("err", RacingData.filterError(result.ErrorHandler));
        if (RacingSetting.isNewPT) {
            RacingSetting.isNewPT = false;
            RacingSetting.newMemberPT = 0;
        }
        var custId = RacingSettingObj.CustInfo.CustId;
        if (isSuccess) {
            window.setTimeout(function () {
                RacingData.getCustomerSetting(custId, RacingSetting.init);
            }, 3000);
        } else {
            RacingData.f5Setting(custId);
        }
    },
    filterError: function (errorHandler) {
        var arrError = [];
        if (errorHandler != null && errorHandler.length > 0) {
            $.each(errorHandler, function (index, msg) {
                var arrMsg = msg.Value.split('<br>');
                var arrGet = [];
                for (var i = 0, len = arrMsg.length; i < len; i++) {
                    var macthed = arrMsg[i].toLowerCase().match(sportHidden.sports.join('|').toLowerCase());
                    if (macthed == null) {
                        arrGet.push(arrMsg[i]);
                    }
                }
                if (arrGet.length > 0) {
                    msg.Value = arrGet.join('<br>');
                }
                arrError.push(msg);
            });
        }
        return arrError;
    },
    filterErrorStatus: function (result) {
        if (result.ErrorMessage) {
            result.ErrorHandler = [{ Key: result.ErrorCode, Value: result.ErrorMessage}];
        }
        return this.filterError(result.ErrorHandler);
    },
    f5Setting: function (custId) {
        if (typeof custId != 'undefined' && parseInt(custId) > 0) {
            var params = { custId: custId, roleId: Roles.CurrRole };
            JAjax.call({
                type: "GET",
                url: "../../../Services/WCF/PT/Racing/Handler.svc/GetCustomerSetting",
                data: params,
                success: function (resp) {
                    if (typeof resp != 'undefined') {
                        try { $.extend(RacingSettingObj, resp); } catch (ex) { }
                    }
                },
                isRefresh: true
            });
        }
    }
};

