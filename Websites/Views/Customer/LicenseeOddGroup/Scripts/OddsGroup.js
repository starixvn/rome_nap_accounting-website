var oddsGroupHandler = (function ($, common, rootUrl) {
    "use strict";
    var updateSection,
        reviewSection,
        reviewOddsGroupTable,
        applyMembersCheckbox,
        applyMembersSpan,
        nextButton,
        updateButton,
        backButton,
        oddsGroupTableRows,
        selectAllCheckbox,
        selectCheckboxes,
        copyIcon,
        dropdownOddsGroups;


    function initialize(reloadFunction) {
        dropdownOddsGroups = $(".dropdownOddsGroup");
        updateSection = $("#update_section");
        reviewSection = $("#review_section");
        reviewOddsGroupTable = reviewSection.find('#reviewoddsgroup_table tbody');
        applyMembersCheckbox = updateSection.find("#apply_members_checkbox");
        applyMembersSpan = reviewSection.find("#apply_members_span");
        oddsGroupTableRows = $('#oddsgroup_table tbody tr');

        dropdownOddsGroups.off("change")
            .on("change", dropdownOddsGroups_change);
        dropdownOddsGroups.select2();

        nextButton = updateSection.find("#btnNext")
            .off("click")
            .on("click", nextButton_click);

        updateButton = reviewSection.find("#btnBetSettingUpdate")
            .off("click")
            .on("click", function () { updateButton_click(getChangedOddsGroup()); });

        backButton = reviewSection.find("#btnBack")
            .off("click")
            .on("click", function () { btnBack_click(reloadFunction); });

        selectAllCheckbox = updateSection.find("#select-all-checkbox")
            .off("change")
            .on("change", selectAllCheckbox_change);

        selectCheckboxes = updateSection.find('.select-checkbox')
            .off("change")
            .on("change", selectCheckbox_change);

        copyIcon = updateSection.find("#copy_setting_icon")
            .off("click")
            .on("click", copyIcon_click);
    }

    function dropdownOddsGroups_change() {
        var $this = $(this);
        $this.closest('tr').find('.isactive-checkbox').attr('disabled', $this.is(':disabled') || $this.val() === '');
    }

    function copyIcon_click() {
        if (copyIcon.hasClass("icon-copy-setting-checked")) {
            copyIcon.removeClass("icon-copy-setting-checked");
            updateSection.find('.select-checkbox, #select-all-checkbox').attr('disabled', false).attr('checked', false);
            revertFrom();
        } else {
            copyIcon.addClass("icon-copy-setting-checked");
            selectCheckboxes.attr('checked', true);
            selectAllCheckbox.attr('checked', true);
            updateSection.find('input, select').attr('disabled', true);
            var $firstRow = oddsGroupTableRows.eq(0);
            oddsGroupTableRows.slice(1).each(function () {
                var $this = $(this);
                $('.dropdownOddsGroup', $this).val($('.dropdownOddsGroup', $firstRow).val());
                $('.dropdownOddsGroup', $this).trigger('change');
                $('.isactive-checkbox', $this).attr('checked', $('.isactive-checkbox', $firstRow).is(':checked'));
                $('.select-checkbox', $this).attr('checked', true);
            });
        }
    }

    function revertFrom() {
        oddsGroupTableRows.each(function () {
            var $this = $(this);
            $this.find('.dropdownOddsGroup').val($this.attr('data-oddsgroup')).change();
            $this.find('.isactive-checkbox').attr('checked', $this.attr('data-isactive') === 'True');
        });
    }

    function selectAllCheckbox_change() {
        var checked = this.checked;
        selectCheckboxes.attr('checked', checked);
        selectCheckboxes.change();
    }

    function selectCheckbox_change() {
        var $this = $(this);
        var checked = this.checked;
        if (checked) {
            $this.closest('tr').find('.dropdownOddsGroup').attr('disabled', false).change();
        }
        else {
            $this.closest('tr').find('.dropdownOddsGroup, .isactive-checkbox').attr('disabled', true);
        }

        selectAllCheckbox.attr('checked', selectCheckboxes.not(':checked').length === 0);
    }

    function getChangedOddsGroup() {
        var changedOddsGroups = [];
        for (var i = 0, max = oddsGroupTableRows.length; i < max; i++) {
            var $currentRow = $(oddsGroupTableRows[i]);
            var isSelected = $currentRow.find('.select-checkbox:checked').length === 1;
            if (isSelected) {
                var origOddsGroup = $currentRow.attr('data-oddsgroup');
                var origIsActive = $currentRow.attr('data-isactive') === 'True' ? true : false;
                var newOddsGroup = $currentRow.find('.dropdownOddsGroup').val();
                var newIsActive = $currentRow.find('.isactive-checkbox').is(':checked');
                changedOddsGroups.push({
                    CustId: $currentRow.attr('data-custid'),
                    UserName: $currentRow.attr('data-username'),
                    OperatorId: $currentRow.attr('data-operatorid'),
                    OddsGroup: newOddsGroup,
                    IsActive: newIsActive,
                    Updated: origOddsGroup !== newOddsGroup || origIsActive !== newIsActive
                });
            }
        }

        return changedOddsGroups;
    }

    function validateChangedOddsGroup(changedOddsGroups) {
        var hasChanged = false;
        for (var i = 0, max = changedOddsGroups.length; i < max; i++) {
            var oddsGroup = changedOddsGroups[i];
            if (oddsGroup.OddsGroup === '') {
                common.showMessage(1, 'Invalid odds group, please check again');
                return false;
            }

            if (oddsGroup.Updated)
                hasChanged = true;
        }

        if (!hasChanged) {
            common.showNoChangeMessage();
            return false;
        }

        return true;
    }

    function nextButton_click() {
        common.blockUI();
        common.hideMessage();
        var changedOddsGroups = getChangedOddsGroup();
        if(validateChangedOddsGroup(changedOddsGroups))
            showReviewForm(changedOddsGroups);

        common.unblockUI();
    }

    function showReviewForm(changedOddsGroups) {
        var html = [];
        for (var i = 0, max = changedOddsGroups.length; i < max; i++) {
            var oddsGroup = changedOddsGroups[i];
            html.push('<tr>');
            html.push('<td class="l">');
            html.push(oddsGroup.UserName);
            html.push('</td>');
            html.push('<td class="l">');
            html.push(oddsGroup.OperatorId);
            html.push('</td>');
            html.push('<td class="r">');
            html.push(oddsGroup.OddsGroup);
            html.push('</td>');
            html.push('<td class="r">');
            html.push(oddsGroup.IsActive ? 'Yes' : 'No');
            html.push('</td>');
            html.push('<td>');
            html.push(oddsGroup.Updated ? 'X' : '');
            html.push('</td>');
            html.push('<td class="l">');
            html.push('<div id="message_' + oddsGroup.CustId + '">');
            html.push('</div>');
            html.push('</td>');
            html.push('</tr>');
        }
        reviewOddsGroupTable.html(html.join(''));
        applyMembersSpan.text(applyMembersCheckbox.is(":checked") ? "Yes" : "No");
        updateSection.addClass("hide");
        reviewSection.removeClass("hide");
    }

    function btnBack_click(reloadFunction) {
        if (updateButton.is(":disabled")) {
            reloadFunction();
        } else {
            updateSection.removeClass("hide");
            reviewSection.addClass("hide");
        }
    }


    function updateButton_click(listChangedOddsGroup) {
        var updatedArray = $.grep(listChangedOddsGroup, function (item, i) {
            return item.Updated;
        });

        if (updatedArray.length === 0) {
            return;
        }

        var applyExistedMem = applyMembersCheckbox.is(":checked");

        if (!applyExistedMem && !confirm("Apply existing members: No. Are you sure you want to continue?")) {
            return;
        }

        common.disableElement(updateButton);
        $.each(updatedArray, function (i, item) {
            var messageDiv = $('#message_' + item.CustId);
            messageDiv.html('<span class="btnRefresh"></span> Updating ...');
            var applyExistedMem = applyMembersCheckbox.is(":checked");
            $.ajax({
                type: "POST",
                url: "UpdateOddsGroup",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ CustId: item.CustId, OddsGroup: item.OddsGroup, IsActive: item.IsActive, ApplyExistingMember: applyExistedMem }),
                success: function (data) {
                    messageDiv.removeClass().html(data.ErrorCode === 0 ? 'Updated Successfully.' : data.ErrorMessage);
                    messageDiv.addClass(data.ErrorCode === 0 ? "msgcorrect" : "msgerror");
                },
                error: function (jqxhr) {
                    messageDiv.removeClass().html(jqxhr.statusText).addClass("msgerror");
                }
            });
        });
    }

    return { initialize: initialize };
})(jQuery, common, rootUrl);