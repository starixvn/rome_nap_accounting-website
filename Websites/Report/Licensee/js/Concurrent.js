//$(document).ready(function () { renderCombobox(); });

var currentSeletedSite = ''; 
var dateFrom = $('#fdate').val();
var dateTo = $('#tdate').val();
var site = $('#siteList').val();
var param = { siteName: site, fromDate: dateFrom, toDate: dateTo };
var TplPeak = '{{each(i, current) rows}}\
                        <div class="contentEachRow {{if ((i%2) > 0)}}oddrow{{/if}}">\
                            <div class="{{if (i== rows.length -1)}}peakcontentLeftLastRow{{else}}peakcontentLeft{{/if}}">${Date}</div>\
                            <div class="{{if (i== rows.length -1)}}peakcontentRightLastRow{{else}}peakcontentRight{{/if}}">${TotalUser}</div>\
                        </div>{{/each}}';

function renderSite() {
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/HelperService.svc/GetSiteList",
        data: "",
        contentType: "application/json; charset=utf-8",
        success: function (rs) {
            var objSelect = $('#siteList');
            for (var i = 0; i < rs.length; i++) {
                var siteLicensee = rs[i];
                if (siteLicensee.Name == "Licensee") {
                    if (siteLicensee.Sites != null && siteLicensee.Sites.length > 0) {
                        for (var j = 0; j < siteLicensee.Sites.length; j++) {
                            var nameSiteLicensee = siteLicensee.Sites[j];
                            if (nameSiteLicensee.Name != "KORIBC" && nameSiteLicensee.Name != "spotico")
                                objSelect.append("<option value='" + nameSiteLicensee.Name + "'>" + nameSiteLicensee.Name + "</option>");
                        }
                    }
                }
            }

        }
    });
}
function bindingPeak() {
    dateFrom = $('#fdate').val();
    dateTo = $('#tdate').val();
    site = $("#siteList option:selected").text();
    currentSeletedSite = site;
    param = { "siteName": site, "fromDate": dateFrom, "toDate": dateTo };
    $("#peakContent").html("");
    $('#table_report').hide();
    $('#reportMain').hide();
    $('#noData').hide();
    $("#dvMsg").html("");
//    $('peakContent1').hide();
//    $('peakContent').hide();
    if (new Date(dateFrom) > new Date()) {
        $('#trMsg').show();
        showAlertMessage('dvMsg', 'From date is newer the current date', 1);
    }
    else if (new Date(dateTo) > new Date()) {
        $('#trMsg').show();
        showAlertMessage('dvMsg', 'To date is newer the current date', 1);
    }
    else if (new Date(dateFrom) > new Date(dateTo)) {
        $('#trMsg').show();
        showAlertMessage('dvMsg', 'Please choose From date and To date again.', 1);
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LicenseeService.svc/GetConcurrentUserList",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs != null && rs.ListConcurrentUser.length > 0) {

                    var result = [{ "rows": rs.ListConcurrentUser}];

                    $.tmpl(TplPeak, result).appendTo("#peakContent");
                    $('#trMsg').hide();
                    $('#peakTotalRight').html(rs.Total);

                    $('#totalUser').html(rs.AverageUser);
                    $('#peakAverage').show();
                    $('#peakMaxRight').html(rs.MaxUser);
                    $('#peakMinRight').html(rs.MinUser);
                    $('#reportMain').show();
                    $('#table_report').show();
                    $('#btnExportIcon').css('display', 'block');
                    $('#noData').hide();

                } else {

                    $('#noData').show();
                    $('#btnExportIcon').css('display', 'none');
                    $('#table_report').show();

                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            error: function () {
                showAlertMessage('dvMsg', 'There is no infomation.', 1);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
}

function Export2Excel() {
    dateFrom = $('#fdate').val();
    dateTo = $('#tdate').val();
    site = $('#siteList').val();
    var params = 'fdate=' + dateFrom + '&tdate=' + dateTo + '&sitelist=' + site + '&export=eportexcel';
    var src = 'ConcurrentUser.aspx?' + params;
    $('#iframePeark').attr('src', src);
}

function ChangeSite() {
    if ($('#siteList').val() != currentSeletedSite) {
        $('#btnExportIcon').css('display', 'none');
    }
    else {
        $('#btnExportIcon').css('display', 'block');
    }
}