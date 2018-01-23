var vsCommon = {
    init: function () {        
        $("#tblTabVirtualSports").click(function () {
            $("#divVirtualSportsContainer").slideToggle("fast", function () {
                if (!$("#divVirtualSportsContainer").is(":visible")) {
                    $("#tblTabVirtualSports").find("td[arrow]").removeClass("md2").addClass("md3");
                }
                else {
                    $("#tblTabVirtualSports").find("td[arrow]").removeClass("md3").addClass("md2");
                }
            });
        });

        //virutal sports
        $("#divVirtualSportsContainer").find("input[type='text']").ForceDecimalOnly().ForceCurrencyOnly();

        //Sportbook
        var pt = 0.9;
        var val;
        while (pt > 0) {
            val = parseFloat(pt.toFixed(2));
            $("#selVirtualSportsPT").append("<option value='" + val + "'>" + val + "</option>");
            pt = pt - 0.01;
        }
        $("#selVirtualSportsPT").append("<option selected='selected' value='0'>0</option>");
    },
    resetValues: function () {
        $("#divVirtualSportsContainer").find("input[type='text']").val("0");
        $("#divVirtualSportsContainer").find("select").find("option[value='0']").attr("selected", "selected");
    },
    initConstraint: function () {
        $("#divVirtualSportsContainer").find("input[type='text']").each(function () {
            if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined" && this.uplineVal > 0) {
                
                $(this).parent().find("span").html(">=" + parseFloat(this.uplineVal).formatMoneyFromVal());
            }
            else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined" && this.uplineVal > 0) {
                $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
            }
            else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined" && this.uplineVal > 0) {
                $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
            }
        });
    },
    verifyDataContrain: function (valid, job, setMsg1, setMsg2, setMsg3, setMsg4, setMsg5, setMsg6, setMsg7, setMsg8, setMsg9, setMsg10, setMsg11) {

        $("#divVirtualSportsContainer").find("input[type='text']").css("border-color", "").each(function () {
            var currentVal = parseFloat($(this).getNumberValue());

            if (valid) {
                if (common.isEmpty($(this).val()) || !common.isNumber($(this).getNumberValue()) || currentVal < 0 || currentVal > common.moneyMax) {
                    job(this);
                    return false;
                }
                var minBet, maxBet, maxPerMatch;

                if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                    maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());

                    if (isNaN(maxBet)) return false;
                    maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                    if (isNaN(maxPerMatch)) return false;
                    if (currentVal > maxBet) {
                        job(this);
                        setMsg1(maxBet);
                        return false;
                    }
                    if (currentVal > maxPerMatch) {
                        job(this);
                        setMsg11(maxPerMatch);
                        return false;
                    }
                }
                else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                    maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                    if (isNaN(maxPerMatch)) return false;
                    if (currentVal > maxPerMatch) {
                        job(this);
                        setMsg6(maxPerMatch);
                        return false;
                    }
                }

                if (typeof this.uplineVal == "undefined") {
                    return;
                }

                var uplineMinBet, uplineMaxBet, uplineMaxPermatch;
                uplineMinBet = $(this).parents("tr:first").find("input[minbet]").get(0).uplineVal;
                uplineMaxBet = $(this).parents("tr:first").find("input[maxbet]").get(0).uplineVal;
                uplineMaxPermatch = $(this).parents("tr:first").find("input[maxpermatch]").get(0).uplineVal;
                if (uplineMinBet == 0 && uplineMaxBet == 0 && uplineMaxPermatch == 0) return; //valid case

                minBet = parseFloat($(this).parents("tr:first").find("input[minbet]").getNumberValue());
                if (isNaN(minBet)) minBet = 0;
                maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
                if (isNaN(maxBet)) maxBet = 0;
                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) maxPerMatch = 0;
                if (minBet == 0 && maxBet == 0 && maxPerMatch == 0) return; //valid case

                if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                    if (currentVal < this.uplineVal) { job(this); setMsg2(this.uplineVal); return; }
                }
                else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                    if (currentVal > this.uplineVal) { job(this); setMsg3(this.uplineVal); return; }
                }
                else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined") {
                    if (currentVal > this.uplineVal) { job(this); setMsg4(this.uplineVal); return; }
                }
            }
        });
    }
};

