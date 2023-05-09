var app = angular.module('transactionIndex', ['ui.bootstrap', 'ui.utils']);


app.controller('transactionController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.TransactionList = {
            records: [],
        };
        $scope.PaymentModeList = [];
        $scope.ExpenseType = [];
        $scope.RoomModalTitle = "";
        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };
        $scope.Transaction =
        {
            Id: 0,
            TransactionDateString: "",
            StartDateString: "",
            EndDateString: "",
            RoomId: 0,
            RentAmount: 0,
            RentPaid: "",
            Remarks: "",
            PaymentModeId: {},
            TransactionDetails: [{
                Id: "",
                TransactionId: "",
                ExpenseId: "",
                Amount: "",
                Remarks: ""
            }]

        };

        $scope.TransactionDetails = {
            Id: "",
            TransactionId: "",
            ExpenseId: "",
            Amount: "",
            Remarks: ""
        };


        $scope.init = function (transctionList, rentAmount) {

            $scope.TransactionList.records = transctionList;
            $scope.GetExpenseType();
            $scope.GetPaymentMode();
            $scope.RentAmount = rentAmount,


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



            $('#modalTransactionDate').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
                todayHighlight: true,
                todayBtn: true
            });
            debugger;
            let todayDateConverted = $filter('date')(new Date(), 'MM/dd/yyyy');
            $("#modalTransactionDate").datepicker('update', todayDateConverted);

        }

        $scope.onClickAdd = function () {

            if ($scope.Transaction.Id == "") {
                data = angular.copy($scope.Transaction);
                data.PaymentModeId = data.PaymentModeId.Id;

                angular.forEach(data.TransactionDetails, function (value) {
                    value.ExpenseId = value.ExpenseId.Id;
                    value.Id = 0;
                    value.TransactionId = 0;
                });

                $http.post("/api/transaction/Add", data).then(function (responsedata) {
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
                data = angular.copy($scope.Transaction);
                data.PaymentModeId = data.PaymentModeId.Id;

                angular.forEach(data.TransactionDetails, function (value) {
                    value.ExpenseId = value.ExpenseId.Id;
                });

                $http.post('/api/transaction/Edit', data).then(function (responsedata) {
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
            $scope.TransactionModalTitle = "Add Transaction";

            $scope.Transaction =
            {
                Id: 0,
                TransactionDateString: "",
                StartDateString: "",
                EndDateString: "",
                RoomId: 0,
                RentPaid: 0,
                Remarks: "",
                RentAmount: $scope.RentAmount,
                PaymentModeId: {},
                TransactionDetails: [{
                    Id: 0,
                    TransactionId: 0,
                    ExpenseId: {},
                    Amount: "",
                    Remarks: ""
                }]

            };


            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {
            $scope.RoomModalTitle = "Edit Transction";

            $http({
                url: "/api/Transaction/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.Transaction = data;
                    $scope.Transaction.PaymentModeId = $scope.PaymentModeList.find(x => x.Id == $scope.Transaction.PaymentModeId);

                    angular.forEach($scope.Transaction.TransactionDetails, function (value, item) {
                        value.ExpenseId = $scope.ExpenseType.find(x => x.Id == value.ExpenseId);
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
                url: "/api/Transaction/Delete",
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
                TransactionId: "",
                ExpenseId: "",
                Amount: "",
                Remarks: ""
            };
            $scope.Transaction.TransactionDetails.push(emptyRow);
        }

        $scope.onRemoveRow = function (index) {
            $scope.Transaction.TransactionDetails.splice(index, 1);
        }

        $scope.GetExpenseType = function () {
            $http.get('/api/expensetype').then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    $scope.ExpenseType = responsedata.data.Data;
                }
            });
        }

        $scope.GetPaymentMode = function () {
            $http.get('/api/PaymentMode').then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    $scope.PaymentModeList = responsedata.data.Data;
                }
            });
        }
    }]);









