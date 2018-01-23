var specialdelete = {
    betteam4: '',
    renderTitleRow: function(row, ind) {
        var html = '';
        var ddldelete = '';
        if (row.IsAllowOptionDelete) {
            ddldelete = '<select onchange="ddl_change(\'' + row.Type + '\');" name="ddl' + row.Type + '" id="ddl' + row.Type + '">\
                            <option selected="selected" value="0" ' + (row.OptionType == '0' ? 'selected="selected"' : '') + ' >Not Delete</option>\
                            <option value="1" ' + (row.OptionType == '1' ? 'selected="selected"' : '') + '> Delete All</option>\
                            <option value="2" ' + (row.OptionType == '2' ? 'selected="selected"' : '') + '> Optional</option>\
                        </select>';
        }
        //width:1040 + 13+13*2
        html = '<tr style="background-color:#6495ed;"><td colspan="14"><div class="c b" style="width:1079px;color:#fff">' + row.Type + '</div></td><td><div class="col80 c">' + ddldelete + '</div></td></tr>'
        return html;
    },
    renderRow: function(row, IsAllowOptionDelete, OptionType, ind) {
        var html = '', oodRow = ((ind + 1) % 2);
        var ref_info = row.Refno + '-' + row.TransId + '-' + row.CurrencyId;
        var optional = '', refno = (oodRow == 0) ? ('<a href="javascript:;" class="mixparlay" ref="' + ref_info + '">' + row.TransId + '<a>') : row.TransId;
        if (IsAllowOptionDelete) {
            optional = '<input type="checkbox" name="chk' + row.Type + '" id="' + row.Refno + '-' + row.Type + '-' + row.IsDelete + '" ' + (OptionType != '2' ? 'disabled="disabled"' : '') + ' ' + ((OptionType == '1' && row.IsDelete != "0") ? 'checked="true"' : '') + '>';
        }

        html = '<tr">\
                <td>\
                    <div class="col40 c">' + row.Order + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.Account + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + refno + '</div>\
                </td>\
                <td >\
                    <div class="col160 c">' + row.Bettype + '</div>\
                </td>\
                <td>\
                    <div class="col40 c">' + row.Betteam + '</div>\
                </td>\
                <td>\
                    <div class="col60 r b"><span style="color:blue;">' + row.Odds + '</span><br>' + row.OddType + '</div>\
                </td>\
                <td>\
                    <div class="col60 r red b">' + row.Amount + '</div>\
                </td>\
                    <td>\
                    <div class="col60 c">' + row.Currency + '</div>\
                </td>\
                    <td>\
                    <div class="col40 c">' + row.Hdp + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.TransTime + '</div>\
                </td>\
                <td>\
                    <div class="col40 c" >' + row.Score.split(' - ')[0] + '</div>\
                </td>\
                 <td>\
                    <div class="col40 c">' + row.Score.split(' - ')[1] + '</div>\
                </td>\
                <td>\
                    <div class="col120 c">' + row.Creator + '</div>\
                </td>\
                <td>\
                    <div class="col60 c">' + row.Status + '</div>\
                </td>\
                <td>\
                    <div class="col80 c">' + optional + '</div>\
                </td>\
            </tr>';
        return html;
    },
    renderSpecialDeleteGrid: function(p) {
        var arr = [];
        var data = {
            'param': p
        };
        specialdelete.betteam4 = p.Betteam4;
        specialdelete.validAction('list');
        $.ajax({
            type: "POST",
            url: "../Services/SpecialDeleteService.svc/GetEarlyKOBetList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function() {
                try { parent.BlockUI(); } catch (e) { }
                $('#box_list_cont').html('<p align="center">Loading...</p>');
            },
            success: function(rs) {
                if (rs) {
                    var bettypes = rs.Items;

                    if (parseInt(rs.ErrorCode) > 0) {
                        showAlertMessage("ErrMsg", rs.ErrorMessage, 1);
                        specialdelete.validAction('norow');
                        $('#box_list_cont').empty().html('<p align="center">There is no information available</p>');
                    }
                    else {
                        $('#box_match_info').empty().show();
                        $('#row_match').tmpl(rs).appendTo($('#box_match_info'));
                        $('#box_list_cont').empty().show();
                        $('#row_bet_tpl').tmpl({ Bettyes: bettypes }).appendTo($('#box_list_cont'));
                        window.setTimeout(function() {
                            var no_row = true;
                            $.each(bettypes, function(ind, bettype) {
                                var rel = ind;
                                if (bettype.KoItems.length > 0) {
                                    rel = ((ind % 2) != 0) ? ind - 1 : rel;
                                    $('div#betrow_title_' + rel).show();
                                    no_row = false;
                                }
                            });
                            if (no_row) {
                                specialdelete.validAction('norow');
                                $('#box_list_cont').empty().html('<p align="center">There is no information available</p>');
                            } else {
                                specialdelete.validAction('hasrow');
                                // row title
                                $('div.title_label').bind('click', function(e) {
                                    var ids = $(this).attr('rel').toString().split(':');
                                    for (i = 0; i < ids.length; i++) {
                                        $('div#' + ids[i]).slideToggle();
                                    }
                                    $(this).find('span').toggleClass('up');
                                });
                                // row line
                                $('div.betrow.line').bind('mouseover', function(e) {
                                    $(this).addClass('cur');
                                });
                                $('div.betrow.line').bind('mouseout', function(e) {
                                    $(this).removeClass('cur');
                                });
                                $('a.mixparlay').bind('click', function(e) {
                                    specialdelete.showMixparlay($(this).attr('ref').toString());
                                });
                            }
                        }, 500);
                    }
                } else {
                    specialdelete.validAction('norow');
                    $('#box_list_cont').empty().html('<p align="center">There is no information available</p>').show();
                }
            },
            complete: function() {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        });
    },
    showMixparlay: function(refno) {
        var info = refno.split('-'), data = { refno: info[0], transid: info[1], currency: info[2] };
        var opts = { title: "Mixparlay " + info[0] + " - Ticket " + info[1], width: 700, height: 500 };
        $('<div />').attr('id', 'popup_shadow').addClass('popup_shadow').appendTo('body').show();
        $('<div />').attr('id', 'popup_popup').addClass('popup_popup')
                .css({ width: 760 })
                .appendTo('body');
        $('<div />').attr('id', 'popup_setting').addClass('popup_setting').appendTo('#popup_popup');
        $('<div />').attr('id', 'popup_title')
                 .addClass('popup_title')
                 .append('<div class="title">' + opts.title + '</div><div class="close" title="Close">&nbsp;</div>')
                 .appendTo('#popup_popup');
        $('<div />').attr('id', 'popup_loading').addClass('popup_loading')
                 .appendTo('#popup_popup').html('<p align="center">Loading...</p>').show();
        $('<div />').attr('id', 'popup_content').addClass('popup_content')
                 .css({ width: 749 })
                 .appendTo('#popup_popup').hide();
        $('#popup_popup').css("left", (($(window).width() / 2) - (750 / 2)));
        $('#popup_popup').show();

        window.setTimeout(function() {
            $('#popup_popup').draggable({
                handle: '#popup_title'
            });
            $('div.close').bind('click', function(e) {
                specialdelete.closePopup();
            });

        }, 500);

        $.ajax({
            type: "POST",
            url: "../Services/SpecialDeleteService.svc/GetEarlyKOMixparlay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function(resp) {
                $('#popup_loading').hide();
                if (resp) {
                    $('#popup_content').append($('#row_bet_mixparlay').tmpl({ Rows: resp })).slideDown();
                } else {
                    $('#popup_content').html('<p align="center">There is no information available</p>');
                }
            }
        });
    },
    closePopup: function() {
        $('#popup_shadow').fadeOut();
        $('#popup_popup').fadeOut();
        window.setTimeout(function() {
            $('#popup_shadow').remove();
            $('#popup_popup').remove();
        }, 1000);
    },
    validAction: function(act) {
        if (act) {
            switch (act) {
                case 'list':
                    $('div.box_header.list').show().find('span').removeClass('up');
                    $('div#box_list_cont').show();
                    $('div#box_match_info').hide();
                    $('div.box_header.message').hide();
                    $('div#box_list_message').hide();
                    $('div.box_button.list').hide();
                    $('div.box_button.delete').hide();
                    this.resetMessage();
                    break;
                case 'norow':
                    $('div.box_button.list').show();
                    $('div.box_button.delete').hide();
                    break;
                case 'hasrow':
                    $('div.box_header.message').show();
                    $('div#box_list_message').show();
                    $('div.box_button.list').show();
                    $('div.box_button.delete').show();
                    break;
                case 'complete':
                    $('div.bg_table').hide();
                default:
                    break;
            }
        }
        return false;
    },
    genCheckOpt: function(row, bettype) {
        try {
            if (bettype) {
                var OptionType = bettype.OptionType, optional = '';
                if (bettype.IsAllowOptionDelete) {
                    optional = '<input type="checkbox" class="chk' + row.Type + '-' + row.IsDelete + '" id="' + row.TransId + '" ' + ((OptionType != '2') ? 'disabled="disabled"' : '') + ' ' + ((OptionType == '1' && row.IsDelete != '0') ? 'checked="true"' : '') + ' value="' + row.IsDelete + '"/>';
                } else {
                    if (row.IsDelete == '1') {
                        optional = '<input type="hidden" class="chk' + row.Type + '-' + row.IsDelete + '" id="' + row.TransId + '" value="' + row.IsDelete + '"/>';
                    }
                }
                return optional;
            }
        }
        catch (e) { }
        return '';
    },
    genBetTitle: function(index, bettype) {
        var tpl_title = '', title = '';
        title = (bettype.Type.toString().indexOf('MixParlay') != -1) ? bettype.Type.toString().replace('MixParlay', ' - Mix Parlay') : bettype.Type.toString();
        if (index % 2 == 0) {
            tpl_title = '<div class="title_label" style="float:left; width: 850px;cursor: pointer;" rel="bettype_' + (index) + ':bettype_' + (index + 1) + '"><span>&nbsp;</span>' + title + '</div>';
        }
        return tpl_title;
    },
    resetMessage: function() {
        $('textarea').val('');
        $('#username').val('');
        $('#userpwd').val('');
        return false;
    }
};