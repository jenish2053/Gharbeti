var app = angular.module('messageIndex', ['ui.select', 'ngSanitize', 'ui.bootstrap', 'ui.utils']);


app.controller('messageController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.Message =
        {
            Id: 0,
            Subject: "",
            Body: "",
            IsAll : "0",
            Tenant:  []
        };

        $scope.HouseList = [];

        $scope.Multiple = {
            tenants: []
        }

        $scope.MessageList = {
            records: [],
        };

        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };

        $scope.DisableMultipleSelect = true;


        $scope.init = function (messageList) {
            $scope.MessageList.records = messageList;
            $scope.GetHouse();
        }

        $scope.onClickAdd = function () {

            data = angular.copy($scope.Message);

            if (data.Id == "") {

                if ($scope.Message.IsAll == 1) {
                    data.IsAll = false;
                    data.Tenant = $scope.Multiple.tenants;
                }
                else {
                    data.IsAll = true;
                }

                $http.post("/api/Message/Add", data).then(function (responsedata) {
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

                if ($scope.Message.IsAll == 1) {
                    data.IsAll = false;
                    data.Tenant = $scope.Multiple.tenants;
                }
                else {
                    data.IsAll = true;
                }

                $http.post('/api/Message/Edit', data).then(function (responsedata) {
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
            $scope.RoomModalTitle = "Add Message";

            $scope.Message =
            {
                Id: 0,
                Subject: "",
                Body: "",
                IsAll: "0",
                Tenant : [],
            };

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowMessage = function (id) {
            $scope.BodyMessage = "";
            var data = $scope.MessageList.records.find(x => x.Id == id);
            $scope.BodyMessage = data.Body;
            $('#messageModal').modal('show');
        }

        $scope.ShowEdit = function (id) {
            $scope.RoomModalTitle = "Edit Message";

            $http({
                url: "/api/Message/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.Message = data;
                    if (data.IsAll) {
                        $scope.Message.IsAll = "0";
                    }
                    else {
                        $scope.Message.IsAll = "1";
                        $scope.Multiple.tenants = data.Tenant;
                        $scope.DisableMultipleSelect = false;
                    }

                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });

        }

        $scope.Delete = function (id) {
            $http({
                url: "/api/Message/Delete",
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


        $scope.GetHouse = function () {
            $http.get('/api/House/GetHousesWithUser').then(function (responsedata) {
                if (responsedata.data.Status) {
                    for (var i = 0; i < responsedata.data.Data.length; i++) {
                        $scope.HouseList.push(responsedata.data.Data[i]);
                    }
                }
                else {
                    alert("Error Occured!!!");
                }
            });
        }

        $scope.onChangeSendType = function () {
            if ($scope.Message.IsAll == 0) {
                $scope.DisableMultipleSelect = true;
            }
            else {
                $scope.DisableMultipleSelect = false;
            }
        }

    }]);









