var CreditTerm = {};
$.extend(CreditTerm, { Group: {
    rows: null,
    rows_show: null,
    getAllView: function () {
        JAjax.call({
            url: '../Services/WCF/CreditTerm/Handler.svc/GetAllGroup',
            data: {},
            success: function (resp) {
                $("#credit_content").show();
                if (typeof (resp) != 'undefined') {
                    CreditTerm.Group.rows = resp;
                    CreditTerm.Group.rows_show = resp;
                    CreditTerm.Group.displayGrid(resp);
                    sorting.init();
                } else {
                    $("#group_content").html('<div class="item">There is no information available.</div>');
                }
            },
            isRefresh: true
        });
    },
    displayGrid: function (resp) {
        if (resp.length > 0) {
            $("#group_content").empty();
            var _templateRow = '{{each Rows}}<div class="item {{if $index % 2==0}}even{{/if}}">'
                        + '<div class="index">${$index+1}</div>'
                        + '<div class="group" rel="group">${$value.GroupId}</div>'
                        + '<div class="uname" rel="username">{{html $value.HeadUsername}}</div>'
                        + '<div class="currency">${$value.Currency}</div>'
                        + '<div class="members">${$value.Members}</div>'
                        + '<div class="checker">${$value.Checker}</div>'
                        + '<div class="consolidationreport">'
                        + '{{if $value.RoleId != 1}}<span onmouseover="" onmouseout="" onclick="CreditTerm.Group.updateConsolidationReportAllowStatus(${$value.CustId}, \'#allow_${$value.GroupId}\')" class="{{if $value.ConsolidationReport}}allow-button{{else}}disallow-button{{/if}} c" id="allow_${$value.GroupId}"></span>{{/if}}'
                        + '</div>'
                        + '<div class="edit"><img rel="${$value.GroupId}" class="edit_group" style="cursor: pointer;" src="../App_Themes/Images/icn_edit.png" alt="Edit" title="Edit Group"></div>'
                    + '</div>{{/each}}';
            $.tmpl(_templateRow, { Rows: resp }).appendTo("#group_content");
            $('div.item').bind('mouseover', function (evt) {
                $('div.item').removeClass('active');
                $(this).addClass('active');
            });
            $('div.item').bind('mouseout', function (evt) {
                $('div.item').removeClass('active');
            });
            $('img.edit_group').bind('click', function (evt) {
                CreditTerm.Group.editGroup($(this).attr('rel'));
            });
            window.setTimeout(function () {
                CreditTerm.Group.formatGrid();
            }, 500);
        } else {
            $("#group_content").html('<div class="item">There is no information available.</div>');
        }
    },
    formatGrid: function () {
        var _key = $.trim($('#txt_search').val());
        if (_key != '') {
            var div_uname = $('div#group_content div.uname');
            $.each(div_uname, function (ind, elm) {
                var _str_replace = sorting.formatResult($(elm).html(), _key);
                $(elm).html(_str_replace);
            });
        }
    },
    editGroup: function (groupId) {
        if (typeof (groupId) != 'undefined') {
            var url = 'Creditterm_Group_Feed.aspx?groupid='.concat(groupId);
            window.location = url;
        }
        return false;
    },
    goSort: function (elm) {
        if ($(elm).hasClass('sorter')) {
            var order = $(elm).hasClass('asc') ? 'desc' : 'asc';
            $(elm).hasClass('asc') ? $(elm).removeClass('asc') : void (0);
            $(elm).hasClass('desc') ? $(elm).removeClass('desc') : void (0);
            $(elm).addClass(order).addClass('active');
            var sort_name = $(elm).attr('sort_name');
            this.rows_show = sorting.execSort(sort_name, order);
            CreditTerm.Group.displayGrid(this.rows_show);
            sorting.reset();
            // update order/sort_name to hidden field (for excel)
            $('#SortOrder').val(order);
            $('#SortName').val(sort_name);
        }
        return false;
    },
    goSearch: function (key) {
        $('div.sorter').removeClass('active');
        var _key = typeof (key) != 'undefined' ? $.trim(key) : $.trim($('#txt_search').val());
        //if (sorting.key_search == _key) { return; }
        sorting.key_search = _key.toLowerCase();
        if (_key != '') {
            if ($('#chkSearchSMA')[0].checked) {
                JAjax.call({
                    url: '../Services/WCF/CreditTerm/Handler.svc/GetGroupIdByUsername?username=' + _key,
                    success: function (resp) {
                        if (resp != 0) {
                            var url = 'Creditterm_Group_Feed.aspx?groupid='.concat(resp);
                            window.location = url;
                        } else {
                            $("#group_content").html('<div class="item">There is no information available.</div>');
                        }
                    }
                });
            }
            else {
                this.rows_show = sorting.execSearch(_key);
            }
        } else {
            this.rows_show = this.rows;
        }
        $('#Query').val(_key);
        if (this.rows_show != null) {
            var elm = $('div.sorter.uname');
            if ($(elm).hasClass('asc')) {
                $(elm).removeClass('asc').addClass('desc');
            } else {
                if ($(elm).hasClass('desc')) {
                    $(elm).removeClass('desc').addClass('asc');
                } else {
                    $(elm).addClass('desc');
                }
            }
            CreditTerm.Group.goSort(elm);
        }
        return false;
    },
    goRefresh: function () {
        JAjax.call({
            url: '../Services/WCF/CreditTerm/Handler.svc/GetAllGroup',
            success: function (resp) {
                if (typeof (resp) != 'undefined') {
                    CreditTerm.Group.rows = resp;
                    CreditTerm.Group.rows_show = resp;
                    var elm = $('div.sorter.uname');
                    $(elm).removeClass('asc');
                    CreditTerm.Group.goSort(elm);
                } else {
                    $("#group_content").html('<div class="item">There is no information available.</div>');
                }
            }
        });
    },
    updateConsolidationReportAllowStatus: function (custId, element) {
        var isViewReport = $(element).hasClass('allow-button');
        JAjax.call({
            url: '../Services/WCF/CreditTerm/Handler.svc/UpdateConsolidationReportAllowStatus',
            data: { custId: custId, isViewReport: !isViewReport },
            success: function (result) {
                if (result.ErrorCode == 0) {
                    if (isViewReport) {
                        $(element).addClass('disallow-button');
                        $(element).removeClass('allow-button');
                    } else {
                        $(element).removeClass('disallow-button');
                        $(element).addClass('allow-button');
                    }
                } else {
                    alert(result.ErrorMessage);
                }
            },
            isRefresh: true
        });
    }
}
});
// js sorting
var sorting = {
    options: {
        order_by: 'asc',
        initial: ''
    },
    key_search: '',
    init: function (options) {
        this.options = $.extend({}, this.options, options);
        $("div.sorter").bind("click", function (evt) {
            $("div.sorter").removeClass('active');
            CreditTerm.Group.goSort($(this));
        });
        if (this.options.initial != '') {
            var elm = $('div.sorter.' + this.options.initial);
            var order = (this.options.order_by == 'asc') ? 'desc' : 'asc';
            $(elm).addClass(order);
            CreditTerm.Group.goSort(elm[0]);
        }
        $('input#btn_search').bind('click', function () {
            CreditTerm.Group.goSearch();
        });
        $('input#txt_search').keydown(function (evt) {
            if (evt.which == '13') {
                evt.preventDefault();
                CreditTerm.Group.goSearch();
            }
        });
        $('div#refresh').bind('click', function (evt) {
            $('input#txt_search').val('');
            $('div.sorter').removeClass('active');
            CreditTerm.Group.goRefresh();
        });
        $('input#btn_clear').bind('click', function (evt) {
            $('input#txt_search').val('');
            $('div.sorter').removeClass('active');
            CreditTerm.Group.goSearch();
        });
        $('input#txt_search').val('');
        if ($.browser.safari || $.browser.webkit) {
            $('#btn_search').css('margin-top', '-5px');
        }
    },
    execSort: function (name, order) {
        var arr_sorted = [];
        if (name == "group") {
            arr_sorted = CreditTerm.Group.rows_show.sort(sorting.compareGroup);
            if (order == 'asc') {
                arr_sorted.reverse();
            }
        } else if (name == "uname") {
            var arr_sorted = CreditTerm.Group.rows_show.sort(sorting.compareUname);
            if (order == 'desc') {
                arr_sorted.reverse();
            }
        }
        return arr_sorted;
    },
    reset: function () {
        $.each($("div.sorter"), function (i, elm) {
            if (!$(elm).hasClass('active')) {
                $(elm).removeClass('asc');
                $(elm).removeClass('desc');
            }
        });
        return false;
    },
    compareGroup: function (a, b) {
        return b.GroupId - a.GroupId;
    },
    compareUname: function (a, b) {
        if (a.HeadUsername.toLowerCase() > b.HeadUsername.toLowerCase()) { return 1; }
        if (a.HeadUsername.toLowerCase() < b.HeadUsername.toLowerCase()) { return -1; }
        return 0;
    },
    execSearch: function (key) {
        key = typeof key == 'undefined' ? '' : key;
        var arr_sorted = [];
        if ($.trim(key) != '') {
            $.each(CreditTerm.Group.rows, function (i, group) {
                var rs = group.HeadUsername.toLowerCase().indexOf(key.toLowerCase());
                if (rs != -1) {
                    arr_sorted.push(group);
                }
            });
        }
        return arr_sorted;
    },
    formatResult: function (value, match_val) {
        var reEscape = new RegExp('(\\' + ['/', '~', '!', '@', '#', '.', '*', '$', '^', '+', '?', '|', '(', ')', '[', ']', '{', '}', '\\'].join('|\\') + ')', 'g');
        var pattern = '(' + match_val.replace(reEscape, '\\$1') + ')';
        return value.replace(new RegExp(pattern, 'gi'), '<b>$1<\/b>');
    }
};
