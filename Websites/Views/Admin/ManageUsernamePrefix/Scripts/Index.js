var ManageUsernamePrefix = (function ($) {
    "use strict";
    var queryParams, deleteParams;

    $.ajaxSetup({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            parent.jQuery.blockUI();
        },
        error: function (xhr, status, error) {
            alert(error);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });

    var api = {
        getGroups: function (callback) {
            $.ajax({
                url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/GetGroupNames",
                success: function (data) {
                    callback(data);
                }
            });
        },
        query: function (data, callback) {
            $.ajax({
                url: "Query",
                data: JSON.stringify(data),
                success: function (data) {
                    callback(data);
                }
            });
        },
        insert: function (data, callback) {
            $.ajax({
                url: "Insert",
                data: JSON.stringify(data),
                success: function (data) {
                    callback(data);
                }
            });
        },
        delete: function (data, callback) {
            $.ajax({
                url: "Delete",
                data: JSON.stringify(data),
                success: function (data) {
                    callback(data);
                }
            });
        }
    }

    function init() {
        $('#btnSearch').click(search);
        $('#btnAdd').click(openAddDialog);
        $('#btnSubmitAdd').click(submitAdd);
        $('#btnYesDelete').click(remove);
        $('#btnNoDelete').click(closeDeleteDialog);
        loadGroups();
    }

    function getFormData($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};

        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = $.trim(n['value']);
        });

        return indexed_array;
    }

    function loadGroups() {
        api.getGroups(function (groups) {
            $('#GroupName').html(generateGroupSelectHtml(groups, "All"));
            $('#GroupName_AddNew').html(generateGroupSelectHtml(groups, "Please select"));
            search();
        });
    }

    function generateGroupSelectHtml(groups, firstOptionText) {
        var html = ['<option value="" selected>' + firstOptionText + '</option>'];

        for (var i = 0; i < groups.length; i++) {
            html.push('<option value="');
            html.push(groups[i]);
            html.push('">');
            html.push(groups[i]);
            html.push("</option>");
        }

        return html.join("");
    }

    function search() {
        queryParams = getFormData($('#frmFilter'));
        searchWithParams(queryParams);
    }

    function searchWithParams(params) {
        api.query(params, function (rows) {
            var groupNames = [];
            for (var row of rows) {
                if (groupNames.indexOf(row.GroupName) === -1) {
                    groupNames.push(row.GroupName);
                }
            }

            var html = [];
            for (var groupName of groupNames) {
                html.push('<tr><td colspan="4" class="b bg group-header-bg">' + groupName + '</td></tr>');
                html.push('<tr>');
                var index = 1;
                for (var row of rows) {
                    if (row.GroupName === groupName) {
                        html.push('<td class="c">' + index + '</td>');
                        html.push('<td class="c">' + row.Prefix + '</td>');
                        html.push('<td class="c">' + (row.AllowNewSuper ? 'Yes' : 'No') + '</td>');
                        if (isHost && row.AllowNewSuper) {
                            html.push('<td class="c"><img data-group-name="' + row.GroupName + '" data-prefix="' + row.Prefix + '" src="' + resolveClientUrl('App_Themes/Images/icn_del.png') + '" style="cursor:pointer" title="Delete"></td>');
                        } else {
                            html.push('<td></td>');
                        }
                    }
                }
                html.push('</tr>');
            }

            $('#tblBody').html(html.join(''));
            $('#tblBody img').click(function () {
                var groupName = $(this).data('group-name');
                var prefix = $(this).data('prefix');
                deleteParams = {
                    groupName: groupName,
                    prefix: prefix
                };
                openDeleteDialog();
            });
        });
    }

    function submitAdd() {
        var msgId = "msgAdd";
        var params = getFormData($('#frmAdd'));
        if (!params.GroupName) {
            showAlertMessage(msgId, "Please select an External Group", 1);
            return;
        }

        var prefixRegex = /^[a-zA-Z0-9]{2,2}$/
        if (!params.Prefix && prefixRegex.test(params.Prefix)) {
            showAlertMessage(msgId, "Please input a prefix of 2 letters (only a-z; 0-9)", 1);
            return;
        }

        api.insert(params, function (data) {
            if (data.ErrorCode === 0) {
                showAlertMessage(msgId, "The new prefix has been successfully created", 0);
                setTimeout(function () {
                    searchWithParams(queryParams);
                    closeAddDialog();
                }, 3000);
            } else {
                showAlertMessage(msgId, data.ErrorMessage, 1);
            }
        });
    }

    function openAddDialog() {
        $('#dialogAddNew').dialog({
            resizable: false,
            height: "auto",
            width: 350,
            modal: true
        });
    }

    function closeAddDialog() {
        $('#dialogAddNew').dialog('close');
    }

    function openDeleteDialog() {
        $('#dialogDelete').dialog({
            resizable: false,
            height: "auto",
            modal: true,
            buttons: {
                Yes: function () {
                    remove();
                },
                No: function () {
                    $(this).dialog("close");
                }
            }
        });
    }

    function closeDeleteDialog() {
        $('#dialogDelete').dialog('close');
    }

    function remove() {
        var msgId = "msgDelete";
        api.delete(deleteParams, function (data) {
            if (data.ErrorCode === 0) {
                showAlertMessage(msgId, "The new prefix has been successfully created", 0);
                setTimeout(function () {
                    searchWithParams(queryParams);
                    closeDeleteDialog();
                }, 3000);
            } else {
                showAlertMessage(msgId, data.ErrorMessage, 1);
            }
        });
    }

    return {
        init: init
    }
})(jQuery);

$(function () {
    ManageUsernamePrefix.init();
});