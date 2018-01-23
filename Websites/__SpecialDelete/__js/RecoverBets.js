function CheckDecNumber(e) {

    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;


    if (code == 37 || code == 39 || code == 36 || code == 35 || code == 118) //e or E or "-"
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9)
        return false;
    return true;
}

var tmplMatchInfo = '';
var tmplBetsConent = '';
function getRecoverBets() {
    $('#dvmsg').html('');
    $('#ErrMsg').html('');

    if ($('#transid').val() == '') {
        $('#dvContent').hide();
        $('#ErrMsg').show();
        
        showAlertMessage("ErrMsg", "Please enter transid", 1);
        return;
    }

    if (isNaN($('#transid').val()))
    {
        $('#dvContent').hide();
        $('#ErrMsg').show();    
        showAlertMessage("ErrMsg", "Invalid transid", 1);
        return;
    }

    try { parent.BlockUI(); } catch (e) { }

    if (tmplMatchInfo == '')
        tmplMatchInfo = $("#tmplMatchInfo").template();
    if (tmplBetsConent == '')
        tmplBetsConent = $("#tmplBetsConent").template();

    var data = 
    {
       'transid': $('#transid').val()
    };
    $.ajax({
        type: "POST",
        url: "Services/SpecialDeleteService.svc/SelRecoverBets",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            if (rs.ErrorCode > 0) {
                $('#ErrMsg').show();
                $('#dvContent').hide();
                showAlertMessage("dvmsg", rs.ErrorMessage, 1);
                parent.jQuery.unblockUI();
                return;
            }
            if (rs.rows.length == 0) {
                $('#ErrMsg').show();
                $('#dvContent').hide();
                showAlertMessage("dvmsg", "There is no information available", 1);
                parent.jQuery.unblockUI();
                return;
            }

            $('#dvContent').show();
            $('#ErrMsg').hide();

            $('#tbodyMachInfo').html('');
            $('#tbodyBetsContent').html('');

            $.tmpl(tmplMatchInfo, rs).appendTo("#tbodyMachInfo");
            $.tmpl(tmplBetsConent, rs.rows).appendTo("#tbodyBetsContent");


            if ($("#dvContentBets").height() > 570)
                $("#dvContentBets").css("height", "570px");

            parent.jQuery.unblockUI();
        }
    });
}

function RecoverBet(transid) {
    if (!confirm('Are you sure you want to recover transaction ' + transid + ' ?'))
        return;

    try { parent.BlockUI(); } catch (e) { }

    var data =
    {
        'transid': transid
    };
    $.ajax({
        type: "POST",
        url: "Services/SpecialDeleteService.svc/RecoverBet",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {

            $('#dvContent').hide();
            $('#dvHeader').hide();
            $('#ErrMsg').show();
            $('#ErrMsg').html(rs.ErrorMessage);
            parent.jQuery.unblockUI();
        }
    });
}
