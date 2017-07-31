var BetListByIP = {
    gridOpts_Username: {
        tblContentId: 'tblContent',
        isShowPager: true,
        manualCopyHeader: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 610, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 60,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    },
    pagerOpts: {
        callback: null,
        start_per_page: 100,
        step_per_page: 100,
        max_items_per_page: 500,
        items_per_page: 100,
        current_page: 1
    },
    params_UsernameBetlist: {
        ip: '',
        fromDate: null,
        pageIndex: 1,
        pageSize: 100
    },
    getBetListByIP_Username: function (isLink, ip, fromdate) {
        if (fromdate == 'nodate') { return false; }

        var ipTmp = isLink == "True" ? ip : $("#txtIP").val();
        var fromDateTmp = isLink == "True" ? fromdate : $("#txtfromdate").val();

        if (BetListByIP.params_UsernameBetlist.ip != ipTmp || BetListByIP.params_UsernameBetlist.fromDate != fromDateTmp)
        {
            BetListByIP.params_UsernameBetlist.ip = ipTmp;
            BetListByIP.params_UsernameBetlist.fromDate = fromDateTmp;
            BetListByIP.params_UsernameBetlist.pageIndex = 1;
            BetListByIP.params_UsernameBetlist.pageSize = 100;
        }
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetBetListByIP_Username",
            data: BetListByIP.params_UsernameBetlist,
            success: function (result) {
                $("#betListContent_Soccer").remove();
                $("#betListContent").html(result);
                BetListByIP.gridOpts_Username.isShowPager = _length == 0 ? false : true;
                $("#BetlistByIP_Username").initGrid(BetListByIP.gridOpts_Username);
                $('#BetlistByIP_Username').removeClass("display-none").gridScroller(BetListByIP.gridOpts_Username);

                BetListByIP.initPager(_length, BetListByIP.params_UsernameBetlist.pageSize,
                    BetListByIP.params_UsernameBetlist.pageIndex - 1);
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },

    initPager: function (total_items, items_per_page, current_page) {
        BetListByIP.pagerOpts.callback = BetListByIP.pageSelectCallback;
        BetListByIP.pagerOpts.items_per_page = items_per_page;
        BetListByIP.pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, BetListByIP.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_length / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        BetListByIP.params_UsernameBetlist.pageIndex = page_index + 1;
        BetListByIP.params_UsernameBetlist.pageSize = _items_per_page;
        BetListByIP.getBetListByIP_Username();
        return false;
    }
};

var BetListByIP_SportType = {
    gridOpts_Soccer: {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 980, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 80,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    },
    pagerOpts: {
        callback: null,
        start_per_page: 10,
        step_per_page: 10,
        item_per_page_arr: [50, 100, 150, 200],
        max_items_per_page: 200,
        items_per_page: 50,
        current_page: 1
    },
    params_SportType:
    {
        customerId: '',
        ip: '',
        fromDate: null,
        isSoccer: null,
        pageIndex: 1,
        pageSize: 50
    },

    getBetListByIP_Soccer: function (custID, fromdate, ip, isSoccer) {
        if (BetListByIP_SportType.params_SportType.ip != ip ||
            BetListByIP_SportType.params_SportType.fromDate != fromdate ||
            BetListByIP_SportType.params_SportType.customerId != custID ||
            BetListByIP_SportType.params_SportType.isSoccer != isSoccer)
        {
            BetListByIP_SportType.params_SportType.customerId = custID;
            BetListByIP_SportType.params_SportType.ip = ip;
            BetListByIP_SportType.params_SportType.fromDate = fromdate
            BetListByIP_SportType.params_SportType.isSoccer = isSoccer;
            BetListByIP_SportType.params_SportType.pageIndex = 1;
            BetListByIP_SportType.params_SportType.pageSize = 50;
        }

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetBetListByIP_Soccer",
            data: BetListByIP_SportType.params_SportType,
            success: function (result) {
                $("#BetlistByIP_Username").remove();
                $("#betListContent").html(result);

                if (typeof _lengthSprortype === 'undefined') _lengthSprortype = 0;
                BetListByIP_SportType.gridOpts_Soccer.isShowPager = _lengthSprortype == 0 ? false : true;
                $("#BetlistByIP_Soccer").initGrid(BetListByIP_SportType.gridOpts_Soccer);
                $('#BetlistByIP_Soccer').removeClass("display-none").gridScroller(BetListByIP_SportType.gridOpts_Soccer);
                BetListByIP_SportType.initPager(_lengthSprortype,
                    BetListByIP_SportType.params_SportType.pageSize,
                    BetListByIP_SportType.params_SportType.pageIndex - 1);

                //                $("#filter").css("width", "").attr('style', 'width:' + $("#betListContent").width() + 'px !important;');
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },


    initPager: function (total_items, items_per_page, current_page) {
        BetListByIP_SportType.pagerOpts.callback = BetListByIP_SportType.pageSelectCallback;
        BetListByIP_SportType.pagerOpts.items_per_page = items_per_page;
        BetListByIP_SportType.pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, BetListByIP_SportType.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_lengthSprortype / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        BetListByIP_SportType.params_SportType.pageIndex = page_index + 1;
        BetListByIP_SportType.params_SportType.pageSize = _items_per_page;
        BetListByIP_SportType.getBetListByIP_Soccer(BetListByIP_SportType.params_SportType.customerId,
            BetListByIP_SportType.params_SportType.fromDate,
            BetListByIP_SportType.params_SportType.ip,
            BetListByIP_SportType.params_SportType.isSoccer);
        return false;
    }
};

var Helper = {
    renderCalendar: function () {
        var defaults = {
            showOtherMonths: true,
            selectOtherMonths: true,
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
            }
        };
        var dates = $("#txtfromdate").datepicker(defaults);
        $("#txtfromdate").datepicker("setDate", _today);

    }
};

function showDetail(id) {
    var e = document.getElementById(id);
    if (e.style.display == 'block') {
        e.style.display = 'none';
    }
    else {
        e.style.display = 'block';
    }

    $("#BetlistByIP_Soccer").gridScroller(BetListByIP_SportType.gridOpts_Soccer);
}

function showCombMPDetail(tranid, date, refno, betid, matchid) {
	var title = "System Parlay";
	var combinationOfferNameContainer = $("#combinationOfferName_" + refno + "0");

	if (combinationOfferNameContainer.is("span")) {
		title += combinationOfferNameContainer.html();
	}

	showDetailsCMP(tranid, title, parseFloat(refno), parseInt(betid), _custid);
}

function showMP(transid, refno) {
    var divEvent = $("#divEvent_" + transid);

    if (divEvent.css("display") === "none") {
        if (divEvent.html() === "") {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: window.rootUrl + "BetList/GetMixParlayDetail",
                data: "transid=" + transid + "&refno=" + refno,
                success: function (result) {
                    divEvent.html(result);
                    divEvent.css("display", "");
                    $("#BetlistByIP_Soccer").gridScroller(BetListByIP_SportType.gridOpts_Soccer);
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
            $("#BetlistByIP_Soccer").gridScroller(BetListByIP_SportType.gridOpts_Soccer);
        }
    }
    else {
        divEvent.css("display", "none");
        $("#BetlistByIP_Soccer").gridScroller(BetListByIP_SportType.gridOpts_Soccer);
    }
}

function showCombMP(transid, custid, refno) {
    showMP(transid, refno);
}

function showNewCombMP(transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam) {
	var divChoiceRight = $("#choice_right_" + transId);
	var divCombinationOfferName = divChoiceRight.find("#combinationOfferName_" + transId + transIndex);
	var divParlay = divChoiceRight.find("#divEvent_" + transId + transIndex);

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
			    divParlay.html(result).css("display", "block");
				divCombinationOfferName.addClass("hide");
				$("#BetlistByIP_Soccer").gridScroller(BetListByIP_SportType.gridOpts_Soccer);
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
	    divParlay.css("display", "none");
		divCombinationOfferName.toggleClass("hide");
		$("#BetlistByIP_Soccer").gridScroller(BetListByIP_SportType.gridOpts_Soccer);
	}
}

head.ready(function (event) {
    Helper.renderCalendar();
    if (_fromDate != 'nodate') {
        $("#txtIP").val(_ip);
        $("#txtfromdate").val(_fromDate);
        $("#viewIP").show();
    }

    BetListByIP.getBetListByIP_Username(_isLink, _ip, _fromDate);
});

