function viewBetListByIP(fromDate, ip) {
    var parentIndex = 1;
    parent[parentIndex].location = window.rootUrl + 'TarusBetList/BetListByIP?IsLink=true&FromDate=' + fromDate + '&IP=' + ip;

    var betListPopUp = parent.$(".ui-dialog");
    if (betListPopUp && betListPopUp.length) {
        betListPopUp.dialog('close');
        betListPopUp.remove();
    }

    var detailPopUp = parent.$("#dialog-modalMicrogamingDetails");
    detailPopUp.dialog('close');
    detailPopUp.remove();
}

var MicroGamingBetDetailsManager = function () {
    var self = this;
    var gridContainer, gridOpts, formData;

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
            url: window.rootUrl + "MicroGamingBetList/BetDetailsPaging?" + $.param(formData),
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
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }

    self.initPage = function () {
        formData = {
            refNo: refNo,
            custId: custId,
            transType: transType,
            pageIndex: 0,
            pageSize: pageSize
        };

        gridOpts = {
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
                refNo: formData.refNo,
                custId: formData.custId,
                transType: formData.transType
            };
            var url = window.rootUrl + "TarusBetList/ExportBetDetails?" + $.param(exportData);
            $("#frmExportExcel").attr("src", url);
        });
    };
};

var microGamingBetDetailsManager = new MicroGamingBetDetailsManager();
                 