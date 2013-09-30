var PTRenderManager = {
    renderNumberGameOptions: function (dropDownNumberGamePT) {
        //NumberGame
        var numberGamePT = 1;
        var ptStep = 0.005;
        var dropDownNumberGamePT = dropDownNumberGamePT ?
            dropDownNumberGamePT :
            $("#selNGPT");
        while (numberGamePT > 0) {
            val = parseFloat(numberGamePT.toFixed(3));
            dropDownNumberGamePT.append("<option value='" + val + "'>" + val + "</option>");
            numberGamePT = numberGamePT - ptStep;
        }

        dropDownNumberGamePT.append("<option selected='selected' value='0'>0</option>");
    }
};