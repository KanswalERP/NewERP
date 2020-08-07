﻿using NtierMvc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtierMvc.Model.MRM
{
    public class PRDetailEntity
    {
        public int Id { get; set; }
        public int PRSetno { get; set; }
        public string PRno { get; set; }
        public string PRdate { get; set; }
        public string ReqFrom { get; set; }
        public string ReqTo { get; set; }
        public int QuoteType { get; set; }
        public string DeptName { get; set; }
        public string LogInDeptName { get; set; }
        public string PRcat { get; set; }
        public int Currency { get; set; }
        public int Priority { get; set; }
        public string CriticalNature { get; set; }
        public int EndUse { get; set; }
        public int EndUseNo { get; set; }
        public int CostCenter { get; set; }
        public int SN { get; set; }
        public string RMcode { get; set; }
        public int RMcat { get; set; }
        public string Desc1 { get; set; }
        public string RMdescription { get; set; }
        public string RMgrade { get; set; }
        public string RMHardness { get; set; }
        public string PSLlevel { get; set; }
        public int UOM { get; set; }
        public string OD { get; set; }
        public string WT { get; set; }
        public string Len { get; set; }
        public int QtyReqd { get; set; }
        public int QtyStock { get; set; }
        public int PRqty { get; set; }
        public string UnitPrice { get; set; }
        public string TotalPrice { get; set; }
        public string TotalPRSetPrice { get; set; }
        public string ReqDlyDate { get; set; }
        public int SupplyTerms { get; set; }
        public string DlyTerms { get; set; }
        public string PaymentTerms { get; set; }
        public string HSCode { get; set; }
        public string Supplier2 { get; set; }
        public string Vendor1 { get; set; }
        public string Vendor2 { get; set; }
        public string WIPno { get; set; }
        public string DRGno { get; set; }
        public string OPNcode { get; set; }
        public string ProcessName { get; set; }
        public string EDR { get; set; }
        public string OPNtime { get; set; }
        public string Certificates { get; set; }
        public string DeliveryTerms { get; set; }
        public string ApprovedSupplier1 { get; set; }
        public string ApprovedSupplier2 { get; set; }
        public string ApprovedReject { get; set; }
        public string Communicate { get; set; }
        public string POno { get; set; }
        public string DeliveryDate { get; set; }
        public int UserId { get; set; }
        public int DeptId { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string EntryDate { get; set; }
        public string SignStatus { get; set; }
        public string PRStatus { get; set; }
        public string EntryPerson { get; set; }
        public string ApprovePerson1 { get; set; }
        public string ApprovePerson2 { get; set; }
        public string EntryPersonSign { get; set; }
        public string ApprovePerson1Sign { get; set; }
        public string ApprovePerson2Sign { get; set; }
        public string EntryType { get; set; }
        public string PRRequestedOn { get; set; }
        public string PRFavouredOn { get; set; }
        public string PurchasePerson { get; set; }
    }

    public class PRDetailEntityBulkSave
    {
        public int Id { get; set; }
        public int PRSetno { get; set; }
        public string PRno { get; set; }
        public string PRdate { get; set; }
        public string ReqFrom { get; set; }
        public string ReqTo { get; set; }
        public int QuoteType { get; set; }
        public string DeptName { get; set; }
        public string PRcat { get; set; }
        public int Currency { get; set; }
        public int Priority { get; set; }
        //public string CriticalNature { get; set; }
        public int EndUse { get; set; }
        public int EndUseNo { get; set; }
        public int CostCenter { get; set; }
        public int SN { get; set; }
        public string RMcode { get; set; }
        public int RMcat { get; set; }
        public string RMdescription { get; set; }
        public string RMgrade { get; set; }
        public string RMHardness { get; set; }
        public string PSLlevel { get; set; }
        public int UOM { get; set; }
        public string OD { get; set; }
        public string WT { get; set; }
        public string Len { get; set; }
        public int QtyReqd { get; set; }
        public int QtyStock { get; set; }
        public int PRqty { get; set; }
        public string UnitPrice { get; set; }
        public string TotalPrice { get; set; }
        public string TotalPRSetPrice { get; set; }
        public string ReqDlyDate { get; set; }
        public int SupplyTerms { get; set; }
        public string DlyTerms { get; set; }
        public string PaymentTerms { get; set; }
        public string HSCode { get; set; }
        public string Supplier2 { get; set; }
        public string Vendor1 { get; set; }
        public string Vendor2 { get; set; }
        public string WIPno { get; set; }
        public string DRGno { get; set; }
        public string OPNcode { get; set; }
        public string ProcessName { get; set; }
        public string EDR { get; set; }
        public string OPNtime { get; set; }
        public string Certificates { get; set; }
        public string DeliveryTerms { get; set; }
        public string ApprovedSupplier1 { get; set; }
        public string ApprovedSupplier2 { get; set; }
        public string ApprovedReject { get; set; }
        public string Communicate { get; set; }
        public string POno { get; set; }
        public string DeliveryDate { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ApproveDate1 { get; set; }
        public DateTime? ApproveDate2 { get; set; }
        public string SignStatus { get; set; }
        public string PRStatus { get; set; }
        public string EntryPerson { get; set; }
        public string ApprovePerson1 { get; set; }
        public string ApprovePerson2 { get; set; }
        public string PRFavouredOn { get; set; }

    }

    public class PRDetailEntityDetails
    {
        public PRDetailEntity pEntity { get; set; }
        public List<PRDetailEntity> lstPREntity { get; set; }
        public int totalcount { get; set; }
        public PRDetailEntityDetails()
        {
            pEntity = new PRDetailEntity();
            lstPREntity = new List<PRDetailEntity>();
        }
    }

}
