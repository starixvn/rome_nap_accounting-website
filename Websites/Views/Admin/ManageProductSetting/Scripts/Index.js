$(document).ready(function () {
    $('.numeric-input-int').NAPNumericInput({
        numOfDecimal: 0,
        minValue: 0,
        maxValue: 2147483647
    });
    $('.numeric-input-smallint').NAPNumericInput({
        numOfDecimal: 0,
        minValue: 0,
        maxValue: 32767
    });
    $('.numeric-input-tinyint').NAPNumericInput({
        numOfDecimal: 0,
        minValue: 0,
        maxValue: 255
    });
});

function OpenInsertSetting() {
    $('#errMsg').html('');
    var $uploadFile = $('#uploadFile');
    $uploadFile.replaceWith($uploadFile.val('').clone(true));
    $('#insert-setting').dialog({
        title: 'Import From Excel',
        modal: true,
        width: 700,
        height: 200
    });
}

function RenderMultiSelect(id) {
    var multiselectData = [];
    var options = $('#' + id + ' > option');
    options.each(function () {
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