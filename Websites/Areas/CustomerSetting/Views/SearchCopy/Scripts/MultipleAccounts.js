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
        $fax = $("#txtFax"),
        $internalUser = $("#chkInternalUser"),
        $tblProgress = $("#tblProgress"),
        $btnSubmit = $("#btnSubmit");
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
            showError("You are not allowed to input [Enter] and backslash '\\' into the Usernames field.");
            return;
        }

        var usernamesArray = usernamesValue.split(",");
        if (usernamesArray.length > vm.maxInputUsernames) {
            showError("The function can only create up to " + vm.maxInputUsernames + " accounts per request.");
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

        // Check duplicate.
        for (var i = 0, max = usernamesArray.length; i < max - 1; i++) {
            if (usernamesArray[i] === usernamesArray[i + 1]) {
                showError("The username '" + usernamesArray[i] + "' is duplicated!");
                return;
            }
        }

        var unavailableUsernames = $.ajax({
            type: "POST",
            url: vm.rootUrl + "Super/CheckAvailableUsernames",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ custId: vm.existingCustId, parentId: vm.uplineId, usernames: usernamesArray.join(",") }),
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

        var isSuper = vm.maxGivenCredit.length === 0;

        var creditValue = $credit.val();
        if (creditValue.length === 0) {
            showError("The " + (isSuper ? "Max" : "Given") + " Credit is required.");
            return;
        }

        creditValue = creditValue.replace(/,/g, "");
        if (!$.isNumeric(creditValue)) {
            showError("The " + (isSuper ? "Max" : "Given") + " Credit is invalid.");
            return;
        }

        var credit = parseInt(creditValue);
        if (!isSuper && usernameCount * credit > parseInt(vm.maxGivenCredit)) {
            showError("Insufficient Given Credit.");
            return;
        }

        return {
            userpwd: passwordValue.ec(vm.code).hc(),
            firstName: $.trim($firstName.val()),
            lastName: $.trim($lastName.val()),
            phone: $.trim($phone.val()),
            mobilePhone: $.trim($mobilePhone.val()),
            site: $site.length ? $site.val() : "",
            fax: $.trim($fax.val()),
            credit: credit,
            isInternal: $internalUser.is(":checked"),
            currencyId: vm.currencyId,
            existingCustId: vm.existingCustId,
            isSyncCasino: vm.isSyncCasino,
            disableCasino: vm.disableCasino,
            custLevel: vm.custLevel,
            uplineId: vm.uplineId,
            isSyncNG2: vm.isSyncNG2,
            disableNG2: vm.disableNG2,
            code: vm.code
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
        $fax.prop("disabled", value);
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
        //$site.val("");
        $firstName.val("");
        $lastName.val("");
        $phone.val("");
        $mobilePhone.val("");
        $fax.val("");
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
            url: vm.rootUrl + "Super/CopyExistingSuper",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(formData)
        }).done(function (data) {
            divStatus.removeClass();
            if (data.Code === 0) {
                divStatus.addClass("statusCreated").html("Created");
            } else {
                divStatus.addClass("statusError").html("Error: " + data.Message.replace(/<\/?[^>]+(>|$)/g, ""));
            }
        }).always(function () {
            copyMultipleAccounts(usernames, formData);
        });
    }

    function initialize() {
        $credit.forceNumber();
        $btnSubmit.click(function () {
            BlockUI();
            emptyAndHideProgressTable();
            var usernames = validateUsernames();
            if (usernames) {
                var formData = validateOthers(usernames.length);
                if (formData) {
                    hideMessage();
                    disableFormData(true);
                    showDataOnProgressTable(usernames);
                    copyMultipleAccounts(usernames, formData);
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

    initialize();
})(jQuery, viewModel, window);