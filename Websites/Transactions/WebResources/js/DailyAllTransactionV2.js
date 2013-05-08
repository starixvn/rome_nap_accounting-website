var _pageIndex = 1;
var _pageSize = 100;
var _totalItems = 0;

var DailyAllTransaction = {
    transDate: '',
    type: '',
    creator: '',
    renderDailyRow : function(row, cssClass, index){
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="c" style="width:40px">' + index + '</div></td>\
                <td><div class="c" style="width:140px">' + row.transDate + '</div></td>\
                <td><div class="c" style="width:120px">' + row.Username + '</div></td>\
                <td><div class="c" style="width:300px">' + row.Description + '</div></td>\
                <td><div class="c" style="width:80px; font-weight:bold;">' + row.Type + '</div></td>\
                <td><div class="r" style="width:80px">' + row.Amount + '</div></td>\
                <td><div class="c" style="width:150px">' + row.Creator + '</div></td>\
            </tr>';
        return html;
    },
    
     initPagerMain: function (total_item, items_per_page, current_page) {
     var opt = {
         start_per_page: 100,
         step_per_page: 100,
         max_items_per_page: 1000,
         item_per_page_arr: [100, 200, 300, 400, 500, 600, 700, 800, 900, 1000],
         callback: DailyAllTransaction.pageSelectCallbackMain };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    
    pageSelectCallbackMain: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        _pageIndex = page_index + 1;
        _pageSize = _items_per_page;
        DailyAllTransaction.renderDailyAllTxn(1);
        return false;
    },
    
    setPostedData: function(fromcache){
         var data = {
            date: DailyAllTransaction.transDate,
            type: DailyAllTransaction.type,
            creator: DailyAllTransaction.creator,
            pageIndex: _pageIndex,
            pageSize: _pageSize,
            fromcache: fromcache
        };
        return data;
        
    },
    
    renderDailyAllTxn: function(fromcache){
        $("#ErrMsg").html("");
        DailyAllTransaction.transDate = $("#txndate").val();
        DailyAllTransaction.type = $("#txtType").val();
        DailyAllTransaction.creator = $("#txtCreator").val();
        if(parseInt(fromcache) == 0)
            _pageIndex = 1;
        var data = DailyAllTransaction.setPostedData(fromcache); 
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../Services/WCF/ServiceDailyTran.svc/GetDailyAllTransData",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function(rs){
                var items = rs.items;
                var arr = [];
                var count = 0;
                if (items != null && items.length > 0) {
                      var numberitems = items.length;
                      for (var i = 0; i < numberitems; i++) {
                         count ++;
                         var index = _pageSize * (_pageIndex - 1) + count;
                         arr.push(DailyAllTransaction.renderDailyRow(items[i], (i % 2 == 0 ? "bgRow" : ""), index));
                      }
                      $("#tblDailyAllTxn").empty().html(arr.join(''));
                      _totalItems = rs.totalRows;
                     DailyAllTransaction.initPagerMain(_totalItems, _pageSize , _pageIndex - 1); 
                    
                }
                else {
                    $("#tblDailyAllTxn").empty().html('<tr><td colspan="10"><div class="c" style="width:928px">There is no information available.</div></td></tr>');
                    $("#pagination").html('&nbsp;');
                }
                 $("#DailyAllTxn").removeClass("hidden").reRenderScroller(931, 600, true);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    exportExcel: function(){
        try { parent.BlockUI(); } catch (e) { }
        var getData = this.setPostedData(1);
        var url = 'DailyAllTransaction.aspx?IsExport=1&transDate=' + DailyAllTransaction.transDate + '&type=' +
            DailyAllTransaction.type + '&creator' + DailyAllTransaction.creator + '&pageIndex' + getData.pageIndex + '&pageSize=' + getData.pageSize +
            '&fromcache=' + getData.fromcache + '&totalRows=' + _totalItems; 
        if(_totalItems > 325000)
            alert('The number of records are too large to export to excel.');
        else
            $('#frm').attr('src', url);
       
        try { parent.jQuery.unblockUI(); } catch (e) { }
       
    }
};