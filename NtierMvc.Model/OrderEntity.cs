﻿using NtierMvc.Common;
using NtierMvc.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtierMvc.Model
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public string UserInitial { get; set; }
        public string UnitNo { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string QuoteTypeValue { get; set; }
        public string QuoteQtyType { get; set; }
        public string QuoteType { get; set; }
        public string FileNo { get; set; }
        public string QuoteNo { get; set; }
        public string QuoteDate { get; set; }
        public string SoNo { get; set; }
        public string PoEntity { get; set; }
        public string PoDate { get; set; }
        public string PoDeliveryDate { get; set; }
        public string PoNo { get; set; }
        public string PoLocation { get; set; }
        public string PoDor { get; set; }
        public string PoQty { get; set; }
        public string POPercent { get; set; }
        public string PoItemDescription { get; set; }
        public string Curr { get; set; }
        public string ExWorkValue { get; set; }
        public string DeliveryTerms { get; set; }
        public string SupplyTerms { get; set; }
        public string PoTerms { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeLocation { get; set; }
        public string InspectionAgency { get; set; }
        public string ModeOfShipment { get; set; }
        public string PaymentTerms { get; set; }
        public string Inspection { get; set; }
        public string EndUser { get; set; }
        public string MainProdGrp { get; set; }        
        public string SubProdGrp { get; set; }        
        public string ProdName { get; set; }        
        public string ipAddress { get; set; }        
        public string MultiQuoteNos { get; set; }
        public string UploadedFile { get; set; }
    }

    public class ItemEntity
    {
        public int Id { get; set; }
        public string UnitNo { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string QuoteNo { get; set; }

        public string QuoteType { get; set; }
        public string SoNo { get; set; }
        public string QuotePrepId { get; set; }
        public string PoNo { get; set; }
        public string PoDate { get; set; }
        public string PoDeliveryDate { get; set; }
        public string PoSLNo { get; set; }
        public string POPercent { get; set; }
        public string PoItemDescription { get; set; }
        public string PoQty { get; set; }
        public string ExWorkValue { get; set; }
        public string SupplyTerms { get; set; }
        public string SupplyTermsText { get; set; }
        public string UnitPrice { get; set; }
        public string LotName { get; set; }
        public string LotWiseQty { get; set; }
        public string LotWiseDate { get; set; }
        public string PoRequirements { get; set; }
        public string NetWeight { get; set; }
        public string GrossWeight { get; set; }
        public string ProductGroup { get; set; }
        public string ipAddress { get; set; }
        public string IsActive { get; set; }

    }

    public class ItemEntityBulkSave
    {
        public int Id { get; set; }
        public string UnitNo { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string QuoteNo { get; set; }

        public string QuoteType { get; set; }
        public string SoNo { get; set; }
        public string QuotePrepId { get; set; }
        public string PoSLNo { get; set; }
        public string PoQty { get; set; }
        public string UnitPrice { get; set; }
        public string LotName { get; set; }
        public string LotWiseQty { get; set; }
        public string LotWiseDate { get; set; }
        public string PoRequirements { get; set; }
        public string ipAddress { get; set; }
        public string IsActive { get; set; }

    }

    public class OrderEntityDetails
    {
        public OrderEntity oEntity { get; set; }
        public List<OrderEntity> lstOrderEntity { get; set; }
        public List<DropDownEntity> lstDdEntity { get; set; }
        public int totalcount { get; set; }
        public OrderEntityDetails()
        {
            oEntity = new OrderEntity();
            lstOrderEntity = new List<OrderEntity>();
            lstDdEntity = new List<DropDownEntity>();
        }
    }
    
}
