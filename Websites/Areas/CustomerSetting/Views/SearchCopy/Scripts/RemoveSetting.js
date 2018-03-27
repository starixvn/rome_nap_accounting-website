var PTSetting = PTSetting || {};
PTSetting.SportType = new function () {

    var delayTime = 2000;

    var RemoveButtons = function (frameId) {
        var $content = $(frameId).contents();
        setTimeout(function () {
            $content.find('.maincontent_header').remove();
            $content.find('table:has(td.custBtn1)').remove();
            $content.find("#divShowInfo").remove();
        }, delayTime);
    };

    var RemoveSportBookCustInfo = function () {
        var $content = $("#frmSportbook").contents();

        setTimeout(function () {
            for (var i = 0; i < 6; i++) {
                $content.find(".tblcollapse tr:eq(0)").remove();
            }
            //disable edit
            $content.find('.BettingLimitHeaderCopied').removeAttr("onclick");
            $content.find('input:text').attr('disabled', true);
            $content.find('.selectcomm').attr('disabled', true);
            $content.find('.PTSelect').attr('disabled', true);
            $content.find('input:button').attr('disabled', true);
            $content.find('tr[id=suspended-row]').css('display', 'none');
        }, 1.5 * delayTime);
    };

    var RemoveSportBookMinPT = function () {
        var $content = $("#frmSportbookMinPT").contents();

        setTimeout(function () {
            for (var i = 0; i < 3; i++) {
                $content.find('.tblcollapse tr:eq(0)').remove();
            }
            //disable edit
            $content.find('.PTSelect').attr('disabled', true);
            $content.find('input:button').attr('disabled', true);
        }, delayTime);

    };

    var RemoveCasinoCustInfo = function () {
        var $content = $("#frmCasino").contents();

        setTimeout(function () {
            for (var i = 0; i < 2; i++) {
                $content.find('.tblcollapse tr:eq(0)').remove();
            }
            $content.find('.maincontent_box').remove();
            //disable edit
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('input:text').attr('disabled', true);
            $content.find('select').attr('disabled', true);
            $content.find('a').css('display', 'none');
            $content.find('input:button').attr('disabled', true);
        }, delayTime);
    };

    var RemoveVirtualSportsCustInfo = function () {
        var $content = $("#frmVirtualSports").contents();

        setTimeout(function () {
            for (var i = 0; i < 5; i++) {
                $content.find('.tblcollapse tr:eq(0)').remove();
            }
            //disable edit
            $content.find('input:text').attr('disabled', true);
            $content.find('.PTSelect').attr('disabled', true);
            $content.find('.btnUpdate').remove();
        }, delayTime);
    };

    var RemoveBingoCustInfo = function () {
        var $content = $("#frmBingo").contents();

        setTimeout(function () {
            for (var i = 0; i < 5; i++) {
                $content.find('.tblcollapse tr:eq(0)').remove();
            }
            $content.find('.maincontent_box').remove();
            $content.find('#tblHideUpdate').remove();
            //disable edit
            $content.find('input:checkbox').attr('disabled', true);
            $content.find('input:text').attr('disabled', true);
            $content.find('select').attr('disabled', true);
            $content.find('.canchange').css('display', 'none');
            $content.find('input:button').attr('disabled', true);
        }, delayTime);
    };

    this.SetFrameLoaded = function () {
        var sport = $('#listSports option:selected').text();

        //remove settings
        switch (sport) {
            case 'Sportsbook': RemoveSportBookCustInfo(); RemoveButtons('#frmSportbook'); break;
            case 'Sportsbook Min PT': RemoveSportBookMinPT(); RemoveButtons('#frmSportbookMinPT'); break;
            case 'Casino': RemoveCasinoCustInfo(); RemoveButtons('#frmCasino'); break;
            case 'Virtual Sports': RemoveVirtualSportsCustInfo(); RemoveButtons('#frmVirtualSports'); break;
            case 'Bingo': RemoveBingoCustInfo(); RemoveButtons('#frmBingo'); break;
        }
    }
};