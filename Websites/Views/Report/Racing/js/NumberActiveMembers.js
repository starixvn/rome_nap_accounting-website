var Helper = {
    renderCalendarFromTo: function (ids) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
            }
        };

        var dates = $(ids).datepicker(defaults);
        var dt = new Date();
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);
        fdate.datepicker("setDate", dt);
        tdate.datepicker("setDate", dt);
    }    
};


$(document).ready(function () {
    Helper.renderCalendarFromTo("#txtfromdate, #txttodate");
    racing.selActiveMembers();
});

var racing = {
    selActiveMembers: function () {
        var param = {
            fromdate: $('#txtfromdate').val(),
            todate: $('#txttodate').val()
        };
        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "POST",
            url: "SelActiveMembers",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                var html = [];
                for (var i = 0; i < rs.length; i++) {
                    var row = rs[i];
                    html.push('<div class="activemember-eachrow">');
                    html.push('<div class="activemember-left">' + row.Text + '</div>');
                    html.push('<div class="activemember-right">' + row.Value + '</div>');
                    html.push('</div>');
                }
                $('#divMainActiveMembers').html(html.join(''));
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    exportExcel: function () {
        var url = 'NumberActiveMembers?IsExport=1';
        url += '&fromdate=' + $('#txtfromdate').val();
        url += '&todate=' + $('#txttodate').val();

        $('#frm').attr('src', url);
    }
};