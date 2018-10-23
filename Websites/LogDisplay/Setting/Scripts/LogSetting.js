var LogPT = {
    page: 1,
    pageSize: 50,
    custName: null,
    custId: 0,
    fromDate: '',
    toDate: '',
    viewBy: '',
    logType: 'setting',
    Rows: null,
    doSearch: function (_page, _size) {

        var _custName = "", _custId = custId, _fromDate = "", _toDate = "", _viewBy = "";
        if (typeof _page == 'undefined') {
            _page = this.page;
        }
        if (typeof _size != 'undefined' && this.pageSize != _size) {
            this.pageSize = _size;
            _page = 1;
        }
        _custName = $('#custName').val();
        if ($.trim(_custName) == "") {
            $('#RequiredUserName').show();
            $('#custName').focus();
            this.custName = "";
            return;
        }
        //if (this.custName != null && this.custName != _custName) {
        //    document.frmLog.submit();
        //}
        $('#RequiredUserName').hide();
        this.custId = _custId;
        this.custName = _custName;
        this.fromDate = $('#fromDate').val();
        this.toDate = $('#toDate').val();
        this.viewBy = $('#viewBy').val();
        this.page = _page;
        this.getPage();
    },
    getPage: function (_page) {

        if (typeof _page != 'undefined') {
            this.page = _page;
        }
        var _method = '', _param = null;

        _param = { custName: this.custName, fromDate: this.fromDate, toDate: this.toDate, page: this.page, pageSize: this.pageSize };
        switch (this.logType) {
            case 'setting':
                _method = 'GetLogSetting';
                _param = $.extend(_param, { viewBy: this.viewBy });
                break;
            case 'status':
                _method = 'GetLogStatus';
                break;
            case 'credit':
                _method = 'GetLogCredit';
                break;
            case 'login':
                _method = 'GetLogLogin';
                break;
            default:
                break;
        }
        JAjax.call({
            url: 'Services/Setting.svc/'.concat(_method),
            data: _param,
            success: function (resp) {
                $('#log_content').empty();

                if (typeof (resp) != 'undefined') {
                    if (resp.ErrorMessage != null) {
                        $('#log_content').html('<div id="log_scroll" class="alertboxerr" style="border: 1px solid red;"><div>' + resp.ErrorMessage + '</div></div>').show();
                    }
                    else {
                        LogPT.mapTemplate(resp);
                    }
                }
            },
            error: function (xhr, msg) {
                if (xhr.status === 403) {
                    var denyObject = $.parseJSON(xhr.responseText);
                    $('#log_content').html('<div id="log_scroll" class="alertboxerr" style="border: 1px solid red;"><div>' + denyObject.ErrorMessage + '</div></div>').show();
                }
                else {
                    $('#log_content').html('<div id="log_scroll"><p align="center">There is no information available.</p></div>').show();
                }
            }
        });
    },
    mapTemplate: function (resp) {
        //$('#log_content').hide();
        switch (resp.Type) {
            case 1: // for setting
                $.extend(resp, { Rows: resp.Setting });
                $('#tpl_sp').tmpl(resp).appendTo('#log_content');
                break;
            case 2: // for status
                $.extend(resp, { Rows: resp.Status });
                $('#tpl_sp').tmpl(resp).appendTo('#log_content');
                break;
            case 3: // for credit
                $.extend(resp, { Rows: resp.Credit });
                $('#tpl_credit').tmpl(resp).appendTo('#log_content');
                break;
            case 4: // for login
                $.extend(resp, { Rows: resp.Login });
                $('#tpl_login').tmpl(resp).appendTo('#log_content');
                break;
            default:
                break;
        }
        $('#log_content').show();
        // render template
        if (resp.Rows != null && resp.Total > 0) {
            // set rows data 
            LogPT.Rows = resp.Rows;

            window.setTimeout(function () {
                var log_height = 0, scroll_height, max_height;
                max_height = $('#log_scroll').attr('maxheight');
                if (resp.Type == 1 || resp.Type == 2) {
                    log_height = $('#tbl_content').height();
                    scroll_height = log_height > parseInt(max_height) ? parseInt(max_height) : log_height;
                    if ($.browser.msie || $.browser.mozilla) {
                        $('div.w101h').css({ width: 79 });
                        $('div.w200h').css({ width: 219 });
                    }
                }
            }, 500);
            // render diff
            LogPT.diffValue(resp.Rows);
            // set paging
            var opt = { callback: function (page, size) { LogPT.doSearch((page + 1), size) } };
            opt["items_per_page"] = LogPT.pageSize;
            opt["current_page"] = LogPT.page - 1;
            $('#pagination').pagination(resp.Total, opt).show();
        } else {
            $('#pagination').empty().hide();
        }
    },
    showTracking: function (tracking, tbl, diff, action) {
        var clsDiff = typeof (diff) == 'undefined' ? '' : ' diff';
        if (typeof (action) == undefined) { var action = ""; }
        var tpl_tracking = '<table class="tblDesc tbl' + tbl + '"><tbody>';
        if (typeof tracking != 'undefined') {
            // fetch attr
            if (tracking.Attrs != null) {
                // fetch rows
                var total_row = Math.ceil(tracking.Attrs.length / 3); // 6 columns = 3 x N:Val
                var ind = 0;
                for (var r = 0; r < total_row; r++) {
                    var trShow = '', rel = 'atttr'.concat(r);
                    tpl_tracking += '<tr class="' + rel + '" style="display:' + trShow + ';">';
                    for (var r1 = 0; r1 < 3; r1++) {
                        var _attr, _etmp = { N: "", Val: "", P: "" };
                        _attr = (typeof tracking.Attrs[ind] != 'undefined') ? tracking.Attrs[ind] : _etmp;
                        tpl_tracking += '<td class="collabel"><div class="colW1sp">' + _attr.N + '</div></td><td><div class="colW2sp ' + tbl + '-' + (r + 1) + '-' + _attr.P + clsDiff + '">' + _attr.Val + '</div></td>';
                        ind++;
                    }
                    tpl_tracking += '</tr>';
                }
            }
            // fetch vals
            if (tracking.Vals != null) {
                for (var i = 0, len1 = tracking.Vals.length; i < len1; i++) {
                    var rel = "div ".concat(tbl) + " info i" + (i + 1);
                    var node = tracking.Vals[i];
                    var node_name = (tracking.Level == 1) ? action : node.N;
                    //var trShow = (i == 0) ? '' : 'none';
                    trShow = '';
                    //if (tracking.Level != 3) {
                    tpl_tracking += '<tr class="headerDetailNormal">'
			                        + '<td colspan="6" onclick="LogPT.showHide(\'' + rel + '\')"  style="cursor: pointer"><span style="float: left;">' + node_name + '</span>'
		                        + '</td></tr>';
                    //}
                    // fetch vals
                    if (node.Vals != null && node.Vals.length > 0) {
                        var total_row = Math.ceil(node.Vals.length / 3); // 6 columns = 3 x N:Val
                        var ind = 0;
                        tpl_tracking += '<tr><td colspan="6">';
                        tpl_tracking += '<div class="' + rel + '" style="display:' + trShow + ';"><table class="tblDesc"><tbody>';
                        for (var r = 0; r < total_row; r++) {
                            tpl_tracking += '<tr>';
                            for (var r1 = 0; r1 < 3; r1++) {
                                var _elm, _etmp = { N: "", Val: "", P: "" };
                                _elm = (typeof node.Vals[ind] != 'undefined') ? node.Vals[ind] : _etmp;
                                tpl_tracking += '<td class="collabel"><div class="colW1sp">' + _elm.N + '</div></td><td><div class="colW2sp ' + tbl + '-' + (i + 1) + '-' + _elm.P + clsDiff + '">' + _elm.Val + '</div></td>';
                                ind++;
                            }
                            tpl_tracking += '</tr>';
                        }
                        tpl_tracking += '</tbody></table></div>';
                        tpl_tracking += '</td></tr>';
                    }
                    // fetch group
                    if (node.Groups != null && node.Groups.length > 0) {
                        tpl_tracking += '<tr><td colspan="6">';
                        tpl_tracking += '<div class="' + rel + '" style="display:' + trShow + ';"><table class="tblDesc"><tbody>';
                        for (var g = 0; g < node.Groups.length; g++) {
                            var group = node.Groups[g], class_group = rel.concat(' group '), rel_group = class_group.concat('g' + (g + 1));
                            if (group.Vals.length == 0) { continue; }
                            tpl_tracking += '<tr class="headerSportTypeDetail"><td colspan="6" style="cursor: pointer;" onclick="LogPT.showHide(\'' + rel_group + '\')">' + group.N + '</td></tr>';
                            tpl_tracking += '<tr><td colspan="6"><div class="' + rel_group + '"><table class="tblDesc"><tbody>';
                            var total_val = Math.ceil(group.Vals.length / 3); // 6 columns = 3 x N:Val
                            var ind_g = 0;
                            for (var r = 0; r < total_val; r++) {
                                tpl_tracking += '<tr>';
                                for (var r1 = 0; r1 < 3; r1++) {
                                    var _elm, _etmp = { N: "", Val: "", P: "" };
                                    _elm = (typeof group.Vals[ind_g] != 'undefined') ? group.Vals[ind_g] : _etmp;
                                    tpl_tracking += '<td class="collabel"><div class="colW1sp">' + _elm.N + '</div></td><td><div class="colW2sp ' + tbl + '-' + (i + 1) + '-' + _elm.P + clsDiff + '">' + _elm.Val + '</div></td>';
                                    ind_g++;
                                }
                                tpl_tracking += '</tr>';
                            }
                            tpl_tracking += '</tbody></table></div></td></tr>';
                        }
                        tpl_tracking += '</tbody></table></div>';
                        tpl_tracking += '</td></tr>';
                    }
                }
            }
        }
        tpl_tracking += '</tbody></table>';
        return tpl_tracking;
    },
    showHide: function (rel) {
        if (rel) {
            $('div.' + rel.replace(/ /g, '.')).slideToggle();
        }
        return false;
    },
    redirect: function (tab) {
        if (tab) {
            var cls = 'current', idTab = $(tab).parent().attr('id').toString();
            $('li.tablog').removeClass(cls);
            $(tab).parent().addClass(cls);
            switch (idTab) {
                case "setting":
                    $('span#log_type').show();
                    break;
                case "status":
                case "credit":
                case "login":
                    $('span#log_type').hide();
                    break;
                default:
                    break;
            }
            if (idTab != "") {
                $('input#tabid').val(idTab);
                if (this.logType != idTab.toLowerCase()) {
                    this.logType = idTab.toLowerCase();
                    this.doSearch(1);
                }
            }
        }
        return false;
    },
    setTab: function (tab) {
        var cls = 'current';
        if (tab.toLowerCase().indexOf('setting') != -1) {
            this.logType = 'setting';
            $('span#log_type').show();
        } else if (tab.toLowerCase().indexOf('status') != -1) {
            this.logType = 'status';
        } else if (tab.toLowerCase().indexOf('credit') != -1) {
            this.logType = 'credit';
        } else if (tab.toLowerCase().indexOf('login') != -1) {
            this.logType = 'login';
        } else {
            this.logType = 'setting';
            $('span#log_type').show();
        }
        $('input#tabid').val(tab);
        $('#' + this.logType).addClass(cls);
    },
    diffValue: function (rows) {
        for (r in rows) {
            var row = rows[r], ID = row.ID;
            var Groups = row.Detail && row.Detail.Vals ? row.Detail.Vals.length : 1;
            this.queue.push({ Diffs: row.Diffs, ID: row.ID, Groups: Groups });
            //            for (e in row.Diffs) {
            //                var elm = row.Diffs[e], relClass = 'div.'.concat(ID).concat('-').concat(elm.P);
            //                $(relClass).parent().css({ 'background-color': '#ffa' });
            //                $(relClass).css({ color: 'red' });
            //                var val_diff = $.trim(elm.Val).length == 0 ? '_' : $.trim(elm.Val);
            //                $(relClass).append('<br /><font color="blue">' + val_diff + '</font>');
            //            }
        }
        this.queue.process();
    },
    diffTip: function (elm, is_show) {
        var pos = $(elm).position(), diff = $(elm).attr('diff');
        var diffTip = $("div#diffTip");
        if (diffTip.length == 0) {
            diffTip = $('<div />').attr('id', 'diffTip')
                .addClass('diffTip')
                .bind('mouseout', function (e) { $(this).hide(); })
                .appendTo('body');
        }
        var pos_left = parseInt(pos.left);
        pos_left = ((pos_left + 130) > $('body').width()) ? (pos_left - 100) : pos_left;
        diffTip.html('<b>Old Value:</b> <font color="blue">' + diff + '</font>').css({ top: parseInt(pos.top - 20), left: pos_left });
        if (is_show) {
            diffTip.show();
        } else {
            diffTip.hide();
        }
        return false;
    },
    diffPopup: function (elm) {
        var pos = $(elm).position(), ID = $(elm).attr('rel'), tracking = null;
        var diffPopup = $("div#diffPopup");
        if (diffPopup.length == 0) {
            diffPopup = $('<div />').attr('id', 'diffPopup')
                .addClass('diffPopup')
                .appendTo('body');
        }
        var row = null;
        for (r in LogPT.Rows) {
            row = LogPT.Rows[r];
            if (row.ID == ID) {
                tracking = row.Old; break;
            }
        }
        diffPopup.empty();
        $('#tpl_diff_popup').tmpl({ ID: ID, tracking: tracking, Act: row.Act }).appendTo(diffPopup);
        window.setTimeout(function () {
            diffPopup.css({ top: parseInt(pos.top - 30), position: 'absolute', width: 450, left: parseInt(pos.left - 830) }).show();
            $(diffPopup).draggable({
                handle: '#diffhead'
            });
            $('#diffclose').bind('click', function (e) { $("div#diffPopup").hide(); });
            if ($.browser.msie) {
                $('#diffclose').css({ 'margin-top': -17 });
            }

            $('div.tbl'.concat(ID)).show();
            $('table.tbl'.concat(ID)).find('div.div.' + ID + '.info').hide();
            $('table.tbl'.concat(ID)).find('div.div.' + ID + '.info.i1').show();
            $('table.tbl'.concat(ID)).find('div.div.' + ID + '.group').hide();
            LogPT.formatDiff(LogPT.Rows);
        }, 500);
        return false;
    },
    formatDiff: function (rows) {
        for (r in rows) {
            var row = rows[r], ID = row.ID;
            for (e in row.Diffs) {
                var elm = row.Diffs[e], diffClass = 'div.'.concat(ID).concat('-').concat(elm.P).concat('.diff');
                $(diffClass).parent().css({ color: 'blue', 'background-color': '#ffa' });
            }
        }
        return false;
    },
    queue: {
        qDiff: [],
        push: function (item) {
            if (item) {
                this.qDiff.push(item);
            }
        },
        process: function () {
            if (this.qDiff.length > 0) {
                var row = this.qDiff.shift(), ID = row.ID;
                var idx = 0;
                for (e in row.Diffs) {
                    var elms = row.Diffs[e];
                    ++idx;
                    for (var i = 0; i < elms.length; i++) {
                        var item = elms[i];

                        for (var j = 0; j < row.Groups; j++) {
                            var tmpIdx = row.Groups > 1 ? j + 1 : j;
                            var relClass = 'div.'.concat(ID).concat('-').concat(idx + tmpIdx).concat('-').concat(item.P);

                            if ($(relClass).text().indexOf('<br />') == -1) {
                                $(relClass).parent().css({ 'background-color': '#ffa' });
                                $(relClass).css({ color: 'red' });
                                var val_diff = $.trim(item.Val).length == 0 ? '_' : $.trim(item.Val);

                                $(relClass).append('<br /><font color="blue">' + val_diff + '</font>');

                                if ($(relClass).parent()[0]) {
                                    break;
                                }
                            }
                        }
                    }
                }
                window.setTimeout(function () {
                    LogPT.queue.process();
                }, 100);
            }
        }
    }
};