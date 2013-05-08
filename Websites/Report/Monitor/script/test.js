function testfc() {
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/ServiceMonitor.svc/LoadFilterReport",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "",
        success: function (rs) {
            alert(rs);
        }
    });
}
