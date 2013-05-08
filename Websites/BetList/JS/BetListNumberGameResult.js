/*--End global variable declaretion--*/
$(document).ready(function() {
    getNumberGameResult();
});

//-----------Load data-----------------//
function getNumberGameResult(isReload) {
    var matchId = gup("matchId");//853602;
    if (matchId!=null)
    {
        var param = '{"matchId":' + matchId + '}';
         $.ajax({
            type: "POST",
            url: "../Services/WCF/BetList/BetListNumberGameResult.svc/GetBetListNumberGame",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                LoadData(rs);
            }
        });
    }
   
}

function LoadData(rs)
{
    if (rs != null) {
        var ResultContent = $("#ResultContent");
        ResultContent.setTemplateURL("../App_Templates/Default/BetList/NumberGame/NumberGameResult.htm");
        ResultContent.processTemplate(rs);
    }
    else {
        $("#err").html('<div class="alertboxerr"><div>There is no information available.</div></div>');
        $(".bg_table").addClass("hidden");
    }
}