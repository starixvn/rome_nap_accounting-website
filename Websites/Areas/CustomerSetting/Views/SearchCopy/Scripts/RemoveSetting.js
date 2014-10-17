var PTSetting = PTSetting || {};
PTSetting.SportType = new function () {

    var RemoveButtons = function (frameId) {
        var $content = $(frameId).contents();
        $content.find('.maincontent_header').css('display', 'none');
        $content.find('table:has(td.custBtn1)').css('display', 'none');
        $content.find("#divShowInfo").css('display', 'none');
    };

    var RemoveSportBookCustInfo = function () {
        var $content = $("#frmSportbook").contents();

        for (var i = 0; i < 6; i++) {
            $content.find(".tblcollapse tr:eq(0)").css('display', 'none');
        }
        //disable edit
        $content.find('input:text').attr('disabled', true);
        $content.find('.selectcomm').attr('disabled', true);
        $content.find('.PTSelect').attr('disabled', true);
        $content.find('input:button').attr('disabled', true);
    };

    var RemoveSportBookMinPT = function () {
        var $content = $("#frmSportbookMinPT").contents();

        for (var i = 0; i < 3; i++) {
            $content.find('.tblcollapse tr:eq(0)').css('display', 'none');
        }
        //disable edit
        $content.find('.PTSelect').attr('disabled', true);
        $content.find('input:button').attr('disabled', true);
    };

    var RemoveRacingCustInfo = function () {
        var $content = $("#frmRacing").contents();

        $content.find('#divCustInfo').css('display', 'none');
        //disable edit
        $content.find('input:checkbox').attr('disabled', true);
        $content.find('input:text').attr('disabled', true);
        $content.find('select').attr('disabled', true);
    };

    var RemoveRacingMinPT = function () {
        var $content = $("#frmRacingMinPT").contents();

        for (var i = 0; i < 4; i++) {
            $content.find('.tblcollapse tr:eq(0)').css('display', 'none');
        }
        $content.find('.maincontent_box').css('display', 'none');
        $content = $content.find('#SettingContent');
        //disable edit
        $content.find('input:checkbox').attr('disabled', true);
        $content.find('select').attr('disabled', true);
    };

    var RemoveCasinoCustInfo = function () {
        var $content = $("#frmCasino").contents();

        for (var i = 0; i < 2; i++) {
            $content.find('.tblcollapse tr:eq(0)').css('display', 'none');
        }
        $content.find('.maincontent_box').css('display', 'none');
        //disable edit
        $content.find('input:checkbox').attr('disabled', true);
        $content.find('input:text').attr('disabled', true);
        $content.find('select').attr('disabled', true);
        $content.find('a').css('display', 'none');
        $content.find('input:button').attr('disabled', true);
    };

    var RemoveVirtualSportsCustInfo = function () {
        var $content = $("#frmVirtualSports").contents();

        for (var i = 0; i < 5; i++) {
            $content.find('.tblcollapse tr:eq(0)').css('display', 'none'); ;
        }
        //disable edit
        $content.find('input:text').attr('disabled', true);
        $content.find('.PTSelect').attr('disabled', true);
        $content.find('.btnUpdate').css('display', 'none');
    };

    var RemoveBingoCustInfo = function () {
        var $content = $("#frmBingo").contents();

        for (var i = 0; i < 5; i++) {
            $content.find('.tblcollapse tr:eq(0)').css('display', 'none');
        }
        $content.find('.maincontent_box').css('display', 'none');
        $content.find('#tblHideUpdate').css('display', 'none');
        //disable edit
        $content.find('input:checkbox').attr('disabled', true);
        $content.find('input:text').attr('disabled', true);
        $content.find('select').attr('disabled', true);
        $content.find('.canchange').css('display', 'none');
        $content.find('input:button').attr('disabled', true);
    };

    var RemoveLiveCasinoCustInfo = function () {
        var $content = $("#frmLiveCasino").contents();

        for (var i = 0; i < 5; i++) {
            $content.find('.tblcollapse tr:eq(0)').css('display', 'none');
        }
        $content.find('.maincontent_box').css('display', 'none');
        $content.find('.btnUpdate').css('display', 'none');
        //disable edit
        $content.find('input:checkbox').attr('disabled', true);
        $content.find('select').attr('disabled', true);
        $content.find('.btnUpdateSetting').css('display', 'none');
    };

    this.SetFrameLoaded = function () {
        var sport = $('#listSports option:selected').text();

        //remove settings
        switch (sport) {
            case 'Sportsbook': RemoveSportBookCustInfo(); RemoveButtons('#frmSportbook'); break;
            case 'Sportsbook Min PT': RemoveSportBookMinPT(); RemoveButtons('#frmSportbookMinPT'); break;
            case 'Racing': RemoveRacingCustInfo(); RemoveButtons('#frmRacing'); break;
            case 'Racing Min PT': RemoveRacingMinPT(); RemoveButtons('#frmRacingMinPT'); break;
            case 'Casino': RemoveCasinoCustInfo(); RemoveButtons('#frmCasino'); break;
            case 'Virtual Sports': RemoveVirtualSportsCustInfo(); RemoveButtons('#frmVirtualSports'); break;
            case 'Bingo': RemoveBingoCustInfo(); RemoveButtons('#frmBingo'); break;
            case 'Live Casino': RemoveLiveCasinoCustInfo(); RemoveButtons('#frmLiveCasino'); break;
        }
    }
};