var numberOfBets = {
    fDate: '',
    tDate: '',
    TplContent: '{{each(i,val) rows}}<div class="{{if (i == rows.length-1)}}countBet-content-foreachrow-lastrow{{else}}countBet-content-foreachrow{{/if}}">\
                            <div class="countBet-content-left">&nbsp;${Site}</div>\
                            <div class="countBet-content-right">${BetCount}&nbsp;</div>\
                        </div>{{/each}}',
    TplFooter: '<div class="countBet-Header-left">${Site}&nbsp;</div>\
               <div class="countBet-Header-right">${BetCount}&nbsp;</div>',
    getReport: function () {
        $('#mainContentReport').hide();
        $('#countbetdetail').html('');
        $('#countbetFooter').html('');
        $('#numberofbets').hide();
        $('#ErrMsg').hide();
        $('#btnExportIcon').hide();

        try { parent.BlockUI(); } catch (e) { }

        this.fDate = $('#fdate').val();
        this.tDate = $('#tdate').val();
        $.ajax({
            type: "POST",
            url: "Services/OthersReportService.svc/SelectNumberOfBetsForCreditSites",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"fromDate":"' + this.fDate + '","toDate":"' + this.tDate + '"}',
            success: function (rs) {
                if (rs != null && rs.rows != null && rs.rows.length > 0) {
                    $.tmpl(numberOfBets.TplContent, rs).appendTo('#countbetdetail');
                    $.tmpl(numberOfBets.TplFooter, rs.total).appendTo('#countbetFooter');
                    $('#numberofbets').show();
                    $('#btnExportIcon').show();
                }
                else {
                    showAlertMessage("ErrMsg", nodataerror, 1);
                    $('#ErrMsg').show();
                }
                $('#mainContentReport').show();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            error: function () {
                showAlertMessage("ErrMsg", nodataerror, 1);
                $('#ErrMsg').show();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    exportExcel: function () {
        this.fDate = $('#fdate').val();
        this.tDate = $('#tdate').val();
        var url = 'NumberOfBets.aspx?export=1&fdate=' + this.fDate + '&tdate=' + this.tDate;
        $('#frm').attr('src', url);
    }
}