var app = angular.module('cleanScheduleIndex', ['ui.bootstrap', 'ui.utils']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.CleanScheduleList = {
            records: [],
        };

        $scope.CleanSchedule =
        {
            Id: 0,
            StartDate: "",
            EndDate: "",
            Remarks: "",
            Tenant: "",
            CreatedBy: ""
        };
        $scope.emptyGuid = "00000000-0000-0000-0000-000000000000";

        $scope.dataTableOpt = {

            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };

        $scope.init = function (cleanScheduleList) {
            $scope.CleanScheduleList.records = cleanScheduleList;
            $scope.GetTenantList();


            $('#modalStartDate').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
                todayHighlight: true,
                todayBtn: true

            });
            $('#modalEndDate').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
                todayHighlight: true,
                todayBtn: true
            });
            //val(convertToTodayFormat(data.StartDateString));

            //$scope.CleanSchedule.EndDate = new Date(data.EndDateString);
            //$('#modalEndDate').val(convertToTodayFormat(data.EndDateString));

        }

        $scope.onClickAdd = function () {

            if ($scope.CleanSchedule.Id == "") {
                data = angular.copy($scope.CleanSchedule);
                data.StartDate = new Date();
                data.EndDate = new Date();
                data.TenantId = data.TenantId.Id;
                data.CreatedBy = $scope.emptyGuid;


                $http.post("/api/CleanSchedule/Add", data).then(function (responsedata) {
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

                data = angular.copy($scope.CleanSchedule);
                data.StartDate = new Date();
                data.EndDate = new Date();
                data.CreatedBy = $scope.emptyGuid;
                data.TenantId = data.TenantId.Id;

                $http.post('/api/CleanSchedule/Edit', data).then(function (responsedata) {
                    debugger;
                    console.log(responsedata.data);
                    if (responsedata.data.Status) {
                        alert(responsedata.data.Message);
                        location.reload();
                    }
                    else {
                        alert("Error Occured");
                    }
                });

            }
        }

        $scope.onClickAddModal = function () {
            $scope.RoomModalTitle = "Add CleanSchedule";

            $scope.CleanSchedule =
            {
                Id: 0,
                StartDate: "",
                EndDate: "",
                Remarks: "",
                Tenant: "",
                CreatedBy: ""
            };

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {
            $scope.CleanScheduleModalTitle = "Edit CleanSchedule";

            $http({
                url: "/api/CleanSchedule/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.CleanSchedule = data;
                    $scope.StartDateString = convertToTodayFormat(data.StartDateString);
                    $scope.EndDateString = convertToTodayFormat(data.EndDateString);
                    $("#modalStartDate").datepicker('update', $scope.StartDateString);
                    $("#modalEndDate").datepicker('update', $scope.EndDateString);

                    $scope.CleanSchedule.TenantId = $scope.TenantList.find(x => x.Id == $scope.CleanSchedule.TenantId);
                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });

        }

        $scope.GetTenantList = function () {
            $scope.TenantList = [];
            $http.get("/api/User/gettenantsamehouse").then(function (responsedata) {
                if (responsedata.data.Status) {
                    $scope.TenantList = responsedata.data.Data;
                }
                else {
                    alert(responsedata.data.Message);
                }
            })
        }

        $scope.Delete = function (id) {
            $http({
                url: "/api/CleanSchedule/Delete",
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


        function convertToTodayFormat(date) {
            var now = new Date(date);

            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);

            //var today = now.getFullYear() + "-" + (month) + "-" + (day);
            var today = (month) + "/" + (day) + "/" + now.getFullYear();

            return today
        }

    }]);









