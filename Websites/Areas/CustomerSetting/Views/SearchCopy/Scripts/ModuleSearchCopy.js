angular.module('SearchCopy.service', []).
 value('message', {
     // Success=0, Error=1, Warning=2
     showAlertMessage: function (containerid, msg, type) {
         $("#" + containerid).removeClass("hidden");
         switch (type) {
             case 0:
                 document.getElementById(containerid).innerHTML = "<div class='alertboxscc'><div>" + msg + "</div></div>";
                 break;
             case 1:
                 document.getElementById(containerid).innerHTML = "<div class='alertboxerr'><div>" + msg + "</div></div>";
                 break;
             case 2:
                 document.getElementById(containerid).innerHTML = "<div class='alertboxwrn'><div>" + msg + "</div></div>"; ;
                 break;
             default:
                 break
         }
     },

     hideAlertMessage: function (containerid) {
         $("#" + containerid).addClass("hidden");
     }     
 }).
 value('validate', {
     setRequired: function (id) {
         $(id).removeClass("ng-valid").removeClass("ng-valid-required");
         $(id).addClass("ng-dirty").addClass("ng-invalid").addClass("ng-invalid-required");
         $(id).focus();
     }
 });

angular.module('SearchCopy.directive', []);
angular.module('SearchCopy.filter', []);
