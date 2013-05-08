
var CasinoComm = {
    UpdateSuperLiveCSComm: function () {
        $('#msgError').hide();
        var username = $('#txtusername').val();
        var valueComm = $('#dllValueComm').val();
        if (username == "") {
            showAlertMessage("msgError", "Please input username !", 1);
            $('#msgError').show();
        }
        else {
            if (!confirm("Are you sure you want to update commission for super " + username + " and all downlines?"))
                return;
            var data = { username: username, valueComm: valueComm };
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: "UpdateCasinoCommForSuper",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (rs != null) {
                        if (rs.ErrorCode != 0) {
                            showAlertMessage("msgError", rs.ErrorMessage, 1);
                        }
                        else {
                            showAlertMessage("msgError", "Updated Successfully!", 0);
                        }
                        $('#msgError').show();
                    }
                    setTimeout(function () { $('#msgError').hide(); }, 4000);
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                    $("#btnSubmit").attr("disabled", "disabled");
                }

            });
        }
    },

    CheckUsername: function () {
        var dllValueComm = $("#dllValueComm");
        dllValueComm.empty();
        var comm = parseFloat(0.027);
        for (var i = comm; i >= 0.02; i = (i - 0.001).toFixed(3)) {
            dllValueComm.append("<option value=" + i + ">" + i + "</option>");
        }
        $('#msgError').hide();
        var username = $('#txtusername').val();
        var valueComm = $('#dllValueComm').val();
        if (username == "") {
            showAlertMessage("msgError", "Please input username !", 1);
            $('#msgError').show();
        }
        else {
            var data = { username: username };
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: "CheckUsername",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (rs != null) {
                        if (rs.ErrorCode != 0) {
                            showAlertMessage("msgError", rs.ErrorMessage, 1);
                            $('#msgError').show();
                            $("#btnSubmit").attr("disabled","disabled");
                        }
                        else {
                            var isExisted = false;
                            $("#dllValueComm").each(function () {
                                $('option', this).each(function () {
                                    if ($(this).val() == rs.valueComm) {
                                        $(this).attr('selected', 'selected');
                                        isExisted = true;
                                        return;
                                    };
                                });
                            });

                            if (!isExisted) {
                                $("#dllValueComm").prepend("<option value='" + rs.valueComm + "'>" + rs.valueComm + "</option>");
                                $("#dllValueComm").find("option:first").attr('selected', 'selected');
                            }
                            $("#btnSubmit").removeAttr("disabled");
                        }
                    }
                    setTimeout(function () { $('#msgError').hide(); }, 4000);
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }

                }

            });
        }
    }
};