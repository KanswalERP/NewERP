angular.module('App').controller("ModalController", function ($scope, $http, $timeout, $compile) {
    $scope.sdata1 = {};

    //Customer Modal Starts
    $scope.SaveCustomer = function () {
        var frm = $("#formSaveCustomerDetail");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus("#formSaveCustomerDetail");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({ url: window.SaveCustomer, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        $scope.FetchCustomerList();
                        $("#ModalPopup").modal('hide');
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });

        }
        
    }

    $scope.IsValid = function () {
        return true;
    }

    //Enquiry Modal Starts
    $scope.SaveEnquiry = function () {
        var frm = $("#formSaveEnquiryDetail");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus("#formSaveEnquiryDetail");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({ url: window.SaveEnquiry, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        $scope.FetchEnquiryList();
                        $("#ModalPopup").modal('hide');
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }
    }

    //Quotation Modal Starts
    $scope.SaveQuotation = function () {
        var frm = $("#formSaveQuotationDetail");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus("#formSaveQuotationDetail");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");
            //alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({ url: window.SaveQuotation, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        $scope.FetchQuotationList();
                        $("#ModalPopup").modal('hide');
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }
        
    }

    //QuotePreparation Modal Starts
    $scope.SaveQuotePreparation = function () {
        if (!confirm("Are you sure you want to Save?")) {
            return;
        }

        var frm = $("#formSaveQuotationPrepDetail");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus("#formSaveQuotationPrepDetail");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");            
        }
        else {
            $http({
                url: window.SaveQuotePreparation, method: 'POST', data: formData, headers: { 'Content-Type': undefined }
            }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        alert(res);
                        ClearAllFields("#formSaveQuotationPrepDetail");
                        $('#divProductDetails').hide();
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }
        
    }

    //Order Modal Starts
    $scope.SaveOrderDetails = function () {

        var frm = $("#formSaveOrderDetails");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus("#formSaveOrderDetails");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({ url: window.SaveOrder, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        $scope.FetchOrdersList();
                        $("#ModalPopup").modal('hide');
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }

    }

    //Item Modal Starts
    $scope.SaveItemDetails = function () {
        var frm = $("#formSaveItemDetails");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus("#formSaveItemDetails");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({ url: window.SaveItemDetails, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        alert(res);
                        //$scope.FetchQuotationList();
                        //$("#ModalPopup").modal('hide');
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }

    }

    //Quotation Modal Starts
    $scope.SaveRevisedQuotation = function () {
        var frm = $("#formSaveRevisedQuotationDetail");
        var formData = new FormData(frm[0]);

        var Status = false;
        Status = GetFormValidationStatus("#formSaveRevisedQuotationDetail");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");
            //alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({ url: window.SaveRevisedQuotation, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        $scope.FetchQuotationList();
                        $("#ModalPopup").modal('hide');
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });
        }

    }

    //Vendor Master Bill Starts
    $scope.SaveInboundGateEntry = function () {
        var frm = $("#formInbound");
        var formData = new FormData(frm[0]);

        //var BillAmount = $('#BillAmount').val();
        //var EndUse = $('#EndUse option:selected').text();
        //if (BillAmount > 1000 && EndUse == 'Non-PO') {
        //    alert('Value exceeds the sanctioned value. Please raise PO');
        //    return;
        //}

        var Status = false;
        Status = GetFormValidationStatus("#formInbound");

        if (!Status) {
            alert("Kindly Fill all mandatory fields");
        }
        else {
            $http({ url: window.SaveGateEntryDetails, method: 'POST', data: formData, headers: { 'Content-Type': undefined } }).success(
                function (res) {
                    if (res == 'Saved Successfully!') {
                        alert(res);
                        $scope.FetchInboundList();
                        $("#ModalPopup").modal('hide');
                    }
                    else {
                        alert(res)
                    }
                }
            ).error(function (res) { showHttpErr(res); });

        }

    }

    
});

//EOF