var ajaxService = (function ($) {
    "use strict";
    var self = {};

    self.getGroups = function (successCallback) {
        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/GetGroupNames",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                successCallback(data);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    };

    self.getSuperList = function (params, successCallback) {
        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/GetSuperList",
            contentType: "application/json; charset=utf-8",
            data: $.toJSON(params),
            beforeSend: function () {
                try { parent.BlockUI(); } catch (e) { }
            },
            success: function (data) {
                successCallback(data);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    };

    self.addSuperToGroup = function (groupName, superName, successCallback) {
        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/AddSuperToGroup",
            contentType: "application/json; charset=utf-8",
            data: $.toJSON({ groupName: groupName, username: superName }),
            success: function (data) {
                successCallback(data);
            }
        });
    };

    self.removeSuperFromGroup = function (custId, successCallback) {
        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/RemoveSuperFromGroup",
            contentType: "application/json; charset=utf-8",
            data: $.toJSON({ custId: custId }),
            success: function (data) {
                successCallback(data);
            }
        });
    };

    self.changePrimaryGroup = function (params, successCallback) {
        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/ChangePrimaryGroup",
            contentType: "application/json; charset=utf-8",
            data: $.toJSON(params),
            success: function (data) {
                successCallback(data);
            }
        });
    };

    return self;
}(jQuery));

var changeGroupService = (function ($, ajaxService) {
    "use strict";
    var self = {};

    var $changeGroupDialog = $("#changeGroupDialog").dialog({
            title: "Change Primary Group",
            autoOpen: false,
            height: 340,
            width: 360,
            modal: true,
            open: function (event, ui) {
                //$("#txtEffectiveDate").click(function () {
                //    $(this).datepicker("show");
                //});
                //$effectiveDate.datepicker("hide");
            },
            close: function (event, ui) {
                //$(ui).find("#txtEffectiveDate").datepicker("destroy");
            }
        });

    var $changeGroupErrorMsg = $("#changeGroupErrorMsg"),
        $spanSuperName = $("#spanSuperName");
    var $effectiveDate;

    var $grid = $("#changeGroupGrid"),
        $table = $("#changeGroupTable");

    var currentPrimaryGroup;

    function assignGridBodyRows(extraPTGroups) {
        var html = [
            "<tr>",
            '<td><div class="w40 c">1</div></td>',
            '<td><div class="w180 noExtraPT">No Extra PT</div></td>',
            '<td><div class="w120 c"><input type="radio" name="isPrimary" value=""></div></td>',
            "</tr>"
        ];

        $.each(extraPTGroups, function (index, group) {
            html.push("<tr>");
            html.push('<td><div class="w40 c">');
            html.push(index + 2);
            html.push("</div></td>");
            html.push('<td><div class="w180');

            if (group.IsPrimary) {
                currentPrimaryGroup = group;
                html.push(' b');
            }

            html.push('">');
            html.push(group.GroupName);

            if (group.EffectiveDate) {
                html.push(" (");
                html.push(group.EffectiveDate);
                html.push(")");
            }

            html.push("</div></td>");
            html.push('<td><div class="w120 c">');
            html.push('<input type="radio" name="isPrimary" value="');
            html.push(group.GroupName);
            html.push('"');

            if (group.IsPrimary) {
                html.push(' checked="checked"');
            }

            html.push(" />");
            html.push("</div></td>");
            html.push("</tr>");
        });

        $table.html(html.join(""));
    }

    function changePrimaryGroup() {
        var newPrimaryGroup = $table.find('input[name=isPrimary]:checked').val();

        if (newPrimaryGroup === currentPrimaryGroup.GroupName) {
            showAlertMessage(
                "changeGroupErrorMsg",
                "The group " + currentPrimaryGroup.GroupName + " has already been the primary group since " + currentPrimaryGroup.EffectiveDate + ".", 1);
            return;
        }

        var newEffectiveDate = $effectiveDate.val();

        if (newEffectiveDate === today) {
            var sayYes = confirm("Changing the Primary Extra PT group which take effect from today might have impact on the reset amounts in the Reset Winning report.\nAre you sure you want to change?");

            if (!sayYes) {
                return;
            }
        }

        var params = {
            username: $spanSuperName.html(),
            primaryGroup: newPrimaryGroup,
            effectiveDate: newEffectiveDate
        };

        ajaxService.changePrimaryGroup(params, function () {
            $changeGroupDialog.addClass("hidden").dialog("close");
        });
    }

    self.initialize = function (today) {
        var dateOption = {
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: new Date(today)
        };

        $effectiveDate = $("#txtEffectiveDate").datepicker(dateOption);

        $("#changeGroupSubmit").click(changePrimaryGroup);
    };

    self.showDialog = function (superName, effectiveDate, extraPTGroups) {
        $changeGroupErrorMsg.empty();
        $spanSuperName.html(superName);
        $effectiveDate.datepicker("setDate", effectiveDate);
        assignGridBodyRows(extraPTGroups);
        
        $changeGroupDialog.removeClass("hidden").dialog("open");

        $grid.reRenderScroller(320, 184, true);
    };

    return self;
}(jQuery, ajaxService));

(function ($, ajaxService, changeGroupService) {
    "use strict";

    var $trMsg = $("#trMsg"),
        $groupName = $("#groupName"),
        $username = $("#txt_search"),
        $grid = $("#gridExAccControl"),
        $tblExternalSite = $("#tblexternalsite"),
        $pagination = $("#pagination"),
        $nodata = $("#nodata"),
        $errNoData = $("#err_nodata");

    var formData = null,
        totalItems = 0,
        gridOnlyOneRow = false;

    var _pageSize = 50;

    var $addSuperToGroupDialog = $("#addSuperToGroupDialog").dialog({
            title: "Add Existing Super",
            autoOpen: false,
            height: 100,
            width: 350,
            modal: true
        }),
        $errMsg = $("#errMsg"),
        $superName = $("#txtSuper");

    function showAddSuperToGroupDialog() {
        $errMsg.addClass("hidden");
        $superName.val("");
        $addSuperToGroupDialog.removeClass("hidden").dialog("open");
    }

    function hideAddSuperToGroupDialog() {
        $addSuperToGroupDialog.addClass("hidden").dialog("close");
    }

    function addSuperToGroup() {
        var superName = $.trim($superName.val());

        if (superName.length === 0) {
            $errMsg.removeClass("hidden");
            showAlertMessage("errMsg", "Please input username!", 1);
            return;
        }

        var addSuper = function (result) {
            if (result.ErrorCode === 0) {
                showAlertMessage("errMsg", "Added Successfully!", 0);

                setTimeout(hideAddSuperToGroupDialog, 2000);
                setTimeout(function () {
                    formData.fromcache = false;
                    formData.pageIndex = 1;
                    getSuperList(formData);
                }, 3000);
            }
            else {
                showAlertMessage("errMsg", result.ErrorMessage, 1);
            }

            $errMsg.removeClass("hidden");
        };

        ajaxService.addSuperToGroup(formData.groupName, superName, addSuper);
    }

    function removeSuperFromGroup(custId) {
        $trMsg.addClass("hidden");

        var sayYes = confirm("Are you sure you want to remove this super?");
        if (sayYes) {
            var removeSuper = function (rs) {
                if (rs.ErrorCode === 0) {
                    showAlertMessage("dvMsg", "Removed Successfully", 0);

                    setTimeout(function () {
                        if (gridOnlyOneRow) {
                            formData.pageIndex = 1;
                        }

                        formData.fromcache = false;
                        getSuperList(formData);
                    }, 2000);
                }
                else {
                    showAlertMessage("dvMsg", rs.ErrorMessage, 1);
                }

                $trMsg.removeClass("hidden");
            };

            ajaxService.removeSuperFromGroup(custId, removeSuper);
        }
    }

    function renderGroups(groups) {
        var groupLength = groups ? groups.length : 0;

        if (groupLength > 0) {
            var html = [];

            for (var i = 0; i < groupLength; i++) {
                html.push('<option value="');
                html.push(groups[i]);
                html.push('">');
                html.push(groups[i]);
                html.push("</option>");
            }

            $groupName.html(html.join(""));
        }
    }

    function getFormData() {
        return {
            groupName: $groupName.val(),
            username: $.trim($username.val()),
            fromcache: false,
            pageIndex: 1,
            pageSize: _pageSize
        };
    }

    function renderExtraPTGroup(extraPTGroup) {
        if (!extraPTGroup || extraPTGroup.length === 0) {
            return "";
        }

        var html = [];
        var date;
        var groupArray;
        var extraPTGroupArray = extraPTGroup.split(";");

        $.each(extraPTGroupArray, function (index, group) {
            // string pattern: groupName,date,isActive.
            groupArray = group.split(",");
            date = groupArray[1];

            if (date.length !== 0) {
                date = " (" + date + ")";
            }

            if (groupArray[2] === "1") {
                html.push('<span class="b">' + groupArray[0] + date + "</span>");
            }
            else {
                html.push(groupArray[0] + date);
            }
        });

        return html.join(", ") + '<img class="changeGroup" style="cursor: pointer; float: right" src="../../App_Themes/Images/icn_edit.png" title="Change Primary Group" />';
    }

    function renderBodyRows(index, row) {
        var rowHtml = [
            '<tr><td><div class="w40 c">',
            formData.pageSize * (formData.pageIndex - 1) + index + 1,
            "</div></td>",
            '<td><div class="w180 c">',
            row.UserName,
            "</div></td>",
            '<td><div class="w160 c">',
            row.FirstName,
            "</div></td>",
            '<td><div class="w160 c">',
            row.LastName,
            "</div></td>",
            '<td><div class="w160">',
            row.NormalGroup.replace(";", ", "),
            "</div></td>",
            '<td><div class="w160">',
            renderExtraPTGroup(row.ExtraPTGroup),
            "</div></td>",
            '<td><div class="w120 c">',
            row.IPLogin,
            "</div></td>",
            '<td><div class="w40 c">',
            '<img class="hand removeSuper" src="../../App_Themes/Images/icn_del.png" title="Remove Super" data-custid="',
            row.CustId,
            '" />',
            "</div></td></tr>"
        ];

        return rowHtml.join("");
    }

    function initPager(items_per_page, current_page) {
        var opt = {
            callback: pageselectCallback,
            items_per_page: items_per_page,
            current_page: current_page
        };

        $pagination.pagination(totalItems, opt);
    }

    function pageselectCallback(page_index, _items_per_page, jq) {
        var max_page = Math.ceil(totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        formData.fromcache = true;
        formData.pageIndex = page_index + 1;
        formData.pageSize = _items_per_page;
        _pageSize = _items_per_page;

        getSuperList(formData);
        return false;
    }

    function getSuperList(params) {
        $trMsg.addClass("hidden");
        $nodata.addClass("hidden");
        $errNoData.addClass("hidden");

        formData = params || getFormData();

        var getSupers = function (rs) {
            totalItems = rs.Total;
            gridOnlyOneRow = rs.Rows.length === 1;

            if (totalItems > 0) {
                var html = [];

                $.each(rs.Rows, function (i, row) {
                    html.push(renderBodyRows(i, row));
                });

                $tblExternalSite.html(html.join(""));

                initPager(formData.pageSize, formData.pageIndex - 1);

                $(".changeGroup")
                    .unbind("click")
                    .bind("click", function () {
                        var extraPTGroups = [
                            { GroupName: "ACCIMBC" },
                            { GroupName: "ACCINDO" },
                            { GroupName: "ACCIRR" },
                            { GroupName: "ACCNTD" },
                            { GroupName: "ACCTHAI", IsPrimary: true, EffectiveDate: "05/19/2016" }
                        ];

                        changeGroupService.showDialog("ITVNSuperTestLCS3", tomorrow, extraPTGroups);
                    });

                $(".removeSuper")
                    .unbind("click")
                    .bind("click", function () {
                        removeSuperFromGroup($(this).attr("data-custid"));
                    })

                $grid.removeClass("hidden")
                    .reRenderScroller(1044, 760, true);
            }
            else {
                if (formData.username.length !== 0) {
                    showAlertMessage("err_nodata", "Username does not belong to " + formData.groupName + " group.", 1);
                    $errNoData.removeClass("hidden");
                }

                $nodata.removeClass("hidden");
                $grid.addClass("hidden");
            }
        };

        ajaxService.getSuperList(formData, getSupers);
    }

    function initialize() {
        ajaxService.getGroups(renderGroups);

        $username.keyup(function (e) {
            var code = e.keyCode || e.which;
            if (code === 13) {
                getSuperList();
            }
        });

        $("#btnSearch").click(function () {
            getSuperList();
        });

        $(".btnRefresh").click(function () {
            formData.fromcache = false;
            formData.pageIndex = 1;
            getSuperList(formData);
        });

        $(".btnAdd").click(showAddSuperToGroupDialog);

        $("#addSuperToGroup").click(addSuperToGroup);

        changeGroupService.initialize(today);
    }

    initialize();
})(jQuery, ajaxService, changeGroupService);
