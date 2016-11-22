var form;
$(document).ready(function () {
    ajax_form($('#mainForm'), function (result) {
        RenderData(result);
    });

    form = $('#mainForm');
});

function GetData() {
    if (!ValidateUploadFile()) {
        return;
    }

    $('#PageSize').val(50);
    $('#PageIndex').val(1);
    try { parent.BlockUI(); } catch (e) { }
    form.attr('action', 'GetData');
    form.submit();
}

function GetExcelFile() {
    if (!ValidateUploadFile()) {
        return;
    }

    $('#PageSize').val(50);
    $('#PageIndex').val(1);
    try { parent.BlockUI(); } catch (e) { }
    form.attr('action', 'ExportExcel');
    form.submit();
    try { parent.jQuery.unblockUI(); } catch (e) { }
}

function RenderData(data) {
    if (data.ErrorMessage != null) {
        showAlertMessage('errMsg', data.ErrorMessage, 1);
        try { parent.jQuery.unblockUI(); } catch (e) { }
        return;
    }

    var bodyRows = [];
    var rowCount = data.TicketToHeadlineCollection.length;
    var mergeRowNumGroupHeadline = 0;
    var mergeRowNumHeadlineType = 0;
    var mergeRowNumCurrency = 0;
    var mergeRowNumSuper = 0;
    var mergeRowNumMaster = 0;
    var mergeRowNumAgent = 0;
    var cacheGroupHeadline = '';
    var currentBGColor = '';
    for (i = 0 ; i < rowCount; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'RowNum', Value: data.TicketToHeadlineCollection[i].RowNum });

        if (data.TicketToHeadlineCollection[i].ErrorCode == 0) {
            if (mergeRowNumGroupHeadline > 0) {

            } else {
                row.Columns.push({ Name: 'GroupHeadline', Value: data.TicketToHeadlineCollection[i].GroupHeadline, MergeRowNumber: data.TicketToHeadlineCollection[i].MergeRowNumGroupHeadline - 1 });
                row.Columns.push({ Name: 'HeadlineType', Value: data.TicketToHeadlineCollection[i].HeadlineType, MergeRowNumber: data.TicketToHeadlineCollection[i].MergeRowNumHeadlineType - 1 });
            }

            if (mergeRowNumSuper > 0) {

            } else {
                row.Columns.push({ Name: 'Currency', Value: data.TicketToHeadlineCollection[i].Currency, MergeRowNumber: data.TicketToHeadlineCollection[i].MergeRowNumCurrency - 1 });
                row.Columns.push({ Name: 'Super', Value: data.TicketToHeadlineCollection[i].Super, MergeRowNumber: data.TicketToHeadlineCollection[i].MergeRowNumSuper - 1 });
            }

            if (mergeRowNumMaster > 0) {

            } else {
                row.Columns.push({ Name: 'Master', Value: data.TicketToHeadlineCollection[i].Master, MergeRowNumber: data.TicketToHeadlineCollection[i].MergeRowNumMaster - 1 });
            }

            if (mergeRowNumAgent > 0) {

            } else {
                row.Columns.push({ Name: 'Agent', Value: data.TicketToHeadlineCollection[i].Agent, MergeRowNumber: data.TicketToHeadlineCollection[i].MergeRowNumAgent - 1 });
            }

            mergeRowNumGroupHeadline = data.TicketToHeadlineCollection[i].MergeRowNumGroupHeadline != 0 ? data.TicketToHeadlineCollection[i].MergeRowNumGroupHeadline - 1 : mergeRowNumGroupHeadline;
            mergeRowNumSuper = data.TicketToHeadlineCollection[i].MergeRowNumSuper != 0 ? data.TicketToHeadlineCollection[i].MergeRowNumSuper - 1 : mergeRowNumSuper;
            mergeRowNumMaster = data.TicketToHeadlineCollection[i].MergeRowNumMaster != 0 ? data.TicketToHeadlineCollection[i].MergeRowNumMaster - 1 : mergeRowNumMaster;
            mergeRowNumAgent = data.TicketToHeadlineCollection[i].MergeRowNumAgent != 0 ? data.TicketToHeadlineCollection[i].MergeRowNumAgent - 1 : mergeRowNumAgent;

            mergeRowNumGroupHeadline--;
            mergeRowNumSuper--;
            mergeRowNumMaster--;
            mergeRowNumAgent--;
            currentBGColor = cacheGroupHeadline != data.TicketToHeadlineCollection[i].GroupHeadline ? toggleColor(currentBGColor) : currentBGColor;
            cacheGroupHeadline = data.TicketToHeadlineCollection[i].GroupHeadline;
        }
        else {
            row.Columns.push({ Name: 'GroupHeadline', Value: '<span class="no-data-not-frozen">' + data.TicketToHeadlineCollection[i].ErrorMessage + '</span>', MergeColNumber: 6 });
            currentBGColor = toggleColor(currentBGColor);
        }

        row.Columns.push({ Name: 'Member', Value: data.TicketToHeadlineCollection[i].Member });
        row.Columns.push({ Name: 'RefNo', Value: data.TicketToHeadlineCollection[i].RefNo });
        row.CssClass = currentBGColor;
        bodyRows.push(row);
    }


    var gridOptions =
       {
           columns: [
               { HeaderText: "#", Width: 40, Name: "RowNum", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "Group Headline", Width: 150, Name: "GroupHeadline", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "Headline Type", Width: 100, Name: "HeadlineType", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "Currency", Width: 80, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
               { HeaderText: "Super", Width: 200, Name: "Super", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
               { HeaderText: "Master", Width: 200, Name: "Master", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
               { HeaderText: "Agent", Width: 200, Name: "Agent", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
               { HeaderText: "Member", Width: 200, Name: "Member", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
               { HeaderText: "Ref No", Width: 150, Name: "RefNo", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true }
           ],
           bodyRows: bodyRows,
           gridExpandHeight: 200,
           showTitle: true,
           titleInfo: {
               text: 'List of tickets with their headlines retrieved'
           },
           showPager: true,
           customPager: true,
           showDistinctRow: false,
           pagerOption: {
               itemsPerPage: parseInt($('#PageSize').val()),
               currentPage: parseInt($('#PageIndex').val()) - 1,
               itemsPerPageArray: [50, 100, 150, 200],
               pagerSelectCallBack: reRenderAfterPaged,
               totalItem: data.TotalRows
           }
       };

    $('#gridId').fanexGrid(gridOptions);
    try { parent.jQuery.unblockUI(); } catch (e) { }
}

function toggleColor(colorCss) {
    if (colorCss == '') {
        return 'bgeven';
    }

    return '';
}

function reRenderAfterPaged(currentPageIndex, itemsPerPage) {
    $('#PageSize').val(itemsPerPage);
    $('#PageIndex').val(currentPageIndex + 1);
    try { parent.BlockUI(); } catch (e) { }
    form.attr('action', 'GetData');
    form.submit();
}

function ValidateUploadFile() {
    var filename = $('#uploadFile').val();
    if (filename == '' || filename == undefined) {
        showAlertMessage('errMsg', 'Please select file to upload.', 1);
        return false;
    }

    var extfilename = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
    if (extfilename != 'xls' && extfilename != 'xlsx') {
        showAlertMessage('errMsg', 'Sorry, You choose invalid file extension. Please choose an Excel file (.xls or .xlsx)', 1);
        return false;
    }

    if (GetFileSize('#uploadFile') > 1024000) {
        showAlertMessage('errMsg', "Your uploaded file's size can't be larger than 1 MB", 1);
        return false;
    }

    HideErrMsg();
    return true;
}

function GetFileSize(eName) {
    if ($.browser.msie) {
        return 1;//IE is not support File API, so no way for get file size.
    }
    else {
        return parseFloat($(eName).get(0).files[0].size);
    }
}

function HideErrMsg() {
    $('#errMsg').html('');
}



function ajax_form($form, on_complete) {
    var iframe;

    if (!$form.attr('target')) {
        //create a unique iframe for the form
        iframe = $("<iframe></iframe>").attr('name', 'ajax_form_' + Math.floor(Math.random() * 999999)).hide().appendTo($('body'));
        $form.attr('target', iframe.attr('name'));
    }

    if (on_complete) {
        iframe = iframe || $('iframe[name=" ' + $form.attr('target') + ' "]');
        iframe.load(function () {
            //get the server response
            var response = $.parseJSON(iframe.contents().find('body').text());
            on_complete(response);
        });
    }
}