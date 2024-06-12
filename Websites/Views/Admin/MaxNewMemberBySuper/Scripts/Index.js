function GetData(pageIndex, pageSize) {
    $('#errMsg1').html('');
    var params = {
        SuperName: $("#SuperName").val(),
        Site: $("#Site").multiSelect("getCheckedItems").join(","),
        PageSize: pageSize,
        PageIndex: pageIndex
    };

    $.ajax({
        type: "POST",
        url: "Search",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(params),
        dataType: "json",
        beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
        success: function (data) {
            var bodyRows = [];
            var rowCount = data.ListSuper.length;

            for (var i = 0 ; i < rowCount; i++) {
                var row = { Columns: [] };
                row.Columns.push({ Name: 'RowNum', Value: data.ListSuper[i].RowNum });
                row.Columns.push({ Name: 'SuperName', Value: data.ListSuper[i].SuperName });
                row.Columns.push({ Name: 'Site', Value: data.ListSuper[i].Site });
                row.Columns.push({ Name: 'NumberOfMember', Value: data.ListSuper[i].NumberOfMember });
                row.Columns.push({ Name: 'MaxMembersLimit', Value: data.ListSuper[i].MaxMembersLimit == -1 ? "Unlimited" : data.ListSuper[i].MaxMembersLimit });
                row.Columns.push({ Name: 'Remark', Value: data.ListSuper[i].Remark });
                bodyRows.push(row);
            }

            var gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 40, Name: "RowNum", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Super", Width: 150, Name: "SuperName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Number Of Members", Width: 150, Name: "NumberOfMember", HeaderAlign: "center", CellAlign: "right" },
                    { HeaderText: "Max Members Limit", Width: 150, Name: "MaxMembersLimit", HeaderAlign: "center", CellAlign: "right" },
                    { HeaderText: "Remark", Width: 200, Name: "Remark", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true }
                ],
                bodyRows: bodyRows,
                gridExpandHeight: 200,
                showPager: true,
                customPager: true,
                showDistinctRow: false,
                pagerOption: {
                    itemsPerPage: parseInt(pageSize),
                    currentPage: parseInt(pageIndex) - 1,
                    itemsPerPageArray: [50, 100, 150, 200],
                    pagerSelectCallBack: reRenderAfterPaged,
                    totalItem: data.TotalRow
                }
            };

            $('#gridContainer').show();
            $('#gridId').fanexGrid(gridOptions);
        },
        error: function (a, b, c, d) {
            $('#gridContainer').hide();
            if (a.responseText.indexOf('Access Denied') > 0)
            {
                $('#errMsg1').append('<div class="alertboxerr"><div>' + lang.AccessDenied + '</div></div>');
            }
            if (a.status == 403) {
                $('#errMsg1').append('<div class="alertboxerr"><div>' + "You don't have permission to access." + '</div></div>');
            }
        },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { }; }
    });
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    try { parent.BlockUI(); } catch (e) { }
    GetData(currentPageIndex + 1, itemsPerPage)
}

function Edit() {
    $('#errMsg').html('');
    $('#txtSuperName').val('');
    $('#txtSuperName').attr('style', 'width: 450px');
    $('#txtSuperName').attr('rows', 5);
    $('#txtMaxMemberLimit').val('');
    $('#txtMaxMemberLimit').removeAttr('disabled');
    $('#chkUnlimited').removeAttr('checked');
    $('#txtRemark').val('');
    $('#txtRemark').attr('style', 'width: 450px');
    $('#txtRemark').attr('rows', 6);
    $("#dialog").dialog({
        width: 480,
        modal: true,
        position: { my: "center top", at: "center top+50" },
        close: function (event, ui) {
            $("#dialog").dialog("destroy");
        }
    });
}

function ValidateData() {
    var usernamesValue = $('#txtSuperName').val();
    if (usernamesValue.trim().length == 0) {
        RenderErrMsg('Please enter Username.');
        return false;
    }

    // Check usernames contains '\'.
    if (usernamesValue.indexOf("\n") >= 0
        || usernamesValue.indexOf("\r") >= 0
        || usernamesValue.indexOf("\\") >= 0) {
        RenderErrMsg("You are not allowed to input [Enter] and backslash '\\' into the Usernames field.");
        return false;
    }

    var usernamesArray = usernamesValue.split(",");
    if (usernamesArray.length > 50) {
        RenderErrMsg('The function can only create up to 50 accounts per request.');
        return false;
    }

    var txtMaxMemberLimit = $('#txtMaxMemberLimit').val();
    var chkUnlimited = $('#chkUnlimited').is(':checked');
    if (txtMaxMemberLimit.length == 0 && !chkUnlimited) {
        RenderErrMsg('Please input the Max Members Limit for the super(s) or set them Unlimited.');
        return false;
    }

    return true;
}

function UpdateData() {
    parent.BlockUI();
    $('#errMsg').html('');
    if (!ValidateData()) {
        parent.jQuery.unblockUI();
        return;
    }

    var params = {
        SuperNames: $("#txtSuperName").val(),
        MaxMemberLimit: $('#chkUnlimited').is(':checked') ? -1 : $("#txtMaxMemberLimit").val(),
        Remark: $("#txtRemark").val()
    };

    $.ajax({
        type: "POST",
        url: "Update",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(params),
        dataType: "json",
        success: function (data) {
            if (data.ErrorCode != null && data.ErrorCode != 0) {
                RenderErrMsg(data.ErrorMessage);
                return;
            }

            var successCount = 0;
            for (var i = 0; i < data.Data.length ; i++) {
                if (data.Data[i].ErrCode == 1) {
                    RenderErrMsg(data.Data[i].UserName + ': ' + data.Data[i].ErrMsg);
                }
                else if (data.Data[i].ErrCode == 2) {
                    RenderWarningMsg(data.Data[i].UserName + ': ' + data.Data[i].ErrMsg);
                }
                else {
                    successCount++;
                }
            }

            if (successCount > 0) {
                RenderSuccessMsg('Successfully updated: ' + successCount + ' super(s).')
            }

            setTimeout(function () { $("#txtSuperName").click(); }, 1000)
        },
        complete: function () { parent.jQuery.unblockUI(); }
    });
}

function chkUnlimited_OnChange() {
    $this = $('#chkUnlimited');
    if ($this.is(':checked')) {
        $('#txtMaxMemberLimit').attr('disabled', 'disabled');
    }
    else {
        $('#txtMaxMemberLimit').removeAttr('disabled');
    }
}

function RenderErrMsg(errMsg) {
    $('#errMsg').append('<div class="alertboxerr"><div>' + errMsg + '</div></div>');
}

function RenderWarningMsg(errMsg) {
    $('#errMsg').append('<div class="alertboxwrn"><div>' + errMsg + '</div></div>');
}

function RenderSuccessMsg(successMsg) {
    $('#errMsg').append('<div class="alertboxscc"><div>' + successMsg + '</div></div>');
}

var buildSiteData = function (siteList) {
            var dataList = [];
            $.each(siteList, function (i, site) {
                dataList.push({
                    Id: site.Id,
                    Value: site.Id,
                    Text: site.Name,
                    Checked: true,
                    Enable: true,
                    Items: site.IsParent ? buildSiteData(site.ChildSites) : []
                });
            });

            return dataList;
};

function assignSites() {
    $("#Site").multiSelect({
        minWidth: 180,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: true,
        showFooter: false,
        checkAllText: "All",
        listData: buildSiteData(jsonSites),
        selectableOptgroup: true,
        noItemText: "No site found."
    });
}

$(document).ready(function () {
    $('#txtMaxMemberLimit').keydown(function (e) {
        var key = e.charCode || e.keyCode || 0;
        //if (key == 109) {
        //    //subtract
        //    if (jQuery(this).val() == "") {
        //        return true;
        //    }
        //}

        var isAccept = (
            //(e.ctrlKey && e.keyCode == 67) || //accept ctrl + c(copy)
            //(e.ctrlKey && e.keyCode == 86) || //accept ctrl + v(paste)
            key == 16 || //shift
            key == 17 || //ctrl
            key == 8 || //backspace
            key == 36 || //home
            key == 35 || //end
            key == 9 || //tab
            key == 46 || //delete
            //key == 109 || //-
            //key == 189 || //-
            (key >= 37 && key <= 40) || //left , right , up , down (arrow)
            (key >= 48 && key <= 57) || //0-9
            (key >= 96 && key <= 105)); //numpad 0-9
        return isAccept;
    }).keyup(function () {
        if (jQuery(this).val() != "-" && isNaN(parseFloat(jQuery(this).val()))) {
            jQuery(this).val("");
        }
    })
    assignSites();
})