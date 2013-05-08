var RNGCasinoWinLoss = {
    minDate: '8/10/2012', //set min date - the launch of RNG casino to Pennine on the 10th August 2012.
    gameGroups: '',
    gameTypes: '',
    licensees: [],
    licenseeSettings: [{ Id: 'ibcbet', Name: 'FDL', StartDate: '8/10/2012', IsActive: false }, { Id: '1b88', Name: 'PIH', StartDate: '8/10/2012', IsActive: true}],
    fromDate: '',
    toDate: '',
    reportTitle: '',
    init: function () {
        this.getAndRenderLicensees();
        this.renderCalendar();
        this.getAndRenderGameGroup();
        /*#region assign event*/
        $('#ddlGameGroup').on('change', function () {
            var groups = $('#ddlGameGroup').val() == "0" ? RNGCasinoWinLoss.gameGroups : $('#ddlGameGroup').val();
            RNGCasinoWinLoss.getAndRenderGameTypeByGroupId(groups);
        });

        $('#btnSubmit').on('click', function () {
            RNGCasinoWinLoss.getWinLossReport();
        });

        /*#endregion assign event*/
    },
    getAndRenderLicensees: function () {
        try { parent.BlockUI(); } catch (e) { }
        /*#region get ressellers*/
        $.ajax({
            type: "POST",
            url: window.rootUrl + "RNGCasinoWinLoss/GetResellerProductCharge",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                if (rs && rs.length > 0) {
                    var tempData = [];
                    var id = "selectLicensee";
                    var totalLicensees = rs.length;
                    for (var i = 0; i < totalLicensees; i++) {
                        var detail = rs[i];
                        var licenseeConfig = jLinq.from(RNGCasinoWinLoss.licenseeSettings).equals("Id", detail.Id).select();
                        if (licenseeConfig && licenseeConfig[0].IsActive == true) {
                            RNGCasinoWinLoss.licensees.push(detail);
                            tempData.push({
                                Id: id + '_' + detail.Id + '_' + detail.Name + '_' + detail.DistributorRevenue + '_' + detail.StarExpressRevenue + '_' + detail.OneWorksRevenue,
                                Value: detail.Id,
                                Text: detail.Name,
                                Checked: true,
                                Enable: true,
                                CssClass: "",
                                Items: []
                            });
                        }

                    }
                    /*#region assign data to dropdown*/
                    $("#ddlLicenseese").multiSelect(
                    {
                        minWidth: 150,
                        maxHeight: 300,
                        shadow: true,
                        showHeader: true,
                        isCheckAll: true,
                        showFooter: false,
                        checkAllText: 'All',
                        listData: tempData,
                        onCheck: function () {
                            if ($('#ddlLicenseese').multiSelect('getCheckedItems').length <= 0) {
                                RNGCasinoWinLoss.showErrorMessage("Please choose Licensee!");
                            }
                            else {
                                $('#err').hide();
                            }
                        },
                        noItemText: 'No Licensee found.'
                    });
                    /*#endregion assign data to dropdown*/
                }
            },
            error: function () {
                RNGCasinoWinLoss.assignNullDataForDropdown('ddlLicenseese', 150, 300, 'No Licensee found.');
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        })


        /*#endregion get ressellers*/
    },
    renderCalendar: function () {
        var minDate = new Date(RNGCasinoWinLoss.minDate);
        var defaults = {
            showOtherMonths: true,
            selectOtherMonths: true,
            maxDate: maxDate,
            minDate: minDate
        };
        $('#txtFDate,#txtTDate').datepicker(defaults).datepicker("setDate", maxDate);
    },
    getAndRenderGameGroup: function () {
        $('#err').hide();
        var mySelects = $('#ddlGameGroup');
        mySelects.append($('<option>', {
            value: '0',
            text: 'All'
        }));
        $.ajax({
            type: "POST",
            url: window.rootUrl + "RNGCasinoWinLoss/SelTSTGameGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                if (rs && rs.length > 0) {
                    var totalGroups = rs.length;
                    $.each(rs, function (i, group) {                        
                        mySelects.append($('<option>', {
                            value: group.Id,
                            text: group.Name
                        }));
                        RNGCasinoWinLoss.gameGroups = RNGCasinoWinLoss.gameGroups + (group.Id + (i < totalGroups - 1 ? ',' : ''));
                    });

                    RNGCasinoWinLoss.getAndRenderGameTypeByGroupId(RNGCasinoWinLoss.gameGroups);
                }
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    getAndRenderGameTypeByGroupId: function (groupIds) {
        $('#err').hide();
        var mySelects = $('#ddlGameType');
        mySelects.html('');
        mySelects.append($('<option>', {
            value: '0',
            text: 'All'
        }));
        RNGCasinoWinLoss.gameTypes = '';
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "RNGCasinoWinLoss/SelTSTGameTypeByGroupId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ 'groupIds': groupIds }),
            success: function (rs) {
                if (rs && rs.length > 0) {
                    var totalGroups = rs.length;
                    $.each(rs, function (i, group) {                        
                        mySelects.append($('<option>', {
                            value: group.Id,
                            text: group.Name
                        }));
                        RNGCasinoWinLoss.gameTypes = RNGCasinoWinLoss.gameTypes + (group.Id + (i < totalGroups - 1 ? ',' : ''));
                    });
                }
            },
            error: function () {
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    showErrorMessage: function (message) {
        $('#err').show();
        $('#err .alertboxerr div').html(message);
    },
    assignNullDataForDropdown: function (ddlId, minWidth, maxHeight, errMsg) {
        $("#" + ddlId).multiSelect({
            minWidth: minWidth,
            maxHeight: maxHeight,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            listData: [],
            noItemText: errMsg
        });
    },
    getParams: function () {
        RNGCasinoWinLoss.fromDate = $('#txtFDate').val();
        RNGCasinoWinLoss.toDate = $('#txtTDate').val();
        var licensees = [];
        if ($('#ddlLicenseeseHeaderCheckBox').prop('checked')) {
            licensees = RNGCasinoWinLoss.licensees;
        }
        else {
            var listChecked = $('#ddlLicenseese').next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $('#ddlLicenseese').attr('id') + "HeaderCheckBox)").filter('[checked]');
            var totalLicenseeChecked = listChecked.length;
            for (var i = 0; i < totalLicenseeChecked; i++) {

                var arrInfo = listChecked[i].id.split('_');
                licensees.push(
                     { Id: arrInfo[1], Name: arrInfo[2], DistributorRevenue: arrInfo[3], StarExpressRevenue: arrInfo[4], OneWorksRevenue: arrInfo[5] }
                );
            }
        }

        var params = {
            licensees: licensees,
            fromDate: RNGCasinoWinLoss.fromDate,
            toDate: RNGCasinoWinLoss.toDate,
            gameTypes: ($('#ddlGameType').val() == '0' ? RNGCasinoWinLoss.gameTypes : $('#ddlGameType').val())
        };

        return params;
    },
    getWinLossReport: function () {
        var params = RNGCasinoWinLoss.getParams();
        if ($('#ddlLicenseese').multiSelect('getCheckedItems').length <= 0) {
            RNGCasinoWinLoss.showErrorMessage("Please choose Licensee!");
            return false;
        }

        $('#err').hide();
        try { parent.BlockUI(); } catch (e) { }

        var newReq = new Date().getMilliseconds();
        var strRequest = "&licensees=" + JSON.stringify(params.licensees) + "&fromDate=" + params.fromDate + "&toDate=" + params.toDate + "&gameTypes=" + params.gameTypes;
        $.ajax({
            type: "GET",
            url: window.rootUrl + "RNGCasinoWinLoss/SelRNGCasinoWinLossReport/?newReq=" + newReq + strRequest,
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (rs) {
                $('#mainreport').html(rs);
            },
            error: function (req, status, error) {
                RNGCasinoWinLoss.showErrorMessage(status);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    exportExcel: function () {
        var params = RNGCasinoWinLoss.getParams();
        var newReq = new Date().getMilliseconds();
        var strRequest = "/?newReq=" + newReq + "&licensees=" + JSON.stringify(params.licensees) + "&fromDate=" + params.fromDate + "&toDate=" + params.toDate + "&gameTypes=" + params.gameTypes + '&reportTitle=' + RNGCasinoWinLoss.reportTitle;

        $("#frameExportExcel").attr("src", linkExportExcel + strRequest);
    }
};

$(document).ready(function () {
    RNGCasinoWinLoss.init();
});