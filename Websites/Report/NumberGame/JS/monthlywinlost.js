//$.tmpl(TplMain, RacingSettingObj).appendTo("#" + RacingSetting.ContainerId);


var tplRowData = '<tr class="bgborder"> \
                        <td>\
                            <div class="col100 c" style="color:${GetNumberColor(Winlost)}">\
                                ${Winlost}</div>\
                        </td>\
                        <td>\
                            <div class="col100 c" style="color:${GetNumberColor(Position)}">\
                                ${Position}</div>\
                        </td>\
                        <td>\
                            <div class="col100 c" style="color:${GetNumberColor(Commission)}">\
                                ${Commission}</div>\
                        </td>\
                        <td>\
                            <div class="col100 c" style="color:${GetNumberColor(LineTotal)}">\
                                ${LineTotal}</div>\
                        </td>\
                        <td>\
                            <div class="col100 c" style="color:${GetNumberColor(TurnOver)}">\
                                ${TurnOver}</div>\
                        </td>\
                        <td>\
                            <div class="col100 c" style="color:${GetNumberColor(Margin)}">\
                                ${Margin}</div>\
                        </td>\
                    </tr>';

function MonthlyWinloss()  {
    this.Winlost = 0;
    this.Position =  0;
    this.Commission = 0;
    this.LineTotal = 0;
    this.TurnOver = 0;
    this.Margin =  0
}

//var MonthlyWinloss =  {
//        Winlost : 0
//    this.Position =  0;
//    this.Commission = 0;
//    this.LineTotal = 0;
//    this.TurnOver = 0;
//    this.Margin =  0
//}

function GetReport() {

    try { parent.BlockUI(); } catch (e) { }
    fromcache = false;
    _pageIndex = 1;
    _pageSize = 50;
    $("#err").remove();
    $("#backbtn").hide();
    $("#matchwinlostdetail").hide();
    
    var selectedMonth = $("#cbMonth").val();
    if (selectedMonth != "") {
        var data = selectedMonth.split("_");

        if (data.length > 1) {
            $.ajax({
                type: "Post",
                url: "../../Services/WCF/Report/ServiceReportNumberGame.svc/MonthlyWinloss",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"month":' + data[0] + ', "year":' + data[1] + '}',
                success: function (rs) {
                    //var data = new MonthlyWinloss();
                    //$.extend(data, rs);
                    var tbContent = $("#tbContent");
                    tbContent.empty();

                    $("#divContent").css("display", "block");
                    $("#btnExport").attr("disabled", "");

                    $.tmpl(tplRowData, rs).appendTo(tbContent);


                    try { parent.jQuery.unblockUI(); } catch (e) { }


                }
            });
        }
    }
}


function ExportToExcel() {


    var selectedMonth = $("#cbMonth").val();
    if (selectedMonth != "") {
        var data = selectedMonth.split("_");
        
        if (data.length > 1) {
            var params = 'export=1';
            params += '&month=' +data[0];
            params += '&year=' + data[1];            
            $("#frmExport").attr('src', 'MonthlyWinloss.aspx?' + params);
        }
    }
          
}


function GetNumberColor(number) {
    
    return parseFloat(number) < 0 ? '#ff0000' : '';
}