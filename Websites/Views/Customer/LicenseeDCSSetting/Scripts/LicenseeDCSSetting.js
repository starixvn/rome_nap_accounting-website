(function ($, common, rootUrl) {
    var boxContentDiv = $("#boxContent"),
        siteDropDown = $("#siteDropDown"),
        superDropDown = $("#superDropDown"),
        productDropDown = $("#productDropDown"),
        functionDropDown = $("#functionDropDown");

    function resetFrom() {
        common.hideMessage();
        boxContentDiv.addClass("hide").empty();
    }

    function renderDropDown(element, data) {
        var datum;
        var html = [];

        for (var i = 0, max = data.length; i < max; i++) {
            datum = data[i];
            html.push('<option value="');
            html.push(datum.Id);
            html.push('">');
            html.push(datum.Name);
            html.push("</option>");
        }

        return element.html(html.join(""));
    }

    function siteDropDown_change() {
        common.blockUI();
        resetFrom();
        var siteId = this.value;
        $.post(
            rootUrl + "LicenseeDcsSetting/Supers",
            { site: siteId },
            function (data) {
                renderDropDown(superDropDown, data);
            }).always(common.unblockUI);
    }

    function productDropDown_change() {
        resetFrom();

        var productId = parseInt(this.value);
        var setting = $.grep(productJSON, function (item, i) {
            return item.Id === productId;
        })[0];

        renderDropDown(functionDropDown, setting.SettingTypes);
    }

    function selectButton_click() {
        common.blockUI();
        resetFrom();

        var functionId = functionDropDown.val(),
            functionUrl = getFunctionUrl(functionId);

        if (functionUrl.length === 0) {
            common.showNotImplementedMessage();
            common.unblockUI();
        } else {
            var filterParam = {
                Site: siteDropDown.val(),
                ProductId: productDropDown.val(),
                ProductName: productDropDown.find("option:selected").text(),
                SuperId: superDropDown.val(),
                SuperName: superDropDown.find("option:selected").text()
            };

            var successFunction = function (data) {
                boxContentDiv.removeClass("hide").html(data);
                var defaultParam = {
                    ProductId: filterParam.ProductId,
                    SuperId: filterParam.SuperId
                };

                switch (functionId) {
                    case "1":
                        ptHandler.initialize(defaultParam);
                        break;
                    case "2":
                        betSettingHandler.initialize(defaultParam, selectButton_click);
                        break;
                    case "4":
                        statusHandler.initialize(defaultParam, selectButton_click);
                        break;
                    case "0":
                        globalStakeLevelHandler.initialize(defaultParam, selectButton_click);
                        break;
                    case "7":
                        stakeLevelHandler.initialize(defaultParam, selectButton_click);
                        break;
                    case "8":
                        stakeLevelGroupHandler.initialize(defaultParam, selectButton_click);
                        break;
                    default:
                        break;
                }
            };

            $.post(functionUrl, filterParam, successFunction).always(common.unblockUI);
        }
    }

    function getFunctionUrl(functionId) {
        var url;
        switch (functionId) {
            case "1":
                url = "LicenseeDcsSetting/PT";
                break;
            case "2":
                url = "LicenseeDcsSetting/BetSetting";
                break;
            case "4":
                url = "LicenseeDcsSetting/Status";
                break;
            case "0":
                url = "LicenseeDcsSetting/GlobalStakeLevel";
                break;
            case "7":
                url = "LicenseeDcsSetting/StakeLevel";
                break;
            case "8":
                url = "LicenseeDcsSetting/StakeLevelGroup";
                break;
            default:
                url = "";
                break;
        }

        return url.length === 0 ? "" : rootUrl + url;
    }

    function initialize() {
        siteDropDown.change(siteDropDown_change).select2();
        superDropDown.change(resetFrom).select2();
        productDropDown.change(productDropDown_change).select2({ minimumResultsForSearch: Infinity });
        functionDropDown.change(resetFrom).select2({ minimumResultsForSearch: Infinity });
        $("#selectButton").click(selectButton_click);
    }

    initialize();
})(jQuery, common, rootUrl);
