var username = '';
jQuery(document).ready(function() {
     //var old_username = '';
     $('#txtUsername').focusout(function() {
           if($('#txtUsername').val().trim() != ""){
               
                EnableNG.SelBetSettingUpline();
                username = $('#txtUsername').val().trim();
           }
           else{
                $('#txtMinbet').val('');
                $('#txtMaxbet').val('');
                $('#txtMaxMatch').val('');
                $('#txtMaxBall').val('');
                $('#divMinbet').hide();
                $('#divMaxbet').hide();
                $('#divMaxMatch').hide();
                $('#divMaxball').hide();    
           }
            
      });

});
var EnableNG = {
    ChangeEnable: function () {
        if ($('#chbEnable').attr('checked')) {
            $('#txtMinbet').attr('disabled', false);
            $('#txtMaxbet').attr('disabled', false);
            $('#txtMaxMatch').attr('disabled', false);
            $('#txtMaxBall').attr('disabled', false);
            $('#divMinbet').show();
            $('#divMaxbet').show();
            $('#divMaxMatch').show();
            $('#divMaxball').show();

            
        }
        else {
            $('#txtMinbet').attr('disabled', true);
            $('#txtMaxbet').attr('disabled', true);
            $('#txtMaxMatch').attr('disabled', true);
            $('#txtMaxBall').attr('disabled', true);
            $('#txtMinbet').val('');
            $('#txtMaxbet').val('');
            $('#txtMaxMatch').val('');
            $('#txtMaxBall').val('');
            $('#divMinbet').hide();
            $('#divMaxbet').hide();
            $('#divMaxMatch').hide();
            $('#divMaxball').hide();

        }
    },

    UpdateBetSettingNGEnable: function () {
        var username = $('#txtUsername').val().trim();
        var minbet = $('#txtMinbet').val().trim();
        var maxbet = $('#txtMaxbet').val().trim();
        var maxperball = $('#txtMaxBall').val().trim();
        var maxpermatch = $('#txtMaxMatch').val().trim();
        var isEnable = 0;
        var str = '';
        if ($('#chbEnable').attr('checked')) {
            isEnable = 1;
            if ($('#txtUsername').val().trim() == "") {
                $('#ErrMsg').show();
                showAlertMessage("ErrMsg", "Please input username!", 1);
                $('#txtUsername').focus();
            }
            else if (!IsNumeric(minbet.trim()) || minbet.trim() == "") {
                $('#ErrMsg').show();
                showAlertMessage("ErrMsg", "Invalid Min bet", 1);
                $('#txtMinbet').focus();

            }
            else if (!IsNumeric(maxbet.trim()) || maxbet.trim() == "") {
                $('#ErrMsg').show();
                showAlertMessage("ErrMsg", "Invalid Max bet", 1);
                $('#txtMaxbet').focus();
            }
            else if (!IsNumeric(maxpermatch.trim()) || maxpermatch.trim() == "") {
                $('#ErrMsg').show();
                showAlertMessage("ErrMsg", "Invalid Max per match", 1);
                $('#txtMaxMatch').focus();
            }
            else if (!IsNumeric(maxperball.trim()) || maxperball.trim() == "") {
                $('#ErrMsg').show();
                showAlertMessage("ErrMsg", "Invalid Max per ball", 1);
                $('#txtMaxBall').focus();
            }
            else {
                str = 'Are you sure to enable Number Game and update bet settings for whole line of ' + username + ' ?';
                EnableNG.CallServiceUpdateNG(username, isEnable, minbet, maxbet, maxperball, maxpermatch, str);
            }
        }
        else {
            if ($('#txtUsername').val().trim() == "") {
                $('#ErrMsg').show();
                showAlertMessage("ErrMsg", "Please input username!", 1);
                $('#txtUsername').focus();
            }
            else
            {
                str = 'Are you sure to disable Number Game for whole line of ' + username + ' ?';
                EnableNG.CallServiceUpdateNG(username, isEnable, minbet, maxbet, maxperball, maxpermatch, str);
            }
        }


    },
    CallServiceUpdateNG: function (username, isEnable, minbet, maxbet, maxperball, maxpermatch, str) {
        var param = { username: username, isEnable: parseInt(isEnable), minbet: minbet, maxbet: maxbet, maxperball: maxperball, maxpermatch: maxpermatch };
        try { parent.BlockUI(); } catch (e) { }
        if(confirm(str)){
            $.ajax({
                type: "POST",
                url: "../../../Services/WCF/Customer/ServiceCustomerInfo.svc/UpdBetSettingEnableNGCustomer",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (rs) {
                    if (rs.ErrorCode == 0) {
                        $('#ErrMsg').show();
                        showAlertMessage("ErrMsg", "Updated bet setting successfully!", 0);
                    }
                    else {
                        $('#ErrMsg').show();
                        showAlertMessage("ErrMsg", rs.ErrorMessage, 1);
                    }
                    var t = setTimeout(function () {
                        $("#ErrMsg").empty();
                    }, 10000);

                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
        else 
             try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    SelBetSettingUpline: function () {
        
        var param = { username: $('#txtUsername').val().trim() };
        $('#divMinbet').show();
        $('#divMaxbet').show();
        $('#divMaxMatch').show();
        $('#divMaxball').show();
        $('#txtMinbet').attr('disabled', true);
        $('#txtMaxbet').attr('disabled', true);
        $('#txtMaxMatch').attr('disabled', true);
        $('#txtMaxBall').attr('disabled', true);
        if (username != $('#txtUsername').val().trim()) {
            $('#txtMinbet').val('');
            $('#txtMaxbet').val('');
            $('#txtMaxMatch').val('');
            $('#txtMaxBall').val('');
            $('#chbEnable').attr('checked', false);
        }
        if($('#chbEnable').attr('checked')){
             $('#txtMinbet').attr('disabled', false);
            $('#txtMaxbet').attr('disabled', false);
            $('#txtMaxMatch').attr('disabled', false);
            $('#txtMaxBall').attr('disabled', false);
        }
        else{
             $('#txtMinbet').attr('disabled', true);
            $('#txtMaxbet').attr('disabled', true);
            $('#txtMaxMatch').attr('disabled', true);
            $('#txtMaxBall').attr('disabled', true);
        }
        if ($('#txtUsername').val().trim() == "") {
            $('#ErrMsg').show();
            showAlertMessage("ErrMsg", "Invalid Username", 1);
        }
        else {
            $.ajax({
                type: "POST",
                url: "../../../Services/WCF/Customer/ServiceCustomerInfo.svc/SelBetSettingUpline",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (rs) {
                    if (rs.errCode != 0) {
                        $('#ErrMsg').show();
                        showAlertMessage("ErrMsg", rs.errMsg, 1);
                        if (rs.isEnable == 0) {
                            $('#divEnable').show();
                            $('#divEnable').html('The upline is disabled Number Game');
                            $('#chbEnable').attr('disabled', true);
                        }
                    }
                    else {
                            $('#divEnable').html('&nbsp;');
                            $('#chbEnable').attr('disabled', false)
                            if (parseFloat(rs.minbet) > 0) {
                                $('#divMinbet').html('>= ' + rs.minbet);
                            }
                            else 
                                $('#divMinbet').html('&nbsp;');
                            if (parseFloat(rs.maxbet) > 0) {
                                $('#divMaxbet').html('<= ' + rs.maxbet);
                            }
                            else
                                 $('#divMaxbet').html('&nbsp;');
                            if (parseFloat(rs.maxpermach) > 0) {
                                $('#divMaxMatch').html('<= ' + rs.maxpermach);
                            }
                            else
                                $('#divMaxMatch').html('&nbsp;');
                            if (parseFloat(rs.maxperball) > 0) {
                                $('#divMaxball').html('<= ' + rs.maxperball);
                            }
                            else
                                $('#divMaxball').html('&nbsp;');
                        }
                    
                    var t = setTimeout(function () {
                        $("#ErrMsg").empty();
                    }, 10000);

                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
    }
};
function IsNumeric(sText)

{
   var ValidChars = "0123456789";
   var IsNumber = true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
   if(sText.length > 14)
        IsNumber = false;
   return IsNumber;
   
   }
   
   function onKeyEnter(e) {
    if (e.keyCode == 13) {
        EnableNG.SelBetSettingUpline();
    }
   }
   
  