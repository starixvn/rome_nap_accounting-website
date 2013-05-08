$(document).ready(function () {
    var options = {
        showOtherMonths: true,
        selectOtherMonths: true,
        checkMinMax: true
    };
    calendarUtils.renderCalendar("#fdate", null, "#fdate", "", fDay, options);
    calendarUtils.renderCalendar("#tdate", null, "#fdate", "#tdate", tDay, options);
    getReport();
});

function getReport() {
    try { parent.BlockUI(); } catch (e) { }

    $("#err").hide();
    var fromDate = $("#fdate").val();
    var toDate = $("#tdate").val();
    $.ajax({
        url: '../Service/ReportService.svc/GetNumberOfActiveMembers',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"fromDate":"' + fromDate + '","toDate":"' + toDate + '"}',
        success: function (rs) {
            if (rs != null) {
                var infoArr = [];
                var totalRow = rs.length;
                for (var i = 0; i < totalRow; i++) {
                    var currentValue = rs[i];
                    infoArr.push('<div class="activemember-eachrow">' +
                                    '<div class="activemember-left">' + currentValue.GameName + '</div>' +
                                    '<div class="activemember-right">' + currentValue.NoActives + '</div></div>');
                }

                $('#divMainActiveMembers').html($(infoArr.join('')));
                $("#divMainActiveMembers").show();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            } else {
                $("#divMainActiveMembers").hide();
                $("#err").show();
                showAlertMessage("err", "There is no data.", 1);

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        },
        error: function (xhr, err) {
            $("#divMainActiveMembers").hide();
            $("#err").show();
            showAlertMessage("err", "There is no data.", 1);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }

    });
}

function Export2Excel() {
    var fromDate = $("#fdate").val();
    var toDate = $("#tdate").val();
    var params = 'type=export&fdate=' + fromDate + '&tdate=' + toDate;
    $('#frm').attr('src', 'GamesActiveMembers.aspx?' + params);
}