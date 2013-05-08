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
        fdate.datepicker("setDate", fromdate);
        tdate.datepicker("setDate", todate);
    },

    format: function (num) {
        if(num == null)
            return '';
        var n = num;
        var IsMargin = num.indexOf('%') > 0;
        n = num.replace('%', '');
        if (parseFloat(n) > 0) {            
            return '<span style="color:red">' + n + (IsMargin ? '%' : '') + '</span>'
        }
        return num;
    },
    showhiderow: function (obj, className) {
        var gridWidth = 630;
        var mainreport = $("#mainreport");
        var gridOpts = {
            tblContentId: 'grid1',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            gridWidth: gridWidth,
            freezeWidth: 150,
            numberFreezeCol: 1
        };

        if ($('#' + className).val() == 'tree-expand') {
            $(obj).removeClass("tree-expand");
            $("#grid1 tr." + className).removeClass("hidden");
            $("#grid_1st_Column tr." + className).removeClass("hidden");            

            $("#grid1 tr." + className + " span.tree-expanded").removeClass("tree-expand");
            $("#grid_1st_Column tr." + className + " span.tree-expanded").removeClass("tree-expand");

            $('#' + className).val('');
        }
        else {
            $("#grid1 tr." + className).addClass("hidden");
            $("#grid_1st_Column tr." + className).addClass("hidden");
            $(obj).addClass("tree-expand");
            $('#' + className).val('tree-expand');
        }

        mainreport.gridScroller(gridOpts);
        
    },
};

$(document).ready(function () {
    Helper.renderCalendarFromTo("#txtfromdate, #txttodate");
    var gridOpts = {
        tblContentId: 'grid1'
    };
    $("#mainreport").initGrid(gridOpts);
});



var report = {

    SelNumGameWL: function () {
        
        var param = {
            fromdate: $('#txtfromdate').val(),
            todate: $('#txttodate').val()
        };

        $('#gridtitle').html('Number Game Monthly Winloss From ' + param.fromdate + ' To ' + param.todate);

        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "Post",
            url: "SelNumberGameWL",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                report.renderReport(rs);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    renderReport: function (rs) {
        var mainreport = $("#mainreport");

        mainreport.removeClass("hidden");
        mainreport.resetGrid("grid1");
        $("#grid1 tbody").empty().append(report.genBody(rs));
        mainreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:150px;" class="c table-cell">' + rs.Total.Name + '</div></th></tr>');
        mainreport.find(".col_footer_content thead").empty().append(report.genFooter(rs.Total));

        var gridWidth = 630;

        var gridOpts = {
            tblContentId: 'grid1',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            gridWidth: gridWidth,
            freezeWidth: 150,
            numberFreezeCol: 1
        };
        mainreport.gridScroller(gridOpts);
    },

    genBody: function (rs) { 
        var html = [];
        var cnt = rs.LiveIndicators.length;
        $('#dvHidden').html('');
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {

                 $('#dvHidden').append('<input type="hidden" value="" id="row_'+ i +'">');

                var rowLiveIndicator = rs.LiveIndicators[i];   
                html.push('<tr style="height: 17px;" class="b"><td style="height: 17px;"><div class="l"><span onclick=\'Helper.showhiderow(this, "row_'+ i +'")\' class="tree-expanded"></span>' + rowLiveIndicator.Name + '</div></td>')
                html.push('<td><div class="r">' + Helper.format(rowLiveIndicator.WinLoss) + '</div></td>');
                html.push('<td><div class="r">' + rowLiveIndicator.Position + '</div></td>');
                html.push('<td><div class="r">' + rowLiveIndicator.Comm + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(rowLiveIndicator.LineTotal) + '</div></td>');
                html.push('<td><div class="r">' + rowLiveIndicator.Turnover + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(rowLiveIndicator.Margin) + '</div></td></tr>');

                //Childs
                var cnt_Childs = rowLiveIndicator.Childs.length;
                for(var j = 0; j<cnt_Childs;j++){
                    var child = rowLiveIndicator.Childs[j];
                    html.push('<tr style="height: 17px;" class="row_'+ i +'"><td style="height: 17px;"><div class="r"><span class="indent"></span>' + child.Name + '</div></td>')
                    html.push('<td><div class="r">' + Helper.format(child.WinLoss) + '</div></td>');
                    html.push('<td><div class="r">' + child.Position + '</div></td>');
                    html.push('<td><div class="r">' + child.Comm + '</div></td>');
                    html.push('<td><div class="r">' + Helper.format(child.LineTotal) + '</div></td>');
                    html.push('<td><div class="r">' + child.Turnover + '</div></td>');
                    html.push('<td><div class="r">' + Helper.format(child.Margin) + '</div></td></tr>');
                }
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="7">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooter: function (Total) { 
        var html = [];
        if (Total != null && Total.Name != null) {
            html.push('<tr><th><div style="width:150px;height:30px;" class="table-cell">&nbsp;</div></th>');
            html.push('<th><div style="width:120px;height:30px;" class="r table-cell">' + Helper.format(Total.WinLoss) + '</div></th>');
            html.push('<th><div style="width:120px;height:30px;" class="r table-cell">' + Total.Position + '</div></th>');
            html.push('<th><div style="width:120px;height:30px;" class="r table-cell">' + Total.Comm + '</div></th>');
            html.push('<th><div style="width:120px;height:30px;" class="r table-cell">' + Helper.format(Total.LineTotal)  + '</div></th>');
            html.push('<th><div style="width:120px;height:30px;" class="r table-cell">' + Total.Turnover + '</div></th>');
            html.push('<th><div style="width:120px;height:30px;" class="r table-cell">' + Helper.format(Total.Margin) + '</div></th></tr>');
            
        }
        return html.join('');
    },

     exportExcel: function () {
        var url = 'NGMonthlyWinLoss?IsExport=1';
        url += '&fromdate=' + $('#txtfromdate').val();
        url += '&todate=' + $('#txttodate').val();

        $('#frm').attr('src', url);
    }
}