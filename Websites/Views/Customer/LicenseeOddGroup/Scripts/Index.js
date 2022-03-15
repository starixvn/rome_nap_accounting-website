(function ($, common, rootUrl) {
    var boxContentDiv = $("#boxContent"),
        siteDropDown = $("#siteDropDown"),
        superDropDown = $("#superDropDown");
    
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
            { site: siteName },
            function (data) {
                renderDropDown(superDropDown, data);
                successCallback(data);
            }).always(common.unblockUI);
    }

    function getAgentList(superId) {
        common.blockUI();
        $.post(
            rootUrl + "LicenseeOddsGroup/Agents",
            { superId: superId },
            renderSelectAgentList)
            .always(common.unblockUI);
    }

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
            showFilter: true,
            checkAllText: "All",
            listData: agents,
            noItemText: "No Agent found."
        });
    }

    function siteDropDown_change() {
        resetFrom();
        getSuperList(this.value, function (data) {
            if (data.length > 0) {
                getAgentList(data[0].Id);
            }
        });
    }

    function superDownDown_change() {
        resetFrom();
        getAgentList(this.value);
    }

    function selectButton_click() {
        common.blockUI();
        resetFrom();

        var filterParam = {
            custIds: $("#agentDropDown").multiSelect("getCheckedItems").join(),
            siteName: siteDropDown.val()
        };

        $.post("GetAgentOddGroups", filterParam, function (result) {
            boxContentDiv.removeClass("hide").html(result);
            oddsGroupHandler.initialize(selectButton_click);
        }).always(common.unblockUI);
    }

    function initialize() {
        renderSelectAgentList(agentJSON);
        siteDropDown.change(siteDropDown_change).select2();
        superDropDown.change(superDownDown_change).select2();
        $("#selectButton").click(selectButton_click);
    }

    initialize();
})(jQuery, common, rootUrl);
