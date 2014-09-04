var agCasinoSetting;

$(document).ready(function () {
    agCasinoSetting.initPage();
});

var AGCasinoSetting = function () {
    var self = this;
    self.Constants = {
        RoleId: {
            SUPER: 4,
            MASTER: 3,
            AGENT: 2,
            MEMBER: 1,
            SuperOnMember: -1,
            MasterOnMember: -2,
            SuperMin: -3,
            MasterMinMax: -4,
            AgentMinMax: -5
        }
    };

    self.Tables = {
    };

    self.CacheData = {
        customerSetting: null,
        isShowUpdate: false,

        changInfo: {},

        selectedDefaultGroup: null,
        selectedApplyGroup: null
    };

    self.Params = {
        getCustomerInfo: {
            CustomerId: 0
        },

        updateCustomerInfo: {
            CustomerId: 0,
            Role: null,
            ProductInfo: null,
            hasChecked: false
        },
        getAGCasinoGroup: null
    };

    self.blockUI = function () {
        try {
            var blocker = parent.parent.BlockUI ? parent.parent : parent;
            blocker.BlockUI();
        }
        catch (e) {
        }
    };

    self.unblockUI = function () {
        try {
            var blocker = parent.parent.$ && parent.parent.$.unblockUI ?
             parent.parent.$ :
             parent.$;
            blocker.unblockUI();
        }
        catch (e) {
        }
    };

    self.CallAjax = function (reportHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(requestParams),
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) == 'function') {
                    successFunction.call(reportHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) == 'function') {
                    errorFunction.call(reportHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) == 'function') {
                    completeFunction();
                }
            }
        });
    };

    self.initPage = function () {
        self.blockUI();
        self.cachedDOMElements();

        self.CacheData.isShowUpdate = viewResources.RoleId !== self.Constants.RoleId.MASTER &&
            viewResources.RoleId !== self.Constants.RoleId.AGENT;
        if (!self.CacheData.isShowUpdate) {
            self.Tables.buttonUpdate.remove();
        }

        self.bindingEvent();
        self.GetCustomerSetting();
    };

    self.cachedDOMElements = function () {
        self.Tables.tableInfo = $("#settingcontent .tableinfo");
        self.Tables.tableInfoBody = self.Tables.tableInfo.find("tbody");

        self.Tables.tableSetting = $("#settingcontent .tablesetting");
        self.Tables.tableSettingBody = self.Tables.tableSetting.find("tbody");

        self.Tables.buttonUpdate = $(".btn-update");

        self.Tables.copyToMemberContainer = $(".copy-tomember-container");
        self.Tables.dropdownCopyToMember = $("#dropdown-copy-tomember");

        self.Tables.defautGroupNewMemberContainer = $(".default-newmembergroup-container");
        self.Tables.dropdownDefautGroupNewMember = $("#dropdown-default-newmembergroup");

        self.Tables.resultMessage = $("#result-message");
    };
    self.bindingEvent = function () {
        if (!viewResources.HasHostPermission) {
            return;
        }

        self.Tables.buttonUpdate.click(self.buttonUpdateClick);
    };
    self.GetCustomerSetting = function () {
        self.Params.getCustomerInfo = {
            CustomerId: viewResources.CustomerId,
            UplineId: viewResources.UplineId,
            RoleId: viewResources.CurrentRole,
            ExchangeId: viewResources.exchangeId
        };

        self.CallAjax(self,
            window.rootUrl + "AGCasino/CustomerSetting",
            self.Params.getCustomerInfo,
            self.RenderCustomerSetting,
            self.RenderCustomerSetting,
            self.unblockUI);
    };

    self.RenderCustomerInfo = function (customerSetting) {
        var html = "";
        html += '<tr>\
                    <td width="120px" class="r b">Username:</td>\
                    <td width="120px" class="l">' + customerSetting.Username + '</td>\
                    <td width="120px" class="r b">Nick name:</td>\
                    <td width="240px" class="l">' + customerSetting.NickName + '</td>\
                </tr>\
                <tr>\
                    <td class="r b">First Name:</td>\
                    <td class="l">' + customerSetting.FirstName + '</td>\
                    <td class="r b">Last Name:</td>\
                    <td class="l">' + customerSetting.LastName + '</td>\
                </tr>\
                <tr>\
                    <td class="r b">Phone:</td>\
                    <td class="l">' + customerSetting.Phone + '</td>\
                    <td class="r b">Mobile Phone:</td>\
                    <td class="l">' + customerSetting.Mobile + '</td>\
                </tr>\
                <tr>\
                    <td class="r b">Fax:</td>\
                    <td class="l">' + customerSetting.Fax + '</td>\
                    <td class="r b"></td>\
                    <td class="l"></td>\
                </tr>';

        return html;
    };
    self.RenderCustomerGameSettingGroupName = function (gameType, gameTypeIndex) {
        var html = "";
        var groupLength = gameType.GameTypeGroup.length;
        html += '<tr>\
            <td class="r col-1">' + gameType.Name + '</td>\
            <td colspan="3" class="l col-gamekindgroups">';
        for (var j = 0; j < groupLength; j++) {
            var group = gameType.GameTypeGroup[j];
            var groupSuffix = "-group-" + group.Id;
            html += '<div id="gametype-' + gameType.Id + groupSuffix + '"' +
                        ' class="col-gamekindgroup label-for-checkbox' + (j === groupLength - 1 ? ' last-group' : '') + '" for="checkbox' + groupSuffix + '"' +
                        ' data-gametypeindex="' + gameTypeIndex + '"' +
                        ' data-groupindex="' + j + '"' +
                        ' onclick="agCasinoSetting.checkGroup($(this));">\
                            <span id="checkbox' + "-gametype-" + gameType.Id + groupSuffix + '"' +
                            ' class="' + (group.Enable ? "checkbox-checked" : "checkbox-unchecked") + '"></span>\
                            <label id="label' + groupSuffix + '">' + group.Name + '</label>\
                        </div>';
        }

        html += '</td></tr>';
        return html;
    };
    self.checkGroup = function (container) {
        var gameType = self.CacheData.customerSetting.ProductInfo[container.attr("data-gametypeindex")];
        var group = gameType.GameTypeGroup[container.attr("data-groupindex")];
        var checkbox = $("#checkbox-gametype-" + gameType.Id + "-group-" + group.Id);
        var isChecked = checkbox.hasClass("checkbox-checked");
        if (isChecked) {
            checkbox.removeClass("checkbox-checked").addClass("checkbox-unchecked");
        } else {
            checkbox.removeClass("checkbox-unchecked").addClass("checkbox-checked");
        }

        self.CacheData.changeInfo["gametype-" + gameType.Id + "-group-" + group.Id] = isChecked == group.Enable;

        self.updateSelectedGroup();
        self.EnableUpdateButton();
    };
    self.EnableUpdateButton = function () {
        if (!viewResources.HasHostPermission) {
            return;
        }

        var isChanged = false;
        for (var changeInfo in self.CacheData.changeInfo) {
            if (self.CacheData.changeInfo[changeInfo]) {
                isChanged = true;
                break;
            }
        }

        isChanged = isChanged ||
            (self.CacheData.selectedDefaultGroup &&
            self.CacheData.selectedDefaultGroup.Name !== self.CacheData.customerSetting.DefaultAGCasinoGroup.Name) ||
            (self.CacheData.selectedApplyGroup &&
            self.CacheData.selectedApplyGroup.Name !== self.CacheData.firstSelectedApplyGroup.Name);
        if (isChanged) {
            self.Tables.buttonUpdate.attr("disabled", null);
        } else {
            self.Tables.buttonUpdate.attr("disabled", "disabled");
        }
    };
    self.RenderCustomerGameSettingGameTypes = function (gameType, groupLength) {
        var html = "";
        var gameKindLength = gameType.GameTypes ? gameType.GameTypes.length : 0;
        var limitFormat = "#,##0";
        for (var j = 0; j < gameKindLength; j++) {
            var gameKind = gameType.GameTypes[j];
            html += '<tr>\
                    <td class="r col-1">' + gameKind.Name + '</td>\
                    <td colspan="3" class="l col-gamekindgroups">';

            for (var k = 0; k < groupLength; k++) {
                var settingLimit = gameKind.SettingLimitByGroup[k];
                if (settingLimit) {
                    html += '<div class="col-gamekindlimit' + (k === groupLength - 1 ? ' last-group' : '') + '">' +
                            numeral(settingLimit.MinBet).format(limitFormat) + " - " +
                            numeral(settingLimit.MaxBet).format(limitFormat) +
                        '</div>';
                } else {
                    html += '<div class="empty-group col-gamekindlimit' + (k === groupLength - 1 ? ' last-group' : '') + '">&nbsp;</div>';
                }
            }

            html += '</td></tr>';
        }

        return html;
    };
    self.RenderCustomerGameSetting = function (customerSetting) {
        var html = "";
        var gameTypeLength = customerSetting.ProductInfo ? customerSetting.ProductInfo.length : 0;
        for (var i = 0; i < gameTypeLength; i++) {
            var gameType = customerSetting.ProductInfo[i];
            var groupLength = gameType.GameTypeGroup.length;
            html += self.RenderCustomerGameSettingGroupName(gameType, i);
            html += self.RenderCustomerGameSettingGameTypes(gameType, groupLength);
        }

        return html;
    };

    self.RenderCustomerSetting = function (customerSetting) {
        self.CacheData.customerSetting = customerSetting;
        self.CacheData.changeInfo = {};
        if (customerSetting === null || customerSetting === undefined) {
            showAlertMessage('result-message', 'There is no infomation available.', 1);
            self.Tables.tableInfo.addClass("hide");
            self.Tables.tableSetting.addClass("hide");
            self.Tables.tableInfoBody.empty();
            self.Tables.tableSettingBody.empty();
            return;
        }

        self.Tables.resultMessage.empty();
        var cutomerInfoHtml = self.RenderCustomerInfo(customerSetting);
        self.Tables.tableInfoBody.empty().append(cutomerInfoHtml);

        var customerSettingHtml = '';
        if (self.CacheData.customerSetting.SelectResult.Code) {
            customerSettingHtml += '<tr class="title1"><td class="l" colspan="4">Game Setting</td></tr>';
            customerSettingHtml += '<tr><td colspan="4" class="c"><div class="no-info">' +
                    self.CacheData.customerSetting.SelectResult.Message +
                '</div></td></tr>';
            self.Tables.tableSetting.empty().append(customerSettingHtml);
        } else {

            var groupLength = customerSetting.ProductInfo[0].GameTypeGroup.length;
            if (groupLength > 4) {
                var tableWidth = 605 + (groupLength - 4) * 120;
                self.Tables.tableSetting.attr("style", "width:" + tableWidth + "px;");
                self.Tables.tableInfo.attr("style", "width:" + tableWidth + "px;");
            }

            customerSettingHtml = self.RenderCustomerGameSetting(customerSetting);
            self.Tables.tableSettingBody.empty().append(customerSettingHtml);
        }

        if (self.CacheData.customerSetting.SelectResult.Code || customerSetting.RoleId !== self.Constants.RoleId.SUPER) {
            self.Tables.copyToMemberContainer.remove();
            self.Tables.defautGroupNewMemberContainer.remove();
            self.Tables.buttonUpdate.remove();
        } else {
            self.Tables.copyToMemberContainer.removeClass("hide");
            self.Tables.defautGroupNewMemberContainer.removeClass("hide");
        }

        self.CacheData.selectedDefaultGroup = self.CacheData.customerSetting.DefaultAGCasinoGroup;
        self.updateSelectedGroup();
    };
    self.checkCopyToMember = function () {
        var isChecked = self.Tables.copyToMember.hasClass("checkbox-checked");
        if (isChecked) {
            self.Tables.copyToMember.removeClass("checkbox-checked")
                .addClass("checkbox-unchecked");
            self.Tables.dropdownCopyToMember.addClass("hide");
        } else {
            self.Tables.copyToMember.removeClass("checkbox-unchecked")
                .addClass("checkbox-checked");
            self.Tables.dropdownCopyToMember.removeClass("hide");
        }
    };

    self.updateSelectedGroup = function () {
        self.RenderApplyGroup();
        self.RenderDefaultGroup();
        self.EnableUpdateButton();
    };
    self.RenderApplyGroup = function () {
        var options = '';
        var gameTypeLength = self.CacheData.customerSetting.ProductInfo ? self.CacheData.customerSetting.ProductInfo.length : 0;
        var isKeepSelected = false;
        for (var i = 0; i < gameTypeLength; i++) {
            var gameType = self.CacheData.customerSetting.ProductInfo[i];
            var groupLength = gameType.GameTypeGroup.length;
            for (var j = 0; j < groupLength; j++) {
                var group = gameType.GameTypeGroup[j];
                var checkbox = $("#checkbox-gametype-" + gameType.Id + "-group-" + group.Id);
                var isChecked = checkbox.hasClass("checkbox-checked");
                if (isChecked) {
                    if (!isKeepSelected) {
                        isKeepSelected = self.CacheData.selectedApplyGroup &&
                            self.CacheData.selectedApplyGroup.Name == group.Name;

                        if (!self.CacheData.selectedApplyGroup &&
                            !self.CacheData.firstSelectedApplyGroup) {
                            self.CacheData.firstSelectedApplyGroup = {
                                Id: group.Id,
                                Name: group.Name
                            };
                        }
                    }

                    options += '<option data-group-id="' + group.Id + '"' +
                                (self.CacheData.selectedApplyGroup &&
                                self.CacheData.selectedApplyGroup.Name == group.Name ? ' selected="selected"' : '') + '>' +
                                group.Name +
                            '</option>';
                }
            }
        }

        if (!isKeepSelected) {
            self.CacheData.selectedApplyGroup = null;
        }

        self.Tables.dropdownCopyToMember.empty().append(options);
        self.Tables.dropdownCopyToMember.change(function () {
            var selectedGroup = self.Tables.dropdownCopyToMember.find("option:selected");
            self.CacheData.selectedApplyGroup = {
                Id: selectedGroup.attr("data-group-id"),
                Name: self.Tables.dropdownCopyToMember.val()
            };
            self.EnableUpdateButton();
        });
    };
    self.RenderDefaultGroup = function () {
        var options = '';
        var gameTypeLength = self.CacheData.customerSetting.ProductInfo ? self.CacheData.customerSetting.ProductInfo.length : 0;
        var isKeepSelected = false;
        for (var i = 0; i < gameTypeLength; i++) {
            var gameType = self.CacheData.customerSetting.ProductInfo[i];
            var groupLength = gameType.GameTypeGroup.length;
            for (var j = 0; j < groupLength; j++) {
                var group = gameType.GameTypeGroup[j];
                var checkbox = $("#checkbox-gametype-" + gameType.Id + "-group-" + group.Id);
                var isChecked = checkbox.hasClass("checkbox-checked");
                if (isChecked) {
                    if (!isKeepSelected) {
                        isKeepSelected = self.CacheData.selectedDefaultGroup &&
                            self.CacheData.selectedDefaultGroup.Name == group.Name;
                    }

                    options += '<option data-group-id="' + group.Id + '"' +
                        (self.CacheData.selectedDefaultGroup &&
                         self.CacheData.selectedDefaultGroup.Name == group.Name ? ' selected="selected"' : '') + '>' +
                             group.Name +
                        '</option>';
                }
            }
        }

        if (!isKeepSelected) {
            self.CacheData.selectedDefaultGroup = null;
        }

        self.Tables.dropdownDefautGroupNewMember.empty().append(options);
        self.Tables.dropdownDefautGroupNewMember.change(function () {
            var selectedGroup = self.Tables.dropdownDefautGroupNewMember.find("option:selected");
            self.CacheData.selectedDefaultGroup = {
                Id: selectedGroup.attr("data-group-id"),
                Name: self.Tables.dropdownDefautGroupNewMember.val()
            };
            self.EnableUpdateButton();
        });
    };

    self.GetUpdateParams = function () {
        self.Params.updateCustomerInfo = $.extend(true, {}, self.CacheData.customerSetting);
        var selectedGroup = self.Tables.dropdownCopyToMember.find("option:selected");
        self.Params.updateCustomerInfo.ApplyGroupToMember = {
            Id: selectedGroup.attr("data-group-id"),
            Name: self.Tables.dropdownCopyToMember.val()
        };

        selectedGroup = self.Tables.dropdownDefautGroupNewMember.find("option:selected");
        self.Params.updateCustomerInfo.DefaultAGCasinoGroup = {
            Id: selectedGroup.attr("data-group-id"),
            Name: self.Tables.dropdownDefautGroupNewMember.val()
        };
        self.Params.updateCustomerInfo.hasChecked = false;

        var gameTypeLength = self.Params.updateCustomerInfo.ProductInfo.length;
        for (var i = 0; i < gameTypeLength; i++) {
            var gameType = self.Params.updateCustomerInfo.ProductInfo[i];
            var groupLength = gameType.GameTypeGroup.length;
            for (var j = 0; j < groupLength; j++) {
                var group = gameType.GameTypeGroup[j];
                var checkbox = $("#checkbox-gametype-" + gameType.Id + "-group-" + group.Id);
                var isChecked = checkbox.hasClass("checkbox-checked");
                self.Params.updateCustomerInfo.ProductInfo[i].GameTypeGroup[j].Enable = isChecked;
                if (isChecked) {
                    self.Params.updateCustomerInfo.hasChecked = true;
                }
            }
        }
    };
    self.CheckUpdateParams = function () {
        if (!self.Params.updateCustomerInfo.hasChecked) {
            showAlertMessage("result-message", "Please choose at least 1 group!", 1);
            return false;
        }

        self.Tables.resultMessage.empty();
        return true;
    };

    self.buttonUpdateClick = function () {
        if (!viewResources.HasHostPermission) {
            return;
        }

        self.blockUI();
        self.GetUpdateParams();
        if (!self.CheckUpdateParams()) {
            self.unblockUI();
            return;
        }

        self.CallAjax(self,
            window.rootUrl + "AGCasino/UpdateCustomerSetting",
            self.Params.updateCustomerInfo,
            self.RenderUpdateResult,
            self.RenderUpdateResult,
            self.unblockUI);
    };
    self.RenderUpdateResult = function (updateResult) {
        if (updateResult == null) {
            showAlertMessage("result-message", "There is a problem! Please try again!", 1);
            return;
        }

        if (updateResult.ErrorCode > 0) {
            showAlertMessage("result-message", updateResult.ErrorMessage, 1);
            return;
        }

        showAlertMessage("result-message", updateResult.ErrorMessage, 0);
        self.Tables.buttonUpdate.attr("disabled", "disabled");
        self.CacheData.customerSetting = self.Params.updateCustomerInfo;
        self.CacheData.selectedDefaultGroup = self.CacheData.customerSetting.DefaultAGCasinoGroup;
        self.CacheData.firstSelectedApplyGroup = self.Params.updateCustomerInfo.ApplyGroupToMember;
        setTimeout(function () {
            self.Tables.resultMessage.empty();
        }, 3000);
    };
};

agCasinoSetting = new AGCasinoSetting();
