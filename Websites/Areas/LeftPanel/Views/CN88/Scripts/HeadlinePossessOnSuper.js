$(document).ready(function () {
    var today = new Date();
    var tomorrow = new Date();
    tomorrow.setDate(today.getDate() + 1);

    $('#NewEffectiveDate').datepicker({
        showOn: 'both',
        buttonImageOnly: true,
        buttonImage: resolveClientUrl('App_Themes/Images/calendar.gif'),
        buttonText: '',
        dateFormat: 'mm/dd/yy',
        minDate: hasMember ? tomorrow : today,
    });

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
            SportbookPossess: $('#NewSportbookPossess').val(),
            NonSportPossess: $('#NewNonSportPossess').val(),
            EffectiveDate: $('#NewEffectiveDate').datepicker('getDate'),
        };

        $.ajax({
            type: "POST",
            url: "UpdateHeadlinePossessOnSuper",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if(result.ErrorCode == 0)
                {
                    $('#successMsg').show();
                    $('#errorMsg').hide();
                    setTimeout(function () {
                        parent.$('#dialog-modalCN88HeadlinePossessOnSuper').dialog('close');
                        parent.$('#dialog-modalCN88HeadlinePossessOnSuper').dialog('destroy');
                    }, 3000)
                }
                else
                {
                    $('#successMsg').hide();
                    $('#errorMsg').html(result.ErrorMessage);
                    $('#errorMsg').show();
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