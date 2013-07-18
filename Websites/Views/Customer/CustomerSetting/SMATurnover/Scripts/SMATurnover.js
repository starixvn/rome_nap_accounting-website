var SMATurnover = {
    constants: {
        CountdownValue: {
            NotRefresh: 0
        },

        MaxPercentageUsed: 100
    },
    cachedData: {
        ddlSport: null,
        ddlLeague: null,
        txtfromdate: null,
        txttodate: null,
        reportParams: null,
        tblSMATurnoverBody: null,
        refreshIntervalCounter: 0,
        refreshIntervalEvent: null,
        sendMailInteralCounter: 10000,
        sendMailIntervalEvent: null,
        lastReport: null,
        reportRows: null,
        updateCountdownEvent: null,
        countdownTime: 0,

        fileUpload: null,
        btnUploadFile: null,
        msgErrorAdd: null,
        ddlSport_AddSMAList: null,
        txtfromdate_AddSMAList: null,
        txttodate_AddSMAList: null,
        btnSearch_AddSMAList: null,
        matchInfo_TableBody: null,
        chbxCheckAllMatch_AddSMAList: null,
        chbxMatchInfoOptions: [],
        selectMatchInfoParams: null,
        isValidFile: false,
        failedRecordsTable: null,
        succeededRecordsTable: null,
        failedRecordsTableBody: null,
        succeededRecordsTableBody: null,
        lnkDownloadTemplate_AddSMAList: null,
        downloadFrm: null,

        btnSubmit_AddSMAList: null,
        btnBackToFilter_AddSMAList: null,

        succeededRecordsTableContainer: null,
        failedRecordsTableContainer: null,
        selectMatchInfoBox: null,
        selectMatchFilterBox: null,
        selectMatchFilter: null,
        uploadFileContainer: null,
        selectMatchInfoBox_Title: null,
        selectMatchFilterBox_Title: null,
        matchInfo_Table: null,
        matchInfo_TableContainer: null,

        _100ReachedBox: null,
        refreshBox: null,
        filter: null
    },
    InitPage: function () {
        var fromdate_opts = { minDate: new Date(balanceUpDay) };
        SMATurnover.RenderCalendarFromTo("#txtfromdate, #txttodate", fromdate_opts);
        SMATurnover.RenderCalendarFromTo_AddSMAList("#txtfromdate_AddSMAList, #txttodate_AddSMAList", fromdate_opts);
        SMATurnover.ChangeTab();
        SMATurnover.RenderSport();
        SMATurnover.RenderLeague("1", currDay, currDay);
        SMATurnover.RenderLeague_AddSMAList({
            sport: "1",
            fromDate: currDay,
            toDate: currDay
        });

        $("#SMATurnover").initGrid({
            isShowPager: false
        });
        $("#SMATurnover").delegate(".MaxTurnover", "keyup", function () {
            var obj = $(this);
            var matchId = obj.attr("matchId");
            var custId = obj.attr("custId");
            var oldValue = obj.attr("oldValue");
            var currValueOriginal = obj.val();
            if (currValueOriginal != "") {
                currValue = Helper.SeparateNumricByThousand(currValueOriginal);
                var formatCurrentValue = formatNumber2(currValue);
                if (formatCurrentValue != "NaN") {
                    obj.val(formatCurrentValue);
                }
            }
            else {
                obj.val('');
            }

            if (formatCurrentValue != oldValue && formatCurrentValue != "NaN" && currValueOriginal != "") {
                $('#btnUpdate_' + matchId + '_' + custId).removeAttr('disabled')
                                                         .attr("title", "Update")
                                                         .css("cursor", "pointer");
            }
            else {
                $('#btnUpdate_' + matchId + '_' + custId).attr('disabled', "disabled")
                                                         .attr("title", "")
                                                         .css("cursor", "default");
            }
        });

        SMATurnover.CacheDOMElement();
        var refreshIcon = $(".refreshBox .refreshIcon");
        refreshIcon.unbind("click");
        refreshIcon.click(function () {
            if (SMATurnover.cachedData.refreshIntervalCounter > 1) {
                var refreshIcon = $(".refreshBox .refreshIcon");
                SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                window.clearInterval(SMATurnover.cachedData.refreshIntervalEvent);
                SMATurnover.cachedData.refreshIntervalEvent = setInterval(function () {
                    SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                    SMATurnover.refreshSMATurnoverList(false);
                }, SMATurnover.cachedData.refreshIntervalCounter);

                refreshIcon.html(SMATurnover.cachedData.countdownTime);
                SMATurnover.refreshSMATurnoverList(false);
            }
        });

        var refreshDropdownIcon = $(".refreshBox .refreshDropdownIcon");
        refreshDropdownIcon.unbind("click");
        refreshDropdownIcon.click(function () {
            SMATurnover.ShowCountDownOptions($(this));
        });

        var countDownOptions = $(".countDownOptionContainer .countDownOption");
        countDownOptions.unbind("click");
        countDownOptions.click(function () {
            SMATurnover.SaveCountDownRefreshOption(this);
        });

        SMATurnover.cachedData.fileUpload.unbind('change');
        SMATurnover.cachedData.fileUpload.change(function () {
            SMATurnover.CheckFormatFile();
        });

        SMATurnover.cachedData.btnUploadFile.unbind('click');
        SMATurnover.cachedData.btnUploadFile.click(function () {
            SMATurnover.uploadSMAList_ByAjaxForm();
        });

        SMATurnover.cachedData.ddlSport_AddSMAList.unbind('change');
        SMATurnover.cachedData.ddlSport_AddSMAList.change(function () {
            SMATurnover.ChangeSport_AddSMAList();
        });

        SMATurnover.cachedData.btnSearch_AddSMAList.unbind("click");
        SMATurnover.cachedData.btnSearch_AddSMAList.click(function () {
            SMATurnover.GetMatchInfo_AddSMAList(this);
        });

        SMATurnover.cachedData.chbxCheckAllMatch_AddSMAList.unbind("click");
        SMATurnover.cachedData.chbxCheckAllMatch_AddSMAList.click(function () {
            var isChecked = $(this).prop("checked") ? true : false;
            SMATurnover.cachedData.chbxMatchInfoOptions.prop("checked", isChecked);
        });

        SMATurnover.cachedData.lnkDownloadTemplate_AddSMAList.unbind("click");
        SMATurnover.cachedData.lnkDownloadTemplate_AddSMAList.click(function () {
            SMATurnover.downloadTemplate_AddSMAList();
        });

        SMATurnover.cachedData.btnSubmit_AddSMAList.unbind("click");
        SMATurnover.cachedData.btnSubmit_AddSMAList.click(function () {
            SMATurnover.AddValidSMAList();
        });

        SMATurnover.cachedData.btnBackToFilter_AddSMAList.unbind("click");
        SMATurnover.cachedData.btnBackToFilter_AddSMAList.click(function () {
            SMATurnover.BackToFilter_AddSMAList();
        });

        SMATurnover.cachedData.selectMatchFilterBox_Title.unbind("click");
        SMATurnover.cachedData.selectMatchFilterBox_Title.click(function () {
            var isShowBox = SMATurnover.cachedData.selectMatchFilter.hasClass("hidden");
            if (isShowBox) {
                $(this).attr("title", "Hide Choose league box.");
                SMATurnover.cachedData.selectMatchFilterBox_TitleIcon.removeClass("collapse")
                                                                     .addClass("expand");
                SMATurnover.cachedData.selectMatchFilter.removeClass("hidden");
            } else {
                $(this).attr("title", "Show Choose league box.");
                SMATurnover.cachedData.selectMatchFilterBox_TitleIcon.removeClass("expand")
                                                                     .addClass("collapse");
                SMATurnover.cachedData.selectMatchFilter.addClass("hidden");
            }
        });

        SMATurnover.cachedData.selectMatchInfoBox_Title.unbind("click");
        SMATurnover.cachedData.selectMatchInfoBox_Title.click(function () {
            var isShowBox = SMATurnover.cachedData.matchInfoContainer.hasClass("hidden");
            if (isShowBox) {
                $(this).attr("title", "Hide Choose match(es) and upload file box.");
                SMATurnover.cachedData.selectMatchInfoBox_TitleIcon.removeClass("collapse")
                                                                   .addClass("expand");
                SMATurnover.cachedData.matchInfoContainer.removeClass("hidden");
                SMATurnover.cachedData.uploadFileContainer.removeClass("hidden");
            } else {
                $(this).attr("title", "Show Choose match(es) and upload file box.");
                SMATurnover.cachedData.selectMatchInfoBox_TitleIcon.removeClass("expand")
                                                                   .addClass("collapse");
                SMATurnover.cachedData.matchInfoContainer.addClass("hidden");
                SMATurnover.cachedData.uploadFileContainer.addClass("hidden");
            }
        });

        SMATurnover.cachedData.matchInfo_TableContainer.initGrid({
            isShowPager: false
        });

        SMATurnover.cachedData.failedRecordsTableContainer.initGrid({
            isShowPager: false
        });

        SMATurnover.cachedData.succeededRecordsTableContainer.initGrid({
            isShowPager: false
        });

        $("#succeededRecordsTable").delegate(".MaxTurnover", "keyup", function () {
            var obj = $(this);
            var oldValue = obj.attr("oldValue");
            var currValueOriginal = obj.val();
            if (currValueOriginal != "") {
                currValue = Helper.SeparateNumricByThousand(currValueOriginal);
                var formatCurrentValue = formatNumber2(currValue);
                if (formatCurrentValue != "NaN") {
                    obj.val(formatCurrentValue);
                }
            }
            else {
                obj.val('');
            }
        });
    },
    BackToFilter_AddSMAList: function () {
        SMATurnover.cachedData.btnSubmit_AddSMAList.addClass("hidden");
        SMATurnover.cachedData.btnBackToFilter_AddSMAList.addClass("hidden");
        SMATurnover.cachedData.failedRecordsTableContainer.addClass("hidden");
        SMATurnover.cachedData.succeededRecordsTableContainer.addClass("hidden");

        SMATurnover.cachedData.selectMatchInfoBox_Title.attr("title", "Hide Choose match(es) and upload file box.");
        SMATurnover.cachedData.selectMatchInfoBox_TitleIcon.removeClass("collapse")
                                                           .addClass("expand");
        SMATurnover.cachedData.matchInfoContainer.removeClass("hidden");
        SMATurnover.cachedData.uploadFileContainer.removeClass("hidden");

        SMATurnover.cachedData.selectMatchFilterBox_Title.attr("title", "Hide Choose league box.");
        SMATurnover.cachedData.selectMatchFilterBox_TitleIcon.removeClass("collapse")
                                                             .addClass("expand");
        SMATurnover.cachedData.selectMatchFilter.removeClass("hidden");

        SMATurnover.cachedData.filter.removeClass("hidden");
    },
    uploadSMAList_ByAjaxForm: function () {
        try { parent.BlockUI(); } catch (e) { }
        //            SMATurnover.UploadSMAList();
        var selectMatchInfoParams = SMATurnover.cachedData.selectMatchInfoParams;
        $("#AccId_UploadFile").val(accId);
        $("#SportId_UploadFile").val(selectMatchInfoParams.SportId);
        $("#LeagueId_UploadFile").val(selectMatchInfoParams.LeagueId);
        $("#FromDate_UploadFile").val(selectMatchInfoParams.FromDate);
        $("#ToDate_UploadFile").val(selectMatchInfoParams.ToDate);
        $("#SportName_UploadFile").val(selectMatchInfoParams.SportName);
        $("#LeagueName_UploadFile").val(selectMatchInfoParams.LeagueName);
        var matchList = [];
        SMATurnover.cachedData.chbxMatchInfoOptions.each(function () {
            var chbx = $(this);
            var isChecked = chbx.prop("checked") ? true : false;
            if (isChecked) {
                matchList.push(chbx.attr("matchId"));
            }
        });

        $("#MatchIds_UploadFile").val(matchList.join(","));
        $('#file_upload_form').ajaxSubmit({
            success: function (rs) {
                if ($.browser.msie) {
                    var prefix = "<pre>";
                    var suffix = "</pre>";
                    if (rs.toLowerCase().indexOf(prefix) == 0) {
                        rs = rs.substring(5, rs.length);
                    }

                    if (rs.toLowerCase().indexOf(suffix) == rs.length - suffix.length) {
                        rs = rs.substring(0, rs.length - suffix.length);
                    }
                }

                var objRs = JSON.parse(rs);
                if (objRs && objRs.ErrorCode > 0) {
                    showAlertMessage("msgErrorAdd", objRs.ErrorMessage, 1);
                    SMATurnover.cachedData.msgErrorAdd.removeClass("hidden");
                } else {
                    SMATurnover.RenderAddResult($.parseJSON(objRs.ActionResult));
                }

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    RenderAddResult: function (addResult) {
        SMATurnover.cachedData.btnSubmit_AddSMAList.removeClass("hidden");
        SMATurnover.cachedData.btnBackToFilter_AddSMAList.removeClass("hidden");
        SMATurnover.cachedData.msgErrorAdd.addClass("hidden");
        if (addResult != null) {
            SMATurnover.cachedData.filter.addClass("hidden");
            if (addResult.SucceededInfoList.length != 0) {
                SMATurnover.cachedData.selectMatchInfoBox_Title.attr("title", "Show Choose match(es) and upload file box.");
                SMATurnover.cachedData.selectMatchInfoBox_TitleIcon.removeClass("expand")
                                                           .addClass("collapse");
                SMATurnover.cachedData.matchInfoContainer.addClass("hidden");
                SMATurnover.cachedData.uploadFileContainer.addClass("hidden");
            }

            if (addResult.FailedInfoList.length != 0 ||
                addResult.SucceededInfoList.length != 0) {
                SMATurnover.cachedData.selectMatchFilterBox_Title.attr("title", "Show Choose league box.");
                SMATurnover.cachedData.selectMatchFilterBox_TitleIcon.removeClass("expand")
                                                             .addClass("collapse");
                SMATurnover.cachedData.selectMatchFilter.addClass("hidden");
            }
        }

        SMATurnover.cachedData.failedRecordsTableContainer.addClass("hidden");
        if (addResult && addResult.FailedInfoList.length) {
            SMATurnover.cachedData.failedRecordsTableContainer.removeClass("hidden");
            var failedInfoLength = addResult.FailedInfoList.length;
            var htmlContent = '';
            for (var i = 0; i < failedInfoLength; i++) {
                var smaInfo = addResult.FailedInfoList[i];
                htmlContent += '<tr>\
                                    <td>' + smaInfo.Username + '</td>\
                                    <td class="r">' + smaInfo.MaxTurnover + '</td>\
                                    <td class="l">' + smaInfo.AddResultMessage + '</td>\
                                </tr>';
            }

            SMATurnover.cachedData.failedRecordsTableBody.empty()
                                                         .append(htmlContent)
                                                         .removeClass("hidden");
            SMATurnover.cachedData.failedRecordsTable.removeClass("hidden");

            var gridOpts = {
                tblContentId: 'failedRecordsTable',
                manualCopyHeader: true,
                fullWidth: true,
                fullHeight: false,
                gridWidth: 900,
                gridHeight: 300,
                freezeWidth: 0,
                numberFreezeCol: 0
            };

            SMATurnover.cachedData.failedRecordsTableContainer.removeClass("hidden").gridScroller(gridOpts);
        }

        SMATurnover.cachedData.succeededRecordsTableContainer.addClass("hidden");
        if (addResult && addResult.SucceededInfoList.length) {
            SMATurnover.cachedData.succeededRecordsTableContainer.removeClass("hidden");
            SMATurnover.cachedData.btnSubmit_AddSMAList.attr("disabled", null);
            var succeededInfoLength = addResult.SucceededInfoList.length;
            htmlContent = '';
            for (var i = 0; i < succeededInfoLength; i++) {
                var succeededMatchInfo = addResult.SucceededInfoList[i];
                var smaListLength = succeededMatchInfo.SucceededInfoList.length;

                htmlContent += '<tr id="succeededSMA_' + succeededMatchInfo.MatchId + '">\
                                    <td class="rowLeague" colspan="3"  style="height:17px">' +
                                        '<div class="lblTeamInfo">' + succeededMatchInfo.HomeTeamName +
                                            ' -vs- ' + succeededMatchInfo.AwayTeamName + '</div>' +
                                        '<div class="rightArrow_LeagueInfo"></div>' +
                                        '<div class="eventDate_LeagueInfo">Event Date: ' + succeededMatchInfo.EventDate + '</div>' +
                                        '<div class="matchIdBox_LeagueInfo">Match ID: ' + succeededMatchInfo.MatchId + '</div>' +
                                    '</td>\
                                </tr>';

                for (var j = 0; j < smaListLength; j++) {
                    var smaInfo = succeededMatchInfo.SucceededInfoList[j];

                    var tbxMaxTurnover = '<div>\
                                                <input type="text" class="MaxTurnover" style="text-align:right; width:157px" maxlength="15" onkeypress="return CheckIntNumber(event)" value="' + smaInfo.MaxTurnover + '" />\
                                          </div>';
                    var btnDeleteSMAInfoRow = '<div>\
                                                <input type="button" onclick="SMATurnover.DeletSMAInfoRow(\'' + succeededMatchInfo.MatchId + '\',\'' + smaInfo.CustId + '\')" class="btnDelete" />\
                                             </div>';
                    htmlContent += '<tr id="succeededSMA_' + succeededMatchInfo.MatchId + '_' +
                                        smaInfo.CustId + '" matchid="' + succeededMatchInfo.MatchId +
                                        '" custid="' + smaInfo.CustId + '" class="succeededSMA">\
                                            <td class="c" style="height:17px">' + smaInfo.Username + '</td>\
                                            <td class="c">' + tbxMaxTurnover + '</td>\
                                            <td class="c">' + btnDeleteSMAInfoRow + '</td>\
                                        </tr>';
                }
            }

            SMATurnover.cachedData.succeededRecordsTableBody.empty()
                                                            .append(htmlContent)
                                                            .removeClass("hidden");
            var gridOpts = {
                tblContentId: 'succeededRecordsTable',
                manualCopyHeader: true,
                fullWidth: true,
                fullHeight: true,
                gridWidth: 877,
                gridHeight: 400,
                freezeWidth: 0,
                numberFreezeCol: 0
            };

            SMATurnover.cachedData.succeededRecordsTableContainer.removeClass("hidden").gridScroller(gridOpts);
        } else {
            SMATurnover.cachedData.succeededRecordsTableContainer.addClass("hidden");
            SMATurnover.cachedData.succeededRecordsTableBody.empty().addClass("hidden");
        }
    },
    DeletSMAInfoRow: function (matchId, custId) {
        var smaInfoRow = $('#succeededSMA_' + matchId + "_" + custId);
        smaInfoRow.remove();
        if ($('.succeededRecordsTable .succeededSMA[id^=succeededSMA_' + matchId + ']').length == 0) {
            var matchHeadline = $('#succeededSMA_' + matchId);
            matchHeadline.remove();
        }

        if (SMATurnover.cachedData.succeededRecordsTableBody.find('tr').length == 0) {
            SMATurnover.cachedData.btnSubmit_AddSMAList.attr("disabled", "disabled");
            SMATurnover.cachedData.succeededRecordsTableContainer.addClass("hidden");
            SMATurnover.cachedData.succeededRecordsTable.addClass("hidden");
        }

        var gridOpts = {
            tblContentId: 'succeededRecordsTable',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridWidth: 877,
            gridHeight: 400,
            freezeWidth: 0,
            numberFreezeCol: 0
        };

        SMATurnover.cachedData.succeededRecordsTableContainer.gridScroller(gridOpts);
    },
    ChangeSport_AddSMAList: function () {
        SMATurnover.RenderLeague_AddSMAList({
            sport: SMATurnover.cachedData.ddlSport_AddSMAList.val(),
            fromDate: SMATurnover.cachedData.txtfromdate_AddSMAList.val(),
            toDate: SMATurnover.cachedData.txttodate_AddSMAList.val()
        });
    },
    RenderLeague_AddSMAList: function (selectParams) {
        $.ajax({
            type: "POST",
            url: "GetLeague",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(selectParams),
            success: function (rs) {
                SMATurnover.cachedData.ddlLeague_AddSMAList.empty();
                SMATurnover.cachedData.ddlLeague_AddSMAList.append('<option value="0">Choose League</option>')
                for (var i = 0; i < rs.length; i++) {
                    var leagueOption = '<option value="' + rs[i].Id + '" title="' + rs[i].Name + '">' + rs[i].Name + '</option>';
                    SMATurnover.cachedData.ddlLeague_AddSMAList.append(leagueOption);
                }
            }
        });
    },
    RenderCalendarFromTo_AddSMAList: function (ids, opts) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate_AddSMAList" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                SMATurnover.ChangeSport_AddSMAList();
            }
        };

        var new_opts = $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids.split(',')[0]).datepicker("setDate", new Date());
        $(ids.split(',')[1]).datepicker("setDate", new Date());
    },
    ValidParams_GetMatchInfo_AddSMAList: function (selectParams) {
        var isValid = true;
        var errorMessage = [];
        if (selectParams.SportId == 0) {
            errorMessage.push("Please choose Sport Game!");
            isValid = false;
        }

        if (selectParams.LeagueId == 0) {
            errorMessage.push("Please choose League!");
            isValid = false;
        }

        if (isValid) {
            SMATurnover.cachedData.msgErrorAdd.html('')
                                              .addClass("hidden");
        } else {
            showAlertMessage("msgErrorAdd", errorMessage.join(" "), 1);
            SMATurnover.cachedData.msgErrorAdd.removeClass("hidden");
        }

        return isValid;
    },
    GetMatchInfo_AddSMAList: function () {
        try { parent.BlockUI(); } catch (e) { }
        var selectParams = {
            SportId: SMATurnover.cachedData.ddlSport_AddSMAList.val(),
            SportName: SMATurnover.cachedData.ddlSport_AddSMAList.find("option:checked").html(),
            LeagueId: SMATurnover.cachedData.ddlLeague_AddSMAList.val(),
            LeagueName: SMATurnover.cachedData.ddlLeague_AddSMAList.find("option:checked").html(),
            FromDate: SMATurnover.cachedData.txtfromdate_AddSMAList.val(),
            ToDate: SMATurnover.cachedData.txttodate_AddSMAList.val()
        };

        if (SMATurnover.ValidParams_GetMatchInfo_AddSMAList(selectParams)) {
            SMATurnover.cachedData.selectMatchInfoParams = selectParams;
            $.ajax({
                type: "POST",
                url: "GetMatchInfo_AddSMAList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(SMATurnover.cachedData.selectMatchInfoParams),
                success: function (rs) {
                    SMATurnover.RenderMatchInfoList_AddSMAList(rs);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        } else {
            SMATurnover.cachedData.matchInfoContainer.addClass("hidden");
            SMATurnover.cachedData.uploadFileContainer.addClass("hidden");
            SMATurnover.cachedData.selectMatchInfoBox.addClass("hidden");
            SMATurnover.cachedData.matchInfo_TableContainer.addClass("hidden");
        
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    },
    RenderMatchInfoList_AddSMAList: function (report) {
        if (report && report.ChildsInfo.length > 0) {
            SMATurnover.cachedData.selectMatchInfoBox_Title.attr("title", "Hide Choose match(es) and upload file box.");
            SMATurnover.cachedData.selectMatchInfoBox_TitleIcon.removeClass("collapse")
                                                               .addClass("expand");
            SMATurnover.cachedData.matchInfoContainer.removeClass("hidden");
            SMATurnover.cachedData.uploadFileContainer.removeClass("hidden");
            SMATurnover.cachedData.chbxCheckAllMatch_AddSMAList.prop("disabled", null)
                                                               .prop("checked", true);
            SMATurnover.cachedData.btnUploadFile.attr('disabled', null);

            SMATurnover.cachedData.selectMatchInfoBox.removeClass("hidden");
            SMATurnover.cachedData.matchInfo_TableContainer.removeClass("hidden");
            var dateGroups = report.ChildsInfo;
            var dateGroupLength = report.ChildsInfo.length;
            var matchInfoOptions = '';
            for (var i = 0; i < dateGroupLength; i++) {
                var dateInfo = report.ChildsInfo[i];
                var infoLength = dateInfo.ChildsInfo.length;
                matchInfoOptions += '<tr class="match-headline">\
                                        <td colspan="2" class="c">' + dateInfo.EventDate + '</td>\
                                     </tr>';
                var j = 0;
                for (; j < infoLength; j++) {
                    var matchInfo = dateInfo.ChildsInfo[j];
                    if (j % 2 == 0) {
                        matchInfoOptions += '<tr>';
                    }

                    var chbxId = 'chbxMatchInfoOptions_AddSMAList_' +
                                    dateInfo.Id + '_' +
                                    matchInfo.LeagueId + '_' +
                                    matchInfo.HomeTeamId + '_' +
                                    matchInfo.AwayTeamId;
                    var teamInfo = matchInfo.HomeTeam + " -vs- " + matchInfo.AwayTeam;
                    matchInfoOptions += '<td class="colmatchInfoOption">\
                                            <input id="' + chbxId + '" class="chbxMatchInfoOption" type="checkbox" checked="checked" matchId="' + matchInfo.Id + '" />\
                                            <label class="lblmatchInfoOption" for="' + chbxId + '">' + teamInfo + '</label>\
                                         </td>';

                    if (j % 2 == 1) {
                        matchInfoOptions += '</tr>';
                    }
                }

                if (j % 2 == 1) {
                    matchInfoOptions += '<td class="colmatchInfoOption"></td>';
                }
            }

            SMATurnover.cachedData.matchInfo_TableBody.empty()
                                                      .append(matchInfoOptions);
            SMATurnover.cachedData.chbxMatchInfoOptions = SMATurnover.cachedData.matchInfo_TableBody.find(".chbxMatchInfoOption");
            SMATurnover.cachedData.chbxMatchInfoOptions.unbind("click");
            SMATurnover.cachedData.chbxMatchInfoOptions.click(function () {
                var isChecked = $(this).prop("checked") ? true : false;
                var numberOfCheckedBox = 0;
                SMATurnover.cachedData.chbxMatchInfoOptions.each(function () {
                    var isChecked = $(this).prop("checked") ? true : false;
                    if (isChecked) {
                        numberOfCheckedBox++;
                    }
                });

                var isCheckAll = isChecked &&
                                 numberOfCheckedBox > 0 &&
                                 SMATurnover.cachedData.chbxMatchInfoOptions.length == numberOfCheckedBox;
                SMATurnover.cachedData.chbxCheckAllMatch_AddSMAList.prop("checked", isCheckAll);
            });

            var gridOpts = {
                tblContentId: 'matchInfo_Table',
                manualCopyHeader: true,
                fullWidth: true,
                fullHeight: true,
                gridWidth: 877,
                gridHeight: 300,
                freezeWidth: 0,
                numberFreezeCol: 0
            };

            SMATurnover.cachedData.matchInfo_TableContainer.gridScroller(gridOpts);
        } else {
            SMATurnover.cachedData.matchInfo_TableBody.empty();
            SMATurnover.cachedData.selectMatchInfoBox.addClass("hidden");
            SMATurnover.cachedData.chbxCheckAllMatch_AddSMAList.prop("disabled", "disabled")
                                                               .prop("checked", false);

            showAlertMessage("msgErrorAdd", "There is no match info.", 1);
            SMATurnover.cachedData.msgErrorAdd.removeClass("hidden");
        }
    },
    downloadTemplate_AddSMAList: function () {
        var getParams = {
            IsExport: true
        };

        var url = 'Index?' + $.param(getParams);
        SMATurnover.cachedData.downloadFrm.attr('src', url);
    },
    ShowCountDownOptions: function (btnDown) {
        var pnlDown = $('.countDownOptionContainer');
        var arrBtnPos = SMATurnover.FindPosition(btnDown.parent());
        if (pnlDown.css("display") == 'none') {
            pnlDown.css("left", arrBtnPos[0] + 'px');
            pnlDown.css("top", (arrBtnPos[1] + 20) + 'px');
            pnlDown.css("display", 'inline-block');
        }
        else {
            pnlDown.css("display", 'none');
        }
    },
    CacheDOMElement: function () {
        SMATurnover.cachedData.tblSMATurnoverBody = $("#tblSMATurnover tbody");
        SMATurnover.cachedData.ddlSport = $('#ddlSport');
        SMATurnover.cachedData.ddlLeague = $('#ddlLeague');
        SMATurnover.cachedData.txtfromdate = $('#txtfromdate');
        SMATurnover.cachedData.txttodate = $('#txttodate');

        SMATurnover.cachedData.fileUpload = $('#fileUpload');
        SMATurnover.cachedData.btnUploadFile = $('#btnUploadFile');
        SMATurnover.cachedData.msgErrorAdd = $('#msgErrorAdd');
        SMATurnover.cachedData.ddlSport_AddSMAList = $('#ddlSport_AddSMAList');
        SMATurnover.cachedData.ddlLeague_AddSMAList = $('#ddlLeague_AddSMAList');
        SMATurnover.cachedData.txtfromdate_AddSMAList = $('#txtfromdate_AddSMAList');
        SMATurnover.cachedData.txttodate_AddSMAList = $('#txttodate_AddSMAList');
        SMATurnover.cachedData.btnSearch_AddSMAList = $('#btnSearch_AddSMAList');

        SMATurnover.cachedData.matchInfo_Table = $('.matchInfo_Table');
        SMATurnover.cachedData.matchInfo_TableBody = SMATurnover.cachedData.matchInfo_Table.find('tbody');
        SMATurnover.cachedData.chbxCheckAllMatch_AddSMAList = $('#chbxCheckAllMatch_AddSMAList');

        SMATurnover.cachedData.failedRecordsTable = $('.failedRecordsTable');
        SMATurnover.cachedData.failedRecordsTableBody = SMATurnover.cachedData.failedRecordsTable.find("tbody");
        SMATurnover.cachedData.succeededRecordsTable = $('.succeededRecordsTable');
        SMATurnover.cachedData.succeededRecordsTableBody = SMATurnover.cachedData.succeededRecordsTable.find("tbody");

        SMATurnover.cachedData.lnkDownloadTemplate_AddSMAList = $('.lnkDownloadTemplate_AddSMAList');
        SMATurnover.cachedData.downloadFrm = $('#downloadFrm');
        SMATurnover.cachedData.btnSubmit_AddSMAList = $('#btnSubmit_AddSMAList');
        SMATurnover.cachedData.btnBackToFilter_AddSMAList = $('#btnBackToFilter_AddSMAList');

        SMATurnover.cachedData.matchInfo_TableContainer = $('.matchInfo_TableContainer');
        SMATurnover.cachedData.succeededRecordsTableContainer = $('.succeededRecordsTableContainer');
        SMATurnover.cachedData.failedRecordsTableContainer = $('.failedRecordsTableContainer');
        SMATurnover.cachedData.selectMatchInfoBox = $('.selectMatchInfoBox');
        SMATurnover.cachedData.selectMatchFilterBox = $('.selectMatchFilterBox');
        SMATurnover.cachedData.matchInfoContainer = $('.matchInfoContainer');

        SMATurnover.cachedData.selectMatchInfoBox_Title = SMATurnover.cachedData.selectMatchInfoBox.find('.showHideBox');
        SMATurnover.cachedData.selectMatchFilterBox_Title = SMATurnover.cachedData.selectMatchFilterBox.find('.showHideBox');
        SMATurnover.cachedData.selectMatchInfoBox_TitleIcon = SMATurnover.cachedData.selectMatchInfoBox_Title.find('.title-icon');
        SMATurnover.cachedData.selectMatchFilterBox_TitleIcon = SMATurnover.cachedData.selectMatchFilterBox_Title.find('.title-icon');

        SMATurnover.cachedData.selectMatchFilter = $('.selectMatchFilter');
        SMATurnover.cachedData.uploadFileContainer = $('.uploadFileContainer');

        SMATurnover.cachedData._100ReachedBox = $('._100ReachedBox');
        SMATurnover.cachedData.refreshBox = $('.refreshBox');
        SMATurnover.cachedData.filter = $('.filter');
    },
    getValidSMAListInfo: function () {
        var succeededSMAList = $(".succeededSMA");
        var succeededSMAListLength = succeededSMAList.length;
        var validSMAListParams = [];
        for (var i = 0; i < succeededSMAListLength; i++) {
            var smaInfo = succeededSMAList.eq(i);
            validSMAListParams.push({
                CustId: smaInfo.attr("custid"),
                MatchId: smaInfo.attr("matchid"),
                MaxTurnover: smaInfo.find(".MaxTurnover").val()
            });
        }

        return validSMAListParams;
    },
    validAddParams: function (addParams) {
        var addParamsLength = addParams.length;
        var isValid = true;
        for (var i = 0; i < addParamsLength; i++) {
            var smaInfo = addParams[i];
            var maxTurnover = addParams[i].MaxTurnover.replace(/,/g, '');
            if (parseFloat(maxTurnover) != maxTurnover) {
                isValid = false;
                break;
            }
        }

        if (isValid) {
            SMATurnover.cachedData.msgErrorAdd.addClass("hidden");
        } else {
            showAlertMessage("msgErrorAdd", "Please review all Max Turnover values!", 1);
            SMATurnover.cachedData.msgErrorAdd.removeClass("hidden");
        }

        return isValid;
    },
    AddValidSMAList: function () {
        try { parent.BlockUI(); } catch (e) { }
        var validSMAListParams = SMATurnover.getValidSMAListInfo();

        if (SMATurnover.validAddParams(validSMAListParams)) {
            $.ajax({
                type: "POST",
                url: "AddValidSMAList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(validSMAListParams),
                success: function (rs) {
                    if (rs && rs.ErrorCode > 0) {
                        showAlertMessage("msgErrorAdd", rs.ErrorMessage, 1);
                        SMATurnover.cachedData.msgErrorAdd.removeClass("hidden");
                    } else {
                        showAlertMessage("msgErrorAdd", "Add SMA List successfully!", 0);
                        SMATurnover.cachedData.msgErrorAdd.removeClass("hidden");
                        setTimeout(function () {
                            SMATurnover.cachedData.msgErrorAdd.addClass("hidden");
                        }, 4000);
                    }

                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        } else {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    },
    FindPosition: function (oElement) {
        if (typeof (oElement.offsetParent()) != 'undefined') {
            for (var posX = 0, posY = 0; oElement && oElement.length; oElement = oElement.offsetParent()) {
                posX += oElement.offset().left;
                posY += oElement.offset().top;
                if (oElement[0].nodeName.toLowerCase() == 'body') {
                    break;
                }
            }

            return [posX, posY];
        } else {
            return [oElement.x, oElement.y];
        }
    },
    SaveCountDownRefreshOption: function (btnCountDownOption) {
        btnCountDownOption = $(btnCountDownOption);
        $(".countDownOption.countDownOptionCheck").removeClass("countDownOptionCheck");
        btnCountDownOption.addClass("countDownOptionCheck");
        setTimeout(function () {
            $('.countDownOptionContainer').css("display", 'none');
            var refreshIcon = $(".refreshBox .refreshIcon");
            var coundownAfter = btnCountDownOption.attr("val");
            if (coundownAfter == "Not Refresh") {
                refreshIcon.css("text-align", 'left');
                refreshIcon.css("padding-left", '8px');
                refreshIcon.css("width", '85px');
                refreshIcon.html(coundownAfter);

                SMATurnover.cachedData.refreshIntervalCounter = 0;
                if (SMATurnover.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(SMATurnover.cachedData.refreshIntervalEvent);
                    window.clearInterval(SMATurnover.cachedData.updateCountdownEvent);
                }
            }
            else {
                refreshIcon.css("text-align", 'center');
                refreshIcon.css("padding-left", '0');
                refreshIcon.css("width", '93px');
                refreshIcon.html(coundownAfter);

                if (SMATurnover.cachedData.refreshIntervalEvent > 0) {
                    window.clearInterval(SMATurnover.cachedData.refreshIntervalEvent);
                    window.clearInterval(SMATurnover.cachedData.updateCountdownEvent);
                }

                SMATurnover.cachedData.refreshIntervalCounter = parseInt(coundownAfter * 1000);
                SMATurnover.cachedData.countdownTime = parseInt(coundownAfter);
                SMATurnover.cachedData.updateCountdownEvent = setInterval(function () {
                    if (SMATurnover.cachedData.countdownTime > 1) {
                        SMATurnover.cachedData.countdownTime--;
                    } else {
                        SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                    }

                    refreshIcon.html(SMATurnover.cachedData.countdownTime);
                }, 1000);
                SMATurnover.cachedData.refreshIntervalEvent = setInterval(function () {
                    SMATurnover.cachedData.countdownTime = SMATurnover.cachedData.refreshIntervalCounter / 1000;
                    SMATurnover.refreshSMATurnoverList(false);
                }, SMATurnover.cachedData.refreshIntervalCounter);
            }

        }, 200);
    },
    RenderCalendarFromTo: function (ids, opts) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                SMATurnover.ChangeSport();
            }
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $(ids).datepicker(new_opts);
        $(ids.split(',')[0]).datepicker("setDate", new Date());
        $(ids.split(',')[1]).datepicker("setDate", new Date());
    },
    ChangeTab: function () {
        $('#ListSMATab').click(function () {
            $(this).attr('class', 'current');
            $('#AddSMATab').removeAttr('class');
            $('#ListSMAContent').show();
            $('#AddSMAContent').hide();
        });

        $('#AddSMATab').click(function () {
            $(this).attr('class', 'current');
            $('#ListSMATab').removeAttr('class');
            $('#ListSMAContent').hide();
            $('#AddSMAContent').show();
            $('#txtMatchId').val('');
            $('#txtLeague').val('');
            $('#txtEventDate').val('');
            $('#txtTeam').val('');
            $('#txtSuperName').val('');
            $('#txtTurnover').val('');
            $('#msgErrorAdd').addClass("hidden");
        });
    },

    RenderSport: function () {
        var data = { mode: "0" };
        $.ajax({
            type: "POST",
            url: "GetSportType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var idSport = $('#ddlSport');
                SMATurnover.cachedData.ddlSport_AddSMAList.empty();
                for (var i = 0; i < rs.length; i++) {
                    var sportOption = '<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>';
                    idSport.append(sportOption);
                    SMATurnover.cachedData.ddlSport_AddSMAList.append(sportOption);
                }
            }
        });
    },

    RenderLeague: function (sportId, fromDate, toDate) {
        var data = { sport: sportId, fromDate: fromDate, toDate: toDate };
        $.ajax({
            type: "POST",
            url: "GetLeague",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                var idLeague = $('#ddlLeague');
                idLeague.append('<option value="0">All Leagues</option>')
                for (var i = 0; i < rs.length; i++) {
                    idLeague.append('<option value="' + rs[i].Id + '" title="' + rs[i].Name + '">' + rs[i].Name + '</option>')
                }
            }
        });
    },

    ChangeSport: function () {
        $('#ddlLeague').empty();
        var sportId = $('#ddlSport').val();
        var fromDate = $('#txtfromdate').val();
        var toDate = $('#txttodate').val();
        SMATurnover.RenderLeague(sportId, fromDate, toDate);
    },

    GetInfoByMatch: function () {
        try { parent.BlockUI(); } catch (e) { }
        $('#msgErrorAdd').addClass("hidden");
        var matchId = $('#txtMatchId').val();
        if (matchId == "") {
            $('#msgErrorAdd').removeClass("hidden");
            showAlertMessage("msgErrorAdd", "Please input matchId !", 1);
        }
        else {
            var data = { matchId: matchId };
            $.ajax({
                type: "POST",
                url: "GetInfoByMatchId",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (rs != null) {
                        if (rs.ErrorCode != 0) {
                            $('#msgErrorAdd').removeClass("hidden");
                            showAlertMessage("msgErrorAdd", rs.ErrorMessage, 1);
                            $('#txtLeague').val('');
                            $('#txtEventDate').val('');
                            $('#txtTeam').val('');
                        }
                        else {
                            $('#msgErrorAdd').addClass("hidden");
                            $('#txtLeague').val(rs.LeagueName);
                            $('#txtEventDate').val(rs.EventDate);
                            $('#txtTeam').val(rs.HomeTeam + ' -vs- ' + rs.AwayTeam);
                        }
                    }

                }
            });
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    getReportParams: function (getNewParams) {
        if (getNewParams || SMATurnover.cachedData.reportParams == null) {
            SMATurnover.cachedData.reportParams = {
                sportId: SMATurnover.cachedData.ddlSport.val(),
                leagueId: SMATurnover.cachedData.ddlLeague.val(),
                fromDate: SMATurnover.cachedData.txtfromdate.val(),
                toDate: SMATurnover.cachedData.txttodate.val()
            };
        }
    },
    GetOldInfo: function (matchId, custId) {
        var report = SMATurnover.cachedData.lastReport;
        var reportLength = report.length;
        for (var i = 0; i < reportLength; i++) {
            var row = report[i];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var j = 0; j < matchLength; j++) {
                var match = matchList[j];
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var k = 0; k < customerLength; k++) {
                    var customer = customerList[k];
                    if (matchId == match.MatchId &&
                        custId == customer.CustId) {
                        return {
                            oldMatch: match,
                            oldCustomer: customer
                        };
                    }
                }
            }
        }

        return null;
    },
    isChangeData: function (oldData, newData) {
    },
    refreshSMATurnoverList: function (getNewParams) {
        try { parent.BlockUI(); } catch (e) { }
        SMATurnover.getReportParams(getNewParams);

        $.ajax({
            type: "POST",
            url: "GetSMATurnoverList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(SMATurnover.cachedData.reportParams),
            success: function (rs) {
                SMATurnover.RenderReport(rs);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    RenderReport: function (report) {
        var arr = [];
        if (report != null && report.length > 0) {
            SMATurnover.cachedData.tblSMATurnoverBody.empty().append(SMATurnover.renderRow(report));
        }
        else {
            SMATurnover.cachedData.tblSMATurnoverBody.empty().append('<tr><td colspan="6"><div class="c">There is no data.</div></td></tr>');
        }

        SMATurnover.initGrid();
    },
    GetSMATurnoverList: function () {
        try { parent.BlockUI(); } catch (e) { }
        SMATurnover.cachedData.reportParams = {
            sportId: $('#ddlSport').val(),
            leagueId: $('#ddlLeague').val(),
            fromDate: $('#txtfromdate').val(),
            toDate: $('#txttodate').val()
        };

        $.ajax({
            type: "POST",
            url: "GetSMATurnoverList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(SMATurnover.cachedData.reportParams),
            success: function (rs) {
                SMATurnover.cachedData._100ReachedBox.removeClass("hidden");
                SMATurnover.cachedData.refreshBox.removeClass("hidden");

                SMATurnover.cachedData.lastReport = rs;
                var arr = [];
                SMATurnover.cachedData.tblSMATurnoverBody = $("#tblSMATurnover tbody");
                if (rs != null && rs.length > 0) {
                    //$("#tblContentHeader tbody").empty().append(SMATurnover.renderFrozenRow(rs));
                    SMATurnover.cachedData.tblSMATurnoverBody.empty().append(SMATurnover.renderRow(rs));
                    SMATurnover.cachedData.reportRows = SMATurnover.cachedData.tblSMATurnoverBody.find(">tr");
                }
                else {
                    //$("#tblContentHeader tbody").empty().append('<tr><td><div class="">&nbsp;</div></td></tr>');
                    //$("#tblSMATurnover tbody").empty().append('<tr><td><div class="">&nbsp;</div></td><td colspan="4"><div class="c">There is no data.</div></td></tr>');
                    SMATurnover.cachedData.tblSMATurnoverBody.empty().append('<tr><td colspan="6"><div class="c">There is no data.</div></td></tr>');
                    $(".lbl100ReachedCounter").html(0);
                }

                SMATurnover.initGrid();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initGrid: function () {
        var gridOpts = {
            tblContentId: 'tblSMATurnover',
            manualCopyHeader: true,
            fullWidth: true,
            gridWidth: 830, //=totalColumnWidth + totalColumn*4 +totalColumn
            gridHeight: 200,
            freezeWidth: 0,
            numberFreezeCol: 0
        };

        $("#SMATurnover").removeClass("hidden").gridScroller(gridOpts);
        // SMATurnover.makeTwoTableSameHeight($("#SMATurnover table.row_header_content"), $("#SMATurnover table.col_header_content"), false, true);
    },

    renderRow: function (result) {
        var html = '';
        var _100PercentageUsedCounter = 0;
        var isChanged = 0;

        for (var index = 0; index < result.length; index++) {
            var row = result[index];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var i = 0; i < matchLength; i++) {
                var match = matchList[i];
                html += '<tr>';
                html += '<td class="rowLeague" colspan="6">\
                            <div class="lblLeagueHeadline">' + row.LeagueName + '</div>' +
                            '<div class="lblTeamInfo">' + match.HomeTeam + ' -vs- ' + match.AwayTeam + '</div>' +
                            '<div class="rightArrow_LeagueInfo"></div>' +
                            '<div class="eventDate_LeagueInfo">Event Date: ' + match.EventDate + '</div>' +
                            '<div class="matchIdBox_LeagueInfo">Match ID: ' + match.MatchId + '</div>' +
                         '</td>';
                html += '</tr>';
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var j = 0; j < customerLength; j++) {
                    var customer = customerList[j];
                    var percentageUsed = customer.PercentageUsed;
                    if (percentageUsed >= SMATurnover.constants.MaxPercentageUsed) {
                        _100PercentageUsedCounter++;
                        percentageUsed = SMATurnover.constants.MaxPercentageUsed;
                    }

                    html += '<tr class="match_' + customer.CustId + '_' + match.MatchId + '">';
                    html += '<td><div class="col200 c">' + customer.Username + '</div></td>';
                    html += '<td><div class="col100 c ">' + customer.Currency + '</div></td>';
                    html += '<td><div class="col160 r colSMATurnover">' + customer.SMATurnover + '</div></td>';
                    if (match.EventStatus != "completed") {
                        html += '<td><div class="col160 table-cell" style="height:17px"><input type="text" class="MaxTurnover" id="txtMaxTurnover_' + match.MatchId + "_" + customer.CustId + '" matchId="' + match.MatchId + '" custId="' + customer.CustId + '" oldValue="' + customer.MaxTurnover + '" value="' + customer.MaxTurnover + '" style="text-align:right; width:157px" maxlength="15" onkeypress="return CheckIntNumber(event)"></div></td>';
                    }
                    else {
                        html += '<td><div class="col160 r" style="height:17px">' + customer.MaxTurnover + '</div></td>';
                    }

                    if (percentageUsed == SMATurnover.constants.MaxPercentageUsed) {
                        html += '<td class="c colPercentUsed">\
                                    <div class="col160 c boxPercentUsed_100Percent ">' + percentageUsed + '%</div>\
                                 </td>';
                    } else {
                        html += '<td class="c colPercentUsed">\
                                    <div class="col160 c boxPercentUsed_Low" style="background:url(\'../Views/Customer/CustomerSetting/SMATurnover/Style/Images/MaxPercentStatus.png\') no-repeat scroll -' + (100 - percentageUsed) + 'px 100% transparent">' + percentageUsed + '%</div>\
                                 </td>';
                    }

                    html += '<td><div class="col60 table-cell c" style="height:17px"><input type="button" disabled="disabled" id="btnUpdate_' + match.MatchId + "_" + customer.CustId + '" onclick="SMATurnover.UpdateSMATurnoverProccess(\'' + customer.CustId + '\',\'' + match.MatchId + '\',\'' + customer.Username + '\')" class="btnSave" style="cursor: default;"/></div></td>';
                    html += '</tr>';
                }
            }
        }

        $(".lbl100ReachedCounter").html(_100PercentageUsedCounter);
        return html;
    },

    renderFrozenRow: function (result) {
        var html = '';
        for (var index = 0; index < result.length; index++) {
            var row = result[index];
            var matchLength = row.CustomerList.length;
            var matchList = row.CustomerList;
            for (var i = 0; i < matchLength; i++) {
                var match = matchList[i]
                html += '<tr>';
                //html += '<td colspan="5" style="background-color:#D9E9FD; font-weight:bold; height:17px"><span style="width:200px">' + row.LeagueName + '</span><span style="width:250px">MatchID: ' + match.MatchId + ' (' + match.EventDate + ')</span><span style="width:170px">' + match.HomeTeam + ' -vs-' + match.AwayTeam + '</span><span style="">Add SMA</span></td>';
                html += '<td class="rowLeague"><div class="col200" >' + row.LeagueName + '</div></td>';

                html += '</tr>';
                var customerLength = match.CustomerList.length;
                var customerList = match.CustomerList;
                for (var j = 0; j < customerLength; j++) {
                    var customer = customerList[j];
                    html += '<tr>';
                    html += '<td><div class="col200 c">' + customer.Username + '</div></td>';
                    html += '</tr>';
                }
            }
        }
        return html;
    },

    RedirectToAddSMA: function (matchId) {
        $('#ListSMAContent').hide();
        $('#AddSMAContent').show();
        $('#ListSMATab').removeAttr('class');
        $('#AddSMATab').attr('class', 'current');
        $('#txtMatchId').val(matchId);
        $('#txtSuperName').val('');
        $('#txtTurnover').val('');
        SMATurnover.GetInfoByMatch();
    },

    AddSMATurnoverProccess: function () {
        var matchId = $('#txtMatchId').val();
        var custName = $('#txtSuperName').val();
        var maxTurnover = $('#txtTurnover').val();
        try { parent.BlockUI(); } catch (e) { }
        var maxturnover = Helper.SeparateNumricByThousand($('#txtTurnover').val());
        if (parseInt(maxturnover) > 999999999999) {
            showAlertMessage("msgErrorAdd", "Max Turnover should be less than or equal 999,999,999,999", 1);
            $('#msgErrorAdd').removeClass("hidden");
        }
        else {
            var data = { matchId: matchId, custName: custName, maxTurnover: maxTurnover, mode: 0 };
            $.ajax({
                type: "POST",
                url: "CheckMaxTurnoverValue",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (parseInt(rs.ErrorCode) == 0) {
                        SMATurnover.AddSuperTurnoverValue(matchId, custName, maxTurnover);
                    }
                    else if (parseInt(rs.ErrorCode) == 100) {
                        showAlertMessage("msgErrorAdd", rs.ErrorMessage, 1);
                        $('#msgErrorAdd').removeClass("hidden");
                    }
                    else {
                        if (!confirm(rs.ErrorMessage)) {
                            try { parent.jQuery.unblockUI(); } catch (e) { }
                            return;
                        }
                        else {
                            SMATurnover.AddSuperTurnoverValue(matchId, custName, maxTurnover);
                        }
                    }

                }
            });
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    AddSuperTurnoverValue: function (matchId, custName, maxTurnover) {
        var data = { matchId: matchId, custName: custName, maxTurnover: maxTurnover };
        $.ajax({
            type: "POST",
            url: "AddSMATurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (parseInt(rs.ErrorCode) == 0) {
                    showAlertMessage("msgErrorAdd", "Add successfully!", 0);
                    $('#txtSuperName').val('');
                    $('#txtTurnover').val('');
                }
                else {
                    showAlertMessage("msgErrorAdd", rs.ErrorMessage, 1);
                }
                $('#msgErrorAdd').removeClass("hidden");
            }
        });
    },

    UpdateSMATurnoverProccess: function (custId, matchId, custName) {
        var maxTurnover = $("#txtMaxTurnover_" + matchId + "_" + custId).val();
        try { parent.BlockUI(); } catch (e) { }
        var maxturnover = Helper.SeparateNumricByThousand(maxTurnover);
        if (parseInt(maxturnover) > 999999999999) {
            showAlertMessage("msgError", "Max Turnover should be less than or equal 999,999,999,999", 1);
            $('#msgError').show();
        }
        else {
            var dataCheckSuperTurnoverValue = { matchId: matchId, custName: custName, maxTurnover: maxTurnover, mode: 1 };
            $.ajax({
                type: "POST",
                url: "CheckMaxTurnoverValue",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(dataCheckSuperTurnoverValue),
                success: function (rs) {
                    if (parseInt(rs.ErrorCode) == 0) {
                        SMATurnover.UpdateSuperTurnoverValue(matchId, custId, maxTurnover);
                    }
                    else if (parseInt(rs.ErrorCode) == 100) {
                        showAlertMessage("msgError", rs.ErrorMessage, 1);
                        $('#msgError').show();
                    }
                    else {
                        if (!confirm(rs.ErrorMessage)) {
                            try { parent.jQuery.unblockUI(); } catch (e) { }
                            return;
                        }
                        else {
                            SMATurnover.UpdateSuperTurnoverValue(matchId, custId, maxTurnover);
                        }

                    }
                }
            });
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    UpdateSuperTurnoverValue: function (matchId, custId, maxTurnover) {
        var data = { matchId: matchId, custId: custId, maxTurnover: maxTurnover };
        $.ajax({
            type: "POST",
            url: "UpdateSMATurnover",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify(data),
            success: function (rs) {
                if (parseInt(rs.ErrorCode) == 0) {
                    showAlertMessage("msgError", "Update successfully!", 0);
                    $('#msgError').show();
                    setTimeout(function () { $('#msgError').hide(); }, 4000);
                }
                else {
                    showAlertMessage("msgError", rs.ErrorMessage, 1);
                    $('#msgError').show();
                }

                SMATurnover.GetSMATurnoverList();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    makeTwoTableSameHeight: function (tableA, tableB, isFormatHeader, isFormatBody) {
        if (isFormatBody) {
            var rows_TableA = tableA.find("> tbody > tr");
            var rows_TableB = tableB.find("> tbody > tr");
            var minRowLength = rows_TableA.length < rows_TableB.length ? rows_TableA.length : rows_TableB.length;
            for (var i = 0; i < minRowLength; i++) {
                var columns_TableA = $(rows_TableA[i]).find("td");
                var columns_TableB = $(rows_TableB[i]).find("td");
                var minColumnLength = columns_TableA.length < columns_TableB.length ? columns_TableA.length : columns_TableB.length;
                if (minColumnLength > 0) {
                    var height_TableA = $(columns_TableA[0]).height();
                    var height_TableB = $(columns_TableB[0]).height();
                    var maxHeight = height_TableA > height_TableB ? height_TableA : height_TableB;

                    if (height_TableA != height_TableB) {
                        $(columns_TableA[0]).css("height", maxHeight + 'px');
                        $(columns_TableB[0]).css("height", maxHeight + 'px');

                        if ($(columns_TableA[0]).hasClass("rowLeague") &&
                            $.browser.mozilla) {
                            $(columns_TableA[0]).css("height", maxHeight + 5 + 'px');
                        }
                    }
                }
            }
        }
    },

    InputMaxTurnover: function () {
        // var maxTurnover = $('#txtTurnover').val();
        var maxturnover = Helper.SeparateNumricByThousand($('#txtTurnover').val());
        if (maxturnover != "") {
            $('#txtTurnover').val(formatNumber2(maxturnover));
        }
    }
}

