$(document).ready(function () {
    $('#submit').click(function () {
        clearMsg();
        $('#submit').attr('disabled', null);
        $.blockUI({
            message: '<div class="loading"></div>',
            css: {
                border: 'none'
            }
        });

        if ($('#credit').val() == undefined || $('#credit').val() == '') {
            $('#errMsg').html('Please input a value number!').show();
            $.unblockUI();
            return;
        }

        var params = {
            searchAccountId: $('#SearchAccountId').val(),
            roleId: $('#RoleId').val(),
            updCredit: $('#credit').val() == undefined ? 0 : $('#credit').val()
        };

        $.ajax({
            type: "POST",
            url: "UpdateCredit",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode == 0) {
                    $('#successMsg').show();
                    $('#errMsg').hide();
                    setTimeout(function () {
                        top.window.frames['content'].submitSearch();
                        parent.$('#dialog-modalCN88EditCustCredit').dialog('close');
                        parent.$('#dialog-modalCN88EditCustCredit').dialog('destroy');
                    }, 3000)
                }
                else {
                    $('#successMsg').hide();
                    $('#errMsg').html(result.ErrorMessage);
                    $('#errMsg').show();
                    $('#submit').removeAttr('disabled');
                }
            },
            error: function (respond) {
                $('#submit').removeAttr('disabled');
                $.unblockUI();
            },
            complete: function () {
                $.unblockUI();
            }
        });
    });

    $('#cancel').click(function () {
        parent.$('#dialog-modalCN88EditCustCredit').dialog('close');
        parent.$('#dialog-modalCN88EditCustCredit').dialog('destroy');
    });

    function clearMsg() {
        $('#successMsg').hide();
        $('#errMsg').empty().hide();
    }
});