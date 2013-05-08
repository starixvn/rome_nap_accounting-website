jQuery(document).ready(function() {
        jQuery("#dialog:ui-dialog" ).dialog("destroy" );	
	    jQuery("#dialog-message" ).dialog({
		    modal: true,
		    autoOpen: false,
		    buttons: {
			    Ok: function() {
			        var strcode=jQuery(this).data('strcode');
			        DoCal2("m", strcode + "|0");
				    jQuery(this).dialog("close");
			    },
			    Cancel: function() {				        
				    jQuery(this).dialog("close");
			    }
		    }
	    });
	    jQuery("#dialog-result").dialog({
		    modal: true,
		    autoOpen: false,
		    buttons: {
			    Ok: function() {				        
				    jQuery(this).dialog("close");
				    var result=jQuery(this).data('result');
				    if (result == 'Calculate Multiple Successfully!' || result == 'Refund Multiple Successfully!' || result == 'Refund Multiple FT Successfully!') {Refresh(); }
			    }
		    }
	    });
     });
    
    function checkMultipleAll() {
        var ischecked = document.getElementById("chk_all").checked;
        jQuery(".chkmultiple:not(:disabled)").attr("checked",ischecked);
    }
    function checkMultipleLeague(leagueCount) {
       
        var ischecked = document.getElementById("chk_" + leagueCount).checked;
        jQuery(".cls_" + leagueCount + ":not(:disabled)").attr("checked", ischecked);
        
        var ischeckedall=true;
        var allchk=jQuery(".chkmultiple");
         jQuery.each(allchk, function (i, n) {
            var chk = jQuery(this);
            if (!chk.attr("checked") && !chk.attr("disabled")) {
                 ischeckedall=false;
                 return;   
            }
        });
        jQuery("#chk_all").attr("checked", ischeckedall);
       
    }
    
   
    function DoCalMultiple() {
        strids = "";
        var strcode = "";
        var chks = jQuery(".calmultiple");
        jQuery.each(chks, function (i, n) {
            var chk = jQuery(this);
            var id = chk.attr("id");
            if (chk.attr("checked") == true) {
                var arr=id.split('_');
                if (strids == "") {
                    strids += arr[1];
                    strcode+= arr[4];
                }
                else {
                    strids += "," + arr[1];
                    strcode+= ", " + arr[4];
                }
            }
        });
  
        //alert(strids);
        var sConfirm="Are you sure you want to CALCULATE MULTIPLE for the matches: " + strcode + " ?";
        jQuery("#msgContent").html(sConfirm);
        if (strids!='')
        {
            //if (!confirm(sConfirm)) return;
             jQuery("#dialog-message" ).data('strcode',strids).dialog("open");
        }
    }
 
 function initMultipleCalculate()
 {
    var t=setTimeout(function(){
        jQuery('.calmultiple').shiftClick();
        
        var isDisabledAll=true;
        jQuery('.group').each(function(){
            var isDisabled=true;
            var id=jQuery(this).attr("id");
            jQuery('.cls_' + id.replace("chk_","")).each(function(){
               if (!jQuery(this).attr("disabled"))
               {
                    isDisabled=false;
                    isDisabledAll=false;
                    return;
               } 
            });
            jQuery(this).attr("disabled",isDisabled);
            
            
        });
       
        jQuery("#chk_all").attr("disabled",isDisabledAll);
        
        jQuery('.calmultiple').bind('click', function() {
            var arrid=jQuery(this).attr("id").split('_');
            var leagueCount="chk_" + arrid[3];
            var leagueclass="cls_" + arrid[3];
            var checked=true;
          
            jQuery("." + leagueclass).each(function(){ 
                if (!jQuery(this).attr("checked") && !jQuery(this).attr("disabled"))
                {
                    checked=false;
                    return;
                }
            });
           
            jQuery("#" + leagueCount).attr("checked", checked);
            
            //check all
            var ischeckedall=true;
            var allchk=jQuery(".chkmultiple");
             jQuery.each(allchk, function (i, n) {
                var chk = jQuery(this);
                if (!chk.attr("checked") && !chk.attr("disabled") ) {
                     ischeckedall=false;
                     return;   
                }
            });
            jQuery("#chk_all").attr("checked", ischeckedall);
        });
    },1000);
 }
