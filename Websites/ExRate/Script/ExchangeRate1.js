$(window).load(
    function () {
        PTBlockUI();
        var arrTrs = $("#tblmixparlay").find("tbody tr");
        /*for (var i = 0; i < arrTrs.length; ++i) {
        var tds = $(arrTrs[i]).find("td");
        $(tds[0]).find("div").width(66);
        $(tds[0]).find("div")[0].style.overflow = "hidden";
        $(tds[0]).find("div").attr("title", $(tds[0]).find("div").html());

        $(tds[1]).find("div").width(72);
        $(tds[2]).find("div").width(73);
        $(tds[3]).find("div").width(73);
        $(tds[4]).find("div").width(65);
        $(tds[5]).find("div").width(64);
        $(tds[6]).find("div").width(64);
        $(tds[7]).find("div").width(64);
        $(tds[8]).find("div").width(63);
        //$(tds[9]).html("<div style='width:48px; text-align:center'>" + $(tds[9]).html() + "</div>");
        }*/

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
