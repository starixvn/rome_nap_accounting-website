var punterByBettype = {

    param: {
        rs:'',
        liveindicator:'',
        bettype:'',
        sporttype:'',
    },    
    columnsSelected:'',
    Periodtype: null,

    renderReport: function (rs, liveindicator, bettype, sporttype) {
        var PunterByBettypereport = $("#PunterByBettypereport");
        PunterByBettypereport.resetGrid("grid4");
        PunterByBettypereport.find(".col_header_content thead").empty().append(punterByBettype.genHeaderFrozenSummary(rs, liveindicator, bettype, sporttype));
        $("#grid4 tbody").empty().append(punterByBettype.genBodyScrollSummary(rs));
        $("#grid4rowheader tbody").empty().append(punterByBettype.gen1stColBodyScroll(rs));

        PunterByBettypereport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:300px;" class="table-cell r">' + rs.Total.Name + '</div></th></tr>');
        PunterByBettypereport.find(".col_footer_content thead").empty().append(punterByBettype.genFooterScrollSummary(rs.Total));

        $('#SummaryLongRange').removeClass("hidden");

        if (punter.param.PeriodType == 4) {
            $('#SummaryLongRange').empty().append(punterByBettype.genSummaryLongRange(rs));
        }
        else {
            $('#SummaryLongRange').empty();
        }

        var gridOpts = {
            tblContentId: 'grid4',
            manualCopyHeader: true,
            fullWidth: true,           
            fullHeight: true,
            gridHeight: 200 + $("#SummaryLongRange").height(),        
            freezeWidth: 300,
            numberFreezeCol: 1
        };

        PunterByBettypereport.removeClass("hidden");
        var PunterBySporttypereport = $("#PunterBySporttypereport");

        PunterBySporttypereport.addClass("hidden");
        PunterByBettypereport.gridScroller(gridOpts);

    },

    SelTopPunterByBettype: function (liveindicator, bettype, sporttype, isViewAnnualFromLongRange, year) {

        // punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(true);
        PunterAnalysisExportExcel.LiveIndicator = liveindicator;
        PunterAnalysisExportExcel.BettypeIdByBettype = bettype;
        PunterAnalysisExportExcel.SportIdByBettype = sporttype;
        PunterAnalysisExportExcel.SportTypeNameByBettype = PunterAnalysisExportExcel.SportTypeName;

        var _LiveIndicator = punter.param.LiveIndicator;
        var _BettypeId = punter.param.BettypeId;
        var _SportId = punter.param.SportId;

        punter.param.LiveIndicator = liveindicator;
        punter.param.BettypeId = bettype;
        punter.param.SportId = sporttype;

        var oldPeriodType = punter.param.PeriodType;
        var oldFromDate = punter.param.FromDate;
        var oldToDate = punter.param.ToDate;
        var tmpfDate, tmptDate;
        if (isViewAnnualFromLongRange) {

            var _oldFromYear = parseInt(oldFromDate.split('/')[2]);
            var _oldToYear = parseInt(oldToDate.split('/')[2]);
            //mm/dd/yyyy
            tmpfDate = get_dmy(oldFromDate);
            tmptDate = get_dmy(oldToDate);

            if (_oldToYear - _oldFromYear == 0) {
                punter.param.FromDate = tmpfDate.month + '/' + tmpfDate.day + '/' + year;
                punter.param.ToDate = tmptDate.month + '/' + punter.daysInMonth(tmptDate.month, year) + '/' + year;
            }
            else if (_oldFromYear == year) {
                punter.param.FromDate = tmpfDate.month + '/' + tmpfDate.day + '/' + year;
                punter.param.ToDate = '12' + '/31/' + year;
            }
            else if (_oldToYear == year) {
                punter.param.FromDate = '01' + '/01/' + year;
                punter.param.ToDate = tmptDate.month + '/' + punter.daysInMonth(tmptDate.month, year) + '/' + year;
            }
            else {
                punter.param.FromDate = '01' + '/01/' + year;
                punter.param.ToDate = '12' + '/31/' + year;
            }

            punter.param.PeriodType = 3;
            punterByBettype.Periodtype = 3;
        }
        else{
            punterByBettype.Periodtype = null;
        }

        PunterAnalysisExportExcel.PeriodTypeByBettype = punter.param.PeriodType;
        PunterAnalysisExportExcel.FromDateByBettype = punter.param.FromDate;
        PunterAnalysisExportExcel.ToDateByBettype = punter.param.ToDate;

        var param = {
            param: punter.param
        };

        try { parent.BlockUI(); } catch (e) { }

        var action = 'PunterAnalysis/SelTopPunterByBettype';


        $('#msg').empty();

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {         
                
                punterByBettype.param.rs = rs;
                punterByBettype.param.liveindicator = liveindicator;
                punterByBettype.param.bettype = bettype;
                punterByBettype.param.sporttype = sporttype;

                punterByBettype.getColumnsHeader(rs, punter.param.PeriodType);
                punterByBettype.renderReport(rs, liveindicator, bettype, sporttype);

            },
            complete: function () {
                punter.param.FromDate = oldFromDate;
                punter.param.ToDate = oldToDate;
                punter.param.PeriodType = oldPeriodType;

                punter.param.LiveIndicator = _LiveIndicator;
                punter.param.BettypeId = _BettypeId;
                punter.param.SportId = _SportId;
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genHeaderFrozenSummary: function (data, liveindicator, bettype, sporttype) {
        var html = [];
        var cnt = data.Punters.length > 0 ? data.Punters[0].Columns.length : 0;
        var title = '';

        //row1
        html.push("<tr>");
        html.push('<th rowspan="2"><div style="height:17px;width: 300px;">&nbsp;</div></th>');

        if (cnt == 0) {
            html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="table-cell c">&nbsp;</div></th>');
            html.push("<tr><th><div style='height:17px;width:100px;'>Winlost</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Turnover</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Margin</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Bet Cnt</div></th></tr>");
        }

        var periodtype = punterByBettype.Periodtype == null ? punter.param.PeriodType : punterByBettype.Periodtype;

        var _header = '', _preyear = '';
        for (var i = 0; i < cnt; i++) {
            var type = data.Punters[0].Columns[i].Type;
            var year = type.split('|')[0];
            var month = type.split('|')[1];
            

            if ( punterByBettype.IsHideColumn(i)) {
                _preyear = year;
                continue;
            }

            switch (periodtype) {
                case 2: //Monthly                                       
                    html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="table-cell c">' + year + ' - ' + punter.arrMonths[month - 1] + '</div></th>');
                    break;
                case 3: //Annual                                         
                    if (i == 0) {
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">Year ' + year + '</div></th>');
                    }
                    else {
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">' + punter.arrMonths[month - 1] + '</div></th>');
                    }
                    break;                
                case 4: //Long Range      
                    if (i == cnt - 1) {
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">Grand Total</div></th>');
                    }
                    else {
                        var onlick = "punterByBettype.SelTopPunterByBettype(" + liveindicator + "," + bettype + "," + sporttype + "," + true + "," + year + ")";
                        html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="c table-cell link" onclick="' + onlick + '" >Year ' + year + '</div></th>');
                    }
                    break;                
                case 0: //Daily
                    if (i == cnt - 1) {                        
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">Grand Total</div></th>');
                    }
                    else {
                        if (_preyear != year) {
                            _preyear = year;
                            _header = 'Year ' + year;
                        }
                        else {
                            _header = punter.arrMonths[month - 1];
                        }

                        var onlick = "";
                        html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="c table-cell" onclick="' + onlick + '">' + _header + '</div></th>');
                    }
                    break;
                case 1: //Weekly
                    if (i == cnt - 1) {                        
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">Grand Total</div></th>');
                    }
                    else {
                       _header = "Week " + month.split('_')[0] + " ("+ month.split('_')[1] +")";
                        var onlick = "";
                        html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="c table-cell" onclick="' + onlick + '">' + _header + '</div></th>');
                    }
                    break;
            }

        }
        html.push("</tr>");

        //row2
        html.push("<tr>");
        for (var i = 0; i < cnt; i++) {
            var type = '';
            
            if (punterByBettype.IsHideColumn(i)) {
                continue;
            }
            
            html.push("<th class='" + type + "'><div style='height:17px;width:100px;'>Winlost</div></th>");
            html.push("<th class='" + type + "'><div style='height:17px;width:100px;'>Turnover</div></th>");
            html.push("<th class='" + type + "'><div style='height:17px;width:100px;'>Margin</div></th>");
            html.push("<th class='" + type + "'><div style='height:17px;width:100px;'>Bet Cnt</div></th>");
        }
        html.push("</tr>");

        return html.join('');
    },

    gen1stColBodyScroll: function (rs) {
        var html = [];
        var cnt = rs.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');
                var name = rs.Punters[i].Name;
                html.push('<td ><div style="width:300px;" class="l" >' + name + '</div></td>');
                html.push('</tr>');
            }
        }
        else {
            html.push('<tr>');
            html.push('<td ><div style="width:300px;height:17px;" class="c" >&nbsp;</div></td>');
            html.push('</tr>');
        }
        return html.join('');
    },

    genBodyScrollSummary: function (rs) {
        var html = [];

        var cnt = rs.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');

                var name = rs.Punters[i].Name;
                html.push('<td><div style="width:300px;" class="c" >' + rs.Punters[i].Name + '</div></td>');
                for (var j = 0; j < rs.Punters[i].Columns.length; j++) {//Tables
                    var data = rs.Punters[i].Columns[j];
                    
                    if ( punterByBettype.IsHideColumn(j)){
                         continue;
                    }

                    html.push('<td class="' + '' + '"><div class="r">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></td>');
                    html.push('<td class="' + '' + '"><div class="r">' + (data.HasData == 1 ? data.Turnover : '') + '</div></td>');
                    html.push('<td class="' + '' + '"><div class="r">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></td>');
                    html.push('<td class="' + '' + '"><div class="r">' + (data.HasData == 1 ? data.Betcount : '') + '</div></td>');
                }

                html.push('</tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="4">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];

        html.push('<tr>');
        html.push('<th><div style="width:300px; height:30px;" class="c table-cell" >' + total.Name + '</div></th>');
        for (var j = 0; j < total.Columns.length; j++) {//Tables
            var data = total.Columns[j];
            
            if (punterByBettype.IsHideColumn(j)){
                 continue;
            }

            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Turnover : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Betcount : '') + '</div></th>');
        }

        html.push('</tr>');

        return html.join('');
    },

    genSummaryLongRange: function (rs) {
        var html = [];
        if (rs.GrandTotal == null || rs.GrandTotal.length == 0)
            return html.join('');

        html.push('<table class="tblSum" style="margin-bottom:6px;">');
        html.push('<tr><td class="title" colspan="5">' + $('#spLiveindicator').html() + '</tr>');
        html.push('<tr><td class="c title" style="width:102px">Year</td>');
        html.push('<td class="c title" style="width:102px">Winlost</td>');
        html.push('<td class="c title" style="width:102px">Turnover</td>');
        html.push('<td class="c title" style="width:102px">Margin</td>');
        html.push('<td class="c title" style="width:102px">Bet Count</td></tr>');

        for (var i = 0; i < rs.GrandTotal.length; i++) {
            var data = rs.GrandTotal[i];
            html.push('<tr><td class="c">' + data.Id + '</td>');
            html.push('<td class="r">' + Helper.format(data.Winlost) + '</td>');
            html.push('<td class="r">' + data.Turnover + '</td>');
            html.push('<td class="r">' + Helper.format(data.Margin) + '</td>');
            html.push('<td class="r">' + data.Betcount + '</td></tr>');
        }

        html.push('</table>');
        return html.join('');
    },

    callBackSelTopPunterBySporttype: function () {
        $("#PunterBySporttypereport").removeClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");
        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
    },
  
    getColumnsHeader: function (rs, periodType) {
        var arrColumns = [];
        if (rs.Punters.length > 0) {
            var columns = rs.Punters[0].Columns;
            var header, preyear, year, month;
            for (var i = 0; i < columns.length; i++) {
                year = columns[i].Type.split('|')[0];
                month = columns[i].Type.split('|')[1];
                if(periodType == 1){//weekly
                     if (i == columns.length - 1) {
                        header = 'Grand Total';
                    }
                    else{
                        header = "Week " + month.split('_')[0] + " ("+ month.split('_')[1] +")";
                    }
                }
                else if(periodType == 2){//Monthly                
                    header = year +  ' - ' + punter.arrMonths[month - 1] ;
                }
                else if(periodType == 3){//Annualy
                   if (preyear != year) {
                        preyear = year;
                        header = 'Year ' + year;
                    }
                    else {
                        header = year + (year.length > 0 ?  ' - ' : '') + punter.arrMonths[month - 1] ;
                    } 
                }
                else{
                    if (preyear != year) {
                        preyear = year;
                        header = 'Year ' + year;
                    }
                    else {
                        header = punter.arrMonths[month - 1] ;
                    }

                    if (i == columns.length - 1) {
                        header = 'Grand Total';
                    }
                }

                arrColumns.push({
                    Id: i,
                    Name: header
                });
            }          
        }

        punterByBettype.renderColumnsMultiSelected(arrColumns, 'ddlGroupColumns');
    },

    getSelectedColumn: function () {
        var id = 'ddlGroupColumns';

        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";

        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }

        result = result.replace(/,*$/, "");
        return result;
    },

    renderColumnsMultiSelected: function (data, id) {
        var columns = [];
        for (var i = 0; i < data.length; i++) {

            columns.push({
                Id: id + data[i].Id,
                Value: data[i].Id + '',
                Text: data[i].Name + '',
                Checked: true,
                Enable: true,
                CssClass: ""
            });
        }

        $("#" + id).multiSelect(
        {
            minWidth: 200,
            maxHeight: 60,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All',
            noneSelectedText: 'Select an option',
            listData: columns,
            onCheck: function () {
                var ids = punterByBettype.getSelectedColumn();                
                punterByBettype.columnsSelected = ids;

                if (ids == '') {
                    showAlertMessage("msg", "Please choose at least one column.", 1);                    
                }
                else {                    
                    $('#msg').html('');
                    
                    var rs = punterByBettype.param.rs;
                    var liveindicator = punterByBettype.param.liveindicator;
                    var bettype = punterByBettype.param.bettype;
                    var sporttype = punterByBettype.param.sporttype;

                    punterByBettype.renderReport(rs, liveindicator, bettype, sporttype);
                }
            },
            onLeave: function()
            {
                punterByBettype.columnSetting();                
            }
        });

        var ids = punterByBettype.getSelectedColumn();                
        punterByBettype.columnsSelected = ids;
    },

    IsHideColumn: function (columnIndex){
        var ids = punterByBettype.columnsSelected;

        if(ids.length ==0)
            return false;

        var columns = ids.split(',');
        
        var isHide = true;        
        for(var i = 0; i<columns.length;i++){
            if(columnIndex == columns[i]){
                isHide = false;
                isExist = true;
                break;
            }
        }        

        return isHide;
    },

    columnSetting: function(){        
        var id = 'spGroupColumns';
        var dropDownId = 'ddlGroupColumns';

        if($('#' + id).hasClass('hide')){
            $('#' + id).removeClass('hide');
            $('#' + dropDownId).multiSelect('openMe');
        }
        else{
            $('#' + id).addClass('hide');
            $('#' + dropDownId).multiSelect('hideMe');
        }
        
    }
    

};