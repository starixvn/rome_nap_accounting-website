function GetCheckBet() {
    $("#divCheckBetContain").empty();
    $("#divErr").empty();

    var fromDate = $("#fdate").val();
    var TplCheckBet = "<table style='width:250px' class='tbllayout' border='0'>\
                                    <tr class='title'>\
                                    <td class='pad33'>Check Time</td>\
                                    <td class='pad33'>Check 1</td>\
                                    <td class='pad33'>Check 2</td>\
                                    </tr>\
                                    <tr class='report-row-odd c'>\
                                    <td class='pad33'>${CheckDate}</td>\
                                    <td class='pad33'>${Check1}</td>\
                                    <td class='pad33'>${Check2}</td>";

    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "../Service/ReportService.svc/GetCheckBet",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"fromDate":"' + fromDate + '"}',
        success: function (rs) {
            if (rs != null) {
                $.tmpl(TplCheckBet, rs).appendTo("#divCheckBetContain");
            }
            else {
                showAlertMessage("divErr", "There is no data.", 1);
            }

            try { parent.jQuery.unblockUI(); } catch (e) { }
        },
        error: function () {
            showAlertMessage("divErr", "There is no data.", 1);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}