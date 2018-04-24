var BetList = BetList || {};

BetList.BetListByRefNo = (function ($) {

    var gridOpts = {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: false,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1000, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 170,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    };

    var $searchTypes;
    var $searchByRefNo, $refNo, $refNoInfoMsg;
    var $searchByProduct, $selectedProduct, $playerId, $username, $gameId, $betId, $fromdate, $toDate, $productInputs;
    var $submit;

    function getData(params, url) {
        $.ajax({
            type: "POST",
            url: url,
            data: params,
            success: function (result) {
                $("#betListContent").html(result);
                $("#BetlistByRefNo").initGrid(gridOpts);
                $('#BetlistByRefNo').removeClass("display-none").gridScroller(gridOpts);
                var divReplace;
                if ($('#choice_left a:first').html() == 'Mix Parlay') {
                    divReplace = '<span style="color: #0066CC;">' + $('#choice_left a:first').html() + '</span>';
                    $('#choice_left a:first').replaceWith(divReplace);
                }

                $("a:[id^='hidden']").each(function () {
                    $(this).removeAttr("href");
                });
                $("#result").attr('style', 'cursor: pointer;cursor: hand;');
            },
            error: function () {
                try {
                    parent.jQuery.unblockUI();
                } catch (e) {
                }
            },
            complete: function () {
                try {
                    parent.jQuery.unblockUI();
                } catch (e) {
                }
            }
        });
    };

    function renderCalendar() {
        var options = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true
        };

        $fromdate.datepicker($.extend({
            onSelect: function (dateText) {
                $toDate.datepicker("option", "minDate", dateText);
            }
        }, options))
        .datepicker("option", "minDate", $fromdate.data("balance-up-day")).datepicker("setDate", new Date());

        $toDate.datepicker(options).datepicker("option", "minDate", new Date()).datepicker("setDate", new Date());
    };

    function showError(message) {
        $("#errMsg .msg").text(message);
        $("#errMsg").show();
    };

    function clearError() {
        $("#errMsg .msg").text("");
        $("#errMsg").hide();
    };

    function selectProduct() {
        $gameId.removeAttr("disabled");
        $betId.removeAttr("disabled");

        var selProductValue = $selectedProduct.val();
        if (selProductValue === "-1") {
            $gameId.attr("disabled", "disabled");
            $betId.attr("disabled", "disabled");
        }
        else if (selProductValue === "22" || selProductValue === "24" || selProductValue === "32") {
            $gameId.attr("disabled", "disabled");
        }
    };

    function selectSearchByRef() {
        $refNo.removeAttr("disabled");
        $refNoInfoMsg.show();
        $productInputs.attr("disabled", "disabled");
    };

    function selectSearchByProduct() {
        $refNo.attr("disabled", "disabled");
        $refNoInfoMsg.hide();
        $productInputs.removeAttr("disabled");

        selectProduct();
    };

    function validateSearchByProduct() {
        var selProductValue = $selectedProduct.val();
        if (selProductValue === "-1") {
            showError("Product is required");
            return false;
        }
        if ($playerId.val().trim() === "" && $username.val().trim() === "") {
            showError("Either PlayerID or Username is required");
            return false;
        }
        if ((selProductValue === "12" || selProductValue === "21" || selProductValue === "30") && $gameId.val().trim() === "" && $betId.val().trim() === "") {
            showError("Either GameID or BetID is required");
            return false;
        }
        if ((selProductValue === "22" || selProductValue === "24") && $betId.val().trim() === "") {
            showError("BetID is required");
            return false;
        }

        return true;
    };

    function searchByRef() {
        try {
            parent.BlockUI();
        } catch (e) { }

        getData({ refno: $refNo.val().trim() }, window.rootUrl + "BetList/GetBetListByRefNo");
    };

    function searchByProduct() {
        try {
            parent.BlockUI();
        } catch (e) { }

        getData(
            {
                productId: $selectedProduct.val(),

                custId: $playerId.val().trim(),
                username: $username.val().trim(),
                gameId: $gameId.val().trim(),
                betId: $betId.val().trim(),
                fromDate: $fromdate.val(),

                toDate: $toDate.val()
            },
        window.rootUrl + "BetList/GetBetListByProduct");
    };

    function getBetList() {
        if ($searchByRefNo.attr("checked")) {
            clearError();
            searchByRef();

        } else if ($searchByProduct.attr("checked")) {
            if (validateSearchByProduct() === true) {
                clearError();
                searchByProduct();
            }
        }
    };

    function setJqueryMap(container) {
        $searchTypes = container.find("input[type=radio][name='search-type']");

        $searchByRefNo = $("#rdByRef");
        $refNo = $("#txtRefNo");
        $refNoInfoMsg = $("#ref-no-info-msg");

        $searchByProduct = $("#rdByProduct");
        $selectedProduct = $("#selProduct");
        $playerId = $("#txtPlayerId");
        $username = $("#txtUsername");
        $gameId = $("#txtGameId");
        $betId = $("#txtBetId");
        $fromdate = $("#txtfromdate");
        $toDate = $("#txttodate");
        $productInputs = container.find(".search-by-product input[type='text'], .search-by-product select");

        $submit = $("#btnReport");
    };

    function init(container) {
        setJqueryMap(container);

        renderCalendar();

        selectSearchByRef();

        $searchTypes.change(function () {
            if (this.value === "1") {
                selectSearchByRef();
            } else if (this.value === "2") {
                selectSearchByProduct();
            }
        });

        $selectedProduct.change(function () {
            selectProduct();
        });

        $submit.click(function () {
            getBetList();
        });
    };

    return {
        init: init
    }

})(jQuery);

$(document).ready(function () {
    BetList.BetListByRefNo.init($("#bet-list-ref-no"));
});

function showCombMPDetail(tranid, date, refno, betid, matchid) {
    var title = "System Parlay";
    var combinationOfferNameContainer = $("#combinationOfferName_" + refno + "0");

    if (combinationOfferNameContainer.is("span")) {
        title += combinationOfferNameContainer.html();
    }

    showDetailsCMP(tranid, title, parseFloat(refno), parseInt(betid), _custid)
}

function showDetail(id) {
    var e = document.getElementById(id);
    if (e.style.display == 'block') {
        e.style.display = 'none';
    }
    else {
        e.style.display = 'block';
    }

    $("#BetlistByRefNo").gridScroller(BetList.gridOpts);
}
