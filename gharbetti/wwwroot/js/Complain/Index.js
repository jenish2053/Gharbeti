var app = angular.module('complainIndex', ['ui.bootstrap', 'ui.utils']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.ComplainList = {
            records: [],
        };

        $scope.Complain =
        {
            Id: 0,
            Reason: "",
            Response: "",
            ComplainDate: "",
            Status: "",
            StatusDisabled: false,
        };
        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };

        $scope.StatusList = [
            {
                Name: "Open",
                Value: 0
            },
            {
                Name: "In-Progress",
                Value: 1
            },
            {
                Name: "Closed",
                Value: 2
            }
        ]

        $scope.init = function (complainList) {
            $scope.ComplainList.records = complainList;
        }

        $scope.onClickAdd = function () {

            if ($scope.Complain.Id == "") {
                //data = angular.copy($scope.Complain);

                var data = {};
                data.Id = 0;
                data.Reason = $scope.Complain.Reason;
                data.Response = $scope.Complain.Response;
                data.ComplainDate = new Date();
                data.Status = $scope.Complain.Status.Value;
                data.TenantId = "00000000-0000-0000-0000-000000000000";

                //data.Status = data.Status.Value;
                //data.ComplainDate = new Date();

                $http.post("/api/Complain/Add", data).then(function (responsedata) {
                    debugger;
                    let result = responsedata.data;
                    if (result.Status) {
                        alert(result.Message);
                        location.reload();
                    }
                    else {
                        alert(result.Message);
                    }
                });
            }
            else {
                data = $scope.Complain;
                data.Status = data.Status.Value;

                $http.post('/api/Complain/Edit', data).then(function (responsedata) {
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
            $scope.RoomModalTitle = "Add Complain";

            $scope.Complain =
            {
                Id: 0,
                Reason: "",
                Response: "",
                ComplainDate: "",
                Status: $scope.StatusList[0],
                StatusDisabled: true,
            };

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {
            $scope.ComplainModalTitle = "Edit Complain";

            $http({
                url: "/api/Complain/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.Complain = data;
                    $scope.Complain.Status = $scope.StatusList.find(x => x.Value == $scope.Complain.Status);
                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });

        }

        $scope.Delete = function (id) {
            $http({
                url: "/api/Complain/Delete",
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









