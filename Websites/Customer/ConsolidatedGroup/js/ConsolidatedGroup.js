var ConsolidatedGroup = {
    AssignGroup: {}
    , AllGroup: {}
    , utils: {}
    , EditGroup: {}
};
ConsolidatedGroup.init = function () {
    BlockUI();
    //just call 1 times
    this.AssignGroup.init();
    //just call 1 times
    this.AllGroup.init();
    //just call 1 times
    this.EditGroup.init();
};

ConsolidatedGroup.EditGroup.init = function() {
    //tbxUsernameEdit
    $("#tbxUsernameEdit").keyup(function(){
        //ConsolidatedGroup.utils.hideMessage();
    });

    //Checkbox button
    this.cbxEditGroupHeader = document.getElementById("cbxEditGroupHeader");
    this.cbxEditGroupHeader._click = function(element) {
        element.checked = !element.checked;
        ConsolidatedGroup.EditGroup.setValueAllCheckBox(element.checked);
    };
    $("#cbxEditGroupHeader").attr("onclick", "javascript:ConsolidatedGroup.EditGroup.cbxEditGroupHeader._click(this);");
    ConsolidatedGroup.utils.Custom.init($("#cbxEditGroupHeader").parent());
    
    //back from : btnBackFromEditGroup
    $("#btnBackFromEditGroup").click(function () {
        ConsolidatedGroup.utils.hideMessage();
        BlockUI();
        $("#divEditGroupHeader").hide();
        $("#divEditGroup").hide();
    
        $("#divTitle").html("View All Groups");
        if (true) { //if (ConsolidatedGroup.EditGroup.hadChangeHeadline) {
            ConsolidatedGroup.AssignGroup.reloadViewModelAfterChangeHeadline();
            ConsolidatedGroup.EditGroup.hadChangeHeadline = false;
        }
        /*else {
            $("#divViewAllGroup").show();
            $("#divViewAllGroupHeader").show();    
            jQuery.unblockUI();
        }*/
    });
    
    //tbxUsernameEdit
    $("#tbxUsernameEdit").focus(function() {
        this.style.border = "1px solid gray";
        $(this).css("font-style", "inherit");
        this.style.color = "black";
    });
    
    //btnAddToGroup
    $("#btnAddToGroup").click($.proxy(function() {
        ConsolidatedGroup.utils.hideMessage();
        if ($("#tbxUsernameEdit").val() == "") {
            $("#tbxUsernameEdit").focus();
            ConsolidatedGroup.utils.showMessage("Please input username.", "error");
        }
        else {
            this.addUserToGroup();
        }
    }, this));
    
    //sort: btnEditGroupUserNameSortUp
    if ($.browser.msie && document.documentMode == 7) {
        $("#btnEditGroupUserNameSortUp").css("margin-left", "-3px");
    }
    else if ($.browser.msie && document.documentMode == 8) {
        $("#btnEditGroupUserNameSortUp").css("margin-left", "-2px");
    }
    $("#btnEditGroupUserNameSortUp").click(function() {
        var indexHeadline = -1;
        $(ConsolidatedGroup.EditGroup.consolidatedMemberTable).each(function(index, row){
            if (row.IsHeader) {
                indexHeadline = index;
            }
        });
        var headline = null;
        if (indexHeadline != -1) {
            headline = ConsolidatedGroup.EditGroup.consolidatedMemberTable.splice(indexHeadline, 1)[0];
        }
        
        jlinq.from(ConsolidatedGroup.EditGroup.consolidatedMemberTable).sort("-UserName").select();
        
        if (headline != null) {
            var arr = [headline];
            ConsolidatedGroup.EditGroup.consolidatedMemberTable = arr.concat(ConsolidatedGroup.EditGroup.consolidatedMemberTable);
        }
        
        ConsolidatedGroup.EditGroup.fillData();
        
        $(this).parent().hide();
        $("#btnEditGroupUserNameSortDown").parent().show();
    });
    
    //sort: btnEditGroupUserNameSortDown
    $("#btnEditGroupUserNameSortDown").click(function() {
        var indexHeadline = -1;
        $(ConsolidatedGroup.EditGroup.consolidatedMemberTable).each(function(index, row) {
            if (row.IsHeader) {
                indexHeadline = index;
            }
        });
        var headline = null;
        if (indexHeadline != -1) {
            headline = ConsolidatedGroup.EditGroup.consolidatedMemberTable.splice(indexHeadline, 1)[0];
        }
    
        jlinq.from(ConsolidatedGroup.EditGroup.consolidatedMemberTable).sort("UserName").select();
        
        if (headline != null) {
            var arr = [headline];
            ConsolidatedGroup.EditGroup.consolidatedMemberTable = arr.concat(ConsolidatedGroup.EditGroup.consolidatedMemberTable);
        }
        
        ConsolidatedGroup.EditGroup.fillData();
        
        $(this).parent().hide();
        $("#btnEditGroupUserNameSortUp").parent().show();
    });
    
    //btnRemoveUser
    $("#btnRemoveUser").click($.proxy(function(){
        BlockUI();
        var getAllCheckedBox = function () {
            var arrCheckBox = []
            $("#divBodyEditGroup").find("table tbody tr td input[type='checkbox']").each(function(index, element) {
                if (element.checked == true) {
                    arrCheckBox[arrCheckBox.length] = element;
                }
            });
            return arrCheckBox;
        };
        
        var getListCustomer = function () {
            var arrResult = []
            //get all checked user
            var listCheckedBox = getAllCheckedBox();
            if (listCheckedBox.length <= 0) {
                return null;
            }
            for (var i = 0; i < listCheckedBox.length; ++i) {
                arrResult[arrResult.length] = $(listCheckedBox[i]).parent().find("input[type='hidden'][username='true']").val() + ";"
                    + $(listCheckedBox[i]).parent().find("input[type='hidden'][custid='true']").val();
            }
            return arrResult.join(",");
        }
        this.removeUsers(getListCustomer());
    }, this));
};

ConsolidatedGroup.EditGroup.removeUsers = function (CustIDList) {
    if (CustIDList == null) {
        jQuery.unblockUI();
        ConsolidatedGroup.utils.showMessage("Please select user to remove.", "error");
        return;
    }
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/RemoveUsers",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"GroupID": ' + $("#hiddenEditGroupGroupID").val() + ', "CustIDList": "' + CustIDList + '"}',
            success: $.proxy(function (msg)
            {
                if (typeof msg != "undefined" && msg != null && msg.ErrorCode != 0) {
                    ConsolidatedGroup.utils.showMessage(msg);
                    jQuery.unblockUI();
                }
                else {
                    ConsolidatedGroup.EditGroup.loadDataAfterRemove($("#hiddenEditGroupGroupID").val());
                }
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.EditGroup.addUserToGroup = function () {
    BlockUI();
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/AddUserToGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"GroupID": ' + $("#hiddenEditGroupGroupID").val() + ', "UserName": "' + $("#tbxUsernameEdit").val() + '"}',
            success: $.proxy(function (msg)
            {
                if (typeof msg != "undefined" && typeof msg.ErrorCode != "undefined" && msg.ErrorCode == 0) {
                    this.consolidatedMemberTable[this.consolidatedMemberTable.length] = {
                        CustID: -1
                        , GroupID: $("#hiddenEditGroupGroupID").val()
                        , IsHeader: false
                        , UserName: $("#tbxUsernameEdit").val()
                    };
                    //msg.ErrorMessage = "Add user to group successfully.";
                }
                this.fillData();
                ConsolidatedGroup.utils.showMessage(msg);
                ConsolidatedGroup.AllGroup.editGroup($("#hiddenEditGroupGroupID").val());
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.AssignGroup.init = function() {
    //Submit button
    $("#btnSubmit").click($.proxy(function() {
        this.save();
    }, this));
    
    //View all group buttons
    $("#btnViewAllGroups").click($.proxy(function (){
        ConsolidatedGroup.AllGroup.viewAllGroup();
    }, this));
    
    //Remark, maxlength = 500
    $("#textRemark").maxlength({
        'feedback' : '.charsLeft'
    });
};

ConsolidatedGroup.AllGroup.viewAllGroup = function() {
    ConsolidatedGroup.utils.hideMessage();
    BlockUI();
    $("#divTitle").html("View All Groups");
    $("#divAssignGroup").hide();
    $("#divAssignGroupHeader").hide();
    
    //tbxSearch
    $("#tbxSearch").css("font-style", "italic");
    $("#tbxSearch").css("color", "gray");
    $("#tbxSearch").val('By username of headline');
    
    if (true) {//if (ConsolidatedGroup.AssignGroup.hadAdded) {//reload viewmodel
        ConsolidatedGroup.AssignGroup.reloadViewModelForViewAllGroup();
        ConsolidatedGroup.AssignGroup.hadAdded = false;
    }
    /*else {
        ConsolidatedGroup.dataTableCustomerGroup = ConsolidatedGroup.dataTableCustomerGroupBackup;
        ConsolidatedGroup.AllGroup.loadGrid();
        $("#divViewAllGroup").show();
        $("#divViewAllGroupHeader").show();
        ConsolidatedGroup.AllGroup.fixScrollBar();
        jQuery.unblockUI();
    }*/
    //fill: selectListCurrency
    ConsolidatedGroup.AllGroup.fillCurrencyList();
    
    //-------------------------------------------------
    $("#btnUsernameDown").parents("tr:first").hide();
    $("#btnUsernameUp").parents("tr:first").show();
    
    $("#btnGroupIDUp").parents("tr:first").show();
    $("#btnGroupIDDown").parents("tr:first").show();
};

ConsolidatedGroup.AssignGroup.reloadViewModelForViewAllGroup = function () {
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/GetAllGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: $.proxy(function (rs)
            {
                ConsolidatedGroup.dataTableCustomerGroup = rs;
                ConsolidatedGroup.dataTableCustomerGroupBackup = rs;
                $("#divViewAllGroup").show();
                $("#divViewAllGroupHeader").show();
                ConsolidatedGroup.AllGroup.loadGrid();
                ConsolidatedGroup.AllGroup.fixScrollBar();
                jQuery.unblockUI();
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.AssignGroup.reloadViewModelAfterAddNewGroup = function () {
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/GetAllGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: $.proxy(function (rs)
            {
                ConsolidatedGroup.dataTableCustomerGroup = rs;
                ConsolidatedGroup.dataTableCustomerGroupBackup = rs;
                jQuery.unblockUI();
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.AssignGroup.reloadViewModelAfterChangeHeadline = function () {
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/GetAllGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: $.proxy(function (rs)
            {
                ConsolidatedGroup.dataTableCustomerGroup = rs;
                ConsolidatedGroup.dataTableCustomerGroupBackup = rs;
                $("#divViewAllGroup").show();
                $("#divViewAllGroupHeader").show();
                ConsolidatedGroup.AllGroup.loadGrid();
                //-------------------------------------------------
                $("#btnUsernameDown").parents("tr:first").hide();
                $("#btnUsernameUp").parents("tr:first").show();
                
                $("#btnGroupIDUp").parents("tr:first").show();
                $("#btnGroupIDDown").parents("tr:first").show();
                
                jQuery.unblockUI();
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.AssignGroup.verifyData = function() {
    //verify customer name
    if ($("#tbxCustomer").val() == "") {
        ConsolidatedGroup.utils.showMessage("Please input customer name.", "error");
        return false;
    }
    else {
        //check existed headline
        var groupExisted = false;
        $(ConsolidatedGroup.dataTableCustomerGroupBackup).each(function(index, row) {
            if (row.UserName == $("#tbxCustomer").val()) {
                groupExisted = true;
            }
        });
        if (groupExisted) {
            ConsolidatedGroup.utils.showMessage("Group existed.", "error");
            return false;
        }
    }
    return true;
};

ConsolidatedGroup.AssignGroup.save = function() {
    ConsolidatedGroup.utils.hideMessage();
    if (!this.verifyData()) return;
    BlockUI();
    var row = {
        GroupID: "0"
        , UserName: $("#tbxCustomer").val()
        , isHeader: true
        , Remark: $("#textRemark").val()
    };
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/UpdateGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(row),
            success: $.proxy(function (msg)
            {
                if (typeof msg != "undefined" && typeof msg.ErrorCode != "undefined" && msg.ErrorCode == 0) {
                    //msg.ErrorMessage = "New Group Created.";
                    //this.reloadViewModelAfterAddNewGroup();
                    this.hadAdded = true;
                }
                jQuery.unblockUI();
                ConsolidatedGroup.utils.showMessage(msg);
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.AllGroup.init = function () {
    //divAddNewGroup
    $("#divAddNewGroup").click(function(){
        BlockUI();
        $("#tbxCustomer").val('');
        $("#textRemark").val('');
        $("#divAssignGroup").show();
        $("#divAssignGroupHeader").show();
        $("#divViewAllGroup").hide();
        $("#divViewAllGroupHeader").hide();
        $("#divTitle").html("New Group");
        jQuery.unblockUI();
    });
    //btnSearch
    $("#btnSearch").click($.proxy(function () {
        this.search($("#tbxSearch").val());
    }, this));
    
    //btnClearText
    $("#btnClearText").click(function () {
        $("#tbxSearch").val('').focus();
    });
    
    //tbxSearch
    $("#tbxSearch").blur(function () {
        if ($(this).val() == "") {
            $(this).css("font-style", "italic");
            $(this).css("color", "gray");
            $(this).val('By username of headline');
        }
    })
    .focus(function () {
        if ($(this).val() == "By username of headline") {
            $(this).css("font-style", "inherit");
            $(this).css("color", "black");
            $(this).val('');
        }
    })
    .keyup($.proxy(function (e) {
        if (e.keyCode == 13) {
            this.search($("#tbxSearch").val());
        }
        else {
            if (ConsolidatedGroup.utils.isAlphanumeric(e) || e.keyCode == 8) {
                if ($("#tbxSearch").val() == '') {
                    ConsolidatedGroup.utils.hideAutoComplete($("#tbxSearch"));
                    return;
                }
                var dataTableCustomerGroup = jlinq.from(ConsolidatedGroup.dataTableCustomerGroupBackup)
                    .contains("UserName", $("#tbxSearch").val())
                    .select();
                ConsolidatedGroup.utils.autoComplete($("#tbxSearch"), dataTableCustomerGroup, "UserName");
            }
        }
    }, this));
    
    //sort: btnGroupIDUp
    if ($.browser.msie && document.documentMode == 7) {
        $("#btnGroupIDUp").css("margin-left", "-2");
    }
    $("#btnGroupIDUp").get(0).sort = function() {
        BlockUI();
        
        jlinq.from(ConsolidatedGroup.dataTableCustomerGroup).sort("-GroupID").select();
        
        ConsolidatedGroup.AllGroup.loadGrid();
        ConsolidatedGroup.AllGroup.fixScrollBar();
        jQuery.unblockUI();
        $(this).parents("tr:first").hide();
        $("#btnGroupIDDown").parents("tr:first").show();
        
        $("#btnUsernameUp").parents("tr:first").show();
        $("#btnUsernameDown").parents("tr:first").show();
    };
    $("#btnGroupIDUp").click(function() {
        this.sort();
    });
    
    //sort: btnGroupIDDown
    $("#btnGroupIDDown").get(0).sort = function() {
        BlockUI();
        
        jlinq.from(ConsolidatedGroup.dataTableCustomerGroup).sort("GroupID").select();
        
        ConsolidatedGroup.AllGroup.loadGrid();
        ConsolidatedGroup.AllGroup.fixScrollBar();
        jQuery.unblockUI();
        $(this).parents("tr:first").hide();
        $("#btnGroupIDUp").parents("tr:first").show();
        
        $("#btnUsernameUp").parents("tr:first").show();
        $("#btnUsernameDown").parents("tr:first").show();
    };
    $("#btnGroupIDDown").click(function() {
        this.sort();
    });
    
    //sort: btnUsernameUp
    $("#btnUsernameUp").get(0).sort = function() {
        BlockUI();
        
        jlinq.from(ConsolidatedGroup.dataTableCustomerGroup).sort("-UserName").select();
            
        ConsolidatedGroup.AllGroup.loadGrid();
        ConsolidatedGroup.AllGroup.fixScrollBar();
        jQuery.unblockUI();
        
        $(this).parents("tr:first").hide();
        $("#btnUsernameDown").parents("tr:first").show();
        
        $("#btnGroupIDUp").parents("tr:first").show();
        $("#btnGroupIDDown").parents("tr:first").show();
    };
    $("#btnUsernameUp").click(function() {
        this.sort();
    });
    
    //sort: btnUsernameDown
    $("#btnUsernameDown").get(0).sort = function() {
        BlockUI();
        
        jlinq.from(ConsolidatedGroup.dataTableCustomerGroup).sort("UserName").select();
            
        ConsolidatedGroup.AllGroup.loadGrid();
        ConsolidatedGroup.AllGroup.fixScrollBar();
        jQuery.unblockUI();
        
        $(this).parents("tr:first").hide();
        $("#btnUsernameUp").parents("tr:first").show();
        
        $("#btnGroupIDUp").parents("tr:first").show();
        $("#btnGroupIDDown").parents("tr:first").show();
    };
    $("#btnUsernameDown").click(function() {
        this.sort();
    });
    
    //filter: divCurrencyHeader
    $("#divCurrencyHeader").click(function(event) {
        if (typeof $("#divListCurrency").get(0).isShowed == "undefined" || $("#divListCurrency").get(0).isShowed == false) {
            $("#divListCurrency").show();
            $("#divListCurrency").get(0).isShowed = true;
        }
        else {
            $("#divListCurrency").hide();
            $("#divListCurrency").get(0).isShowed = false;
        }
        event.stopPropagation();
    });
    
    //selectListCurrency
    $(document).click(function() {
        $("#divListCurrency").hide();
        $("#divListCurrency").get(0).isShowed = false;
    });
    $("#divListCurrency").click(function(event) {
        event.stopPropagation();
    });
    $("#selectListCurrency").click(function(event) {
        event.stopPropagation();
    })
    .change($.proxy(function() {
        $("#divListCurrency").hide();
        $("#divListCurrency").get(0).isShowed = false;
        
        //Search by currency
        BlockUI();
        if ($("#selectListCurrency").val() != "-1") {
            ConsolidatedGroup.dataTableCustomerGroup = jlinq.from(ConsolidatedGroup.dataTableCustomerGroupBackup)
                .equals("Currency", $("#selectListCurrency").val())
                .select();
        }
        else {
            ConsolidatedGroup.dataTableCustomerGroup = jlinq.from(ConsolidatedGroup.dataTableCustomerGroupBackup)
                .select();
        }
        this.loadGrid();
        this.fixScrollBar();  
        jQuery.unblockUI();
    }, this));
    
    //load grid
    ConsolidatedGroup.AllGroup.viewAllGroup();
};

ConsolidatedGroup.AllGroup.editGroup = function (GroupID) {
    BlockUI();
    $("#divViewAllGroup").hide();
    $("#divViewAllGroupHeader").hide();
    
    $("#divEditGroupHeader").show();
    $("#divTitle").html("Edit Group");
    $("#divGroupID").text(GroupID);
    $("#hiddenEditGroupGroupID").val(GroupID);
    $("#tbxUsernameEdit").val('');
    
    ConsolidatedGroup.EditGroup.loadData(GroupID);
};

ConsolidatedGroup.EditGroup.loadData = function (GroupID) {
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/GetMemberByGroupID",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"GroupID":' + GroupID + '}',
            success: $.proxy(function (rs)
            {
                this.consolidatedMemberTable = rs;
                this.fillData();
                jQuery.unblockUI();
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.EditGroup.loadDataAfterRemove = function (GroupID) {
    $.ajax({
            type: "Post",
            url: "Service/ConsolidatedGroupService.svc/GetMemberByGroupID",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"GroupID":' + GroupID + '}',
            success: $.proxy(function (rs)
            {
                this.consolidatedMemberTable = rs;
                this.fillData();
                jQuery.unblockUI();
                var msg = {
                    ErrorCode: 0
                    , ErrorMessage: "Remove user successfully."
                };
                ConsolidatedGroup.utils.showMessage(msg);
            }, this),
            error: function(msg) {
                ConsolidatedGroup.utils.showMessage(msg);
                jQuery.unblockUI();
            }
        });
};

ConsolidatedGroup.EditGroup.fillData = function() {
    ko.applyBindings({consolidatedMemberTable: this.consolidatedMemberTable}, $("#divEditGroupHeader").get(0));
    ko.applyBindings({consolidatedMemberTable: this.consolidatedMemberTable}, $("#divEditGroup").get(0));
    $("#scriptEditGroup").tmpl(this.consolidatedMemberTable).appendTo($("#tbodyEditGroup").empty());
    
    $("#tbodyEditGroup").find("tr:last td").each(function(index, element) {
        element.style.borderBottom = "0px none";
    });
    
    $("#tbodyEditGroup").find("tr").each(function(index, element) {
        if (index % 2 != 0) {
            element.style.backgroundColor = "#DFEAFF";
        }
        else {
            element.style.backgroundColor = "white";
        }
    });
    
    $("#divEditGroup").show();
    ConsolidatedGroup.utils.Custom.init("tbodyEditGroup");
    $("#divEditGroupScrollBar").width(ConsolidatedGroup.utils.getScrollbarWidth());
    
    //fix at scroll bar y, for FireFox
    if (this.consolidatedMemberTable.length == 1 && ConsolidatedGroup.utils.isIE() == false) {
        $("#divBodyEditGroup").height($("#divBodyEditGroup").height() - 15);
    }
    //setting checkbox
    $("#divBodyEditGroup").find("table tbody tr td input[type='checkbox']").each(function(index, element) {
        $(element).attr("onclick", "javascript:ConsolidatedGroup.EditGroup.checkBox_Click(event);");
    });
    
    //setheadline event handler
    $("#tbodyEditGroup").find("img[setheadline='true']").each(function(index, element){
        $(element).click(function(event){
            ConsolidatedGroup.EditGroup.changeHeadlineHandler(event);
        });
    });
    $("#cbxEditGroupHeader").get(0).checked = false;
    $("#cbxEditGroupHeader").parent().find("span[class='checkboxstyled']").get(0).style.backgroundPosition = "0 0";
    
    $("#btnEditGroupUserNameSortUp").parent().show();
    $("#btnEditGroupUserNameSortDown").parent().show();
};

ConsolidatedGroup.EditGroup.changeHeadlineHandler = function(event) {
            BlockUI();
            var target = event.target;
            var custid = $(target).parent().find("input[type='hidden'][custid='true']").val();
            var custName = $(target).parent().find("input[type='hidden'][username='true']").val();
            $.ajax({
                    type: "Post",
                    url: "Service/ConsolidatedGroupService.svc/ChangeHeadline",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{"GroupID": ' + $("#hiddenEditGroupGroupID").val() + ', "CustID": ' + custid + ', "CustName": "' + custName + '"}',
                    success: $.proxy(function (msg)
                    {
                        if (typeof msg != "undefined" && typeof msg.ErrorCode != "undefined" && msg.ErrorCode == 0) {
                            msg.ErrorMessage = "Change headline successfully.";
                            /*//update viewmodel
                            var head_custid_old = $("#tbodyEditGroup").find("tr td div img[imgheadline='true']").parent()
                                .find("input[type='hidden'][custid='true']").val();
                            var head_custid_new = $(target).parent()
                                .find("input[type='hidden'][custid='true']").val();
                            
                            var rows_head_old = jlinq.from(ConsolidatedGroup.EditGroup.consolidatedMemberTable).equals("CustID", head_custid_old).select();
                            if (rows_head_old.length > 0) {
                                rows_head_old[0].IsHeader = false;
                            }
                           
                            var rows_head_new = jlinq.from(ConsolidatedGroup.EditGroup.consolidatedMemberTable).equals("CustID", head_custid_new).select();
                            if (rows_head_new.length > 0) {
                                rows_head_new[0].IsHeader = true;
                            }
                           
                            //update gui
                            $("#tbodyEditGroup").find("tr td div img[imgheadline='true']")
                                .attr("src", "img/headline.gif")
                                .removeAttr("imgheadline")
                                .attr("setheadline", "true")
                                .click(function(event){
                                    ConsolidatedGroup.EditGroup.changeHeadlineHandler(event);
                                })
                                .parents("tr:first").find("input[type='hidden'][isheader='true']").each(function(index, element){
                                    $(element).val("false");
                                });
                            
                            $(target).attr("src", "img/headlineH.gif")
                                .removeAttr("setheadline")
                                .attr("imgheadline", "true")
                                .unbind()
                                .parents("tr:first").find("input[isheader='true'][type='hidden']").each(function(index, element){
                                $(element).val("true");
                            });
                            ConsolidatedGroup.EditGroup.hadChangeHeadline = true;*/
                            ConsolidatedGroup.EditGroup.loadData($("#hiddenEditGroupGroupID").val());
                        }
                        jQuery.unblockUI();
                        ConsolidatedGroup.utils.showMessage(msg);
                    }, this),
                    error: function(msg) {
                        ConsolidatedGroup.utils.showMessage(msg);
                        jQuery.unblockUI();
                    }
                });
        };

ConsolidatedGroup.EditGroup.setValueAllCheckBox = function (checked) {
    $("#divBodyEditGroup").find("table tbody tr td input[type='checkbox']").each(function(index, element) {
        if ($(element).parent().find("input[type='hidden'][isheader='true']").val() != "true") {
            element.checked = checked;    
        }
    });
};

ConsolidatedGroup.EditGroup.checkBox_Click = function(event) {
    var target = event.target || event.srcElement;
    target.checked = !target.checked;
    if (target.checked == true) {
        if ($(target).parent().find("input[type='hidden'][isheader='true']").val() == "true") {
            target.checked = !target.checked;
            ConsolidatedGroup.utils.showMessage("Can not delete headline.", "error");
        }
    }
    if (target.checked == false) {
        $("#cbxEditGroupHeader").get(0).checked = false;
    }
    else {
        var numberChecked = 0;
        $("#divBodyEditGroup").find("table tbody tr td input[type='checkbox']").each(function(index, element) {
            if ($(element).parent().find("input[type='hidden'][isheader='true']").val() != "true") {
                if ($(element).get(0).checked) {
                    ++numberChecked;
                }
            }
        });
        if (numberChecked == this.consolidatedMemberTable.length - 1) {
            $("#cbxEditGroupHeader").get(0).checked = true;
        }
    }
};

ConsolidatedGroup.EditGroup.fixScrollBar = function () {
    if (ConsolidatedGroup.utils.CheckScrollBar("divBodyEditGroup", "y")) {
        var scrollBarWidth = ConsolidatedGroup.utils.getScrollbarWidth();
        $("#divBodyEditGroup").find("table tbody tr div[endcol]").each(function(index, element){
            $(element).width($(element).width() - scrollBarWidth - 1/*1:border-right-width*/);
        });
    }
};

ConsolidatedGroup.AllGroup.search = function(value) {
    BlockUI();
    if (value == "" || value == "By username of headline") {
        ConsolidatedGroup.dataTableCustomerGroup = jlinq.from(ConsolidatedGroup.dataTableCustomerGroupBackup)
            .select();
        this.fillCurrencyList();
    }
    else {
        ConsolidatedGroup.dataTableCustomerGroup = jlinq.from(ConsolidatedGroup.dataTableCustomerGroupBackup)
            .starts("UserName", value)
            .select();
    }
    this.loadGrid();
    this.fixScrollBar();
    jQuery.unblockUI();
    $("#tbxSearch").focus();
};

ConsolidatedGroup.AllGroup.fillCurrencyList = function() {
    var checkExisted = function(arr, value) {
        for(var i = 0; i < arr.length; ++i) {
            if (arr[i].Currency == value) return true;
        }
        return false;
    };
    var listCurrency = function() {
        var arr = [];
        for(var i = 0; i < ConsolidatedGroup.dataTableCustomerGroup.length; ++i) {
            if (!checkExisted(arr, ConsolidatedGroup.dataTableCustomerGroup[i].Currency)) {
                arr[arr.length] = {
                    Currency: ConsolidatedGroup.dataTableCustomerGroup[i].Currency
                    , CurrencyName: ConsolidatedGroup.dataTableCustomerGroup[i].CurrencyName
                };
            }
        }
        return arr;
    };
    var arrCurrency = listCurrency();
    jlinq.from(arrCurrency).sort("CurrencyName").select();
    
    //Option 1: combobox without checkbox
    /*$("#selectListCurrency").empty();
    $("#selectListCurrency").append("<option selected='selected' value='-1'>All</option>");
    for(var i = 0; i < arrCurrency.length; ++i) {
        $("#selectListCurrency").append("<option value='" + arrCurrency[i].Currency + "'>" + arrCurrency[i].CurrencyName + "</option>");
    }*/
    
    //Option 2: table with chekbox
    $("#tbodyCurrencyList").empty();
    
    var strHTML = '';
    
    strHTML += "<tr>";
    strHTML += "<td align='left'>";
    strHTML += "<input all='true' type='checkbox' onclick='javascript:ConsolidatedGroup.AllGroup.filterCurrencyHandler(event);' value='-1' class='styled' checked='checked' />";
    strHTML += "</td>";
    strHTML += "<td align='left'>";
    strHTML += "<span>All</span>";
    strHTML += "</td>";
    strHTML += "</tr>";
    $("#tbodyCurrencyList").append(strHTML);
    
    for(var i = 0; i < arrCurrency.length; ++i) {
        strHTML = '';
        strHTML += "<tr>";
        strHTML += "<td align='left'>";
        strHTML += "<input type='checkbox' onclick='javascript:ConsolidatedGroup.AllGroup.filterCurrencyHandler(event);' value='" 
            + arrCurrency[i].Currency + "' class='styled' checked='checked' />";
        strHTML += "</td>";
        strHTML += "<td align='left'>";
        strHTML += "<span>" + arrCurrency[i].CurrencyName + "</span>";
        strHTML += "</td>";
        strHTML += "</tr>";
        $("#tbodyCurrencyList").append(strHTML);
    }
    ConsolidatedGroup.utils.Custom.init("tbodyCurrencyList");
};

ConsolidatedGroup.AllGroup.filterCurrencyHandler = function(event) {
    if (event.srcElement) event.target = event.srcElement;
    event.target.checked = !event.target.checked;
    //Search by currency
    BlockUI();
    
    if ($(event.target).val() == "-1") {//All
        if (event.target.checked) {
            $("#tbodyCurrencyList").find("input[type='checkbox']").each(function(index, e){
                if ($(e).val() != "-1")
                {
                    e.checked = true;
                }
            });
        }
        else {
            $("#tbodyCurrencyList").find("input[type='checkbox']").each(function(index, e){
                if ($(e).val() != "-1")
                {
                    e.checked = false;
                }
            });
        }
    }
    else {
        if (!event.target.checked) {
            $("#tbodyCurrencyList").find("input[type='checkbox'][all='true']").get(0).checked = false;
        }
        else {
            var numberChecked = 0;
            $("#tbodyCurrencyList").find("input[type='checkbox']").each(function(index, e) {
                if (e.checked) {
                    ++numberChecked;
                }
            });
            if (numberChecked == $("#tbodyCurrencyList").find("input[type='checkbox']").length - 1) {
                $("#tbodyCurrencyList").find("input[type='checkbox'][all='true']").get(0).checked = true;
            }
        }
    }
    
    var arrCurrency = [];
    $("#tbodyCurrencyList").find("input[type='checkbox']").each(function(index, element){
        if (element.checked)
            arrCurrency[arrCurrency.length] = $(element).val();
    });
    var filtered_dataTableCustomerGroup = [];
    for (var i = 0; i < arrCurrency.length; ++i) {
        var dataTableCustomerGroup = jlinq.from(ConsolidatedGroup.dataTableCustomerGroupBackup)
            .equals("Currency", arrCurrency[i])
            .select();
        for (var j = 0; j < dataTableCustomerGroup.length; ++j) {
            filtered_dataTableCustomerGroup.push(dataTableCustomerGroup[j]);
        }
    }
    ConsolidatedGroup.dataTableCustomerGroup = filtered_dataTableCustomerGroup;
    this.loadGrid();
    
    //keep previous sorted
    if ($("#btnUsernameUp").parents("tr:first").is(":visible") && !$("#btnUsernameDown").parents("tr:first").is(":visible")) {
        $("#btnUsernameDown").get(0).sort();
    }
    else if ($("#btnUsernameDown").parents("tr:first").is(":visible") && !$("#btnUsernameUp").parents("tr:first").is(":visible")) {
        $("#btnUsernameUp").get(0).sort();
    }
    else if ($("#btnGroupIDUp").parents("tr:first").is(":visible") && !$("#btnGroupIDDown").parents("tr:first").is(":visible")) {
        $("#btnGroupIDDown").get(0).sort();
    }
    else if ($("#btnGroupIDDown").parents("tr:first").is(":visible") && !$("#btnGroupIDUp").parents("tr:first").is(":visible")) {
        $("#btnGroupIDUp").get(0).sort();
    }
    
    this.fixScrollBar();
    jQuery.unblockUI();
};

ConsolidatedGroup.AllGroup.loadGrid = function() {
    ko.applyBindings(ConsolidatedGroup, $("#divViewAllGroupHeader").get(0));
    ko.applyBindings(ConsolidatedGroup, $("#divViewAllGroup").get(0));
    $("#scriptAllGroup").tmpl(ConsolidatedGroup.dataTableCustomerGroup).appendTo($("#tbodyAllGroup").empty());
    
    $("#tbodyAllGroup").find("tr").each(function(index, element) {
        if (index % 2 != 0) {
            element.style.backgroundColor = "#DFEAFF";
        }
        else {
            element.style.backgroundColor = "white";
        }
    });
    $("#divViewAllGroup").find("table tbody tr:last td").each(function(index, element) {
        element.style.borderBottom = "0px none";
    });
    
    if ($("#tbodyAllGroup").parent().height() < $("#divBodyAllGroup").height()) {
        $("#divBodyAllGroup").height($("#tbodyAllGroup").parent().height());
    }
};

ConsolidatedGroup.AllGroup.fixScrollBar = function () {
    if (ConsolidatedGroup.utils.CheckScrollBar("divBodyAllGroup", "y")) {
        var scrollBarWidth = ConsolidatedGroup.utils.getScrollbarWidth();
        $("#divBodyAllGroup").find("table tbody tr div[endcol]").each(function(index, element){
            $(element).width($(element).width() - scrollBarWidth - 1/*1:border-right-width*/);
        });
    }
};

ConsolidatedGroup.utils.showMessage = function (msg, type) {
    if (msg) {
        if (typeof msg.ErrorCode != "undefined" && typeof msg.ErrorMessage != "undefined" && msg.ErrorCode != 0) {
            /*$.msg({
                content: "<div type='message.custom' class='alertboxerr'><div>" + msg.ErrorMessage + "</div></div>"
                , fadeOut: 200
                , timeOut: 3000
                , klass: 'black-on-black'
            });*/
            $("#divMessage").html("<div type='message.custom' class='alertboxerr'><div>" + msg.ErrorMessage + "</div></div>").show();
        }
        else if (typeof msg.ErrorMessage != "undefined") {
            /*$.msg({
                content: "<div type='message.custom' class='alertboxscc'><div>" + msg.ErrorMessage + "</div></div>"
                , fadeOut: 200
                , timeOut: 3000
                , klass: 'black-on-black'
            });*/
            $("#divMessage").html("<div type='message.custom' class='alertboxscc'><div>" + msg.ErrorMessage + "</div></div>").show();
        }
        else if (typeof msg == "string" && typeof type == "string" && type == "error") {
            /*$.msg({
                content: "<div type='message.custom' class='alertboxerr'><div>" + msg + "</div></div>"
                , fadeOut: 200
                , timeOut: 3000
                , klass: 'black-on-black'
            });*/
            $("#divMessage").html("<div type='message.custom' class='alertboxerr'><div>" + msg + "</div></div>").show();
        }
    }
};

ConsolidatedGroup.utils.hideMessage = function () {
    $("#divMessage").hide();
};

ConsolidatedGroup.utils.showTooltip = function (obj, content) {
    if (!obj) return;
    obj = $(obj);
    obj.CreateBubblePopup();
    obj.ShowBubblePopup({
        innerHtml: content,
        alwaysVisible: false,
        closingDelay: 200,
        themeName: 'azure',
        themePath: '../../js/jQuery/Bubblepopup/images/jquerybubblepopup-theme'
    });
};

ConsolidatedGroup.utils.isIE = function () {
    if (navigator.appName == "Microsoft Internet Explorer") {
        return true;
    }
    return false;
};

ConsolidatedGroup.utils.CheckScrollBar = function (ObjectId, Aces) {
    if (!Aces) { Aces = "either x or y"; }
    return this.IsScrollBarExist(ObjectId, Aces);
};

ConsolidatedGroup.utils.IsScrollBarExist = function (ObjectId, Aces) {
    if (!ObjectId) return false;
    if (typeof ObjectId != "string") return false;
    if (this.isIE()) {
        var hasScrollbar = function (elementId) {
            var div = document.getElementById(elementId); // need real DOM Node, not jQuery wrapper
            if (!div) return false;
            var hasVerticalScrollbar = div.scrollHeight > div.clientHeight + 1;
            var hasHorizontalScrollbar = div.scrollWidth > div.clientWidth + 1;
            return { "hasVerticalScrollbar": hasVerticalScrollbar, "hasHorizontalScrollbar": hasHorizontalScrollbar };
        };
        var srcollbarDetechResult = hasScrollbar(ObjectId);
        switch (Aces) {
            case 'x':
                return srcollbarDetechResult.hasHorizontalScrollbar;
                break;
            case 'y':
                return srcollbarDetechResult.hasVerticalScrollbar;
                break;
            default:
                return false;
                break;
        }
    }
    var Object = document.getElementById(ObjectId);
    if (!Object) return false;
    switch (Aces) {
        case 'x':
            Object.scrollLeft = 1;
            if (Object.scrollLeft > 0) { Object.scrollLeft = 0; return true; }
            else { return false; }
            break;
        case 'y':
            Object.scrollTop = 1;
            if (Object.scrollTop > 0) { Object.scrollTop = 0; return true; }
            else { return false; }
            break;
        case 'xy':
            Object.scrollTop = 1;
            Object.scrollLeft = 1;
            if (Object.scrollTop > 0 && Object.scrollLeft > 0) { Object.scrollTop = 0; Object.scrollLeft = 0; return true; }
            else { return false; }
            break;
        default:
            Object.scrollTop = 1;
            Object.scrollLeft = 1;
            if (Object.scrollTop > 0 || Object.scrollLeft > 0) { Object.scrollTop = 0; Object.scrollLeft = 0; return true; }
            else { return false; }
            break;
    }
};

ConsolidatedGroup.utils.getScrollbarWidth = function () {
    var scrollbarWidth;
    var div = $('<div style="width:50px;height:50px;overflow:hidden;position:absolute;top:-200px;left:-200px;"><div style="height:100px;"></div></div>');
    $('body').append(div);
    var w1 = $('div', div).innerWidth();
    div.css('overflow-y', 'auto');
    var w2 = $('div', div).innerWidth();
    $(div).remove();
    scrollbarWidth = (w1 - w2);
    return scrollbarWidth;
};

ConsolidatedGroup.utils.hideAutoComplete = function(target) {
    $(target).parent().find("div[popupcontianer]").remove();
};

ConsolidatedGroup.utils.autoComplete = function (target, arrayValues, key) {
    if (!target) return;
    if (typeof target == "string") {
        if (target.indexOf("#") < 0)
            target = $("#" + target);
        else target = $(target);
    }
    else {
        target = $(target);
    }

    $(target).attr("autocomplete", "off");
    $(target).parent().find("div[popupcontianer]").remove();
    
    if (arrayValues.length <= 0) {
        return;
    }
    
    $(target).parent().append("<div popupcontianer='true'></div>");
    var container = $(target).parent().find("div[popupcontianer]");

    var htmlList = "";
    var itemStyle = " style='padding-left:4px; padding-top:2px; padding-botton:2px;' ";

    for (var i = 0; i < arrayValues.length; ++i) {
        htmlList += "<div" + itemStyle + " index='" + (i + 1) + "' value='" + arrayValues[i][key] + "'>" + arrayValues[i][key] + "</div>";
    }

    container.append("<div style='border:1px solid #1C94CD; background-color:white;position:absolute;' listpopup='true'></div>");
    var divListPopup = container.find("div[listpopup]");

    divListPopup.width(target.width());
    divListPopup.empty().append(htmlList);
    var height = 0;
    divListPopup.find("div").each(function (index, element) {
        height += $(element).height() + 2;
    });
    divListPopup.height(height);
    divListPopup.get(0).selectedIndex = 0;
    //---------------------------------------------------------------
    var oldHover = null;
    divListPopup.find("div").hover(function (e) {
        if (oldHover != null) {
            $(oldHover).get(0).style.backgroundColor = "white";
        }
        var _target = ConsolidatedGroup.utils.getTarget(e);
        $(_target).get(0).style.backgroundColor = "#DFDFDF";
        oldHover = _target;
        divListPopup.get(0).selectedIndex = parseInt($(_target).attr("index"));
    })
    .click(function (e) {
        var _target = ConsolidatedGroup.utils.getTarget(e);
        $(target).val($(_target).attr('value'));
        container.remove();
    })
    .highlight($(target).val());
    //---------------------------------------------------------------
    target.keyup(function (e) {
        if (divListPopup.parent().parent().length <= 0) {
            return;
        }
        if (divListPopup.find("div").length <= 0 || divListPopup.get(0).style.display == "none") {
            return;
        }
        //down key
        if (e.keyCode == 40) {
            if (divListPopup.get(0).selectedIndex == arrayValues.length) {
                return;
            }
            divListPopup.get(0).selectedIndex += 1;
            if (oldHover) {
                $(oldHover).get(0).style.backgroundColor = "white";
            }
            if (divListPopup.find("div").length >= divListPopup.get(0).selectedIndex) {
                divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0).style.backgroundColor = "#DFDFDF";
                oldHover = divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0);
                $(target).val($(oldHover).attr('value'));
                target.focus();
                target.select();
            }
        }
        else if (e.keyCode == 38) {//up key
            if (divListPopup.get(0).selectedIndex == 1 || divListPopup.get(0).selectedIndex == 0) {
                return;
            }
            divListPopup.get(0).selectedIndex -= 1;
            if (oldHover) {
                $(oldHover).get(0).style.backgroundColor = "white";
            }
            if (divListPopup.find("div").length >= divListPopup.get(0).selectedIndex) {
                divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0).style.backgroundColor = "#DFDFDF";
                oldHover = divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0);
                $(target).val($(oldHover).attr('value'));
                target.focus();
                target.select();
            }
        }
        else if (e.keyCode == 13) {//Enter press
            if (oldHover) {
                $(target).val($(oldHover).attr('value'));
            }
            oldHover = null;
            container.remove();
        }
        else if (e.keyCode == 27) {//ESC press
            oldHover = null;
            container.remove();
        }
    });
    $(document).click(function () {
        oldHover = null;
        container.remove();
    });
    divListPopup.show();
};

ConsolidatedGroup.utils.isAlphanumeric = function (e) {
    var key;
    if (e.keyCode) {
        key = e.keyCode;
    }
    else if (e.which) {
        key = e.which;
    }
    return /[A-Za-z0-9 ]/.test(String.fromCharCode(key));
};

ConsolidatedGroup.utils.getTarget = function (e) {
    if (!e) {
        return null;
    }
    e = e || window.event;
    return e.target || e.srcElement;
};

ConsolidatedGroup.utils.Custom = {
    init: function(container) {
        if (typeof container == "string") {
            if (container.indexOf("#") >= 0) {
                container = $(container);
            }
            else {
                container = $("#" + container);
            }
        }
        else if (typeof container != "undefined" || container != null) {
            container = $(container);
        }
        var inputs = container.get(0).getElementsByTagName("input"), span = Array(), textnode, option, active;
        for (a = 0; a < inputs.length; a++) {
            if ((inputs[a].type == "checkbox" || inputs[a].type == "radio") && inputs[a].className == "styled") {
                span[a] = document.createElement("span");
                span[a].className = inputs[a].type == "checkbox" ? "checkboxstyled" : "radiostyled";
                if (inputs[a].checked == true) {
                    if (inputs[a].type == "checkbox") {
                        position = "0 -" + (checkboxHeight * 2) + "px";
                        span[a].style.backgroundPosition = position;
                    } else {
                        position = "0 -" + (radioHeight * 2) + "px";
                        span[a].style.backgroundPosition = position;
                    }
                }
                inputs[a].parentNode.insertBefore(span[a], inputs[a]);
                inputs[a].onchange = this.clear;
                if (!inputs[a].getAttribute("disabled")) {
                    span[a].onmousedown = this.pushed;
                    $(span[a]).mouseup($.proxy(function(event){
                        this.check(event);
                    }, this));
                } else {
                    span[a].className = span[a].className += " disabled";
                }
            }
        }

        $(document).mouseup($.proxy(function(){
            this.clear();
        }, this));
    },
    pushed: function(evt) {
        var evt1;
        if (null != window.event && 'object' == typeof (window.event)) {
            evt1 = event.button;
        }
        else {
            evt1 = evt.button;
        }
        if (evt1 < 2) {
            element = this.nextSibling;
            if (element.checked == true && element.type == "checkbox") {
                this.style.backgroundPosition = "0 -" + checkboxHeight * 3 + "px";
            } else if (element.checked == true && element.type == "radio") {
                this.style.backgroundPosition = "0 -" + radioHeight * 3 + "px";
            } else if (element.checked != true && element.type == "checkbox") {
                this.style.backgroundPosition = "0 -" + checkboxHeight + "px";
            } else {
                this.style.backgroundPosition = "0 -" + radioHeight + "px";
            }
        }
    },
    check: function(evt) {
        var evt1;
        if (null != window.event && 'object' == typeof (window.event)) {
            evt1 = event.button;
        }
        else {
            evt1 = evt.button;
        }
        if (evt1 < 2) {
            element = evt.target.nextSibling;
            if (element.checked == true && element.type == "checkbox") {
                evt.target.style.backgroundPosition = "0 0";
                element.checked = false;

            } else {
                if (element.type == "checkbox") {
                    evt.target.style.backgroundPosition = "0 -" + checkboxHeight * 2 + "px";
                } else {
                    evt.target.style.backgroundPosition = "0 -" + radioHeight * 2 + "px";
                    group = evt.target.nextSibling.name;
                    inputs = document.getElementsByTagName("input");
                    for (a = 0; a < inputs.length; a++) {
                        if (inputs[a].name == group && inputs[a] != evt.target.nextSibling) {
                            inputs[a].previousSibling.style.backgroundPosition = "0 0";
                        }
                    }
                }
                element.checked = true;
            }
            if (this.hasEvent(element, "onclick")) {
                if ($.browser.msie && (parseFloat($.browser.version) == 7 || document.documentMode == 7)) {
                    eval($(element).attr("onclick"));
                }
                else {
                    element.click();
                }
            }
        }
    },
    clear: function() {
        inputs = document.getElementsByTagName("input");
        for (var b = 0; b < inputs.length; b++) {
            if (inputs[b].previousSibling == null) {
                continue;
            }
            if (typeof inputs[b].previousSibling == "undefined" || typeof inputs[b].previousSibling.style == "undefined") {
                continue;
            }
            if (inputs[b].type == "checkbox" && inputs[b].checked == true && inputs[b].className == "styled") {
                inputs[b].previousSibling.style.backgroundPosition = "0 -" + checkboxHeight * 2 + "px";

            } else if (inputs[b].type == "checkbox" && inputs[b].className == "styled") {
                inputs[b].previousSibling.style.backgroundPosition = "0 0";
            } else if (inputs[b].type == "radio" && inputs[b].checked == true && inputs[b].className == "styled") {
                inputs[b].previousSibling.style.backgroundPosition = "0 -" + radioHeight * 2 + "px";
            } else if (inputs[b].type == "radio" && inputs[b].className == "styled") {
                inputs[b].previousSibling.style.backgroundPosition = "0 0";
            }
        }
    },
    choose: function() {
        option = this.getElementsByTagName("option");
        for (d = 0; d < option.length; d++) {
            if (option[d].selected == true) {
                document.getElementById("select" + this.name).childNodes[0].nodeValue = option[d].childNodes[0].nodeValue;
            }
        }
    }
    ,hasEvent: function(elem, evt) {
        if (elem != null) {
            if (eval('elem.' + evt))
                return true;
        }
        return false;
    }
};