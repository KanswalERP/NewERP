﻿
using NtierMvc.Areas.MRM.Models;
using NtierMvc.Common;
using NtierMvc.Infrastructure;
using NtierMvc.Model;
using NtierMvc.Model.Account;
using NtierMvc.Model.Application;
using NtierMvc.Model.DesignEng;
using NtierMvc.Model.MRM;
using NtierMvc.Model.Stores;
using NtierMvc.Model.Vendor;
using NtierMvc.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
//using Excel = Microsoft.Office.Interop.Excel;

namespace NtierMvc.Areas.MRM.Controllers
{
    [SessionExpire]
    public class MRMController : Controller
    {
        private MRMManager objManager;
       
        BaseModel model;

        public MRMController()
        {
            model = new BaseModel();
            objManager = new MRMManager();
        }

        [HttpGet]
        public ActionResult MRMMaster()
        {
            //For BOM
            ViewBag.ListQuoteType = model.GetMasterTableStringList("DesignPRP", "Id", "QuoteNo", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListQuoteNo = model.GetDropDownList("DesignPRP", GeneralConstants.ListTypeD, "QuoteNo", "QuoteNo", "", "");
            ViewBag.ListSONo = model.GetDropDownList("DesignPRP", GeneralConstants.ListTypeD, "SONo", "SONo", "", "");
            ViewBag.ListVendorId = model.GetMasterTableStringList("DesignPRP", "Id", "VendorID", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListVendorName = model.GetMasterTableStringList("DesignPRP", "Id", "VendorID", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListProductGroup = model.GetMasterTableStringList("Master.Product", "Id", "ProductName", "", "", GeneralConstants.ListTypeD);
           //For vendor

            ViewBag.ListCustomerName = model.GetMasterTableStringList("Customer", "CustomerName", "CustomerName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListCustomerId = model.GetMasterTableStringList("Customer", "Id", "CustomerId", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListCountry = model.GetMasterTableStringList("Master.Country", "Id", "Country", "", "", GeneralConstants.ListTypeN);

            //Bill Monitoring
            ViewBag.ListType = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "QuoteType", "Property", GeneralConstants.ListTypeD);
            ViewBag.ListVendorNature = "";
            ViewBag.ListVendorName = "";
            ViewBag.ListBillDate = "";
            ViewBag.ListFunctionArea = "";
            //ViewBag.ListFinancialYear = model.GetMasterTableStringList("GateEntry", "FinancialYear", "FinancialYear", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListApprovalStatus = model.GetMasterTableStringList("ApproveStatus", "Id", "Status", "", "", GeneralConstants.ListTypeN);

            //For 
            ViewBag.ListVendorType = model.GetMasterTableStringList("Master.VendorType", "VendorId", "VendorName");
            ViewBag.ListSupplierId = model.GetMasterTableStringList("Clientele_Master", "Id", "VendorID", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListSupplierName = model.GetMasterTableStringList("Clientele_Master", "Id", "VendorName", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListTotalAmount = model.GetMasterTableStringList("BillMonitoring", "TotalAmount", "TotalAmount", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListRMCategory = model.GetMasterTableStringList("Master.Taxonomy", "DropDownID", "ObjectName", "PRCat", "Property", GeneralConstants.ListTypeD);
            List<DropDownEntity> newlst = model.GetMasterTableStringList("BillMonitoring", "ApprovedDate", "ApprovedDate", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListVendorName = model.GetMasterTableStringList("Vendor", "VendorName", "VendorName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListVendor = model.GetMasterTableStringList("Vendor", "VendorId", "VendorId", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListCountry = model.GetMasterTableStringList("Master.Country", "Id", "Country", "", "", GeneralConstants.ListTypeN);
          
            ViewBag.ListSupplierType = model.GetMasterTableStringList("Master.SupplierType", "Id", "SupplierName");

            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime dateTime;
            foreach (var item in newlst)
            {
                if (DateTime.TryParse(item.DataStringValueField, out dateTime))
                {
                    item.DataStringValueField = Convert.ToDateTime(item.DataStringValueField).ToString("MM-dd-yyyy");
                    item.DataTextField = Convert.ToDateTime(item.DataTextField).ToString("MM-dd-yyyy");
                }
            }

            ViewBag.ListDeliveryDate = newlst;
            var UserDetails = (UserEntity)Session["UserModel"];
            ViewBag.DeptName = UserDetails.DeptName;



            return View();
        }


        [HttpGet]
        public ActionResult PRPlanning()
        {
            PRDetailEntity prObj = new PRDetailEntity();
            return PartialView(prObj);
        }

        public JsonResult FetchPRDetailsList(string pageIndex, string pageSize, string SearchVendorTypeId = null, string SearchSupplierId = null, string SearchRMCategory = null, string SearchDeliveryDateFrom = null, string SearchDeliveryDateTo = null)
        {
            SearchVendorTypeId = SearchVendorTypeId == null ? string.Empty : SearchVendorTypeId;
            SearchSupplierId = SearchSupplierId == null ? string.Empty : SearchSupplierId;
            SearchRMCategory = SearchRMCategory == null ? string.Empty : SearchRMCategory;
            SearchDeliveryDateFrom = SearchDeliveryDateFrom == null ? string.Empty : SearchDeliveryDateFrom;
            SearchDeliveryDateTo = SearchDeliveryDateTo == null ? string.Empty : SearchDeliveryDateTo;

            var UserDetails = (UserEntity)Session["UserModel"];

            PRDetailEntityDetails prEntity = new PRDetailEntityDetails();
            prEntity = objManager.GetPRDetailsList(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), UserDetails.DeptName, SearchVendorTypeId, SearchSupplierId, SearchRMCategory, SearchDeliveryDateFrom, SearchDeliveryDateTo);
            return new JsonResult { Data = prEntity, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult GetStateDetail(string countryId)
        {
            List<GeographyEntity> StateList = new List<GeographyEntity>();
            StateList = model.GetStateDetail(countryId);
            return new JsonResult { Data = StateList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FetchVendorList(string SearchVendorType, string pageIndex, string pageSize, string SearchVendorName, string SearchVendorCountry, string SupplierType)
        {
            SearchVendorType = SearchVendorType == "-1" ? string.Empty : SearchVendorType;
            SearchVendorName = SearchVendorName == "-1" ? string.Empty : SearchVendorName;
            SearchVendorCountry = SearchVendorCountry == "-1" ? string.Empty : SearchVendorCountry;
            SupplierType = SupplierType == "-1" ? string.Empty : SupplierType;

            VendorEntityDetails  custDetail = objManager.GetVendorDetails(SearchVendorType,Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), SearchVendorName, SearchVendorCountry, SupplierType);
            return new JsonResult { Data = custDetail, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return custDetail.LstCusEnt;
        }

        public JsonResult GetEmpCertificates(string VendorId)
        {
             List<TableRecordsEntity> mdd = new List<TableRecordsEntity>();
            Dictionary<string, string> tt = new Dictionary<string, string>();
          
            var m = model.GetMasterTableStringList("Vendor", "Documents", "VendorId", VendorId, "VendorId");
            if(m.Count>0)
            {
                var md = m[1].DataStringValueField.Split(',');
                for (int i = 1; i <= md.Length; i++)
                {
                    tt.Add("RequiredColumn"+i, md[i-1]);
                }
            }
            return Json(tt);
        }
        [HttpPost]
        public ActionResult DeleteDocument(string VendorId,string Documents)
        {
            string result = "";
            DocumentModel Documents1 = new DocumentModel();
            Documents1.VendorId = VendorId;
            Documents1.Documents = Documents;

            try
            {

                if (!string.IsNullOrEmpty(Documents))
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["VendorDouments"]);

                    if (System.IO.File.Exists(path + Documents))
                        System.IO.File.Delete(path + Documents);

                    result = objManager.DeleteDocument(Documents1);
                }


            }
            catch (Exception Ex)
            {
                result = GeneralConstants.NotDeletedError + " " + Ex.Message;
            }


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult SaveVendorDetails(VendorEntity vdr)
        {
          ViewBag.ListFUNCTION_AREA = model.GetMasterTableList("Master.FunctionalArea", "Id", "FunctionArea");

            //For Image Upload
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Request.Files != null)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    actualFileName = file.FileName;
                    fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    int size = file.ContentLength;

                    try
                    {
                        //System.IO.Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings["VendorDouments"]));
                        //file.SaveAs(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["VendorDouments"]), fileName));
                        vdr.Documents +=   fileName + ',';
                        flag = true;
                    }
                    catch (Exception)
                    {
                        Message = " File upload failed! Please try again";
                    }

                }
            }
            //Image Upload End
            vdr.UserInitial = Session["UserName"].ToString();
            vdr.ipAddress = ERPContext.UserContext.IpAddress;
            vdr.UnitNo = Session["UserId"].ToString();
            if (vdr.Documents != "" && vdr.Documents != null)
                vdr.Documents= vdr.Documents.Substring(0, vdr.Documents.Length - 1);
            string result = objManager.SaveVendorDetails(vdr);

            //TempData["VendorName"] = vdr.CustomerName; if (string.IsNullOrEmpty(result) && (result != GeneralConstants.Inserted))
          
            //TempData["UserName"] = vdr.UserInitial;
            string data = string.Empty;
            if (!string.IsNullOrEmpty(result) && (result == GeneralConstants.Inserted || result == GeneralConstants.Updated))
            {
                data = GeneralConstants.SavedSuccess;
                if (Request.Files != null)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        actualFileName = file.FileName;
                        fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        int size = file.ContentLength;

                        try
                        {
                            //System.IO.Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings["VendorDouments"]));
                            //file.SaveAs(Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["VendorDouments"]), fileName));
                            vdr.Documents += fileName + ',';
                            flag = true;
                        }
                        catch (Exception)
                        {
                            Message = " File upload failed! Please try again";
                        }

                    }
                }
            }
            else
            {

                
                data = GeneralConstants.NotSavedError + result;
            }

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult DeleteVendorDetails(string VendorId)
        {
            if (ModelState.IsValid)
            {
                //string msgCode = objManager.DeleteVendorDetail(id);
                string msgCode = model.DeleteFromTable("Vendor", "VendorId", VendorId);

                if (msgCode == GeneralConstants.DeleteSuccess)
                {
                    //return RedirectToAction("Vendor");
                    return new JsonResult { Data = GeneralConstants.DeleteSuccess, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = GeneralConstants.NotDeletedError, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //Response.StatusCode = 444;
                    //Response.StatusDescription = "Not Saved";
                    //return null;
                }
            }
            else
            {
                Response.StatusCode = 444;
                Response.Status = "Not Saved";
                return null;
            }

        }

        [HttpPost]
        public ActionResult VendorPopup(string actionType, string VendorId = null)
        {
            VendorEntity cus=new VendorEntity();
            string countryId = "0";
            ViewBag.StatusList = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "Status", "Property", GeneralConstants.ListTypeN);

            ViewBag.ListState = model.GetStateDetail(countryId); //Given wrong CountryId to not get any value

            ViewBag.ListCountry = model.GetMasterTableStringList("Master.Country", "Id", "Country");

            ViewBag.ListVendorType = model.GetMasterTableStringList("Master.VendorType", "VendorId", "VendorName");
            ViewBag.ListSupplierType = "";
            string UnitNo = string.Empty;
            if (!string.IsNullOrEmpty(Session["UserId"].ToString()))
            {
                 UnitNo = Session["UserId"].ToString();
                cus.UnitNo = Session["UserId"].ToString();
            }

            if (actionType == "VIEW" || actionType == "EDIT")
            {
                if (!string.IsNullOrEmpty(VendorId))
                   cus.VendorId = VendorId;
                cus = objManager.VendorDetailsPopup(cus);
                ViewBag.ListSupplierType = model.GetMasterTableStringList("Master.SupplierType", "Id","SupplierName");
                //ViewBag.ListCountry = model.GetMasterTableStringList("Master.Country","Id", "Country",cus.CountryId,"Id");


            }
            if (actionType == "ADD")
            {
                cus = objManager.GetUserDetails(UnitNo);
                //eModel = objManager.AddCustomerDetailsPopup(eModel);
            }

            return base.PartialView("~/Areas/MRM/Views/MRM/VendorDetails.cshtml", cus);
        }
        [HttpPost]
        public ActionResult GetSupplierType(string VendorId)
        {
            try
            {
                List <DropDownEntity> ddl = model.GetDropDownList("Master.SupplierType", GeneralConstants.ListTypeD, "Id", "SupplierName", VendorId, "VendorId");

                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }
        }


        [HttpPost]
        public ActionResult BindVendorBillPopup(string actionType, string Id)
        {
            ViewBag.ListProductName = model.GetMasterTableStringList("Master.Product", "Id", "ProductName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListProductCode = model.GetMasterTableStringList("Master.Product", "Id", "ProductCode", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListPL = model.GetMasterTableStringList("Master.Product", "PL", "PL", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListProductNo = model.GetMasterTableStringList("Master.Product", "Id", "ProductNo", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListCasingSize = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "CasingSize", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListCasingPPF = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "Ppf", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListGrade = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "MatGrade", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListUom = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "Uom", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListOpenHoleSize = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "OpenHoleSize", "Property", GeneralConstants.ListTypeN);

            GateEntryEntity vMB = new GateEntryEntity();
            if (actionType == "VIEW" || actionType == "EDIT")
            {
                //if (!string.IsNullOrEmpty(Id))
                //bOM.Id = Convert.ToInt32(Id);
                //eOrder = objManager.BOMPopup(eOrder);
            }
            if (actionType == "ADD")
            {
                //eOrder = objManager.GetUserDetails(oeObj.UnitNo);
            }

            return base.PartialView("~/Areas/DesignEng/Views/DesignEng/_BOMDetails.cshtml");
        }

        public ActionResult PRDetailPopup(string actionType, string PRSetno)
        {
            ViewBag.ListSupplier = model.GetMasterTableStringList("ApprovedSupplier", "Id", "SupplierName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListCurrency = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "Currency", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListPriority = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "Priority", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListRMcat = model.GetMasterTableStringList("Master.RMCategory", "CategoryName", "CategoryName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListUOM = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "UOM", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListEndUse = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "EndUse", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListCostCenter = model.GetMasterTableStringList("Master.Department", "Id", "DeptName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListQuoteType = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "QuoteType", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListSupplyTerms = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "SupplyTerms", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListAcceptReject = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "AcceptReject", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListCommunicate = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "YesNo", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListPRCat = model.GetMasterTableStringList("Master.Taxonomy", "DropDownValue", "ObjectName", "PRCat", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListEndUseNo = "";
            ViewBag.ListPRRequestedOn = "";

            if (Session["CommonDetails"] != null)
            {
                var CommonDetails = (CommonDetailsEntity)Session["CommonDetails"];

                ViewBag.CompanyName = CommonDetails.CompanyName;
                ViewBag.Logo = CommonDetails.Logo;
                ViewBag.Address1 = CommonDetails.Address1;
                ViewBag.Address2 = CommonDetails.Address2;
                ViewBag.Address3 = CommonDetails.Address3;
                ViewBag.Phone = CommonDetails.Phone;
                ViewBag.Mobile = CommonDetails.Mobile;
                ViewBag.Website = CommonDetails.Website;
                ViewBag.Email1 = CommonDetails.Email1;
                ViewBag.Email2 = CommonDetails.Email2;
                ViewBag.Email3 = CommonDetails.Email3;
                ViewBag.Notation = CommonDetails.Notation;
            }

            var UserDetails = (UserEntity)Session["UserModel"];
            ViewBag.DeptName = UserDetails.DeptName;

            PRDetailEntity prObj = new PRDetailEntity();
            prObj.UserId = UserDetails.UserId;

            if (actionType == "VIEW" || actionType == "EDIT")
            {
                prObj.PRSetno = Convert.ToInt32(PRSetno);
                prObj = objManager.GetSavedPRDetailsPopup(prObj);

                if (prObj.SignStatus == "Entry")
                    prObj.ApprovePerson1Sign = UserDetails.SignImage;
                else if (prObj.SignStatus == "Approved1")
                    prObj.ApprovePerson2Sign = UserDetails.SignImage;
                else if (prObj.SignStatus == "Approved2")
                    ViewBag.PurchaseRequest = UserDetails.SignImage;


                ViewBag.ListPRRequestedOn = "";
            }
            else if (actionType == "ADD")
            {
                prObj = objManager.GetPRDetailsPopup(prObj);
                prObj.ReqFrom = UserDetails.FirstName + " " + UserDetails.LastName;
                prObj.PRdate = DateTime.Now.ToShortDateString();
                prObj.DeptName = UserDetails.DeptName;
                prObj.EntryPersonSign = UserDetails.SignImage;
            }


            return base.PartialView("~/Areas/MRM/Views/MRM/_PRDetailPopup.cshtml", prObj);
        }

        [HttpPost]
        public JsonResult ChangeEndUseNoForQuoteOrSoNo(string EndUse, string quoteType = null)
        {
            List<DropDownEntity> lstEndUseNo = new List<DropDownEntity>();
            lstEndUseNo = objManager.GetSONoQuoteNoList(EndUse, quoteType);

            return new JsonResult { Data = lstEndUseNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public JsonResult ChangeEndUseNoForNonPO(string EndUse)
        {
            List<DropDownEntity> lstEndUseNo = new List<DropDownEntity>();
            lstEndUseNo = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "NonPO", "Property", GeneralConstants.ListTypeD);

            return new JsonResult { Data = lstEndUseNo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult ChangeRMCat(string CatType)
        {
            List<DropDownEntity> lstRMCat = new List<DropDownEntity>();

            if (CatType == "RM")
                lstRMCat = model.GetDropDownList("Master.RMCategory", GeneralConstants.ListTypeD, "Id", "CategoryName", "", "");
            else if (CatType == "BOI")
                lstRMCat = model.GetDropDownList("Master.BuyOut", GeneralConstants.ListTypeD, "Id", "CategoryName", "", "");

            return new JsonResult { Data = lstRMCat, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult SavePRDetailsList(PRDetailEntity[] PRDetails)
        {
            model = new BaseModel();
            //ViewBag.ListVENDORTYPE = model.GetMasterTableList("Master.Vendor", "Id", "VendorType");
            //ViewBag.ListVENDOR_NATURE = model.GetMasterTableList("Master.Vendor", "Id", "VendorNature");
            //ViewBag.ListFUNCTION_AREA = model.GetMasterTableList("Master.FunctionalArea", "Id", "FunctionArea");

            try
            {
                if (PRDetails != null)
                {
                    BulkUploadEntity objBU = new BulkUploadEntity();
                    objBU.DataRecordTable = new DataTable();

                    List<PRDetailEntity> prList = new List<PRDetailEntity>();
                    List<PRDetailEntityBulkSave> itemListBulk = new List<PRDetailEntityBulkSave>();
                    prList = PRDetails.OfType<PRDetailEntity>().ToList();

                    var UserDetails = (UserEntity)Session["UserModel"];

                    foreach (var item in prList)
                    {
                        PRDetailEntityBulkSave newObj = new PRDetailEntityBulkSave();

                        //newObj.Id = item.Id;

                        newObj.QuoteType = item.QuoteType;
                        newObj.PRSetno = item.PRSetno;
                        newObj.PRno = item.PRno;
                        newObj.ReqFrom = item.ReqFrom;
                        newObj.ReqTo = item.ReqTo;
                        newObj.DeptName = item.DeptName;
                        newObj.PRcat = item.PRcat;
                        newObj.Currency = item.Currency;
                        newObj.Priority = item.Priority;
                        newObj.EndUse = item.EndUse;
                        newObj.QuoteType = item.QuoteType;
                        newObj.EndUseNo = item.EndUseNo;
                        newObj.CostCenter = item.CostCenter;
                        newObj.RMcat = item.RMcat;
                        newObj.RMdescription = item.RMdescription;
                        newObj.RMgrade = item.RMgrade;
                        newObj.RMHardness = item.RMHardness;
                        newObj.PSLlevel = item.PSLlevel;
                        newObj.UOM = item.UOM;
                        newObj.SN = item.SN;
                        newObj.OD = item.OD;
                        newObj.WT = item.WT;
                        newObj.Len = item.Len;
                        newObj.QtyReqd = item.QtyReqd;
                        newObj.QtyStock = item.QtyStock;
                        newObj.PRqty = item.PRqty;
                        newObj.UnitPrice = item.UnitPrice;
                        newObj.TotalPrice = item.TotalPrice;
                        newObj.DeliveryDate = item.DeliveryDate;
                        newObj.SupplyTerms = item.SupplyTerms;
                        newObj.DeliveryTerms = item.DeliveryTerms;
                        newObj.PaymentTerms = item.PaymentTerms;
                        newObj.Certificates = item.Certificates;
                        newObj.ApprovedSupplier1 = item.ApprovedSupplier1;
                        newObj.ApprovedSupplier2 = item.ApprovedSupplier2;
                        newObj.ApprovedReject = item.ApprovedReject;
                        newObj.Communicate = item.Communicate;
                        newObj.POno = item.POno;
                        newObj.ExpectedDeliveryDate = item.ExpectedDeliveryDate;
                        newObj.EntryDate = DateTime.Now;
                        newObj.SignStatus = item.SignStatus;
                        newObj.PRStatus = item.PRStatus;
                        newObj.EntryPerson = UserDetails.UserId.ToString();
                        newObj.TotalPRSetPrice = item.TotalPRSetPrice;
                        newObj.PRFavouredOn = item.PRFavouredOn;

                        if (item.DeptName == "Stores")
                        {
                            newObj.ApproveDate1 = DateTime.Now;
                            newObj.SignStatus = "Approved1";
                            newObj.ApprovePerson1 = UserDetails.UserId.ToString();
                        }
                        itemListBulk.Add(newObj);
                    }

                    string result = string.Empty;
                    ExtensionMethods lsttodt = new ExtensionMethods();
                    objBU.DataRecordTable = lsttodt.ToDataTable(itemListBulk);
                    objBU.IdentityNo = PRDetails[0].PRSetno;
                    result = objManager.SavePRDetailsList(objBU);

                    string data = string.Empty;
                    if (!string.IsNullOrEmpty(result) && (result == GeneralConstants.Inserted || result == GeneralConstants.Updated))
                    {
                        //Payment Gateway
                        data = GeneralConstants.SavedSuccess;
                    }
                    else
                    {
                        data = GeneralConstants.NotSavedError + " Reason: " + result;
                    }

                    return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return Json("Unable to save Item Details! Please Provide correct information", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Unable to save your Item Details! Please try again later.", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateApproveReject(string PRSetno, string SignStatus, string PRFavouredOn, string PRStatus)
        {
            string msgCode = string.Empty;
            var UserDetails = (UserEntity)Session["UserModel"];
            msgCode = objManager.UpdateApproveReject(PRSetno, SignStatus, UserDetails.UserId.ToString(), PRFavouredOn, PRStatus);

            return new JsonResult { Data = msgCode, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetPRTableDetails(string PRSetno)
        {
            List<PRDetailEntity> prObjList = new List<PRDetailEntity>();
            prObjList = objManager.GetPRTableDetails(PRSetno);

            return new JsonResult { Data = prObjList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult SavePurchaseDetails(string PRSetno, string Communicate, string PONo, string ExpectedDeliveryDate)
        {
            string msgCode = string.Empty;
            var UserDetails = (UserEntity)Session["UserModel"];
            msgCode = objManager.SavePurchaseDetails(PRSetno, Communicate, PONo, ExpectedDeliveryDate, UserDetails.UserId.ToString());

            return new JsonResult { Data = msgCode, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult CreateDownloadDocument(string PRSetNo)
        {
            string FileName = "";
            try
            {

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                // open the template in Edit mode
                string path = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PRExcel"]);
                FileName = Path.GetFileName(path);
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = excelApp.Workbooks.Open(Filename: @path, Editable: true);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Sheets["Sheet1"];

                //string[] DataColumns = { "PRSetNo" };
                //string[] DataParam = { PRSetNo };
                //string[] RequiredColumn = { };

                //List<string> RC = new List<string>();
                //RC.Add("PRno");
                //RC.Add("PRdate");
                //RC.Add("DeliveryDate");
                //RC.Add("ApprovedSupplier1");
                //RC.Add("ApprovedSupplier2");
                //RC.Add("PRStatus");
                //RC.Add("Communicate");
                //RC.Add("POno");
                //RC.Add("ExpectedDeliveryDate");

                //RequiredColumn = RC.ToArray();

                //DataTable resultData = model.GetDataTableForDocument(GeneralConstants.ListTypeD, "PurchaseRequest", DataColumns, DataParam, RequiredColumn);

                //RC.Clear();
                //RC.Add("SN");
                //RC.Add("RMDescription");
                //RC.Add("OD");
                //RC.Add("WT");
                //RC.Add("Len");
                //RC.Add("QtyReqd");
                //RC.Add("QtyStock");
                //RC.Add("PRQty");

                DataTable resultData = objManager.GetPRDataForDocument(PRSetNo);
                DataTable resultList = objManager.GetPRListForDocument(PRSetNo);

                //Getting Single Fields
                xlWorkbook.Worksheets[1].Cells.Replace("#PRno", resultData.Rows[0]["PRno"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#PRdate", resultData.Rows[0]["PRdate"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#DeliveryDate", resultData.Rows[0]["DeliveryDate"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ApprovedSupplier1", resultData.Rows[0]["ApprovedSupplier1"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ApprovedSupplier2", resultData.Rows[0]["ApprovedSupplier2"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#PRStatus", resultData.Rows[0]["PRStatus"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#Communicate", resultData.Rows[0]["Communicate"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#POno", resultData.Rows[0]["POno"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ExpectedDeliveryDate", resultData.Rows[0]["ExpectedDeliveryDate"]);

                ////////////////For Image////////////////////
                #region Image
                Microsoft.Office.Interop.Excel.Range cells = xlWorkbook.Worksheets[1].Cells;

                //ReqBy
                Microsoft.Office.Interop.Excel.Range matchReqBy = cells.Find("#RequestedBySign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#RequestedBySign", "");

                string matchAdd = matchReqBy != null ? matchReqBy.Address : null;
                if (matchReqBy != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchReqBy.Row, matchReqBy.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["RequestedBySign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }

                //Store
                Microsoft.Office.Interop.Excel.Range matchStore = cells.Find("#StoreSign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#StoreSign", "");

                string matchStoreAdd = matchStore != null ? matchStore.Address : null;
                if (matchStoreAdd != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchStore.Row, matchStore.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["StoreSign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }

                //Approver
                Microsoft.Office.Interop.Excel.Range matchApprove = cells.Find("#ApproverSign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#ApproverSign", "");

                string matchApproveAdd = matchApprove != null ? matchApprove.Address : null;
                if (matchApproveAdd != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchApprove.Row, matchApprove.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["ApproverSign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }

                //Purchase
                Microsoft.Office.Interop.Excel.Range matchPurchase = cells.Find("#PurchaseSign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#PurchaseSign", "");

                string matchPurchaseAdd = matchPurchase != null ? matchPurchase.Address : null;
                if (matchPurchaseAdd != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchPurchase.Row, matchPurchase.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["PurchaseSign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }
                #endregion
                ////////////////For Image////////////////////


                Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[10, 1];
                Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[(resultList.Rows.Count - 2) + 10, resultList.Columns.Count];
                Microsoft.Office.Interop.Excel.Range range = ws.get_Range(c1, c2);
                range.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);

                object[,] arr = new object[resultList.Rows.Count, resultList.Columns.Count];
                for (int r = 0; r <= resultList.Rows.Count - 1; r++)
                {
                    DataRow dr = resultList.Rows[r];
                    for (int c = 0; c < resultList.Columns.Count; c++)
                    {
                        arr[r, c] = dr[c];
                    }
                }


                Microsoft.Office.Interop.Excel.Range c3 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[9, 1];
                Microsoft.Office.Interop.Excel.Range c4 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[(resultList.Rows.Count - 1) + 9, resultList.Columns.Count];
                Microsoft.Office.Interop.Excel.Range range1 = ws.get_Range(c3, c4);
                range1.Value = arr;

                string fullPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"]), FileName);
                xlWorkbook.SaveAs(fullPath);
                xlWorkbook.Close();
                excelApp.Quit();

                Download(FileName);
            }
            catch (Exception ex)
            {
                var response = ex.Message;
            }

            return Json(new { fileName = FileName, errorMessage = "Error While Generating Excel. Contact Support." });
        }

        private void Download(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"]), fileName);
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = true;

            if (System.IO.File.Exists(fullPath))
            {
                ////Get the temp folder and file path in server
                Microsoft.Office.Interop.Excel.Workbooks books = excelApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook sheet = books.Open(fullPath, 0, true, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, Microsoft.Office.Interop.Excel.XlCorruptLoad.xlNormalLoad);
                System.IO.File.Delete(fullPath);
                //return Json(new { data = "", errorMessage = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        //Do Not Delete Commented Text Below
        //public ActionResult Download(string fileName)
        //{
        //    string fullPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"]), fileName);

        //    if (System.IO.File.Exists(fullPath))
        //    {
        //        ////Get the temp folder and file path in server
        //        byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
        //        System.IO.File.Delete(fullPath);
        //        return File(fileByteArray, "application/vnd.ms-excel", fileName);

        //    }

        //    else
        //        return Json(new { data = "", errorMessage = "Error While Generating Excel. Contact Support." }, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public ActionResult POPlanning()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult PODetailsPopup(string actionType, string POSetNo)
        {
            var UserDetails = (UserEntity)Session["UserModel"];

            ViewBag.ListPRno = objManager.GetPRNoList(UserDetails.DeptName);
            ViewBag.ListModeOfTransport = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "Transport", "Property", GeneralConstants.ListTypeD);
            ViewBag.ListPRCat = model.GetMasterTableStringList("Master.Taxonomy", "DropDownValue", "ObjectName", "PRCat", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListCostCenter = model.GetMasterTableStringList("Master.Department", "Id", "DeptName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListSupplyTerms = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "SupplyTerms", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListSupplyType = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "SupplyType", "Property", GeneralConstants.ListTypeN);

            List<DropDownEntity> ListPO = new List<DropDownEntity>();
            DropDownEntity ddl = new DropDownEntity();
            ddl.DataTextField = "Select"; ddl.DataStringValueField = "";
            ListPO.Add(ddl);

            DropDownEntity ddl1 = new DropDownEntity();
            ddl1.DataTextField = ddl1.DataStringValueField = "Rev01";
            ListPO.Add(ddl1);

            DropDownEntity ddl2 = new DropDownEntity();
            ddl2.DataTextField = ddl2.DataStringValueField = "Rev02";
            ListPO.Add(ddl2);

            DropDownEntity ddl3 = new DropDownEntity();
            ddl3.DataTextField = ddl3.DataStringValueField = "Rev03";
            ListPO.Add(ddl3);

            ViewBag.ListPORevNo = ListPO;

            if (Session["CommonDetails"] != null)
            {
                var CommonDetails = (CommonDetailsEntity)Session["CommonDetails"];

                ViewBag.CompanyName = CommonDetails.CompanyName;
                ViewBag.Logo = CommonDetails.Logo;
                ViewBag.Address1 = CommonDetails.Address1;
                ViewBag.Address2 = CommonDetails.Address2;
                ViewBag.Address3 = CommonDetails.Address3;
                ViewBag.Phone = CommonDetails.Phone;
                ViewBag.Mobile = CommonDetails.Mobile;
                ViewBag.Website = CommonDetails.Website;
                ViewBag.Email1 = CommonDetails.Email1;
                ViewBag.Email2 = CommonDetails.Email2;
                ViewBag.Email3 = CommonDetails.Email3;
                ViewBag.Notation = CommonDetails.Notation;
            }

            ViewBag.DeptName = UserDetails.DeptName;

            PODetailEntity poObj = new PODetailEntity();
            poObj.UserId = UserDetails.UserId;

            if (actionType == "VIEW" || actionType == "EDIT")
            {
                poObj.POSetno = POSetNo;
                poObj = objManager.GetSavedPODetails(POSetNo);

                //if (poObj.SignStatus == "Entry")
                //    poObj.ApprovePerson1Sign = UserDetails.SignImage;
                //else if (poObj.SignStatus == "Approved1")
                //    poObj.ApprovePerson2Sign = UserDetails.SignImage;
                //else if (poObj.SignStatus == "Approved2")
                //    ViewBag.PurchaseRequest = UserDetails.SignImage;

            }
            else if (actionType == "ADD")
            {
                poObj.CompShortName = ConfigurationManager.AppSettings["CompanyShortName"];
                poObj = objManager.GetPODetailsForPopup(poObj);
                poObj.POdate = DateTime.Now.ToShortDateString();
            }

            return base.PartialView("~/Areas/MRM/Views/MRM/_PODetailsPopup.cshtml", poObj);
        }

        [HttpPost]
        public ActionResult SavePODetailsList(PODetailEntity[] PODetails)
        {
            model = new BaseModel();

            try
            {
                if (PODetails != null)
                {
                    BulkUploadEntity objBU = new BulkUploadEntity();
                    objBU.DataRecordTable = new DataTable();

                    List<PODetailEntity> prList = new List<PODetailEntity>();
                    List<PODetailEntityBulkSave> itemListBulk = new List<PODetailEntityBulkSave>();
                    prList = PODetails.OfType<PODetailEntity>().ToList();

                    var UserDetails = (UserEntity)Session["UserModel"];

                    foreach (var item in prList)
                    {
                        PODetailEntityBulkSave newObj = new PODetailEntityBulkSave();

                        newObj.PRSetno = item.PRSetno;
                        newObj.PONo = item.PONo;
                        newObj.POSetno = item.POSetno;
                        newObj.POdate = item.POdate;
                        newObj.SN = item.SN;
                        newObj.RMdescription = item.RMdescription;
                        newObj.RMgrade = item.RMgrade;
                        newObj.RMHardness = item.RMHardness;
                        newObj.PSLlevel = item.PSLlevel;
                        newObj.QtyReqd = item.QtyReqd;
                        newObj.QtyStock = item.QtyStock;
                        newObj.PRqty = item.PRqty;
                        newObj.UnitPrice = item.UnitPrice;
                        newObj.TotalPrice = item.TotalPrice;
                        newObj.Discount = item.Discount;
                        newObj.FinalPrice = item.FinalPrice;
                        newObj.WorkNo = item.WorkNo;
                        newObj.DeliveryDate = item.DeliveryDate;
                        newObj.SupplyType = item.SupplyType;
                        newObj.ItemCategory = item.ItemCategory;
                        newObj.POValidity = item.POValidity;
                        newObj.GeneralCondition = item.GeneralCondition;
                        newObj.POQMSRequirement = item.POQMSRequirement;
                        newObj.POQuality = item.POQuality;
                        newObj.POPackForward = item.POPackForward;
                        newObj.ModeOfPayment = item.ModeOfPayment;
                        newObj.PaymentTerms = item.PaymentTerms;
                        newObj.ModeOfTransport = item.ModeOfTransport;
                        newObj.AnyOtherRequirements = item.AnyOtherRequirements;

                        newObj.LotName = item.LotName;
                        newObj.LotDate = item.LotDate;
                        newObj.LotQty = item.LotQty;
                        newObj.CostCenter = item.CostCenter;

                        newObj.PORevNo = item.PORevNo == "" ? "" : item.PONo + item.PORevNo;

                        newObj.TotalPRSetPrice = item.TotalPRSetPrice;

                        itemListBulk.Add(newObj);
                    }

                    string result = string.Empty;
                    ExtensionMethods lsttodt = new ExtensionMethods();
                    objBU.DataRecordTable = lsttodt.ToDataTable(itemListBulk);
                    objBU.IdentityNo = PODetails[0].PRSetno;
                    result = objManager.SavePODetailsList(objBU);

                    string data = string.Empty;
                    if (!string.IsNullOrEmpty(result) && (result == GeneralConstants.Inserted || result == GeneralConstants.Updated))
                    {
                        data = GeneralConstants.SavedSuccess;
                    }
                    else
                    {
                        data = GeneralConstants.NotSavedError + " Reason: " + result;
                    }

                    return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return Json("Unable to save Item Details! Please Provide correct information", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Unable to save your Item Details! Please try again later.", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FetchPODetailsList(string pageIndex, string pageSize, string SearchVendorTypeId = null, string SearchSupplierId = null, string SearchRMCategory = null, string SearchDeliveryDateFrom = null, string SearchDeliveryDateTo = null)
        {
            SearchVendorTypeId = SearchVendorTypeId == null ? string.Empty : SearchVendorTypeId;
            SearchSupplierId = SearchSupplierId == null ? string.Empty : SearchSupplierId;
            SearchRMCategory = SearchRMCategory == null ? string.Empty : SearchRMCategory;
            SearchDeliveryDateFrom = SearchDeliveryDateFrom == null ? string.Empty : SearchDeliveryDateFrom;
            SearchDeliveryDateTo = SearchDeliveryDateTo == null ? string.Empty : SearchDeliveryDateTo;

            PODetailEntityDetails prEntity = new PODetailEntityDetails();
            prEntity = objManager.GetPODetailsList(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), SearchVendorTypeId, SearchSupplierId, SearchRMCategory, SearchDeliveryDateFrom, SearchDeliveryDateTo);
            return new JsonResult { Data = prEntity, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult CreateDocumentForPO(string PRSetNo)
        {
            string FileName = "";
            try
            {
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                // open the template in Edit mode
                string path = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["POExcelFile"]);
                FileName = Path.GetFileName(path);
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = excelApp.Workbooks.Open(Filename: @path, Editable: true);
                Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Sheets["Sheet1"];

                DataTable resultData = objManager.GetPODetailForDocument(PRSetNo);
                DataTable resultList = objManager.GetPOListDataForDocument(PRSetNo);

                //Getting Single Fields
                xlWorkbook.Worksheets[1].Cells.Replace("#PRno", resultData.Rows[0]["PRno"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#PONo", resultData.Rows[0]["PONo"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#POSetno", resultData.Rows[0]["POSetno"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#POdate", resultData.Rows[0]["POdate"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#PRdate", resultData.Rows[0]["PRdate"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#WorkNo", resultData.Rows[0]["WorkNo"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#QuoteNo", resultData.Rows[0]["QuoteNo"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#QuoteDate", resultData.Rows[0]["QuoteDate"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#DeliveryDate", resultData.Rows[0]["DeliveryDate"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#POValidity", resultData.Rows[0]["POValidity"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#PORevNo", resultData.Rows[0]["PORevNo"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ItemCategory", resultData.Rows[0]["ItemCategory"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#Curr", resultData.Rows[0]["Currency"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#EntryPerson", resultData.Rows[0]["EntryPerson"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ApprovePerson1", resultData.Rows[0]["ApprovePerson1"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ApprovePerson2", resultData.Rows[0]["ApprovePerson2"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#Supp1Name", resultData.Rows[0]["Supp1Name"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#Supp1Address", resultData.Rows[0]["Supp1Address"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#Supp1ContactPerson", resultData.Rows[0]["Supp1ContactPerson"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#Supp1ContactNo", resultData.Rows[0]["Supp1ContactNo"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#Supp1Email", resultData.Rows[0]["Supp1Email"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#website", resultData.Rows[0]["Website"]);

                xlWorkbook.Worksheets[1].Cells.Replace("#GeneralCondition", resultData.Rows[0]["GeneralCondition"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#POQMSRequirement", resultData.Rows[0]["POQMSRequirement"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#POQuality", resultData.Rows[0]["POQuality"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#POPackForward", resultData.Rows[0]["POPackForward"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ModeOfPayment", resultData.Rows[0]["ModeOfPayment"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#PaymentTerms", resultData.Rows[0]["PaymentTerms"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#ModeOfTransport", resultData.Rows[0]["ModeOfTransport"]);
                xlWorkbook.Worksheets[1].Cells.Replace("#AnyOtherRequirements", resultData.Rows[0]["AnyOtherRequirements"]);

                ////////////////For Image////////////////////
                #region Image
                Microsoft.Office.Interop.Excel.Range cells = xlWorkbook.Worksheets[1].Cells;

                //ReqBy
                Microsoft.Office.Interop.Excel.Range matchReqBy = cells.Find("#RequestedBySign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#RequestedBySign", "");

                string matchAdd = matchReqBy != null ? matchReqBy.Address : null;
                if (matchReqBy != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchReqBy.Row, matchReqBy.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["RequestedBySign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }

                //Store
                Microsoft.Office.Interop.Excel.Range matchStore = cells.Find("#StoreSign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#StoreSign", "");

                string matchStoreAdd = matchStore != null ? matchStore.Address : null;
                if (matchStoreAdd != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchStore.Row, matchStore.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["StoreSign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }

                //Approver
                Microsoft.Office.Interop.Excel.Range matchApprove = cells.Find("#ApproverSign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#ApproverSign", "");

                string matchApproveAdd = matchApprove != null ? matchApprove.Address : null;
                if (matchApproveAdd != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchApprove.Row, matchApprove.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["ApproverSign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }

                //Purchase
                Microsoft.Office.Interop.Excel.Range matchPurchase = cells.Find("#PurchaseSign", LookAt: Microsoft.Office.Interop.Excel.XlLookAt.xlPart) as Microsoft.Office.Interop.Excel.Range;
                xlWorkbook.Worksheets[1].Cells.Replace("#PurchaseSign", "");

                string matchPurchaseAdd = matchPurchase != null ? matchPurchase.Address : null;
                if (matchPurchaseAdd != null)
                {
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)ws.Cells[matchPurchase.Row, matchPurchase.Column];
                    float Left = (float)((double)oRange.Left);
                    float Top = (float)((double)oRange.Top);
                    const float ImageSize = 40;
                    string filePath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["SignImagePath"]), resultData.Rows[0]["PurchaseSign"].ToString());
                    if (System.IO.File.Exists(filePath))
                        ws.Shapes.AddPicture(filePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, Left, Top, ImageSize, ImageSize);
                }
                #endregion
                ////////////////For Image////////////////////


                Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[12, 1];
                Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[(resultList.Rows.Count - 2) + 12, resultList.Columns.Count];
                Microsoft.Office.Interop.Excel.Range range = ws.get_Range(c1, c2);
                range.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);

                double CubMtr = 0;
                object[,] arr = new object[resultList.Rows.Count, resultList.Columns.Count];
                for (int r = 0; r <= resultList.Rows.Count - 1; r++)
                {
                    DataRow dr = resultList.Rows[r];
                    for (int c = 0; c < resultList.Columns.Count; c++)
                    {
                        arr[r, c] = dr[c];
                    }
                    CubMtr = CubMtr + Convert.ToDouble(dr[7]);
                }

                xlWorkbook.Worksheets[1].Cells.Replace("#TotalValue", CubMtr);
                string TotalWords = model.AmountToWordINR(CubMtr);
                xlWorkbook.Worksheets[1].Cells.Replace("#ValueInWords", TotalWords);

                Microsoft.Office.Interop.Excel.Range c3 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[12, 1];
                Microsoft.Office.Interop.Excel.Range c4 = (Microsoft.Office.Interop.Excel.Range)ws.Cells[(resultList.Rows.Count - 1) + 12, resultList.Columns.Count];
                Microsoft.Office.Interop.Excel.Range range1 = ws.get_Range(c3, c4);
                range1.Value = arr;

                string fullPath = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["TempFolder"]), FileName);
                xlWorkbook.SaveAs(fullPath);
                xlWorkbook.Close();
                excelApp.Quit();

                Download(FileName);
            }
            catch (Exception ex)
            {
                var response = ex.Message;
            }

            return Json(new { fileName = FileName, errorMessage = "Error While Generating Excel. Contact Support." });
        }

        [HttpGet]
        public JsonResult GetPOTableDetails(string POSetno)
        {
            List<PODetailEntity> prObjList = new List<PODetailEntity>();
            prObjList = objManager.GetPOTableDetails(POSetno);

            return new JsonResult { Data = prObjList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult DeleteVendorPODetails(string id)
        {
            if (ModelState.IsValid)
            {
                string msgCode = model.DeleteFromTable("RMPO", "POSetNo", id);
                if (msgCode != "")
                {
                    //return RedirectToAction("Technical");
                    return new JsonResult { Data = GeneralConstants.DeleteSuccess, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = GeneralConstants.NotDeletedError, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else
            {
                Response.StatusCode = 444;
                Response.Status = "Not Saved";
                return null;
            }

        }

        public JsonResult GetSuppliers(string VendorTypeId)
        {
            try
            {
                List<DropDownEntity> ddl = model.GetDropDownList("Clientele_Master", GeneralConstants.ListTypeD, "Id", "VendorId", VendorTypeId, "VendorTypeId");
                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }

        }

        public JsonResult GetRMCategories(string SupplierId)
        {
            try
            {
                List<DropDownEntity> ddl = objManager.GetRMCategories(SupplierId);
                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }

        }

        public JsonResult GetDeliveryDates(string RMCategory)
        {
            try
            {
                List<DropDownEntity> ddl = objManager.GetDeliveryDates(RMCategory);

                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime dateTime;
                foreach (var item in ddl)
                {
                    if (DateTime.TryParse(item.DataStringValueField, out dateTime))
                    {
                        item.DataStringValueField = Convert.ToDateTime(item.DataStringValueField).ToString("MM-dd-yyyy");
                        item.DataTextField = Convert.ToDateTime(item.DataTextField).ToString("MM-dd-yyyy");
                    }
                }

                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }
        }

        [HttpGet]
        public ActionResult PartialBOM()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult BOMPopup(string actionType, string BOMId)
        {
            ViewBag.ListProductName = model.GetMasterTableStringList("Master.Product", "Id", "ProductName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListProductCode = model.GetMasterTableStringList("Master.Product", "Id", "ProductCode", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListPL = model.GetMasterTableStringList("Master.Product", "PL", "PL", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListProductNo = model.GetMasterTableStringList("Master.Product", "Id", "ProductNo", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListCasingSize = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "CasingSize", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListCasingPPF = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "Ppf", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListGrade = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "MatGrade", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListUom = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "Uom", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListOpenHoleSize = model.GetMasterTableStringList("Master.Taxonomy", "dropdownId", "dropdownvalue", "OpenHoleSize", "Property", GeneralConstants.ListTypeN);

            BOMEntity bOM = new BOMEntity();
            if (actionType == "VIEW" || actionType == "EDIT")
            {
                if (!string.IsNullOrEmpty(BOMId))
                    bOM.Id = Convert.ToInt32(BOMId);
                //eOrder = objManager.BOMPopup(eOrder);
            }
            if (actionType == "ADD")
            {
                //eOrder = objManager.GetUserDetails(PRPObj.UnitNo);
            }

            return base.PartialView("~/Areas/DesignEng/Views/DesignEng/_BOMDetails.cshtml", bOM);
        }

        [HttpGet]
        public ActionResult PartialMRMBillMonitoring()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MRMBillMonitoringPopUp(string actionType, string BMno=null)
        {
            ViewBag.ListType = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "SupplyType", "Property", GeneralConstants.ListTypeD);
            ViewBag.ListVendorNature = model.GetMasterTableStringList("Master.Vendor", "Id", "VendorNature", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListVendorId = model.GetMasterTableStringList("Clientele_Master", "Id", "VendorId", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListEndUse = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "EndUse", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListEndUseNo = "";
            ViewBag.ListUom = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "Uom", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListCurrency = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "Currency", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListSupplyTerms = model.GetMasterTableStringList("Master.Taxonomy", "DropDownId", "DropDownValue", "SupplyTerms", "Property", GeneralConstants.ListTypeN);
            ViewBag.ListDepartment = model.GetMasterTableStringList("Master.Department", "Id", "DeptName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListSCCNO = model.GetMasterTableStringList("SCCNO", "Id", "Heading", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListCostCenter = model.GetMasterTableStringList("Master.Department", "Id", "DeptName", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListApproveStatus = model.GetMasterTableStringList("ApproveStatus", "Id", "Status", "", "", GeneralConstants.ListTypeN);
            ViewBag.ListGateControlNo = model.GetMasterTableStringList("GateEntry", "GateNo", "GateControlNo", "", "", GeneralConstants.ListTypeD);

            MRMBillMonitoringEntity vmbE = new MRMBillMonitoringEntity();

            if (actionType == "VIEW" || actionType == "EDIT")
            {
                if (!string.IsNullOrEmpty(BMno))
                    vmbE.BMno = Convert.ToInt32(BMno);

                vmbE = objManager.GetBillDetailsPopup(BMno);

            }
            if (actionType == "ADD")
            {
                vmbE.BMno = objManager.GetBillMonitoringNo();

            }

            return base.PartialView("~/Areas/MRM/Views/MRM/_MRMBillMonitoringPopUp.cshtml", vmbE);
        }

        public JsonResult GetMRMDetailForGateControlNo(string GateControlNo, string BMno=null)
        {
            MRMBillMonitoringEntityDetails bmObj = new MRMBillMonitoringEntityDetails();
            bmObj = objManager.GetMRMDetailForGateControlNo(GateControlNo, BMno);

            return new JsonResult { Data = bmObj.lstMRMEntity, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult SaveButtonMRMBillMonitoring(MRMBillMonitoringBulkEntity[] MRMBillDetails)
        {
            string data = string.Empty;
            var UserDetails = (UserEntity)Session["UserModel"];
            try
            {
                if (MRMBillDetails != null)
                {
                    BulkUploadEntity objBU = new BulkUploadEntity();
                    objBU.DataRecordTable = new DataTable();
                    List<MRMBillMonitoringBulkEntity> itemListBulk = new List<MRMBillMonitoringBulkEntity>();
                    itemListBulk = MRMBillDetails.OfType<MRMBillMonitoringBulkEntity>().ToList();

                    foreach (var Item in itemListBulk)
                    {
                        Item.ApprovedBy = UserDetails.UserId.ToString();
                        //Item.ApprovedDate = Convert.ToString(DateTime.Now.ToString("dd-MM-yyyy"));
                        //Item.GSTAmount = Convert.ToString(Math.Round(((Decimal.Parse(Item.TotalPrice) - Decimal.Parse(Item.GSTPercent)) * 1000) / 10) / 100);
                    }
                    string result = string.Empty;
                    ExtensionMethods lsttodt = new ExtensionMethods();
                    objBU.DataRecordTable = lsttodt.ToDataTable(itemListBulk);
                    objBU.IdentityNo = MRMBillDetails[0].BMno;
                    objBU.DestinationTable = "BillMonitoring";
                    objBU.IdentityNoColumnName = "BMno";
                    result = model.SaveBulkEntryDetails(objBU);

                    if (!string.IsNullOrEmpty(result) && (result == GeneralConstants.Inserted || result == GeneralConstants.Updated))
                    {
                        data = GeneralConstants.SavedSuccess;
                    }
                    else
                    {
                        data = GeneralConstants.NotSavedError + " Reason: " + result;
                    }

                    return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                return Json("Unable to save Item Details! Please Provide correct information", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Unable to save your Item Details! Please try again later.", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FetchBillMonitoringList(string pageIndex, string pageSize, string MRMSearchVendorTypeId = null, string MRMSearchSupplierId = null, string MRMSearchSupplierName = null, string MRMSearchApprovedDate = null, string MRMSearchTotalAmount = null)
        {
            MRMSearchVendorTypeId = MRMSearchVendorTypeId == null ? string.Empty : MRMSearchVendorTypeId;
            MRMSearchSupplierId = MRMSearchSupplierId == null ? string.Empty : MRMSearchSupplierId;
            MRMSearchSupplierName = MRMSearchSupplierName == null ? string.Empty : MRMSearchSupplierName;
            MRMSearchApprovedDate = MRMSearchApprovedDate == null ? string.Empty : MRMSearchApprovedDate;
            MRMSearchTotalAmount = MRMSearchTotalAmount == null ? string.Empty : MRMSearchTotalAmount;

            MRMBillMonitoringEntityDetails bmD = new MRMBillMonitoringEntityDetails();

            bmD = objManager.FetchBillMonitoringList(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), MRMSearchVendorTypeId, MRMSearchSupplierId , MRMSearchSupplierName , MRMSearchApprovedDate , MRMSearchTotalAmount);

            return new JsonResult { Data = bmD, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return custDetail.LstCusEnt;
        }

        //public JsonResult FetchVendorList(string pageIndex, string pageSize, string SearchCustomerName, string SearchCustomerID)
        //{
        //    SearchCustomerName = SearchCustomerName == "-1" ? string.Empty : SearchCustomerName;
        //    SearchCustomerID = SearchCustomerID == "-1" ? string.Empty : SearchCustomerID;
        //    VendorEntityDetails venDetail = new VendorEntityDetails(); ;
        //    venDetail = objManager.GetVendorDetails(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), SearchCustomerName, SearchCustomerID);
        //    return new JsonResult { Data = venDetail, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    //return custDetail.LstCusEnt;
        //}
        public JsonResult GetSupplierName(string SupplierId)
        {
            try
            {
                List<DropDownEntity> ddl = model.GetDropDownList("Clientele_Master", GeneralConstants.ListTypeD, "Id", "VendorName", SupplierId, "Id");
                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }

        }

        public JsonResult GetApprovedDate(string SupplierId)
        {
            try
            {
                List<DropDownEntity> ddl = model.GetDropDownList("BillMonitoring", GeneralConstants.ListTypeD, "Id", "ApprovedDate", SupplierId, "VendorId");
                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }

        }

        [HttpGet]
        public ActionResult PartialVendor()
        {
            return PartialView();
        }


        //[HttpGet]
        //public ActionResult PartialVendor()
        //{
        //    PRDetailEntity prObj = new PRDetailEntity();
        //    return PartialView(prObj);
        //}


    }
}