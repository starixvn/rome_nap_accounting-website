Namespace.Register("Fanex.Accounting");

Fanex.Accounting.AutomateTransferList = function () {
    var me = this;

    me.autoTransferListCriteria = {
        userName: '',
        status: '',
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
};

var automateTransferAll = new Fanex.Accounting.AutomateTransferList();

$(document).ready(function (event) {
    automateTransferAll.getAutoTransfers(true);
    automateTransferAll.bindEvent();
});

