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
        } else if (productId === 21) {
            height = 320;
            width = 800;
        } else if (productId === 33) {
            height = 400;
            width = 800;
        }

        parent.OpenNextPopup("ProductStatusSetting", url, title, width, height, true, false, window, 0, null, null);
    };
};

window.top.reloadLeftPanel = function () {
    window.top.content.$("#dialog-modalProductStatusSetting").dialog('close');
    var iframe = parent.document.getElementById("leftPanel");
    iframe.src = iframe.src;
    this.$.unblockUI();
}

var leftPanelManager = new LeftPanelManager();
