"use strict";

var KenoModule = angular.module('kenoSettingModule', ['ngDialog']);


KenoModule.directive('ieSelectFix', function ($timeout) {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function (scope, element, attrs, ctrl) {
            attrs.$observe(
                    "ieSelectFix",
                    function (value) {
                        var isIE = document.attachEvent;
                        if (isIE) {
                            var control = element[0];
                            var option = document.createElement("option");
                            control.add(option, null);
                            control.remove(control.options.length - 1);

                            var tmp = angular.element('<option></option>');
                            element.append(tmp);
                            tmp.remove();
                        }
                    });
        }
    }
});