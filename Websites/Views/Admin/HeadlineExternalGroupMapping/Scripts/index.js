var cachedData = [];
var selectedPrimaryGroup = "";
var selectedMapping = null;
var isNoExtraPtGroup = true;
var requests = [];

function InitForm() {

    $("#add-new-group_mapping").validate({
        submitHandler: function () {
            InsertMapping();
        }
    });

    SetupCalendar("NewEffectiveFromId", "#NewPrimaryGroupId");
    SetupCalendar("EditEffectiveFromId", "#EditPrimaryGroupId");
}

function GetGroupsRequest() {
    var rq = [];
    rq.push($.ajax({
        url: 'GetHeadlines',
        type: 'GET',
        dataType: 'json'
    }));

    rq.push($.ajax({
        url: 'GetExternalGroups',
        type: 'GET',
        dataType: 'json'
    }));

    rq.push($.ajax({
        url: 'GetPrimaryGroups',
        type: 'GET',
        dataType: 'json'
    }));

    return rq;
}

function InsertMapping() {
    if ($('#GroupHeadlineId').val() === "") {
        showAlertMessage('addNewErrMsg', "Group headline is required!", 1);
        return;
    }

    if ($('#SiteId').val() === "") {
        showAlertMessage('addNewErrMsg', "Site is required!", 1);
        return;
    }

    if ($('#CurrencyId').val() === "") {
        showAlertMessage('addNewErrMsg', "Currency is required!", 1);
        return;
    }

    if ($('#NewExternalGroupIds').val() === null) {
        showAlertMessage('addNewErrMsg', "External group is required!", 1);
        return;
    }

    if ($('#NewPrimaryGroupId').val() === "" && !isNoExtraPtGroup) {
        showAlertMessage('addNewErrMsg', "Primary group is required!", 1);
        return;
    }

    if ($('#NewEffectiveFromId').val() === "" && !isNoExtraPtGroup) {
        showAlertMessage('addNewErrMsg', "EffectedDate is required!", 1);
        return;
    }

    try { parent.BlockUI(); } catch (e) { }

    $.ajax({
        url: 'Insert',
        type: 'POST',
        dataType: 'json',
        data: {
            GroupHeadlineId: $('#GroupHeadlineId').val(),
            SiteId: $('#SiteId').val(),
            ExchangeId: $('#CurrencyId').val(),
            ExtraPTGroup: $('#NewExternalGroupIds').val().toString(),
            CurrentPrimaryExtraPTGroup: isNoExtraPtGroup ? "" : $('#NewPrimaryGroupId').val(),
            CurrentEffectiveDate: isNoExtraPtGroup? "" : $('#NewEffectiveFromId').val()
        },
        success: function (result) {
            if (result.ErrorCode === 0) {
                result.ErrorMessage = 'The mapping has been successfully created.';
                setTimeout(function () {
                    $('#add-new-group_mapping').dialog('close');
                    SearchMapping();
                }, 3000);
            }

            showAlertMessage('addNewErrMsg', result.ErrorMessage, result.ErrorCode == 0 ? 0 : 1);
        },
        error: function (xhr) {
            showAlertMessage('addNewErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function AddNewMapping() {
    $('#addNewErrMsg').html('');
    $('#GroupHeadlineId').val("").trigger('change');
    $('#SiteId').val("").trigger('change');
    $('#CurrencyId').val("").trigger('change');
    $('#NewEffectiveFromId').val(tomorrow);
    try { parent.BlockUI(); } catch (e) { }
    $.when.apply(undefined, GetGroupsRequest()).then(function () {
        var responses = arguments;
        var headlines = responses[0][0];
        var externalGroups = responses[1][0];
        var primaryGroups = responses[2][0];
        RenderGroups("New", headlines, externalGroups, primaryGroups);
        $('#NewExternalGroupIds').val(null).trigger('change');
        $('#NewPrimaryGroupId').val("").trigger('change');

        try { parent.jQuery.unblockUI(); } catch (e) { }
    });
    
    $('#add-new-group_mapping').dialog({
        title: 'New Headline/External Group Mapping',
        modal: true,
        width: 'auto',
        height: 'auto'
    });
}

function SearchMapping() {
    try { parent.BlockUI(); } catch (e) { }

    var isCheckAllPrimaryGroup = $('#ddlPrimaryGroups').multiSelect("getUncheckItems").length === 0;
    var primaryGroupsExtra = 'all';
    if (!isCheckAllPrimaryGroup) {
        primaryGroupsExtra = $('#ddlPrimaryGroups').multiSelect("getCheckedItems").join();
    }

    $.ajax({
        url: 'Select',
        type: 'POST',
        data: {
            HeadlineName: $('#txtHeadline').val(),
            SiteIds: $('#ddlSites').multiSelect("getCheckedItems").join(),
            ExchangeIds: $('#ddlCurrency').multiSelect("getCheckedItems").join(),
            ExternalGroups: $('#ddlExternalGroups').multiSelect("getCheckedItems").join(),
            PrimaryExtraGroups: primaryGroupsExtra
        },
        success: function (data) {
            cachedData = data;
            RenderMapping(data);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderMapping(data) {
    var bodyRows = [];
    for (var i = 0; i < data.length; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'Headline', Value: data[i].HeadlineName });
        row.Columns.push({ Name: 'Site', Value: data[i].Site });
        row.Columns.push({ Name: 'Currency', Value: data[i].Currency });
        row.Columns.push({ Name: 'NormalGroup', Value: data[i].NormalGroup });
        row.Columns.push({ Name: 'ExtraPTGroup', Value: data[i].ExtraPTGroup });
        row.Columns.push({ Name: 'PrimaryExtraPTGroup', Value: data[i].PrimaryExtraPTGroup });
        row.Columns.push({
            Name: 'Action',
            Value: '<img class="edit-entity" alt="Edit" title="Edit" src="../App_Themes/Images/icn_edit.png" style="cursor: pointer; padding: 0 2px;" onclick="EditMapping(' + i + ')" >'
                + '<img class="edit-entity" alt="Delete" title="Delete" src="../App_Themes/Images/icn_del.png" style="cursor: pointer; padding: 0 2px;" onclick="DeleteMapping(' + i + ')" >'
        });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Headline", Width: 150, Name: "Headline", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Currency", Width: 100, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Normal Group", Width: 150, Name: "NormalGroup", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Extra PT Group", Width: 250, Name: "ExtraPTGroup", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Primary Extra PT Group", Width: 250, Name: "PrimaryExtraPTGroup", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    $('#MappingGrid').fanexGrid(gridOptions);
}

function EditMapping(index) {
    selectedMapping = null;
    $('#EditPrimaryGroupId').off("change");
    if (cachedData === null || cachedData.length === 0 || cachedData.length <= index) {
        return;
    }

    $('#editErrMsg').html('');
    selectedMapping = cachedData[index];
    $('#EditGroupHeadline').html(selectedMapping.HeadlineName);
    $('#EditSite').html(selectedMapping.Site);
    $('#EditCurrency').html(selectedMapping.Currency);


    try { parent.BlockUI(); } catch (e) { }
    $.when.apply(undefined, GetGroupsRequest()).then(function () {
        var responses = arguments;
        var headlines = responses[0][0];
        var externalGroups = responses[1][0];
        var primaryGroups = responses[2][0];
        RenderGroups("Edit", headlines, externalGroups, primaryGroups);
        var selectedExternalGroups = selectedMapping.ExtraPTGroup.split(',').map(group => group.trim()).join();
        if (selectedMapping.NormalGroup !== "") {
            selectedExternalGroups = selectedMapping.NormalGroup.split(',').map(group => group.trim()).join() + ',' + selectedExternalGroups;
        }

        $('#EditExternalGroupIds').val(selectedExternalGroups.split(',')).trigger('change');
        if (selectedMapping.CurrentPrimaryExtraPTGroup === "" && selectedMapping.ExtraPTGroup !== "") {
            selectedMapping.CurrentPrimaryExtraPTGroup = "No Extra PT";
        }

        $('#EditPrimaryGroupId').val(selectedMapping.CurrentPrimaryExtraPTGroup.trim()).trigger('change');

        $('#EditEffectiveFromId').val(selectedMapping.CurrentEffectiveDate).trigger('change');

        $('#btnSubmitEdit').off();
        $('#btnSubmitEdit').click(function (event) {
            event.preventDefault();
            UpdateMapping(selectedExternalGroups.split(',').filter(group => group !== "").sort().toString());
        });

        try { parent.jQuery.unblockUI(); } catch (e) { }
    });

    $('#edit-group_mapping').dialog({
        title: 'Edit Headline/External Group Mapping',
        modal: true,
        width: 'auto',
        height: 'auto'
    });
}

function EnableDisableEditDate() {
    var selectedDate = new Date($('#EditEffectiveFromId').val());
}

function UpdateMapping(externalGroups) {
    
    if ($('#EditExternalGroupIds').val() === null) {
        showAlertMessage('editErrMsg', "External group is required!", 1);
        return;
    }

    if ($('#EditPrimaryGroupId').val() === "" && !isNoExtraPtGroup) {
        showAlertMessage('editErrMsg', "Primary group is required!", 1);
        return;
    }

    if ($('#EditEffectiveFromId').val() === "" && !isNoExtraPtGroup) {
        showAlertMessage('editErrMsg', "EffectedDate is required!", 1);
        return;
    }

    if ($('#EditExternalGroupIds').val().sort().toString().toUpperCase() === externalGroups.toUpperCase()
        && $('#EditPrimaryGroupId').val() === selectedMapping.CurrentPrimaryExtraPTGroup.trim()
        && $('#EditEffectiveFromId').val() === selectedMapping.CurrentEffectiveDate) {
        showAlertMessage('editErrMsg', "There is no change to be updated.", 1);
        return;
    }

    try { parent.BlockUI(); } catch (e) { }

    $.ajax({
        url: 'Update',
        type: 'POST',
        dataType: 'json',
        data: {
            GroupHeadlineId: selectedMapping.GroupHeadlineId,
            SiteId: selectedMapping.SiteId,
            ExchangeId: selectedMapping.ExchangeId,
            ExtraPTGroup: $('#EditExternalGroupIds').val().toString(),
            CurrentPrimaryExtraPTGroup: $('#EditPrimaryGroupId').val(),
            CurrentEffectiveDate: $('#EditEffectiveFromId').val()
        },
        success: function (result) {
            if (result.ErrorCode === 0) {
                result.ErrorMessage = 'The mapping has been successfully updated.';
                showAlertMessage('editErrMsg', result.ErrorMessage, 0);
                setTimeout(function () {
                    $('#edit-group_mapping').dialog('close');
                    SearchMapping();
                }, 3000);
            } else {
                showAlertMessage('editErrMsg', result.ErrorMessage, 1);
            }
        },
        error: function (xhr) {
            showAlertMessage('editErrMsg', 'Internal Server Error.', 1);
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function DeleteMapping(index) {
    var selectedDeleteMapping = cachedData[index];
    var externalGroups = selectedDeleteMapping.ExtraPTGroup.split(',');
    if (selectedDeleteMapping.NormalGroup !== "") {
        externalGroups = externalGroups.concat(selectedDeleteMapping.NormalGroup.split(','));
    }
    var stringExternalGroups = externalGroups.map(group => group.trim()).filter(group => group !== "").join();
    if (selectedDeleteMapping.CurrentPrimaryExtraPTGroup === "" && selectedDeleteMapping.ExtraPTGroup !== "") {
        selectedDeleteMapping.CurrentPrimaryExtraPTGroup = "No Extra PT";
    }

    var newLine = "\r\n";
    var confirmMessage = 'Do you want to remove this mapping ?';
    confirmMessage += newLine;
    confirmMessage += '- The current supers belonging to this headline/site/currency will be removed from their External Groups and related Sub External Groups.';
    confirmMessage += newLine;
    confirmMessage += '- The upcoming supers added into this headline group will not be automatically added into external groups anymore.';
    if (confirm(confirmMessage)) {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            url: 'Delete',
            type: 'POST',
            dataType: 'json',
            data: {
                GroupHeadlineId: selectedDeleteMapping.GroupHeadlineId,
                SiteId: selectedDeleteMapping.SiteId,
                ExchangeId: selectedDeleteMapping.ExchangeId,
                ExtraPTGroup: stringExternalGroups,
                CurrentPrimaryExtraPTGroup: selectedDeleteMapping.CurrentPrimaryExtraPTGroup.trim(),
                CurrentEffectiveDate: selectedDeleteMapping.CurrentEffectiveDate
            },
            success: function (result) {
                if (result.ErrorCode === 0) {
                    if (result.AffectedSupers === '') {
                        result.ErrorMessage = 'The mapping has been successfully deleted.';
                    } else {
                        result.ErrorMessage = "The mapping has been successfully deleted.The following supers are enqueued to be removed from their old External Groups at 0: 00 on tomorrow: " + result.AffectedSupers;
                    }

                    alert(result.ErrorMessage);
                    SearchMapping();
                } else {
                    alert(result.ErrorMessage);
                }
            },
            error: function (xhr) {
                alert('Internal Server Error.');
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
}

function RenderMultiSelect(id, mw = 120) {
    var multiselectData = [];
    $('#' + id + ' > option').each(function (index, _) {
        var $this = $(this);
        multiselectData.push({
            Id: id + $this.attr('value'),
            Value: $this.attr('value'),
            Text: $this.html(),
            Checked: index === 0,
            Enable: true,
            CssClass: "",
            Items: []
        });
    });



    $('#' + id).multiSelect({
        minWidth: mw,
        maxHeight: 450,
        shadow: true,
        showHeader: true,
        showTooltip: false,
        isCheckAll: true,
        showFooter: false,
        showFilter: true,
        checkAllText: "All",
        listData: multiselectData,
        noItemText: "No item selected."
    });
}

function RenderGroups(type, headlines, externalGroups, primaryGroups) {
    var headLineLength = headlines ? headlines.length : 0;
    var groupLength = externalGroups ? externalGroups.length : 0;
    var divId = "#" + type + "ExternalGroupIds";

    var html = [];
    if (type === "New") {
        for (var i = 0; i < headLineLength; i++) {
            html.push('<option value="');
            html.push(headlines[i].Value);
            html.push('">');
            html.push(headlines[i].Text);
            html.push("</option>");
        }
        $('#GroupHeadlineId').html(html.join("")).select2({
            placeholder: "Please Select",
            allowClear: false,
            width: "150px"
        });
        html = [];
    }

    if (externalGroups.length > 0) {
        var externalGroupsName = externalGroups.map(group => group.Value.toUpperCase());
        for (var i = 0; i < groupLength; i++) {
            html.push('<option value="');
            html.push(externalGroupsName[i]);
            html.push('">');
            html.push(externalGroupsName[i]);
            html.push("</option>");
        }

        var htmlString = html.join("");

        $(divId).html(htmlString).select2({
            placeholder: "Please Select",
            allowClear: false
        });
        $(divId).on("change", function (e) { RenderPrimaryGroups(type, primaryGroups); });
    }
}

function RenderPrimaryGroups(type, primaryGroups) {
    isNoExtraPtGroup = true;
    var primaryGroupDivId = '#' + type + 'PrimaryGroupId';
    var selectedGroupId = '#' + type + 'ExternalGroupIds';
    var effectedDate = '#' + type + 'EffectiveFromId';
    $(primaryGroupDivId).html("");
    var selectedExternalGroups = $(selectedGroupId).val();

    if (selectedExternalGroups === null) {
        selectedExternalGroups = [];
    }
    selectedExternalGroups = selectedExternalGroups.map(name => name.toUpperCase());
    var primaryGroupsName = primaryGroups.map(group => group.Value.toUpperCase());

    const filteredGroups = primaryGroupsName.filter(value => selectedExternalGroups.includes(value));
    var groupLength = filteredGroups ? filteredGroups.length : 0;

    var html = [];
    html.push('<option value>Please select</option>');
    html.push('<option value="No Extra PT">No Extra PT</option>');
    if (groupLength > 0) {
        isNoExtraPtGroup = false;
        for (var i = 0; i < groupLength; i++) {
            html.push('<option value="');
            html.push(filteredGroups[i]);
            html.push('">');
            html.push(filteredGroups[i]);
            html.push("</option>");
        }
    }

    var htmlString = html.join("");

    $(primaryGroupDivId).html(htmlString);

    $(primaryGroupDivId).select2({
        placeholder: "Please Select",
        allowClear: false
    });

    if (isNoExtraPtGroup) {
        selectedPrimaryGroup = "";
        $(primaryGroupDivId).val("").trigger("change");
        $(primaryGroupDivId).prop('disabled', true);
        $(effectedDate).prop('disabled', true);
        $(effectedDate + '_Trigger').hide();
    } else {
        $(primaryGroupDivId).val(selectedPrimaryGroup).trigger("change");
        $(primaryGroupDivId).prop('disabled', false);
        $(effectedDate).prop('disabled', false);
        $(effectedDate + '_Trigger').show();
    }

    if (type === 'Edit') {
        $(primaryGroupDivId).change(function () {
            if (selectedPrimaryGroup !== $(primaryGroupDivId).val()) {
                selectedPrimaryGroup = $(primaryGroupDivId).val();
                $("#EditEffectiveFromId").val(tomorrow).trigger('change');
            }

            if (selectedPrimaryGroup === selectedMapping.CurrentPrimaryExtraPTGroup) {
                $('#EditEffectiveFromId').val(selectedMapping.CurrentEffectiveDate).trigger('change');
            }
        });
    } else {
        $(primaryGroupDivId).change(function () {
            selectedPrimaryGroup = $(primaryGroupDivId).val();
        });
    }

    
}

function ConvertDateToString(date) {
    var day = date.getDate().toString();
    var month = (date.getMonth() + 1).toString();
    var year = date.getFullYear().toString();

    return month + '/' + day + '/' + year;
}

function SetupCalendar(id, idMsg) {
    $('#' + id).datetimepicker({
        format: 'm/d/Y',
        timepicker: false,
        defaultDate: tomorrow,
        minDate: today,
        formatDate: 'm/d/Y',
        closeOnInputClick: true,
        onClose: function (ct, $i, event) {
            $i.find('.xdsoft_month,.xdsoft_year')
                .find('.xdsoft_select')
                .hide();
            $(idMsg).focus();
        }
    });

    $('#' + id + '_Trigger').click(function () {
        jQuery('#' + id).datetimepicker('show');
    });
}

$(function () {
    RenderMultiSelect("ddlSites");
    RenderMultiSelect("ddlCurrency");
    RenderMultiSelect("ddlExternalGroups", 180);
    RenderMultiSelect("ddlPrimaryGroups", 180);
    InitForm();
    SearchMapping();
});