$(document).ready(function () {
    updateMultiUsers.InitPage();
});

UpdateMultiUsers = function () {
    var self = this;
    self.Tables = {
        txtUserList: null,
        btnGetUserList: null,
        chbUseBaseSecurityCode: null,
        txtBaseSecurityCode: null,
        btnCheckSecurityCodes: null,
        btnUpdateUsersSecurityCode: null,
        errorMessage: null,
        updateSecurityCodeFilters: null,

        report: null,
        reportRowHeader: null,
        reportContent: null,
        chbUpdateAllUser: null,
        chbSelection: null,
        userSecurityCode: null,
        baseSecurityCodeValidation: null,
        bg_table: null,
        btnUploadFile: null,
        fileUpload: null,

        chbUseExcelFile: null,
        useTextboxFilters: null,
        useExcelFileFilters: null
    };

    self.Params = {
        GetUserList: {
            ListUserName: null
        },

        UpdateSecurityCodes: {
            UserList: null
        }
    };

    self.TempHtml = {
        frozenHtml: '',
        contentHtml: '',
        footerFrozenHtml: '',
        footerContentHtml: ''
    };

    self.Options = {
        report: {
            isShowPager: true,
            tblContentId: 'userListInfo_Content',
            manualCopyHeader: true,
            fullHeight: true,
            fullWidth: true,
            gridWidth: 620,
            gridHeight: 250,
            freezeWidth: 100,
            freezeHeight: 150,
            numberFreezeCol: 1
        }
    };

    self.CacheData = {
        userList: null,
        totalUser: 0
    };

    self.InitPage = function () {
        self.CachedDOMElements();
        self.BindingEvent();
        self.InitGridScrollerTables();
        self.Tables.fileUpload.attr("disabled", "disabled");
        self.Tables.useExcelFileFilters.addClass("hidden");
    };

    self.CachedDOMElements = function () {
        self.Tables.txtUserList = $("#txtUserList");
        self.Tables.btnGetUserList = $("#btnGetUserList");
        self.Tables.chbUseBaseSecurityCode = $("#chbUseBaseSecurityCode");
        self.Tables.txtBaseSecurityCode = $("#txtBaseSecurityCode");
        self.Tables.btnCheckSecurityCodes = $("#btnCheckSecurityCodes");
        self.Tables.btnUpdateUsersSecurityCode = $("#btnUpdateUsersSecurityCode");
        self.Tables.errorMessage = $("#errorMessage");

        self.Tables.updateSecurityCodeFilters = $("#updateSecurityCodeFilters");

        self.Tables.report = $("#userListInfo");
        self.Tables.reportRowHeader = self.Tables.report.find("#userListInfo_RowHeader tbody");
        self.Tables.reportContent = self.Tables.report.find("#userListInfo_Content tbody");
        self.Tables.chbUpdateAllUser = $(".chbUpdateAllUser");
        self.Tables.baseSecurityCodeValidation = $("#baseSecurityCodeValidation");
        self.Tables.bg_table = $("#bg_table");

        self.Tables.fileUpload = $("#fileUpload");
        self.Tables.btnUploadFile = $("#btnUploadFile");
        self.Tables.chbUseExcelFile = $("#chbUseExcelFile");
        self.Tables.useExcelFileFilters = $("#useExcelFileFilters");
        self.Tables.useTextboxFilters = $("#useTextboxFilters");
    };

    self.BindingEvent = function () {
        self.Tables.btnGetUserList.unbind("click");
        self.Tables.btnGetUserList.click(function () {
            self.GetUserList();
        });

        self.Tables.btnCheckSecurityCodes.unbind("click");
        self.Tables.btnCheckSecurityCodes.click(function () {
            self.CheckSecurityCodes();
        });

        self.Tables.btnUpdateUsersSecurityCode.unbind("click");
        self.Tables.btnUpdateUsersSecurityCode.click(function () {
            self.UpdateUsersSecurityCode();
        });

        self.Tables.chbUseBaseSecurityCode.unbind("click");
        self.Tables.chbUseBaseSecurityCode.click(function () {
            var isChecked = $(this).attr("checked") ? true : false;
            if (isChecked) {
                self.Tables.txtBaseSecurityCode.attr("disabled", null);
                self.Tables.userSecurityCode.attr("disabled", "disabled");
                //                self.Tables.chbSelection.attr("disabled", "disabled");
                //                self.Tables.chbUpdateAllUser.attr("disabled", "disabled");
            } else {
                self.Tables.txtBaseSecurityCode.attr("disabled", "disabled");
                self.Tables.userSecurityCode.attr("disabled", null);
                //                self.Tables.chbSelection.attr("disabled", null);
                //                self.Tables.chbUpdateAllUser.attr("disabled", null);
                self.Tables.txtBaseSecurityCode.val("");
                self.Tables.baseSecurityCodeValidation.addClass("hidden");
            }
        });

        self.Tables.chbUpdateAllUser.unbind("click");
        self.Tables.chbUpdateAllUser.click(function () {
            var isChecked = $(this).attr("checked") ? true : false;
            self.Tables.chbSelection.attr("checked", isChecked);
        });

        self.Tables.txtBaseSecurityCode.unbind("focusout");
        self.Tables.txtBaseSecurityCode.focusout(function () {
            var txtBaseSecurityCode = self.Tables.txtBaseSecurityCode.val();
            if (self.IsSecurityCode(txtBaseSecurityCode)) {
                self.Tables.baseSecurityCodeValidation.removeClass("invalidIcon")
                                                      .addClass("validIcon")
                                                      .removeClass("hidden");
                //                self.Tables.btnUpdateUsersSecurityCode.attr("disabled", null);
            } else {
                self.Tables.baseSecurityCodeValidation.removeClass("validIcon")
                                                      .addClass("invalidIcon")
                                                      .removeClass("hidden");
                //                self.Tables.btnUpdateUsersSecurityCode.attr("disabled", "disabled");
            }
        });

        self.Tables.btnUploadFile.click(function () {
            self.ValidateUsers();
        });

        self.Tables.fileUpload.unbind('change');
        self.Tables.fileUpload.change(function () {
            self.CheckFormatFile();
        });

        self.Tables.chbUseExcelFile.unbind("click");
        self.Tables.chbUseExcelFile.click(function () {
            var isChecked = $(this).attr("checked") ? true : false;
            if (isChecked) {
                self.Tables.txtUserList.attr("disabled", "disabled");
                self.Tables.btnGetUserList.attr("disabled", "disabled");
                self.Tables.fileUpload.attr("disabled", null);
                self.Tables.useTextboxFilters.addClass("hidden");
                self.Tables.useExcelFileFilters.removeClass("hidden");
            } else {
                self.Tables.txtUserList.attr("disabled", null);
                self.Tables.btnGetUserList.attr("disabled", null);
                self.Tables.fileUpload.attr("disabled", "disabled");
                self.Tables.useExcelFileFilters.addClass("hidden");
                self.Tables.useTextboxFilters.removeClass("hidden");
            }
        });
    };
    self.InitGridScrollerTables = function () {
        $("#userListInfo").initGrid({
            isShowPager: false
        });
    };
    self.GetUserList = function () {
        try { parent.BlockUI(); } catch (e) { }
        self.Tables.btnGetUserList.attr("disabled", "disabled");
        self.Tables.btnUploadFile.attr("disabled", "disabled");
        //        self.GetUserListParams();
        self.Tables.errorMessage.empty();
        self.Tables.errorMessage.addClass("hidden");
        self.Params.GetUserList.ListUserName = $.trim(self.Tables.txtUserList.val());
        $.ajax({
            type: "POST",
            url: window.rootUrl + "AdminSecurityCode/GetUserList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(self.Params.GetUserList),
            success: function (rs) {
                self.RenderUserList(rs);
            },
            error: function () {
                self.RenderUserList(null);
            },
            complete: function () {
                self.Tables.btnGetUserList.attr("disabled", null);
                self.Tables.btnUploadFile.attr("disabled", null);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    };
    self.GetUserListParams = function () {
    };
    self.RenderUserList = function (userList) {
        self.resetTempHtml();
        //        userList = [{ UserId: 1, UserName: "ABC1", SecurityCode: "176894" },
        //                    { UserId: 2, UserName: "ABC1", SecurityCode: "176894" },
        //                    { UserId: 3, UserName: "ABC1", SecurityCode: "176894" },
        //                    { UserId: 4, UserName: "ABC1", SecurityCode: "176894" },
        //                    { UserId: 5, UserName: "ABC1", SecurityCode: "176894"}];
        self.CacheData.userList = userList;
        if (userList && userList.length > 0) {
            var userLength = userList.length;
            self.CacheData.totalUser = userLength;
            var frozenHtml = '';
            var info;
            for (var i = 0; i < userLength; i++) {
                info = userList[i];
                var isValid = self.IsSecurityCode(info.SecurityCode);
                var txtSecurityCode = '<input id="userSecurityCode' + info.UserId + '" class="userSecurityCode txt c mainAccount" type="text" maxlength="6" value="' + info.SecurityCode + '" />';
                var chbSelection = '<input id="chbSelection' + info.UserId + '" class="chbSelection" type="checkbox" checked="checked" />';
                var divValidation = '<div id="divValidation' + info.UserId + '" class="divValidation ' + (isValid ? "validIcon" : "invalidIcon") + ' isValid="' + isValid + '" />';
                frozenHtml = '<td>\
                                  <div class="' + (info.SubAccounts && info.SubAccounts.length > 0 ? 'tree-expanded tree-expand' : '') + '" style="display: inline-block;" onclick="updateMultiUsers.AccountGroupToggle(this,\'' + info.UserId + '\');"></div>\
                                  <div class="col80" style="height: 19px; display: inline-block;">' + info.UserId + '</div>\
                              </td>';

                self.TempHtml.frozenHtml += '<tr class="mainAccount">' + frozenHtml + ' </tr>';
                self.TempHtml.contentHtml += '<tr class="mainAccount">' +
                                                    frozenHtml +
                                                    '<td><div class="col120 l" style="height: 19px;">' + info.UserName + '</div></td>' +
                                                    '<td><div class="col120 c">' + txtSecurityCode + '</div></td>' +
                                                    '<td><div class="col140 c">' + chbSelection + '</div></td>' +
                                                    '<td><div class="col120 c">' + divValidation + '</div></td>' +
                                              '</tr>';

                if (info.SubAccounts) {
                    var subAccountLength = info.SubAccounts.length;
                    self.CacheData.totalUser += subAccountLength;
                    for (var j = 0; j < subAccountLength; j++) {
                        var subAccount = info.SubAccounts[j];
                        var subIsValid = self.IsSecurityCode(subAccount.SecurityCode);
                        var subTxtSecurityCode = '<input id="userSecurityCode' + subAccount.UserId + '" class="userSecurityCode txt c userSecurityCode_SubAcc' + info.UserId + '" type="text" maxlength="6" value="' + subAccount.SecurityCode + '" subAccountId="' + subAccount.UserId + '" />';
                        var subChbSelection = '<input id="chbSelection' + subAccount.UserId + '" class="chbSelection" type="checkbox" checked="checked" />';
                        var subDivValidation = '<div id="divValidation' + subAccount.UserId + '" class="divValidation ' + (subIsValid ? "validIcon" : "invalidIcon") + ' divValidation_SubAcc' + info.UserId + '" isValid="' + subIsValid + '" />';

                        frozenHtml = '<td><div class="col80 colId" style="height: 19px;">' + subAccount.UserId + '</div></td>';
                        self.TempHtml.frozenHtml += '<tr class="subAccount subAccount_' + info.UserId + ' hidden">' + frozenHtml + ' </tr>';
                        self.TempHtml.contentHtml += '<tr class="subAccount subAccount_' + info.UserId + ' hidden">' +
                                                            frozenHtml +
                                                            '<td><div class="col120 l" style="height: 19px;">' + subAccount.UserName + '</div></td>' +
                                                            '<td><div class="col120 c">' + subTxtSecurityCode + '</div></td>' +
                                                            '<td><div class="col140 c">' + subChbSelection + '</div></td>' +
                                                            '<td><div class="col120 c">' + subDivValidation + '</div></td>' +
                                                      '</tr>';
                    }
                }
            }

            self.removeSpaceIE9TempHtml();
            self.Tables.reportRowHeader.empty().append(self.TempHtml.frozenHtml);
            self.Tables.reportContent.empty().append(self.TempHtml.contentHtml);
            self.Tables.updateSecurityCodeFilters.removeClass("hidden");

            self.Tables.errorMessage.addClass("hidden");
            self.Tables.errorMessage.empty();
            self.Tables.bg_table.removeClass("hidden");

            self.Tables.report.removeClass("hidden").gridScroller(self.Options.report);
            self.Tables.chbSelection = self.Tables.report.find(".chbSelection");
            self.Tables.userSecurityCode = self.Tables.report.find(".userSecurityCode");

            for (var i = 0; i < userLength; i++) {
                self.BindingEvent_SecurityCodeTextFocusOut(userList[i]);
            }

            self.Tables.chbSelection.unbind("change");
            self.Tables.chbSelection.change(function (e) {
                var isChecked = $(this).attr("checked") ? true : false;
                if (isChecked) {
                    if (self.Tables.report.find(".chbSelection").length == self.CacheData.totalUser) {
                        self.UpdateCheckAllCheckbox(true);
                    }
                } else {
                    self.UpdateCheckAllCheckbox(false);
                }

                e.preventDefault();
            });
        }
        else {
            self.CacheData.totalUser = 0;
            showAlertMessage("errorMessage", "There is no information.", 1);
            self.Tables.errorMessage.removeClass("hidden");
            self.Tables.bg_table.addClass("hidden");

            //            self.Tables.reportRowHeader.empty().append('<tr><td style="height: 19px;"></td></tr>');
            //            self.Tables.reportContent.empty().append('<tr><td style="height: 19px;"></td><td colspan="4" class="b">There is no information available.</td></tr>');
            //            self.Tables.chbUpdateAllUser.attr("disabled", "disabled");
            //            self.Tables.report.removeClass("hidden").gridScroller(self.Options.report);
            //            self.Tables.bg_table.removeClass("hidden");

            self.Tables.updateSecurityCodeFilters.addClass("hidden");
        }
    };
    self.BindingEvent_SecurityCodeTextFocusOut = function (userInfo) {
        $("#userSecurityCode" + userInfo.UserId).unbind("focusout");
        $("#userSecurityCode" + userInfo.UserId).focusout(function () {
            var securityCode = $("#userSecurityCode" + userInfo.UserId).val();
            var isValid = self.IsSecurityCode(securityCode);
            var divValidation = $("#divValidation" + userInfo.UserId);
            divValidation.attr("isValid", isValid);
            if (isValid) {
                divValidation.removeClass("invalidIcon").addClass("validIcon")
                                                        .removeClass("hidden");
                if ($(this).hasClass("mainAccount")) {
                    $(".userSecurityCode_SubAcc" + userInfo.UserId).val(securityCode);
                    $(".divValidation_SubAcc" + userInfo.UserId).attr("isValid", true);
                    $(".divValidation_SubAcc" + userInfo.UserId).removeClass("invalidIcon").addClass("validIcon");
                }
            } else {
                divValidation.removeClass("validIcon").addClass("invalidIcon")
                                                      .removeClass("hidden");
                $("#chbSelection" + userInfo.UserId).attr("checked", false);
                self.UpdateCheckAllCheckbox(false);
            }
        });

        if (userInfo.SubAccounts) {
            var subAccountLength = userInfo.SubAccounts.length;
            for (var i = 0; i < subAccountLength; i++) {
                var subAccount = userInfo.SubAccounts[i];
                $("#userSecurityCode" + subAccount.UserId).unbind("focusout");
                $("#userSecurityCode" + subAccount.UserId).focusout(function () {
                    var userId = $(this).attr("subAccountId");
                    var securityCode = $("#userSecurityCode" + userId).val();
                    var isValid = self.IsSecurityCode(securityCode);
                    var divValidation = $("#divValidation" + userId);
                    divValidation.attr("isValid", isValid);
                    if (isValid) {
                        divValidation.removeClass("invalidIcon").addClass("validIcon")
                                                            .removeClass("hidden");
                    } else {
                        divValidation.removeClass("validIcon").addClass("invalidIcon")
                                                          .removeClass("hidden");
                        $("#chbSelection" + userId).attr("checked", false);
                    }
                });
            }
        }
    };
    self.resetTempHtml = function () {
        self.TempHtml.frozenHtml = '';
        self.TempHtml.contentHtml = '';
        self.TempHtml.footerFrozenHtml = '';
        self.TempHtml.footerContentHtml = '';
    };
    self.removeSpaceIE9TempHtml = function () {
        self.TempHtml.frozenHtml = stringformat.removeSpaceIE9(self.TempHtml.frozenHtml);
        self.TempHtml.contentHtml = stringformat.removeSpaceIE9(self.TempHtml.contentHtml);
        self.TempHtml.footerFrozenHtml = stringformat.removeSpaceIE9(self.TempHtml.footerFrozenHtml);
        self.TempHtml.footerContentHtml = stringformat.removeSpaceIE9(self.TempHtml.footerContentHtml);
    };
    self.CheckSecurityCodes = function () {
        if (self.CacheData.userList == null) {
            return;
        }

        var userLength = self.CacheData.userList.length;
        var info;
        var isAllValid = true;
        for (var i = 0; i < userLength; i++) {
            info = self.CacheData.userList[i];
            var isSecurityCode = self.IsSecurityCode($("#userSecurityCode" + info.UserId).val());
            $("#divValidation" + info.UserId).attr("isValid", isSecurityCode);
            if (isSecurityCode == false) {
                $("#divValidation" + info.UserId).removeClass("validIcon").addClass("invalidIcon");
                isAllValid = false;
            } else {
                $("#divValidation" + info.UserId).removeClass("invalidIcon").addClass("validIcon");
            }
        }

        if (isAllValid) {
            showAlertMessage("errorMessage", "All security codes are valid.", 0);
            self.Tables.errorMessage.removeClass("hidden");
        } else {
            self.Tables.errorMessage.empty();
            self.Tables.errorMessage.addClass("hidden");
        }
    };
    self.IsSecurityCode = function (secCode) {
        var secCodeLength = secCode.length;
        if (secCodeLength != 6) { // check length
            return false;
        }

        var regex = /^([0-9])(?!\1+$)[0-9]+$/; // check digits and all of them are not the same
        var result = secCode.search(regex) != -1 ? true : false;
        if (result) { // check 'Forward Run' and 'Backward Run'
            var isForwardRun = true;
            var isBackwardRun = true;
            var forward = 0;
            var backward = 0;
            var forward = parseInt(secCode.charAt(0));
            var backward = parseInt(secCode.charAt(0));
            for (var i = 1; i < secCodeLength; i++) {
                var current = parseInt(secCode.charAt(i));
                if (++forward != current) {
                    isForwardRun = false;
                }

                if (--backward != current) {
                    isBackwardRun = false;
                }
            }

            result = !(isForwardRun || isBackwardRun);
        }

        return result;
    };
    self.UpdateUsersSecurityCode = function () {
        if (self.CacheData.userList == null) {
            return;
        }

        try { parent.BlockUI(); } catch (e) { }
        var isUseBaseSecurityCode = self.Tables.chbUseBaseSecurityCode.attr("checked") ? true : false;
        if (!isUseBaseSecurityCode) {
            self.CheckSecurityCodes();
        }

        var isUseBaseSecurityCode = self.Tables.chbUseBaseSecurityCode.attr("checked") ? true : false;
        var txtBaseSecurityCode = self.Tables.txtBaseSecurityCode.val();
        var userLength = self.CacheData.userList.length;
        var userList = [];
        for (var i = 0; i < userLength; i++) {
            var info = self.CacheData.userList[i];
            var securityCode = isUseBaseSecurityCode ?
                txtBaseSecurityCode :
                $("#userSecurityCode" + info.UserId).val();
            var isValid = self.IsSecurityCode(securityCode);
            var isSelected = $("#chbSelection" + info.UserId).attr("checked") ? true : false;

            //            var subAccounts = [];
            if (info.SubAccounts) {
                var subAccountLength = info.SubAccounts.length;
                for (var j = 0; j < subAccountLength; j++) {
                    var subAccount = info.SubAccounts[j];
                    var subSecurityCode = isUseBaseSecurityCode ?
                    txtBaseSecurityCode :
                    $("#userSecurityCode" + subAccount.UserId).val();
                    var subIsValid = self.IsSecurityCode(subSecurityCode);
                    var subIsSelected = $("#chbSelection" + subAccount.UserId).attr("checked") ? true : false;

                    userList.push({
                        UserId: subAccount.UserId,
                        UserName: subAccount.UserName,
                        SecurityCode: subSecurityCode,
                        RoleId: subAccount.RoleId,
                        IsValid: subIsValid,
                        IsSelected: subIsSelected,
                        IsSubAccount: true
                    });
                }
            }

            userList.push({
                UserId: info.UserId,
                UserName: info.UserName,
                SecurityCode: securityCode,
                RoleId: info.RoleId,
                IsValid: isValid,
                IsSelected: isSelected,
                IsSubAccount: false
            });
        }

        if (userList.length == 0) {
            showAlertMessage("errorMessage", "No user with valid security code's found.", 1);
            self.Tables.errorMessage.removeClass("hidden");
            try { parent.jQuery.unblockUI(); } catch (e) { }
            return;
        }

        self.Tables.errorMessage.empty();
        self.Tables.errorMessage.addClass("hidden");
        self.Params.UpdateSecurityCodes.UserList = userList;
        $.ajax({
            traditional: true,
            type: "POST",
            url: window.rootUrl + "AdminSecurityCode/UpdateUsersSecurityCode",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(self.Params.UpdateSecurityCodes.UserList),
            success: function (rs) {
                if (rs && rs.ErrorCode > 0) {
                    showAlertMessage("errorMessage", rs.ErrorMessage.replace(/\n/g, "<br />"), 1);
                } else {
                    showAlertMessage("errorMessage", "Update security codes successfully!", 0);
                }

                self.Tables.errorMessage.removeClass("hidden");
            },
            error: function () {
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    };

    self.CheckFormatFile = function () {
        var filename = self.Tables.fileUpload.val();
        self.Tables.errorMessage.empty().addClass("hidden");
        var isValidFile = false;

        if (filename != "") {
            var ext = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
            isValidFile = (ext == "xls" || ext == "xlsx");
            if (!isValidFile) {
                self.Tables.btnUploadFile.attr('disabled', true);
            }
            else {
                self.Tables.btnUploadFile.attr('disabled', false);
                isValidFile = true;
            }
        }
        return isValidFile;
    };
    self.ValidateUsers = function () {
        if (self.CheckFormatFile()) {
            var formdata = new FormData(); //FormData object
            var fileInput = document.getElementById('fileUpload');
            for (i = 0; i < fileInput.files.length; i++) {
                formdata.append(fileInput.files[i].name, fileInput.files[i]);
            }

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'ValidateUsers');
            xhr.send(formdata);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    self.RenderUserList(jQuery.parseJSON(xhr.responseText));
                }
                else {
                    self.RenderUserList(null);
                }
            }
        }
    };
    self.AccountGroupToggle = function (btn, mainAccountId) {
        var $this = $(btn);
        if ($this.hasClass("tree-expand")) {
            $this.removeClass("tree-expand");
            $(".subAccount_" + mainAccountId).removeClass("hidden");
        } else {
            $(".subAccount_" + mainAccountId).addClass("hidden");
            $this.addClass("tree-expand");
        }

        self.Tables.report.gridScroller(self.Options.report);
    };

    self.UpdateCheckAllCheckbox = function (isCheckAll) {
        if (isCheckAll) {
            self.Tables.chbUpdateAllUser.attr("checked", true);
        }
        else {
            self.Tables.chbUpdateAllUser.attr("checked", false);
        }
    };
};

var updateMultiUsers = new UpdateMultiUsers();