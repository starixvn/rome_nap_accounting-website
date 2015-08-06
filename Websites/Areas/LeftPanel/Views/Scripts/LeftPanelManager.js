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
        productId) {
        var url = 'customersetting/enablestatus/productstatus?' +
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
                hideDownline: !hasDownline
            });

        var width = 475;
        var height = 300;
        parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
    };
};

var leftPanelManager = new LeftPanelManager();