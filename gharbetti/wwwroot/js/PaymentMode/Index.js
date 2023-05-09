var app = angular.module('paymentModeIndex', ['ui.bootstrap', 'ui.utils']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.PaymentModeList = {
            records: [],
        };
        $scope.PaymentMode = {
            Id: 0,
            Name: "",
        }
        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };

        $scope.init = function (PaymentModeList) {

            $scope.PaymentModeList.records = PaymentModeList;
        }

        $scope.onClickAdd = function () {

            if ($scope.PaymentMode.Id == "") {
                data = $scope.PaymentMode;
                $http.post("/api/PaymentMode/Add", data).then(function (responsedata) {
                    debugger;
                    console.log(responsedata.data);
                    if (responsedata.data.Status) {
                        location.reload();
                    }
                    else {
                        alert("Error Occured");
                    }
                });
            }
            else {
                data = $scope.PaymentMode
                $http.post('/api/PaymentMode/Edit', data).then(function (responsedata) {
                    debugger;
                    console.log(responsedata.data);
                    if (responsedata.data.Status) {
                        location.reload();
                    }
                    else {
                        alert("Error Occured");
                    }
                });

            }
        }

        $scope.onClickAddModal = function () {
            $scope.PaymentMode = {
                Id: 0,
                Name: "",
            }

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {

            $http({
                url: "/api/PaymentMode/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.PaymentMode.Id = data.Id;
                    $scope.PaymentMode.Name = data.Name;

                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });
          
        }

        $scope.Delete = function (id) {

            $http({
                url: "/api/PaymentMode/Delete",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    alert(responsedata.data.Message);
                    location.reload();
                   }
                else {
                    alert(responsedata.data.Message);
                }
            });

        }
    }]);









