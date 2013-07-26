var ProgressiveBonus = angular.module("ProgressiveBonus", []);

ProgressiveBonus.controller(
    "ProgressiveBonusController",
    function ($scope) {
        $scope.module = null;
        $scope.products = [];
        $scope.userName = '';
        $scope.roleId = 0;
        $scope.error = null;

        $(document).ready(function () {
            $scope.load();
        });

        $scope.load = function () {
            $.ajax({
                type: "POST",
                url: "Casino/GetProducts",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {

                },
                success: function (rs) {
                    $scope.$apply(function () {
                        $scope.module = rs;
                        $scope.products = rs.Products;
                        $scope.roleId = rs.RoleId;
                        $scope.userName = $scope.getUserName(rs.UserName);
                    });
                },
                error: function (e) { },
                complete: function (e) {

                }
            });
        },

        $scope.getUserName = function (username) {
            var _userName = '';
            switch ($scope.roleId) {
                case 4:
                    _userName = 'Super: ' + username;
                    break
                case 3:
                    _userName = 'Master: ' + username
                    break
                case 2:
                    _userName = 'Agent: ' + username;
                    break
                case 1:
                    _userName = 'Member: ' + username;
                    break
            }

            return _userName;
        },

		$scope.hideEnableDownlineLink = function () {
		    return this.roleId == 1; // Member
		};

        $scope.getClass = function () {
            if ($scope.error) {
                return $scope.error.ErrorCode == 0 ? 'alertboxscc' : 'alertboxerr';
            }
        };

        $scope.parentChange = function (product) {
            if (product.preVal) {
                product.EnableDownlines = false;
                product.EnableDownlinesButton = false;
            }
            else {
                if (product.Enable) {
                    product.EnableDownlines = false;
                    product.EnableDownlinesButton = true;
                }
                else {
                    product.EnableDownlines = false;
                    product.EnableDownlinesButton = false;
                }
            }
        };

        $scope.update = function () {
            $.ajax({
                type: "POST",
                url: "Casino/UpdateProgressive",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify($scope.module),
                beforeSend: function () {

                },
                success: function (rs) {
                    if (rs && rs.length > 0) {
                        $scope.$apply(function () {
                            $scope.error = rs[0];
                        });
                    }
                },
                error: function (e) { },
                complete: function (e) {

                }
            });
        }
    }
);

