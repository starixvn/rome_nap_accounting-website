var gridOpts = {
    tblContentId: 'tblContent',
    manualCopyHeader: true,
    isShowPager: true,
    fullWidth: true,
    fullHeight: true,
    gridWidth: 1000, //=totalColumnWidth + totalColumn*4 +totalColumn
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

function showMP(transid, refno) {
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
    if ($("#td_right_" + transid).hasClass("isClient")) {
        if (!$("#td_right_" + transid).hasClass("isShow")) {
            $(".parlay_" + transid).each(function () {
                $(this).hide();
                var idTdParent = $(this).parent().parent().attr('id');
                $("#" + idTdParent).addClass("isShow");
                $("#BetlistStatement").gridScroller(gridOpts);
            });
        }
        else {
            $(".parlay_" + transid).each(function () {
                $(this).show();
                $("#choice_left_" + transid).css("height", "");
                var idTdParent = $(this).parent().parent().attr('id');
                $("#" + idTdParent).removeClass("isShow");
                $("#BetlistStatement").gridScroller(gridOpts);
            });
        }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetMixParlayDetail",
            data: "transid=" + transid + "&refno=" + refno,
            success: function (result) {
                $(".parlay_" + transid).each(function () {
                    $(this).html(result);
                    var id = $(this).parent().attr('id');
                    $("#" + id).css("height", "");

                    var idTdParent = $(this).parent().parent().attr('id');
                    $("#" + idTdParent).addClass("isClient");
                });
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
}

function showCombMP(transid, transIndex, refno) {
    showMP(transid, refno);
}

function showNewCombMP(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam) {
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
	var divParlay = divChoiceRight.find(".parlay_" + transId);

	if ($.trim(divParlay.html()) === "") {
		try { parent.BlockUI(); } catch (e) { }
		var params = [
			"transid=", transId, "&refno=", refNo, "&hdp1=", handicap1, "&hdp2=", handicap2, "&betcheck=", betcheck, "&betteam=", betteam
		];

		$.ajax({
			type: "POST",
			url: window.rootUrl + "BetList/GetMixParlayDetail",
			data: params.join(""),
			success: function (result) {
				divParlay.html(result);
				divCombinationOfferName.addClass("hide");
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
		divParlay.toggleClass("hide");
		divCombinationOfferName.toggleClass("hide");
		$("#BetlistStatement").gridScroller(gridOpts);
	}
}

function showCombMPDetail(tranid, date, refno, betid, matchid) {
    var username = $(".combinationLink span:last").html();
    showDetailsCMP(tranid, "System Parlay - " + username, parseFloat(refno), parseInt(betid), _custid)
}