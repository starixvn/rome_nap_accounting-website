//Grid style example: Racing Group Daily
var MasterAgentList = {
    
};
MasterAgentList.assignWidth = function(agent_type) {
    var scrollbarWidth = this.getScrollbarWidth();
    var getWidth = function(isWindowResize) {
        var width = $(window).width();
        if (MasterAgentList.isIE()) {
            width = width - 10;
        }
        else if (MasterAgentList.isFirefox()) {
            if (isWindowResize) {
                if (typeof agent_type != "undefined" && agent_type == "supper") {
                    width = width - 10;
                }
                if (typeof agent_type != "undefined" && agent_type == "master_agent") {
                    width = width - 10;
                }
                if (typeof agent_type != "undefined" && agent_type == "agent") {
                    width = width - 10;
                }
                if (typeof agent_type != "undefined" && agent_type == "member") {
                    width = width - 10;
                }
            }
            else {
                if (typeof agent_type != "undefined" && agent_type == "supper") {
                    width = width + 10;
                }
                if (typeof agent_type != "undefined" && agent_type == "master_agent") {
                    width = width + 10;
                }
                if (typeof agent_type != "undefined" && agent_type == "agent") {
                    width = width - 10;
                }
                if (typeof agent_type != "undefined" && agent_type == "member") {
                    width = width - 10;
                }
            }
        }
        return width;
        /*if (MasterAgentList.isExternal()) {
        }
        else {
            if (typeof agent_type != "undefined" && agent_type == "supper_finance") {
            return width;
            }
            else {
                if (typeof agent_type != "undefined" && agent_type == "master_agent" && MasterAgentList.isIE()) {
                    return width;
                }
                else {
                    if (typeof agent_type != "undefined" && agent_type == "agent") {
                        return width;
                    }
                    else {
                        if (typeof agent_type != "undefined" && agent_type == "supper"  && MasterAgentList.isIE()) {
                            return width;
                        }
                        else {
                            if (typeof agent_type != "undefined" && agent_type == "member") {
                                return width;
                            }
                            else {
                                width = width + scrollbarWidth;
                                return width;
                            }
                        }    
                    }
                }
            }
        }*/
    };
    var resizeDivContent = function(isWindowResize){
        var width = getWidth(isWindowResize);
        $("#strcontent").width(width);
        $("#divtitle").width(width);
        $("#divTable").width(width);
        
        $("#bdiv").width(width - $("#bdiv2").width());
        $("#hdiv2").parent().width($("#bdiv").width());
    };
    resizeDivContent();
    $(window).resize(function(){
        resizeDivContent(true);
    });
};

MasterAgentList.isIE = function () {
    if (navigator.appName == "Microsoft Internet Explorer") {
        return true;
    }
    else {
        return false;
    }
};

MasterAgentList.isFirefox = function () {
    if (navigator.appName == "Microsoft Internet Explorer") {
        return false;
    }
    else {
        return true;
    }
};

MasterAgentList.assignColumnSpan = function(agent_type) {
    if (!this.isExternal()) {
        if (typeof agent_type != "undefined" && agent_type == "member") {
            $("#th1_bdiv").attr("colspan", "4");
        }
    }
};

MasterAgentList.assignHeight = function() {
    var scrollbarWidth = this.getScrollbarWidth();
    if (this.isIE()) {
        $("#bdiv").height(542);
    }
    else {
        $("#bdiv").height(539);
    }
    $("#bdiv2").parent().height($("#bdiv").height() - scrollbarWidth);
};

MasterAgentList.assignPosition = function() {
    if ($("#searchbox").length > 0 && $("#searchbox").get(0).style.display == "none") {
        $("#searchbox").parent().hide();
        $("#strcontent").parent().get(0).style.marginTop = "-2px";
    }
    else if ($("#searchbox").length <= 0) {
        $("#strcontent").parent().get(0).style.marginTop = "-2px";
    }
};

MasterAgentList.assignRowCss = function() {
    //Assign row css
    var isEven = false;
    var arr_tr_left = $("#bdiv1").find("tr");
    var arr_tr_right = $("#btable").find("tr");
    
    for (var i = 0; i < arr_tr_left.length; ++i) {
        arr_tr_left.get(i).style.height = arr_tr_right.get(i).style.height = "20px";
        if ($(arr_tr_left.get(i)).attr("status") == "Disabled") {
            arr_tr_left.get(i).style.backgroundColor = arr_tr_right.get(i).style.backgroundColor = "#A0A97A";
            isEven = !isEven;
            continue;
        }
        if ($(arr_tr_left.get(i)).attr("status") == "Closed") {
            arr_tr_left.get(i).style.backgroundColor = arr_tr_right.get(i).style.backgroundColor = "#CCC7BA";
            isEven = !isEven;
            continue;
        }
        if ($(arr_tr_left.get(i)).attr("status") == "Suspended") {
            arr_tr_left.get(i).style.backgroundColor = arr_tr_right.get(i).style.backgroundColor = "#F8DED7";
            isEven = !isEven;
            continue;
        }
        if (!isEven) {
            arr_tr_left.get(i).style.backgroundColor = arr_tr_right.get(i).style.backgroundColor = "#E6E9F0";
        }
        else {
            arr_tr_left.get(i).style.backgroundColor = arr_tr_right.get(i).style.backgroundColor = "white";
        }
        isEven = !isEven;
    }
};

MasterAgentList.assignOtherCss = function() {
    $("#strcontent").get(0).style.border = "1px solid #7C98A4";
    $("#strcontent").get(0).style.marginTop = "2px";
    $("#strcontent").get(0).style.backgroundColor = "#E6E9F0";
    $("#strcontent").get(0).style.overflow = "hidden";
    
    $("#divTable").get(0).style.borderBottom = "1px solid #7C98A4";
    $("#divtitle").get(0).style.borderBottom = "1px solid #7C98A4";
    
    if ($("#btable").find("tr").length < 24 ) {
        if ($("#td_body_left").length > 0)
            $("#td_body_left").get(0).style.backgroundColor = "#E6E9F0";
    }
};

MasterAgentList.assignTdCss = function() {
    var arr_tr_left = $("#bdiv1").find("tr");
    var arr_tr_right = $("#btable").find("tr");
    
    var borderBottom = "1px solid #7C98A4";
    var borderRight = "1px solid #7C98A4";
    
    for (var i = 0; i < arr_tr_left.length; ++i) {
        var arr_td_left = $(arr_tr_left.get(i)).find("td");
        for (var i1 = 0; i1 < arr_td_left.length; ++i1) {
            arr_td_left.get(i1).style.borderBottom = borderBottom;
            arr_td_left.get(i1).style.borderRight = borderRight;
        }
        
        var arr_td_right = $(arr_tr_right.get(i)).find("td");
        for (var i2 = 0; i2 < arr_td_right.length; ++i2) {
            arr_td_right.get(i2).style.borderBottom = borderBottom;
            arr_td_right.get(i2).style.borderRight = borderRight;
        }
    }
};

MasterAgentList.assignHeaderCss = function() {
    var borderRight = "1px solid #7C98A4";
    var height = "20px";
    
    var arr_th_left = $("#hdiv1").find("table thead tr th");
    for (var i1 = 0; i1 < arr_th_left.length; ++i1) {
        arr_th_left.get(i1).style.borderRight = borderRight;
        arr_th_left.get(i1).style.height = height;
    }
    
    var arr_th_right = $("#hdiv2").find("table thead tr th");
    for (var i2 = 0; i2 < arr_th_right.length; ++i2) {
        arr_th_right.get(i2).style.borderRight = borderRight;
        arr_th_right.get(i2).style.height = height;
    }
    $("#divtitle").get(0).style.backgroundColor = "#B5C4D0";
};

MasterAgentList.assignCSS = function() {
    this.assignHeaderCss();
    this.assignRowCss();
    this.assignTdCss();
    this.assignOtherCss();
};

MasterAgentList.init = function (agent_type) {
    this.assignCSS();
    this.assignWidth(agent_type);
    this.assignHeight();
    this.assignPosition();
    this.assignColumnSpan(agent_type);
    this.attachScrollEvent();
};

MasterAgentList.isExternal = function() {
    if ($("#th1_bdiv").attr("colspan") == "3"){
        return true;
    }
    else {
        return false;
    }
};

MasterAgentList.attachScrollEvent = function(){
    var scrollTop = 0;
    var scrollLeft = 0;
    $("#bdiv").scroll(function(){
        var isHorizontalScrolling = true;
        if ($(this).scrollTop() != scrollTop){
            isHorizontalScrolling = false;
        }
        
        var current_scrollTop = $(this).scrollTop();
        var current_scrollLeft = $(this).scrollLeft();
        
        scrollTop = current_scrollTop;
        scrollLeft = current_scrollLeft;
        
        if (isHorizontalScrolling){
            $("#hdiv2").get(0).style.marginLeft = (-1 * current_scrollLeft) + "px";
        }
        else{
            $("#bdiv2").get(0).style.marginTop = (-1 * current_scrollTop) + "px";
        }
    });
};

MasterAgentList.getScrollbarWidth = function () {
    var inner = document.createElement('p');
	inner.style.width = "100%";
	inner.style.height = "200px";

	var outer = document.createElement('div');
	outer.style.position = "absolute";
	outer.style.top = "0px";
	outer.style.left = "0px";
	outer.style.visibility = "hidden";
	outer.style.width = "200px";
	outer.style.height = "150px";
	outer.style.overflow = "hidden";
	outer.appendChild (inner);

	document.body.appendChild (outer);
	var w1 = inner.offsetWidth;
	outer.style.overflow = 'scroll';
	var w2 = inner.offsetWidth;
	if (w1 == w2) w2 = outer.clientWidth;

	document.body.removeChild (outer);

	return (w1 - w2);
};