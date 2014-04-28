var PositionTakingRenderManager = function () {
    var self = this;
    self.renderSportbookPTOptions = function (dropDownSportbookPT) {
        var dropDownSportbookPT = dropDownSportbookPT ?
            dropDownSportbookPT :
            $("#dropdown-sportsbook-positiontaking");

        var pt = 0.85;
        var val;
        while (pt > 0) {
            val = parseFloat(pt.toFixed(2));
            dropDownSportbookPT.append("<option value='" + val + "'>" + val + "</option>");
            pt = pt - 0.01;
        }

        dropDownSportbookPT.append("<option selected='selected' value='0'>0</option>");
    };

    self.renderNumberGamePTOptions = function (dropDownNumberGamePT) {
        var numberGamePT = 1;
        var ptStep = 0.005;
        var dropDownNumberGamePT = dropDownNumberGamePT ?
            dropDownNumberGamePT :
            $("#dropdown-numbergame-positiontaking");
        while (numberGamePT > 0) {
            val = parseFloat(numberGamePT.toFixed(3));
            dropDownNumberGamePT.append("<option value='" + val + "'>" + val + "</option>");
            numberGamePT = numberGamePT - ptStep;
        }

        dropDownNumberGamePT.append("<option selected='selected' value='0'>0</option>");
    }
};