var PJUnitPrice = {
    init: function () {
        var width = 372;
        var height = 570;
        $("#PJUnitPrice").removeClass("hidden").reRenderScroller(width, height, true);
        $("#exportExcel").click(this.exportExcel)
    },

    editClick: function (e) {
        var target = e.target || e.srcElement;
        var row = $(target).parents("tr:first");
        if (row.find("input[editor]").length > 0) return;

        var pjUnitPrice = row.find("div[pjunitprice]");
        pjUnitPrice.hide();
        var value = pjUnitPrice.html();
        pjUnitPrice.parent()
            .append("<input type='text' maxlength='11' class='col120' editor='true' pjunitprice='true' style='text-align:right' value='" + value.trim() + "' />");
        pjUnitPrice.parent().find("input[pjunitprice]").on('input', this.validateInput);

        var divEdit = $(target).parent();
        divEdit.find("img[alt='Edit']").hide();
        divEdit.find("img[alt='Save']").show();
        divEdit.find("img[alt='Cancel']").show();
    },

    validateInput: function (e) {
        var text = this.value
            .replace(/[^\d.]/g, '')
            .replace(/(^[\d]{5})[\d]/g, '$1')
            .replace(/(\..*)\./g, '$1')
            .replace(/(\.[\d]{4})./g, '$1');

        var indexOfDot = text.indexOf('.');
        var realNumber = indexOfDot === -1 ? parseInt(text) : parseInt(text.substring(0, indexOfDot));
        var fraction = indexOfDot === -1 ? '' : text.substring(indexOfDot, indexOfDot + 5);

        if ($.isNumeric(realNumber)) {
            this.value = realNumber.toLocaleString("en-US", { maximumFractionDigits: 4 }) + fraction;
        } else {
            this.value = text;
        }
    },

    cancelClick: function (e) {
        var target = e.target || e.srcElement;
        var row = $(target).parents("tr:first");
        row.find("input[editor]").remove();
        row.find("div[pjunitprice]").show();

        var divEdit = $(target).parent();

        divEdit.find("img[alt='Edit']").show();
        divEdit.find("img[alt='Save']").hide();
        divEdit.find("img[alt='Cancel']").hide();
    },

    saveClick: function (e, currencyId) {
        var target = e.target || e.srcElement;
        var row = $(target).parents("tr:first");
        var pjUnitPrice = row.find("input[editor][pjunitprice]").val();
        var self = this;

        if (!this.isPositiveNumeric(pjUnitPrice.replace(/\,/g, ""))) {
            this.showErr("PJUnitPrice is invalid!");
            return;
        }

        var param = {
            currencyId: currencyId,
            pjUnitPrice: pjUnitPrice
        };

        this.blockUI();
        $.post("EditPJUnitPrice", param, function (data) {
            self.editSuccess(data, row, pjUnitPrice);
        });
    },

    exportExcel: function () {
        $("#frmExportExcel").attr("src", "ExportExcelPJUnitPrice");
    },

    editSuccess: function (data, row, value) {
        if (data.ErrorCode !== 0) {
            this.showErr(data.ErrorMessage);
        }
        else {
            var colPJUnitPrice = row.find("div[pjunitprice]");

            colPJUnitPrice.html(value).show();

            row.find("input[editor]").remove();
            row.find("img[alt='Edit']").show();
            row.find("img[alt='Save']").hide();
            row.find("img[alt='Cancel']").hide();

            this.showSuccess("Update successfully!");
        }
        this.unBlockUI();
    },

    isPositiveNumeric: function (n) {
        var number = parseFloat(n);
        return !isNaN(parseFloat(number))
            && isFinite(number)
            && number >= 0
            && number <= 99999.9999;
    },

    showErr: function (msg) {
        showAlertMessage("err", msg, 1);
        $("#err").show();
    },

    showSuccess: function (msg) {
        showAlertMessage("err", msg, 0);
        $("#err").show();
        var timerId = setTimeout($.proxy(function () {
            $("#err").hide();
            clearTimeout(timerId);
        }, this), 2000);
    },

    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },

    unBlockUI: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    }
};

$(document).ready(function () {
    PJUnitPrice.init();
});
