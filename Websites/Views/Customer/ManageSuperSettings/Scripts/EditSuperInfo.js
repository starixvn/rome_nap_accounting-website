var editSuperInfo = {
    superList: '',
    updateData: null,
    showGivenCredit: true,
    showUpdatePassword: true,
    showUpdateSecurityCode: true,
    enableDisableInput: function (flag) {
        $("#tab-esi-superlist").prop('disabled', flag);
        $("#tab-esi-btnCheckSupers").prop('disabled', flag);
    },
    resetSelect: function () {
        editSuperInfo.enableDisableInput(false);

        $("#tab-esi-superlist").val('');
        $("#tab-esi-superSetting").addClass("hidden");
        $('#tab-esi-errMsg').html('');
        $('#tab-esi-errSupers').html('');
    },
    confirmBox: function (html, successCallback) {
        $("#tab-esi-dialog-confirm").html(html);
        $("#tab-esi-dialog-confirm").dialog({
            resizable: false,
            height: "auto",
            width: 600,
            modal: true,
            buttons: {
                "Yes": function () {
                    $(this).dialog("close");
                    if (typeof successCallback === 'function')
                        successCallback();
                },
                "No": function () {
                    $(this).dialog("close");
                }
            }
        });
    },
    init: function () {
        $('#ckbGivenCredit').click(function () { editSuperInfo.showHideGivenCredit(); });
        $('#ckbResetPwd').click(function () { editSuperInfo.showHideUpdatePassword(); });
        $('#ckbResetSecCode').click(function () { editSuperInfo.showHideUpdateSecurityCode(); });
        $('.btn-icon').click(function () { editSuperInfo.showHidePwd($(this)); });

        $("#save-ugc").click(function () { editSuperInfo.saveGivenCredit(); });
        $("#save-pwd").click(function () { editSuperInfo.savePassword(); });
        $("#save-scc").click(function () { editSuperInfo.saveSecurityCode(); });
        $("#btnSubmitUpdate").click(function () { editSuperInfo.saveInfo(); });
        $('#SuperGivenCredit').change(function (e) {
            var $this = $(this);
            var val = $this.val();
            $this.val(val.replace(/,/g, '').replace(/(?<!\..*)(\d)(?=(?:\d{3})+(?:\.|$))/g, '$1,'));
        });
    },
    createMessageBox: function (msg, isError) {
        if (isError) {
            return '<div class="alertboxerr"><div>' + msg + '</div></div>';
        }

        return '<div class="alertboxscc"><div>' + msg + '</div></div>';
    },
    showHideGivenCredit: function () {
        var checked = $("#ckbGivenCredit").prop("checked");
        var tab = $('#updateGivenCredit');
        if (checked) {
            tab.removeClass("hidden");
        }
        else {
            tab.addClass("hidden");
        }

        editSuperInfo.showGivenCredit = checked;
        if (checked || editSuperInfo.showUpdatePassword || editSuperInfo.showUpdateSecurityCode) {
            $("#btnSubmitUpdate").attr("disabled", null);
        } else {
            $("#btnSubmitUpdate").attr("disabled", "disabled");
        }
    },
    showHideUpdatePassword: function () {
        var checked = $("#ckbResetPwd").prop("checked");
        var tab = $('#updatePwdTab');
        if (checked) {
            tab.removeClass("hidden");
        }
        else {
            tab.addClass("hidden");
        }

        editSuperInfo.showUpdatePassword = checked;
        if (checked || editSuperInfo.showGivenCredit || editSuperInfo.showUpdateSecurityCode) {
            $("#btnSubmitUpdate").attr("disabled", null);
        } else {
            $("#btnSubmitUpdate").attr("disabled", "disabled");
        }
    },
    showHideUpdateSecurityCode: function () {
        var checked = $("#ckbResetSecCode").prop("checked");
        var tab = $('#updateSecCodeTab');
        if (checked) {
            tab.removeClass("hidden");
        }
        else {
            tab.addClass("hidden");
        }

        editSuperInfo.showUpdateSecurityCode = checked;
        if (checked || editSuperInfo.showGivenCredit || editSuperInfo.showUpdatePassword) {
            $("#btnSubmitUpdate").attr("disabled", null);
        } else {
            $("#btnSubmitUpdate").attr("disabled", "disabled");
        }
    },
    showHidePwd: function (iConSpan) {
        var targetId = iConSpan.data("id");
        var currentType = $("#" + targetId).prop("type");
        if (currentType === "text") {
            $("#" + targetId).prop("type", "password");
            iConSpan.removeClass("icon-eye-off").addClass("icon-eye");
        }
        else {
            $("#" + targetId).prop("type", "text");
            iConSpan.removeClass("icon-eye").addClass("icon-eye-off");
        }
    },

    getSuperInfo: function () {
        parent.BlockUI();
        $('#tab-esi-errMsg').html('');
        $('#tab-esi-errSupers').html('');
        var superList = $("#tab-esi-superlist").val();

        if (superList === "") {
            showAlertMessage('tab-esi-errMsg', "Please enter at least 1 Super Account username.", 1);
            parent.jQuery.unblockUI();
            return;
        }

        if (superList.split(',').length > 100) {
            showAlertMessage('tab-esi-errMsg', "There're more than 100 Usernames.", 1);
            parent.jQuery.unblockUI();
            return;
        }

        var request = {
            SourceSuper: '',
            SuperList: superList,
            IsCopy: false,
            Mode: 'AccountInfo'
        };

        editSuperInfo.superList = superList;
        $("#tab-esi-superSetting").addClass("hidden");
        $.ajax({
            type: "POST",
            url: "CheckSuperList",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.ErrorCode === 999) {
                    showAlertMessage('tab-esi-errMsg', data.ErrorMessage, 1);
                    return;
                }

                editSuperInfo.updateData = data;
                if (data.Result.ErrorCode !== 0) {
                    showAlertMessage('tab-esi-errMsg', data.Result.ErrorMessage, 1);
                }

                if (data.SuperList && data.SuperList.length > 0) {
                    editSuperInfo.showInfo();
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("tab-esi-errMsg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
            }
        });
    },
    showInfo: function () {
        var resultContainer = $("#tab-esi-update-result");
        resultContainer.empty();

        $("#ckbGivenCredit").prop("checked", true);
        $('#SuperGivenCredit').val('');
        $('#updateGivenCredit').removeClass("hidden");

        $("#ckbResetPwd").prop("checked", true);
        $('#SuperPassword').val('');
        $('#SuperConfirmPwd').val('');
        $("#ForceChangePassword").prop("checked", true);
        $('#updatePwdTab').removeClass("hidden");

        $("#ckbResetSecCode").prop("checked", true);
        $('#SuperSecCode').val('');
        $('#SuperConfirmSecCode').val('');
        $("#SuperForceChange").prop("checked", true);
        $('#updateSecCodeTab').removeClass("hidden");

        $("#tab-esi-superSetting").removeClass("hidden");
    },
    saveGivenCredit: function () {
        parent.BlockUI();
        var resultContainer = $("#tab-esi-update-result");
        resultContainer.empty();

        var amount = $('#SuperGivenCredit').val().replace(/,/g, '');
        if (!amount.isNumeric()) {
            resultContainer.append(editSuperInfo.createMessageBox('Invalid Given Credit value!', true));
            parent.jQuery.unblockUI();
            return;
        }

        var request = {
            Supers: editSuperInfo.updateData.SuperList,
            EditGivenCreditRequest: { amount: amount }
        };

        $.ajax({
            type: "POST",
            url: "EditSuperInfo",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.ErrorCode === 999) {
                    resultContainer.append(editSuperInfo.createMessageBox(data.ErrorMessage, true));
                    return;
                }

                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var r = data[i];
                        resultContainer.append(editSuperInfo.createMessageBox(r.ErrorMessage, r.ErrorCode !== 0));
                    }
                }

                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    resultContainer.append(editSuperInfo.createMessageBox($.parseJSON(xhr.responseText).ErrorMessage, true));
                }
            }
        });
    },
    savePassword: function () {
        parent.BlockUI();
        var resultContainer = $("#tab-esi-update-result");
        resultContainer.empty();

        var password = $('#SuperPassword').val();
        if (password === '') {
            resultContainer.append(editSuperInfo.createMessageBox('Sorry, password cannot be empty.', true));
            window.scrollTo(0, 300);
            parent.jQuery.unblockUI();
            return;
        }

        if (password.length < 4) {
            resultContainer.append(editSuperInfo.createMessageBox('Password must have at least 4 characters.', true));
            window.scrollTo(0, 300);
            parent.jQuery.unblockUI();
            return;
        }

        var confirmedPassword = $('#SuperConfirmPwd').val();
        if (confirmedPassword !== password) {
            resultContainer.append(editSuperInfo.createMessageBox('Password was not correctly confirmed.', true));
            window.scrollTo(0, 300);
            parent.jQuery.unblockUI();
            return;
        }

        var code = $("#EncryptorCode").val();
        var request = {
            Supers: editSuperInfo.updateData.SuperList,
            EditPasswordRequest: {
                password: password.encrypt(code),
                confirmedPassword: confirmedPassword.encrypt(code),
                forceChangePassword: $("#ForceChangePassword").prop("checked")
            }
        };

        $.ajax({
            type: "POST",
            url: "EditSuperInfo",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.ErrorCode === 999) {
                    resultContainer.append(editSuperInfo.createMessageBox(data.ErrorMessage, true));
                    return;
                }

                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var r = data[i];
                        resultContainer.append(editSuperInfo.createMessageBox(r.ErrorMessage, r.ErrorCode !== 0));
                    }
                }

                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    resultContainer.append(editSuperInfo.createMessageBox($.parseJSON(xhr.responseText).ErrorMessage, true));
                }
            }
        });
    },
    saveSecurityCode: function () {
        parent.BlockUI();
        var resultContainer = $("#tab-esi-update-result");
        resultContainer.empty();

        var secCode = $('#SuperSecCode').val();
        if (secCode === '') {
            resultContainer.append(editSuperInfo.createMessageBox("Sorry, security code can't be empty.", true));
            window.scrollTo(0, 300);
            parent.jQuery.unblockUI();
            return;
        }

        var confirmedSecCode = $('#SuperConfirmSecCode').val();
        if (confirmedSecCode !== secCode) {
            resultContainer.append(editSuperInfo.createMessageBox("Sorry, confirm security code doesn't match with security code.", true));
            window.scrollTo(0, 300);
            parent.jQuery.unblockUI();
            return;
        }

        if (!editSuperInfo.isSecurityCode(secCode)) {
            resultContainer.append(editSuperInfo.createMessageBox("Sorry, security code is incorrect format.", true));
            window.scrollTo(0, 300);
            parent.jQuery.unblockUI();
            return;
        }

        var code = $("#EncryptorCode").val();
        var request = {
            Supers: editSuperInfo.updateData.SuperList,
            EditSecurityCodeRequest: {
                SecurityCode: secCode.encrypt(code),
                ConfirmedSecurityCode: confirmedSecCode.encrypt(code),
                ForceChangeSecurityCode: $("#SuperForceChangeSecCode").prop("checked")
            }
        };

        $.ajax({
            type: "POST",
            url: "EditSuperInfo",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.ErrorCode === 999) {
                    resultContainer.append(editSuperInfo.createMessageBox(data.ErrorMessage, true));
                    return;
                }

                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var r = data[i];
                        resultContainer.append(editSuperInfo.createMessageBox(r.ErrorMessage, r.ErrorCode !== 0));
                    }
                }

                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    resultContainer.append(editSuperInfo.createMessageBox($.parseJSON(xhr.responseText).ErrorMessage, true));
                }
            }
        });
    },
    isSecurityCode: function (secCode) {
        var secCodeLength = secCode.length;
        if (secCodeLength !== 6) { // check length
            return false;
        }

        var regex = /^([0-9])(?!\1+$)[0-9]+$/; // check digits and all of them are not the same
        var result = secCode.search(regex) !== -1 ? true : false;
        if (result) { // check 'Forward Run' and 'Backward Run'
            var isForwardRun = true;
            var isBackwardRun = true;
            var forward = parseInt(secCode.charAt(0));
            var backward = parseInt(secCode.charAt(0));
            for (var i = 1; i < secCodeLength; i++) {
                var current = parseInt(secCode.charAt(i));
                if (++forward !== current) {
                    isForwardRun = false;
                }

                if (--backward !== current) {
                    isBackwardRun = false;
                }
            }

            result = !(isForwardRun || isBackwardRun);
        }

        return result;
    },
    saveInfo: function () {
        parent.BlockUI();
        var resultContainer = $("#tab-esi-update-result");
        resultContainer.empty();

        var isUpdateGivenCredit = $("#ckbGivenCredit").prop("checked");
        if (isUpdateGivenCredit) {
            var amount = $('#SuperGivenCredit').val().replace(/,/g, '');
            if (!amount.isNumeric()) {
                resultContainer.append(editSuperInfo.createMessageBox('Invalid Given Credit value!', true));
                parent.jQuery.unblockUI();
                return;
            }
        }

        var isUpdatePassword = $("#ckbResetPwd").prop("checked");
        if (isUpdatePassword) {
            var password = $('#SuperPassword').val();
            if (password === '') {
                resultContainer.append(editSuperInfo.createMessageBox('Sorry, password cannot be empty.', true));
                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
                return;
            }

            if (password.length < 4) {
                resultContainer.append(editSuperInfo.createMessageBox('Password must have at least 4 characters.', true));
                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
                return;
            }

            var confirmedPassword = $('#SuperConfirmPwd').val();
            if (confirmedPassword !== password) {
                resultContainer.append(editSuperInfo.createMessageBox('Password was not correctly confirmed.', true));
                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
                return;
            }

        }

        var isUpdateSecurityCode = $("#ckbResetSecCode").prop("checked");
        if (isUpdateSecurityCode) {
            var secCode = $('#SuperSecCode').val();
            if (secCode === '') {
                resultContainer.append(editSuperInfo.createMessageBox("Sorry, security code can't be empty.", true));
                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
                return;
            }

            var confirmedSecCode = $('#SuperConfirmSecCode').val();
            if (confirmedSecCode !== secCode) {
                resultContainer.append(editSuperInfo.createMessageBox("Sorry, confirm security code doesn't match with security code.", true));
                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
                return;
            }

            if (!editSuperInfo.isSecurityCode(secCode)) {
                resultContainer.append(editSuperInfo.createMessageBox("Sorry, security code is incorrect format.", true));
                window.scrollTo(0, 300);
                parent.jQuery.unblockUI();
                return;
            }
        }

        var code = $("#EncryptorCode").val();

        var request = {
            Supers: editSuperInfo.updateData.SuperList,
            EditGivenCreditRequest: isUpdateGivenCredit ? { amount: amount } : null,
            EditPasswordRequest: isUpdatePassword ? {
                password: password.encrypt(code),
                confirmedPassword: confirmedPassword.encrypt(code),
                forceChangePassword: $("#ForceChangePassword").prop("checked")
            } : null,
            EditSecurityCodeRequest: isUpdateSecurityCode ? {
                SecurityCode: secCode.encrypt(code),
                ConfirmedSecurityCode: confirmedSecCode.encrypt(code),
                ForceChangeSecurityCode: $("#SuperForceChangeSecCode").prop("checked")
            } : null
        };

        $.ajax({
            type: "POST",
            url: "EditSuperInfo",
            contentType: "application/json",
            data: JSON.stringify(request),
            success: function (data) {
                if (data.ErrorCode === 999) {
                    resultContainer.append(editSuperInfo.createMessageBox(data.ErrorMessage, true));
                    return;
                }

                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var r = data[i];
                        resultContainer.append(editSuperInfo.createMessageBox(r.ErrorMessage, r.ErrorCode !== 0));
                    }
                }

                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    resultContainer.append(editSuperInfo.createMessageBox($.parseJSON(xhr.responseText).ErrorMessage, true));
                }
            }
        });
    }
};