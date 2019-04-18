$(document).ready(function () {
    $('#submit').click(function () {
        $('#submit').attr('disabled', null);
        $.blockUI({
            message: '<div class="loading"></div>',
            css: {
                border: 'none'
            }
        });
        
        var params = {
            custId: $('#CustId').val(),
            updStatusId: $('#Status').val()
        };

        $.ajax({
            type: "POST",
            url: "UpdateCN88CustomerStatus",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if(result.ErrorCode == 0)
                {
                    $('#successMsg').show();
                    $('#errMsg').hide();
                    setTimeout(function () {
                        top.window.frames['content'].submitSearch();
                        parent.$('#dialog-modalCN88CustomerStatusSetting').dialog('close');
                        parent.$('#dialog-modalCN88CustomerStatusSetting').dialog('destroy');
                    }, 3000)
                }
                else
                {
                    $('#successMsg').hide();
                    $('#errMsg').html(result.ErrorMessage);
                    $('#errMsg').show();
                    $('#submit').removeAttr('disabled');
                }
            },
            error: function () {
                $('#submit').removeAttr('disabled');
                $.unblockUI();
            },
            complete: function () {
                $.unblockUI();
            }
        });
    });
});