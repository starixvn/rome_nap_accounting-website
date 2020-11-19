function SearchCanSee() {
    parent.BlockUI();
    $.ajax({
        url: 'SelectCanSee',
        type: 'GET',
        data: {
            SportType: $('#ddlCanSeeSportTypeId').val(),
            BetType: $('#txtBetType').val(),
            BetTypeName: $('#txtBetTypeName').val(),
            Currencies: $('#ddlCanSeeCurrencyId').multiSelect("getCheckedItems").join(),
            SiteID: $('#ddlCanSeeSiteNameId').val()
        },
        success: function (data) {
            RenderCanSeeTable(data);
            BindElement();
        },
        complete: function () {
            parent.jQuery.unblockUI();
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function RenderCanSeeTable(data) {
    var bodyRows = [];
    for (i = 0; i < data.length; i++) {
        var checkboxDelete = data[i].IsTest === true ? '' : 'disabled';
        var checkboxIsTest = data[i].IsTest === true ? 'checked' : '';
        var checkboxIsLaunchedLevel = data[i].LaunchedLevel === 1 ? 'checked' : '';
        var row = { Columns: [] };
        row.Columns.push({ Name: 'SportType', Value: data[i].SportType });
        row.Columns.push({ Name: 'SportTypeName', Value: data[i].SportTypeName });
        row.Columns.push({ Name: 'BetType', Value: data[i].BetType });
        row.Columns.push({ Name: 'BetTypeName', Value: data[i].BetTypeName });
        row.Columns.push({ Name: 'Currency', Value: data[i].Currency });
        row.Columns.push({ Name: 'CurrencyName', Value: data[i].CurrencyName });
        row.Columns.push({ Name: 'SiteId', Value: data[i].SiteId });
        row.Columns.push({ Name: 'SiteName', Value: data[i].SiteName });
        row.Columns.push({ Name: 'IsLaunched', Value: data[i].IsLaunchedText });
        row.Columns.push({
            Name: 'IsTest',
            Value: '<input type="checkbox" data-on="UAT" data-off="Pro" data-size="mini" data-offstyle="danger" class="toggle cansee_checkbox_istest_row" data-sporttype="' + data[i].SportType +
                '" data-istest="' + data[i].IsTest + '" data-bettype="' + data[i].BetType + '" data-currency="' + data[i].Currency + '" data-siteid="' + data[i].SiteId +
                '" data-launchedlevel="' + data[i].LaunchedLevel + '"' + checkboxIsTest + ' />'
        });
        row.Columns.push({
            Name: 'LaunchedLevel',
            Value: '<input type="checkbox" data-size="mini" class="toggle cansee_checkbox_launchedlevel_row" data-sporttype="' + data[i].SportType +
                '" data-bettype="' + data[i].BetType + '" data-currency="' + data[i].Currency + '" data-siteid="' + data[i].SiteId +
                '" data-launchedlevel="' + data[i].LaunchedLevel + '" data-istest="' + data[i].IsTest + '" data-on="BetType" data-off="SportType/Product"' + checkboxIsLaunchedLevel + ' />'
        });
        row.Columns.push({
            Name: 'Delete',
            Value: '<input type="checkbox" ' + checkboxDelete + ' onclick="delete_row_click()" class="cansee_checkbox_delete_row" data-sporttype="' + data[i].SportType +
                '" data-bettype="' + data[i].BetType + '" data-currency="' + data[i].Currency + '" data-siteid="' + data[i].SiteId + '" />'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "SportType", Width: 50, Name: "SportType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SportTypeName", Width: 100, Name: "SportTypeName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BetType", Width: 50, Name: "BetType", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "BetTypeName", Width: 200, Name: "BetTypeName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Currency", Width: 50, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "CurrencyName", Width: 100, Name: "CurrencyName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SiteId", Width: 50, Name: "SiteId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "SiteName", Width: 100, Name: "SiteName", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "IsLaunched", Width: 80, Name: "IsLaunched", HeaderAlign: "center", CellAlign: "center" },
            {
                HeaderText: 'IsTest <br/>' +
                    '<img class="edit-entity" title="Save" src="../App_Themes/Images/btUpdateL.gif" style="cursor: pointer" width="14" height="14" onclick="UpdateCanSee_IsTest()" >' +
                    '&nbsp;&nbsp;<input type="checkbox" data-size="mini" id="canseeCheckAllIsTest" checked data-on="UAT" data-off="Pro" data-offstyle="danger" class="toggle"/>',
                Width: 120, Name: "IsTest", HeaderAlign: "center", CellAlign: "center"
            },
            {
                HeaderText: 'LaunchedLevel <br/>' +
                    '<img class="edit-entity" title="Save" src="../App_Themes/Images/btUpdateL.gif" style="cursor: pointer" width="14" height="14" onclick="UpdateCanSee_LaunchedLevel()" >' +
                    '&nbsp;&nbsp;<input type="checkbox" data-size="mini" id="canseeCheckAllLaunchedLevel" checked data-on="BetType" data-off="SportType/Product" class="toggle"/>',
                Width: 200, Name: "LaunchedLevel", HeaderAlign: "center", CellAlign: "center"
            },
            {
                HeaderText:
                    'Delete <br/> <img class="edit-entity" title="You cannot delete row(s) which IsTest is Pro." src="../App_Themes/Images/icn_del.png" style="cursor: pointer" onclick="DeleteCanSee()" >' +
                    '&nbsp;&nbsp;<input type="checkbox" id="canseeCheckAll" onclick="canseeCheckAllDelete(this)" />',
                Width: 60, Name: "Delete", HeaderAlign: "center", CellAlign: "center"
            }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    $('#gridCanSee').fanexGrid(gridOptions);
}

function BindElement() {
    $(".toggle").bootstrapToggle();
    $('#canseeCheckAllIsTest').change(function () {
        var toggleValue = $(this).prop('checked') === true ? "on" : "off";
        $('#gridCanSee').find('.cansee_checkbox_istest_row').bootstrapToggle(toggleValue);
    });

    $('#canseeCheckAllLaunchedLevel').change(function () {
        var toggleValue = $(this).prop('checked') === true ? "on" : "off";
        $('#gridCanSee').find('.cansee_checkbox_launchedlevel_row').bootstrapToggle(toggleValue);
    });
}

function delete_row_click() {
    var flag = true;
    $(".cansee_checkbox_delete_row").each(function () {
        if (this.checked === false && !this.disabled)
            flag = false;
    });
    $("#canseeCheckAll").attr('checked', flag);
}

function canseeCheckAllDelete(checkboxall) {
    $('#gridCanSee').find('.cansee_checkbox_delete_row').not(':disabled').prop('checked', checkboxall.checked);
}

function UpdateCanSee_LaunchedLevel() {
    var rows = [];
    var checkboxs = $('#gridCanSee').find('.cansee_checkbox_launchedlevel_row');
    checkboxs.each(function (index, option) {
        var launchedlevel_old = $(option).attr("data-launchedlevel") === "1";
        var launchedlevel_new = $(option).prop("checked");
        if (launchedlevel_old !== launchedlevel_new) {
            rows.push({
                SportType: $(option).attr("data-sporttype"),
                BetType: $(option).attr("data-bettype"),
                Currency: $(option).attr("data-currency"),
                SiteId: $(option).attr("data-siteid"),
                IsTest: $(option).attr("data-istest") == "false" ? "0" : "1",
                LaunchedLevel: launchedlevel_new ? "1" : "0"
            });
        }

    });
    if (rows.length > 0) {
        if (confirm('Are you sure you want to update LaunchedLevel on these rows?')) {
            parent.BlockUI();
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: 'UpdateCanSee',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify({ 'Entities': rows, 'ColumnMode': 2 }),
                success: function (result) {
                    if (result.ErrorCode === 0) {
                        SearchCanSee();
                    } else {
                        alert(result.ErrorMessage);
                    }
                },
                error: function (xhr) {
                    alert('Internal Server Error.');
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        }
    }
    
}

function UpdateCanSee_IsTest() {
    var rows = [];
    var checkboxs = $('#gridCanSee').find('.cansee_checkbox_istest_row');
    checkboxs.each(function (index, option) {
        var isTest_old = $(option).attr("data-istest") === "true";
        var isTest_new = $(option).prop("checked");
        if (isTest_old !== isTest_new) {
            rows.push({
                SportType: $(option).attr("data-sporttype"),
                BetType: $(option).attr("data-bettype"),
                Currency: $(option).attr("data-currency"),
                SiteId: $(option).attr("data-siteid"),
                IsTest: isTest_new ? "1" : "0",
                LaunchedLevel: $(option).attr("data-launchedlevel")
            });
        }

    });
    if (rows.length > 0) {
        if (confirm('Are you sure you want to update IsTest on these rows?')) {
            parent.BlockUI();

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: 'UpdateCanSee',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify({ 'Entities': rows, 'ColumnMode': 1 }),
                success: function (result) {
                    if (result.ErrorCode === 0) {
                        SearchCanSee();
                    } else {
                        alert(result.ErrorMessage);
                    }
                },
                error: function (xhr) {
                    alert('Internal Server Error.');
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        }
    }    
}

function DeleteCanSee() {
    var rows = [];
    var checkboxs = $('#gridCanSee').find('.cansee_checkbox_delete_row');
    checkboxs.each(function (index, option) {
        if ($(option).prop("checked")) {
            rows.push({
                SportType: $(option).attr("data-sporttype"),
                BetType: $(option).attr("data-bettype"),
                Currency: $(option).attr("data-currency"),
                SiteId: $(option).attr("data-siteid")
            });
        }
    });
    if (rows.length > 0) {
        if (confirm('Are you sure you want to delete these rows?')) {
            parent.BlockUI();
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                url: 'DeleteCanSee',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify({ 'Entities': rows }),
                success: function (result) {
                    if (result.ErrorCode === 0) {
                        alert('Deleted successfully.');
                        SearchCanSee();
                    } else {
                        alert(result.ErrorMessage);
                    }
                },
                error: function (xhr) {
                    alert('Internal Server Error.');
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        }
    }
}

function RenderMultiSelect(id) {
    var multiselectData = [];
    $('#' + id + ' > option').each(function () {
        var $this = $(this);
        multiselectData.push({
            Id: id + $this.attr('value'),
            Value: $this.attr('value'),
            Text: $this.html(),
            Checked: true,
            Enable: true,
            CssClass: "",
            Items: []
        });
    });


    $('#' + id).multiSelect({
        minWidth: 255,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        isCheckAll: true,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "No item selected."
    });
}

$(document).ready(function () {
    $("#ddlCanSeeSportTypeId").select2();
    $("#ddlCanSeeSiteNameId").select2();
    RenderMultiSelect('ddlCanSeeCurrencyId');
});