(function ($, common, rootUrl) {
    const exchangeProductId = 31;
    var boxContentDiv = $("#boxContent"),
        siteDropDown = $("#siteDropDown"),
        superDropDown = $("#superDropDown"),
        productDropDown = $("#productDropDown"),
        functionDropDown = $("#functionDropDown"),
        agentSection = $(".agent-section");
    var isMultipleSuperDropDownOpen = false;
    var agentsExtraInfo = [];

    const pipe = (...fns) => {
        return fns.reduce((f, g) => (x) => g(f(x)));
    };

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

    function getSelectedAgents() {
        var agentIds = $('#agentDropDown').multiSelect("getCheckedItems");
        var agentNames = agentIds.length > 0 ? $('#agentDropDown').multiSelect("getCheckedItemsByText") : [];

        return {
            AgentIds: agentIds.join(","),
            AgentNames: agentNames.join(", "),
            AgentsExtraInfo: agentsExtraInfo
        };
    }

    function getSelectedSupers() {
        var superIds = $("#superMultipleDropDown").multiSelect("getCheckedItems");
        var superNames = superIds.length > 0 ? $("#superMultipleDropDown").multiSelect("getCheckedItemsByText") : [];

        return {
            SuperIds: superIds.join(","),
            SuperId: superIds.join(","),
            SuperNames: superNames.join(", "),
            SuperName: superNames.join(", ")
        };
    }

    function getSuperList(siteName, successCallback) {
        common.blockUI();
        $.post(
            rootUrl + "LicenseeDcsSetting/Supers",
            {site: siteName},
            function (data) {
                renderSelectSuperList(data)
                successCallback(data);
            }).always(common.unblockUI);
    }

    function getAgentList(superId, isCheckAllAgent = false) {
        common.blockUI();
        $.post(
            rootUrl + `LicenseeDcsSetting/Agents`,
            {superId: superId},
            (data) => renderSelectAgentList(data, isCheckAllAgent))
            .always(common.unblockUI);
    }


    function renderSelectAgentList(data, isChecked = false) {
        var agent;
        var agents = [];
        agentsExtraInfo = data;
        for (var i = 0, max = data.length; i < max; i++) {
            agent = data[i];
            agents.push({
                Id: "agentDropDown" + agent.Id,
                Value: agent.Id,
                Text: agent.Name,
                Checked: isChecked,
                Enable: true,
                CssClass: "",
                Items: [],
            });
        }

        $('#agentDropDown').multiSelect({
            minWidth: 200,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: isChecked,
            showFooter: false,
            checkAllText: "All",
            listData: agents,
            noItemText: "No Agent found.",
            showFilter: true,

        });
    }

    function renderSelectSuperList(data, isChecked = false) {
        var superUserName;
        var superUserNames = [];

        for (var i = 0, max = data.length; i < max; i++) {
            superUserName = data[i];
            superUserNames.push({
                Id: "superDropDown" + superUserName.Id,
                Value: superUserName.Id,
                Text: superUserName.Name,
                Checked: isChecked,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }

        $("#superMultipleDropDown").multiSelect({
            minWidth: 200,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: isChecked,
            showFooter: false,
            checkAllText: "All",
            listData: superUserNames,
            noItemText: "No Super found.",
            showFilter: true,
        });
        addMultipleSuperDropDownEvents();
    }


    function siteDropDown_change() {
        resetFrom();
        getSuperList(this.value, function (data) {
            superMultipleDownDown_change();
        });
    }

    function superMultipleDownDown_change() {
        resetFrom();
        const superIds = $("#superMultipleDropDown").multiSelect("getCheckedItems").join(",");
        getAgentList(superIds, true);

    }

    function productDropDown_change() {
        resetFrom();

        var productId = parseInt(this.value);
        var setting = $.grep(productJSON, function (item, i) {
            return item.Id === productId;
        })[0];

        renderDropDown(functionDropDown, setting.SettingTypes);
		functionDropDown.change();
    }

    function functionDropDown_change() {
        resetFrom();
        var settingType = parseInt(this.value);
        if (settingType == 5 || settingType == 18) {
            agentSection.hide();
        }
        else {
            agentSection.show();
        }
    }
    

    function validateSuper(warningMessage = "") {
        if (warningMessage !== "") return warningMessage;

        const {SuperNames} = getSelectedSupers();
        const unselectsAllSuper = SuperNames.length === 0;

        warningMessage += unselectsAllSuper ? "Super Username is required." : "";
        return warningMessage

    }

    function validateAgent(warningMessage = "") {
        if (warningMessage !== "") return warningMessage;

        const {AgentNames} = getSelectedAgents();
        const unselectsAllSuper = AgentNames.length === 0;

        warningMessage += unselectsAllSuper ? "Agent Username is required." : "";
        return warningMessage

    }

    function validateNumberOfSuper(warningMessage = "") {
        if (warningMessage !== "") return warningMessage;

        const functionId = functionDropDown.val();
        const superIds = $("#superMultipleDropDown").multiSelect("getCheckedItems");
        let message = "";
        if (["2","10","11","12"].includes(functionId)) {
            message = "Please select only 1 Super when updating the Bet Setting.";
        }
        if (["7","8"].includes(functionId)) {
            message = "Please select only 1 Super when updating the Bet Limit.";
        }
        if (["0"].includes(functionId)) {
            message = "Please select only 1 Super when updating the Global Bet Limit.";
        }

        warningMessage += superIds.length !== 1 ? message : "";

        return warningMessage;
    }

    function selectButton_click() {
        common.blockUI();
        var functionId = functionDropDown.val(),
            functionUrl = getFunctionUrl(functionId);

        const validate = pipe(validateSuper, validateAgent, validateNumberOfSuper);
        const message = validate();
        if (message) {
            common.showMessage(1, message);
            common.unblockUI();
            return false;
        }
        resetFrom();
        if (functionUrl.length === 0) {
            common.showNotImplementedMessage();
            common.unblockUI();
        } else {
            //const {SuperIds, SuperNames} = 
            var filterParam = {
                Site: siteDropDown.val(),
                ProductId: productDropDown.val(),
                ProductName: productDropDown.find("option:selected").text(),
                ...getSelectedSupers()
            };

            var successFunction = function (data) {
                boxContentDiv.removeClass("hide").html(data);
                var defaultParam = {
                    ProductId: filterParam.ProductId,
                    SuperId: filterParam.SuperId,
                    ...getSelectedSupers()
                };

                switch (functionId) {
                    case "0":
                        globalStakeLevelHandler.initialize(defaultParam, selectButton_click);
                        break;
                    case "1":
                        ptHandler.initialize(defaultParam, getSelectedAgents);
                        break;
                    case "2":
                        betSettingHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "4":
                        statusHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "5":
                        chargeCommissionHandler.initialize(defaultParam);
                        break;
                    case "7":
                        stakeLevelHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "8":
                        stakeLevelGroupHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "10":
                        betSettingWithMaxPayoutPerMatchHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "12":
                        betSettingWithMaxPerBallHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "18":
                        chargePTHandler.initialize(defaultParam);
                        break;
                    default:
                        break;
                }
            };

            if (functionId !== "0") {
                filterParam = $.extend(filterParam, getSelectedAgents());
            }

            $.post(functionUrl, filterParam, successFunction).always(common.unblockUI);
        }
    }

    function getFunctionUrl(functionId) {
        var url;
        switch (functionId) {
            case "0":
                url = "LicenseeDcsSetting/GlobalStakeLevel";
                break;
            case "1":
                url = "LicenseeDcsSetting/PT";
                break;
            case "2":
                url = "LicenseeDcsSetting/BetSetting";
                break;
            case "4":
                url = "LicenseeDcsSetting/Status";
                break;
            case "5":
                url = "LicenseeDcsSetting/ChargeCommission";
                break;
            case "7":
                url = "LicenseeDcsSetting/StakeLevel";
                break;
            case "8":
                url = "LicenseeDcsSetting/StakeLevelGroup";
                break;
            case "10":
                url = "LicenseeDcsSetting/BetSettingWithMaxPayoutPerMatch";
                break;
            case "12":
                url = "LicenseeDcsSetting/BetSettingWithMaxPerBall";
                break;
            case "18":
                url = "LicenseeDcsSetting/ChargePT";
                break;
            default:
                url = "";
                break;
        }

        return url.length === 0 ? "" : rootUrl + url;
    }

    function addMultipleSuperDropDownEvents() {
        const multipleSuperDropDown = $(".multipleSuperSelections .ui-multiselect-options");

        multipleSuperDropDown.on("open", function () {
            isMultipleSuperDropDownOpen = true;
        });
        multipleSuperDropDown.on("close", function () {
            if (isMultipleSuperDropDownOpen) {
                superMultipleDownDown_change();
                isMultipleSuperDropDownOpen = false;
            }
        });
    }

    function initialize() {
        renderSelectSuperList(superJSON);
        renderSelectAgentList([]);
        siteDropDown.change(siteDropDown_change).select2();
        productDropDown.change(productDropDown_change).select2();
        functionDropDown.change(functionDropDown_change).select2({minimumResultsForSearch: Infinity});
        $("#selectButton").click(selectButton_click);
    }

    initialize();
})(jQuery, common, rootUrl);
