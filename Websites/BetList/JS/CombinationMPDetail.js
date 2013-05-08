$(document).ready(function(){
    var CombinationMPDetail = {
        ui: {}
        , utils: {}
    };

    CombinationMPDetail.init = function() {
        this.ui.init();
    };

    CombinationMPDetail.ui.init = function() {
        $("#divContainer").find("div[barcol='true']").width(CombinationMPDetail.utils.getScrollbarWidth() + ($.browser.msie ? 0 : 1));
        
        //------------------------------
        $("#divBody").width($("#divHeader").find("table").width());
        $("#divContainer").width($("#divBody").width() + ($.browser.msie ? 0 : 1));
        $("#divFooter").width($("#divBody").width() - ($.browser.msie ? 1 : 0));
        //------------------------------
        $("#tbodyData").find("tr").each(function(index, element){
            if (index % 2 != 0) {
                element.style.backgroundColor = "#DFEAFF";
            }
        });
        $("#tbodyData").find("tr:last td").each(function(index, element){
            $(element).get(0).style.borderBottom = "0px none";
        });
    };

    CombinationMPDetail.utils.getScrollbarWidth = function () {
        var scrollbarWidth;
        var div = $('<div style="width:50px;height:50px;overflow:hidden;position:absolute;top:-200px;left:-200px;"><div style="height:100px;"></div></div>');
        $('body').append(div);
        var w1 = $('div', div).innerWidth();
        div.css('overflow-y', 'auto');
        var w2 = $('div', div).innerWidth();
        $(div).remove();
        scrollbarWidth = (w1 - w2);
        return scrollbarWidth;
    };
    
    CombinationMPDetail.init();
});
