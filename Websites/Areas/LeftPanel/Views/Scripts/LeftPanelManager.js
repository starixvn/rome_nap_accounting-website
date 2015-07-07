var LeftPanelManager = function () {
    var self = this;
    self.updateBetTrade = function (
        custId,
        username,
        roleId,
        hasDownline,
        accId,
        productId) {
        var title = 'Update Bet Trade Status';
        var url = 'customersetting/enablestatus/productstatus?' +
            $.param({
                custId: custId,
                username: username,
                hasdownline: hasDownline,
                level: roleId,
                uplineid: 0,
                userid: accId,
                productid: 'BetTrade',
                enable: true,
                enabledownlines: true,
                enableuplines: true,
                product: productId
            });

        var width = 475;
        var height = 300;
        parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
    };

    self.updateMicroGaming = function (
        custId,
        username,
        roleId,
        hasDownline,
        accId,
        productId) {
        var title = 'Update Microgaming Status';
        var url = 'customersetting/enablestatus/productstatus?' +
            $.param({
                custId: custId,
                username: username,
                hasdownline: hasDownline,
                level: roleId,
                uplineid: 0,
                userid: accId,
                productid: 'MicroGaming',
                enable: true,
                enabledownlines: true,
                enableuplines: true,
                product: productId
            });

        var width = 475;
        var height = 300;
        parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
    };
};

var leftPanelManager = new LeftPanelManager();