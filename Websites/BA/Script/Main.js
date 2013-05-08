function gotoURL(frmName, url) {
    $('#' + frmName).attr({ src: url });
}

function update_online_status() {
    UpdateOnlineStatus.location.href = "../Handlers/OnlineUpd.ashx";
}