var app = angular.module('roomIndex', ['ui.bootstrap', 'ui.utils']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.RoomList = {
            records: [],
        };
        $scope.RoomTypeList = [];
        $scope.FloorType = [];
        $scope.RoomModalTitle = "";

        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };
        $scope.Room =
        {
            Id: 0,
            RoomNo: "",
            FloorId: "",
            RentAmount: 0,
            SquareFootage: "",
            Remarks: "",
            RoomDetails: [{
                Id: "",
                RoomId: "",
                RoomTypeId: "",
                SquareFootage: ""
            }]

        };

        $scope.RoomDetailRow = {
            Id: "",
            RoomId: "",
            RoomTypeId: "",
            SquareFootage: ""
        };


        $scope.init = function (roomList) {

            $scope.RoomList.records = roomList;
            $scope.GetFloor();
            $scope.GetRoomType();
        }

        $scope.onClickAdd = function () {

            if ($scope.Room.Id == "") {
                data = $scope.Room;
                data.FloorId = data.FloorId.Id;

                angular.forEach(data.RoomDetails, function (value, key) {
                    value.Id = 0;
                    value.RoomId = 0;
                    value.RoomTypeId = value.RoomTypeId.Id;
                });

                $http.post("/api/Room/Add", data).then(function (responsedata) {
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
                data = $scope.Room;
                data.FloorId = data.FloorId.Id;

                angular.forEach(data.RoomDetails, function (value, key) {
                    value.Id = 0;
                    value.RoomId = 0;
                    value.RoomTypeId = value.RoomTypeId.Id;
                });

                $http.post('/api/Room/Edit', data).then(function (responsedata) {
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
            $scope.RoomModalTitle = "Add Room";

            $scope.Room =
            {
                Id: 0,
                RoomNo: "",
                FloorId: "",
                RentAmount: 0,
                SquareFootage: "",
                Remarks: "",
                RoomDetails: [{
                    Id: "",
                    RoomId: "",
                    RoomTypeId: "",
                    SquareFootage: ""
                }]

            };

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {
            $scope.RoomModalTitle = "Edit Room";

            $http({
                url: "/api/Room/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.Room = data;
                    $scope.Room.FloorId = $scope.FloorType.find(x => x.Id == $scope.Room.FloorId);

                    angular.forEach($scope.Room.RoomDetails, function (value, item) {
                        value.RoomTypeId = $scope.RoomTypeList.find(x => x.Id == value.RoomTypeId);
                    });

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

        $scope.onAddRoomType = function () {

            let emptyRow = {
                Id: "",
                RoomId: "",
                RoomTypeId: "",
                SquareFootage: ""
            };
            $scope.Room.RoomDetails.push(emptyRow);
        }

        $scope.onRemoveRow = function (index) {
            $scope.Room.RoomDetails.splice(index, 1);
        }

        $scope.GetFloor = function () {
            $http.get('/api/floor').then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    $scope.FloorType = responsedata.data.Data;
                }
            });
        }

        $scope.GetRoomType = function () {
            $http.get('/api/roomtype').then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    $scope.RoomTypeList = responsedata.data.Data;
                }
            });
        }
    }]);









