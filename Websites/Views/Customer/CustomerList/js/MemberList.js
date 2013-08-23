var MemberList = {
    params: {
        agentId: '',
        pageSize: 50,
        pageIndex: 1,
        type: 0,
        synccasinoUpline: "",
        doubleCommAgent: "",
        synccasinoUpline: false
    },
    paramsUpdate: {
        custDetail: "",
        userId: 0,
        isDoubleComm: false
    },
    gridOption: {
        tblContentId: 'tbMemberList',
        manualCopyHeader: true,  //totalColumnWidth = 1270
        isShowPager: true, //=totalColumnWidth + totalColumn*4 +totalColumn
        fullWidth: true,
        fullHeight: true,
        gridWidth: 1585,
        gridHeight: 40,
        freezeWidth: 350, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 5
    },
    getMemberList: function (isSearch) {
        try { parent.BlockUI(); } catch (e) { }
        MemberList.getParams(isSearch);
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/MemberList",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(MemberList.params),
            success: function (result) {
                if ($.browser.msie) {
                    var ie = parseInt($.browser.version, 10);
                    if (ie == 9) {
                        result = result.replace(expr, '><');
                    }
                }
                MemberList.reCalculateGrid();
                $('#MemberListMain').html(result);
                $('#MemberListgrid').initGrid(MemberList.gridOption);
                MemberList.initPager(_totalChildren, MemberList.params.pageSize, MemberList.params.pageIndex - 1);
                $('#MemberListgrid').removeClass("hidden").gridScroller(MemberList.gridOption);
                $("#MemberListMain").show();
            },
            error: function () {
                $("#MemberListMain").hide();

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = MemberList.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = _totalPages;
        page_index = page_index < max_page ? page_index : max_page - 1;
        MemberList.params.pageIndex = page_index + 1;
        if (_items_per_page != MemberList.params.pageSize) {
            MemberList.params.pageIndex = 1;
        }
        MemberList.params.pageSize = _items_per_page;
        MemberList.getMemberList();
        return false;
    },
    getParams: function (isSearch) {
        MemberList.params.agentId = _agentId;
        if (isSearch) {
            MemberList.params.pageIndex = 1;
        }
        MemberList.params.hr = "";
        var synccasinoUpline = gup("isSynccasino");
        if (synccasinoUpline != "") {
            MemberList.params.synccasinoUpline = "";
        }
        if (_agentId == 0) {
            MemberList.params.doubleCommAgent = "";
        }
        else {
            MemberList.params.doubleCommAgent = _agentDcom.toLowerCase();
        }

        var synccasinoUpline = gup("isSynccasino");
        if (synccasinoUpline != "") {
            MemberList.params.synccasinoUpline = "";
        }
    },
    confirmUpdateSingle: function (isMultiple, id, isAllow) {
        var bCconfirm;
        if (!isMultiple) {
            var s = "'s";
            if (isAllow) {

                bCconfirm = '<tr><td> Are you sure you want to turn ON Double Commission feature for Sportsbook' + s + ' Losing tickets on this customer?</tr></tr>';
            }
            else {
                bCconfirm = '<tr><td> Are you sure you want to turn OFF Double Commission feature for Sportsbook' + s + ' Losing tickets on this customer?</tr></tr>';
            }
            $('#tblconfirm').empty().append(bCconfirm);
            $("#tblconfirm").dialog({
                title: "Edit Single Double Commission for Losing Tickets",
                autoOpen: false,
                height: 180,
                width: 400,
                modal: true,
                buttons: {
                    "OK": function () {
                        MemberList.updateDoubleComm(id, isAllow, isMultiple);
                    }
                },
                close: function (event, ui) {
                }
            });
            $('a.ui-dialog-titlebar-close').show();
            $("#tblconfirm").dialog("open");
        }
        else {
            id = CustomerList.getSelectedID();
            if (!id) return false;
            bCconfirm = '<tr><td><input type="radio" id="rdbAllow" name="type" value="0" checked="checked" /></td><td class="b"> Turn ON Double Commission</td></tr>\
            <td><input type="radio" id="rdbDisAllow" name="type" value="1"/><td class="b"> Turn OFF Double Commission</td></tr>';
            $('#tblconfirm').empty().append(bCconfirm);
            $("#tblconfirm").dialog({
                title: "Edit Multiple Double Commission for Losing Tickets",
                autoOpen: false,
                height: 180,
                width: 400,
                modal: true,
                buttons: {
                    "Submit": function () {
                        var temp = $('input[name=type]:checked').val();
                        if (temp == "0") {
                            MemberList.updateDoubleComm(id, true, isMultiple);
                        }
                        else {
                            MemberList.updateDoubleComm(id, false, isMultiple);
                        }
                    }
                },
                close: function (event, ui) {
                }
            });
            $('a.ui-dialog-titlebar-close').show();
            $("#tblconfirm").dialog("open");
        }
    },
    updateDoubleComm: function (id, isAllow, isMultiple) {
        MemberList.paramsUpdate.userId = _agentId;
            MemberList.paramsUpdate.custDetail = id.toString();
  
        MemberList.paramsUpdate.isDoubleComm = isAllow;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerList/UpdateDoubleComm",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(MemberList.paramsUpdate),
            success: function (ResultCustomerlist) {
                if (ResultCustomerlist && ResultCustomerlist.ErrorCode == 1008) {
                    $('#tblconfirm').empty().append(ResultCustomerlist.ErrorMessage).css({ color: 'red' });
                    $("#tblconfirm").dialog({
                        title: "Confirm Update Fail",
                        autoOpen: false,
                        height: 180,
                        width: 400,
                        modal: true,
                        buttons: {
                            "OK": function () {
                                $(this).dialog("close");
                            }
                        },
                        close: function (event, ui) {
                        }
                    });
                    $('a.ui-dialog-titlebar-close').show();
                    $("#tblconfirm").dialog("open");
                    return;
                }

                if (ResultCustomerlist.length > 0) {
                    var errorFlag = false;
                    var length = ResultCustomerlist.length;
                    var arr = [];
                    for (var i = 0; i < length; i++) {
                        if (ResultCustomerlist[i].Result.ErrorCode != 0) {
                            var error = '<tr><td><span class="username">' + ResultCustomerlist[i].UserName + '</span><span class="fail"> :'+ (ResultCustomerlist[i].Result.ErrorMessage != "" ? ResultCustomerlist[i].Result.ErrorMessage : " Update fail .") +'</span></tr></tr>';
                            arr.push(error)
                            errorFlag = true;
                        }
                    }

                    if (!errorFlag) {
                        setTimeout('$("#tblconfirm").dialog("close")', 2000);
                        var htmlsuccess = '<tr><td><span class="successful">Update successful</span></tr></tr>';
                     
                        var title = "";
                        if (!isMultiple) {
                            title = "Edit Single Double Commission for Losing tickets";
                        }
                        else {
                            title = "Edit Multiple Double Commission for Losing tickets";
                        }

                        $("#tblconfirm").dialog({
                            title: title,
                            autoOpen: false,
                            height: 100,
                            width: 400,
                            modal: true,
                            buttons: {
                                "OK": function () {
                                    $(this).dialog("close");
                                }
                            },
                            close: function (event, ui) {
                                MemberList.getMemberList();
                            }
                        });
                        $('div.ui-dialog-buttonpane').append(htmlsuccess);
                        $('a.ui-dialog-titlebar-close').show();
                        $("#tblconfirm").dialog("open");

                    }
                    else {
                       $('#tblconfirm').empty().append(arr.join(''));
                        $("#tblconfirm").dialog({
                            title: "Confirm Update Fail",
                            autoOpen: false,
                            height: 180,
                            width: 400,
                            modal: true,
                            buttons: {
                                "OK": function () {
                                    $(this).dialog("close");
                                }
                            },
                            close: function (event, ui) {
                            }
                        });
                        
                        $('a.ui-dialog-titlebar-close').show();
                        $("#tblconfirm").dialog("open");
                    }
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    OpenEdit: function (agentid, custid, uname) {
        var urlEdit = "";
        if (_IsShowCustomerSettingRenovation == 3) {
            urlEdit = "EditMember/EditMemberPage";
        }
        else {

            urlEdit = "Customer/v2/EditCustomerNew.aspx";
        }
        var url = urlEdit + "?custid=" + custid + "&m=0&agentId=" + agentid + "&uname=" + uname;
        var pageIndex = "1";
        if (location.href.indexOf("#") > -1) {
            pageIndex = location.href.split("#")[1];
        }
        showPopup("Update Member Setting", url, "", "", 1000, 650, true, 2000, function () { callAjax(getParams(pageIndex)); });
    },
    editSetting: function (custId, roleId) {
        showPopup("Poker"
                , "Customer/PT/Poker/MemberUpdate.aspx?custId=" + custId + "&roleId=" + roleId
                , 0, 0, 640, 480, false, 2000, null);
    },
    liveCasinoSetting: function (custId, exchangeId, uplineId) {
        showPopup("Live Casino"
                , "LiveCasino/PTSetting/EditMember.aspx?custId=" + custId + "&uplineId=" + uplineId
                , 0, 0, 640, 480, false, 2000, null);
    },
    virtualSportsSetting: function (custId, uplineId, roleId) {
        showPopup("Virtual Sport"
                , "VirtualSportMemberPT/EditMember?custId=" + custId + "&uplineId=" + uplineId + "&roleId=" + roleId
                , 0, 0, 860, 480, false, 2000, null);
    },
    reCalculateGrid: function () {
        var initWidth = 1370;
        if (_IsVisibleVirtualRate == "True") {
            initWidth += 65;
        }
        if (_enableNumberGame == "True") {
            initWidth += 75;
        }
        MemberList.gridOption.gridWidth = initWidth;
    }
};


head.ready(function (event) {
    try { parent.BlockUI(); } catch (e) { }
    MemberList.reCalculateGrid();
    $('#MemberListgrid').initGrid(MemberList.gridOption);
    MemberList.initPager(_totalChildren, MemberList.params.pageSize, MemberList.params.pageIndex - 1);
    $('#MemberListgrid').removeClass("hidden").gridScroller(MemberList.gridOption);
    $("#MemberListMain").show();
    try { parent.jQuery.unblockUI(); } catch (e) { }
});
