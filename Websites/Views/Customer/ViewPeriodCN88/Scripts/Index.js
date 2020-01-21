var viewPeriodCN88 = {
    DOMElements: {},
    Init: function () {
        viewPeriodCN88.DOMElements = {
            txtHeadlineUsername: $('#headlineUsername'),
            btnSearch: $('#btnSearch'),
            gridContainer: $('#gridContainer'),
            mainGrid: $('#main-grid'),
            sectionMessage: $('#sectionMessage')
        }
        viewPeriodCN88.DOMElements.btnSearch.click(viewPeriodCN88.Search);
    },
    Search: function () {
        parent.BlockUI();
        $.get('Search', { headlineUserName: viewPeriodCN88.DOMElements.txtHeadlineUsername.val() }, function (data) {
            var bodyRows = [];
            for (var i = 0; i < data.length; i++) {
                var currentRow = data[i];
                bodyRows.push({
                    Columns: [
                        { Name: 'No', Value: i },
                        { Name: 'HeadlineUsername', Value: currentRow.HeadlineUsername },
                        { Name: 'FirstName', Value: currentRow.FirstName },
                        { Name: 'LastName', Value: currentRow.LastName },
                        { Name: 'Currency', Value: currentRow.Currency },
                        { Name: 'IsViewAllPeriods', Value: '<span class="' + (currentRow.IsViewAllPeriods ? 'chkchecked' : 'chkuncheck') + '" onclick="viewPeriodCN88.chkIsViewAllPeriods_OnClick(this)" data-headline-id="' + currentRow.HeadlineId + '" ></span>' }
                    ]
                });
            }
            var gridOption = {
                columns: [
                    { HeaderText: '#', Width: 40, Name: 'No', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Headline Username', Width: 100, Name: 'HeadlineUsername', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'First Name', Width: 100, Name: 'FirstName', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Last Name', Width: 100, Name: 'LastName', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'Currency', Width: 50, Name: 'Currency', HeaderAlign: 'center', CellAlign: 'center' },
                    { HeaderText: 'View all Periods', Width: 100, Name: 'IsViewAllPeriods', HeaderAlign: 'center', CellAlign: 'center' }
                ],
                bodyRows: bodyRows,
                gridExpandHeight: 120
            };

            viewPeriodCN88.DOMElements.gridContainer.removeClass('hide').show();
            viewPeriodCN88.DOMElements.mainGrid.fanexGrid(gridOption);
        }).always(function () { parent.jQuery.unblockUI() });
    },
    UpdatePeriod: function (headlineId, isViewAllPeriods) {
        parent.BlockUI();
        $.post('UpdatePeriod', { headlineId: headlineId, isViewAllPeriods: isViewAllPeriods }, function (result) {
            if (result.ErrorCode == 0) {
                viewPeriodCN88.ShowSuccessMessage('Updated Successfully.');
            }
            else {
                viewPeriodCN88.ShowErrorMessage(result.ErrorMessage);
            }

            setTimeout(function () {
                viewPeriodCN88.DOMElements.sectionMessage.hide();
                viewPeriodCN88.Search();
            }, 3000);
        }).always(function () { parent.jQuery.unblockUI() });
    },
    chkIsViewAllPeriods_OnClick: function (element) {
        var $this = $(element);
        if (confirm($this.hasClass('chkchecked') ? resources.ConfirmUncheckViewAllPeriods : resources.ConfirmCheckViewAllPeriods)) {
            $this.toggleClass('chkchecked chkuncheck');
            viewPeriodCN88.UpdatePeriod($this.attr('data-headline-id'), $this.hasClass('chkchecked'));
        }
    },
    ShowErrorMessage: function (msg) {
        viewPeriodCN88.DOMElements.sectionMessage.empty().html('<div class="alertboxerr"><div>' + msg + '</div></div>');
        viewPeriodCN88.DOMElements.sectionMessage.removeClass('hide').show();
    },
    ShowSuccessMessage: function (msg) {
        viewPeriodCN88.DOMElements.sectionMessage.empty().html('<div class="alertboxscc"><div>' + msg + '</div></div>');
        viewPeriodCN88.DOMElements.sectionMessage.removeClass('hide').show();
    },
}

$(document).ready(function () {
    viewPeriodCN88.Init();
});