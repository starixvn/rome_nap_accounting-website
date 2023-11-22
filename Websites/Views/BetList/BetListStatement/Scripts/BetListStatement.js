var gridOpts = {
    tblContentId: 'tblContent',
    manualCopyHeader: true,
    isShowPager: true,
    fullWidth: true,
    fullHeight: true,
    gridWidth: 1080, //=totalColumnWidth + totalColumn*4 +totalColumn
    gridHeight: 10,
    freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
    numberFreezeCol: 0
};

head.ready(function (event) {
    try { parent.BlockUI(); } catch (e) { }
    $('#btnExport').click(function () {
        exportExcel();
    });

    $("#BetlistStatement").initGrid(gridOpts);
    $('#BetlistStatement').removeClass("display-none").gridScroller(gridOpts);

    try { parent.jQuery.unblockUI(); } catch (e) { }
});

function exportExcel() {
    url = window.rootUrl + 'BetList/ExportBetlistStatement?username=' + _custName + '&TransType=' + _transType + '&FromDate=' + _fromDate;
    $('#frm').attr('src', url);
}

function showDetail(id) {
    var e = document.getElementById(id);
    if (e.style.display == 'block') {
        e.style.display = 'none';
    }
    else {
        e.style.display = 'block';
    }

    $('#BetlistStatement').gridScroller(gridOpts);
}

function showMP(transid, refno, custId, transIndex) {
    var gridOpts = {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1000, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 10,
        freezeWidth: 510, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    };

    transIndex = transIndex == undefined ? "" : transIndex;
    var divEvent = $("#divEvent_" + transid + transIndex);

    if (divEvent.css("display") === "none") {
        if (divEvent.html() === "") {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: window.rootUrl + "BetList/GetMixParlayDetail",
                data: "transid=" + transid + "&refno=" + refno + "&custId=" + custId,
                success: function (result) {
                    divEvent.html(result);
                    divEvent.css("display", "");
                    $("#BetlistStatement").gridScroller(gridOpts);
                },
                error: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
        else {
            divEvent.css("display", "");
            $("#BetlistStatement").gridScroller(gridOpts);
        }
    }
    else {
        divEvent.css("display", "none");
        $("#BetlistStatement").gridScroller(gridOpts);
    }
}

function showSingleMatchParlayDetail(transid, custid, status, transIndex) {
    var divEvent = $("#divEvent_" + transid + transIndex);

    if (divEvent.css("display") === "none") {
        if (divEvent.html() === "") {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "GET",
                url: window.rootUrl + "BetBuilder/ViewDetail",
                data: "TransactionId=" + transid + "&CustomerId=" + custid + "&Status=" + status,
                success: function (result) {
                    divEvent.html(result);
                    divEvent.css("display", "");
                    $("#BetlistStatement").gridScroller(gridOpts);
                },
                error: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
        else {
            divEvent.css("display", "");
            $("#BetlistStatement").gridScroller(gridOpts);
        }
    }
    else {
        divEvent.css("display", "none");
        $("#BetlistStatement").gridScroller(gridOpts);
    }
}

function showCombMP(transid, transIndex, refno, custId) {
    showMP(transid, refno, custId, transIndex);
}

function showNewCombMP(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
    var gridOpts = {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1000, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 10,
        freezeWidth: 510, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    };

    var divChoiceRight = $("#choice_right_" + transId);
    var divCombinationOfferName = divChoiceRight.find("#combinationOfferName_" + transId + transIndex);
    var divMaxPayout = divChoiceRight.find("#maxPayout_" + transId + transIndex);
    var divParlay = divChoiceRight.find("#divEvent_" + transId + transIndex);

    if (divParlay.css("display") === "none") {
        if ($.trim(divParlay.html()) === "") {
            try { parent.BlockUI(); } catch (e) { }
            var params = [
                "transid=", transId, "&refno=", refNo, "&hdp1=", handicap1, "&hdp2=", handicap2, "&betcheck=", betcheck, "&betteam=", betteam, "&custId=", custId
            ];

            $.ajax({
                type: "POST",
                url: window.rootUrl + "BetList/GetMixParlayDetail",
                data: params.join(""),
                success: function (result) {
                    divParlay.html(result).css("display", "block");
                    divCombinationOfferName.addClass("hide");
                    if (divMaxPayout.length > 0) {
                        divMaxPayout.addClass("hide");
                        divParlay.find(".max-payout").each(function () {
                            $(this).removeClass("hide");
                        });
                    }
                    $("#BetlistStatement").gridScroller(gridOpts);
                },
                error: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
        else {
            divParlay.css("display", "block");
            if (divMaxPayout.length > 0) {
                divMaxPayout.toggleClass("hide");
            }
            divCombinationOfferName.toggleClass("hide");
            $("#BetlistStatement").gridScroller(gridOpts);
        }
    }
    else {
        divParlay.css("display", "none");
        if (divMaxPayout.length > 0) {
            divMaxPayout.toggleClass("hide");
        }
        divCombinationOfferName.toggleClass("hide");
        $("#BetlistStatement").gridScroller(gridOpts);
    }
}

function showCombMPDetail(tranid, date, matchid, betteam) {
    var title = betteam === "1" ? "Mix Parlay" : "System Parlay";
    var combinationOfferNameContainer = $("#combinationOfferName_" + tranid + "0");

    if (combinationOfferNameContainer.is("span")) {
        title += combinationOfferNameContainer.html();
    }

    showDetailsCMP(tranid, title, _custid);
}
