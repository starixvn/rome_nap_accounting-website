$(window).load(
    function () {
        PTBlockUI();
        var arrTrs = $("#tblmixparlay").find("tbody tr");   

        $("#tblmixparlay").find("tbody tr").each
        (
            function () {
                $(this).find("td")[0].style.textAlign = "center";
            }
        );
        arrTrs.last().find("td").each(function (index, element) {
            element.style.borderBottom = "0px none";
        });
        $(".body-scroll").height(574);
        $("#tblmixparlay").show();

        $('#tblmixparlay tbody tr').each(function () {
            var obj = $(this).find("td:eq(1) a");
            var des = $(this).find("td:eq(1) div").attr('des');
            if (obj.html() != "INH") {
                tooltip(obj, des);
                
            }
            else {
                tooltip(obj, "INH");
               
            }
        });
        PTUnBlockUI();
    }
);

    function tooltip(obj, content) {
        obj.tipsy(
        { fade: false, gravity: "sw", html: false, title: function () { return content }, opacity: 1 });

//        $("#" + id).tipsy(
//        { fade: true, gravity: "nw", html: true, title: function () { return document.getElementById(desc).innerHTML; } });

       
    }
