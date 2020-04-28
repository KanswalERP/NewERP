﻿var CustomerDetails = function () {

    var SmallFuntion = function (e) {
        var fn = $('#' + e.id).data('inneraction');
        var CustomerDetails = "CustomerDetails";
        window[CustomerDetails][fn](e);
    }
    var validation = function (e) {
        if (($('#' + e.id).val() === null) || ($('#' + e.id).val() === "-1") || ($('#' + e.id).val() === "") || ($('#' + e.id).val().length <= 0)) {
            $(e).next("span").removeClass('HideValidMsg');
            $(e).next("span").addClass('CustomRequiredCSS');
            $(e).closest('.form-group').find('.ErrorMessages').find('.HideValidMsg').addClass('CustomRequiredCSS');
            $(e).closest('.form-group').find('.ErrorMessages').find('.CustomRequiredCSS').removeClass('HideValidMsg');
        } else {
            $(e).next("span").removeClass('CustomRequiredCSS');
            $(e).next("span").addClass('HideValidMsg');
            $(e).closest('.form-group').find('.ErrorMessages').find('.CustomRequiredCSS').addClass('HideValidMsg');
            $(e).closest('.form-group').find('.ErrorMessages').find('.HideValidMsg').removeClass('CustomRequiredCSS');
        }
    }
    var BindPopup = function () {
        //$(".btn-Add-CustomerDetails").on("click", function (e) {
        var _actionType = "ADD"
        //var _CustomerDetailsId = $(this).parents("tr:first").find("#CustomerDetailsId").val();
        //var _staffProfileName = $(this).parents("tr:first").find("#StaffFirstName").val();
        $.ajax({
            type: "POST",
            data: { actionType: _actionType },
            datatype: "JSON",
            url: window.OtherDetailsPopup,
            success: function (html) {
                SetModalTitle("Add Customer Details")
                SetModalBody(html);
                HideLoadder();
                ShowModal();
            },
            error: function (r) {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
        //});
    }
    var onButtonRelease = function (e) {
        alert(e);
    }
    var SearchDetails = function () {
        var _search = $('#txtSearch').val();

        $.ajax({
            type: "POST",
            data: { search: _search },
            datatype: "JSON",
            url: window.CustomerWithSearch,
            success: function (response) {
                
            },
            error: function (r) {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }

    var SaveData = function () {

            var IsValid = true;
            //if ($('#ddlposttype').val() === "" || $('#ddlposttype').val() === null || $('#ddlposttype').val() === "-1") {
            //    IsValid = false;
            //    $('#ddlposttype').next("span").removeClass('HideValidMsg');
            //    $('#ddlposttype').next("span").addClass('CustomRequiredCSS');
            //} else {
            //    $('#ddlposttype').next("span").removeClass('CustomRequiredCSS');
            //    $('#ddlposttype').next("span").addClass('HideValidMsg');
            //}
            //if ($('#ddlrole').val() === "" || $('#ddlrole').val() === null || $('#ddlrole').val() === "-1") {
            //    IsValid = false;
            //    $('#ddlrole').next("span").removeClass('HideValidMsg');
            //    $('#ddlrole').next("span").addClass('CustomRequiredCSS');
            //} else {
            //    $('#ddlrole').next("span").removeClass('CustomRequiredCSS');
            //    $('#ddlrole').next("span").addClass('HideValidMsg');
            //}
            //if ($('#ddlcourse').val() === "" || $('#ddlcourse').val() === null || $('#ddlcourse').val() === "-1") {
            //    IsValid = false;
            //    $('#ddlcourse').next("span").removeClass('HideValidMsg');
            //    $('#ddlcourse').next("span").addClass('CustomRequiredCSS');
            //} else {
            //    $('#ddlcourse').next("span").removeClass('CustomRequiredCSS');
            //    $('#ddlcourse').next("span").addClass('HideValidMsg');
            //}
            //if ($('#ddldesignation').val() === "" || $('#ddldesignation').val() === null || $('#ddldesignation').val() === "-1") {
            //    IsValid = false;
            //    $('#ddldesignation').next("span").removeClass('HideValidMsg');
            //    $('#ddldesignation').next("span").addClass('CustomRequiredCSS');
            //} else {
            //    $('#ddldesignation').next("span").removeClass('CustomRequiredCSS');
            //    $('#ddldesignation').next("span").addClass('HideValidMsg');
            //}
            //if ($('#ddlAppointment').val() === "" || $('#ddlAppointment').val() === null || $('#ddlAppointment').val() === "-1") {
            //    IsValid = false;
            //    $('#ddlAppointment').next("span").removeClass('HideValidMsg');
            //    $('#ddlAppointment').next("span").addClass('CustomRequiredCSS');
            //} else {
            //    $('#ddlAppointment').next("span").removeClass('CustomRequiredCSS');
            //    $('#ddlAppointment').next("span").addClass('HideValidMsg');
            //}
            //if ($('#IsApprovedUniversity').val().length <= 0) {
            //    IsValid = false;
            //    $('#IsApprovedUniversity').next("span").removeClass('CustomRequiredCSS');
            //    $('#IsApprovedUniversity').next("span").addClass('HideValidMsg');
            //} else {
            //    if ($('input[name=IsApprovedUniversity]:checked').val() == "False") {
            //        $('#ApporvalLetterNo').val("0");
            //        $('#ApporvalLetterDate').val("");
            //        $('#ApporvalLetterNo').next("span").removeClass('CustomRequiredCSS');
            //        $('#ApporvalLetterNo').next("span").addClass('HideValidMsg');
            //        $('#ApporvalLetterDate').next("span").removeClass('CustomRequiredCSS');
            //        $('#ApporvalLetterDate').next("span").addClass('HideValidMsg');
            //    }
            //    else if ($('#ApporvalLetterNo').val() <= 0 || $('#ApporvalLetterDate').val() == '') {
            //        IsValid = false;

            //        $('#ApporvalLetterNo').next("span").addClass('CustomRequiredCSS');
            //        $('#ApporvalLetterNo').next("span").removeClass('HideValidMsg');
            //        $('#ApporvalLetterDate').next("span").addClass('CustomRequiredCSS');
            //        $('#ApporvalLetterDate').next("span").removeClass('HideValidMsg');
            //    }

            //}
            //if ($('#NoSectionPosts').val() <= 0) {
            //    IsValid = false;
            //    $('#NoSectionPosts').next("span").addClass('CustomRequiredCSS');
            //    $('#NoSectionPosts').next("span").removeClass('HideValidMsg');
            //}

            if (IsValid) {
                $('#formSaveCustomerDetail').submit();
            } else {
                alertNotification("Please Select all the Details.");
                return false;
            }
    }
    var LoadViewPopup = function (_CustomerDetailsId) {
        var _actionType = "VIEW"
        //var ID = e.target.id;
        $.ajax({
            type: "POST",
            data: { actionType: _actionType, CustomerId: _CustomerDetailsId },
            datatype: "JSON",
            url: window.OtherDetailsPopup,
            success: function (html) {
                SetModalTitle("View Customer Details")
                SetModalBody(html);
                HideLoadder();
                $('#formSaveCustomerDetail input[type=radio],input[type=text], select').prop("disabled", true);
                $('#save_results').css('display', 'none');
                $('#cancel_results').css('display', 'none');
                $('.bs-tooltip-top').css('display', 'none');
                ShowModal();
            },
            error: function () {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }
    var LoadEditPopup = function (_CustomerDetailsId) {
        var _actionType = "EDIT"
        //var ID = e.target.id;
        $.ajax({
            type: "POST",
            data: { actionType: _actionType, CustomerId: _CustomerDetailsId },
            datatype: "JSON",
            url: window.OtherDetailsPopup,
            success: function (html) {
                SetModalTitle("Edit Customer Details")
                SetModalBody(html);
                HideLoadder();
                $('.bs-tooltip-top').css('display', 'none');
                ShowModal();
            },
            error: function () {
                HideLoadder();
                alert(window.ErrorMsg);
            }
        })
    }
    return {
        Callfuntion: function (e) {
            SmallFuntion(e);
        },
        validationCall: function (e) {
            validation(e);
        },
        OnRelease: function (e) {
            onButtonRelease(Element);
        },
        FinalSave: function () {
            SaveData();
        },
        OnSearch: function () {
            SearchDetails();
        },
        Load: function () {

            $(document).on("click", ".btn-Add-CustomerDetails", function () {
                var _CustomerDetailsId = $(this).parents("tr:first").find("#CustomerId").val();
                BindPopup();
            });
            $(document).on("click", ".btn-Edit-CustomerDetails", function () {
                var _CustomerDetailsId = $(this).parents("tr:first").find("#CustomerId").val();
                LoadEditPopup(_CustomerDetailsId);
            });
            $(document).on("click", ".btn-view-CustomerDetails", function () {
                var _CustomerDetailsId = $(this).parents("tr:first").find("#CustomerId").val();
                LoadViewPopup(_CustomerDetailsId);
            });
        }
    }
}();