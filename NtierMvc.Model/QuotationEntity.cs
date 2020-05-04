﻿using NtierMvc.Common;
using NtierMvc.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtierMvc.Model
{
    public class QuotationEntity
    {
        public int Id { get; set; }
        public string UserInitial { get; set; }
        public string UnitNo { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string QuoteType { get; set; }
        public string FileNo { get; set; }
        public string EnqRef { get; set; }
        public string EnqDt { get; set; }
        public string EnqFor { get; set; }
        public string EnqNo { get; set; }
        public string ProdGrp { get; set; }
        public string QuoteNo { get; set; }
        public string QuoteTypeNo { get; set; }
        public string GeoArea { get; set; }
        public string GeoCode { get; set; }
        public string QuoteDate { get; set; }
        public string QuoteValidity { get; set; }
        public string BgReq { get; set; }
        public string Inspection { get; set; }
        public string Remarks { get; set; }
        public string Country { get; set; }
        public string CountryId { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string ProductType { get; set; }
        public string DeliveryTerms { get; set; }
        public string ModeOfDespatch { get; set; }
        public string PortOfDischarge { get; set; }
        public string LeadTime { get; set; }
        public string ipAddress { get; set; }
        public string RevisedQuoteNo { get; set; }
        public string ViewRevisedQuoteNo { get; set; }

    }

    public class QuotationEntityDetails
    {
        public CustomerEntity cusEnt { get; set; }
        public QuotationEntity quoteEntity { get; set; }
        public List<QuotationEntity> lstQuoteEntity { get; set; }
        public List<DropDownEntity> lstDdEntity { get; set; }
        public int totalcount { get; set; }
        public QuotationEntityDetails()
        {
            cusEnt = new CustomerEntity();
            quoteEntity = new QuotationEntity();
            lstQuoteEntity = new List<QuotationEntity>();
            lstDdEntity = new List<DropDownEntity>();
        }
    }

    public class QuoteTypeDetails
    {
        public List<DropDownEntity> lstQuoteTypeEntity { get; set; }
        public List<DropDownEntity> lstQuoteNos { get; set; }
        public List<DropDownEntity> lstVendors { get; set; }
        public string QuoteNo { get; set; }
        public string RevisedQuoteNo { get; set; }
        public QuoteTypeDetails()
        {
            lstQuoteTypeEntity = new List<DropDownEntity>();
            lstQuoteNos = new List<DropDownEntity>();
            lstVendors = new List<DropDownEntity>();
        }
    }

}