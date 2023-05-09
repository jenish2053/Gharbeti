var app = angular.module('roomIndex', ['ui.bootstrap', 'ui.utils']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.RoomList = {
            records: [],
        };
        $scope.Room = {
            Id: 0,
            Name: "",
            IsActive: true,
        }

        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };

        $scope.init = function (roomList) {

            $scope.RoomList.records = roomList;
        }

        $scope.onClickAdd = function () {

            if ($scope.Room.Id == "") {
                data = $scope.Room;
                $http.post("/api/RoomType/Add", data).then(function (responsedata) {
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
                data = $scope.Room
                $http.post('/api/RoomType/Edit', data).then(function (responsedata) {
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
            $scope.Room = {
                Id: 0,
                Name: "",
                IsActive: true,
            }

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {

            $http({
                url: "/api/RoomType/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.Room.Id = data.Id;
                    $scope.Room.Name = data.Name;

                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });
          
        }

        $scope.Delete = function (id) {

            $http({
                url: "/api/RoomType/Delete",
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









