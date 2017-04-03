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
        var url = ((productId == 24 || productId == 25) ? 'customersetting/enablestatus/productstatus/GetProductStatusNotAllowDisabledDownline?' : 'customersetting/enablestatus/productstatus/?') +
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
        roleId,
        productId,
        title) {
        var param = {
            custId: custId,
            roleId: roleId,
            productId: productId
        };

        parent.BlockUI();
        $.post("ThirdPartyProductStatus/Index", param, function (data) { SuccessGetThirdPartyProductStatus(data, title); });
    };

    function SuccessGetThirdPartyProductStatus(data, title) {
        var dialoghtml = jQuery('<div id="dialog-modal" title="' + title + '">' + data + '</div>');
        dialoghtml.appendTo(jQuery("body"));
        parent.jQuery.unblockUI();
        parent.OpenPopup2($("#dialog-modal"));
    }
};

var leftPanelManager = new LeftPanelManager();