using NtierMvc.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NtierMvc.Common;
using NtierMvc.DataAccess.Common;

namespace NtierMvc.DataAccess.Pool
{
    public partial class Repository : IDisposable
    {

        #region Class Methods

        public DataTable GetProductList(string productLine)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@productLine", productLine);
            var spName = ConfigurationManager.AppSettings["GetProductListDetails"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetProductNumber(string productNameId, int productType)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@productNameId", productNameId);
            parms.Add("@productType", productType);
            var spName = ConfigurationManager.AppSettings["GetProductNumber"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataSet GetQuoteRegList(int pageIndex, int pageSize, string SearchQuoteType = null, string SearchQuoteCustomerID = null, string SearchSubject = null, string SearchDeliveryTerms = null)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("@pageIndex", pageIndex);
            parms.Add("@pageSize", pageSize);
            parms.Add("@SearchDeliveryTerms", SearchDeliveryTerms);
            parms.Add("@SearchSubject", SearchSubject);
            parms.Add("@SearchQuoteCustomerID", SearchQuoteCustomerID);
            parms.Add("@SearchQuoteType", SearchQuoteType);
            string spName = ConfigurationManager.AppSettings["GetQuoteRegistration"];
            return _dbAccess.GetDataSet(spName, parms);
        }

        public DataTable GetEnqNoList(string vendorId = "")
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@CustomerId", vendorId);
            var spName = ConfigurationManager.AppSettings["GetEnqNoList"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public string GetQuoteNo(string quotetypeId = null, string finYear = null)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@quotetypeId", quotetypeId);
            parms.Add("@finYear", finYear);
            var spName = ConfigurationManager.AppSettings["GetQuoteNo"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return Convert.ToString(dt.Rows[0]["QuoteNo"]);
        }

        public DataTable GetQuoteItemSlNoList(string quotetypeId = "", string SoNo = null)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(quotetypeId))
                quotetypeId = "";
            parms.Add("@quotetypeId", quotetypeId);
            parms.Add("@SoNo", SoNo);
            var spName = ConfigurationManager.AppSettings["GetQuoteItemSlNoList"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetQuoteNoList(string quotetypeId = "", string SoNo = null)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(quotetypeId))
                quotetypeId = "";
            parms.Add("@quotetypeId", quotetypeId);
            parms.Add("@SoNo", SoNo);
            var spName = ConfigurationManager.AppSettings["GetQuoteNoList"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetQuoteEnqNoList(QuotationEntity qE)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@QuoteId", qE.Id);
            var spName = ConfigurationManager.AppSettings["FetchEnquiryNumber"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetPrepProductNames(string productId, string casingSize, string type = null)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@productId", productId);
            parms.Add("@casingSize", casingSize);
            parms.Add("@type", type);
            var spName = ConfigurationManager.AppSettings["FetchProductDetailsById"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetOrderQuoteDetails(string quoteType, string quoteNoId)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(quoteType))
                quoteType = "";
            parms.Add("@quotetype", quoteType);
            parms.Add("@quoteNoId", quoteNoId);
            var spName = ConfigurationManager.AppSettings["GetOrderQuoteDetails"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetOrderDetailsForQuotes(string quoteType, string quoteNoId)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(quoteType))
                quoteType = "";
            parms.Add("@quotetype", quoteType);
            parms.Add("@quoteNoId", quoteNoId);
            var spName = ConfigurationManager.AppSettings["GetOrderDetailsForQuotes"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetOrderDetailsFromSO(string SoNoView)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@SoNoView", SoNoView);
            var spName = ConfigurationManager.AppSettings["GetOrderDetailsFromSO"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }


        public DataTable GetDataForDocument(string downloadTypeId, string quoteTypeId, string quoteNumberId)
        {
            var parms = new Dictionary<string, object>();
            var spName = ConfigurationManager.AppSettings["GetDataForDocument"];
            parms.Add("@QuoteTypeId", quoteTypeId);
            parms.Add("@QuoteNumberId", quoteNumberId);
            var dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetListForDocument(string downloadTypeId, string quoteTypeId, string quoteNumberId)
        {
            var parms = new Dictionary<string, object>();
            var spName = ConfigurationManager.AppSettings["GetListForDocument"];
            parms.Add("@QuoteTypeId", quoteTypeId);
            parms.Add("@QuoteNumberId", quoteNumberId);
            var dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public string SaveClarificationData(ClarificationEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@QuoteType", Model.QuoteType);
            Params.Add("@QuoteNo", Model.QuoteNo);
            Params.Add("@MailId", Model.MailId);

            var SPName = ConfigurationManager.AppSettings["SaveClarificationData"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public string SaveOrderClarificationData(ClarificationEntity cEn)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@SoNo", cEn.SoNo);
            Params.Add("@OrderDocName", cEn.OrderDocName);

            var SPName = ConfigurationManager.AppSettings["SaveOrderClarificationData"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public string SaveQuoteNotes(ClarificationEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@QuoteType", Model.QuoteType);
            Params.Add("@QuoteNo", Model.QuoteNo);
            Params.Add("@NoteMsg", Model.Notes);

            var SPName = ConfigurationManager.AppSettings["SaveQuoteNotes"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public DataTable GetDataForContractReview(string EnqNo, string ItemNo, string type)
        {
            var parms = new Dictionary<string, object>();
            var spName = ConfigurationManager.AppSettings["GetDataForContractReview"];
            parms.Add("@EnqNo", EnqNo);
            parms.Add("@ItemNo", ItemNo);
            parms.Add("@type", type);
            var dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }


        public string SaveContractReviewData(ContractReview Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@ENQNo", Model.ENQNo);
            Params.Add("@FileName", Model.FileName);

            var SPName = ConfigurationManager.AppSettings["SaveContractReviewData"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public DataTable LoadMasterPLlist(int skip, int pageSize, string sortColumn, string sortColumnDir, string search)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("@skip", skip);
            parms.Add("@PageSize", pageSize);
            parms.Add("@sortColumn", sortColumn);
            parms.Add("@sortColumnDir", sortColumnDir);
            parms.Add("@search", search);
            string spName = ConfigurationManager.AppSettings["LoadMasterPLlist"];
            return _dbAccess.GetDataTable(spName, parms);
        }

        public DataTable LoadQuotePrepListDetails(int skip, int pageSize, string sortColumn, string sortColumnDir, string search, string quoteType = null, string quoteNo = null, string itemNo = null, string financialYear = null)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("@skip", skip);
            parms.Add("@PageSize", pageSize);
            parms.Add("@sortColumn", sortColumn);
            parms.Add("@sortColumnDir", sortColumnDir);
            parms.Add("@search", search);
            parms.Add("@quoteType", quoteType);
            parms.Add("@quoteNo", quoteNo == "Select" ? null : quoteNo);
            parms.Add("@itemNo", itemNo);
            parms.Add("@financialYear", financialYear);
            string spName = ConfigurationManager.AppSettings["QuotePrepListDetail"];
            return _dbAccess.GetDataTable(spName, parms);
        }

        public DataTable LoadItemWiseOrders(int skip, int pageSize, string sortColumn, string sortColumnDir, string search)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("@skip", skip);
            parms.Add("@PageSize", pageSize);
            parms.Add("@sortColumn", sortColumn);
            parms.Add("@sortColumnDir", sortColumnDir);
            parms.Add("@search", search);
            string spName = ConfigurationManager.AppSettings["LoadItemWiseOrders"];
            return _dbAccess.GetDataTable(spName, parms);
        }

        public DataTable GetContractReviews(string customerId = null)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@customerId", customerId);
            var spName = ConfigurationManager.AppSettings["GetContractReviews"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetQuoteItemSlNos(string quoteType, string quoteNo, string finYear)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@quoteType", quoteType);
            parms.Add("@quoteNo", quoteNo);
            parms.Add("@finYear", finYear);
            var spName = ConfigurationManager.AppSettings["GetQuoteItemSlNos"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetQuoteNoDetailsforRevisedQuote(string quoteNoId, string quotetypeId, string financialYr)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@quoteType", quotetypeId);
            parms.Add("@quoteNo", quoteNoId);
            parms.Add("@finYear", financialYr);
            var spName = ConfigurationManager.AppSettings["GetQuoteNoDetailsforRevisedQuote"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataSet QuotationDetailsPopup(QuotationEntity Model)
        {
            var SPName = ConfigurationManager.AppSettings["FetchQuotationDetailsById"];
            var Params = new Dictionary<string, object>();
            Params.Add("@QuotationId", Model.Id);
            return _dbAccess.GetDataSet(SPName, Params);
        }

        public string SaveQuotationDetails(QuotationEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@QuotationId", Model.Id == 0 ? 0 : Model.Id);
            Params.Add("@UserInitial", Model.UserInitial);
            Params.Add("@UnitNo", Model.UnitNo);
            Params.Add("@CustomerId", Model.CustomerId);
            Params.Add("@CustomerName", Model.CustomerName);
            Params.Add("@QuoteType", Model.QuoteType);
            Params.Add("@FileNo", Model.FileNo);
            Params.Add("@EnqRef", Model.EnqRef);
            Params.Add("@EnqNo", Model.EnqNo);
            Params.Add("@EnqDt", Model.EnqDt);
            Params.Add("@EnqFor", Model.EnqFor);
            Params.Add("@MainProdGrp", Model.MainProdGrp);
            Params.Add("@SubProdGrp", Model.SubProdGrp);
            Params.Add("@ProdName", Model.ProdName);
            Params.Add("@QuoteNoView", Model.QuoteNoView);
            Params.Add("@QuoteNo", Model.QuoteNo);
            Params.Add("@GeoCode", Model.GeoCode);
            Params.Add("@QuoteSentOn", Model.QuoteSentOn);
            Params.Add("@QuoteValidity", Model.QuoteValidity);
            Params.Add("@BgReq", Model.BgReq);
            Params.Add("@Inspection", Model.Inspection);
            Params.Add("@Remarks", Model.Remarks);
            Params.Add("@Country", Model.CountryId);
            Params.Add("@Currency", Model.Currency);
            Params.Add("@Status", Model.Status);

            Params.Add("@ProductType", Model.ProductType);
            Params.Add("@DeliveryTerms", Model.DeliveryTerms);
            Params.Add("@ModeOfDespatch", Model.ModeOfDespatch);
            Params.Add("@PortOfDischarge", Model.PortOfDischarge);
            Params.Add("@LeadTime", Model.LeadTime);
            Params.Add("@LeadTimeDuration", Model.LeadTimeDuration);
            Params.Add("@DeliveryTime", Model.DeliveryTime);
            Params.Add("@PaymentTerms", Model.PaymentTerms);
            Params.Add("@SalesPerson", Model.SalesPerson);
            Params.Add("@Subject", Model.Subject);
            Params.Add("@SupplyTerms", Model.SupplyTerms);
            Params.Add("@RevisedQuoteNo", Model.RevisedQuoteNo);
            Params.Add("@QuoteDate", Model.QuoteDate);
            Params.Add("@FinancialYear", Model.FinancialYear);

            var SPName = ConfigurationManager.AppSettings["SaveQuotationDetails"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public string SaveQuotePreparation(QuotationPreparationEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@Id", Model.Id == 0 ? 0 : Model.Id);
            Params.Add("@QuoteType", Model.QuoteType);
            Params.Add("@QuoteNo", Model.QuoteNo);
            Params.Add("@ItemNo", Model.ItemNo);
            Params.Add("@EnqSrNo", Model.EnqSrNo);
            Params.Add("@VendorName", "");
            Params.Add("@MainProdGrp", Model.MainProdGrp);
            Params.Add("@SubProdGrp", Model.SubProdGrp);
            Params.Add("@ProductName", Model.ProductName);
            Params.Add("@ProductNo", Model.ProductNo);
            Params.Add("@CasingSize", Model.CasingSize);
            Params.Add("@CasingPpf", Model.CasingPpf);
            Params.Add("@MaterialGrade", Model.MaterialGrade);
            Params.Add("@Connection", Model.Connection);
            Params.Add("@Qty", Model.Qty);
            Params.Add("@Uom", Model.Uom);
            Params.Add("@UnitPrice", Model.UnitPrice);
            Params.Add("@OpenHoleSize", Model.OpenHoleSize);
            Params.Add("@Currency", Model.Currency);
            Params.Add("@BallSize", Model.BallSize);
            Params.Add("@WallThickness", Model.WallThickness);

            Params.Add("@ODSize", Model.ODSize);
            Params.Add("@TotalBows", Model.TotalBows);

            //For Product Table
            Params.Add("@ViewProductId", Model.ViewProductId);
            Params.Add("@ViewProductName", Model.ViewProductName);
            Params.Add("@ViewProductCode", Model.ViewProductCode);
            Params.Add("@ViewPL", Model.ViewPL);
            Params.Add("@ViewProductNo", Model.ViewProductNo);
            Params.Add("@ViewPos1", Model.ViewPos1);
            Params.Add("@ViewPos2", Model.ViewPos2);
            Params.Add("@ViewPos3", Model.ViewPos3);
            Params.Add("@ViewPos4", Model.ViewPos4);
            Params.Add("@ViewPos5", Model.ViewPos5);
            Params.Add("@ViewPos6", Model.ViewPos6);
            Params.Add("@ViewPos7", Model.ViewPos7);
            Params.Add("@ViewPos8", Model.ViewPos8);
            Params.Add("@ViewPos9", Model.ViewPos9);
            Params.Add("@ViewPos10", Model.ViewPos10);
            Params.Add("@ViewDES", Model.ViewDES);
            Params.Add("@ViewProductDetails", Model.ViewProductDetails);
            Params.Add("@PDCDrillable", Model.PDCFeatures);
            Params.Add("@FinancialYear", Model.FinancialYear);
            Params.Add("@QuoteNoView", Model.QuoteNoView);

            var SPName = ConfigurationManager.AppSettings["SaveQuotePreparation"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public string DeleteQuotationDetail(int QuotationId)
        {
            string spName = ConfigurationManager.AppSettings["DeleteQuotationDetails"];
            string msgCode = "";
            var parms = new Dictionary<string, object>();
            parms.Add("@QuotationId", QuotationId);
            _dbAccess.ExecuteNonQuery(spName, parms, "@o_MsgCode", out msgCode);
            return msgCode;

        }

        public DataTable GetUserQuoteDetails(string unitNo)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@unitNo", unitNo);
            var spName = ConfigurationManager.AppSettings["GetUserQuoteDetails"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public DataTable GetVendorQuoteDetails(string vendorId)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@vendorId", vendorId);
            var spName = ConfigurationManager.AppSettings["GetVendorQuoteDetails"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }

        public string SaveRevisedQuotationDetails(QuotationEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@Id", Model.Id);
            Params.Add("@UserInitial", Model.UserInitial);
            Params.Add("@UnitNo", Model.UnitNo);
            Params.Add("@CustomerId", Model.CustomerId);
            Params.Add("@CustomerName", Model.CustomerName);
            Params.Add("@QuoteType", Model.QuoteType);
            Params.Add("@FileNo", Model.FileNo);
            Params.Add("@EnqRef", Model.EnqRef);
            Params.Add("@EnqNo", Model.EnqNos);
            Params.Add("@EnqDt", Model.EnqDt);
            Params.Add("@EnqFor", Model.EnqFor);
            Params.Add("@ProdName", Model.ProdName);
            Params.Add("@QuoteNoView", Model.QuoteNoView);
            Params.Add("@QuoteNo", Model.QuoteNo);
            Params.Add("@GeoCode", Model.GeoCode);
            Params.Add("@QuoteSentOn", Model.QuoteSentOn);
            Params.Add("@QuoteValidity", Model.QuoteValidity);
            Params.Add("@BgReq", Model.BgReq);
            Params.Add("@Inspection", Model.Inspection);
            Params.Add("@Remarks", Model.Remarks);
            Params.Add("@Country", Model.CountryId);
            Params.Add("@Currency", Model.Currency);
            Params.Add("@Status", Model.Status);

            Params.Add("@ProductType", Model.ProductType);
            Params.Add("@DeliveryTerms", Model.DeliveryTerms);
            Params.Add("@ModeOfDespatch", Model.ModeOfDespatch);
            Params.Add("@PortOfDischarge", Model.PortOfDischarge);
            Params.Add("@LeadTime", Model.LeadTime);
            Params.Add("@LeadTimeDuration", Model.LeadTimeDuration);
            Params.Add("@DeliveryTime", Model.DeliveryTime);
            Params.Add("@PaymentTerms", Model.PaymentTerms);
            Params.Add("@SalesPerson", Model.SalesPerson);
            Params.Add("@Subject", Model.Subject);
            Params.Add("@SupplyTerms", Model.SupplyTerms);
            Params.Add("@RevisedQuoteNo", Model.RevisedQuoteNo);
            Params.Add("@QuoteDate", Model.QuoteDate);
            Params.Add("@FinancialYear", Model.FinancialYear);
            Params.Add("@Ids", Model.ids);
            Params.Add("@QuoteAddOnType", Model.QuoteAddOnType);


            var SPName = ConfigurationManager.AppSettings["SaveRevisedQuotationDetails"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public string DeleteClarificationMails(string[] param)
        {
            string spName = ConfigurationManager.AppSettings["DeleteClarificationMails"];
            string msgCode = "";

            List<int> numbers = new List<int>(Array.ConvertAll(param[0].Split(','), int.Parse));

            var parms = new Dictionary<string, object>();
            foreach (int num in numbers)
            {
                parms.Add("@Id", num);
                _dbAccess.ExecuteNonQuery(spName, parms, "@o_MsgCode", out msgCode);
                parms.Clear();
            }

            return msgCode;
        }

        public string DeleteOrderClarifications(string[] param)
        {
            string spName = ConfigurationManager.AppSettings["DeleteOrderClarifications"];
            string msgCode = "";

            List<int> numbers = new List<int>(Array.ConvertAll(param[0].Split(','), int.Parse));

            var parms = new Dictionary<string, object>();
            foreach (int num in numbers)
            {
                parms.Add("@Id", num);
                _dbAccess.ExecuteNonQuery(spName, parms, "@o_MsgCode", out msgCode);
                parms.Clear();
            }

            return msgCode;
        }

        public DataTable GetDdlValueForQuote(string type, string VendorId = null, string QuoteType = null, string SubjectId = null)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("@Type", type);
            parms.Add("@CustomerId", VendorId);
            parms.Add("@QuoteType", QuoteType);
            parms.Add("@SubjectId", SubjectId);
            string spName = ConfigurationManager.AppSettings["GetDdlValueForQuote"];
            return _dbAccess.GetDataTable(spName, parms);
        }

        public string SaveNewDescDetail(DescEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@MainPL", Model.MainPL);
            Params.Add("@SubPL", Model.SubPL);
            Params.Add("@ProductName", Model.ProductName);
            Params.Add("@ProductNo", Model.ProductNo);
            Params.Add("@DESQuery", Model.DESQuery);

            Params.Add("@Pos1", Model.Pos1);
            Params.Add("@Pos2", Model.Pos2);
            Params.Add("@Pos3", Model.Pos3);
            Params.Add("@Pos4", Model.Pos4);
            Params.Add("@Pos5", Model.Pos5);
            Params.Add("@Pos6", Model.Pos6);

            Params.Add("@Pos7", Model.Pos7);
            Params.Add("@Pos8", Model.Pos8);
            Params.Add("@Pos9", Model.Pos9);
            Params.Add("@Pos10", Model.Pos10);

            Params.Add("@FieldName1", Model.FieldName1);
            Params.Add("@FieldName2", Model.FieldName2);
            Params.Add("@FieldName3", Model.FieldName3);
            Params.Add("@FieldName4", Model.FieldName4);
            Params.Add("@FieldName5", Model.FieldName5);
            Params.Add("@FieldName6", Model.FieldName6);

            Params.Add("@FieldName7", Model.FieldName7);
            Params.Add("@FieldName8", Model.FieldName8);
            Params.Add("@FieldName9", Model.FieldName9);
            Params.Add("@FieldName10", Model.FieldName10);


            var SPName = ConfigurationManager.AppSettings["SaveNewDescDetail"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public string SaveOrderNote(OrderEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@SoNo", Model.SoNo);
            Params.Add("@Notes", Model.Notes);

            var SPName = ConfigurationManager.AppSettings["SaveOrderNotes"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public DataTable LoadDescDetail(int skip, int pageSize, string sortColumn, string sortColumnDir, string search)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("@skip", skip);
            parms.Add("@PageSize", pageSize);
            parms.Add("@sortColumn", sortColumn);
            parms.Add("@sortColumnDir", sortColumnDir);
            parms.Add("@search", search);
            string spName = ConfigurationManager.AppSettings["LoadDescDetail"];
            return _dbAccess.GetDataTable(spName, parms);
        }

        public DataSet GetWorkAuthReport(string SoNo, string FromDate, string ToDate, string ReportType)
        {
            string SPName = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@SoNo", SoNo);
            Params.Add("@FromDate", FromDate);
            Params.Add("@ToDate", ToDate);
            switch (ReportType)
            {
                case "WAuthReport":
                    SPName = ConfigurationManager.AppSettings["WAReport"];
                    break;
                case "ProductPerformance":
                    SPName = ConfigurationManager.AppSettings["ProductReport"];
                    break;
                case "EnquiryReport":
                    SPName = ConfigurationManager.AppSettings["EQAndQRReport"];
                    break;
                case "CUSTOMERFEEDBACK":
                    SPName = ConfigurationManager.AppSettings["CustomerFeedback"];
                    break;
                case "ConEnquiryReport":
                    SPName = ConfigurationManager.AppSettings["ConsolidatedEnquiryReport"];
                    break;
                case "ConQuotationReport":
                    SPName = ConfigurationManager.AppSettings["ConsolidatedQuotationReport"];
                    break;
                case "ConMasterOrderReport":
                    SPName = ConfigurationManager.AppSettings["ConsolidatedOrderReport"];
                    break;
                case "ConOrderItemwiseReport":
                    SPName = ConfigurationManager.AppSettings["ConsolidatedOrderItemwiseReport"];
                    break;
            }

            return _dbAccess.GetDataSet(SPName, Params);
        }
        public string SaveRevisedOrderDetails(OrderEntity Model)
        {
            string msgCode = "";
            var Params = new Dictionary<string, object>();
            Params.Add("@OrderId", Model.Id == 0 ? 0 : Model.Id);
            Params.Add("@UserInitial", Model.UserInitial);
            Params.Add("@UnitNo", Model.UnitNo);
            //Params.Add("@VendorId", Model.VendorId);
            //Params.Add("@VendorName", Model.VendorName);
            Params.Add("@QuoteType", Model.QuoteType);
            Params.Add("@QuoteQtyType", Model.QuoteQtyType);
            //Params.Add("@FileNo", Model.FileNo);
            Params.Add("@QuoteNo", Model.QuoteNo);
            Params.Add("@QuoteNoView", Model.QuoteNoView);
            Params.Add("@QuoteDate", Model.QuoteDate);
            Params.Add("@SoNo", Model.SoNo);
            Params.Add("@PoEntity", Model.PoEntity);
            Params.Add("@PoLocation", Model.PoLocation);
            Params.Add("@PoDor", Model.PoDor);
            Params.Add("@PoNo", Model.PoNo);
            Params.Add("@PoDate", Model.PoDate);
            //Params.Add("@PoSLNo", Model.PoSLNo);
            //Params.Add("@PoItemDescription", Model.PoItemDescription);
            //Params.Add("@PoQty", Model.PoQty);
            Params.Add("@Curr", Model.Curr);
            //Params.Add("@UnitPrice", Model.UnitPrice);
            Params.Add("@PoDeliveryDate", Model.PoDeliveryDate);
            Params.Add("@PoTerms", Model.PoTerms);
            Params.Add("@SupplyTerms", Model.SupplyTerms);
            //Params.Add("@LotWiseQty", Model.LotWiseQty);
            //Params.Add("@LotWiseDate", Model.LotWiseDate);
            Params.Add("@ConsigneeName", Model.ConsigneeName);
            Params.Add("@ConsigneeLocation", Model.ConsigneeLocation);
            Params.Add("@InspectionAgency", Model.InspectionAgency);
            Params.Add("@ModeOfShipment", Model.ModeOfShipment);
            Params.Add("@PaymentTerms", Model.PaymentTerms);
            //Params.Add("@PoRequirements", Model.PoRequirements);

            Params.Add("@DeliveryTerms", Model.DeliveryTerms);
            Params.Add("@ExWorkValue", Model.ExWorkValue);
            Params.Add("@Inspection", Model.Inspection);
            Params.Add("@EndUser", Model.EndUser);
            Params.Add("@ProductGroup", Model.MainProdGrp);
            Params.Add("@MultiQuoteNos", Model.MultiQuoteNos);
            Params.Add("@RevisedPoNo", Model.RevisedPoNo);
            Params.Add("@RevisedPoDate", Model.RevisedPoDate);
            Params.Add("@FinancialYear", Model.FinancialYear);
            Params.Add("@QuoteFinYear", Model.QuoteFinYear);
            Params.Add("@RevisedOrderNo", Model.RevisedOrderNo);
            Params.Add("@SoNoView", Model.SoNoView);

            var SPName = ConfigurationManager.AppSettings["SaveRevisedOrderDetails"];
            _dbAccess.ExecuteNonQuery(SPName, Params, "@o_MsgCode", out msgCode);

            return msgCode;
        }

        public DataTable GetItemNosForEnqs(string EnqNo)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("@EnqNo", EnqNo);
            string spName = ConfigurationManager.AppSettings["GetItemNosForEnqs"];
            return _dbAccess.GetDataTable(spName, parms);
        }

        public DataTable GetRevAndOriginalQuotes(string quotetypeId, string financialYr, string quoteAddOn = null)
        {
            DataTable dt = new DataTable();
            var parms = new Dictionary<string, object>();
            parms.Add("@quoteType", quotetypeId);
            parms.Add("@finYear", financialYr);
            parms.Add("@quoteAddOn", quoteAddOn);
            var spName = ConfigurationManager.AppSettings["GetRevAndOriginalQuotes"];
            dt = _dbAccess.GetDataTable(spName, parms);
            return dt;
        }


        #endregion
    }
}
