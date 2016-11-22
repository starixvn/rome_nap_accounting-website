var model = null;

function check() {
    if ($('#usernames').val().trim() == '') {
        showAlertMessage("errMsg", "Please enter username!", 1);
    }
    else if ($('#usernames').val().trim().split(',').length > 10) {
        showAlertMessage("errMsg", "You input more than 10 customers!", 1);
    }
    else {
        $("#errMsg").hide();

        parent.BlockUI();

        $.ajax({
            type: "POST",
            url: "Check",
            data: { usernames: document.getElementById("usernames").value },
            success: function (rs) {
                model = rs;

                if (rs.IsSuccess == false) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 1);
                    $("#errMsg").show();
                }
                else {
                    if (rs.Usernames.length > 0) {
                        $('#sync').attr('disabled', false);
                    }
                }
            },
            error: function (rs) {
                if (rs.IsSuccess == false) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 1);
                    $("#errMsg").show();
                }
                else {
                    console.log(rs);
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    }
}

function sync() {
    $.ajax({
        type: "POST",
        url: "Sync",
        data: { syncCustomerModel: JSON.stringify(model) },
        success: function (rs) {
            if (rs.IsSuccess == false) {
                showAlertMessage("errMsg", rs.ErrorMessage, 1);
                $("#errMsg").show();
            }
            else {
                if (rs.Usernames.length > 0) {
                    $('#sync').attr('disabled', false);
                }
            }
        },
        error: function (rs) {
            if (rs.IsSuccess == false) {
                showAlertMessage("errMsg", rs.ErrorMessage, 1);
                $("#errMsg").show();
            }
            else {
                console.log(rs);
            }
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}