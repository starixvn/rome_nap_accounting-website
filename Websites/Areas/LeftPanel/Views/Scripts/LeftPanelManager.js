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
        custName,
        roleId,
        productId,
        title,
        isDeposit) {
        var param = {
            custId: custId,
            custName: custName,
            custRoleId: roleId,
            productId: productId,
            isDeposit: isDeposit
        };

        var url = "ProductStatusSetting/Index?" + $.param(param);
        var width = 680;
        var height = 250;
        if (productId === 8) {
            height = 500;
        } else if (productId === 21) {
            height = 300;
        }

        parent.OpenNextPopup("ProductStatusSetting", url, title, width, height, true, false, window, 0, null, null);
    };
};

var leftPanelManager = new LeftPanelManager();