angular.module('App').controller("HrController", function ($scope, $http, $timeout, $compile) {
    $scope.VendorId = "";
    //For Pagination
    $scope.maxsize = 5;
    $scope.FetchEmployeeList = $scope.EmpTotalCount = 0;
    $scope.EmpPageIndex = 1;
    $scope.EmpPageSize = "50";

    //Employee Starts
    $scope.DefaultEmployeeList = function () {    
        $scope.SearchEmployeeNameId = "";
        $scope.SearchDesignation = "";
        $scope.SearchDepartment = "";
        $scope.SearchEmpStatus = "";
    }

    $scope.FetchEmployeeList = function () {
        $http.get(window.FetchEmployeeList + "?pageindex=" + $scope.EmpPageIndex + "&pagesize=" + $scope.EmpPageSize + "&SearchEmployeeNameId=" + $scope.SearchEmployeeNameId + "&SearchDesignation=" + $scope.SearchDesignation + "&SearchDepartment=" + $scope.SearchDepartment + "&SearchEmpStatus=" + $scope.SearchEmpStatus).success(function (response) {
            $scope.AvailableEmployeeList = response.ListEmployeeEnt;
            $scope.EmpTotalCount = response.totalcount;
        }, function (error) {
            alert('failed');
        });
    }

    //$scope.FetchEmployeeList();

    $scope.EmpPageChanged = function () {
        $scope.FetchEmployeeList();
    }

    $scope.EmpChangePageSize = function () {
        $scope.EmpPageIndex = 1;
        $scope.FetchEmployeeList();
    }

    $scope.SelectedFileForUpload = null;

    // THIS IS REQUIRED AS File Control is not supported 2 way binding features of Angular
    // ------------------------------------------------------------------------------------
    //File Validation
    $scope.ChechFileValid = function (file) {
        var isValid = false;
        if ($scope.SelectedFileForUpload != null) {
            if ((file.type == 'image/png' || file.type == 'image/jpeg' || file.type == 'image/gif') && file.size <= (512 * 1024)) {
                $scope.FileInvalidMessage = "";
                isValid = true;
            }
            else {
                alert("Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)");
                $scope.FileInvalidMessage = "Selected file is Invalid. (only file type png, jpeg and gif and 512 kb size allowed)";
            }
        }
        else {
            alert("Image required!");
            $scope.FileInvalidMessage = "Image required!";
        }
        $scope.IsFileValid = isValid;
    };

    //File Select event 
    $scope.selectFileforUpload = function (file) {
        $scope.SelectedFileForUpload = file[0];
    }
    //----------------------------------------------------------------------------------------


    $scope.SaveEmployee = function () {

        var Status = false;
        Status = GetFormValidationStatus("#formSaveEmployeeDetail");

        $scope.ChechFileValid($scope.SelectedFileForUpload);
        var frm = $("#formSaveEmployeeDetail");
        var formData = new FormData(frm[0]);
        formData.append("file", $scope.SelectedFileForUpload);

        var MobileLen = $('#mobile').val();

        if (!Status) {
            HideLoadder();
            alert("Kindly Fill all mandatory fields");
        }
        else if ($scope.FileInvalidMessage != '') {
            HideLoadder();
        }
        else if (MobileLen.length < 10) {
            alert('Mobile Value should be 10 digits long');
        }
        else {
            $http({ url: window.SaveEmployee, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res > 0) {
                        $scope.FetchEmployeeList();
                        SaveExpDetails(res);
                        $("#ModalPopup").modal('hide');
                    }
                    else
                        alert("Not Saved! Please Contact Support.")

                    HideLoadder();

                    //if (res == 'Saved Successfully!') {

                    //}
                    //else {
                    //    alert(res);
                    //    HideLoadder();
                    //}
                }
            ).error(function (res) { showHttpErr(res); HideLoadder(); });
        }


    }

    $scope.BindHREmpPopup = function () {
        var _actionType = "ADD"
        $.ajax({
            type: "POST",
            data: { actionType: _actionType },
            datatype: "JSON",
            url: window.HREmpPopup,
            success: function (html) {
                html = $compile(html)($scope);
                SetModalTitle("Employee Detail")
                SetModalBody(html);
                HideLoadder();
                SetModalWidth("1200px");
                ShowModal();

                if (!($('.modal.in').length)) {
                    $('.modal-dialog').css({
                        top: '5%',
                        left: '5%'
                    });
                }
                $('#ModalPopup').modal({
                    backdrop: false,
                    show: true
                });

                $('.modal-dialog').draggable({
                    handle: ".modal-body"
                });

            },
            error: function (r) {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
        //});
    }

    $scope.LoadEmployeeViewPopup = function (_EmployeeDetailsId) {
        var _actionType = "VIEW"
        //var ID = e.target.id;
        $.ajax({
            type: "POST",
            data: { actionType: _actionType, HREmpId: _EmployeeDetailsId },
            datatype: "JSON",
            url: window.HREmpPopup,
            success: function (html) {
                SetModalTitle("View Employee Details")
                SetModalBody(html);
                HideLoadder();
                $('#formSaveEmployeeDetail input[type=radio],input[type=text], select').prop("disabled", true);
                $('#save_results').css('display', 'none');
                $('#EmpImageUpload').css('display', 'none');
                $('.bs-tooltip-top').css('display', 'none');
                SetModalWidth("1200px");
                ShowModal();

                if (!($('.modal.in').length)) {
                    $('.modal-dialog').css({
                        top: '5%',
                        left: '5%'
                    });
                }
                $('#ModalPopup').modal({
                    backdrop: false,
                    show: true
                });

                $('.modal-dialog').draggable({
                    handle: ".modal-body"
                });

            },
            error: function () {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }

    $scope.LoadEmployeeEditPopup = function (_EmployeeDetailsId) {
        var _actionType = "EDIT";
        //var ID = e.target.id;
        $.ajax({
            type: "POST",
            data: { actionType: _actionType, HREmpId: _EmployeeDetailsId },
            datatype: "JSON",
            url: window.HREmpPopup,
            success: function (res) {
                var html = $compile(res)($scope);
                SetModalTitle("Edit Employee Details")
                SetModalBody(html);
                HideLoadder();
                $('.bs-tooltip-top').css('display', 'none');
                SetModalWidth("1200px");
                ShowModal();

                $.ajax({
                    type: "GET",
                    data: { EmpId: _EmployeeDetailsId },
                    datatype: "JSON",
                    url: window.GetEmpExpDetails,
                    success: function (data) {

                        if (data.length > 0) {
                            $('#tableExp tbody').empty();
                            $.each(data, function (i, item) {
                                let rowHtml = '<tr sn=${item.SN}> <td><span class="HRSN">${item.SN}</span></td><td><input type="text" class="HREmployer form-control" /></td><td><input type="text" class="HRDesignation form-control " /></td><td><input type="text" class="HRPeriodFrom form-control " /></td><td><input type="text" class="HRPeriodTo form-control " /></td><td><a href="#" class="removeHR">Remove</a></td></tr>';
                                // create object from html string
                                let $row = $(rowHtml)
                                // set value of the select within this row instance
                                $row.find('.HRSN').text(item.RequiredColumn1);
                                $row.find('.HREmployer').val(item.RequiredColumn2);
                                $row.find('.HRDesignation').val(item.RequiredColumn3);
                                $row.find('.HRPeriodFrom').val(item.RequiredColumn4);
                                $row.find('.HRPeriodTo').val(item.RequiredColumn5);
                                // append updated object to DOM
                                $('#tableExp > tbody:last-child').append($row);

                            })

                            $('.CalenderTillTodayDate').datepicker({
                                format: 'dd/mm/yyyy',
                                autoclose: true,
                                changeMonth: true,
                                changeYear: true,
                                endDate: 'today',
                            });

                            $('.removeHR').on("click", function (e) {
                                e.preventDefault();
                                $(this).parent().parent().remove();
                            });

                        }


                    }

                })

                if (!($('.modal.in').length)) {
                    $('.modal-dialog').css({
                        top: '5%',
                        left: '5%'
                    });
                }
                $('#ModalPopup').modal({
                    backdrop: false,
                    show: true
                });

                $('.modal-dialog').draggable({
                    handle: ".modal-body"
                });

            },
            error: function () {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }

    $scope.DeleteEmployee = function (id) {
        if (!confirm("Are you sure to delete?")) {
            return;
        }
        //show_loader();
        $http({ url: window.DeleteEmployeeDetail, method: 'POST', data: { id: id } }).success(
            function (res) {
                if (res == 'Deleted Successfully!') {
                    $scope.FetchEmployeeList();
                } else {
                    alert(res, 'E');
                }
            }
        ).error(function (res) { showHttpErr(res); });
    }

    $scope.GetState = function (Country) {
        $http({ url: window.StateDetail, method: 'POST', data: { countryId: Country } }).success(
            function (res) {
                $scope.ListState = res;
                //$('#VendorName').val(res);
            }
        ).error(function (res) { showHttpErr(res); });
    }

    //Quotation Preparation Starts
    $scope.BindPayroll = function () {
        var _actionType = "ADD"
        $.ajax({
            type: "GET",
            data: { actionType: _actionType },
            datatype: "JSON",
            url: window.PayrollTab,
            success: function (html) {
                html = $compile(html)($scope);
                SetModalPanelBody(html);
                HideLoadder();

            },
            error: function (r) {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }

    //QuotePreparation Modal Starts
    $scope.SavePayrollDetails = function () {
        var frm = $("#formSavePayrollDetail");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus();

        if (!Status) {
            //$scope.ValidationMessage = "Kindly Fill all mandatory fields";
            alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({
                url: window.SavePayrollDetails, method: 'POST', data: formData, headers: { 'Content-Type': undefined }
            }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        alert(res);
                        ClearAllFields();
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }

    }


    //$scope.GetEmpPayrollData = function (EmpId) {
    //    $http({ url: '/Hr/GetEmpPayrollData', method: 'POST', data: { EmpId: EmpId } }).success(
    //        function (res) {
    //            //alert(res);
    //            $scope.UserInitial = res.UserInitial;
    //            $scope.UnitNo = res.UnitNo;
    //            $scope.EmpCode = res.EmpCode;
    //            $scope.BasicPay = res.BasicPay;
    //            $scope.HRA = res.HRA;
    //            $scope.Conv = res.Conv;
    //            $scope.OtherAllow = res.OtherAllow;
    //            $scope.CarAllow = res.CarAllow;
    //            $scope.EPF = res.EPF;
    //            $scope.PPF = res.PPF;
    //            $scope.ESI = res.ESI;
    //            $scope.TDSAMT = res.TDSAMT;
    //            $scope.leaveAdj = res.leaveAdj;
    //            $scope.Absent = res.Absent;
    //            $scope.LoanAmt = res.LoanAmt;
    //            $scope.Adv = res.Adv;
    //            $scope.NetPay = res.NetPay
    //        }
    //    ).error(function (res) { showHttpErr(res); });
    //}

    $scope.GetEmpNameForDept = function (Dept) {
        $http({ url: window.GetEmpDetailsForPayroll, method: 'POST', data: { Data: Dept, Type: 'EmpName' } }).success(
            function (res) {
                //alert(res);
                $scope.ListEmpName = res;
            }
        ).error(function (res) { showHttpErr(res); });
    }

    $scope.GetEmpCodeForName = function (EmpName) {
        $http({ url: window.GetEmpDetailsForPayroll, method: 'POST', data: { Data: EmpName, Type: 'EmpCode' } }).success(
            function (res) {
                //alert(res);
                $scope.ListEmpCode = res;
            }
        ).error(function (res) { showHttpErr(res); });
    }

    //$scope.EmpId = '';
    //$scope.Year = '';


    $scope.GetMonthlyEmpPayrollData = function (Month) {
        var EmpId = $scope.EmpId;
        var Year = $scope.Year;

        $http({ url: window.GetEmpPayrollData, method: 'POST', data: { EmpId: EmpId, Yr: Year, mnth: Month } }).success(
            function (res) {
                //alert(res);
                $scope.UserInitial = res.UserInitial;
                $scope.UnitNo = res.UnitNo;
                $scope.EmpCode = res.EmpCode;
                $scope.BasicPay = res.BasicPay;
                $scope.HRA = res.HRA;
                $scope.Conv = res.Conv;
                $scope.OtherAllow = res.OtherAllow;
                $scope.CarAllow = res.CarAllow;
                $scope.EPF = res.EPF;
                $scope.PPF = res.PPF;
                $scope.ESI = res.ESI;
                $scope.TDSAMT = res.TDSAMT;
                $scope.leaveAdj = res.leaveAdj;
                $scope.Absent = res.Absent;
                $scope.LoanAmt = res.LoanAmt;
                $scope.Adv = res.Adv;
                $scope.NetPay = res.NetPay
            }
        ).error(function (res) { showHttpErr(res); });
    }

    //Leave Management Starts
    $scope.BindLeaveManagement = function () {
        var _actionType = "ADD"
        $.ajax({
            type: "GET",
            data: { actionType: _actionType },
            datatype: "JSON",
            url: window.LeaveManagementTab,
            success: function (html) {
                html = $compile(html)($scope);
                SetModalPanelBody(html);
                HideLoadder();

            },
            error: function (r) {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }

    $scope.SaveEmpLeaveDetails = function () {
        var frm = $("#formSaveLeaveDetail");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus();

        if (!Status) {
            //$scope.ValidationMessage = "Kindly Fill all mandatory fields";
            alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({
                url: window.SaveEmpLeaveDetails, method: 'POST', data: formData, headers: { 'Content-Type': undefined }
            }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        alert(res);
                        ClearAllFields();
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }

    }

    $scope.GetEmpLeaveDetails = function (EmpId) {
        $http({ url: window.GetEmpLeaveDetails, method: 'POST', data: { empId: EmpId } }).success(
            function (res) {
                $scope.AvailableEmpLeavesList = res.ListLeaveMgmt;
            }
        ).error(function (res) { showHttpErr(res); });
    }


    $scope.LoadCertificateViewPopup = function (_EmpId) {
        var _actionType = "VIEW";

        $.ajax({
            type: "POST",
            data: { actionType: _actionType, EmpId: _EmpId },
            datatype: "JSON",
            url: window.CertificatePopup,
            success: function (html) {
                SetModalTitle("Upload - View Certificates");
                SetModalBody(html);
                HideLoadder();
                //SetModalWidth("1400px");
                $('#formSaveCertificateDetail input[type=radio],input[type=text], select').prop("disabled", true);
                $('#saveCertificate').css('display', 'none');
                $('.bs-tooltip-top').css('display', 'none');
                ShowModal();

                if (!($('.modal.in').length)) {
                    $('.modal-dialog').css({
                        top: '5%',
                        left: '1%'
                    });
                }
                $('#ModalPopup').modal({
                    backdrop: false,
                    show: true
                });

                $('.modal-dialog').draggable({
                    handle: ".modal-body"
                });
            },
            error: function () {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }


    //$scope.LoadCertificateEditPopup = function (_EmpId) {
    //    var _actionType = "EDIT";
    //    //var ID = e.target.id;
    //    $.ajax({
    //        type: "POST",
    //        data: { actionType: _actionType, EmpId: _EmpId },
    //        datatype: "JSON",
    //        url: window.CertificatePopup,
    //        success: function (res) {
    //            var html = $compile(res)($scope);
    //            SetModalTitle("Edit Certificate Details")
    //            SetModalBody(html);
    //            HideLoadder();
    //            $('.bs-tooltip-top').css('display', 'none');
    //            //SetModalWidth("1200px");
    //            ShowModal();

    //            if (!($('.modal.in').length)) {
    //                $('.modal-dialog').css({
    //                    top: '5%',
    //                    left: '1%'
    //                });
    //            }
    //            $('#ModalPopup').modal({
    //                backdrop: false,
    //                show: true
    //            });

    //            $('.modal-dialog').draggable({
    //                handle: ".modal-body"
    //            });

    //        },
    //        error: function () {
    //            HideLoadder();
    //            alert(window.ErrorMsg);
    //        }
    //    })
    //}


});

//EOF