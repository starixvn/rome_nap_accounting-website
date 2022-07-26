(function ($, common, rootUrl) {
    var boxContentDiv = $("#boxContent"),
        siteDropDown = $("#siteDropDown"),
        superDropDown = $("#superDropDown"),
        productDropDown = $("#productDropDown"),
        functionDropDown = $("#functionDropDown");
    var isMultipleSuperDropDownOpen = false;
    var styleSupperDD = "single";
    const PositionTakingAndStatusSettingTypeIds = [1, 18, 4, 11, 13];

    function getSelectedAgents() {
        var agentIds = $("#agentDropDown").multiSelect("getCheckedItems");
        var agentNames = agentIds.length > 0 ? $("#agentDropDown").multiSelect("getCheckedItemsByText") : [];

        return {
            AgentIds: agentIds.join(","),
            AgentNames: agentNames.join(", ")
        };
    }

    function getSelectedSupers() {
        var superIds = $("#superMultipleDropDown").multiSelect("getCheckedItems");
        var superNames = superIds.length > 0 ? $("#superMultipleDropDown").multiSelect("getCheckedItemsByText") : [];

        return {
            SuperIds: superIds.join(","),
            SuperNames: superNames.join(", ")
        };
    }

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

    function getSuperList(siteName, successCallback) {
        common.blockUI();
        $.post(
            rootUrl + "LicenseeDcsSetting/Supers",
            {site: siteName},
            function (data) {
                renderDropDown(superDropDown, data);
                renderSelectSuperList(data)
                addMultipleSuperDropDownEvents();
                successCallback(data);
            }).always(common.unblockUI);
    }

    function getAgentList(superId) {
        common.blockUI();
        $.post(
            rootUrl + `LicenseeDcsSetting/Agents`,
            {superId: superId},
            renderSelectAgentList)
            .always(common.unblockUI);
    }

    /* function getAgentList(param, url) {
         common.blockUI();
         $.post(rootUrl + `${url}`, param, renderSelectAgentList).always(common.unblockUI);
     }
 */
    function renderSelectAgentList(data) {
        var agent;
        var agents = [];

        for (var i = 0, max = data.length; i < max; i++) {
            agent = data[i];
            agents.push({
                Id: "agentDropDown" + agent.Id,
                Value: agent.Id,
                Text: agent.Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }

        $("#agentDropDown").multiSelect({
            minWidth: 200,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: "All",
            listData: agents,
            noItemText: "No Agent found.",
            showFilter: true,

        });
    }

    function renderSelectSuperList(data) {
        var superUserName;
        var superUserNames = [];

        for (var i = 0, max = data.length; i < max; i++) {
            superUserName = data[i];
            superUserNames.push({
                Id: "superDropDown" + superUserName.Id,
                Value: superUserName.Id,
                Text: superUserName.Name,
                Checked: true,
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
            isCheckAll: true,
            showFooter: false,
            checkAllText: "All",
            listData: superUserNames,
            noItemText: "No Super found.",
            showFilter: true,
        });
    }


    function siteDropDown_change() {
        resetFrom();
        getSuperList(this.value, function (data) {
            if (styleSupperDD === "single") {
                superDownDown_change();
            } else {
                superMultipleDownDown_change();

            }
        });
    }

    function superDownDown_change() {
        resetFrom();
        getAgentList(superDropDown.val().toString());
    }

    function superMultipleDownDown_change() {
        resetFrom();
        const superIds = $("#superMultipleDropDown").multiSelect("getCheckedItems").join(",");
        getAgentList(superIds);
    }

    function productDropDown_change() {
        resetFrom();

        var productId = parseInt(this.value);
        var setting = $.grep(productJSON, function (item, i) {
            return item.Id === productId;
        })[0];

        renderDropDown(functionDropDown, setting.SettingTypes);
    }

    function functionDropDown_change() {
        resetFrom();
        var functionId = parseInt(this.value);
        changeSuperDropDownStyle(PositionTakingAndStatusSettingTypeIds.includes(functionId));
    }

    function changeSuperDropDownStyle(isMultipleSelectionsSuper) {
        if (isMultipleSelectionsSuper) {
            styleSupperDD = "multi";
            superMultipleDownDown_change();
            $(".singleSuperSelection").hide();
            $(".multipleSuperSelections").show();
        } else {
            styleSupperDD = "single";
            superDownDown_change();
            $(".singleSuperSelection").show();
            $(".multipleSuperSelections").hide();

        }
    }
    
    function validateSuperAgent() {
        const {AgentNames} = getSelectedAgents();
        const unselectsAllAgent = AgentNames.length === 0;

        const {SuperNames} = getSelectedSupers();
        const unselectsAllSuper = SuperNames.length === 0;

        let warningMessage = "";
        if (unselectsAllAgent) {
            warningMessage += "Super Username is required. ";
        }
        if (unselectsAllSuper) {
            warningMessage += "Agent Username is required.";
        }
        return {
            isInvalid: warningMessage !== "",
            message: warningMessage
        };
    }

    function selectButton_click() {
        common.blockUI();
        const {isInvalid, message} = validateSuperAgent();
        if (isInvalid) {
            common.showMessage(1,message);
            common.unblockUI();
            return;
        }
        resetFrom();

        var functionId = functionDropDown.val(),
            functionUrl = getFunctionUrl(functionId);

        if (functionUrl.length === 0) {
            common.showNotImplementedMessage();
            common.unblockUI();
        } else {
            //const {SuperIds, SuperNames} = 
            var filterParam = {
                Site: siteDropDown.val(),
                ProductId: productDropDown.val(),
                ProductName: productDropDown.find("option:selected").text(),
                SuperId: superDropDown.val(),
                SuperName: superDropDown.find("option:selected").text(),
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
                    case "1":
                        ptHandler.initialize(defaultParam, getSelectedAgents);
                        break;
                    case "2":
                        betSettingHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "11":
                        betSettingWithoutMaxPerMatchHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "4":
                        statusHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "0":
                        globalStakeLevelHandler.initialize(defaultParam, selectButton_click);
                        break;
                    case "7":
                        stakeLevelHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
                        break;
                    case "8":
                        stakeLevelGroupHandler.initialize(defaultParam, selectButton_click, getSelectedAgents);
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
            case "1":
                url = "LicenseeDcsSetting/PT";
                break;
            case "2":
                url = "LicenseeDcsSetting/BetSetting";
                break;
            case "11":
                url = "LicenseeDcsSetting/BetSettingWithoutMaxPerMatch";
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
        renderSelectAgentList(agentJSON);
        renderSelectSuperList(superJSON);
        siteDropDown.change(siteDropDown_change).select2();
        superDropDown.change(superDownDown_change).select2();
        productDropDown.change(productDropDown_change).select2();
        functionDropDown.change(functionDropDown_change).select2({minimumResultsForSearch: Infinity});
        $("#selectButton").click(selectButton_click);
        changeSuperDropDownStyle(PositionTakingAndStatusSettingTypeIds.includes(parseInt((functionDropDown.val()))));
    }

    initialize();
})(jQuery, common, rootUrl);
