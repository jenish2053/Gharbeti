var app = angular.module('expenseTypeIndex', ['ui.bootstrap', 'ui.utils']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.PaymentList = {
            records: [],
        };
       
        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };

        $scope.init = function (expenseTypeList) {

            $scope.PaymentList.records = expenseTypeList;
        }
       
    }]);









