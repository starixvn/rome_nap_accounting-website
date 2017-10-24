function viewBetListByIP(fromDate, ip) {
    var parentIndex = 1;
    parent[parentIndex].location = window.rootUrl + 'TarusBetList/BetListByIP?IsLink=true&FromDate=' + fromDate + '&IP=' + ip;

    var betListPopUp = parent.$(".ui-dialog");
    if (betListPopUp && betListPopUp.length) {
        betListPopUp.dialog('close');
        betListPopUp.remove();
    }

    var productName = "RNGCasino";
    var detailPopUp = parent.$("#dialog-modal" + productName + "Details");
    detailPopUp.dialog('close');
    detailPopUp.remove();
}

var BetDetailsManager = function () {
    var self = this;
    var gridContainer, gridOpts, formData;

    function initGridOpts() {
        return {
            tblContentId: 'tblContent',
            manualCopyHeader: true,
            isShowPager: true,
            fullWidth: true,
            fullHeight: true,
            gridWidth: 930,
            gridHeight: 20,
            freezeWidth: 0,
            numberFreezeCol: 0,
            pagerOpts: {
                callback: pagerOptsCallback,
                item_per_page_arr: [100, 200, 300, 500, 1000]
            }
        };
    }

    function pagerOptsCallback(page_index, items_per_page, jq) {
        var max_page = Math.ceil(totalRows / items_per_page);
        page_index = page_index < max_page ? page_index : --max_page;

        formData.pageIndex = page_index;
        formData.pageSize = items_per_page;

        betDetailsPaging();
        return false;
    }

    function betDetailsPaging() {
        $.ajax({
            type: "GET",
            url: window.rootUrl + "VoidbridgeBetList/BetDetailsPaging?" + $.param(formData),
            beforeSend: function () {
                try { parent.BlockUI(); } catch (e) { }
            },
            success: function (result) {
                $("#tblContent tbody").html(result);

                gridContainer.gridScroller(gridOpts);

                if (totalRows > 0) {
                    gridOpts.pagerOpts.items_per_page = formData.pageSize;
                    gridOpts.pagerOpts.current_page = formData.pageIndex;

                    $("#pagination").pagination(totalRows, gridOpts.pagerOpts);
                }
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }

    self.initPage = function () {
        gridOpts = initGridOpts();
        formData = {
            transId: transId,
            custId: custId,
            transType: transType,
            pageIndex: 0,
            pageSize: pageSize
        };

        gridContainer = $("#betlist-details");
        gridContainer.initGrid(gridOpts);
        gridContainer.removeClass("display-none").gridScroller(gridOpts);

        if (totalRows > 0) {
            gridOpts.pagerOpts.items_per_page = formData.pageSize;
            gridOpts.pagerOpts.current_page = formData.pageIndex;

            $("#pagination").pagination(totalRows, gridOpts.pagerOpts);
        }

        $("#btnExport").click(function () {
            var exportData = {
                sportType: sportType,
                refNo: formData.transId,
                custId: formData.custId,
                transType: formData.transType
            };
            var url = window.rootUrl + "TarusBetList/ExportBetDetails?" + $.param(exportData);
            $("#frmExportExcel").attr("src", url);
        });
    };
};

var betDetailsManager = new BetDetailsManager();
