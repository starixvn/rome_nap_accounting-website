function check() {
    if ($('#usernames').val().trim() == '') {
        showAlertMessage("errMsg", "Please enter username!", 1);
    }
    else {
        $("#errMsg").hide();
    }
}

function sync() {

}