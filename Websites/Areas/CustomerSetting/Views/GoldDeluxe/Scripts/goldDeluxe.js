/* global model, rootUrl, PTBlockUI, PTUnBlockUI, showAlertMessage */
/* jshint unused: vars */
(function () {
    'use strict';

    var dirty = false;
    var formData = {
        custId: model.CustId
    };
    function statusToggle(status) {
        var oldStatus = model.Enable;
        var oldDownlineStatus = model.DownlineEnable;
        var downlineCheckbox = $('#chkDownlineSetting');
        var downlineLabel = $('#lblDownlineSetting');
        if (status) {
            if (!oldStatus) {
                downlineCheckbox.removeAttr('disabled');
                downlineLabel.removeAttr('disabled');
            }
            if (oldDownlineStatus) {
                downlineCheckbox.attr('checked', 'checked');
            }
        }
        else {
            downlineCheckbox.removeAttr('checked').attr('disabled', 'disabled');
            downlineLabel.attr('disabled', 'disabled');
        }

        checkFormDirty();
    }

    function showMessage(message, type) {
        type = type || 0;
        showAlertMessage('result-message', message, type);
    }

    function checkFormDirty() {
        formData.enable = $('#chkSetting').attr('checked');
        dirty = model.Enable !== formData.enable;

        var custBtn1 = $('.custBtn1'),
            custBtn2 = $('.custBtn2');
        if (dirty) {
            custBtn1.removeAttr('disabled');
            custBtn2.removeAttr('disabled');
        }
        else {
            custBtn1.attr('disabled', 'disabled');
            custBtn2.attr('disabled', 'disabled');
        }

        return dirty;
    }

    function initialize() {
        if (!model) {
            $('#setting-container').addClass('hide');
            showMessage('Invalid customer.', 1);
            return;
        }

        var html = [];
        html.push('<tr class="l">');
        html.push('<td>');
        html.push('<input type="checkbox" id="chkSetting"');
        if (model.Enable) {
            html.push(' checked="checked"');
        }

        html.push('/>');
        html.push('<label class="label-for-checkbox" for="chkSetting">' + model.Name + '</label>');
        html.push('</td>');
        // Downline
        html.push('<td>');
        html.push('<input type="checkbox" id="chkDownlineSetting"');
        if (model.DownlineEnable) {
            html.push(' checked="checked"');
        }

        html.push(' disabled="disabled"/>');
        html.push('<label id="lblDownlineSetting"');
        html.push(' class="label-for-checkbox" for="chkDownlineSetting" disabled="disabled">' + model.DownlineName + '</label>');
        html.push('</td>');
        html.push('</tr>');

        $('#customer-info-title').html(model.Username);

        $('#table-settings > tbody').html(html.join(''));

        // Register events
        $('#chkSetting').click(function () {
            var $this = $(this);
            statusToggle($this.attr('checked'), $this.attr('data-target'));
        });

        $('#button-update').click(function () {
            if (!dirty) {
                return;
            }

            PTBlockUI();
            formData.downlineEnable = $('#chkDownlineSetting').attr('checked');
            $.ajax({
                type: 'POST',
                url: rootUrl + 'GoldDeluxe/Update',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify(formData),
                beforeSend: function (jqXHR, settings) { },
                success: function (data, textStatus, jqXHR) {
                    showMessage(data.ErrorMessage, data.ErrorCode);
                    if (data.ErrorCode === 0) {
                        window.setTimeout(function () {
                            try { parent.ClosePopup(); } catch (e) { }
                        }, 3000);
                    }
                },
                complete: function (jqXHR, textStatus) { PTUnBlockUI(); }
            });
        });

        $('#result-message').html('');
    }

    initialize();
})();