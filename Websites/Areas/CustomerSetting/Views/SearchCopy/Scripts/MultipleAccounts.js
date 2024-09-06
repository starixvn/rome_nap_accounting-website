/* forceNumber()
 * ==========
 * Allow input Number only.
 *
 * @usage: $(selector).forceNumber();
 *
 */
$.fn.forceNumber = function () {
    var number_0 = 48,
        number_9 = 57,
        numpad_0 = 96,
        numpad_9 = 105,
        end = 35,
        home = 36,
        leftArrow = 37,
        rightArrow = 39,
        backspace = 8,
        tab = 9,
        enter = 13,
        escape = 27,
        del = 46,
        a = 65,
        c = 67,
        v = 86,
        x = 88;

    function isNumeric(text) {
        var regExp = /^-?(\d+|\d+\.\d*|\d*\.\d+)$/;
        return text.match(regExp);
    }

    function keydownHandler(e) {
        var code = e.which || e.keyCode;
        if (number_0 <= code && code <= number_9 ||
            numpad_0 <= code && code <= numpad_9 ||
            code === end ||
            code === home ||
            code === leftArrow ||
            code === rightArrow ||
            code === backspace ||
            code === tab ||
            code === enter ||
            code === escape ||
            code === del) {
            return true;
        }
        var ctrlKey = e.ctrlKey;
        var metaKey = e.metaKey;
        return (code === a || code === c || code === v || code === x) && (ctrlKey || metaKey);
    }

    function keyupHandler(e) {
        var text = this.value;
        if (text.length === 0) {
            return;
        }
        text = text.replace(/\,/g, "");
        if (isNumeric(text)) {
            var number = parseInt(text).toLocaleString("en-US");
            this.value = number;
        }
    }

    this.keydown(keydownHandler)
        .keyup(keyupHandler);
};

(function ($, vm, window) {
    "use strict";
    var $usernames = $("#txtUsernames"),
        $password = $("#txtPassword"),
        $credit = $("#txtCredit"),
        $site = $("#ddSite"),
        $firstName = $("#txtFirstName"),
        $lastName = $("#txtLastName"),
        $phone = $("#txtPhone"),
        $mobilePhone = $("#txtMobilePhone"),
        $internalUser = $("#chkInternalUser"),
        $tblProgress = $("#tblProgress"),
        $btnSubmit = $("#btnSubmit"),
        $siteRow = $("#siteRow"),
        $superCreditRow = $("#superCreditRow"),
        $memberCreditRow = $("#memberCreditRow"),
        $passwordRow = $("#passwordRow");

    var userid = -1;
    var parentId = 0;
    var MEMBER = 1;
    var SUPER = 4;
    var MAX_ACCOUNTS_PER_REQUEST = 50;

    var existedUser = {
        userid: -1,
        username: '',
        selectedSite: {},
        sites: [],
        site: "",
        maxCredit: '',
        roleId: 0,
    };

    var isProcessing = false;

    function showError(message) {
        showMessage('<div class="alertboxerr"><div>' + message + "</div></div>");
    }

    function showSuccess(message) {
        showMessage('<div class="alertboxscc"><div>' + message + "</div></div>");
    }

    function showMessage(html) {
        var messageDiv = $("#sectionMessage");
        messageDiv.html(html).removeClass("hide");
    }

    function hideMessage() {
        var messageDiv = $("#sectionMessage");
        messageDiv.addClass("hide").html("");
    }

    function validateUsernames() {
        var usernamesValue = $.trim($usernames.val());
        if (usernamesValue.length === 0) {
            showError("The Usernames is required.");
            return;
        }

        // Check usernames contains '\'.
        if (usernamesValue.indexOf("\n") >= 0
            || usernamesValue.indexOf("\r") >= 0
            || usernamesValue.indexOf("\\") >= 0) {
            showError("You are not allowed to input [Enter], [Space] and backslash '\\' into the Usernames field.");
            return;
        }

        var usernamesArray = usernamesValue.split(",");
        if (usernamesArray.length > MAX_ACCOUNTS_PER_REQUEST) {
            showError("The function can only create up to " + MAX_ACCOUNTS_PER_REQUEST + " accounts per request.");
            return;
        }

        // Remove empty username and do trim(username).
        usernamesArray = usernamesArray
            .filter(function (item) {
                return $.trim(item).length > 0;
            })
            .map(function (item) {
                return $.trim(item);
            })
            .sort();

        for (var i = 0, max = usernamesArray.length; i <= max - 1; i++) {
            if (usernamesArray[i].indexOf(" ") >= 0) {
                showError("You are not allowed to input [Enter], [Space] and backslash '\\' into the Usernames field.");
                return;
            }
        }

        // Check duplicate.
        for (var i = 0, max = usernamesArray.length; i < max - 1; i++) {
            if (usernamesArray[i] === usernamesArray[i + 1]) {
                showError("The username '" + usernamesArray[i] + "' is duplicated!");
                return;
            }
        }

        var unavailableUsernames = $.ajax({
            type: "POST",
            url: toUrl("Super/CheckAvailableUsernames"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ custId: existedUser.userid, parentId: parentId, usernames: usernamesArray.join(",") }),
            async: false
        }).responseText.replace(/\"/g, "");

        if (unavailableUsernames.length) {
            showError(unavailableUsernames);
            return;
        }

        return usernamesArray;
    }

    function validateOthers(usernameCount) {
        var passwordValue = $password.val();
        if (passwordValue.length === 0) {
            showError("The Password is required.");
            return;
        }

        var isSuper = existedUser.roleId === SUPER;

        var creditValue = $("#txtCredit").val();
        if (creditValue.length === 0) {
            showError("The " + (isSuper ? "Max" : "Given") + " Credit is required.");
            return;
        }

        creditValue = creditValue.replace(/,/g, "");
        if (!$.isNumeric(creditValue) || parseInt(creditValue) < 0) {
            showError("The " + (isSuper ? "Max" : "Given") + " Credit is invalid.");
            return;
        }

        var credit = parseInt(creditValue);
        if (!isSuper && usernameCount * credit > parseInt(existedUser.maxCredit)) {
            showError("Insufficient Given Credit.");
            return;
        }

        return {
            password: passwordValue.encrypt(vm.code),
            firstName: $.trim($firstName.val()),
            lastName: $.trim($lastName.val()),
            phone: $.trim($phone.val()),
            mobilePhone: $.trim($mobilePhone.val()),
            site: isSuper ? $("#ddSite").val() : existedUser.site,
            fax: $.trim($("#txtFax").val()),
            credit: credit,
            isInternal: $("#chkInternalUser").is(":checked"),
            sourceCustId: userid,
            custLevel: existedUser.roleId,
            uplineId: parentId,
            code: vm.code,
            currency: $('#currency').data().currencyId
        };
    }

    function disableFormData(value) {
        $usernames.prop("disabled", value);
        $password.prop("disabled", value);
        $credit.prop("disabled", value);
        $site.prop("disabled", value);
        $firstName.prop("disabled", value);
        $lastName.prop("disabled", value);
        $phone.prop("disabled", value);
        $mobilePhone.prop("disabled", value);
        $("#txtFax").prop("disabled", value);
        $internalUser.prop("disabled", value);
        $btnSubmit.prop("disabled", value);
        if (value) {
            $btnSubmit.removeClass("btn");
        } else {
            $btnSubmit.addClass("btn");
        }
    }

    function resetAndEnableFormData() {
        $usernames.val("");
        $password.val("");
        $credit.val("");
        $firstName.val("");
        $lastName.val("");
        $phone.val("");
        $mobilePhone.val("");
        $("#txtFax").val("");
        $internalUser.prop("checked", false);
        disableFormData(false);
    }

    function showDataOnProgressTable(usernames) {
        var html = [];
        for (var i = 0, max = usernames.length; i < max; i++) {
            html.push('<tr><td class="r">');
            html.push(i + 1);
            html.push("</td>");
            html.push("<td>");
            html.push(usernames[i]);
            html.push("</td>");
            html.push('<td><div id="divStatus');
            html.push(usernames[i]);
            html.push('">Not created</div></td>');
        }

        if (html.length > 0) {
            $tblProgress.find("tbody").html(html.join(""));
            $tblProgress.removeClass("hide");
        }
    }

    function emptyAndHideProgressTable() {
        $tblProgress
            .addClass("hide")
            .find("tbody").html("");
    }

    function copyMultipleAccounts(usernames, formData) {
        if (usernames.length === 0) {
            isProcessing = false;
            resetAndEnableFormData();
            return;
        }

        isProcessing = true;
        formData.userName = usernames.shift();
        var divStatus = $("#divStatus" + formData.userName)
            .addClass("statusInprogress")
            .html("In progress");
        $.ajax({
            type: "POST",
            url: "../Customer/Copy",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(formData)
        }).fail(function (jqXHR) {
            if (jqXHR.status === 403 || jqXHR.status === 401) {
                divStatus.removeClass().addClass("statusError").html("Error: You don't have permission on this function.");
            }
        }).done(function (data) {
            divStatus.removeClass();
            if (data.Code === 0) {
                divStatus.addClass("statusCreated").html("Created");
            } else {
                var message = "";
                if (data.MessageContent) {
                    var messageContent = data.MessageContent;
                    message = messageContent.MainMessage
                    if (messageContent.SubMessages && messageContent.SubMessages.length) {
                        var subMessages = messageContent.SubMessages.map(s => s.Message);
                        message += ` (${subMessages.join(", ")})`;
                    }
                } else {
                    message = data.message;
                }

                divStatus.addClass("statusError").html(message.replace(/<\/?[^>]+(>|$)/g, ""));
            }
        }).always(function () {
            copyMultipleAccounts(usernames, formData);
        });
    }

    function renderCreateSuperView(disableCredit = false) {
        $passwordRow.append(`
            <td class="col-internal">
                <div class="col-internal-user">
                    <input type="checkbox" id="chkInternalUser"/>
                    <label for= "chkInternalUser"> Internal User</label>
                </div>
            </td>`);
        $siteRow.show();
        $siteRow.append(`
            <td>
                <label for="ddSite">Site</label>
                <select id="ddSite" title="Site"></select>
            </td>
            <td>
                <label for="txtFax">Fax</label>
                <input type="text" id="txtFax" maxlength="50" />
            </td>`);
        $superCreditRow.show();
        $superCreditRow.append(`
            <td>
                <label for="txtCredit">Max Credit<span class="require"> *</span></label>
                <input type="text" id="txtCredit" maxlength="18"  ${disableCredit ? 'disabled' : ''} />
            </td>
            <td>
                <label>Currency</label>
                <label class="currency" id="currency"></label>
            </td>`);
    }

    function renderCreateMemberView(disableCredit = false) {
        $passwordRow.append('<td colspan="2"></td>');
        $memberCreditRow.show();
        $memberCreditRow.append(`
            <td>
                <div class="col-credit">
                    <label for="txtCredit">Given Credit<span class="require"> *</span></label>
                    <input type="text" id="txtCredit" maxlength="18" ${disableCredit ? 'disabled' : ''}/>
                    <div class="memberMaxCredit" id="memberMaxCredit"></div>
                <div>
            </td>
            <td>
                <label for="txtFax">Fax</label>
                <input type="text" id="txtFax" maxlength="50" />
            </td>`);
    }

    function renderViewByLevel() {
        var noPermissionEditCredit = $('#noPermissionEditCredit').val();
        if (parent.level == "super") {
            renderCreateSuperView(noPermissionEditCredit);
        } else {
            renderCreateMemberView(noPermissionEditCredit);
        }
        $("#txtCredit").val(0);
        $("#txtCredit").forceNumber();
    }

    function registerSearchEvent() {
        $("#existedUserName").focus(function () {
            hideMessage();
            $("#checkExisted").hide();
            userid = -1;
        });

        $("#existedUserName").focusout(function () {
            if (userid === -1) {
                searchExistingUser(this.value, null);
            }
            $("#checkExisted").show();
        });
    }

    function validateSourceAccount() {
        var sourceAccount = $("#existedUserName").val();
        if (sourceAccount.length === 0) {
            showError("The Source Account is required.");
            return false;
        }

        if (userid === -1) {
            return false;
        }

        return true;
    }

    function handleSubmit() {
        $btnSubmit.click(function () {
            BlockUI();
            emptyAndHideProgressTable();
            if (validateSourceAccount()) {
                var usernames = validateUsernames();
                if (usernames) {
                    var formData = validateOthers(usernames.length);
                    if (formData && confirm("Please REVIEW the Source " + (existedUser.roleId == SUPER ? "Super " : "Member ") + existedUser.username + "(" + existedUser.userid + ") once again.")) {
                        hideMessage();
                        disableFormData(true);
                        showDataOnProgressTable(usernames);
                        copyMultipleAccounts(usernames, formData);
                    }
                }
            }
            $.unblockUI();
        });
        $(window).on("beforeunload", function (e) {
            if (isProcessing) {
                var confirmationMessage =
                    "There are one or many accounts which have not been created yet."
                    + " Closing this browser tab will cancel their creation process."
                    + "\nAre you sure you want to close the tab?";
                e.returnValue = confirmationMessage;
                return confirmationMessage;
            }
        });
    }

    function RenderSite(sites) {
        var htmlOptions = [];
        sites.forEach(function (item, index) {
            if (item.IsParent) {
                htmlOptions.push('<optgroup label="' + item.SiteName + '">');
                item.ChildSites.forEach(function (child, i) {
                    htmlOptions.push('<option value="');
                    htmlOptions.push(child.SiteName + '"');
                    if (existedUser.site == child.SiteName) {
                        htmlOptions.push('selected="selected"');
                    }
                    htmlOptions.push('>');
                    htmlOptions.push(child.SiteName);
                    htmlOptions.push('</option>');
                });
                htmlOptions.push('</optgroup>');
            } else {
                htmlOptions.push('<option value="');
                htmlOptions.push(item.SiteName + '"');
                if (existedUser.site == item.SiteName) {
                    htmlOptions.push('selected="selected"');
                }
                htmlOptions.push('>');
                htmlOptions.push(item.SiteName);
                htmlOptions.push('</option>');
            }
        });
        $("#ddSite").html(htmlOptions.join(""));
    }

    function getNotAllowedNewSuperPrefixes() {
        var isSuper = parent.level === "super";
        if (!isSuper) { return; }

        $.ajax({
            type: "GET",
            url: vm.getSuperPrefixesUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var $notAllowedSuperPrefixesMultiple = $("#notAllowedSuperPrefixes_multiple");
                if (data && data.length > 0) {
                    var warning = "For 789y site, the following username prefixes are not allowed for new accounts anymore";
                    $.each(data, function (index, item) {
                        warning += `<br />- ${item.groupName}: ${item.prefixes}`;
                    });
                    $notAllowedSuperPrefixesMultiple.addClass("headlineBelongSiteMsg");
                    $notAllowedSuperPrefixesMultiple.html(warning);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) { }
        });
    }

    function getHeadlineBelongSite() {
        var isSuper = parent.level === "super";
        if (!isSuper) { return; }

        $.ajax({
            type: "GET",
            url: vm.getHeadlineBelongSiteUrl,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var $messageHtml = $("#headlineBelongSite");
                if (data && data.length > 0) {
                    var message = "For 789y site group, the following headlines belong to site:";
                    $.each(data, function (index, item) {
                        message += `<br />- ${item.site}: ${item.headline}`;
                    });
                    $messageHtml.addClass("headlineBelongSiteMsg");
                    $messageHtml.html(message);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) { }
        });
    };

    function getAllSites() {
        $.ajax({
            type: "POST",
            url: toUrl("Super/GetSites"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (data) {
                hideMessage();
                existedUser.sites = data;
                RenderSite(data);
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                showError("Error for getting sites of existing user !");
            }
        });
    }

    function renderCurrency(currency, currencyid) {
        $("#currency").text(currency).data('currency-id', currencyid);
    }

    function formatCurrency(value) {
        return (Math.trunc(value * 100) / 100).toLocaleString("en-US");
    }

    function renderMemberMaxCredit(custInfo) {
        var maxCredit = custInfo.MaxCredit - custInfo.Credit;
        var memberMaxCredit = custInfo.MemberMaxCredit;
        var max = memberMaxCredit ? Math.min(memberMaxCredit, maxCredit) : maxCredit;
        existedUser.maxCredit = max;
        var hint = `<span>Min: 0 - Max: ${formatCurrency(max)} </span>`;
        if (memberMaxCredit) {
            hint += ` <span class="icon-credit" data-title="Available Credit: ${formatCurrency(maxCredit)}\nThe Maximum Member Credit set by your upline: ${formatCurrency(memberMaxCredit)} "></span>`;
        }
        $("#memberMaxCredit").html(hint);
    }

    function getCustomerInfoByUserId(custid) {
        var param = {
            custid: custid
        };

        $.ajax({
            type: "POST",
            url: "../Customer/GetCustomerInfoByUserId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (custInfo) {
                hideMessage();

                if (existedUser.selectedSite) {
                    existedUser.selectedSite = custInfo.Site || "";
                }
                else {
                    existedUser.selectedSite = {};
                    existedUser.selectedSite = custInfo.Site || "";
                }

                existedUser.userid = userid;
                existedUser.roleId = custInfo.RoleId;
                existedUser.username = custInfo.UserName;
                existedUser.maxCredit = custInfo.MaxCredit;
                existedUser.site = custInfo.Site;
                existedUser.isDirectCustomer = custInfo.IsDirectCustomer;

                renderCurrency(custInfo.CurrencyName, custInfo.CurrencyId);

                if (custInfo.RoleId == MEMBER) {
                    renderMemberMaxCredit(custInfo);
                }

                getAllSites();
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                showError("Error for getting site by user id !");
            }
        });
    }

    function searchExistingUser(username, callback) {
        if (username == undefined || username == "") {
            $("#checkExisted").removeClass('success');
            $("#checkExisted").removeClass('not_success');
            return;
        }

        userid = -1;

        var param = {
            username: username,
            level: parent.level
        };

        try { parent.BlockUI(); } catch (e) { }
        try { parent.parent.BlockUI(); } catch (e) { }
        try { parent.parent.parent.BlockUI(); } catch (e) { }
        $("body").addClass("wait");
        $.ajax({
            type: "POST",
            url: toUrl("Super/SearchExistingSuper"),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (customerSettingView) {
                if (customerSettingView.ErrorCode > 0) {
                    $("#checkExisted").removeClass('success');
                    $("#checkExisted").addClass('not_success');
                    $("#breadCrumbs").text("");
                    showError(customerSettingView.ErrorMessage)
                }
                else {
                    var custid = customerSettingView.UserId;
                    userid = custid;
                    parentId = customerSettingView.ParentId;
                    if (custid != "-1") {
                        hideMessage();
                        $("#checkExisted").removeClass('not_success');
                        $("#checkExisted").addClass('success');
                        if (customerSettingView.BreadCrumbs) {
                            $("#breadCrumbs").text(customerSettingView.BreadCrumbs);
                        }

                        getCustomerInfoByUserId(custid);

                        if (callback) {
                            callback();
                        }
                    }
                    else {
                        $("#checkExisted").removeClass('success');
                        $("#checkExisted").addClass('not_success');
                        $("#breadCrumbs").text("");
                        showError("The " + parent.level + " doesn’t exist !", 1);
                    }
                }

                try { parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.parent.parent.jQuery.unblockUI(); } catch (e) { }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorMessage = xhr.status === 403
                    ? $.parseJSON(xhr.responseText).ErrorMessage
                    : "Error for searching existing user !";
                showError(errorMessage)
                try { parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.parent.parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    };

    function renderTableNote() {
        getNotAllowedNewSuperPrefixes();
        getHeadlineBelongSite();
        var isSuper = parent.level === "super";
        if (isSuper) {
            $("#tableNote").show();
        }
    }

    function initialize() {
        renderTableNote();
        renderViewByLevel();
        registerSearchEvent();
        handleSubmit();
    }

    $(document).ready(function () {
        initialize();
    });
})(jQuery, viewModel, window);