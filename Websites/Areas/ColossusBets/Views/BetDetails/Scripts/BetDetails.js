function showColossusBetsDetails(refno, winlossDate, transId, custId) {
    var params = $.param({
        Refno: refno,
        WinLossDate: winlossDate,
        TransId: transId,
        CustId: custId
    });

    parent.OpenNextPopup(
        "ColossusBetsDetails",
        'ColossusBets/BetDetails?' + params,
        'Colossus Bets Details',
        600,
        400,
        true,
        false,
        window,
        0,
        null,
        null);
}