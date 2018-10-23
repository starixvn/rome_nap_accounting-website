var limitNewMember = (function ($) {
    var divMessage = $('#sectionMessage');
    var txtSuper = $('#super').focus();
    var chkUnlimited = $('#chkUnlimited');
    var txtMaxPerDay = $('#txtMaxPerDay');
    var btnUpdateMaxPerDay = $('#btnUpdateMaxPerDay');
    var superLimit = {};

    function showErrorMessage(error) {
        divMessage.removeClass('hide');
        showAlertMessage('sectionMessage', error, 1);
    }

    function showSuccessMessage(message) {
        divMessage.removeClass('hide');
        showAlertMessage('sectionMessage', message, 0);
    }

    function hideSectionMessage() {
        divMessage.empty().addClass('hide');
    }

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if ((charCode >= 48 && charCode <= 57) ||
        (charCode === 8 || charCode === 35 || charCode === 36 || charCode === 37 || charCode === 39 || charCode === 46)) {
            return true;
        }
        else if (charCode === 13) {
            $('#frmUpdateMaxPerDay').trigger('submit');
        }

        return false;
    }

    function submitForm(form, data, successCallback) {
        $.ajax({
            type: form.attr('method'),
            url: form.attr('action'),
            data: data,
            dataType: 'json',
            beforeSend: function (jqXHR, settings) { },
            success: function (data, textStatus, jqXHR) {
                if (successCallback && typeof successCallback === 'function') {
                    successCallback(data);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status === 403) {
                    var denyObject = $.parseJSON(jqXHR.responseText);
                    showErrorMessage(denyObject.ErrorMessage);
                }
            },
            complete: function (jqXHR, textStatus) { }
        });
    }

    function init() {
        $('#frmSearchSuper').submit(function (e) {
            e.preventDefault();
            txtMaxPerDay.val('');
            btnUpdateMaxPerDay.attr('disabled', true);
            var superName = txtSuper.val().trim();
            if (superName.length === 0) {
                txtSuper.focus();
                showErrorMessage(limitNewMemberResource.SuperEmpty);
                return;
            }

            hideSectionMessage();
            submitForm($(this), { Super: superName }, function (data) {
                if (data) {
                    if (data.OutputResult.ErrorCode === 0) {
                        var maxPerDay = data.MaxPerDay;
                        superLimit = { Super: data.Super, MaxPerDay: maxPerDay }
                        if (maxPerDay === -1) {
                            chkUnlimited.attr('checked', true);
                            txtMaxPerDay.val('').attr('disabled', 'disabled');
                        }
                        else {
                            chkUnlimited.attr('checked', false);
                            txtMaxPerDay.val(maxPerDay).removeAttr('disabled');
                        }

                        btnUpdateMaxPerDay.attr('disabled', false);
                    }
                    else {
                        superLimit = undefined;
                        showErrorMessage(data.OutputResult.ErrorMessage);
                    }
                }
                else {
                    superLimit = undefined;
                    txtSuper.focus();
                    showErrorMessage(limitNewMemberResource.SuperNotFound);
                }
            });
        });

        $('#frmUpdateMaxPerDay').submit(function (e) {
            e.preventDefault();
            if (!superLimit || !superLimit.Super) {
                txtSuper.focus();
                showErrorMessage(limitNewMemberResource.SuperEmpty);
                return;
            }

            var maxPerDay = chkUnlimited.attr('checked') ? -1 : txtMaxPerDay.val().trim();
            if (maxPerDay.length === 0) {
                txtMaxPerDay.focus();
                showErrorMessage(limitNewMemberResource.MaxPerDayEmpty);
                return;
            }
            else if (isNaN(maxPerDay)) {
                txtMaxPerDay.focus();
                showErrorMessage(limitNewMemberResource.MaxPerDayInvalid);
                return;
            }

            hideSectionMessage();
            submitForm($(this), { Super: superLimit.Super, Limit: maxPerDay }, function (data) {
                if (data) {
                    if (data.ErrorCode === 0) {
                        superLimit.MaxPerDay = maxPerDay;
                        showSuccessMessage(data.ErrorMessage);
                    }
                    else {
                        showErrorMessage(data.ErrorMessage);
                    }
                }
            });
        });

        chkUnlimited.click(function () {
            var checked = $(this).attr('checked');
            if (checked) {
                txtMaxPerDay.val('').attr('disabled', 'disabled');
            }
            else {
                if (superLimit && superLimit.MaxPerDay >= 0) {
                    txtMaxPerDay.val(superLimit.MaxPerDay);
                }

                txtMaxPerDay.removeAttr('disabled');
            }
        });
    }

    return {
        isNumberKey: function (evt) {
            return isNumberKey(evt);
        },
        init: function () {
            return init();
        }
    };
})(jQuery);

$(document).ready(function () {
    limitNewMember.init(); 
});