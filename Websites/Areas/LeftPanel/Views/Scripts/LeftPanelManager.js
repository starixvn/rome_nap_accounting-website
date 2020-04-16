var LeftPanelManager = function () {
    var self = this;
    self.updateStatus = function (
        custId,
        username,
        roleId,
        hasDownline,
        accId,
        enable,
        enableuplines,
        enabledownlines,
        title,
        productName,
        productId,
        settingType) {
        var url = ((productId == 24 || productId == 25) ? 'customersetting/enablestatus/productstatus/GetProductStatusNotAllowDisabledDownline?' : 'customersetting/enablestatus/productstatus?') +
            $.param({
                custId: custId,
                username: username,
                hasdownline: hasDownline ? true : false,
                level: roleId,
                uplineid: 0,
                userid: accId,
                productid: productName,
                enable: !enable,
                enabledownlines: false,
                enableuplines: enableuplines ? true : false,
                product: productId,
                hideDownline: !hasDownline,
                settingType: settingType
            });

        var width = 510;
        var height = 300;
        parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
    };

    self.updateThirdPartyProductStatus = function (
        custId,
        custName,
        roleId,
        productId,
        title,
        isDeposit) {
        var param = {
            custId: custId,
            custName: custName,
            roleId: roleId,
            productId: productId,
            isDeposit: isDeposit
        };

        parent.BlockUI();
        var jqxhr = $.post("ThirdPartyProductStatus/Index", param, function (data) { SuccessGetThirdPartyProductStatus(data, title); });
        jqxhr.fail(noPermission);
    };

    function SuccessGetThirdPartyProductStatus(data, title) {
        var dialoghtml = jQuery('<div id="dialog-modal" title="' + title + '">' + data + '</div>');
        dialoghtml.appendTo(jQuery("body"));
        parent.jQuery.unblockUI();
        parent.OpenPopup2($("#dialog-modal"));
    }

    function noPermission(xhr) {
        if (xhr.status === 403) {
            var denyObject = $.parseJSON(xhr.responseText);
            var dialoghtml = jQuery('<div id="dialog-modal">' + denyObject.ErrorMessage + '</div>');
            dialoghtml.appendTo(jQuery("body"));
            parent.jQuery.unblockUI();
            parent.OpenPopup2($("#dialog-modal"));
        }
    }

    self.viewProductStatusSetting = function (
        custId,
        custName,
        custRole,
        productId,
        title) {
        var param = {
            custId: custId,
            custName: custName,
            custRole: custRole,
            productId: productId
        };
        var url = "ProductStatusSetting/Index?" + $.param(param);
        var width = 680;
        var height = 270;
        if (productId === 8) {
            height = 520;
        } else if (productId === 41) {
            height = 360;
            width = 800;
        } else if (productId === 33) {
            height = 400;
            width = 800;
        } else if (productId === 44) {
            height = 320;
        }

        parent.OpenNextPopup("ProductStatusSetting", url, title, width, height, true, false, window, 0, null, null);
    };

    self.CN88EditCustStatus = function (custId, userName, status) {
        var param = {
            custId: custId,
            userName: userName,
            status: status
        };
        var url = "CustomerSetting/ViewCN88CustomerStatus?" + $.param(param);
        parent.OpenNextPopup("CN88CustomerStatusSetting", url, "Update Account Status", 400, 200, true, false, window, 0, null, null);
    }

    self.CN88EditCustCredit = function (searchAccountId, roleId, userName) {
        var param = {
            searchAccountId: searchAccountId,
            roleId: roleId
        };
        var url = "CustomerSetting/CreditPopup?" + $.param(param);
        parent.OpenNextPopup("CN88EditCustCredit", url, 'Update Credits - ' + userName, 400, 250, true, false, window, 0, null, null);
    }

    self.CN88UnlockCust = function (custId) {
        if (confirm('Do you want to unlock this account?')) {
            parent.BlockUI();
            $.ajax({
                type: "POST",
                url: "CustomerSetting/UpdateCN88UnlockAccess",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ custId: custId }),
                success: function (result) {
                    if (result.IsSuccess) {
                        setTimeout(function () {
                            top.window.frames['content'].submitSearch();
                        }, 3000)
                    }
                    else {
                        alert(result.ErrorMessage);
                    }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
    }

    self.CN88HeadlinePossess = function (custId, isAfterCreateNewSuper) {
        var param = {
            custId: custId,
            isAfterCreateNewSuper: isAfterCreateNewSuper
        };
        var url = "CustomerSetting/ViewHeadlinePossessOnSuper?" + $.param(param);
        parent.OpenNextPopup("CN88HeadlinePossessOnSuper", url, 'Edit Headline Possess on Super', 500, isAfterCreateNewSuper ? 400 : 300, true, false, window, 0, null, null);
    }
};

window.top.reloadLeftPanel = function () {
    window.top.content.$("#dialog-modalProductStatusSetting").dialog('close');
    var iframe = parent.document.getElementById("leftPanel");
    iframe.src = iframe.src;
    this.$.unblockUI();
}

var leftPanelManager = new LeftPanelManager();
try { parent.jQuery.unblockUI(); } catch (e) { }