
var app = angular.module('approveIndex', ['ui.bootstrap', 'ui.utils']);



app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.ApproveList = {
            records: [],
        };
        $scope.Address = {
            AddressLine1: "",
            AddressLine2: "",
            AddressLine3: "",
            City: "",
            PostalCode: "",
            County: "",
            Country: ""
        }

       



        $scope.PersonalDetail = {
            Email: "",
            PhoneNumber: "",
            MobileNumber: "",
            Dob: "",

        }
        $scope.dataTableOpt = {
            //custom datatable options 
            // or load data through ajax call also
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };

        $scope.SubmitRemarks = {
            Id: "",
            Remarks: ""
        }

        $scope.init = function (tenantList) {

            $scope.ApproveList.records = tenantList;
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.onClickAddress = function (id) {

            $scope.Address = {
                AddressLine1: "",
                AddressLine2: "",
                AddressLine3: "",
                City: "",
                PostalCode: "",
                County: "",
                Country: ""
            }

            let data = $scope.ApproveList.records.find(x => x.Id == id);

            $scope.Address.AddressLine1 = data.AddressLine1;
            $scope.Address.AddressLine2 = data.AddressLine2;
            $scope.Address.AddressLine3 = data.AddressLine3;
            $scope.Address.PostalCode = data.PostalCode;
            $scope.Address.County = data.County;
            $scope.Address.Country = data.Country;
            $scope.Address.City = data.City;
            $('#addModal').modal('show');
        }


        $scope.onClickReSubmit = function (id) {
            $scope.SubmitRemarks = {
                Id: id,
                Remarks: ""
            }

            $('#resubmitRemark').modal('show');
        }


        $scope.onClickSaveRemarks = function () {
            $http.get(`/api/Approve/Resubmit/?userId=${$scope.SubmitRemarks.Id}&remarks=${$scope.SubmitRemarks.Remarks}`)
                .then(function (responsedata) {
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


        $scope.onClickDetail = function (id) {

            $scope.PersonalDetail = {
                Email: "",
                PhoneNumber: "",
                MobileNumber: "",
                Dob: "",

            }

            let data = $scope.ApproveList.records.find(x => x.Id == id);

            $scope.PersonalDetail.Email = data.Email
            $scope.PersonalDetail.PhoneNumber = data.PhoneNumber
            $scope.PersonalDetail.MobileNumber = data.MobileNumber
            $scope.PersonalDetail.Dob = data.Dob

            $('#userModal').modal('show');
        }


        $scope.onClickApprove = function (id) {

            if (confirm("Do you really want to Approve this Tenant Registration?")) {
                $http.get(`/api/Approve/register/?id=${id}`).then(function (responsedata) {
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

       


    }]);









