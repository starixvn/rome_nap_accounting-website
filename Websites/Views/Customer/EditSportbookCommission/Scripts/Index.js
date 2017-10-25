(function ($) {
    $.fn.toggleDisabled = function () {
        return this.each(function () {
            this.disabled = !this.disabled;
        });
    };
})(jQuery);

$(document).ready(function () {
    $('#btnCheckMembers').click(function () {
        if ($('#Usernames').val() == undefined || $('#Usernames').val() == '') {
            $('#errorMessage').text('Please input Username.');
            $('.alertboxerr').show();
            return;
        }

        $('.alertboxerr').hide();
        $('.alertboxscc').hide();
        try { parent.BlockUI(); } catch (e) { }
        var params = { ListUserName: $('#Usernames').val() };
        $.ajax({
            type: "POST",
            url: 'CheckMembers',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode != 0) {
                    $('#contentUpdateCommission').hide();
                    $('#errorMessage').text(result.ErrorMessage);
                    $('.alertboxerr').show();
                }
                else {
                    $('#txtListUserName').text(result.ErrorMessage);
                    $('[id^="sel"][id*="Comm"]').val(0);
                    $('[id^="sel"][id*="Comm"]').attr('disabled', true);
                    $('[id^="chk"]:checkbox').attr('checked', false);
                    $('#btnUpdate').attr('disabled', true);
                    $('#contentUpdateCommission').show();
                }
            },
            error: function () {
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    });

    $('#btnUpdate').click(function () {
        if ($('[id^="chk_"]:checked').length == 0)
        {
            $('#errorMessage').text('Nothing to update.');
            $('.alertboxerr').show();
            return;
        }

        $('.alertboxerr').hide();
        $('.alertboxscc').hide();
        try { parent.BlockUI(); } catch (e) { }
        var params = GetUpdateParams();
        $.ajax({
            type: "POST",
            url: 'Update',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode != 0) {
                    $('#errorMessage').text(result.ErrorMessage);
                    $('.alertboxerr').show();
                }
                else {
                    $('.alertboxscc').show();
                }
            },
            error: function () {
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    });

    $('[id^="sel"][id*="Comm"]').change(function () {
        $('#btnUpdate').removeAttr('disabled');
    });

    $('[id^="chk_"]').change(function () {
        if (this.checked) {
            $('[id^="sel"][id*="Comm"][id*="_' + $(this).attr('sporttype') + '_' + $(this).attr('bettype') + '"]').removeAttr('disabled');
        }
        else {
            $('[id^="sel"][id*="Comm"][id*="_' + $(this).attr('sporttype') + '_' + $(this).attr('bettype') + '"]').attr('disabled', true);
        }

        $('#chkAll').attr('checked', $('[id^="chk_"]:checked').length == $('[id^="chk_"]').length);
        $('#btnUpdate').removeAttr('disabled');
    });

    $('#chkAll').change(function () {
        $('[id^="chk_"]').attr('checked', this.checked).trigger('change');
        $('#btnUpdate').removeAttr('disabled');
    });

    var GetUpdateParams = function () {
        var discountString = '<ROOT>';
        $('[id^="chk_"]').each(function () {
            var $this = $(this);
            if (!this.checked)
            {
                return;
            }

            var sportType = $this.attr('sporttype');
            var betType = $this.attr('bettype');
            var sdiscount = $('#selSComm_' + sportType + '_' + betType).val();
            var mdiscount = $('#selMComm_' + sportType + '_' + betType).val();
            var adiscount = $('#selAComm_' + sportType + '_' + betType).val();
            var playerdiscount = $('#selMemComm_' + sportType + '_' + betType).val();
            discountString += '<Discount SportType="' + sportType + '" BetType="' + betType + '" sdiscount="' + sdiscount + '" mdiscount="' + mdiscount + '" adiscount="' + adiscount + '" playerdiscount="' + playerdiscount + '" />';
        });

        discountString += '</ROOT>';
        return { ListUserName: $('#txtListUserName').text(), Discount: discountString };
    }
});