$.handleError = function( s, xhr, status, e ) {
	// If a local callback was specified, fire it
	if ( s.error ) s.error( xhr, status, e );

	// Fire the global callback
	if ( s.global )
		jQuery.event.trigger( "ajaxError", [xhr, s, e] );
};

var autoTran = {
};

autoTran.init = function() {
    //left_tab1_1
    $("#left_tab1_1").click(function() {
        $(this).attr("class", "current");
        $("#left_tab1_2").removeAttr("class");
        
        $("#content_tab1").show();
        if ($("#divcontainer").get(0).isShowing) {
            $("#divcontainer").show();
        }
        $("#content_tab2").hide();
        $("#divCrossCurTran").hide();
                
        autoTran.hideError();
    });
    
    //left_tab1_2
    $("#left_tab1_2").click(function() {
        $(this).attr("class", "current");
        $("#left_tab1_1").removeAttr("class");
        
        $("#content_tab2").show();
        $("#content_tab1").hide();
        $("#divcontainer").hide();
        
        if ($("#divCrossCurTran").get(0).isShowing) {
            $("#divCrossCurTran").show();
        }
        autoTran.hideError();
    });
};

autoTran.showError = function(msg) {
    $('#msgError').show();
    showAlertMessage("msgError", msg, 1);
};

autoTran.hideError = function() {
    $('#msgError').hide().html("");
};

autoTran.showHelpMultiCrossTran = function (functionName) {
    var url = "../UserGuide/View.aspx?functionName=" + functionName;
    ShowPopupSettle(url, "Multiple Cross-Currency Finance Transfer", 680, 360, 50, 200);
};

autoTran.isValidFileExtension = function() {
    $("#divCrossCurTran").hide();
    $("#divCrossCurTran").get(0).isShowing = false;
    this.hideError();
    var filename = $("#fileCrossCur").val();
    if (filename != "") {
        var ext = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
        if (ext != "xls") {
            this.showError("Sorry, You choose invalid file extension. Please choose an Excel file (.xls)");
            $("#fileCrossCur").val("");
            $("#btnUploadFile").attr("disabled", "disabled");
            return false;
        }
        else if (this.getFileSize("#fileCrossCur") > 1024000) {
            this.showError("Your uploaded file's size can't be larger than 1 MB");
            $("#fileCrossCur").val("");
            $("#btnUploadFile").attr("disabled", "disabled");
            return false;
        }
        else {
            this.hideError();
            $("#btnUploadFile").removeAttr("disabled");
            return true;
        }
    }
    return false;
};

autoTran.getFileSize = function (eName) {
    if ($.browser.msie) {
        return 1;//IE is not support File API, so now way for get file size.
    }
    else {
	    return parseFloat($(eName).get(0).files[0].size);
	}
};

autoTran.UploadFile = function() {
    this.hideError();
    if (this.isValidFileExtension()) {
        BlockUI();
        $("#btnUploadFile").attr("disabled", "disabled");
        $.ajaxFileUpload({
            url: 'UploadCrossCurrency.ashx',
            secureuri: false,
            fileElementId: 'fileCrossCur',
            dataType: 'html',
            success: function(data, status) {
                $.unblockUI();
                if (typeof data == "undefined" || data == null) {
                    return;
                }
                
                //--------------------------------------
                var arr = data.split('|');
                var msg = arr[0];
                var html = arr[1];
                
                if (msg != "") {
                    autoTran.showError(msg);
                    return;
                }
                
                var listForexAccName = arr[2].split(',');
                
                $("#selectForexAccount").empty();
                for(var i = 0; i < listForexAccName.length; ++i) {
                    $("#selectForexAccount").append("<option " +
                        (i == 0 ? "selected='selected'" : "")
                        + " value='" + listForexAccName[i] + "'>" + listForexAccName[i] + "</option>");
                }
                
                //---------------------------------------
                $("#divHTML").html(html);
                if ($("#upHidden").attr('haveerror') == 'false') {
                    $("#divErrorPart").hide();
                    $("#divErrorMsgPart").hide();
                }
                
                if ($("#upHidden").attr('havesuccess') == 'false') {
                    $("#divSuccessPart").hide();
                    $("#divSuccessMsgPart").hide();
                    $("#uploadCrossCur").attr("disabled", "disabled");
                    $("#uploadCrossCur").removeAttr("onclick");
                }
                else {
                    $("#uploadCrossCur").removeAttr("disabled");
                }
                
                if ($("#upHidden").attr('haveerror') == 'false' && $("#upHidden").attr('havesuccess') == 'false') {
                    $("#divCrossCurTran").hide();
                    $("#divCrossCurTran").get(0).isShowing = false;
                }
                else {
                    $("#divCrossCurTran").show();
                    $("#divUploadCCT").show();
                    $("#divCrossCurTran").get(0).isShowing = true;
                }
            },
            error: function(data, status, e) {
                autoTran.showError(e.toString());
                $.unblockUI();
            }
        }); 
    }
};

$(window).unload(function(){
    autoTran.submit(true);
});

autoTran.submit = function(delTemp) {
    if ($("#upHidden").attr('havesuccess') == 'false') return;
    this.hideError();
    
    if ($("#hdFCTFileInfo").length <= 0) return;
    
    if (!delTemp) {
        var allowUpd = confirm('Are you sure you want to update Finance Cross-Currency Transfer Transactions for these customers?');
        if (!allowUpd) return;
    }
    var fileName = $("#hdFCTFileInfo").attr("filename");
    var fileSize = $("#hdFCTFileInfo").attr("filesize");
    BlockUI();
    $.ajax({
            url: 'UpdateCrossCurrencyTransfer.ashx?delTemp=' + delTemp + '&filename=' + fileName + '&filesize=' + fileSize + '&forexname=' + $("#selectForexAccount").val(),
            dataType: 'html',
            cache: false,
            success: function(data) {
                $.unblockUI();
                if (typeof data == "undefined") return;
                
                var arr = data.split('|');
                var msg = arr[0];
                var html = arr[1];
                
                if (msg != "") {
                    autoTran.showError(msg);
                    return;
                }
                $("#divHTML").html(html);
                $("#divUploadCCT").hide();
            },
            error: function(e) {
                $.unblockUI();
                if (typeof e != "undefined") {
                    autoTran.showError(e.toString());
                }
            }
    });
};

$(document).ready(function() {
    autoTran.init();
});