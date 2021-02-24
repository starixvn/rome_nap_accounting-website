"use strict";
(function ($, rootUrl) {
    var $txtMember = null,
        $fromDate = null,
        $toDate = null,
        $boxContent = null;

    function blockUI() {
        try { parent.BlockUI(); } catch (e) { console.error(e); }
    }

    function unblockUI() {
        try { parent.jQuery.unblockUI(); } catch (e) { console.error(e); }
    }

    function cacheElements() {
        $txtMember = $("#txt-member");
        $fromDate = $("#txt-fromDate");
        $toDate = $("#txt-toDate");
        $boxContent = $("#boxContent");
    }

    function initFromToDate() {
        var minDate = new Date($fromDate.val()),
            maxDate = new Date($toDate.val());
        var defaultOptions = {
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: minDate,
            maxDate: maxDate
        };
        $fromDate.datepicker($.extend({}, defaultOptions, {
            onSelect: function (selectedDate) {
                $toDate.datepicker("option", "minDate", selectedDate);
            }
        }));
        $toDate.datepicker($.extend({}, defaultOptions, {
            onSelect: function (selectedDate) {
                $fromDate.datepicker("option", "maxDate", selectedDate);
            }
        }));
    }

    function bindSubmitForm() {
        $("#form-betCreditHistory").submit(function (e) {
            e.preventDefault();

            if ($.trim($txtMember.val()).length) {
                getBetCreditHistory();
            } else {
                showBoxContent('<div class="alertboxerr"><div>Member is required.</div></div>');
            }
        });
    }

    function getBetCreditHistory() {
        blockUI();
        var url = rootUrl + "BetCreditHistory/Search"
            + "?member=" + $txtMember.val()
            + "&fromdate=" + $fromDate.val()
            + "&todate=" + $toDate.val();
        $.get(url, showBoxContent).fail(handleForbiddenRequest).always(unblockUI);
    }

    function showBoxContent(html) {
        $boxContent.html(html);
        $boxContent.hasClass("hide") && $boxContent.removeClass("hide");
    }

    function handleForbiddenRequest(jqXHR) {
        if (jqXHR.status === 403) {
            var error = JSON.parse(jqXHR.responseText);
            showBoxContent('<div class="alertboxerr"><div>' + error.ErrorMessage + '</div></div>');
        }
    }

    $(function () {
        cacheElements();
        initFromToDate();
        bindSubmitForm();
    });
})(jQuery, rootUrl);

