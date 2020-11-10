Namespace.Register("Fanex.Accounting");

Fanex.Accounting.AutomateTransferList = function () {
    var me = this;

    me.autoTransferListCriteria = {
        userName: '',
        status: '',
        failureReason: 'All',
        pageIndex: 1,
        pageSize: 50
    };

    me.gridOpts = {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 970, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 120,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    };

    me.pagerOpts = {
        callback: null,
        start_per_page: 50,
        step_per_page: 50,
        item_per_page_arr: [50, 100, 150, 200],
        max_items_per_page: 200,
        items_per_page: 50,
        current_page: 1
    };

    me.canTransferUsernames = [];
    me.selectedTransferUsernames = [];
    me.ErrorTransferAllCust = [];
    me.IsFromTransferAll = false;

    me.getAutoTransfers = function (isFromSearchButton) {
        try { parent.BlockUI(); } catch (e) { }
        $("#err").hide();
        me.getParams(isFromSearchButton);
        $.ajax({
            type: "POST",
            url: window.rootUrl + 'AutoTransferAll/GetAutoTransfers',
            data: me.autoTransferListCriteria,
            success: function (result) {
                $("#autoTransferDiv").empty().html(result);

                if (typeof _length != "undefined") {
                    me.gridRender();
                }

                me.InitCanTranferUsername(pageData);
            },
            error: function () {
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    };

    me.gridRender = function () {
        $("#transferGid").initGrid(me.gridOpts);
        $('#transferGid').removeClass("hidden").gridScroller(me.gridOpts);

        me.initPager(_length,
        me.autoTransferListCriteria.pageSize,
        me.autoTransferListCriteria.pageIndex - 1);
    };

    me.initPager = function (total_items, items_per_page, current_page) {
        me.pagerOpts.callback = me.pageSelectCallback;
        me.pagerOpts.items_per_page = items_per_page;
        me.pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, me.pagerOpts);
    };

    me.pageSelectCallback = function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_length / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        me.autoTransferListCriteria.pageIndex = page_index + 1;
        me.autoTransferListCriteria.pageSize = _items_per_page;
        me.getAutoTransfers(false);
        return false;
    };

    me.getParams = function (isFromSearchButton) {
        var username = me.trim($('#txtUsername').val());
        if (isFromSearchButton) {
            me.autoTransferListCriteria.pageIndex = 1;
        }

        me.autoTransferListCriteria.userName = username;
        me.autoTransferListCriteria.status = $('#ddlFilterStatus').val();
        me.autoTransferListCriteria.failureReason = $('#ddlFilterReason').val();
    };

    me.trim = function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    };

    me.confirmClearBalance = function (username, custId) {
        var transfertype = $('#ddlTranferType_' + custId).val() == 1 ? "members" : "downlines";
        $("#tblconfirmUpdated").html(" Are you sure you want to transfer balance for all " + transfertype + " under user " + username + "? ");
        $("#tblconfirmUpdated").dialog({
            title: "Updated Transfer",
            autoOpen: false,
            height: 120,
            width: 300,
            modal: true,
            buttons: {
                "Submit": function () {
                    me.clearBalance(username, custId);
                    $("#tblconfirmUpdated").dialog("close");
                },
                "Cancel": function () {
                    $("#tblconfirmUpdated").dialog("close");
                }
            },
            close: function (event, ui) { }
        });
        $('a.ui-dialog-titlebar-close').show();
        $("#tblconfirmUpdated").dialog("open");
    };

    me.clearBalance = function (username, custId) {
        try { parent.BlockUI(); } catch (e) { }
        var params = { username: username, transferType: $('#ddlTranferType_' + custId).val(), custId: custId };
        $("#err").empty().hide();
        $.ajax({
            type: "POST",
            url: window.rootUrl + 'AutoTransferAll/ClearBalance',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (result) {
                var statusSymbol = params.transferType == 1 ? " (M)" : " (D)";

                if (result.ErrorCode != 0) {
                    $("#err").show().html("<div class='alerterror'>" + result.ErrorMessage + "</div>");
                    $('#status_' + custId).empty().html("failed" + statusSymbol).css('color', "red");
                }
                else {
                    $("#err").show().html("<div class='alertsuccess'>Transfer successfully!</div>");
                    $('#btnTransferAll_' + custId).attr('disabled', 'disabled');
                    $('#status_' + custId).empty().removeClass("red").html("completed" + statusSymbol);
                    setTimeout(function () {
                        $("#err").hide();
                        me.getAutoTransfers(false);
                    }, 3000);
                }

            },
            error: function () {
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    };

    me.updatedTransfer = function (username, custId) {
        try { parent.BlockUI(); } catch (e) { }
        var params = { custId: custId, transferType: $('#ddlTranferType_' + custId).val() };

        var color = $('#ddlTranferType_' + custId).val() == 1 ? "blue" : "green";
        $('#ddlTranferType_' + custId).css('color', color);
        $("#err").empty();
        $.ajax({
            type: "POST",
            url: window.rootUrl + 'AutoTransferAll/UpdatedTransfer',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode != 0) {
                    $("#err").show().html("<div class='alerterror'>Updated transfer for " + username + " failed!</div>");
                }
            },
            error: function () {
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    };

    me.confirmDeletedTransfer = function (username, custId) {
        $("#tblconfirmDeleted").html(" Are you sure you want to remove this agent ?");
        $("#tblconfirmDeleted").dialog({
            title: "Delete Agent" + username,
            autoOpen: false,
            height: 100,
            width: 300,
            modal: true,
            buttons: {
                "Submit": function () {
                    me.deletedTransfer(custId);
                    $("#tblconfirmDeleted").dialog("close");
                },
                "Cancel": function () {
                    $("#tblconfirmDeleted").dialog("close");
                }
            },
            close: function (event, ui) { }
        });
        $('a.ui-dialog-titlebar-close').show();
        $("#tblconfirmDeleted").dialog("open");
    };

    me.deletedTransfer = function (custId) {
        try { parent.BlockUI(); } catch (e) { }
        var params = { custId: custId };
        $("#err").empty();
        $.ajax({
            type: "POST",
            url: window.rootUrl + 'AutoTransferAll/DeletedTransfer',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode != 0) {
                    $("#err").show().html("<div class='alerterror'>" + result.ErrorMessage + "</div>");
                }
                else {
                    $("#err").show().html("<div class='alertsuccess'>Removed agent successfully!</div>");
                }
            },
            error: function () {
                parent.jQuery.unblockUI();
            },
            complete: function () {
                setTimeout(function () {
                    me.getAutoTransfers();
                }, 3000);
                parent.jQuery.unblockUI();
            }
        });
    };

    me.confirmAddNewAgentFromPopup = function () {
        $("#errPopup").empty();
        $('#usernamesTextarea').val('');
        $("#tblconfirm").dialog({
            title: "Add Agent",
            autoOpen: false,
            height: 300,
            width: 500,
            modal: true,
            buttons: {
                "Submit": function () {
                    me.AddNewAgentFromPopup();
                },
                "Cancel": function () {
                    $("#tblconfirm").dialog("close");
                }
            },
            close: function (event, ui) { }
        });
        $('a.ui-dialog-titlebar-close').show();
        $("#tblconfirm").dialog("open");
    };

    me.AddNewAgentFromPopup = function () {
        try { parent.BlockUI(); } catch (e) { }
        var params = { username: $('#usernamesTextarea').val(), transferType: $('#ddlTranferTypePopup').val() };
        
        $.ajax({
            type: "POST",
            url: window.rootUrl + 'AutoTransferAll/InsertAgentsFromPopup',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode != 0) {
                    $("#errPopup").show().html("<div class='alerterror'>" + result.ErrorMessage + "</div>");
                }
                else {
                    $("#errPopup").show().html("<div class='alertsuccess'>Added  agent(s) successfully!</div>");
                    setTimeout(function () {
                        $("#tblconfirm").dialog("close");
                        automateTransferAll.getAutoTransfers();
                    }, 3000);
                }
            },
            error: function () {
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    };

    me.bindEvent = function () {
        $("#btnSearch").unbind("click");
        $("#btnSearch").click(function () {
            automateTransferAll.getAutoTransfers(true);
        });

        $("#btnAddAgent").unbind("click");
        $("#btnAddAgent").click(function () {
            automateTransferAll.confirmAddNewAgentFromPopup();
        });

        $("#usernamesTextarea").keypress(function (event) {
            if (event.which == '13') {
                return false;
            }
        });

        $('.maincontent').delegate('.freegrid tr', 'hover', function (event) {
            var obj = $('*[name=' + $(this).attr('name') + ']');
            if (event.type === 'mouseenter')
                obj.each(function () {
                    $(this).addClass("yellow");
                })
            else
                obj.each(function () {
                    $(this).removeClass("yellow");
                })
        });

        $("#usernamesTextarea").on("paste", function (e) {
            var element = this;
            setTimeout(function () {
                var context = $(element).val();
                context = context.replace(/(\r\n|\n|\r)/gm, " ");
                context = context.replace(/\s+/g, " ");
                $('#usernamesTextarea').empty().val(context);
            }, 100);
        });

        $("#usernamesTextarea").resizable();
    };

    me.confirmTransferAll = function () {
        $("#tblconfirmUpdated").html("Do you want to transfer all selected rows on this page?");
        $("#tblconfirmUpdated").dialog({
            title: "Multiple Transfer All",
            autoOpen: false,
            height: 120,
            width: 300,
            modal: true,
            buttons: {
                "Submit": function () {
                    me.clearAllSelectedBalance();
                    $("#tblconfirmUpdated").dialog("close");
                },
                "Cancel": function () {
                    $("#tblconfirmUpdated").dialog("close");
                }
            },
            close: function (event, ui) { }
        });
        $('a.ui-dialog-titlebar-close').show();
        $("#tblconfirmUpdated").dialog("open");
    };

    me.clearAllSelectedBalance = function () {
        $("#err").empty().hide();
        if (me.selectedTransferUsernames.length > 0) {
            try { parent.BlockUI(); } catch (e) { }
            me.clearAllSelectedBalanceSequentially();
        }
    };

    me.clearAllSelectedBalanceSequentially = function () {
        if (me.selectedTransferUsernames.length > 0) {
            var params = { username: me.selectedTransferUsernames[0].username, transferType: $('#ddlTranferType_' + me.selectedTransferUsernames[0].custId).val(), custId: me.selectedTransferUsernames[0].custId };
            $.ajax({
                type: "POST",
                url: window.rootUrl + 'AutoTransferAll/ClearBalance',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(params),
                success: function (result) {
                    if (result.ErrorCode != 0) {
                        me.ErrorTransferAllCust.push(params);
                    }
                },
                error: function () {
                    me.ErrorUsernames.push(params.username);
                },
                complete: function () {
                    me.selectedTransferUsernames.splice(0, 1);
                    if (me.selectedTransferUsernames.length === 0) {
                        me.IsFromTransferAll = true;
                        me.getAutoTransfers(false);
                    } else {
                        me.clearAllSelectedBalanceSequentially();
                    }
                }
            });
        }
    };

    me.ShowTransferAllResult = function () {
        if (me.ErrorTransferAllCust.length > 0) {
            var usernames = me.ErrorTransferAllCust.map(e => e.username).join(",");
            $("#err").show().html("<div class='alerterror'>Transfer All failed with Usernames: " + usernames + "</div>");
        } else {
            $("#err").show().html("<div class='alertsuccess'>Transfer All completed successfully!</div>");
        }
        me.ErrorTransferAllCust = [];
        me.IsFromTransferAll = false;
    };

    me.InitCanTranferUsername = function (data) {
        me.canTransferUsernames = [];
        me.selectedTransferUsernames = [];
        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                var autoTransfer = data[i];
                if (autoTransfer.StatusName.trim().toLowerCase() === "failed") {
                    me.canTransferUsernames.push({
                        username: autoTransfer.UserName,
                        custId: autoTransfer.CustId
                    });
                }
            }
        }

        me.InitSelectorEvent();
    };

    me.InitSelectorEvent = function () {
        if (me.canTransferUsernames.length > 0) {
            $('#ckbTransferAll').removeClass("hidden");
            $('#ckbTransferAll').change(function () {
                var isChecked = this.checked;
                $('[id^="ckbTransferAll__"]').each(function () {
                    this.checked = isChecked;
                });
                $('#btnTransferAll').attr('disabled', !isChecked);
                if (isChecked) {
                    me.selectedTransferUsernames = me.canTransferUsernames.map((item) => item);
                } else {
                    me.selectedTransferUsernames = [];
                }
            });

            $('[id^="ckbTransferAll__"]').change(function () {
                var isChecked = this.checked;
                if (isChecked) {
                    $('#btnTransferAll').attr('disabled', false);
                }
            });
        } else {
            $('#ckbTransferAll').addClass("hidden");
        }
    };

    me.AddOrRemoveTranferUsernameList = function (username, custId) {
        var isCheck = $('#ckbTransferAll__' + custId).is(':checked');
        for (var i = 0; i < me.selectedTransferUsernames.length; i++) {
            if (me.selectedTransferUsernames[i].username === username) {
                me.selectedTransferUsernames.splice(i, 1);
            }
        }
        if (isCheck) {
            me.selectedTransferUsernames.push({
                username: username,
                custId: custId
            });
        } else {
            $('#ckbTransferAll').prop('checked', false);
        }

        if (me.selectedTransferUsernames.length === 0) {
            $('#ckbTransferAll').prop('checked', false);
            $('#btnTransferAll').attr('disabled', true);
        }

        if (me.canTransferUsernames.length > 0 && me.canTransferUsernames.length === me.selectedTransferUsernames.length) {
            $('#ckbTransferAll').prop('checked', true);
        }
        
    };
};

var automateTransferAll = new Fanex.Accounting.AutomateTransferList();

$(document).ready(function (event) {
    automateTransferAll.getAutoTransfers(true);
    automateTransferAll.bindEvent();
});

