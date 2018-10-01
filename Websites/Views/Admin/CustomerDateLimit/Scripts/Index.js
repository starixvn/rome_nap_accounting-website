var gridHeight = $(window).height() - 150;

var CustomerDataRange = {
    pageIndex: 1,
    pageSize: 10,
    totalItems: 0,
    width: 516,
    height: 590,

    CachedData: {
        editIcon: '../App_Themes/Images/icn_edit.png',
    },

    init: function(){
        $("#txtusername").keyup(function (e) {
            var code = e.keyCode || e.which;
            if (code === 13) {
                CustomerDataRange.getList(0);
            }
        });

        CustomerDataRange.getList(0);
    },

    getList: function (fromCache) {
        if (fromCache == 0)
            CustomerDataRange.pageIndex = 1;

        var data = { UserName: $('#txtusername').val(), PageSize: CustomerDataRange.pageSize, PageIndex: CustomerDataRange.pageIndex };
        try { parent.BlockUI(); } catch (e) { }

        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerDateLimit/Get",
            dataType: "json",
            data: data,
            success: function (rs) {
                var arr = [];
                if (rs != null) {
                    if (rs.Customers.length > 0) {
                        var index = (CustomerDataRange.pageSize * (CustomerDataRange.pageIndex - 1) + 1);
                        for (var i = 0; i < rs.Customers.length; i++) {
                            var Item = rs.Customers[i];
                            if (i % 2 != 0)
                                arr.push(CustomerDataRange.renderRows(index, Item, 'bgeven'));
                            else
                                arr.push(CustomerDataRange.renderRows(index, Item, ''));
                            index++;
                        }
                        $("#tblCustomerDataRange").empty().html(arr.join(''));
                        CustomerDataRange.totalItems = rs.TotalRecords;
                        CustomerDataRange.initPager(CustomerDataRange.totalItems, CustomerDataRange.pageSize, CustomerDataRange.pageIndex - 1);
                    }else {
                        $("#pagination").html('&nbsp;');
                        $("#tblCustomerDataRange").html('<tr><td colspan="4"><div class="c" style="width:526px;">There is no information available.</div></td></tr>');
                    }

                    $('#CustomerDataRange').removeClass("hidden").reRenderScroller(529, gridHeight, true);
                }
                else {
                    $("#pagination").html('&nbsp;');
                    $("#tblCustomerDataRange").html('<tr><td colspan="4"><div class="c" style="width:526px;">There is no information available.</div></td></tr>');
                    $('#CustomerDataRange').removeClass("hidden").reRenderScroller(529, gridHeight, true);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        });
    },    

    initPager: function (total_item, items_per_page, current_page) {
        var opt = {
            start_per_page: 50,
            step_per_page: 50,
            max_items_per_page: 500,
            item_per_page_arr: [10, 50, 100, 200, 500],
            callback: CustomerDataRange.pageSelectCallback
        };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(CustomerDataRange.totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        CustomerDataRange.pageIndex = page_index + 1;
        CustomerDataRange.pageSize = _items_per_page;
        CustomerDataRange.getList(1);
        return false;
    },

    renderRows: function (index, row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">';
        html += '<td><div class="col40 c">' + index + '</div></td>';
        html += '<td><div class="col200 l">' + row.UserName + '</div></td>';
        html += '<td><div class="col160 r">' + row.DataAvailable + '</div></td>';
        html += '<td><div class="col160 c"> <img style="cursor:pointer" title="Edit" class="editcontrol editicon" src="'
                     + CustomerDataRange.CachedData.editIcon + '" onclick="CustomerDataRange.edit(' + row.CustId + ',\'' + row.UserName + '\')"\/>\ </div></td>';
        return html;

    },
    edit: function (custId, userName) {
        var url = window.rootUrl + 'CustomerDateLimit/GetById?custId=' + custId;
        parent.OpenNextPopup('', url, 'Edit Data Range For ' + userName, 300, 250, true, false, this, 2000, null, function () {
            location.reload(true);
        });
    },
    add: function () {
        var url = window.rootUrl + 'CustomerDateLimit/Add';
        parent.OpenNextPopup('', url, 'Add Super ', 600, 250, true, false, this, 2000, null, function () {
            location.reload(true);
        });
    }
};

$(document).ready(function () {
    CustomerDataRange.init();
});