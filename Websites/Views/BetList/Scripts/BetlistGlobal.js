function virtualHRPopup(sporttype, matchID, raceNo) {
    try { parent.BlockUI(); } catch (e) { }
    var URL = "Report/GetVirtualHRPopup.aspx?MatchID=" + matchID + "&RaceNo=" + raceNo + "&sporttype=" + sporttype;
    var title = "";
    var heightTmp = 0;
    var widthTmp = 0;
    var params = {
        MatchID: matchID,
        RaceNo: raceNo
    };
    switch (sporttype) {
        case 181:          
            title = "Virtual Horse Racing";
            heightTmp = 200;
            widthTmp = 750;
            break;
        case 182:
            title = "Virtual Greyhound";
            heightTmp = 200;
            widthTmp = 750;
            break;
        case 183:
            title = "Virtual Speedway";
            heightTmp = 200;
            widthTmp = 750;
            break;
        case 184:
            title = "Virtual Motor Racing";
            heightTmp = 200;
            widthTmp = 750;
            break;
        case 185:
            title = "Virtual Cycling";
            heightTmp = 200;
            widthTmp = 750;
            break;
        case 186:
            title = "Tennis - Virtual Tennis";
            heightTmp = 180;
            widthTmp = 700;
            break;
    }

    parent.OpenNextPopup("popupVirtualHR", window.rootUrl + URL, title, widthTmp, heightTmp, true, true, window, 2000, null, null);

    try { parent.jQuery.unblockUI(); } catch (e) { }
}


function ViewTennisPopup(matchID, bettype, betId, leagueName) {
    try { parent.BlockUI(); } catch (e) { }
    URL = 'Report/ViewResult.aspx?matchid=' + matchID + (bettype ? '&bettype=' + bettype : '') + '&betid=' + betId;

    parent.OpenNextPopup("TennisResult", window.rootUrl + URL, "Tennis", 945, 300, true, true, window, 2000, null, null);

    try { parent.jQuery.unblockUI(); } catch (e) { }
}


function ViewIP(ip) {
    //    window.open('http://anonym.to/?http://legacytools.dnsstuff.com/tools/ipall.ch?tool_id=67&token=&toolhandler_redirect=0&j=1&ip=' + ip);
    window.open('http://www.iplocationfinder.com/' + ip);
} 