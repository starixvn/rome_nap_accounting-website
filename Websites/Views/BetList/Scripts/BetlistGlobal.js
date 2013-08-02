function virtualHRPopup(sporttype, matchID, raceNo) {
    try { parent.BlockUI(); } catch (e) { }
    var URL = "";
    var title = "";
    var heightTmp = 0;
    var widthTmp = 0;
    var params = {
        MatchID: matchID,
        RaceNo: raceNo
    };
    switch (sporttype) {
        case 181:
            URL = "BetList/GetVirtualHRPopup?MatchID=" + matchID + "&RaceNo=" + raceNo;
            title = "Virtual Horse Racing";
            heightTmp = 200;
            widthTmp = 850;
            break;
        case 186:
            URL = "BetList/GetVirtualTennisPopup?MatchID=" + matchID + "&RaceNo=" + raceNo;
            title = "Tennis - Virtual Tennis";
            heightTmp = 180;
            widthTmp = 750;
            break;
    }

    parent.OpenNextPopup("popup", window.rootUrl + URL, title, widthTmp, heightTmp, true, true, window, 2000, null, null);

     try { parent.jQuery.unblockUI(); } catch (e) { }
//    $.ajax({
//        type: "POST",
//        url: window.rootUrl + URL,
//        data: params,
//        success: function (result) {
//            $("#Popup").html(result);

//            $("#Popup").dialog({
//                title: title,
//                autoOpen: false,
//                resizable: true,
//                height: heightTmp,
//                width: widthTmp,
//                modal: true,
//                position: ['middle', 250],
//                close: function (event, ui) {
//                }
//            });

//            $('a.ui-dialog-titlebar-close').show();
//            if (sporttype == 186) {
//                title += _leagueName;
//                $("span.ui-dialog-title").text(title);
//            }
//            $("#Popup").dialog("open");

//        },
//        complete: function () {
//            //                                $("#virtualHRPopup").dialog("close");
//            try { parent.jQuery.unblockUI(); } catch (e) { }
//        }
//    });
}


function ViewIP(ip) {
    window.open('http://anonym.to/?http://legacytools.dnsstuff.com/tools/ipall.ch?tool_id=67&token=&toolhandler_redirect=0&j=1&ip=' + ip);
} 