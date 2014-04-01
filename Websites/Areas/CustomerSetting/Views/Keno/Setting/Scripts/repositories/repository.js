"use strict";

KenoModule.factory('kenoRepositories', function ($http, $q, dataService) {
    return {
        //kenoSetting: dataService.getKenoSetting(3),
        kenoSetting: model,
        save: function (url, kenoSetting) {
            var deferred = $q.defer();
            $http.post(url, kenoSetting)
                .success(function (data, status, headers, config) { deferred.resolve(data); })
                .error(function () { deferred.reject(); });
            return deferred.promise;
        }
    }
});

