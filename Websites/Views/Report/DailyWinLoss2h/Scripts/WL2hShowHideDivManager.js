

var WL2hShowHideDivManager = {
    showCombinationMixParlay: function (sportId, wldate) {
        if (isAllowRead2HOReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");
            bettype = 29;
            CombinationMixParlayReportManager.initGrid(sportId, wldate, bettype);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromCMPR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#combinationMixParlayReport").addClass("hidden");
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showAdvanceMixParlayReport: function (sportId, wldate) {
        if (isAllowReadMixParlayReport) {
            //            try { parent.BlockUI(); } catch (e) { }
            //            $("#dailyWLReportDiv").addClass("hidden");
            //            WL2hShowHideDivManager.updateShareElement();
            //            $("#dailyWinLossReport").addClass("hidden");

            //            bettype = 9;
            //            AdvanceMixParlayReportManager.initGrid(sportId, wldate, bettype);
            bettype = 9;
            var mixParlayReportUrl = '../MixParlay/DailyParlayWinLoss?reportDate=' + wldate +
                                        '&IsRedirect=true' +
                                        '&BetType=' + bettype;
            
            window.location = mixParlayReportUrl;
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showAdvanceMixParlayReportFromMPR: function (sportId, wldate) {
        if (isAllowReadMixParlayReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#mixParlayReport").addClass("hidden");

            bettype = 9;
            AdvanceMixParlayReportManager.initGrid(sportId, wldate, bettype);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromAMPR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#advanceMixParlayReport").addClass("hidden");
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMixParlayReport: function (sportId, wldate) {
        if (isAllowReadMixParlayReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            bettype = 9;
            MixParlayReportManager.initGrid(sportId, wldate, bettype);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMixParlayReportFromAMPR: function (sportId, wldate) {
        if (isAllowReadMixParlayReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#advanceMixParlayReport").addClass("hidden");

            bettype = 9;
            MixParlayReportManager.initGrid(sportId, wldate, bettype);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromMPR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#mixParlayReport").addClass("hidden");
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showLiveCasinoReport: function (sportId, wldate) {
        if (isAllowReadLiveCasinoReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            bettype = 9;
            LiveCasinoReportManager.initGrid(sportId, wldate, bettype);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromLCR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#liveCasinoReport").addClass("hidden");
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },

    showSoccerReport: function (sportId, wldate) {
        if (isAllowReadSoccerReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            SoccerReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromSCR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            scoreInfoManager.ClosePopup();
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();

            $("#gridSplitter_SCR").addClass('hidden');
            SoccerReportManager.rightDetailReport.addClass("hidden");
            $("#soccerReport").addClass("hidden");

            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showHorseRacingReport: function (sportId, wldate) {
        if (isAllowReadHorseRacingReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            HorseRacingReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromHRR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#horseRacingReport").addClass("hidden");

            $("#dailyWLBySport").addClass("hidden");
            $("#dailyWLRacing").addClass("hidden");
            HorseRacingReportManager.showRacingBettypeReport(false);
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showNumberGameReport: function (sportId, wldate) {
        if (isAllowReadNumberGameReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            NumberGameReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromNGR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();

            $("#numberGameReport").addClass("hidden");
            $("#gridSplitter_NGR").addClass('hidden');
            NumberGameReportManager.rightDetailReport.addClass("hidden");

            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showCasinoReport: function (sportId, wldate) {
        if (isAllowReadCasinoReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            CasinoReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromCSR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();

            CasinoReportManager.rightDetailReport.addClass("hidden");
            $("#gridSplitterCSR").addClass("hidden");
            $("#casinoReport").addClass("hidden");

            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showBingoReport: function (sportId, wldate) {
        if (isAllowReadBingoReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();

            $("#dailyWinLossReport").addClass("hidden");

            BingoReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromBGR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();

            $("#gridSplitterCSR").addClass("hidden");
            BingoReportManager.rightDetailReport.addClass("hidden");
            $("#gridSplitterBGR").addClass("hidden");
            $("#bingoReport").addClass("hidden");

            DailyWinLoss2hManager.reloadGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showOtherSportsReport: function (sportId, wldate) {
        if (isAllowReadOtherSportsReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            OtherSportsReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromOSR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();

            OtherSportsReportManager.hideAllReport();

            DailyWinLoss2hManager.reloadGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showFinanceReport: function (sportId, wldate) {
        if (isAllowReadFinanceReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            FinancialReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromFIR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();

            $("#gridSplitter_FIR").addClass("hidden");
            FinancialReportManager.rightDetailReport.addClass("hidden");
            $("#financeReport").addClass("hidden");
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);
        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showVirtualSportsReport: function (sportId, wldate) {
        if (isAllowReadVirtualSportsReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            virtualSportsReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromVSR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.updateShareElement();
            virtualSportsReportManager.hideAllReport();
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    updateShareElement: function () {
        $("#InformationMssg").hide();
        $("#InformationMssg").empty();
        $(".mainreport").css("width", $(window).width() + "px");
    }
}