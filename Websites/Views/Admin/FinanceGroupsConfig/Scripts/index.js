var radioHtml = $("input:radio[name=finance]"),
    rdoFinanceCode = $("#rdoFinanceCode"),
    rdoFinanceUsername = $("#rdoFinanceUsername"),
    txtFinanceCode = $("#txtFinanceCode"),
    txtFinanceUsername = $("#txtFinanceUsername"),
    ddlLevel = $("#ddlLevel"),
    ddlAllowViewing = $("#ddlAllowViewing"),
    financeGroupsGrid = $("#financeGroupsGrid"),
    confirmBox = $("#confirm-box");

var divAddNewGroup = $("#add-new-group"),
    divNewContent = $("#new-content"),
    divReviewContent = $("#review-content"),
    txtNewUsers = $("#txtNewUsers"),
    txtNewCodes = $("#txtNewCodes"),
    btnCreateGroup = $("#btnCreateGroup"),
    msgAddNewGroup = $("#msgAddNewGroup");

var divAddUserToGroup = $("#add-user-to-group"),
    divAddUserNewContent = $("#add-user-new-content"),
    divReviewAddUserContent = $("#review-add-user-content"),
    ddlAddUserGroups = $("#ddlAddUserGroups"),
    txtAssignUsers = $("#txtAssignUsers"),
    spanAddUserGroupId = $('#spanAddUserGroupId'),
    btnAddUser = $("#btnAddUser"),
    msgAddUser = $("#msgAddUser");

var divAddCodeToGroup = $("#add-code-to-group"),
    divAddCodeNewContent = $("#add-code-new-content"),
    divReviewAddCodeContent = $("#review-add-code-content"),
    ddlAddCodeGroups = $("#ddlAddCodeGroups"),
    txtAssignCodes = $("#txtAssignCodes"),
    spanAddCodeGroupId = $('#spanAddCodeGroupId'),
    btnAddCode = $("#btnAddCode"),
    msgAddCode = $("#msgAddCode");

var divMessageBox = $("#message-box");

var cachedValidationUsers = [];
var cachedValidationCodes = [];
var cachedGroupsDropdown = [];
var cachedGroupId = null;
var cachedCodeTotalItems = [];
var cachedCodes = [];
var cachedUserPageSize = 50;
var cachedUserPageIndex = 0;
var cachedCodePageSize = 50;
var cachedCodePageIndex = 0;

function Initialize() {
    RenderGroupsMultiSelect();

    txtFinanceUsername.prop('disabled', true);

    radioHtml.change(function () {
        if (this.value === "financeCode") {
            txtFinanceUsername.prop('disabled', true);

            txtFinanceCode.prop('disabled', false);
            ddlLevel.prop('disabled', false);
            ddlAllowViewing.prop('disabled', false);
        } else if (this.value === "financeUsername") {
            txtFinanceUsername.prop('disabled', false);

            txtFinanceCode.prop('disabled', true);
            ddlLevel.prop('disabled', true);
            ddlAllowViewing.prop('disabled', true);
        }
    });

    $('input[type=text], textarea').keydown(function (e) {
        if (e.key == 'Tab') {
            e.preventDefault();
            var start = this.selectionStart;
            var end = this.selectionEnd;

            // set textarea value to: text before caret + tab + text after caret
            this.value = this.value.substring(0, start) +
                "\t" + this.value.substring(end);

            // put caret at right position again
            this.selectionStart =
                this.selectionEnd = start + 1;
        }
    });
}

function Search(pageSize = 50, pageIndex = 0) {
    $('#err-msg').html('');

    var groupIds = $("#ddlGroups").multiSelect("getCheckedItems").join();
    if (groupIds.length === 0) {
        showAlertMessage('err-msg', 'Please choose a Finance Group.', 1);
        return;
    }

    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    var isCodeSelected = radioHtml.filter(":checked").val() === "financeCode";
    var isCheckedAll = isMultiselectCheckedAll('ddlGroups');

    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/Select',
        type: 'Post',
        dataType: 'json',
        data: {
            GroupIds: isCheckedAll ? "" : groupIds,
            UsernameList: isCodeSelected ? txtFinanceCode.val().trim() : txtFinanceUsername.val().trim(),
            FinanceType: isCodeSelected ? 0 : 1,
            Level: ddlLevel.val(),
            IsView: ddlAllowViewing.val(),
            PageSize: pageSize,
            PageIndex: pageIndex + 1
        },
        success: function (result) {
            if (isCodeSelected) {
                cachedCodes = result.FinanceCodes;
                cachedCodeTotalItems = result.TotalItems;
                RenderFinanceCodeGrid(result.FinanceCodes, result.TotalItems, pageSize, pageIndex);
            } else {
                RenderFinanceUsernameGrid(result.FinanceUsers, result.TotalItems, pageSize, pageIndex);
            }
        },
        error: function (xhr) {
            showAlertMessage('err-msg', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function InitFinanceUsernameGridOptions() {
    return {
        columns: [
            { HeaderText: "Finance User Name", Width: 220, Name: "FinanceUserName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Finance Group", Width: 180, Name: "FinanceGroup", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Edit", Width: 180, Name: "Edit", HeaderAlign: "center", CellAlign: "center" },
        ],
        showDistinctRow: true,
        showPager: true,
        customPager: true,
        pagerOption: {
            itemsPerPageArray: [50, 100, 200, 300, 500],
            pagerSelectCallBack: function (pageIndex, pageSize) {
                cachedUserPageSize = pageSize;
                cachedUserPageIndex = pageIndex;
                Search(pageSize, pageIndex);
            }
        }
    }
}

function InitFinanceCodeGridOptions() {
    var isRemoveAllDisabled = isMultiselectCheckedAll('ddlGroups') ? 'disabled' : '';
    return {
        columns: [
            { HeaderText: "Group", Width: 160, Name: "Group", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Super", Width: 200, Name: "Super", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Master", Width: 200, Name: "Master", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Agent", Width: 200, Name: "Agent", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Member", Width: 200, Name: "Member", HeaderAlign: "center", CellAlign: "center" },
            {
                HeaderText: 'Allow Viewing\
                             <br />\
                             <div style="display: flex;justify-content: center;align-items: center;margin-top: 3px">\
                                 <input type="checkbox" style="margin-right: 5px;margin-top: 5px;" onclick="CheckAllIsView(this);"/>\
                                 <input type="button" class="btn" style="width: 80px;padding: 0px;" value="Update" onclick="OpenUpdateIsView();" />\
                             </div>',
                Width: 200, Name: "AllowViewing", HeaderAlign: "center", CellAlign: "center"
            },
            {
                HeaderText: 'Remove\
                            <br />\
                            <button ' + isRemoveAllDisabled + ' class="btn ' + isRemoveAllDisabled + '" style="width: 80px;padding: 0px;margin-top: 5px;" onclick="OpenRemoveAllConfigurations();">\
                                <img src="../App_Themes/Images/icn_del.png" alt="Remove All" style="height:12px;margin-right:2px;" /> All\
                            </button>' ,
                Width: 140, Name: "Remove", HeaderAlign: "center", CellAlign: "center"
            },
        ],
        showDistinctRow: true,
        headerColumnHeight: 60,
        showPager: true,
        customPager: true,
        pagerOption: {
            itemsPerPageArray: [50, 100, 200, 300, 500],
            pagerSelectCallBack: function (pageIndex, pageSize) {
                cachedCodePageSize = pageSize;
                cachedCodePageIndex = pageIndex;
                Search(pageSize, pageIndex);
            }
        }
    };
}

function RenderFinanceUsernameGrid(data, totalItems, pageSize, pageIndex) {
    var userGridOptions = InitFinanceUsernameGridOptions();
    var bodyRows = [];
    if (data.length === 0) {
        bodyRows.push({ Columns: [{ Name: "FinanceUserName", MergeColNumber: 3, Value: '<i style="color: red;">No information is available</i>' }] });
        userGridOptions.pagerOption.currentPage = 0;
        userGridOptions.pagerOption.itemsPerPage = 1;
        userGridOptions.pagerOption.totalItem = 1;
    } else {
        for (var i = 0; i < data.length; i++) {
            var row = { Columns: [] };
            row.Columns.push({ Name: "FinanceUserName", Value: data[i].UserName });
            row.Columns.push({
                Name: "FinanceGroup",
                Value: '<div id="group-cell-' + i + '">' + data[i].GroupId + '</div>'
            });
            row.Columns.push({
                Name: "Edit",
                Value: '<img style="cursor:pointer" src="../App_Themes/Images/icn_edit.png" title="Edit" alt="Edit" onclick="HandleEditGroup(' + i + ',\'' + data[i].GroupId + '\',this);">\
                        <div class="hidden">\
                            <img border="0" style="cursor:pointer; width: 15px" src="../App_Themes/Images/save1.jpg" title="Save" alt="Save" onclick="HandleSaveGroup(' + i + ',\'' + data[i].GroupId + '\',\'' + data[i].UserName + '\');">\
                            <img border="0" style="cursor:pointer; width: 15px; margin-left: 7px;" src="../App_Themes/Images/undo.png" title="Cancel" alt="Cancel" onclick="HandleCancelGroup( ' + i + ',\'' + data[i].GroupId + '\',this);">\
                        </div>',
            });
            bodyRows.push(row);
        }
        userGridOptions.pagerOption.currentPage = pageIndex;
        userGridOptions.pagerOption.itemsPerPage = pageSize;
        userGridOptions.pagerOption.totalItem = totalItems;
    }

    cachedUserPageSize = pageSize;
    cachedUserPageIndex = pageIndex;

    userGridOptions.bodyRows = bodyRows;
    financeGroupsGrid.fanexGrid(userGridOptions);
}

function RenderFinanceCodeGrid(data, totalItems, pageSize, pageIndex) {
    var codeGridOptions = InitFinanceCodeGridOptions();
    var bodyRows = [];
    if (data.length === 0) {
        bodyRows.push({ Columns: [{ Name: "Group", MergeColNumber: 7, Value: '<i style="color: red;">No information is available</i>' }] });

        codeGridOptions.pagerOption.currentPage = 0;
        codeGridOptions.pagerOption.itemsPerPage = 1;
        codeGridOptions.pagerOption.totalItem = 1;
    } else {
        var groups = jlinq.from(data).group("GroupId");
        var index = 0;
        for (var groupId in groups) {
            var group = groups[groupId];

            for (var i = 0; i < group.length; i++) {
                var row = { Columns: [] };
                var isChecked = group[i].IsView ? 'checked' : ' ';
                
                if (i == 0) {
                    row.Columns.push({ Name: "Group", Value: group[i].GroupId, MergeRowNumber: group.length });
                }

                row.Columns.push({ Name: "Super", Value: RenderFinanceCodeCell(group[i].SuperName, group[i].FinanceCode) });
                row.Columns.push({ Name: "Master", Value: RenderFinanceCodeCell(group[i].MasterName, group[i].FinanceCode) });
                row.Columns.push({ Name: "Agent", Value: RenderFinanceCodeCell(group[i].AgentName, group[i].FinanceCode) });
                row.Columns.push({ Name: "Member", Value: RenderFinanceCodeCell(group[i].MemberName, group[i].FinanceCode) });
                row.Columns.push({ Name: "AllowViewing", Value: '<input type="checkbox" class="is-view" ' + isChecked + ' onclick="HandleCheckIsView(' + index + ', this);" />'});
                row.Columns.push({ Name: "Remove", Value: '<img alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer; padding: 0 2px;" onclick="OpenRemoveConfiguration(' + group[i].GroupId + ',\'' + group[i].FinanceCode + '\',' + group[i].IsView + ',' + group[i].RoleId + ')" />' });
                
                bodyRows.push(row);
                index++;
            }
        }

        codeGridOptions.pagerOption.currentPage = pageIndex;
        codeGridOptions.pagerOption.itemsPerPage = pageSize;
        codeGridOptions.pagerOption.totalItem = totalItems;
    }

    cachedCodePageSize = pageSize;
    cachedCodePageIndex = pageIndex;
    
    codeGridOptions.bodyRows = bodyRows;
    financeGroupsGrid.fanexGrid(codeGridOptions);
}

function RenderFinanceCodeCell(value, financeCode) {
    if (value === '') return 'All Codes';
    if (value === financeCode) return '<b style="color: green;">' + value + '</b>';

    return value;
}

function HandleEditGroup(index, groupId, obj) {
    $(obj).next().removeClass('hidden');
    $(obj).addClass('hidden');

    $('#group-cell-' + index).html('<select id="group-select-' + index + '" style="width: 100%; height: 25px;"></select>');
    RenderGroupsSelect('group-select-' + index, cachedGroupsDropdown, groupId);
}

function HandleSaveGroup(index, oldGroupId, username) {
    var newGroupId = $('#group-select-' + index).val();
    
    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/Update',
        type: 'Post',
        dataType: 'json',
        data: {
            UserName: username,
            OldGroupId: oldGroupId === 'None' ? '' : oldGroupId,
            NewGroupId: newGroupId
        },
        success: function (result) {
            if (result.ErrorCode) {
                showAlertMessage('err-msg', result.ErrorMessage , 1);
                return;
            }

            RenderGroupsMultiSelect(null, function () {
                Search(cachedUserPageSize, cachedUserPageIndex);
            });
        },
        error: function (xhr) {
            showAlertMessage('err-msg', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function HandleCancelGroup(index, groupId, obj) {
    $(obj).parent().addClass('hidden');
    $(obj).parent().prev().removeClass('hidden');
    $('#group-cell-' + index).html(groupId);
}

function HandleCheckIsView(index, obj) {
    cachedCodes[index].IsView = $(obj).prop('checked') ? 1 : 0;
}

function CheckAllIsView(obj) {
    var isChecked = $(obj).prop('checked');
    var isView = isChecked ? 1 : 0;

    $('.is-view').each(function (index) {
        $(this).prop('checked', isChecked);
        cachedCodes[index].IsView = isView;
    });
}

function OpenUpdateIsView() {
    OpenConfirmBox('Are you sure you want to update Allow Viewing of these Finance Codes', "Update Allow Viewing", function () {
        try { parent.BlockUI(); } catch (e) { BlockUI(); }
        $.ajax({
            url: window.rootUrl + 'FinanceGroupsConfig/UpdateIsView',
            type: 'Post',
            dataType: 'json',
            data: {
                FinanceCodes: cachedCodes
            },
            success: function (result) {
                if (result.ErrorCode) {
                    showAlertMessage('err-msg', result.ErrorMessage, 1);
                    return;
                }

                Search(cachedCodePageSize, cachedCodePageIndex);
            },
            error: function (xhr) {
                showAlertMessage('err-msg', 'Internal Server Error.', 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
            }
        });
    });
}

function OpenRemoveAllConfigurations() {
    OpenConfirmBox('Are you sure you want to remove <b>' + cachedCodeTotalItems + '</b> configurations? <br/> This action is permanent and cannot be undone.', "Delete Configurations", function () {
        try { parent.BlockUI(); } catch (e) { BlockUI(); }
        $.ajax({
            url: window.rootUrl + 'FinanceGroupsConfig/RemoveAll',
            type: 'Post',
            dataType: 'json',
            data: {
                GroupIds: $("#ddlGroups").multiSelect("getCheckedItems").join(),
                UsernameList: txtFinanceCode.val().trim(),
                Level: ddlLevel.val(),
                IsView: ddlAllowViewing.val(),
                TotalItems: cachedCodeTotalItems,
            },
            success: function (result) {
                if (result.ErrorCode) {
                    showAlertMessage('err-msg', result.ErrorMessage, 1);
                    return;
                }

                RenderGroupsMultiSelect(null, function () {
                    Search();
                })
            },
            error: function (xhr) {
                showAlertMessage('err-msg', 'Internal Server Error.', 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
            }
        });
    });
}

function OpenRemoveConfiguration(groupId, financeCode, isView, roleId) {
    OpenConfirmBox("Are you sure you want to remove this configurations? <br/> This action is permanent and cannot be undone.", "Delete Configuration", function () {
        RemoveConfig([{ GroupId: groupId, UserName: financeCode, IsView: isView, RoleId: roleId }]);
    });
}

function RemoveConfig(groups) {
    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/Remove',
        type: 'Post',
        dataType: 'json',
        data: {
            FinanceGroups: groups
        },
        success: function (result) {
            if (result.ErrorCode) {
                showAlertMessage('err-msg', result.ErrorMessage, 1);
                return;
            }

            RenderGroupsMultiSelect(null, function () {
                Search(cachedCodePageSize, cachedCodePageIndex);
            })
        },
        error: function (xhr) {
            showAlertMessage('err-msg', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function OpenConfirmBox(message, removeText, removeFunction) {
    confirmBox.html(message);
    confirmBox.dialog({
        autoOpen: false,
        height: 120,
        width: 300,
        modal: true,
        buttons: {
            "Cancel": function () {
                confirmBox.dialog("close");
            },
            [removeText]: function () {
                removeFunction();
                confirmBox.dialog("close");
            }
        },
        close: function (event, ui) { }
    });
    confirmBox.dialog('open');
}

// Add New Group
function OpenAddNewGroup() {
    ClearOldAddNewGroupData();

    divAddNewGroup.dialog({
        title: 'Create new Finance Group',
        modal: true,
        width: 'auto',
        height: 'auto',
    });
    divAddNewGroup.parent().addClass("custom-dialog");

    txtNewUsers.off("change").on("change", function () {
        HandleSeperateValuesWithCommas(txtNewUsers);
    });

    txtNewCodes.off("change").on("change", function () {
        HandleSeperateValuesWithCommas(txtNewCodes);
    });
}

function ClearOldAddNewGroupData() {
    msgAddNewGroup.html('');
    txtNewUsers.val("");
    txtNewCodes.val("");
    divNewContent.removeClass("hidden");
    divReviewContent.addClass("hidden");
}

function CloseAddNewGroup() {
    divAddNewGroup.dialog('close');
    divAddNewGroup.dialog('destroy');
}

function OpenReviewNewGroup() {
    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/ValidateGroupInsertion',
        type: 'Post',
        dataType: 'json',
        data: {
            FinanceCode: txtNewCodes.val(),
            FinanceUser: txtNewUsers.val()
        },
        success: function (result) {
            if (result.ErrorCode) {
                showAlertMessage('msgAddNewGroup', result.ErrorMessage, 1);
                return;
            }

            msgAddNewGroup.html('');
            divNewContent.addClass("hidden");
            divReviewContent.removeClass("hidden");

            cachedValidationUsers = result.FinanceUsers;
            cachedValidationCodes = result.FinanceCodes;

            RenderUserReview('new-user-grid');
            RenderCodeReview('new-code-grid');

            btnCreateGroup.prop('disabled', result.IsCreateGroup === 1 ? false : true);
            
        },
        error: function (xhr) {
            showAlertMessage('msgAddNewGroup', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function BackAddNewGroup() {
    divReviewContent.addClass("hidden");
    divNewContent.removeClass("hidden");
}

function SubmitAddNewGroup() {
    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/InsertGroup',
        type: 'Post',
        dataType: 'json',
        data: {
            FinanceCodes: cachedValidationCodes.filter(function (item) {
                return item.Status !== "Invalid";
            }),
            FinanceUsers: cachedValidationUsers.filter(function (item) {
                return item.Status !== "Invalid";
            })
        },
        success: function (result) {
            if (result.ErrorCode) {
                CloseAddNewGroup();
                OpenMessageBox('', result.ErrorMessage);
                return;
            }

            var successMsg = 'Finance Group ' + result.GroupId + ' was created successfully with <b>' + result.NUsers + ' Finance Users</b> and  <b>' + result.NCodes + ' Finance Codes </b>';
            CloseAddNewGroup();
            OpenMessageBox(successMsg, '', result.GroupId);
        },
        error: function (xhr) {
            showAlertMessage('msgAddNewGroup', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

// Add User To Group
function OpenAddUsersToGroup() {
    ClearOldAddUserToGroupData();

    divAddUserToGroup.dialog({
        title: 'Assign Finance Users to Group',
        modal: true,
        width: 'auto',
        height: 'auto',
    });

    RenderGroupsSelect('ddlAddUserGroups', cachedGroupsDropdown);

    txtAssignUsers.off("change").on("change", function () {
        HandleSeperateValuesWithCommas(txtAssignUsers);
    });
}

function ClearOldAddUserToGroupData() {
    msgAddUser.html('');
    txtAssignUsers.val('');
    ddlAddUserGroups.val('1');
    divReviewAddUserContent.addClass("hidden");
    divAddUserNewContent.removeClass("hidden");
}

function CloseAddUserToGroup() {
    divAddUserToGroup.dialog('close');
    divAddUserToGroup.dialog('destroy');
}

function OpenReviewAddUserToGroup() {
    if (ddlAddUserGroups.val() === '') {
        showAlertMessage('msgAddUser', 'Please choose a Finance Group.', 1);
        return;
    }

    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/ValidateUserInsertion',
        type: 'Post',
        dataType: 'json',
        data: {
            Usernamelist: txtAssignUsers.val(),
        },
        success: function (result) {
            if (result.ErrorCode) {
                showAlertMessage('msgAddUser', result.ErrorMessage, 1);
                return;
            }
            cachedValidationUsers = result.FinanceUsers;
            cachedGroupId = ddlAddUserGroups.val();

            msgAddUser.html('');
            divAddUserNewContent.addClass("hidden");
            divReviewAddUserContent.removeClass("hidden");

            spanAddUserGroupId.html(cachedGroupId);
            RenderUserReview('add-user-grid');

            btnAddUser.prop('disabled', result.IsAssignUsers === 1 ? false : true);

        },
        error: function (xhr) {
            showAlertMessage('msgAddUser', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function SubmitAddUserToGroup() {
    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/InsertUser',
        type: 'Post',
        dataType: 'json',
        data: {
            FinanceUsers: cachedValidationUsers.filter(function (item) {
                return item.Status !== "Invalid";
            }),
            GroupId: cachedGroupId
        },
        success: function (result) {
            if (result.ErrorCode) {
                CloseAddUserToGroup();
                OpenMessageBox('', result.ErrorMessage);
                return;
            }

            var successMsg = '<b> ' + result.NRows + ' Finance Users</b> were assigned to Group ' + cachedGroupId + ' successfully.';
            CloseAddUserToGroup();
            OpenMessageBox(successMsg, '', cachedGroupId);
        },
        error: function (xhr) {
            showAlertMessage('msgAddUser', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function BackAddUserToGroup() {
    divReviewAddUserContent.addClass("hidden");
    divAddUserNewContent.removeClass("hidden");
}

// Add Code To Group
function OpenAddCodesToGroup() {
    ClearOldAddCodeToGroupData();

    divAddCodeToGroup.dialog({
        title: 'Assign Finance Codes to Group',
        modal: true,
        width: 'auto',
        height: 'auto',
    });

    var groups = cachedGroupsDropdown.filter(function (group) {
        return group.Id !== 0 && group.Id !== 1;
    });
    RenderGroupsSelect('ddlAddCodeGroups', groups);

    txtAssignCodes.off("change").on("change", function () {
        HandleSeperateValuesWithCommas(txtAssignCodes);
    });
}

function ClearOldAddCodeToGroupData() {
    msgAddCode.html('');
    txtAssignCodes.val('');
    ddlAddCodeGroups.val('1');
    divReviewAddCodeContent.addClass("hidden");
    divAddCodeNewContent.removeClass("hidden");
}

function CloseAddCodeToGroup() {
    divAddCodeToGroup.dialog('close');
    divAddCodeToGroup.dialog('destroy');
}

function OpenReviewAddCodeToGroup() {
    // Validate
    if (ddlAddCodeGroups.val() === '') {
        showAlertMessage('msgAddCode', 'Please choose a Finance Group.', 1);
        return;
    }

    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/ValidateCodeInsertion',
        type: 'Post',
        dataType: 'json',
        data: {
            Usernamelist: txtAssignCodes.val(),
        },
        success: function (result) {
            if (result.ErrorCode) {
                showAlertMessage('msgAddCode', result.ErrorMessage, 1);
                return;
            }
            cachedValidationCodes = result.FinanceCodes;
            cachedGroupId = ddlAddCodeGroups.val();

            msgAddCode.html('');
            divAddCodeNewContent.addClass("hidden");
            divReviewAddCodeContent.removeClass("hidden");

            spanAddCodeGroupId.html(cachedGroupId);
            RenderCodeReview('add-code-grid');

            btnAddCode.prop('disabled', result.IsAssignCodes === 1 ? false : true);
        },
        error: function (xhr) {
            showAlertMessage('msgAddCode', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function SubmitAddCodeToGroup() {
    $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/InsertCode',
        type: 'Post',
        dataType: 'json',
        data: {
            FinanceCodes: cachedValidationCodes.filter(function (item) {
                return item.Status !== "Invalid";
            }),
            GroupId: cachedGroupId
        },
        success: function (result) {
            if (result.ErrorCode) {
                CloseAddCodeToGroup();
                OpenMessageBox('', result.ErrorMessage);
                return;
            }

            var successMsg = '<b> ' + result.NRows + ' Finance Codes</b> were assigned to Group ' + cachedGroupId + ' successfully.';
            CloseAddCodeToGroup();
            OpenMessageBox(successMsg, '', cachedGroupId);
        },
        error: function (xhr) {
            showAlertMessage('msgAddCode', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { UnBlockUI(); }
        }
    });
}

function BackAddCodeToGroup() {
    divReviewAddCodeContent.addClass("hidden");
    divAddCodeNewContent.removeClass("hidden");
}

// Message Box
function OpenMessageBox(successMsg, errorMsg, newGroupId) {
    divMessageBox.dialog({
        modal: true,
        width: '700px',
        height: 'auto',
    });
    if (successMsg !== '') {
        $('#finance-msg').html(successMsg);
        $('#message-btn-group').removeClass('hidden');
        $('#btnViewGroupUsers').off('click').on('click', function () {
            ViewGroupsUsers(newGroupId);
        });
        $('#btnViewGroupCodes').off('click').on('click', function () {
            ViewGroupsCodes(newGroupId);
        });
        $('#btnOk').off('click').on('click', function () {
            Reload();
        });
    } else {
        $('#finance-msg').html(errorMsg).css({
            'color': 'red',
            'font-style': 'italic'
        });
        $('#message-btn-group').addClass('hidden');
    }
}

function CloseMessageBox() {
    divMessageBox.dialog('close');
    divMessageBox.dialog('destroy');
}

function ViewGroupsUsers(newGroupId) {
    CloseMessageBox();
    RenderGroupsMultiSelect(newGroupId, function () {
        ResetFilters();
        rdoFinanceUsername.prop('checked', true).change();
        Search();
    }); 
}

function ViewGroupsCodes(newGroupId) {
    CloseMessageBox();
    RenderGroupsMultiSelect(newGroupId, function () {
        ResetFilters();
        rdoFinanceCode.prop('checked', true).change();
        Search();
    });
}

function Reload() {
    CloseMessageBox();
    RenderGroupsMultiSelect(null, function () {
        ResetFilters();
        rdoFinanceCode.prop('checked', true).change();
        Search();
    });
}

// Common
function RenderUserReview(divContainer) {
    var bodyRows = [];
    for (var i = 0; i < cachedValidationUsers.length; i++) {
        var row = { Columns: [] };
        var isInvalid = cachedValidationUsers[i].Status === 'Invalid' ? true : false;

        row.Columns.push({ Name: 'FinanceUsers', Value: cachedValidationUsers[i].UserName });
        row.Columns.push({ Name: 'Status', Value: cachedValidationUsers[i].Status });
        row.Columns.push({ Name: 'Assigned', Value: isInvalid ? cachedValidationUsers[i].Status : cachedValidationUsers[i].GroupId });

        if (isInvalid) {
            row.CssClass = "invalid-row";
        }

        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Finance Users", Width: 300, Name: "FinanceUsers", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Status", Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Assigned to Group", Width: 150, Name: "Assigned", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        showDistinctRow: false,
        fullHeight: false,
        gridHeight: 350,
        minHeight: 350
    };

    $('#' + divContainer).fanexGrid(gridOptions);
}

function RenderCodeReview(divContainer) {
    var bodyRows = [];
    for (var i = 0; i < cachedValidationCodes.length; i++) {
        var row = { Columns: [] };
        var isInvalid = cachedValidationCodes[i].Status === 'Invalid' ? true : false;

        row.Columns.push({ Name: 'FinanceCodes', Value: cachedValidationCodes[i].UserName });
        row.Columns.push({
            Name: 'AllowViewing', Value: isInvalid ? cachedValidationCodes[i].Status : '<input type="checkbox" checked onclick="HandleCheckIsViewCodeValidation(' + i + ')">'
        });

        if (isInvalid) {
            row.CssClass = "invalid-row";
        }

        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Finance Codes", Width: 400, Name: "FinanceCodes", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Allow Viewing", Width: 150, Name: "AllowViewing", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        showDistinctRow: false,
        fullHeight: false,
        gridHeight: 350,
        minHeight: 350
    };

    $('#' + divContainer).fanexGrid(gridOptions);
}

function HandleCheckIsViewCodeValidation(index) {
    cachedValidationCodes[index].IsView = cachedValidationCodes[index].IsView === 1 ? 0 : 1 ;
}

function HandleSeperateValuesWithCommas(inputSelector) {
    var inputValue = $.trim(inputSelector.val());

    if (inputValue.length > 0) {
        if (inputValue.indexOf("\n") >= 0) {
            inputValue = inputValue.replace(/\n/g, ",");
        }

        if (inputValue.indexOf("\r") >= 0) {
            inputValue = inputValue.replace(/\r/g, ",");
        }

        if (inputValue.indexOf("\t") >= 0) {
            inputValue = inputValue.replace(/\t/g, ",");
        }

        var valuesArray = inputValue.split(",").filter(item => item.length > 0);
        inputSelector.val(valuesArray.join(","));
    }
}

function ResetFilters() {
    txtFinanceCode.val("");
    ddlLevel.val("");
    ddlAllowViewing.val("");
    txtFinanceUsername.val("");
}

// Utils
function RenderGroupsMultiSelect(newGroupId = null, callback = null) {
    return $.ajax({
        url: window.rootUrl + 'FinanceGroupsConfig/GetGroups',
        type: 'GET',
        success: function (result) {
            cachedGroupsDropdown = result;
            var listData = result.map(function (item) {
                return {
                    Id: item.Id,
                    Value: item.Id,
                    Text: item.Name,
                    Checked: newGroupId ? item.Id == newGroupId : true,
                    Enable: true
                };
            });

            $('#ddlGroups').multiSelect({
                minWidth: 120,
                maxHeight: 450,
                shadow: true,
                showHeader: true,
                showTooltip: false,
                isCheckAll: newGroupId ? false : true,
                showFooter: false,
                showFilter: true,
                checkAllText: "All",
                listData: listData,
                noItemText: "No item selected."
            });

            if (callback) {
                callback();
            }
        }
    })
}

function RenderGroupsSelect(id, groups, selectedGroupId = null) {
    var defaultOption = selectedGroupId ? '<option value="" selected>None</option>' : '<option value="">Select a group</option>';
    var options = groups.map(function (item) {
        var isSelected = selectedGroupId == item.Id ? 'selected' : '';
        return '<option value="' + item.Id + '" ' + isSelected + ' >' + item.Name + '</option>';
    }).join('');

    $('#' + id).html(defaultOption + options);
}

function BlockUI() {
    $.blockUI({
        message: '<img src="' + window.rootUrl + 'Areas/Common/Views/Shared/Images/Loading.gif" />',
        css: {
            border: 'none',
            backgroundColor: 'transparent'
        }
    });
}

function UnBlockUI() {
    $.unblockUI(document);
}

function isMultiselectCheckedAll(id) {
    return $('#' + id).next("div.ui-multiselect-options").find("#" + $('#' + id).attr("id") + "HeaderCheckBox").is(":checked");
}

$(function () {
    Initialize();
});